using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FolderThemeStudio.App.Services;

[method: JsonConstructor]
public sealed record FolderCategoryEntry(string FolderPath, string Category, string Note, string GradientStartHex, string GradientEndHex)
{
    public FolderCategoryEntry(string folderPath, string category, string note, string colorHex)
        : this(folderPath, category, note, colorHex, colorHex) { }

    [JsonIgnore]
    public string ColorHex => GradientStartHex;
}

public interface IFolderCategoryCatalog
{
    Task<IReadOnlyList<FolderCategoryEntry>> LoadAsync(CancellationToken token = default);
    Task UpsertAsync(FolderCategoryEntry entry, CancellationToken token = default);
    Task RemoveAsync(string folderPath, CancellationToken token = default);
}

public sealed class JsonFolderCategoryCatalog : IFolderCategoryCatalog
{
    private const int SchemaVersion = 2;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };
    private readonly string path;
    private readonly SemaphoreSlim gate = new(1, 1);

    public JsonFolderCategoryCatalog(string? path = null)
    {
        this.path = Path.GetFullPath(path ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio", "folder-categories.json"));
    }

    public async Task<IReadOnlyList<FolderCategoryEntry>> LoadAsync(CancellationToken token = default)
    {
        await gate.WaitAsync(token).ConfigureAwait(false);
        try { return await LoadCoreAsync(token).ConfigureAwait(false); }
        finally { gate.Release(); }
    }

    public async Task UpsertAsync(FolderCategoryEntry entry, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entry);
        var normalized = Normalize(entry, requireDirectory: true);
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var entries = (await LoadCoreAsync(token).ConfigureAwait(false)).ToList();
            var index = entries.FindIndex(item => PathEquals(item.FolderPath, normalized.FolderPath));
            if (index < 0) entries.Add(normalized);
            else entries[index] = normalized;
            await SaveCoreAsync(entries, token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    public async Task RemoveAsync(string folderPath, CancellationToken token = default)
    {
        var canonicalPath = CanonicalPath(folderPath);
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var entries = (await LoadCoreAsync(token).ConfigureAwait(false))
                .Where(item => !PathEquals(item.FolderPath, canonicalPath))
                .ToList();
            await SaveCoreAsync(entries, token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    private async Task<IReadOnlyList<FolderCategoryEntry>> LoadCoreAsync(CancellationToken token)
    {
        if (!File.Exists(path)) return [];
        try
        {
            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete, 4096, FileOptions.Asynchronous);
            using var json = await JsonDocument.ParseAsync(stream, cancellationToken: token).ConfigureAwait(false);
            if (!json.RootElement.TryGetProperty("schemaVersion", out var versionElement))
                throw new InvalidDataException("The folder category catalog version is missing.");
            var version = versionElement.GetInt32();
            IReadOnlyList<FolderCategoryEntry> entries;
            if (version == 1)
            {
                var old = json.RootElement.Deserialize<LegacyCatalogDocument>(JsonOptions);
                if (old?.Entries is null) throw new InvalidDataException("The folder category catalog contents are invalid.");
                entries = old.Entries.Select(entry => new FolderCategoryEntry(entry.FolderPath, entry.Category, entry.Note, entry.ColorHex)).ToArray();
            }
            else if (version == SchemaVersion)
            {
                entries = json.RootElement.Deserialize<CatalogDocument>(JsonOptions)?.Entries
                    ?? throw new InvalidDataException("The folder category catalog contents are invalid.");
            }
            else throw new InvalidDataException("The folder category catalog version is unsupported.");

            var normalized = entries.Select(entry => Normalize(entry, requireDirectory: false)).ToArray();
            if (normalized.Select(item => item.FolderPath).Distinct(StringComparer.OrdinalIgnoreCase).Count() != normalized.Length)
                throw new InvalidDataException("The folder category catalog contains duplicate folder paths.");
            return normalized.OrderBy(item => item.FolderPath, StringComparer.OrdinalIgnoreCase).ToArray();
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException("The folder category catalog is malformed.", exception);
        }
    }

    private async Task SaveCoreAsync(IReadOnlyList<FolderCategoryEntry> entries, CancellationToken token)
    {
        var directory = Path.GetDirectoryName(path)!;
        Directory.CreateDirectory(directory);
        var temporaryPath = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            await using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                var document = new CatalogDocument(SchemaVersion, entries.OrderBy(item => item.FolderPath, StringComparer.OrdinalIgnoreCase).ToArray());
                await JsonSerializer.SerializeAsync(stream, document, JsonOptions, token).ConfigureAwait(false);
                await stream.FlushAsync(token).ConfigureAwait(false);
            }
            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath)) File.Delete(temporaryPath);
        }
    }

    private static FolderCategoryEntry Normalize(FolderCategoryEntry entry, bool requireDirectory)
    {
        ArgumentNullException.ThrowIfNull(entry);
        var canonicalPath = CanonicalPath(entry.FolderPath);
        var category = entry.Category?.Trim() ?? string.Empty;
        var note = entry.Note?.Trim() ?? string.Empty;
        var start = entry.GradientStartHex?.Trim() ?? string.Empty;
        var end = entry.GradientEndHex?.Trim() ?? string.Empty;
        if (requireDirectory && !Directory.Exists(canonicalPath))
            throw new DirectoryNotFoundException("Choose an existing folder for its category.");
        if (category.Length is < 1 or > 32)
            throw new ArgumentException("Category must contain between 1 and 32 characters.", nameof(entry));
        if (note.Length > 140)
            throw new ArgumentException("The category note cannot exceed 140 characters.", nameof(entry));
        if (!IsHex(start) || !IsHex(end))
            throw new ArgumentException("The accent must be a six-digit hexadecimal color such as #2868D7.", nameof(entry));
        return new FolderCategoryEntry(canonicalPath, category, note, start.ToUpperInvariant(), end.ToUpperInvariant());
    }

    private static bool IsHex(string color) => color.Length == 7 && color[0] == '#' && color.AsSpan(1).ToString().All(Uri.IsHexDigit);

    private static string CanonicalPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("A folder path is required.", nameof(path));
        return Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
    }

    private static bool PathEquals(string left, string right) =>
        string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

    private sealed record CatalogDocument(int SchemaVersion, IReadOnlyList<FolderCategoryEntry> Entries);
    private sealed record LegacyCatalogDocument(int SchemaVersion, IReadOnlyList<LegacyEntry> Entries);
    private sealed record LegacyEntry(string FolderPath, string Category, string Note, string ColorHex);
}
