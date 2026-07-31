using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Tests.TestSupport;
using FolderThemeStudio.Core.Themes;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class MainViewModelTests
{
    [Fact]
    public async Task CompatibleApplyWithPersistence_UpsertsRecursiveMonitoringRule()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.ApplyResult = new ApplyResult(true, Guid.NewGuid(), [new(@"C:\Root", "Applied")], [], [], false, false)
        {
            IconArtifactPath = @"C:\Generated\style.ico"
        };
        using var vm = fixture.CreateViewModel();
        vm.SelectedMode = ApplicationMode.Compatible;
        vm.SelectedRoots.Add(@"C:\Root");
        vm.ContinueApplyingToNewFolders = true;

        await vm.PlanAsync();
        await vm.ApplyAsync();

        Assert.Equal(@"C:\Generated\style.ico", fixture.MonitoringAssets.SourcePath);
        var rule = Assert.Single(fixture.Monitoring.Rules);
        Assert.Equal(@"C:\Root", rule.RootPath);
        Assert.Equal(fixture.MonitoringAssets.DurablePath, rule.IcoPath);
    }

    [Fact]
    public async Task ImportImage_SwitchesSourceAndPlansWithPersistedAsset()
    {
        var fixture = new MainViewModelFixture();
        fixture.Dialogs.ImageImportPath = @"C:\Pictures\folder.png";
        fixture.Images.NextResult = ImportedImageResult.Succeeded(
            @"C:\AppData\imports\asset.png",
            fixture.Renderer.Pixel);
        using var vm = fixture.CreateViewModel();
        await vm.WaitForPreviewAsync();

        await vm.ChooseImportedImageAsync();
        await vm.WaitForPreviewAsync();
        await vm.PlanAsync();

        Assert.Equal(IconEditorMode.ImportedImage, vm.SelectedIconEditorMode);
        Assert.Equal(@"C:\AppData\imports\asset.png", vm.ImportedImageAssetPath);
        Assert.Same(fixture.Renderer.Pixel, vm.ImportedImagePreview);
        Assert.Equal(IconSourceKind.ImportedImage, fixture.Coordinator.LastRequest!.IconSource.Kind);
        Assert.Equal(@"C:\AppData\imports\asset.png", fixture.Coordinator.LastRequest.IconSource.ImportedImagePath);
    }

    [Fact]
    public async Task FailedImageImport_KeepsPreviousImageAndShowsError()
    {
        var fixture = new MainViewModelFixture();
        fixture.Dialogs.ImageImportPath = @"C:\Pictures\broken.png";
        fixture.Images.NextResult = ImportedImageResult.Failure("Unsupported image");
        using var vm = fixture.CreateViewModel();

        await vm.ChooseImportedImageAsync();

        Assert.Equal(IconEditorMode.BuiltIn, vm.SelectedIconEditorMode);
        Assert.Null(vm.ImportedImageAssetPath);
        Assert.Contains(fixture.Dialogs.Errors, error => error.Message.Contains("Unsupported image"));
    }

    [Fact]
    public void ImportedModeWithoutImage_CannotCalculatePlan()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();

        vm.SelectedIconEditorMode = IconEditorMode.ImportedImage;

        Assert.False(vm.CanPlan);
        Assert.Contains("PNG", vm.ValidationMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ICO", vm.ValidationMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void VisualPaletteChangesBuiltInThemeColor()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();

        vm.PaletteEditor.GradientStart.Hex = "#123456";

        Assert.Equal("#123456", vm.GradientStart);
    }

    [Fact]
    public async Task RestoreConflict_ShowsExactChangedPathAsUnresolved()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.RestoreResult = new RestoreResult(
            false,
            false,
            Guid.NewGuid(),
            [new ApplyFailure(ApplyPhase.Restore, "Desktop.ini changed since apply.", @"C:\Themes\changed")])
        {
            Changed = [new ApplyRecord(@"C:\Themes\changed", "Changed since apply")]
        };
        using var vm = fixture.CreateViewModel();
        await vm.WaitForPreviewAsync();

        await vm.RestoreAsync();

        Assert.Contains(vm.Results, item =>
            item.Path == @"C:\Themes\changed" && item.Kind == ResultKind.StateUncertain);
        Assert.Contains("partially", vm.OperationStatus, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PreviewRendering_RunsOnPreviewWorkerBeforeDispatcherPublication()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();

        await vm.WaitForPreviewAsync();

        Assert.Equal(1, fixture.PreviewWorker.RunCount);
        Assert.True(fixture.Renderer.AllCallsRanOnPreviewWorker);
        Assert.Equal([16, 32, 64, 256], fixture.Renderer.Calls.Select(call => call.Size));
    }
    [Fact]
    public async Task StartsWithExactIceBlueThemeAndAllRequiredPreviewSizes()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();

        await vm.WaitForPreviewAsync();

        Assert.Equal(FolderTheme.IceBlue, vm.CurrentTheme);
        Assert.Same(FolderTheme.IceBlue, vm.SelectedSavedTheme);
        Assert.Equal([16, 32, 64, 256], vm.Previews.Select(preview => preview.Size));
        Assert.All(vm.Previews, preview => Assert.NotNull(preview.Image));
    }

    [Fact]
    public async Task EditingColor_RefreshesAllPreviewSizes()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();
        await vm.WaitForPreviewAsync();
        fixture.Renderer.ClearCalls();

        vm.GradientStart = "#22AAFF";
        await vm.WaitForPreviewAsync();

        Assert.Equal([16, 32, 64, 256], vm.Previews.Select(preview => preview.Size));
        Assert.Equal([16, 32, 64, 256], fixture.Renderer.Calls.Select(call => call.Size));
        Assert.All(fixture.Renderer.Calls, call => Assert.Equal("#22AAFF", call.Theme.GradientStart));
    }

    [Fact]
    public void EditingSavedTheme_ClearsSelectionToShowUnsavedChanges()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();

        vm.GradientStart = "#22AAFF";

        Assert.Null(vm.SelectedSavedTheme);
    }

    [Fact]
    public async Task RapidEdits_CancelObsoletePreviewRender()
    {
        var fixture = new MainViewModelFixture(useControlledDelay: true);
        using var vm = fixture.CreateViewModel();
        fixture.Delay.CompleteLatest();
        await vm.WaitForPreviewAsync();
        fixture.Renderer.ClearCalls();

        vm.GradientStart = "#112233";
        vm.GradientStart = "#445566";

        Assert.True(vm.IsPreviewRendering);
        Assert.False(vm.ApplyCommand.CanExecute(null));

        fixture.Delay.CompleteLatest();
        await vm.WaitForPreviewAsync();

        Assert.Equal(4, fixture.Renderer.Calls.Count);
        Assert.All(fixture.Renderer.Calls, call => Assert.Equal("#445566", call.Theme.GradientStart));
    }

    [Fact]
    public void InvalidTheme_DisablesApplyAndExplainsTheError()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();

        vm.GradientStart = "blue";

        Assert.False(vm.ApplyCommand.CanExecute(null));
        Assert.False(vm.IsThemeValid);
        Assert.Contains(nameof(FolderTheme.GradientStart), vm.ValidationMessage);
    }

    [Fact]
    public async Task InvalidIntermediateEdit_DoesNotRenderOrShowAnExpectedErrorDialog()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();
        await vm.WaitForPreviewAsync();
        fixture.Renderer.ClearCalls();

        vm.GradientStart = "#22";
        await vm.WaitForPreviewAsync();

        Assert.Empty(fixture.Renderer.Calls);
        Assert.Empty(fixture.Dialogs.Errors);
    }

    [Fact]
    public void CompatibilityMode_RequiresExplicitRoot()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();

        vm.SelectedMode = ApplicationMode.Compatible;

        Assert.False(vm.ApplyCommand.CanExecute(null));
        Assert.Contains("explicit folder", vm.ValidationMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CompatibleApply_PlansExplicitRootsAndConfirmsCountsBeforeApplying()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.Plan = new ApplyPlan(
            FolderTheme.IceBlue,
            ApplicationMode.Compatible,
            [
                new(@"C:\Themes", FolderDecision.Allowed),
                new(@"C:\Themes\Known", FolderDecision.KnownFolder),
                new(@"C:\Themes\Existing", FolderDecision.ExistingCustomization)
            ],
            []);
        fixture.Coordinator.ApplyResult = new ApplyResult(
            true,
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            [new(@"C:\Themes", "Applied")],
            [new(@"C:\Themes\Known", "Known folder"), new(@"C:\Themes\Existing", "Existing customization")],
            [],
            false,
            false);
        using var vm = fixture.CreateViewModel();
        vm.SelectedMode = ApplicationMode.Compatible;
        vm.SelectedRoots.Add(@"C:\Themes");

        await vm.PlanAsync();
        await vm.ApplyAsync();

        Assert.Equal([@"C:\Themes"], fixture.Coordinator.LastRequest!.ExplicitRoots);
        Assert.Contains(@"C:\Themes", fixture.Dialogs.LastConfirmationMessage);
        Assert.Contains("1 allowed", fixture.Dialogs.LastConfirmationMessage);
        Assert.Contains("2 skipped", fixture.Dialogs.LastConfirmationMessage);
        Assert.Contains("never scans a whole drive", fixture.Dialogs.LastConfirmationMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(1, fixture.Coordinator.ApplyCallCount);
        Assert.Equal(1, vm.PlanAllowedCount);
        Assert.Equal(2, vm.PlanSkippedCount);
        Assert.Equal("Applied: 1  Skipped: 2  Failed: 0", vm.ResultSummary);
    }

    [Fact]
    public async Task DecliningGlobalConfirmation_DoesNotApply()
    {
        var fixture = new MainViewModelFixture();
        fixture.Dialogs.ConfirmResult = false;
        using var vm = fixture.CreateViewModel();

        await vm.PlanAsync();
        await vm.ApplyAsync();

        Assert.Contains("HKEY_CURRENT_USER", fixture.Dialogs.LastConfirmationMessage);
        Assert.Contains("Shell Icons", fixture.Dialogs.LastConfirmationMessage);
        Assert.Contains("values 3 and 4", fixture.Dialogs.LastConfirmationMessage);
        Assert.Contains("Restore latest", fixture.Dialogs.LastConfirmationMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(0, fixture.Coordinator.ApplyCallCount);
    }

    [Fact]
    public async Task Planning_DisablesApplyUntilPlanningFinishes()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.PausePlanning = true;
        using var vm = fixture.CreateViewModel();

        var planning = vm.PlanAsync();
        await fixture.Coordinator.PlanningStarted.Task;

        Assert.True(vm.IsPlanning);
        Assert.False(vm.AreInputsEnabled);
        Assert.False(vm.ApplyCommand.CanExecute(null));

        fixture.Coordinator.ReleasePlanning();
        await planning;
        Assert.False(vm.IsPlanning);
        Assert.True(vm.ApplyCommand.CanExecute(null));
    }

    [Fact]
    public async Task InputRevisionChange_DiscardsStalePlanBeforeApplyIsEnabled()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.PausePlanning = true;
        using var vm = fixture.CreateViewModel();
        vm.SelectedMode = ApplicationMode.Compatible;
        vm.SelectedRoots.Add(@"C:\Original");

        var planning = vm.PlanAsync();
        await fixture.Coordinator.PlanningStarted.Task;
        vm.ThemeName = "Changed after planning started";
        vm.SelectedRoots.Clear();
        vm.SelectedRoots.Add(@"C:\Changed");
        fixture.Coordinator.ReleasePlanning();
        await planning;

        Assert.Equal(FolderTheme.IceBlue.Name, fixture.Coordinator.LastRequest!.Theme.Name);
        Assert.False(vm.ApplyCommand.CanExecute(null));
        Assert.Contains("changed while planning", vm.OperationStatus, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ApplyRequiresAPlanForTheCurrentInputRevision()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();

        Assert.False(vm.ApplyCommand.CanExecute(null));

        await vm.PlanAsync();
        Assert.True(vm.ApplyCommand.CanExecute(null));

        vm.GradientStart = "#22AAFF";
        await vm.WaitForPreviewAsync();
        Assert.False(vm.ApplyCommand.CanExecute(null));

        await vm.PlanAsync();
        Assert.True(vm.ApplyCommand.CanExecute(null));
    }

    [Fact]
    public async Task Applying_DisablesApplyUntilResultReturns()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();
        await vm.PlanAsync();
        fixture.Coordinator.PauseApply = true;

        var apply = vm.ApplyAsync();
        await fixture.Coordinator.ApplyStarted.Task;

        Assert.True(vm.IsApplying);
        Assert.False(vm.ApplyCommand.CanExecute(null));

        fixture.Coordinator.ReleaseApply();
        await apply;
        Assert.False(vm.IsApplying);
    }

    [Fact]
    public async Task Restoring_DisablesApplyUntilResultReturns()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.PauseRestore = true;
        using var vm = fixture.CreateViewModel();

        var restore = vm.RestoreAsync();
        await fixture.Coordinator.RestoreStarted.Task;

        Assert.True(vm.IsRestoring);
        Assert.False(vm.ApplyCommand.CanExecute(null));

        fixture.Coordinator.ReleaseRestore();
        await restore;
        Assert.False(vm.IsRestoring);
    }

    [Fact]
    public async Task ApplyResult_ListsSuccessSkippedAndFailedPaths()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.ApplyResult = new ApplyResult(
            false,
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            [new(@"C:\Good", "Applied")],
            [new(@"C:\Skipped", "Known folder")],
            [new(ApplyPhase.SystemMutation, "Access denied", @"C:\Failed")],
            true,
            true);
        using var vm = fixture.CreateViewModel();

        await vm.PlanAsync();
        await vm.ApplyAsync();

        Assert.Equal("Changed then rolled back: 1  Skipped: 1  Failed: 1", vm.ResultSummary);
        Assert.Contains(vm.Results, item => item.Kind == ResultKind.RolledBack && item.Path == @"C:\Good");
        Assert.Contains(vm.Results, item => item.Kind == ResultKind.Skipped && item.Path == @"C:\Skipped");
        Assert.Contains(vm.Results, item => item.Kind == ResultKind.Failed && item.Path == @"C:\Failed");
        Assert.True(vm.CopyFailedPathsCommand.CanExecute(null));

        vm.CopyFailedPathsCommand.Execute(null);
        await vm.ExportFailedPathsAsync();

        Assert.Equal(@"C:\Failed", fixture.Dialogs.CopiedText);
        Assert.Contains(@"C:\Failed", fixture.Dialogs.ExportedText);
        Assert.Contains("Access denied", fixture.Dialogs.ExportedText);
    }

    [Fact]
    public async Task FailureReportIoErrors_AreShownWithoutEscapingCommands()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.ApplyResult = new ApplyResult(
            false,
            Guid.NewGuid(),
            [],
            [],
            [new(ApplyPhase.SystemMutation, "Access denied", @"C:\Failed")],
            false,
            false);
        using var vm = fixture.CreateViewModel();
        await vm.PlanAsync();
        await vm.ApplyAsync();

        fixture.Dialogs.ThrowOnCopy = true;
        var copyException = Record.Exception(() => vm.CopyFailedPathsCommand.Execute(null));
        fixture.Dialogs.ThrowOnExport = true;
        var exportException = await Record.ExceptionAsync(vm.ExportFailedPathsAsync);

        Assert.Null(copyException);
        Assert.Null(exportException);
        Assert.Contains(fixture.Dialogs.Errors, error => error.Title == "Could not copy failed paths");
        Assert.Contains(fixture.Dialogs.Errors, error => error.Title == "Could not export failure report");
    }

    [Fact]
    public async Task IncompleteRollback_MarksPreviouslyChangedPathsAsStateUncertain()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.ApplyResult = new ApplyResult(
            false,
            Guid.NewGuid(),
            [new(@"C:\Changed", "Mutation completed")],
            [new(@"C:\Skipped", "Known folder")],
            [new(ApplyPhase.Rollback, "Could not restore mapping")],
            true,
            false);
        using var vm = fixture.CreateViewModel();
        await vm.PlanAsync();

        await vm.ApplyAsync();

        Assert.Contains("state uncertain", vm.ResultSummary, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("rollback incomplete", vm.OperationStatus, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(vm.Results, item => item.Kind == ResultKind.StateUncertain && item.Path == @"C:\Changed");
        Assert.DoesNotContain(vm.Results, item => item.Kind == ResultKind.Success);
        Assert.Contains(vm.Results, item => item.Kind == ResultKind.Skipped && item.Path == @"C:\Skipped");
        Assert.Contains(vm.Results, item => item.Kind == ResultKind.Failed && item.Detail.Contains("Could not restore"));
    }

    [Fact]
    public async Task ImportTheme_RefreshesCatalogAndSelectsImportedTheme()
    {
        var fixture = new MainViewModelFixture();
        var imported = FolderTheme.IceBlue with { Id = Guid.NewGuid(), Name = "Imported theme" };
        fixture.Dialogs.ThemeImportPath = @"C:\Themes\import.json";
        fixture.Themes.ImportResult = new ThemeImportResult(imported, []);
        using var vm = fixture.CreateViewModel();

        await vm.ImportThemeAsync();

        Assert.Equal(@"C:\Themes\import.json", fixture.Themes.ImportSourcePath);
        Assert.Contains(imported, vm.SavedThemes);
        Assert.Equal(imported, vm.SelectedSavedTheme);
        Assert.Equal(imported, vm.CurrentTheme);
    }

    [Fact]
    public async Task ImportThemeFailure_ShowsAllCatalogErrors()
    {
        var fixture = new MainViewModelFixture();
        fixture.Dialogs.ThemeImportPath = @"C:\Themes\broken.json";
        fixture.Themes.ImportResult = new ThemeImportResult(null, ["Invalid JSON", "GradientStart"]);
        using var vm = fixture.CreateViewModel();

        await vm.ImportThemeAsync();

        var error = Assert.Single(fixture.Dialogs.Errors);
        Assert.Equal("Could not import theme", error.Title);
        Assert.Contains("Invalid JSON", error.Message);
        Assert.Contains("GradientStart", error.Message);
    }

    [Fact]
    public async Task ExportTheme_UsesSelectedThemeAndChosenDestination()
    {
        var fixture = new MainViewModelFixture();
        fixture.Dialogs.ThemeExportPath = @"C:\Themes\ice-blue.json";
        using var vm = fixture.CreateViewModel();

        await vm.ExportThemeAsync();

        Assert.Equal(FolderTheme.IceBlue.Id, fixture.Themes.ExportedThemeId);
        Assert.Equal(@"C:\Themes\ice-blue.json", fixture.Themes.ExportDestinationPath);
        Assert.Contains("Exported", vm.OperationStatus);
    }

    [Fact]
    public async Task DeleteBuiltInTheme_IsRejectedWithVisibleError()
    {
        var fixture = new MainViewModelFixture();
        using var vm = fixture.CreateViewModel();

        await vm.DeleteThemeAsync();

        Assert.Null(fixture.Themes.DeletedThemeId);
        Assert.Contains(fixture.Dialogs.Errors, error =>
            error.Title == "Cannot delete built-in theme" &&
            error.Message.Contains("built-in", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task DeletePersonalTheme_ConfirmsThenRefreshesCatalog()
    {
        var fixture = new MainViewModelFixture();
        var personal = FolderTheme.IceBlue with { Id = Guid.NewGuid(), Name = "Personal" };
        fixture.Themes.Items.Add(personal);
        using var vm = fixture.CreateViewModel();
        await vm.InitializeAsync();
        vm.SelectedSavedTheme = personal;

        await vm.DeleteThemeAsync();

        Assert.Equal(personal.Id, fixture.Themes.DeletedThemeId);
        Assert.Contains("Personal", fixture.Dialogs.LastConfirmationMessage);
        Assert.DoesNotContain(personal, vm.SavedThemes);
        Assert.Equal(FolderTheme.IceBlue, vm.SelectedSavedTheme);
    }

    [Fact]
    public async Task SaveTheme_EditDuringAwait_PreservesNewEditAndLeavesItUnsaved()
    {
        var fixture = new MainViewModelFixture();
        fixture.Themes.PauseSave = true;
        using var vm = fixture.CreateViewModel();

        var save = vm.SaveThemeAsync();
        await fixture.Themes.SaveStarted.Task;
        Assert.True(vm.IsManagingThemes);
        Assert.False(vm.AreInputsEnabled);

        vm.GradientStart = "#112233";
        fixture.Themes.ReleaseSave();
        await save;

        Assert.Equal("#112233", vm.GradientStart);
        Assert.Null(vm.SelectedSavedTheme);
        Assert.Contains("current edits remain unsaved", vm.OperationStatus, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RestoreFailure_RemainsVisibleAlongsidePartialCompletionStatus()
    {
        var fixture = new MainViewModelFixture();
        fixture.Coordinator.RestoreResult = new RestoreResult(
            false,
            false,
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            [new(ApplyPhase.Restore, "Could not restore an edited desktop.ini", @"C:\Changed")]);
        using var vm = fixture.CreateViewModel();

        await vm.RestoreAsync();

        Assert.Contains("partially", vm.OperationStatus, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(vm.Results, item =>
            item.Kind == ResultKind.Failed &&
            item.Path == @"C:\Changed" &&
            item.Detail.Contains("Could not restore", StringComparison.Ordinal));
    }
}
