using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FolderThemeStudio.Core.Tests.Rendering;

internal static class TestPng
{
    public static byte[] Create(int size)
    {
        var bitmap = new WriteableBitmap(size, size, 96, 96, PixelFormats.Pbgra32, null);
        var pixels = new byte[size * size * 4];
        bitmap.WritePixels(new Int32Rect(0, 0, size, size), pixels, size * 4, 0);

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));

        using var stream = new MemoryStream();
        encoder.Save(stream);
        return stream.ToArray();
    }
}
