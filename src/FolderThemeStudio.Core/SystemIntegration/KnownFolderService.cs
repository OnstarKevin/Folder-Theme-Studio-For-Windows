using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

[assembly: InternalsVisibleTo("FolderThemeStudio.Core.Tests")]

namespace FolderThemeStudio.Core.SystemIntegration;

public enum FolderDecision
{
    Allowed,
    KnownFolder,
    ProtectedRoot,
    NetworkPath,
    ReparsePoint,
    NoWriteAccess,
    ExistingCustomization
}

public sealed record FolderPlanItem(
    string Path,
    FolderDecision Decision,
    string? SelectedRoot = null,
    string? CanonicalPath = null,
    string? CanonicalRoot = null);

internal sealed record FolderMutationValidation(
    bool IsAllowed,
    string? CanonicalPath,
    string Detail);

internal enum DesktopIniStatus { Absent, Present, Indeterminate }

internal sealed record FolderInspection(
    string CanonicalPath,
    FolderDecision? Rejection,
    System.IO.DriveType DriveType,
    System.IO.FileAttributes Attributes,
    DesktopIniStatus DesktopIniStatus,
    bool CanWrite);

internal interface IFolderSystem
{
    FolderInspection Inspect(string path);
    IEnumerable<string> EnumerateDirectories(string path, System.IO.EnumerationOptions options);
}

public sealed class KnownFolderService
{
    private const uint FileAddFile = 0x0002;
    private const uint FileShareReadWriteDelete = 0x0007;
    private const uint OpenExisting = 3;
    private const uint FileFlagBackupSemantics = 0x02000000;

    private static readonly Guid[] KnownFolderIds =
    [
        new("B4BFCC3A-DB2C-424C-B029-7FE99A87C641"), // Desktop
        new("374DE290-123F-4565-9164-39C4925E467B"), // Downloads
        new("FDD39AD0-238F-46AF-ADB4-6C85480369C7"), // Documents
        new("33E28130-4E1E-4676-835A-98395C3BC3BB"), // Pictures
        new("4BD8D571-6D19-48D3-BE97-422220080E43"), // Music
        new("18989B1D-99B5-455B-841C-AB7C74E4DDFC"), // Videos
        new("1777F761-68AD-4D8A-87BD-30B759FA33DD"), // Favorites
        new("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"), // Saved Games
        new("A52BBA46-E9E1-435F-B3D9-28DAA648C0F6"), // OneDrive
        new("DFDF76A2-C82A-4D63-906A-5644AC457385"), // Public
        new("C4AA340D-F20F-4863-AFEF-F87EF2E6BA25"), // Public Desktop
        new("ED4824AF-DCE4-45A8-81E2-FC7965083634"), // Public Documents
        new("3D644C9B-1FB8-4F30-9B45-F670235F79C0"), // Public Downloads
        new("B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5"), // Public Pictures
        new("3214FAB5-9757-4298-BB61-92A9DEAA44FF"), // Public Music
        new("2400183A-6185-49FB-A2D8-4A392A602BA3")  // Public Videos
    ];

    private readonly HashSet<string> knownFolderPaths;
    private readonly string[] protectedRoots;
    private readonly IFolderSystem folderSystem;

    public KnownFolderService()
        : this(new WindowsFolderSystem(), null)
    {
    }

    internal static KnownFolderService CreateForTesting(IFolderSystem folderSystem, IEnumerable<string>? injectedKnownFolderPaths) =>
        new(folderSystem, injectedKnownFolderPaths);

    private KnownFolderService(IFolderSystem folderSystem, IEnumerable<string>? injectedKnownFolderPaths)
    {
        this.folderSystem = folderSystem;
        knownFolderPaths = CanonicalizePaths(ResolveKnownFolderPaths().Concat(injectedKnownFolderPaths ?? []));
        protectedRoots = CanonicalizePaths(GetProtectedRoots()).ToArray();
    }

