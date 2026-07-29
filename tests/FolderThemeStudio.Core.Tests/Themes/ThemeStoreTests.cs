using System.Text.Json;
using FolderThemeStudio.Core.Tests.TestSupport;
using FolderThemeStudio.Core.Themes;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Themes;

public sealed class ThemeStoreTests : IDisposable
{
    private readonly TemporaryDirectory temp = new();

    [Fact]
    public async Task SaveThenLoad_RoundTripsTheme()
    {
        var store = new ThemeStore(temp.Path);
        await store.SaveAsync(FolderTheme.IceBlue with { Id = Guid.NewGuid(), Name = "Mine" });

        var loaded = await store.LoadAsync();

        Assert.Equal("Mine", loaded.Themes.Single(x => x.Name == "Mine").Name);
        Assert.Equal(FolderTheme.IceBlue, loaded.Themes[0]);
        Assert.Empty(loaded.Diagnostics);
    }

    [Fact]
    public async Task Import_InvalidTheme_ReturnsValidationErrorWithoutWritingTheme()
    {
        var importPath = System.IO.Path.Combine(temp.Path, "invalid.json");
        var invalidTheme = FolderTheme.IceBlue with { Id = Guid.NewGuid(), Opacity = 1.01 };
        await System.IO.File.WriteAllTextAsync(importPath, JsonSerializer.Serialize(invalidTheme));
        var store = new ThemeStore(System.IO.Path.Combine(temp.Path, "themes"));

        var result = await store.ImportAsync(importPath);

        Assert.False(result.Success);
        Assert.Contains(nameof(FolderTheme.Opacity), result.Errors);
        Assert.Null(result.Theme);
        Assert.Single((await store.LoadAsync()).Themes);
    }

    [Fact]
    public async Task DeleteBuiltIn_IsRejected()
    {
        var store = new ThemeStore(temp.Path);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            store.DeleteAsync(FolderTheme.IceBlue.Id));
    }

    [Fact]
    public async Task Import_IdCollision_AssignsNewId()
    {
        var store = new ThemeStore(temp.Path);
        var original = FolderTheme.IceBlue with { Id = Guid.NewGuid(), Name = "Original" };
        await store.SaveAsync(original);
        var importPath = System.IO.Path.Combine(temp.Path, "collision.json");
        await System.IO.File.WriteAllTextAsync(importPath, JsonSerializer.Serialize(original with { Name = "Imported" }));

        var result = await store.ImportAsync(importPath);

        Assert.True(result.Success);
        Assert.NotNull(result.Theme);
        Assert.NotEqual(original.Id, result.Theme!.Id);
        Assert.Contains((await store.LoadAsync()).Themes, theme => theme.Name == "Imported");
    }

    [Fact]
    public async Task Load_CorruptUserTheme_SkipsFileAndReturnsDiagnostic()
    {
        await System.IO.File.WriteAllTextAsync(System.IO.Path.Combine(temp.Path, "broken.json"), "not json");
        var store = new ThemeStore(temp.Path);

        var result = await store.LoadAsync();

        Assert.Equal([FolderTheme.IceBlue], result.Themes);
        Assert.Single(result.Diagnostics);
    }

    [Fact]
    public async Task Load_CorruptThemeDoesNotPreventLaterValidThemeFromLoading()
    {
        await System.IO.File.WriteAllTextAsync(System.IO.Path.Combine(temp.Path, "01-broken.json"), "not json");
        var validTheme = FolderTheme.IceBlue with { Id = Guid.NewGuid(), Name = "Valid after corrupt" };
        await System.IO.File.WriteAllTextAsync(
            System.IO.Path.Combine(temp.Path, "02-valid.json"),
            JsonSerializer.Serialize(validTheme));
        var store = new ThemeStore(temp.Path);

        var result = await store.LoadAsync();

        Assert.Equal([FolderTheme.IceBlue, validTheme], result.Themes);
        Assert.Single(result.Diagnostics);
    }

    [Fact]
    public async Task ExportThenImport_PreservesTheme()
    {
        var source = new ThemeStore(System.IO.Path.Combine(temp.Path, "source"));
        var theme = FolderTheme.IceBlue with { Id = Guid.NewGuid(), Name = "To export" };
        await source.SaveAsync(theme);
        var exportedPath = System.IO.Path.Combine(temp.Path, "exported.json");

        await source.ExportAsync(theme.Id, exportedPath);
        var result = await new ThemeStore(System.IO.Path.Combine(temp.Path, "destination")).ImportAsync(exportedPath);

        Assert.True(result.Success);
        Assert.Equal(theme, result.Theme);
    }

    public void Dispose() => temp.Dispose();
}
