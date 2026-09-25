using System.IO;
using FolderThemeStudio.Core.SystemIntegration;

namespace FolderThemeStudio.App.Monitoring;

public sealed record MonitoringStatus(bool Paused, int TotalRules, int ActiveRules, int InactiveRules);
public sealed record MonitoringOutcome(bool Success, string Path, string? Error);

public interface IFolderMonitoringCoordinator : IDisposable
{
    MonitoringStatus Status { get; }
    event EventHandler<MonitoringOutcome>? OutcomeProduced;
    Task StartAsync(CancellationToken token = default);
    Task PauseAsync(CancellationToken token = default);
    Task ResumeAsync(CancellationToken token = default);
    Task ReplaceRuleAsync(MonitoringRule rule, CancellationToken token = default);
    Task ReplaceFallbackIconAsync(string rootPath, string icoPath, CancellationToken token = default);
    Task RemoveRuleAsync(string rootPath, CancellationToken token = default);
}

public sealed class FolderMonitoringCoordinator : IFolderMonitoringCoordinator
{
    private static readonly TimeSpan[] RetryDelays =
        [TimeSpan.FromMilliseconds(250), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3)];
    private readonly IMonitoringRuleStore store;
    private readonly IMonitoredFolderIconService applier;
    private readonly IDirectoryWatcherFactory watcherFactory;
    private readonly Func<TimeSpan, CancellationToken, Task> delay;
    private readonly Dictionary<string, MonitoringRule> rules = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, IDirectoryWatcher> watchers = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> pending = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Task> work = [];
    private readonly object sync = new();
    private bool paused;
    private bool disposed;

    public FolderMonitoringCoordinator(
        IMonitoringRuleStore store,
        IMonitoredFolderIconService applier,
        IDirectoryWatcherFactory? watcherFactory = null,
        Func<TimeSpan, CancellationToken, Task>? delay = null)
    {
        this.store = store;
        this.applier = applier;
        this.watcherFactory = watcherFactory ?? new DirectoryWatcherFactory();
        this.delay = delay ?? Task.Delay;
    }

    public MonitoringStatus Status => new(paused, rules.Count, watchers.Count, rules.Count - watchers.Count);
    public event EventHandler<MonitoringOutcome>? OutcomeProduced;

    public async Task StartAsync(CancellationToken token = default)
    {
        var loaded = await store.LoadAsync(token).ConfigureAwait(false);
        rules.Clear();
        foreach (var rule in loaded.Rules) rules[rule.RootPath] = rule;
        paused = loaded.Paused;
        RebuildWatchers();
    }

    public async Task PauseAsync(CancellationToken token = default)
    {
        paused = true;
        await store.SetPausedAsync(true, token).ConfigureAwait(false);
        ClearWatchers();
    }

    public async Task ResumeAsync(CancellationToken token = default)
    {
        paused = false;
        await store.SetPausedAsync(false, token).ConfigureAwait(false);
        RebuildWatchers();
    }

    public async Task ReplaceRuleAsync(MonitoringRule rule, CancellationToken token = default)
    {
        await store.UpsertAsync(rule, token).ConfigureAwait(false);
        rules[Path.TrimEndingDirectorySeparator(Path.GetFullPath(rule.RootPath))] = rule;
        RebuildWatchers();
    }

    public async Task ReplaceFallbackIconAsync(string rootPath, string icoPath, CancellationToken token = default)
    {
        var canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
        MonitoringRule? current = rules.GetValueOrDefault(canonical);
        if (current is null)
        {
            var loaded = await store.LoadAsync(token).ConfigureAwait(false);
            current = loaded.Rules.FirstOrDefault(item =>
                string.Equals(Path.TrimEndingDirectorySeparator(Path.GetFullPath(item.RootPath)), canonical, StringComparison.OrdinalIgnoreCase));
        }

        var replacement = current is null
            ? new MonitoringRule(1, canonical, icoPath, DateTimeOffset.UtcNow)
            : current with { IcoPath = icoPath, UpdatedAtUtc = DateTimeOffset.UtcNow };
        await ReplaceRuleAsync(replacement, token).ConfigureAwait(false);
    }

    public async Task RemoveRuleAsync(string rootPath, CancellationToken token = default)
    {
        var canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
        await store.RemoveAsync(canonical, token).ConfigureAwait(false);
        rules.Remove(canonical);
        RebuildWatchers();
    }

    public async Task WaitForIdleAsync()
    {
        while (true)
        {
            Task[] snapshot;
            lock (sync) snapshot = work.Where(task => !task.IsCompleted).ToArray();
            if (snapshot.Length == 0) return;
            await Task.WhenAll(snapshot).ConfigureAwait(false);
        }
    }

    private void RebuildWatchers()
    {
        ClearWatchers();
        if (paused || disposed) return;
        foreach (var rule in rules.Values)
        {
            if (!Directory.Exists(rule.RootPath)) continue;
            var watcher = watcherFactory.Create(rule.RootPath);
            watcher.DirectoryCreated += DirectoryCreated;
            watchers[rule.RootPath] = watcher;
        }
    }

    private void DirectoryCreated(object? sender, DirectoryCreatedEventArgs e)
    {
        if (sender is not IDirectoryWatcher watcher || !rules.TryGetValue(watcher.RootPath, out var rule)) return;
        string canonical;
        try { canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(e.Path)); }
        catch (Exception) { return; }
        lock (sync)
        {
            if (!pending.Add(canonical)) return;
            var task = ProcessAsync(canonical, rule, e.WasRenamed);
            work.Add(task);
        }
    }

    private async Task ProcessAsync(string path, MonitoringRule rule, bool wasRenamed)
    {
        try
        {
            await Task.Yield();
            var icoPath = wasRenamed ? rule.IcoPath : ResolveIcoPath(path, rule);
            MonitoredApplyOutcome? outcome = null;
            for (var attempt = 0; attempt < RetryDelays.Length; attempt++)
            {
                await delay(RetryDelays[attempt], CancellationToken.None).ConfigureAwait(false);
                outcome = await applier.ApplyAsync(path, icoPath, CancellationToken.None).ConfigureAwait(false);
                if (outcome.Success) break;
            }
            outcome ??= MonitoredApplyOutcome.Failure(path, "Monitoring did not run.");
            OutcomeProduced?.Invoke(this, new(outcome.Success, outcome.Path, outcome.Error));
        }
        finally { lock (sync) pending.Remove(path); }
    }

    private static string ResolveIcoPath(string folderPath, MonitoringRule rule)
    {
        var leaf = Path.GetFileName(Path.TrimEndingDirectorySeparator(Path.GetFullPath(folderPath)));
        return rule.NameRules
            .Where(item => item.Enabled)
            .OrderBy(item => item.Order)
            .ThenBy(item => item.Id, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(item => leaf.Contains(item.Keyword, StringComparison.OrdinalIgnoreCase))?.IcoPath
            ?? rule.IcoPath;
    }

    private void ClearWatchers()
    {
        foreach (var watcher in watchers.Values)
        {
            watcher.DirectoryCreated -= DirectoryCreated;
            watcher.Dispose();
        }
        watchers.Clear();
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        ClearWatchers();
    }
}
