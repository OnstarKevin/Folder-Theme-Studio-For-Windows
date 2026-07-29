using System.IO;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Collections.Concurrent;

namespace FolderThemeStudio.Core.Recovery;

internal interface IFileSystem
{
    void CreateDirectory(string path);
    bool FileExists(string path);
    Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken);
    Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken);
    void MoveFile(string sourcePath, string destinationPath);
    void ReplaceFile(string sourcePath, string destinationPath);
    void DeleteFile(string path);
}

internal interface IRestoreProcessLock
{
    Task<IAsyncDisposable> AcquireAsync(string lockPath, CancellationToken cancellationToken);
}

internal sealed class FileRestoreProcessLock : IRestoreProcessLock
{
    public async Task<IAsyncDisposable> AcquireAsync(string lockPath, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lockPath);
        var fullPath = Path.GetFullPath(lockPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)
            ?? throw new InvalidOperationException("A restore lock requires a parent directory."));

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var stream = new FileStream(
                    fullPath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    1,
                    FileOptions.WriteThrough);
                return new FileRestoreProcessLease(stream);
            }
            catch (IOException)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(40), cancellationToken).ConfigureAwait(false);
            }
        }
    }

    private sealed class FileRestoreProcessLease(FileStream stream) : IAsyncDisposable
    {
        public ValueTask DisposeAsync() => stream.DisposeAsync();
    }
}

internal sealed class NoOpRestoreProcessLock : IRestoreProcessLock
{
    public Task<IAsyncDisposable> AcquireAsync(string lockPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IAsyncDisposable>(new Lease());
    }

    private sealed class Lease : IAsyncDisposable
    {
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}

public sealed class BackupService
{
    private const int CurrentSchemaVersion = 3;
    private const string SnapshotFileName = "snapshot.v1.json";
    private const string LatestFileName = "latest.v1.json";
    private const string RestoreFileName = "restore.v1.json";
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> RestoreGates = new(StringComparer.OrdinalIgnoreCase);
    private readonly IFileSystem fileSystem;
    private readonly IRestoreProcessLock restoreProcessLock;
    private readonly string backupsRoot;
    private readonly SemaphoreSlim gate = new(1, 1);

    public BackupService()
        : this(new WindowsFileSystem(), Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio",
            "backups"), new FileRestoreProcessLock())
    {
    }

    internal static BackupService CreateForTesting(IFileSystem fileSystem, string backupsRoot) =>
        new(fileSystem, backupsRoot, new NoOpRestoreProcessLock());

    internal static BackupService CreateForTesting(
        IFileSystem fileSystem,
        string backupsRoot,
        IRestoreProcessLock restoreProcessLock) =>
        new(fileSystem, backupsRoot, restoreProcessLock);

    private BackupService(IFileSystem fileSystem, string backupsRoot, IRestoreProcessLock restoreProcessLock)
    {
        this.fileSystem = fileSystem;
        this.backupsRoot = backupsRoot;
        this.restoreProcessLock = restoreProcessLock;
    }

