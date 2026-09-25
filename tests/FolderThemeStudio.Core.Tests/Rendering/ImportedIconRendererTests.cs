using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Rendering;

public sealed class ImportedIconRendererTests
{
    [Fact]
    public async Task Render_WideImage_IsCenteredWithTransparentPaddingAndNoCropping()
    {
        using var temp = new TemporaryDirectory();
        var source = TestImageFiles.Write("wide.png", 80, 40, temp.Path);
        var imported = await new ImportedImageService(Path.Combine(temp.Path, "imports"))
            .ImportAsync(source, CancellationToken.None);

        var bitmap = ImportedIconRenderer.Render(imported.AssetPath!, 64);

        Assert.Equal(64, bitmap.PixelWidth);
        Assert.Equal(64, bitmap.PixelHeight);
        Assert.Equal(0, TestImageFiles.AlphaAt(bitmap, 32, 4));
        Assert.Equal(255, TestImageFiles.AlphaAt(bitmap, 32, 32));
        Assert.Equal(255, TestImageFiles.AlphaAt(bitmap, 1, 32));
        Assert.Equal(255, TestImageFiles.AlphaAt(bitmap, 62, 32));
    }

    [Fact]
    public void Render_MultiframeIco_SelectsFrameClosestToRequestedSize()
    {
        using var temp = new TemporaryDirectory();
        var icoPath = Path.Combine(temp.Path, "multiple.ico");
        var frames = FolderIconRenderer.RequiredSizes.ToDictionary(
            size => size,
            size => SolidPng(size, Color.FromRgb((byte)size, 0, 0)));
        File.WriteAllBytes(icoPath, IcoEncoder.Encode(frames));

        Assert.Equal((byte)64, CenterPixelRed(ImportedIconRenderer.Render(icoPath, 64)));
        Assert.Equal((byte)48, CenterPixelRed(ImportedIconRenderer.Render(icoPath, 40)));
        Assert.Equal((byte)0, CenterPixelRed(ImportedIconRenderer.Render(icoPath, 300)));
    }

    private static byte[] SolidPng(int size, Color color)
    {
        var pixels = new byte[size * size * 4];
        for (var index = 0; index < pixels.Length; index += 4)
        {
            pixels[index] = color.B;
            pixels[index + 1] = color.G;
            pixels[index + 2] = color.R;
            pixels[index + 3] = color.A;
        }
        var bitmap = BitmapSource.Create(size, size, 96, 96, PixelFormats.Bgra32, null, pixels, size * 4);
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        using var stream = new MemoryStream();
        encoder.Save(stream);
        return stream.ToArray();
    }

    private static byte CenterPixelRed(BitmapSource bitmap)
    {
        var bgra = new byte[4];
        new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0).CopyPixels(
            new Int32Rect(bitmap.PixelWidth / 2, bitmap.PixelHeight / 2, 1, 1), bgra, 4, 0);
        return bgra[2];
    }
}
