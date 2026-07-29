using System.IO;
using System.Windows.Media.Imaging;
using FolderThemeStudio.Core.Recovery;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Application;

internal interface IThemeValidationService
{
    ThemeValidationResult Validate(FolderTheme theme);
}

internal interface IIconArtifactService
{
    Task<IconArtifactResult> CreateAndVerifyAsync(IconSource source, Guid operationId, CancellationToken token);
}

internal interface IKnownFolderPlanningService
{
    IAsyncEnumerable<FolderPlanItem> PlanTreeAsync(string root, CancellationToken token);
}

internal interface IApplicationBackupService
{
    Task CreateAsync(OperationSnapshot snapshot, CancellationToken token);
    Task<OperationSnapshot?> LoadLatestAsync(CancellationToken token);
    Task<IApplicationRestoreLease> AcquireRestoreLeaseAsync(Guid operationId, CancellationToken token);
}

internal interface IApplicationRestoreLease : IAsyncDisposable
{
    bool AlreadyRestored { get; }
    Task MarkCompletedAsync(DateTimeOffset completedAtUtc, CancellationToken token);
}

internal interface IGlobalIconOperations
{
    GlobalSnapshotResult CaptureSnapshot();
    OperationResult Apply(string icoPath);
    Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token);
}

internal interface ICompatibleFolderOperations
{
    Task<CompatibleOperationResult> ApplyAsync(
        IReadOnlyList<FolderPlanItem> plan,
        string icoPath,
        Guid operationId,
        CancellationToken token);

    Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token);
}

internal interface IApplicationShellRefreshService
{
    OperationResult Refresh();
}

internal sealed record IconArtifactResult(bool IsSuccess, string? Path, string? Error)
{
    internal static IconArtifactResult Success(string path) => new(true, path, null);
    internal static IconArtifactResult Failure(string error) => new(false, null, error);
}

internal sealed record OperationResult(bool IsSuccess, string? Error)
{
    internal static OperationResult Success() => new(true, null);
    internal static OperationResult Failure(string error) => new(false, error);
}

internal sealed record GlobalSnapshotResult(
    bool IsSuccess,
    RegistryValueSnapshot? RegistryValue3,
    RegistryValueSnapshot? RegistryValue4,
    string? Error);

internal sealed record CompatibleOperationResult(
    bool IsSuccess,
    int Applied,
    int Skipped,
    int Failed,
    bool Cancelled,
    string? Error)
{
    internal IReadOnlyList<ApplyRecord> Successful { get; init; } = [];
    internal IReadOnlyList<ApplyRecord> SkippedItems { get; init; } = [];
    internal IReadOnlyList<ApplyFailure> Failures { get; init; } = [];
    internal IReadOnlyList<ApplyFailure> Unresolved { get; init; } = [];
}

internal sealed record RestoreOperationResult(
    bool IsSuccess,
    IReadOnlyList<ApplyRecord> Successful,
    IReadOnlyList<ApplyRecord> Skipped,
    IReadOnlyList<ApplyRecord> Changed,
    IReadOnlyList<ApplyFailure> Failures)
{
    internal static RestoreOperationResult Success() => new(true, [], [], [], []);
    internal static RestoreOperationResult Failure(string error, string? path = null) =>
        new(false, [], [], [], [new ApplyFailure(ApplyPhase.Restore, error, path)]);
}

public sealed class ThemeApplicationService
{
    private readonly IThemeValidationService validator;
    private readonly IIconArtifactService icons;
    private readonly IKnownFolderPlanningService planner;
    private readonly IApplicationBackupService backup;
    private readonly IGlobalIconOperations global;
    private readonly ICompatibleFolderOperations compatible;
    private readonly IApplicationShellRefreshService refresh;
    private readonly Func<Guid> newOperationId;
    private readonly Func<DateTimeOffset> utcNow;

