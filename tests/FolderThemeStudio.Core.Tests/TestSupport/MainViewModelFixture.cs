using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Themes;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.App.Monitoring;

namespace FolderThemeStudio.Core.Tests.TestSupport;

internal sealed class MainViewModelFixture
{
    internal MainViewModelFixture(bool useControlledDelay = false)
    {
        Delay = new DeterministicPreviewDelay(useControlledDelay);
        Renderer.IsPreviewWorkerActive = () => PreviewWorker.IsRunning;
    }

    internal RecordingApplicationCoordinator Coordinator { get; } = new();
    internal RecordingMainViewModelDialogs Dialogs { get; } = new();
    internal RecordingThemeCatalog Themes { get; } = new();
    internal RecordingPreviewRenderer Renderer { get; } = new();
    internal RecordingImportedImageService Images { get; } = new();
    internal RecordingFolderMonitoringCoordinator Monitoring { get; } = new();
    internal RecordingMonitoringIconAssetService MonitoringAssets { get; } = new();
    internal DeterministicPreviewDelay Delay { get; }
    internal RecordingPreviewWorkScheduler PreviewWorker { get; } = new();

    internal MainViewModel CreateViewModel() => new(
        Coordinator,
        Dialogs,
        Themes,
        Renderer,
        Delay,
        new ImmediateViewModelDispatcher(),
        PreviewWorker,
        Images,
        Monitoring,
        MonitoringAssets);
}

internal sealed class RecordingApplicationCoordinator : IThemeApplicationCoordinator
{
    private TaskCompletionSource? planningRelease;
    private TaskCompletionSource? applyRelease;
    private TaskCompletionSource? restoreRelease;

    internal ApplyPlan Plan { get; set; } = new(FolderTheme.IceBlue, ApplicationMode.Global, [], []);
    internal ApplyResult ApplyResult { get; set; } = new(true, Guid.NewGuid(), [], [], [], false, false);
    internal RestoreResult RestoreResult { get; set; } = new(true, false, Guid.NewGuid(), []);
    internal ApplyRequest? LastRequest { get; private set; }
    internal int ApplyCallCount { get; private set; }
    internal bool PausePlanning { get; set; }
    internal bool PauseApply { get; set; }
    internal bool PauseRestore { get; set; }
    internal TaskCompletionSource PlanningStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    internal TaskCompletionSource ApplyStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    internal TaskCompletionSource RestoreStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public async Task<ApplyPlan> PlanAsync(ApplyRequest request, CancellationToken token)
    {
        LastRequest = request;
        PlanningStarted.TrySetResult();
        if (PausePlanning)
        {
            planningRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            await planningRelease.Task.WaitAsync(token);
        }

        return Plan with { IconSource = request.IconSource, Mode = request.Mode };
    }

    public async Task<ApplyResult> ApplyAsync(ApplyPlan plan, IProgress<ApplyProgress> progress, CancellationToken token)
    {
        ApplyCallCount++;
        progress.Report(new ApplyProgress(ApplyPhase.Rendering, "Rendering"));
        ApplyStarted.TrySetResult();
        if (PauseApply)
        {
            applyRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            await applyRelease.Task.WaitAsync(token);
        }
        return ApplyResult;
    }

    public async Task<RestoreResult> RestoreLatestAsync(IProgress<ApplyProgress> progress, CancellationToken token)
    {
        progress.Report(new ApplyProgress(ApplyPhase.Restore, "Restoring"));
        RestoreStarted.TrySetResult();
        if (PauseRestore)
        {
            restoreRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            await restoreRelease.Task.WaitAsync(token);
        }
        return RestoreResult;
    }

    internal void ReleasePlanning() => planningRelease!.TrySetResult();
    internal void ReleaseApply() => applyRelease!.TrySetResult();
    internal void ReleaseRestore() => restoreRelease!.TrySetResult();
}

internal sealed class RecordingMainViewModelDialogs : IMainViewModelDialogs
{
    internal bool ConfirmResult { get; set; } = true;
    internal string LastConfirmationMessage { get; private set; } = string.Empty;
    internal string CopiedText { get; private set; } = string.Empty;
    internal string ExportedText { get; private set; } = string.Empty;
    internal List<(string Title, string Message)> Errors { get; } = [];
    internal bool ThrowOnCopy { get; set; }
    internal bool ThrowOnExport { get; set; }
    internal string? ThemeImportPath { get; set; }
    internal string? ThemeExportPath { get; set; }
    internal string? ImageImportPath { get; set; }

    public Task<bool> ConfirmAsync(string title, string message)
    {
        LastConfirmationMessage = message;
        return Task.FromResult(ConfirmResult);
    }

    public Task<string?> PickFolderAsync() => Task.FromResult<string?>(null);

    public Task<string?> PickThemeImportPathAsync() => Task.FromResult(ThemeImportPath);

    public Task<string?> PickImageImportPathAsync() => Task.FromResult(ImageImportPath);

    public Task<string?> PickThemeExportPathAsync(string suggestedFileName) =>
        Task.FromResult(ThemeExportPath);

    public void CopyText(string text)
    {
        if (ThrowOnCopy)
        {
            throw new IOException("clipboard busy");
        }
        CopiedText = text;
    }

    public Task ExportTextAsync(string suggestedFileName, string text)
    {
        if (ThrowOnExport)
        {
            throw new IOException("destination unavailable");
        }
        ExportedText = text;
        return Task.CompletedTask;
    }

    public void ShowError(string title, string message)
    {
        Errors.Add((title, message));
    }
}

internal sealed class RecordingThemeCatalog : IThemeCatalog
{
    private TaskCompletionSource? saveRelease;

