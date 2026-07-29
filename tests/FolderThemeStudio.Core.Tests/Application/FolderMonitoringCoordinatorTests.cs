using System.IO;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class FolderMonitoringCoordinatorTests
{
    [Fact]
    public async Task Start_CreatesRecursiveWatcherAndDeduplicatesBurst()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new(1, root, @"C:\style.ico", DateTimeOffset.UtcNow));
        var watcherFactory = new RecordingWatcherFactory();
        var applier = new RecordingMonitoredApplier();
        using var coordinator = new FolderMonitoringCoordinator(store, applier, watcherFactory, (_, _) => Task.CompletedTask);

        await coordinator.StartAsync();
        var child = Path.Combine(root, "a", "b");
        watcherFactory.Single.Emit(child);
        watcherFactory.Single.Emit(child);
        await coordinator.WaitForIdleAsync();

        Assert.True(watcherFactory.Single.IncludeSubdirectories);
        Assert.Equal([(child, @"C:\style.ico")], applier.Calls);
        Assert.Equal(1, coordinator.Status.ActiveRules);
    }

    [Fact]
    public async Task PauseResumeAndReplaceRulePersistAcrossCoordinatorRestart()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        using (var first = new FolderMonitoringCoordinator(store, new RecordingMonitoredApplier(), new RecordingWatcherFactory(), (_, _) => Task.CompletedTask))
        {
            await first.ReplaceRuleAsync(new(1, root, @"C:\new.ico", DateTimeOffset.UtcNow));
            await first.PauseAsync();
        }

        var watchers = new RecordingWatcherFactory();
        using var second = new FolderMonitoringCoordinator(store, new RecordingMonitoredApplier(), watchers, (_, _) => Task.CompletedTask);
        await second.StartAsync();
        Assert.True(second.Status.Paused);
        Assert.Empty(watchers.Created);

        await second.ResumeAsync();
        Assert.Single(watchers.Created);
        Assert.False(second.Status.Paused);
    }

    private sealed class RecordingMonitoredApplier : IMonitoredFolderIconService
    {
        internal List<(string Path, string Ico)> Calls { get; } = [];
        public Task<MonitoredApplyOutcome> ApplyAsync(string folderPath, string icoPath, CancellationToken token)
        {
            Calls.Add((folderPath, icoPath));
            return Task.FromResult(MonitoredApplyOutcome.Succeeded(folderPath));
        }
    }

    private sealed class RecordingWatcherFactory : IDirectoryWatcherFactory
    {
        internal List<RecordingWatcher> Created { get; } = [];
        internal RecordingWatcher Single => Assert.Single(Created);
        public IDirectoryWatcher Create(string rootPath)
        {
            var watcher = new RecordingWatcher(rootPath);
            Created.Add(watcher);
            return watcher;
        }
    }

    private sealed class RecordingWatcher(string rootPath) : IDirectoryWatcher
    {
        public string RootPath { get; } = rootPath;
        public bool IncludeSubdirectories => true;
        public event EventHandler<DirectoryCreatedEventArgs>? DirectoryCreated;
        internal void Emit(string path) => DirectoryCreated?.Invoke(this, new(path));
        public void Dispose() { }
    }
}
