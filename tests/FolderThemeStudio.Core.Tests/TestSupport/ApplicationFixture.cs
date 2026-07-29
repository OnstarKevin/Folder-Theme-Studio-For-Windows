using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Recovery;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Themes;
using System.IO;
using System.Runtime.CompilerServices;

namespace FolderThemeStudio.Core.Tests.TestSupport;

internal sealed class ApplicationFixture
{
    private static readonly Guid OperationId = new("88888888-8888-8888-8888-888888888888");

    internal ApplicationFixture()
    {
        Validator = new RecordingValidator(CallOrder);
        Icons = new RecordingIcons(CallOrder);
        Planner = new RecordingPlanner(CallOrder);
        Backup = new RecordingBackup(CallOrder);
        Global = new RecordingGlobal(CallOrder);
        Compatible = new RecordingCompatible(CallOrder);
        Refresh = new RecordingRefresh(CallOrder);
        Service = CreateService();

        ValidGlobalPlan = new ApplyPlan(Theme, ApplicationMode.Global, [], []);
        ValidCompatiblePlan = new ApplyPlan(
            Theme,
            ApplicationMode.Compatible,
            [new FolderPlanItem(@"C:\Themes", FolderDecision.Allowed, @"C:\Themes", @"C:\Themes", @"C:\Themes")],
            []);
        GlobalSnapshot = Snapshot(ApplicationMode.Global);
        CompatibleSnapshot = Snapshot(ApplicationMode.Compatible);
    }

    internal ThemeApplicationService CreateService() => new(
            Validator,
            Icons,
            Planner,
            Backup,
            Global,
            Compatible,
            Refresh,
            () => OperationId,
            () => new DateTimeOffset(2026, 7, 27, 9, 0, 0, TimeSpan.Zero));

    internal List<string> CallOrder { get; } = [];
    internal FolderTheme Theme { get; } = FolderTheme.IceBlue;
    internal ThemeApplicationService Service { get; }
    internal RecordingValidator Validator { get; }
    internal RecordingIcons Icons { get; }
    internal RecordingPlanner Planner { get; }
    internal RecordingBackup Backup { get; }
    internal RecordingGlobal Global { get; }
    internal RecordingCompatible Compatible { get; }
    internal RecordingRefresh Refresh { get; }
    internal ApplyPlan ValidGlobalPlan { get; }
    internal ApplyPlan ValidCompatiblePlan { get; }
    internal OperationSnapshot GlobalSnapshot { get; }
    internal OperationSnapshot CompatibleSnapshot { get; }
    internal IProgress<ApplyProgress> Progress { get; } = new ProgressCollector();

    private static OperationSnapshot Snapshot(ApplicationMode mode) => new(
        OperationId,
        new DateTimeOffset(2026, 7, 27, 9, 0, 0, TimeSpan.Zero),
        new RegistryValueSnapshot("3", false, null),
        new RegistryValueSnapshot("4", false, null),
        [],
        false)
    {
        ApplicationMode = mode
    };
}

internal sealed class RecordingValidator(List<string> calls) : IThemeValidationService
{
    internal IReadOnlyList<string> Errors { get; set; } = [];

    public ThemeValidationResult Validate(FolderTheme theme)
    {
        calls.Add("validate");
        return new ThemeValidationResult(Errors);
    }
}

internal sealed class RecordingIcons(List<string> calls) : IIconArtifactService
{
    internal bool Fail { get; set; }
    internal IconSource? LastSource { get; private set; }

    public Task<IconArtifactResult> CreateAndVerifyAsync(IconSource source, Guid operationId, CancellationToken token)
    {
        calls.Add("render");
        LastSource = source;
        return Task.FromResult(Fail
            ? IconArtifactResult.Failure("render failed")
            : IconArtifactResult.Success(@"C:\Icons\theme.ico"));
    }
}

internal sealed class RecordingPlanner(List<string> calls) : IKnownFolderPlanningService
{
    internal IReadOnlyList<FolderPlanItem> Items { get; set; } = [];

    public async IAsyncEnumerable<FolderPlanItem> PlanTreeAsync(
        string root,
        [EnumeratorCancellation] CancellationToken token)
    {
        calls.Add($"plan:{root}");
        foreach (var item in Items)
        {
            token.ThrowIfCancellationRequested();
            yield return item;
            await Task.Yield();
        }
    }
}

internal sealed class RecordingBackup(List<string> calls) : IApplicationBackupService
{
    private readonly SemaphoreSlim restoreGate = new(1, 1);
    internal bool FailCreate { get; set; }
    internal bool FailMarkRestore { get; set; }
    internal bool RestoreCompleted { get; set; }
    internal Exception? LoadException { get; set; }
    internal OperationSnapshot? Latest { get; set; }

    public Task CreateAsync(OperationSnapshot snapshot, CancellationToken token)
    {
        calls.Add("snapshot");
        if (FailCreate) throw new IOException("snapshot failed");
        Latest = snapshot;
        return Task.CompletedTask;
    }

    public Task<OperationSnapshot?> LoadLatestAsync(CancellationToken token)
    {
        calls.Add("load-latest");
        if (LoadException is not null) throw LoadException;
        return Task.FromResult(Latest);
    }

    public async Task<IApplicationRestoreLease> AcquireRestoreLeaseAsync(Guid operationId, CancellationToken token)
    {
        await restoreGate.WaitAsync(token);
        return new RecordingRestoreLease(this, restoreGate, RestoreCompleted);
    }

