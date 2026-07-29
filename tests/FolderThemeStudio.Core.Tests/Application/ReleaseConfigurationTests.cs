using System.Diagnostics;
using System.IO;
using FolderThemeStudio.App;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class ReleaseConfigurationTests
{
    [Fact]
    public void AppAssembly_IdentifiesBeta6Release()
    {
        var version = FileVersionInfo.GetVersionInfo(typeof(FolderThemeStudio.App.App).Assembly.Location);

        Assert.StartsWith("0.1.0-beta.6", version.ProductVersion);
        Assert.Equal("0.1.0.6", version.FileVersion);
    }

    [Fact]
    public void PackageScript_DefaultsToBeta6AndExcludesDesktopIni()
    {
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var script = File.ReadAllText(Path.Combine(root, "build", "Package-Release.ps1"));

        Assert.Contains("v0.1.0-beta.6", script);
        Assert.Contains("desktop.ini", script, StringComparison.OrdinalIgnoreCase);
        var installer = File.ReadAllText(Path.Combine(root, "build", "FolderThemeStudio.iss"));
        Assert.Contains("RegDeleteValue", installer);
        Assert.Contains("FolderThemeStudio", installer);
    }
}
