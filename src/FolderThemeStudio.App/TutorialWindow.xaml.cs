using System.Windows;
using FolderThemeStudio.App.Controls;
using FolderThemeStudio.App.ViewModels;

namespace FolderThemeStudio.App;

public partial class TutorialWindow : RoundedWindow
{
    private readonly TutorialViewModel viewModel;

    public TutorialWindow(TutorialViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        DataContext = viewModel;
        viewModel.RequestClose += ViewModel_RequestClose;
        Closed += (_, _) => viewModel.RequestClose -= ViewModel_RequestClose;
    }

    private void ViewModel_RequestClose(object? sender, EventArgs e)
    {
        DialogResult = true;
        Close();
    }
}
