using System.Text.RegularExpressions;

namespace FolderThemeStudio.Core.Themes;

public static class ThemeValidator
{
    private static readonly Regex HexColorPattern = new(
        "^#[0-9A-Fa-f]{6}$",
        RegexOptions.CultureInvariant);

    public static ThemeValidationResult Validate(FolderTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        var errors = new List<string>();

        ValidateSchemaVersion(theme, errors);
        ValidateName(theme, errors);
        ValidateColors(theme, errors);
        ValidateAngle(theme, errors);
        ValidateUnitRangeProperties(theme, errors);
        ValidateGeometryProperties(theme, errors);

        return new ThemeValidationResult(errors);
    }

    public static void ThrowIfInvalid(FolderTheme theme)
    {
        var result = Validate(theme);

        if (!result.IsValid)
        {
            throw new ArgumentException(
                $"The theme contains invalid properties: {string.Join(", ", result.Errors)}.",
                nameof(theme));
        }
    }

    private static void ValidateSchemaVersion(FolderTheme theme, ICollection<string> errors)
    {
        if (theme.SchemaVersion != FolderTheme.CurrentSchemaVersion)
        {
            errors.Add(nameof(FolderTheme.SchemaVersion));
        }
    }

    private static void ValidateName(FolderTheme theme, ICollection<string> errors)
    {
        if (string.IsNullOrWhiteSpace(theme.Name))
        {
            errors.Add(nameof(FolderTheme.Name));
        }
    }

    private static void ValidateColors(FolderTheme theme, ICollection<string> errors)
    {
        AddIfInvalidColor(theme.GradientStart, nameof(FolderTheme.GradientStart), errors);
        AddIfInvalidColor(theme.GradientEnd, nameof(FolderTheme.GradientEnd), errors);
        AddIfInvalidColor(theme.StrokeColor, nameof(FolderTheme.StrokeColor), errors);
        AddIfInvalidColor(theme.GlowColor, nameof(FolderTheme.GlowColor), errors);
    }

    private static void ValidateAngle(FolderTheme theme, ICollection<string> errors)
    {
        if (!IsInRange(theme.GradientAngle, 0, 360))
        {
            errors.Add(nameof(FolderTheme.GradientAngle));
        }
    }

    private static void ValidateUnitRangeProperties(FolderTheme theme, ICollection<string> errors)
    {
        AddIfOutsideUnitRange(theme.Opacity, nameof(FolderTheme.Opacity), errors);
        AddIfOutsideUnitRange(theme.StrokeOpacity, nameof(FolderTheme.StrokeOpacity), errors);
        AddIfOutsideUnitRange(theme.HighlightStrength, nameof(FolderTheme.HighlightStrength), errors);
        AddIfOutsideUnitRange(theme.GlowStrength, nameof(FolderTheme.GlowStrength), errors);
        AddIfOutsideUnitRange(theme.ShadowStrength, nameof(FolderTheme.ShadowStrength), errors);
    }

    private static void ValidateGeometryProperties(FolderTheme theme, ICollection<string> errors)
    {
        AddIfNegative(theme.CornerRadius, nameof(FolderTheme.CornerRadius), errors);
        AddIfNegative(theme.StrokeWidth, nameof(FolderTheme.StrokeWidth), errors);
        AddIfNegative(theme.GlowRadius, nameof(FolderTheme.GlowRadius), errors);
        AddIfNegative(theme.ShadowOffset, nameof(FolderTheme.ShadowOffset), errors);
    }

    private static void AddIfInvalidColor(string? color, string propertyName, ICollection<string> errors)
    {
        if (color is null || !HexColorPattern.IsMatch(color))
        {
            errors.Add(propertyName);
        }
    }

    private static void AddIfOutsideUnitRange(double value, string propertyName, ICollection<string> errors)
    {
        if (!IsInRange(value, 0, 1))
        {
            errors.Add(propertyName);
        }
    }

    private static void AddIfNegative(double value, string propertyName, ICollection<string> errors)
    {
        if (!double.IsFinite(value) || value < 0)
        {
            errors.Add(propertyName);
        }
    }

    private static bool IsInRange(double value, double minimum, double maximum) =>
        double.IsFinite(value) && value >= minimum && value <= maximum;
}
