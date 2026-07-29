using System.IO;
using System.Text;
using System.Text.Json;

namespace FolderThemeStudio.Core.Themes;

public sealed record ThemeLoadResult(
    IReadOnlyList<FolderTheme> Themes,
    IReadOnlyList<string> Diagnostics);

public sealed record ThemeImportResult(
    FolderTheme? Theme,
    IReadOnlyList<string> Errors)
{
    public bool Success => Theme is not null && Errors.Count == 0;
}

public sealed class ThemeStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private readonly string themesDirectory;

    public ThemeStore(string? themesDirectory = null)
    {
        this.themesDirectory = Path.GetFullPath(themesDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio",
            "themes"));
    }

    public async Task<ThemeLoadResult> LoadAsync()
    {
        var themes = new List<FolderTheme> { FolderTheme.IceBlue };
        var diagnostics = new List<string>();
        var themeIds = new HashSet<Guid> { FolderTheme.IceBlue.Id };

        if (!Directory.Exists(themesDirectory))
        {
            return new ThemeLoadResult(themes, diagnostics);
        }

        try
        {
            foreach (var filePath in Directory.EnumerateFiles(themesDirectory, "*.json")
                         .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase))
            {
                var theme = await TryReadThemeAsync(filePath, diagnostics);

                if (theme is null)
                {
                    continue;
                }

                if (!themeIds.Add(theme.Id))
                {
                    diagnostics.Add($"Skipped '{Path.GetFileName(filePath)}' because its theme ID is already in use.");
                    continue;
                }

                themes.Add(theme);
            }
        }
        catch (IOException exception)
        {
            diagnostics.Add($"Could not enumerate stored themes: {exception.Message}");
        }
        catch (UnauthorizedAccessException exception)
        {
            diagnostics.Add($"Could not enumerate stored themes: {exception.Message}");
        }

        return new ThemeLoadResult(themes, diagnostics);
    }

    public async Task SaveAsync(FolderTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);
        ThemeValidator.ThrowIfInvalid(theme);

        if (theme.Id == FolderTheme.IceBlue.Id)
        {
            throw new InvalidOperationException("The built-in theme cannot be saved as a user theme.");
        }

        await WriteThemeAsync(theme, GetThemePath(theme.Id));
    }

    public Task DeleteAsync(Guid themeId)
    {
        if (themeId == FolderTheme.IceBlue.Id)
        {
            throw new InvalidOperationException("The built-in theme cannot be deleted.");
        }

        var themePath = GetThemePath(themeId);
        if (File.Exists(themePath))
        {
            File.Delete(themePath);
        }

        return Task.CompletedTask;
    }

    public async Task ExportAsync(Guid themeId, string destinationPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationPath);

        var theme = (await LoadAsync()).Themes.SingleOrDefault(candidate => candidate.Id == themeId)
            ?? throw new KeyNotFoundException("The requested theme was not found.");

        await WriteThemeAsync(theme, Path.GetFullPath(destinationPath));
    }

    public async Task<ThemeImportResult> ImportAsync(string sourcePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);

        var errors = new List<string>();
        var theme = await TryReadThemeAsync(Path.GetFullPath(sourcePath), errors);

        if (theme is null)
        {
            return new ThemeImportResult(null, errors);
        }

        var existingThemes = await LoadAsync();
        var existingIds = existingThemes.Themes.Select(candidate => candidate.Id).ToHashSet();

        if (existingIds.Contains(theme.Id))
        {
            do
            {
                theme = theme with { Id = Guid.NewGuid() };
            }
            while (existingIds.Contains(theme.Id));
        }

        await SaveAsync(theme);
        return new ThemeImportResult(theme, []);
    }

    private async Task<FolderTheme?> TryReadThemeAsync(string path, ICollection<string> diagnostics)
    {
        try
        {
            await using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 81920,
                FileOptions.Asynchronous | FileOptions.SequentialScan);
            var theme = await JsonSerializer.DeserializeAsync<FolderTheme>(stream, SerializerOptions);

            if (theme is null)
            {
                diagnostics.Add($"Skipped '{Path.GetFileName(path)}' because it does not contain a theme.");
                return null;
            }

            var validation = ThemeValidator.Validate(theme);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    diagnostics.Add(error);
                }
                return null;
            }

            return theme;
        }
        catch (JsonException exception)
        {
            diagnostics.Add($"Skipped '{Path.GetFileName(path)}' because it is not valid JSON: {exception.Message}");
        }
        catch (IOException exception)
        {
            diagnostics.Add($"Could not read '{Path.GetFileName(path)}': {exception.Message}");
        }
        catch (UnauthorizedAccessException exception)
        {
            diagnostics.Add($"Could not read '{Path.GetFileName(path)}': {exception.Message}");
        }

        return null;
    }

    private async Task WriteThemeAsync(FolderTheme theme, string destinationPath)
    {
        var destinationDirectory = Path.GetDirectoryName(destinationPath)
            ?? throw new ArgumentException("The destination path must include a directory.", nameof(destinationPath));
        Directory.CreateDirectory(destinationDirectory);

        var temporaryPath = $"{destinationPath}.{Guid.NewGuid():N}.tmp";

        try
        {
            await using (var stream = new FileStream(
                temporaryPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                await JsonSerializer.SerializeAsync(stream, theme, SerializerOptions);
                await stream.FlushAsync();
                stream.Flush(flushToDisk: true);
            }

            if (File.Exists(destinationPath))
            {
                File.Replace(temporaryPath, destinationPath, destinationBackupFileName: null);
            }
            else
            {
                File.Move(temporaryPath, destinationPath);
            }
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    private string GetThemePath(Guid themeId) =>
        Path.Combine(themesDirectory, $"{themeId:N}.json");
}
