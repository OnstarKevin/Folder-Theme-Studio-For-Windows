using System.Windows;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Themes;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.Core.SystemIntegration;

namespace FolderThemeStudio.App;

public partial class App : System.Windows.Application
{
    private TrayIconService? trayIcon;
    private MainWindow? mainWindow;
    private FolderQuickEditMouseHook? quickEditHook;

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        var settingsStore = new JsonAppSettingsStore();
        var settings = await settingsStore.LoadAsync();
        var localization = new LocalizationService(settings.Language);
        var dialogs = new DialogService(() => MainWindow);
        var monitoringStore = new JsonMonitoringRuleStore();
        var folderCategoryCatalog = new JsonFolderCategoryCatalog();
        var folderMonitoring = new FolderMonitoringCoordinator(
            monitoringStore,
            new MonitoredFolderIconService());
        var monitoringAssets = new MonitoringIconAssetService();
        var categoryPopup = new FolderCategoryPopupWindow();
        categoryPopup.SetCardOpacity(settings.CardOpacity);
        var folderHoverMonitor = new FolderHoverMonitor(
            folderCategoryCatalog,
            new ExplorerFolderHoverTargetResolver(),
            new WindowsCursorPositionSource(),
            categoryPopup,
            new WpfViewModelDispatcher());
        var viewModel = new MainViewModel(
            new ThemeApplicationCoordinator(new ThemeApplicationService()),
            dialogs,
            new ThemeCatalog(new ThemeStore()),
            new FolderPreviewRenderer(),
            new TaskPreviewDelay(),
            new WpfViewModelDispatcher(),
            folderMonitoring: folderMonitoring,
            monitoringAssets: monitoringAssets);
        viewModel.ApplyPalette(settings.Palette);
        var startupRegistration = new StartupRegistrationService();
        try { startupRegistration.SetEnabled(settings.StartWithWindows); }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.Security.SecurityException)
        {
            // Startup registration is optional; the main application remains usable.
        }
        trayIcon = new TrayIconService();
        var window = new MainWindow(
            viewModel,
            settingsStore,
            localization,
            settings,
            startupRegistration,
            () => trayIcon.ShowBackgroundNotice(),
            monitoringStore,
            folderMonitoring,
            monitoringAssets,
            dialogs,
            folderCategoryCatalog,
            folderHoverMonitor,
            categoryPopup.SetCardOpacity,
            chord => quickEditHook?.UpdateChord(chord));
        mainWindow = window;
        MainWindow = window;
        try
        {
            quickEditHook = new FolderQuickEditMouseHook(folderHoverMonitor,
                path => _ = window.OpenFolderCategoryEditorAsync(path), settings.EditChord, Dispatcher);
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // Folder styling remains available if Windows denies the global mouse hook.
        }
        window.Closed += (_, _) => quickEditHook?.Dispose();
        trayIcon.OpenRequested += (_, _) => window.Dispatcher.Invoke(window.ShowFromTray);
        trayIcon.ToggleMonitoringRequested += async (_, _) => await viewModel.ToggleMonitoringAsync();
        trayIcon.ExitRequested += (_, _) => window.Dispatcher.Invoke(ExitApplication);
        await viewModel.InitializeAsync();
        await window.StartBackgroundServicesAsync();
        var background = e.Args.Any(argument => string.Equals(argument, "--background", StringComparison.OrdinalIgnoreCase));
        if (!background)
        {
            window.Show();
            await window.ShowFirstRunTutorialAsync();
        }
    }

    private void ExitApplication()
    {
        quickEditHook?.Dispose();
        trayIcon?.Dispose();
        trayIcon = null;
        mainWindow?.RequestExplicitExit();
        Shutdown();
    }
}
