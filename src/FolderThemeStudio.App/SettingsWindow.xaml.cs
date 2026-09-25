using System.Windows;
using FolderThemeStudio.App.Controls;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.App.Services;
using System.Windows.Input;

namespace FolderThemeStudio.App;

public partial class SettingsWindow : RoundedWindow
{
    private readonly SettingsViewModel viewModel;

    public SettingsWindow(SettingsViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        DataContext = viewModel;
    }

    private async void Save_Click(object sender, RoutedEventArgs e)
    {
        if (!await viewModel.SaveAsync()) return;
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        viewModel.Cancel();
        DialogResult = false;
        Close();
    }

    private void Recommended_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is FolderPalette palette)
            viewModel.ApplyRecommended(palette);
    }

    private void Recent_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is string color)
            viewModel.ApplyRecent(color);
    }

    private void EditChord_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        var button = e.ChangedButton switch
        {
            MouseButton.Right => FolderMouseButton.Right,
            MouseButton.Middle => FolderMouseButton.Middle,
            _ => (FolderMouseButton?)null,
        };
        if (button is null) return;
        var chord = new FolderEditChord(Keyboard.Modifiers, button.Value);
        if (FolderEditChord.TryNormalize(chord, out var normalized)) viewModel.EditChord = normalized;
        e.Handled = true;
    }
}