    public ThemeApplicationService()
    {
        var backupService = new BackupService();
        validator = new DefaultThemeValidationService();
        icons = new DefaultIconArtifactService();
        planner = new DefaultKnownFolderPlanningService(new KnownFolderService());
        backup = new DefaultApplicationBackupService(backupService);
        global = new DefaultGlobalIconOperations(new GlobalIconService());
        compatible = new DefaultCompatibleFolderOperations(new CompatibleFolderService(backupService));
        refresh = new DefaultApplicationShellRefreshService(new ShellRefreshService());
        newOperationId = Guid.NewGuid;
        utcNow = () => DateTimeOffset.UtcNow;
    }

    internal ThemeApplicationService(
        IThemeValidationService validator,
        IIconArtifactService icons,
        IKnownFolderPlanningService planner,
        IApplicationBackupService backup,
        IGlobalIconOperations global,
        ICompatibleFolderOperations compatible,
        IApplicationShellRefreshService refresh,
        Func<Guid> newOperationId,
        Func<DateTimeOffset> utcNow)
    {
        this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this.icons = icons ?? throw new ArgumentNullException(nameof(icons));
        this.planner = planner ?? throw new ArgumentNullException(nameof(planner));
        this.backup = backup ?? throw new ArgumentNullException(nameof(backup));
        this.global = global ?? throw new ArgumentNullException(nameof(global));
        this.compatible = compatible ?? throw new ArgumentNullException(nameof(compatible));
        this.refresh = refresh ?? throw new ArgumentNullException(nameof(refresh));
        this.newOperationId = newOperationId ?? throw new ArgumentNullException(nameof(newOperationId));
        this.utcNow = utcNow ?? throw new ArgumentNullException(nameof(utcNow));
    }

    public async Task<ApplyPlan> PlanAsync(ApplyRequest request, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.IconSource);
        token.ThrowIfCancellationRequested();

        var validationErrors = request.IconSource.Kind == IconSourceKind.BuiltIn
            ? validator.Validate(request.IconSource.Theme!).Errors
            : [];
        if (validationErrors.Count > 0)
        {
            return new ApplyPlan(request.IconSource, request.Mode, [], validationErrors);
        }

        if (request.Mode == ApplicationMode.Global)
        {
            return new ApplyPlan(request.IconSource, request.Mode, [], []);
        }

        if (request.ExplicitRoots is null || request.ExplicitRoots.Count == 0)
        {
            return new ApplyPlan(
                request.IconSource,
                request.Mode,
                [],
                ["Compatibility mode requires at least one explicit root."]);
        }

