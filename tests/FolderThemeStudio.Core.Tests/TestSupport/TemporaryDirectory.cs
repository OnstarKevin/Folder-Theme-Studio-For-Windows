namespace FolderThemeStudio.Core.Tests.TestSupport;

public sealed class TemporaryDirectory : IDisposable
{
    private static readonly string TestRoot = System.IO.Path.GetFullPath(
        System.IO.Path.Combine(System.IO.Path.GetTempPath(), "FolderThemeStudioTests"));

    public TemporaryDirectory()
    {
        System.IO.Directory.CreateDirectory(TestRoot);
        Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(TestRoot, Guid.NewGuid().ToString("N")));
        System.IO.Directory.CreateDirectory(Path);
    }

    public string Path { get; }

    public void Dispose()
    {
        if (!System.IO.Directory.Exists(Path) || !IsOwnedDirectory(Path))
        {
            return;
        }

        System.IO.Directory.Delete(Path, recursive: true);
    }

    private static bool IsOwnedDirectory(string candidate)
    {
        var normalizedCandidate = System.IO.Path.GetFullPath(candidate);
        var relativePath = System.IO.Path.GetRelativePath(TestRoot, normalizedCandidate);

        return !string.IsNullOrWhiteSpace(relativePath)
            && relativePath is not "."
            && !System.IO.Path.IsPathRooted(relativePath)
            && !relativePath.Equals("..", StringComparison.Ordinal)
            && !relativePath.StartsWith($"..{System.IO.Path.DirectorySeparatorChar}", StringComparison.Ordinal)
            && !relativePath.StartsWith($"..{System.IO.Path.AltDirectorySeparatorChar}", StringComparison.Ordinal);
    }
}
