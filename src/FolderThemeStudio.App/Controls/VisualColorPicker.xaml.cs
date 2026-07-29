using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FolderThemeStudio.App.ViewModels;

namespace FolderThemeStudio.App.Controls;

public partial class VisualColorPicker : System.Windows.Controls.UserControl
{
    public VisualColorPicker()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => UpdateMarkers();
        SizeChanged += (_, _) => UpdateMarkers();
    }

    private void Palette_Mouse(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || DataContext is not ColorPickerViewModel picker) return;
        var point = e.GetPosition(PaletteSurface);
        picker.SetPalettePoint(point.X, point.Y, PaletteSurface.ActualWidth, PaletteSurface.ActualHeight);
        UpdateMarkers();
        PaletteSurface.CaptureMouse();
    }

    private void Hue_Mouse(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || DataContext is not ColorPickerViewModel picker) return;
        var point = e.GetPosition(HueSurface);
        picker.SetHuePoint(point.Y, HueSurface.ActualHeight);
        UpdateMarkers();
        HueSurface.CaptureMouse();
    }

    private void EndMouseInteraction(object sender, MouseButtonEventArgs e)
    {
        if (sender is UIElement element) element.ReleaseMouseCapture();
    }

    private void UpdateMarkers()
    {
        if (DataContext is not ColorPickerViewModel picker || PaletteSurface.ActualWidth <= 0) return;
        Canvas.SetLeft(PaletteMarker, picker.Saturation * PaletteSurface.ActualWidth - 7);
        Canvas.SetTop(PaletteMarker, (1 - picker.Value) * PaletteSurface.ActualHeight - 7);
    }
}
