using System.IO;
using System.Windows;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class ExplorerFolderHoverTargetResolverTests
{
    [Fact]
    public void TryResolve_CombinesExplorerFolderAndListItemName()
    {
        using var temp = new TemporaryDirectory();
        var currentFolder = Path.Combine(temp.Path, "Parent");
        var child = Path.Combine(currentFolder, "Project Alpha");
        Directory.CreateDirectory(child);
        var bounds = new Rect(100, 80, 220, 30);
        var probe = new RecordingHoverListItemProbe(new("Project Alpha", 123, new nint(456), bounds, "explorer", true));
        var windowPath = new RecordingExplorerWindowPathResolver(currentFolder);
        var resolver = new ExplorerFolderHoverTargetResolver(probe, windowPath, Directory.Exists);

        var result = resolver.TryResolve(new Point(150, 95));

        Assert.Equal(Path.GetFullPath(child), result!.FolderPath);
        Assert.Equal(bounds, result.ItemBounds);
    }

    [Theory]
    [InlineData("notepad", true)]
    [InlineData("explorer", false)]
    public void TryResolve_IgnoresNonExplorerOrNonFileListItems(string processName, bool isFileListItem)
    {
        using var temp = new TemporaryDirectory();
        var currentFolder = Path.Combine(temp.Path, "Parent");
        Directory.CreateDirectory(Path.Combine(currentFolder, "Child"));
        var probe = new RecordingHoverListItemProbe(new("Child", 123, new nint(456), new Rect(0, 0, 200, 40), processName, isFileListItem));
        var resolver = new ExplorerFolderHoverTargetResolver(probe, new RecordingExplorerWindowPathResolver(currentFolder), Directory.Exists);

        Assert.Null(resolver.TryResolve(new Point(10, 10)));
    }

    [Fact]
    public void TryResolve_RejectsANameThatDoesNotResolveToAnExistingDirectory()
    {
        using var temp = new TemporaryDirectory();
        var currentFolder = Path.Combine(temp.Path, "Parent");
        Directory.CreateDirectory(currentFolder);
        var probe = new RecordingHoverListItemProbe(new("not-a-folder", 123, new nint(456), new Rect(0, 0, 200, 40), "explorer", true));
        var resolver = new ExplorerFolderHoverTargetResolver(probe, new RecordingExplorerWindowPathResolver(currentFolder), Directory.Exists);

        Assert.Null(resolver.TryResolve(new Point(10, 10)));
    }

    [Fact]
    public void TryResolve_StaleAutomationFailureReturnsNoTarget()
    {
        var resolver = new ExplorerFolderHoverTargetResolver(
            new RecordingHoverListItemProbe(exception: new InvalidOperationException("Element is no longer available.")),
            new RecordingExplorerWindowPathResolver(null), _ => false);

        Assert.Null(resolver.TryResolve(new Point(10, 10)));
    }

    [Fact]
    public void TryResolve_DesktopFolderUsesRealDesktopPath()
    {
        using var temp = new TemporaryDirectory();
        var userDesktop = Path.Combine(temp.Path, "UserDesktop");
        var publicDesktop = Path.Combine(temp.Path, "PublicDesktop");
        var child = Path.Combine(userDesktop, "Project");
        Directory.CreateDirectory(child);
        Directory.CreateDirectory(publicDesktop);
        var bounds = new Rect(10, 10, 100, 80);
        var probe = new RecordingHoverListItemProbe(new("Project", 123, new nint(456), bounds, "explorer", true, HoverSurface.DesktopList));
        var resolver = new ExplorerFolderHoverTargetResolver(probe, new RecordingExplorerWindowPathResolver(null), Directory.Exists,
            new DesktopFolderPathResolver(userDesktop, publicDesktop));

        Assert.Equal(child, resolver.TryResolve(new Point(20, 20))?.FolderPath);
    }

    [Fact]
    public void TryResolve_DesktopDuplicateNameDoesNotGuess()
    {
        using var temp = new TemporaryDirectory();
        var userDesktop = Path.Combine(temp.Path, "UserDesktop");
        var publicDesktop = Path.Combine(temp.Path, "PublicDesktop");
        Directory.CreateDirectory(Path.Combine(userDesktop, "Project"));
        Directory.CreateDirectory(Path.Combine(publicDesktop, "Project"));
        var probe = new RecordingHoverListItemProbe(new("Project", 123, new nint(456), new Rect(10, 10, 100, 80), "explorer", true, HoverSurface.DesktopList));
        var resolver = new ExplorerFolderHoverTargetResolver(probe, new RecordingExplorerWindowPathResolver(null), Directory.Exists,
            new DesktopFolderPathResolver(userDesktop, publicDesktop));

        Assert.Null(resolver.TryResolve(new Point(20, 20)));
    }

    [Theory]
    [InlineData("Project.lnk", false)]
    [InlineData("Document.txt", false)]
    [InlineData("Project", true)]
    public void TryResolve_DesktopRejectsNonDirectoriesAndOffItemPoints(string name, bool offItem)
    {
        using var temp = new TemporaryDirectory();
        var userDesktop = Path.Combine(temp.Path, "UserDesktop");
        Directory.CreateDirectory(Path.Combine(userDesktop, "Project"));
        var probe = new RecordingHoverListItemProbe(new(name, 123, new nint(456), new Rect(10, 10, 100, 80), "explorer", true, HoverSurface.DesktopList));
        var resolver = new ExplorerFolderHoverTargetResolver(probe, new RecordingExplorerWindowPathResolver(null), Directory.Exists,
            new DesktopFolderPathResolver(userDesktop, Path.Combine(temp.Path, "PublicDesktop")));

        Assert.Null(resolver.TryResolve(offItem ? new Point(150, 150) : new Point(20, 20)));
    }

    private sealed class RecordingHoverListItemProbe(HoverListItemCandidate? candidate = null, Exception? exception = null) : IHoverListItemProbe
    {
        public HoverListItemCandidate? FindListItem(Point point)
        {
            if (exception is not null) throw exception;
            return candidate;
        }
    }

    private sealed class RecordingExplorerWindowPathResolver(string? path) : IExplorerWindowPathResolver
    {
        public string? GetCurrentFolderPath(nint rootWindowHandle) => path;
    }
}
