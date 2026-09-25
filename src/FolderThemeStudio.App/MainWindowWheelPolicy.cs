namespace FolderThemeStudio.App;

public static class MainWindowWheelPolicy
{
    public static double NextOffset(double current, int delta, int lines, double lineHeight) =>
        Math.Max(0, current - ((double)delta / 120 * Math.Max(1, lines) * lineHeight * 2));
}
