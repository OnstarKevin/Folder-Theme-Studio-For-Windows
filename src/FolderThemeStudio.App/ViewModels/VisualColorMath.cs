namespace FolderThemeStudio.App.ViewModels;

public static class VisualColorMath
{
    public static (double Saturation, double Value) FromPalettePoint(
        double x,
        double y,
        double width,
        double height)
    {
        if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

        return (
            Math.Clamp(x / width, 0, 1),
            1 - Math.Clamp(y / height, 0, 1));
    }

    public static double HueFromPoint(double y, double height)
    {
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
        return Math.Clamp(y / height, 0, 1) * 359;
    }
}