    public async Task CreateAsync(OperationSnapshot snapshot, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        snapshot.Validate();

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshotPath = GetSnapshotPath(snapshot.Id);
            if (fileSystem.FileExists(snapshotPath))
            {
                var existing = await LoadByIdAsync(snapshot.Id, cancellationToken).ConfigureAwait(false);
                if (existing != snapshot)
                {
                    throw new InvalidOperationException($"A different snapshot for operation '{snapshot.Id}' already exists.");
                }

                await WriteJsonAtomicallyAsync(GetLatestPath(), new LatestDocument(CurrentSchemaVersion, snapshot.Id), cancellationToken).ConfigureAwait(false);
                return;
            }

            fileSystem.CreateDirectory(GetOperationDirectory(snapshot.Id));
            await WriteJsonAtomicallyAsync(snapshotPath, new SnapshotDocument(CurrentSchemaVersion, snapshot), cancellationToken).ConfigureAwait(false);
            await WriteJsonAtomicallyAsync(GetLatestPath(), new LatestDocument(CurrentSchemaVersion, snapshot.Id), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task AppendMutationAsync(Guid operationId, FolderMutationRecord mutation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(mutation);
        mutation.Validate();

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshot = await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
            var existing = snapshot.Mutations.SingleOrDefault(candidate => string.Equals(
                candidate.CreatedFilePath,
                mutation.CreatedFilePath,
                StringComparison.OrdinalIgnoreCase));
            if (existing is not null)
            {
                if (existing == mutation)
                {
                    return;
                }

                throw new InvalidOperationException("An operation cannot claim the same Desktop.ini path with different ownership data.");
            }

            var updated = snapshot with { Mutations = snapshot.Mutations.Append(mutation).ToArray() };
            await WriteJsonAtomicallyAsync(GetSnapshotPath(operationId), new SnapshotDocument(CurrentSchemaVersion, updated), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task CompleteAsync(Guid operationId, CancellationToken cancellationToken = default)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshot = await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
            var updated = snapshot with { IsCompleted = true };
            await WriteJsonAtomicallyAsync(GetSnapshotPath(operationId), new SnapshotDocument(CurrentSchemaVersion, updated), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task CompleteMutationAsync(
        Guid operationId,
        string createdFilePath,
        FileAttributes addedFolderAttributes,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(createdFilePath);

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshot = await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
            var matchingIndexes = snapshot.Mutations
                .Select((mutation, index) => (mutation, index))
                .Where(candidate => string.Equals(
                    candidate.mutation.CreatedFilePath,
                    createdFilePath,
                    StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (matchingIndexes.Length != 1)
            {
                throw new InvalidOperationException("A mutation completion must identify exactly one owned Desktop.ini record.");
            }

            var (existing, index) = matchingIndexes[0];
            var completed = existing with
            {
                AddedFolderAttributes = addedFolderAttributes,
                IsCompleted = true,
                Phase = FolderMutationPhase.Completed
            };
            completed.Validate();
            if (existing == completed)
            {
                return;
            }

            if (existing.IsCompleted)
            {
                throw new InvalidOperationException("A completed mutation cannot be completed again with different attribute ownership.");
            }

            var mutations = snapshot.Mutations.ToArray();
            mutations[index] = completed;
            var updated = snapshot with { Mutations = mutations };
            await WriteJsonAtomicallyAsync(GetSnapshotPath(operationId), new SnapshotDocument(CurrentSchemaVersion, updated), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public Task PersistFolderAttributeIntentAsync(
        Guid operationId,
        string createdFilePath,
        FileAttributes addedFolderAttributes,
        CancellationToken cancellationToken = default) =>
        UpdateMutationPhaseAsync(
            operationId,
            createdFilePath,
            FolderMutationPhase.FolderAttributeIntentPersisted,
            addedFolderAttributes,
            cancellationToken);

    public Task ResetMutationToFileCreatedAsync(
        Guid operationId,
        string createdFilePath,
        CancellationToken cancellationToken = default) =>
        UpdateMutationPhaseAsync(
            operationId,
            createdFilePath,
            FolderMutationPhase.FileCreated,
            0,
            cancellationToken);

    private async Task UpdateMutationPhaseAsync(
        Guid operationId,
        string createdFilePath,
        FolderMutationPhase phase,
        FileAttributes addedFolderAttributes,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(createdFilePath);
        if (phase == FolderMutationPhase.Completed)
        {
            throw new ArgumentException("Use CompleteMutationAsync for completed mutations.", nameof(phase));
        }

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshot = await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
            var matches = snapshot.Mutations
                .Select((mutation, index) => (mutation, index))
                .Where(candidate => string.Equals(candidate.mutation.CreatedFilePath, createdFilePath, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (matches.Length != 1)
            {
                throw new InvalidOperationException("A mutation phase update must identify exactly one owned Desktop.ini record.");
            }

            var (existing, index) = matches[0];
            if (existing.IsCompleted)
            {
                throw new InvalidOperationException("A completed mutation cannot return to an incomplete phase.");
            }

            var updatedMutation = existing with
            {
                AddedFolderAttributes = addedFolderAttributes,
                IsCompleted = false,
                Phase = phase
            };
            updatedMutation.Validate();
            if (updatedMutation == existing) return;

            var mutations = snapshot.Mutations.ToArray();
            mutations[index] = updatedMutation;
            var updated = snapshot with { Mutations = mutations };
            await WriteJsonAtomicallyAsync(GetSnapshotPath(operationId), new SnapshotDocument(CurrentSchemaVersion, updated), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<OperationSnapshot?> LoadLatestAsync(CancellationToken cancellationToken = default)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (!fileSystem.FileExists(GetLatestPath()))
            {
                return null;
            }

            var latest = await DeserializeAsync<LatestDocument>(GetLatestPath(), cancellationToken).ConfigureAwait(false);
            EnsureVersion(latest.Version);
            return await LoadByIdAsync(latest.OperationId, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<OperationSnapshot> LoadAsync(Guid operationId, CancellationToken cancellationToken = default)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<RestoreLease> AcquireRestoreLeaseAsync(
        Guid operationId,
        CancellationToken cancellationToken = default)
    {
        if (operationId == Guid.Empty)
        {
            throw new ArgumentException("An operation identifier is required.", nameof(operationId));
        }

        var restoreGate = RestoreGates.GetOrAdd(GetRestoreGateKey(operationId), _ => new SemaphoreSlim(1, 1));
        await restoreGate.WaitAsync(cancellationToken).ConfigureAwait(false);
        IAsyncDisposable? processLease = null;
        try
        {
            fileSystem.CreateDirectory(GetOperationDirectory(operationId));
            processLease = await restoreProcessLock
                .AcquireAsync(GetRestoreLockPath(operationId), cancellationToken)
                .ConfigureAwait(false);
            var restorePath = GetRestorePath(operationId);
            var alreadyRestored = false;
            if (fileSystem.FileExists(restorePath))
            {
                var document = await DeserializeAsync<RestoreDocument>(restorePath, cancellationToken).ConfigureAwait(false);
                EnsureVersion(document.Version);
                if (document.OperationId != operationId)
                {
                    throw new InvalidDataException("The restore marker does not match the requested operation.");
                }

                alreadyRestored = true;
            }

            return new RestoreLease(this, operationId, restoreGate, processLease, alreadyRestored);
        }
        catch
        {
            if (processLease is not null) await processLease.DisposeAsync().ConfigureAwait(false);
            restoreGate.Release();
            throw;
        }
    }

    private async Task MarkRestoreCompletedAsync(
        Guid operationId,
        DateTimeOffset completedAtUtc,
        CancellationToken cancellationToken)
    {
        if (completedAtUtc == default || completedAtUtc.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("A restore completion requires a UTC timestamp.", nameof(completedAtUtc));
        }

        await WriteJsonAtomicallyAsync(
            GetRestorePath(operationId),
            new RestoreDocument(CurrentSchemaVersion, operationId, completedAtUtc),
            cancellationToken).ConfigureAwait(false);
    }

    private async Task<OperationSnapshot> LoadByIdAsync(Guid operationId, CancellationToken cancellationToken)
    {
        if (operationId == Guid.Empty)
        {
            throw new ArgumentException("An operation identifier is required.", nameof(operationId));
        }

        var document = await DeserializeAsync<SnapshotDocument>(GetSnapshotPath(operationId), cancellationToken).ConfigureAwait(false);
        EnsureVersion(document.Version);
        try
        {
            document.Snapshot.Validate();
            return document.Snapshot;
        }
        catch (Exception exception) when (exception is ArgumentException or NullReferenceException)
        {
            throw new InvalidDataException("Backup snapshot contains invalid required values.", exception);
        }
    }

    private async Task<T> DeserializeAsync<T>(string path, CancellationToken cancellationToken)
    {
        var contents = await fileSystem.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
        try
        {
            using var document = JsonDocument.Parse(contents);
            ValidateDocument<T>(document.RootElement);
            return JsonSerializer.Deserialize<T>(contents, JsonOptions)
                ?? throw new InvalidDataException($"Backup data at '{path}' is empty or malformed.");
        }
        catch (Exception exception) when (exception is JsonException or FormatException or ArgumentException or InvalidOperationException)
        {
            throw new InvalidDataException($"Backup data at '{path}' is empty or malformed.", exception);
        }
    }

    private static void ValidateDocument<T>(JsonElement root)
    {
        RequireObject(root, "document");
        var version = ReadInt32(root, "Version");

        if (typeof(T) == typeof(LatestDocument))
        {
            var operationId = RequireString(root, "OperationId");
            if (!Guid.TryParse(operationId, out var parsedOperationId) || parsedOperationId == Guid.Empty)
            {
                throw new InvalidDataException("The latest backup pointer has an invalid operation identifier.");
            }

            return;
        }

        if (typeof(T) == typeof(RestoreDocument))
        {
            var operationId = RequireString(root, "OperationId");
            if (!Guid.TryParse(operationId, out var parsedOperationId) || parsedOperationId == Guid.Empty)
            {
                throw new InvalidDataException("The restore marker has an invalid operation identifier.");
            }

            var completedAt = RequireString(root, "CompletedAtUtc");
            if (!DateTimeOffset.TryParse(
                    completedAt,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind,
                    out var parsedCompletedAt)
                || parsedCompletedAt == default
                || parsedCompletedAt.Offset != TimeSpan.Zero)
            {
                throw new InvalidDataException("The restore marker requires a valid UTC completion timestamp.");
            }

            return;
        }

        if (typeof(T) != typeof(SnapshotDocument))
        {
            throw new InvalidOperationException($"Unsupported backup document type '{typeof(T).Name}'.");
        }

        var snapshot = RequireObjectProperty(root, "Snapshot");
        var snapshotId = RequireString(snapshot, "Id");
        if (!Guid.TryParse(snapshotId, out var parsedSnapshotId) || parsedSnapshotId == Guid.Empty)
        {
            throw new InvalidDataException("A snapshot requires a non-empty identifier.");
        }

        var createdAt = RequireString(snapshot, "CreatedAtUtc");
        if (!DateTimeOffset.TryParse(createdAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out _))
        {
            throw new InvalidDataException("A snapshot requires a valid creation timestamp.");
        }

        ValidateRegistryValue(RequireObjectProperty(snapshot, "RegistryValue3"), version);
        ValidateRegistryValue(RequireObjectProperty(snapshot, "RegistryValue4"), version);
        var mutations = RequireArrayProperty(snapshot, "Mutations");
        foreach (var mutation in mutations.EnumerateArray())
        {
            ValidateMutation(mutation, version);
        }

        RequireBoolean(snapshot, "IsCompleted");
    }

    private static void ValidateRegistryValue(JsonElement registryValue, int version)
    {
        RequireString(registryValue, "Name");
        RequireBoolean(registryValue, "Existed");
        var value = RequireProperty(registryValue, "Value");
        if (value.ValueKind is not (JsonValueKind.String or JsonValueKind.Null))
        {
            throw new InvalidDataException("A registry value must be a string or null.");
        }


        if (version >= 2)
        {
            var kind = RequireProperty(registryValue, "Kind");
            if (kind.ValueKind is not (JsonValueKind.Number or JsonValueKind.Null)
                || (kind.ValueKind == JsonValueKind.Number && !kind.TryGetInt32(out _)))
            {
                throw new InvalidDataException("A registry value kind must be a 32-bit number or null.");
            }
        }
    }

    private static void ValidateMutation(JsonElement mutation, int version)
    {
        RequireObject(mutation, "mutation");
        RequireString(mutation, "FolderPath");
        RequireInt32(mutation, "OriginalFolderAttributes");
        RequireString(mutation, "CreatedFilePath");
        RequireString(mutation, "ExpectedDesktopIniSha256");
        RequireInt32(mutation, "AddedFolderAttributes");
        RequireBoolean(mutation, "IsCompleted");
        if (version >= 2) RequireInt32(mutation, "Phase");
        if (version >= 3)
        {
            RequireBoolean(mutation, "FileOriginallyExisted");
            var original = RequireProperty(mutation, "OriginalDesktopIniBase64");
            if (original.ValueKind is not (JsonValueKind.String or JsonValueKind.Null))
                throw new InvalidDataException("Original Desktop.ini bytes must be a string or null.");
            var attributes = RequireProperty(mutation, "OriginalDesktopIniAttributes");
            if (attributes.ValueKind is not (JsonValueKind.Number or JsonValueKind.Null)
                || (attributes.ValueKind == JsonValueKind.Number && !attributes.TryGetInt32(out _)))
                throw new InvalidDataException("Original Desktop.ini attributes must be a number or null.");
        }
    }

    private static JsonElement RequireObjectProperty(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        RequireObject(property, name);
        return property;
    }

    private static JsonElement RequireArrayProperty(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidDataException($"Backup field '{name}' must be an array.");
        }

        return property;
    }

    private static string RequireString(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind != JsonValueKind.String || property.GetString() is not { } value)
        {
            throw new InvalidDataException($"Backup field '{name}' must be a string.");
        }

        return value;
    }

    private static void RequireInt32(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind != JsonValueKind.Number || !property.TryGetInt32(out _))
        {
            throw new InvalidDataException($"Backup field '{name}' must be a 32-bit number.");
        }
    }

    private static int ReadInt32(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind != JsonValueKind.Number || !property.TryGetInt32(out var value))
        {
            throw new InvalidDataException($"Backup field '{name}' must be a 32-bit number.");
        }

        return value;
    }

    private static void RequireBoolean(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
        {
            throw new InvalidDataException($"Backup field '{name}' must be a boolean.");
        }
    }

    private static JsonElement RequireProperty(JsonElement parent, string name)
    {
        if (!parent.TryGetProperty(name, out var property))
        {
            throw new InvalidDataException($"Backup data is missing required field '{name}'.");
        }

        return property;
    }

    private static void RequireObject(JsonElement element, string name)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException($"Backup {name} must be an object.");
        }
    }

    private async Task WriteJsonAtomicallyAsync<T>(string destinationPath, T document, CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(destinationPath)
            ?? throw new InvalidOperationException("Backup paths must have a parent directory.");
        fileSystem.CreateDirectory(directory);
        var temporaryPath = Path.Combine(directory, $".{Path.GetFileName(destinationPath)}.{Guid.NewGuid():N}.tmp");

        try
        {
            await fileSystem.WriteAllTextAsync(temporaryPath, JsonSerializer.Serialize(document, JsonOptions), cancellationToken).ConfigureAwait(false);
            if (fileSystem.FileExists(destinationPath))
            {
                fileSystem.ReplaceFile(temporaryPath, destinationPath);
            }
            else
            {
                fileSystem.MoveFile(temporaryPath, destinationPath);
            }
        }
        finally
        {
            fileSystem.DeleteFile(temporaryPath);
        }
    }

    private string GetOperationDirectory(Guid operationId) => Path.Combine(backupsRoot, operationId.ToString("D"));
    private string GetSnapshotPath(Guid operationId) => Path.Combine(GetOperationDirectory(operationId), SnapshotFileName);
    private string GetRestorePath(Guid operationId) => Path.Combine(GetOperationDirectory(operationId), RestoreFileName);
    private string GetRestoreLockPath(Guid operationId) => Path.Combine(GetOperationDirectory(operationId), "restore.lock");
    private string GetLatestPath() => Path.Combine(backupsRoot, LatestFileName);
    private string GetRestoreGateKey(Guid operationId) =>
        $"{Path.TrimEndingDirectorySeparator(Path.GetFullPath(backupsRoot))}|{operationId:D}";

    private static void EnsureVersion(int version)
    {
        if (version is < 1 or > CurrentSchemaVersion)
        {
            throw new InvalidDataException($"Unsupported backup schema version '{version}'.");
        }
    }

    private sealed record SnapshotDocument(int Version, OperationSnapshot Snapshot);
    private sealed record LatestDocument(int Version, Guid OperationId);
    private sealed record RestoreDocument(int Version, Guid OperationId, DateTimeOffset CompletedAtUtc);

    public sealed class RestoreLease : IAsyncDisposable
    {
        private readonly BackupService owner;
        private readonly SemaphoreSlim restoreGate;
        private readonly IAsyncDisposable processLease;
        private int disposed;

        internal RestoreLease(
            BackupService owner,
            Guid operationId,
            SemaphoreSlim restoreGate,
            IAsyncDisposable processLease,
            bool alreadyRestored)
        {
            this.owner = owner;
            OperationId = operationId;
            this.restoreGate = restoreGate;
            this.processLease = processLease;
            AlreadyRestored = alreadyRestored;
        }

        public Guid OperationId { get; }
        public bool AlreadyRestored { get; private set; }

        public async Task MarkCompletedAsync(
            DateTimeOffset completedAtUtc,
            CancellationToken cancellationToken = default)
        {
            ObjectDisposedException.ThrowIf(disposed != 0, this);
            if (AlreadyRestored)
            {
                return;
            }

            await owner.MarkRestoreCompletedAsync(OperationId, completedAtUtc, cancellationToken).ConfigureAwait(false);
            AlreadyRestored = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref disposed, 1) == 0)
            {
                try { await processLease.DisposeAsync().ConfigureAwait(false); }
                finally { restoreGate.Release(); }
            }
        }
    }
}

internal sealed class WindowsFileSystem : IFileSystem
{
    public void CreateDirectory(string path) => Directory.CreateDirectory(path);
    public bool FileExists(string path) => File.Exists(path);
    public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken) => File.ReadAllTextAsync(path, cancellationToken);

    public async Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous | FileOptions.WriteThrough);
        await using var writer = new StreamWriter(stream, new UTF8Encoding(false), 4096, leaveOpen: true);
        await writer.WriteAsync(contents.AsMemory(), cancellationToken).ConfigureAwait(false);
        await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    public void MoveFile(string sourcePath, string destinationPath) => File.Move(sourcePath, destinationPath);
    public void ReplaceFile(string sourcePath, string destinationPath) => File.Replace(sourcePath, destinationPath, destinationBackupFileName: null);
    public void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