    public FolderDecision Evaluate(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return FolderDecision.NoWriteAccess;
        }

        FolderInspection inspection;
        try
        {
            inspection = folderSystem.Inspect(path);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException or System.IO.IOException or UnauthorizedAccessException)
        {
            return FolderDecision.NoWriteAccess;
        }

        if (inspection.Rejection is { } rejection)
        {
            return rejection;
        }

        var canonicalPath = inspection.CanonicalPath;
        if (inspection.DriveType == System.IO.DriveType.Network || IsVolumeRoot(canonicalPath) || protectedRoots.Any(root => IsAtOrBelow(canonicalPath, root)))
        {
            return inspection.DriveType == System.IO.DriveType.Network ? FolderDecision.NetworkPath : FolderDecision.ProtectedRoot;
        }

        if (knownFolderPaths.Contains(canonicalPath))
        {
            return FolderDecision.KnownFolder;
        }

        if ((inspection.Attributes & System.IO.FileAttributes.Directory) == 0)
        {
            return FolderDecision.NoWriteAccess;
        }

        if ((inspection.Attributes & System.IO.FileAttributes.ReparsePoint) != 0)
        {
            return FolderDecision.ReparsePoint;
        }

        return (inspection.DesktopIniStatus is DesktopIniStatus.Absent or DesktopIniStatus.Present) && inspection.CanWrite
            ? FolderDecision.Allowed
            : FolderDecision.NoWriteAccess;
    }

    internal FolderMutationValidation RevalidateForMutation(FolderPlanItem item)
    {
        if (string.IsNullOrWhiteSpace(item.SelectedRoot)
            || string.IsNullOrWhiteSpace(item.CanonicalRoot)
            || string.IsNullOrWhiteSpace(item.CanonicalPath))
        {
            return new FolderMutationValidation(false, null, "MissingRootProvenance");
        }

        try
        {
            var rootInspection = folderSystem.Inspect(item.SelectedRoot);
            var targetInspection = folderSystem.Inspect(item.Path);
            var rootDecision = EvaluateBasePolicy(rootInspection, includeCustomization: false);
            if (rootDecision is not (FolderDecision.Allowed or FolderDecision.KnownFolder))
            {
                return new FolderMutationValidation(false, null, rootDecision.ToString());
            }

            var targetDecision = EvaluateBasePolicy(targetInspection, includeCustomization: false);
            if (targetDecision != FolderDecision.Allowed)
            {
                return new FolderMutationValidation(false, null, targetDecision.ToString());
            }

            if (!string.Equals(rootInspection.CanonicalPath, item.CanonicalRoot, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(targetInspection.CanonicalPath, item.CanonicalPath, StringComparison.OrdinalIgnoreCase)
                || !IsAtOrBelow(targetInspection.CanonicalPath, rootInspection.CanonicalPath))
            {
                return new FolderMutationValidation(false, null, FolderDecision.ProtectedRoot.ToString());
            }

            return new FolderMutationValidation(true, targetInspection.CanonicalPath, FolderDecision.Allowed.ToString());
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException or System.IO.IOException or UnauthorizedAccessException or KeyNotFoundException)
        {
            return new FolderMutationValidation(false, null, FolderDecision.NoWriteAccess.ToString());
        }
    }

    private FolderDecision EvaluateBasePolicy(FolderInspection inspection, bool includeCustomization)
    {
        if (inspection.Rejection is { } rejection) return rejection;
        var canonicalPath = inspection.CanonicalPath;
        if (inspection.DriveType == System.IO.DriveType.Network) return FolderDecision.NetworkPath;
        if (IsVolumeRoot(canonicalPath) || protectedRoots.Any(root => IsAtOrBelow(canonicalPath, root))) return FolderDecision.ProtectedRoot;
        if (knownFolderPaths.Contains(canonicalPath)) return FolderDecision.KnownFolder;
        if ((inspection.Attributes & System.IO.FileAttributes.Directory) == 0) return FolderDecision.NoWriteAccess;
        if ((inspection.Attributes & System.IO.FileAttributes.ReparsePoint) != 0) return FolderDecision.ReparsePoint;
        if (inspection.DesktopIniStatus == DesktopIniStatus.Indeterminate || !inspection.CanWrite) return FolderDecision.NoWriteAccess;
        return FolderDecision.Allowed;
    }

    public async IAsyncEnumerable<FolderPlanItem> PlanTreeAsync(
        string root,
        [EnumeratorCancellation] CancellationToken token)
    {
        if (!TryNormalizePath(root, out var normalizedRoot))
        {
            yield return new FolderPlanItem(root, FolderDecision.NoWriteAccess);
            yield break;
        }

        var pending = new Queue<string>();
        pending.Enqueue(normalizedRoot);
        var enumerationOptions = new System.IO.EnumerationOptions
        {
            RecurseSubdirectories = false,
            IgnoreInaccessible = true,
            AttributesToSkip = System.IO.FileAttributes.ReparsePoint,
            ReturnSpecialDirectories = false
        };
        var reportingEnumerationOptions = new System.IO.EnumerationOptions
        {
            RecurseSubdirectories = false,
            IgnoreInaccessible = true,
            AttributesToSkip = 0,
            ReturnSpecialDirectories = false
        };

        while (pending.TryDequeue(out var current))
        {
            token.ThrowIfCancellationRequested();
            var currentDecision = Evaluate(current);
            var isSelectedKnownFolder = currentDecision == FolderDecision.KnownFolder
                && string.Equals(current, normalizedRoot, StringComparison.OrdinalIgnoreCase);

            if (currentDecision != FolderDecision.Allowed && !isSelectedKnownFolder)
            {
                yield return CreatePlanItem(current, currentDecision, normalizedRoot);
                continue;
            }

            var skipped = new List<FolderPlanItem>();
            var reportedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!TryScanChildren(current, enumerationOptions, enqueueAllowedChildren: true, token, pending, skipped, reportedPaths)
                || !TryScanChildren(current, reportingEnumerationOptions, enqueueAllowedChildren: false, token, pending, skipped, reportedPaths))
            {
                yield return CreatePlanItem(
                    current,
                    currentDecision == FolderDecision.Allowed ? FolderDecision.NoWriteAccess : currentDecision,
                    normalizedRoot);
                continue;
            }

            yield return CreatePlanItem(current, currentDecision, normalizedRoot);
            foreach (var skippedItem in skipped) yield return skippedItem;
            await Task.Yield();
        }
    }

    private FolderPlanItem CreatePlanItem(string path, FolderDecision decision, string selectedRoot)
    {
        try
        {
            return new FolderPlanItem(
                path,
                decision,
                selectedRoot,
                folderSystem.Inspect(path).CanonicalPath,
                folderSystem.Inspect(selectedRoot).CanonicalPath);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException or System.IO.IOException or UnauthorizedAccessException)
        {
            return new FolderPlanItem(path, decision, selectedRoot);
        }
    }

    private bool TryScanChildren(
        string current,
        System.IO.EnumerationOptions options,
        bool enqueueAllowedChildren,
        CancellationToken token,
        Queue<string> pending,
        List<FolderPlanItem> skipped,
        HashSet<string> reportedPaths)
    {
        try
        {
            foreach (var child in folderSystem.EnumerateDirectories(current, options))
            {
                token.ThrowIfCancellationRequested();
                var decision = Evaluate(child);
                if (decision == FolderDecision.Allowed)
                {
                    if (enqueueAllowedChildren)
                    {
                        pending.Enqueue(child);
                    }
                }
                else if (reportedPaths.Add(child))
                {
                    skipped.Add(new FolderPlanItem(child, decision));
                }
            }

            return true;
        }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.IO.DirectoryNotFoundException or System.IO.IOException or System.Security.SecurityException)
        {
            return false;
        }
    }

    private static IEnumerable<string> ResolveKnownFolderPaths()
    {
        foreach (var folderId in KnownFolderIds)
        {
            if (SHGetKnownFolderPath(folderId, 0, IntPtr.Zero, out var pathPointer) != 0 || pathPointer == IntPtr.Zero)
            {
                continue;
            }

            try
            {
                var path = Marshal.PtrToStringUni(pathPointer);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    yield return path;
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(pathPointer);
            }
        }
    }

    private static IEnumerable<string> GetProtectedRoots()
    {
        var candidates = new[]
        {
            Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            Environment.SystemDirectory,
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
        };

        return candidates
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(NormalizePath)
            .Distinct(StringComparer.OrdinalIgnoreCase);
    }

    private HashSet<string> CanonicalizePaths(IEnumerable<string> paths)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var path in paths.Where(path => !string.IsNullOrWhiteSpace(path)))
        {
            try
            {
                result.Add(NormalizePath(path));
                result.Add(folderSystem.Inspect(path).CanonicalPath);
            }
            catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException or System.IO.IOException or UnauthorizedAccessException)
            {
                // The lexical path remains protected when physical resolution is unavailable.
            }
        }

        return result;
    }

    internal static bool CanCreateFileIn(string path)
    {
        using var handle = CreateFile(path, FileAddFile, FileShareReadWriteDelete, IntPtr.Zero, OpenExisting, FileFlagBackupSemantics, IntPtr.Zero);
        return !handle.IsInvalid;
    }

    private static bool IsAtOrBelow(string path, string root) =>
        path.Equals(root, StringComparison.OrdinalIgnoreCase)
        || path.StartsWith(root + System.IO.Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
        || path.StartsWith(root + System.IO.Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);

    internal static bool IsNetworkPath(string path) =>
        path.StartsWith(@"\\?\UNC\", StringComparison.OrdinalIgnoreCase)
        || (path.StartsWith(@"\\", StringComparison.Ordinal) && !path.StartsWith(@"\\?\", StringComparison.Ordinal));

    private static bool IsVolumeRoot(string path) =>
        path.Equals(System.IO.Path.GetPathRoot(path), StringComparison.OrdinalIgnoreCase);

    private static bool TryNormalizePath(string path, out string normalizedPath)
    {
        try
        {
            normalizedPath = NormalizePath(path);
            return true;
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException)
        {
            normalizedPath = string.Empty;
            return false;
        }
    }

    internal static string NormalizePath(string path)
    {
        var fullPath = System.IO.Path.GetFullPath(path);
        var root = System.IO.Path.GetPathRoot(fullPath) ?? string.Empty;
        return fullPath.Length > root.Length
            ? fullPath.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar)
            : fullPath;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern int SHGetKnownFolderPath(
        [In] Guid rfid,
        uint dwFlags,
        IntPtr hToken,
        out IntPtr ppszPath);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(
        string fileName,
        uint desiredAccess,
        uint shareMode,
        IntPtr securityAttributes,
        uint creationDisposition,
        uint flagsAndAttributes,
        IntPtr templateFile);
}

internal sealed class WindowsFolderSystem : IFolderSystem
{
    private const uint FileReadAttributes = 0x0080;
    private const uint FileShareReadWriteDelete = 0x0007;
    private const uint OpenExisting = 3;
    private const uint FileFlagBackupSemantics = 0x02000000;

    public FolderInspection Inspect(string path)
    {
        if (KnownFolderService.IsNetworkPath(path))
            return Rejected(path, FolderDecision.NetworkPath);

        string lexicalPath;
        try { lexicalPath = KnownFolderService.NormalizePath(NormalizeExtendedPath(path)); }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException)
        { return Rejected(path, FolderDecision.NoWriteAccess); }

        var root = System.IO.Path.GetPathRoot(lexicalPath);
        try
        {
            if (root is not null && new System.IO.DriveInfo(root).DriveType == System.IO.DriveType.Network)
                return Rejected(lexicalPath, FolderDecision.NetworkPath);
        }
        catch (System.IO.IOException) { return Rejected(lexicalPath, FolderDecision.NoWriteAccess); }

        foreach (var ancestor in Ancestors(lexicalPath))
        {
            System.IO.FileAttributes attributes;
            try { attributes = System.IO.File.GetAttributes(ancestor); }
            catch (Exception exception) when (exception is UnauthorizedAccessException or System.IO.IOException)
            { return Rejected(lexicalPath, FolderDecision.NoWriteAccess); }
            if ((attributes & System.IO.FileAttributes.ReparsePoint) != 0)
                return Rejected(lexicalPath, FolderDecision.ReparsePoint);
        }

        var attributesForPath = System.IO.File.GetAttributes(lexicalPath);
        if ((attributesForPath & System.IO.FileAttributes.Directory) == 0)
            return Rejected(lexicalPath, FolderDecision.NoWriteAccess);
        if (!TryGetFinalPath(lexicalPath, out var canonicalPath))
            return Rejected(lexicalPath, FolderDecision.NoWriteAccess);

        var desktopIniStatus = ProbeDesktopIni(canonicalPath);
        return new FolderInspection(canonicalPath, null, System.IO.DriveType.Fixed, attributesForPath, desktopIniStatus, KnownFolderService.CanCreateFileIn(canonicalPath));
    }

    public IEnumerable<string> EnumerateDirectories(string path, System.IO.EnumerationOptions options) =>
        System.IO.Directory.EnumerateDirectories(path, "*", options);

    private static FolderInspection Rejected(string path, FolderDecision decision) =>
        new(path, decision, System.IO.DriveType.Unknown, 0, DesktopIniStatus.Indeterminate, false);

    private static IEnumerable<string> Ancestors(string path)
    {
        var current = new System.IO.DirectoryInfo(path);
        while (current is not null)
        {
            yield return current.FullName;
            current = current.Parent;
        }
    }

    private static DesktopIniStatus ProbeDesktopIni(string directory)
    {
        try
        {
            using var stream = new System.IO.FileStream(System.IO.Path.Combine(directory, "desktop.ini"), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite | System.IO.FileShare.Delete);
            return DesktopIniStatus.Present;
        }
        catch (System.IO.FileNotFoundException) { return DesktopIniStatus.Absent; }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.IO.IOException) { return DesktopIniStatus.Indeterminate; }
    }

    private static string NormalizeExtendedPath(string path) =>
        path.StartsWith(@"\\?\", StringComparison.Ordinal) ? path[4..] : path;

    private static bool TryGetFinalPath(string path, out string canonicalPath)
    {
        using var handle = CreateFile(path, FileReadAttributes, FileShareReadWriteDelete, IntPtr.Zero, OpenExisting, FileFlagBackupSemantics, IntPtr.Zero);
        if (handle.IsInvalid) { canonicalPath = string.Empty; return false; }
        var buffer = new StringBuilder(512);
        var length = GetFinalPathNameByHandle(handle, buffer, (uint)buffer.Capacity, 0);
        if (length == 0) { canonicalPath = string.Empty; return false; }
        if (length >= buffer.Capacity)
        {
            buffer.EnsureCapacity((int)length + 1);
            length = GetFinalPathNameByHandle(handle, buffer, (uint)buffer.Capacity, 0);
            if (length == 0) { canonicalPath = string.Empty; return false; }
        }
        canonicalPath = KnownFolderService.NormalizePath(NormalizeExtendedPath(buffer.ToString()));
        return true;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(string fileName, uint desiredAccess, uint shareMode, IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes, IntPtr templateFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern uint GetFinalPathNameByHandle(Microsoft.Win32.SafeHandles.SafeFileHandle file, StringBuilder path, uint length, uint flags);
}
