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
        var loadedRule = Assert.Single(loaded.Rules);
        Assert.Equal(@"C:\icons\two.ico", loadedRule.IcoPath);
        Assert.Empty(loadedRule.NameRules);
        Assert.DoesNotContain(".tmp", Directory.EnumerateFiles(temp.Path).Select(Path.GetFileName));
    }

    [Fact]
    public async Task Upsert_RoundTripsOrderedNameRulesAndWritesSchemaTwo()
    {
        using var temp = new TemporaryDirectory();
        var path = Path.Combine(temp.Path, "rules.json");
        var root = Path.Combine(temp.Path, "Root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(path);
        var rule = new MonitoringRule(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow)
        {
            NameRules =
            [
                new("projects", "Project", @"C:\icons\project.ico", true, 1),
                new("archive", "Archive", @"C:\icons\archive.ico", true, 0)
            ]
        };

        await store.UpsertAsync(rule);
        var loaded = await store.LoadAsync();
        using var document = JsonDocument.Parse(await File.ReadAllTextAsync(path));

        Assert.Equal(2, document.RootElement.GetProperty("schemaVersion").GetInt32());
        Assert.Equal(["Archive", "Project"], Assert.Single(loaded.Rules).NameRules.Select(item => item.Keyword));
        Assert.Equal(@"C:\icons\fallback.ico", Assert.Single(loaded.Rules).IcoPath);
    }

    [Theory]
    [InlineData("", @"C:\icons\valid.ico")]
    [InlineData("Project", " ")]
    public async Task Upsert_RejectsInvalidNameRuleAndPreservesExistingRule(string keyword, string icoPath)
    {
        using var temp = new TemporaryDirectory();
        var path = Path.Combine(temp.Path, "rules.json");
        var root = Path.Combine(temp.Path, "Root");
        Directory.CreateDirectory(root);
        var store = new JsonMonitoringRuleStore(path);
        var existing = new MonitoringRule(1, root, @"C:\icons\fallback.ico", DateTimeOffset.UtcNow);
        await store.UpsertAsync(existing);

        var invalid = existing with
        {
            NameRules = [new("invalid", keyword, icoPath, true, 0)]
        };
        await Assert.ThrowsAsync<InvalidDataException>(() => store.UpsertAsync(invalid));

        var loaded = Assert.Single((await store.LoadAsync()).Rules);
        Assert.Empty(loaded.NameRules);
        Assert.Equal(existing.IcoPath, loaded.IcoPath);
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
