using System.IO;
using System.Text.Json;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class MonitoringRuleStoreTests
{
    [Fact]
    public async Task Upsert_ReplacesCanonicalRootCaseInsensitivelyAndRoundTripsPause()
    {
        using var temp = new TemporaryDirectory();
        var path = Path.Combine(temp.Path, "rules.json");
        var store = new JsonMonitoringRuleStore(path);
        var root = Path.Combine(temp.Path, "Root");
        Directory.CreateDirectory(root);

        await store.UpsertAsync(new MonitoringRule(1, root, @"C:\icons\one.ico", DateTimeOffset.UtcNow));
        await store.UpsertAsync(new MonitoringRule(1, root.ToUpperInvariant(), @"C:\icons\two.ico", DateTimeOffset.UtcNow));
        await store.SetPausedAsync(true);
        var loaded = await store.LoadAsync();

        Assert.True(loaded.Paused);
        Assert.Empty(loaded.Diagnostics);
        Assert.Equal(@"C:\icons\two.ico", Assert.Single(loaded.Rules).IcoPath);
        Assert.DoesNotContain(".tmp", Directory.EnumerateFiles(temp.Path).Select(Path.GetFileName));
    }

    [Fact]
    public async Task Load_KeepsValidRulesAndReportsMalformedEntries()
    {
        using var temp = new TemporaryDirectory();
        var path = Path.Combine(temp.Path, "rules.json");
        var root = Path.Combine(temp.Path, "Root");
        Directory.CreateDirectory(root);
        await File.WriteAllTextAsync(path, $$"""
            {"schemaVersion":1,"paused":false,"rules":[
              {"schemaVersion":1,"rootPath":"{{JsonEncodedText.Encode(root)}}","icoPath":"C:\\icons\\ok.ico","updatedAtUtc":"2026-07-29T00:00:00+00:00"},
              {"schemaVersion":99,"rootPath":"","icoPath":"","updatedAtUtc":"bad"}
            ]}
            """);

        var loaded = await new JsonMonitoringRuleStore(path).LoadAsync();

        Assert.Single(loaded.Rules);
        Assert.Single(loaded.Diagnostics);
    }

    [Fact]
    public async Task Remove_DeletesOnlySelectedRoot()
    {
        using var temp = new TemporaryDirectory();
        var store = new JsonMonitoringRuleStore(Path.Combine(temp.Path, "rules.json"));
        var first = Path.Combine(temp.Path, "one");
        var second = Path.Combine(temp.Path, "two");
        Directory.CreateDirectory(first);
        Directory.CreateDirectory(second);
        await store.UpsertAsync(new(1, first, @"C:\one.ico", DateTimeOffset.UtcNow));
        await store.UpsertAsync(new(1, second, @"C:\two.ico", DateTimeOffset.UtcNow));

        await store.RemoveAsync(first);

        Assert.Equal(second, Assert.Single((await store.LoadAsync()).Rules).RootPath, ignoreCase: true);
    }
}
