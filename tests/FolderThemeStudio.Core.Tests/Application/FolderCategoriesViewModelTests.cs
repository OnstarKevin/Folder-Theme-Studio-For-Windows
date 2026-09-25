using System.IO;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class FolderCategoriesViewModelTests
{
    [Fact]
    public async Task PickFolderAsync_LoadsExistingCategoryForEditing()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "Research");
        Directory.CreateDirectory(folder);
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "catalog.json"));
        await catalog.UpsertAsync(new(folder, "Work", "Active project", "#2868D7"));
        var fixture = new MainViewModelFixture();
        fixture.Dialogs.FolderPathToPick = folder;
        using var viewModel = CreateViewModel(catalog, fixture.Dialogs, new RecordingHoverMonitor());
        await viewModel.LoadAsync();

        await viewModel.PickFolderAsync();

        Assert.Equal(Path.GetFullPath(folder), viewModel.FolderPath);
        Assert.Equal("Work", viewModel.Category);
        Assert.Equal("Active project", viewModel.Note);
        Assert.Equal("#2868D7", viewModel.ColorHex);
    }

    [Fact]
    public async Task SaveAsync_PersistsEditedEntryAndRefreshesHoverMonitor()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "Archive");
        Directory.CreateDirectory(folder);
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "catalog.json"));
        var fixture = new MainViewModelFixture();
        fixture.Dialogs.FolderPathToPick = folder;
        var monitor = new RecordingHoverMonitor();
        using var viewModel = CreateViewModel(catalog, fixture.Dialogs, monitor);
        await viewModel.PickFolderAsync();
        viewModel.Category = "Reference";
        viewModel.Note = "Keep for later";
        viewModel.ColorHex = "#3A6F91";

        Assert.True(await viewModel.SaveAsync());

        var saved = Assert.Single(await catalog.LoadAsync());
        Assert.Equal("Reference", saved.Category);
        Assert.Equal("Keep for later", saved.Note);
        Assert.Equal("#3A6F91", saved.ColorHex);
        Assert.Equal(1, monitor.ReloadCount);
    }

    [Fact]
    public async Task RemoveAsync_DeletesEntryAndRefreshesHoverMonitor()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "Archive");
        Directory.CreateDirectory(folder);
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "catalog.json"));
        await catalog.UpsertAsync(new(folder, "Reference", "", "#3A6F91"));
        var fixture = new MainViewModelFixture();
        var monitor = new RecordingHoverMonitor();
        using var viewModel = CreateViewModel(catalog, fixture.Dialogs, monitor);
        await viewModel.LoadAsync();
        viewModel.SelectedEntry = Assert.Single(viewModel.Entries);

        await viewModel.RemoveAsync();

        Assert.Empty(await catalog.LoadAsync());
        Assert.Equal(1, monitor.ReloadCount);
        Assert.Null(viewModel.SelectedEntry);
    }

    [Fact]
    public async Task SelectFolder_LoadsExistingGradientForQuickEditing()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "Research");
        Directory.CreateDirectory(folder);
        var catalog = new JsonFolderCategoryCatalog(Path.Combine(temp.Path, "catalog.json"));
        await catalog.UpsertAsync(new FolderCategoryEntry(folder, "Work", "keep", "#112233", "#AABBCC"));
        var fixture = new MainViewModelFixture();
        using var viewModel = CreateViewModel(catalog, fixture.Dialogs, new RecordingHoverMonitor());
        await viewModel.LoadAsync();

        viewModel.SelectFolder(folder);

        Assert.Equal("Work", viewModel.Category);
        Assert.Equal("keep", viewModel.Note);
        Assert.Equal("#112233", viewModel.GradientStartPicker.Hex);
        Assert.Equal("#AABBCC", viewModel.GradientEndPicker.Hex);
    }

    private static FolderCategoriesViewModel CreateViewModel(
        IFolderCategoryCatalog catalog,
        IMainViewModelDialogs dialogs,
        IFolderHoverMonitorService monitor) =>
        new(catalog, monitor, dialogs, new LocalizationService("en-US", applyToApplication: false), new ImmediateViewModelDispatcher());

    private sealed class RecordingHoverMonitor : IFolderHoverMonitorService
    {
        internal int ReloadCount { get; private set; }
        public Task StartAsync(CancellationToken token = default) => Task.CompletedTask;
        public Task ReloadCatalogAsync(CancellationToken token = default) { ReloadCount++; return Task.CompletedTask; }
        public void Dispose() { }
    }
}