        var folders = new List<FolderPlanItem>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var root in request.ExplicitRoots)
        {
            token.ThrowIfCancellationRequested();
            await foreach (var item in planner.PlanTreeAsync(root, token).WithCancellation(token).ConfigureAwait(false))
            {
                var rootedItem = item with
                {
                    SelectedRoot = item.SelectedRoot ?? root,
                    CanonicalPath = item.CanonicalPath ?? item.Path,
                    CanonicalRoot = item.CanonicalRoot ?? root
                };
                if (seen.Add(rootedItem.Path))
                {
                    folders.Add(rootedItem);
                }
            }
        }

        return new ApplyPlan(request.IconSource, request.Mode, folders, []);
    }

    public async Task<ApplyResult> ApplyAsync(
        ApplyPlan plan,
        IProgress<ApplyProgress> progress,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(progress);

        if (!plan.CanApply)
        {
            return FailedBeforeSnapshot(
                plan.ValidationErrors.Select(error => new ApplyFailure(ApplyPhase.Validation, error)).ToArray());
        }

        var operationId = newOperationId();
        progress.Report(new ApplyProgress(ApplyPhase.Rendering, "Rendering and verifying the icon."));
        IconArtifactResult artifact;
        try
        {
            artifact = await icons.CreateAndVerifyAsync(plan.IconSource, operationId, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Rendering, "Icon generation was cancelled.")]);
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Rendering, exception.Message)]);
        }

        if (!artifact.IsSuccess || string.IsNullOrWhiteSpace(artifact.Path))
        {
            return FailedBeforeSnapshot([
                new ApplyFailure(ApplyPhase.Rendering, artifact.Error ?? "Icon generation did not produce a verified ICO.")]);
        }

        SnapshotBuildResult snapshotResult;
        try
        {
            snapshotResult = BuildSnapshot(plan.Mode, operationId);
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Snapshot, exception.Message)]);
        }
        if (!snapshotResult.IsSuccess || snapshotResult.Snapshot is null)
        {
            return FailedBeforeSnapshot([
                new ApplyFailure(ApplyPhase.Snapshot, snapshotResult.Error ?? "Could not capture the original system state.")]);
        }

        progress.Report(new ApplyProgress(ApplyPhase.Snapshot, "Saving the recovery snapshot."));
        try
        {
            await backup.CreateAsync(snapshotResult.Snapshot, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Snapshot, "Snapshot creation was cancelled.")]);
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Snapshot, exception.Message)]);
        }

        progress.Report(new ApplyProgress(ApplyPhase.SystemMutation, "Applying the folder icon."));
        var mutation = await ApplyMutationAsync(plan, artifact.Path, operationId, token).ConfigureAwait(false);
        if (!mutation.IsSuccess)
        {
            return await RollBackFailedApplyAsync(
                plan,
                snapshotResult.Snapshot,
                operationId,
                mutation,
                progress).ConfigureAwait(false);
        }

        progress.Report(new ApplyProgress(ApplyPhase.Refresh, "Refreshing Windows folder icons."));
        var refreshResult = TryRefresh();
        if (!refreshResult.IsSuccess)
        {
            var refreshFailure = new MutationResult(
                false,
                mutation.Successful,
                mutation.Skipped,
                [new ApplyFailure(ApplyPhase.Refresh, refreshResult.Error ?? "Shell refresh failed.")]);
            return await RollBackFailedApplyAsync(
                plan,
                snapshotResult.Snapshot,
                operationId,
                refreshFailure,
                progress).ConfigureAwait(false);
        }

        return new ApplyResult(
            true,
            operationId,
            mutation.Successful,
            mutation.Skipped,
            [],
            false,
            false)
        {
            IconArtifactPath = artifact.Path
        };
    }

    public async Task<RestoreResult> RestoreLatestAsync(
        IProgress<ApplyProgress> progress,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(progress);
        OperationSnapshot? snapshot;
        try
        {
            snapshot = await backup.LoadLatestAsync(token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return new RestoreResult(false, false, null, [new ApplyFailure(ApplyPhase.Restore, "Restore was cancelled.")]);
        }
        catch (Exception exception)
        {
            return new RestoreResult(false, false, null, [new ApplyFailure(ApplyPhase.Restore, exception.Message)]);
        }

        if (snapshot is null)
        {
            return new RestoreResult(true, true, null, []);
        }

        IApplicationRestoreLease restoreLease;
        try
        {
            restoreLease = await backup.AcquireRestoreLeaseAsync(snapshot.Id, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message.Length == 0 ? "Restore was cancelled." : exception.Message);
        }
        catch (Exception exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message);
        }

        await using var heldRestoreLease = restoreLease;
        if (heldRestoreLease.AlreadyRestored)
        {
            return new RestoreResult(true, true, snapshot.Id, []);
        }

        progress.Report(new ApplyProgress(ApplyPhase.Restore, "Restoring the latest saved state."));
        RestoreOperationResult restored;
        try
        {
            restored = await RestoreMatchingAsync(snapshot, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message.Length == 0 ? "Restore was cancelled." : exception.Message);
        }
        catch (Exception exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message);
        }

        if (!restored.IsSuccess)
        {
            return new RestoreResult(false, false, snapshot.Id, restored.Failures)
            {
                Successful = restored.Successful,
                Skipped = restored.Skipped,
                Changed = restored.Changed
            };
        }

        progress.Report(new ApplyProgress(ApplyPhase.Refresh, "Refreshing Windows folder icons."));
        OperationResult refreshed;
        try
        {
            token.ThrowIfCancellationRequested();
            refreshed = refresh.Refresh();
        }
        catch (OperationCanceledException exception)
        {
            return RestoreFailure(
                snapshot.Id,
                exception.Message.Length == 0 ? "Restore was cancelled before refresh." : exception.Message,
                restored,
                ApplyPhase.Refresh);
        }
        catch (Exception exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message, restored, ApplyPhase.Refresh);
        }

        if (!refreshed.IsSuccess)
        {
            return RestoreFailure(
                snapshot.Id,
                refreshed.Error ?? "Shell refresh failed.",
                restored,
                ApplyPhase.Refresh);
        }

        try
        {
            token.ThrowIfCancellationRequested();
            await heldRestoreLease.MarkCompletedAsync(utcNow(), token).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            return RestoreFailure(
                snapshot.Id,
                exception.Message.Length == 0 ? "Restore completion was cancelled." : exception.Message,
                restored);
        }
        catch (Exception exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message, restored);
        }

        return new RestoreResult(true, false, snapshot.Id, [])
        {
            Successful = restored.Successful,
            Skipped = restored.Skipped,
            Changed = restored.Changed
        };
    }

    private SnapshotBuildResult BuildSnapshot(ApplicationMode mode, Guid operationId)
    {
        if (mode == ApplicationMode.Compatible)
        {
            return SnapshotBuildResult.Success(new OperationSnapshot(
                operationId,
                utcNow(),
                new RegistryValueSnapshot("3", false, null),
                new RegistryValueSnapshot("4", false, null),
                [],
                false)
            {
                ApplicationMode = ApplicationMode.Compatible
            });
        }

        var captured = global.CaptureSnapshot();
        if (!captured.IsSuccess || captured.RegistryValue3 is null || captured.RegistryValue4 is null)
        {
            return SnapshotBuildResult.Failure(captured.Error ?? "Could not read the current folder icon mappings.");
        }

        return SnapshotBuildResult.Success(new OperationSnapshot(
            operationId,
            utcNow(),
            captured.RegistryValue3,
            captured.RegistryValue4,
            [],
            false)
        {
            ApplicationMode = ApplicationMode.Global
        });
    }

    private async Task<MutationResult> ApplyMutationAsync(
        ApplyPlan plan,
        string icoPath,
        Guid operationId,
        CancellationToken token)
    {
        try
        {
            if (plan.Mode == ApplicationMode.Global)
            {
                var globalResult = global.Apply(icoPath);
                return globalResult.IsSuccess
                    ? new MutationResult(true, [new ApplyRecord(null, "Global folder icon mapping applied.")], [], [])
                    : new MutationResult(false, [], [], [
                        new ApplyFailure(ApplyPhase.SystemMutation, globalResult.Error ?? "Global icon application failed.")]);
            }

            var result = await compatible.ApplyAsync(plan.Folders, icoPath, operationId, token).ConfigureAwait(false);
            var skipped = result.SkippedItems;
            var successful = result.Successful;
            var failures = result.Failures.Count > 0
                ? result.Failures
                : result.IsSuccess && result.Failed == 0 && !result.Cancelled
                    ? []
                    : [new ApplyFailure(
                        ApplyPhase.SystemMutation,
                        result.Error ?? (result.Cancelled
                            ? "Compatibility application was cancelled."
                            : $"Compatibility application failed for {result.Failed} folder(s)."))];
            return new MutationResult(failures.Count == 0, successful, skipped, failures)
            {
                Unresolved = result.Unresolved
            };
        }
        catch (OperationCanceledException)
        {
            return new MutationResult(false, [], [], [
                new ApplyFailure(ApplyPhase.SystemMutation, "System mutation was cancelled.")]);
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return new MutationResult(false, [], [], [new ApplyFailure(ApplyPhase.SystemMutation, exception.Message)]);
        }
    }

    private async Task<ApplyResult> RollBackFailedApplyAsync(
        ApplyPlan plan,
        OperationSnapshot snapshot,
        Guid operationId,
        MutationResult mutation,
        IProgress<ApplyProgress> progress)
    {
        progress.Report(new ApplyProgress(ApplyPhase.Rollback, "Rolling back the failed operation."));
        RestoreOperationResult rollback;
        var rollbackCompletedDurably = false;
        try
        {
            await using var restoreLease = await backup.AcquireRestoreLeaseAsync(operationId, CancellationToken.None).ConfigureAwait(false);
            if (restoreLease.AlreadyRestored)
            {
                rollback = RestoreOperationResult.Success();
                rollbackCompletedDurably = mutation.Unresolved.Count == 0;
            }
            else
            {
                rollback = plan.Mode == ApplicationMode.Global
                    ? await global.RestoreAsync(snapshot, CancellationToken.None).ConfigureAwait(false)
                    : await compatible.RestoreAsync(snapshot, CancellationToken.None).ConfigureAwait(false);
                if (rollback.IsSuccess)
                {
                    var refreshed = TryRefresh();
                    if (!refreshed.IsSuccess)
                    {
                        rollback = RestoreOperationResult.Failure(refreshed.Error ?? "Shell refresh after rollback failed.");
                    }
                    else if (mutation.Unresolved.Count == 0)
                    {
                        await restoreLease.MarkCompletedAsync(utcNow(), CancellationToken.None).ConfigureAwait(false);
                        rollbackCompletedDurably = true;
                    }
                }
            }
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            rollback = RestoreOperationResult.Failure(exception.Message);
        }

        var failures = mutation.Failures.ToList();
        failures.AddRange(mutation.Unresolved.Select(failure => failure with { Phase = ApplyPhase.Rollback }));
        if (!rollback.IsSuccess || !rollbackCompletedDurably)
        {
            if (rollback.Failures.Count == 0 && mutation.Unresolved.Count == 0)
            {
                failures.Add(new ApplyFailure(ApplyPhase.Rollback, "Rollback did not reach durable completion."));
            }
            else
            {
                failures.AddRange(rollback.Failures.Select(failure => failure with { Phase = ApplyPhase.Rollback }));
            }
        }

        return new ApplyResult(
            false,
            operationId,
            mutation.Successful,
            mutation.Skipped,
            failures,
            true,
            rollback.IsSuccess && rollbackCompletedDurably);
    }

    private Task<RestoreOperationResult> RestoreMatchingAsync(OperationSnapshot snapshot, CancellationToken token) =>
        snapshot.ApplicationMode == ApplicationMode.Global
            ? global.RestoreAsync(snapshot, token)
            : compatible.RestoreAsync(snapshot, token);

    private OperationResult TryRefresh()
    {
        try
        {
            return refresh.Refresh();
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return OperationResult.Failure(exception.Message);
        }
    }

    private static ApplyResult FailedBeforeSnapshot(IReadOnlyList<ApplyFailure> failures) =>
        new(false, null, [], [], failures, false, false);

    private static RestoreResult RestoreFailure(Guid? operationId, string error, ApplyPhase phase = ApplyPhase.Restore) =>
        new(false, false, operationId, [new ApplyFailure(phase, error)]);

    private static RestoreResult RestoreFailure(
        Guid? operationId,
        string error,
        RestoreOperationResult restored,
        ApplyPhase phase = ApplyPhase.Restore) =>
        new RestoreResult(false, false, operationId, [new ApplyFailure(phase, error)])
        {
            Successful = restored.Successful,
            Skipped = restored.Skipped,
            Changed = restored.Changed
        };

    private static bool IsOperationalFailure(Exception exception) =>
        exception is IOException
            or UnauthorizedAccessException
            or System.Security.SecurityException
            or InvalidOperationException
            or ArgumentException;

    private sealed record SnapshotBuildResult(bool IsSuccess, OperationSnapshot? Snapshot, string? Error)
    {
        internal static SnapshotBuildResult Success(OperationSnapshot snapshot) => new(true, snapshot, null);
        internal static SnapshotBuildResult Failure(string error) => new(false, null, error);
    }

    private sealed record MutationResult(
        bool IsSuccess,
        IReadOnlyList<ApplyRecord> Successful,
        IReadOnlyList<ApplyRecord> Skipped,
        IReadOnlyList<ApplyFailure> Failures)
    {
        internal IReadOnlyList<ApplyFailure> Unresolved { get; init; } = [];
    }
}

