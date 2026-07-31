using System.Windows;
using FolderThemeStudio.App.Services;

namespace FolderThemeStudio.App.Controls;

public class RoundedWindow : Window
{
    public RoundedWindow()
    {
        SourceInitialized += (_, _) => NativeWindowCornerService.TryApplyRoundedCorners(this);
    }
}
