using System.IO;
using System.Text;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.SystemIntegration;

public sealed class MonitoredFolderIconServiceTests
{
    [Fact]
    public async Task Apply_MergesExistingConfigurationAndSetsFolderAttributes()
    {
        using var temp = new TemporaryDirectory();
        var folder = Path.Combine(temp.Path, "new-folder");
        Directory.CreateDirectory(folder);
        var desktopIni = Path.Combine(folder, "desktop.ini");
        File.WriteAllText(desktopIni, "[.ShellClassInfo]\r\nInfoTip=Keep\r\n", new UnicodeEncoding(false, true));
        var ico = Path.Combine(temp.Path, "style.ico");
        await File.WriteAllBytesAsync(ico, [0, 1]);

        var outcome = await new MonitoredFolderIconService().ApplyAsync(folder, ico, CancellationToken.None);

        Assert.True(outcome.Success, outcome.Error);
        var text = File.ReadAllText(desktopIni, Encoding.Unicode);
        Assert.Contains("InfoTip=Keep", text);
        Assert.Contains($"IconFile={ico}", text);
        Assert.True((File.GetAttributes(folder) & FileAttributes.ReadOnly) != 0);
        File.SetAttributes(desktopIni, FileAttributes.Normal);
        File.SetAttributes(folder, FileAttributes.Directory);
    }
}