internal sealed class DefaultThemeValidationService : IThemeValidationService
{
    public ThemeValidationResult Validate(FolderTheme theme) => ThemeValidator.Validate(theme);
}

internal sealed class DefaultKnownFolderPlanningService(KnownFolderService service) : IKnownFolderPlanningService
{
    public IAsyncEnumerable<FolderPlanItem> PlanTreeAsync(string root, CancellationToken token) =>
        service.PlanTreeAsync(root, token);
}

internal sealed class DefaultApplicationBackupService(BackupService service) : IApplicationBackupService
{
    public Task CreateAsync(OperationSnapshot snapshot, CancellationToken token) => service.CreateAsync(snapshot, token);
    public Task<OperationSnapshot?> LoadLatestAsync(CancellationToken token) => service.LoadLatestAsync(token);

    public async Task<IApplicationRestoreLease> AcquireRestoreLeaseAsync(Guid operationId, CancellationToken token) =>
        new DefaultApplicationRestoreLease(await service.AcquireRestoreLeaseAsync(operationId, token).ConfigureAwait(false));
}

internal sealed class DefaultApplicationRestoreLease(BackupService.RestoreLease lease) : IApplicationRestoreLease
{
    public bool AlreadyRestored => lease.AlreadyRestored;

    public Task MarkCompletedAsync(DateTimeOffset completedAtUtc, CancellationToken token) =>
        lease.MarkCompletedAsync(completedAtUtc, token);

