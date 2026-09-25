using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using FolderThemeStudio.App;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Tests.TestSupport;
using FolderThemeStudio.Core.Themes;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class MainWindowTests
{
    [Theory]
    [InlineData(200, -120, 296)]
    [InlineData(200, 120, 104)]
    public void DiyWheel_TravelsTwiceThreeLinesOfSixteenPixels(double current, int delta, double expected)
    {
        Assert.Equal(expected, MainWindowWheelPolicy.NextOffset(current, delta, 3, 16));
    }
    [Fact]
    public void ImmediateUseHint_ExplainsThatSavingIsOptionalInBothLanguages()
    {
        var fixture = new MainViewModelFixture();
        var viewModel = fixture.CreateViewModel();
        LocalizationService? localization = null;
        MainWindow? window = null;

        try
        {
            WpfTestHost.Invoke(() =>
            {
                localization = new LocalizationService("zh-CN");
                window = new MainWindow(
                    viewModel,
                    new JsonAppSettingsStore(),
                    localization,
                    AppSettings.Default with { CloseToTray = false });
                var hint = Assert.IsType<System.Windows.Controls.TextBlock>(window.FindName("ImmediateUseHint"));
                Assert.Contains("无需保存", hint.Text);
                localization.ApplyLanguage("en-US");
                Assert.Contains("without saving", hint.Text, StringComparison.OrdinalIgnoreCase);
            });
        }
        finally
        {
            if (window is not null) WpfTestHost.Invoke(window.RequestExplicitExit);
        }
    }

    [Fact]
    public void TopActions_UpdateWhenLanguageChanges()
    {
        using var directory = new TemporaryDirectory();
        var fixture = new MainViewModelFixture();
        var viewModel = fixture.CreateViewModel();
        LocalizationService? localization = null;
        MainWindow? window = null;
        WpfTestHost.Invoke(() =>
        {
            localization = new LocalizationService("zh-CN");
            window = new MainWindow(
                viewModel,
                new JsonAppSettingsStore(Path.Combine(directory.Path, "settings.json")),
                localization,
                AppSettings.Default);
        });
        try
        {
            WpfTestHost.Invoke(() =>
            {
                var tutorial = Assert.IsType<System.Windows.Controls.Button>(window!.FindName("TutorialButton"));
                var settings = Assert.IsType<System.Windows.Controls.Button>(window.FindName("SettingsButton"));
                var folderCategories = Assert.IsType<System.Windows.Controls.Button>(window.FindName("FolderCategoriesButton"));
                Assert.Equal("使用教程", tutorial.Content);
                Assert.Equal("设置", settings.Content);
                Assert.Equal("文件夹分类卡片", folderCategories.Content);
            });
            WpfTestHost.Invoke(() => localization!.ApplyLanguage("en-US"));
            WpfTestHost.Invoke(() =>
            {
                var tutorial = Assert.IsType<System.Windows.Controls.Button>(window!.FindName("TutorialButton"));
                var settings = Assert.IsType<System.Windows.Controls.Button>(window.FindName("SettingsButton"));
                var folderCategories = Assert.IsType<System.Windows.Controls.Button>(window.FindName("FolderCategoriesButton"));
                Assert.Equal("_Tutorial", tutorial.Content);
                Assert.Equal("_Settings", settings.Content);
                Assert.Equal("Folder Hover Cards", folderCategories.Content);
            });
        }
        finally
        {
            if (window is not null) WpfTestHost.Invoke(window.RequestExplicitExit);
        }
    }

    [Fact]
    public void ProgressBinding_CannotWriteToReadOnlyViewModelProperty()
    {
        WpfTestHost.Invoke(() =>
        {
            var fixture = new MainViewModelFixture();
            var viewModel = fixture.CreateViewModel();
            var window = new MainWindow(viewModel)
            {
                ShowActivated = false,
                ShowInTaskbar = false,
                Opacity = 0,
                Left = -10000,
                Top = -10000,
            };
            try
            {
                window.Show();
                window.UpdateLayout();
            }
            finally
            {
                window.Close();
            }
        });
    }

    [Fact]
    public void AutoStyleRulesAction_UpdatesWhenLanguageChanges()
    {
        var fixture = new MainViewModelFixture();
        var viewModel = fixture.CreateViewModel();
        var localization = new LocalizationService("zh-CN");
        MainWindow? window = null;
        WpfTestHost.Invoke(() => window = new MainWindow(
            viewModel, new JsonAppSettingsStore(), localization, AppSettings.Default with { CloseToTray = false }));
        try
        {
            WpfTestHost.Invoke(() =>
            {
                var button = Assert.IsType<System.Windows.Controls.Button>(window!.FindName("AutoStyleRulesButton"));
                Assert.Equal("自动样式规则", button.Content);
            });
            WpfTestHost.Invoke(() => localization.ApplyLanguage("en-US"));
            WpfTestHost.Invoke(() =>
            {
                var button = Assert.IsType<System.Windows.Controls.Button>(window!.FindName("AutoStyleRulesButton"));
                Assert.Equal("_Automatic Rules", button.Content);
            });
        }
        finally
        {
            if (window is not null) WpfTestHost.Invoke(window.RequestExplicitExit);
        }
    }

    [Fact]
    public void AutoStyleRulesWindow_RendersListEditorPreviewAndGuidance()
    {
        using var temp = new TemporaryDirectory();
        var fixture = new MainViewModelFixture();
        AutoStyleRulesWindow? window = null;
        WpfTestHost.Invoke(() =>
        {
            var localization = new LocalizationService("zh-CN");
            var viewModel = new AutoStyleRulesViewModel(
                new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json")),
                new NullFolderMonitoringCoordinator(),
                new MonitoringIconAssetService(Path.Combine(temp.Path, "assets")),
                fixture.Dialogs,
                localization,
                new ImmediateViewModelDispatcher(),
                FolderTheme.IceBlue);
            viewModel.LoadAsync().GetAwaiter().GetResult();
            window = new AutoStyleRulesWindow(viewModel);

            Assert.Equal("自动样式规则", window.Title);
            Assert.IsType<System.Windows.Controls.ListView>(window.FindName("AutoRulesList"));
            Assert.IsType<System.Windows.Controls.Border>(window.FindName("AutoRulesEmptyState"));
            Assert.IsType<System.Windows.Controls.TextBlock>(window.FindName("AutoRulesFallback"));
            Assert.IsType<System.Windows.Controls.TextBox>(window.FindName("AutoRulesKeyword"));
            Assert.IsType<System.Windows.Controls.Image>(window.FindName("AutoRulesIconPreview"));
            Assert.IsType<System.Windows.Controls.Button>(window.FindName("AutoRulesSaveButton"));
            Assert.IsType<System.Windows.Controls.Button>(window.FindName("AutoRulesCancelButton"));
            Assert.IsType<System.Windows.Controls.TextBlock>(window.FindName("AutoRulesHelp"));
            localization.ApplyLanguage("en-US");
            Assert.Equal("Automatic Style Rules", window.Title);
        });
    }

    [Fact]
    public void Close_WithCloseToTray_HidesUntilExplicitExit()
    {
        WpfTestHost.Invoke(() =>
        {
            var fixture = new MainViewModelFixture();
            var window = new MainWindow(
                fixture.CreateViewModel(),
                new JsonAppSettingsStore(Path.Combine(Path.GetTempPath(), $"fts-{Guid.NewGuid():N}.json")),
                new LocalizationService(applyToApplication: false),
                AppSettings.Default);
            window.Show();
            window.Close();
            Assert.False(window.IsVisible);
            Assert.True(window.IsLoaded);
            window.RequestExplicitExit();
        });
    }

    [Fact]
    public void FolderHoverMonitor_StaysActiveInTrayAndDisposesOnExplicitExit()
    {
        var fixture = new MainViewModelFixture();
        var monitor = new RecordingFolderHoverMonitor();
        MainWindow? window = null;
        WpfTestHost.Invoke(() =>
        {
            window = new MainWindow(
                fixture.CreateViewModel(),
                new JsonAppSettingsStore(Path.Combine(Path.GetTempPath(), $"fts-{Guid.NewGuid():N}.json")),
                new LocalizationService(applyToApplication: false),
                AppSettings.Default,
                folderHoverMonitor: monitor);
            window.StartBackgroundServicesAsync().GetAwaiter().GetResult();
            window.Show();
            window.Close();
            Assert.False(window.IsVisible);
            Assert.Equal(0, monitor.DisposeCount);

            window.ShowFromTray();
            Assert.Equal(1, monitor.StartCount);
            window.RequestExplicitExit();

            Assert.Equal(1, monitor.DisposeCount);
        });
    }

    [Fact]
    public void FolderCategoriesAction_OpensTheManagementDialog()
    {
        using var temp = new TemporaryDirectory();
        var fixture = new MainViewModelFixture();
        var monitor = new RecordingFolderHoverMonitor();
        MainWindow? window = null;
        var opened = false;
        WpfTestHost.Invoke(() =>
        {
            var localization = new LocalizationService("zh-CN");
            window = new MainWindow(
                fixture.CreateViewModel(),
                new JsonAppSettingsStore(Path.Combine(temp.Path, "settings.json")),
                localization,
                AppSettings.Default with { CloseToTray = false },
                ruleDialogs: fixture.Dialogs,
                folderCategoryCatalog: new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "categories.json")),
                folderHoverMonitor: monitor);
            var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(25), DispatcherPriority.Background, (_, _) =>
            {
                var dialog = System.Windows.Application.Current.Windows.OfType<FolderCategoriesWindow>().FirstOrDefault();
                if (dialog is null) return;
                opened = true;
                dialog.Close();
            }, window.Dispatcher);
            timer.Start();

            try
            {
                window.Show();
                var button = Assert.IsType<System.Windows.Controls.Button>(window.FindName("FolderCategoriesButton"));
                button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
            finally
            {
                timer.Stop();
                if (window.IsLoaded) window.RequestExplicitExit();
            }
        });

        Assert.True(opened);
    }

    [Fact]
    public void QuickEdit_OpensSelectedFolderWhileMainWindowIsHidden()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "Project");
        Directory.CreateDirectory(folder);
        var fixture = new MainViewModelFixture();
        var opened = false;
        WpfTestHost.Invoke(() =>
        {
            var window = new MainWindow(
                fixture.CreateViewModel(),
                new JsonAppSettingsStore(Path.Combine(temp.Path, "settings.json")),
                new LocalizationService("zh-CN"),
                AppSettings.Default,
                ruleDialogs: fixture.Dialogs,
                folderCategoryCatalog: new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "categories.json")));
            var frame = new DispatcherFrame();
            var deadline = DateTime.UtcNow.AddSeconds(3);
            DateTime? firstDialogAt = null;
            var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(30), DispatcherPriority.Background, (_, _) =>
            {
                var dialog = System.Windows.Application.Current.Windows.OfType<FolderCategoriesWindow>().FirstOrDefault();
                if (dialog?.DataContext is FolderCategoriesViewModel categories)
                {
                    firstDialogAt ??= DateTime.UtcNow;
                    if (DateTime.UtcNow - firstDialogAt < TimeSpan.FromMilliseconds(200)) return;
                    opened = dialog.IsVisible && categories.FolderPath == folder &&
                             System.Windows.Application.Current.Windows.OfType<FolderCategoriesWindow>().Count() == 1;
                    foreach (var categoryWindow in System.Windows.Application.Current.Windows.OfType<FolderCategoriesWindow>().ToArray())
                        categoryWindow.Close();
                    window.RequestExplicitExit();
                    frame.Continue = false;
                }
                else if (DateTime.UtcNow >= deadline) frame.Continue = false;
            }, window.Dispatcher);
            timer.Start();
            _ = window.OpenFolderCategoryEditorAsync(folder);
            _ = window.OpenFolderCategoryEditorAsync(folder);
            Dispatcher.PushFrame(frame);
            timer.Stop();
        });

        Assert.True(opened);
    }

    private sealed class RecordingFolderHoverMonitor : IFolderHoverMonitorService
    {
        public int StartCount { get; private set; }
        public int DisposeCount { get; private set; }
        public Task StartAsync(CancellationToken token = default) { StartCount++; return Task.CompletedTask; }
        public Task ReloadCatalogAsync(CancellationToken token = default) => Task.CompletedTask;
        public void Dispose() => DisposeCount++;
    }
}
