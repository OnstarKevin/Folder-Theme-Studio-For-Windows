using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Themes;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.Monitoring;

namespace FolderThemeStudio.App.ViewModels;

public interface IThemeApplicationCoordinator
{
    Task<ApplyPlan> PlanAsync(ApplyRequest request, CancellationToken token);
    Task<ApplyResult> ApplyAsync(ApplyPlan plan, IProgress<ApplyProgress> progress, CancellationToken token);
    Task<RestoreResult> RestoreLatestAsync(IProgress<ApplyProgress> progress, CancellationToken token);
}

public interface IMainViewModelDialogs
{
    Task<bool> ConfirmAsync(string title, string message);
    Task<string?> PickFolderAsync();
    Task<string?> PickThemeImportPathAsync();
    Task<string?> PickImageImportPathAsync();
    Task<string?> PickThemeExportPathAsync(string suggestedFileName);
    void CopyText(string text);
    Task ExportTextAsync(string suggestedFileName, string text);
    void ShowError(string title, string message);
}

public interface IThemeCatalog
{
    Task<ThemeLoadResult> LoadAsync();
    Task SaveAsync(FolderTheme theme);
    Task<ThemeImportResult> ImportAsync(string sourcePath);
    Task ExportAsync(Guid themeId, string destinationPath);
    Task DeleteAsync(Guid themeId);
}

public interface IPreviewRenderer
{
    BitmapSource Render(IconSource source, int size);
}

public interface IPreviewDelay
{
    Task DelayAsync(TimeSpan delay, CancellationToken token);
}

public interface IPreviewWorkScheduler : IDisposable
{
    Task<T> RunAsync<T>(Func<T> work, CancellationToken token);
}

public interface IViewModelDispatcher
{
    Task InvokeAsync(Action action);
}

public enum ResultKind
{
    Success,
    RolledBack,
    StateUncertain,
    Skipped,
    Failed
}

public enum IconEditorMode
{
    BuiltIn,
    ImportedImage
}

public sealed record OperationResultItem(ResultKind Kind, string? Path, string Detail)
{
    public string StatusLabel => Kind switch
    {
        ResultKind.Success => "Succeeded",
        ResultKind.RolledBack => "Rolled back",
        ResultKind.StateUncertain => "State uncertain",
        ResultKind.Skipped => "Skipped",
        _ => "Failed"
    };
}

public sealed class PreviewImageViewModel(int size) : INotifyPropertyChanged
{
    private BitmapSource? image;

    public int Size { get; } = size;

