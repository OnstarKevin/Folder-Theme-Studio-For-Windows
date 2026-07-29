using System.IO;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class TutorialViewModelTests
{
    [Fact]
    public void Navigation_IsBoundedAcrossFiveLocalizedSteps()
    {
        using var directory = new TemporaryDirectory();
        var viewModel = new TutorialViewModel(
            AppSettings.Default,
            new JsonAppSettingsStore(Path.Combine(directory.Path, "settings.json")),
            new LocalizationService(applyToApplication: false));

        Assert.Equal(5, viewModel.Steps.Count);
        Assert.Equal("选择主题", viewModel.CurrentStep.Title);
        Assert.False(viewModel.PreviousCommand.CanExecute(null));

        for (var index = 0; index < 8; index++) viewModel.NextCommand.Execute(null);

        Assert.Equal(4, viewModel.StepIndex);
        Assert.False(viewModel.NextCommand.CanExecute(null));
        Assert.Equal("需要时恢复", viewModel.CurrentStep.Title);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task CompleteAsync_FromFinishOrSkip_PersistsCompletionWithoutChangingPalette(bool skipped)
    {
        using var directory = new TemporaryDirectory();
        var store = new JsonAppSettingsStore(Path.Combine(directory.Path, "settings.json"));
        var initial = AppSettings.Default with
        {
            Language = "en-US",
            Palette = new FolderPalette("#112233", "#445566", "#778899", "#AABBCC"),
        };
        var viewModel = new TutorialViewModel(initial, store, new LocalizationService("en-US", applyToApplication: false));

        await viewModel.CompleteAsync(skipped);
        var saved = await store.LoadAsync();

        Assert.True(saved.TutorialCompleted);
        Assert.Equal(initial.Language, saved.Language);
        Assert.Equal(initial.Palette, saved.Palette);
        Assert.True(viewModel.CloseRequested);
    }
}
