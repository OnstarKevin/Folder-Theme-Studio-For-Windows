using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.SystemIntegration;

public sealed class KnownFolderServiceTests
{
    private readonly KnownFolderService service = new();

    [Theory]
    [InlineData(@"C:\Windows", FolderDecision.ProtectedRoot)]
    [InlineData(@"\\server\share", FolderDecision.NetworkPath)]
    public void Evaluate_RejectsUnsafeRoots(string path, FolderDecision expected)
    {
        Assert.Equal(expected, service.Evaluate(path));
    }

    [Fact]
    public void Evaluate_RejectsInjectedKnownFolder()
    {
        var folders = new FakeFolderSystem();
        folders.AddDirectory(@"C:\Users\Test\Downloads");

        Assert.Equal(FolderDecision.KnownFolder, KnownFolderService.CreateForTesting(folders, [@"C:\Users\Test\Downloads"]).Evaluate(@"C:\Users\Test\Downloads"));
    }

    [Fact]
    public void Evaluate_RejectsDefaultDesktopKnownFolder()
    {
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        Assert.Equal(FolderDecision.KnownFolder, new KnownFolderService().Evaluate(desktop));
    }

    [Fact]
    public void Evaluate_RejectsDescendantOfProtectedWindowsDirectory()
    {
        Assert.Equal(FolderDecision.ProtectedRoot, service.Evaluate(@"C:\Windows\System32"));
    }

    [Fact]
    public void Evaluate_AllowsDirectoryWithExistingDesktopIni()
    {
        using var temp = new TemporaryDirectory();
        System.IO.File.WriteAllText(System.IO.Path.Combine(temp.Path, "desktop.ini"), "[.ShellClassInfo]");

        Assert.Equal(FolderDecision.Allowed, service.Evaluate(temp.Path));
    }

    [Fact]
    public void Evaluate_RejectsRegularFileAsNonWritableFolderCandidate()
    {
        using var temp = new TemporaryDirectory();
        var regularFile = System.IO.Path.Combine(temp.Path, "not-a-folder.txt");
        System.IO.File.WriteAllText(regularFile, "not a directory");

        Assert.Equal(FolderDecision.NoWriteAccess, service.Evaluate(regularFile));
    }

    [Fact]
    public async Task PlanTreeAsync_KnownSelectedRootStillDiscoversOrdinaryChildren()
    {
        using var temp = new TemporaryDirectory();
        var knownFolder = System.IO.Path.Combine(temp.Path, "known");
        var nestedFolder = System.IO.Path.Combine(knownFolder, "nested");
        System.IO.Directory.CreateDirectory(nestedFolder);
        var folders = new FakeFolderSystem();
        folders.AddDirectory(knownFolder);
        folders.AddDirectory(nestedFolder);
        folders.AddChild(knownFolder, nestedFolder);
        var planner = KnownFolderService.CreateForTesting(folders, [knownFolder]);

        var plan = await CollectAsync(planner.PlanTreeAsync(knownFolder, CancellationToken.None));

        Assert.Contains(plan, item => item.Path == knownFolder && item.Decision == FolderDecision.KnownFolder);
        Assert.Contains(plan, item => item.Path == nestedFolder && item.Decision == FolderDecision.Allowed);
    }

    [Fact]
    public void RevalidateForMutation_AllowsOrdinaryChildOfKnownSelectedRoot()
    {
        var folders = new FakeFolderSystem();
        folders.AddDirectory(@"C:\Users\Test\Desktop");
        folders.AddDirectory(@"C:\Users\Test\Desktop\ordinary");
        var policy = KnownFolderService.CreateForTesting(folders, [@"C:\Users\Test\Desktop"]);
        var item = new FolderPlanItem(
            @"C:\Users\Test\Desktop\ordinary",
            FolderDecision.Allowed,
            @"C:\Users\Test\Desktop",
            @"C:\Users\Test\Desktop\ordinary",
            @"C:\Users\Test\Desktop");

        var validation = policy.RevalidateForMutation(item);

        Assert.True(validation.IsAllowed);
    }