    public BitmapSource? Image
    {
        get => image;
        internal set
        {
            if (ReferenceEquals(image, value))
            {
                return;
            }

            image = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}

public sealed class MainViewModel : INotifyPropertyChanged, IDisposable
{
    private static readonly TimeSpan PreviewDebounce = TimeSpan.FromMilliseconds(100);
    private static readonly int[] PreviewSizes = [16, 32, 64, 256];

    private readonly IThemeApplicationCoordinator coordinator;
    private readonly IMainViewModelDialogs dialogs;
    private readonly IThemeCatalog themes;
    private readonly IPreviewRenderer previewRenderer;
    private readonly IPreviewDelay previewDelay;
    private readonly IViewModelDispatcher dispatcher;
    private readonly IPreviewWorkScheduler previewWorkScheduler;
    private readonly IImportedImageService importedImageService;
    private readonly IFolderMonitoringCoordinator folderMonitoring;
    private readonly IMonitoringIconAssetService monitoringAssets;
    private readonly RelayCommand applyCommand;
    private readonly RelayCommand restoreCommand;
    private readonly RelayCommand planCommand;
    private readonly RelayCommand addRootCommand;
    private readonly RelayCommand removeRootCommand;
    private readonly RelayCommand saveThemeCommand;
    private readonly RelayCommand importThemeCommand;
    private readonly RelayCommand exportThemeCommand;
    private readonly RelayCommand deleteThemeCommand;
    private readonly RelayCommand copyFailedPathsCommand;
    private readonly RelayCommand exportFailedPathsCommand;
    private readonly RelayCommand chooseImportedImageCommand;
    private readonly RelayCommand clearImportedImageCommand;
    private readonly RelayCommand toggleMonitoringCommand;
    private readonly RelayCommand removeMonitoringRuleCommand;
    private CancellationTokenSource? previewCancellation;
    private CancellationTokenSource? operationCancellation;
    private Task previewCompletion = Task.CompletedTask;
    private Guid themeId;
    private string themeName;
    private string gradientStart;
    private string gradientEnd;
    private double gradientAngle;
    private double opacity;
    private double cornerRadius;
    private string strokeColor;
    private double strokeOpacity;
    private double strokeWidth;
    private double highlightStrength;
    private string glowColor;
    private double glowStrength;
    private double glowRadius;
    private double shadowStrength;
    private double shadowOffset;
    private FolderTheme? selectedSavedTheme;
    private ApplicationMode selectedMode;
    private bool isThemeValid;
    private string validationMessage = string.Empty;
    private bool isPreviewRendering;
    private bool isPlanning;
    private bool isApplying;
    private bool isRestoring;
    private bool isManagingThemes;
    private int planAllowedCount;
    private int planSkippedCount;
    private string planDetails = "Plan not calculated yet.";
    private string progressMessage = "Ready.";
    private int progressCompleted;
    private int progressTotal;
    private string operationStatus = "No changes have been applied.";
    private string resultSummary = "No results yet.";
    private IReadOnlyList<string> serviceValidationErrors = [];
    private long inputRevision;
    private ApplyPlan? reviewedPlan;
    private ApplyRequest? reviewedRequest;
    private long reviewedPlanRevision = -1;
    private IconEditorMode selectedIconEditorMode;
    private string? importedImageAssetPath;
    private BitmapSource? importedImagePreview;
    private bool syncingPalette;
    private bool continueApplyingToNewFolders;

    public MainViewModel(
        IThemeApplicationCoordinator coordinator,
        IMainViewModelDialogs dialogs,
        IThemeCatalog themes,
        IPreviewRenderer previewRenderer,
        IPreviewDelay previewDelay,
        IViewModelDispatcher dispatcher,
        IPreviewWorkScheduler? previewWorkScheduler = null,
        IImportedImageService? importedImageService = null,
        IFolderMonitoringCoordinator? folderMonitoring = null,
        IMonitoringIconAssetService? monitoringAssets = null)
    {
        this.coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
        this.dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));
        this.themes = themes ?? throw new ArgumentNullException(nameof(themes));
        this.previewRenderer = previewRenderer ?? throw new ArgumentNullException(nameof(previewRenderer));
        this.previewDelay = previewDelay ?? throw new ArgumentNullException(nameof(previewDelay));
        this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        this.previewWorkScheduler = previewWorkScheduler ?? new StaPreviewWorkScheduler();
        this.importedImageService = importedImageService ?? new ImportedImageService();
        this.folderMonitoring = folderMonitoring ?? new NullFolderMonitoringCoordinator();
        this.monitoringAssets = monitoringAssets ?? new PassthroughMonitoringIconAssetService();
        this.folderMonitoring.OutcomeProduced += MonitoringOutcomeProduced;

        var initial = FolderTheme.IceBlue;
        themeId = initial.Id;
        themeName = initial.Name;
        gradientStart = initial.GradientStart;
        gradientEnd = initial.GradientEnd;
        gradientAngle = initial.GradientAngle;
        opacity = initial.Opacity;
        cornerRadius = initial.CornerRadius;
        strokeColor = initial.StrokeColor;
        strokeOpacity = initial.StrokeOpacity;
        strokeWidth = initial.StrokeWidth;
        highlightStrength = initial.HighlightStrength;
        glowColor = initial.GlowColor;
        glowStrength = initial.GlowStrength;
        glowRadius = initial.GlowRadius;
        shadowStrength = initial.ShadowStrength;
        shadowOffset = initial.ShadowOffset;
        selectedSavedTheme = initial;
        PaletteEditor = new PaletteEditorViewModel(new FolderPalette(
            initial.GradientStart, initial.GradientEnd, initial.StrokeColor, initial.GlowColor));
        PaletteEditor.PaletteChanged += PaletteEditorChanged;

        Previews = new ObservableCollection<PreviewImageViewModel>(
            PreviewSizes.Select(size => new PreviewImageViewModel(size)));
        SavedThemes = [initial];
        SelectedRoots.CollectionChanged += SelectedRootsChanged;

        applyCommand = new RelayCommand(_ => _ = ApplyAsync(), _ => CanApply);
        restoreCommand = new RelayCommand(_ => _ = RestoreAsync(), _ => !IsBusy);
        planCommand = new RelayCommand(_ => _ = PlanAsync(), _ => CanPlan);
        addRootCommand = new RelayCommand(_ => _ = AddRootAsync(), _ => !IsBusy);
        removeRootCommand = new RelayCommand(RemoveRoot, parameter => !IsBusy && parameter is string);
        saveThemeCommand = new RelayCommand(_ => _ = SaveThemeAsync(), _ => IsThemeValid && !IsBusy);
        importThemeCommand = new RelayCommand(_ => _ = ImportThemeAsync(), _ => !IsBusy);
        exportThemeCommand = new RelayCommand(_ => _ = ExportThemeAsync(), _ => SelectedSavedTheme is not null && !IsBusy);
        deleteThemeCommand = new RelayCommand(_ => _ = DeleteThemeAsync(), _ => SelectedSavedTheme is not null && !IsBusy);
        copyFailedPathsCommand = new RelayCommand(_ => CopyFailedPaths(), _ => FailedResults.Count > 0);
        exportFailedPathsCommand = new RelayCommand(_ => _ = ExportFailedPathsAsync(), _ => FailedResults.Count > 0);
        chooseImportedImageCommand = new RelayCommand(_ => _ = ChooseImportedImageAsync(), _ => !IsBusy);
        clearImportedImageCommand = new RelayCommand(_ => ClearImportedImage(), _ => !IsBusy && importedImageAssetPath is not null);
        toggleMonitoringCommand = new RelayCommand(_ => _ = ToggleMonitoringAsync(), _ => !IsBusy);
        removeMonitoringRuleCommand = new RelayCommand(parameter => _ = RemoveMonitoringRuleAsync(parameter as string), parameter => !IsBusy && parameter is string);

        RefreshValidation();
        QueuePreviewRefresh();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<PreviewImageViewModel> Previews { get; }
    public ObservableCollection<FolderTheme> SavedThemes { get; }
    public ObservableCollection<string> SelectedRoots { get; } = [];
    public ObservableCollection<OperationResultItem> Results { get; } = [];
    public PaletteEditorViewModel PaletteEditor { get; }

    public IconEditorMode SelectedIconEditorMode
    {
        get => selectedIconEditorMode;
        set
        {
            if (!SetField(ref selectedIconEditorMode, value)) return;
            OnPropertyChanged(nameof(IsBuiltInEditor));
            OnPropertyChanged(nameof(IsImportedImageEditor));
            IconSourceChanged();
        }
    }

    public bool IsBuiltInEditor
    {
        get => SelectedIconEditorMode == IconEditorMode.BuiltIn;
        set { if (value) SelectedIconEditorMode = IconEditorMode.BuiltIn; }
    }

    public bool IsImportedImageEditor
    {
        get => SelectedIconEditorMode == IconEditorMode.ImportedImage;
        set { if (value) SelectedIconEditorMode = IconEditorMode.ImportedImage; }
    }

    public string? ImportedImageAssetPath
    {
        get => importedImageAssetPath;
        private set => SetField(ref importedImageAssetPath, value);
    }

    public BitmapSource? ImportedImagePreview
    {
        get => importedImagePreview;
        private set => SetField(ref importedImagePreview, value);
    }

    public IReadOnlyList<OperationResultItem> FailedResults =>
        Results.Where(result => result.Kind is ResultKind.Failed or ResultKind.StateUncertain).ToArray();

    public IReadOnlyList<ApplicationMode> AvailableModes { get; } =
        [ApplicationMode.Global, ApplicationMode.Compatible];

    public bool IsCompatibleMode => SelectedMode == ApplicationMode.Compatible;
    public bool ContinueApplyingToNewFolders
    {
        get => continueApplyingToNewFolders;
        set => SetField(ref continueApplyingToNewFolders, value);
    }
    public string MonitoringStatusText => folderMonitoring.Status.Paused
        ? $"监控已暂停（{folderMonitoring.Status.TotalRules} 条规则）"
        : $"正在监控 {folderMonitoring.Status.ActiveRules} 个目录；{folderMonitoring.Status.InactiveRules} 个目录暂不可用";
    public int ActiveMonitoringRuleCount => folderMonitoring.Status.ActiveRules;

    public void ApplyPalette(FolderPalette palette)
    {
        ArgumentNullException.ThrowIfNull(palette);
        syncingPalette = true;
        try
        {
            PaletteEditor.Apply(palette);
            GradientStart = palette.GradientStart;
            GradientEnd = palette.GradientEnd;
            StrokeColor = palette.Stroke;
            GlowColor = palette.Glow;
        }
        finally { syncingPalette = false; }
    }

    public FolderTheme CurrentTheme => new(
        FolderTheme.CurrentSchemaVersion,
        themeId,
        ThemeName,
        GradientStart,
        GradientEnd,
        GradientAngle,
        Opacity,
        CornerRadius,
        StrokeColor,
        StrokeOpacity,
        StrokeWidth,
        HighlightStrength,
        GlowColor,
        GlowStrength,
        GlowRadius,
        ShadowStrength,
        ShadowOffset);

    public FolderTheme? SelectedSavedTheme
    {
        get => selectedSavedTheme;
        set
        {
            if (!SetField(ref selectedSavedTheme, value) || value is null)
            {
                return;
            }

            LoadTheme(value);
        }
    }

    public string ThemeName
    {
        get => themeName;
        set => SetThemeField(ref themeName, value ?? string.Empty);
    }

    public string GradientStart
    {
        get => gradientStart;
        set => SetThemeField(ref gradientStart, value ?? string.Empty);
    }

    public string GradientEnd
    {
        get => gradientEnd;
        set => SetThemeField(ref gradientEnd, value ?? string.Empty);
    }

    public double GradientAngle
    {
        get => gradientAngle;
        set => SetThemeField(ref gradientAngle, value);
    }

    public double Opacity
    {
        get => opacity;
        set => SetThemeField(ref opacity, value);
    }

    public double CornerRadius
    {
        get => cornerRadius;
        set => SetThemeField(ref cornerRadius, value);
    }

    public string StrokeColor
    {
        get => strokeColor;
        set => SetThemeField(ref strokeColor, value ?? string.Empty);
    }

    public double StrokeOpacity
    {
        get => strokeOpacity;
        set => SetThemeField(ref strokeOpacity, value);
    }

    public double StrokeWidth
    {
        get => strokeWidth;
        set => SetThemeField(ref strokeWidth, value);
    }

    public double HighlightStrength
    {
        get => highlightStrength;
        set => SetThemeField(ref highlightStrength, value);
    }

    public string GlowColor
    {
        get => glowColor;
        set => SetThemeField(ref glowColor, value ?? string.Empty);
    }

    public double GlowStrength
    {
        get => glowStrength;
        set => SetThemeField(ref glowStrength, value);
    }

    public double GlowRadius
    {
        get => glowRadius;
        set => SetThemeField(ref glowRadius, value);
    }

    public double ShadowStrength
    {
        get => shadowStrength;
        set => SetThemeField(ref shadowStrength, value);
    }

    public double ShadowOffset
    {
        get => shadowOffset;
        set => SetThemeField(ref shadowOffset, value);
    }

    public ApplicationMode SelectedMode
    {
        get => selectedMode;
        set
        {
            if (!SetField(ref selectedMode, value))
            {
                return;
            }

            inputRevision++;
            OnPropertyChanged(nameof(IsCompatibleMode));
            InvalidatePlan();
            RefreshValidation();
        }
    }

    public bool IsThemeValid
    {
        get => isThemeValid;
        private set => SetField(ref isThemeValid, value);
    }

    public string ValidationMessage
    {
        get => validationMessage;
        private set => SetField(ref validationMessage, value);
    }

    public bool IsPreviewRendering
    {
        get => isPreviewRendering;
        private set
        {
            if (SetField(ref isPreviewRendering, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public bool IsPlanning
    {
        get => isPlanning;
        private set
        {
            if (SetField(ref isPlanning, value))
            {
                RaiseBusyState();
            }
        }
    }

    public bool IsApplying
    {
        get => isApplying;
        private set
        {
            if (SetField(ref isApplying, value))
            {
                RaiseBusyState();
            }
        }
    }

    public bool IsRestoring
    {
        get => isRestoring;
        private set
        {
            if (SetField(ref isRestoring, value))
            {
                RaiseBusyState();
            }
        }
    }

    public bool IsManagingThemes
    {
        get => isManagingThemes;
        private set
        {
            if (SetField(ref isManagingThemes, value))
            {
                RaiseBusyState();
            }
        }
    }

    public bool IsBusy => IsPlanning || IsApplying || IsRestoring || IsManagingThemes;
    public bool AreInputsEnabled => !IsBusy;
    public bool CanApply =>
        HasValidIconSource &&
        HasRequiredRoots &&
        !IsBusy &&
        !IsPreviewRendering &&
        reviewedPlan is { CanApply: true } &&
        reviewedRequest is not null &&
        reviewedPlanRevision == inputRevision;
    public bool CanPlan => HasValidIconSource && HasRequiredRoots && !IsBusy && !IsPreviewRendering;
    public bool HasProgressRange => ProgressTotal > 0;

    public int PlanAllowedCount
    {
        get => planAllowedCount;
        private set => SetField(ref planAllowedCount, value);
    }

    public int PlanSkippedCount
    {
        get => planSkippedCount;
        private set => SetField(ref planSkippedCount, value);
    }

    public string PlanDetails
    {
        get => planDetails;
        private set => SetField(ref planDetails, value);
    }

    public string ProgressMessage
    {
        get => progressMessage;
        private set => SetField(ref progressMessage, value);
    }

    public int ProgressCompleted
    {
        get => progressCompleted;
        private set => SetField(ref progressCompleted, value);
    }

    public int ProgressTotal
    {
        get => progressTotal;
        private set
        {
            if (SetField(ref progressTotal, value))
            {
                OnPropertyChanged(nameof(HasProgressRange));
            }
        }
    }

    public string OperationStatus
    {
        get => operationStatus;
        private set => SetField(ref operationStatus, value);
    }

    public string ResultSummary
    {
        get => resultSummary;
        private set => SetField(ref resultSummary, value);
    }

    public ICommand ApplyCommand => applyCommand;
    public ICommand RestoreCommand => restoreCommand;
    public ICommand PlanCommand => planCommand;
    public ICommand AddRootCommand => addRootCommand;
    public ICommand RemoveRootCommand => removeRootCommand;
    public ICommand SaveThemeCommand => saveThemeCommand;
    public ICommand ImportThemeCommand => importThemeCommand;
    public ICommand ExportThemeCommand => exportThemeCommand;
    public ICommand DeleteThemeCommand => deleteThemeCommand;
    public ICommand CopyFailedPathsCommand => copyFailedPathsCommand;
    public ICommand ExportFailedPathsCommand => exportFailedPathsCommand;
    public ICommand ChooseImportedImageCommand => chooseImportedImageCommand;
    public ICommand ClearImportedImageCommand => clearImportedImageCommand;
    public ICommand ToggleMonitoringCommand => toggleMonitoringCommand;
    public ICommand RemoveMonitoringRuleCommand => removeMonitoringRuleCommand;

    public async Task ChooseImportedImageAsync()
    {
        if (IsBusy) return;
        var sourcePath = await dialogs.PickImageImportPathAsync();
        if (string.IsNullOrWhiteSpace(sourcePath)) return;
        var result = await importedImageService.ImportAsync(sourcePath, CancellationToken.None);
        if (!result.Success || result.AssetPath is null || result.Preview is null)
        {
            dialogs.ShowError("无法导入图片", result.Error ?? "请选择有效的 PNG、JPG 或 BMP 图片。");
            return;
        }

        ImportedImageAssetPath = result.AssetPath;
        ImportedImagePreview = result.Preview;
        selectedIconEditorMode = IconEditorMode.ImportedImage;
        OnPropertyChanged(nameof(SelectedIconEditorMode));
        OnPropertyChanged(nameof(IsBuiltInEditor));
        OnPropertyChanged(nameof(IsImportedImageEditor));
        IconSourceChanged();
    }

    private void ClearImportedImage()
    {
        ImportedImageAssetPath = null;
        ImportedImagePreview = null;
        IconSourceChanged();
    }

    public async Task InitializeAsync()
    {
        await folderMonitoring.StartAsync();
        RaiseMonitoringState();
        try
        {
            var loaded = await RefreshThemeCatalogAsync();
            var selected = loaded.Themes.FirstOrDefault(theme => theme.Id == themeId);
            if (selected is not null)
            {
                selectedSavedTheme = selected;
                OnPropertyChanged(nameof(SelectedSavedTheme));
                RaiseCommandStates();
            }

            if (loaded.Diagnostics.Count > 0)
            {
                OperationStatus = $"Loaded themes with {loaded.Diagnostics.Count} diagnostic message(s).";
            }
        }
        catch (Exception exception)
        {
            OperationStatus = "Saved themes could not be loaded; the built-in theme remains available.";
            dialogs.ShowError("Could not load themes", exception.Message);
        }

        await WaitForPreviewAsync();
    }

    public Task WaitForPreviewAsync() => previewCompletion;

    public async Task PlanAsync()
    {
        if (!CanPlan)
        {
            return;
        }

        var plannedRevision = inputRevision;
        var request = CreateApplyRequest();
        ClearReviewedPlan();
        IsPlanning = true;
        OperationStatus = "Calculating a write-free application plan…";
        try
        {
            var plan = await coordinator.PlanAsync(request, CancellationToken.None);
            if (plannedRevision != inputRevision)
            {
                OperationStatus = "Inputs changed while planning. Review the current settings and calculate the plan again.";
                return;
            }
            reviewedPlan = plan;
            reviewedRequest = request;
            reviewedPlanRevision = plannedRevision;
            UpdatePlan(plan);
            RaiseCommandStates();
            OperationStatus = plan.CanApply
                ? "Plan ready. No system changes were made."
                : "The plan found issues that must be fixed before applying.";
        }
        catch (Exception exception)
        {
            OperationStatus = "Planning failed. No system changes were made.";
            dialogs.ShowError("Planning failed", exception.Message);
        }
        finally
        {
            IsPlanning = false;
        }
    }

    public async Task ApplyAsync()
    {
        if (!CanApply)
        {
            return;
        }

        var plannedRevision = reviewedPlanRevision;
        var request = reviewedRequest!;
        var plan = reviewedPlan!;
        operationCancellation?.Dispose();
        operationCancellation = new CancellationTokenSource();
        var token = operationCancellation.Token;
        IsApplying = true;
        Results.Clear();
        RefreshResultCommands();
        ResultSummary = "No results yet.";

        try
        {
            if (plannedRevision != inputRevision)
            {
                ClearReviewedPlan();
                OperationStatus = "Inputs changed after planning. Review a fresh plan before applying.";
                return;
            }

            var confirmed = await dialogs.ConfirmAsync(
                plan.Mode == ApplicationMode.Global ? "Apply for current user?" : "Apply to selected folders?",
                BuildApplyConfirmation(plan, request.ExplicitRoots ?? []));
            if (!confirmed)
            {
                OperationStatus = "Apply cancelled before any system changes.";
                return;
            }

            if (plannedRevision != inputRevision)
            {
                ClearReviewedPlan();
                OperationStatus = "Inputs changed during confirmation. Review a fresh plan before applying.";
                return;
            }

            OperationStatus = "Applying the folder theme…";
            ClearReviewedPlan();
            var progress = new DispatcherProgress<ApplyProgress>(dispatcher, ReportProgress);
            var result = await coordinator.ApplyAsync(plan, progress, token);
            if (result.Success && plan.Mode == ApplicationMode.Compatible && ContinueApplyingToNewFolders &&
                !string.IsNullOrWhiteSpace(result.IconArtifactPath))
            {
                var durableIco = await monitoringAssets.PersistAsync(result.IconArtifactPath, token);
                foreach (var root in request.ExplicitRoots ?? [])
                    await folderMonitoring.ReplaceRuleAsync(new MonitoringRule(1, root, durableIco, DateTimeOffset.UtcNow), token);
                RaiseMonitoringState();
            }
            ShowApplyResult(result);
        }
        catch (OperationCanceledException)
        {
            OperationStatus = "Apply cancelled. Review the result list for any completed work.";
        }
        catch (Exception exception)
        {
            OperationStatus = "Apply failed before a complete result was available.";
            dialogs.ShowError("Apply failed", exception.Message);
        }
        finally
        {
            IsApplying = false;
        }
    }

    public async Task RestoreAsync()
    {
        if (IsBusy)
        {
            return;
        }

        var confirmed = await dialogs.ConfirmAsync(
            "Restore latest backup?",
            "Restore latest will use the most recent recovery snapshot. Successful reversions remain reported even if another path cannot be restored. Continue?");
        if (!confirmed)
        {
            OperationStatus = "Restore cancelled before any system changes.";
            return;
        }

        operationCancellation?.Dispose();
        operationCancellation = new CancellationTokenSource();
        IsRestoring = true;
        Results.Clear();
        RefreshResultCommands();
        try
        {
            var progress = new DispatcherProgress<ApplyProgress>(dispatcher, ReportProgress);
            var result = await coordinator.RestoreLatestAsync(progress, operationCancellation.Token);
            ShowRestoreResult(result);
        }
        catch (OperationCanceledException)
        {
            OperationStatus = "Restore cancelled. Successfully reverted items remain reverted.";
        }
        catch (Exception exception)
        {
            OperationStatus = "Restore partially completed or failed; review the error details.";
            dialogs.ShowError("Restore failed", exception.Message);
        }
        finally
        {
            IsRestoring = false;
        }
    }

    public async Task ExportFailedPathsAsync()
    {
        var text = BuildFailureText();
        if (text.Length > 0)
        {
            try
            {
                await dialogs.ExportTextAsync("folder-theme-failures.txt", text);
            }
            catch (Exception exception)
            {
                dialogs.ShowError("Could not export failure report", exception.Message);
            }
        }
    }

    public void Dispose()
    {
        folderMonitoring.OutcomeProduced -= MonitoringOutcomeProduced;
        folderMonitoring.Dispose();
        PaletteEditor.PaletteChanged -= PaletteEditorChanged;
        SelectedRoots.CollectionChanged -= SelectedRootsChanged;
        previewCancellation?.Cancel();
        previewCancellation?.Dispose();
        operationCancellation?.Cancel();
        operationCancellation?.Dispose();
        previewWorkScheduler.Dispose();
    }

    private bool HasRequiredRoots =>
        SelectedMode != ApplicationMode.Compatible || SelectedRoots.Count > 0;

    private async Task AddRootAsync()
    {
        var path = await dialogs.PickFolderAsync();
        if (!string.IsNullOrWhiteSpace(path) &&
            !SelectedRoots.Contains(path, StringComparer.OrdinalIgnoreCase))
        {
            SelectedRoots.Add(path);
        }
    }

    private void RemoveRoot(object? parameter)
    {
        if (parameter is string path)
        {
            SelectedRoots.Remove(path);
        }
    }

    public async Task SaveThemeAsync()
    {
        if (!IsThemeValid || IsBusy)
        {
            return;
        }

        var savedRevision = inputRevision;
        var theme = CurrentTheme;
        if (theme.Id == FolderTheme.IceBlue.Id)
        {
            theme = theme with { Id = Guid.NewGuid() };
        }

        IsManagingThemes = true;
        try
        {
            await themes.SaveAsync(theme);
            var loaded = await RefreshThemeCatalogAsync();
            if (savedRevision != inputRevision)
            {
                OperationStatus = $"Saved theme ‘{theme.Name}’; current edits remain unsaved.";
                return;
            }

            var saved = loaded.Themes.First(item => item.Id == theme.Id);
            if (themeId != saved.Id)
            {
                themeId = saved.Id;
                inputRevision++;
                InvalidatePlan();
                OnPropertyChanged(nameof(CurrentTheme));
            }
            selectedSavedTheme = saved;
            OnPropertyChanged(nameof(SelectedSavedTheme));
            RaiseCommandStates();
            OperationStatus = $"Saved theme ‘{saved.Name}’.";
        }
        catch (Exception exception)
        {
            dialogs.ShowError("Could not save theme", exception.Message);
        }
        finally
        {
            IsManagingThemes = false;
        }
    }

    public async Task ImportThemeAsync()
    {
        if (IsBusy)
        {
            return;
        }

        var sourcePath = await dialogs.PickThemeImportPathAsync();
        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            return;
        }

        IsManagingThemes = true;
        try
        {
            var result = await themes.ImportAsync(sourcePath);
            if (!result.Success || result.Theme is null)
            {
                dialogs.ShowError(
                    "Could not import theme",
                    result.Errors.Count == 0 ? "The selected file did not contain a valid theme." : string.Join(Environment.NewLine, result.Errors));
                return;
            }

            var loaded = await RefreshThemeCatalogAsync();
            var imported = loaded.Themes.First(theme => theme.Id == result.Theme.Id);
            SelectedSavedTheme = imported;
            OperationStatus = $"Imported theme ‘{imported.Name}’.";
        }
        catch (Exception exception)
        {
            dialogs.ShowError("Could not import theme", exception.Message);
        }
        finally
        {
            IsManagingThemes = false;
        }
    }

    public async Task ExportThemeAsync()
    {
        if (IsBusy || SelectedSavedTheme is not { } selected)
        {
            return;
        }

        var destinationPath = await dialogs.PickThemeExportPathAsync($"{MakeSafeFileName(selected.Name)}.json");
        if (string.IsNullOrWhiteSpace(destinationPath))
        {
            return;
        }

        IsManagingThemes = true;
        try
        {
            await themes.ExportAsync(selected.Id, destinationPath);
            OperationStatus = $"Exported theme ‘{selected.Name}’.";
        }
        catch (Exception exception)
        {
            dialogs.ShowError("Could not export theme", exception.Message);
        }
        finally
        {
            IsManagingThemes = false;
        }
    }

    public async Task DeleteThemeAsync()
    {
        if (IsBusy || SelectedSavedTheme is not { } selected)
        {
            return;
        }

        if (selected.Id == FolderTheme.IceBlue.Id)
        {
            dialogs.ShowError("Cannot delete built-in theme", "The built-in IceBlue theme cannot be deleted.");
            return;
        }

        var confirmed = await dialogs.ConfirmAsync(
            "Delete personal theme?",
            $"Delete the saved theme ‘{selected.Name}’? This removes its theme file and cannot be undone.");
        if (!confirmed)
        {
            return;
        }

        IsManagingThemes = true;
        try
        {
            await themes.DeleteAsync(selected.Id);
            var loaded = await RefreshThemeCatalogAsync();
            var fallback = loaded.Themes.First(theme => theme.Id == FolderTheme.IceBlue.Id);
            SelectedSavedTheme = fallback;
            OperationStatus = $"Deleted theme ‘{selected.Name}’.";
        }
        catch (Exception exception)
        {
            dialogs.ShowError("Could not delete theme", exception.Message);
        }
        finally
        {
            IsManagingThemes = false;
        }
    }

    private async Task<ThemeLoadResult> RefreshThemeCatalogAsync()
    {
        var loaded = await themes.LoadAsync();
        SavedThemes.Clear();
        foreach (var item in loaded.Themes)
        {
            SavedThemes.Add(item);
        }
        return loaded;
    }

    private static string MakeSafeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var safe = new string(name.Select(character => invalid.Contains(character) ? '-' : character).ToArray()).Trim();
        return safe.Length == 0 ? "folder-theme" : safe;
    }

    private void LoadTheme(FolderTheme theme)
    {
        themeId = theme.Id;
        themeName = theme.Name;
        gradientStart = theme.GradientStart;
        gradientEnd = theme.GradientEnd;
        gradientAngle = theme.GradientAngle;
        opacity = theme.Opacity;
        cornerRadius = theme.CornerRadius;
        strokeColor = theme.StrokeColor;
        strokeOpacity = theme.StrokeOpacity;
        strokeWidth = theme.StrokeWidth;
        highlightStrength = theme.HighlightStrength;
        glowColor = theme.GlowColor;
        glowStrength = theme.GlowStrength;
        glowRadius = theme.GlowRadius;
        shadowStrength = theme.ShadowStrength;
        shadowOffset = theme.ShadowOffset;
        inputRevision++;

        foreach (var property in ThemePropertyNames)
        {
            OnPropertyChanged(property);
        }
        OnPropertyChanged(nameof(CurrentTheme));
        serviceValidationErrors = [];
        InvalidatePlan();
        RefreshValidation();
        QueuePreviewRefresh();
    }

    private static readonly string[] ThemePropertyNames =
    [
        nameof(ThemeName), nameof(GradientStart), nameof(GradientEnd), nameof(GradientAngle),
        nameof(Opacity), nameof(CornerRadius), nameof(StrokeColor), nameof(StrokeOpacity),
        nameof(StrokeWidth), nameof(HighlightStrength), nameof(GlowColor), nameof(GlowStrength),
        nameof(GlowRadius), nameof(ShadowStrength), nameof(ShadowOffset)
    ];

    private void SelectedRootsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        inputRevision++;
        InvalidatePlan();
        RefreshValidation();
    }

    private ApplyRequest CreateApplyRequest() => new(
        CurrentIconSource,
        SelectedMode,
        SelectedMode == ApplicationMode.Compatible ? SelectedRoots.ToArray() : null);

    private void UpdatePlan(ApplyPlan plan)
    {
        PlanAllowedCount = plan.AllowedCount;
        PlanSkippedCount = plan.SkippedCount;
        serviceValidationErrors = plan.ValidationErrors;
        RefreshValidation();

        if (plan.Mode == ApplicationMode.Global)
        {
            PlanDetails = "Current-user Shell Icons values 3 and 4 will be updated; no folder tree is scanned.";
            return;
        }

        var reasons = plan.SkippedReasons.Count == 0
            ? "No skipped folders."
            : string.Join(", ", plan.SkippedReasons.Select(pair => $"{pair.Key}: {pair.Value}"));
        PlanDetails = $"{plan.AllowedCount} allowed, {plan.SkippedCount} skipped. {reasons}";
    }

    private static string BuildApplyConfirmation(ApplyPlan plan, IReadOnlyList<string> explicitRoots)
    {
        if (plan.Mode == ApplicationMode.Global)
        {
            return $"For the current Windows user, HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Icons values 3 and 4 will be changed to the rendered icon for ‘{plan.Theme.Name}’. The original mappings are backed up first and remain available through Restore latest. Continue?";
        }

        var roots = string.Join(Environment.NewLine, explicitRoots.Select(root => $"• {root}"));
        return $"Only these explicit folder roots will be considered; compatible mode never scans a whole drive:{Environment.NewLine}{roots}{Environment.NewLine}{Environment.NewLine}{plan.AllowedCount} allowed; {plan.SkippedCount} skipped. Existing Desktop.ini files keep unrelated settings while icon fields are updated. Known folders, network paths, reparse points, and unwritable folders are skipped. Continue?";
    }

    private void ShowApplyResult(ApplyResult result)
    {
        Results.Clear();
        var rolledBack = !result.Success && result.RollbackAttempted && result.RollbackSucceeded;
        var rollbackIncomplete = !result.Success && result.RollbackAttempted && !result.RollbackSucceeded;
        foreach (var item in result.Successful)
        {
            Results.Add(new OperationResultItem(
                rollbackIncomplete
                    ? ResultKind.StateUncertain
                    : rolledBack ? ResultKind.RolledBack : ResultKind.Success,
                item.Path,
                rollbackIncomplete
                    ? $"{item.Detail ?? "Changed"}; rollback incomplete, final state uncertain"
                    : rolledBack ? $"{item.Detail ?? "Changed"}; then rolled back" : item.Detail ?? "Applied"));
        }
        foreach (var item in result.Skipped)
        {
            Results.Add(new OperationResultItem(ResultKind.Skipped, item.Path, item.Detail ?? "Skipped"));
        }
        foreach (var failure in result.Failures)
        {
            Results.Add(new OperationResultItem(ResultKind.Failed, failure.Path, failure.Error));
        }

        ResultSummary = rollbackIncomplete
            ? $"State uncertain after incomplete rollback: {result.Successful.Count} changed  Skipped: {result.Skipped.Count}  Failed: {result.Failures.Count}"
            : rolledBack
                ? $"Changed then rolled back: {result.Successful.Count}  Skipped: {result.Skipped.Count}  Failed: {result.Failures.Count}"
                : $"Applied: {result.Successful.Count}  Skipped: {result.Skipped.Count}  Failed: {result.Failures.Count}";
        OperationStatus = result.Success
            ? "Apply completed successfully. A recovery snapshot is available."
            : rollbackIncomplete
                ? "Apply failed and rollback incomplete; system state is uncertain. Review every affected path and failure."
                : rolledBack
                ? "Apply did not complete; the recorded changes were rolled back."
                : "Apply partially completed or failed. Review each result below.";
        RefreshResultCommands();
    }

    private void ShowRestoreResult(RestoreResult result)
    {
        Results.Clear();
        foreach (var item in result.Successful)
        {
            Results.Add(new OperationResultItem(ResultKind.Success, item.Path, item.Detail ?? "Restored"));
        }
        foreach (var item in result.Skipped)
        {
            Results.Add(new OperationResultItem(ResultKind.Skipped, item.Path, item.Detail ?? "Already absent"));
        }
        foreach (var item in result.Changed)
        {
            Results.Add(new OperationResultItem(ResultKind.StateUncertain, item.Path, item.Detail ?? "Changed since apply; ownership remains unresolved"));
        }

        var changedPaths = result.Changed
            .Select(item => item.Path)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var failure in result.Failures)
        {
            if (failure.Path is not null && changedPaths.Contains(failure.Path)) continue;
            Results.Add(new OperationResultItem(ResultKind.Failed, failure.Path, failure.Error));
        }

        ResultSummary = $"Restored: {result.Successful.Count}  Already absent: {result.Skipped.Count}  Changed: {result.Changed.Count}  Failed: {result.Failures.Count - result.Changed.Count}";
        OperationStatus = result.Success
            ? result.AlreadyRestored
                ? "The latest backup had already been restored; no additional changes were made."
                : "Restore completed successfully."
            : "Restore partially completed or failed. Successful reversions remain in place; review the failures below.";
        RefreshResultCommands();
    }

    private void CopyFailedPaths()
    {
        var paths = FailedResults
            .Select(result => result.Path)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Distinct(StringComparer.OrdinalIgnoreCase);
        var text = string.Join(Environment.NewLine, paths);
        if (text.Length > 0)
        {
            try
            {
                dialogs.CopyText(text);
            }
            catch (Exception exception)
            {
                dialogs.ShowError("Could not copy failed paths", exception.Message);
            }
        }
    }

    private string BuildFailureText()
    {
        var builder = new StringBuilder();
        foreach (var result in FailedResults)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            builder.Append(result.Path ?? "(no path)");
            builder.Append('\t');
            builder.Append(result.Detail);
        }
        return builder.ToString();
    }

