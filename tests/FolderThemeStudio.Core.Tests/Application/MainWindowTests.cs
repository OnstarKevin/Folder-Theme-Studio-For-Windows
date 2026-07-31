using System.IO;
using System.Windows;
using FolderThemeStudio.App;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class MainWindowTests
{
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
                Assert.Equal("使用教程", tutorial.Content);
                Assert.Equal("设置", settings.Content);
            });
            WpfTestHost.Invoke(() => localization!.ApplyLanguage("en-US"));
            WpfTestHost.Invoke(() =>
            {
                var tutorial = Assert.IsType<System.Windows.Controls.Button>(window!.FindName("TutorialButton"));
                var settings = Assert.IsType<System.Windows.Controls.Button>(window.FindName("SettingsButton"));
                Assert.Equal("_Tutorial", tutorial.Content);
                Assert.Equal("_Settings", settings.Content);
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
}
