using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Rendering;

public static class FolderIconRenderer
{
    public static readonly int[] RequiredSizes = [16, 20, 24, 32, 48, 64, 128, 256];

    public static BitmapSource Render(FolderTheme theme, int size)
    {
        ThemeValidator.ThrowIfInvalid(theme);
        if (!RequiredSizes.Contains(size))
        {
            throw new ArgumentOutOfRangeException(nameof(size));
        }

        var target = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Pbgra32);
        target.Render(DrawFolder(theme, size, simplify: size <= 32));
        target.Freeze();
        return target;
    }

    private static DrawingVisual DrawFolder(FolderTheme theme, int size, bool simplify)
    {
        var visual = new DrawingVisual();
        using var context = visual.RenderOpen();
        context.PushTransform(new ScaleTransform(size, size));
        var inset = 1d / size;
        context.PushClip(new RectangleGeometry(new Rect(inset, inset, 1d - (2d * inset), 1d - (2d * inset))));

        var radius = Math.Min(theme.CornerRadius / size, 0.12);
        var outlineWidth = simplify ? 1d / size : Math.Max(theme.StrokeWidth / size, 1d / size);
        var body = new Rect(0.09, 0.30, 0.82, 0.55);
        var tab = new Rect(0.15, 0.18, 0.36, 0.24);
        var fill = CreateGradient(theme);
        var outline = new Pen(CreateBrush(theme.StrokeColor, theme.StrokeOpacity), outlineWidth);

        if (!simplify && theme.GlowRadius > 0 && theme.GlowStrength > 0)
        {
            var glowWidth = Math.Max(outlineWidth * 2, theme.GlowRadius / size);
            context.DrawRoundedRectangle(null, new Pen(CreateBrush(theme.GlowColor, theme.GlowStrength * 0.3), glowWidth), body, radius, radius);
        }

        context.DrawRoundedRectangle(CreateGradient(theme, 0.9), outline, tab, radius, radius);

        if (theme.ShadowStrength > 0 && theme.ShadowOffset > 0)
        {
            var shadow = body;
            shadow.Offset(0, theme.ShadowOffset / size);
            context.DrawRoundedRectangle(CreateBrush("#000000", theme.ShadowStrength * 0.5), null, shadow, radius, radius);
        }

        context.DrawRoundedRectangle(fill, outline, body, radius, radius);

        var separatorY = body.Top + 0.09;
        context.DrawLine(new Pen(CreateBrush(theme.StrokeColor, theme.StrokeOpacity * 0.7), outlineWidth),
            new Point(body.Left + 0.035, separatorY), new Point(body.Right - 0.035, separatorY));

        var highlightWidth = Math.Max(theme.HighlightStrength * 0.035, 0);
        if (!simplify || highlightWidth * size >= 1)
        {
            var highlight = new StreamGeometry();
            using (var geometryContext = highlight.Open())
            {
                geometryContext.BeginFigure(new Point(body.Left + 0.10, body.Top + 0.12), true, true);
                geometryContext.LineTo(new Point(body.Right - 0.18, body.Top + 0.12), true, false);
                geometryContext.LineTo(new Point(body.Right - 0.33, body.Bottom - 0.10), true, false);
                geometryContext.LineTo(new Point(body.Left + 0.02, body.Bottom - 0.10), true, false);
            }

            highlight.Freeze();
            context.DrawGeometry(CreateBrush("#FFFFFF", theme.HighlightStrength * 0.28), null, highlight);
        }

        context.Pop();
        context.Pop();
        return visual;
    }

    private static LinearGradientBrush CreateGradient(FolderTheme theme, double opacityMultiplier = 1)
    {
        var radians = theme.GradientAngle * Math.PI / 180;
        var x = Math.Cos(radians) * 0.5;
        var y = Math.Sin(radians) * 0.5;
        var brush = new LinearGradientBrush(
            CreateColor(theme.GradientStart, theme.Opacity * opacityMultiplier),
            CreateColor(theme.GradientEnd, theme.Opacity * opacityMultiplier),
            new Point(0.5 - x, 0.5 - y),
            new Point(0.5 + x, 0.5 + y));
        brush.Freeze();
        return brush;
    }

    private static SolidColorBrush CreateBrush(string color, double opacity)
    {
        var brush = new SolidColorBrush(CreateColor(color, opacity));
        brush.Freeze();
        return brush;
    }

    private static Color CreateColor(string value, double opacity)
    {
        var color = (Color)ColorConverter.ConvertFromString(value)!;
        color.A = (byte)Math.Round(Math.Clamp(opacity, 0, 1) * byte.MaxValue);
        return color;
    }
}
