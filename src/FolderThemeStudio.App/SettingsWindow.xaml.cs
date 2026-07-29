using System.Windows;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.ViewModels;

namespace FolderThemeStudio.App;

public partial class SettingsWindow : Window
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
}