    public ValueTask DisposeAsync() => lease.DisposeAsync();
}

internal sealed class DefaultGlobalIconOperations(GlobalIconService service) : IGlobalIconOperations
{
    public GlobalSnapshotResult CaptureSnapshot()
    {
        var result = service.CaptureSnapshot();
        return new GlobalSnapshotResult(
            result.Success,
            result.RegistryValue3,
            result.RegistryValue4,
            result.Error);
    }

    public OperationResult Apply(string icoPath)
    {
        var result = service.ApplyWithoutRefresh(icoPath);
        return result.Success ? OperationResult.Success() : OperationResult.Failure(result.Error ?? "Global application failed.");
    }

    public Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var result = service.RestoreWithoutRefresh(snapshot);
        return Task.FromResult(result.Success
            ? RestoreOperationResult.Success()
            : RestoreOperationResult.Failure(result.Error ?? "Global restore failed."));
    }
}

internal sealed class DefaultCompatibleFolderOperations(CompatibleFolderService service) : ICompatibleFolderOperations
{
    public async Task<CompatibleOperationResult> ApplyAsync(
        IReadOnlyList<FolderPlanItem> plan,
        string icoPath,
        Guid operationId,
        CancellationToken token)
    {
        var result = await service.ApplyWithExistingSnapshotAsync(plan, icoPath, operationId, token).ConfigureAwait(false);
        var success = result.Failed == 0 && !result.Cancelled;
        return new CompatibleOperationResult(
            success,
            result.Applied,
            result.Skipped,
            result.Failed,
            result.Cancelled,
            success ? null : "One or more compatibility folder operations failed.")
        {
            Successful = result.Outcomes
                .Where(outcome => outcome.Kind == CompatibleApplyOutcomeKind.Applied)
                .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
                .ToArray(),
            SkippedItems = result.Outcomes
                .Where(outcome => outcome.Kind == CompatibleApplyOutcomeKind.Skipped)
                .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
                .ToArray(),
            Failures = result.Outcomes
                .Where(outcome => outcome.Kind == CompatibleApplyOutcomeKind.Failed)
                .Select(outcome => new ApplyFailure(
                    ApplyPhase.SystemMutation,
                    outcome.Detail ?? "Compatibility folder application failed.",
                    outcome.Path))
                .ToArray(),
            Unresolved = result.Unresolved
                .Select(outcome => new ApplyFailure(
                    ApplyPhase.Rollback,
                    outcome.Detail ?? "Compatibility folder mutation remains unresolved.",
                    outcome.Path))
                .ToArray()
        };
    }

