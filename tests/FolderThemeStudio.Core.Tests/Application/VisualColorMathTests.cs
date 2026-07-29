using FolderThemeStudio.App.ViewModels;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class VisualColorMathTests
{
    [Theory]
    [InlineData(0, 0, 200, 100, 0, 1)]
    [InlineData(100, 50, 200, 100, 0.5, 0.5)]
    [InlineData(200, 100, 200, 100, 1, 0)]
    [InlineData(-20, 150, 200, 100, 0, 0)]
    public void FromPalettePoint_MapsAndClampsCoordinates(
        double x,
        double y,
        double width,
        double height,
        double expectedSaturation,
        double expectedValue)
    {
        var actual = VisualColorMath.FromPalettePoint(x, y, width, height);

        Assert.Equal(expectedSaturation, actual.Saturation, 6);
        Assert.Equal(expectedValue, actual.Value, 6);
    }

    [Theory]
    [InlineData(0, 100, 0)]
    [InlineData(50, 100, 179.5)]
    [InlineData(100, 100, 359)]
    public void HueFromPoint_MapsVerticalStrip(double y, double height, double expected)
    {
        Assert.Equal(expected, VisualColorMath.HueFromPoint(y, height), 6);
    }

    [Theory]
    [InlineData(0, 100)]
    [InlineData(100, 0)]
    public void FromPalettePoint_NonPositiveDimensionThrows(double width, double height)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            VisualColorMath.FromPalettePoint(10, 10, width, height));
    }
}