    private void ReportProgress(ApplyProgress progress)
    {
        ProgressMessage = $"{progress.Phase}: {progress.Message}";
        ProgressCompleted = progress.Completed;
        ProgressTotal = progress.Total;
    }

    private void RefreshValidation()
    {
        var validation = ThemeValidator.Validate(CurrentTheme);
        IsThemeValid = validation.IsValid;
        if (SelectedIconEditorMode == IconEditorMode.ImportedImage && importedImageAssetPath is null)
        {
            ValidationMessage = "请先选择一张 PNG、JPG 或 BMP 图片。";
        }
        else if ((SelectedIconEditorMode == IconEditorMode.BuiltIn && !validation.IsValid) || serviceValidationErrors.Count > 0)
        {
            var errors = validation.Errors.Concat(serviceValidationErrors).Distinct(StringComparer.Ordinal);
            ValidationMessage = $"Fix these fields before applying: {string.Join(", ", errors)}.";
        }
        else if (!HasRequiredRoots)
        {
            ValidationMessage = "Compatibility mode requires at least one explicit folder root. No whole-drive root is selected automatically.";
        }
        else
        {
            ValidationMessage = "图标已准备好，可以预览或计算方案。";
        }

        RaiseCommandStates();
    }

    private void InvalidatePlan()
    {
        ClearReviewedPlan();
        PlanAllowedCount = 0;
        PlanSkippedCount = 0;
        PlanDetails = "Plan not calculated yet.";
        serviceValidationErrors = [];
    }

