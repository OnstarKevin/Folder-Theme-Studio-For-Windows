using System.IO;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class VisualColorPickerTests
{
    [Fact]
    public void PaletteSurfaces_ReleaseMouseCaptureWhenSelectionEnds()
    {
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var xaml = File.ReadAllText(Path.Combine(root, "src", "FolderThemeStudio.App", "Controls", "VisualColorPicker.xaml"));
        var codeBehind = File.ReadAllText(Path.Combine(root, "src", "FolderThemeStudio.App", "Controls", "VisualColorPicker.xaml.cs"));

        Assert.Equal(2, xaml.Split("MouseLeftButtonUp=\"EndMouseInteraction\"").Length - 1);
        Assert.Contains("ReleaseMouseCapture", codeBehind);
    }

    [Fact]
    public void PickingGradientStartOnPalette_UpdatesMainThemeColor()
    {
        var fixture = new MainViewModelFixture();
        using var viewModel = fixture.CreateViewModel();
        var mainColor = viewModel.PaletteEditor.GradientStart;

        mainColor.SetHuePoint(120, 360);
        mainColor.SetPalettePoint(150, 40, 200, 200);

        Assert.Equal(mainColor.Hex, viewModel.GradientStart);
        Assert.NotEqual("#65D9FF", viewModel.GradientStart);
    }
}
