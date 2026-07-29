using System.IO;
using FolderThemeStudio.Core.Recovery;

namespace FolderThemeStudio.Core.Tests.TestSupport;

internal sealed class InMemoryFileSystem : IFileSystem
{
    private readonly Dictionary<string, string> files = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> directories = new(StringComparer.OrdinalIgnoreCase);
    private int replaceCallCount;

    internal bool FailNextReplace { get; set; }
    internal HashSet<int> FailOnReplaceCalls { get; } = [];
    internal Action? OnReplaceFailure { get; set; }

    internal void SetFile(string path, string contents) => files[path] = contents;

    public void CreateDirectory(string path) => directories.Add(path);

    public bool FileExists(string path) => files.ContainsKey(path);

    public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken) =>
        Task.FromResult(files[path]);

    public Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken)
    {
        files[path] = contents;
        return Task.CompletedTask;
    }

    public void MoveFile(string sourcePath, string destinationPath) =>
        files[destinationPath] = files.Remove(sourcePath, out var contents)
            ? contents
            : throw new FileNotFoundException("The temporary file does not exist.", sourcePath);

    public void ReplaceFile(string sourcePath, string destinationPath)
    {
        replaceCallCount++;
        if (FailNextReplace || FailOnReplaceCalls.Remove(replaceCallCount))
        {
            FailNextReplace = false;
            OnReplaceFailure?.Invoke();
            throw new IOException("Simulated interrupted replacement.");
        }

        MoveFile(sourcePath, destinationPath);
    }

    public void DeleteFile(string path) => files.Remove(path);
}
