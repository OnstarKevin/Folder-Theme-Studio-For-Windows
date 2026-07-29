using System.Text.RegularExpressions;

namespace FolderThemeStudio.App.Settings;

public sealed record FolderPalette(string GradientStart, string GradientEnd, string Stroke, string Glow)
{
    public static FolderPalette Default { get; } = new("#65D9FF", "#478CFF", "#B9F5FF", "#43BFFF");
}

public sealed record AppSettings(
    string Language,
    bool TutorialCompleted,
    FolderPalette Palette,
    IReadOnlyList<string> RecentColors,
    bool StartWithWindows = true,
    bool CloseToTray = true,
    bool MonitoringPaused = false)
{
    private static readonly Regex HexColor = new("^#[0-9A-Fa-f]{6}$", RegexOptions.CultureInvariant);

    public static AppSettings Default { get; } = new("zh-CN", false, FolderPalette.Default, Array.Empty<string>());

    public static bool TryNormalize(AppSettings? value, out AppSettings normalized)
    {
        normalized = Default;
        if (value is null || value.Language is not ("zh-CN" or "en-US") || value.Palette is null)
        {
            return false;
        }

        var paletteColors = new[]
        {
            value.Palette.GradientStart,
            value.Palette.GradientEnd,
            value.Palette.Stroke,
            value.Palette.Glow,
        };
        if (paletteColors.Any(color => color is null || !HexColor.IsMatch(color)))
        {
            return false;
        }

        var recent = (value.RecentColors ?? Array.Empty<string>())
            .Where(color => color is not null && HexColor.IsMatch(color))
            .Select(color => color.ToUpperInvariant())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(12)
            .ToArray();
        normalized = new AppSettings(
            value.Language,
            value.TutorialCompleted,
            new FolderPalette(
                value.Palette.GradientStart.ToUpperInvariant(),
                value.Palette.GradientEnd.ToUpperInvariant(),
                value.Palette.Stroke.ToUpperInvariant(),
                value.Palette.Glow.ToUpperInvariant()),
            recent,
            value.StartWithWindows,
            value.CloseToTray,
            value.MonitoringPaused);
        return true;
    }
}
