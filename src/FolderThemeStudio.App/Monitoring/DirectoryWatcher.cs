using System.IO;

namespace FolderThemeStudio.App.Monitoring;

public sealed record DirectoryCreatedEventArgs(string Path, bool WasRenamed = false);

public interface IDirectoryWatcher : IDisposable
{
    string RootPath { get; }
    bool IncludeSubdirectories { get; }
    event EventHandler<DirectoryCreatedEventArgs>? DirectoryCreated;
}

public interface IDirectoryWatcherFactory
{
    IDirectoryWatcher Create(string rootPath);
}

public sealed class DirectoryWatcherFactory : IDirectoryWatcherFactory
{
    public IDirectoryWatcher Create(string rootPath) => new DirectoryWatcher(rootPath);
}

internal sealed class DirectoryWatcher : IDirectoryWatcher
{
    private readonly FileSystemWatcher watcher;
    public DirectoryWatcher(string rootPath)
    {
        RootPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
        watcher = new FileSystemWatcher(RootPath)
        {
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.DirectoryName,
            EnableRaisingEvents = true
        };
        watcher.Created += Created;
        watcher.Renamed += Renamed;
    }

    public string RootPath { get; }
    public bool IncludeSubdirectories => watcher.IncludeSubdirectories;
    public event EventHandler<DirectoryCreatedEventArgs>? DirectoryCreated;
    private void Created(object sender, FileSystemEventArgs e) { if (Directory.Exists(e.FullPath)) DirectoryCreated?.Invoke(this, new(e.FullPath)); }
    private void Renamed(object sender, RenamedEventArgs e) { if (Directory.Exists(e.FullPath)) DirectoryCreated?.Invoke(this, new(e.FullPath, WasRenamed: true)); }
    public void Dispose()
    {
        watcher.EnableRaisingEvents = false;
        watcher.Created -= Created;
        watcher.Renamed -= Renamed;
        watcher.Dispose();
    }
}
