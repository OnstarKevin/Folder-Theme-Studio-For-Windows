using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FolderThemeStudio.Core.Rendering;

namespace FolderThemeStudio.BrandAssets;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        var root = args.Length > 0 ? Path.GetFullPath(args[0]) : FindRepositoryRoot();
        var appAssets = Path.Combine(root, "src", "FolderThemeStudio.App", "Assets");
        var rasterAssets = Path.Combine(root, "assets", "brand", "raster");
        Directory.CreateDirectory(appAssets);
        Directory.CreateDirectory(rasterAssets);

        var frames = new Dictionary<int, byte[]>();
        foreach (var size in FolderIconRenderer.RequiredSizes)
        {
            var png = EncodePng(DrawLogo(size));
            frames[size] = png;
            File.WriteAllBytes(Path.Combine(rasterAssets, $"folder-theme-studio-{size}.png"), png);
        }

        File.WriteAllBytes(Path.Combine(appAssets, "FolderThemeStudio.png"), frames[256]);
        File.WriteAllBytes(Path.Combine(appAssets, "FolderThemeStudio.ico"), IcoEncoder.Encode(frames));
        return 0;
    }

    private static BitmapSource DrawLogo(int size)
    {
        var target = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Pbgra32);
        var visual = new DrawingVisual();
        using (var context = visual.RenderOpen())
        {
            context.PushTransform(new ScaleTransform(size / 256d, size / 256d));
            var folder = Geometry.Parse("M39,83 L39,72 Q39,56 55,56 L104,56 L125,77 L195,77 Q217,77 217,99 L217,186 Q217,208 195,208 L61,208 Q39,208 39,186 Z");
            var fill = new LinearGradientBrush(
                Color.FromRgb(237, 247, 255), Color.FromRgb(221, 214, 254), new Point(0, 0), new Point(1, 1));
            var outline = new Pen(new SolidColorBrush(Color.FromRgb(66, 87, 214)), 10)
            {
                LineJoin = PenLineJoin.Round,
            };
            context.DrawGeometry(fill, outline, folder);
            context.DrawLine(new Pen(new SolidColorBrush(Color.FromRgb(124, 140, 232)), 7)
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round,
            }, new Point(45, 90), new Point(211, 90));

            var wand = new Pen(new LinearGradientBrush(
                Color.FromRgb(37, 99, 235), Color.FromRgb(139, 92, 246), new Point(0, 1), new Point(1, 0)), 13)
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round,
            };
            context.DrawLine(wand, new Point(137, 169), new Point(205, 91));
            context.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(210, 255, 255, 255)), 3)
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round,
            }, new Point(143, 163), new Point(199, 98));
            DrawStar(context, new Point(210, 81), 18, Color.FromRgb(251, 191, 36), true);
            DrawStar(context, new Point(170, 89), 11, Color.FromRgb(96, 165, 250), false);
            context.Pop();
        }
        target.Render(visual);
        target.Freeze();
        return target;
    }

    private static void DrawStar(DrawingContext context, Point center, double radius, Color color, bool outlined)
    {
        var geometry = new StreamGeometry();
        using (var path = geometry.Open())
        {
            path.BeginFigure(new Point(center.X, center.Y - radius), true, true);
            path.LineTo(new Point(center.X + radius * .28, center.Y - radius * .28), true, false);
            path.LineTo(new Point(center.X + radius, center.Y), true, false);
            path.LineTo(new Point(center.X + radius * .28, center.Y + radius * .28), true, false);
            path.LineTo(new Point(center.X, center.Y + radius), true, false);
            path.LineTo(new Point(center.X - radius * .28, center.Y + radius * .28), true, false);
            path.LineTo(new Point(center.X - radius, center.Y), true, false);
            path.LineTo(new Point(center.X - radius * .28, center.Y - radius * .28), true, false);
        }
        geometry.Freeze();
        context.DrawGeometry(new SolidColorBrush(color), outlined ? new Pen(Brushes.White, 3) : null, geometry);
    }

    private static byte[] EncodePng(BitmapSource bitmap)
    {
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        using var stream = new MemoryStream();
        encoder.Save(stream);
        return stream.ToArray();
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "FolderThemeStudio.sln")))
            directory = directory.Parent;
        return directory?.FullName ?? throw new DirectoryNotFoundException("FolderThemeStudio.sln was not found.");
    }
}