    internal List<FolderTheme> Items { get; } = [FolderTheme.IceBlue];
    internal ThemeImportResult ImportResult { get; set; } = new(null, ["Import result not configured."]);
    internal string? ImportSourcePath { get; private set; }
    internal Guid? ExportedThemeId { get; private set; }
    internal string? ExportDestinationPath { get; private set; }
    internal Guid? DeletedThemeId { get; private set; }
    internal bool PauseSave { get; set; }
    internal TaskCompletionSource SaveStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public Task<ThemeLoadResult> LoadAsync() =>
        Task.FromResult(new ThemeLoadResult(Items, []));

    public async Task SaveAsync(FolderTheme theme)
    {
        SaveStarted.TrySetResult();
        if (PauseSave)
        {
            saveRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            await saveRelease.Task;
        }

        var existing = Items.FindIndex(item => item.Id == theme.Id);
        if (existing >= 0)
        {
            Items[existing] = theme;
        }
        else
        {
            Items.Add(theme);
        }
    }

    public Task<ThemeImportResult> ImportAsync(string sourcePath)
    {
        ImportSourcePath = sourcePath;
        if (ImportResult.Success)
        {
            Items.Add(ImportResult.Theme!);
        }
        return Task.FromResult(ImportResult);
    }

    public Task ExportAsync(Guid themeId, string destinationPath)
    {
        ExportedThemeId = themeId;
        ExportDestinationPath = destinationPath;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid themeId)
    {
        DeletedThemeId = themeId;
        Items.RemoveAll(item => item.Id == themeId);
        return Task.CompletedTask;
    }

    internal void ReleaseSave() => saveRelease!.TrySetResult();
}

internal sealed class RecordingPreviewRenderer : IPreviewRenderer
{
    internal BitmapSource Pixel { get; } = CreatePixel();

    internal List<PreviewRenderCall> Calls { get; } = [];
    internal Func<bool> IsPreviewWorkerActive { get; set; } = () => false;
    internal bool AllCallsRanOnPreviewWorker { get; private set; } = true;

    public BitmapSource Render(IconSource source, int size)
    {
        AllCallsRanOnPreviewWorker &= IsPreviewWorkerActive();
        Calls.Add(new PreviewRenderCall(source, size));
        return Pixel;
    }

    internal void ClearCalls() => Calls.Clear();

    private static BitmapSource CreatePixel()
    {
        var pixel = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgra32, null, new byte[4], 4);
        pixel.Freeze();
        return pixel;
    }
}

internal sealed record PreviewRenderCall(IconSource Source, int Size)
{
    internal FolderTheme Theme => Source.Theme ?? FolderTheme.IceBlue;
}

internal sealed class RecordingImportedImageService : IImportedImageService
{
    internal ImportedImageResult NextResult { get; set; } = ImportedImageResult.Failure("Not configured");
    internal string? LastSourcePath { get; private set; }

    public Task<ImportedImageResult> ImportAsync(string sourcePath, CancellationToken token)
    {
        LastSourcePath = sourcePath;
        return Task.FromResult(NextResult);
    }
}

internal sealed class RecordingFolderMonitoringCoordinator : IFolderMonitoringCoordinator
{
    internal List<MonitoringRule> Rules { get; } = [];
    internal List<string> Removed { get; } = [];
    internal bool IsPaused { get; private set; }
    public MonitoringStatus Status => new(IsPaused, Rules.Count, IsPaused ? 0 : Rules.Count, 0);
    public event EventHandler<MonitoringOutcome>? OutcomeProduced;
    public Task StartAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task PauseAsync(CancellationToken token = default) { IsPaused = true; return Task.CompletedTask; }
    public Task ResumeAsync(CancellationToken token = default) { IsPaused = false; return Task.CompletedTask; }
    public Task ReplaceRuleAsync(MonitoringRule rule, CancellationToken token = default)
    {
        Rules.RemoveAll(item => string.Equals(item.RootPath, rule.RootPath, StringComparison.OrdinalIgnoreCase));
        Rules.Add(rule);
        return Task.CompletedTask;
    }
    public Task RemoveRuleAsync(string rootPath, CancellationToken token = default) { Removed.Add(rootPath); return Task.CompletedTask; }
    internal void Emit(MonitoringOutcome outcome) => OutcomeProduced?.Invoke(this, outcome);
    public void Dispose() { }
}

internal sealed class RecordingMonitoringIconAssetService : IMonitoringIconAssetService
{
    internal string DurablePath { get; set; } = @"C:\AppData\monitoring\style.ico";
    internal string? SourcePath { get; private set; }
    public Task<string> PersistAsync(string generatedIcoPath, CancellationToken token)
    {
        SourcePath = generatedIcoPath;
        return Task.FromResult(DurablePath);
    }
}

internal sealed class RecordingPreviewWorkScheduler : IPreviewWorkScheduler
{
    internal int RunCount { get; private set; }
    internal bool IsRunning { get; private set; }

    public Task<T> RunAsync<T>(Func<T> work, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        RunCount++;
        IsRunning = true;
        try
        {
            return Task.FromResult(work());
        }
        finally
        {
            IsRunning = false;
        }
    }

    public void Dispose() { }
}

internal sealed class DeterministicPreviewDelay(bool controlled) : IPreviewDelay
{
    private readonly List<TaskCompletionSource> pending = [];

    public Task DelayAsync(TimeSpan delay, CancellationToken token)
    {
        if (!controlled)
        {
            return Task.CompletedTask;
        }

        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        pending.Add(completion);
        token.Register(() => completion.TrySetCanceled(token));
        return completion.Task;
    }

    internal void CompleteLatest() => pending[^1].TrySetResult();
}

internal sealed class ImmediateViewModelDispatcher : IViewModelDispatcher
{
    public Task InvokeAsync(Action action)
    {
        action();
        return Task.CompletedTask;
    }
}
