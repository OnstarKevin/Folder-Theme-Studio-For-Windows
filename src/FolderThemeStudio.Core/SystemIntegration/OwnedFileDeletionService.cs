using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Win32.SafeHandles;

namespace FolderThemeStudio.Core.SystemIntegration;

internal enum OwnedFileDeletionOutcome
{
    Deleted,
    Missing,
    HashMismatch
}

internal interface IOwnedFileDeletionService
{
    OwnedFileDeletionOutcome DeleteIfHashMatches(string path, string expectedSha256);
}

internal sealed class WindowsOwnedFileDeletionService : IOwnedFileDeletionService
{
    private const uint GenericRead = 0x80000000;
    private const uint DeleteAccess = 0x00010000;
    private const uint FileShareRead = 0x00000001;
    private const uint OpenExisting = 3;
    private const int FileDispositionInfoClass = 4;
    private const int ErrorFileNotFound = 2;
    private const int ErrorPathNotFound = 3;

    public OwnedFileDeletionOutcome DeleteIfHashMatches(string path, string expectedSha256)
    {
        using var handle = CreateFile(
            path,
            GenericRead | DeleteAccess,
            FileShareRead,
            IntPtr.Zero,
            OpenExisting,
            0,
            IntPtr.Zero);
        if (handle.IsInvalid)
        {
            var error = Marshal.GetLastWin32Error();
            if (error is ErrorFileNotFound or ErrorPathNotFound)
            {
                return OwnedFileDeletionOutcome.Missing;
            }

            throw new IOException($"Could not open the owned Desktop.ini for verified deletion: {new Win32Exception(error).Message}", new Win32Exception(error));
        }

        using var stream = new FileStream(handle, FileAccess.Read);
        var actualSha256 = Convert.ToHexString(SHA256.HashData(stream));
        if (!string.Equals(actualSha256, expectedSha256, StringComparison.OrdinalIgnoreCase))
        {
            return OwnedFileDeletionOutcome.HashMismatch;
        }

        var disposition = new FileDispositionInfo { DeleteFile = true };
        if (!SetFileInformationByHandle(
            handle,
            FileDispositionInfoClass,
            ref disposition,
            (uint)Marshal.SizeOf<FileDispositionInfo>()))
        {
            var error = Marshal.GetLastWin32Error();
            throw new IOException($"Could not mark the verified Desktop.ini handle for deletion: {new Win32Exception(error).Message}", new Win32Exception(error));
        }

        return OwnedFileDeletionOutcome.Deleted;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct FileDispositionInfo
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool DeleteFile;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateFile(
        string fileName,
        uint desiredAccess,
        uint shareMode,
        IntPtr securityAttributes,
        uint creationDisposition,
        uint flagsAndAttributes,
        IntPtr templateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetFileInformationByHandle(
        SafeFileHandle file,
        int fileInformationClass,
        ref FileDispositionInfo fileInformation,
        uint bufferSize);
}