    private void ClearReviewedPlan()
    {
        reviewedPlan = null;
        reviewedRequest = null;
        reviewedPlanRevision = -1;
        RaiseCommandStates();
    }

    private void QueuePreviewRefresh()
    {
        previewCancellation?.Cancel();
        previewCancellation?.Dispose();
        var cancellation = new CancellationTokenSource();
        previewCancellation = cancellation;
        IsPreviewRendering = true;
        previewCompletion = RefreshPreviewsAsync(cancellation);
    }

    private async Task RefreshPreviewsAsync(CancellationTokenSource cancellation)
    {
        try
        {
            await previewDelay.DelayAsync(PreviewDebounce, cancellation.Token);
            cancellation.Token.ThrowIfCancellationRequested();
            if (!HasValidIconSource)
            {
                await dispatcher.InvokeAsync(() =>
                {
                    foreach (var preview in Previews) preview.Image = null;
                });
                return;
            }
            var source = CurrentIconSource;
            var sizes = Previews.Select(preview => preview.Size).ToArray();
            var rendered = await previewWorkScheduler.RunAsync(() =>
            {
                var images = new List<(int Size, BitmapSource Image)>();
                foreach (var size in sizes)
                {
                    cancellation.Token.ThrowIfCancellationRequested();
                    var image = previewRenderer.Render(source, size);
                    if (image.CanFreeze && !image.IsFrozen) image.Freeze();
                    images.Add((size, image));
                }

                return (IReadOnlyList<(int Size, BitmapSource Image)>)images;
            }, cancellation.Token);
            await dispatcher.InvokeAsync(() =>
            {
                cancellation.Token.ThrowIfCancellationRequested();
                foreach (var item in rendered)
                {
                    Previews.Single(preview => preview.Size == item.Size).Image = item.Image;
                }
            });
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            await dispatcher.InvokeAsync(() =>
            {
                OperationStatus = "Preview rendering failed. Fix the theme values and try again.";
                dialogs.ShowError("Preview failed", exception.Message);
            });
        }
        finally
        {
            if (ReferenceEquals(previewCancellation, cancellation))
            {
                await dispatcher.InvokeAsync(() => IsPreviewRendering = false);
            }
        }
    }

