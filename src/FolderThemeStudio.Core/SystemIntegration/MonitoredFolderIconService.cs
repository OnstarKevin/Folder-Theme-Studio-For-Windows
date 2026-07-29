using System.IO;

namespace FolderThemeStudio.Core.SystemIntegration;

public sealed record MonitoredApplyOutcome(bool Success, string Path, string? Error)
{
    public static MonitoredApplyOutcome Succeeded(string path) => new(true, path, null);
    public static MonitoredApplyOutcome Failure(string path, string error) => new(false, path, error);
}

public interface IMonitoredFolderIconService
{
    Task<MonitoredApplyOutcome> ApplyAsync(string folderPath, string icoPath, CancellationToken token);
}

public sealed class MonitoredFolderIconService : IMonitoredFolderIconService
{
    private readonly KnownFolderService policy = new();
    private readonly ICompatibleFolderFileSystem files = new WindowsCompatibleFolderFileSystem();

    public async Task<MonitoredApplyOutcome> ApplyAsync(string folderPath, string icoPath, CancellationToken token)
    {
        try
        {
            token.ThrowIfCancellationRequested();
            var canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(folderPath));
            if (policy.Evaluate(canonical) != FolderDecision.Allowed)
                return MonitoredApplyOutcome.Failure(canonical, "Folder is not an allowed ordinary folder.");
            var fullIco = Path.GetFullPath(icoPath);
            if (!File.Exists(fullIco)) return MonitoredApplyOutcome.Failure(canonical, "Monitoring icon asset is missing.");

            var ini = Path.Combine(canonical, "desktop.ini");
            var existed = files.FileExists(ini);
            var merge = DesktopIniDocument.Merge(existed ? files.ReadAllBytes(ini) : null, fullIco);
            if (!merge.Success || merge.Bytes is null)
                return MonitoredApplyOutcome.Failure(canonical, merge.Error ?? "Desktop.ini could not be merged.");

            if (existed)
            {
                await files.ReplaceDesktopIniAtomicallyAsync(ini, merge.Bytes).ConfigureAwait(false);
            }
            else
            {
                var text = System.Text.Encoding.UTF8.GetString(merge.Bytes);
                var created = await files.WriteDesktopIniAtomicallyAsync(ini, text).ConfigureAwait(false);
                if (!created.Created)
                {
                    merge = DesktopIniDocument.Merge(files.ReadAllBytes(ini), fullIco);
                    if (!merge.Success || merge.Bytes is null)
                        return MonitoredApplyOutcome.Failure(canonical, merge.Error ?? "Concurrent Desktop.ini could not be merged.");
                    await files.ReplaceDesktopIniAtomicallyAsync(ini, merge.Bytes).ConfigureAwait(false);
                }
            }

            files.SetAttributes(ini, files.GetAttributes(ini) | FileAttributes.Hidden | FileAttributes.System);
            files.SetAttributes(canonical, files.GetAttributes(canonical) | FileAttributes.ReadOnly);
            return MonitoredApplyOutcome.Succeeded(canonical);
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException or ArgumentException or InvalidOperationException)
        {
            return MonitoredApplyOutcome.Failure(folderPath, exception.Message);
        }
    }
}
