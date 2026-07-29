using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FolderThemeStudio.Core.SystemIntegration;

internal readonly record struct AtomicDesktopIniWriteResult(bool Created, string? ExpectedSha256)
{
    internal static AtomicDesktopIniWriteResult SuccessfullyCreated(string expectedSha256) => new(true, expectedSha256);
    internal static AtomicDesktopIniWriteResult DestinationExists() => new(false, null);
}

internal interface ICompatibleFolderFileSystem
{
    bool FileExists(string path);
    bool DirectoryExists(string path);
    FileAttributes GetAttributes(string path);
    void SetAttributes(string path, FileAttributes attributes);
    byte[] ReadAllBytes(string path);
    Task<AtomicDesktopIniWriteResult> WriteDesktopIniAtomicallyAsync(string destinationPath, string contents);
    Task<string> ReplaceDesktopIniAtomicallyAsync(string destinationPath, byte[] bytes);
}

internal sealed class WindowsCompatibleFolderFileSystem : ICompatibleFolderFileSystem
{
    public bool FileExists(string path) => File.Exists(path);
    public bool DirectoryExists(string path) => Directory.Exists(path);
    public FileAttributes GetAttributes(string path) => File.GetAttributes(path);
    public void SetAttributes(string path, FileAttributes attributes) => File.SetAttributes(path, attributes);
    public byte[] ReadAllBytes(string path) => File.ReadAllBytes(path);

    public async Task<AtomicDesktopIniWriteResult> WriteDesktopIniAtomicallyAsync(string destinationPath, string contents)
    {
        var directory = Path.GetDirectoryName(destinationPath)
            ?? throw new InvalidOperationException("Desktop.ini must have a parent folder.");
        var temporaryPath = Path.Combine(directory, $".desktop.ini.{Guid.NewGuid():N}.tmp");
        var encoding = new UnicodeEncoding(false, true, true);
        var preamble = encoding.GetPreamble();
        var contentBytes = encoding.GetBytes(contents);
        var bytes = new byte[preamble.Length + contentBytes.Length];
        preamble.CopyTo(bytes, 0);
        contentBytes.CopyTo(bytes, preamble.Length);
        var expectedHash = Convert.ToHexString(SHA256.HashData(bytes));

        try
        {
            await using (var stream = new FileStream(
                temporaryPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                4096,
                FileOptions.Asynchronous | FileOptions.WriteThrough))
            {
                await stream.WriteAsync(bytes).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                stream.Flush(flushToDisk: true);
            }

            try
            {
                File.Move(temporaryPath, destinationPath);
            }
            catch (IOException) when (File.Exists(destinationPath))
            {
                return AtomicDesktopIniWriteResult.DestinationExists();
            }

            return AtomicDesktopIniWriteResult.SuccessfullyCreated(expectedHash);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    public async Task<string> ReplaceDesktopIniAtomicallyAsync(string destinationPath, byte[] bytes)
    {
        var directory = Path.GetDirectoryName(destinationPath)
            ?? throw new InvalidOperationException("Desktop.ini must have a parent folder.");
        var temporaryPath = Path.Combine(directory, $".desktop.ini.{Guid.NewGuid():N}.tmp");
        var expectedHash = Convert.ToHexString(SHA256.HashData(bytes));
        var originalAttributes = File.GetAttributes(destinationPath);
        try
        {
            await using (var stream = new FileStream(
                temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096,
                FileOptions.Asynchronous | FileOptions.WriteThrough))
            {
                await stream.WriteAsync(bytes).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                stream.Flush(flushToDisk: true);
            }
            File.SetAttributes(destinationPath, FileAttributes.Normal);
            try
            {
                File.Move(temporaryPath, destinationPath, overwrite: true);
            }
            catch
            {
                if (File.Exists(destinationPath)) File.SetAttributes(destinationPath, originalAttributes);
                throw;
            }
            return expectedHash;
        }
        finally
        {
            if (File.Exists(temporaryPath)) File.Delete(temporaryPath);
        }
    }
}