    private void RefreshResultCommands()
    {
        OnPropertyChanged(nameof(FailedResults));
        copyFailedPathsCommand.RaiseCanExecuteChanged();
        exportFailedPathsCommand.RaiseCanExecuteChanged();
    }

    private void RaiseBusyState()
    {
        OnPropertyChanged(nameof(IsBusy));
        OnPropertyChanged(nameof(AreInputsEnabled));
        OnPropertyChanged(nameof(CanApply));
        OnPropertyChanged(nameof(CanPlan));
        RaiseCommandStates();
    }

    private void RaiseCommandStates()
    {
        OnPropertyChanged(nameof(CanApply));
        OnPropertyChanged(nameof(CanPlan));
        applyCommand?.RaiseCanExecuteChanged();
        restoreCommand?.RaiseCanExecuteChanged();
        planCommand?.RaiseCanExecuteChanged();
        addRootCommand?.RaiseCanExecuteChanged();
        removeRootCommand?.RaiseCanExecuteChanged();
        saveThemeCommand?.RaiseCanExecuteChanged();
        importThemeCommand?.RaiseCanExecuteChanged();
        exportThemeCommand?.RaiseCanExecuteChanged();
        deleteThemeCommand?.RaiseCanExecuteChanged();
        chooseImportedImageCommand?.RaiseCanExecuteChanged();
        clearImportedImageCommand?.RaiseCanExecuteChanged();
    }

