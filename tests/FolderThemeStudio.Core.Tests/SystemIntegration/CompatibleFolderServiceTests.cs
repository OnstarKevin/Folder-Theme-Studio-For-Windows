using System.IO;
using System.Security.Cryptography;
using System.Text;
using FolderThemeStudio.Core.Recovery;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.SystemIntegration;

public sealed class CompatibleFolderServiceTests
{
    [Fact]
    public async Task AllowedFolder_WritesUtf16DesktopIniAndAddsRequiredAttributes()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Applied);
        Assert.Equal(0, result.Skipped);
        Assert.Equal(
            $"[.ShellClassInfo]\r\nIconFile={Path.GetFullPath(fixture.IcoPath)}\r\nIconIndex=0\r\n",
            fixture.ReadDesktopIni());
        Assert.Equal([0xff, 0xfe], File.ReadAllBytes(fixture.DesktopIniPath)[..2]);
        Assert.Equal(
            FileAttributes.Hidden | FileAttributes.System,
            File.GetAttributes(fixture.DesktopIniPath) & (FileAttributes.Hidden | FileAttributes.System));
        Assert.True((File.GetAttributes(fixture.FolderPath) & FileAttributes.ReadOnly) != 0);
    }

    [Fact]
    public async Task NonAllowedPlanEntry_IsSkippedWithoutCreatingDesktopIni()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);
        FolderPlanItem[] plan = [new(fixture.FolderPath, FolderDecision.KnownFolder)];

        var result = await service.ApplyAsync(plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(0, result.Applied);
        Assert.Equal(1, result.Skipped);
        Assert.False(File.Exists(fixture.DesktopIniPath));
        Assert.Equal(fixture.OriginalAttributes, File.GetAttributes(fixture.FolderPath));
    }

    [Fact]
    public async Task ExistingDesktopIni_IsMergedWithoutLosingUnrelatedConfiguration()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);
        fixture.WriteDesktopIni("[.ShellClassInfo]\r\nInfoTip=Keep me\r\n");
        var before = File.ReadAllBytes(fixture.DesktopIniPath);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Applied);
        Assert.Equal(0, result.Skipped);
        Assert.Contains("InfoTip=Keep me", fixture.ReadDesktopIni());
        Assert.Contains($"IconFile={Path.GetFullPath(fixture.IcoPath)}", fixture.ReadDesktopIni());
        Assert.NotEqual(before, File.ReadAllBytes(fixture.DesktopIniPath));
        var mutation = Assert.Single((await fixture.BackupService.LoadAsync(result.OperationId)).Mutations);
        Assert.True(mutation.FileOriginallyExisted);
        Assert.Equal(before, Convert.FromBase64String(mutation.OriginalDesktopIniBase64!));
    }

    [Fact]
    public async Task Restore_ExistingDesktopIniRestoresExactBytesAndAttributes()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);
        fixture.WriteDesktopIni("[.ShellClassInfo]\r\nInfoTip=Keep me\r\nIconFile=old.ico\r\nIconIndex=3\r\n");
        File.SetAttributes(fixture.DesktopIniPath, FileAttributes.Hidden | FileAttributes.Archive);
        var before = File.ReadAllBytes(fixture.DesktopIniPath);
        var beforeAttributes = File.GetAttributes(fixture.DesktopIniPath);

        var apply = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
        var restore = await service.RestoreAsync(apply.OperationId, CancellationToken.None);

        Assert.Equal(1, restore.Restored);
        Assert.Equal(before, File.ReadAllBytes(fixture.DesktopIniPath));
        Assert.Equal(beforeAttributes, File.GetAttributes(fixture.DesktopIniPath));
    }

    [Fact]
    public async Task Restore_DeletesOnlyMatchingOwnedFileAndRestoresAttributes()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);

        var summary = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
        var restore = await service.RestoreAsync(summary.OperationId, CancellationToken.None);

        Assert.Equal(1, restore.Restored);
        Assert.Equal(0, restore.ChangedSinceApply);
        Assert.False(File.Exists(fixture.DesktopIniPath));
        Assert.Equal(fixture.OriginalAttributes, File.GetAttributes(fixture.FolderPath));
    }

    [Fact]
    public async Task Restore_WhenOwnedFileContentChanged_LeavesFileAndReportsConflict()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);
        var summary = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
        File.SetAttributes(fixture.DesktopIniPath, FileAttributes.Normal);
        fixture.WriteDesktopIni("[.ShellClassInfo]\r\nInfoTip=Changed later\r\n");
        var fileAttributesBeforeRestore = File.GetAttributes(fixture.DesktopIniPath);
        var folderAttributesBeforeRestore = File.GetAttributes(fixture.FolderPath);

        var restore = await service.RestoreAsync(summary.OperationId, CancellationToken.None);

        Assert.Equal(0, restore.Restored);
        Assert.Equal(1, restore.ChangedSinceApply);
        Assert.True(File.Exists(fixture.DesktopIniPath));
        Assert.Contains("Changed later", fixture.ReadDesktopIni(), StringComparison.Ordinal);
        Assert.Equal(fileAttributesBeforeRestore, File.GetAttributes(fixture.DesktopIniPath));
        Assert.Equal(folderAttributesBeforeRestore, File.GetAttributes(fixture.FolderPath));
    }

    [Fact]
    public async Task Restore_ClearsOnlyReadOnlyBitAddedByApply()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);
        var summary = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
        File.SetAttributes(fixture.FolderPath, File.GetAttributes(fixture.FolderPath) | FileAttributes.Hidden);

        await service.RestoreAsync(summary.OperationId, CancellationToken.None);

        var restored = File.GetAttributes(fixture.FolderPath);
        Assert.True((restored & FileAttributes.ReadOnly) == 0);
        Assert.True((restored & FileAttributes.Hidden) != 0);
    }

    [Fact]
    public async Task Restore_WhenFileIsReplacedAfterVerification_NeverDeletesReplacementOrChangesFolderAttributes()
    {
        using var fixture = new TemporaryFolderFixture();
        var deletion = new ReplacingAfterVerificationDeletion("[.ShellClassInfo]\r\nInfoTip=User replacement\r\n");
        var service = CompatibleFolderService.CreateForTesting(fixture.BackupService, deletion);
        var summary = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
        var folderAttributesBeforeRestore = File.GetAttributes(fixture.FolderPath);

        var restore = await service.RestoreAsync(summary.OperationId, CancellationToken.None);

        Assert.Equal(0, restore.Restored);
        Assert.Equal(1, restore.ChangedSinceApply);
        Assert.True(File.Exists(fixture.DesktopIniPath));
        Assert.Contains("User replacement", fixture.ReadDesktopIni(), StringComparison.Ordinal);
        Assert.Equal(FileAttributes.Archive, File.GetAttributes(fixture.DesktopIniPath));
        Assert.Equal(folderAttributesBeforeRestore, File.GetAttributes(fixture.FolderPath));
    }

    [Fact]
    public async Task Restore_WhenHandleBoundDeletionFails_PreservesAllFileAndFolderAttributes()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = CompatibleFolderService.CreateForTesting(fixture.BackupService, new ThrowingOwnedFileDeletion());
        var summary = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
        var fileAttributesBeforeRestore = File.GetAttributes(fixture.DesktopIniPath);
        var folderAttributesBeforeRestore = File.GetAttributes(fixture.FolderPath);

        var restore = await service.RestoreAsync(summary.OperationId, CancellationToken.None);

        Assert.Equal(1, restore.Failed);
        Assert.True(File.Exists(fixture.DesktopIniPath));
        Assert.Equal(fileAttributesBeforeRestore, File.GetAttributes(fixture.DesktopIniPath));
        Assert.Equal(folderAttributesBeforeRestore, File.GetAttributes(fixture.FolderPath));
    }

    [Fact]
    public async Task Apply_WhenFolderAttributeWriteFails_PersistsIncompleteOwnershipAndCountsFailure()
    {
        using var fixture = new TemporaryFolderFixture();
        var folderSystem = new ControlledCompatibleFolderFileSystem
        {
            FailWhenSettingAttributesFor = fixture.FolderPath
        };
        var service = CompatibleFolderService.CreateForTesting(
            fixture.BackupService,
            new WindowsOwnedFileDeletionService(),
            folderSystem);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Failed);
        Assert.Equal(0, result.Skipped);
        var mutation = Assert.Single((await fixture.BackupService.LoadAsync(result.OperationId)).Mutations);
        Assert.False(mutation.IsCompleted);
        Assert.Equal(FolderMutationPhase.FolderAttributeIntentPersisted, mutation.Phase);
        Assert.Equal(FileAttributes.ReadOnly, mutation.AddedFolderAttributes);
        Assert.True(File.Exists(fixture.DesktopIniPath));
        Assert.Equal(fixture.OriginalAttributes, File.GetAttributes(fixture.FolderPath));
    }

    [Fact]
    public async Task Restore_IncompleteOwnership_DeletesMatchingFileWithoutChangingFolderAttributes()
    {
        using var fixture = new TemporaryFolderFixture();
        var folderSystem = new ControlledCompatibleFolderFileSystem
        {
            FailWhenSettingAttributesFor = fixture.FolderPath
        };
        var service = CompatibleFolderService.CreateForTesting(
            fixture.BackupService,
            new WindowsOwnedFileDeletionService(),
            folderSystem);
        var apply = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
        File.SetAttributes(
            fixture.FolderPath,
            fixture.OriginalAttributes | FileAttributes.ReadOnly | FileAttributes.Hidden);
        var folderAttributesBeforeRestore = File.GetAttributes(fixture.FolderPath);

        var restore = await service.RestoreAsync(apply.OperationId, CancellationToken.None);

        Assert.Equal(0, restore.Restored);
        Assert.Equal(1, restore.Failed);
        Assert.False(File.Exists(fixture.DesktopIniPath));
        Assert.Equal(folderAttributesBeforeRestore, File.GetAttributes(fixture.FolderPath));
    }

    [Fact]
    public async Task Apply_WhenPostRenameRecoveryDeletionFails_CountsFailedAndKeepsIncompleteOwnership()
    {
        using var fixture = new TemporaryFolderFixture();
        var backupFileSystem = new InMemoryFileSystem { FailNextReplace = true };
        var backup = BackupService.CreateForTesting(backupFileSystem, @"C:\Backups");
        var service = CompatibleFolderService.CreateForTesting(backup, new ThrowingOwnedFileDeletion());

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Failed);
        Assert.Equal(0, result.Skipped);
        var mutation = Assert.Single((await backup.LoadAsync(result.OperationId)).Mutations);
        Assert.False(mutation.IsCompleted);
        Assert.True(File.Exists(fixture.DesktopIniPath));
    }

    [Fact]
    public async Task Apply_WhenDestinationAppearsAtRename_SkipsWithoutClaimingOwnership()
    {
        using var fixture = new TemporaryFolderFixture();
        var folderSystem = new ControlledCompatibleFolderFileSystem { SimulateRenameCollision = true };
        var service = CompatibleFolderService.CreateForTesting(
            fixture.BackupService,
            new WindowsOwnedFileDeletionService(),
            folderSystem);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Skipped);
        Assert.Equal(0, result.Failed);
        Assert.Empty((await fixture.BackupService.LoadAsync(result.OperationId)).Mutations);
        Assert.Contains("Concurrent owner", fixture.ReadDesktopIni(), StringComparison.Ordinal);
        Assert.Equal(fixture.OriginalAttributes, File.GetAttributes(fixture.FolderPath));
    }

    [Fact]
    public async Task Apply_WhenMutationCompletionWriteFails_RevertsOnlyAddedFolderAttributesAndKeepsIncompleteOwnership()
    {
        using var fixture = new TemporaryFolderFixture();
        var backupFileSystem = new InMemoryFileSystem();
        backupFileSystem.FailOnReplaceCalls.Add(3);
        var backup = BackupService.CreateForTesting(backupFileSystem, @"C:\Backups");
        var service = new CompatibleFolderService(backup);
        var attributesBeforeApply = fixture.OriginalAttributes | FileAttributes.Hidden;
        File.SetAttributes(fixture.FolderPath, attributesBeforeApply);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Failed);
        Assert.Equal(attributesBeforeApply, File.GetAttributes(fixture.FolderPath));
        Assert.Equal(
            FileAttributes.Hidden | FileAttributes.System,
            File.GetAttributes(fixture.DesktopIniPath) & (FileAttributes.Hidden | FileAttributes.System));
        var snapshot = await backup.LoadAsync(result.OperationId);
        var mutation = Assert.Single(snapshot.Mutations);
        Assert.False(mutation.IsCompleted);
        Assert.Equal(FolderMutationPhase.FileCreated, mutation.Phase);
        Assert.Equal((FileAttributes)0, mutation.AddedFolderAttributes);
        Assert.True(snapshot.IsCompleted);
    }

    [Fact]
    public async Task Apply_WhenCompletionWriteAndCompensationFail_RetryPersistsActualStateForRestore()
    {
        using var fixture = new TemporaryFolderFixture();
        var backupFileSystem = new InMemoryFileSystem();
        backupFileSystem.FailOnReplaceCalls.Add(3);
        var backup = BackupService.CreateForTesting(backupFileSystem, @"C:\Backups");
        var folderSystem = new ControlledCompatibleFolderFileSystem
        {
            FailWhenRemovingAttributesFor = fixture.FolderPath
        };
        var service = CompatibleFolderService.CreateForTesting(
            backup,
            new WindowsOwnedFileDeletionService(),
            folderSystem);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Failed);
        Assert.True((File.GetAttributes(fixture.FolderPath) & FileAttributes.ReadOnly) != 0);
        var snapshot = await backup.LoadAsync(result.OperationId);
        var mutation = Assert.Single(snapshot.Mutations);
        Assert.True(mutation.IsCompleted);
        Assert.Equal(FileAttributes.ReadOnly, mutation.AddedFolderAttributes);

        var restore = await service.RestoreAsync(result.OperationId, CancellationToken.None);

        Assert.Equal(1, restore.Restored);
        Assert.Equal(fixture.OriginalAttributes, File.GetAttributes(fixture.FolderPath));
    }

    [Fact]
    public async Task Apply_WhenCompletionRetryAlsoFails_DoesNotMarkOperationComplete()
    {
        using var fixture = new TemporaryFolderFixture();
        var backupFileSystem = new InMemoryFileSystem();
        backupFileSystem.FailOnReplaceCalls.UnionWith([3, 4]);
        var backup = BackupService.CreateForTesting(backupFileSystem, @"C:\Backups");
        var folderSystem = new ControlledCompatibleFolderFileSystem
        {
            FailWhenRemovingAttributesFor = fixture.FolderPath
        };
        var service = CompatibleFolderService.CreateForTesting(
            backup,
            new WindowsOwnedFileDeletionService(),
            folderSystem);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Failed);
        var snapshot = await backup.LoadAsync(result.OperationId);
        Assert.False(snapshot.IsCompleted);
        Assert.False(Assert.Single(snapshot.Mutations).IsCompleted);
    }

    [Fact]
    public async Task Apply_WhenCompensationReadbackFails_DoesNotGuessOwnershipOrCompleteOperation()
    {
        using var fixture = new TemporaryFolderFixture();
        var backupFileSystem = new InMemoryFileSystem();
        backupFileSystem.FailOnReplaceCalls.Add(3);
        var backup = BackupService.CreateForTesting(backupFileSystem, @"C:\Backups");
        var folderSystem = new ControlledCompatibleFolderFileSystem
        {
            FailWhenRemovingAttributesFor = fixture.FolderPath,
            FailReadbackAfterRemovalFailureFor = fixture.FolderPath
        };
        var service = CompatibleFolderService.CreateForTesting(
            backup,
            new WindowsOwnedFileDeletionService(),
            folderSystem);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Failed);
        var snapshot = await backup.LoadAsync(result.OperationId);
        Assert.False(snapshot.IsCompleted);
        var mutation = Assert.Single(snapshot.Mutations);
        Assert.False(mutation.IsCompleted);
        Assert.Equal(FolderMutationPhase.FolderAttributeIntentPersisted, mutation.Phase);
        Assert.Equal(FileAttributes.ReadOnly, mutation.AddedFolderAttributes);
    }

    [Fact]
    public async Task Apply_WhenDesktopIniIsReplacedBeforeCompensation_DoesNotTouchReplacement()
    {
        using var fixture = new TemporaryFolderFixture();
        var backupFileSystem = new InMemoryFileSystem();
        backupFileSystem.FailOnReplaceCalls.Add(3);
        const string replacementContents = "[.ShellClassInfo]\r\nInfoTip=Replacement during compensation\r\n";
        var replacementAttributes = FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.System;
        backupFileSystem.OnReplaceFailure = () =>
        {
            File.Move(fixture.DesktopIniPath, fixture.DesktopIniPath + ".original-owned");
            fixture.WriteDesktopIni(replacementContents);
            File.SetAttributes(fixture.DesktopIniPath, replacementAttributes);
        };
        var backup = BackupService.CreateForTesting(backupFileSystem, @"C:\Backups");
        var service = new CompatibleFolderService(backup);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Failed);
        Assert.Equal(replacementContents, fixture.ReadDesktopIni());
        Assert.Equal(replacementAttributes, File.GetAttributes(fixture.DesktopIniPath));
    }

    [Fact]
    public async Task Apply_WhenInitialOwnershipAppendFailsAndCleanupHashMismatches_ReportsUnresolvedPath()
    {
        using var fixture = new TemporaryFolderFixture();
        var backupFileSystem = new InMemoryFileSystem();
        backupFileSystem.FailOnReplaceCalls.UnionWith([1, 2]);
        var backup = BackupService.CreateForTesting(backupFileSystem, @"C:\Backups");
        const string replacementContents = "[.ShellClassInfo]\r\nInfoTip=Replacement during cleanup\r\n";
        var deletion = new ReplacingAfterVerificationDeletion(replacementContents);
        var service = CompatibleFolderService.CreateForTesting(backup, deletion);

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Failed);
        Assert.Contains("Replacement during cleanup", fixture.ReadDesktopIni(), StringComparison.Ordinal);
        var unresolved = Assert.Single(result.Unresolved);
        Assert.Equal(fixture.FolderPath, unresolved.Path);
        Assert.Equal(CompatibleApplyOutcomeKind.Failed, unresolved.Kind);
    }

    [Fact]
    public async Task Apply_WhenCleanupThrowsNonIoAndRetryFails_ReportsUnresolvedPath()
    {
        using var fixture = new TemporaryFolderFixture();
        var backupFileSystem = new InMemoryFileSystem();
        backupFileSystem.FailOnReplaceCalls.UnionWith([1, 2]);
        var backup = BackupService.CreateForTesting(backupFileSystem, @"C:\Backups");
        var service = CompatibleFolderService.CreateForTesting(backup, new ThrowingInvalidOperationDeletion());

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(1, result.Failed);
        Assert.True(File.Exists(fixture.DesktopIniPath));
        var unresolved = Assert.Single(result.Unresolved);
        Assert.Equal(fixture.FolderPath, unresolved.Path);
    }

    [Fact]
    public async Task CancellationBeforeScheduling_DoesNotCreateDesktopIni()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, cancellation.Token);

        Assert.True(result.Cancelled);
        Assert.Equal(0, result.Applied);
        Assert.False(File.Exists(fixture.DesktopIniPath));
    }

    [Fact]
    public async Task Apply_RevalidatesCachedPlanAndRejectsPhysicalPathSubstitution()
    {
        var policyFileSystem = new MutableFolderSystem();
        policyFileSystem.Set(@"C:\Safe", @"C:\Safe");
        policyFileSystem.Set(@"C:\Safe\folder", @"C:\Windows\folder");
        var policy = KnownFolderService.CreateForTesting(policyFileSystem, []);
        var folderFileSystem = new RejectUnexpectedMutationFileSystem();
        var backup = BackupService.CreateForTesting(new InMemoryFileSystem(), @"C:\Backups");
        var service = CompatibleFolderService.CreateForTesting(
            backup,
            new ThrowingOwnedFileDeletion(),
            folderFileSystem,
            policy);
        FolderPlanItem[] plan =
        [
            new(
                @"C:\Safe\folder",
                FolderDecision.Allowed,
                @"C:\Safe",
                @"C:\Safe\folder",
                @"C:\Safe")
        ];

        var result = await service.ApplyAsync(plan, @"C:\Icons\theme.ico", CancellationToken.None);

        Assert.Equal(0, result.Applied);
        Assert.Equal(1, result.Skipped);
        var outcome = Assert.Single(result.Outcomes);
        Assert.Equal(@"C:\Safe\folder", outcome.Path);
        Assert.Equal(CompatibleApplyOutcomeKind.Skipped, outcome.Kind);
        Assert.Equal(FolderDecision.ProtectedRoot.ToString(), outcome.Detail);
        Assert.False(folderFileSystem.WasAccessed);
    }

    [Fact]
    public async Task Apply_RejectsAllowedPlanWithoutSelectedRootProvenance()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);
        FolderPlanItem[] plan = [new(fixture.FolderPath, FolderDecision.Allowed)];

        var result = await service.ApplyAsync(plan, fixture.IcoPath, CancellationToken.None);

        Assert.Equal(0, result.Applied);
        Assert.Equal(1, result.Skipped);
        Assert.False(File.Exists(fixture.DesktopIniPath));
        Assert.Contains(result.Outcomes, outcome =>
            outcome.Path == fixture.FolderPath
            && outcome.Kind == CompatibleApplyOutcomeKind.Skipped
            && outcome.Detail == "MissingRootProvenance");
    }

    [Fact]
    public async Task Apply_CrashAfterFolderAttributeWriteLeavesDurableAttributeIntentForRestartRestore()
    {
        using var fixture = new TemporaryFolderFixture();
        var folderSystem = new ControlledCompatibleFolderFileSystem
        {
            ThrowAfterSettingAttributesFor = fixture.FolderPath
        };
        var service = CompatibleFolderService.CreateForTesting(
            fixture.BackupService,
            new WindowsOwnedFileDeletionService(),
            folderSystem,
            new KnownFolderService());

        var apply = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);

        var mutation = Assert.Single((await fixture.BackupService.LoadAsync(apply.OperationId)).Mutations);
        Assert.Equal(FolderMutationPhase.FolderAttributeIntentPersisted, mutation.Phase);
        Assert.Equal(FileAttributes.ReadOnly, mutation.AddedFolderAttributes);
        Assert.True((File.GetAttributes(fixture.FolderPath) & FileAttributes.ReadOnly) != 0);

        var restarted = new CompatibleFolderService(fixture.BackupService);
        var restore = await restarted.RestoreAsync(apply.OperationId, CancellationToken.None);

        Assert.Equal(1, restore.Restored);
        Assert.Equal(fixture.OriginalAttributes, File.GetAttributes(fixture.FolderPath));
        Assert.Contains(restore.Outcomes, outcome =>
            outcome.Path == fixture.FolderPath && outcome.Kind == CompatibleRestoreOutcomeKind.Restored);
    }

    [Fact]
    public async Task Restore_ReportsExactChangedPathAndLeavesOwnershipUnresolved()
    {
        using var fixture = new TemporaryFolderFixture();
        var service = new CompatibleFolderService(fixture.BackupService);
        var apply = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
        File.SetAttributes(fixture.DesktopIniPath, FileAttributes.Normal);
        fixture.WriteDesktopIni("[.ShellClassInfo]\r\nInfoTip=Changed by user\r\n");

        var restore = await service.RestoreAsync(apply.OperationId, CancellationToken.None);

        Assert.Equal(1, restore.ChangedSinceApply);
        var outcome = Assert.Single(restore.Outcomes);
        Assert.Equal(fixture.FolderPath, outcome.Path);
        Assert.Equal(CompatibleRestoreOutcomeKind.ChangedSinceApply, outcome.Kind);
        Assert.False(restore.IsFullyRestored);
    }

    private sealed class ReplacingAfterVerificationDeletion(string replacementContents) : IOwnedFileDeletionService
    {
        public OwnedFileDeletionOutcome DeleteIfHashMatches(string path, string expectedSha256)
        {
            using (var stream = File.OpenRead(path))
            {
                var verifiedHash = Convert.ToHexString(SHA256.HashData(stream));
                if (!string.Equals(verifiedHash, expectedSha256, StringComparison.OrdinalIgnoreCase))
                {
                    return OwnedFileDeletionOutcome.HashMismatch;
                }
            }

            File.Move(path, path + ".verified-owned");
            File.WriteAllText(path, replacementContents, new UnicodeEncoding(false, true));
            File.SetAttributes(path, FileAttributes.Archive);
            return OwnedFileDeletionOutcome.HashMismatch;
        }
    }

    private sealed class ThrowingOwnedFileDeletion : IOwnedFileDeletionService
    {
        public OwnedFileDeletionOutcome DeleteIfHashMatches(string path, string expectedSha256) =>
            throw new IOException("Simulated handle-bound deletion failure.");
    }

    private sealed class ThrowingInvalidOperationDeletion : IOwnedFileDeletionService
    {
        public OwnedFileDeletionOutcome DeleteIfHashMatches(string path, string expectedSha256) =>
            throw new InvalidOperationException("Simulated non-I/O deletion failure.");
    }

    private sealed class ControlledCompatibleFolderFileSystem : ICompatibleFolderFileSystem
    {
        private readonly ICompatibleFolderFileSystem inner = new WindowsCompatibleFolderFileSystem();
        private bool hasFailedAttributeRemoval;

        internal string? FailWhenSettingAttributesFor { get; init; }
        internal string? FailWhenRemovingAttributesFor { get; init; }
        internal string? FailReadbackAfterRemovalFailureFor { get; init; }
        internal string? ThrowAfterSettingAttributesFor { get; init; }
        internal bool SimulateRenameCollision { get; init; }

        public bool FileExists(string path) => inner.FileExists(path);
        public bool DirectoryExists(string path) => inner.DirectoryExists(path);
        public byte[] ReadAllBytes(string path) => inner.ReadAllBytes(path);
        public FileAttributes GetAttributes(string path)
        {
            if (hasFailedAttributeRemoval
                && string.Equals(path, FailReadbackAfterRemovalFailureFor, StringComparison.OrdinalIgnoreCase))
            {
                throw new IOException("Simulated attribute readback failure.");
            }

            return inner.GetAttributes(path);
        }

        public void SetAttributes(string path, FileAttributes attributes)
        {
            if (string.Equals(path, FailWhenSettingAttributesFor, StringComparison.OrdinalIgnoreCase))
            {
                throw new IOException("Simulated attribute write failure.");
            }

            if (string.Equals(path, FailWhenRemovingAttributesFor, StringComparison.OrdinalIgnoreCase)
                && !hasFailedAttributeRemoval
                && (GetAttributes(path) & ~attributes) != 0)
            {
                hasFailedAttributeRemoval = true;
                throw new IOException("Simulated attribute compensation failure.");
            }

            inner.SetAttributes(path, attributes);

            if (string.Equals(path, ThrowAfterSettingAttributesFor, StringComparison.OrdinalIgnoreCase))
            {
                throw new IOException("Simulated crash after the folder attribute write.");
            }
        }

        public Task<AtomicDesktopIniWriteResult> WriteDesktopIniAtomicallyAsync(string destinationPath, string contents)
        {
            if (!SimulateRenameCollision)
            {
                return inner.WriteDesktopIniAtomicallyAsync(destinationPath, contents);
            }

            File.WriteAllText(
                destinationPath,
                "[.ShellClassInfo]\r\nInfoTip=Concurrent owner\r\n",
                new UnicodeEncoding(false, true));
            return Task.FromResult(AtomicDesktopIniWriteResult.DestinationExists());
        }
        public Task<string> ReplaceDesktopIniAtomicallyAsync(string destinationPath, byte[] bytes) =>
            inner.ReplaceDesktopIniAtomicallyAsync(destinationPath, bytes);
    }

    private sealed class MutableFolderSystem : IFolderSystem
    {
        private readonly Dictionary<string, FolderInspection> inspections = new(StringComparer.OrdinalIgnoreCase);

        internal void Set(string path, string canonicalPath, DesktopIniStatus desktopIni = DesktopIniStatus.Absent) =>
            inspections[path] = new FolderInspection(
                canonicalPath,
                null,
                DriveType.Fixed,
                FileAttributes.Directory,
                desktopIni,
                true);

        public FolderInspection Inspect(string path) => inspections.TryGetValue(path, out var inspection)
            ? inspection
            : new FolderInspection(
                path,
                FolderDecision.NoWriteAccess,
                DriveType.Unknown,
                0,
                DesktopIniStatus.Indeterminate,
                false);
        public IEnumerable<string> EnumerateDirectories(string path, EnumerationOptions options) => [];
    }

    private sealed class RejectUnexpectedMutationFileSystem : ICompatibleFolderFileSystem
    {
        internal bool WasAccessed { get; private set; }

        public bool FileExists(string path) { WasAccessed = true; throw new InvalidOperationException(); }
        public bool DirectoryExists(string path) { WasAccessed = true; throw new InvalidOperationException(); }
        public FileAttributes GetAttributes(string path) { WasAccessed = true; throw new InvalidOperationException(); }
        public void SetAttributes(string path, FileAttributes attributes) { WasAccessed = true; throw new InvalidOperationException(); }
        public byte[] ReadAllBytes(string path) { WasAccessed = true; throw new InvalidOperationException(); }
        public Task<AtomicDesktopIniWriteResult> WriteDesktopIniAtomicallyAsync(string destinationPath, string contents)
        { WasAccessed = true; throw new InvalidOperationException(); }
        public Task<string> ReplaceDesktopIniAtomicallyAsync(string destinationPath, byte[] bytes)
        { WasAccessed = true; throw new InvalidOperationException(); }
    }
}
