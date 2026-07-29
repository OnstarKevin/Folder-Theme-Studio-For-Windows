using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FolderThemeStudio.Core.Rendering;

public static class ImportedIconRenderer
{
    public static BitmapSource Render(string normalizedPngPath, int size)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(normalizedPngPath);
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

        var source = Load(normalizedPngPath);
        var scale = Math.Min(size / (double)source.PixelWidth, size / (double)source.PixelHeight);
        var width = source.PixelWidth * scale;
        var height = source.PixelHeight * scale;
        var x = (size - width) / 2;
        var y = (size - height) / 2;

        var visual = new DrawingVisual();
        RenderOptions.SetBitmapScalingMode(visual, BitmapScalingMode.HighQuality);
        using (var context = visual.RenderOpen())
        {
            context.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, size, size));
            context.DrawImage(source, new Rect(x, y, width, height));
        }

        var target = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Pbgra32);
        target.Render(visual);
        target.Freeze();
        return target;
    }

    private static BitmapSource Load(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);
        var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
        var source = decoder.Frames[0];
        source.Freeze();
        return source;
    }
}
