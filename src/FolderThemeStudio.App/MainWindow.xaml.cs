using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using FolderThemeStudio.App.Controls;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.ViewModels;

namespace FolderThemeStudio.App;

public partial class MainWindow : RoundedWindow
{
    private const double StackedLayoutWidth = 900;
    private readonly MainViewModel viewModel;
    private readonly IAppSettingsStore settingsStore;
    private readonly ILocalizationService localization;
    private readonly IStartupRegistrationService? startupRegistration;
    private readonly Action? backgrounded;
    private readonly IMonitoringRuleStore monitoringRuleStore;
    private readonly IFolderMonitoringCoordinator folderMonitoring;
    private readonly IMonitoringIconAssetService monitoringAssets;
    private readonly IMainViewModelDialogs ruleDialogs;
    private readonly IFolderCategoryCatalog folderCategoryCatalog;
    private readonly IFolderHoverMonitorService folderHoverMonitor;
    private readonly Action<double>? updateCardOpacity;
    private AppSettings currentSettings;
    private bool explicitExit;
    private FolderCategoriesWindow? quickCategoryDialog;
    private FolderCategoriesViewModel? quickCategoryViewModel;
    private readonly Action<FolderThemeStudio.App.Services.FolderEditChord>? updateEditChord;

    public MainWindow(MainViewModel viewModel) : this(
        viewModel,
        new JsonAppSettingsStore(),
        new LocalizationService(AppSettings.Default.Language),
        AppSettings.Default with { CloseToTray = false },
        monitoringRuleStore: new JsonMonitoringRuleStore(),
        folderMonitoring: new NullFolderMonitoringCoordinator(),
        monitoringAssets: new MonitoringIconAssetService())
    {
    }

    public MainWindow(
        MainViewModel viewModel,
        IAppSettingsStore settingsStore,
        ILocalizationService localization,
        AppSettings currentSettings,
        IStartupRegistrationService? startupRegistration = null,
        Action? backgrounded = null,
        IMonitoringRuleStore? monitoringRuleStore = null,
        IFolderMonitoringCoordinator? folderMonitoring = null,
        IMonitoringIconAssetService? monitoringAssets = null,
        IMainViewModelDialogs? ruleDialogs = null,
        IFolderCategoryCatalog? folderCategoryCatalog = null,
        IFolderHoverMonitorService? folderHoverMonitor = null,
        Action<double>? updateCardOpacity = null,
        Action<FolderThemeStudio.App.Services.FolderEditChord>? updateEditChord = null)
    {
        InitializeComponent();
        this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        this.settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
        this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        this.currentSettings = currentSettings ?? throw new ArgumentNullException(nameof(currentSettings));
        this.startupRegistration = startupRegistration;
        this.backgrounded = backgrounded;
        this.monitoringRuleStore = monitoringRuleStore ?? new JsonMonitoringRuleStore();
        this.folderMonitoring = folderMonitoring ?? new NullFolderMonitoringCoordinator();
        this.monitoringAssets = monitoringAssets ?? new MonitoringIconAssetService();
        this.ruleDialogs = ruleDialogs ?? new DialogService(() => this);
        this.folderCategoryCatalog = folderCategoryCatalog ?? new JsonFolderCategoryCatalog();
        this.folderHoverMonitor = folderHoverMonitor ?? new NullFolderHoverMonitor();
        this.updateCardOpacity = updateCardOpacity;
        this.updateEditChord = updateEditChord;
        DataContext = viewModel;
        Closing += Window_Closing;
        Closed += (_, _) =>
        {
            this.folderHoverMonitor.Dispose();
            viewModel.Dispose();
        };
        UpdateResponsiveLayout(Width);
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e) =>
        UpdateResponsiveLayout(e.NewSize.Width);