    private bool SetThemeField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!SetField(ref field, value, propertyName))
        {
            return false;
        }

        inputRevision++;
        if (selectedSavedTheme is not null)
        {
            selectedSavedTheme = null;
            OnPropertyChanged(nameof(SelectedSavedTheme));
        }
        OnPropertyChanged(nameof(CurrentTheme));
        if (!syncingPalette)
        {
            var picker = propertyName switch
            {
                nameof(GradientStart) => PaletteEditor.GradientStart,
                nameof(GradientEnd) => PaletteEditor.GradientEnd,
                nameof(StrokeColor) => PaletteEditor.Stroke,
                nameof(GlowColor) => PaletteEditor.Glow,
                _ => null
            };
            if (value is string color) picker?.TrySetHex(color);
        }
        serviceValidationErrors = [];
        InvalidatePlan();
        RefreshValidation();
        QueuePreviewRefresh();
        return true;
    }

    public async Task ToggleMonitoringAsync()
    {
        if (folderMonitoring.Status.Paused) await folderMonitoring.ResumeAsync();
        else await folderMonitoring.PauseAsync();
        RaiseMonitoringState();
    }

    private async Task RemoveMonitoringRuleAsync(string? root)
    {
        if (string.IsNullOrWhiteSpace(root)) return;
        await folderMonitoring.RemoveRuleAsync(root);
        RaiseMonitoringState();
    }

    private void MonitoringOutcomeProduced(object? sender, MonitoringOutcome outcome) =>
        _ = dispatcher.InvokeAsync(() =>
        {
            Results.Add(new OperationResultItem(outcome.Success ? ResultKind.Success : ResultKind.Failed, outcome.Path,
                outcome.Success ? "后台监控已应用图标" : outcome.Error ?? "后台监控应用失败"));
            RefreshResultCommands();
        });

    private void RaiseMonitoringState()
    {
        OnPropertyChanged(nameof(MonitoringStatusText));
        OnPropertyChanged(nameof(ActiveMonitoringRuleCount));
        toggleMonitoringCommand.RaiseCanExecuteChanged();
        removeMonitoringRuleCommand.RaiseCanExecuteChanged();
    }

    private bool HasValidIconSource => SelectedIconEditorMode == IconEditorMode.ImportedImage
        ? importedImageAssetPath is not null
        : IsThemeValid;

    private IconSource CurrentIconSource => SelectedIconEditorMode == IconEditorMode.ImportedImage
        ? IconSource.ImportedImage(importedImageAssetPath ?? throw new InvalidOperationException("Choose an image first."))
        : IconSource.BuiltIn(CurrentTheme);

    private void IconSourceChanged()
    {
        inputRevision++;
        InvalidatePlan();
        RefreshValidation();
        QueuePreviewRefresh();
        clearImportedImageCommand.RaiseCanExecuteChanged();
    }

    private void PaletteEditorChanged(object? sender, EventArgs e)
    {
        if (!syncingPalette) ApplyPalette(PaletteEditor.Palette);
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private sealed class DispatcherProgress<T>(
        IViewModelDispatcher dispatcher,
        Action<T> report) : IProgress<T>
    {
        public void Report(T value) => _ = dispatcher.InvokeAsync(() => report(value));
    }
}