    [Fact]
    public async Task PlanTreeAsync_ReportsReparsePointWithoutFollowingIt()
    {
        using var root = new TemporaryDirectory();
        using var target = new TemporaryDirectory();
        var targetChild = System.IO.Path.Combine(target.Path, "child");
        System.IO.Directory.CreateDirectory(targetChild);
        var link = System.IO.Path.Combine(root.Path, "directory-link");
        using var createJunction = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c mklink /J \"{link}\" \"{target.Path}\"",
            CreateNoWindow = true,
            UseShellExecute = false
        });
        Assert.NotNull(createJunction);
        await createJunction.WaitForExitAsync();
        Assert.Equal(0, createJunction.ExitCode);

        try
        {
            var plan = await CollectAsync(new KnownFolderService().PlanTreeAsync(root.Path, CancellationToken.None));

            Assert.Contains(plan, item => item.Path == link && item.Decision == FolderDecision.ReparsePoint);
            Assert.DoesNotContain(plan, item => item.Path == targetChild);
        }
        finally
        {
            System.IO.Directory.Delete(link);
        }
    }

    [Fact]
    public void Evaluate_RejectsNetworkDriveReportedByFolderSystem()
    {
        var folderSystem = new FakeFolderSystem();
        folderSystem.AddDirectory(@"N:\theme", driveType: System.IO.DriveType.Network);

        var decision = KnownFolderService.CreateForTesting(folderSystem, []).Evaluate(@"N:\theme");

        Assert.Equal(FolderDecision.NetworkPath, decision);
    }

    [Fact]
    public void Evaluate_RejectsAliasWhosePhysicalPathIsProtected()
    {
        var folderSystem = new FakeFolderSystem();
        folderSystem.AddDirectory(@"C:\alias\theme", canonicalPath: @"C:\Windows\theme");

        var decision = KnownFolderService.CreateForTesting(folderSystem, []).Evaluate(@"C:\alias\theme");

        Assert.Equal(FolderDecision.ProtectedRoot, decision);
    }

    [Fact]
    public void Evaluate_RejectsIndeterminateDesktopIniProbe()
    {
        var folderSystem = new FakeFolderSystem();
        folderSystem.AddDirectory(@"C:\ordinary", desktopIniStatus: DesktopIniStatus.Indeterminate);

        var decision = KnownFolderService.CreateForTesting(folderSystem, []).Evaluate(@"C:\ordinary");

        Assert.Equal(FolderDecision.NoWriteAccess, decision);
    }

    [Fact]
    public void Evaluate_InjectedPathsDoNotDisableMandatoryKnownFolders()
    {
        var folderSystem = new FakeFolderSystem();
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        folderSystem.AddDirectory(desktop);

        var decision = KnownFolderService.CreateForTesting(folderSystem, []).Evaluate(desktop);

        Assert.Equal(FolderDecision.KnownFolder, decision);
    }

    [Fact]
    public async Task PlanTreeAsync_ObservesCancellationBetweenIncrementalDirectoryEntries()
    {
        var folders = new FakeFolderSystem();
        folders.AddDirectory(@"C:\ordinary");
        folders.AddDirectory(@"C:\ordinary\one");
        folders.AddDirectory(@"C:\ordinary\two");
        folders.AddChild(@"C:\ordinary", @"C:\ordinary\one");
        folders.AddChild(@"C:\ordinary", @"C:\ordinary\two");
        using var cancellation = new CancellationTokenSource();
        folders.BeforeYield = cancellation.Cancel;

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            CollectAsync(KnownFolderService.CreateForTesting(folders, []).PlanTreeAsync(@"C:\ordinary", cancellation.Token)));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task PlanTreeAsync_EnumerationFailureReportsOneSkipWithoutThrowing(bool throwDuringMoveNext)
    {
        var folders = new FakeFolderSystem();
        folders.AddDirectory(@"C:\ordinary");
        folders.EnumerationFailure = new System.IO.IOException("simulated enumeration race");
        folders.ThrowDuringMoveNext = throwDuringMoveNext;

        var plan = await CollectAsync(KnownFolderService.CreateForTesting(folders, []).PlanTreeAsync(@"C:\ordinary", CancellationToken.None));

        Assert.Equal(
            [new FolderPlanItem(@"C:\ordinary", FolderDecision.NoWriteAccess, @"C:\ordinary", @"C:\ordinary", @"C:\ordinary")],
            plan);
    }

    [Fact]
    public async Task PlanTreeAsync_OrdinaryTreeYieldsEachPathExactlyOnce()
    {
        var folders = new FakeFolderSystem();
        folders.AddDirectory(@"C:\ordinary");
        folders.AddDirectory(@"C:\ordinary\one");
        folders.AddDirectory(@"C:\ordinary\one\two");
        folders.AddChild(@"C:\ordinary", @"C:\ordinary\one");
        folders.AddChild(@"C:\ordinary\one", @"C:\ordinary\one\two");

        var plan = await CollectAsync(KnownFolderService.CreateForTesting(folders, []).PlanTreeAsync(@"C:\ordinary", CancellationToken.None));

        Assert.Equal([@"C:\ordinary", @"C:\ordinary\one", @"C:\ordinary\one\two"], plan.Select(item => item.Path));
        Assert.All(plan, item => Assert.Equal(FolderDecision.Allowed, item.Decision));
    }

    private static async Task<List<FolderPlanItem>> CollectAsync(IAsyncEnumerable<FolderPlanItem> items)
    {
        var result = new List<FolderPlanItem>();
        await foreach (var item in items)
        {
            result.Add(item);
        }

        return result;
    }

    private sealed class FakeFolderSystem : IFolderSystem
    {
        private readonly Dictionary<string, FolderInspection> inspections = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<string>> children = new(StringComparer.OrdinalIgnoreCase);

        public Action? BeforeYield { get; set; }
        public Exception? EnumerationFailure { get; set; }
        public bool ThrowDuringMoveNext { get; set; }

        public void AddDirectory(string path, string? canonicalPath = null, System.IO.DriveType driveType = System.IO.DriveType.Fixed, DesktopIniStatus desktopIniStatus = DesktopIniStatus.Absent) =>
            inspections[path] = new FolderInspection(canonicalPath ?? path, null, driveType, System.IO.FileAttributes.Directory, desktopIniStatus, true);

        public void AddChild(string parent, string child)
        {
            if (!children.TryGetValue(parent, out var values)) children[parent] = values = [];
            values.Add(child);
        }

        public FolderInspection Inspect(string path) => inspections.TryGetValue(path, out var inspection)
            ? inspection
            : new FolderInspection(path, null, System.IO.DriveType.Fixed, System.IO.FileAttributes.Directory, DesktopIniStatus.Absent, true);

        public IEnumerable<string> EnumerateDirectories(string path, System.IO.EnumerationOptions options)
        {
            if (EnumerationFailure is { } creationFailure && !ThrowDuringMoveNext) return new ThrowOnGetEnumerator(creationFailure);
            if (EnumerationFailure is { } moveNextFailure && ThrowDuringMoveNext) return ThrowDuringEnumeration(moveNextFailure);
            return children.TryGetValue(path, out var values) ? YieldValues(values) : [];
        }

        private IEnumerable<string> YieldValues(IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                BeforeYield?.Invoke();
                yield return value;
            }
        }

        private static IEnumerable<string> ThrowDuringEnumeration(Exception exception)
        {
            yield return ThrowOnMoveNext(exception);
        }

        private static string ThrowOnMoveNext(Exception exception) => throw exception;

        private sealed class ThrowOnGetEnumerator(Exception exception) : IEnumerable<string>
        {
            public IEnumerator<string> GetEnumerator() => throw exception;
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