    public async Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token)
    {
        var result = await service.RestoreAsync(snapshot.Id, token).ConfigureAwait(false);
        var successful = result.Outcomes
            .Where(outcome => outcome.Kind == CompatibleRestoreOutcomeKind.Restored)
            .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
            .ToArray();
        var skipped = result.Outcomes
            .Where(outcome => outcome.Kind == CompatibleRestoreOutcomeKind.Missing)
            .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
            .ToArray();
        var changed = result.Outcomes
            .Where(outcome => outcome.Kind == CompatibleRestoreOutcomeKind.ChangedSinceApply)
            .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
            .ToArray();
        var failures = result.Outcomes
            .Where(outcome => outcome.Kind == CompatibleRestoreOutcomeKind.Failed)
            .Select(outcome => new ApplyFailure(
                ApplyPhase.Restore,
                outcome.Detail ?? "Compatibility folder restore failed.",
                outcome.Path))
            .Concat(changed.Select(outcome => new ApplyFailure(
                ApplyPhase.Restore,
                outcome.Detail ?? "Desktop.ini changed since apply.",
                outcome.Path)))
            .ToList();
        if (result.Cancelled)
        {
            failures.Add(new ApplyFailure(ApplyPhase.Restore, "Compatibility restore was cancelled."));
        }

        return new RestoreOperationResult(result.IsFullyRestored, successful, skipped, changed, failures);
    }
}

