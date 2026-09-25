using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using FolderThemeStudio.App;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class ReleaseConfigurationTests
{
    [Fact]
    public void AppAssembly_IdentifiesBeta8Release()
    {
        var version = FileVersionInfo.GetVersionInfo(typeof(FolderThemeStudio.App.App).Assembly.Location);

        Assert.StartsWith("0.1.0-beta.8", version.ProductVersion);
        Assert.Equal("0.1.0.8", version.FileVersion);
    }

    [Fact]
    public void PackageScript_DefaultsToBeta8AndExcludesLocalArtifacts()
    {
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var script = File.ReadAllText(Path.Combine(root, "build", "Package-Release.ps1"));
        var sourceArchiveScript = File.ReadAllText(Path.Combine(root, "build", "New-SourceArchive.ps1"));

        Assert.Contains("v0.1.0-beta.8", script);
        Assert.Contains("desktop.ini", sourceArchiveScript, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("node_modules", sourceArchiveScript, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(".hyperframes", sourceArchiveScript, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("renders", sourceArchiveScript, StringComparison.OrdinalIgnoreCase);
        var installer = File.ReadAllText(Path.Combine(root, "build", "FolderThemeStudio.iss"));
        Assert.Contains("0.1.0.8", installer);
        Assert.Contains("RegDeleteValue", installer);
        Assert.Contains("FolderThemeStudio", installer);
    }

    [Fact]
    public void SourceArchive_AllowsFilesWithPreZipTimestamps()
    {
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var testRoot = Path.Combine(root, "artifacts", $".source-archive-test-{Guid.NewGuid():N}");
        var sourceRoot = Path.Combine(testRoot, "source");
        var stagingRoot = Path.Combine(testRoot, "staging");
        var archivePath = Path.Combine(testRoot, "source.zip");

        try
        {
            Directory.CreateDirectory(sourceRoot);
            var sourceFile = Path.Combine(sourceRoot, "legacy-timestamp.txt");
            File.WriteAllText(sourceFile, "archive me");
            File.SetLastWriteTimeUtc(sourceFile, new DateTime(1970, 1, 1, 0, 0, 1, DateTimeKind.Utc));

            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };
            startInfo.ArgumentList.Add("-NoProfile");
            startInfo.ArgumentList.Add("-ExecutionPolicy");
            startInfo.ArgumentList.Add("Bypass");
            startInfo.ArgumentList.Add("-File");
            startInfo.ArgumentList.Add(Path.Combine(root, "build", "New-SourceArchive.ps1"));
            startInfo.ArgumentList.Add("-RepositoryRoot");
            startInfo.ArgumentList.Add(sourceRoot);
            startInfo.ArgumentList.Add("-SourceStagingPath");
            startInfo.ArgumentList.Add(stagingRoot);
            startInfo.ArgumentList.Add("-DestinationPath");
            startInfo.ArgumentList.Add(archivePath);

            using var process = Process.Start(startInfo)!;
            var standardOutput = process.StandardOutput.ReadToEnd();
            var standardError = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Assert.True(process.ExitCode == 0, $"{standardOutput}{Environment.NewLine}{standardError}");
            using var archive = ZipFile.OpenRead(archivePath);
            var entry = Assert.Single(archive.Entries);
            Assert.Equal("legacy-timestamp.txt", entry.FullName);
            Assert.True(entry.LastWriteTime >= new DateTimeOffset(1980, 1, 1, 0, 0, 0, TimeSpan.Zero));
        }
        finally
        {
            if (Directory.Exists(testRoot))
            {
                Directory.Delete(testRoot, recursive: true);
            }
        }
    }
}
