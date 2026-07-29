namespace FolderThemeStudio.Core.Themes;

public sealed record FolderTheme(
    int SchemaVersion,
    Guid Id,
    string Name,
    string GradientStart,
    string GradientEnd,
    double GradientAngle,
    double Opacity,
    double CornerRadius,
    string StrokeColor,
    double StrokeOpacity,
    double StrokeWidth,
    double HighlightStrength,
    string GlowColor,
    double GlowStrength,
    double GlowRadius,
    double ShadowStrength,
    double ShadowOffset)
{
    public const int CurrentSchemaVersion = 1;

    public static FolderTheme IceBlue { get; } = new(
        1, Guid.Parse("7fcb41ba-7c24-4914-9353-2f90be4bc4f0"), "玻璃霓虹 / 冰川蓝",
        "#65D9FF", "#478CFF", 135, .82, 16, "#B9F5FF", .90, 2,
        .58, "#43BFFF", .45, 8, .18, 6);
}

public sealed record ThemeValidationResult(IReadOnlyList<string> Errors)
{
    public bool IsValid => Errors.Count == 0;
}
