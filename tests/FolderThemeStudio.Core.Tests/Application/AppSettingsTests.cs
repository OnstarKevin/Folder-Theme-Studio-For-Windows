using System.IO;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class AppSettingsTests
{
    [Fact]
    public async Task LoadAsync_MissingFile_ReturnsChineseDefaults()
    {
        using var directory = new TemporaryDirectory();
        var store = new JsonAppSettingsStore(Path.Combine(directory.Path, "settings.json"));

        var settings = await store.LoadAsync();

        Assert.Equal("zh-CN", settings.Language);
        Assert.False(settings.TutorialCompleted);
        Assert.Equal("#65D9FF", settings.Palette.GradientStart);
        Assert.Empty(settings.RecentColors);
        Assert.True(settings.StartWithWindows);
        Assert.True(settings.CloseToTray);
        Assert.False(settings.MonitoringPaused);
    }

    [Fact]
    public async Task SaveAsync_ThenLoadAsync_NormalizesAndLimitsRecentColors()
    {
        using var directory = new TemporaryDirectory();
        var store = new JsonAppSettingsStore(Path.Combine(directory.Path, "settings.json"));
        var recent = Enumerable.Range(0, 14).Select(index => $"#{index:X6}").ToArray();
        var input = new AppSettings(
            "en-US",
            true,
            new FolderPalette("#abcdef", "#123456", "#a1b2c3", "#fedcba"),
            recent);

        await store.SaveAsync(input);
        var loaded = await store.LoadAsync();

        Assert.Equal("en-US", loaded.Language);
        Assert.True(loaded.TutorialCompleted);
        Assert.Equal(new FolderPalette("#ABCDEF", "#123456", "#A1B2C3", "#FEDCBA"), loaded.Palette);
        Assert.Equal(12, loaded.RecentColors.Count);
        Assert.Equal("#000000", loaded.RecentColors[0]);
        Assert.True(loaded.StartWithWindows);
        Assert.True(loaded.CloseToTray);
    }

    [Fact]
    public async Task LoadAsync_CorruptOrUnsupportedSettings_ReturnsDefaults()
    {
        using var directory = new TemporaryDirectory();
        var path = Path.Combine(directory.Path, "settings.json");
        await File.WriteAllTextAsync(path, "{not json");
        var store = new JsonAppSettingsStore(path);

        Assert.Equal(AppSettings.Default, await store.LoadAsync());

        await File.WriteAllTextAsync(path, """
            {"language":"fr-FR","tutorialCompleted":true,"palette":{"gradientStart":"red","gradientEnd":"#123456","stroke":"#123456","glow":"#123456"},"recentColors":[]}
            """);
        Assert.Equal(AppSettings.Default, await store.LoadAsync());
    }

    [Fact]
    public async Task LoadAsync_OlderSettings_EnablesBackgroundDefaults()
    {
        using var directory = new TemporaryDirectory();
        var path = Path.Combine(directory.Path, "settings.json");
        await File.WriteAllTextAsync(path, """
            {"language":"zh-CN","tutorialCompleted":true,"palette":{"gradientStart":"#112233","gradientEnd":"#223344","stroke":"#334455","glow":"#445566"},"recentColors":[]}
            """);

        var settings = await new JsonAppSettingsStore(path).LoadAsync();

        Assert.True(settings.StartWithWindows);
        Assert.True(settings.CloseToTray);
        Assert.False(settings.MonitoringPaused);
    }
}
