using System.IO;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Tests.Rendering;
using FolderThemeStudio.Core.Tests.TestSupport;
using FolderThemeStudio.Core.Themes;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class AutoStyleRulesViewModelTests
{
    [Fact]
    public async Task SaveRuleAsync_PersistsRenderedThemeAndPreservesFallback()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow));
        var fixture = new MainViewModelFixture();
        using var monitor = new FolderMonitoringCoordinator(store, new RecordingApplier());
        var assets = new MonitoringIconAssetService(Path.Combine(temp.Path, "assets"));
        AutoStyleRulesViewModel? viewModel = null;

        WpfTestHost.Invoke(() =>
        {
            viewModel = new AutoStyleRulesViewModel(
                store, monitor, assets, fixture.Dialogs, new LocalizationService("zh-CN", false), new ImmediateViewModelDispatcher(), FolderTheme.IceBlue);
            viewModel.LoadAsync().GetAwaiter().GetResult();
            viewModel.SelectedRootPath = root;
            viewModel.Keyword = "Project";
            Assert.True(viewModel.SaveRuleAsync().GetAwaiter().GetResult());
        });

        var saved = Assert.Single((await store.LoadAsync()).Rules);
        Assert.Equal(@"C:\icons\fallback.ico", saved.IcoPath);
        var mapped = Assert.Single(saved.NameRules);
        Assert.Equal("Project", mapped.Keyword);
        Assert.True(File.Exists(mapped.IcoPath));
        Assert.Equal(["Project"], viewModel!.Rules.Select(rule => rule.Keyword));
    }

    [Fact]
    public async Task SaveRuleAsync_ImportedImageBecomesDurableIco()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var imagePath = TestImageFiles.Write("custom.png", 20, 20, temp.Path);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow));
        var fixture = new MainViewModelFixture();
        fixture.Dialogs.ImageImportPath = imagePath;
        using var monitor = new FolderMonitoringCoordinator(store, new RecordingApplier());
        var assets = new MonitoringIconAssetService(Path.Combine(temp.Path, "assets"));

        WpfTestHost.Invoke(() =>
        {
            var viewModel = new AutoStyleRulesViewModel(
                store, monitor, assets, fixture.Dialogs, new LocalizationService("en-US", false), new ImmediateViewModelDispatcher(), FolderTheme.IceBlue);
            viewModel.LoadAsync().GetAwaiter().GetResult();
            viewModel.SelectedRootPath = root;
            viewModel.Keyword = "Graphics";
            viewModel.UseEditorTheme = false;
            viewModel.ChooseImageAsync().GetAwaiter().GetResult();
            Assert.True(viewModel.SaveRuleAsync().GetAwaiter().GetResult());
        });

        var iconPath = Assert.Single(Assert.Single((await store.LoadAsync()).Rules).NameRules).IcoPath;
        var bytes = await File.ReadAllBytesAsync(iconPath);
        Assert.Equal(new byte[] { 0, 0, 1, 0 }, bytes[..4]);
        Assert.Equal(FolderIconRenderer.RequiredSizes.Length, BitConverter.ToUInt16(bytes, 4));
    }

    [Fact]
    public async Task SaveRuleAsync_EditingTextReusesOriginalMultiframeIco()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var icoPath = TestImageFiles.WriteIco("high-resolution.ico", temp.Path);
        var originalBytes = await File.ReadAllBytesAsync(icoPath);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new MonitoringRule(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow)
        {
            NameRules = [new("project", "Project", icoPath, true, 0)]
        });
        var fixture = new MainViewModelFixture();
        using var monitor = new FolderMonitoringCoordinator(store, new RecordingApplier());
        var viewModel = new AutoStyleRulesViewModel(store, monitor, new MonitoringIconAssetService(Path.Combine(temp.Path, "assets")),
            fixture.Dialogs, new LocalizationService("en-US", false), new ImmediateViewModelDispatcher(), FolderTheme.IceBlue);

        WpfTestHost.Invoke(() =>
        {
            viewModel.LoadAsync().GetAwaiter().GetResult();
            viewModel.SelectedRootPath = root;
            viewModel.SelectedRule = Assert.Single(viewModel.Rules);
            viewModel.Keyword = "Project Archive";
            Assert.True(viewModel.SaveRuleAsync().GetAwaiter().GetResult());
        });

        var saved = Assert.Single(Assert.Single((await store.LoadAsync()).Rules).NameRules);
        Assert.Equal(icoPath, saved.IcoPath);
        Assert.Equal(originalBytes, await File.ReadAllBytesAsync(icoPath));
    }

    [Fact]
    public async Task SaveRuleAsync_RootChangeDuringAssetPersistenceKeepsOriginalTarget()
    {
        using var temp = new TemporaryDirectory();
        var rootA = Path.Combine(temp.Path, "root-a");
        var rootB = Path.Combine(temp.Path, "root-b");
        Directory.CreateDirectory(rootA);
        Directory.CreateDirectory(rootB);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new MonitoringRule(1, rootA, @"C:\icons\a.ico", DateTimeOffset.UtcNow));
        await store.UpsertAsync(new MonitoringRule(1, rootB, @"C:\icons\b.ico", DateTimeOffset.UtcNow)
        {
            NameRules = [new("keep", "Keep", @"C:\icons\keep.ico", true, 0)]
        });
        var fixture = new MainViewModelFixture();
        var delayedAssets = new DeferredMonitoringIconAssetService(Path.Combine(temp.Path, "persisted.ico"));
        using var monitor = new FolderMonitoringCoordinator(store, new RecordingApplier());
        AutoStyleRulesViewModel? viewModel = null;
        Task<bool>? save = null;
        WpfTestHost.Invoke(() =>
        {
            viewModel = new AutoStyleRulesViewModel(store, monitor, delayedAssets, fixture.Dialogs,
                new LocalizationService("en-US", false), new ImmediateViewModelDispatcher(), FolderTheme.IceBlue);
            viewModel.LoadAsync().GetAwaiter().GetResult();
            viewModel.SelectedRootPath = rootA;
            viewModel.Keyword = "new";
            save = viewModel.SaveRuleAsync();
        });
        await delayedAssets.Started.Task.WaitAsync(TimeSpan.FromSeconds(5));
        WpfTestHost.Invoke(() =>
        {
            Assert.False(viewModel!.CanEdit);
            viewModel.SelectedRootPath = rootB;
        });
        delayedAssets.Release();
        Assert.True(await save!);
        Assert.True(viewModel!.CanEdit);

        var savedRoots = (await store.LoadAsync()).Rules.ToDictionary(rule => rule.RootPath);
        Assert.Equal("new", Assert.Single(savedRoots[rootA].NameRules).Keyword);
        Assert.Equal("keep", Assert.Single(savedRoots[rootB].NameRules).Id);
    }

    [Fact]
    public async Task MoveAndDeleteRuleAsync_PersistOrderedRuleList()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new MonitoringRule(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow)
        {
            NameRules =
            [
                new("archive", "Archive", @"C:\icons\archive.ico", true, 0),
                new("project", "Project", @"C:\icons\project.ico", true, 1)
            ]
        });
        var fixture = new MainViewModelFixture();
        using var monitor = new FolderMonitoringCoordinator(store, new RecordingApplier());
        var viewModel = new AutoStyleRulesViewModel(
            store, monitor, new MonitoringIconAssetService(Path.Combine(temp.Path, "assets")),
            fixture.Dialogs, new LocalizationService("zh-CN", false), new ImmediateViewModelDispatcher(), FolderTheme.IceBlue);

        WpfTestHost.Invoke(() =>
        {
            viewModel.LoadAsync().GetAwaiter().GetResult();
            viewModel.SelectedRootPath = root;
            viewModel.MoveRuleAsync("project", -1).GetAwaiter().GetResult();
        });
        var ordered = Assert.Single((await store.LoadAsync()).Rules).NameRules;
        Assert.Equal(["project", "archive"], ordered.Select(rule => rule.Id));

        WpfTestHost.Invoke(() => viewModel.DeleteRuleAsync("project").GetAwaiter().GetResult());
        Assert.Equal("archive", Assert.Single(Assert.Single((await store.LoadAsync()).Rules).NameRules).Id);
    }

    [Fact]
    public async Task SaveRuleAsync_RejectsBlankKeywordWithoutChangingStoredMappings()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new MonitoringRule(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow)
        {
            NameRules = [new("existing", "Existing", @"C:\icons\existing.ico", true, 0)]
        });
        var fixture = new MainViewModelFixture();
        using var monitor = new FolderMonitoringCoordinator(store, new RecordingApplier());
        var viewModel = new AutoStyleRulesViewModel(
            store, monitor, new MonitoringIconAssetService(Path.Combine(temp.Path, "assets")),
            fixture.Dialogs, new LocalizationService("zh-CN", false), new ImmediateViewModelDispatcher(), FolderTheme.IceBlue);

        WpfTestHost.Invoke(() =>
        {
            viewModel.LoadAsync().GetAwaiter().GetResult();
            viewModel.SelectedRootPath = root;
            viewModel.BeginNewRule();
            viewModel.Keyword = "  ";
            Assert.False(viewModel.SaveRuleAsync().GetAwaiter().GetResult());
        });

        Assert.Equal("Existing", Assert.Single(Assert.Single((await store.LoadAsync()).Rules).NameRules).Keyword);
    }

    [Fact]
    public async Task CancelEdit_RestoresSelectedRuleValues()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new MonitoringRule(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow)
        {
            NameRules = [new("project", "Project", @"C:\icons\project.ico", true, 0)]
        });
        var fixture = new MainViewModelFixture();
        using var monitor = new FolderMonitoringCoordinator(store, new RecordingApplier());
        var viewModel = new AutoStyleRulesViewModel(
            store, monitor, new MonitoringIconAssetService(Path.Combine(temp.Path, "assets")),
            fixture.Dialogs, new LocalizationService("zh-CN", false), new ImmediateViewModelDispatcher(), FolderTheme.IceBlue);

        WpfTestHost.Invoke(() =>
        {
            viewModel.LoadAsync().GetAwaiter().GetResult();
            viewModel.SelectedRootPath = root;
            viewModel.SelectedRule = Assert.Single(viewModel.Rules);
            viewModel.Keyword = "Changed";
            viewModel.Enabled = false;
            viewModel.CancelEdit();
        });

        Assert.Equal("Project", viewModel.Keyword);
        Assert.True(viewModel.Enabled);
    }

    private sealed class RecordingApplier : FolderThemeStudio.Core.SystemIntegration.IMonitoredFolderIconService
    {
        public Task<FolderThemeStudio.Core.SystemIntegration.MonitoredApplyOutcome> ApplyAsync(
            string folderPath, string icoPath, CancellationToken token) =>
            Task.FromResult(FolderThemeStudio.Core.SystemIntegration.MonitoredApplyOutcome.Succeeded(folderPath));
    }

    private sealed class DeferredMonitoringIconAssetService(string durablePath) : IMonitoringIconAssetService
    {
        internal TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource release = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public async Task<string> PersistAsync(string generatedIcoPath, CancellationToken token)
        {
            Started.TrySetResult();
            await release.Task.WaitAsync(token);
            return durablePath;
        }

        internal void Release() => release.TrySetResult();
    }
}
