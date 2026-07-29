using System.IO;
using FolderThemeStudio.Core.Recovery;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;
using Microsoft.Win32;

namespace FolderThemeStudio.Core.Tests.Recovery;

public sealed class BackupServiceTests
{
    private const string BackupRoot = @"C:\Backups";

    [Fact]
    public async Task VersionOneExistingRegistryValueWithoutKind_FailsClosed()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var id = SnapshotFactory.First().Id;
        fileSystem.SetFile(@"C:\Backups\latest.v1.json", $$"""{"Version":1,"OperationId":"{{id}}"}""");
        fileSystem.SetFile(
            $@"C:\Backups\{id:D}\snapshot.v1.json",
            "{\"Version\":1,\"Snapshot\":{\"Id\":\"" + id +
            "\",\"CreatedAtUtc\":\"2026-07-27T07:00:00+00:00\",\"RegistryValue3\":{\"Name\":\"3\",\"Existed\":true,\"Value\":\"%SystemRoot%\\\\shell32.dll,3\"},\"RegistryValue4\":{\"Name\":\"4\",\"Existed\":false,\"Value\":null},\"Mutations\":[],\"IsCompleted\":false}}");

        await Assert.ThrowsAsync<InvalidDataException>(() => service.LoadLatestAsync());
    }

    [Fact]
    public async Task SnapshotRoundTrip_PreservesRegistryValueKinds()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var snapshot = SnapshotFactory.First() with
        {
            RegistryValue3 = new RegistryValueSnapshot("3", true, "%SystemRoot%\\shell32.dll,3", RegistryValueKind.ExpandString),
            RegistryValue4 = new RegistryValueSnapshot("4", true, "shell32.dll,4", RegistryValueKind.String)
        };

        await service.CreateAsync(snapshot);

        Assert.Equal(snapshot, await service.LoadLatestAsync());
    }

    [Fact]
    public async Task FileRestoreProcessLock_SerializesIndependentLockInstances()
    {
        using var temp = new TemporaryDirectory();
        var lockPath = Path.Combine(temp.Path, "restore.lock");
        var firstProvider = new FileRestoreProcessLock();
        var secondProvider = new FileRestoreProcessLock();
        await using var first = await firstProvider.AcquireAsync(lockPath, CancellationToken.None);

        var secondTask = secondProvider.AcquireAsync(lockPath, CancellationToken.None);
        await Task.Delay(100);
        Assert.False(secondTask.IsCompleted);

        await first.DisposeAsync();
        await using var second = await secondTask.WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task SnapshotRoundTrip_PreservesOriginalRegistryValuesAndFolderAttributes()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, @"C:\Backups");
        var snapshot = SnapshotFactory.WithRegistryAndFolderMutation();

        await service.CreateAsync(snapshot);

        Assert.Equal(snapshot, await service.LoadLatestAsync());
    }

    [Fact]
    public async Task InterruptedTemporaryWrite_DoesNotReplaceLastGoodSnapshot()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, @"C:\Backups");
        await service.CreateAsync(SnapshotFactory.First());
        fileSystem.FailNextReplace = true;

        await Assert.ThrowsAsync<IOException>(() => service.CreateAsync(SnapshotFactory.Second()));

        Assert.Equal(SnapshotFactory.First().Id, (await service.LoadLatestAsync())!.Id);
    }

    [Fact]
    public async Task RetryingInterruptedCreate_CompletesTheOriginalOperation()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, @"C:\Backups");
        await service.CreateAsync(SnapshotFactory.First());
        fileSystem.FailNextReplace = true;
        await Assert.ThrowsAsync<IOException>(() => service.CreateAsync(SnapshotFactory.Second()));

        await service.CreateAsync(SnapshotFactory.Second());

        Assert.Equal(SnapshotFactory.Second(), await service.LoadLatestAsync());
    }

    [Fact]
    public async Task AppendMutationAsync_PersistsOwnershipImmediately()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, @"C:\Backups");
        var snapshot = SnapshotFactory.First();
        var mutation = SnapshotFactory.FolderMutation();
        await service.CreateAsync(snapshot);

        await service.AppendMutationAsync(snapshot.Id, mutation);

        var saved = await service.LoadLatestAsync();
        Assert.Equal([mutation], saved!.Mutations);
        Assert.Equal("8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F", saved.Mutations.Single().ExpectedDesktopIniSha256);
    }

    [Fact]
    public async Task CompleteAsync_PersistsCompletionState()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, @"C:\Backups");
        var snapshot = SnapshotFactory.First();
        await service.CreateAsync(snapshot);

        await service.CompleteAsync(snapshot.Id);

        Assert.True((await service.LoadLatestAsync())!.IsCompleted);
    }

    [Fact]
    public async Task RestoreCompletion_IsVisibleToANewBackupServiceInstance()
    {
        var fileSystem = new InMemoryFileSystem();
        var firstService = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var secondService = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var snapshot = SnapshotFactory.First();
        await firstService.CreateAsync(snapshot);

        await using (var firstLease = await firstService.AcquireRestoreLeaseAsync(snapshot.Id))
        {
            Assert.False(firstLease.AlreadyRestored);
            await firstLease.MarkCompletedAsync(new DateTimeOffset(2026, 7, 27, 10, 0, 0, TimeSpan.Zero));
        }

        await using var secondLease = await secondService.AcquireRestoreLeaseAsync(snapshot.Id);
        Assert.True(secondLease.AlreadyRestored);
    }

    [Fact]
    public async Task ConcurrentRestoreLeases_SerializeAndObserveCompletion()
    {
        var fileSystem = new InMemoryFileSystem();
        var firstService = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var secondService = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var snapshot = SnapshotFactory.First();
        await firstService.CreateAsync(snapshot);
        await using var firstLease = await firstService.AcquireRestoreLeaseAsync(snapshot.Id);

        var secondLeaseTask = secondService.AcquireRestoreLeaseAsync(snapshot.Id);
        Assert.False(secondLeaseTask.IsCompleted);
        await firstLease.MarkCompletedAsync(new DateTimeOffset(2026, 7, 27, 10, 0, 0, TimeSpan.Zero));
        await firstLease.DisposeAsync();

        await using var secondLease = await secondLeaseTask;
        Assert.True(secondLease.AlreadyRestored);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{\"Version\":4,\"OperationId\":\"11111111-1111-1111-1111-111111111111\",\"CompletedAtUtc\":\"2026-07-27T10:00:00+00:00\"}")]
    [InlineData("{\"Version\":1,\"OperationId\":\"11111111-1111-1111-1111-111111111111\"}")]
    [InlineData("{\"Version\":1,\"OperationId\":\"11111111-1111-1111-1111-111111111111\",\"CompletedAtUtc\":\"2026-07-27T18:00:00+08:00\"}")]
    [InlineData("{\"Version\":1,\"OperationId\":\"22222222-2222-2222-2222-222222222222\",\"CompletedAtUtc\":\"2026-07-27T10:00:00+00:00\"}")]
    public async Task MalformedRestoreMarker_FailsClosed(string markerJson)
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var snapshot = SnapshotFactory.First();
        await service.CreateAsync(snapshot);
        fileSystem.SetFile($@"C:\Backups\{snapshot.Id:D}\restore.v1.json", markerJson);

        await Assert.ThrowsAsync<InvalidDataException>(async () =>
        {
            await using var lease = await service.AcquireRestoreLeaseAsync(snapshot.Id);
        });
    }

    [Fact]
    public async Task CompleteMutationAsync_PersistsOnlyConfirmedAddedFolderAttributeBits()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var snapshot = SnapshotFactory.First();
        var incomplete = SnapshotFactory.FolderMutation() with
        {
            AddedFolderAttributes = 0,
            IsCompleted = false,
            Phase = FolderMutationPhase.FileCreated
        };
        await service.CreateAsync(snapshot);
        await service.AppendMutationAsync(snapshot.Id, incomplete);

        await service.PersistFolderAttributeIntentAsync(
            snapshot.Id,
            incomplete.CreatedFilePath,
            FileAttributes.ReadOnly);

        await service.CompleteMutationAsync(
            snapshot.Id,
            incomplete.CreatedFilePath,
            FileAttributes.ReadOnly);

        var saved = await service.LoadAsync(snapshot.Id);
        var completed = Assert.Single(saved.Mutations);
        Assert.True(completed.IsCompleted);
        Assert.Equal(FileAttributes.ReadOnly, completed.AddedFolderAttributes);
    }

    [Fact]
    public async Task CreateAsync_RejectsIncompleteMutationThatClaimsChangedFolderAttributes()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var mutation = SnapshotFactory.FolderMutation() with
        {
            AddedFolderAttributes = FileAttributes.ReadOnly,
            IsCompleted = false
        };
        var snapshot = SnapshotFactory.First() with { Mutations = [mutation] };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(snapshot));
    }

    [Fact]
    public async Task CreateAsync_RejectsMutationClaimingAnAttributeThatWasAlreadyPresent()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var mutation = SnapshotFactory.FolderMutation() with
        {
            OriginalFolderAttributes = FileAttributes.Directory | FileAttributes.ReadOnly,
            AddedFolderAttributes = FileAttributes.ReadOnly,
            IsCompleted = true
        };
        var snapshot = SnapshotFactory.First() with { Mutations = [mutation] };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(snapshot));
    }

    [Fact]
    public async Task CreateAsync_RejectsMutationWhoseOwnedFileIsOutsideItsFolder()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, @"C:\Backups");
        var snapshot = SnapshotFactory.WithRegistryAndFolderMutation() with
        {
            Mutations = [SnapshotFactory.FolderMutation() with { CreatedFilePath = @"C:\Unrelated\desktop.ini" }]
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(snapshot));
    }

    [Fact]
    public async Task CreateAsync_RejectsRelativeOwnershipPaths()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var snapshot = SnapshotFactory.WithRegistryAndFolderMutation() with
        {
            Mutations = [SnapshotFactory.FolderMutation() with
            {
                FolderPath = @"Pictures\Holiday",
                CreatedFilePath = @"Pictures\Holiday\desktop.ini"
            }]
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(snapshot));
    }

    [Fact]
    public async Task CreateAsync_RejectsAbsoluteDotSegmentOwnershipAliases()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var snapshot = SnapshotFactory.WithRegistryAndFolderMutation() with
        {
            Mutations = [SnapshotFactory.FolderMutation() with
            {
                FolderPath = @"C:\Pictures\Holiday\.",
                CreatedFilePath = @"C:\Pictures\Holiday\.\desktop.ini"
            }]
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(snapshot));
    }

    [Fact]
    public async Task CreateAsync_RejectsTrailingSeparatorOwnershipAliases()
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var snapshot = SnapshotFactory.WithRegistryAndFolderMutation() with
        {
            Mutations = [SnapshotFactory.FolderMutation() with
            {
                FolderPath = "C:\\Pictures\\Holiday\\",
                CreatedFilePath = "C:\\Pictures\\Holiday\\desktop.ini"
            }]
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(snapshot));
    }

    [Theory]
    [MemberData(nameof(IncompleteVersionOneSnapshots))]
    public async Task LoadLatestAsync_IncompleteVersionOneSnapshot_FailsClosed(string snapshotJson)
    {
        var fileSystem = new InMemoryFileSystem();
        var service = BackupService.CreateForTesting(fileSystem, BackupRoot);
        var id = SnapshotFactory.First().Id;
        fileSystem.SetFile(@"C:\Backups\latest.v1.json", $$"""{"Version":1,"OperationId":"{{id}}"}""");
        fileSystem.SetFile($@"C:\Backups\{id:D}\snapshot.v1.json", snapshotJson);

        await Assert.ThrowsAsync<InvalidDataException>(() => service.LoadLatestAsync());
    }

    public static IEnumerable<object[]> IncompleteVersionOneSnapshots()
    {
        const string prefix = "{\"Version\":1,\"Snapshot\":{\"Id\":\"11111111-1111-1111-1111-111111111111\",\"CreatedAtUtc\":\"2026-07-27T07:00:00+00:00\",\"RegistryValue3\":{\"Name\":\"3\",\"Existed\":true,\"Value\":\"shell32.dll,3\"},\"RegistryValue4\":{\"Name\":\"4\",\"Existed\":false,\"Value\":null},";
        const string suffix = "}}";

        yield return [prefix + "\"Mutations\":[]" + suffix]; // Snapshot IsCompleted boolean omitted.
        yield return [prefix + "\"Mutations\":[{\"FolderPath\":\"C:\\\\Pictures\\\\Holiday\",\"CreatedFilePath\":\"C:\\\\Pictures\\\\Holiday\\\\desktop.ini\",\"ExpectedDesktopIniSha256\":\"8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F\",\"IsCompleted\":true}],\"IsCompleted\":false" + suffix]; // OriginalFolderAttributes enum omitted.
        yield return [prefix + "\"Mutations\":[{\"FolderPath\":\"C:\\\\Pictures\\\\Holiday\",\"OriginalFolderAttributes\":48,\"CreatedFilePath\":\"C:\\\\Pictures\\\\Holiday\\\\desktop.ini\",\"ExpectedDesktopIniSha256\":\"8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F\",\"IsCompleted\":true}],\"IsCompleted\":false" + suffix]; // AddedFolderAttributes enum omitted.
        yield return ["{\"Version\":1,\"Snapshot\":{\"Id\":\"11111111-1111-1111-1111-111111111111\",\"RegistryValue3\":{\"Name\":\"3\",\"Existed\":true,\"Value\":\"shell32.dll,3\"},\"RegistryValue4\":{\"Name\":\"4\",\"Existed\":false,\"Value\":null},\"Mutations\":[],\"IsCompleted\":false}}"];
        yield return ["{\"Version\":1,\"Snapshot\":{\"Id\":\"11111111-1111-1111-1111-111111111111\",\"CreatedAtUtc\":\"2026-07-27T07:00:00+00:00\",\"RegistryValue4\":{\"Name\":\"4\",\"Existed\":false,\"Value\":null},\"Mutations\":[],\"IsCompleted\":false}}"];
        yield return [prefix + "\"Mutations\":[{\"FolderPath\":\"C:\\\\Pictures\\\\Holiday\",\"OriginalFolderAttributes\":48,\"CreatedFilePath\":\"C:\\\\Pictures\\\\Holiday\\\\desktop.ini\",\"IsCompleted\":true}],\"IsCompleted\":false" + suffix]; // ExpectedDesktopIniSha256 omitted.
    }
}
