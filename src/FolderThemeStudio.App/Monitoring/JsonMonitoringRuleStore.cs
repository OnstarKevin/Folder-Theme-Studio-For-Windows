using System.IO;
using System.Text.Json;

namespace FolderThemeStudio.App.Monitoring;

public sealed class JsonMonitoringRuleStore : IMonitoringRuleStore
{
    private const int SchemaVersion = 2;
    private const int LegacySchemaVersion = 1;
    private const int MonitoringRuleSchemaVersion = 1;
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    private readonly string path;
    private readonly SemaphoreSlim gate = new(1, 1);

    public JsonMonitoringRuleStore(string? path = null)
    {
        this.path = Path.GetFullPath(path ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio", "monitoring-rules.json"));
    }

    public async Task<MonitoringRuleLoadResult> LoadAsync(CancellationToken token = default)
    {
        await gate.WaitAsync(token).ConfigureAwait(false);
        try { return await LoadCoreAsync(token).ConfigureAwait(false); }
        finally { gate.Release(); }
    }

    public async Task UpsertAsync(MonitoringRule rule, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(rule);
        var normalized = Normalize(rule);
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var current = await LoadCoreAsync(token).ConfigureAwait(false);
            var rules = current.Rules
                .Where(item => !string.Equals(item.RootPath, normalized.RootPath, StringComparison.OrdinalIgnoreCase))
                .Append(normalized)
                .OrderBy(item => item.RootPath, StringComparer.OrdinalIgnoreCase)
                .ToArray();
            await SaveCoreAsync(rules, current.Paused, token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    public async Task RemoveAsync(string rootPath, CancellationToken token = default)
    {
        var canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var current = await LoadCoreAsync(token).ConfigureAwait(false);
            await SaveCoreAsync(
                current.Rules.Where(item => !string.Equals(item.RootPath, canonical, StringComparison.OrdinalIgnoreCase)).ToArray(),
                current.Paused,
                token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    public async Task SetPausedAsync(bool paused, CancellationToken token = default)
    {
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var current = await LoadCoreAsync(token).ConfigureAwait(false);
            await SaveCoreAsync(current.Rules, paused, token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    private async Task<MonitoringRuleLoadResult> LoadCoreAsync(CancellationToken token)
    {
        if (!File.Exists(path)) return new([], false, []);
        try
        {
            await using var stream = File.OpenRead(path);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: token).ConfigureAwait(false);
            var root = document.RootElement;
            var paused = root.TryGetProperty("paused", out var pausedElement) && pausedElement.ValueKind == JsonValueKind.True;
            var rules = new List<MonitoringRule>();
            var diagnostics = new List<string>();
            if (!root.TryGetProperty("schemaVersion", out var version) ||
                version.GetInt32() is not (LegacySchemaVersion or SchemaVersion))
                return new([], paused, ["Unsupported monitoring rule document version."]);
            if (!root.TryGetProperty("rules", out var entries) || entries.ValueKind != JsonValueKind.Array)
                return new([], paused, ["Monitoring rule list is missing."]);

            var index = 0;
            foreach (var entry in entries.EnumerateArray())
            {
                try
                {
                    var rule = JsonSerializer.Deserialize<MonitoringRule>(entry.GetRawText(), Options)
                        ?? throw new InvalidDataException("Rule is empty.");
                    rules.Add(Normalize(rule));
                }
                catch (Exception exception) when (exception is JsonException or InvalidDataException or ArgumentException or FormatException)
                {
                    diagnostics.Add($"Rule {index + 1}: {exception.Message}");
                }
                index++;
            }
            return new(rules, paused, diagnostics);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or JsonException)
        {
            return new([], false, [exception.Message]);
        }
    }

    private async Task SaveCoreAsync(IReadOnlyList<MonitoringRule> rules, bool paused, CancellationToken token)
    {
        var directory = Path.GetDirectoryName(path) ?? throw new InvalidOperationException("Rules path has no parent.");
        Directory.CreateDirectory(directory);
        var temporary = path + ".tmp";
        try
        {
            await using (var stream = new FileStream(temporary, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                await JsonSerializer.SerializeAsync(stream, new RuleDocument(SchemaVersion, paused, rules), Options, token).ConfigureAwait(false);
                await stream.FlushAsync(token).ConfigureAwait(false);
            }
            File.Move(temporary, path, overwrite: true);
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }

    private static MonitoringRule Normalize(MonitoringRule rule)
    {
        if (rule.SchemaVersion != MonitoringRuleSchemaVersion) throw new InvalidDataException("Unsupported monitoring rule version.");
        if (string.IsNullOrWhiteSpace(rule.RootPath) || string.IsNullOrWhiteSpace(rule.IcoPath))
            throw new InvalidDataException("Monitoring paths are required.");
        if (rule.UpdatedAtUtc == default) throw new InvalidDataException("Monitoring update time is required.");
        return rule with
        {
            RootPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rule.RootPath)),
            IcoPath = Path.GetFullPath(rule.IcoPath),
            UpdatedAtUtc = rule.UpdatedAtUtc.ToUniversalTime(),
            NameRules = rule.NameRules.Select(Normalize)
                .OrderBy(item => item.Order)
                .ThenBy(item => item.Id, StringComparer.OrdinalIgnoreCase)
                .ToArray()
        };
    }

    private static FolderNameStyleRule Normalize(FolderNameStyleRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        if (string.IsNullOrWhiteSpace(rule.Id) || string.IsNullOrWhiteSpace(rule.Keyword) || string.IsNullOrWhiteSpace(rule.IcoPath))
            throw new InvalidDataException("Name style rule ID, keyword, and ICO path are required.");
        if (rule.Order < 0)
            throw new InvalidDataException("Name style rule order cannot be negative.");
        return rule with
        {
            Id = rule.Id.Trim(),
            Keyword = rule.Keyword.Trim(),
            IcoPath = Path.GetFullPath(rule.IcoPath)
        };
    }

    private sealed record RuleDocument(int SchemaVersion, bool Paused, IReadOnlyList<MonitoringRule> Rules);
}
