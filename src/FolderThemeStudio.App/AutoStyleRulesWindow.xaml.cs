using System.Windows;
using FolderThemeStudio.App.Controls;
using FolderThemeStudio.App.ViewModels;

namespace FolderThemeStudio.App;

public partial class AutoStyleRulesWindow : RoundedWindow
{
    public AutoStyleRulesWindow(AutoStyleRulesViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}