public sealed class ThemeApplicationCoordinator(ThemeApplicationService service) : IThemeApplicationCoordinator
{
    public Task<ApplyPlan> PlanAsync(ApplyRequest request, CancellationToken token) =>
        service.PlanAsync(request, token);

    public Task<ApplyResult> ApplyAsync(ApplyPlan plan, IProgress<ApplyProgress> progress, CancellationToken token) =>
        service.ApplyAsync(plan, progress, token);

    public Task<RestoreResult> RestoreLatestAsync(IProgress<ApplyProgress> progress, CancellationToken token) =>
        service.RestoreLatestAsync(progress, token);
}

public sealed class ThemeCatalog(ThemeStore store) : IThemeCatalog
{
    public Task<ThemeLoadResult> LoadAsync() => store.LoadAsync();
    public Task SaveAsync(FolderTheme theme) => store.SaveAsync(theme);
    public Task<ThemeImportResult> ImportAsync(string sourcePath) => store.ImportAsync(sourcePath);
    public Task ExportAsync(Guid themeId, string destinationPath) => store.ExportAsync(themeId, destinationPath);
    public Task DeleteAsync(Guid themeId) => store.DeleteAsync(themeId);
}

public sealed class FolderPreviewRenderer : IPreviewRenderer
{
    public BitmapSource Render(IconSource source, int size) => source.Kind switch
    {
        IconSourceKind.BuiltIn => FolderIconRenderer.Render(source.Theme!, size),
        IconSourceKind.ImportedImage => ImportedIconRenderer.Render(source.ImportedImagePath!, size),
        _ => throw new ArgumentOutOfRangeException(nameof(source))
    };
}

