using FolderThemeStudio.App.ViewModels;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class ColorPickerViewModelTests
{
    [Theory]
    [InlineData(0, 1, 1, "#FF0000")]
    [InlineData(180, 1, 1, "#00FFFF")]
    [InlineData(0, 0, 0.5, "#808080")]
    public void HsvValues_ProduceExpectedHex(double hue, double saturation, double value, string expected)
    {
        var picker = new ColorPickerViewModel("#000000")
        {
            Hue = hue,
            Saturation = saturation,
            Value = value,
        };

        Assert.Equal(expected, picker.Hex);
    }

    [Theory]
    [InlineData("#abc", "#AABBCC")]
    [InlineData("abcdef", "#ABCDEF")]
    [InlineData("#123456", "#123456")]
    public void TrySetHex_ValidInput_NormalizesColor(string input, string expected)
    {
        var picker = new ColorPickerViewModel("#000000");

        Assert.True(picker.TrySetHex(input));
        Assert.Equal(expected, picker.Hex);
        Assert.True(picker.IsValid);
    }

    [Fact]
    public void TrySetHex_InvalidInput_LeavesPreviousColorUnchanged()
    {
        var picker = new ColorPickerViewModel("#123456");

        Assert.False(picker.TrySetHex("orange"));
        Assert.Equal("#123456", picker.Hex);
    }

    [Fact]
    public void HexEditor_InvalidInput_MarksPickerInvalidUntilCorrected()
    {
        var picker = new ColorPickerViewModel("#123456");

        picker.Hex = "not-a-color";
        Assert.False(picker.IsValid);

        picker.Hex = "#abcdef";
        Assert.True(picker.IsValid);
        Assert.Equal("#ABCDEF", picker.Hex);
    }

    [Fact]
    public void SetPalettePoint_UpdatesSaturationValueHexAndBrush()
    {
        var picker = new ColorPickerViewModel("#FF0000") { Hue = 0 };

        picker.SetPalettePoint(50, 50, 100, 100);

        Assert.Equal(0.5, picker.Saturation, 6);
        Assert.Equal(0.5, picker.Value, 6);
        Assert.Equal("#804040", picker.Hex);
        Assert.NotNull(picker.SelectedBrush);
    }

    [Fact]
    public void SetHuePoint_UpdatesHueFromVisualStrip()
    {
        var picker = new ColorPickerViewModel("#FF0000");

        picker.SetHuePoint(50, 100);

        Assert.Equal(179.5, picker.Hue, 6);
    }
}
