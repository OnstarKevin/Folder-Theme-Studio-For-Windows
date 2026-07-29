using System.IO;
using System.Security.Cryptography;

namespace FolderThemeStudio.App.Monitoring;

public sealed class MonitoringIconAssetService : IMonitoringIconAssetService
{
    private readonly string root;

    public MonitoringIconAssetService(string? root = null)
    {
        this.root = Path.GetFullPath(root ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio", "monitoring-icons"));
    }

    public async Task<string> PersistAsync(string generatedIcoPath, CancellationToken token)
    {
        var source = Path.GetFullPath(generatedIcoPath);
        var bytes = await File.ReadAllBytesAsync(source, token).ConfigureAwait(false);
        var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        Directory.CreateDirectory(root);
        var destination = Path.Combine(root, hash + ".ico");
        if (File.Exists(destination)) return destination;
        var temporary = destination + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            await File.WriteAllBytesAsync(temporary, bytes, token).ConfigureAwait(false);
            try { File.Move(temporary, destination); }
            catch (IOException) when (File.Exists(destination)) { }
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
        return destination;
    }
}
