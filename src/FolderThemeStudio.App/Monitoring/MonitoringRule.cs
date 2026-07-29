using System.IO;

namespace FolderThemeStudio.App.Monitoring;

public sealed record MonitoringRule(
    int SchemaVersion,
    string RootPath,
    string IcoPath,
    DateTimeOffset UpdatedAtUtc);

public sealed record MonitoringRuleLoadResult(
    IReadOnlyList<MonitoringRule> Rules,
    bool Paused,
    IReadOnlyList<string> Diagnostics);

public interface IMonitoringRuleStore
{
    Task<MonitoringRuleLoadResult> LoadAsync(CancellationToken token = default);
    Task UpsertAsync(MonitoringRule rule, CancellationToken token = default);
    Task RemoveAsync(string rootPath, CancellationToken token = default);
    Task SetPausedAsync(bool paused, CancellationToken token = default);
}

public interface IMonitoringIconAssetService
{
    Task<string> PersistAsync(string generatedIcoPath, CancellationToken token);
}

public sealed class NullFolderMonitoringCoordinator : IFolderMonitoringCoordinator
{
    public MonitoringStatus Status => new(false, 0, 0, 0);
    public event EventHandler<MonitoringOutcome>? OutcomeProduced { add { } remove { } }
    public Task StartAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task PauseAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task ResumeAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task ReplaceRuleAsync(MonitoringRule rule, CancellationToken token = default) => Task.CompletedTask;
    public Task RemoveRuleAsync(string rootPath, CancellationToken token = default) => Task.CompletedTask;
    public void Dispose() { }
}

public sealed class PassthroughMonitoringIconAssetService : IMonitoringIconAssetService
{
    public Task<string> PersistAsync(string generatedIcoPath, CancellationToken token) =>
        Task.FromResult(Path.GetFullPath(generatedIcoPath));
}