    private void MainScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        for (var element = e.OriginalSource as DependencyObject; element is not null && !ReferenceEquals(element, MainScrollViewer);
             element = VisualTreeHelper.GetParent(element))
        {
            if (element is ScrollViewer) return;
        }
        MainScrollViewer.ScrollToVerticalOffset(MainWindowWheelPolicy.NextOffset(
            MainScrollViewer.VerticalOffset, e.Delta, SystemParameters.WheelScrollLines, 16));
        e.Handled = true;
    }

    public async Task ShowFirstRunTutorialAsync()
    {
        if (!currentSettings.TutorialCompleted) await OpenTutorialAsync();
    }

    private async void Tutorial_Click(object sender, RoutedEventArgs e) => await OpenTutorialAsync();

    private async void AutoStyleRules_Click(object sender, RoutedEventArgs e)
    {
        var viewModel = new AutoStyleRulesViewModel(
            monitoringRuleStore,
            folderMonitoring,
            monitoringAssets,
            ruleDialogs,
            localization,
            new WpfViewModelDispatcher(),
            this.viewModel.CurrentTheme);
        await viewModel.LoadAsync();
        var dialog = new AutoStyleRulesWindow(viewModel) { Owner = this };
        dialog.ShowDialog();
    }

    private async void FolderCategories_Click(object sender, RoutedEventArgs e)
    {
        var categories = new FolderCategoriesViewModel(
            folderCategoryCatalog,
            folderHoverMonitor,
            ruleDialogs,
            localization,
            new WpfViewModelDispatcher());
        await categories.LoadAsync();
        var dialog = new FolderCategoriesWindow(categories) { Owner = this };
        dialog.ShowDialog();
    }

    public async Task OpenFolderCategoryEditorAsync(string folderPath)
    {
        if (string.IsNullOrWhiteSpace(folderPath) || !System.IO.Directory.Exists(folderPath)) return;
        if (quickCategoryDialog is { IsVisible: true } && quickCategoryViewModel is not null)
        {
            quickCategoryViewModel.SelectFolder(folderPath);
            quickCategoryDialog.Activate();
            return;
        }
        var categories = new FolderCategoriesViewModel(
            folderCategoryCatalog, folderHoverMonitor, ruleDialogs, localization, new WpfViewModelDispatcher());
        await categories.LoadAsync();
        categories.SelectFolder(folderPath);
        var dialog = new FolderCategoriesWindow(categories);
        if (IsVisible) dialog.Owner = this;
        else dialog.ShowInTaskbar = true;
        dialog.Closed += (_, _) =>
        {
            if (ReferenceEquals(quickCategoryDialog, dialog))
            {
                quickCategoryDialog = null;
                quickCategoryViewModel = null;
            }
        };
        quickCategoryDialog = dialog;
        quickCategoryViewModel = categories;
        dialog.Show();
        dialog.Activate();
    }

    public Task StartBackgroundServicesAsync(CancellationToken token = default) => folderHoverMonitor.StartAsync(token);

    private async void Settings_Click(object sender, RoutedEventArgs e)
    {
        var settingsViewModel = new SettingsViewModel(
            currentSettings,
            settingsStore,
            localization,
            viewModel.ApplyPalette,
            new FolderPreviewRenderer());
        var dialog = new SettingsWindow(settingsViewModel) { Owner = this };
        if (dialog.ShowDialog() == true)
        {
            currentSettings = await settingsStore.LoadAsync();
            updateCardOpacity?.Invoke(currentSettings.CardOpacity);
            updateEditChord?.Invoke(currentSettings.EditChord);
            startupRegistration?.SetEnabled(currentSettings.StartWithWindows);
        }
    }

    private async Task OpenTutorialAsync()
    {
        var tutorial = new TutorialViewModel(currentSettings, settingsStore, localization);
        var dialog = new TutorialWindow(tutorial) { Owner = this };
        dialog.ShowDialog();
        currentSettings = await settingsStore.LoadAsync();
    }

    public void ShowFromTray()
    {
        Show();
        WindowState = WindowState.Normal;
        ShowInTaskbar = true;
        Activate();
    }

    public void RequestExplicitExit()
    {
        explicitExit = true;
        Close();
    }

    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        if (explicitExit || !currentSettings.CloseToTray) return;
        e.Cancel = true;
        Hide();
        backgrounded?.Invoke();
    }

    private void UpdateResponsiveLayout(double width)
    {
        var stacked = width < StackedLayoutWidth;
        var compactHeader = width < 900;
        Grid.SetRow(TopActionsPanel, compactHeader ? 1 : 0);
        Grid.SetColumn(TopActionsPanel, compactHeader ? 0 : 1);
        Grid.SetColumnSpan(TopActionsPanel, compactHeader ? 2 : 1);
        TopActionsPanel.MaxWidth = compactHeader ? Math.Max(0, width - 48) : double.PositiveInfinity;
        TopActionsPanel.HorizontalAlignment = compactHeader ? System.Windows.HorizontalAlignment.Right : System.Windows.HorizontalAlignment.Stretch;
        HeaderGrid.RowDefinitions[1].Height = compactHeader ? GridLength.Auto : new GridLength(0);
        EditorColumn.Width = stacked ? new GridLength(1, GridUnitType.Star) : new GridLength(1.05, GridUnitType.Star);
        ApplicationColumn.Width = stacked ? new GridLength(0) : new GridLength(.95, GridUnitType.Star);
        TopRow.Height = GridLength.Auto;
        StackedRow.Height = stacked ? GridLength.Auto : new GridLength(0);

        Grid.SetColumn(EditorPanel, 0);
        Grid.SetRow(EditorPanel, 0);
        Grid.SetColumn(ApplicationPanel, stacked ? 0 : 1);
        Grid.SetRow(ApplicationPanel, stacked ? 1 : 0);
        EditorPanel.Margin = stacked ? new Thickness(0) : new Thickness(0, 0, 10, 0);
        ApplicationPanel.Margin = stacked ? new Thickness(0, 8, 0, 0) : new Thickness(10, 0, 0, 0);
    }
}
