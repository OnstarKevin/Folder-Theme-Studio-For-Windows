using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Application;

public enum ApplicationMode
{
    Global,
    Compatible
}

public sealed record ApplyRequest(
    IconSource IconSource,
    ApplicationMode Mode,
    IReadOnlyList<string>? ExplicitRoots = null)
{
    public ApplyRequest(FolderTheme theme, ApplicationMode mode, IReadOnlyList<string>? explicitRoots = null)
        : this(IconSource.BuiltIn(theme), mode, explicitRoots)
    {
    }

    public FolderTheme Theme => IconSource.Theme ?? FolderTheme.IceBlue;
}

public sealed record ApplyPlan(
    IconSource IconSource,
    ApplicationMode Mode,
    IReadOnlyList<FolderPlanItem> Folders,
    IReadOnlyList<string> ValidationErrors)
{
    public ApplyPlan(
        FolderTheme theme,
        ApplicationMode mode,
        IReadOnlyList<FolderPlanItem> folders,
        IReadOnlyList<string> validationErrors)
        : this(IconSource.BuiltIn(theme), mode, folders, validationErrors)
    {
    }

    public FolderTheme Theme => IconSource.Theme ?? FolderTheme.IceBlue;
    public bool CanApply => ValidationErrors.Count == 0;
    public int AllowedCount => Folders.Count(item => item.Decision == FolderDecision.Allowed);
    public int SkippedCount => Folders.Count - AllowedCount;
    public IReadOnlyDictionary<FolderDecision, int> SkippedReasons => Folders
        .Where(item => item.Decision != FolderDecision.Allowed)
        .GroupBy(item => item.Decision)
        .ToDictionary(group => group.Key, group => group.Count());
}

public enum ApplyPhase
{
    Validation,
    Rendering,
    Snapshot,
    SystemMutation,
    Refresh,
    Rollback,
    Restore
}

public sealed record ApplyProgress(ApplyPhase Phase, string Message, int Completed = 0, int Total = 0);

public sealed record ApplyRecord(string? Path, string? Detail);

public sealed record ApplyFailure(ApplyPhase Phase, string Error, string? Path = null);

public sealed record ApplyResult(
    bool Success,
    Guid? OperationId,
    IReadOnlyList<ApplyRecord> Successful,
    IReadOnlyList<ApplyRecord> Skipped,
    IReadOnlyList<ApplyFailure> Failures,
    bool RollbackAttempted,
    bool RollbackSucceeded)
{
    public string? IconArtifactPath { get; init; }
}

public sealed record RestoreResult(
    bool Success,
    bool AlreadyRestored,
    Guid? OperationId,
    IReadOnlyList<ApplyFailure> Failures)
{
    public IReadOnlyList<ApplyRecord> Successful { get; init; } = [];
    public IReadOnlyList<ApplyRecord> Skipped { get; init; } = [];
    public IReadOnlyList<ApplyRecord> Changed { get; init; } = [];
}
