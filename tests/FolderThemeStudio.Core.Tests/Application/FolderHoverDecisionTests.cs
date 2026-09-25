using System.Windows;
using FolderThemeStudio.App.Services;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class FolderHoverDecisionTests
{
    private static readonly IReadOnlyDictionary<string, FolderCategoryEntry> Entries = new Dictionary<string, FolderCategoryEntry>(StringComparer.OrdinalIgnoreCase)
    {
        [@"C:\Work\Project"] = new(@"C:\Work\Project", "Work", "Current project", "#2868D7"),
        [@"C:\Work\Archive"] = new(@"C:\Work\Archive", "Archive", "Keep for reference", "#445566")
    };

    [Fact]
    public void Update_DoesNotShowBeforeDwellAndShowsAtDwell()
    {
        var decision = new FolderHoverDecision(TimeSpan.FromMilliseconds(550));
        var target = new ExplorerHoverTarget(@"C:\Work\Project", new Rect(10, 10, 100, 50));
        var point = new Point(40, 30);
        var start = DateTimeOffset.UtcNow;

        Assert.Null(decision.Update(target, point, Entries, start));
        Assert.Null(decision.Update(target, point, Entries, start.AddMilliseconds(549)));
        Assert.Equal("Work", decision.Update(target, point, Entries, start.AddMilliseconds(550))!.Category);
    }

    [Fact]
    public void Update_LeavingBoundsAndChangingFoldersRestartsDwell()
    {
        var decision = new FolderHoverDecision(TimeSpan.FromMilliseconds(550));
        var first = new ExplorerHoverTarget(@"C:\Work\Project", new Rect(10, 10, 100, 50));
        var second = new ExplorerHoverTarget(@"C:\Work\Archive", new Rect(200, 10, 100, 50));
        var start = DateTimeOffset.UtcNow;

        decision.Update(first, new Point(20, 20), Entries, start);
        Assert.Null(decision.Update(first, new Point(180, 20), Entries, start.AddMilliseconds(600)));
        decision.Update(first, new Point(20, 20), Entries, start.AddSeconds(1));
        Assert.Null(decision.Update(second, new Point(220, 20), Entries, start.AddSeconds(2)));
        Assert.Null(decision.Update(second, new Point(220, 20), Entries, start.AddSeconds(2).AddMilliseconds(549)));
        Assert.NotNull(decision.Update(second, new Point(220, 20), Entries, start.AddSeconds(2).AddMilliseconds(550)));
    }

    [Fact]
    public void Update_UncategorizedOrMissingTargetReturnsNull()
    {
        var decision = new FolderHoverDecision(TimeSpan.Zero);
        Assert.Null(decision.Update(new ExplorerHoverTarget(@"C:\Other", new Rect(0, 0, 50, 50)), new Point(5, 5), Entries, DateTimeOffset.UtcNow));
        Assert.Null(decision.Update(null, new Point(5, 5), Entries, DateTimeOffset.UtcNow));
    }
}
