using System.Collections.Concurrent;
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
    public async Task CreatedFolder_UsesFirstEnabledCaseInsensitiveMatchAndFallback()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        var rootRule = new MonitoringRule(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow)
        {
            NameRules =
            [
                new("disabled", "Project", @"C:\icons\disabled.ico", false, 0),
                new("project", "Project", @"C:\icons\project.ico", true, 1),
                new("archive", "Archive", @"C:\icons\archive.ico", true, 2)
            ]
        };
        await store.UpsertAsync(rootRule);
        var watcherFactory = new RecordingWatcherFactory();
        var applier = new RecordingMonitoredApplier();
        using var coordinator = new FolderMonitoringCoordinator(store, applier, watcherFactory, (_, _) => Task.CompletedTask);
        await coordinator.StartAsync();

        var matched = Path.Combine(root, "nested", "PROJECT Archive");
        var unmatched = Path.Combine(root, "other", "miscellaneous");
        watcherFactory.Single.Emit(matched);
        watcherFactory.Single.Emit(unmatched);
        await coordinator.WaitForIdleAsync();

        var byFolderName = applier.Calls.ToDictionary(call => Path.GetFileName(call.Path), call => call.Ico, StringComparer.OrdinalIgnoreCase);
        Assert.Equal(@"C:\icons\project.ico", byFolderName["PROJECT Archive"]);
        Assert.Equal(@"C:\icons\fallback.ico", byFolderName["miscellaneous"]);
    }

    [Fact]
    public async Task CreatedFolders_UseMappingsFromTheirOwnMonitoredRoot()
    {
        using var temp = new TemporaryDirectory();
        var firstRoot = Path.Combine(temp.Path, "first");
        var secondRoot = Path.Combine(temp.Path, "second");
        Directory.CreateDirectory(firstRoot);
        Directory.CreateDirectory(secondRoot);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new MonitoringRule(1, firstRoot, @"C:\icons\fallback-one.ico", DateTimeOffset.UtcNow)
        {
            NameRules = [new("team-one", "Team", @"C:\icons\team-one.ico", true, 0)]
        });
        await store.UpsertAsync(new MonitoringRule(1, secondRoot, @"C:\icons\fallback-two.ico", DateTimeOffset.UtcNow)
        {
            NameRules = [new("team-two", "Team", @"C:\icons\team-two.ico", true, 0)]
        });
        var watcherFactory = new RecordingWatcherFactory();
        var applier = new RecordingMonitoredApplier();
        using var coordinator = new FolderMonitoringCoordinator(store, applier, watcherFactory, (_, _) => Task.CompletedTask);
        await coordinator.StartAsync();

        foreach (var watcher in watcherFactory.Created)
        {
            watcher.Emit(Path.Combine(watcher.RootPath, "Team shared"));
            await coordinator.WaitForIdleAsync();
        }

        var byRootName = applier.Calls.ToDictionary(call => Path.GetFileName(Path.GetDirectoryName(call.Path)!), call => call.Ico, StringComparer.OrdinalIgnoreCase);
        Assert.Equal(@"C:\icons\team-one.ico", byRootName["first"]);
        Assert.Equal(@"C:\icons\team-two.ico", byRootName["second"]);
    }

    [Fact]
    public async Task RenamedFolder_UsesRootFallbackInsteadOfNameRule()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new MonitoringRule(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow)
        {
            NameRules = [new("project", "Project", @"C:\icons\project.ico", true, 0)]
        });
        var watcherFactory = new RecordingWatcherFactory();
        var applier = new RecordingMonitoredApplier();
        using var coordinator = new FolderMonitoringCoordinator(store, applier, watcherFactory, (_, _) => Task.CompletedTask);
        await coordinator.StartAsync();

        watcherFactory.Single.Emit(Path.Combine(root, "Project Folder"), wasRenamed: true);
        await coordinator.WaitForIdleAsync();

        Assert.Equal(@"C:\icons\fallback.ico", Assert.Single(applier.Calls).Ico);
    }

    [Fact]
    public async Task DirectoryWatcher_ReportsNewAndRenamedNestedFoldersWithTheirEventKind()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        var nested = Path.Combine(root, "nested");
        Directory.CreateDirectory(nested);
        using var watcher = new DirectoryWatcherFactory().Create(root);
        var created = new TaskCompletionSource<DirectoryCreatedEventArgs>(TaskCreationOptions.RunContinuationsAsynchronously);
        var renamed = new TaskCompletionSource<DirectoryCreatedEventArgs>(TaskCreationOptions.RunContinuationsAsynchronously);
        watcher.DirectoryCreated += (_, args) =>
        {
            if (Path.GetFileName(args.Path) == "Project Created") created.TrySetResult(args);
            if (Path.GetFileName(args.Path) == "Project Renamed") renamed.TrySetResult(args);
        };

        var original = Path.Combine(nested, "Project Created");
        Directory.CreateDirectory(original);
        var createdEvent = await created.Task.WaitAsync(TimeSpan.FromSeconds(5));
        Directory.Move(original, Path.Combine(nested, "Project Renamed"));
        var renamedEvent = await renamed.Task.WaitAsync(TimeSpan.FromSeconds(5));

        Assert.False(createdEvent.WasRenamed);
        Assert.True(renamedEvent.WasRenamed);
    }

    [Fact]
    public async Task ReplaceFallbackIconAsync_PreservesNameRulesEvenBeforeCoordinatorStart()
    {
        using var temp = new TemporaryDirectory();
        var root = Path.Combine(temp.Path, "root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        await store.UpsertAsync(new MonitoringRule(1, root, @"C:\icons\old.ico", DateTimeOffset.UtcNow)
        {
            NameRules = [new("project", "Project", @"C:\icons\project.ico", true, 0)]
        });
        using var coordinator = new FolderMonitoringCoordinator(store, new RecordingMonitoredApplier());

        await coordinator.ReplaceFallbackIconAsync(root, @"C:\icons\new.ico");

        var updated = Assert.Single((await store.LoadAsync()).Rules);
        Assert.Equal(@"C:\icons\new.ico", updated.IcoPath);
        Assert.Equal("project", Assert.Single(updated.NameRules).Id);
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
        internal ConcurrentQueue<(string Path, string Ico)> Calls { get; } = new();
        public Task<MonitoredApplyOutcome> ApplyAsync(string folderPath, string icoPath, CancellationToken token)
        {
            Calls.Enqueue((folderPath, icoPath));
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
        internal void Emit(string path, bool wasRenamed = false) => DirectoryCreated?.Invoke(this, new(path, wasRenamed));
        public void Dispose() { }
    }
}