public sealed class TaskPreviewDelay : IPreviewDelay
{
    public Task DelayAsync(TimeSpan delay, CancellationToken token) => Task.Delay(delay, token);
}

public sealed class StaPreviewWorkScheduler : IPreviewWorkScheduler
{
    private readonly BlockingCollection<IWorkItem> queue = [];
    private readonly Thread thread;
    private int disposed;

    public StaPreviewWorkScheduler()
    {
        thread = new Thread(ProcessQueue)
        {
            IsBackground = true,
            Name = "Folder Theme Studio preview renderer"
        };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
    }

    public Task<T> RunAsync<T>(Func<T> work, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(work);
        ObjectDisposedException.ThrowIf(disposed != 0, this);
        token.ThrowIfCancellationRequested();
        var item = new WorkItem<T>(work, token);
        queue.Add(item, token);
        return item.Task;
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref disposed, 1) == 0) queue.CompleteAdding();
    }

    private void ProcessQueue()
    {
        foreach (var item in queue.GetConsumingEnumerable()) item.Execute();
    }

    private interface IWorkItem { void Execute(); }

    private sealed class WorkItem<T>(Func<T> work, CancellationToken token) : IWorkItem
    {
        private readonly TaskCompletionSource<T> completion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        internal Task<T> Task => completion.Task;

        public void Execute()
        {
            if (token.IsCancellationRequested)
            {
                completion.TrySetCanceled(token);
                return;
            }

            try { completion.TrySetResult(work()); }
            catch (OperationCanceledException exception) when (exception.CancellationToken == token)
            { completion.TrySetCanceled(token); }
            catch (Exception exception) { completion.TrySetException(exception); }
        }
    }
}

public sealed class WpfViewModelDispatcher : IViewModelDispatcher
{
    public Task InvokeAsync(Action action)
    {
        var application = System.Windows.Application.Current;
        if (application is null || application.Dispatcher.CheckAccess())
        {
            action();
            return Task.CompletedTask;
        }

        return application.Dispatcher.InvokeAsync(action).Task;
    }
}
