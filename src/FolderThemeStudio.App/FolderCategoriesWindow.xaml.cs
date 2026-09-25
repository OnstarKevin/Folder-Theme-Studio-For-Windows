using System.Windows;
using FolderThemeStudio.App.Controls;
using FolderThemeStudio.App.ViewModels;

namespace FolderThemeStudio.App;

public partial class FolderCategoriesWindow : RoundedWindow
{
    private readonly FolderCategoriesViewModel viewModel;

    public FolderCategoriesWindow(FolderCategoriesViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        DataContext = viewModel;
        Closed += (_, _) => this.viewModel.Dispose();
    }

    private void ColorPreset_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.Tag is string color) viewModel.ChoosePresetColor(color);
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}
