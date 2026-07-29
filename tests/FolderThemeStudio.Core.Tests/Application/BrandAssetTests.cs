using System.IO;
using System.Windows.Media.Imaging;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class BrandAssetTests
{
    [Fact]
    public void CheckedInBrandAssets_HaveExpectedRasterAndIconSizes()
    {
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var pngPath = Path.Combine(root, "src", "FolderThemeStudio.App", "Assets", "FolderThemeStudio.png");
        var icoPath = Path.Combine(root, "src", "FolderThemeStudio.App", "Assets", "FolderThemeStudio.ico");
        var svgPath = Path.Combine(root, "assets", "brand", "folder-theme-studio-logo.svg");

        Assert.True(File.Exists(svgPath), "The editable SVG brand source must be checked in.");
        Assert.True(File.Exists(pngPath), "The 256px application logo must be checked in.");
        using (var png = File.OpenRead(pngPath))
        {
            var frame = BitmapDecoder.Create(png, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad).Frames.Single();
            Assert.Equal(256, frame.PixelWidth);
            Assert.Equal(256, frame.PixelHeight);
        }

        using var icon = new BinaryReader(File.OpenRead(icoPath));
        Assert.Equal((ushort)0, icon.ReadUInt16());
        Assert.Equal((ushort)1, icon.ReadUInt16());
        Assert.Equal((ushort)8, icon.ReadUInt16());
        var sizes = new List<int>();
        for (var index = 0; index < 8; index++)
        {
            var width = icon.ReadByte();
            var height = icon.ReadByte();
            sizes.Add(width == 0 ? 256 : width);
            Assert.Equal(width, height);
            icon.BaseStream.Position += 14;
        }
        Assert.Equal(new[] { 16, 20, 24, 32, 48, 64, 128, 256 }, sizes);
    }
}
