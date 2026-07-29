using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Recovery;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class ThemeApplicationServiceTests
{
    [Fact]
    public async Task PlanAsync_ImportedImage_DoesNotRunBuiltInThemeValidation()
    {
        var fixture = new ApplicationFixture();
        fixture.Validator.Errors = ["unused built-in theme failure"];
        var source = IconSource.ImportedImage(@"C:\Imports\photo.png");

        var plan = await fixture.Service.PlanAsync(
            new ApplyRequest(source, ApplicationMode.Global),
            CancellationToken.None);

        Assert.True(plan.CanApply);
        Assert.Equal(source, plan.IconSource);
        Assert.Empty(fixture.CallOrder);
    }

    [Fact]
    public async Task ApplyAsync_PassesReviewedIconSourceToArtifactService()
    {
        var fixture = new ApplicationFixture();
        var source = IconSource.ImportedImage(@"C:\Imports\photo.png");

        await fixture.Service.ApplyAsync(
            new ApplyPlan(source, ApplicationMode.Global, [], []),
            fixture.Progress,
            CancellationToken.None);

        Assert.Equal(source, fixture.Icons.LastSource);
    }

    [Fact]
    public async Task PlanAsync_GlobalMode_IsWriteFree()
    {
        var fixture = new ApplicationFixture();

        var plan = await fixture.Service.PlanAsync(
            new ApplyRequest(fixture.Theme, ApplicationMode.Global),
            CancellationToken.None);

        Assert.True(plan.CanApply);
        Assert.Empty(plan.Folders);
        Assert.Equal(["validate"], fixture.CallOrder);
    }

    [Fact]
    public async Task PlanAsync_CompatibilityMode_ReportsAllowedAndSkippedFolders()
    {
        var fixture = new ApplicationFixture();
        fixture.Planner.Items =
        [
            new(@"C:\Themes", FolderDecision.Allowed),
            new(@"C:\Themes\Desktop", FolderDecision.KnownFolder),
            new(@"C:\Themes\Existing", FolderDecision.ExistingCustomization)
        ];

        var plan = await fixture.Service.PlanAsync(
            new ApplyRequest(fixture.Theme, ApplicationMode.Compatible, [@"C:\Themes"]),
            CancellationToken.None);

        Assert.True(plan.CanApply);
        Assert.Equal(1, plan.AllowedCount);
        Assert.Equal(2, plan.SkippedCount);
        Assert.Equal(1, plan.SkippedReasons[FolderDecision.KnownFolder]);
        Assert.Equal(1, plan.SkippedReasons[FolderDecision.ExistingCustomization]);
        Assert.Equal(["validate", "plan:C:\\Themes"], fixture.CallOrder);
    }

    [Fact]
    public async Task PlanAsync_InvalidTheme_DoesNotInspectFolders()
    {
        var fixture = new ApplicationFixture();
        fixture.Validator.Errors = ["Opacity"];

        var plan = await fixture.Service.PlanAsync(
            new ApplyRequest(fixture.Theme, ApplicationMode.Compatible, [@"C:\Themes"]),
            CancellationToken.None);

        Assert.False(plan.CanApply);
        Assert.Equal(["Opacity"], plan.ValidationErrors);
        Assert.Equal(["validate"], fixture.CallOrder);
    }

    [Fact]
    public async Task Apply_CreatesIconThenSnapshotBeforeSystemWrite()
    {
        var fixture = new ApplicationFixture();

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidGlobalPlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal(["render", "snapshot", "global-write", "refresh"], fixture.CallOrder);
    }

    [Fact]
    public async Task Apply_RenderFailure_DoesNotCreateSnapshotOrWriteSystemState()
    {
        var fixture = new ApplicationFixture();
        fixture.Icons.Fail = true;

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidGlobalPlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.False(result.RollbackAttempted);
        Assert.Equal(["render"], fixture.CallOrder);
    }

    [Fact]
    public async Task Apply_SnapshotFailure_DoesNotWriteSystemState()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.FailCreate = true;

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidGlobalPlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.False(result.RollbackAttempted);
        Assert.Equal(["render", "snapshot"], fixture.CallOrder);
    }

    [Fact]
    public async Task Apply_SnapshotCaptureException_IsReportedWithoutSystemWrite()
    {
        var fixture = new ApplicationFixture();
        fixture.Global.CaptureException = new InvalidOperationException("registry read failed");

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidGlobalPlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains(result.Failures, failure => failure.Phase == ApplyPhase.Snapshot);
        Assert.Equal(["render"], fixture.CallOrder);
    }

    [Fact]
    public async Task FailedSystemWrite_AttemptsRollbackAndReportsBothResults()
    {
        var fixture = new ApplicationFixture();
        fixture.Global.FailApply = true;

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidGlobalPlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.True(result.RollbackAttempted);
        Assert.True(result.RollbackSucceeded);
        Assert.True(fixture.Backup.RestoreCompleted);
        Assert.Equal(["render", "snapshot", "global-write", "global-restore", "refresh"], fixture.CallOrder);
    }

    [Fact]
    public async Task FailedSystemWrite_WhenRollbackMarkerFails_DoesNotClaimRollbackSuccess()
    {
        var fixture = new ApplicationFixture();
        fixture.Global.FailApply = true;
        fixture.Backup.FailMarkRestore = true;

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidGlobalPlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.True(result.RollbackAttempted);
        Assert.False(result.RollbackSucceeded);
        Assert.False(fixture.Backup.RestoreCompleted);
        Assert.Contains(result.Failures, failure => failure.Phase == ApplyPhase.Rollback);
    }

    [Fact]
    public async Task FailedCompatibleApply_WithUnresolvedMutation_DoesNotMarkRollbackComplete()
    {
        var fixture = new ApplicationFixture();
        var unresolvedPath = @"C:\Themes\unresolved";
        fixture.Compatible.FailApply = true;
        fixture.Compatible.UnresolvedPaths = [unresolvedPath];

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidCompatiblePlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.True(result.RollbackAttempted);
        Assert.False(result.RollbackSucceeded);
        Assert.False(fixture.Backup.RestoreCompleted);
        Assert.Contains(result.Failures, failure =>
            failure.Phase == ApplyPhase.Rollback
            && failure.Path == unresolvedPath);
    }

    [Fact]
    public async Task CompatibleApply_UsesExactPerFolderOutcomesRatherThanCountOrderInference()
    {
        var fixture = new ApplicationFixture();
        var first = @"C:\Themes\first";
        var second = @"C:\Themes\second";
        var plan = fixture.ValidCompatiblePlan with
        {
            Folders =
            [
                new(first, FolderDecision.Allowed, @"C:\Themes", first, @"C:\Themes"),
                new(second, FolderDecision.Allowed, @"C:\Themes", second, @"C:\Themes")
            ]
        };
        fixture.Compatible.AppliedPaths = [second];
        fixture.Compatible.SkippedPaths = [first];

        var result = await fixture.Service.ApplyAsync(plan, fixture.Progress, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal([second], result.Successful.Select(item => item.Path));
        Assert.Equal([first], result.Skipped.Select(item => item.Path));
    }

    [Fact]
    public async Task CompatibleAdapter_MapsUnresolvedPathFromRealService()
    {
        using var folder = new TemporaryFolderFixture();
        var backupFileSystem = new InMemoryFileSystem();
        backupFileSystem.FailOnReplaceCalls.UnionWith([1, 2]);
        var backup = BackupService.CreateForTesting(backupFileSystem, @"C:\Backups");
        var operationId = Guid.NewGuid();
        await backup.CreateAsync(new OperationSnapshot(
            operationId,
            DateTimeOffset.UtcNow,
            new RegistryValueSnapshot("3", false, null),
            new RegistryValueSnapshot("4", false, null),
            [],
            false)
        {
            ApplicationMode = ApplicationMode.Compatible
        });
        var service = CompatibleFolderService.CreateForTesting(backup, new ThrowingAdapterDeletion());
        var adapter = new DefaultCompatibleFolderOperations(service);

        var result = await adapter.ApplyAsync(
            folder.Plan,
            folder.IcoPath,
            operationId,
            CancellationToken.None);

        var unresolved = Assert.Single(result.Unresolved);
        Assert.Equal(folder.FolderPath, unresolved.Path);
        Assert.Equal(ApplyPhase.Rollback, unresolved.Phase);
    }

    [Fact]
    public async Task CompatibleRestore_ChangedHashIsPartialAndDoesNotWriteRestoreMarker()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.Latest = fixture.CompatibleSnapshot;
        fixture.Compatible.ChangedPaths = [@"C:\Themes\changed"];

        var result = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);

        Assert.False(result.Success);
        Assert.False(result.AlreadyRestored);
        Assert.False(fixture.Backup.RestoreCompleted);
        Assert.Equal([@"C:\Themes\changed"], result.Changed.Select(item => item.Path));
        Assert.Contains(result.Failures, failure =>
            failure.Path == @"C:\Themes\changed" && failure.Phase == ApplyPhase.Restore);
    }

    [Fact]
    public async Task FailedRollback_IsReportedAndCannotProduceSuccess()
    {
        var fixture = new ApplicationFixture();
        fixture.Global.FailApply = true;
        fixture.Global.FailRestore = true;

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidGlobalPlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.True(result.RollbackAttempted);
        Assert.False(result.RollbackSucceeded);
        Assert.Contains(result.Failures, failure => failure.Phase == ApplyPhase.Rollback);
    }

    [Fact]
    public async Task FailedRefresh_RollsBackTheMatchingMode()
    {
        var fixture = new ApplicationFixture();
        fixture.Refresh.Fail = true;

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidCompatiblePlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.False(result.RollbackSucceeded);
        Assert.Equal(
            ["render", "snapshot", "compatible-write", "refresh", "compatible-restore", "refresh"],
            fixture.CallOrder);
    }

    [Fact]
    public async Task RefreshException_AfterSnapshot_RollsBackInsteadOfEscaping()
    {
        var fixture = new ApplicationFixture();
        fixture.Refresh.Exception = new InvalidOperationException("refresh crashed");

        var result = await fixture.Service.ApplyAsync(
            fixture.ValidGlobalPlan,
            fixture.Progress,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.False(result.RollbackSucceeded);
        Assert.Equal(
            ["render", "snapshot", "global-write", "refresh", "global-restore", "refresh"],
            fixture.CallOrder);
    }

    [Fact]
    public async Task RestoreLatest_IsIdempotent()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.Latest = fixture.GlobalSnapshot;

        var first = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);
        var second = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);

        Assert.True(first.Success);
        Assert.False(first.AlreadyRestored);
        Assert.True(second.Success);
        Assert.True(second.AlreadyRestored);
        Assert.Equal(
            ["load-latest", "global-restore", "refresh", "load-latest"],
            fixture.CallOrder);
    }

    [Fact]
    public async Task RestoreLatest_NewServiceInstance_UsesDurableCompletionState()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.Latest = fixture.GlobalSnapshot;
        var secondService = fixture.CreateService();

        var first = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);
        var second = await secondService.RestoreLatestAsync(fixture.Progress, CancellationToken.None);

        Assert.True(first.Success);
        Assert.False(first.AlreadyRestored);
        Assert.True(second.Success);
        Assert.True(second.AlreadyRestored);
        Assert.Equal(1, fixture.Global.RestoreCallCount);
    }

    [Fact]
    public async Task RestoreLatest_ConcurrentServiceInstances_PerformOneRestore()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.Latest = fixture.GlobalSnapshot;
        fixture.Global.PauseRestore = true;
        var secondService = fixture.CreateService();

        var firstTask = fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);
        await fixture.Global.RestoreStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
        var secondTask = secondService.RestoreLatestAsync(fixture.Progress, CancellationToken.None);
        fixture.Global.ReleaseRestore.TrySetResult();
        var results = await Task.WhenAll(firstTask, secondTask);

        Assert.Single(results, result => !result.AlreadyRestored);
        Assert.Single(results, result => result.AlreadyRestored);
        Assert.All(results, result => Assert.True(result.Success));
        Assert.Equal(1, fixture.Global.RestoreCallCount);
    }

    [Fact]
    public async Task RestoreLatest_RestoreException_ReturnsFailure()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.Latest = fixture.GlobalSnapshot;
        fixture.Global.RestoreException = new InvalidOperationException("restore crashed");

        var result = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains(result.Failures, failure =>
            failure.Phase == ApplyPhase.Restore && failure.Error.Contains("restore crashed"));
        Assert.False(fixture.Backup.RestoreCompleted);
    }

    [Fact]
    public async Task RestoreLatest_BackupLoadException_ReturnsFailure()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.LoadException = new Exception("backup load crashed");

        var result = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains(result.Failures, failure =>
            failure.Phase == ApplyPhase.Restore && failure.Error.Contains("backup load crashed"));
        Assert.Equal(0, fixture.Global.RestoreCallCount);
    }

    [Fact]
    public async Task RestoreLatest_RestoreCancellation_ReturnsFailure()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.Latest = fixture.GlobalSnapshot;
        fixture.Global.RestoreException = new OperationCanceledException("restore cancelled");

        var result = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains(result.Failures, failure => failure.Phase == ApplyPhase.Restore);
        Assert.False(fixture.Backup.RestoreCompleted);
    }

    [Fact]
    public async Task RestoreLatest_MarkerFailure_PreservesCompletedOutcomesAndAllowsRetry()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.Latest = fixture.CompatibleSnapshot;
        fixture.Compatible.RestoreSuccessfulPaths = [@"C:\Themes\restored"];
        fixture.Compatible.RestoreSkippedPaths = [@"C:\Themes\missing"];
        fixture.Compatible.ChangedPaths = [@"C:\Themes\changed"];
        fixture.Compatible.RestoreSuccessOverride = true;
        fixture.Backup.FailMarkRestore = true;

        var first = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);
        fixture.Backup.FailMarkRestore = false;
        var second = await fixture.CreateService().RestoreLatestAsync(fixture.Progress, CancellationToken.None);

        Assert.False(first.Success);
        Assert.Contains(first.Failures, failure => failure.Phase == ApplyPhase.Restore);
        Assert.Equal(@"C:\Themes\restored", Assert.Single(first.Successful).Path);
        Assert.Equal(@"C:\Themes\missing", Assert.Single(first.Skipped).Path);
        Assert.Equal(@"C:\Themes\changed", Assert.Single(first.Changed).Path);
        Assert.True(second.Success);
        Assert.False(second.AlreadyRestored);
        Assert.Equal(2, fixture.CallOrder.Count(call => call == "compatible-restore"));
    }

    [Fact]
    public async Task RestoreLatest_RefreshFailure_PreservesCompletedOutcomes()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.Latest = fixture.CompatibleSnapshot;
        fixture.Compatible.RestoreSuccessfulPaths = [@"C:\Themes\restored"];
        fixture.Compatible.RestoreSkippedPaths = [@"C:\Themes\missing"];
        fixture.Compatible.ChangedPaths = [@"C:\Themes\changed"];
        fixture.Compatible.RestoreSuccessOverride = true;
        fixture.Refresh.Fail = true;

        var result = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);

        Assert.False(result.Success);
        Assert.False(result.AlreadyRestored);
        Assert.Equal(["load-latest", "compatible-restore", "refresh"], fixture.CallOrder);
        Assert.False(fixture.Backup.RestoreCompleted);
        Assert.Equal(@"C:\Themes\restored", Assert.Single(result.Successful).Path);
        Assert.Equal(@"C:\Themes\missing", Assert.Single(result.Skipped).Path);
        Assert.Equal(@"C:\Themes\changed", Assert.Single(result.Changed).Path);
    }

    [Fact]
    public async Task RestoreLatest_RefreshException_PreservesCompletedOutcomesWithoutMarker()
    {
        var fixture = new ApplicationFixture();
        fixture.Backup.Latest = fixture.CompatibleSnapshot;
        fixture.Compatible.RestoreSuccessfulPaths = [@"C:\Themes\restored"];
        fixture.Compatible.RestoreSkippedPaths = [@"C:\Themes\missing"];
        fixture.Compatible.ChangedPaths = [@"C:\Themes\changed"];
        fixture.Compatible.RestoreSuccessOverride = true;
        fixture.Refresh.Exception = new InvalidOperationException("refresh crashed");

        var result = await fixture.Service.RestoreLatestAsync(fixture.Progress, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains(result.Failures, failure =>
            failure.Phase == ApplyPhase.Refresh && failure.Error.Contains("refresh crashed"));
        Assert.False(fixture.Backup.RestoreCompleted);
        Assert.Equal(@"C:\Themes\restored", Assert.Single(result.Successful).Path);
        Assert.Equal(@"C:\Themes\missing", Assert.Single(result.Skipped).Path);
        Assert.Equal(@"C:\Themes\changed", Assert.Single(result.Changed).Path);
    }

    private sealed class ThrowingAdapterDeletion : IOwnedFileDeletionService
    {
        public OwnedFileDeletionOutcome DeleteIfHashMatches(string path, string expectedSha256) =>
            throw new InvalidOperationException("Simulated non-I/O deletion failure.");
    }
}
