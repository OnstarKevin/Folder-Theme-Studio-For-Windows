using System.IO;
using System.Security.Cryptography;
using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Recovery;

namespace FolderThemeStudio.Core.SystemIntegration;

public sealed record CompatibleApplySummary(
    Guid OperationId,
    int Applied,
    int Skipped,
    int Failed,
    bool Cancelled)
{
    public IReadOnlyList<CompatibleApplyOutcome> Outcomes { get; init; } = [];
    public IReadOnlyList<CompatibleApplyOutcome> Unresolved { get; init; } = [];
}

public enum CompatibleApplyOutcomeKind { Applied, Skipped, Failed }
public sealed record CompatibleApplyOutcome(string Path, CompatibleApplyOutcomeKind Kind, string? Detail);

public sealed record CompatibleRestoreSummary(
    Guid OperationId,
    int Restored,
    int ChangedSinceApply,
    int Missing,
    int Failed,
    bool Cancelled)
{
    public IReadOnlyList<CompatibleRestoreOutcome> Outcomes { get; init; } = [];
    public bool IsFullyRestored => ChangedSinceApply == 0 && Failed == 0 && !Cancelled;
}

public enum CompatibleRestoreOutcomeKind { Restored, Missing, ChangedSinceApply, Failed }
public sealed record CompatibleRestoreOutcome(string Path, CompatibleRestoreOutcomeKind Kind, string? Detail);

public sealed class CompatibleFolderService
{
    private const FileAttributes DesktopIniAttributes = FileAttributes.Hidden | FileAttributes.System;
    private const FileAttributes FolderAttributes = FileAttributes.ReadOnly;
    private readonly BackupService backupService;
    private readonly IOwnedFileDeletionService ownedFileDeletion;
    private readonly ICompatibleFolderFileSystem folderFileSystem;
    private readonly KnownFolderService folderPolicy;

    public CompatibleFolderService(BackupService backupService)
        : this(backupService, new WindowsOwnedFileDeletionService(), new WindowsCompatibleFolderFileSystem(), new KnownFolderService())
    {
    }

    internal static CompatibleFolderService CreateForTesting(
        BackupService backupService,
        IOwnedFileDeletionService ownedFileDeletion) =>
        new(backupService, ownedFileDeletion, new WindowsCompatibleFolderFileSystem(), new KnownFolderService());

    internal static CompatibleFolderService CreateForTesting(
        BackupService backupService,
        IOwnedFileDeletionService ownedFileDeletion,
        ICompatibleFolderFileSystem folderFileSystem) =>
        new(backupService, ownedFileDeletion, folderFileSystem, new KnownFolderService());

    internal static CompatibleFolderService CreateForTesting(
        BackupService backupService,
        IOwnedFileDeletionService ownedFileDeletion,
        ICompatibleFolderFileSystem folderFileSystem,
        KnownFolderService folderPolicy) =>
        new(backupService, ownedFileDeletion, folderFileSystem, folderPolicy);

    private CompatibleFolderService(
        BackupService backupService,
        IOwnedFileDeletionService ownedFileDeletion,
        ICompatibleFolderFileSystem folderFileSystem,
        KnownFolderService folderPolicy)
    {
        this.backupService = backupService ?? throw new ArgumentNullException(nameof(backupService));
        this.ownedFileDeletion = ownedFileDeletion ?? throw new ArgumentNullException(nameof(ownedFileDeletion));
        this.folderFileSystem = folderFileSystem ?? throw new ArgumentNullException(nameof(folderFileSystem));
        this.folderPolicy = folderPolicy ?? throw new ArgumentNullException(nameof(folderPolicy));
    }

    public async Task<CompatibleApplySummary> ApplyAsync(
        IReadOnlyList<FolderPlanItem> plan,
        string icoPath,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentException.ThrowIfNullOrWhiteSpace(icoPath);

        var operationId = Guid.NewGuid();
        var snapshot = new OperationSnapshot(
            operationId,
            DateTimeOffset.UtcNow,
            new RegistryValueSnapshot("3", false, null),
            new RegistryValueSnapshot("4", false, null),
            [],
            false)
        {
            ApplicationMode = ApplicationMode.Compatible
        };
        await backupService.CreateAsync(snapshot, CancellationToken.None).ConfigureAwait(false);

        return await ApplyWithExistingSnapshotAsync(plan, icoPath, operationId, token).ConfigureAwait(false);
    }

