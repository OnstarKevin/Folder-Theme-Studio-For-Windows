using System.IO;
using FolderThemeStudio.App;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class SettingsViewModelTests
{
    [Fact]
    public void SettingsWindow_ShowsWithLivePalettePreview()
    {
        WpfTestHost.Invoke(() =>
        {
            using var directory = new TemporaryDirectory();
            var fixture = new MainViewModelFixture();
            using var main = fixture.CreateViewModel();
            var viewModel = new SettingsViewModel(
                AppSettings.Default,
                new JsonAppSettingsStore(Path.Combine(directory.Path, "settings.json")),
                new LocalizationService(applyToApplication: false),
                main.ApplyPalette,
                fixture.Renderer);
            var window = new SettingsWindow(viewModel)
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
                Assert.NotNull(viewModel.PreviewImage);
            }
            finally
            {
                window.Close();
            }
        });
    }

    [Fact]
    public void Cancel_LeavesMainThemeAndSettingsFileUnchanged()
    {
        using var directory = new TemporaryDirectory();
        var path = Path.Combine(directory.Path, "settings.json");
        var store = new JsonAppSettingsStore(path);
        var fixture = new MainViewModelFixture();
        using var main = fixture.CreateViewModel();
        var original = main.CurrentTheme;
        var viewModel = new SettingsViewModel(
            AppSettings.Default, store, new LocalizationService(applyToApplication: false), main.ApplyPalette, fixture.Renderer);

        viewModel.GradientStart.Hex = "#112233";
        viewModel.Cancel();

        Assert.Equal(original, main.CurrentTheme);
        Assert.False(File.Exists(path));
    }

    [Fact]
    public async Task SaveAsync_PersistsAndAppliesAllFourPaletteColors()
    {
        using var directory = new TemporaryDirectory();
        var store = new JsonAppSettingsStore(Path.Combine(directory.Path, "settings.json"));
        var fixture = new MainViewModelFixture();
        using var main = fixture.CreateViewModel();
        var localization = new LocalizationService(applyToApplication: false);
        var viewModel = new SettingsViewModel(AppSettings.Default, store, localization, main.ApplyPalette, fixture.Renderer)
        {
            SelectedLanguage = "en-US",
            StartWithWindows = false,
            CloseToTray = false,
        };
        viewModel.GradientStart.Hex = "#112233";
        viewModel.GradientEnd.Hex = "#445566";
        viewModel.Stroke.Hex = "#778899";
        viewModel.Glow.Hex = "#AABBCC";

        Assert.True(await viewModel.SaveAsync());
        var saved = await store.LoadAsync();

        Assert.Equal("en-US", saved.Language);
        Assert.Equal(new FolderPalette("#112233", "#445566", "#778899", "#AABBCC"), saved.Palette);
        Assert.Equal("#112233", main.GradientStart);
        Assert.Equal("#445566", main.GradientEnd);
        Assert.Equal("#778899", main.StrokeColor);
        Assert.Equal("#AABBCC", main.GlowColor);
        Assert.Equal("en-US", localization.CurrentLanguage);
        Assert.Equal(4, saved.RecentColors.Count);
        Assert.False(saved.StartWithWindows);
        Assert.False(saved.CloseToTray);
    }

    [Fact]
    public async Task SaveAsync_InvalidHex_DoesNotPersistOrApply()
    {
        using var directory = new TemporaryDirectory();
        var path = Path.Combine(directory.Path, "settings.json");
        var fixture = new MainViewModelFixture();
        using var main = fixture.CreateViewModel();
        var original = main.CurrentTheme;
        var viewModel = new SettingsViewModel(
            AppSettings.Default,
            new JsonAppSettingsStore(path),
            new LocalizationService(applyToApplication: false),
            main.ApplyPalette,
            fixture.Renderer);
        viewModel.Glow.Hex = "invalid";

        Assert.False(viewModel.CanSave);
        Assert.False(await viewModel.SaveAsync());
        Assert.Equal(original, main.CurrentTheme);
        Assert.False(File.Exists(path));
    }
}
