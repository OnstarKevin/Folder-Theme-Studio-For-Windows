using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.ViewModels;

namespace FolderThemeStudio.App;

public partial class MainWindow : Window
{
    private const double StackedLayoutWidth = 900;
    private readonly MainViewModel viewModel;
    private readonly IAppSettingsStore settingsStore;
    private readonly ILocalizationService localization;
    private readonly IStartupRegistrationService? startupRegistration;
    private readonly Action? backgrounded;
    private AppSettings currentSettings;
    private bool explicitExit;

    public MainWindow(MainViewModel viewModel) : this(
        viewModel,
        new JsonAppSettingsStore(),
        new LocalizationService(AppSettings.Default.Language),
        AppSettings.Default with { CloseToTray = false })
    {
    }

    public MainWindow(
        MainViewModel viewModel,
        IAppSettingsStore settingsStore,
        ILocalizationService localization,
        AppSettings currentSettings,
        IStartupRegistrationService? startupRegistration = null,
        Action? backgrounded = null)
    {
        InitializeComponent();
        this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        this.settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
        this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        this.currentSettings = currentSettings ?? throw new ArgumentNullException(nameof(currentSettings));
        this.startupRegistration = startupRegistration;
        this.backgrounded = backgrounded;
        DataContext = viewModel;
        Closing += Window_Closing;
        Closed += (_, _) => viewModel.Dispose();
        UpdateResponsiveLayout(Width);
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e) =>
        UpdateResponsiveLayout(e.NewSize.Width);

    public async Task ShowFirstRunTutorialAsync()
    {
        if (!currentSettings.TutorialCompleted) await OpenTutorialAsync();
    }

    private async void Tutorial_Click(object sender, RoutedEventArgs e) => await OpenTutorialAsync();

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
