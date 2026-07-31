using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FolderThemeStudio.Core.Rendering;

namespace FolderThemeStudio.Core.Tests.Rendering;

internal static class TestImageFiles
{
    internal static string WriteIco(string fileName, string directory)
    {
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, fileName);
        var frames = FolderIconRenderer.RequiredSizes.ToDictionary(size => size, TestPng.Create);
        File.WriteAllBytes(path, IcoEncoder.Encode(frames));
        return path;
    }

    internal static string Write(string fileName, int width, int height, string directory)
    {
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, fileName);
        var pixels = new byte[width * height * 4];
        for (var offset = 0; offset < pixels.Length; offset += 4)
        {
            pixels[offset] = 0x20;
            pixels[offset + 1] = 0x80;
            pixels[offset + 2] = 0xE0;
            pixels[offset + 3] = 0xFF;
        }

        var bitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, pixels, width * 4);
        BitmapEncoder encoder = Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".png" => new PngBitmapEncoder(),
            ".jpg" or ".jpeg" => new JpegBitmapEncoder { QualityLevel = 95 },
            ".bmp" => new BmpBitmapEncoder(),
            _ => throw new ArgumentOutOfRangeException(nameof(fileName))
        };
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        using var stream = File.Create(path);
        encoder.Save(stream);
        return path;
    }

    internal static byte AlphaAt(BitmapSource bitmap, int x, int y)
    {
        var converted = new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0);
        var pixel = new byte[4];
        converted.CopyPixels(new Int32Rect(x, y, 1, 1), pixel, 4, 0);
        return pixel[3];
    }
}