    public async Task<CompatibleApplySummary> ApplyWithExistingSnapshotAsync(
        IReadOnlyList<FolderPlanItem> plan,
        string icoPath,
        Guid operationId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentException.ThrowIfNullOrWhiteSpace(icoPath);
        if (operationId == Guid.Empty)
        {
            throw new ArgumentException("An operation identifier is required.", nameof(operationId));
        }

        var fullIcoPath = Path.GetFullPath(icoPath);

        var applied = 0;
        var skipped = 0;
        var failed = 0;
        var cancelled = false;
        var hasUntrackedMutation = false;
        var outcomes = new List<CompatibleApplyOutcome>();
        var unresolved = new List<CompatibleApplyOutcome>();

        foreach (var item in plan)
        {
            if (token.IsCancellationRequested)
            {
                cancelled = true;
                break;
            }

            if (item.Decision != FolderDecision.Allowed)
            {
                skipped++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Skipped, item.Decision.ToString()));
                continue;
            }

            var revalidated = folderPolicy.RevalidateForMutation(item);
            if (!revalidated.IsAllowed || string.IsNullOrWhiteSpace(revalidated.CanonicalPath))
            {
                skipped++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Skipped, revalidated.Detail));
                continue;
            }

            var folderPath = NormalizeDirectoryPath(revalidated.CanonicalPath);
            var desktopIniPath = Path.Combine(folderPath, "desktop.ini");
            var fileOriginallyExisted = folderFileSystem.FileExists(desktopIniPath);
            FileAttributes originalFolderAttributes;
            FileAttributes? originalDesktopIniAttributes = null;
            byte[]? originalDesktopIniBytes = null;
            string expectedHash;
            FolderMutationRecord incompleteMutation;
            try
            {
                originalFolderAttributes = folderFileSystem.GetAttributes(folderPath);
                if (fileOriginallyExisted)
                {
                    originalDesktopIniAttributes = folderFileSystem.GetAttributes(desktopIniPath);
                    originalDesktopIniBytes = folderFileSystem.ReadAllBytes(desktopIniPath);
                    var merge = DesktopIniDocument.Merge(originalDesktopIniBytes, fullIcoPath);
                    if (!merge.Success || merge.Bytes is null)
                        throw new InvalidDataException(merge.Error ?? "Desktop.ini could not be merged safely.");
                    expectedHash = Convert.ToHexString(SHA256.HashData(merge.Bytes));
                    incompleteMutation = new FolderMutationRecord(
                        folderPath, originalFolderAttributes, desktopIniPath, expectedHash, 0, false)
                    {
                        Phase = FolderMutationPhase.OriginalCaptured,
                        FileOriginallyExisted = true,
                        OriginalDesktopIniBase64 = Convert.ToBase64String(originalDesktopIniBytes),
                        OriginalDesktopIniAttributes = originalDesktopIniAttributes
                    };
                    await backupService.AppendMutationAsync(operationId, incompleteMutation, CancellationToken.None).ConfigureAwait(false);
                    await folderFileSystem.ReplaceDesktopIniAtomicallyAsync(desktopIniPath, merge.Bytes).ConfigureAwait(false);
                    await backupService.ResetMutationToFileCreatedAsync(operationId, desktopIniPath, CancellationToken.None).ConfigureAwait(false);
                }
                else
                {
                    var contents = $"[.ShellClassInfo]\r\nIconFile={fullIcoPath}\r\nIconIndex=0\r\n";
                    var writeResult = await folderFileSystem.WriteDesktopIniAtomicallyAsync(desktopIniPath, contents).ConfigureAwait(false);
                    if (!writeResult.Created)
                    {
                        skipped++;
                        outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Skipped, "Desktop.ini appeared concurrently."));
                        continue;
                    }
                    expectedHash = writeResult.ExpectedSha256
                        ?? throw new InvalidOperationException("A successful Desktop.ini write must return its ownership hash.");
                    incompleteMutation = new FolderMutationRecord(
                        folderPath, originalFolderAttributes, desktopIniPath, expectedHash, 0, false);
                }
            }
            catch (Exception exception) when (IsOperationalFailure(exception))
            {
                failed++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Failed, exception.Message));
                continue;
            }

            try
            {
                if (!fileOriginallyExisted)
                    await backupService.AppendMutationAsync(operationId, incompleteMutation, CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception appendException) when (IsOperationalFailure(appendException))
            {
                var cleanupResolved = false;
                try
                {
                    var deletion = ownedFileDeletion.DeleteIfHashMatches(desktopIniPath, expectedHash);
                    cleanupResolved = deletion is OwnedFileDeletionOutcome.Deleted or OwnedFileDeletionOutcome.Missing;
                }
                catch (Exception deletionException) when (IsOperationalFailure(deletionException))
                {
                    cleanupResolved = false;
                }

                if (!cleanupResolved)
                {
                    try
                    {
                        await backupService.AppendMutationAsync(operationId, incompleteMutation, CancellationToken.None).ConfigureAwait(false);
                    }
                    catch (Exception retryException) when (IsOperationalFailure(retryException))
                    {
                        hasUntrackedMutation = true;
                        unresolved.Add(new CompatibleApplyOutcome(
                            item.Path,
                            CompatibleApplyOutcomeKind.Failed,
                            "Desktop.ini could not be safely removed or durably recorded."));
                    }
                }

                failed++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Failed, appendException.Message));
                continue;
            }

            FileAttributes addedFolderAttributes;
            try
            {
                var currentFileAttributes = folderFileSystem.GetAttributes(desktopIniPath);
                folderFileSystem.SetAttributes(desktopIniPath, currentFileAttributes | DesktopIniAttributes);

                var currentFolderAttributes = folderFileSystem.GetAttributes(folderPath);
                addedFolderAttributes = FolderAttributes & ~currentFolderAttributes;
                await backupService.PersistFolderAttributeIntentAsync(
                    operationId,
                    desktopIniPath,
                    addedFolderAttributes,
                    CancellationToken.None).ConfigureAwait(false);
                folderFileSystem.SetAttributes(folderPath, currentFolderAttributes | FolderAttributes);
            }
            catch (Exception exception) when (IsOperationalFailure(exception))
            {
                failed++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Failed, exception.Message));
                continue;
            }

            try
            {
                await backupService.CompleteMutationAsync(
                    operationId,
                    desktopIniPath,
                    addedFolderAttributes,
                    CancellationToken.None).ConfigureAwait(false);
                applied++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Applied, null));
            }
            catch (Exception exception) when (IsOperationalFailure(exception))
            {
                var compensationSucceeded = TryRevertAddedAttributes(folderPath, addedFolderAttributes);
                if (compensationSucceeded)
                {
                    try
                    {
                        await backupService.ResetMutationToFileCreatedAsync(
                            operationId,
                            desktopIniPath,
                            CancellationToken.None).ConfigureAwait(false);
                    }
                    catch (Exception resetException) when (IsOperationalFailure(resetException))
                    {
                        hasUntrackedMutation = true;
                    }
                }
                else
                {
                    if (!TryGetRemainingAddedAttributes(
                        folderPath,
                        addedFolderAttributes,
                        out var remainingAddedFolderAttributes))
                    {
                        hasUntrackedMutation = true;
                    }
                    else
                    {
                        try
                        {
                            await backupService.CompleteMutationAsync(
                                operationId,
                                desktopIniPath,
                                remainingAddedFolderAttributes,
                                CancellationToken.None).ConfigureAwait(false);
                        }
                        catch (Exception retryException) when (IsOperationalFailure(retryException))
                        {
                            hasUntrackedMutation = true;
                        }
                    }
                }

                failed++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Failed, exception.Message));
            }
        }

        if (!cancelled && !hasUntrackedMutation)
        {
            await backupService.CompleteAsync(operationId, CancellationToken.None).ConfigureAwait(false);
        }

        return new CompatibleApplySummary(operationId, applied, skipped, failed, cancelled)
        {
            Outcomes = outcomes,
            Unresolved = unresolved
        };
    }

    public async Task<CompatibleRestoreSummary> RestoreAsync(Guid operationId, CancellationToken token)
    {
        var snapshot = await backupService.LoadAsync(operationId, CancellationToken.None).ConfigureAwait(false);
        var restored = 0;
        var changedSinceApply = 0;
        var missing = 0;
        var failed = 0;
        var cancelled = false;
        var outcomes = new List<CompatibleRestoreOutcome>();

        foreach (var mutation in snapshot.Mutations)
        {
            if (token.IsCancellationRequested)
            {
                cancelled = true;
                break;
            }

            try
            {
                if (mutation.FileOriginallyExisted)
                {
                    if (!folderFileSystem.FileExists(mutation.CreatedFilePath))
                    {
                        changedSinceApply++;
                        outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.ChangedSinceApply, "The original Desktop.ini is now missing."));
                        continue;
                    }

                    var currentBytes = folderFileSystem.ReadAllBytes(mutation.CreatedFilePath);
                    var currentHash = Convert.ToHexString(SHA256.HashData(currentBytes));
                    var originalBytes = Convert.FromBase64String(mutation.OriginalDesktopIniBase64!);
                    var originalHash = Convert.ToHexString(SHA256.HashData(originalBytes));
                    if (string.Equals(currentHash, mutation.ExpectedDesktopIniSha256, StringComparison.OrdinalIgnoreCase))
                    {
                        await folderFileSystem.ReplaceDesktopIniAtomicallyAsync(mutation.CreatedFilePath, originalBytes).ConfigureAwait(false);
                        folderFileSystem.SetAttributes(mutation.CreatedFilePath, mutation.OriginalDesktopIniAttributes!.Value);
                    }
                    else if (!string.Equals(currentHash, originalHash, StringComparison.OrdinalIgnoreCase))
                    {
                        changedSinceApply++;
                        outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.ChangedSinceApply, "Desktop.ini changed since apply."));
                        continue;
                    }

                    if (RequiresFolderAttributeRestore(mutation)) RestoreChangedFolderAttributes(mutation);
                    restored++;
                    outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.Restored, null));
                    continue;
                }

                var deletion = ownedFileDeletion.DeleteIfHashMatches(
                    mutation.CreatedFilePath,
                    mutation.ExpectedDesktopIniSha256);
                if (deletion == OwnedFileDeletionOutcome.Missing)
                {
                    if (RequiresFolderAttributeRestore(mutation)) RestoreChangedFolderAttributes(mutation);
                    missing++;
                    outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.Missing, "Owned Desktop.ini was already missing; owned attributes were resolved."));
                }
                else if (deletion == OwnedFileDeletionOutcome.HashMismatch)
                {
                    changedSinceApply++;
                    outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.ChangedSinceApply, "Desktop.ini changed since apply."));
                    continue;
                }
                else
                {
                    if (RequiresFolderAttributeRestore(mutation)) RestoreChangedFolderAttributes(mutation);
                    restored++;
                    outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.Restored, null));
                }
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException)
            {
                failed++;
                outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.Failed, exception.Message));
            }
        }

        return new CompatibleRestoreSummary(operationId, restored, changedSinceApply, missing, failed, cancelled)
        {
            Outcomes = outcomes
        };
    }

    private static bool RequiresFolderAttributeRestore(FolderMutationRecord mutation) =>
        mutation.Phase is FolderMutationPhase.FolderAttributeIntentPersisted or FolderMutationPhase.Completed;

    private static string NormalizeDirectoryPath(string path) =>
        Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));

    private void RestoreChangedFolderAttributes(FolderMutationRecord mutation)
    {
        if (!folderFileSystem.DirectoryExists(mutation.FolderPath))
        {
            return;
        }

        if (mutation.AddedFolderAttributes == 0)
        {
            return;
        }

        var currentAttributes = folderFileSystem.GetAttributes(mutation.FolderPath);
        folderFileSystem.SetAttributes(
            mutation.FolderPath,
            currentAttributes & ~mutation.AddedFolderAttributes);
    }

    private bool TryRevertAddedAttributes(string path, FileAttributes addedAttributes)
    {
        if (addedAttributes == 0)
        {
            return true;
        }

        try
        {
            var currentAttributes = folderFileSystem.GetAttributes(path);
            folderFileSystem.SetAttributes(path, currentAttributes & ~addedAttributes);
            return true;
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return false;
        }
    }

    private bool TryGetRemainingAddedAttributes(
        string path,
        FileAttributes addedAttributes,
        out FileAttributes remainingAddedAttributes)
    {
        try
        {
            remainingAddedAttributes = folderFileSystem.GetAttributes(path) & addedAttributes;
            return true;
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            remainingAddedAttributes = 0;
            return false;
        }
    }

    private static bool IsOperationalFailure(Exception exception) =>
        exception is IOException
            or UnauthorizedAccessException
            or System.Security.SecurityException
            or InvalidOperationException
            or ArgumentException;
}
