using System.IO;
using System.Security.Cryptography;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class MonitoringIconAssetServiceTests
{
    [Fact]
    public async Task Persist_UsesContentHashAndReusesIdenticalAsset()
    {
        using var temp = new TemporaryDirectory();
        var source = Path.Combine(temp.Path, "source.ico");
        var root = Path.Combine(temp.Path, "assets");
        var bytes = new byte[] { 0, 1, 2, 3 };
        await File.WriteAllBytesAsync(source, bytes);
        var service = new MonitoringIconAssetService(root);

        var first = await service.PersistAsync(source, CancellationToken.None);
        var second = await service.PersistAsync(source, CancellationToken.None);

        Assert.Equal(first, second);
        Assert.Equal(Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant() + ".ico", Path.GetFileName(first));
        Assert.Equal(bytes, await File.ReadAllBytesAsync(first));
        Assert.Single(Directory.EnumerateFiles(root));
    }
}
