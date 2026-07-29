using System.IO;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Application;

public enum IconSourceKind
{
    BuiltIn,
    ImportedImage
}

public sealed record IconSource
{
    private IconSource(IconSourceKind kind, FolderTheme? theme, string? importedImagePath)
    {
        Kind = kind;
        Theme = theme;
        ImportedImagePath = importedImagePath;
    }

    public IconSourceKind Kind { get; }
    public FolderTheme? Theme { get; }
    public string? ImportedImagePath { get; }

    public static IconSource BuiltIn(FolderTheme theme) =>
        new(IconSourceKind.BuiltIn, theme ?? throw new ArgumentNullException(nameof(theme)), null);

    public static IconSource ImportedImage(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return new IconSource(IconSourceKind.ImportedImage, null, Path.GetFullPath(path));
    }
}