internal sealed class DefaultApplicationShellRefreshService(ShellRefreshService service) : IApplicationShellRefreshService
{
    public OperationResult Refresh()
    {
        var result = service.RefreshIcons();
        return result.Success
            ? OperationResult.Success()
            : OperationResult.Failure(result.Error ?? "Shell refresh failed.");
    }
}

internal sealed class DefaultIconArtifactService : IIconArtifactService
{
    private readonly string iconsRoot;

    internal DefaultIconArtifactService()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio",
            "icons"))
    {
    }

    internal DefaultIconArtifactService(string iconsRoot)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(iconsRoot);
        this.iconsRoot = Path.GetFullPath(iconsRoot);
    }

    public async Task<IconArtifactResult> CreateAndVerifyAsync(
        IconSource source,
        Guid operationId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(source);
        var frames = new Dictionary<int, byte[]>();
        foreach (var size in FolderIconRenderer.RequiredSizes)
        {
            token.ThrowIfCancellationRequested();
            var bitmap = source.Kind switch
            {
                IconSourceKind.BuiltIn => FolderIconRenderer.Render(source.Theme!, size),
                IconSourceKind.ImportedImage => ImportedIconRenderer.Render(source.ImportedImagePath!, size),
                _ => throw new ArgumentOutOfRangeException(nameof(source))
            };
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using var stream = new MemoryStream();
            encoder.Save(stream);
            frames.Add(size, stream.ToArray());
        }

        var ico = IcoEncoder.Encode(frames);
        VerifyIco(ico);

        Directory.CreateDirectory(iconsRoot);
        var destination = Path.Combine(iconsRoot, $"{operationId:D}.ico");
        var temporary = destination + ".tmp";
        try
        {
            await File.WriteAllBytesAsync(temporary, ico, token).ConfigureAwait(false);
            File.Move(temporary, destination, overwrite: true);
            var persisted = await File.ReadAllBytesAsync(destination, token).ConfigureAwait(false);
            if (!persisted.AsSpan().SequenceEqual(ico))
            {
                throw new IOException("The persisted ICO did not match the verified icon payload.");
            }
        }
        finally
        {
            if (File.Exists(temporary)) File.Delete(temporary);
        }

        return IconArtifactResult.Success(destination);
    }

    private static void VerifyIco(byte[] ico)
    {
        using var stream = new MemoryStream(ico, writable: false);
        using var reader = new BinaryReader(stream);
        if (reader.ReadUInt16() != 0
            || reader.ReadUInt16() != 1
            || reader.ReadUInt16() != FolderIconRenderer.RequiredSizes.Length)
        {
            throw new InvalidDataException("The generated ICO header is invalid.");
        }

        foreach (var size in FolderIconRenderer.RequiredSizes)
        {
            var width = reader.ReadByte();
            var height = reader.ReadByte();
            reader.ReadBytes(14);
            var expected = size == 256 ? 0 : size;
            if (width != expected || height != expected)
            {
                throw new InvalidDataException("The generated ICO frame table is invalid.");
            }
        }
    }
}
