using FolderThemeStudio.Core.Themes;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Themes;

public sealed class ThemeValidatorTests
{
    [Fact]
    public void Validate_IceBluePreset_ReturnsValidResult()
    {
        var result = ThemeValidator.Validate(FolderTheme.IceBlue);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    public void Validate_OpacityOutsideUnitRange_ReturnsOpacityError(double opacity)
    {
        var theme = FolderTheme.IceBlue with { Opacity = opacity };

        var result = ThemeValidator.Validate(theme);

        Assert.False(result.IsValid);
        Assert.Contains(nameof(FolderTheme.Opacity), result.Errors);
    }

    public static IEnumerable<object[]> InvalidThemes()
    {
        yield return [FolderTheme.IceBlue with { SchemaVersion = 2 }, nameof(FolderTheme.SchemaVersion)];
        yield return [FolderTheme.IceBlue with { Name = " " }, nameof(FolderTheme.Name)];
        yield return [FolderTheme.IceBlue with { GradientStart = "#12345" }, nameof(FolderTheme.GradientStart)];
        yield return [FolderTheme.IceBlue with { GradientEnd = "blue" }, nameof(FolderTheme.GradientEnd)];
        yield return [FolderTheme.IceBlue with { StrokeColor = "#1234567" }, nameof(FolderTheme.StrokeColor)];
        yield return [FolderTheme.IceBlue with { GlowColor = "#GGGGGG" }, nameof(FolderTheme.GlowColor)];
        yield return [FolderTheme.IceBlue with { GradientAngle = -0.01 }, nameof(FolderTheme.GradientAngle)];
        yield return [FolderTheme.IceBlue with { GradientAngle = 360.01 }, nameof(FolderTheme.GradientAngle)];
        yield return [FolderTheme.IceBlue with { StrokeOpacity = 1.01 }, nameof(FolderTheme.StrokeOpacity)];
        yield return [FolderTheme.IceBlue with { HighlightStrength = -0.01 }, nameof(FolderTheme.HighlightStrength)];
        yield return [FolderTheme.IceBlue with { GlowStrength = 1.01 }, nameof(FolderTheme.GlowStrength)];
        yield return [FolderTheme.IceBlue with { ShadowStrength = -0.01 }, nameof(FolderTheme.ShadowStrength)];
        yield return [FolderTheme.IceBlue with { CornerRadius = -0.01 }, nameof(FolderTheme.CornerRadius)];
        yield return [FolderTheme.IceBlue with { StrokeWidth = -0.01 }, nameof(FolderTheme.StrokeWidth)];
        yield return [FolderTheme.IceBlue with { GlowRadius = -0.01 }, nameof(FolderTheme.GlowRadius)];
        yield return [FolderTheme.IceBlue with { ShadowOffset = -0.01 }, nameof(FolderTheme.ShadowOffset)];
    }

    [Theory]
    [MemberData(nameof(InvalidThemes))]
    public void Validate_InvalidProperty_ReturnsItsExactName(FolderTheme theme, string propertyName)
    {
        var result = ThemeValidator.Validate(theme);

        Assert.False(result.IsValid);
        Assert.Contains(propertyName, result.Errors);
    }

    [Fact]
    public void ThrowIfInvalid_InvalidTheme_ListsInvalidPropertyName()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            ThemeValidator.ThrowIfInvalid(FolderTheme.IceBlue with { Opacity = 1.01 }));

        Assert.Contains(nameof(FolderTheme.Opacity), exception.Message);
    }
}
