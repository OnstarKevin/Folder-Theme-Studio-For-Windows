using System.IO;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class FolderCategoryCatalogTests
{
    [Fact]
    public async Task LoadAsync_MissingCatalogReturnsEmpty()
    {
        using var temp = new TemporaryDirectory();
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "categories.json"));

        Assert.Empty(await catalog.LoadAsync());
    }

    [Fact]
    public async Task UpsertAsync_ReloadsCanonicalFolderEntryWithoutWritingIntoFolder()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "Research");
        Directory.CreateDirectory(folder);
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "categories.json"));
        var entry = new FolderCategoryEntry(folder, "Work", "Keep active projects together", "#2868D7");

        await catalog.UpsertAsync(entry);

        var saved = Assert.Single(await catalog.LoadAsync());
        Assert.Equal(Path.GetFullPath(folder), saved.FolderPath);
        Assert.Equal("Work", saved.Category);
        Assert.Equal(["categories.json"], Directory.GetFiles(temp.Path).Select(Path.GetFileName));
        Assert.Empty(Directory.GetFiles(folder));
    }

    [Fact]
    public async Task UpsertAsync_CaseInsensitivePathReplacesExistingEntry()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "Projects");
        Directory.CreateDirectory(folder);
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "categories.json"));

        await catalog.UpsertAsync(new(folder, "Old", "", "#112233"));
        await catalog.UpsertAsync(new(folder.ToUpperInvariant(), "New", "Updated", "#445566"));

        var saved = Assert.Single(await catalog.LoadAsync());
        Assert.Equal("New", saved.Category);
        Assert.Equal("Updated", saved.Note);
    }

    [Fact]
    public async Task RemoveAsync_RemovesOnlyMatchingFolder()
    {
        using var temp = new TemporaryDirectory();
        var first = Path.Combine(temp.Path, "first");
        var second = Path.Combine(temp.Path, "second");
        Directory.CreateDirectory(first);
        Directory.CreateDirectory(second);
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "categories.json"));
        await catalog.UpsertAsync(new(first, "One", "", "#112233"));
        await catalog.UpsertAsync(new(second, "Two", "", "#445566"));

        await catalog.RemoveAsync(first);

        var remaining = Assert.Single(await catalog.LoadAsync());
        Assert.Equal(Path.GetFullPath(second), remaining.FolderPath);
    }

    [Theory]
    [InlineData("", "note", "#112233")]
    [InlineData("   ", "note", "#112233")]
    [InlineData("Category name that is much too long to be useful", "note", "#112233")]
    [InlineData("Work", "note text that is intentionally far too long to fit comfortably in a small folder hover card and so it should be rejected before saving123456", "#112233")]
    [InlineData("Work", "note", "blue")]
    [InlineData("Work", "note", "#12345")]
    public async Task UpsertAsync_RejectsInvalidEntryWithoutChangingCatalog(string category, string note, string color)
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "folder");
        Directory.CreateDirectory(folder);
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "categories.json"));
        var existing = new FolderCategoryEntry(folder, "Existing", "unchanged", "#334455");
        await catalog.UpsertAsync(existing);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            catalog.UpsertAsync(new FolderCategoryEntry(folder, category, note, color)));

        Assert.Equal(existing, Assert.Single(await catalog.LoadAsync()));
    }

    [Fact]
    public async Task UpsertAsync_RejectsMissingDirectory()
    {
        using var temp = new TemporaryDirectory();
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "categories.json"));

        await Assert.ThrowsAsync<DirectoryNotFoundException>(() =>
            catalog.UpsertAsync(new(Path.Combine(temp.Path, "missing"), "Archive", "", "#334455")));
    }

    [Fact]
    public async Task LoadAsync_MalformedCatalogThrowsAndLeavesBytesUntouched()
    {
        using var temp = new TemporaryDirectory();
        var path = Path.Combine(temp.Path, "categories.json");
        var malformed = "{ definitely not json"u8.ToArray();
        await File.WriteAllBytesAsync(path, malformed);
        var catalog = new JsonFolderCategoryCatalog(path);

        await Assert.ThrowsAsync<InvalidDataException>(() => catalog.LoadAsync());

        Assert.Equal(malformed, await File.ReadAllBytesAsync(path));
    }

    [Fact]
    public async Task LoadAsync_MigratesSingleColorRecordWithoutLosingNote()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "Archive");
        Directory.CreateDirectory(folder);
        var path = Path.Combine(temp.Path, "categories.json");
        var json = System.Text.Json.JsonSerializer.Serialize(new { schemaVersion = 1, entries = new[] { new { folderPath = folder, category = "Old", note = "keep me", colorHex = "#2868D7" } } });
        await File.WriteAllTextAsync(path, json);

        var entry = Assert.Single(await new JsonFolderCategoryCatalog(path).LoadAsync());

        Assert.Equal("keep me", entry.Note);
        Assert.Equal("#2868D7", entry.GradientStartHex);
        Assert.Equal("#2868D7", entry.GradientEndHex);
    }

    [Fact]
    public async Task UpsertAsync_RoundTripsTwoDifferentAccentColors()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "Archive");
        Directory.CreateDirectory(folder);
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "categories.json"));

        await catalog.UpsertAsync(new FolderCategoryEntry(folder, "Work", "note", "#112233", "#AABBCC"));

        var entry = Assert.Single(await catalog.LoadAsync());
        Assert.Equal("#112233", entry.GradientStartHex);
        Assert.Equal("#AABBCC", entry.GradientEndHex);
    }
}
