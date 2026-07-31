using System.IO;
using System.Windows;
using FolderThemeStudio.App.ViewModels;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;
using Clipboard = System.Windows.Clipboard;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace FolderThemeStudio.App.Services;

public sealed class DialogService(Func<Window?> owner) : IMainViewModelDialogs
{
    public Task<bool> ConfirmAsync(string title, string message)
    {
        var result = MessageBox.Show(
            owner(),
            message,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning,
            MessageBoxResult.No);
        return Task.FromResult(result == MessageBoxResult.Yes);
    }

    public Task<string?> PickFolderAsync()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Select an explicit folder root",
            Multiselect = false
        };
        var result = dialog.ShowDialog(owner());
        return Task.FromResult(result == true ? dialog.FolderName : null);
    }

    public Task<string?> PickThemeImportPathAsync()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Import folder theme",
            DefaultExt = ".json",
            Filter = "Folder theme files (*.json)|*.json|All files (*.*)|*.*",
            Multiselect = false,
            CheckFileExists = true
        };
        return Task.FromResult(dialog.ShowDialog(owner()) == true ? dialog.FileName : null);
    }

    public Task<string?> PickImageImportPathAsync()
    {
        var dialog = new OpenFileDialog
        {
            Title = "选择文件夹图标图片",
            Filter = "图片和图标 (*.png;*.jpg;*.jpeg;*.bmp;*.ico)|*.png;*.jpg;*.jpeg;*.bmp;*.ico",
            Multiselect = false,
            CheckFileExists = true
        };
        return Task.FromResult(dialog.ShowDialog(owner()) == true ? dialog.FileName : null);
    }

    public Task<string?> PickThemeExportPathAsync(string suggestedFileName)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Export folder theme",
            FileName = suggestedFileName,
            DefaultExt = ".json",
            Filter = "Folder theme files (*.json)|*.json|All files (*.*)|*.*",
            AddExtension = true,
            OverwritePrompt = true
        };
        return Task.FromResult(dialog.ShowDialog(owner()) == true ? dialog.FileName : null);
    }

    public void CopyText(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            Clipboard.SetText(text);
        }
    }

    public async Task ExportTextAsync(string suggestedFileName, string text)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Export failed paths",
            FileName = suggestedFileName,
            DefaultExt = ".txt",
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            AddExtension = true,
            OverwritePrompt = true
        };
        if (dialog.ShowDialog(owner()) == true)
        {
            await File.WriteAllTextAsync(dialog.FileName, text);
        }
    }

    public void ShowError(string title, string message) =>
        MessageBox.Show(owner(), message, title, MessageBoxButton.OK, MessageBoxImage.Error);
}
