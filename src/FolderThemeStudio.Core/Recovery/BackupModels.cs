using System.IO;
using System.Text.RegularExpressions;
using FolderThemeStudio.Core.Application;
using Microsoft.Win32;

namespace FolderThemeStudio.Core.Recovery;

public sealed record RegistryValueSnapshot(
    string Name,
    bool Existed,
    string? Value,
    RegistryValueKind? Kind = null)
{
    internal void Validate(string expectedName)
    {
        if (!string.Equals(Name, expectedName, StringComparison.Ordinal))
        {
            throw new ArgumentException($"The snapshot must describe registry value '{expectedName}'.", nameof(Name));
        }

        if (Existed != (Value is not null) || Existed != (Kind is not null))
        {
            throw new ArgumentException("A registry value is present exactly when its raw value and kind are both saved.", nameof(Value));
        }

        if (Kind is not null && Kind is not (RegistryValueKind.String or RegistryValueKind.ExpandString))
        {
            throw new ArgumentException("Only REG_SZ and REG_EXPAND_SZ folder mappings are supported.", nameof(Kind));
        }
    }
}

public enum FolderMutationPhase
{
    FileCreated,
    FolderAttributeIntentPersisted,
    Completed,
    OriginalCaptured
}

public sealed record FolderMutationRecord(
    string FolderPath,
    FileAttributes OriginalFolderAttributes,
    string CreatedFilePath,
    string ExpectedDesktopIniSha256,
    FileAttributes AddedFolderAttributes,
    bool IsCompleted)
{
    private static readonly Regex Sha256 = new("^[0-9A-Fa-f]{64}$", RegexOptions.CultureInvariant);

    public FolderMutationPhase Phase { get; init; } = IsCompleted
        ? FolderMutationPhase.Completed
        : FolderMutationPhase.FileCreated;
    public bool FileOriginallyExisted { get; init; }
    public string? OriginalDesktopIniBase64 { get; init; }
    public FileAttributes? OriginalDesktopIniAttributes { get; init; }

    internal void Validate()
    {
        if (string.IsNullOrWhiteSpace(FolderPath))
        {
            throw new ArgumentException("A mutation must identify its folder.", nameof(FolderPath));
        }

        if (string.IsNullOrWhiteSpace(CreatedFilePath))
        {
            throw new ArgumentException("A mutation must identify the file created by the tool.", nameof(CreatedFilePath));
        }

        if (!IsDesktopIniInsideFolder(FolderPath, CreatedFilePath))
        {
            throw new ArgumentException("The tool-owned file must be desktop.ini directly inside the recorded folder.", nameof(CreatedFilePath));
        }

        if (!Sha256.IsMatch(ExpectedDesktopIniSha256))
        {
            throw new ArgumentException("The expected Desktop.ini hash must be a SHA-256 value.", nameof(ExpectedDesktopIniSha256));
        }

        if ((AddedFolderAttributes & ~FileAttributes.ReadOnly) != 0)
        {
            throw new ArgumentException("A compatibility mutation may record only the ReadOnly folder attribute bit.", nameof(AddedFolderAttributes));
        }

        if (!Enum.IsDefined(Phase))
        {
            throw new ArgumentException("A mutation phase is invalid.", nameof(Phase));
        }

        if (FileOriginallyExisted)
        {
            if (string.IsNullOrWhiteSpace(OriginalDesktopIniBase64) || OriginalDesktopIniAttributes is null)
                throw new ArgumentException("An existing Desktop.ini mutation requires original bytes and attributes.");
            try { _ = Convert.FromBase64String(OriginalDesktopIniBase64); }
            catch (FormatException exception) { throw new ArgumentException("Original Desktop.ini bytes must be base64.", exception); }
        }
        else if (OriginalDesktopIniBase64 is not null || OriginalDesktopIniAttributes is not null)
        {
            throw new ArgumentException("A newly created Desktop.ini cannot include an original-file snapshot.");
        }

        if ((Phase is FolderMutationPhase.FileCreated or FolderMutationPhase.OriginalCaptured) && AddedFolderAttributes != 0)
        {
            throw new ArgumentException("A file-created mutation cannot claim folder attribute changes.", nameof(AddedFolderAttributes));
        }

        if (IsCompleted != (Phase == FolderMutationPhase.Completed))
        {
            throw new ArgumentException("Mutation completion must agree with its durable phase.", nameof(IsCompleted));
        }

        if ((AddedFolderAttributes & OriginalFolderAttributes) != 0)
        {
            throw new ArgumentException("A mutation cannot claim an attribute bit that was already present.", nameof(AddedFolderAttributes));
        }
    }

    private static bool IsDesktopIniInsideFolder(string folderPath, string createdFilePath)
    {
        try
        {
            var canonicalFolder = GetCanonicalRootedPath(folderPath);
            var canonicalFile = GetCanonicalRootedPath(createdFilePath);
            return canonicalFolder is not null
                && canonicalFile is not null
                && string.Equals(canonicalFile, Path.Combine(canonicalFolder, "desktop.ini"), StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException)
        {
            return false;
        }
    }

    private static string? GetCanonicalRootedPath(string path)
    {
        if (!Path.IsPathRooted(path))
        {
            return null;
        }

        var fullPath = Path.GetFullPath(path);
        var canonicalPath = Path.TrimEndingDirectorySeparator(fullPath);
        return string.Equals(path, canonicalPath, StringComparison.OrdinalIgnoreCase)
            ? canonicalPath
            : null;
    }
}

public sealed record OperationSnapshot(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    RegistryValueSnapshot RegistryValue3,
    RegistryValueSnapshot RegistryValue4,
    IReadOnlyList<FolderMutationRecord> Mutations,
    bool IsCompleted)
{
    public ApplicationMode ApplicationMode { get; init; } = ApplicationMode.Global;

    internal void Validate()
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("An operation snapshot requires a non-empty identifier.", nameof(Id));
        }

        if (!Enum.IsDefined(ApplicationMode))
        {
            throw new ArgumentException("The snapshot application mode is invalid.", nameof(ApplicationMode));
        }

        RegistryValue3.Validate("3");
        RegistryValue4.Validate("4");
        foreach (var mutation in Mutations)
        {
            mutation.Validate();
        }
    }

    public bool Equals(OperationSnapshot? other) =>
        other is not null
        && Id == other.Id
        && CreatedAtUtc == other.CreatedAtUtc
        && EqualityComparer<RegistryValueSnapshot>.Default.Equals(RegistryValue3, other.RegistryValue3)
        && EqualityComparer<RegistryValueSnapshot>.Default.Equals(RegistryValue4, other.RegistryValue4)
        && ApplicationMode == other.ApplicationMode
        && IsCompleted == other.IsCompleted
        && Mutations.SequenceEqual(other.Mutations);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Id);
        hash.Add(CreatedAtUtc);
        hash.Add(RegistryValue3);
        hash.Add(RegistryValue4);
        hash.Add(ApplicationMode);
        hash.Add(IsCompleted);
        foreach (var mutation in Mutations)
        {
            hash.Add(mutation);
        }

        return hash.ToHashCode();
    }
}
