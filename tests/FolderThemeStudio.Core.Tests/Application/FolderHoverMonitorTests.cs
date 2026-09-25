using System.Collections.Concurrent;
using System.Windows;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class FolderHoverMonitorTests
{
    private static readonly FolderCategoryEntry Project = new(@"C:\Work\Project", "Work", "Current project", "#2868D7");

    [Fact]
    public async Task StartAsync_EmptyCatalogStillResolvesFolderForQuickEditButDoesNotShowCard()
    {
        var catalog = new RecordingCatalog([]);
        var cursor = new MutableCursor(new Point(20, 20));
        var resolver = new MutableResolver(new ExplorerHoverTarget(Project.FolderPath, new Rect(10, 10, 100, 50)));
        var presenter = new RecordingPresenter();
        using var monitor = CreateMonitor(catalog, cursor, resolver, presenter);

        await monitor.StartAsync();
        await Task.Delay(350);

        Assert.True(resolver.CallCount > 0);
        Assert.False(presenter.IsVisible);
        Assert.Equal(Project.FolderPath, monitor.TryGetRecentTarget(new Point(20, 20))?.FolderPath);
    }

    [Fact]
    public async Task StartAsync_OverCategorizedFolderShowsAfterDwell()
    {
        var catalog = new RecordingCatalog([Project]);
        var cursor = new MutableCursor(new Point(20, 20));
        var resolver = new MutableResolver(new ExplorerHoverTarget(Project.FolderPath, new Rect(10, 10, 100, 50)));
        var presenter = new RecordingPresenter();
        using var monitor = CreateMonitor(catalog, cursor, resolver, presenter);

        await monitor.StartAsync();
        Assert.False(presenter.IsVisible);
        await WaitUntilAsync(() => presenter.IsVisible, TimeSpan.FromSeconds(2));

        Assert.Equal(Project, Assert.Single(presenter.Shown));
    }

    [Fact]
    public async Task Poll_LeavingFolderBoundsHidesCard()
    {
        var catalog = new RecordingCatalog([Project]);
        var cursor = new MutableCursor(new Point(20, 20));
        var resolver = new MutableResolver(new ExplorerHoverTarget(Project.FolderPath, new Rect(10, 10, 100, 50)));
        var presenter = new RecordingPresenter();
        using var monitor = CreateMonitor(catalog, cursor, resolver, presenter);
        await monitor.StartAsync();
        await WaitUntilAsync(() => presenter.IsVisible, TimeSpan.FromSeconds(2));

        cursor.Position = new Point(300, 300);
        resolver.Target = null;
        await WaitUntilAsync(() => !presenter.IsVisible, TimeSpan.FromSeconds(2));

        Assert.True(presenter.HideCount > 0);
    }

    [Fact]
    public async Task Poll_WindowChangesUnderStationaryCursorHidesCard()
    {
        var catalog = new RecordingCatalog([Project]);
        var cursor = new MutableCursor(new Point(20, 20));
        var resolver = new MutableResolver(new ExplorerHoverTarget(Project.FolderPath, new Rect(10, 10, 100, 50), new nint(456)));
        var presenter = new RecordingPresenter();
        nint root = new nint(456);
        using var monitor = new FolderHoverMonitor(catalog, resolver, cursor, presenter,
            new ImmediateViewModelDispatcher(), rootAtPoint: _ => root);
        await monitor.StartAsync();
        await WaitUntilAsync(() => presenter.IsVisible, TimeSpan.FromSeconds(2));

        Assert.Null(monitor.TryGetRecentTarget(new Point(20, 20), nint.Zero));

        root = new nint(789);
        resolver.Target = null;
        await WaitUntilAsync(() => !presenter.IsVisible, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public async Task Poll_MovingInsideCategorizedFolderKeepsCardStationary()
    {
        var catalog = new RecordingCatalog([Project]);
        var cursor = new MutableCursor(new Point(20, 20));
        var resolver = new MutableResolver(new ExplorerHoverTarget(Project.FolderPath, new Rect(10, 10, 100, 50)));
        var presenter = new RecordingPresenter();
        using var monitor = CreateMonitor(catalog, cursor, resolver, presenter);
        await monitor.StartAsync();
        await WaitUntilAsync(() => presenter.IsVisible, TimeSpan.FromSeconds(2));

        var initial = presenter.Position;
        cursor.Position = new Point(40, 30);
        await Task.Delay(400);

        Assert.Equal(initial, presenter.Position);
    }

    [Fact]
    public async Task ReloadCatalogAsync_RemovingVisibleCategoryHidesCard()
    {
        var catalog = new RecordingCatalog([Project]);
        var cursor = new MutableCursor(new Point(20, 20));
        var resolver = new MutableResolver(new ExplorerHoverTarget(Project.FolderPath, new Rect(10, 10, 100, 50)));
        var presenter = new RecordingPresenter();
        using var monitor = CreateMonitor(catalog, cursor, resolver, presenter);
        await monitor.StartAsync();
        await WaitUntilAsync(() => presenter.IsVisible, TimeSpan.FromSeconds(2));

        catalog.Entries = [];
        await monitor.ReloadCatalogAsync();
        await WaitUntilAsync(() => !presenter.IsVisible, TimeSpan.FromSeconds(2));

        Assert.True(presenter.HideCount > 0);
    }

    [Fact]
    public async Task StartAsync_IsIdempotentAndDisposeHidesTheCard()
    {
        var catalog = new RecordingCatalog([Project]);
        var cursor = new MutableCursor(new Point(20, 20));
        var resolver = new MutableResolver(new ExplorerHoverTarget(Project.FolderPath, new Rect(10, 10, 100, 50)));
        var presenter = new RecordingPresenter();
        var monitor = CreateMonitor(catalog, cursor, resolver, presenter);

        await monitor.StartAsync();
        await monitor.StartAsync();
        await WaitUntilAsync(() => presenter.IsVisible, TimeSpan.FromSeconds(2));
        monitor.Dispose();
        var callsAfterDispose = resolver.CallCount;
        await Task.Delay(400);

        Assert.False(presenter.IsVisible);
        Assert.Equal(callsAfterDispose, resolver.CallCount);
    }

    [Fact]
    public async Task Poll_ResolverFailureHidesCardAndKeepsMonitorAlive()
    {
        var catalog = new RecordingCatalog([Project]);
        var cursor = new MutableCursor(new Point(20, 20));
        var resolver = new MutableResolver(new ExplorerHoverTarget(Project.FolderPath, new Rect(10, 10, 100, 50)));
        var presenter = new RecordingPresenter();
        using var monitor = CreateMonitor(catalog, cursor, resolver, presenter);
        await monitor.StartAsync();
        await WaitUntilAsync(() => presenter.IsVisible, TimeSpan.FromSeconds(2));

        resolver.Throw = true;
        cursor.Position = new Point(300, 300);
        await WaitUntilAsync(() => !presenter.IsVisible, TimeSpan.FromSeconds(2));
        resolver.Throw = false;
        cursor.Position = new Point(20, 20);
        await WaitUntilAsync(() => presenter.IsVisible, TimeSpan.FromSeconds(2));
    }

    private static FolderHoverMonitor CreateMonitor(RecordingCatalog catalog, MutableCursor cursor, MutableResolver resolver, RecordingPresenter presenter) =>
        new(catalog, resolver, cursor, presenter, new ImmediateViewModelDispatcher());

    private static async Task WaitUntilAsync(Func<bool> condition, TimeSpan timeout)
    {
        var deadline = DateTime.UtcNow + timeout;
        while (!condition())
        {
            if (DateTime.UtcNow >= deadline) throw new TimeoutException("The expected hover-monitor state was not reached.");
            await Task.Delay(40);
        }
    }

    private sealed class RecordingCatalog(IReadOnlyList<FolderCategoryEntry> entries) : IFolderCategoryCatalog
    {
        internal IReadOnlyList<FolderCategoryEntry> Entries { get; set; } = entries;
        public Task<IReadOnlyList<FolderCategoryEntry>> LoadAsync(CancellationToken token = default) => Task.FromResult(Entries);
        public Task UpsertAsync(FolderCategoryEntry entry, CancellationToken token = default) => Task.CompletedTask;
        public Task RemoveAsync(string folderPath, CancellationToken token = default) => Task.CompletedTask;
    }

    private sealed class MutableCursor(Point position) : ICursorPositionSource
    {
        private Point position = position;
        private readonly object sync = new();
        internal Point Position { get { lock (sync) return position; } set { lock (sync) position = value; } }
        public bool TryGetPosition(out Point screenPoint) { screenPoint = Position; return true; }
    }

    private sealed class MutableResolver(ExplorerHoverTarget? target) : IExplorerFolderHoverTargetResolver
    {
        private ExplorerHoverTarget? target = target;
        private int callCount;
        private int throwFlag;
        internal ExplorerHoverTarget? Target { get => Volatile.Read(ref target); set => Volatile.Write(ref target, value); }
        internal int CallCount => Volatile.Read(ref callCount);
        internal bool Throw { get => Volatile.Read(ref throwFlag) != 0; set => Volatile.Write(ref throwFlag, value ? 1 : 0); }
        public ExplorerHoverTarget? TryResolve(Point screenPoint)
        {
            Interlocked.Increment(ref callCount);
            if (Throw) throw new InvalidOperationException("UI Automation provider disconnected.");
            return Target;
        }
    }

    private sealed class RecordingPresenter : IFolderHoverCardPresenter
    {
        private readonly ConcurrentQueue<FolderCategoryEntry> shown = new();
        private int visible;
        private int hideCount;
        private Rect position;
        internal IReadOnlyList<FolderCategoryEntry> Shown => shown.ToArray();
        internal bool IsVisible => Volatile.Read(ref visible) != 0;
        internal int HideCount => Volatile.Read(ref hideCount);
        internal Rect Position { get { lock (shown) return position; } }
        public void Show(FolderCategoryEntry entry, Rect itemBounds)
        {
            shown.Enqueue(entry);
            lock (shown) position = itemBounds;
            Volatile.Write(ref visible, 1);
        }
        public void Hide()
        {
            Interlocked.Increment(ref hideCount);
            Volatile.Write(ref visible, 0);
        }
    }
}
