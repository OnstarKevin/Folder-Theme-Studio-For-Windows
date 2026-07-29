using System.IO;
using System.Text;
using FolderThemeStudio.Core.Recovery;
using FolderThemeStudio.Core.SystemIntegration;

namespace FolderThemeStudio.Core.Tests.TestSupport;

public sealed class TemporaryFolderFixture : IDisposable
{
    private static readonly string TestRoot = Path.GetFullPath(
        Path.Combine(Path.GetTempPath(), "FolderThemeStudioTests", "CompatibleFolderService"));

    public TemporaryFolderFixture()
    {
        Directory.CreateDirectory(TestRoot);
        RootPath = Path.GetFullPath(Path.Combine(TestRoot, Guid.NewGuid().ToString("N")));
        FolderPath = Path.Combine(RootPath, "folder");
        Directory.CreateDirectory(FolderPath);
        OriginalAttributes = File.GetAttributes(FolderPath);
        DesktopIniPath = Path.Combine(FolderPath, "desktop.ini");
        IcoPath = Path.Combine(RootPath, "theme.ico");
        File.WriteAllBytes(IcoPath, [0]);
        Plan = [new FolderPlanItem(FolderPath, FolderDecision.Allowed, RootPath, FolderPath, RootPath)];
        BackupService = BackupService.CreateForTesting(new WindowsFileSystem(), Path.Combine(RootPath, "backups"));
    }

    public string RootPath { get; }
    public string FolderPath { get; }
    public string DesktopIniPath { get; }
    public FileAttributes OriginalAttributes { get; }
    public string IcoPath { get; }
    public IReadOnlyList<FolderPlanItem> Plan { get; }
    public BackupService BackupService { get; }

    public void WriteDesktopIni(string contents) =>
        File.WriteAllText(DesktopIniPath, contents, new UnicodeEncoding(false, true));

    public string ReadDesktopIni() => File.ReadAllText(DesktopIniPath, Encoding.Unicode);

    public void Dispose()
    {
        if (!Directory.Exists(RootPath) || !IsOwnedDirectory(RootPath))
        {
            return;
        }

        foreach (var path in Directory.EnumerateFiles(RootPath, "*", SearchOption.AllDirectories))
        {
            File.SetAttributes(path, FileAttributes.Normal);
        }

        foreach (var path in Directory.EnumerateDirectories(RootPath, "*", SearchOption.AllDirectories).Prepend(RootPath))
        {
            File.SetAttributes(path, FileAttributes.Directory);
        }

        Directory.Delete(RootPath, recursive: true);
    }

    private static bool IsOwnedDirectory(string candidate)
    {
        var relativePath = Path.GetRelativePath(TestRoot, Path.GetFullPath(candidate));
        return !string.IsNullOrWhiteSpace(relativePath)
            && relativePath is not "."
            && !Path.IsPathRooted(relativePath)
            && relativePath is not ".."
            && !relativePath.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
            && !relativePath.StartsWith($"..{Path.AltDirectorySeparatorChar}", StringComparison.Ordinal);
    }
}