    private sealed class RecordingRestoreLease(
        RecordingBackup owner,
        SemaphoreSlim restoreGate,
        bool alreadyRestored) : IApplicationRestoreLease
    {
        private int disposed;

        public bool AlreadyRestored { get; private set; } = alreadyRestored;

        public Task MarkCompletedAsync(DateTimeOffset completedAtUtc, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (owner.FailMarkRestore) throw new IOException("restore marker failed");
            owner.RestoreCompleted = true;
            AlreadyRestored = true;
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref disposed, 1) == 0) restoreGate.Release();
            return ValueTask.CompletedTask;
        }
    }
}

internal sealed class RecordingGlobal(List<string> calls) : IGlobalIconOperations
{
    private int restoreCallCount;
    internal bool FailApply { get; set; }
    internal bool FailRestore { get; set; }
    internal Exception? CaptureException { get; set; }
    internal Exception? RestoreException { get; set; }
    internal bool PauseRestore { get; set; }
    internal int RestoreCallCount => Volatile.Read(ref restoreCallCount);
    internal TaskCompletionSource RestoreStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    internal TaskCompletionSource ReleaseRestore { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public GlobalSnapshotResult CaptureSnapshot()
    {
        if (CaptureException is not null) throw CaptureException;
        return new GlobalSnapshotResult(
            true,
            new RegistryValueSnapshot("3", false, null),
            new RegistryValueSnapshot("4", false, null),
            null);
    }

    public OperationResult Apply(string icoPath)
    {
        calls.Add("global-write");
        return FailApply ? OperationResult.Failure("global failed") : OperationResult.Success();
    }

    public async Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token)
    {
        calls.Add("global-restore");
        Interlocked.Increment(ref restoreCallCount);
        RestoreStarted.TrySetResult();
        if (PauseRestore) await ReleaseRestore.Task.WaitAsync(token);
        if (RestoreException is not null) throw RestoreException;
        return FailRestore
            ? RestoreOperationResult.Failure("restore failed")
            : RestoreOperationResult.Success();
    }
}

internal sealed class RecordingCompatible(List<string> calls) : ICompatibleFolderOperations
{
    internal bool FailApply { get; set; }
    internal bool FailRestore { get; set; }
    internal IReadOnlyList<string> AppliedPaths { get; set; } = [@"C:\Themes"];
    internal IReadOnlyList<string> SkippedPaths { get; set; } = [];
    internal IReadOnlyList<string> RestoreSuccessfulPaths { get; set; } = [];
    internal IReadOnlyList<string> RestoreSkippedPaths { get; set; } = [];
    internal IReadOnlyList<string> ChangedPaths { get; set; } = [];
    internal IReadOnlyList<string> UnresolvedPaths { get; set; } = [];
    internal bool? RestoreSuccessOverride { get; set; }

    public Task<CompatibleOperationResult> ApplyAsync(
        IReadOnlyList<FolderPlanItem> plan,
        string icoPath,
        Guid operationId,
        CancellationToken token)
    {
        calls.Add("compatible-write");
        var failures = FailApply
            ? new[] { new ApplyFailure(ApplyPhase.SystemMutation, "compatible failed", plan.FirstOrDefault()?.Path) }
            : [];
        return Task.FromResult(new CompatibleOperationResult(
            !FailApply,
            AppliedPaths.Count,
            SkippedPaths.Count,
            failures.Length,
            false,
            FailApply ? "compatible failed" : null)
        {
            Successful = AppliedPaths.Select(path => new ApplyRecord(path, null)).ToArray(),
            SkippedItems = SkippedPaths.Select(path => new ApplyRecord(path, "Skipped")).ToArray(),
            Failures = failures,
            Unresolved = UnresolvedPaths
                .Select(path => new ApplyFailure(ApplyPhase.Rollback, "Desktop.ini remains unresolved.", path))
                .ToArray()
        });
    }

    public Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token)
    {
        calls.Add("compatible-restore");
        if (FailRestore)
        {
            return Task.FromResult(RestoreOperationResult.Failure("restore failed"));
        }

        var successful = RestoreSuccessfulPaths.Select(path => new ApplyRecord(path, "Restored")).ToArray();
        var skipped = RestoreSkippedPaths.Select(path => new ApplyRecord(path, "Missing")).ToArray();
        var changed = ChangedPaths.Select(path => new ApplyRecord(path, "Changed since apply")).ToArray();
        return Task.FromResult(new RestoreOperationResult(
            RestoreSuccessOverride ?? changed.Length == 0,
            successful,
            skipped,
            changed,
            changed.Select(item => new ApplyFailure(ApplyPhase.Restore, "Desktop.ini changed since apply.", item.Path)).ToArray()));
    }
}

internal sealed class RecordingRefresh(List<string> calls) : IApplicationShellRefreshService
{
    internal bool Fail { get; set; }
    internal Exception? Exception { get; set; }

    public OperationResult Refresh()
    {
        calls.Add("refresh");
        if (Exception is not null) throw Exception;
        return Fail ? OperationResult.Failure("refresh failed") : OperationResult.Success();
    }
}

internal sealed class ProgressCollector : IProgress<ApplyProgress>
{
    internal List<ApplyProgress> Values { get; } = [];
    public void Report(ApplyProgress value) => Values.Add(value);
}
