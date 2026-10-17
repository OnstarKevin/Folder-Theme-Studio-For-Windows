# 桌面灵动岛：完整项目源码

本文件为 beta.18 完整源码快照。使用源码 ZIP 中的原始文件打开 FolderThemeStudio.sln。运行和授权说明见 DYNAMIC-ISLAND.zh-CN.md。

## 文件结构

```text
FolderThemeStudio.sln
NuGet.Config
src/FolderThemeStudio.App/App.xaml
src/FolderThemeStudio.App/App.xaml.cs
src/FolderThemeStudio.App/AutoStyleRulesWindow.xaml
src/FolderThemeStudio.App/AutoStyleRulesWindow.xaml.cs
src/FolderThemeStudio.App/Controls/RoundedWindow.cs
src/FolderThemeStudio.App/Controls/VisualColorPicker.xaml
src/FolderThemeStudio.App/Controls/VisualColorPicker.xaml.cs
src/FolderThemeStudio.App/DynamicIsland/ClipboardTextListener.cs
src/FolderThemeStudio.App/DynamicIsland/DynamicIslandController.cs
src/FolderThemeStudio.App/DynamicIsland/DynamicIslandSettingsWindow.xaml
src/FolderThemeStudio.App/DynamicIsland/DynamicIslandSettingsWindow.xaml.cs
src/FolderThemeStudio.App/DynamicIsland/DynamicIslandWindow.xaml
src/FolderThemeStudio.App/DynamicIsland/DynamicIslandWindow.xaml.cs
src/FolderThemeStudio.App/DynamicIsland/IslandDragPhysics.cs
src/FolderThemeStudio.App/DynamicIsland/IslandNotificationHub.cs
src/FolderThemeStudio.App/DynamicIsland/IslandNotificationQueue.cs
src/FolderThemeStudio.App/DynamicIsland/IslandSettings.cs
src/FolderThemeStudio.App/DynamicIsland/IslandViewModel.cs
src/FolderThemeStudio.App/DynamicIsland/LocalNotificationBridge.cs
src/FolderThemeStudio.App/DynamicIsland/LrcLyricsSource.cs
src/FolderThemeStudio.App/DynamicIsland/LyricsCoordinator.cs
src/FolderThemeStudio.App/DynamicIsland/OnlineLyricsSource.cs
src/FolderThemeStudio.App/DynamicIsland/SystemMetricsSource.cs
src/FolderThemeStudio.App/DynamicIsland/WindowsMediaSource.cs
src/FolderThemeStudio.App/DynamicIsland/WindowsNotificationSource.cs
src/FolderThemeStudio.App/FolderCategoriesWindow.xaml
src/FolderThemeStudio.App/FolderCategoriesWindow.xaml.cs
src/FolderThemeStudio.App/FolderCategoryPopupWindow.xaml
src/FolderThemeStudio.App/FolderCategoryPopupWindow.xaml.cs
src/FolderThemeStudio.App/FolderQuickNoteWindow.xaml
src/FolderThemeStudio.App/FolderQuickNoteWindow.xaml.cs
src/FolderThemeStudio.App/FolderThemeStudio.App.csproj
src/FolderThemeStudio.App/Localization/LocalizationService.cs
src/FolderThemeStudio.App/Localization/Strings.en-US.xaml
src/FolderThemeStudio.App/Localization/Strings.zh-CN.xaml
src/FolderThemeStudio.App/MainWindow.xaml
src/FolderThemeStudio.App/MainWindow.xaml.cs
src/FolderThemeStudio.App/MainWindowWheelPolicy.cs
src/FolderThemeStudio.App/Monitoring/DirectoryWatcher.cs
src/FolderThemeStudio.App/Monitoring/FolderMonitoringCoordinator.cs
src/FolderThemeStudio.App/Monitoring/JsonMonitoringRuleStore.cs
src/FolderThemeStudio.App/Monitoring/MonitoringIconAssetService.cs
src/FolderThemeStudio.App/Monitoring/MonitoringRule.cs
src/FolderThemeStudio.App/Services/DialogService.cs
src/FolderThemeStudio.App/Services/ExplorerFolderHoverTargetResolver.cs
src/FolderThemeStudio.App/Services/ExplorerInfoTipSuppressor.cs
src/FolderThemeStudio.App/Services/FolderCategoryCatalog.cs
src/FolderThemeStudio.App/Services/FolderEditChord.cs
src/FolderThemeStudio.App/Services/FolderHoverDecision.cs
src/FolderThemeStudio.App/Services/FolderHoverMonitor.cs
src/FolderThemeStudio.App/Services/FolderInfoSummary.cs
src/FolderThemeStudio.App/Services/FolderLiveContentService.cs
src/FolderThemeStudio.App/Services/FolderQuickEditMouseHook.cs
src/FolderThemeStudio.App/Services/NativeWindowCornerService.cs
src/FolderThemeStudio.App/Services/StartupRegistrationService.cs
src/FolderThemeStudio.App/Services/TrayIconService.cs
src/FolderThemeStudio.App/Settings/AppSettings.cs
src/FolderThemeStudio.App/Settings/AppleColors.cs
src/FolderThemeStudio.App/Settings/JsonAppSettingsStore.cs
src/FolderThemeStudio.App/SettingsWindow.xaml
src/FolderThemeStudio.App/SettingsWindow.xaml.cs
src/FolderThemeStudio.App/Styles/AppleStyles.xaml
src/FolderThemeStudio.App/TutorialWindow.xaml
src/FolderThemeStudio.App/TutorialWindow.xaml.cs
src/FolderThemeStudio.App/ViewModels/AutoStyleRulesViewModel.cs
src/FolderThemeStudio.App/ViewModels/ColorPickerViewModel.cs
src/FolderThemeStudio.App/ViewModels/FolderCategoriesViewModel.cs
src/FolderThemeStudio.App/ViewModels/MainViewModel.cs
src/FolderThemeStudio.App/ViewModels/PaletteEditorViewModel.cs
src/FolderThemeStudio.App/ViewModels/RelayCommand.cs
src/FolderThemeStudio.App/ViewModels/SettingsViewModel.cs
src/FolderThemeStudio.App/ViewModels/TutorialViewModel.cs
src/FolderThemeStudio.App/ViewModels/VisualColorMath.cs
src/FolderThemeStudio.Core/Application/ApplyModels.cs
src/FolderThemeStudio.Core/Application/IconSource.cs
src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs
src/FolderThemeStudio.Core/FolderThemeStudio.Core.csproj
src/FolderThemeStudio.Core/Recovery/BackupModels.cs
src/FolderThemeStudio.Core/Recovery/BackupService.cs
src/FolderThemeStudio.Core/Rendering/FolderIconRenderer.cs
src/FolderThemeStudio.Core/Rendering/IcoEncoder.cs
src/FolderThemeStudio.Core/Rendering/ImportedIconRenderer.cs
src/FolderThemeStudio.Core/Rendering/ImportedImageService.cs
src/FolderThemeStudio.Core/Rendering/MaterialPainter.cs
src/FolderThemeStudio.Core/Rendering/StickerPainter.cs
src/FolderThemeStudio.Core/SystemIntegration/CompatibleFolderFileSystem.cs
src/FolderThemeStudio.Core/SystemIntegration/CompatibleFolderService.cs
src/FolderThemeStudio.Core/SystemIntegration/DesktopIniDocument.cs
src/FolderThemeStudio.Core/SystemIntegration/GlobalIconService.cs
src/FolderThemeStudio.Core/SystemIntegration/KnownFolderService.cs
src/FolderThemeStudio.Core/SystemIntegration/MonitoredFolderIconService.cs
src/FolderThemeStudio.Core/SystemIntegration/OwnedFileDeletionService.cs
src/FolderThemeStudio.Core/SystemIntegration/ShellRefreshService.cs
src/FolderThemeStudio.Core/Themes/FolderTheme.cs
src/FolderThemeStudio.Core/Themes/ThemeStore.cs
src/FolderThemeStudio.Core/Themes/ThemeValidator.cs
```

## FolderThemeStudio.sln

```text
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.31903.59
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "FolderThemeStudio.Core", "src\FolderThemeStudio.Core\FolderThemeStudio.Core.csproj", "{4FF9369E-4D01-48F6-BD4B-AD312C4884BF}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "FolderThemeStudio.App", "src\FolderThemeStudio.App\FolderThemeStudio.App.csproj", "{3074FA43-5F9D-4E83-92A9-2F449E60A590}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "FolderThemeStudio.Core.Tests", "tests\FolderThemeStudio.Core.Tests\FolderThemeStudio.Core.Tests.csproj", "{FF94BF33-82F5-466D-A8C9-F2A01225471A}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{4FF9369E-4D01-48F6-BD4B-AD312C4884BF}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{4FF9369E-4D01-48F6-BD4B-AD312C4884BF}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{4FF9369E-4D01-48F6-BD4B-AD312C4884BF}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{4FF9369E-4D01-48F6-BD4B-AD312C4884BF}.Release|Any CPU.Build.0 = Release|Any CPU
		{3074FA43-5F9D-4E83-92A9-2F449E60A590}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3074FA43-5F9D-4E83-92A9-2F449E60A590}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3074FA43-5F9D-4E83-92A9-2F449E60A590}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3074FA43-5F9D-4E83-92A9-2F449E60A590}.Release|Any CPU.Build.0 = Release|Any CPU
		{FF94BF33-82F5-466D-A8C9-F2A01225471A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FF94BF33-82F5-466D-A8C9-F2A01225471A}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FF94BF33-82F5-466D-A8C9-F2A01225471A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FF94BF33-82F5-466D-A8C9-F2A01225471A}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
EndGlobal
```

## NuGet.Config

```text
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```

## src/FolderThemeStudio.App/App.xaml

```xml
<Application x:Class="FolderThemeStudio.App.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             ShutdownMode="OnExplicitShutdown" Startup="OnStartup">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Styles/AppleStyles.xaml" />
                <ResourceDictionary Source="Localization/Strings.zh-CN.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

## src/FolderThemeStudio.App/App.xaml.cs

```csharp
using System.Windows;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.DynamicIsland;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Themes;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.Core.SystemIntegration;

namespace FolderThemeStudio.App;

public partial class App : System.Windows.Application
{
    private TrayIconService? trayIcon;
    private MainWindow? mainWindow;
    private FolderQuickEditMouseHook? quickEditHook;
    private DynamicIslandController? islandController;

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        var settingsStore = new JsonAppSettingsStore();
        var settings = await settingsStore.LoadAsync();
        var localization = new LocalizationService(settings.Language);
        var dialogs = new DialogService(() => MainWindow);
        var monitoringStore = new JsonMonitoringRuleStore();
        var folderCategoryCatalog = new JsonFolderCategoryCatalog();
        var folderMonitoring = new FolderMonitoringCoordinator(
            monitoringStore,
            new MonitoredFolderIconService());
        var monitoringAssets = new MonitoringIconAssetService();
        var categoryPopup = new FolderCategoryPopupWindow();
        categoryPopup.SetCardOpacity(settings.CardOpacity);
        var folderHoverMonitor = new FolderHoverMonitor(
            folderCategoryCatalog,
            new ExplorerFolderHoverTargetResolver(),
            new WindowsCursorPositionSource(),
            categoryPopup,
            new WpfViewModelDispatcher());
        var viewModel = new MainViewModel(
            new ThemeApplicationCoordinator(new ThemeApplicationService()),
            dialogs,
            new ThemeCatalog(new ThemeStore()),
            new FolderPreviewRenderer(),
            new TaskPreviewDelay(),
            new WpfViewModelDispatcher(),
            folderMonitoring: folderMonitoring,
            monitoringAssets: monitoringAssets);
        viewModel.Material = MaterialStyle.Satin;
        viewModel.Opacity = 1;
        viewModel.HighlightStrength = .28;
        viewModel.GlowStrength = .06;
        viewModel.StrokeOpacity = .45;
        viewModel.ShadowStrength = .22;
        viewModel.ShadowOffset = 4;
        viewModel.SelectedMode = ApplicationMode.Compatible;
        viewModel.ApplyPalette(settings.Palette);
        var startupRegistration = new StartupRegistrationService();
        try { startupRegistration.SetEnabled(settings.StartWithWindows); }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.Security.SecurityException)
        {
            // Startup registration is optional; the main application remains usable.
        }
        trayIcon = new TrayIconService();
        islandController = new DynamicIslandController(new IslandSettingsStore(),
            () => mainWindow?.StartupEnabled ?? settings.StartWithWindows,
            enabled => mainWindow!.SetStartupEnabledAsync(enabled), ExitApplication);
        var window = new MainWindow(
            viewModel,
            settingsStore,
            localization,
            settings,
            startupRegistration,
            () => trayIcon.ShowBackgroundNotice(),
            monitoringStore,
            folderMonitoring,
            monitoringAssets,
            dialogs,
            folderCategoryCatalog,
            folderHoverMonitor,
            categoryPopup.SetCardOpacity,
            chord => quickEditHook?.UpdateChord(chord), islandController);
        mainWindow = window;
        MainWindow = window;
        try
        {
            quickEditHook = new FolderQuickEditMouseHook(folderHoverMonitor,
                path => _ = window.OpenFolderCategoryEditorAsync(path), settings.EditChord, Dispatcher);
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // Folder styling remains available if Windows denies the global mouse hook.
        }
        window.Closed += (_, _) => quickEditHook?.Dispose();
        trayIcon.OpenRequested += (_, _) => window.Dispatcher.Invoke(window.ShowFromTray);
        trayIcon.ToggleMonitoringRequested += async (_, _) => await viewModel.ToggleMonitoringAsync();
        trayIcon.ExitRequested += (_, _) => window.Dispatcher.Invoke(ExitApplication);
        await viewModel.InitializeAsync();
        await window.StartBackgroundServicesAsync();
        try { await islandController.RestoreAsync(); }
        catch (Exception ex) when (ex is System.IO.IOException or UnauthorizedAccessException) { /* 主软件保持可用，用户仍可从首页重试。 */ }
        var background = e.Args.Any(argument => string.Equals(argument, "--background", StringComparison.OrdinalIgnoreCase));
        if (!background)
        {
            window.Show();
            await window.ShowFirstRunTutorialAsync();
        }
    }

    private void ExitApplication()
    {
        islandController?.Dispose();
        quickEditHook?.Dispose();
        trayIcon?.Dispose();
        trayIcon = null;
        mainWindow?.RequestExplicitExit();
        Shutdown();
    }
}
```

## src/FolderThemeStudio.App/AutoStyleRulesWindow.xaml

```xml
<controls:RoundedWindow x:Class="FolderThemeStudio.App.AutoStyleRulesWindow" Style="{StaticResource {x:Type Window}}"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:controls="clr-namespace:FolderThemeStudio.App.Controls"
        mc:Ignorable="d"
        Title="{DynamicResource AutoRules.Window.Title}"
        Width="1080"
        Height="760"
        MinWidth="780"
        MinHeight="600"
        WindowStartupLocation="CenterOwner"
        ShowInTaskbar="False"
        UseLayoutRounding="True">
    <Grid Margin="24">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <Grid Grid.Row="0" Margin="0,0,0,18">
            <StackPanel>
                <TextBlock FontSize="25" FontWeight="SemiBold" Text="{DynamicResource AutoRules.Window.Title}" />
                <TextBlock Margin="0,4,0,0" Foreground="{StaticResource MutedTextBrush}"
                           Text="{DynamicResource AutoRules.Window.Subtitle}" />
            </StackPanel>
        </Grid>

        <Grid Grid.Row="1" IsEnabled="{Binding CanEdit}">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="1.08*" />
                <ColumnDefinition Width="16" />
                <ColumnDefinition Width="0.92*" />
            </Grid.ColumnDefinitions>

            <Border Grid.Column="0" Style="{StaticResource CardStyle}" Padding="18">
                <DockPanel LastChildFill="True">
                    <StackPanel DockPanel.Dock="Top" Margin="0,0,0,12">
                        <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource AutoRules.Root.Label}" />
                        <ComboBox ItemsSource="{Binding RootPaths}"
                                  SelectedItem="{Binding SelectedRootPath, UpdateSourceTrigger=PropertyChanged}"
                                  ToolTip="{DynamicResource AutoRules.Root.Label}">
                            <ComboBox.ItemTemplate>
                                <DataTemplate>
                                    <TextBlock Text="{Binding}" TextTrimming="CharacterEllipsis" />
                                </DataTemplate>
                            </ComboBox.ItemTemplate>
                        </ComboBox>
                        <TextBlock x:Name="AutoRulesFallback" Margin="2,6,0,0" FontSize="11"
                                   Foreground="{StaticResource MutedTextBrush}" TextTrimming="CharacterEllipsis">
                            <Run Text="{DynamicResource AutoRules.Fallback}" />
                            <Run Text="{Binding FallbackIconName, Mode=OneWay}" />
                        </TextBlock>
                        <Grid Margin="0,20,0,8">
                            <TextBlock FontSize="17" FontWeight="SemiBold" Text="{DynamicResource AutoRules.List.Title}" />
                            <Button HorizontalAlignment="Right" Padding="10,4" MinHeight="30"
                                    Command="{Binding AddNewRuleCommand}"
                                    Content="{DynamicResource AutoRules.Action.New}" />
                        </Grid>
                    </StackPanel>

                    <Grid>
                        <ListView x:Name="AutoRulesList" ItemsSource="{Binding Rules}"
                                  SelectedItem="{Binding SelectedRule, Mode=TwoWay}"
                                  BorderThickness="0"
                                  Background="Transparent"
                                  ScrollViewer.HorizontalScrollBarVisibility="Disabled">
                            <ListView.ItemContainerStyle>
                                <Style TargetType="ListViewItem">
                                    <Setter Property="Padding" Value="0" />
                                    <Setter Property="Margin" Value="0,0,0,8" />
                                    <Setter Property="HorizontalContentAlignment" Value="Stretch" />
                                </Style>
                            </ListView.ItemContainerStyle>
                            <ListView.ItemTemplate>
                                <DataTemplate>
                                    <Border Background="#F7F9FC" BorderBrush="{StaticResource BorderBrush}"
                                            BorderThickness="1" CornerRadius="12" Padding="12">
                                        <Grid>
                                            <Grid.ColumnDefinitions>
                                                <ColumnDefinition Width="54" />
                                                <ColumnDefinition Width="*" />
                                                <ColumnDefinition Width="Auto" />
                                            </Grid.ColumnDefinitions>
                                            <Border Width="48" Height="48" CornerRadius="11" Background="White"
                                                    BorderBrush="{StaticResource BorderBrush}" BorderThickness="1">
                                                <Image Margin="5" Source="{Binding Preview}" Stretch="Uniform" />
                                            </Border>
                                            <StackPanel Grid.Column="1" VerticalAlignment="Center" Margin="8,0">
                                                <TextBlock FontWeight="SemiBold" Text="{Binding Keyword}" TextTrimming="CharacterEllipsis" />
                                                <TextBlock Margin="0,4,0,0" FontSize="11" Foreground="{StaticResource MutedTextBrush}"
                                                           Text="{Binding IconName}" TextTrimming="CharacterEllipsis" />
                                            </StackPanel>
                                            <StackPanel Grid.Column="2" VerticalAlignment="Center" HorizontalAlignment="Right">
                                                <TextBlock FontSize="11" Foreground="{StaticResource MutedTextBrush}"
                                                           Text="{DynamicResource AutoRules.List.Priority}" HorizontalAlignment="Right" />
                                                <TextBlock FontSize="14" FontWeight="SemiBold" Text="{Binding Order, StringFormat={}{0:00}}"
                                                           HorizontalAlignment="Right" />
                                                <CheckBox Margin="0,4,0,0" IsChecked="{Binding Enabled, Mode=OneWay}"
                                                          IsEnabled="False" ToolTip="{DynamicResource AutoRules.List.Enabled}"
                                                          HorizontalAlignment="Right" />
                                            </StackPanel>
                                        </Grid>
                                    </Border>
                                </DataTemplate>
                            </ListView.ItemTemplate>
                        </ListView>
                        <Border x:Name="AutoRulesEmptyState" Margin="12" Padding="24" Background="#F7F9FC" CornerRadius="14"
                                Visibility="{Binding IsEmpty, Converter={StaticResource BoolToVisibility}}">
                            <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center" MaxWidth="300">
                                <TextBlock FontSize="17" FontWeight="SemiBold" TextAlignment="Center"
                                           Text="{DynamicResource AutoRules.List.EmptyTitle}" />
                                <TextBlock Margin="0,8,0,0" Foreground="{StaticResource MutedTextBrush}"
                                           TextAlignment="Center" TextWrapping="Wrap"
                                           Text="{DynamicResource AutoRules.List.EmptyBody}" />
                            </StackPanel>
                        </Border>
                    </Grid>
                </DockPanel>
            </Border>

            <Border Grid.Column="2" Style="{StaticResource CardStyle}" Padding="20">
                <ScrollViewer VerticalScrollBarVisibility="Auto" HorizontalScrollBarVisibility="Disabled">
                    <StackPanel>
                        <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource AutoRules.Form.Title}" />
                        <Label Style="{StaticResource FieldLabelStyle}" Content="{DynamicResource AutoRules.Form.Keyword}" />
                        <TextBox x:Name="AutoRulesKeyword" Text="{Binding Keyword, UpdateSourceTrigger=PropertyChanged}" />

                        <TextBlock Margin="0,16,0,6" FontWeight="SemiBold" Text="{DynamicResource AutoRules.Form.Preview}" />
                        <Grid MinHeight="132">
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="132" />
                                <ColumnDefinition Width="*" />
                            </Grid.ColumnDefinitions>
                            <Border Width="116" Height="116" CornerRadius="18" Background="#F7F9FC"
                                    BorderBrush="{StaticResource BorderBrush}" BorderThickness="1">
                                <Image x:Name="AutoRulesIconPreview" Margin="12" Source="{Binding IconPreview}" Stretch="Uniform" />
                            </Border>
                            <StackPanel Grid.Column="1" VerticalAlignment="Center">
                                <CheckBox Content="{DynamicResource AutoRules.Form.CurrentTheme}"
                                          IsChecked="{Binding UseEditorTheme, Mode=TwoWay}" />
                                <Button Margin="0,8,0,0" HorizontalAlignment="Left"
                                        Command="{Binding ChooseImageCommand}"
                                        Content="{DynamicResource AutoRules.Form.ChooseImage}" />
                                <TextBlock MaxWidth="250" Foreground="{StaticResource MutedTextBrush}"
                                           Text="{Binding SelectedImagePath}" TextTrimming="CharacterEllipsis" />
                            </StackPanel>
                        </Grid>

                        <CheckBox Margin="0,16,0,0" Content="{DynamicResource AutoRules.Form.Enabled}"
                                  IsChecked="{Binding Enabled, Mode=TwoWay}" />

                        <WrapPanel Margin="0,20,0,0">
                            <Button x:Name="AutoRulesSaveButton" Command="{Binding SaveRuleCommand}" Style="{StaticResource PrimaryButtonStyle}"
                                    Content="{DynamicResource AutoRules.Action.Save}" />
                            <Button x:Name="AutoRulesCancelButton" Command="{Binding CancelEditCommand}"
                                    Content="{DynamicResource AutoRules.Action.Cancel}" />
                            <Button Command="{Binding DeleteRuleCommand}" CommandParameter="{Binding SelectedRule.Id}"
                                    Content="{DynamicResource AutoRules.Action.Delete}" />
                        </WrapPanel>
                        <WrapPanel>
                            <Button Command="{Binding MoveUpCommand}" CommandParameter="{Binding SelectedRule.Id}"
                                    Content="{DynamicResource AutoRules.Action.MoveUp}" />
                            <Button Command="{Binding MoveDownCommand}" CommandParameter="{Binding SelectedRule.Id}"
                                    Content="{DynamicResource AutoRules.Action.MoveDown}" />
                        </WrapPanel>
                        <TextBlock Margin="0,8,0,0" Foreground="{StaticResource MutedTextBrush}"
                                   TextWrapping="Wrap" Text="{Binding StatusText}" />
                    </StackPanel>
                </ScrollViewer>
            </Border>
        </Grid>

        <Border Grid.Row="2" Margin="0,16,0,0" Padding="16" CornerRadius="12"
                Background="#EAF1FB" BorderBrush="#D7E4F5" BorderThickness="1">
            <DockPanel>
                <Button DockPanel.Dock="Right" Margin="12,0,0,0" Content="{DynamicResource AutoRules.Action.Close}"
                        IsCancel="True" Click="Close_Click" />
                <TextBlock x:Name="AutoRulesHelp" VerticalAlignment="Center" Foreground="{StaticResource MutedTextBrush}"
                           TextWrapping="Wrap" Text="{DynamicResource AutoRules.Help}" />
            </DockPanel>
        </Border>
    </Grid>
</controls:RoundedWindow>
```

## src/FolderThemeStudio.App/AutoStyleRulesWindow.xaml.cs

```csharp
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
```

## src/FolderThemeStudio.App/Controls/RoundedWindow.cs

```csharp
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
```

## src/FolderThemeStudio.App/Controls/VisualColorPicker.xaml

```xml
<UserControl x:Class="FolderThemeStudio.App.Controls.VisualColorPicker"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             MinWidth="300">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*" />
            <ColumnDefinition Width="16" />
            <ColumnDefinition Width="28" />
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="210" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <Border x:Name="PaletteSurface" Grid.Column="0" CornerRadius="8"
                Background="{Binding HueBrush}"
                MouseLeftButtonDown="Palette_Mouse" MouseMove="Palette_Mouse"
                MouseLeftButtonUp="EndMouseInteraction">
            <Grid ClipToBounds="True">
                <Rectangle>
                    <Rectangle.Fill>
                        <LinearGradientBrush StartPoint="0,0.5" EndPoint="1,0.5">
                            <GradientStop Color="White" Offset="0" />
                            <GradientStop Color="#00FFFFFF" Offset="1" />
                        </LinearGradientBrush>
                    </Rectangle.Fill>
                </Rectangle>
                <Rectangle>
                    <Rectangle.Fill>
                        <LinearGradientBrush StartPoint="0.5,0" EndPoint="0.5,1">
                            <GradientStop Color="#00000000" Offset="0" />
                            <GradientStop Color="Black" Offset="1" />
                        </LinearGradientBrush>
                    </Rectangle.Fill>
                </Rectangle>
                <Canvas IsHitTestVisible="False">
                    <Ellipse x:Name="PaletteMarker" Width="14" Height="14" Stroke="White" StrokeThickness="2">
                        <Ellipse.Effect><DropShadowEffect BlurRadius="3" ShadowDepth="0" /></Ellipse.Effect>
                    </Ellipse>
                </Canvas>
            </Grid>
        </Border>

        <Border x:Name="HueSurface" Grid.Column="2" CornerRadius="6"
                MouseLeftButtonDown="Hue_Mouse" MouseMove="Hue_Mouse"
                MouseLeftButtonUp="EndMouseInteraction">
            <Border.Background>
                <LinearGradientBrush StartPoint="0.5,0" EndPoint="0.5,1">
                    <GradientStop Color="#FFFF0000" Offset="0" />
                    <GradientStop Color="#FFFFFF00" Offset="0.167" />
                    <GradientStop Color="#FF00FF00" Offset="0.333" />
                    <GradientStop Color="#FF00FFFF" Offset="0.5" />
                    <GradientStop Color="#FF0000FF" Offset="0.667" />
                    <GradientStop Color="#FFFF00FF" Offset="0.833" />
                    <GradientStop Color="#FFFF0000" Offset="1" />
                </LinearGradientBrush>
            </Border.Background>
        </Border>

        <Grid Grid.Row="1" Grid.ColumnSpan="3" Margin="0,12,0,0">
            <Grid.ColumnDefinitions><ColumnDefinition Width="48" /><ColumnDefinition Width="*" /></Grid.ColumnDefinitions>
            <Border Width="38" Height="38" CornerRadius="6" Background="{Binding SelectedBrush}" />
            <TextBox Grid.Column="1" Text="{Binding Hex, UpdateSourceTrigger=PropertyChanged}" />
        </Grid>
    </Grid>
</UserControl>
```

## src/FolderThemeStudio.App/Controls/VisualColorPicker.xaml.cs

```csharp
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
```

## src/FolderThemeStudio.App/DynamicIsland/ClipboardTextListener.cs

```csharp
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;

namespace FolderThemeStudio.App.DynamicIsland;

public static class ClipboardPreview
{
    /// <summary>按文本元素截取，避免在第 80 个字符处切断中文组合字或 emoji。</summary>
    public static string Create(string text)
    {
        var normalized = text.Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ').Trim();
        var starts = StringInfo.ParseCombiningCharacters(normalized);
        return starts.Length > 80 ? normalized[..starts[80]] + "…" : normalized;
    }
}

/// <summary>仅在用户开启剪贴板模块时注册监听；最新预览只保存在内存，关闭模块即清空。</summary>
public interface IIslandClipboardListener : IDisposable { void SetEnabled(bool value); }
public sealed class ClipboardTextListener : IIslandClipboardListener
{
    private readonly HwndSource source;
    private readonly Action<string> changed;
    private readonly DispatcherTimer retry = new() { Interval = TimeSpan.FromMilliseconds(100) };
    private bool enabled;
    private int attempts;
    public ClipboardTextListener(HwndSource source, Action<string> changed)
    {
        this.source = source;
        this.changed = changed;
        source.AddHook(WindowMessage);
        retry.Tick += Retry_Tick;
    }
    public void SetEnabled(bool value)
    {
        if (value == enabled) return;
        retry.Stop();
        if (value)
        {
            if (!AddClipboardFormatListener(source.Handle)) { changed("剪贴板监听不可用"); return; }
            enabled = true;
            Read();
        }
        else
        {
            RemoveClipboardFormatListener(source.Handle);
            enabled = false;
            changed(string.Empty);
        }
    }
    private nint WindowMessage(nint hwnd, int message, nint wParam, nint lParam, ref bool handled)
    {
        if (enabled && message == 0x031D) { attempts = 0; Read(); } // WM_CLIPBOARDUPDATE
        return nint.Zero;
    }
    private void Retry_Tick(object? sender, EventArgs e) { retry.Stop(); Read(); }
    private void Read()
    {
        if (!enabled) return;
        try
        {
            changed(System.Windows.Clipboard.ContainsText() ? ClipboardPreview.Create(System.Windows.Clipboard.GetText()) : string.Empty);
        }
        catch (ExternalException)
        {
            // 其他应用可能短暂占用剪贴板。限次重试，不在消息回调里阻塞 UI。
            if (++attempts <= 3) retry.Start();
        }
    }
    public void Dispose()
    {
        retry.Stop();
        retry.Tick -= Retry_Tick;
        if (enabled) RemoveClipboardFormatListener(source.Handle);
        enabled = false;
        source.RemoveHook(WindowMessage);
    }
    [DllImport("user32.dll", SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] private static extern bool AddClipboardFormatListener(nint hwnd);
    [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] private static extern bool RemoveClipboardFormatListener(nint hwnd);
}
```

## src/FolderThemeStudio.App/DynamicIsland/DynamicIslandController.cs

```csharp
using System.IO;
using System.Windows.Threading;

namespace FolderThemeStudio.App.DynamicIsland;

public interface IDynamicIslandController : IDisposable { Task ShowAsync(); }

/// <summary>全应用仅创建一个灵动岛；位置/透明度合并保存，退出软件保留 Enabled，下次启动恢复。</summary>
public sealed class DynamicIslandController : IDynamicIslandController
{
    private readonly IIslandSettingsStore store;
    private readonly Func<bool> startup;
    private readonly Func<bool, Task> setStartup;
    private readonly Action exit;
    private readonly Func<IslandSettings, DynamicIslandWindow> createWindow;
    private readonly SemaphoreSlim gate = new(1, 1);
    private readonly DispatcherTimer debounce = new() { Interval = TimeSpan.FromMilliseconds(450) };
    private IslandSettings settings = new();
    private DynamicIslandWindow? window;
    private DynamicIslandSettingsWindow? settingsWindow;
    private bool loaded, disposed, applyingSettings;
    public DynamicIslandController(IIslandSettingsStore store, Func<bool> startup, Func<bool, Task> setStartup, Action exit,
        Func<IslandSettings, DynamicIslandWindow>? createWindow = null)
    {
        this.store = store; this.startup = startup; this.setStartup = setStartup; this.exit = exit;
        this.createWindow = createWindow ?? (settings => new DynamicIslandWindow(settings));
        debounce.Tick += SaveDebounced_Tick;
    }
    public async Task RestoreAsync()
    {
        settings = await store.LoadAsync(); loaded = true;
        if (settings.Enabled) await ShowAsync();
    }
    public async Task ShowAsync()
    {
        await gate.WaitAsync();
        try
        {
            if (disposed) return;
            if (!loaded) { settings = await store.LoadAsync(); loaded = true; }
            if (disposed) return;
            if (window is not null) { window.Show(); return; }
            settings = settings with { Enabled = true };
            await store.SaveAsync(settings);
            if (disposed) return;
            window = createWindow(settings);
            window.SnapshotChanged += (_, _) => { if (!applyingSettings) { debounce.Stop(); debounce.Start(); } };
            window.SettingsRequested += (_, _) => OpenSettings();
            window.CloseRequested += async (_, _) => await HideAsync();
            window.ExitRequested += (_, _) => exit();
            window.Show();
        }
        finally { gate.Release(); }
    }
    private async void SaveDebounced_Tick(object? sender, EventArgs e)
    {
        debounce.Stop();
        var savingWindow = window;
        await gate.WaitAsync();
        try
        {
            if (savingWindow is null || window != savingWindow || disposed || applyingSettings) return;
            settings = savingWindow.CaptureSettings();
            try { await store.SaveAsync(settings); window?.Model.ReportStatus(string.Empty); }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { window?.Model.ReportStatus("位置保存失败，请检查配置目录权限"); }
        }
        finally { gate.Release(); }
    }
    private void OpenSettings()
    {
        if (window is null) return;
        if (settingsWindow is not null) { settingsWindow.Activate(); return; }
        var editingWindow = window;
        settingsWindow = new DynamicIslandSettingsWindow(window.CaptureSettings(), startup(), async (next, enabled) =>
        {
            await gate.WaitAsync();
            try
            {
                // 保留面板打开期间发生的拖动/滚轮更改，设置面板只覆盖它管理的字段。
                if (window != editingWindow || disposed) return;
                var position = window.CaptureSettings();
                next = next with { Enabled = true, Left = position.Left, Top = position.Top };
                applyingSettings = true;
                debounce.Stop();
                try
                {
                    await store.SaveAsync(next);
                    await setStartup(enabled);
                    if (window is null || disposed) return;
                    settings = next;
                    window.ApplySettings(next);
                }
                finally
                {
                    applyingSettings = false;
                    if (window is not null && !disposed) debounce.Start();
                }
            }
            finally { gate.Release(); }
        }, window.Model.RequestNotificationAccessAsync);
        settingsWindow.Closed += (_, _) => settingsWindow = null;
        settingsWindow.Show();
    }
    private async Task HideAsync()
    {
        // 与打开/设置保存共用生命周期锁，关闭开始后不能被待处理的设置写回 Enabled=true。
        await gate.WaitAsync();
        try
        {
            debounce.Stop();
            if (window is null || disposed) return;
            var closing = window;
            var next = closing.CaptureSettings() with { Enabled = false };
            try { await store.SaveAsync(next); }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { if (!disposed) closing.Model.ReportStatus("关闭设置保存失败，请重试"); return; }
            if (window != closing || disposed) return;
            settings = next;
            settingsWindow?.Close();
            window.CloseFromController(); window = null;
        }
        finally { gate.Release(); }
    }
    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        debounce.Stop(); debounce.Tick -= SaveDebounced_Tick;
        if (window is not null) settings = window.CaptureSettings();
        settingsWindow?.Close(); window?.CloseFromController(); window = null;
        // Store 的 I/O 使用 ConfigureAwait(false)，退出时可安全同步刷新最后的位置。
        if (loaded) try { store.SaveAsync(settings).GetAwaiter().GetResult(); } catch (Exception e) when (e is IOException or UnauthorizedAccessException) { }
    }
}
```

## src/FolderThemeStudio.App/DynamicIsland/DynamicIslandSettingsWindow.xaml

```xml
<Window x:Class="FolderThemeStudio.App.DynamicIsland.DynamicIslandSettingsWindow" Style="{StaticResource {x:Type Window}}"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="灵动岛设置" Width="470" Height="690" MinHeight="500" ResizeMode="CanResize"
        WindowStartupLocation="CenterScreen" ShowInTaskbar="False">
    <Grid Margin="24">
        <Grid.RowDefinitions><RowDefinition/><RowDefinition Height="Auto"/></Grid.RowDefinitions>
        <ScrollViewer VerticalScrollBarVisibility="Auto">
            <StackPanel Margin="0,0,6,12">
                <TextBlock Text="桌面灵动岛" FontSize="24" FontWeight="SemiBold"/>
                <TextBlock Text="左键拖动位置，滚轮调整透明度，右键打开设置。" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Margin="0,10,0,22"/>
                <TextBlock Text="显示模块" FontWeight="SemiBold" Margin="0,0,0,10"/>
                <CheckBox x:Name="TimeCheck" Content="当前时间" Margin="0,5"/>
                <CheckBox x:Name="CpuCheck" Content="CPU 使用率" Margin="0,5"/>
                <CheckBox x:Name="MemoryCheck" Content="内存使用率" Margin="0,5"/>
                <CheckBox x:Name="ClipboardCheck" Content="最新剪贴板文本（仅保留内存预览）" Margin="0,5"/>
                <CheckBox x:Name="MusicCheck" Content="歌曲信息与同步歌词" Margin="0,5"/>
                <CheckBox x:Name="OnlineLyricsCheck" Content="在线获取歌词（向歌词服务发送歌名和歌手）" Margin="0,5"/>
                <CheckBox x:Name="NotificationsCheck" Content="通知与消息（系统通知访问需用户授权）" Margin="0,5"/>
                <Button Content="请求通知读取权限" Click="NotificationAccess_Click" HorizontalAlignment="Left" Margin="0,8,0,10"/>
                <TextBlock x:Name="NotificationStatus" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap"/>
                <TextBlock Text="屏蔽消息来源（应用名称或 ID，多项用分号分隔）" Margin="0,12,0,5" TextWrapping="Wrap"/>
                <TextBox x:Name="BlockedSourcesBox" MaxLength="2048"/>
                <TextBlock Text="只提醒开启后的新消息，每条 6 秒；相同消息去重，最多排队 8 条。来源名称显示在提醒与悬停详情中。" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Margin="0,6,0,8"/>
                <CheckBox x:Name="LocalNotificationsCheck" Content="启用本地消息接口（仅 127.0.0.1，需通知模块开启）" Margin="0,10,0,5"/>
                <TextBlock Text="本地消息端口（1024–65535）" Margin="0,5"/>
                <TextBox x:Name="NotificationPortBox"/>
                <DockPanel Margin="0,8"><Button DockPanel.Dock="Right" Content="复制接口令牌" Click="CopyToken_Click"/><TextBlock Text="令牌只在本机使用" VerticalAlignment="Center"/></DockPanel>
                <TextBlock Text="透明度" FontWeight="SemiBold" Margin="0,18,0,8"/>
                <Slider x:Name="OpacitySlider" Minimum="20" Maximum="100" TickFrequency="5" IsSnapToTickEnabled="True"/>
                <CheckBox x:Name="DragMotionCheck" Content="弹性拖动（遵循 Windows 动画设置）" Margin="0,14,0,5"/>
                <CheckBox x:Name="SnapEdgesCheck" Content="靠近屏幕边缘时吸附" Margin="0,5"/>
                <TextBlock Text="刷新间隔（秒，1–60）" FontWeight="SemiBold" Margin="0,18,0,8"/>
                <TextBox x:Name="IntervalBox"/>
                <TextBlock Text="LRC 歌词文件夹" FontWeight="SemiBold" Margin="0,18,0,8"/>
                <DockPanel><Button DockPanel.Dock="Right" Content="选择" Click="LyricsDirectory_Click" Margin="8,0,0,0"/><TextBox x:Name="LyricsDirectoryBox"/></DockPanel>
                <TextBlock Text="本地 LRC 优先，文件名为歌曲名.lrc 或 歌手 - 歌曲名.lrc；开启在线歌词后自动回退获取。" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Margin="0,8,0,14"/>
                <CheckBox x:Name="StartupCheck" Content="随 Windows 启动软件（与主软件共用此设置）" Margin="0,6"/>
                <TextBlock x:Name="ErrorText" TextWrapping="Wrap" Foreground="#D42B25" Margin="0,12,0,0"/>
            </StackPanel>
        </ScrollViewer>
        <StackPanel Grid.Row="1" Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,14,0,0">
            <Button Content="取消" Click="Cancel_Click" Margin="0,0,10,0"/>
            <Button x:Name="SaveButton" Content="保存" Click="Save_Click" Style="{StaticResource PrimaryButtonStyle}" Margin="0"/>
        </StackPanel>
    </Grid>
</Window>
```

## src/FolderThemeStudio.App/DynamicIsland/DynamicIslandSettingsWindow.xaml.cs

```csharp
using System.IO;
using System.Windows;

namespace FolderThemeStudio.App.DynamicIsland;

/// <summary>编辑副本，只有保存成功才应用到窗口；取消不会修改正在运行的配置。</summary>
public partial class DynamicIslandSettingsWindow : Window
{
    private readonly IslandSettings initial;
    private readonly Func<IslandSettings, bool, Task> save;
    private readonly Func<Task<string>> requestAccess;
    public DynamicIslandSettingsWindow(IslandSettings settings, bool startup, Func<IslandSettings, bool, Task> save, Func<Task<string>> requestAccess)
    {
        InitializeComponent();
        initial = settings; this.save = save; this.requestAccess = requestAccess;
        TimeCheck.IsChecked = settings.ShowTime; CpuCheck.IsChecked = settings.ShowCpu; MemoryCheck.IsChecked = settings.ShowMemory;
        ClipboardCheck.IsChecked = settings.ShowClipboard; MusicCheck.IsChecked = settings.ShowMusic; NotificationsCheck.IsChecked = settings.ShowNotifications;
        StartupCheck.IsChecked = startup; OpacitySlider.Value = settings.Opacity * 100;
        IntervalBox.Text = settings.RefreshSeconds.ToString(); LyricsDirectoryBox.Text = settings.LyricsDirectory;
        OnlineLyricsCheck.IsChecked = settings.OnlineLyrics;
        LocalNotificationsCheck.IsChecked = settings.LocalNotifications;
        NotificationPortBox.Text = settings.NotificationPort.ToString();
        BlockedSourcesBox.Text = settings.BlockedNotificationSources;
        DragMotionCheck.IsChecked = settings.DragMotion; SnapEdgesCheck.IsChecked = settings.SnapToEdges;
    }
    private void LyricsDirectory_Click(object sender, RoutedEventArgs e)
    {
        var picker = new Microsoft.Win32.OpenFolderDialog { Title = "选择 LRC 歌词文件夹" };
        if (picker.ShowDialog(this) == true) LyricsDirectoryBox.Text = picker.FolderName;
    }
    private async void NotificationAccess_Click(object sender, RoutedEventArgs e) => NotificationStatus.Text = await requestAccess();
    private void CopyToken_Click(object sender, RoutedEventArgs e)
    {
        try { System.Windows.Clipboard.SetText(initial.NotificationToken); NotificationStatus.Text = "已复制本地接口令牌"; }
        catch (System.Runtime.InteropServices.ExternalException) { NotificationStatus.Text = "剪贴板被占用，请稍后重试"; }
    }
    private async void Save_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(IntervalBox.Text, out var seconds) || seconds is < 1 or > 60) { ErrorText.Text = "刷新间隔需为 1–60 的整数。"; return; }
        if (!int.TryParse(NotificationPortBox.Text, out var port) || port is < 1024 or > 65535) { ErrorText.Text = "消息端口需为 1024–65535 的整数。"; return; }
        var lyrics = LyricsDirectoryBox.Text.Trim();
        if (lyrics.Length > 0 && !Directory.Exists(lyrics)) { ErrorText.Text = "歌词文件夹不存在，请重新选择。"; return; }
        SaveButton.IsEnabled = false;
        try
        {
            await save(initial with { ShowTime = TimeCheck.IsChecked == true, ShowCpu = CpuCheck.IsChecked == true,
                ShowMemory = MemoryCheck.IsChecked == true, ShowClipboard = ClipboardCheck.IsChecked == true,
                ShowMusic = MusicCheck.IsChecked == true, ShowNotifications = NotificationsCheck.IsChecked == true,
                Opacity = OpacitySlider.Value / 100, RefreshSeconds = seconds, LyricsDirectory = lyrics,
                OnlineLyrics = OnlineLyricsCheck.IsChecked == true, LocalNotifications = LocalNotificationsCheck.IsChecked == true,
                NotificationPort = port, BlockedNotificationSources = BlockedSourcesBox.Text,
                DragMotion = DragMotionCheck.IsChecked == true, SnapToEdges = SnapEdgesCheck.IsChecked == true }, StartupCheck.IsChecked == true);
            Close();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        { ErrorText.Text = "保存失败，请检查配置目录或开机启动权限。"; }
        finally { SaveButton.IsEnabled = true; }
    }
    private void Cancel_Click(object sender, RoutedEventArgs e) => Close();
}
```

## src/FolderThemeStudio.App/DynamicIsland/DynamicIslandWindow.xaml

```xml
<Window x:Class="FolderThemeStudio.App.DynamicIsland.DynamicIslandWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="桌面灵动岛" Width="260" Height="64" WindowStyle="None"
        AllowsTransparency="True" Background="Transparent" Topmost="True" ShowInTaskbar="False"
        ShowActivated="False" ResizeMode="NoResize" WindowStartupLocation="Manual"
        MouseLeftButtonDown="Window_MouseLeftButtonDown" PreviewMouseWheel="Window_PreviewMouseWheel"
        Loaded="Window_Loaded" LocationChanged="Window_LocationChanged">
    <Window.Resources>
        <BooleanToVisibilityConverter x:Key="IslandVisible"/>
        <Style TargetType="TextBlock">
            <Setter Property="Foreground" Value="#F5F5F7"/>
            <Setter Property="FontSize" Value="11"/>
            <Setter Property="TextWrapping" Value="NoWrap"/>
            <Setter Property="TextTrimming" Value="CharacterEllipsis"/>
        </Style>
    </Window.Resources>
    <!-- 固定外形：全部模块开启或文本很长时，也不增高为信息面板。 -->
    <Border x:Name="IslandSurface" Margin="6" Padding="12,7" Background="#F00B0B0D"
            CornerRadius="26" BorderBrush="#24FFFFFF" BorderThickness="1" RenderTransformOrigin=".5,.5">
        <Border.RenderTransform><TransformGroup><ScaleTransform x:Name="DragScale"/><RotateTransform x:Name="DragTilt"/></TransformGroup></Border.RenderTransform>
        <Border.Effect><DropShadowEffect x:Name="DragShadow" BlurRadius="10" ShadowDepth="2" Opacity=".2"/></Border.Effect>
        <Border.ContextMenu>
            <ContextMenu>
                <MenuItem Header="设置" Click="Settings_Click"/>
                <Separator/>
                <MenuItem Header="关闭灵动岛" Click="CloseIsland_Click"/>
                <MenuItem Header="退出软件" Click="ExitApplication_Click"/>
                <Separator/>
                <MenuItem Header="播放 / 暂停" Click="TogglePlayback_Click"/>
                <MenuItem Header="上一首" Click="PreviousTrack_Click"/>
                <MenuItem Header="下一首" Click="NextTrack_Click"/>
            </ContextMenu>
        </Border.ContextMenu>
        <Grid>
            <Grid.RowDefinitions><RowDefinition Height="17"/><RowDefinition Height="*"/></Grid.RowDefinitions>
            <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                <TextBlock Text="{Binding NotificationBadge}" Foreground="#92D5AB" Width="13" FontSize="9" Margin="0,0,4,0" ToolTip="{Binding NotificationSource}"/>
                <TextBlock Text="{Binding TimeText}" FontFamily="Consolas" FontSize="12" Margin="0,0,10,0"
                           Visibility="{Binding ShowTime, Converter={StaticResource IslandVisible}}"/>
                <TextBlock Text="{Binding CpuText, StringFormat=CPU {0}}" Foreground="#92D5AB" Margin="0,0,10,0"
                           Visibility="{Binding ShowCpu, Converter={StaticResource IslandVisible}}"/>
                <TextBlock Text="{Binding MemoryText, StringFormat=RAM {0}}" Foreground="#B5ACF7"
                           Visibility="{Binding ShowMemory, Converter={StaticResource IslandVisible}}"/>
            </StackPanel>
            <TextBlock x:Name="CompactMessage" Grid.Row="1" Text="{Binding CompactText}" Foreground="#ACACB5"
                       FontSize="10" VerticalAlignment="Center" ToolTipService.InitialShowDelay="700">
                <TextBlock.ToolTip>
                    <ToolTip Background="#202024" BorderBrush="#404047"><TextBlock Text="{Binding CompactDetails}" TextWrapping="Wrap" MaxWidth="300" MaxHeight="160" Foreground="#F5F5F7"/></ToolTip>
                </TextBlock.ToolTip>
            </TextBlock>
        </Grid>
    </Border>
</Window>
```

## src/FolderThemeStudio.App/DynamicIsland/DynamicIslandWindow.xaml.cs

```csharp
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace FolderThemeStudio.App.DynamicIsland;

public static class IslandWindowPolicy
{
    public static double WheelOpacity(double current, int delta) => Math.Clamp(current + delta / 120d * .05, .2, 1);
    public static Point ClampPosition(Point requested, Rect workArea, Size windowSize) => new(
        Math.Clamp(requested.X, workArea.Left, Math.Max(workArea.Left, workArea.Right - windowSize.Width)),
        Math.Clamp(requested.Y, workArea.Top, Math.Max(workArea.Top, workArea.Bottom - windowSize.Height)));
    public static Point SnapPosition(Point position, Rect workArea, Size size)
    {
        var clamped = ClampPosition(position, workArea, size);
        var right = Math.Max(workArea.Left, workArea.Right - size.Width);
        var bottom = Math.Max(workArea.Top, workArea.Bottom - size.Height);
        return new(Math.Abs(clamped.X - workArea.Left) <= 18 ? workArea.Left : Math.Abs(clamped.X - right) <= 18 ? right : clamped.X,
            Math.Abs(clamped.Y - workArea.Top) <= 18 ? workArea.Top : Math.Abs(clamped.Y - bottom) <= 18 ? bottom : clamped.Y);
    }
}

/// <summary>窗口拥有计时器与剪贴板句柄；关闭时全部解除，主软件仍可继续运行。</summary>
public partial class DynamicIslandWindow : Window
{
    public IslandViewModel Model { get; }
    private readonly DispatcherTimer clock = new() { Interval = TimeSpan.FromSeconds(1) };
    private readonly DispatcherTimer refresh = new();
    private IIslandClipboardListener? clipboard;
    private bool controlledClose;
    private readonly IslandDragPhysics dragPhysics = new();
    private readonly Stopwatch dragClock = Stopwatch.StartNew();
    private bool dragging, rendering;
    private double lastLocationTime, lastFrameTime;
    private Point lastDragPosition;
    public event EventHandler? SnapshotChanged;
    public event EventHandler? SettingsRequested;
    public event EventHandler? CloseRequested;
    public event EventHandler? ExitRequested;

    public DynamicIslandWindow(IslandSettings settings, IslandViewModel? model = null,
        Func<HwndSource, Action<string>, IIslandClipboardListener>? clipboardFactory = null)
    {
        InitializeComponent();
        Model = model ?? new IslandViewModel(settings);
        DataContext = Model;
        Opacity = settings.Normalize().Opacity;
        refresh.Interval = TimeSpan.FromSeconds(Model.Settings.RefreshSeconds);
        clock.Tick += Clock_Tick;
        refresh.Tick += Refresh_Tick;
        SourceInitialized += (_, _) =>
        {
            var source = (HwndSource)PresentationSource.FromVisual(this);
            clipboard = clipboardFactory?.Invoke(source, Model.SetClipboard) ?? new ClipboardTextListener(source, Model.SetClipboard);
            clipboard.SetEnabled(Model.ShowClipboard);
        };
        SizeChanged += (_, _) => { if (IsLoaded) ConstrainPosition(); };
        Closing += (_, e) =>
        {
            if (!controlledClose && CloseRequested is not null) { e.Cancel = true; CloseRequested.Invoke(this, EventArgs.Empty); }
        };
        Closed += (_, _) =>
        {
            StopDragRendering();
            clock.Stop(); refresh.Stop();
            clock.Tick -= Clock_Tick; refresh.Tick -= Refresh_Tick;
            clipboard?.Dispose();
            Model.Dispose();
        };
    }
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var work = GetWorkArea();
        var saved = Model.Settings;
        Left = saved.Left ?? work.Left + (work.Width - ActualWidth) / 2;
        Top = saved.Top ?? work.Top + 16;
        ConstrainPosition();
        clock.Start(); refresh.Start();
        await Model.RefreshAsync();
    }
    private void Clock_Tick(object? sender, EventArgs e) => Model.TickClock();
    private async void Refresh_Tick(object? sender, EventArgs e) => await Model.RefreshAsync();
    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState != MouseButtonState.Pressed) return;
        dragging = true; lastDragPosition = new(Left, Top); lastLocationTime = dragClock.Elapsed.TotalSeconds;
        if (MotionEnabled) StartDragRendering();
        try { DragMove(); } catch (InvalidOperationException) { }
        finally
        {
            dragging = false; dragPhysics.Release();
            var position = Model.Settings.SnapToEdges
                ? IslandWindowPolicy.SnapPosition(new(Left, Top), GetWorkArea(), new(ActualWidth, ActualHeight))
                : IslandWindowPolicy.ClampPosition(new(Left, Top), GetWorkArea(), new(ActualWidth, ActualHeight));
            Left = position.X; Top = position.Y;
            SnapshotChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        Opacity = IslandWindowPolicy.WheelOpacity(Opacity, e.Delta);
        e.Handled = true;
        SnapshotChanged?.Invoke(this, EventArgs.Empty);
    }
    private void Window_LocationChanged(object? sender, EventArgs e)
    {
        if (dragging && MotionEnabled)
        {
            var now = dragClock.Elapsed.TotalSeconds;
            var dt = Math.Max(now - lastLocationTime, 1d / 240);
            dragPhysics.Drag((Left - lastDragPosition.X) / dt, (Top - lastDragPosition.Y) / dt);
            lastDragPosition = new(Left, Top); lastLocationTime = now;
        }
        if (IsLoaded && !dragging) SnapshotChanged?.Invoke(this, EventArgs.Empty);
    }
    private bool MotionEnabled => Model.Settings.DragMotion && SystemParameters.ClientAreaAnimation;
    private void StartDragRendering()
    {
        if (rendering) return;
        rendering = true; lastFrameTime = dragClock.Elapsed.TotalSeconds;
        CompositionTarget.Rendering += RenderDragFrame;
    }
    private void StopDragRendering()
    {
        CompositionTarget.Rendering -= RenderDragFrame; rendering = false; dragPhysics.Reset();
        DragScale.ScaleX = DragScale.ScaleY = 1; DragTilt.Angle = 0;
        DragShadow.BlurRadius = 10; DragShadow.Opacity = .2;
    }
    private void RenderDragFrame(object? sender, EventArgs e)
    {
        if (!MotionEnabled) { StopDragRendering(); return; }
        var now = dragClock.Elapsed.TotalSeconds;
        // 停止移动后即使鼠标仍按着也自然恢复，帧回调只在拖动/回弹时存在。
        if (dragging && now - lastLocationTime > .08) dragPhysics.Release();
        dragPhysics.Step(now - lastFrameTime); lastFrameTime = now;
        DragScale.ScaleX = dragPhysics.ScaleX; DragScale.ScaleY = dragPhysics.ScaleY; DragTilt.Angle = dragPhysics.Angle;
        var strength = Math.Clamp(Math.Abs(dragPhysics.ScaleX - 1) / .012, 0, 1);
        DragShadow.BlurRadius = 10 + strength * 3; DragShadow.Opacity = .2 + strength * .1;
        if (!dragging && dragPhysics.IsResting) StopDragRendering();
    }
    public IslandSettings CaptureSettings() => Model.Settings with { Left = Left, Top = Top, Opacity = Opacity };
    public void CloseFromController() { controlledClose = true; Close(); }
    public void ApplySettings(IslandSettings settings)
    {
        Model.ApplySettings(settings);
        if (!MotionEnabled) StopDragRendering();
        Opacity = settings.Normalize().Opacity;
        refresh.Interval = TimeSpan.FromSeconds(Model.Settings.RefreshSeconds);
        clipboard?.SetEnabled(Model.ShowClipboard);
        _ = Model.RefreshAsync();
    }
    private Rect GetWorkArea()
    {
        // Screen 的坐标是物理像素；通过当前 HWND 的 DPI 变换换成 WPF 坐标。
        var work = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle).WorkingArea;
        var transform = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformFromDevice ?? Matrix.Identity;
        return new Rect(transform.Transform(new Point(work.Left, work.Top)), transform.Transform(new Point(work.Right, work.Bottom)));
    }
    private void ConstrainPosition()
    {
        var point = IslandWindowPolicy.ClampPosition(new(Left, Top), GetWorkArea(), new(ActualWidth, ActualHeight));
        Left = point.X; Top = point.Y;
    }
    private void Settings_Click(object sender, RoutedEventArgs e) => SettingsRequested?.Invoke(this, EventArgs.Empty);
    private void CloseIsland_Click(object sender, RoutedEventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
    private void ExitApplication_Click(object sender, RoutedEventArgs e) => ExitRequested?.Invoke(this, EventArgs.Empty);
    private async void TogglePlayback_Click(object sender, RoutedEventArgs e) => await Model.ControlMediaAsync(IslandMediaCommand.TogglePlayPause);
    private async void PreviousTrack_Click(object sender, RoutedEventArgs e) => await Model.ControlMediaAsync(IslandMediaCommand.Previous);
    private async void NextTrack_Click(object sender, RoutedEventArgs e) => await Model.ControlMediaAsync(IslandMediaCommand.Next);
}
```

## src/FolderThemeStudio.App/DynamicIsland/IslandDragPhysics.cs

```csharp
namespace FolderThemeStudio.App.DynamicIsland;

/// <summary>只改变胶囊外观，不让窗口惯性漂移。小步积分使不同刷新率下的阻尼弹簧稳定。</summary>
public sealed class IslandDragPhysics
{
    private double x, y, angle, vx, vy, va, targetX, targetY, targetAngle;
    public double ScaleX => 1 + x;
    public double ScaleY => 1 + y;
    public double Angle => angle;
    public bool IsResting => Math.Abs(x) + Math.Abs(y) + Math.Abs(angle) + Math.Abs(vx) + Math.Abs(vy) + Math.Abs(va) < .0001;
    public void Drag(double velocityX, double velocityY)
    {
        velocityX = double.IsFinite(velocityX) ? Math.Clamp(velocityX, -5000, 5000) : 0;
        velocityY = double.IsFinite(velocityY) ? Math.Clamp(velocityY, -5000, 5000) : 0;
        var speed = Math.Min(1, Math.Sqrt(velocityX * velocityX + velocityY * velocityY) / 2500);
        targetX = .012 * speed; targetY = -.012 * speed;
        targetAngle = Math.Clamp(velocityX / 3500, -1, 1);
    }
    public void Release() { targetX = targetY = targetAngle = 0; }
    public void Reset() { Release(); x = y = angle = vx = vy = va = 0; }
    public void Step(double seconds)
    {
        if (!double.IsFinite(seconds) || seconds <= 0) return;
        seconds = Math.Min(seconds, .1);
        while (seconds > 0)
        {
            var dt = Math.Min(seconds, 1d / 120); seconds -= dt;
            Spring(ref x, ref vx, targetX, dt); Spring(ref y, ref vy, targetY, dt); Spring(ref angle, ref va, targetAngle, dt);
        }
    }
    private static void Spring(ref double position, ref double velocity, double target, double dt)
    {
        velocity += ((target - position) * 240 - velocity * 24) * dt;
        position += velocity * dt;
    }
}
```

## src/FolderThemeStudio.App/DynamicIsland/IslandNotificationHub.cs

```csharp
namespace FolderThemeStudio.App.DynamicIsland;

/// <summary>原生系统通知与可选本地消息汇合，关闭模块就关闭网络监听。</summary>
public sealed class IslandNotificationHub : IIslandNotificationSource, IIslandSettingsAware, IIslandNotificationBatchSource
{
    private readonly WindowsNotificationSource native = new();
    private LocalNotificationBridge? bridge;
    private IslandSettings settings;
    public string Status => bridge is null ? native.Status : bridge.Status + "；" + native.Status;
    public IslandNotificationHub(IslandSettings settings) { this.settings = settings; ApplySettings(settings); }
    public Task<string> RequestAccessAsync() => native.RequestAccessAsync();
    public async Task<IslandNotification?> ReadLatestAsync(CancellationToken token)
        => (await ReadRecentAsync(token)).LastOrDefault();
    public async Task<IReadOnlyList<IslandNotification>> ReadRecentAsync(CancellationToken token)
    {
        var readingBridge = bridge;
        var system = await native.ReadRecentAsync(token);
        var local = readingBridge?.ReadRecent() ?? [];
        return system.Concat(local).OrderBy(item => item.CreatedAt).ToArray();
    }
    public void ApplySettings(IslandSettings next)
    {
        var restart = bridge is null || settings.NotificationPort != next.NotificationPort || settings.NotificationToken != next.NotificationToken;
        settings = next;
        if (!next.ShowNotifications || !next.LocalNotifications) { bridge?.Dispose(); bridge = null; return; }
        if (restart) { bridge?.Dispose(); bridge = new(next.NotificationPort, next.NotificationToken); bridge.Start(); }
    }
    public void Dispose() { bridge?.Dispose(); bridge = null; native.Dispose(); }
}
```

## src/FolderThemeStudio.App/DynamicIsland/IslandNotificationQueue.cs

```csharp
using System.Security.Cryptography;
using System.Text;

namespace FolderThemeStudio.App.DynamicIsland;

/// <summary>短暂提醒而非通知历史：只接收启用后的消息，最多排队八条，指纹和正文只留在内存。</summary>
public sealed class IslandNotificationQueue
{
    private readonly Queue<IslandNotification> pending = new();
    private readonly HashSet<string> seen = new(StringComparer.Ordinal);
    private readonly Queue<string> seenOrder = new();
    private DateTimeOffset startedAt, until;
    public IslandNotification? Current { get; private set; }
    public int PendingCount => pending.Count;
    public IslandNotificationQueue(DateTimeOffset startedAt) => this.startedAt = startedAt;

    public void Clear(DateTimeOffset now)
    {
        pending.Clear(); seen.Clear(); seenOrder.Clear(); Current = null; startedAt = now;
    }
    public void Enqueue(IEnumerable<IslandNotification> messages, DateTimeOffset now, string blockedSources)
    {
        var blocked = blockedSources.Split([';', '\r', '\n'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        foreach (var message in messages.OrderBy(item => item.CreatedAt))
        {
            // 未提供时间的旧适配器仍可接入；原生/本地消息均提供明确时间与身份。
            if (message.CreatedAt != default && (message.CreatedAt < startedAt || message.CreatedAt < now.AddSeconds(-90) || message.CreatedAt > now.AddSeconds(5))) continue;
            if (blocked.Any(item => item.Equals(message.Source, StringComparison.OrdinalIgnoreCase) || item.Equals(message.SourceId, StringComparison.OrdinalIgnoreCase))) continue;
            var identity = $"{message.SourceId}\0{message.Source}\0{message.Id}\0{message.CreatedAt.UtcTicks}\0{message.Title}\0{message.Body}";
            var fingerprint = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(identity)));
            if (!seen.Add(fingerprint)) continue;
            seenOrder.Enqueue(fingerprint);
            if (seenOrder.Count > 512) seen.Remove(seenOrder.Dequeue());
            if (pending.Count == 8) pending.Dequeue();
            pending.Enqueue(message);
        }
        Advance(now);
    }
    public void Advance(DateTimeOffset now)
    {
        if (Current is not null && now < until) return;
        Current = null;
        while (pending.TryDequeue(out var next))
        {
            if (next.CreatedAt != default && next.CreatedAt < now.AddSeconds(-90)) continue;
            Current = next; until = now.AddSeconds(6); break;
        }
    }
}

/// <summary>批量读取避免两次 UI 刷新之间的突发消息被覆盖；兼容原有单条适配器。</summary>
public interface IIslandNotificationBatchSource
{
    Task<IReadOnlyList<IslandNotification>> ReadRecentAsync(CancellationToken token);
}
```

## src/FolderThemeStudio.App/DynamicIsland/IslandSettings.cs

```csharp
using System.IO;
using System.Text.Json;

namespace FolderThemeStudio.App.DynamicIsland;

/// <summary>灵动岛使用独立配置，避免和文件夹主题的设置互相覆盖。位置是 WPF 的逻辑像素。</summary>
public sealed record IslandSettings
{
    public bool Enabled { get; init; }
    public bool ShowTime { get; init; } = true;
    public bool ShowCpu { get; init; } = true;
    public bool ShowMemory { get; init; } = true;
    public bool ShowClipboard { get; init; }
    public bool ShowMusic { get; init; } = true;
    public bool ShowNotifications { get; init; }
    public double Opacity { get; init; } = .94;
    public int RefreshSeconds { get; init; } = 1;
    public double? Left { get; init; }
    public double? Top { get; init; }
    public string LyricsDirectory { get; init; } = string.Empty;
    public bool OnlineLyrics { get; init; }
    public bool LocalNotifications { get; init; }
    public bool DragMotion { get; init; } = true;
    public bool SnapToEdges { get; init; } = true;
    public string BlockedNotificationSources { get; init; } = string.Empty;
    public int NotificationPort { get; init; } = 47301;
    public string NotificationToken { get; init; } = Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(24));

    public IslandSettings Normalize() => this with
    {
        Opacity = double.IsFinite(Opacity) ? Math.Clamp(Opacity, .2, 1) : .94,
        RefreshSeconds = Math.Clamp(RefreshSeconds, 1, 60),
        Left = Left is { } x && double.IsFinite(x) ? x : null,
        Top = Top is { } y && double.IsFinite(y) ? y : null,
        LyricsDirectory = LyricsDirectory?.Trim() ?? string.Empty,
        BlockedNotificationSources = (BlockedNotificationSources ?? string.Empty).Trim()[..Math.Min((BlockedNotificationSources ?? string.Empty).Trim().Length, 2048)],
        NotificationPort = Math.Clamp(NotificationPort, 1024, 65535),
        NotificationToken = NotificationToken is { Length: >= 32 and <= 128 } && NotificationToken.All(char.IsAsciiLetterOrDigit)
            ? NotificationToken : Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(24)),
    };
}

public interface IIslandSettingsStore
{
    Task<IslandSettings> LoadAsync();
    Task SaveAsync(IslandSettings settings);
}

/// <summary>先写临时文件再原子替换；串行化保存，拖动和滚轮事件不会争抢同一个文件。</summary>
public sealed class IslandSettingsStore : IIslandSettingsStore
{
    private readonly string path;
    private readonly SemaphoreSlim gate = new(1, 1);
    private static readonly JsonSerializerOptions Json = new() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    public IslandSettingsStore(string? path = null) => this.path = path ?? Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FolderThemeStudio", "dynamic-island.json");

    public async Task<IslandSettings> LoadAsync()
    {
        await gate.WaitAsync().ConfigureAwait(false);
        try
        {
            if (!File.Exists(path)) return new();
            return (JsonSerializer.Deserialize<IslandSettings>(await File.ReadAllTextAsync(path).ConfigureAwait(false), Json) ?? new()).Normalize();
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or JsonException) { return new(); }
        finally { gate.Release(); }
    }

    public async Task SaveAsync(IslandSettings settings)
    {
        await gate.WaitAsync().ConfigureAwait(false);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
            await File.WriteAllTextAsync(path + ".tmp", JsonSerializer.Serialize(settings.Normalize(), Json)).ConfigureAwait(false);
            File.Move(path + ".tmp", path, overwrite: true);
        }
        finally { gate.Release(); }
    }
}
```

## src/FolderThemeStudio.App/DynamicIsland/IslandViewModel.cs

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FolderThemeStudio.App.DynamicIsland;

/// <summary>只负责显示数据；Win32/WinRT 都经接口注入，测试不需要更改用户的剪贴板或播放器。</summary>
public sealed class IslandViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly ISystemMetricsSource metrics;
    private readonly IIslandMediaSource media;
    private readonly IIslandLyricsSource lyrics;
    private readonly IIslandNotificationSource notifications;
    private readonly CancellationTokenSource stop = new();
    private IReadOnlyList<LyricLine> lyricLines = [];
    private string loadedSong = string.Empty;
    private bool refreshing;
    private bool disposed;
    private int settingsRevision;
    private IslandSettings settings;
    private SystemMetrics snapshot = new(null, null, 0, 0);
    private IslandMedia? currentSong;
    private bool hasNotification, hasClipboard;
    private int summarySlot;
    private readonly Func<DateTimeOffset> utcNow;
    private DateTimeOffset? commandFeedbackUntil;
    private readonly IslandNotificationQueue messageQueue;
    public IslandViewModel(IslandSettings settings, ISystemMetricsSource? metrics = null, IIslandMediaSource? media = null,
        IIslandLyricsSource? lyrics = null, IIslandNotificationSource? notifications = null, Func<DateTimeOffset>? utcNow = null)
    {
        this.settings = settings.Normalize();
        this.utcNow = utcNow ?? (() => DateTimeOffset.UtcNow);
        messageQueue = new(this.utcNow());
        this.metrics = metrics ?? new WindowsSystemMetricsSource();
        this.media = media ?? new WindowsMediaSource();
        this.lyrics = lyrics ?? new LyricsCoordinator(this.settings);
        this.notifications = notifications ?? new IslandNotificationHub(this.settings);
    }
    public IslandSettings Settings => settings;
    public bool ShowTime => settings.ShowTime;
    public bool ShowCpu => settings.ShowCpu;
    public bool ShowMemory => settings.ShowMemory;
    public bool ShowMetrics => ShowCpu || ShowMemory;
    public bool ShowClipboard => settings.ShowClipboard;
    public bool ShowMusic => settings.ShowMusic;
    public bool ShowNotifications => settings.ShowNotifications;
    public string TimeText { get; private set; } = DateTime.Now.ToString("HH:mm:ss");
    public string CpuText => snapshot.CpuPercent is { } value ? $"{value:0}%" : "—";
    public string MemoryText => snapshot.MemoryPercent is { } value ? $"{value:0}%" : "—";
    public double CpuValue => snapshot.CpuPercent ?? 0;
    public double MemoryValue => snapshot.MemoryPercent ?? 0;
    public string MemoryDetail => snapshot.TotalMemoryBytes > 0 ? $"{snapshot.UsedMemoryBytes / 1073741824d:0.#} / {snapshot.TotalMemoryBytes / 1073741824d:0.#} GB" : "正在采样";
    public string ClipboardText { get; private set; } = "开启后显示最新文本";
    public string MusicTitle { get; private set; } = "等待音乐播放";
    public string MusicArtist { get; private set; } = "播放器需启用系统媒体控制";
    public string LyricText { get; private set; } = "可在设置中选择 LRC 歌词文件夹";
    public string LyricTranslation { get; private set; } = "";
    public string NotificationTitle { get; private set; } = "系统通知";
    public string NotificationBody { get; private set; } = "需要 MSIX 身份与通知读取授权";
    public string NotificationSource { get; private set; } = "";
    public string NotificationBadge => !hasNotification ? "●" : System.Globalization.StringInfo.GetNextTextElement(
        string.IsNullOrWhiteSpace(NotificationSource) ? "通" : NotificationSource);
    public string NotificationQueueDetail => messageQueue.PendingCount > 0 ? $"另有 {messageQueue.PendingCount} 条待显示" : "";
    public string StatusText { get; private set; } = string.Empty;
    /// <summary>迷你胶囊只显示一行；换行详情供主动悬停查看，不影响窗口尺寸。</summary>
    public string CompactDetails
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(StatusText)) parts.Add(StatusText);
            if (ShowNotifications) parts.Add($"{NotificationSource}：{NotificationTitle} {NotificationBody} {NotificationQueueDetail}");
            if (ShowMusic)
            {
                parts.Add($"音乐：{MusicTitle} · {MusicArtist}");
                if (currentSong is { } song) parts.Add($"{song.Album} · {song.SourceApp} · {song.Position:mm\\:ss}/{song.Duration:mm\\:ss} · {(song.Playing ? "播放中" : "已暂停")}");
                if (lyricLines.Count > 0 && !string.IsNullOrWhiteSpace(LyricText)) parts.Add(LyricText);
                if (!string.IsNullOrWhiteSpace(LyricTranslation)) parts.Add(LyricTranslation);
                if (lyrics is LyricsCoordinator coordinator) parts.Add(coordinator.Status);
            }
            if (ShowClipboard) parts.Add($"剪贴板：{ClipboardText}");
            return parts.Count == 0 ? "右键打开设置" : string.Join(Environment.NewLine, parts.Select(ClipboardPreview.Create));
        }
    }
    public string CompactText
    {
        get
        {
            if (StatusText.Length > 0) return ClipboardPreview.Create(StatusText);
            if (ShowNotifications && hasNotification) return ClipboardPreview.Create($"{NotificationSource} · {NotificationTitle} {NotificationBody}");
            var items = new List<string>();
            if (ShowMusic) items.Add("♫ " + (lyricLines.Count > 0 && LyricText.Length > 0 ? LyricText : MusicTitle));
            if (ShowClipboard && hasClipboard) items.Add("复制 · " + ClipboardText);
            if (items.Count == 0) return ShowNotifications ? ClipboardPreview.Create(NotificationBody) : "右键打开设置";
            return ClipboardPreview.Create(items[summarySlot % items.Count]);
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    public void ApplySettings(IslandSettings next)
    {
        var clearMessages = settings.ShowNotifications != next.ShowNotifications || settings.BlockedNotificationSources != next.BlockedNotificationSources;
        settings = next.Normalize();
        if (clearMessages) { messageQueue.Clear(utcNow()); hasNotification = false; }
        settingsRevision++;
        loadedSong = string.Empty;
        lyricLines = [];
        LyricTranslation = "";
        if (lyrics is IIslandSettingsAware configurableLyrics) configurableLyrics.ApplySettings(settings);
        if (notifications is IIslandSettingsAware configurableNotifications) configurableNotifications.ApplySettings(settings);
        if (!ShowClipboard) ClipboardText = string.Empty;
        if (!ShowNotifications) { notifications.Dispose(); NotificationTitle = "系统通知"; NotificationBody = "通知模块已关闭"; }
        UpdateNotificationPresentation();
        foreach (var name in new[] { nameof(ShowTime), nameof(ShowCpu), nameof(ShowMemory), nameof(ShowMetrics), nameof(ShowClipboard), nameof(ShowMusic), nameof(ShowNotifications), nameof(ClipboardText) }) Notify(name);
        Notify(nameof(NotificationTitle)); Notify(nameof(NotificationBody));
    }
    public void TickClock()
    {
        TimeText = DateTime.Now.ToString("HH:mm:ss");
        summarySlot = (int)(utcNow().ToUnixTimeSeconds() / 5 % int.MaxValue);
        if (commandFeedbackUntil is { } until && utcNow() >= until) ReportStatus("");
        messageQueue.Advance(utcNow());
        if (ShowNotifications) UpdateNotificationPresentation();
        Notify(nameof(TimeText)); Notify(nameof(CompactText));
    }
    public void SetClipboard(string text) { hasClipboard = ShowClipboard && !string.IsNullOrWhiteSpace(text); ClipboardText = !ShowClipboard ? string.Empty : string.IsNullOrEmpty(text) ? "剪贴板没有文本" : ClipboardPreview.Create(text); Notify(nameof(ClipboardText)); }
    public void ReportStatus(string text) { commandFeedbackUntil = null; StatusText = text; Notify(nameof(StatusText)); }
    public Task<string> RequestNotificationAccessAsync() => notifications.RequestAccessAsync();

    public async Task RefreshAsync()
    {
        if (disposed || refreshing) return;
        refreshing = true;
        var revision = settingsRevision;
        try
        {
            if (ShowMetrics)
            {
                var next = await Task.Run(metrics.Read, stop.Token);
                if (disposed || revision != settingsRevision) return;
                snapshot = next;
                foreach (var name in new[] { nameof(CpuText), nameof(MemoryText), nameof(CpuValue), nameof(MemoryValue), nameof(MemoryDetail) }) Notify(name);
            }
            if (ShowMusic)
            {
                var song = await media.ReadAsync(stop.Token);
                if (disposed || revision != settingsRevision) return;
                currentSong = song;
                if (song is null && lyrics is LyricsCoordinator idleLyrics) idleLyrics.CancelPending();
                MusicTitle = string.IsNullOrWhiteSpace(song?.Title) ? "等待音乐播放" : song.Title;
                MusicArtist = song is null ? "播放器需启用系统媒体控制" : song.Artist;
                var key = $"{song?.Title}\0{song?.Artist}\0{song?.Duration}\0{song?.SourceApp}\0{settings.LyricsDirectory}";
                var lyricReadFailed = false;
                if (loadedSong != key || lyricLines.Count == 0)
                {
                    try
                    {
                        lyricLines = song is null ? [] : lyrics is IContextualLyricsSource contextual
                            ? await contextual.ReadAsync(song, settings.LyricsDirectory, stop.Token)
                            : await lyrics.ReadAsync(song.Title, song.Artist, settings.LyricsDirectory, stop.Token);
                        loadedSong = key;
                    }
                    catch (Exception e) when (e is System.IO.IOException or UnauthorizedAccessException)
                    {
                        // 歌词文件被占用/失去权限时只降级歌词，继续发布歌曲元数据和刷新通知。
                        // 不缓存失败的 key，文件恢复可读后下一次采样自动重试。
                        lyricLines = [];
                        lyricReadFailed = true;
                    }
                }
                if (disposed || revision != settingsRevision) return;
                var line = lyricReadFailed ? "歌词读取暂不可用，请检查文件权限" : song is null ? "可在设置中选择 LRC 歌词文件夹" : lyricLines.Count == 0
                    ? lyrics is LyricsCoordinator coordinator ? coordinator.Status : "未找到匹配的 LRC 歌词" : LrcLyricsSource.CurrentLine(lyricLines, song.Position);
                LyricTranslation = song is null ? "" : lyricLines.LastOrDefault(item => item.At <= song.Position)?.Translation ?? "";
                if (line != LyricText) { LyricText = line; Notify(nameof(LyricText)); }
                Notify(nameof(MusicTitle)); Notify(nameof(MusicArtist));
            }
            if (ShowNotifications)
            {
                IReadOnlyList<IslandNotification> messages;
                if (notifications is IIslandNotificationBatchSource batch) messages = await batch.ReadRecentAsync(stop.Token);
                else { var latest = await notifications.ReadLatestAsync(stop.Token); messages = latest is null ? [] : [latest]; }
                if (disposed || revision != settingsRevision) return;
                messageQueue.Enqueue(messages, utcNow(), settings.BlockedNotificationSources);
                UpdateNotificationPresentation();
            }
        }
        catch (OperationCanceledException) when (disposed) { }
        catch (Exception e) when (e is System.IO.IOException or UnauthorizedAccessException or System.Runtime.InteropServices.COMException or InvalidOperationException)
        { if (!disposed) ReportStatus("部分信息暂不可用，请检查播放器或歌词目录"); }
        finally { refreshing = false; if (disposed) stop.Dispose(); }
    }
    public async Task ControlMediaAsync(IslandMediaCommand command)
    {
        if (disposed || !ShowMusic) return;
        try
        {
            var accepted = media is IIslandMediaControls controls && await controls.ExecuteAsync(command, stop.Token);
            if (disposed) return;
            ReportStatus(accepted ? "" : "播放器当前不支持此控制");
            if (!accepted) commandFeedbackUntil = utcNow().AddSeconds(4);
            await RefreshAsync();
        }
        catch (OperationCanceledException) when (disposed) { }
    }
    private void UpdateNotificationPresentation()
    {
        var message = ShowNotifications ? messageQueue.Current : null;
        hasNotification = message is not null;
        NotificationTitle = message?.Title ?? "系统通知";
        NotificationBody = message?.Body ?? (ShowNotifications ? notifications.Status : "通知模块已关闭");
        NotificationSource = message?.Source ?? "";
        Notify(nameof(NotificationTitle)); Notify(nameof(NotificationBody));
        Notify(nameof(NotificationSource)); Notify(nameof(NotificationBadge)); Notify(nameof(NotificationQueueDetail));
    }
    private void Notify([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new(name));
        if (name is nameof(ShowMusic) or nameof(ShowClipboard) or nameof(ShowNotifications) or nameof(MusicTitle)
            or nameof(MusicArtist) or nameof(LyricText) or nameof(ClipboardText) or nameof(NotificationTitle)
            or nameof(NotificationBody) or nameof(StatusText))
        {
            PropertyChanged?.Invoke(this, new(nameof(CompactText)));
            PropertyChanged?.Invoke(this, new(nameof(CompactDetails)));
        }
    }
    public void Dispose() { if (disposed) return; disposed = true; stop.Cancel(); messageQueue.Clear(utcNow()); notifications.Dispose(); (lyrics as IDisposable)?.Dispose(); if (!refreshing) stop.Dispose(); }
}
```

## src/FolderThemeStudio.App/DynamicIsland/LocalNotificationBridge.cs

```csharp
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FolderThemeStudio.App.DynamicIsland;

/// <summary>
/// 可选的本地消息入口，绑定 IPv4 Loopback，不开放局域网、不需要 URL ACL 或管理员。
/// 只接收带 Bearer 令牌的短 JSON POST；拒绝浏览器 Origin，限长、限时、串行处理。
/// 消息仅保存在内存，不能替代未经授权的 Windows 全局通知监听。
/// </summary>
public sealed class LocalNotificationBridge : IDisposable
{
    private readonly object sync = new();
    private readonly CancellationTokenSource stop = new();
    private readonly TcpListener listener;
    private readonly string token;
    private IslandNotification? latest;
    private readonly Queue<IslandNotification> pending = new();
    public string Status { get; private set; } = "本地消息接口未启动";
    public int Port => ((IPEndPoint)listener.LocalEndpoint).Port;
    public LocalNotificationBridge(int port, string token) { listener = new(IPAddress.Loopback, port); this.token = token; }
    public bool Start()
    {
        try { listener.Start(8); Status = $"本地消息接口已开启（127.0.0.1:{Port}）"; _ = Task.Run(ListenAsync); return true; }
        catch (SocketException) { Status = "本地消息端口不可用，请更换端口"; return false; }
    }
    public IslandNotification? ReadLatest() { lock (sync) return latest is { } message && message.CreatedAt > DateTimeOffset.UtcNow.AddSeconds(-90) ? message : null; }
    public IReadOnlyList<IslandNotification> ReadRecent()
    {
        // 保留有界快照供 UI 设置变更后的重试；成功显示去重由展示队列负责。
        lock (sync) return pending.Where(message => message.CreatedAt >= DateTimeOffset.UtcNow.AddSeconds(-90)).ToArray();
    }
    private async Task ListenAsync()
    {
        while (!stop.IsCancellationRequested)
        {
            try
            {
                using var client = await listener.AcceptTcpClientAsync(stop.Token).ConfigureAwait(false);
                using var timeout = CancellationTokenSource.CreateLinkedTokenSource(stop.Token);
                timeout.CancelAfter(TimeSpan.FromSeconds(3));
                var stream = client.GetStream();
                await HandleAsync(stream, timeout.Token).ConfigureAwait(false);
                // 拒绝请求时可能尚有正文。先半关闭发送，再有界排空，避免 Windows 以 RST 覆盖已写出的 HTTP 状态。
                client.Client.Shutdown(SocketShutdown.Send);
                var discard = new byte[1024];
                for (var drained = 0; drained < 16384;)
                {
                    var count = await stream.ReadAsync(discard, timeout.Token).ConfigureAwait(false);
                    if (count == 0) break;
                    drained += count;
                }
            }
            catch (Exception e) when (e is OperationCanceledException or SocketException or IOException or ObjectDisposedException) { }
        }
    }
    private async Task HandleAsync(NetworkStream stream, CancellationToken cancellation)
    {
        var header = new List<byte>();
        var one = new byte[1];
        while (header.Count < 8192)
        {
            if (await stream.ReadAsync(one, cancellation).ConfigureAwait(false) == 0) return;
            header.Add(one[0]);
            var n = header.Count;
            if (n >= 4 && header[n - 4] == 13 && header[n - 3] == 10 && header[n - 2] == 13 && header[n - 1] == 10) break;
        }
        if (header.Count >= 8192) { await ReplyAsync(stream, 413, cancellation); return; }
        var lines = Encoding.ASCII.GetString(header.ToArray()).Split("\r\n");
        var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in lines.Skip(1).Where(line => line.Length > 0))
        {
            var colon = line.IndexOf(':');
            if (colon <= 0 || !fields.TryAdd(line[..colon], line[(colon + 1)..].Trim())) { await ReplyAsync(stream, 400, cancellation); return; }
        }
        if (!lines[0].StartsWith("POST /api/notifications HTTP/1.", StringComparison.Ordinal)) { await ReplyAsync(stream, 404, cancellation); return; }
        if (fields.ContainsKey("Origin") || !fields.TryGetValue("Host", out var host) || (host != $"127.0.0.1:{Port}" && host != $"localhost:{Port}")) { await ReplyAsync(stream, 403, cancellation); return; }
        if (!fields.TryGetValue("Authorization", out var auth) || !FixedEquals(auth, "Bearer " + token)) { await ReplyAsync(stream, 401, cancellation); return; }
        if (fields.ContainsKey("Transfer-Encoding") || !fields.TryGetValue("Content-Length", out var length) || !int.TryParse(length, out var size) || size is <= 0 or > 16384) { await ReplyAsync(stream, 413, cancellation); return; }
        var body = new byte[size];
        await stream.ReadExactlyAsync(body, cancellation).ConfigureAwait(false);
        try
        {
            using var json = JsonDocument.Parse(body);
            var root = json.RootElement;
            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("title", out var title) || title.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(title.GetString())) { await ReplyAsync(stream, 400, cancellation); return; }
            var text = root.TryGetProperty("body", out var content) && content.ValueKind == JsonValueKind.String ? content.GetString() ?? "" : "";
            var source = root.TryGetProperty("source", out var origin) && origin.ValueKind == JsonValueKind.String
                ? ClipboardPreview.Create(origin.GetString() ?? "") : "本地消息";
            if (string.IsNullOrWhiteSpace(source)) source = "本地消息";
            lock (sync)
            {
                latest = new(ClipboardPreview.Create(title.GetString()!), ClipboardPreview.Create(text))
                    { CreatedAt = DateTimeOffset.UtcNow, Source = source, SourceId = "local:" + source };
                if (pending.Count == 8) pending.Dequeue();
                pending.Enqueue(latest);
            }
            await ReplyAsync(stream, 200, cancellation);
        }
        catch (JsonException) { await ReplyAsync(stream, 400, cancellation); }
    }
    private static bool FixedEquals(string left, string right) => CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(left), Encoding.UTF8.GetBytes(right));
    private static Task ReplyAsync(NetworkStream stream, int code, CancellationToken token)
    {
        var body = code == 200 ? "{\"ok\":true}" : "{\"ok\":false}";
        var bytes = Encoding.ASCII.GetBytes($"HTTP/1.1 {code} Result\r\nContent-Type: application/json\r\nContent-Length: {body.Length}\r\nConnection: close\r\n\r\n{body}");
        return stream.WriteAsync(bytes, token).AsTask();
    }
    public void Dispose() { stop.Cancel(); listener.Stop(); lock (sync) { latest = null; pending.Clear(); } }
}
```

## src/FolderThemeStudio.App/DynamicIsland/LrcLyricsSource.cs

```csharp
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace FolderThemeStudio.App.DynamicIsland;

public sealed record LyricLine(TimeSpan At, string Text, string Translation = "");
public interface IIslandLyricsSource { Task<IReadOnlyList<LyricLine>> ReadAsync(string title, string artist, string directory, CancellationToken token); }

/// <summary>歌曲名.lrc 或 歌手 - 歌曲名.lrc。系统媒体 API 不提供歌词，此接口也可替换为合法的本地 HTTP 数据源。</summary>
public sealed class LrcLyricsSource : IIslandLyricsSource
{
    private static readonly Regex Timestamp = new(@"\[(\d+):(\d{2})(?:[.:](\d{1,3}))?\]", RegexOptions.CultureInvariant);
    private static readonly Regex Offset = new(@"\[offset:([+-]?\d+)\]", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    public async Task<IReadOnlyList<LyricLine>> ReadAsync(string title, string artist, string directory, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(directory)) return [];
        var names = new[] { SafeName($"{artist} - {title}") + ".lrc", SafeName(title) + ".lrc" };
        foreach (var name in names)
        {
            var path = Path.Combine(directory, name);
            if (!File.Exists(path)) continue;
            if (new FileInfo(path).Length > 2 * 1024 * 1024) return []; // 不读取任意大小的文件
            return Parse(await File.ReadAllTextAsync(path, token));
        }
        return [];
    }
    private static string SafeName(string name) => string.Concat(name.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c)).Trim();

    public static IReadOnlyList<LyricLine> Parse(string text)
    {
        var result = new List<LyricLine>();
        var offsetMatch = Offset.Match(text);
        var offset = offsetMatch.Success && int.TryParse(offsetMatch.Groups[1].Value, out var value) ? value : 0;
        foreach (var raw in text.Split('\n'))
        {
            var matches = Timestamp.Matches(raw);
            if (matches.Count == 0) continue;
            var words = Timestamp.Replace(raw, string.Empty).Trim();
            if (words.Length == 0) continue;
            foreach (Match match in matches)
            {
                if (!int.TryParse(match.Groups[1].Value, out var minutes) || !int.TryParse(match.Groups[2].Value, out var seconds) || seconds >= 60) continue;
                var fraction = match.Groups[3].Value;
                var ms = fraction.Length == 0 ? 0 : int.Parse(fraction.PadRight(3, '0'), CultureInfo.InvariantCulture);
                var at = TimeSpan.FromMilliseconds(Math.Max(0, minutes * 60000d + seconds * 1000d + ms + offset));
                result.Add(new(at, words));
            }
        }
        return result.OrderBy(line => line.At).ToArray();
    }
    public static string CurrentLine(IReadOnlyList<LyricLine> lines, TimeSpan position) =>
        lines.LastOrDefault(line => line.At <= position)?.Text ?? string.Empty;
}
```

## src/FolderThemeStudio.App/DynamicIsland/LyricsCoordinator.cs

```csharp
namespace FolderThemeStudio.App.DynamicIsland;

public interface IIslandSettingsAware { void ApplySettings(IslandSettings settings); }
public interface IContextualLyricsSource { Task<IReadOnlyList<LyricLine>> ReadAsync(IslandMedia song, string directory, CancellationToken token); }

/// <summary>本地优先；网络任务在后台独立执行，刷新调用仅查看缓存，不等网络。</summary>
public sealed class LyricsCoordinator : IIslandLyricsSource, IContextualLyricsSource, IIslandSettingsAware, IDisposable
{
    private readonly object sync = new();
    private readonly OnlineLyricsSource online;
    private readonly LrcLyricsSource local = new();
    private readonly Dictionary<string, (LyricsResult Result, DateTimeOffset Expires)> cache = new();
    private IslandSettings settings;
    private CancellationTokenSource? pending;
    private string pendingKey = "";
    private bool disposed;
    private int revision;
    public string Status { get; private set; } = "未找到歌词";
    public LyricsCoordinator(IslandSettings settings, OnlineLyricsSource? online = null) { this.settings = settings; this.online = online ?? new(); }
    public Task<IReadOnlyList<LyricLine>> ReadAsync(string title, string artist, string directory, CancellationToken token) => ReadAsync(new(title, artist, TimeSpan.Zero, false), directory, token);
    public async Task<IReadOnlyList<LyricLine>> ReadAsync(IslandMedia song, string directory, CancellationToken token)
    {
        var key = $"{song.Title}\0{song.Artist}\0{song.Duration.TotalSeconds:0}";
        int readRevision;
        lock (sync)
        {
            if (disposed) return [];
            readRevision = revision;
            if (pendingKey != key) CancelPending();
        }
        var localLines = await local.ReadAsync(song.Title, song.Artist, directory, token);
        if (localLines.Count > 0) { Status = "本地 LRC"; return localLines; }
        lock (sync)
        {
            if (disposed) return [];
            if (readRevision != revision) return [];
            if (!settings.OnlineLyrics) { Status = "在设置中开启在线歌词，或选择 LRC 文件夹"; return []; }
            if (song.Status.Length > 0 || string.IsNullOrWhiteSpace(song.Title) || string.IsNullOrWhiteSpace(song.Artist)) { Status = "该媒体没有完整歌名和歌手"; return []; }
            if (cache.TryGetValue(key, out var entry) && entry.Expires > DateTimeOffset.UtcNow) { Status = entry.Result.Provider; return entry.Result.Lines; }
            if (pendingKey == key) { Status = "正在获取在线歌词…"; return []; }
            pending?.Cancel();
            var request = CancellationTokenSource.CreateLinkedTokenSource(token);
            pending = request; pendingKey = key; Status = "正在获取在线歌词…";
            _ = Task.Run(() => FetchAsync(key, song, request));
            return [];
        }
    }
    private async Task FetchAsync(string key, IslandMedia song, CancellationTokenSource request)
    {
        try
        {
            var result = await online.FetchAsync(song, request.Token).ConfigureAwait(false);
            lock (sync)
            {
                if (disposed || request.IsCancellationRequested || pending != request) return;
                if (cache.Count >= 24) cache.Remove(cache.Keys.First());
                cache[key] = (result, DateTimeOffset.UtcNow.Add(result.Lines.Count > 0 ? TimeSpan.FromHours(12) : TimeSpan.FromMinutes(3)));
                Status = result.Provider;
            }
        }
        catch (OperationCanceledException) { }
        finally { lock (sync) { if (pending == request) { pending = null; pendingKey = ""; } } request.Dispose(); }
    }
    public void CancelPending() { lock (sync) { pending?.Cancel(); pending = null; pendingKey = ""; } }
    public void ApplySettings(IslandSettings next) { lock (sync) { revision++; settings = next; CancelPending(); } }
    public void Dispose() { lock (sync) { disposed = true; pending?.Cancel(); cache.Clear(); } }
}
```

## src/FolderThemeStudio.App/DynamicIsland/OnlineLyricsSource.cs

```csharp
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FolderThemeStudio.App.DynamicIsland;

public sealed record LyricsResult(IReadOnlyList<LyricLine> Lines, string Provider);

/// <summary>
/// 根据 NotchPeninsula 的公开接口路线独立实现，使用系统 TLS 校验。
/// 每一来源独立超时，响应限制 2 MiB；只有匹配歌名/歌手的曲目才取词。
/// 不执行响应内容、不下载封面或音频，也不伪造请求者 IP。
/// </summary>
public sealed class OnlineLyricsSource
{
    private static readonly HttpClient Shared = new() { Timeout = Timeout.InfiniteTimeSpan };
    private readonly HttpClient http;
    public OnlineLyricsSource(HttpClient? http = null) => this.http = http ?? Shared;

    public async Task<LyricsResult> FetchAsync(IslandMedia song, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(song.Title) || string.IsNullOrWhiteSpace(song.Artist)) return new([], "缺少歌名或歌手");
        var neteaseFirst = song.SourceApp.Contains("netease", StringComparison.OrdinalIgnoreCase) || song.SourceApp.Contains("cloudmusic", StringComparison.OrdinalIgnoreCase);
        var providers = neteaseFirst ? new[] { "netease", "tencent", "official", "lrclib" } : new[] { "tencent", "netease", "official", "lrclib" };
        foreach (var provider in providers)
        {
            token.ThrowIfCancellationRequested();
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(token);
            timeout.CancelAfter(TimeSpan.FromSeconds(3));
            try
            {
                var result = provider switch
                {
                    "lrclib" => await LrcLibAsync(song, timeout.Token),
                    "official" => await NeteaseAsync(song, timeout.Token),
                    _ => await LuoYueAsync(song, provider, timeout.Token),
                };
                if (result.Lines.Count > 0) return result;
            }
            catch (OperationCanceledException) when (!token.IsCancellationRequested) { }
            catch (Exception e) when (e is HttpRequestException or System.IO.IOException or JsonException or InvalidOperationException or FormatException) { }
        }
        return new([], "未找到同步歌词或服务暂不可用");
    }

    private async Task<LyricsResult> LuoYueAsync(IslandMedia song, string platform, CancellationToken token)
    {
        var query = Uri.EscapeDataString($"{song.Title} {song.Artist}");
        var path = platform == "tencent" ? "tencent/search/song" : "netease";
        using var search = await GetAsync($"https://api.vkeys.cn/v2/music/{path}?word={query}", token);
        var list = Property(search.RootElement, "data");
        if (list.ValueKind != JsonValueKind.Array) return new([], "");
        foreach (var candidate in list.EnumerateArray().Take(60))
        {
            if (!Matches(song, Text(candidate, "song"), Text(candidate, "singer"))) continue;
            // 回退来源也要检验已知时长，不能让同名演唱会/剪辑版本先命中。
            var interval = Property(candidate, "interval");
            var duration = Property(candidate, "duration");
            var candidateSeconds = interval.TryNumber(out var seconds) ? seconds : duration.TryNumber(out var durationValue) ? durationValue : 0;
            if (candidateSeconds > 0 && song.Duration > TimeSpan.Zero && Math.Abs(candidateSeconds - song.Duration.TotalSeconds) > 4) continue;
            var id = Property(candidate, "id");
            var identifier = id.ValueKind == JsonValueKind.Number ? id.GetRawText() : id.ValueKind == JsonValueKind.String ? id.GetString() : null;
            if (string.IsNullOrEmpty(identifier)) continue;
            using var lyric = await GetAsync($"https://api.vkeys.cn/v2/music/{platform}/lyric?id={Uri.EscapeDataString(identifier)}", token);
            var data = Property(lyric.RootElement, "data");
            var lrc = Text(data, "lrc");
            if (lrc.Length == 0) lrc = Text(data, "rc");
            var lines = Parse(lrc, Text(data, "trans"));
            if (lines.Count > 0) return new(lines, platform == "tencent" ? "落月 / QQ" : "落月 / 网易云");
        }
        return new([], "");
    }

    private async Task<LyricsResult> NeteaseAsync(IslandMedia song, CancellationToken token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://music.163.com/api/search/get/web")
        { Content = new FormUrlEncodedContent(new Dictionary<string, string> { ["s"] = $"{song.Title} {song.Artist}", ["type"] = "1", ["limit"] = "20", ["offset"] = "0" }) };
        request.Headers.Referrer = new("https://music.163.com/");
        using var search = await JsonAsync(request, token);
        var candidates = Property(Property(search.RootElement, "result"), "songs");
        if (candidates.ValueKind != JsonValueKind.Array) return new([], "");
        foreach (var candidate in candidates.EnumerateArray().Take(20))
        {
            var artists = Property(candidate, "artists");
            var artist = artists.ValueKind == JsonValueKind.Array ? string.Join("/", artists.EnumerateArray().Select(item => Text(item, "name"))) : "";
            if (!Matches(song, Text(candidate, "name"), artist)) continue;
            var duration = Property(candidate, "duration");
            if (duration.TryNumber(out var ms) && song.Duration > TimeSpan.Zero && Math.Abs(ms / 1000 - song.Duration.TotalSeconds) > 4) continue;
            var id = Property(candidate, "id");
            if (id.ValueKind != JsonValueKind.Number || !id.TryGetInt64(out var songId)) continue;
            using var lyric = await GetAsync($"https://music.163.com/api/song/lyric?id={songId}&lv=-1&tv=-1", token);
            var lines = Parse(Text(Property(lyric.RootElement, "lrc"), "lyric"), Text(Property(lyric.RootElement, "tlyric"), "lyric"));
            if (lines.Count > 0) return new(lines, "网易云");
        }
        return new([], "");
    }

    private async Task<LyricsResult> LrcLibAsync(IslandMedia song, CancellationToken token)
    {
        var query = $"https://lrclib.net/api/get?track_name={Uri.EscapeDataString(song.Title)}&artist_name={Uri.EscapeDataString(song.Artist)}";
        if (song.Duration > TimeSpan.Zero) query += "&duration=" + song.Duration.TotalSeconds.ToString("0", CultureInfo.InvariantCulture);
        using var data = await GetAsync(query, token);
        if (!Matches(song, Text(data.RootElement, "trackName"), Text(data.RootElement, "artistName"))) return new([], "");
        var duration = Property(data.RootElement, "duration");
        if (duration.TryNumber(out var seconds) && song.Duration > TimeSpan.Zero && Math.Abs(seconds - song.Duration.TotalSeconds) > 4) return new([], "");
        return new(Parse(Text(data.RootElement, "syncedLyrics"), ""), "LRCLIB");
    }

    private Task<JsonDocument> GetAsync(string uri, CancellationToken token) => JsonAsync(new(HttpMethod.Get, uri), token, ownsRequest: true);
    private async Task<JsonDocument> JsonAsync(HttpRequestMessage request, CancellationToken token, bool ownsRequest = false)
    {
        try
        {
            request.Headers.UserAgent.ParseAdd("FolderThemeStudio/0.1 (Windows; lyrics)");
            using var response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            if (response.Content.Headers.ContentLength > 2 * 1024 * 1024) throw new HttpRequestException("Lyrics response too large");
            await using var stream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
            using var memory = new System.IO.MemoryStream();
            var buffer = new byte[8192];
            int count;
            while ((count = await stream.ReadAsync(buffer, token).ConfigureAwait(false)) > 0)
            {
                if (memory.Length + count > 2 * 1024 * 1024) throw new HttpRequestException("Lyrics response too large");
                memory.Write(buffer, 0, count);
            }
            return JsonDocument.Parse(memory.ToArray());
        }
        finally { if (ownsRequest) request.Dispose(); }
    }
    private static IReadOnlyList<LyricLine> Parse(string lrc, string translation)
    {
        var lines = LrcLyricsSource.Parse(WebUtility.HtmlDecode(lrc));
        var translated = LrcLyricsSource.Parse(WebUtility.HtmlDecode(translation)).GroupBy(line => line.At).ToDictionary(group => group.Key, group => group.First().Text);
        return lines.Select(line => translated.TryGetValue(line.At, out var text) ? line with { Translation = text } : line).ToArray();
    }
    private static bool Matches(IslandMedia song, string title, string artist)
    {
        static string Normalize(string value) => Regex.Replace(value.Normalize(NormalizationForm.FormKC), @"[\s\p{P}\p{S}]", "").ToUpperInvariant();
        if (Normalize(title).Length == 0 || Normalize(title) != Normalize(song.Title)) return false;
        var candidates = Regex.Split(artist, @"[/,&、;]").Select(Normalize).Where(value => value.Length > 0).ToArray();
        var requested = Regex.Split(song.Artist, @"[/,&、;]").Select(Normalize).Where(value => value.Length > 0).ToArray();
        return requested.Length > 0 && requested.All(candidates.Contains);
    }
    private static JsonElement Property(JsonElement value, string name) => value.ValueKind == JsonValueKind.Object && value.TryGetProperty(name, out var property) ? property : default;
    private static string Text(JsonElement value, string name) => Property(value, name) is { ValueKind: JsonValueKind.String } property ? property.GetString() ?? "" : "";
}

internal static class JsonNumber
{
    internal static bool TryNumber(this JsonElement value, out double number) { number = 0; return value.ValueKind == JsonValueKind.Number && value.TryGetDouble(out number) && double.IsFinite(number); }
}
```

## src/FolderThemeStudio.App/DynamicIsland/SystemMetricsSource.cs

```csharp
using System.Runtime.InteropServices;

namespace FolderThemeStudio.App.DynamicIsland;

public readonly record struct CpuTimes(ulong Idle, ulong Kernel, ulong User);
public sealed record SystemMetrics(double? CpuPercent, double? MemoryPercent, ulong UsedMemoryBytes, ulong TotalMemoryBytes);
public interface ISystemMetricsSource { SystemMetrics Read(); }

/// <summary>GetSystemTimes 的 Kernel 已包含 Idle；CPU 用两个采样间的增量计算，而不是开机累计值。</summary>
public static class CpuUsageCalculator
{
    public static double? Calculate(CpuTimes previous, CpuTimes current)
    {
        if (current.Idle < previous.Idle || current.Kernel < previous.Kernel || current.User < previous.User) return null;
        var idle = current.Idle - previous.Idle;
        var total = (double)(current.Kernel - previous.Kernel) + (current.User - previous.User);
        return total <= 0 || idle > total ? null : Math.Clamp((total - idle) / total * 100, 0, 100);
    }
}

/// <summary>使用 Win32 获取全机 CPU/物理内存，不依赖性能计数器或管理员权限。</summary>
public sealed class WindowsSystemMetricsSource : ISystemMetricsSource
{
    private CpuTimes? previous;
    public SystemMetrics Read()
    {
        double? cpu = null, memory = null;
        if (GetSystemTimes(out var idle, out var kernel, out var user))
        {
            var next = new CpuTimes(idle.Value, kernel.Value, user.Value);
            if (previous is { } last) cpu = CpuUsageCalculator.Calculate(last, next);
            previous = next;
        }
        var status = new MemoryStatus { Length = (uint)Marshal.SizeOf<MemoryStatus>() };
        ulong used = 0, total = 0;
        if (GlobalMemoryStatusEx(ref status) && status.TotalPhysical > 0)
        {
            total = status.TotalPhysical;
            used = total - Math.Min(total, status.AvailablePhysical);
            memory = used / (double)total * 100;
        }
        return new(cpu, memory, used, total);
    }
    [StructLayout(LayoutKind.Sequential)] private struct FileTime
    {
        internal uint Low, High;
        internal readonly ulong Value => ((ulong)High << 32) | Low;
    }
    [StructLayout(LayoutKind.Sequential)] private struct MemoryStatus
    {
        internal uint Length, Load;
        internal ulong TotalPhysical, AvailablePhysical, TotalPageFile, AvailablePageFile, TotalVirtual, AvailableVirtual, AvailableExtendedVirtual;
    }
    [DllImport("kernel32.dll")] [return: MarshalAs(UnmanagedType.Bool)] private static extern bool GetSystemTimes(out FileTime idle, out FileTime kernel, out FileTime user);
    [DllImport("kernel32.dll", SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] private static extern bool GlobalMemoryStatusEx(ref MemoryStatus status);
}
```

## src/FolderThemeStudio.App/DynamicIsland/WindowsMediaSource.cs

```csharp
using Windows.Media.Control;

namespace FolderThemeStudio.App.DynamicIsland;

public sealed record IslandMedia(string Title, string Artist, TimeSpan Position, bool Playing, string Status = "")
{
    public TimeSpan Duration { get; init; }
    public string Album { get; init; } = "";
    public string SourceApp { get; init; } = "";
}
public interface IIslandMediaSource { Task<IslandMedia?> ReadAsync(CancellationToken token); }
public enum IslandMediaCommand { TogglePlayPause, Previous, Next }
public interface IIslandMediaControls { Task<bool> ExecuteAsync(IslandMediaCommand command, CancellationToken token); }

/// <summary>直接读取 SMTC，无需安装 Now Playing 或猜测 HTTP 端口；播放器需向 Windows 发布媒体会话。</summary>
public sealed class WindowsMediaSource : IIslandMediaSource, IIslandMediaControls
{
    private GlobalSystemMediaTransportControlsSessionManager? manager;
    private DateTimeOffset retryAfter;
    public async Task<IslandMedia?> ReadAsync(CancellationToken token)
    {
        if (DateTimeOffset.UtcNow < retryAfter) return null;
        try
        {
            manager ??= await GlobalSystemMediaTransportControlsSessionManager.RequestAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(2), token);
            var session = SelectSession(manager);
            if (session is null) return null;
            var properties = await session.TryGetMediaPropertiesAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(2), token);
            var timeline = session.GetTimelineProperties();
            var playback = session.GetPlaybackInfo();
            var playing = playback.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
            var position = timeline.Position;
            // Position 是 LastUpdatedTime 时刻的进度；播放中需要补上经过的时间，暂停时不能补。
            if (playing && timeline.LastUpdatedTime.Year >= 2000)
            {
                var elapsed = DateTimeOffset.UtcNow - timeline.LastUpdatedTime;
                var rate = playback.PlaybackRate ?? 1;
                if (elapsed > TimeSpan.Zero && elapsed < TimeSpan.FromDays(1) && double.IsFinite(rate) && rate is > 0 and <= 4)
                    position += TimeSpan.FromSeconds(elapsed.TotalSeconds * rate);
            }
            if (timeline.EndTime > TimeSpan.Zero && position > timeline.EndTime) position = timeline.EndTime;
            if (position < TimeSpan.Zero) position = TimeSpan.Zero;
            return new(properties.Title, properties.Artist, position, playing)
            { Duration = timeline.EndTime > timeline.StartTime ? timeline.EndTime - timeline.StartTime : TimeSpan.Zero,
              Album = properties.AlbumTitle ?? "", SourceApp = session.SourceAppUserModelId ?? "" };
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested) { throw; }
        catch (Exception e) when (e is System.Runtime.InteropServices.COMException or UnauthorizedAccessException or TimeoutException or InvalidOperationException)
        {
            manager = null;
            retryAfter = DateTimeOffset.UtcNow.AddSeconds(10);
            return new("媒体会话暂不可用", "请确认播放器已启用系统媒体控制", TimeSpan.Zero, false, e.GetType().Name);
        }
    }
    private static GlobalSystemMediaTransportControlsSession? SelectSession(GlobalSystemMediaTransportControlsSessionManager manager)
    {
        var current = manager.GetCurrentSession();
        if (current?.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing) return current;
        return manager.GetSessions().FirstOrDefault(session => session.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing) ?? current;
    }
    public async Task<bool> ExecuteAsync(IslandMediaCommand command, CancellationToken token)
    {
        try
        {
            manager ??= await GlobalSystemMediaTransportControlsSessionManager.RequestAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(2), token);
            var session = SelectSession(manager);
            if (session is null) return false;
            var action = command switch
            {
                IslandMediaCommand.Previous => session.TrySkipPreviousAsync(),
                IslandMediaCommand.Next => session.TrySkipNextAsync(),
                _ => session.TryTogglePlayPauseAsync(),
            };
            return await action.AsTask().WaitAsync(TimeSpan.FromSeconds(2), token);
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested) { throw; }
        catch (Exception e) when (e is System.Runtime.InteropServices.COMException or UnauthorizedAccessException or TimeoutException or InvalidOperationException) { return false; }
    }
}
```

## src/FolderThemeStudio.App/DynamicIsland/WindowsNotificationSource.cs

```csharp
using System.Runtime.InteropServices;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;

namespace FolderThemeStudio.App.DynamicIsland;

public sealed record IslandNotification(string Title, string Body)
{
    public DateTimeOffset CreatedAt { get; init; }
    public string Source { get; init; } = "Windows";
    public string SourceId { get; init; } = "";
    public string Id { get; init; } = "";
}
public interface IIslandNotificationSource : IDisposable
{
    string Status { get; }
    Task<string> RequestAccessAsync();
    Task<IslandNotification?> ReadLatestAsync(CancellationToken token);
}

/// <summary>完整 WinRT 适配器。MSIX 需声明 userNotificationListener，必须由用户在 UI 线程请求授权。</summary>
public sealed class WindowsNotificationSource : IIslandNotificationSource, IIslandNotificationBatchSource
{
    private readonly bool packaged = HasPackageIdentity();
    private UserNotificationListener? listener;
    private IReadOnlyList<IslandNotification> recent = [];
    private int dirty = 1;
    public string Status { get; private set; } = "通知读取需要 MSIX 身份与用户授权";
    public async Task<string> RequestAccessAsync()
    {
        try
        {
            var access = await UserNotificationListener.Current.RequestAccessAsync();
            return Status = access == UserNotificationListenerAccessStatus.Allowed ? "已获通知读取权限" : "未获权限，请在 Windows 设置中允许通知访问";
        }
        catch (Exception e) when (e is COMException or UnauthorizedAccessException or InvalidOperationException or NotSupportedException or NotImplementedException)
        { return Status = packaged ? "无法获得通知权限，请检查 MSIX 能力声明" : "当前 Windows 不允许 EXE 读取通知，可使用本地消息接口或 MSIX 版本"; }
    }
    public async Task<IslandNotification?> ReadLatestAsync(CancellationToken token)
        => (await ReadRecentAsync(token)).LastOrDefault();
    public async Task<IReadOnlyList<IslandNotification>> ReadRecentAsync(CancellationToken token)
    {
        try
        {
            var current = UserNotificationListener.Current;
            if (current.GetAccessStatus() != UserNotificationListenerAccessStatus.Allowed) { Status = "通知未授权，请点击设置中的读取权限按钮"; recent = []; Interlocked.Exchange(ref dirty, 1); return recent; }
            if (listener is null) { listener = current; listener.NotificationChanged += NotificationChanged; }
            if (Interlocked.Exchange(ref dirty, 0) == 0) return recent;
            var notifications = await listener.GetNotificationsAsync(NotificationKinds.Toast).AsTask().WaitAsync(TimeSpan.FromSeconds(2), token);
            var messages = new List<IslandNotification>();
            foreach (var item in notifications.OrderByDescending(item => item.CreationTime).Take(32).Reverse())
            {
                var texts = item.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric)?.GetTextElements();
                if (texts is null || texts.Count == 0) continue;
                var source = "Windows"; var sourceId = "";
                try { source = item.AppInfo.DisplayInfo.DisplayName; sourceId = item.AppInfo.AppUserModelId; }
                // 某些旧通知的 AppInfo 会映射 E_NOTIMPL 为 .NET NotImplementedException。
                // 这只影响来源名称，不应丢弃正文或让计时器的 async void 回调崩溃。
                catch (Exception e) when (e is COMException or UnauthorizedAccessException or InvalidOperationException or NotSupportedException or NotImplementedException) { }
                messages.Add(new(ClipboardPreview.Create(texts[0].Text), ClipboardPreview.Create(string.Join(" ", texts.Skip(1).Select(text => text.Text))))
                    { CreatedAt = item.CreationTime, Source = source, SourceId = sourceId, Id = item.Id.ToString() });
            }
            Status = "已连接系统通知，等待新消息";
            return recent = messages;
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested) { throw; }
        catch (Exception e) when (e is COMException or UnauthorizedAccessException or InvalidOperationException or NotSupportedException or NotImplementedException or TimeoutException)
        { Interlocked.Exchange(ref dirty, 1); Status = packaged ? "通知读取暂不可用，请检查授权" : "系统通知暂不可用，可使用本地消息接口；MSIX 版本需授权"; return []; }
    }
    // WinRT 回调可能来自其他线程，只更新标记。WPF 的刷新任务在 UI 线程更新绑定。
    private void NotificationChanged(UserNotificationListener sender, UserNotificationChangedEventArgs args) => Interlocked.Exchange(ref dirty, 1);
    public void Dispose() { if (listener is not null) listener.NotificationChanged -= NotificationChanged; listener = null; recent = []; Interlocked.Exchange(ref dirty, 1); }
    private static bool HasPackageIdentity() { uint length = 0; return GetCurrentPackageFullName(ref length, nint.Zero) == 122; }
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)] private static extern int GetCurrentPackageFullName(ref uint length, nint name);
}
```

## src/FolderThemeStudio.App/FolderCategoriesWindow.xaml

```xml
<controls:RoundedWindow x:Class="FolderThemeStudio.App.FolderCategoriesWindow" Style="{StaticResource {x:Type Window}}"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:controls="clr-namespace:FolderThemeStudio.App.Controls"
        xmlns:settings="clr-namespace:FolderThemeStudio.App.Settings"
        Title="{DynamicResource FolderCategories.Window.Title}"
        Width="960" Height="680" MinWidth="740" MinHeight="560"
        WindowStartupLocation="CenterOwner" ShowInTaskbar="False"
        Icon="Assets/FolderThemeStudio.png" UseLayoutRounding="True">
    <Grid Margin="22">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <StackPanel Grid.Row="0" Margin="0,0,0,16">
            <TextBlock FontSize="25" FontWeight="SemiBold" Text="{DynamicResource FolderCategories.Window.Title}" />
            <TextBlock Margin="0,4,0,0" Foreground="{StaticResource MutedTextBrush}"
                       Text="{DynamicResource FolderCategories.Window.Subtitle}" />
        </StackPanel>

        <Grid Grid.Row="1" IsEnabled="{Binding CanEdit}">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="0.9*" />
                <ColumnDefinition Width="16" />
                <ColumnDefinition Width="1.1*" />
            </Grid.ColumnDefinitions>

            <Border Grid.Column="0" Style="{StaticResource CardStyle}" Padding="16">
                <DockPanel>
                    <StackPanel DockPanel.Dock="Top">
                        <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource FolderCategories.List.Title}" />
                        <WrapPanel>
                            <Button Command="{Binding PickFolderCommand}" Content="{DynamicResource FolderCategories.Action.AddFolder}" />
                            <Button Command="{Binding NewEntryCommand}" Content="{DynamicResource FolderCategories.Action.New}" />
                        </WrapPanel>
                    </StackPanel>
                    <TextBlock DockPanel.Dock="Bottom" Margin="2,8,2,0" Foreground="{StaticResource MutedTextBrush}"
                               TextWrapping="Wrap" Text="{DynamicResource FolderCategories.List.EmptyHint}"
                               Visibility="{Binding IsEmpty, Converter={StaticResource BoolToVisibility}}" />
                    <ListBox ItemsSource="{Binding Entries}" SelectedItem="{Binding SelectedEntry, Mode=TwoWay}"
                             BorderThickness="0" Background="Transparent">
                        <ListBox.ItemTemplate>
                            <DataTemplate>
                                <Border Margin="0,0,0,8" Padding="12" Background="#F7F9FC"
                                        BorderBrush="{StaticResource BorderBrush}" BorderThickness="1" CornerRadius="11">
                                    <StackPanel>
                                        <TextBlock FontWeight="SemiBold" Text="{Binding Category}" TextTrimming="CharacterEllipsis" />
                                        <TextBlock Margin="0,4,0,0" FontSize="11" Foreground="{StaticResource MutedTextBrush}"
                                                   Text="{Binding FolderPath}" TextTrimming="CharacterEllipsis" />
                                        <TextBlock Margin="0,4,0,0" FontSize="12" Foreground="{StaticResource MutedTextBrush}"
                                                   Text="{Binding Note}" TextTrimming="CharacterEllipsis" />
                                    </StackPanel>
                                </Border>
                            </DataTemplate>
                        </ListBox.ItemTemplate>
                    </ListBox>
                </DockPanel>
            </Border>

            <Border Grid.Column="2" Style="{StaticResource CardStyle}" Padding="20">
                <ScrollViewer VerticalScrollBarVisibility="Auto" HorizontalScrollBarVisibility="Disabled">
                    <StackPanel>
                        <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource FolderCategories.Form.Title}" />
                        <TextBlock Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap"
                                   Text="{Binding FolderPathDisplay}" />
                        <Button Margin="0,10,0,0" HorizontalAlignment="Left" Command="{Binding PickFolderCommand}"
                                Content="{DynamicResource FolderCategories.Action.ChooseFolder}" />

                        <Label Style="{StaticResource FieldLabelStyle}" Content="{DynamicResource FolderCategories.Form.Category}" />
                        <TextBox Text="{Binding Category, UpdateSourceTrigger=PropertyChanged}" MaxLength="32" />

                        <Label Style="{StaticResource FieldLabelStyle}" Content="{DynamicResource FolderCategories.Form.Note}" />
                        <TextBox Height="112" Padding="10" AcceptsReturn="True" TextWrapping="Wrap" VerticalScrollBarVisibility="Auto"
                                 VerticalContentAlignment="Top" MaxLength="140"
                                 Text="{Binding Note, UpdateSourceTrigger=PropertyChanged}" />

                        <TextBlock Margin="0,14,0,6" FontWeight="SemiBold" Text="{DynamicResource FolderCategories.Form.Color}" />
                        <Border Height="32" CornerRadius="8" Background="{Binding AccentPreview}"
                                BorderBrush="{StaticResource BorderBrush}" BorderThickness="1" />
                        <ListBox Margin="0,10,0,6" ItemsSource="{x:Static settings:AppleColors.All}" BorderThickness="0" Background="Transparent"
                                 SelectedValuePath="Hex" SelectedValue="{Binding ColorHex, Mode=TwoWay}"
                                 ItemContainerStyle="{StaticResource ColorSwatchItemStyle}" ItemTemplate="{StaticResource ColorSwatchTemplate}">
                            <ListBox.ItemsPanel><ItemsPanelTemplate><WrapPanel/></ItemsPanelTemplate></ListBox.ItemsPanel>
                        </ListBox>
                        <Expander Header="{DynamicResource Studio.Advanced}"><StackPanel>
                        <TextBlock Margin="0,12,0,6" Text="{DynamicResource FolderCategories.Form.GradientStart}" />
                        <controls:VisualColorPicker DataContext="{Binding GradientStartPicker}" />
                        <TextBlock Margin="0,16,0,6" Text="{DynamicResource FolderCategories.Form.GradientEnd}" />
                        <controls:VisualColorPicker DataContext="{Binding GradientEndPicker}" />
                        </StackPanel></Expander>

                        <WrapPanel Margin="0,18,0,0">
                            <Button Command="{Binding SaveCommand}" Style="{StaticResource PrimaryButtonStyle}"
                                    Content="{DynamicResource FolderCategories.Action.Save}" />
                            <Button Command="{Binding RemoveCommand}" Content="{DynamicResource FolderCategories.Action.Remove}" />
                        </WrapPanel>
                        <TextBlock Margin="0,8,0,0" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap"
                                   Text="{Binding StatusText}" />
                    </StackPanel>
                </ScrollViewer>
            </Border>
        </Grid>

        <Border Grid.Row="2" Margin="0,14,0,0" Padding="14" CornerRadius="12"
                Background="#EAF1FB" BorderBrush="#D7E4F5" BorderThickness="1">
            <DockPanel>
                <Button DockPanel.Dock="Right" Content="{DynamicResource FolderCategories.Action.Close}" IsCancel="True" Click="Close_Click" />
                <TextBlock VerticalAlignment="Center" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap"
                           Text="{DynamicResource FolderCategories.Help}" />
            </DockPanel>
        </Border>
    </Grid>
</controls:RoundedWindow>
```

## src/FolderThemeStudio.App/FolderCategoriesWindow.xaml.cs

```csharp
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
```

## src/FolderThemeStudio.App/FolderCategoryPopupWindow.xaml

```xml
<Window x:Class="FolderThemeStudio.App.FolderCategoryPopupWindow" Style="{StaticResource {x:Type Window}}"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Folder category"
        Width="350"
        SizeToContent="Height"
        MinHeight="96"
        MaxWidth="360"
        WindowStyle="None"
        ResizeMode="NoResize"
        AllowsTransparency="True"
        Background="Transparent"
        ShowInTaskbar="False"
        ShowActivated="False"
        Topmost="True"
        Focusable="False"
        AllowDrop="True"
        WindowStartupLocation="Manual"
        UseLayoutRounding="True">
    <Border x:Name="CardPanel" Margin="8" Padding="16" Background="#FDFDFE" BorderBrush="#E5E5EA" BorderThickness="1"
            AllowDrop="True" DragEnter="Card_DragEnter" DragOver="Card_DragOver"
            CornerRadius="16">
        <Border.Effect>
            <DropShadowEffect BlurRadius="22" ShadowDepth="4" Opacity="0.12" Color="#1D1D1F" />
        </Border.Effect>
        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="5" />
                <ColumnDefinition Width="12" />
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>
            <Border x:Name="CategoryAccent" Grid.RowSpan="2" CornerRadius="3" Background="#2868D7" />
            <StackPanel Grid.Column="2">
                <TextBlock x:Name="CategoryTitle" FontSize="15" FontWeight="SemiBold" Foreground="#172033"
                           TextTrimming="CharacterEllipsis" />
                <TextBlock x:Name="CategoryNote" Margin="0,5,0,0" Foreground="#58647A" TextWrapping="Wrap"
                           MaxWidth="290" />
                <TextBlock x:Name="FolderInfoText" Margin="0,10,0,0" Foreground="#6E6E73" FontSize="11"
                           LineHeight="18" TextWrapping="Wrap" Visibility="Collapsed" />
                <Separator Margin="0,12,0,10" Background="#E6EAF0" />
                <TextBlock FontSize="12" FontWeight="SemiBold" Foreground="#58647A"
                           Text="{DynamicResource LiveFolder.Recent}" />
                <ItemsControl x:Name="RecentFilesList" Margin="0,6,0,0">
                    <ItemsControl.ItemTemplate>
                        <DataTemplate>
                            <TextBlock Margin="0,3,0,3" Foreground="#172033" Text="{Binding Name}"
                                       TextTrimming="CharacterEllipsis" ToolTip="{Binding Path}" />
                        </DataTemplate>
                    </ItemsControl.ItemTemplate>
                </ItemsControl>
                <TextBlock x:Name="EmptyHint" Margin="0,6,0,0" Foreground="#8A96A8"
                           Text="{DynamicResource LiveFolder.Empty}" />
                <TextBlock x:Name="DropHint" Margin="0,12,0,0" Foreground="#2868D7" FontSize="12"
                           Text="{DynamicResource LiveFolder.DropHint}" TextWrapping="Wrap" />
                <StackPanel x:Name="DropTargetsPanel" Margin="0,8,0,0" Visibility="Collapsed" />
                <TextBlock x:Name="DropStatus" Margin="0,7,0,0" Foreground="#58647A" TextWrapping="Wrap" />
            </StackPanel>
        </Grid>
    </Border>
</Window>
```

## src/FolderThemeStudio.App/FolderCategoryPopupWindow.xaml.cs

```csharp
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using FolderThemeStudio.App.Services;
using Brush = System.Windows.Media.Brush;
using Point = System.Windows.Point;
using ColorConverter = System.Windows.Media.ColorConverter;
using DragEventArgs = System.Windows.DragEventArgs;
using DragDropEffects = System.Windows.DragDropEffects;
using DataFormats = System.Windows.DataFormats;

namespace FolderThemeStudio.App;

public partial class FolderCategoryPopupWindow : Window, IFolderHoverCardPresenter
{
    private const uint SwpNoSize = 0x0001;
    private const uint SwpNoMove = 0x0002;
    private const uint SwpNoZOrder = 0x0004;
    private const uint SwpNoActivate = 0x0010;
    private const uint SwpFrameChanged = 0x0020;
    private const uint SwpShowWindow = 0x0040;
    private const uint MonitorDefaultToNearest = 2;
    private static readonly nint HwndTopmostHandle = new(-1);
    private readonly FolderLiveContentService liveContent = new();
    private readonly FolderDropService dropService = new();
    private ExplorerInfoTipSuppressor? infoTips;
    private nint handle;
    private int showRevision;
    private volatile bool shown;
    private bool dropInteractionEnabled;
    private string? currentFolderPath;
    private Rect lastItemBounds;

    public FolderCategoryPopupWindow()
    {
        InitializeComponent();
        SourceInitialized += (_, _) => ApplyOverlayStyle();
        Closed += (_, _) => infoTips?.Dispose();
    }

    public void Show(FolderCategoryEntry entry, Rect itemBounds)
    {
        currentFolderPath = entry.FolderPath;
        lastItemBounds = itemBounds;
        var revision = ++showRevision;
        CategoryTitle.Text = entry.Category;
        CategoryNote.Text = entry.Note;
        CategoryNote.Visibility = string.IsNullOrWhiteSpace(entry.Note) ? Visibility.Collapsed : Visibility.Visible;
        CategoryAccent.Background = new LinearGradientBrush(
            (System.Windows.Media.Color)ColorConverter.ConvertFromString(entry.GradientStartHex),
            (System.Windows.Media.Color)ColorConverter.ConvertFromString(entry.GradientEndHex),
            new Point(0, 0), new Point(0, 1));

        RecentFilesList.ItemsSource = null;
        FolderInfoText.Text = string.Empty;
        FolderInfoText.Visibility = Visibility.Collapsed;
        EmptyHint.Visibility = Visibility.Collapsed;
        DropTargetsPanel.Visibility = Visibility.Collapsed;
        DropStatus.Text = string.Empty;
        if (!IsVisible) base.Show();
        shown = true;
        UpdateLayout();
        PositionNear(itemBounds);
        _ = LoadContentAsync(entry.FolderPath, revision);
    }

    public new void Hide()
    {
        infoTips?.SetTarget(null);
        shown = false;
        showRevision++;
        if (IsVisible) base.Hide();
        SetDropInteractionEnabled(false);
    }

    public bool IsPointerOverCard(Point screenPoint)
    {
        if (!shown || handle == nint.Zero || !NativeMethods.GetWindowRect(handle, out var bounds)) return false;
        return screenPoint.X >= bounds.Left && screenPoint.X < bounds.Right &&
               screenPoint.Y >= bounds.Top && screenPoint.Y < bounds.Bottom;
    }

    public void SetCardOpacity(double opacity) => CardPanel.Opacity = Math.Clamp(opacity, 0, 1);

    public void SetInfoTipTarget(ExplorerHoverTarget? target)
    {
        if (target is not null && target.RootWindowHandle != nint.Zero) infoTips ??= new ExplorerInfoTipSuppressor();
        infoTips?.SetTarget(target);
    }

    public void SetDropInteractionEnabled(bool enabled)
    {
        if (dropInteractionEnabled == enabled) return;
        dropInteractionEnabled = enabled;
        if (handle != nint.Zero) ApplyOverlayStyle();
    }

    private void ApplyOverlayStyle()
    {
        handle = new WindowInteropHelper(this).Handle;
        var style = NativeMethods.GetWindowLongPtr(handle, NativeMethods.GwlExStyle).ToInt64();
        style |= NativeMethods.WsExNoActivate | NativeMethods.WsExToolWindow;
        if (dropInteractionEnabled) style &= ~NativeMethods.WsExTransparent;
        else style |= NativeMethods.WsExTransparent;
        NativeMethods.SetWindowLongPtr(handle, NativeMethods.GwlExStyle, new nint(style));
        NativeMethods.SetWindowPos(handle, nint.Zero, 0, 0, 0, 0,
            SwpNoSize | SwpNoMove | SwpNoZOrder | SwpNoActivate | SwpFrameChanged);
    }

    private async Task LoadContentAsync(string path, int revision)
    {
        FolderLiveContent content;
        try { content = await Task.Run(() => liveContent.Read(path)).ConfigureAwait(false); }
        catch (Exception exception) when (exception is ArgumentException or System.IO.IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            content = new FolderLiveContent([], [new FolderDropTarget("", path)]);
        }
        if (Dispatcher.HasShutdownStarted) return;
        try { await Dispatcher.InvokeAsync(() =>
        {
            if (!shown || revision != showRevision || !string.Equals(currentFolderPath, path, StringComparison.OrdinalIgnoreCase)) return;
            RecentFilesList.ItemsSource = content.RecentFiles;
            if (content.Info is { } info)
            {
                var culture = System.Globalization.CultureInfo.CurrentUICulture;
                var format = Convert.ToString(TryFindResource("LiveFolder.InfoFormat")) ?? "Created: {0}\nModified: {1}\nContents: {2} files · {3} folders\nSize: {4}";
                var sizeText = FolderInfoSummary.FormatSize(info.SizeBytes);
                if (info.SizePartial) sizeText += Convert.ToString(TryFindResource("LiveFolder.InfoPartial")) ?? " (partial)";
                var prefix = info.ContentsPartial ? "≥ " : string.Empty;
                FolderInfoText.Text = string.Format(culture, format,
                    info.CreatedUtc.ToLocalTime().ToString("g", culture), info.ModifiedUtc.ToLocalTime().ToString("g", culture),
                    prefix + info.FileCount, prefix + info.FolderCount, sizeText);
                FolderInfoText.Visibility = Visibility.Visible;
            }
            EmptyHint.Visibility = content.RecentFiles.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            DropTargetsPanel.Children.Clear();
            foreach (var target in content.DropTargets)
            {
                var label = string.Equals(target.Path, path, StringComparison.OrdinalIgnoreCase)
                    ? Convert.ToString(TryFindResource("LiveFolder.ThisFolder")) ?? target.Name
                    : target.Name;
                var row = new Border
                {
                    Tag = target.Path,
                    AllowDrop = true,
                    Margin = new Thickness(0, 3, 0, 3),
                    Padding = new Thickness(8, 6, 8, 6),
                    CornerRadius = new CornerRadius(6),
                    Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(238, 244, 253)),
                    Child = new TextBlock { Text = label, Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(23, 32, 51)) }
                };
                row.DragOver += DropTarget_DragOver;
                row.Drop += DropTarget_Drop;
                DropTargetsPanel.Children.Add(row);
            }
            UpdateLayout();
            PositionNear(lastItemBounds);
        }); }
        catch (TaskCanceledException) { }
    }

    private void Card_DragEnter(object sender, DragEventArgs e)
    {
        if (GetDraggedFiles(e).Length == 0) return;
        DropTargetsPanel.Visibility = Visibility.Visible;
        UpdateLayout();
        PositionNear(lastItemBounds);
        e.Effects = DragDropEffects.None;
        e.Handled = true;
    }

    private void Card_DragOver(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.None;
        e.Handled = true;
    }

    private void DropTarget_DragOver(object sender, DragEventArgs e)
    {
        e.Effects = GetDraggedFiles(e).Length == 0 ? DragDropEffects.None : DragDropEffects.Move;
        e.Handled = true;
    }

    private async void DropTarget_Drop(object sender, DragEventArgs e)
    {
        e.Handled = true;
        var paths = GetDraggedFiles(e);
        if (paths.Length == 0 || sender is not Border { Tag: string destination } || currentFolderPath is null) return;
        var root = currentFolderPath;
        var revision = showRevision;
        DropTargetsPanel.Visibility = Visibility.Collapsed;
        var result = await Task.Run(() => dropService.Move(paths, root, destination));
        if (!shown || revision != showRevision) return;
        var format = Convert.ToString(TryFindResource("LiveFolder.MoveResult")) ?? "Moved {0} file(s); {1} failed";
        DropStatus.Text = string.Format(System.Globalization.CultureInfo.CurrentCulture, format, result.MovedCount, result.Errors.Count);
        DropStatus.ToolTip = result.Errors.Count == 0 ? null : string.Join(Environment.NewLine, result.Errors);
        await LoadContentAsync(root, revision);
    }

    private static string[] GetDraggedFiles(DragEventArgs e) =>
        e.Data.GetDataPresent(DataFormats.FileDrop) && e.Data.GetData(DataFormats.FileDrop) is string[] paths
            ? paths
            : [];

    private void PositionNear(Rect bounds)
    {
        if (handle == nint.Zero) handle = new WindowInteropHelper(this).Handle;
        if (!NativeMethods.GetWindowRect(handle, out var windowRect)) return;
        var monitor = NativeMethods.MonitorFromPoint(new NativeMethods.NativePoint((int)bounds.Left, (int)bounds.Top), MonitorDefaultToNearest);
        var info = new NativeMethods.MonitorInfo { Size = Marshal.SizeOf<NativeMethods.MonitorInfo>() };
        if (monitor == nint.Zero || !NativeMethods.GetMonitorInfo(monitor, ref info)) return;

        var width = windowRect.Right - windowRect.Left;
        var height = windowRect.Bottom - windowRect.Top;
        var x = (int)bounds.Right + 18;
        var y = (int)bounds.Top;
        if (x + width > info.Work.Right) x = (int)bounds.Left - width - 18;
        if (y + height > info.Work.Bottom) y = info.Work.Bottom - height;
        x = Math.Clamp(x, info.Work.Left, Math.Max(info.Work.Left, info.Work.Right - width));
        y = Math.Clamp(y, info.Work.Top, Math.Max(info.Work.Top, info.Work.Bottom - height));
        NativeMethods.SetWindowPos(handle, HwndTopmostHandle, x, y, 0, 0, SwpNoSize | SwpNoActivate | SwpShowWindow);
    }

    private static class NativeMethods
    {
        internal const int GwlExStyle = -20;
        internal const long WsExTransparent = 0x00000020L;
        internal const long WsExToolWindow = 0x00000080L;
        internal const long WsExNoActivate = 0x08000000L;

        [StructLayout(LayoutKind.Sequential)]
        internal struct NativePoint
        {
            internal int X;
            internal int Y;
            internal NativePoint(int x, int y) { X = x; Y = y; }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NativeRect { internal int Left; internal int Top; internal int Right; internal int Bottom; }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct MonitorInfo
        {
            internal int Size;
            internal NativeRect Monitor;
            internal NativeRect Work;
            internal uint Flags;
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
        internal static extern nint GetWindowLongPtr(nint hwnd, int index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
        internal static extern nint SetWindowLongPtr(nint hwnd, int index, nint value);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(nint hwnd, out NativeRect rect);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern nint MonitorFromPoint(NativePoint point, uint flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo(nint monitor, ref MonitorInfo info);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(nint hwnd, nint insertAfter, int x, int y, int width, int height, uint flags);
    }
}
```

## src/FolderThemeStudio.App/FolderQuickNoteWindow.xaml

```xml
<controls:RoundedWindow x:Class="FolderThemeStudio.App.FolderQuickNoteWindow" Style="{StaticResource {x:Type Window}}"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:controls="clr-namespace:FolderThemeStudio.App.Controls"
    xmlns:settings="clr-namespace:FolderThemeStudio.App.Settings"
    Title="{DynamicResource QuickNote.Title}" Width="420" SizeToContent="Height"
    ResizeMode="NoResize" ShowInTaskbar="False" WindowStartupLocation="Manual"
    Background="{StaticResource CardBrush}" UseLayoutRounding="True"
    PreviewKeyDown="Window_PreviewKeyDown" Icon="Assets/FolderThemeStudio.png">
    <Grid Margin="24,18,24,20">
        <Grid.RowDefinitions><RowDefinition Height="Auto"/><RowDefinition Height="Auto"/><RowDefinition Height="Auto"/></Grid.RowDefinitions>
        <StackPanel>
            <TextBlock FontSize="19" FontWeight="SemiBold" Text="{Binding FolderName}" TextTrimming="CharacterEllipsis" />
            <TextBlock Margin="0,5,0,16" Foreground="{StaticResource MutedTextBrush}" FontSize="11"
                       Text="{Binding FolderPath}" ToolTip="{Binding FolderPath}" TextTrimming="CharacterEllipsis" />
        </StackPanel>
        <StackPanel Grid.Row="1" IsEnabled="{Binding CanEdit}">
            <TextBox x:Name="NoteBox" Height="104" AcceptsReturn="True" TextWrapping="Wrap"
                     VerticalContentAlignment="Top" VerticalScrollBarVisibility="Auto" MaxLength="140"
                     Text="{Binding Note, UpdateSourceTrigger=PropertyChanged}"
                     automation:AutomationProperties.Name="Folder note"
                     xmlns:automation="clr-namespace:System.Windows.Automation;assembly=PresentationCore"/>
            <Expander Margin="0,12,0,4" Header="{DynamicResource QuickNote.Details}">
                <StackPanel Margin="0,10,0,4">
                    <TextBox Text="{Binding Category, UpdateSourceTrigger=PropertyChanged}" MaxLength="32" />
                    <ListBox Margin="0,10,0,0" ItemsSource="{x:Static settings:AppleColors.All}" BorderThickness="0" Background="Transparent"
                             SelectedValuePath="Hex" SelectedValue="{Binding ColorHex, Mode=TwoWay}"
                             ItemContainerStyle="{StaticResource ColorSwatchItemStyle}" ItemTemplate="{StaticResource ColorSwatchTemplate}">
                        <ListBox.ItemsPanel><ItemsPanelTemplate><WrapPanel/></ItemsPanelTemplate></ListBox.ItemsPanel>
                    </ListBox>
                </StackPanel>
            </Expander>
        </StackPanel>
        <StackPanel Grid.Row="2" Margin="0,12,0,0">
            <TextBlock x:Name="ErrorText" Visibility="Collapsed" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Text="{Binding StatusText}" Margin="0,0,0,8"/>
            <Grid>
                <TextBlock VerticalAlignment="Center" FontSize="11" Foreground="{StaticResource MutedTextBrush}" Text="{DynamicResource QuickNote.Shortcut}"/>
                <Button x:Name="SaveNoteButton" HorizontalAlignment="Right" Margin="0" MinWidth="84" Style="{StaticResource PrimaryButtonStyle}"
                        IsEnabled="{Binding CanSave}" Content="{DynamicResource QuickNote.Save}" Click="Save_Click"/>
            </Grid>
        </StackPanel>
    </Grid>
</controls:RoundedWindow>
```

## src/FolderThemeStudio.App/FolderQuickNoteWindow.xaml.cs

```csharp
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using FolderThemeStudio.App.Controls;
using FolderThemeStudio.App.ViewModels;

namespace FolderThemeStudio.App;

public partial class FolderQuickNoteWindow : RoundedWindow
{
    private readonly FolderCategoriesViewModel viewModel;

    public FolderQuickNoteWindow(FolderCategoriesViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        DataContext = viewModel;
        Loaded += (_, _) => { PositionNearPointer(); FocusNote(); };
        SizeChanged += (_, _) => { if (IsLoaded) ConstrainToWorkArea(); };
        Closed += (_, _) => viewModel.Dispose();
    }

    public void FocusNote()
    {
        NoteBox.Focus();
        NoteBox.CaretIndex = NoteBox.Text.Length;
    }

    private void PositionNearPointer()
    {
        var pointer = System.Windows.Forms.Cursor.Position;
        var work = System.Windows.Forms.Screen.FromPoint(pointer).WorkingArea;
        var transform = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformFromDevice
            ?? System.Windows.Media.Matrix.Identity;
        var anchor = transform.Transform(new System.Windows.Point(pointer.X + 16, pointer.Y + 18));
        var topLeft = transform.Transform(new System.Windows.Point(work.Left + 12, work.Top + 12));
        var bottomRight = transform.Transform(new System.Windows.Point(work.Right - 12, work.Bottom - 12));
        Left = Math.Clamp(anchor.X, topLeft.X, Math.Max(topLeft.X, bottomRight.X - ActualWidth));
        Top = Math.Clamp(anchor.Y, topLeft.Y, Math.Max(topLeft.Y, bottomRight.Y - ActualHeight));
    }

    private async void Save_Click(object sender, RoutedEventArgs e) => await SaveAndCloseAsync();

    private void ConstrainToWorkArea()
    {
        var handle = new WindowInteropHelper(this).Handle;
        var work = System.Windows.Forms.Screen.FromHandle(handle).WorkingArea;
        var transform = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformFromDevice
            ?? System.Windows.Media.Matrix.Identity;
        var topLeft = transform.Transform(new System.Windows.Point(work.Left + 12, work.Top + 12));
        var bottomRight = transform.Transform(new System.Windows.Point(work.Right - 12, work.Bottom - 12));
        Left = Math.Clamp(Left, topLeft.X, Math.Max(topLeft.X, bottomRight.X - ActualWidth));
        Top = Math.Clamp(Top, topLeft.Y, Math.Max(topLeft.Y, bottomRight.Y - ActualHeight));
    }

    private async Task SaveAndCloseAsync()
    {
        if (!viewModel.CanSave) return;
        if (await viewModel.SaveAsync()) Close();
        else ErrorText.Visibility = Visibility.Visible;
    }

    private async void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Escape) { e.Handled = true; Close(); }
        else if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
        {
            e.Handled = true;
            await SaveAndCloseAsync();
        }
    }
}
```

## src/FolderThemeStudio.App/FolderThemeStudio.App.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-windows10.0.19041.0</TargetFramework>
    <UseWPF>true</UseWPF>
    <UseWindowsForms>true</UseWindowsForms>
    <DisableImplicitNamespaceImports>true</DisableImplicitNamespaceImports>
    <OutputType>WinExe</OutputType>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <SelfContained>false</SelfContained>
    <PublishSingleFile>true</PublishSingleFile>
    <PublishReadyToRun>false</PublishReadyToRun>
    <PublishTrimmed>false</PublishTrimmed>
    <DebugType>embedded</DebugType>
    <ApplicationIcon>Assets\FolderThemeStudio.ico</ApplicationIcon>
    <Version>0.1.0-beta.18</Version>
    <AssemblyVersion>0.1.0.0</AssemblyVersion>
    <FileVersion>0.1.0.18</FileVersion>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\FolderThemeStudio.Core\FolderThemeStudio.Core.csproj" />
  </ItemGroup>
  <ItemGroup>
    <Resource Include="Assets\FolderThemeStudio.png" />
    <Resource Include="Assets\DefaultWallpaper.png" />
  </ItemGroup>
</Project>
```

## src/FolderThemeStudio.App/Localization/LocalizationService.cs

```csharp
using System.Globalization;
using System.Windows;

namespace FolderThemeStudio.App.Localization;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    event EventHandler? LanguageChanged;
    void ApplyLanguage(string language);
    string Get(string key, params object[] arguments);
}

public sealed class LocalizationService : ILocalizationService
{
    private static readonly IReadOnlyDictionary<string, string> English = new Dictionary<string, string>
    {
        ["App.Title"] = "Folder Theme Studio",
        ["Top.Settings"] = "Settings",
        ["FolderCategories.Form.NoFolder"] = "No folder selected",
        ["FolderCategories.Status.Ready"] = "Choose or create a folder category.",
        ["FolderCategories.Status.Selected"] = "Existing category selected.",
        ["FolderCategories.Status.New"] = "Folder selected. Enter a category name.",
        ["FolderCategories.Status.Saved"] = "Category saved and hover card updated.",
        ["FolderCategories.Status.Removed"] = "Category removed.",
        ["FolderCategories.Status.Error"] = "Could not read or save categories. Check permissions or the local configuration file.",
        ["AutoRules.Status.Ready"] = "Add or edit name-match rules for monitored folders.",
        ["AutoRules.Status.NoRoots"] = "No monitored roots yet. Enable monitoring when applying a style first.",
        ["AutoRules.Status.KeywordRequired"] = "Enter a keyword before saving this rule.",
        ["AutoRules.Status.ImageRequired"] = "Choose an image or switch to the current editor style.",
        ["AutoRules.Status.ImageSelected"] = "Image selected. Save the rule to use it for new folders.",
        ["AutoRules.Status.Saved"] = "Rule saved and monitoring updated.",
        ["AutoRules.Status.Deleted"] = "Rule deleted.",
        ["AutoRules.Status.Error"] = "The rule could not be saved. Check the selected folder and icon file.",
        ["Plan.Allowed"] = "Allowed: {0}",
        ["Settings.GradientStart"] = "Gradient start",
        ["Settings.GradientEnd"] = "Gradient end",
        ["Settings.Stroke"] = "Stroke",
        ["Settings.Glow"] = "Glow",
        ["Tutorial.Step1.Title"] = "Choose a style",
        ["Tutorial.Step1.Body"] = "Open Icon studio from Home. Choose an Apple-inspired color or import your own image. Fine-tune advanced settings later.",
        ["Tutorial.Step2.Title"] = "Choose folders",
        ["Tutorial.Step2.Body"] = "Add the folders you want to change. Enable automatic styling if new folders should inherit the same style.",
        ["Tutorial.Step3.Title"] = "Preview & apply",
        ["Tutorial.Step3.Body"] = "Preview the application plan, then apply it. A backup is created automatically. To undo, open Backup & restore from Home.",
        ["Tutorial.Step4.Title"] = "Apply safely",
        ["Tutorial.Step4.Body"] = "Calculate the plan, review it, then apply to ordinary folders.",
        ["Tutorial.Step5.Title"] = "Restore when needed",
        ["Tutorial.Step5.Body"] = "Restore the latest backup to undo the most recent application.",
    };

    private static readonly IReadOnlyDictionary<string, string> Chinese = new Dictionary<string, string>
    {
        ["App.Title"] = "文件夹主题工坊",
        ["Top.Settings"] = "设置",
        ["FolderCategories.Form.NoFolder"] = "尚未选择文件夹",
        ["FolderCategories.Status.Ready"] = "选择或新建一个文件夹分类。",
        ["FolderCategories.Status.Selected"] = "已选择现有分类。",
        ["FolderCategories.Status.New"] = "文件夹已选定，请填写分类名称。",
        ["FolderCategories.Status.Saved"] = "分类已保存，悬浮卡片已更新。",
        ["FolderCategories.Status.Removed"] = "分类已移除。",
        ["FolderCategories.Status.Error"] = "无法读取或保存分类，请检查权限或本地配置文件。",
        ["AutoRules.Status.Ready"] = "可为监控目录添加或编辑名称匹配规则。",
        ["AutoRules.Status.NoRoots"] = "还没有监控目录。请先在应用样式时启用新文件夹监控。",
        ["AutoRules.Status.KeywordRequired"] = "保存规则前请填写关键词。",
        ["AutoRules.Status.ImageRequired"] = "请选择图片，或切换为当前编辑样式。",
        ["AutoRules.Status.ImageSelected"] = "图片已选择；保存规则后即可用于新建文件夹。",
        ["AutoRules.Status.Saved"] = "规则已保存，监控已更新。",
        ["AutoRules.Status.Deleted"] = "规则已删除。",
        ["AutoRules.Status.Error"] = "规则保存失败，请检查监控目录和图标文件。",
        ["Plan.Allowed"] = "允许：{0}",
        ["Settings.GradientStart"] = "渐变起始色",
        ["Settings.GradientEnd"] = "渐变结束色",
        ["Settings.Stroke"] = "描边",
        ["Settings.Glow"] = "光晕",
        ["Tutorial.Step1.Title"] = "选择主题",
        ["Tutorial.Step1.Body"] = "从首页进入图标设计，选择苹果风格色块或导入自己的图片。高级参数可稍后再调整。",
        ["Tutorial.Step2.Title"] = "选择文件夹",
        ["Tutorial.Step2.Body"] = "点击添加文件夹，选择要更改的目录。需要时启用持续应用，让新文件夹继承样式。",
        ["Tutorial.Step3.Title"] = "预览并应用",
        ["Tutorial.Step3.Body"] = "先预览应用方案，再确认应用。软件会自动备份；需要撤销时，从首页进入备份与恢复。",
        ["Tutorial.Step4.Title"] = "安全应用",
        ["Tutorial.Step4.Body"] = "先计算并检查方案，再应用到普通文件夹。",
        ["Tutorial.Step5.Title"] = "需要时恢复",
        ["Tutorial.Step5.Body"] = "使用最近备份撤销上一次应用操作。",
    };

    private readonly bool applyToApplication;
    private IReadOnlyDictionary<string, string> text = Chinese;

    public LocalizationService(string language = "zh-CN", bool applyToApplication = true)
    {
        this.applyToApplication = applyToApplication;
        ApplyLanguage(language);
    }

    public string CurrentLanguage { get; private set; } = "zh-CN";
    public event EventHandler? LanguageChanged;

    public void ApplyLanguage(string language)
    {
        var selected = language is "en-US" ? "en-US" : "zh-CN";
        if (applyToApplication && System.Windows.Application.Current is not null)
        {
            var next = new ResourceDictionary
            {
                Source = new Uri($"/FolderThemeStudio.App;component/Localization/Strings.{selected}.xaml", UriKind.Relative),
            };
            var dictionaries = System.Windows.Application.Current.Resources.MergedDictionaries;
            var old = dictionaries.Where(item =>
                item.Source?.OriginalString.Contains("Localization/Strings.", StringComparison.Ordinal) == true).ToArray();
            foreach (var dictionary in old)
            {
                dictionaries.Remove(dictionary);
            }
            dictionaries.Add(next);
        }

        text = selected == "en-US" ? English : Chinese;
        CurrentLanguage = selected;
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(selected);
        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }

    public string Get(string key, params object[] arguments)
    {
        var template = text.TryGetValue(key, out var value) ? value : key;
        return arguments.Length == 0
            ? template
            : string.Format(CultureInfo.GetCultureInfo(CurrentLanguage), template, arguments);
    }
}
```

## src/FolderThemeStudio.App/Localization/Strings.en-US.xaml

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:sys="clr-namespace:System;assembly=System.Runtime">
    <sys:String x:Key="App.Title">Folder Theme Studio</sys:String>
    <sys:String x:Key="App.TitleEnglish">Folder Theme Studio</sys:String>
    <sys:String x:Key="App.Subtitle">Create a personal folder style, preview it, and apply it safely.</sys:String>
    <sys:String x:Key="App.AutomationName">Folder Theme Studio editor</sys:String>
    <sys:String x:Key="Top.Tutorial">_Tutorial</sys:String>
    <sys:String x:Key="Top.Settings">_Settings</sys:String>
    <sys:String x:Key="Top.AutoStyleRules">_Automatic Rules</sys:String>
    <sys:String x:Key="Top.FolderCategories">Folder Hover Cards</sys:String>
    <sys:String x:Key="Top.FolderCategoriesAutomation">Open folder hover cards</sys:String>
    <sys:String x:Key="Top.TutorialAutomation">Open tutorial</sys:String>
    <sys:String x:Key="Top.SettingsAutomation">Open settings</sys:String>
    <sys:String x:Key="Top.AutoStyleRulesAutomation">Manage folder-name matching rules</sys:String>
    <sys:String x:Key="FolderCategories.Window.Title">Folder Hover Cards</sys:String>
    <sys:String x:Key="FolderCategories.Window.Subtitle">Show your category and note when the pointer rests over a desktop or File Explorer folder.</sys:String>
    <sys:String x:Key="FolderCategories.List.Title">Categorized folders</sys:String>
    <sys:String x:Key="FolderCategories.List.EmptyHint">Choose a folder and give it a short category. Your settings stay in this Windows user profile.</sys:String>
    <sys:String x:Key="FolderCategories.Form.Title">Card details</sys:String>
    <sys:String x:Key="FolderCategories.Form.NoFolder">No folder selected</sys:String>
    <sys:String x:Key="FolderCategories.Form.Category">Category (up to 32 characters)</sys:String>
    <sys:String x:Key="FolderCategories.Form.Note">Note (optional, up to 140 characters)</sys:String>
    <sys:String x:Key="FolderCategories.Form.Color">Card accent color</sys:String>
    <sys:String x:Key="FolderCategories.Form.GradientStart">Gradient start</sys:String>
    <sys:String x:Key="FolderCategories.Form.GradientEnd">Gradient end</sys:String>
    <sys:String x:Key="Settings.CardOpacity">Note card opacity</sys:String>
    <sys:String x:Key="Settings.QuickEditChord">Quick-edit gesture</sys:String>
    <sys:String x:Key="Settings.QuickEditHint">Hold modifier keys and right- or middle-click the button. Plain right-click is not allowed.</sys:String>
    <sys:String x:Key="FolderCategories.Action.AddFolder">Add folder…</sys:String>
    <sys:String x:Key="FolderCategories.Action.ChooseFolder">Choose folder…</sys:String>
    <sys:String x:Key="FolderCategories.Action.New">New</sys:String>
    <sys:String x:Key="FolderCategories.Action.Save">Save category</sys:String>
    <sys:String x:Key="FolderCategories.Action.Remove">Remove category</sys:String>
    <sys:String x:Key="FolderCategories.Action.Close">Close</sys:String>
    <sys:String x:Key="FolderCategories.Help">Rest the pointer over a folder in Explorer's file list for about half a second. Cards appear only for real folders with a saved category; no folder icon or contents are changed.</sys:String>
    <sys:String x:Key="FolderCategories.Status.Ready">Choose or create a folder category.</sys:String>
    <sys:String x:Key="FolderCategories.Status.Selected">Existing category selected.</sys:String>
    <sys:String x:Key="FolderCategories.Status.New">Folder selected. Enter a category name.</sys:String>
    <sys:String x:Key="FolderCategories.Status.Saved">Category saved and hover card updated.</sys:String>
    <sys:String x:Key="FolderCategories.Status.Removed">Category removed.</sys:String>
    <sys:String x:Key="FolderCategories.Status.Error">Could not read or save categories. Check permissions or the local configuration file.</sys:String>
    <sys:String x:Key="AutoRules.Window.Title">Automatic Style Rules</sys:String>
    <sys:String x:Key="AutoRules.Window.Subtitle">Choose icon styles for new folders by their names</sys:String>
    <sys:String x:Key="AutoRules.Root.Label">Monitored folder</sys:String>
    <sys:String x:Key="AutoRules.Fallback">Unmatched folders use:</sys:String>
    <sys:String x:Key="AutoRules.List.Title">Rules</sys:String>
    <sys:String x:Key="AutoRules.List.EmptyTitle">No name rules yet</sys:String>
    <sys:String x:Key="AutoRules.List.EmptyBody">Add a keyword and icon style to automatically match new folders.</sys:String>
    <sys:String x:Key="AutoRules.List.Priority">Priority</sys:String>
    <sys:String x:Key="AutoRules.List.Enabled">Enabled</sys:String>
    <sys:String x:Key="AutoRules.Form.Title">Rule details</sys:String>
    <sys:String x:Key="AutoRules.Form.Keyword">Folder name contains</sys:String>
    <sys:String x:Key="AutoRules.Form.CurrentTheme">Use current editor style</sys:String>
    <sys:String x:Key="AutoRules.Form.ImportedImage">Use an image or ICO file</sys:String>
    <sys:String x:Key="AutoRules.Form.ChooseImage">Choose image / ICO…</sys:String>
    <sys:String x:Key="AutoRules.Form.Enabled">Enable this rule</sys:String>
    <sys:String x:Key="AutoRules.Form.Preview">Icon preview</sys:String>
    <sys:String x:Key="AutoRules.Action.New">New rule</sys:String>
    <sys:String x:Key="AutoRules.Action.Save">Save rule</sys:String>
    <sys:String x:Key="AutoRules.Action.Cancel">Cancel edit</sys:String>
    <sys:String x:Key="AutoRules.Action.Delete">Delete</sys:String>
    <sys:String x:Key="AutoRules.Action.MoveUp">Move up</sys:String>
    <sys:String x:Key="AutoRules.Action.MoveDown">Move down</sys:String>
    <sys:String x:Key="AutoRules.Help">Folders under the selected root are monitored recursively. Names match keywords case-insensitively; the first rule wins. Unmatched folders keep the root's default icon. Rules apply only to folders created after monitoring is active.</sys:String>
    <sys:String x:Key="AutoRules.Action.Close">Close</sys:String>
    <sys:String x:Key="Section.SavedThemes">Personal themes (optional)</sys:String>
    <sys:String x:Key="Section.ColorMaterial">Color and material</sys:String>
    <sys:String x:Key="Section.EdgeDepth">Edge and depth</sys:String>
    <sys:String x:Key="Section.LivePreview">Live preview</sys:String>
    <sys:String x:Key="Section.ApplySafely">Apply safely</sys:String>
    <sys:String x:Key="Section.Results">Results</sys:String>
    <sys:String x:Key="Field.SavedTheme">_Saved theme</sys:String>
    <sys:String x:Key="Field.ThemeName">Theme _name</sys:String>
    <sys:String x:Key="Field.GradientStart">Gradient s_tart</sys:String>
    <sys:String x:Key="Field.GradientEnd">Gradient _end</sys:String>
    <sys:String x:Key="Field.GradientAngle">Gradient ang_le</sys:String>
    <sys:String x:Key="Field.Opacity">_Opacity</sys:String>
    <sys:String x:Key="Field.Highlight">_Highlight strength</sys:String>
    <sys:String x:Key="Field.GlowColor">_Glow color</sys:String>
    <sys:String x:Key="Field.GlowStrength">Glow strength</sys:String>
    <sys:String x:Key="Field.StrokeColor">Stroke _color</sys:String>
    <sys:String x:Key="Field.StrokeWidth">Stroke _width</sys:String>
    <sys:String x:Key="Field.StrokeOpacity">Stroke o_pacity</sys:String>
    <sys:String x:Key="Field.CornerRadius">Corner _radius</sys:String>
    <sys:String x:Key="Field.GlowRadius">Glow radius</sys:String>
    <sys:String x:Key="Field.ShadowStrength">Shadow strength</sys:String>
    <sys:String x:Key="Field.ShadowOffset">Shadow offset</sys:String>
    <sys:String x:Key="Field.ApplicationMode">Application _mode</sys:String>
    <sys:String x:Key="Field.Roots">Explicit folder roots</sys:String>
    <sys:String x:Key="Button.SaveTheme">Sa_ve as personal theme</sys:String>
    <sys:String x:Key="Button.ImportTheme">_Import theme…</sys:String>
    <sys:String x:Key="Button.ExportTheme">E_xport selected…</sys:String>
    <sys:String x:Key="Button.DeleteTheme">_Delete selected…</sys:String>
    <sys:String x:Key="Button.AddFolder">Add folder…</sys:String>
    <sys:String x:Key="Button.RemoveFolder">Remove selected</sys:String>
    <sys:String x:Key="Button.CalculatePlan">Calculate plan</sys:String>
    <sys:String x:Key="Button.Apply">_Apply folder theme</sys:String>
    <sys:String x:Key="Button.Restore">Restore latest _backup</sys:String>
    <sys:String x:Key="Button.CopyFailed">Cop_y failed paths</sys:String>
    <sys:String x:Key="Button.ExportFailure">Export _failure report…</sys:String>
    <sys:String x:Key="Preview.Description">All previews use the same renderer as the exported folder icon.</sys:String>
    <sys:String x:Key="Preview.Light">On a light background</sys:String>
    <sys:String x:Key="Preview.Dark">On a dark background</sys:String>
    <sys:String x:Key="Apply.Description">Global mode changes current-user mappings. Compatible mode changes only selected roots.</sys:String>
    <sys:String x:Key="Plan.Allowed">Allowed: {0}</sys:String>
    <sys:String x:Key="Plan.Skipped">Skipped: {0}</sys:String>
    <sys:String x:Key="Result.Status">Status</sys:String>
    <sys:String x:Key="Result.Path">Path</sys:String>
    <sys:String x:Key="Result.Detail">Detail</sys:String>
    <sys:String x:Key="Settings.Title">Settings</sys:String>
    <sys:String x:Key="Settings.General">General</sys:String>
    <sys:String x:Key="Settings.Palette">Icon Palette</sys:String>
    <sys:String x:Key="Settings.Language">Language</sys:String>
    <sys:String x:Key="Settings.Chinese">简体中文</sys:String>
    <sys:String x:Key="Settings.English">English</sys:String>
    <sys:String x:Key="Settings.ColorRole">Color role</sys:String>
    <sys:String x:Key="Settings.GradientStart">Gradient start</sys:String>
    <sys:String x:Key="Settings.GradientEnd">Gradient end</sys:String>
    <sys:String x:Key="Settings.Stroke">Stroke</sys:String>
    <sys:String x:Key="Settings.Glow">Glow</sys:String>
    <sys:String x:Key="Settings.Hue">Hue</sys:String>
    <sys:String x:Key="Settings.Saturation">Saturation</sys:String>
    <sys:String x:Key="Settings.Value">Brightness</sys:String>
    <sys:String x:Key="Settings.Hex">Hex color</sys:String>
    <sys:String x:Key="Settings.Recommended">Recommended</sys:String>
    <sys:String x:Key="Settings.Recent">Recent</sys:String>
    <sys:String x:Key="Common.Save">_Save</sys:String>
    <sys:String x:Key="Common.Cancel">_Cancel</sys:String>
    <sys:String x:Key="Common.Previous">_Previous</sys:String>
    <sys:String x:Key="Common.Next">_Next</sys:String>
    <sys:String x:Key="Common.Skip">_Skip</sys:String>
    <sys:String x:Key="Common.Finish">_Finish</sys:String>
    <sys:String x:Key="Tutorial.Title">Quick tutorial</sys:String>
    <sys:String x:Key="Tutorial.Step1.Title">Choose a style</sys:String>
    <sys:String x:Key="Tutorial.Step1.Body">Open Icon studio from Home. Choose an Apple-inspired color or import your own image. Fine-tune advanced settings later.</sys:String>
    <sys:String x:Key="Tutorial.Step2.Title">Choose folders</sys:String>
    <sys:String x:Key="Tutorial.Step2.Body">Add the folders you want to change. Enable automatic styling if new folders should inherit the same style.</sys:String>
    <sys:String x:Key="Tutorial.Step3.Title">Preview &amp; apply</sys:String>
    <sys:String x:Key="Tutorial.Step3.Body">Preview the application plan, then apply it. A backup is created automatically. To undo, open Backup &amp; restore from Home.</sys:String>
    <sys:String x:Key="Tutorial.Step4.Title">Apply safely</sys:String>
    <sys:String x:Key="Tutorial.Step4.Body">Calculate the plan, review it, then apply to ordinary folders.</sys:String>
    <sys:String x:Key="Tutorial.Step5.Title">Restore when needed</sys:String>
    <sys:String x:Key="Tutorial.Step5.Body">Restore the latest backup to undo the most recent application.</sys:String>
    <sys:String x:Key="IconSource.Title">Icon source</sys:String>
    <sys:String x:Key="IconSource.BuiltIn">Built-in folder style</sys:String>
    <sys:String x:Key="IconSource.Imported">Imported image icon</sys:String>
    <sys:String x:Key="IconSource.ImmediateUseHint">Current edits and newly imported icons can be previewed, planned, and applied without saving. Save a personal theme only for later reuse.</sys:String>
    <sys:String x:Key="ImageImport.Title">Custom image icon</sys:String>
    <sys:String x:Key="ImageImport.Description">Import a PNG, JPG, BMP, or ICO. The image keeps its aspect ratio, is centered, and uses transparent padding.</sys:String>
    <sys:String x:Key="ImageImport.Empty">No image selected</sys:String>
    <sys:String x:Key="ImageImport.Choose">Choose image</sys:String>
    <sys:String x:Key="ImageImport.Clear">Clear image</sys:String>
    <sys:String x:Key="Palette.Title">Visual color palette</sys:String>
    <sys:String x:Key="Palette.Description">Choose a color role, then pick a color directly with the mouse.</sys:String>
    <sys:String x:Key="Section.Sticker">Sticker</sys:String>
    <sys:String x:Key="Field.Material">Folder material</sys:String>
    <sys:String x:Key="Material.Glass">Neon glass</sys:String>
    <sys:String x:Key="Material.Clay">Soft clay</sys:String>
    <sys:String x:Key="Material.FrostedGlass">Frosted glass</sys:String>
    <sys:String x:Key="Material.Paper">Paper</sys:String>
    <sys:String x:Key="Field.Sticker">Sticker shape</sys:String>
    <sys:String x:Key="Field.StickerColor">Sticker color</sys:String>
    <sys:String x:Key="Field.StickerScale">Sticker size</sys:String>
    <sys:String x:Key="Field.StickerRotation">Sticker rotation</sys:String>
    <sys:String x:Key="Sticker.None">None</sys:String>
    <sys:String x:Key="Sticker.Star">Star</sys:String>
    <sys:String x:Key="Sticker.Flower">Flower</sys:String>
    <sys:String x:Key="Sticker.Heart">Heart</sys:String>
    <sys:String x:Key="Sticker.Paw">Paw</sys:String>
    <sys:String x:Key="Sticker.DragHint">Drag the sticker on the large preview to position it.</sys:String>
    <sys:String x:Key="Section.DesktopPreview">Simulated desktop</sys:String>
    <sys:String x:Key="Field.DesktopBackground">Background</sys:String>
    <sys:String x:Key="Field.DesktopIconSize">Icon display size</sys:String>
    <sys:String x:Key="Desktop.Midnight">Midnight blue</sys:String>
    <sys:String x:Key="Desktop.DefaultWallpaper">Rugu · Default landscape</sys:String>
    <sys:String x:Key="Desktop.Dusk">Dusk purple</sys:String>
    <sys:String x:Key="Desktop.Forest">Forest green</sys:String>
    <sys:String x:Key="Desktop.Projects">Projects</sys:String>
    <sys:String x:Key="Desktop.Photos">Photos</sys:String>
    <sys:String x:Key="Desktop.Notes">Notes</sys:String>
    <sys:String x:Key="LiveFolder.Recent">Recent files</sys:String>
    <sys:String x:Key="LiveFolder.Empty">No regular files here yet</sys:String>
    <sys:String x:Key="LiveFolder.DropHint">Drag files here, then drop onto a destination below to move them</sys:String>
    <sys:String x:Key="LiveFolder.ThisFolder">This folder</sys:String>
    <sys:String x:Key="LiveFolder.InfoFormat" xml:space="preserve">Created: {0}&#10;Modified: {1}&#10;Contents: {2} files · {3} folders&#10;Size: {4}</sys:String>
    <sys:String x:Key="LiveFolder.InfoPartial"> (partial)</sys:String>
    <sys:String x:Key="LiveFolder.MoveResult">Moved {0} file(s); {1} failed</sys:String>
    <sys:String x:Key="Home.BrandSubtitle">A desktop of your own.</sys:String>
    <sys:String x:Key="Home.Nav">Home</sys:String>
    <sys:String x:Key="Home.Subtitle">Your everyday tools, in one place.</sys:String>
    <sys:String x:Key="Home.HeroTitle">Make your desktop yours.</sys:String>
    <sys:String x:Key="Home.HeroBody">Start with a color you love. Personalize folders, add notes, and keep new folders consistent.</sys:String>
    <sys:String x:Key="Home.Start">Start designing</sys:String>
    <sys:String x:Key="Home.Tools">Explore your tools</sys:String>
    <sys:String x:Key="Home.Design">Icon studio</sys:String>
    <sys:String x:Key="Home.DesignBody">Choose a color or import an image, then preview and apply.</sys:String>
    <sys:String x:Key="Home.LiveFolder">Live folders &amp; notes</sys:String>
    <sys:String x:Key="Home.DynamicIsland">Desktop Island</sys:String>
    <sys:String x:Key="Home.DynamicIslandBody">Time, performance, music and clipboard in one place. Drag to move; scroll to adjust opacity.</sys:String>
    <sys:String x:Key="Home.LiveBody">Preview recent files on hover and keep a useful note.</sys:String>
    <sys:String x:Key="Home.AutoStyle">Automatic styles</sys:String>
    <sys:String x:Key="Home.AutoBody">Match folder names and style new folders automatically.</sys:String>
    <sys:String x:Key="Home.Recovery">Backup &amp; restore</sys:String>
    <sys:String x:Key="Home.RecoveryBody">Review results and restore your latest backup when needed.</sys:String>
    <sys:String x:Key="Home.QuickNoteTip">Quick notes: Alt + right-click a folder by default. The clicked folder is selected for you. Change the shortcut in Settings.</sys:String>
    <sys:String x:Key="Studio.Subtitle">Choose a color you love, then decide where it belongs.</sys:String>
    <sys:String x:Key="Studio.Steps">1  Choose a style     →     2  Choose folders     →     3  Preview &amp; apply</sys:String>
    <sys:String x:Key="Studio.ColorTitle">Apple-inspired colors</sys:String>
    <sys:String x:Key="Studio.ColorHint">Choose a swatch to start. Fine-tune the details in Advanced options.</sys:String>
    <sys:String x:Key="Studio.MyStyles">My styles · save &amp; manage</sys:String>
    <sys:String x:Key="Studio.Advanced">Advanced · color &amp; finish</sys:String>
    <sys:String x:Key="Studio.SizePreview">Check light, dark &amp; small sizes</sys:String>
    <sys:String x:Key="Studio.ApplyTitle">Choose folders &amp; apply</sys:String>
    <sys:String x:Key="Material.Satin">Soft satin</sys:String>
    <sys:String x:Key="QuickNote.Title">Folder note</sys:String>
    <sys:String x:Key="QuickNote.Details">Category &amp; color</sys:String>
    <sys:String x:Key="QuickNote.Shortcut">Ctrl + Enter to save · Esc to cancel</sys:String>
    <sys:String x:Key="Recovery.Description">A backup is created before applying a style. Restore the latest application below and review which items were restored, skipped, or failed.</sys:String>
    <sys:String x:Key="Apply.ModeCompatible">Selected folders only</sys:String>
    <sys:String x:Key="Apply.ModeGlobal">All ordinary folders for this user</sys:String>
    <sys:String x:Key="QuickNote.Save">Save</sys:String>
    <sys:String x:Key="Studio.ChooseFoldersHint">Your style is ready. Choose the folders you want to change.</sys:String>
    <sys:String x:Key="Studio.ReadyHint">Your style is ready to preview and apply.</sys:String>
    <sys:String x:Key="Studio.ChooseImageHint">Choose a PNG, JPG, BMP, or ICO image.</sys:String>
</ResourceDictionary>
```

## src/FolderThemeStudio.App/Localization/Strings.zh-CN.xaml

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:sys="clr-namespace:System;assembly=System.Runtime">
    <sys:String x:Key="App.Title">文件夹主题工坊</sys:String>
    <sys:String x:Key="App.TitleEnglish">Folder Theme Studio</sys:String>
    <sys:String x:Key="App.Subtitle">创建个性化文件夹样式，实时预览并安全应用。</sys:String>
    <sys:String x:Key="App.AutomationName">文件夹主题工坊编辑器</sys:String>
    <sys:String x:Key="Top.Tutorial">使用教程</sys:String>
    <sys:String x:Key="Top.Settings">设置</sys:String>
    <sys:String x:Key="Top.AutoStyleRules">自动样式规则</sys:String>
    <sys:String x:Key="Top.FolderCategories">文件夹分类卡片</sys:String>
    <sys:String x:Key="Top.FolderCategoriesAutomation">打开文件夹悬浮分类卡片</sys:String>
    <sys:String x:Key="Top.TutorialAutomation">打开使用教程</sys:String>
    <sys:String x:Key="Top.SettingsAutomation">打开设置</sys:String>
    <sys:String x:Key="Top.AutoStyleRulesAutomation">管理文件夹名称匹配规则</sys:String>
    <sys:String x:Key="FolderCategories.Window.Title">悬浮分类卡片</sys:String>
    <sys:String x:Key="FolderCategories.Window.Subtitle">鼠标停留在桌面或资源管理器中的文件夹上时，显示你为它添加的分类和备注。</sys:String>
    <sys:String x:Key="FolderCategories.List.Title">已分类文件夹</sys:String>
    <sys:String x:Key="FolderCategories.List.EmptyHint">选择一个文件夹，为它添加简短分类。设置保存在当前 Windows 用户配置中。</sys:String>
    <sys:String x:Key="FolderCategories.Form.Title">分类内容</sys:String>
    <sys:String x:Key="FolderCategories.Form.NoFolder">尚未选择文件夹</sys:String>
    <sys:String x:Key="FolderCategories.Form.Category">分类名称（最多 32 字）</sys:String>
    <sys:String x:Key="FolderCategories.Form.Note">备注（可选，最多 140 字）</sys:String>
    <sys:String x:Key="FolderCategories.Form.Color">卡片强调色</sys:String>
    <sys:String x:Key="FolderCategories.Form.GradientStart">渐变起始色</sys:String>
    <sys:String x:Key="FolderCategories.Form.GradientEnd">渐变结束色</sys:String>
    <sys:String x:Key="Settings.CardOpacity">备注卡片不透明度（0% 全透明）</sys:String>
    <sys:String x:Key="Settings.QuickEditChord">快速编辑快捷操作</sys:String>
    <sys:String x:Key="Settings.QuickEditHint">在上方按钮按下修饰键并点击鼠标右键或中键；不能使用无修饰键的右键。</sys:String>
    <sys:String x:Key="FolderCategories.Action.AddFolder">添加文件夹…</sys:String>
    <sys:String x:Key="FolderCategories.Action.ChooseFolder">选择文件夹…</sys:String>
    <sys:String x:Key="FolderCategories.Action.New">新建</sys:String>
    <sys:String x:Key="FolderCategories.Action.Save">保存分类</sys:String>
    <sys:String x:Key="FolderCategories.Action.Remove">移除分类</sys:String>
    <sys:String x:Key="FolderCategories.Action.Close">关闭</sys:String>
    <sys:String x:Key="FolderCategories.Help">在资源管理器的文件列表中将鼠标停留约半秒；仅为已设置分类的真实文件夹显示卡片。不会修改文件夹图标或内容。</sys:String>
    <sys:String x:Key="FolderCategories.Status.Ready">选择或新建一个文件夹分类。</sys:String>
    <sys:String x:Key="FolderCategories.Status.Selected">已选择现有分类。</sys:String>
    <sys:String x:Key="FolderCategories.Status.New">文件夹已选定，请填写分类名称。</sys:String>
    <sys:String x:Key="FolderCategories.Status.Saved">分类已保存，悬浮卡片已更新。</sys:String>
    <sys:String x:Key="FolderCategories.Status.Removed">分类已移除。</sys:String>
    <sys:String x:Key="FolderCategories.Status.Error">无法读取或保存分类，请检查权限或本地配置文件。</sys:String>
    <sys:String x:Key="AutoRules.Window.Title">自动样式规则</sys:String>
    <sys:String x:Key="AutoRules.Window.Subtitle">为监控目录中的新文件夹按名称自动选择图标样式</sys:String>
    <sys:String x:Key="AutoRules.Root.Label">监控目录</sys:String>
    <sys:String x:Key="AutoRules.Fallback">未匹配时使用：</sys:String>
    <sys:String x:Key="AutoRules.List.Title">规则列表</sys:String>
    <sys:String x:Key="AutoRules.List.EmptyTitle">还没有名称规则</sys:String>
    <sys:String x:Key="AutoRules.List.EmptyBody">添加关键词和图标样式，让新建文件夹自动匹配。</sys:String>
    <sys:String x:Key="AutoRules.List.Priority">优先级</sys:String>
    <sys:String x:Key="AutoRules.List.Enabled">已启用</sys:String>
    <sys:String x:Key="AutoRules.Form.Title">规则详情</sys:String>
    <sys:String x:Key="AutoRules.Form.Keyword">名称包含关键词</sys:String>
    <sys:String x:Key="AutoRules.Form.CurrentTheme">使用当前编辑样式</sys:String>
    <sys:String x:Key="AutoRules.Form.ImportedImage">使用图片或 ICO 文件</sys:String>
    <sys:String x:Key="AutoRules.Form.ChooseImage">选择图片 / ICO…</sys:String>
    <sys:String x:Key="AutoRules.Form.Enabled">启用此规则</sys:String>
    <sys:String x:Key="AutoRules.Form.Preview">图标预览</sys:String>
    <sys:String x:Key="AutoRules.Action.New">新建规则</sys:String>
    <sys:String x:Key="AutoRules.Action.Save">保存规则</sys:String>
    <sys:String x:Key="AutoRules.Action.Cancel">取消编辑</sys:String>
    <sys:String x:Key="AutoRules.Action.Delete">删除</sys:String>
    <sys:String x:Key="AutoRules.Action.MoveUp">上移</sys:String>
    <sys:String x:Key="AutoRules.Action.MoveDown">下移</sys:String>
    <sys:String x:Key="AutoRules.Help">递归监控所选目录；按文件夹名称包含关键词（不区分大小写）匹配，列表靠前规则优先。未命中时沿用目录默认图标。规则仅作用于之后新建的文件夹。</sys:String>
    <sys:String x:Key="AutoRules.Action.Close">关闭</sys:String>
    <sys:String x:Key="Section.SavedThemes">个人主题（可选）</sys:String>
    <sys:String x:Key="Section.ColorMaterial">颜色与材质</sys:String>
    <sys:String x:Key="Section.EdgeDepth">边缘与层次</sys:String>
    <sys:String x:Key="Section.LivePreview">实时预览</sys:String>
    <sys:String x:Key="Section.ApplySafely">安全应用</sys:String>
    <sys:String x:Key="Section.Results">操作结果</sys:String>
    <sys:String x:Key="Field.SavedTheme">已保存主题</sys:String>
    <sys:String x:Key="Field.ThemeName">主题名称</sys:String>
    <sys:String x:Key="Field.GradientStart">渐变起始色</sys:String>
    <sys:String x:Key="Field.GradientEnd">渐变结束色</sys:String>
    <sys:String x:Key="Field.GradientAngle">渐变角度</sys:String>
    <sys:String x:Key="Field.Opacity">不透明度</sys:String>
    <sys:String x:Key="Field.Highlight">高光强度</sys:String>
    <sys:String x:Key="Field.GlowColor">光晕颜色</sys:String>
    <sys:String x:Key="Field.GlowStrength">光晕强度</sys:String>
    <sys:String x:Key="Field.StrokeColor">描边颜色</sys:String>
    <sys:String x:Key="Field.StrokeWidth">描边宽度</sys:String>
    <sys:String x:Key="Field.StrokeOpacity">描边不透明度</sys:String>
    <sys:String x:Key="Field.CornerRadius">圆角半径</sys:String>
    <sys:String x:Key="Field.GlowRadius">光晕半径</sys:String>
    <sys:String x:Key="Field.ShadowStrength">阴影强度</sys:String>
    <sys:String x:Key="Field.ShadowOffset">阴影偏移</sys:String>
    <sys:String x:Key="Field.ApplicationMode">应用模式</sys:String>
    <sys:String x:Key="Field.Roots">指定文件夹根目录</sys:String>
    <sys:String x:Key="Button.SaveTheme">保存为个人主题</sys:String>
    <sys:String x:Key="Button.ImportTheme">导入主题…</sys:String>
    <sys:String x:Key="Button.ExportTheme">导出所选主题…</sys:String>
    <sys:String x:Key="Button.DeleteTheme">删除所选主题…</sys:String>
    <sys:String x:Key="Button.AddFolder">添加文件夹…</sys:String>
    <sys:String x:Key="Button.RemoveFolder">移除所选项</sys:String>
    <sys:String x:Key="Button.CalculatePlan">计算方案</sys:String>
    <sys:String x:Key="Button.Apply">应用文件夹主题</sys:String>
    <sys:String x:Key="Button.Restore">恢复最近备份</sys:String>
    <sys:String x:Key="Button.CopyFailed">复制失败路径</sys:String>
    <sys:String x:Key="Button.ExportFailure">导出失败报告…</sys:String>
    <sys:String x:Key="Preview.Description">所有预览与导出的文件夹图标使用同一个渲染器。</sys:String>
    <sys:String x:Key="Preview.Light">浅色背景</sys:String>
    <sys:String x:Key="Preview.Dark">深色背景</sys:String>
    <sys:String x:Key="Apply.Description">全局模式修改当前用户映射；兼容模式仅修改选定根目录。</sys:String>
    <sys:String x:Key="Plan.Allowed">允许：{0}</sys:String>
    <sys:String x:Key="Plan.Skipped">跳过：{0}</sys:String>
    <sys:String x:Key="Result.Status">状态</sys:String>
    <sys:String x:Key="Result.Path">路径</sys:String>
    <sys:String x:Key="Result.Detail">详情</sys:String>
    <sys:String x:Key="Settings.Title">设置</sys:String>
    <sys:String x:Key="Settings.General">常规</sys:String>
    <sys:String x:Key="Settings.Palette">图标调色板</sys:String>
    <sys:String x:Key="Settings.Language">语言</sys:String>
    <sys:String x:Key="Settings.Chinese">简体中文</sys:String>
    <sys:String x:Key="Settings.English">English</sys:String>
    <sys:String x:Key="Settings.ColorRole">颜色角色</sys:String>
    <sys:String x:Key="Settings.GradientStart">渐变起始色</sys:String>
    <sys:String x:Key="Settings.GradientEnd">渐变结束色</sys:String>
    <sys:String x:Key="Settings.Stroke">描边</sys:String>
    <sys:String x:Key="Settings.Glow">光晕</sys:String>
    <sys:String x:Key="Settings.Hue">色相</sys:String>
    <sys:String x:Key="Settings.Saturation">饱和度</sys:String>
    <sys:String x:Key="Settings.Value">明度</sys:String>
    <sys:String x:Key="Settings.Hex">十六进制颜色</sys:String>
    <sys:String x:Key="Settings.Recommended">推荐配色</sys:String>
    <sys:String x:Key="Settings.Recent">最近使用</sys:String>
    <sys:String x:Key="Common.Save">保存</sys:String>
    <sys:String x:Key="Common.Cancel">取消</sys:String>
    <sys:String x:Key="Common.Previous">上一步</sys:String>
    <sys:String x:Key="Common.Next">下一步</sys:String>
    <sys:String x:Key="Common.Skip">跳过</sys:String>
    <sys:String x:Key="Common.Finish">完成</sys:String>
    <sys:String x:Key="Tutorial.Title">快速使用教程</sys:String>
    <sys:String x:Key="Tutorial.Step1.Title">选择主题</sys:String>
    <sys:String x:Key="Tutorial.Step1.Body">从首页进入图标设计，选择苹果风格色块或导入自己的图片。高级参数可稍后再调整。</sys:String>
    <sys:String x:Key="Tutorial.Step2.Title">选择文件夹</sys:String>
    <sys:String x:Key="Tutorial.Step2.Body">点击添加文件夹，选择要更改的目录。需要时启用持续应用，让新文件夹继承样式。</sys:String>
    <sys:String x:Key="Tutorial.Step3.Title">预览并应用</sys:String>
    <sys:String x:Key="Tutorial.Step3.Body">先预览应用方案，再确认应用。软件会自动备份；需要撤销时，从首页进入备份与恢复。</sys:String>
    <sys:String x:Key="Tutorial.Step4.Title">安全应用</sys:String>
    <sys:String x:Key="Tutorial.Step4.Body">先计算并检查方案，再应用到普通文件夹。</sys:String>
    <sys:String x:Key="Tutorial.Step5.Title">需要时恢复</sys:String>
    <sys:String x:Key="Tutorial.Step5.Body">使用最近备份撤销上一次应用操作。</sys:String>
    <sys:String x:Key="IconSource.Title">图标来源</sys:String>
    <sys:String x:Key="IconSource.BuiltIn">内置文件夹样式</sys:String>
    <sys:String x:Key="IconSource.Imported">导入图片图标</sys:String>
    <sys:String x:Key="IconSource.ImmediateUseHint">当前编辑或刚导入的图标无需保存即可直接预览、计算方案并应用；保存个人主题仅用于以后复用。</sys:String>
    <sys:String x:Key="ImageImport.Title">自定义图片图标</sys:String>
    <sys:String x:Key="ImageImport.Description">导入 PNG、JPG、BMP 或 ICO。图片会保持比例并自动居中，空白区域保持透明。</sys:String>
    <sys:String x:Key="ImageImport.Empty">尚未选择图片</sys:String>
    <sys:String x:Key="ImageImport.Choose">选择图片</sys:String>
    <sys:String x:Key="ImageImport.Clear">清除图片</sys:String>
    <sys:String x:Key="Palette.Title">可视化调色板</sys:String>
    <sys:String x:Key="Palette.Description">先选择要修改的颜色，再直接在色板中用鼠标取色。</sys:String>
    <sys:String x:Key="Section.Sticker">贴纸装饰</sys:String>
    <sys:String x:Key="Field.Material">文件夹材质</sys:String>
    <sys:String x:Key="Material.Glass">玻璃霓虹</sys:String>
    <sys:String x:Key="Material.Clay">奶油黏土</sys:String>
    <sys:String x:Key="Material.FrostedGlass">磨砂玻璃</sys:String>
    <sys:String x:Key="Material.Paper">纸张</sys:String>
    <sys:String x:Key="Field.Sticker">贴纸图案</sys:String>
    <sys:String x:Key="Field.StickerColor">贴纸颜色</sys:String>
    <sys:String x:Key="Field.StickerScale">贴纸大小</sys:String>
    <sys:String x:Key="Field.StickerRotation">贴纸角度</sys:String>
    <sys:String x:Key="Sticker.None">无</sys:String>
    <sys:String x:Key="Sticker.Star">星星</sys:String>
    <sys:String x:Key="Sticker.Flower">花朵</sys:String>
    <sys:String x:Key="Sticker.Heart">爱心</sys:String>
    <sys:String x:Key="Sticker.Paw">猫爪</sys:String>
    <sys:String x:Key="Sticker.DragHint">在右侧大图上拖动贴纸，调整位置。</sys:String>
    <sys:String x:Key="Section.DesktopPreview">模拟桌面</sys:String>
    <sys:String x:Key="Field.DesktopBackground">背景颜色</sys:String>
    <sys:String x:Key="Field.DesktopIconSize">图标显示尺寸</sys:String>
    <sys:String x:Key="Desktop.Midnight">午夜蓝</sys:String>
    <sys:String x:Key="Desktop.DefaultWallpaper">如故 · 默认山水</sys:String>
    <sys:String x:Key="Desktop.Dusk">暮光紫</sys:String>
    <sys:String x:Key="Desktop.Forest">森林绿</sys:String>
    <sys:String x:Key="Desktop.Projects">项目</sys:String>
    <sys:String x:Key="Desktop.Photos">照片</sys:String>
    <sys:String x:Key="Desktop.Notes">笔记</sys:String>
    <sys:String x:Key="LiveFolder.Recent">最近文件</sys:String>
    <sys:String x:Key="LiveFolder.Empty">这里还没有普通文件</sys:String>
    <sys:String x:Key="LiveFolder.DropHint">拖入文件后，选择下方目标文件夹进行移动</sys:String>
    <sys:String x:Key="LiveFolder.ThisFolder">此文件夹</sys:String>
    <sys:String x:Key="LiveFolder.InfoFormat" xml:space="preserve">创建：{0}&#10;修改：{1}&#10;内容：{2} 个文件 · {3} 个文件夹&#10;大小：{4}</sys:String>
    <sys:String x:Key="LiveFolder.InfoPartial">（部分统计）</sys:String>
    <sys:String x:Key="LiveFolder.MoveResult">已移动 {0} 个文件，失败 {1} 个</sys:String>
    <sys:String x:Key="Home.BrandSubtitle">桌面，自成一格。</sys:String>
    <sys:String x:Key="Home.Nav">首页</sys:String>
    <sys:String x:Key="Home.Subtitle">常用功能，触手可及。</sys:String>
    <sys:String x:Key="Home.HeroTitle">让桌面，更像你。</sys:String>
    <sys:String x:Key="Home.HeroBody">从一抹喜欢的颜色开始，为文件夹设计样式，让备注与自动应用融入日常。</sys:String>
    <sys:String x:Key="Home.Start">开始设计</sys:String>
    <sys:String x:Key="Home.Tools">探索功能</sys:String>
    <sys:String x:Key="Home.Design">图标设计</sys:String>
    <sys:String x:Key="Home.DesignBody">选择内置配色或导入图片，预览后应用到文件夹。</sys:String>
    <sys:String x:Key="Home.LiveFolder">活文件夹与备注</sys:String>
    <sys:String x:Key="Home.DynamicIsland">桌面灵动岛</sys:String>
    <sys:String x:Key="Home.DynamicIslandBody">把时间、性能、音乐和剪贴板放到一处。左键拖动，滚轮调整透明度。</sys:String>
    <sys:String x:Key="Home.LiveBody">悬停预览最近文件，为文件夹添加一条实用备注。</sys:String>
    <sys:String x:Key="Home.AutoStyle">自动样式</sys:String>
    <sys:String x:Key="Home.AutoBody">按文件夹名称匹配规则，让新文件夹自动继承样式。</sys:String>
    <sys:String x:Key="Home.Recovery">备份与恢复</sys:String>
    <sys:String x:Key="Home.RecoveryBody">查看应用结果，需要时恢复最近一次备份。</sys:String>
    <sys:String x:Key="Home.QuickNoteTip">桌面快捷备注：默认 Alt＋右键点击文件夹，直接编辑该文件夹的备注。组合键可在设置中调整。</sys:String>
    <sys:String x:Key="Studio.Subtitle">先选喜欢的颜色，再选择应用范围。</sys:String>
    <sys:String x:Key="Studio.Steps">1  选择样式     →     2  选择文件夹     →     3  预览并应用</sys:String>
    <sys:String x:Key="Studio.ColorTitle">苹果风格配色</sys:String>
    <sys:String x:Key="Studio.ColorHint">点一下色块即可使用，也可以展开高级选项自由调整。</sys:String>
    <sys:String x:Key="Studio.MyStyles">我的样式 · 保存与管理</sys:String>
    <sys:String x:Key="Studio.Advanced">高级选项 · 颜色与质感</sys:String>
    <sys:String x:Key="Studio.SizePreview">检查浅色、深色与不同尺寸</sys:String>
    <sys:String x:Key="Studio.ApplyTitle">选择文件夹并应用</sys:String>
    <sys:String x:Key="Material.Satin">柔光 · Satin</sys:String>
    <sys:String x:Key="QuickNote.Title">文件夹备注</sys:String>
    <sys:String x:Key="QuickNote.Details">分类与颜色</sys:String>
    <sys:String x:Key="QuickNote.Shortcut">Ctrl＋Enter 保存 · Esc 取消</sys:String>
    <sys:String x:Key="Recovery.Description">应用样式前会自动创建备份。点击下方按钮恢复最近一次应用；完成后会显示成功、跳过或失败的项目。</sys:String>
    <sys:String x:Key="Apply.ModeCompatible">仅所选文件夹</sys:String>
    <sys:String x:Key="Apply.ModeGlobal">当前用户的全部普通文件夹</sys:String>
    <sys:String x:Key="QuickNote.Save">保存</sys:String>
    <sys:String x:Key="Studio.ChooseFoldersHint">样式已准备好，接下来选择要更改的文件夹。</sys:String>
    <sys:String x:Key="Studio.ReadyHint">样式已准备好，可以预览应用方案。</sys:String>
    <sys:String x:Key="Studio.ChooseImageHint">请选择一张 PNG、JPG、BMP 或 ICO 图片。</sys:String>
</ResourceDictionary>
```

## src/FolderThemeStudio.App/MainWindow.xaml

```xml
<controls:RoundedWindow x:Class="FolderThemeStudio.App.MainWindow" Style="{StaticResource {x:Type Window}}"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:automation="clr-namespace:System.Windows.Automation;assembly=PresentationCore"
        xmlns:controls="clr-namespace:FolderThemeStudio.App.Controls"
        xmlns:settings="clr-namespace:FolderThemeStudio.App.Settings"
        xmlns:application="clr-namespace:FolderThemeStudio.Core.Application;assembly=FolderThemeStudio.Core"
        xmlns:themes="clr-namespace:FolderThemeStudio.Core.Themes;assembly=FolderThemeStudio.Core"
        mc:Ignorable="d"
        Title="{DynamicResource App.Title}"
        Width="1280"
        Height="860"
        MinWidth="620"
        MinHeight="620"
        FontFamily="Segoe UI Variable Text, Segoe UI"
        FontSize="13"
        UseLayoutRounding="True"
        WindowStartupLocation="CenterScreen"
        Icon="Assets/FolderThemeStudio.png"
        SizeChanged="Window_SizeChanged"
        automation:AutomationProperties.Name="{DynamicResource App.AutomationName}">

    <Grid>
        <Grid.ColumnDefinitions><ColumnDefinition x:Name="SidebarColumn" Width="196"/><ColumnDefinition Width="*"/></Grid.ColumnDefinitions>
        <Border Background="#EEEFF3" BorderBrush="#E2E2E8" BorderThickness="0,0,1,0" Padding="16,24,16,16">
            <DockPanel LastChildFill="True">
                <StackPanel DockPanel.Dock="Top" Margin="8,0,0,24">
                    <Image Width="38" Height="38" HorizontalAlignment="Left" Source="Assets/FolderThemeStudio.png" Margin="0,0,0,12"/>
                    <TextBlock Text="Folder Studio" FontSize="17" FontWeight="SemiBold"/>
                    <TextBlock Text="{DynamicResource Home.BrandSubtitle}" Foreground="{StaticResource MutedTextBrush}" FontSize="11" Margin="0,3,0,0"/>
                </StackPanel>
                <StackPanel DockPanel.Dock="Bottom">
                    <Button x:Name="TutorialButton" Style="{StaticResource SidebarButtonStyle}" Content="{DynamicResource Top.Tutorial}" Click="Tutorial_Click"/>
                    <Button x:Name="SettingsButton" Style="{StaticResource SidebarButtonStyle}" Content="{DynamicResource Top.Settings}" Click="Settings_Click"/>
                    <TextBlock Text="BETA 18" FontSize="10" Foreground="#98989D" Margin="12,18,0,0"/>
                </StackPanel>
                <StackPanel>
                    <Button x:Name="HomeNavButton" Style="{StaticResource SidebarButtonStyle}" Content="{DynamicResource Home.Nav}" Click="Home_Click" Background="#DEEAFA"/>
                    <Button x:Name="DesignNavButton" Style="{StaticResource SidebarButtonStyle}" Content="{DynamicResource Home.Design}" Click="Designer_Click"/>
                    <Button x:Name="FolderCategoriesButton" Style="{StaticResource SidebarButtonStyle}" Content="{DynamicResource Home.LiveFolder}" Click="FolderCategories_Click"/>
                    <Button x:Name="DynamicIslandButton" Style="{StaticResource SidebarButtonStyle}" Content="{DynamicResource Home.DynamicIsland}" Click="DynamicIsland_Click"/>
                    <Button x:Name="AutoStyleRulesButton" Style="{StaticResource SidebarButtonStyle}" Content="{DynamicResource Home.AutoStyle}" Click="AutoStyleRules_Click"/>
                    <Button x:Name="RecoveryNavButton" Style="{StaticResource SidebarButtonStyle}" Content="{DynamicResource Home.Recovery}" Click="Recovery_Click"/>
                </StackPanel>
            </DockPanel>
        </Border>
        <Border Grid.Column="1" x:Name="WorkspaceWallpaper" IsHitTestVisible="False">
            <Border.Background><ImageBrush ImageSource="Assets/DefaultWallpaper.png" Stretch="UniformToFill"/></Border.Background>
        </Border>
        <Border Grid.Column="1" x:Name="WorkspaceReadabilityVeil" Background="#D9F7F8FA" IsHitTestVisible="False"/>
        <ScrollViewer Grid.Column="1" x:Name="MainScrollViewer" PreviewMouseWheel="MainScroll_PreviewMouseWheel"
                      HorizontalScrollBarVisibility="Disabled" VerticalScrollBarVisibility="Auto" Focusable="False">
            <StackPanel Margin="28,24,28,28">
                <Grid x:Name="HeaderGrid" Margin="0,0,0,22">
                    <StackPanel>
                        <TextBlock x:Name="PageTitle" FontSize="25" FontWeight="SemiBold" Text="{DynamicResource Home.Nav}"/>
                        <TextBlock x:Name="PageSubtitle" Margin="0,5,0,0" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Text="{DynamicResource Home.Subtitle}"/>
                    </StackPanel>
                </Grid>
                <StackPanel x:Name="HomePanel">
                    <Border CornerRadius="22" Padding="28" Margin="0,0,0,20">
                        <Border.Background><LinearGradientBrush StartPoint="0,0" EndPoint="1,1"><GradientStop Color="#E6F0FF" Offset="0"/><GradientStop Color="#F0EAFA" Offset="1"/></LinearGradientBrush></Border.Background>
                        <Grid>
                            <Grid.ColumnDefinitions><ColumnDefinition Width="*"/><ColumnDefinition Width="180"/></Grid.ColumnDefinitions>
                            <StackPanel VerticalAlignment="Center">
                                <TextBlock Text="{DynamicResource Home.HeroTitle}" FontSize="30" FontWeight="SemiBold" TextWrapping="Wrap"/>
                                <TextBlock Text="{DynamicResource Home.HeroBody}" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Margin="0,12,0,20" MaxWidth="390" HorizontalAlignment="Left" LineHeight="23"/>
                                <Button x:Name="HomeStartButton" Style="{StaticResource PrimaryButtonStyle}" Click="Designer_Click" Content="{DynamicResource Home.Start}" HorizontalAlignment="Left" Margin="0"/>
                            </StackPanel>
                            <Image x:Name="HomeHeroIcon" Grid.Column="1" Width="170" Height="170" Source="{Binding Previews[3].Image}" Stretch="Uniform"/>
                        </Grid>
                    </Border>
                    <TextBlock Text="{DynamicResource Home.Tools}" Style="{StaticResource SectionTitleStyle}" Margin="2,2,0,14"/>
                    <Button x:Name="HomeIslandButton" Style="{StaticResource FeatureCardButtonStyle}" Click="DynamicIsland_Click" Margin="0,0,0,16">
                        <Grid><Grid.ColumnDefinitions><ColumnDefinition Width="Auto"/><ColumnDefinition/></Grid.ColumnDefinitions>
                            <Border Width="100" Height="46" Background="#19191D" CornerRadius="23" Margin="0,0,22,0" VerticalAlignment="Center">
                                <StackPanel Orientation="Horizontal" HorizontalAlignment="Center" VerticalAlignment="Center"><Ellipse Width="5" Height="5" Fill="#34C759" Margin="0,0,8,0"/><TextBlock Text="ISLAND" Foreground="White" FontSize="11" FontWeight="SemiBold"/></StackPanel>
                            </Border>
                            <StackPanel Grid.Column="1"><TextBlock Text="{DynamicResource Home.DynamicIsland}" FontSize="17" FontWeight="SemiBold"/>
                                <TextBlock Text="{DynamicResource Home.DynamicIslandBody}" TextWrapping="Wrap" Foreground="{StaticResource MutedTextBrush}" Margin="0,8,0,0" LineHeight="21"/>
                            </StackPanel>
                        </Grid>
                    </Button>
                    <UniformGrid x:Name="HomeFeaturesGrid" Columns="2">
                        <Button Style="{StaticResource FeatureCardButtonStyle}" Click="Designer_Click" Margin="0,0,8,16">
                            <StackPanel><TextBlock Text="&#xE8B7;" FontFamily="Segoe MDL2 Assets" Foreground="#007AFF" FontSize="25" Margin="0,0,0,15"/>
                                <TextBlock Text="{DynamicResource Home.Design}" FontSize="17" FontWeight="SemiBold"/>
                                <TextBlock Text="{DynamicResource Home.DesignBody}" TextWrapping="Wrap" Foreground="{StaticResource MutedTextBrush}" Margin="0,8,0,0" LineHeight="21"/>
                            </StackPanel>
                        </Button>
                        <Button Style="{StaticResource FeatureCardButtonStyle}" Click="FolderCategories_Click" Margin="8,0,0,16">
                            <StackPanel><TextBlock Text="&#xE70B;" FontFamily="Segoe MDL2 Assets" Foreground="#AF52DE" FontSize="25" Margin="0,0,0,15"/>
                                <TextBlock Text="{DynamicResource Home.LiveFolder}" FontSize="17" FontWeight="SemiBold"/>
                                <TextBlock Text="{DynamicResource Home.LiveBody}" TextWrapping="Wrap" Foreground="{StaticResource MutedTextBrush}" Margin="0,8,0,0" LineHeight="21"/>
                            </StackPanel>
                        </Button>
                        <Button Style="{StaticResource FeatureCardButtonStyle}" Click="AutoStyleRules_Click" Margin="0,0,8,16">
                            <StackPanel><TextBlock Text="&#xE713;" FontFamily="Segoe MDL2 Assets" Foreground="#34C759" FontSize="25" Margin="0,0,0,15"/>
                                <TextBlock Text="{DynamicResource Home.AutoStyle}" FontSize="17" FontWeight="SemiBold"/>
                                <TextBlock Text="{DynamicResource Home.AutoBody}" TextWrapping="Wrap" Foreground="{StaticResource MutedTextBrush}" Margin="0,8,0,0" LineHeight="21"/>
                            </StackPanel>
                        </Button>
                        <Button Style="{StaticResource FeatureCardButtonStyle}" Click="Recovery_Click" Margin="8,0,0,16">
                            <StackPanel><TextBlock Text="&#xE777;" FontFamily="Segoe MDL2 Assets" Foreground="#FF9500" FontSize="25" Margin="0,0,0,15"/>
                                <TextBlock Text="{DynamicResource Home.Recovery}" FontSize="17" FontWeight="SemiBold"/>
                                <TextBlock Text="{DynamicResource Home.RecoveryBody}" TextWrapping="Wrap" Foreground="{StaticResource MutedTextBrush}" Margin="0,8,0,0" LineHeight="21"/>
                            </StackPanel>
                        </Button>
                    </UniformGrid>
                    <Border Padding="18" Background="#EBEBF0" CornerRadius="14">
                        <TextBlock Text="{DynamicResource Home.QuickNoteTip}" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" LineHeight="22"/>
                    </Border>
                </StackPanel>
                <StackPanel x:Name="EditorWorkspace" Visibility="Collapsed">
                    <Border CornerRadius="12" Background="#EAEAF0" Padding="16,12" Margin="0,0,0,18">
                        <TextBlock Text="{DynamicResource Studio.Steps}" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap"/>
                    </Border>
            <Grid x:Name="AdaptiveGrid" Grid.Row="0">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition x:Name="EditorColumn" Width="1.05*" />
                    <ColumnDefinition x:Name="ApplicationColumn" Width="0.95*" />
                </Grid.ColumnDefinitions>
                <Grid.RowDefinitions>
                    <RowDefinition x:Name="TopRow" Height="Auto" />
                    <RowDefinition x:Name="StackedRow" Height="0" />
                </Grid.RowDefinitions>

                <StackPanel x:Name="EditorPanel" Grid.Row="0" Grid.Column="0" Margin="0,0,10,0" IsEnabled="{Binding AreInputsEnabled}">
                    <Border Style="{StaticResource CardStyle}">
                        <StackPanel>
                            <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource IconSource.Title}" />
                            <WrapPanel>
                                <RadioButton Margin="0,0,24,0" Padding="6" Content="{DynamicResource IconSource.BuiltIn}"
                                             IsChecked="{Binding IsBuiltInEditor}" />
                                <RadioButton Padding="6" Content="{DynamicResource IconSource.Imported}"
                                             IsChecked="{Binding IsImportedImageEditor}" />
                            </WrapPanel>
                            <TextBlock x:Name="ImmediateUseHint"
                                       Margin="0,8,0,0"
                                       Foreground="{StaticResource MutedTextBrush}"
                                       TextWrapping="Wrap"
                                       Text="{DynamicResource IconSource.ImmediateUseHint}" />
                        </StackPanel>
                    </Border>

                    <Border Style="{StaticResource CardStyle}"
                            Visibility="{Binding IsImportedImageEditor, Converter={StaticResource BoolToVisibility}}">
                        <StackPanel>
                            <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource ImageImport.Title}" />
                            <TextBlock Margin="0,0,0,12" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap"
                                       Text="{DynamicResource ImageImport.Description}" />
                            <Border Height="230" Padding="12" Background="#FFF7F9FC" BorderBrush="{StaticResource BorderBrush}" BorderThickness="1" CornerRadius="10">
                                <Image Source="{Binding ImportedImagePreview}" Stretch="Uniform" />
                            </Border>
                            <TextBlock Margin="0,10,0,10" Foreground="{StaticResource MutedTextBrush}" TextTrimming="CharacterEllipsis"
                                       Text="{Binding ImportedImageAssetPath, TargetNullValue=—}" />
                            <WrapPanel>
                                <Button Style="{StaticResource PrimaryButtonStyle}" Command="{Binding ChooseImportedImageCommand}" Content="{DynamicResource ImageImport.Choose}" />
                                <Button Command="{Binding ClearImportedImageCommand}" Content="{DynamicResource ImageImport.Clear}" />
                            </WrapPanel>
                        </StackPanel>
                    </Border>

                    <Border Style="{StaticResource CardStyle}" Visibility="{Binding IsBuiltInEditor, Converter={StaticResource BoolToVisibility}}">
                        <StackPanel>
                            <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource Studio.ColorTitle}"/>
                            <TextBlock Foreground="{StaticResource MutedTextBrush}" Text="{DynamicResource Studio.ColorHint}" TextWrapping="Wrap"/>
                            <ListBox x:Name="AppleColorSwatches" Margin="0,12,0,10" BorderThickness="0" Background="Transparent"
                                     ItemsSource="{x:Static settings:AppleColors.All}" SelectionChanged="AppleColor_SelectionChanged"
                                     SelectedValuePath="Hex" SelectedValue="{Binding GradientStart, Mode=OneWay}"
                                     ItemContainerStyle="{StaticResource ColorSwatchItemStyle}" ItemTemplate="{StaticResource ColorSwatchTemplate}"
                                     ScrollViewer.HorizontalScrollBarVisibility="Disabled" automation:AutomationProperties.Name="Apple-inspired folder colors">
                                <ListBox.ItemsPanel><ItemsPanelTemplate><WrapPanel/></ItemsPanelTemplate></ListBox.ItemsPanel>
                            </ListBox>
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=MaterialPicker}" Content="{DynamicResource Field.Material}" />
                            <ComboBox x:Name="MaterialPicker" SelectedValuePath="Tag" SelectedValue="{Binding Material}"
                                      automation:AutomationProperties.Name="Folder material">
                                <ComboBoxItem Tag="{x:Static themes:MaterialStyle.Satin}" Content="{DynamicResource Material.Satin}" />
                                <ComboBoxItem Tag="{x:Static themes:MaterialStyle.Glass}" Content="{DynamicResource Material.Glass}" />
                                <ComboBoxItem Tag="{x:Static themes:MaterialStyle.Clay}" Content="{DynamicResource Material.Clay}" />
                                <ComboBoxItem Tag="{x:Static themes:MaterialStyle.FrostedGlass}" Content="{DynamicResource Material.FrostedGlass}" />
                                <ComboBoxItem Tag="{x:Static themes:MaterialStyle.Paper}" Content="{DynamicResource Material.Paper}" />
                            </ComboBox>
                        </StackPanel>
                    </Border>
                    <Border Style="{StaticResource CardStyle}"
                            Visibility="{Binding IsBuiltInEditor, Converter={StaticResource BoolToVisibility}}">
                        <Expander Header="{DynamicResource Studio.MyStyles}"><StackPanel>
                            <Label Style="{StaticResource FieldLabelStyle}"
                                   Target="{Binding ElementName=SavedThemePicker}"
                                   Content="{DynamicResource Field.SavedTheme}" />
                            <ComboBox x:Name="SavedThemePicker"
                                      ItemsSource="{Binding SavedThemes}"
                                      SelectedItem="{Binding SelectedSavedTheme}"
                                      DisplayMemberPath="Name"
                                      automation:AutomationProperties.Name="Saved theme" />
                            <Label Style="{StaticResource FieldLabelStyle}"
                                   Target="{Binding ElementName=ThemeNameBox}"
                                   Content="{DynamicResource Field.ThemeName}" />
                            <TextBox x:Name="ThemeNameBox"
                                     Text="{Binding ThemeName, UpdateSourceTrigger=PropertyChanged}"
                                     automation:AutomationProperties.Name="Theme name" />
                            <WrapPanel Margin="0,12,0,0">
                                <Button Command="{Binding SaveThemeCommand}"
                                        Content="{DynamicResource Button.SaveTheme}"
                                        automation:AutomationProperties.Name="Save as personal theme" />
                                <Button Command="{Binding ImportThemeCommand}"
                                        Content="{DynamicResource Button.ImportTheme}"
                                        automation:AutomationProperties.Name="Import theme from file" />
                                <Button Command="{Binding ExportThemeCommand}"
                                        Content="{DynamicResource Button.ExportTheme}"
                                        automation:AutomationProperties.Name="Export selected theme" />
                                <Button Command="{Binding DeleteThemeCommand}"
                                        Content="{DynamicResource Button.DeleteTheme}"
                                        automation:AutomationProperties.Name="Delete selected personal theme" />
                            </WrapPanel>
                        </StackPanel></Expander>
                    </Border>

                    <Border Style="{StaticResource CardStyle}"
                            Visibility="{Binding IsBuiltInEditor, Converter={StaticResource BoolToVisibility}}">
                        <Expander Header="{DynamicResource Studio.Advanced}"><StackPanel>

                            <Grid>
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*" />
                                    <ColumnDefinition Width="16" />
                                    <ColumnDefinition Width="*" />
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=GradientStartBox}" Content="{DynamicResource Field.GradientStart}" />
                                    <TextBox x:Name="GradientStartBox" Text="{Binding GradientStart, UpdateSourceTrigger=PropertyChanged}" automation:AutomationProperties.Name="Gradient start hexadecimal color" />
                                </StackPanel>
                                <StackPanel Grid.Column="2">
                                    <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=GradientEndBox}" Content="{DynamicResource Field.GradientEnd}" />
                                    <TextBox x:Name="GradientEndBox" Text="{Binding GradientEnd, UpdateSourceTrigger=PropertyChanged}" automation:AutomationProperties.Name="Gradient end hexadecimal color" />
                                </StackPanel>
                            </Grid>
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=GradientAngleSlider}" Content="{DynamicResource Field.GradientAngle}" />
                            <Slider x:Name="GradientAngleSlider" Minimum="0" Maximum="360" TickFrequency="15" IsSnapToTickEnabled="False" Value="{Binding GradientAngle}" automation:AutomationProperties.Name="Gradient angle" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=OpacitySlider}" Content="{DynamicResource Field.Opacity}" />
                            <Slider x:Name="OpacitySlider" Minimum="0" Maximum="1" TickFrequency="0.05" Value="{Binding Opacity}" automation:AutomationProperties.Name="Folder opacity" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=HighlightSlider}" Content="{DynamicResource Field.Highlight}" />
                            <Slider x:Name="HighlightSlider" Minimum="0" Maximum="1" TickFrequency="0.05" Value="{Binding HighlightStrength}" automation:AutomationProperties.Name="Highlight strength" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=GlowColorBox}" Content="{DynamicResource Field.GlowColor}" />
                            <TextBox x:Name="GlowColorBox" Text="{Binding GlowColor, UpdateSourceTrigger=PropertyChanged}" automation:AutomationProperties.Name="Glow hexadecimal color" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=GlowStrengthSlider}" Content="{DynamicResource Field.GlowStrength}" />
                            <Slider x:Name="GlowStrengthSlider" Minimum="0" Maximum="1" TickFrequency="0.05" Value="{Binding GlowStrength}" automation:AutomationProperties.Name="Glow strength" />
                        </StackPanel></Expander>
                    </Border>

                    <Border Style="{StaticResource CardStyle}"
                            Visibility="{Binding IsBuiltInEditor, Converter={StaticResource BoolToVisibility}}">
                        <StackPanel>
                            <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource Palette.Title}" />
                            <TextBlock Margin="0,0,0,10" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap"
                                       Text="{DynamicResource Palette.Description}" />
                            <ComboBox Margin="0,0,0,12" ItemsSource="{Binding PaletteEditor.Roles}"
                                      SelectedItem="{Binding PaletteEditor.SelectedRole}" DisplayMemberPath="DisplayName" />
                            <controls:VisualColorPicker DataContext="{Binding PaletteEditor.SelectedRole.Picker}" />
                        </StackPanel>
                    </Border>

                    <Border Style="{StaticResource CardStyle}"
                            Visibility="{Binding IsBuiltInEditor, Converter={StaticResource BoolToVisibility}}">
                        <StackPanel>
                            <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource Section.EdgeDepth}" />
                            <Grid>
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*" />
                                    <ColumnDefinition Width="16" />
                                    <ColumnDefinition Width="*" />
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=StrokeColorBox}" Content="{DynamicResource Field.StrokeColor}" />
                                    <TextBox x:Name="StrokeColorBox" Text="{Binding StrokeColor, UpdateSourceTrigger=PropertyChanged}" automation:AutomationProperties.Name="Stroke hexadecimal color" />
                                </StackPanel>
                                <StackPanel Grid.Column="2">
                                    <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=StrokeWidthSlider}" Content="{DynamicResource Field.StrokeWidth}" />
                                    <Slider x:Name="StrokeWidthSlider" Minimum="0" Maximum="8" TickFrequency="0.25" Value="{Binding StrokeWidth}" automation:AutomationProperties.Name="Stroke width" />
                                </StackPanel>
                            </Grid>
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=StrokeOpacitySlider}" Content="{DynamicResource Field.StrokeOpacity}" />
                            <Slider x:Name="StrokeOpacitySlider" Minimum="0" Maximum="1" TickFrequency="0.05" Value="{Binding StrokeOpacity}" automation:AutomationProperties.Name="Stroke opacity" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=CornerRadiusSlider}" Content="{DynamicResource Field.CornerRadius}" />
                            <Slider x:Name="CornerRadiusSlider" Minimum="0" Maximum="36" TickFrequency="1" Value="{Binding CornerRadius}" automation:AutomationProperties.Name="Corner radius" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=GlowRadiusSlider}" Content="{DynamicResource Field.GlowRadius}" />
                            <Slider x:Name="GlowRadiusSlider" Minimum="0" Maximum="24" TickFrequency="1" Value="{Binding GlowRadius}" automation:AutomationProperties.Name="Glow radius" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=ShadowStrengthSlider}" Content="{DynamicResource Field.ShadowStrength}" />
                            <Slider x:Name="ShadowStrengthSlider" Minimum="0" Maximum="1" TickFrequency="0.05" Value="{Binding ShadowStrength}" automation:AutomationProperties.Name="Shadow strength" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=ShadowOffsetSlider}" Content="{DynamicResource Field.ShadowOffset}" />
                            <Slider x:Name="ShadowOffsetSlider" Minimum="0" Maximum="20" TickFrequency="1" Value="{Binding ShadowOffset}" automation:AutomationProperties.Name="Shadow offset" />
                        </StackPanel>
                    </Border>
                    <Border Style="{StaticResource CardStyle}"
                            Visibility="{Binding IsBuiltInEditor, Converter={StaticResource BoolToVisibility}}">
                        <Expander Header="{DynamicResource Section.Sticker}"><StackPanel>
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=StickerPicker}" Content="{DynamicResource Field.Sticker}" />
                            <ComboBox x:Name="StickerPicker" SelectedValuePath="Tag" SelectedValue="{Binding Sticker}"
                                      automation:AutomationProperties.Name="Folder sticker">
                                <ComboBoxItem Tag="{x:Static themes:StickerStyle.None}" Content="{DynamicResource Sticker.None}" />
                                <ComboBoxItem Tag="{x:Static themes:StickerStyle.Star}" Content="{DynamicResource Sticker.Star}" />
                                <ComboBoxItem Tag="{x:Static themes:StickerStyle.Flower}" Content="{DynamicResource Sticker.Flower}" />
                                <ComboBoxItem Tag="{x:Static themes:StickerStyle.Heart}" Content="{DynamicResource Sticker.Heart}" />
                                <ComboBoxItem Tag="{x:Static themes:StickerStyle.Paw}" Content="{DynamicResource Sticker.Paw}" />
                            </ComboBox>
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=StickerColorBox}" Content="{DynamicResource Field.StickerColor}" />
                            <TextBox x:Name="StickerColorBox" Text="{Binding StickerColor, UpdateSourceTrigger=PropertyChanged}" automation:AutomationProperties.Name="Sticker hexadecimal color" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=StickerScaleSlider}" Content="{DynamicResource Field.StickerScale}" />
                            <Slider x:Name="StickerScaleSlider" Minimum="0.15" Maximum="0.60" TickFrequency="0.05" Value="{Binding StickerScale}" automation:AutomationProperties.Name="Sticker size" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=StickerRotationSlider}" Content="{DynamicResource Field.StickerRotation}" />
                            <Slider x:Name="StickerRotationSlider" Minimum="-180" Maximum="180" TickFrequency="15" Value="{Binding StickerRotation}" automation:AutomationProperties.Name="Sticker rotation" />
                            <TextBlock Margin="0,8,0,0" Foreground="{StaticResource MutedTextBrush}" Text="{DynamicResource Sticker.DragHint}" TextWrapping="Wrap" />
                        </StackPanel></Expander>
                    </Border>
                </StackPanel>

                <StackPanel x:Name="ApplicationPanel" Grid.Row="0" Grid.Column="1" Margin="10,0,0,0">
                    <Border Style="{StaticResource CardStyle}">
                        <StackPanel>
                            <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource Section.LivePreview}" />
                            <TextBlock Foreground="{StaticResource MutedTextBrush}"
                                       TextWrapping="Wrap"
                                       Text="{DynamicResource Preview.Description}" />
                            <Border Visibility="{Binding IsBuiltInEditor, Converter={StaticResource BoolToVisibility}}" Margin="0,10,0,0" Background="#FFF0F4F9" HorizontalAlignment="Left" CornerRadius="8">
                                <Image x:Name="StickerDragPreview" Width="200" Height="200" Source="{Binding Previews[3].Image}"
                                       Cursor="Hand" MouseLeftButtonDown="StickerPreview_MouseLeftButtonDown"
                                       MouseMove="StickerPreview_MouseMove" MouseLeftButtonUp="StickerPreview_MouseLeftButtonUp"
                                       LostMouseCapture="StickerPreview_LostMouseCapture"
                                       automation:AutomationProperties.Name="Drag sticker on folder preview" />
                            </Border>
                            <Expander Header="{DynamicResource Studio.SizePreview}" Margin="0,14,0,0"><StackPanel><TextBlock Margin="0,12,0,5" FontWeight="SemiBold" Text="{DynamicResource Preview.Light}" />
                            <Border Padding="12" Background="#FFF7F9FC" BorderBrush="#D7DFEA" BorderThickness="1" CornerRadius="8">
                                <ItemsControl ItemsSource="{Binding Previews}">
                                    <ItemsControl.ItemsPanel>
                                        <ItemsPanelTemplate><WrapPanel VerticalAlignment="Bottom" /></ItemsPanelTemplate>
                                    </ItemsControl.ItemsPanel>
                                    <ItemsControl.ItemTemplate>
                                        <DataTemplate>
                                            <StackPanel Margin="8" VerticalAlignment="Bottom">
                                                <Image Width="{Binding Size}" Height="{Binding Size}" Source="{Binding Image}" Stretch="Uniform" automation:AutomationProperties.Name="{Binding Size, StringFormat={}{0} pixel folder preview on light background}" />
                                                <TextBlock Margin="0,5,0,0" HorizontalAlignment="Center" Text="{Binding Size, StringFormat={}{0}px}" />
                                            </StackPanel>
                                        </DataTemplate>
                                    </ItemsControl.ItemTemplate>
                                </ItemsControl>
                            </Border>
                            <TextBlock Margin="0,12,0,5" FontWeight="SemiBold" Text="{DynamicResource Preview.Dark}" />
                            <Border Padding="12" Background="#FF172033" CornerRadius="8">
                                <ItemsControl ItemsSource="{Binding Previews}" Foreground="White">
                                    <ItemsControl.ItemsPanel>
                                        <ItemsPanelTemplate><WrapPanel VerticalAlignment="Bottom" /></ItemsPanelTemplate>
                                    </ItemsControl.ItemsPanel>
                                    <ItemsControl.ItemTemplate>
                                        <DataTemplate>
                                            <StackPanel Margin="8" VerticalAlignment="Bottom">
                                                <Image Width="{Binding Size}" Height="{Binding Size}" Source="{Binding Image}" Stretch="Uniform" automation:AutomationProperties.Name="{Binding Size, StringFormat={}{0} pixel folder preview on dark background}" />
                                                <TextBlock Margin="0,5,0,0" Foreground="White" HorizontalAlignment="Center" Text="{Binding Size, StringFormat={}{0}px}" />
                                            </StackPanel>
                                        </DataTemplate>
                                    </ItemsControl.ItemTemplate>
                                </ItemsControl>
                            </Border>
                            </StackPanel></Expander>
                            <TextBlock Margin="0,12,0,0"
                                       TextWrapping="Wrap"
                                       Foreground="{StaticResource MutedTextBrush}"
                                       automation:AutomationProperties.LiveSetting="Polite">
                                <TextBlock.Style>
                                    <Style TargetType="TextBlock">
                                        <Setter Property="Text" Value="{Binding ValidationMessage}" />
                                        <Style.Triggers>
                                            <DataTrigger Binding="{Binding ValidationMessage}" Value="Compatibility mode requires at least one explicit folder root. No whole-drive root is selected automatically.">
                                                <Setter Property="Text" Value="{DynamicResource Studio.ChooseFoldersHint}" />
                                            </DataTrigger>
                                            <DataTrigger Binding="{Binding ValidationMessage}" Value="图标已准备好，可以预览或计算方案。">
                                                <Setter Property="Text" Value="{DynamicResource Studio.ReadyHint}" />
                                            </DataTrigger>
                                            <DataTrigger Binding="{Binding ValidationMessage}" Value="请先选择一张 PNG、JPG、BMP 或 ICO 图片。">
                                                <Setter Property="Text" Value="{DynamicResource Studio.ChooseImageHint}" />
                                            </DataTrigger>
                                        </Style.Triggers>
                                    </Style>
                                </TextBlock.Style>
                            </TextBlock>
                        </StackPanel>
                    </Border>



                    <Border Style="{StaticResource CardStyle}">
                        <StackPanel>
                            <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource Studio.ApplyTitle}" />
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=ModePicker}" Content="{DynamicResource Field.ApplicationMode}" />
                            <ComboBox x:Name="ModePicker"
                                      SelectedValuePath="Tag" SelectedValue="{Binding SelectedMode}"
                                      IsEnabled="{Binding AreInputsEnabled}"
                                      automation:AutomationProperties.Name="Application mode">
                                <ComboBoxItem Tag="{x:Static application:ApplicationMode.Compatible}" Content="{DynamicResource Apply.ModeCompatible}"/>
                                <ComboBoxItem Tag="{x:Static application:ApplicationMode.Global}" Content="{DynamicResource Apply.ModeGlobal}"/>
                            </ComboBox>
                            <TextBlock Margin="0,8,0,12" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Text="{DynamicResource Apply.Description}" />

                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=RootList}" Content="{DynamicResource Field.Roots}" />
                            <ListBox x:Name="RootList"
                                     MinHeight="70"
                                     MaxHeight="150"
                                     ItemsSource="{Binding SelectedRoots}"
                                     IsEnabled="{Binding AreInputsEnabled}"
                                     automation:AutomationProperties.Name="Explicit folder roots" />
                            <WrapPanel Margin="0,8,0,0">
                                <Button Command="{Binding AddRootCommand}" Content="{DynamicResource Button.AddFolder}" automation:AutomationProperties.Name="Add explicit folder root" />
                                <Button Command="{Binding RemoveRootCommand}" CommandParameter="{Binding SelectedItem, ElementName=RootList}" Content="{DynamicResource Button.RemoveFolder}" automation:AutomationProperties.Name="Remove selected folder root" />
                            </WrapPanel>

                            <StackPanel Visibility="{Binding IsCompatibleMode, Converter={StaticResource BoolToVisibility}}">
                                <CheckBox Margin="0,6,0,8" Content="持续应用到该目录中新建的文件夹（递归）"
                                          IsChecked="{Binding ContinueApplyingToNewFolders}" />
                                <TextBlock Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Text="{Binding MonitoringStatusText}" />
                                <WrapPanel Margin="0,8,0,0">
                                    <Button Command="{Binding ToggleMonitoringCommand}" Content="暂停 / 继续监控" />
                                    <Button Command="{Binding RemoveMonitoringRuleCommand}"
                                            CommandParameter="{Binding SelectedItem, ElementName=RootList}"
                                            Content="停止监控所选目录" />
                                </WrapPanel>
                            </StackPanel>

                            <WrapPanel Margin="0,8,0,0">
                                <Button Command="{Binding PlanCommand}" Content="{DynamicResource Button.CalculatePlan}" automation:AutomationProperties.Name="Calculate write-free application plan" />
                                <TextBlock Margin="8,8,16,0" Text="{Binding PlanAllowedCount, StringFormat=Allowed: {0}}" />
                                <TextBlock Margin="0,8,0,0" Text="{Binding PlanSkippedCount, StringFormat=Skipped: {0}}" />
                            </WrapPanel>
                            <TextBlock Margin="0,4,0,14" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Text="{Binding PlanDetails}" />

                            <WrapPanel>
                                <Button Style="{StaticResource PrimaryButtonStyle}" Command="{Binding ApplyCommand}" Content="{DynamicResource Button.Apply}" automation:AutomationProperties.Name="Apply folder theme" />
                                <Button MinHeight="44" Command="{Binding RestoreCommand}" Content="{DynamicResource Button.Restore}" automation:AutomationProperties.Name="Restore latest recovery snapshot" />
                            </WrapPanel>

                            <ProgressBar Height="8" Margin="0,8,0,6" Minimum="0" Maximum="{Binding ProgressTotal, FallbackValue=1, TargetNullValue=1}" Value="{Binding ProgressCompleted, Mode=OneWay}" automation:AutomationProperties.Name="Operation progress" />
                            <TextBlock TextWrapping="Wrap" Text="{Binding ProgressMessage}" automation:AutomationProperties.LiveSetting="Polite" />
                            <TextBlock Margin="0,8,0,0" FontWeight="SemiBold" TextWrapping="Wrap" Text="{Binding OperationStatus}" automation:AutomationProperties.LiveSetting="Assertive" />
                        </StackPanel>
                    </Border>

                    <Border Style="{StaticResource CardStyle}">
                        <Expander Header="{DynamicResource Section.DesktopPreview}"><StackPanel>
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=DesktopBackgroundPicker}" Content="{DynamicResource Field.DesktopBackground}" />
                            <ComboBox x:Name="DesktopBackgroundPicker" SelectedValuePath="Tag" SelectedValue="{Binding DesktopBackground, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                                      automation:AutomationProperties.Name="Desktop preview background">
                                <ComboBoxItem Tag="#1B2A4A" Content="{DynamicResource Desktop.Midnight}" />
                                <ComboBoxItem Tag="#4A2D51" Content="{DynamicResource Desktop.Dusk}" />
                                <ComboBoxItem Tag="#204A46" Content="{DynamicResource Desktop.Forest}" />
                                <ComboBoxItem Tag="DefaultWallpaper" Content="{DynamicResource Desktop.DefaultWallpaper}" />
                            </ComboBox>
                            <Label Style="{StaticResource FieldLabelStyle}" Target="{Binding ElementName=DesktopIconSizeSlider}" Content="{DynamicResource Field.DesktopIconSize}" />
                            <Slider x:Name="DesktopIconSizeSlider" Minimum="64" Maximum="128" TickFrequency="64" IsSnapToTickEnabled="True"
                                    Value="{Binding DesktopIconSize, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" automation:AutomationProperties.Name="Desktop preview icon size" />
                            <Border x:Name="DesktopPreviewSurface" Height="250" Margin="0,10,0,0" CornerRadius="8" ClipToBounds="True">
                                <Border.Style>
                                    <Style TargetType="Border">
                                        <Setter Property="Background"><Setter.Value><ImageBrush ImageSource="Assets/DefaultWallpaper.png" Stretch="UniformToFill"/></Setter.Value></Setter>
                                        <Style.Triggers>
                                            <DataTrigger Binding="{Binding DesktopBackground}" Value="#1B2A4A"><Setter Property="Background" Value="#1B2A4A"/></DataTrigger>
                                            <DataTrigger Binding="{Binding DesktopBackground}" Value="#4A2D51"><Setter Property="Background" Value="#4A2D51"/></DataTrigger>
                                            <DataTrigger Binding="{Binding DesktopBackground}" Value="#204A46"><Setter Property="Background" Value="#204A46"/></DataTrigger>
                                        </Style.Triggers>
                                    </Style>
                                </Border.Style>
                                <Grid>
                                    <WrapPanel HorizontalAlignment="Center" VerticalAlignment="Center">
                                        <StackPanel Width="155" HorizontalAlignment="Center">
                                            <Image x:Name="DesktopSampleIcon" Width="{Binding DesktopIconSize}" Height="{Binding DesktopIconSize}" Source="{Binding Previews[3].Image}" />
                                            <TextBlock Foreground="White" Text="{DynamicResource Desktop.Projects}" HorizontalAlignment="Center" Margin="0,5,0,0" />
                                        </StackPanel>
                                        <StackPanel Width="155" HorizontalAlignment="Center">
                                            <Image Width="{Binding DesktopIconSize}" Height="{Binding DesktopIconSize}" Source="{Binding Previews[3].Image}" />
                                            <TextBlock Foreground="White" Text="{DynamicResource Desktop.Photos}" HorizontalAlignment="Center" Margin="0,5,0,0" />
                                        </StackPanel>
                                        <StackPanel Width="155" HorizontalAlignment="Center">
                                            <Image Width="{Binding DesktopIconSize}" Height="{Binding DesktopIconSize}" Source="{Binding Previews[3].Image}" />
                                            <TextBlock Foreground="White" Text="{DynamicResource Desktop.Notes}" HorizontalAlignment="Center" Margin="0,5,0,0" />
                                        </StackPanel>
                                    </WrapPanel>
                                    <Border Height="28" VerticalAlignment="Bottom" Background="#66000000">
                                        <TextBlock Foreground="White" VerticalAlignment="Center" HorizontalAlignment="Right" Margin="0,0,12,0" Text="◦ ◦ ◦" />
                                    </Border>
                                </Grid>
                            </Border>
                        </StackPanel></Expander>
                    </Border>
                    <Border Style="{StaticResource CardStyle}">
                        <Expander Header="{DynamicResource Section.Results}"><StackPanel>
                            <TextBlock Margin="0,0,0,8" FontWeight="SemiBold" Text="{Binding ResultSummary}" />
                            <ListView MinHeight="120" MaxHeight="260" ItemsSource="{Binding Results}" automation:AutomationProperties.Name="Application and restore results">
                                <ListView.View>
                                    <GridView>
                                        <GridViewColumn Width="80" Header="{DynamicResource Result.Status}" DisplayMemberBinding="{Binding StatusLabel}" />
                                        <GridViewColumn Width="180" Header="{DynamicResource Result.Path}" DisplayMemberBinding="{Binding Path}" />
                                        <GridViewColumn Width="240" Header="{DynamicResource Result.Detail}" DisplayMemberBinding="{Binding Detail}" />
                                    </GridView>
                                </ListView.View>
                            </ListView>
                            <WrapPanel Margin="0,10,0,0">
                                <Button Command="{Binding CopyFailedPathsCommand}" Content="{DynamicResource Button.CopyFailed}" automation:AutomationProperties.Name="Copy failed paths" />
                                <Button Command="{Binding ExportFailedPathsCommand}" Content="{DynamicResource Button.ExportFailure}" automation:AutomationProperties.Name="Export failure report" />
                            </WrapPanel>
                        </StackPanel></Expander>
                    </Border>
                </StackPanel>
            </Grid>
                </StackPanel>
                <StackPanel x:Name="RecoveryPanel" Visibility="Collapsed">
                    <Border Style="{StaticResource CardStyle}">
                        <StackPanel>
                            <TextBlock Text="{DynamicResource Home.Recovery}" FontSize="22" FontWeight="SemiBold"/>
                            <TextBlock Text="{DynamicResource Recovery.Description}" Foreground="{StaticResource MutedTextBrush}" TextWrapping="Wrap" Margin="0,12,0,22" LineHeight="23"/>
                            <Button Style="{StaticResource PrimaryButtonStyle}" Command="{Binding RestoreCommand}" Content="{DynamicResource Button.Restore}" HorizontalAlignment="Left"/>
                            <TextBlock Text="{Binding ProgressMessage}" TextWrapping="Wrap" Margin="0,12,0,0"/>
                            <TextBlock Text="{Binding OperationStatus}" TextWrapping="Wrap" FontWeight="SemiBold" Margin="0,8,0,0"/>
                            <TextBlock Text="{Binding ResultSummary}" Margin="0,12,0,0"/>
                            <ItemsControl ItemsSource="{Binding Results}" Margin="0,12,0,0">
                                <ItemsControl.ItemTemplate><DataTemplate><TextBlock Text="{Binding Path}" TextWrapping="Wrap" Margin="0,3"/></DataTemplate></ItemsControl.ItemTemplate>
                            </ItemsControl>
                        </StackPanel>
                    </Border>
                </StackPanel>
            </StackPanel>
        </ScrollViewer>
    </Grid>
</controls:RoundedWindow>
```

## src/FolderThemeStudio.App/MainWindow.xaml.cs

```csharp
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using FolderThemeStudio.App.Controls;
using FolderThemeStudio.App.DynamicIsland;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.ViewModels;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Themes;
using Image = System.Windows.Controls.Image;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App;

public partial class MainWindow : RoundedWindow
{
    private const double StackedLayoutWidth = 900;
    private readonly MainViewModel viewModel;
    private readonly IAppSettingsStore settingsStore;
    private readonly ILocalizationService localization;
    private readonly IStartupRegistrationService? startupRegistration;
    private readonly Action? backgrounded;
    private readonly IMonitoringRuleStore monitoringRuleStore;
    private readonly IFolderMonitoringCoordinator folderMonitoring;
    private readonly IMonitoringIconAssetService monitoringAssets;
    private readonly IMainViewModelDialogs ruleDialogs;
    private readonly IFolderCategoryCatalog folderCategoryCatalog;
    private readonly IFolderHoverMonitorService folderHoverMonitor;
    private readonly Action<double>? updateCardOpacity;
    private AppSettings currentSettings;
    private bool explicitExit;
    private bool isDraggingSticker;
    private FolderQuickNoteWindow? quickCategoryDialog;
    private FolderCategoriesViewModel? quickCategoryViewModel;
    private int quickNoteRequest;
    private readonly Action<FolderThemeStudio.App.Services.FolderEditChord>? updateEditChord;
    private IDynamicIslandController? islandController;

    public MainWindow(MainViewModel viewModel) : this(
        viewModel,
        new JsonAppSettingsStore(),
        new LocalizationService(AppSettings.Default.Language),
        AppSettings.Default with { CloseToTray = false },
        monitoringRuleStore: new JsonMonitoringRuleStore(),
        folderMonitoring: new NullFolderMonitoringCoordinator(),
        monitoringAssets: new MonitoringIconAssetService())
    {
    }

    public MainWindow(
        MainViewModel viewModel,
        IAppSettingsStore settingsStore,
        ILocalizationService localization,
        AppSettings currentSettings,
        IStartupRegistrationService? startupRegistration = null,
        Action? backgrounded = null,
        IMonitoringRuleStore? monitoringRuleStore = null,
        IFolderMonitoringCoordinator? folderMonitoring = null,
        IMonitoringIconAssetService? monitoringAssets = null,
        IMainViewModelDialogs? ruleDialogs = null,
        IFolderCategoryCatalog? folderCategoryCatalog = null,
        IFolderHoverMonitorService? folderHoverMonitor = null,
        Action<double>? updateCardOpacity = null,
        Action<FolderThemeStudio.App.Services.FolderEditChord>? updateEditChord = null,
        IDynamicIslandController? islandController = null)
    {
        InitializeComponent();
        this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        this.settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
        this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        this.currentSettings = currentSettings ?? throw new ArgumentNullException(nameof(currentSettings));
        this.startupRegistration = startupRegistration;
        this.backgrounded = backgrounded;
        this.monitoringRuleStore = monitoringRuleStore ?? new JsonMonitoringRuleStore();
        this.folderMonitoring = folderMonitoring ?? new NullFolderMonitoringCoordinator();
        this.monitoringAssets = monitoringAssets ?? new MonitoringIconAssetService();
        this.ruleDialogs = ruleDialogs ?? new DialogService(() => this);
        this.folderCategoryCatalog = folderCategoryCatalog ?? new JsonFolderCategoryCatalog();
        this.folderHoverMonitor = folderHoverMonitor ?? new NullFolderHoverMonitor();
        this.updateCardOpacity = updateCardOpacity;
        this.updateEditChord = updateEditChord;
        this.islandController = islandController;
        DataContext = viewModel;
        Closing += Window_Closing;
        Closed += (_, _) =>
        {
            this.folderHoverMonitor.Dispose();
            this.islandController?.Dispose();
            viewModel.Dispose();
        };
        UpdateResponsiveLayout(Width);
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e) =>
        UpdateResponsiveLayout(e.NewSize.Width);

    private void MainScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        for (var element = e.OriginalSource as DependencyObject; element is not null && !ReferenceEquals(element, MainScrollViewer);
             element = VisualTreeHelper.GetParent(element))
        {
            if (element is ScrollViewer) return;
        }
        MainScrollViewer.ScrollToVerticalOffset(MainWindowWheelPolicy.NextOffset(
            MainScrollViewer.VerticalOffset, e.Delta, SystemParameters.WheelScrollLines, 16));
        e.Handled = true;
    }

    public async Task ShowFirstRunTutorialAsync()
    {
        if (!currentSettings.TutorialCompleted) await OpenTutorialAsync();
    }

    private async void Tutorial_Click(object sender, RoutedEventArgs e) => await OpenTutorialAsync();

    public bool StartupEnabled => currentSettings.StartWithWindows;
    public async Task SetStartupEnabledAsync(bool enabled)
    {
        var previous = currentSettings.StartWithWindows;
        startupRegistration?.SetEnabled(enabled);
        try
        {
            var next = currentSettings with { StartWithWindows = enabled };
            await settingsStore.SaveAsync(next);
            currentSettings = next;
        }
        catch { startupRegistration?.SetEnabled(previous); throw; }
    }
    private async void DynamicIsland_Click(object sender, RoutedEventArgs e)
    {
        islandController ??= new DynamicIslandController(new IslandSettingsStore(), () => StartupEnabled,
            SetStartupEnabledAsync, () => { RequestExplicitExit(); System.Windows.Application.Current.Shutdown(); });
        try { await islandController.ShowAsync(); }
        catch (Exception ex) when (ex is System.IO.IOException or UnauthorizedAccessException)
        { ruleDialogs.ShowError("无法开启灵动岛", "请检查本地配置目录权限。"); }
    }

    private void Home_Click(object sender, RoutedEventArgs e) => ShowSection(HomePanel, "Home.Nav", "Home.Subtitle");
    private void Designer_Click(object sender, RoutedEventArgs e) => ShowSection(EditorWorkspace, "Home.Design", "Studio.Subtitle");
    private void Recovery_Click(object sender, RoutedEventArgs e) => ShowSection(RecoveryPanel, "Home.Recovery", "Home.RecoveryBody");

    private void ShowSection(FrameworkElement selected, string titleKey, string subtitleKey)
    {
        foreach (var section in new FrameworkElement[] { HomePanel, EditorWorkspace, RecoveryPanel })
            section.Visibility = ReferenceEquals(section, selected) ? Visibility.Visible : Visibility.Collapsed;
        PageTitle.SetResourceReference(TextBlock.TextProperty, titleKey);
        PageSubtitle.SetResourceReference(TextBlock.TextProperty, subtitleKey);
        HomeNavButton.Background = ReferenceEquals(selected, HomePanel) ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(222, 234, 250)) : System.Windows.Media.Brushes.Transparent;
        DesignNavButton.Background = ReferenceEquals(selected, EditorWorkspace) ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(222, 234, 250)) : System.Windows.Media.Brushes.Transparent;
        RecoveryNavButton.Background = ReferenceEquals(selected, RecoveryPanel) ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(222, 234, 250)) : System.Windows.Media.Brushes.Transparent;
        MainScrollViewer.ScrollToTop();
        if (SystemParameters.ClientAreaAnimation)
            selected.BeginAnimation(OpacityProperty, new DoubleAnimation(0.4, 1, TimeSpan.FromMilliseconds(140)) { FillBehavior = FillBehavior.Stop });
    }

    private void AppleColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (viewModel is null || e.AddedItems.Count == 0 || e.AddedItems[0] is not AppleColorPreset color) return;
        if (string.Equals(viewModel.GradientStart, color.Hex, StringComparison.OrdinalIgnoreCase)) return;
        viewModel.IsBuiltInEditor = true;
        viewModel.ApplyPalette(color.Palette);
    }

    private async void AutoStyleRules_Click(object sender, RoutedEventArgs e)
    {
        var viewModel = new AutoStyleRulesViewModel(
            monitoringRuleStore,
            folderMonitoring,
            monitoringAssets,
            ruleDialogs,
            localization,
            new WpfViewModelDispatcher(),
            this.viewModel.CurrentTheme);
        await viewModel.LoadAsync();
        var dialog = new AutoStyleRulesWindow(viewModel) { Owner = this };
        dialog.ShowDialog();
    }

    private async void FolderCategories_Click(object sender, RoutedEventArgs e)
    {
        var categories = new FolderCategoriesViewModel(
            folderCategoryCatalog,
            folderHoverMonitor,
            ruleDialogs,
            localization,
            new WpfViewModelDispatcher());
        await categories.LoadAsync();
        var dialog = new FolderCategoriesWindow(categories) { Owner = this };
        dialog.ShowDialog();
    }

    public async Task OpenFolderCategoryEditorAsync(string folderPath)
    {
        if (string.IsNullOrWhiteSpace(folderPath) || !System.IO.Directory.Exists(folderPath)) return;
        var request = ++quickNoteRequest;
        if (quickCategoryDialog is { IsVisible: true } && quickCategoryViewModel is not null)
        {
            if (quickCategoryViewModel.IsBusy) return;
            quickCategoryViewModel.SelectFolderForQuickNote(folderPath);
            quickCategoryDialog.Activate();
            quickCategoryDialog.FocusNote();
            return;
        }
        var categories = new FolderCategoriesViewModel(
            folderCategoryCatalog, folderHoverMonitor, ruleDialogs, localization, new WpfViewModelDispatcher());
        await categories.LoadAsync();
        if (request != quickNoteRequest) { categories.Dispose(); return; }
        categories.SelectFolderForQuickNote(folderPath);
        var dialog = new FolderQuickNoteWindow(categories);
        if (IsVisible) dialog.Owner = this;
        dialog.Closed += (_, _) =>
        {
            if (ReferenceEquals(quickCategoryDialog, dialog))
            {
                quickCategoryDialog = null;
                quickCategoryViewModel = null;
            }
        };
        quickCategoryDialog = dialog;
        quickCategoryViewModel = categories;
        dialog.Show();
        dialog.Activate();
    }

    public Task StartBackgroundServicesAsync(CancellationToken token = default) => folderHoverMonitor.StartAsync(token);

    private async void Settings_Click(object sender, RoutedEventArgs e)
    {
        var settingsViewModel = new SettingsViewModel(
            currentSettings,
            settingsStore,
            localization,
            viewModel.ApplyPalette,
            new FolderPreviewRenderer());
        var dialog = new SettingsWindow(settingsViewModel) { Owner = this };
        if (dialog.ShowDialog() == true)
        {
            currentSettings = await settingsStore.LoadAsync();
            updateCardOpacity?.Invoke(currentSettings.CardOpacity);
            updateEditChord?.Invoke(currentSettings.EditChord);
            startupRegistration?.SetEnabled(currentSettings.StartWithWindows);
        }
    }

    private async Task OpenTutorialAsync()
    {
        var tutorial = new TutorialViewModel(currentSettings, settingsStore, localization);
        var dialog = new TutorialWindow(tutorial) { Owner = this };
        dialog.ShowDialog();
        currentSettings = await settingsStore.LoadAsync();
    }

    public void ShowFromTray()
    {
        Show();
        WindowState = WindowState.Normal;
        ShowInTaskbar = true;
        Activate();
    }

    public void RequestExplicitExit()
    {
        explicitExit = true;
        Close();
    }

    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        if (explicitExit || !currentSettings.CloseToTray) return;
        e.Cancel = true;
        Hide();
        backgrounded?.Invoke();
    }

    private void StickerPreview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (viewModel.Sticker == StickerStyle.None || !viewModel.IsBuiltInEditor || sender is not Image image) return;
        isDraggingSticker = image.CaptureMouse();
        UpdateStickerPosition(image, e.GetPosition(image));
        e.Handled = true;
    }

    private void StickerPreview_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDraggingSticker && sender is Image image)
            UpdateStickerPosition(image, e.GetPosition(image));
    }

    private void StickerPreview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is Image image && isDraggingSticker)
        {
            UpdateStickerPosition(image, e.GetPosition(image));
            image.ReleaseMouseCapture();
            isDraggingSticker = false;
            e.Handled = true;
        }
    }

    private void StickerPreview_LostMouseCapture(object sender, MouseEventArgs e) => isDraggingSticker = false;

    private void UpdateStickerPosition(Image image, Point pointer)
    {
        if (image.ActualWidth <= 0 || image.ActualHeight <= 0) return;
        var face = new Rect(.09, .30, .82, .55);
        var position = new Point(
            (pointer.X / image.ActualWidth - face.Left) / face.Width,
            (pointer.Y / image.ActualHeight - face.Top) / face.Height);
        var clamped = StickerPainter.ClampPosition(position, viewModel.StickerScale, face);
        viewModel.StickerX = clamped.X;
        viewModel.StickerY = clamped.Y;
    }

    private void UpdateResponsiveLayout(double width)
    {
        if (SidebarColumn is null || EditorColumn is null) return;
        SidebarColumn.Width = new GridLength(width < 800 ? 156 : 196);
        var stacked = width - SidebarColumn.Width.Value < StackedLayoutWidth;
        HomeFeaturesGrid.Columns = width < 920 ? 1 : 2;
        HomeHeroIcon.Visibility = width < 880 ? Visibility.Collapsed : Visibility.Visible;
        ((Grid)HomeHeroIcon.Parent).ColumnDefinitions[1].Width = width < 880 ? new GridLength(0) : new GridLength(180);
        EditorColumn.Width = stacked ? new GridLength(1, GridUnitType.Star) : new GridLength(1.05, GridUnitType.Star);
        ApplicationColumn.Width = stacked ? new GridLength(0) : new GridLength(.95, GridUnitType.Star);
        TopRow.Height = GridLength.Auto;
        StackedRow.Height = stacked ? GridLength.Auto : new GridLength(0);

        Grid.SetColumn(EditorPanel, 0);
        Grid.SetRow(EditorPanel, 0);
        Grid.SetColumn(ApplicationPanel, stacked ? 0 : 1);
        Grid.SetRow(ApplicationPanel, stacked ? 1 : 0);
        EditorPanel.Margin = stacked ? new Thickness(0) : new Thickness(0, 0, 10, 0);
        ApplicationPanel.Margin = stacked ? new Thickness(0, 8, 0, 0) : new Thickness(10, 0, 0, 0);
    }
}
```

## src/FolderThemeStudio.App/MainWindowWheelPolicy.cs

```csharp
namespace FolderThemeStudio.App;

public static class MainWindowWheelPolicy
{
    public static double NextOffset(double current, int delta, int lines, double lineHeight) =>
        Math.Max(0, current - ((double)delta / 120 * Math.Max(1, lines) * lineHeight * 2));
}
```

## src/FolderThemeStudio.App/Monitoring/DirectoryWatcher.cs

```csharp
using System.IO;

namespace FolderThemeStudio.App.Monitoring;

public sealed record DirectoryCreatedEventArgs(string Path, bool WasRenamed = false);

public interface IDirectoryWatcher : IDisposable
{
    string RootPath { get; }
    bool IncludeSubdirectories { get; }
    event EventHandler<DirectoryCreatedEventArgs>? DirectoryCreated;
}

public interface IDirectoryWatcherFactory
{
    IDirectoryWatcher Create(string rootPath);
}

public sealed class DirectoryWatcherFactory : IDirectoryWatcherFactory
{
    public IDirectoryWatcher Create(string rootPath) => new DirectoryWatcher(rootPath);
}

internal sealed class DirectoryWatcher : IDirectoryWatcher
{
    private readonly FileSystemWatcher watcher;
    public DirectoryWatcher(string rootPath)
    {
        RootPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
        watcher = new FileSystemWatcher(RootPath)
        {
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.DirectoryName,
            EnableRaisingEvents = true
        };
        watcher.Created += Created;
        watcher.Renamed += Renamed;
    }

    public string RootPath { get; }
    public bool IncludeSubdirectories => watcher.IncludeSubdirectories;
    public event EventHandler<DirectoryCreatedEventArgs>? DirectoryCreated;
    private void Created(object sender, FileSystemEventArgs e) { if (Directory.Exists(e.FullPath)) DirectoryCreated?.Invoke(this, new(e.FullPath)); }
    private void Renamed(object sender, RenamedEventArgs e) { if (Directory.Exists(e.FullPath)) DirectoryCreated?.Invoke(this, new(e.FullPath, WasRenamed: true)); }
    public void Dispose()
    {
        watcher.EnableRaisingEvents = false;
        watcher.Created -= Created;
        watcher.Renamed -= Renamed;
        watcher.Dispose();
    }
}
```

## src/FolderThemeStudio.App/Monitoring/FolderMonitoringCoordinator.cs

```csharp
using System.IO;
using FolderThemeStudio.Core.SystemIntegration;

namespace FolderThemeStudio.App.Monitoring;

public sealed record MonitoringStatus(bool Paused, int TotalRules, int ActiveRules, int InactiveRules);
public sealed record MonitoringOutcome(bool Success, string Path, string? Error);

public interface IFolderMonitoringCoordinator : IDisposable
{
    MonitoringStatus Status { get; }
    event EventHandler<MonitoringOutcome>? OutcomeProduced;
    Task StartAsync(CancellationToken token = default);
    Task PauseAsync(CancellationToken token = default);
    Task ResumeAsync(CancellationToken token = default);
    Task ReplaceRuleAsync(MonitoringRule rule, CancellationToken token = default);
    Task ReplaceFallbackIconAsync(string rootPath, string icoPath, CancellationToken token = default);
    Task RemoveRuleAsync(string rootPath, CancellationToken token = default);
}

public sealed class FolderMonitoringCoordinator : IFolderMonitoringCoordinator
{
    private static readonly TimeSpan[] RetryDelays =
        [TimeSpan.FromMilliseconds(250), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3)];
    private readonly IMonitoringRuleStore store;
    private readonly IMonitoredFolderIconService applier;
    private readonly IDirectoryWatcherFactory watcherFactory;
    private readonly Func<TimeSpan, CancellationToken, Task> delay;
    private readonly Dictionary<string, MonitoringRule> rules = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, IDirectoryWatcher> watchers = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> pending = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Task> work = [];
    private readonly object sync = new();
    private bool paused;
    private bool disposed;

    public FolderMonitoringCoordinator(
        IMonitoringRuleStore store,
        IMonitoredFolderIconService applier,
        IDirectoryWatcherFactory? watcherFactory = null,
        Func<TimeSpan, CancellationToken, Task>? delay = null)
    {
        this.store = store;
        this.applier = applier;
        this.watcherFactory = watcherFactory ?? new DirectoryWatcherFactory();
        this.delay = delay ?? Task.Delay;
    }

    public MonitoringStatus Status => new(paused, rules.Count, watchers.Count, rules.Count - watchers.Count);
    public event EventHandler<MonitoringOutcome>? OutcomeProduced;

    public async Task StartAsync(CancellationToken token = default)
    {
        var loaded = await store.LoadAsync(token).ConfigureAwait(false);
        rules.Clear();
        foreach (var rule in loaded.Rules) rules[rule.RootPath] = rule;
        paused = loaded.Paused;
        RebuildWatchers();
    }

    public async Task PauseAsync(CancellationToken token = default)
    {
        paused = true;
        await store.SetPausedAsync(true, token).ConfigureAwait(false);
        ClearWatchers();
    }

    public async Task ResumeAsync(CancellationToken token = default)
    {
        paused = false;
        await store.SetPausedAsync(false, token).ConfigureAwait(false);
        RebuildWatchers();
    }

    public async Task ReplaceRuleAsync(MonitoringRule rule, CancellationToken token = default)
    {
        await store.UpsertAsync(rule, token).ConfigureAwait(false);
        rules[Path.TrimEndingDirectorySeparator(Path.GetFullPath(rule.RootPath))] = rule;
        RebuildWatchers();
    }

    public async Task ReplaceFallbackIconAsync(string rootPath, string icoPath, CancellationToken token = default)
    {
        var canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
        MonitoringRule? current = rules.GetValueOrDefault(canonical);
        if (current is null)
        {
            var loaded = await store.LoadAsync(token).ConfigureAwait(false);
            current = loaded.Rules.FirstOrDefault(item =>
                string.Equals(Path.TrimEndingDirectorySeparator(Path.GetFullPath(item.RootPath)), canonical, StringComparison.OrdinalIgnoreCase));
        }

        var replacement = current is null
            ? new MonitoringRule(1, canonical, icoPath, DateTimeOffset.UtcNow)
            : current with { IcoPath = icoPath, UpdatedAtUtc = DateTimeOffset.UtcNow };
        await ReplaceRuleAsync(replacement, token).ConfigureAwait(false);
    }

    public async Task RemoveRuleAsync(string rootPath, CancellationToken token = default)
    {
        var canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
        await store.RemoveAsync(canonical, token).ConfigureAwait(false);
        rules.Remove(canonical);
        RebuildWatchers();
    }

    public async Task WaitForIdleAsync()
    {
        while (true)
        {
            Task[] snapshot;
            lock (sync) snapshot = work.Where(task => !task.IsCompleted).ToArray();
            if (snapshot.Length == 0) return;
            await Task.WhenAll(snapshot).ConfigureAwait(false);
        }
    }

    private void RebuildWatchers()
    {
        ClearWatchers();
        if (paused || disposed) return;
        foreach (var rule in rules.Values)
        {
            if (!Directory.Exists(rule.RootPath)) continue;
            var watcher = watcherFactory.Create(rule.RootPath);
            watcher.DirectoryCreated += DirectoryCreated;
            watchers[rule.RootPath] = watcher;
        }
    }

    private void DirectoryCreated(object? sender, DirectoryCreatedEventArgs e)
    {
        if (sender is not IDirectoryWatcher watcher || !rules.TryGetValue(watcher.RootPath, out var rule)) return;
        string canonical;
        try { canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(e.Path)); }
        catch (Exception) { return; }
        lock (sync)
        {
            if (!pending.Add(canonical)) return;
            var task = ProcessAsync(canonical, rule, e.WasRenamed);
            work.Add(task);
        }
    }

    private async Task ProcessAsync(string path, MonitoringRule rule, bool wasRenamed)
    {
        try
        {
            await Task.Yield();
            var icoPath = wasRenamed ? rule.IcoPath : ResolveIcoPath(path, rule);
            MonitoredApplyOutcome? outcome = null;
            for (var attempt = 0; attempt < RetryDelays.Length; attempt++)
            {
                await delay(RetryDelays[attempt], CancellationToken.None).ConfigureAwait(false);
                outcome = await applier.ApplyAsync(path, icoPath, CancellationToken.None).ConfigureAwait(false);
                if (outcome.Success) break;
            }
            outcome ??= MonitoredApplyOutcome.Failure(path, "Monitoring did not run.");
            OutcomeProduced?.Invoke(this, new(outcome.Success, outcome.Path, outcome.Error));
        }
        finally { lock (sync) pending.Remove(path); }
    }

    private static string ResolveIcoPath(string folderPath, MonitoringRule rule)
    {
        var leaf = Path.GetFileName(Path.TrimEndingDirectorySeparator(Path.GetFullPath(folderPath)));
        return rule.NameRules
            .Where(item => item.Enabled)
            .OrderBy(item => item.Order)
            .ThenBy(item => item.Id, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(item => leaf.Contains(item.Keyword, StringComparison.OrdinalIgnoreCase))?.IcoPath
            ?? rule.IcoPath;
    }

    private void ClearWatchers()
    {
        foreach (var watcher in watchers.Values)
        {
            watcher.DirectoryCreated -= DirectoryCreated;
            watcher.Dispose();
        }
        watchers.Clear();
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        ClearWatchers();
    }
}
```

## src/FolderThemeStudio.App/Monitoring/JsonMonitoringRuleStore.cs

```csharp
using System.IO;
using System.Text.Json;

namespace FolderThemeStudio.App.Monitoring;

public sealed class JsonMonitoringRuleStore : IMonitoringRuleStore
{
    private const int SchemaVersion = 2;
    private const int LegacySchemaVersion = 1;
    private const int MonitoringRuleSchemaVersion = 1;
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    private readonly string path;
    private readonly SemaphoreSlim gate = new(1, 1);

    public JsonMonitoringRuleStore(string? path = null)
    {
        this.path = Path.GetFullPath(path ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio", "monitoring-rules.json"));
    }

    public async Task<MonitoringRuleLoadResult> LoadAsync(CancellationToken token = default)
    {
        await gate.WaitAsync(token).ConfigureAwait(false);
        try { return await LoadCoreAsync(token).ConfigureAwait(false); }
        finally { gate.Release(); }
    }

    public async Task UpsertAsync(MonitoringRule rule, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(rule);
        var normalized = Normalize(rule);
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var current = await LoadCoreAsync(token).ConfigureAwait(false);
            var rules = current.Rules
                .Where(item => !string.Equals(item.RootPath, normalized.RootPath, StringComparison.OrdinalIgnoreCase))
                .Append(normalized)
                .OrderBy(item => item.RootPath, StringComparer.OrdinalIgnoreCase)
                .ToArray();
            await SaveCoreAsync(rules, current.Paused, token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    public async Task RemoveAsync(string rootPath, CancellationToken token = default)
    {
        var canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var current = await LoadCoreAsync(token).ConfigureAwait(false);
            await SaveCoreAsync(
                current.Rules.Where(item => !string.Equals(item.RootPath, canonical, StringComparison.OrdinalIgnoreCase)).ToArray(),
                current.Paused,
                token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    public async Task SetPausedAsync(bool paused, CancellationToken token = default)
    {
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var current = await LoadCoreAsync(token).ConfigureAwait(false);
            await SaveCoreAsync(current.Rules, paused, token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    private async Task<MonitoringRuleLoadResult> LoadCoreAsync(CancellationToken token)
    {
        if (!File.Exists(path)) return new([], false, []);
        try
        {
            await using var stream = File.OpenRead(path);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: token).ConfigureAwait(false);
            var root = document.RootElement;
            var paused = root.TryGetProperty("paused", out var pausedElement) && pausedElement.ValueKind == JsonValueKind.True;
            var rules = new List<MonitoringRule>();
            var diagnostics = new List<string>();
            if (!root.TryGetProperty("schemaVersion", out var version) ||
                version.GetInt32() is not (LegacySchemaVersion or SchemaVersion))
                return new([], paused, ["Unsupported monitoring rule document version."]);
            if (!root.TryGetProperty("rules", out var entries) || entries.ValueKind != JsonValueKind.Array)
                return new([], paused, ["Monitoring rule list is missing."]);

            var index = 0;
            foreach (var entry in entries.EnumerateArray())
            {
                try
                {
                    var rule = JsonSerializer.Deserialize<MonitoringRule>(entry.GetRawText(), Options)
                        ?? throw new InvalidDataException("Rule is empty.");
                    rules.Add(Normalize(rule));
                }
                catch (Exception exception) when (exception is JsonException or InvalidDataException or ArgumentException or FormatException)
                {
                    diagnostics.Add($"Rule {index + 1}: {exception.Message}");
                }
                index++;
            }
            return new(rules, paused, diagnostics);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or JsonException)
        {
            return new([], false, [exception.Message]);
        }
    }

    private async Task SaveCoreAsync(IReadOnlyList<MonitoringRule> rules, bool paused, CancellationToken token)
    {
        var directory = Path.GetDirectoryName(path) ?? throw new InvalidOperationException("Rules path has no parent.");
        Directory.CreateDirectory(directory);
        var temporary = path + ".tmp";
        try
        {
            await using (var stream = new FileStream(temporary, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                await JsonSerializer.SerializeAsync(stream, new RuleDocument(SchemaVersion, paused, rules), Options, token).ConfigureAwait(false);
                await stream.FlushAsync(token).ConfigureAwait(false);
            }
            File.Move(temporary, path, overwrite: true);
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }

    private static MonitoringRule Normalize(MonitoringRule rule)
    {
        if (rule.SchemaVersion != MonitoringRuleSchemaVersion) throw new InvalidDataException("Unsupported monitoring rule version.");
        if (string.IsNullOrWhiteSpace(rule.RootPath) || string.IsNullOrWhiteSpace(rule.IcoPath))
            throw new InvalidDataException("Monitoring paths are required.");
        if (rule.UpdatedAtUtc == default) throw new InvalidDataException("Monitoring update time is required.");
        return rule with
        {
            RootPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rule.RootPath)),
            IcoPath = Path.GetFullPath(rule.IcoPath),
            UpdatedAtUtc = rule.UpdatedAtUtc.ToUniversalTime(),
            NameRules = rule.NameRules.Select(Normalize)
                .OrderBy(item => item.Order)
                .ThenBy(item => item.Id, StringComparer.OrdinalIgnoreCase)
                .ToArray()
        };
    }

    private static FolderNameStyleRule Normalize(FolderNameStyleRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        if (string.IsNullOrWhiteSpace(rule.Id) || string.IsNullOrWhiteSpace(rule.Keyword) || string.IsNullOrWhiteSpace(rule.IcoPath))
            throw new InvalidDataException("Name style rule ID, keyword, and ICO path are required.");
        if (rule.Order < 0)
            throw new InvalidDataException("Name style rule order cannot be negative.");
        return rule with
        {
            Id = rule.Id.Trim(),
            Keyword = rule.Keyword.Trim(),
            IcoPath = Path.GetFullPath(rule.IcoPath)
        };
    }

    private sealed record RuleDocument(int SchemaVersion, bool Paused, IReadOnlyList<MonitoringRule> Rules);
}
```

## src/FolderThemeStudio.App/Monitoring/MonitoringIconAssetService.cs

```csharp
using System.IO;
using System.Security.Cryptography;

namespace FolderThemeStudio.App.Monitoring;

public sealed class MonitoringIconAssetService : IMonitoringIconAssetService
{
    private readonly string root;

    public MonitoringIconAssetService(string? root = null)
    {
        this.root = Path.GetFullPath(root ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio", "monitoring-icons"));
    }

    public async Task<string> PersistAsync(string generatedIcoPath, CancellationToken token)
    {
        var source = Path.GetFullPath(generatedIcoPath);
        var bytes = await File.ReadAllBytesAsync(source, token).ConfigureAwait(false);
        var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        Directory.CreateDirectory(root);
        var destination = Path.Combine(root, hash + ".ico");
        if (File.Exists(destination)) return destination;
        var temporary = destination + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            await File.WriteAllBytesAsync(temporary, bytes, token).ConfigureAwait(false);
            try { File.Move(temporary, destination); }
            catch (IOException) when (File.Exists(destination)) { }
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
        return destination;
    }
}
```

## src/FolderThemeStudio.App/Monitoring/MonitoringRule.cs

```csharp
using System.IO;

namespace FolderThemeStudio.App.Monitoring;

public sealed record MonitoringRule(
    int SchemaVersion,
    string RootPath,
    string IcoPath,
    DateTimeOffset UpdatedAtUtc)
{
    public IReadOnlyList<FolderNameStyleRule> NameRules { get; init; } = [];
}

public sealed record FolderNameStyleRule(
    string Id,
    string Keyword,
    string IcoPath,
    bool Enabled,
    int Order);

public sealed record MonitoringRuleLoadResult(
    IReadOnlyList<MonitoringRule> Rules,
    bool Paused,
    IReadOnlyList<string> Diagnostics);

public interface IMonitoringRuleStore
{
    Task<MonitoringRuleLoadResult> LoadAsync(CancellationToken token = default);
    Task UpsertAsync(MonitoringRule rule, CancellationToken token = default);
    Task RemoveAsync(string rootPath, CancellationToken token = default);
    Task SetPausedAsync(bool paused, CancellationToken token = default);
}

public interface IMonitoringIconAssetService
{
    Task<string> PersistAsync(string generatedIcoPath, CancellationToken token);
}

public sealed class NullFolderMonitoringCoordinator : IFolderMonitoringCoordinator
{
    public MonitoringStatus Status => new(false, 0, 0, 0);
    public event EventHandler<MonitoringOutcome>? OutcomeProduced { add { } remove { } }
    public Task StartAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task PauseAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task ResumeAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task ReplaceRuleAsync(MonitoringRule rule, CancellationToken token = default) => Task.CompletedTask;
    public Task ReplaceFallbackIconAsync(string rootPath, string icoPath, CancellationToken token = default) => Task.CompletedTask;
    public Task RemoveRuleAsync(string rootPath, CancellationToken token = default) => Task.CompletedTask;
    public void Dispose() { }
}

public sealed class PassthroughMonitoringIconAssetService : IMonitoringIconAssetService
{
    public Task<string> PersistAsync(string generatedIcoPath, CancellationToken token) =>
        Task.FromResult(Path.GetFullPath(generatedIcoPath));
}
```

## src/FolderThemeStudio.App/Services/DialogService.cs

```csharp
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
```

## src/FolderThemeStudio.App/Services/ExplorerFolderHoverTargetResolver.cs

```csharp
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public sealed record ExplorerHoverTarget(string FolderPath, Rect ItemBounds, nint RootWindowHandle = default);

public enum HoverSurface { ExplorerList, DesktopList, Other }

public sealed record HoverListItemCandidate(
    string Name,
    int ProcessId,
    nint RootWindowHandle,
    Rect Bounds,
    string ProcessName,
    bool IsFileListItem,
    HoverSurface Surface = HoverSurface.ExplorerList);

public interface IHoverListItemProbe
{
    HoverListItemCandidate? FindListItem(Point screenPoint);
}

public interface IExplorerWindowPathResolver
{
    string? GetCurrentFolderPath(nint rootWindowHandle);
}

public sealed class ExplorerFolderHoverTargetResolver : IExplorerFolderHoverTargetResolver
{
    private readonly IHoverListItemProbe itemProbe;
    private readonly IExplorerWindowPathResolver windowPathResolver;
    private readonly Func<string, bool> directoryExists;
    private readonly DesktopFolderPathResolver desktopPathResolver;

    public ExplorerFolderHoverTargetResolver(
        IHoverListItemProbe? itemProbe = null,
        IExplorerWindowPathResolver? windowPathResolver = null,
        Func<string, bool>? directoryExists = null,
        DesktopFolderPathResolver? desktopPathResolver = null)
    {
        this.itemProbe = itemProbe ?? new WindowsHoverListItemProbe();
        this.windowPathResolver = windowPathResolver ?? new ShellExplorerWindowPathResolver();
        this.directoryExists = directoryExists ?? Directory.Exists;
        this.desktopPathResolver = desktopPathResolver ?? new DesktopFolderPathResolver();
    }

    public ExplorerHoverTarget? TryResolve(Point screenPoint)
    {
        if (!double.IsFinite(screenPoint.X) || !double.IsFinite(screenPoint.Y)) return null;
        try
        {
            var item = itemProbe.FindListItem(screenPoint);
            if (item is null || !item.IsFileListItem ||
                !string.Equals(item.ProcessName, "explorer", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(item.Name) || item.Bounds.IsEmpty || !item.Bounds.Contains(screenPoint))
                return null;

            if (item.Surface == HoverSurface.DesktopList)
            {
                var desktopPath = desktopPathResolver.Resolve(item.Name);
                return desktopPath is null ? null : new ExplorerHoverTarget(desktopPath, item.Bounds, item.RootWindowHandle);
            }
            if (item.Surface != HoverSurface.ExplorerList) return null;
            var parentPath = windowPathResolver.GetCurrentFolderPath(item.RootWindowHandle);
            if (string.IsNullOrWhiteSpace(parentPath) || !Path.IsPathFullyQualified(parentPath)) return null;
            var leafName = item.Name.Trim();
            if (leafName is "." or ".." || Path.IsPathRooted(leafName) ||
                !string.Equals(Path.GetFileName(leafName), leafName, StringComparison.Ordinal))
                return null;

            var folderPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(Path.Combine(parentPath, leafName)));
            return directoryExists(folderPath) ? new ExplorerHoverTarget(folderPath, item.Bounds, item.RootWindowHandle) : null;
        }
        catch (Exception exception) when (exception is ArgumentException or IOException or UnauthorizedAccessException
            or InvalidOperationException or COMException or ElementNotAvailableException)
        {
            return null;
        }
    }
}

internal sealed class WindowsHoverListItemProbe : IHoverListItemProbe
{
    public HoverListItemCandidate? FindListItem(Point screenPoint)
    {
        var element = AutomationElement.FromPoint(screenPoint);
        var walker = TreeWalker.ControlViewWalker;
        for (var depth = 0; element is not null && depth < 8; depth++)
        {
            var current = element.Current;
            if (current.ControlType == ControlType.ListItem || current.ControlType == ControlType.DataItem)
            {
                if (current.BoundingRectangle.IsEmpty) return null;
                var isFileListItem = IsWithinListControl(element, walker);
                var hwnd = NativeMethods.GetAncestor(NativeMethods.WindowFromPoint(
                    new NativeMethods.NativePoint((int)screenPoint.X, (int)screenPoint.Y)), NativeMethods.GetAncestorRoot);
                if (hwnd == nint.Zero) return null;
                var rootClass = GetWindowClass(hwnd);
                var surface = rootClass is "Progman" or "WorkerW" ? HoverSurface.DesktopList : HoverSurface.ExplorerList;
                string processName;
                try
                {
                    using var process = Process.GetProcessById(current.ProcessId);
                    processName = process.ProcessName;
                }
                catch (Exception exception) when (exception is ArgumentException or System.ComponentModel.Win32Exception)
                {
                    return null;
                }
                return new HoverListItemCandidate(
                    current.Name,
                    current.ProcessId,
                    hwnd,
                    current.BoundingRectangle,
                    processName,
                    isFileListItem,
                    surface);
            }
            element = walker.GetParent(element);
        }
        return null;
    }

    private static bool IsWithinListControl(AutomationElement item, TreeWalker walker)
    {
        var parent = walker.GetParent(item);
        for (var depth = 0; parent is not null && depth < 6; depth++)
        {
            var type = parent.Current.ControlType;
            if (type == ControlType.List) return true;
            if (type == ControlType.Tree || type == ControlType.TreeItem || type == ControlType.Pane) return false;
            parent = walker.GetParent(parent);
        }
        return false;
    }

    private static string GetWindowClass(nint hwnd)
    {
        var result = new StringBuilder(256);
        return NativeMethods.GetClassName(hwnd, result, result.Capacity) > 0 ? result.ToString() : string.Empty;
    }

    private static class NativeMethods
    {
        internal const uint GetAncestorRoot = 2;

        [StructLayout(LayoutKind.Sequential)]
        internal readonly struct NativePoint(int x, int y)
        {
            internal readonly int X = x;
            internal readonly int Y = y;
        }

        [DllImport("user32.dll")]
        internal static extern nint WindowFromPoint(NativePoint point);

        [DllImport("user32.dll")]
        internal static extern nint GetAncestor(nint hwnd, uint flags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int GetClassName(nint hwnd, StringBuilder className, int maxCount);
    }
}

public sealed class DesktopFolderPathResolver
{
    private readonly string userDesktop;
    private readonly string publicDesktop;
    private readonly Func<string, bool> directoryExists;

    public DesktopFolderPathResolver(string? userDesktop = null, string? publicDesktop = null, Func<string, bool>? directoryExists = null)
    {
        this.userDesktop = userDesktop ?? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        this.publicDesktop = publicDesktop ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        this.directoryExists = directoryExists ?? Directory.Exists;
    }

    public string? Resolve(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName)) return null;
        var name = itemName.Trim();
        if (name is "." or ".." || Path.IsPathRooted(name) || !string.Equals(Path.GetFileName(name), name, StringComparison.Ordinal))
            return null;
        var matches = new[] { userDesktop, publicDesktop }
            .Where(root => !string.IsNullOrWhiteSpace(root))
            .Select(root => Path.GetFullPath(Path.Combine(root, name)))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(directoryExists)
            .Take(2)
            .ToArray();
        return matches.Length == 1 ? matches[0] : null;
    }
}

internal sealed class ShellExplorerWindowPathResolver : IExplorerWindowPathResolver
{
    private const BindingFlags InvokeFlags = BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;

    public string? GetCurrentFolderPath(nint rootWindowHandle)
    {
        var shellType = Type.GetTypeFromProgID("Shell.Application");
        if (shellType is null) return null;
        var shell = Activator.CreateInstance(shellType);
        if (shell is null) return null;
        object? windows = null;
        try
        {
            windows = Invoke(shell, "Windows");
            if (windows is null) return null;
            var count = Convert.ToInt32(Read(windows, "Count"), System.Globalization.CultureInfo.InvariantCulture);
            for (var index = 0; index < count; index++)
            {
                object? browser = null;
                object? document = null;
                object? folder = null;
                object? self = null;
                try
                {
                    browser = Invoke(windows, "Item", index);
                    if (browser is null || Convert.ToInt64(Read(browser, "HWND"), System.Globalization.CultureInfo.InvariantCulture) != rootWindowHandle.ToInt64())
                        continue;
                    document = Read(browser, "Document");
                    if (document is null) return null;
                    folder = Read(document, "Folder");
                    if (folder is null) return null;
                    self = Read(folder, "Self");
                    return Convert.ToString(Read(self, "Path"), System.Globalization.CultureInfo.InvariantCulture);
                }
                finally
                {
                    Release(self);
                    Release(folder);
                    Release(document);
                    Release(browser);
                }
            }
            return null;
        }
        finally
        {
            Release(windows);
            Release(shell);
        }
    }

    private static object? Invoke(object target, string member, params object[] arguments) =>
        target.GetType().InvokeMember(member, InvokeFlags, null, target, arguments);

    private static object? Read(object? target, string member) => target is null ? null : Invoke(target, member);

    private static void Release(object? value)
    {
        if (value is not null && Marshal.IsComObject(value)) Marshal.FinalReleaseComObject(value);
    }
}
```

## src/FolderThemeStudio.App/Services/ExplorerInfoTipSuppressor.cs

```csharp
using System.Runtime.InteropServices;
using System.Text;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public interface IExplorerInfoTipPlatform
{
    IDisposable WatchShown(Action<nint> onShown);
    IEnumerable<nint> FindInfoTips(nint root);
    bool IsPointerOnTarget(ExplorerHoverTarget target);
    bool BelongsToFolderView(nint infoTip, nint root);
    void Dismiss(nint infoTip);
}

// Dismiss only the current folder's transient tip; never change Explorer's persistent settings.
public sealed class ExplorerInfoTipSuppressor : IDisposable
{
    private readonly IExplorerInfoTipPlatform platform;
    private readonly IDisposable subscription;
    private ExplorerHoverTarget? target;
    private bool disposed;

    public ExplorerInfoTipSuppressor(IExplorerInfoTipPlatform? platform = null)
    {
        this.platform = platform ?? new WindowsExplorerInfoTipPlatform();
        subscription = this.platform.WatchShown(TryDismiss);
    }

    public void SetTarget(ExplorerHoverTarget? next)
    {
        if (disposed) return;
        target = next;
        if (next is null || next.RootWindowHandle == nint.Zero) return;
        foreach (var tip in platform.FindInfoTips(next.RootWindowHandle)) TryDismiss(tip);
    }

    private void TryDismiss(nint tip)
    {
        var current = target;
        if (!disposed && current is not null && current.RootWindowHandle != nint.Zero &&
            platform.IsPointerOnTarget(current) && platform.BelongsToFolderView(tip, current.RootWindowHandle))
            platform.Dismiss(tip);
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        target = null;
        subscription.Dispose();
    }
}

internal sealed class WindowsExplorerInfoTipPlatform : IExplorerInfoTipPlatform
{
    public IDisposable WatchShown(Action<nint> onShown) => new ShowSubscription(onShown);

    public IEnumerable<nint> FindInfoTips(nint root)
    {
        var result = new HashSet<nint>();
        var thread = Native.GetWindowThreadProcessId(root, out _);
        if (thread == 0) return result;
        Native.EnumProc collect = (window, _) => { if (ClassName(window) == "tooltips_class32") result.Add(window); return true; };
        Native.EnumThreadWindows(thread, collect, nint.Zero);
        Native.EnumChildWindows(root, collect, nint.Zero);
        return result;
    }

    public bool IsPointerOnTarget(ExplorerHoverTarget target) =>
        Native.GetCursorPos(out var point) && target.ItemBounds.Contains(new Point(point.X, point.Y)) &&
        Native.GetAncestor(Native.WindowFromPoint(point), 2) == target.RootWindowHandle;

    public bool BelongsToFolderView(nint tip, nint root)
    {
        if (ClassName(tip) != "tooltips_class32" ||
            ClassName(root) is not ("CabinetWClass" or "ExploreWClass" or "Progman" or "WorkerW")) return false;
        Native.GetWindowThreadProcessId(root, out var rootProcess);
        Native.GetWindowThreadProcessId(tip, out var tipProcess);
        if (rootProcess == 0 || tipProcess != rootProcess) return false;
        // Tooltip owners are normally the shell list view or its containing view window.
        var owner = Native.GetWindow(tip, 4);
        if (owner == nint.Zero) owner = Native.GetParent(tip);
        return owner != nint.Zero && (owner == root || Native.GetAncestor(owner, 2) == root);
    }

    public void Dismiss(nint tip)
    {
        // TTM_POP has no pointer parameters and is safe across process boundaries.
        Native.SendMessageTimeout(tip, 0x0400 + 28, nint.Zero, nint.Zero, 0x0002 | 0x0020, 30, out _);
    }

    private static string ClassName(nint window)
    {
        var name = new StringBuilder(128);
        return Native.GetClassName(window, name, name.Capacity) > 0 ? name.ToString() : string.Empty;
    }

    private sealed class ShowSubscription : IDisposable
    {
        private readonly Native.WinEventProc callback;
        private GCHandle callbackRoot;
        private nint hook;
        public ShowSubscription(Action<nint> onShown)
        {
            callback = (_, _, window, objectId, childId, _, _) =>
            {
                if (objectId != 0 || childId != 0 || window == nint.Zero) return;
                try { onShown(window); } catch (Exception) { }
            };
            callbackRoot = GCHandle.Alloc(callback);
            hook = Native.SetWinEventHook(0x8002, 0x8002, nint.Zero, callback, 0, 0, 2);
            if (hook == nint.Zero) callbackRoot.Free();
        }
        public void Dispose()
        {
            if (hook == nint.Zero) return;
            Native.UnhookWinEvent(hook);
            hook = nint.Zero;
            if (callbackRoot.IsAllocated) callbackRoot.Free();
        }
    }

    private static class Native
    {
        internal delegate bool EnumProc(nint window, nint param);
        internal delegate void WinEventProc(nint hook, uint eventType, nint window, int objectId, int childId, uint thread, uint time);
        [StructLayout(LayoutKind.Sequential)] internal struct NativePoint { internal int X; internal int Y; }
        [DllImport("user32.dll")] internal static extern nint SetWinEventHook(uint min, uint max, nint module, WinEventProc callback, uint process, uint thread, uint flags);
        [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool UnhookWinEvent(nint hook);
        [DllImport("user32.dll")] internal static extern uint GetWindowThreadProcessId(nint window, out uint process);
        [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool EnumThreadWindows(uint thread, EnumProc callback, nint param);
        [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool EnumChildWindows(nint parent, EnumProc callback, nint param);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] internal static extern int GetClassName(nint window, StringBuilder name, int count);
        [DllImport("user32.dll")] internal static extern nint GetWindow(nint window, uint command);
        [DllImport("user32.dll")] internal static extern nint GetParent(nint window);
        [DllImport("user32.dll")] internal static extern nint GetAncestor(nint window, uint flags);
        [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool GetCursorPos(out NativePoint point);
        [DllImport("user32.dll")] internal static extern nint WindowFromPoint(NativePoint point);
        [DllImport("user32.dll", EntryPoint = "SendMessageTimeoutW")] internal static extern nint SendMessageTimeout(nint window, uint message, nint wParam, nint lParam, uint flags, uint timeout, out nuint result);
    }
}
```

## src/FolderThemeStudio.App/Services/FolderCategoryCatalog.cs

```csharp
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FolderThemeStudio.App.Services;

[method: JsonConstructor]
public sealed record FolderCategoryEntry(string FolderPath, string Category, string Note, string GradientStartHex, string GradientEndHex)
{
    public FolderCategoryEntry(string folderPath, string category, string note, string colorHex)
        : this(folderPath, category, note, colorHex, colorHex) { }

    [JsonIgnore]
    public string ColorHex => GradientStartHex;
}

public interface IFolderCategoryCatalog
{
    Task<IReadOnlyList<FolderCategoryEntry>> LoadAsync(CancellationToken token = default);
    Task UpsertAsync(FolderCategoryEntry entry, CancellationToken token = default);
    Task RemoveAsync(string folderPath, CancellationToken token = default);
}

public sealed class JsonFolderCategoryCatalog : IFolderCategoryCatalog
{
    private const int SchemaVersion = 2;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };
    private readonly string path;
    private readonly SemaphoreSlim gate = new(1, 1);

    public JsonFolderCategoryCatalog(string? path = null)
    {
        this.path = Path.GetFullPath(path ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio", "folder-categories.json"));
    }

    public async Task<IReadOnlyList<FolderCategoryEntry>> LoadAsync(CancellationToken token = default)
    {
        await gate.WaitAsync(token).ConfigureAwait(false);
        try { return await LoadCoreAsync(token).ConfigureAwait(false); }
        finally { gate.Release(); }
    }

    public async Task UpsertAsync(FolderCategoryEntry entry, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entry);
        var normalized = Normalize(entry, requireDirectory: true);
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var entries = (await LoadCoreAsync(token).ConfigureAwait(false)).ToList();
            var index = entries.FindIndex(item => PathEquals(item.FolderPath, normalized.FolderPath));
            if (index < 0) entries.Add(normalized);
            else entries[index] = normalized;
            await SaveCoreAsync(entries, token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    public async Task RemoveAsync(string folderPath, CancellationToken token = default)
    {
        var canonicalPath = CanonicalPath(folderPath);
        await gate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            var entries = (await LoadCoreAsync(token).ConfigureAwait(false))
                .Where(item => !PathEquals(item.FolderPath, canonicalPath))
                .ToList();
            await SaveCoreAsync(entries, token).ConfigureAwait(false);
        }
        finally { gate.Release(); }
    }

    private async Task<IReadOnlyList<FolderCategoryEntry>> LoadCoreAsync(CancellationToken token)
    {
        if (!File.Exists(path)) return [];
        try
        {
            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete, 4096, FileOptions.Asynchronous);
            using var json = await JsonDocument.ParseAsync(stream, cancellationToken: token).ConfigureAwait(false);
            if (!json.RootElement.TryGetProperty("schemaVersion", out var versionElement))
                throw new InvalidDataException("The folder category catalog version is missing.");
            var version = versionElement.GetInt32();
            IReadOnlyList<FolderCategoryEntry> entries;
            if (version == 1)
            {
                var old = json.RootElement.Deserialize<LegacyCatalogDocument>(JsonOptions);
                if (old?.Entries is null) throw new InvalidDataException("The folder category catalog contents are invalid.");
                entries = old.Entries.Select(entry => new FolderCategoryEntry(entry.FolderPath, entry.Category, entry.Note, entry.ColorHex)).ToArray();
            }
            else if (version == SchemaVersion)
            {
                entries = json.RootElement.Deserialize<CatalogDocument>(JsonOptions)?.Entries
                    ?? throw new InvalidDataException("The folder category catalog contents are invalid.");
            }
            else throw new InvalidDataException("The folder category catalog version is unsupported.");

            var normalized = entries.Select(entry => Normalize(entry, requireDirectory: false)).ToArray();
            if (normalized.Select(item => item.FolderPath).Distinct(StringComparer.OrdinalIgnoreCase).Count() != normalized.Length)
                throw new InvalidDataException("The folder category catalog contains duplicate folder paths.");
            return normalized.OrderBy(item => item.FolderPath, StringComparer.OrdinalIgnoreCase).ToArray();
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException("The folder category catalog is malformed.", exception);
        }
    }

    private async Task SaveCoreAsync(IReadOnlyList<FolderCategoryEntry> entries, CancellationToken token)
    {
        var directory = Path.GetDirectoryName(path)!;
        Directory.CreateDirectory(directory);
        var temporaryPath = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            await using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                var document = new CatalogDocument(SchemaVersion, entries.OrderBy(item => item.FolderPath, StringComparer.OrdinalIgnoreCase).ToArray());
                await JsonSerializer.SerializeAsync(stream, document, JsonOptions, token).ConfigureAwait(false);
                await stream.FlushAsync(token).ConfigureAwait(false);
            }
            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath)) File.Delete(temporaryPath);
        }
    }

    private static FolderCategoryEntry Normalize(FolderCategoryEntry entry, bool requireDirectory)
    {
        ArgumentNullException.ThrowIfNull(entry);
        var canonicalPath = CanonicalPath(entry.FolderPath);
        var category = entry.Category?.Trim() ?? string.Empty;
        var note = entry.Note?.Trim() ?? string.Empty;
        var start = entry.GradientStartHex?.Trim() ?? string.Empty;
        var end = entry.GradientEndHex?.Trim() ?? string.Empty;
        if (requireDirectory && !Directory.Exists(canonicalPath))
            throw new DirectoryNotFoundException("Choose an existing folder for its category.");
        if (category.Length is < 1 or > 32)
            throw new ArgumentException("Category must contain between 1 and 32 characters.", nameof(entry));
        if (note.Length > 140)
            throw new ArgumentException("The category note cannot exceed 140 characters.", nameof(entry));
        if (!IsHex(start) || !IsHex(end))
            throw new ArgumentException("The accent must be a six-digit hexadecimal color such as #2868D7.", nameof(entry));
        return new FolderCategoryEntry(canonicalPath, category, note, start.ToUpperInvariant(), end.ToUpperInvariant());
    }

    private static bool IsHex(string color) => color.Length == 7 && color[0] == '#' && color.AsSpan(1).ToString().All(Uri.IsHexDigit);

    private static string CanonicalPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("A folder path is required.", nameof(path));
        return Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
    }

    private static bool PathEquals(string left, string right) =>
        string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

    private sealed record CatalogDocument(int SchemaVersion, IReadOnlyList<FolderCategoryEntry> Entries);
    private sealed record LegacyCatalogDocument(int SchemaVersion, IReadOnlyList<LegacyEntry> Entries);
    private sealed record LegacyEntry(string FolderPath, string Category, string Note, string ColorHex);
}
```

## src/FolderThemeStudio.App/Services/FolderEditChord.cs

```csharp
using System.Windows;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public enum FolderMouseButton { Right, Middle }

public sealed record FolderEditChord(ModifierKeys Modifiers, FolderMouseButton Button)
{
    private const ModifierKeys Allowed = ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Windows;
    public static FolderEditChord Default { get; } = new(ModifierKeys.Alt, FolderMouseButton.Right);

    public static bool TryNormalize(FolderEditChord? chord, out FolderEditChord normalized)
    {
        normalized = Default;
        if (chord is null || chord.Modifiers == ModifierKeys.None || (chord.Modifiers & ~Allowed) != 0 ||
            chord.Button is not (FolderMouseButton.Right or FolderMouseButton.Middle)) return false;
        normalized = chord;
        return true;
    }

    public override string ToString()
    {
        var parts = new List<string>();
        if (Modifiers.HasFlag(ModifierKeys.Control)) parts.Add("Ctrl");
        if (Modifiers.HasFlag(ModifierKeys.Alt)) parts.Add("Alt");
        if (Modifiers.HasFlag(ModifierKeys.Shift)) parts.Add("Shift");
        if (Modifiers.HasFlag(ModifierKeys.Windows)) parts.Add("Win");
        parts.Add(Button == FolderMouseButton.Right ? "Right Click" : "Middle Click");
        return string.Join(" + ", parts);
    }
}

public sealed class FolderQuickEditDecision(FolderEditChord chord)
{
    private FolderMouseButton? consumedButton;
    private FolderEditChord chord = chord;

    public void UpdateChord(FolderEditChord value) => chord = FolderEditChord.TryNormalize(value, out var normalized)
        ? normalized : FolderEditChord.Default;

    public string? OnMouseDown(FolderMouseButton button, ModifierKeys modifiers, Point point, ExplorerHoverTarget? target)
    {
        if (button != chord.Button || modifiers != chord.Modifiers || target is null || !target.ItemBounds.Contains(point)) return null;
        consumedButton = button;
        return target.FolderPath;
    }

    public bool OnMouseUp(FolderMouseButton button)
    {
        if (consumedButton != button) return false;
        consumedButton = null;
        return true;
    }

    public bool TryBeginDeferredEdit(FolderMouseButton button, ModifierKeys modifiers)
    {
        if (button != chord.Button || modifiers != chord.Modifiers) return false;
        consumedButton = button;
        return true;
    }
}
```

## src/FolderThemeStudio.App/Services/FolderHoverDecision.cs

```csharp
using System.Windows;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public interface IExplorerFolderHoverTargetResolver
{
    ExplorerHoverTarget? TryResolve(Point screenPoint);
}

public sealed class FolderHoverDecision
{
    private readonly TimeSpan dwell;
    private string? candidatePath;
    private DateTimeOffset candidateSince;

    public FolderHoverDecision(TimeSpan? dwell = null)
    {
        this.dwell = dwell ?? TimeSpan.FromMilliseconds(550);
        if (this.dwell < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(dwell));
    }

    public FolderCategoryEntry? Update(
        ExplorerHoverTarget? target,
        Point screenPoint,
        IReadOnlyDictionary<string, FolderCategoryEntry> entries,
        DateTimeOffset now)
    {
        if (target is null || !target.ItemBounds.Contains(screenPoint) ||
            !entries.TryGetValue(target.FolderPath, out var entry))
        {
            Reset();
            return null;
        }

        if (!string.Equals(candidatePath, target.FolderPath, StringComparison.OrdinalIgnoreCase))
        {
            candidatePath = target.FolderPath;
            candidateSince = now;
        }

        return now - candidateSince >= dwell ? entry : null;
    }

    public ExplorerHoverTarget? UpdateTarget(ExplorerHoverTarget? target, Point screenPoint, DateTimeOffset now)
    {
        if (target is null || !target.ItemBounds.Contains(screenPoint))
        {
            Reset();
            return null;
        }

        if (!string.Equals(candidatePath, target.FolderPath, StringComparison.OrdinalIgnoreCase))
        {
            candidatePath = target.FolderPath;
            candidateSince = now;
        }

        return now - candidateSince >= dwell ? target : null;
    }

    public void Reset()
    {
        candidatePath = null;
        candidateSince = default;
    }
}

internal sealed class NativeMouseInteraction
{
    private static readonly TimeSpan ClickCooldown = TimeSpan.FromMilliseconds(750);
    private const double DragDistanceSquared = 64;
    private readonly object sync = new();
    private Point downPoint;
    private bool buttonDown;
    private DateTimeOffset suppressUntil;

    public void OnDown(Point point, DateTimeOffset now)
    {
        lock (sync)
        {
            downPoint = point;
            buttonDown = true;
            suppressUntil = now + ClickCooldown;
        }
    }

    public void OnUp(DateTimeOffset now)
    {
        lock (sync)
        {
            buttonDown = false;
            suppressUntil = now + ClickCooldown;
        }
    }

    public bool ShouldSuppress(Point point, DateTimeOffset now)
    {
        lock (sync)
        {
            if (!buttonDown) return now < suppressUntil;
            return !HasMovedForDrag(point);
        }
    }

    public bool IsDragging(Point point)
    {
        lock (sync) return buttonDown && HasMovedForDrag(point);
    }

    private bool HasMovedForDrag(Point point)
    {
        var dx = point.X - downPoint.X;
        var dy = point.Y - downPoint.Y;
        return dx * dx + dy * dy >= DragDistanceSquared;
    }
}
```

## src/FolderThemeStudio.App/Services/FolderHoverMonitor.cs

```csharp
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using FolderThemeStudio.App.ViewModels;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public interface IFolderHoverMonitorService : IDisposable
{
    Task StartAsync(CancellationToken token = default);
    Task ReloadCatalogAsync(CancellationToken token = default);
}

public interface ICursorPositionSource
{
    bool TryGetPosition(out Point screenPoint);
}

public interface IFolderHoverCardPresenter
{
    void Show(FolderCategoryEntry entry, Rect itemBounds);
    void Hide();
    bool IsPointerOverCard(Point screenPoint) => false;
    void SetDropInteractionEnabled(bool enabled) { }
    void SetInfoTipTarget(ExplorerHoverTarget? target) { }
}

public sealed class FolderHoverMonitor : IFolderHoverMonitorService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromMilliseconds(150);
    private readonly IFolderCategoryCatalog catalog;
    private readonly IExplorerFolderHoverTargetResolver targetResolver;
    private readonly ICursorPositionSource cursor;
    private readonly IFolderHoverCardPresenter presenter;
    private readonly IViewModelDispatcher dispatcher;
    private readonly TimeProvider timeProvider;
    private readonly Func<Point, nint> rootAtPoint;
    private readonly SemaphoreSlim lifecycleGate = new(1, 1);
    private readonly NativeMouseInteraction nativeMouse = new();
    private IReadOnlyDictionary<string, FolderCategoryEntry> entries = new Dictionary<string, FolderCategoryEntry>(StringComparer.OrdinalIgnoreCase);
    private CancellationTokenSource? stopSource;
    private Task? pollingTask;
    private volatile bool disposed;
    private HoverTargetSnapshot? quickEditSnapshot;

    private sealed record HoverTargetSnapshot(ExplorerHoverTarget Target, DateTimeOffset Time);

    public ExplorerHoverTarget? TryGetRecentTarget(Point point, nint rootWindowHandle = default)
    {
        var snapshot = Volatile.Read(ref quickEditSnapshot);
        if (snapshot is null || timeProvider.GetUtcNow() - snapshot.Time > TimeSpan.FromSeconds(1) ||
            !snapshot.Target.ItemBounds.Contains(point) ||
            (snapshot.Target.RootWindowHandle != nint.Zero &&
             snapshot.Target.RootWindowHandle != rootWindowHandle)) return null;
        return snapshot.Target;
    }

    public void NotifyNativeMouseDown(Point point)
    {
        nativeMouse.OnDown(point, timeProvider.GetUtcNow());
        Volatile.Write(ref quickEditSnapshot, null);
        try { _ = dispatcher.InvokeAsync(SafeHide); }
        catch (InvalidOperationException) { SafeHide(); }
    }

    public void NotifyNativeMouseUp() => nativeMouse.OnUp(timeProvider.GetUtcNow());

    public Task<ExplorerHoverTarget?> ResolveQuickEditTargetAsync(Point point, nint rootWindowHandle) => Task.Run(() =>
    {
        if (disposed || rootWindowHandle == nint.Zero) return null;
        try
        {
            if (rootAtPoint(point) != rootWindowHandle) return null;
            var target = targetResolver.TryResolve(point);
            return !disposed && target is not null && target.ItemBounds.Contains(point) &&
                (target.RootWindowHandle == nint.Zero || target.RootWindowHandle == rootWindowHandle)
                ? target : null;
        }
        catch (Exception) { return null; }
    });

    public FolderHoverMonitor(
        IFolderCategoryCatalog catalog,
        IExplorerFolderHoverTargetResolver targetResolver,
        ICursorPositionSource cursor,
        IFolderHoverCardPresenter presenter,
        IViewModelDispatcher dispatcher,
        TimeProvider? timeProvider = null,
        Func<Point, nint>? rootAtPoint = null)
    {
        this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        this.targetResolver = targetResolver ?? throw new ArgumentNullException(nameof(targetResolver));
        this.cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
        this.presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        this.timeProvider = timeProvider ?? TimeProvider.System;
        this.rootAtPoint = rootAtPoint ?? WindowsRootAtPoint;
    }

    public async Task StartAsync(CancellationToken token = default)
    {
        await lifecycleGate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            if (pollingTask is not null) return;
            try { await ReloadCatalogCoreAsync(token).ConfigureAwait(false); }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or
                ArgumentException or System.Text.Json.JsonException)
            {
                Volatile.Write(ref entries, new Dictionary<string, FolderCategoryEntry>(StringComparer.OrdinalIgnoreCase));
            }
            stopSource = new CancellationTokenSource();
            pollingTask = PollAsync(stopSource.Token);
        }
        finally { lifecycleGate.Release(); }
    }

    public Task ReloadCatalogAsync(CancellationToken token = default) => ReloadCatalogCoreAsync(token);

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        stopSource?.Cancel();
        Volatile.Write(ref quickEditSnapshot, null);
        try { _ = dispatcher.InvokeAsync(SafeHide); }
        catch (InvalidOperationException) { SafeHide(); }
        stopSource?.Dispose();
    }

    private async Task ReloadCatalogCoreAsync(CancellationToken token)
    {
        var loaded = await catalog.LoadAsync(token).ConfigureAwait(false);
        var next = loaded.ToDictionary(item => item.FolderPath, StringComparer.OrdinalIgnoreCase);
        Volatile.Write(ref entries, next);
    }

    private async Task PollAsync(CancellationToken token)
    {
        using var timer = new PeriodicTimer(PollInterval, timeProvider);
        var decision = new FolderHoverDecision();
        ExplorerHoverTarget? cachedTarget = null;
        ExplorerHoverTarget? displayed = null;
        FolderCategoryEntry? displayedEntry = null;
        DateTimeOffset lastOnFolder = default;

        try
        {
            while (await timer.WaitForNextTickAsync(token).ConfigureAwait(false))
            {
                var currentEntries = Volatile.Read(ref entries);
                if (!cursor.TryGetPosition(out var point))
                {
                    cachedTarget = null;
                    Volatile.Write(ref quickEditSnapshot, null);
                    decision.Reset();
                    await HideIfVisibleAsync(displayed).ConfigureAwait(false);
                    displayed = null;
                    displayedEntry = null;
                    continue;
                }

                if (nativeMouse.ShouldSuppress(point, timeProvider.GetUtcNow()))
                {
                    cachedTarget = null;
                    Volatile.Write(ref quickEditSnapshot, null);
                    decision.Reset();
                    await HideIfVisibleAsync(displayed).ConfigureAwait(false);
                    displayed = null;
                    displayedEntry = null;
                    continue;
                }

                if (displayed is not null && presenter.IsPointerOverCard(point))
                {
                    lastOnFolder = timeProvider.GetUtcNow();
                    continue;
                }

                var shouldResolve = cachedTarget is null
                    ? true
                    : !cachedTarget.ItemBounds.Contains(point) ||
                      (cachedTarget.RootWindowHandle != nint.Zero && rootAtPoint(point) != cachedTarget.RootWindowHandle);
                if (shouldResolve)
                {
                    try
                    {
                        cachedTarget = targetResolver.TryResolve(point);
                    }
                    catch (Exception)
                    {
                        cachedTarget = null;
                    }
                }
                Volatile.Write(ref quickEditSnapshot, cachedTarget is null ? null : new HoverTargetSnapshot(cachedTarget, timeProvider.GetUtcNow()));
                await dispatcher.InvokeAsync(() => presenter.SetInfoTipTarget(
                    !disposed && !nativeMouse.ShouldSuppress(point, timeProvider.GetUtcNow()) ? cachedTarget : null)).ConfigureAwait(false);

                var now = timeProvider.GetUtcNow();
                var visibleTarget = decision.UpdateTarget(cachedTarget, point, now);
                if (visibleTarget is null && displayed is not null && cachedTarget is not null &&
                    string.Equals(cachedTarget.FolderPath, displayed.FolderPath, StringComparison.OrdinalIgnoreCase) &&
                    cachedTarget.ItemBounds.Contains(point))
                    visibleTarget = cachedTarget;
                if (visibleTarget is null)
                {
                    if (displayed is not null &&
                        (cachedTarget is null || string.Equals(cachedTarget.FolderPath, displayed.FolderPath, StringComparison.OrdinalIgnoreCase)) &&
                        now - lastOnFolder < TimeSpan.FromMilliseconds(500))
                        continue;
                    await HideIfVisibleAsync(displayed).ConfigureAwait(false);
                    displayed = null;
                    displayedEntry = null;
                    continue;
                }

                lastOnFolder = now;
                var visibleEntry = currentEntries.TryGetValue(visibleTarget.FolderPath, out var category)
                    ? category
                    : new FolderCategoryEntry(visibleTarget.FolderPath,
                        Path.GetFileName(visibleTarget.FolderPath), string.Empty, "#2868D7");
                if (!string.Equals(displayed?.FolderPath, visibleTarget.FolderPath, StringComparison.OrdinalIgnoreCase) ||
                    !Equals(displayedEntry, visibleEntry))
                {
                    var showed = false;
                    await dispatcher.InvokeAsync(() =>
                    {
                        if (!disposed && !nativeMouse.ShouldSuppress(point, timeProvider.GetUtcNow()))
                        {
                            presenter.SetDropInteractionEnabled(nativeMouse.IsDragging(point));
                            presenter.Show(visibleEntry, cachedTarget!.ItemBounds);
                            showed = true;
                        }
                    }).ConfigureAwait(false);
                    if (showed)
                    {
                        displayed = visibleTarget;
                        displayedEntry = visibleEntry;
                    }
                }
            }
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested) { }
        catch (Exception)
        {
            await HideIfVisibleAsync(displayed).ConfigureAwait(false);
            displayed = null;
            if (!token.IsCancellationRequested)
            {
                stopSource?.Cancel();
            }
        }
        finally
        {
            await dispatcher.InvokeAsync(SafeHide).ConfigureAwait(false);
        }
    }

    private async Task HideIfVisibleAsync(ExplorerHoverTarget? displayed)
    {
        if (displayed is null) return;
        await dispatcher.InvokeAsync(SafeHide).ConfigureAwait(false);
    }

    private void SafeHide()
    {
        try { presenter.SetInfoTipTarget(null); }
        catch (Exception) { }
        try { presenter.Hide(); }
        catch (Exception) { }
    }

    private static nint WindowsRootAtPoint(Point point) =>
        NativeMethods.GetAncestor(NativeMethods.WindowFromPoint(new NativeMethods.NativePoint((int)point.X, (int)point.Y)), 2);

    private static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal readonly struct NativePoint(int x, int y)
        {
            internal readonly int X = x;
            internal readonly int Y = y;
        }

        [DllImport("user32.dll")]
        internal static extern nint WindowFromPoint(NativePoint point);

        [DllImport("user32.dll")]
        internal static extern nint GetAncestor(nint window, uint flags);
    }
}

internal sealed class WindowsCursorPositionSource : ICursorPositionSource
{
    public bool TryGetPosition(out Point screenPoint)
    {
        if (NativeMethods.GetCursorPos(out var point))
        {
            screenPoint = new Point(point.X, point.Y);
            return true;
        }
        screenPoint = default;
        return false;
    }

    internal static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct NativePoint { internal int X; internal int Y; }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(out NativePoint point);
    }
}

public sealed class NullFolderHoverMonitor : IFolderHoverMonitorService
{
    public Task StartAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task ReloadCatalogAsync(CancellationToken token = default) => Task.CompletedTask;
    public void Dispose() { }
}
```

## src/FolderThemeStudio.App/Services/FolderInfoSummary.cs

```csharp
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace FolderThemeStudio.App.Services;

public sealed record FolderInfoSummary(DateTime CreatedUtc, DateTime ModifiedUtc,
    int FileCount, int FolderCount, long SizeBytes, bool ContentsPartial, bool SizePartial)
{
    public static string FormatSize(long bytes)
    {
        string[] units = ["B", "KB", "MB", "GB", "TB"];
        var size = (double)bytes;
        var unit = 0;
        while (size >= 1024 && unit < units.Length - 1) { size /= 1024; unit++; }
        return $"{size.ToString(unit == 0 ? "0" : "0.#", CultureInfo.CurrentUICulture)} {units[unit]}";
    }
}

public sealed class FolderInfoSummaryService(int maxEntries = 4096, TimeSpan? timeBudget = null)
{
    public FolderInfoSummary? Read(string folderPath, CancellationToken token = default)
    {
        var root = new DirectoryInfo(folderPath);
        if (!root.Exists) return null;
        var created = root.CreationTimeUtc;
        var modified = root.LastWriteTimeUtc;
        if ((root.Attributes & FileAttributes.ReparsePoint) != 0)
            return new(created, modified, 0, 0, 0, true, true);

        var timer = Stopwatch.StartNew();
        var pending = new Queue<DirectoryInfo>();
        pending.Enqueue(root);
        int files = 0, folders = 0, visited = 0;
        long size = 0;
        bool contentsPartial = false, sizePartial = false;
        while (pending.TryDequeue(out var directory))
        {
            var isRoot = string.Equals(directory.FullName, root.FullName, StringComparison.OrdinalIgnoreCase);
            try
            {
                foreach (var item in directory.EnumerateFileSystemInfos())
                {
                    token.ThrowIfCancellationRequested();
                    if (visited++ >= maxEntries || timer.Elapsed >= (timeBudget ?? TimeSpan.FromMilliseconds(200)))
                        return new(created, modified, files, folders, size, contentsPartial || isRoot, true);
                    if (isRoot) { if (item is DirectoryInfo) folders++; else files++; }
                    if ((item.Attributes & FileAttributes.ReparsePoint) != 0) { sizePartial = true; continue; }
                    if (item is DirectoryInfo child) pending.Enqueue(child);
                    else if (item is FileInfo file) size += file.Length;
                }
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException)
            {
                contentsPartial |= isRoot;
                sizePartial = true;
            }
        }
        return new(created, modified, files, folders, size, contentsPartial, sizePartial);
    }
}
```

## src/FolderThemeStudio.App/Services/FolderLiveContentService.cs

```csharp
using System.IO;

namespace FolderThemeStudio.App.Services;

public sealed record RecentFolderFile(string Name, string Path, DateTime LastWriteTimeUtc);
public sealed record FolderDropTarget(string Name, string Path);
public sealed record FolderLiveContent(IReadOnlyList<RecentFolderFile> RecentFiles, IReadOnlyList<FolderDropTarget> DropTargets,
    FolderInfoSummary? Info = null);
public sealed record FolderDropResult(int MovedCount, IReadOnlyList<string> Errors);

public sealed class FolderLiveContentService
{
    public FolderLiveContent Read(string folderPath)
    {
        var root = Path.GetFullPath(folderPath);
        if (!Directory.Exists(root)) return new([], []);
        try
        {
            var directory = new DirectoryInfo(root);
            var files = directory.EnumerateFiles()
                .Where(file => (file.Attributes & (FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReparsePoint)) == 0)
                .OrderByDescending(file => file.LastWriteTimeUtc)
                .Take(5)
                .Select(file => new RecentFolderFile(file.Name, file.FullName, file.LastWriteTimeUtc))
                .ToArray();
            var children = directory.EnumerateDirectories()
                .Where(child => (child.Attributes & (FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReparsePoint)) == 0)
                .OrderBy(child => child.Name, StringComparer.CurrentCultureIgnoreCase)
                .Take(8)
                .Select(child => new FolderDropTarget(child.Name, child.FullName));
            var targets = new[] { new FolderDropTarget("此文件夹", root) }.Concat(children).ToArray();
            return new(files, targets, new FolderInfoSummaryService().Read(root));
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            return new([], [new FolderDropTarget("此文件夹", root)]);
        }
    }
}

public sealed class FolderDropService
{
    public FolderDropResult Move(IReadOnlyList<string> sourcePaths, string rootPath, string destinationPath)
    {
        ArgumentNullException.ThrowIfNull(sourcePaths);
        var errors = new List<string>();
        int moved = 0;
        string root;
        string destination;
        try
        {
            root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
            destination = Path.TrimEndingDirectorySeparator(Path.GetFullPath(destinationPath));
            var isRoot = string.Equals(root, destination, StringComparison.OrdinalIgnoreCase);
            var isDirectChild = string.Equals(Path.GetDirectoryName(destination), root, StringComparison.OrdinalIgnoreCase);
            if ((!isRoot && !isDirectChild) || !Directory.Exists(root) || !Directory.Exists(destination) ||
                (new DirectoryInfo(root).Attributes & FileAttributes.ReparsePoint) != 0 ||
                (new DirectoryInfo(destination).Attributes & FileAttributes.ReparsePoint) != 0)
                return new(0, ["目标必须是当前文件夹或其直属子文件夹。"]);
        }
        catch (Exception exception) when (exception is ArgumentException or IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            return new(0, [exception.Message]);
        }

        foreach (var sourcePath in sourcePaths)
        {
            try
            {
                var source = Path.GetFullPath(sourcePath);
                if (!File.Exists(source) || (new FileInfo(source).Attributes & FileAttributes.ReparsePoint) != 0)
                {
                    errors.Add($"仅支持移动普通文件：{Path.GetFileName(source)}");
                    continue;
                }
                var target = Path.Combine(destination, Path.GetFileName(source));
                if (File.Exists(target) || Directory.Exists(target))
                {
                    errors.Add($"目标已有同名项目：{Path.GetFileName(source)}");
                    continue;
                }
                File.Move(source, target);
                moved++;
            }
            catch (Exception exception) when (exception is ArgumentException or IOException or UnauthorizedAccessException or NotSupportedException or System.Security.SecurityException)
            {
                errors.Add($"{Path.GetFileName(sourcePath)}：{exception.Message}");
            }
        }
        return new(moved, errors);
    }
}
```

## src/FolderThemeStudio.App/Services/FolderQuickEditMouseHook.cs

```csharp
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public sealed class FolderQuickEditMouseHook : IDisposable
{
    private const int WhMouseLl = 14;
    private const int WmLeftDown = 0x0201;
    private const int WmLeftUp = 0x0202;
    private const int WmLeftDoubleClick = 0x0203;
    private const int WmRightDown = 0x0204;
    private const int WmRightUp = 0x0205;
    private const int WmRightDoubleClick = 0x0206;
    private const int WmMiddleDown = 0x0207;
    private const int WmMiddleUp = 0x0208;
    private const int WmMiddleDoubleClick = 0x0209;
    private const int GaRoot = 2;
    private readonly FolderHoverMonitor monitor;
    private readonly Action<string> openEditor;
    private readonly Dispatcher dispatcher;
    private readonly FolderQuickEditDecision decision;
    private readonly HookProc callback;
    private nint handle;
    private int latestEditRequest;

    public FolderQuickEditMouseHook(FolderHoverMonitor monitor, Action<string> openEditor, FolderEditChord chord, Dispatcher dispatcher)
    {
        this.monitor = monitor;
        this.openEditor = openEditor;
        this.dispatcher = dispatcher;
        decision = new FolderQuickEditDecision(chord);
        callback = OnMouseEvent;
        handle = SetWindowsHookEx(WhMouseLl, callback, GetModuleHandle(null), 0);
        if (handle == nint.Zero) throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to install folder quick-edit mouse hook.");
    }

    public void UpdateChord(FolderEditChord chord) => decision.UpdateChord(chord);

    public void Dispose()
    {
        if (handle == nint.Zero) return;
        UnhookWindowsHookEx(handle);
        handle = nint.Zero;
    }

    private nint OnMouseEvent(int code, nint message, nint data)
    {
        if (code < 0 || handle == nint.Zero) return CallNextHookEx(handle, code, message, data);
        var kind = message.ToInt32();
        if (kind is WmLeftUp or WmRightUp or WmMiddleUp)
        {
            monitor.NotifyNativeMouseUp();
            if (kind is WmRightUp or WmMiddleUp &&
                decision.OnMouseUp(kind == WmRightUp ? FolderMouseButton.Right : FolderMouseButton.Middle))
                return new nint(1);
            return CallNextHookEx(handle, code, message, data);
        }
        if (kind is not (WmLeftDown or WmLeftDoubleClick or WmRightDown or WmRightDoubleClick or WmMiddleDown or WmMiddleDoubleClick))
            return CallNextHookEx(handle, code, message, data);

        var mouse = Marshal.PtrToStructure<MouseHookData>(data);
        var point = new Point(mouse.Position.X, mouse.Position.Y);
        if (kind is WmLeftDown or WmLeftDoubleClick)
        {
            monitor.NotifyNativeMouseDown(point);
            return CallNextHookEx(handle, code, message, data);
        }

        var button = kind is WmRightDown or WmRightDoubleClick ? FolderMouseButton.Right : FolderMouseButton.Middle;
        var root = GetAncestor(WindowFromPoint(mouse.Position), GaRoot);
        monitor.NotifyNativeMouseDown(point);
        if (!IsExplorerWindow(root) || !decision.TryBeginDeferredEdit(button, CurrentModifiers()))
            return CallNextHookEx(handle, code, message, data);
        var request = ++latestEditRequest;
        _ = dispatcher.BeginInvoke(() => _ = OpenResolvedEditorAsync(point, root, request));
        return new nint(1);
    }

    private async Task OpenResolvedEditorAsync(Point point, nint root, int request)
    {
        var target = await monitor.ResolveQuickEditTargetAsync(point, root);
        if (handle != nint.Zero && request == latestEditRequest && target is not null) openEditor(target.FolderPath);
    }

    private static bool IsExplorerWindow(nint window)
    {
        var name = new StringBuilder(128);
        return GetClassName(window, name, name.Capacity) > 0 &&
            name.ToString() is "CabinetWClass" or "ExploreWClass" or "Progman" or "WorkerW";
    }

    private static ModifierKeys CurrentModifiers()
    {
        var result = ModifierKeys.None;
        if (IsDown(0x12)) result |= ModifierKeys.Alt;
        if (IsDown(0x11)) result |= ModifierKeys.Control;
        if (IsDown(0x10)) result |= ModifierKeys.Shift;
        if (IsDown(0x5B) || IsDown(0x5C)) result |= ModifierKeys.Windows;
        return result;
    }

    private static bool IsDown(int key) => (GetAsyncKeyState(key) & 0x8000) != 0;

    private delegate nint HookProc(int code, nint message, nint data);

    [StructLayout(LayoutKind.Sequential)]
    private struct NativePoint { public int X; public int Y; }

    [StructLayout(LayoutKind.Sequential)]
    private struct MouseHookData
    {
        public NativePoint Position;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public nint ExtraInfo;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint SetWindowsHookEx(int hook, HookProc callback, nint module, uint threadId);
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(nint hook);
    [DllImport("user32.dll")]
    private static extern nint CallNextHookEx(nint hook, int code, nint message, nint data);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern nint GetModuleHandle(string? name);
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int key);
    [DllImport("user32.dll")]
    private static extern nint WindowFromPoint(NativePoint point);
    [DllImport("user32.dll")]
    private static extern nint GetAncestor(nint window, int flags);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetClassName(nint window, StringBuilder className, int maximumCount);
}
```

## src/FolderThemeStudio.App/Services/NativeWindowCornerService.cs

```csharp
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FolderThemeStudio.App.Services;

internal static class NativeWindowCornerService
{
    private const int DwmWindowAttributeWindowCornerPreference = 33;
    private const int DwmWindowCornerPreferenceRound = 2;

    internal static void TryApplyRoundedCorners(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);
        if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000))
        {
            return;
        }

        var handle = new WindowInteropHelper(window).EnsureHandle();
        var preference = DwmWindowCornerPreferenceRound;
        _ = DwmSetWindowAttribute(
            handle,
            DwmWindowAttributeWindowCornerPreference,
            ref preference,
            sizeof(int));
    }

    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(
        nint windowHandle,
        int attribute,
        ref int attributeValue,
        int attributeSize);
}
```

## src/FolderThemeStudio.App/Services/StartupRegistrationService.cs

```csharp
using Microsoft.Win32;
using System.IO;

namespace FolderThemeStudio.App.Services;

public interface IStartupRegistrationService
{
    void SetEnabled(bool enabled);
}

public interface IStartupRegistry
{
    void SetValue(string name, string value);
    void DeleteValue(string name);
}

public sealed class CurrentUserStartupRegistry : IStartupRegistry
{
    private const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

    public void SetValue(string name, string value)
    {
        using var key = Registry.CurrentUser.CreateSubKey(RunKey, writable: true);
        key?.SetValue(name, value, RegistryValueKind.String);
    }

    public void DeleteValue(string name)
    {
        using var key = Registry.CurrentUser.OpenSubKey(RunKey, writable: true);
        key?.DeleteValue(name, throwOnMissingValue: false);
    }
}

public sealed class StartupRegistrationService : IStartupRegistrationService
{
    public const string ValueName = "FolderThemeStudio";
    private readonly IStartupRegistry registry;
    private readonly Func<string> executablePath;

    public StartupRegistrationService(IStartupRegistry? registry = null, Func<string>? executablePath = null)
    {
        this.registry = registry ?? new CurrentUserStartupRegistry();
        this.executablePath = executablePath ?? (() => Environment.ProcessPath
            ?? throw new InvalidOperationException("Cannot determine executable path."));
    }

    public void SetEnabled(bool enabled)
    {
        if (enabled)
        {
            registry.SetValue(ValueName, $"\"{Path.GetFullPath(executablePath())}\" --background");
        }
        else
        {
            registry.DeleteValue(ValueName);
        }
    }
}
```

## src/FolderThemeStudio.App/Services/TrayIconService.cs

```csharp
using System.Drawing;
using Forms = System.Windows.Forms;

namespace FolderThemeStudio.App.Services;

public sealed class TrayIconService : IDisposable
{
    private readonly Forms.NotifyIcon icon;
    private readonly Icon? ownedIcon;

    public TrayIconService()
    {
        var executable = Environment.ProcessPath;
        ownedIcon = executable is null ? null : Icon.ExtractAssociatedIcon(executable);
        var menu = new Forms.ContextMenuStrip();
        menu.Items.Add("打开主窗口", null, (_, _) => OpenRequested?.Invoke(this, EventArgs.Empty));
        menu.Items.Add("暂停 / 恢复监控", null, (_, _) => ToggleMonitoringRequested?.Invoke(this, EventArgs.Empty));
        menu.Items.Add(new Forms.ToolStripSeparator());
        menu.Items.Add("退出", null, (_, _) => ExitRequested?.Invoke(this, EventArgs.Empty));
        icon = new Forms.NotifyIcon
        {
            Text = "文件夹样式",
            Icon = ownedIcon ?? SystemIcons.Application,
            ContextMenuStrip = menu,
            Visible = true,
        };
        icon.DoubleClick += (_, _) => OpenRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? OpenRequested;
    public event EventHandler? ToggleMonitoringRequested;
    public event EventHandler? ExitRequested;

    public void ShowBackgroundNotice() => icon.ShowBalloonTip(
        2500,
        "文件夹样式仍在运行",
        "递归监控已转入后台，可从托盘图标重新打开。",
        Forms.ToolTipIcon.Info);

    public void Dispose()
    {
        icon.Visible = false;
        icon.ContextMenuStrip?.Dispose();
        icon.Dispose();
        ownedIcon?.Dispose();
    }
}
```

## src/FolderThemeStudio.App/Settings/AppSettings.cs

```csharp
using System.Text.RegularExpressions;
using FolderThemeStudio.App.Services;

namespace FolderThemeStudio.App.Settings;

public sealed record FolderPalette(string GradientStart, string GradientEnd, string Stroke, string Glow)
{
    public static FolderPalette Default { get; } = new("#007AFF", "#0055D4", "#FFFFFF", "#007AFF");
}

public sealed record AppSettings(
    string Language,
    bool TutorialCompleted,
    FolderPalette Palette,
    IReadOnlyList<string> RecentColors,
    bool StartWithWindows = true,
    bool CloseToTray = true,
    bool MonitoringPaused = false,
    double CardOpacity = 1)
{
    public FolderEditChord EditChord { get; init; } = FolderEditChord.Default;
    private static readonly Regex HexColor = new("^#[0-9A-Fa-f]{6}$", RegexOptions.CultureInvariant);

    public static AppSettings Default { get; } = new("zh-CN", false, FolderPalette.Default, Array.Empty<string>());

    public static bool TryNormalize(AppSettings? value, out AppSettings normalized)
    {
        normalized = Default;
        if (value is null || value.Language is not ("zh-CN" or "en-US") || value.Palette is null ||
            !double.IsFinite(value.CardOpacity) || value.CardOpacity is < 0 or > 1 ||
            !FolderEditChord.TryNormalize(value.EditChord, out var normalizedChord))
        {
            return false;
        }

        var paletteColors = new[]
        {
            value.Palette.GradientStart,
            value.Palette.GradientEnd,
            value.Palette.Stroke,
            value.Palette.Glow,
        };
        if (paletteColors.Any(color => color is null || !HexColor.IsMatch(color)))
        {
            return false;
        }

        var recent = (value.RecentColors ?? Array.Empty<string>())
            .Where(color => color is not null && HexColor.IsMatch(color))
            .Select(color => color.ToUpperInvariant())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(12)
            .ToArray();
        normalized = new AppSettings(
            value.Language,
            value.TutorialCompleted,
            new FolderPalette(
                value.Palette.GradientStart.ToUpperInvariant(),
                value.Palette.GradientEnd.ToUpperInvariant(),
                value.Palette.Stroke.ToUpperInvariant(),
                value.Palette.Glow.ToUpperInvariant()),
            recent,
            value.StartWithWindows,
            value.CloseToTray,
            value.MonitoringPaused,
            value.CardOpacity) { EditChord = normalizedChord };
        return true;
    }
}
```

## src/FolderThemeStudio.App/Settings/AppleColors.cs

```csharp
namespace FolderThemeStudio.App.Settings;

public sealed record AppleColorPreset(string Name, string Hex, string GradientEnd)
{
    public FolderPalette Palette => new(Hex, GradientEnd, "#FFFFFF", Hex);
}

public static class AppleColors
{
    public static IReadOnlyList<AppleColorPreset> All { get; } =
    [
        new("蓝 · Blue", "#007AFF", "#0055D4"),
        new("绿 · Green", "#34C759", "#209C43"),
        new("靛蓝 · Indigo", "#5856D6", "#3D3BAC"),
        new("紫 · Purple", "#AF52DE", "#8935BB"),
        new("粉 · Pink", "#FF2D55", "#CF2047"),
        new("红 · Red", "#FF3B30", "#D42B25"),
        new("橙 · Orange", "#FF9500", "#DD7600"),
        new("黄 · Yellow", "#FFCC00", "#D9A500"),
        new("青 · Teal", "#30B0C7", "#238CA2"),
        new("薄荷 · Mint", "#00C7BE", "#009F99"),
        new("银 · Silver", "#C7C7CC", "#98989D"),
        new("石墨 · Graphite", "#8E8E93", "#636366")
    ];
}
```

## src/FolderThemeStudio.App/Settings/JsonAppSettingsStore.cs

```csharp
using System.IO;
using System.Text.Json;

namespace FolderThemeStudio.App.Settings;

public interface IAppSettingsStore
{
    Task<AppSettings> LoadAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(AppSettings settings, CancellationToken cancellationToken = default);
}

public sealed class JsonAppSettingsStore : IAppSettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    private readonly string path;

    public JsonAppSettingsStore(string? path = null)
    {
        this.path = Path.GetFullPath(path ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio",
            "settings.json"));
    }

    public async Task<AppSettings> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(path))
        {
            return AppSettings.Default;
        }

        try
        {
            await using var stream = File.OpenRead(path);
            var value = await JsonSerializer.DeserializeAsync<AppSettings>(stream, JsonOptions, cancellationToken);
            return AppSettings.TryNormalize(value, out var normalized) ? normalized : AppSettings.Default;
        }
        catch (Exception exception) when (exception is IOException or JsonException or UnauthorizedAccessException)
        {
            return AppSettings.Default;
        }
    }

    public async Task SaveAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(settings);
        if (!AppSettings.TryNormalize(settings, out var normalized))
        {
            throw new ArgumentException("Settings contain an unsupported language or invalid color.", nameof(settings));
        }

        var directory = Path.GetDirectoryName(path) ?? throw new InvalidOperationException("Settings path has no parent directory.");
        Directory.CreateDirectory(directory);
        var temporaryPath = path + ".tmp";
        await using (var stream = new FileStream(temporaryPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
        {
            await JsonSerializer.SerializeAsync(stream, normalized, JsonOptions, cancellationToken);
            await stream.FlushAsync(cancellationToken);
        }

        File.Move(temporaryPath, path, overwrite: true);
    }
}
```

## src/FolderThemeStudio.App/SettingsWindow.xaml

```xml
<controls:RoundedWindow x:Class="FolderThemeStudio.App.SettingsWindow" Style="{StaticResource {x:Type Window}}"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:controls="clr-namespace:FolderThemeStudio.App.Controls"
        Title="{DynamicResource Settings.Title}"
        Width="980" Height="680" MinWidth="760" MinHeight="560"
        WindowStartupLocation="CenterOwner"
        Icon="Assets/FolderThemeStudio.png"
        UseLayoutRounding="True">
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <TabControl Grid.Row="0">
            <TabItem Header="{DynamicResource Settings.General}">
                <Border Margin="12" Style="{StaticResource CardStyle}">
                    <StackPanel MaxWidth="480" HorizontalAlignment="Left">
                        <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource Settings.Language}" />
                        <ComboBox Width="260" HorizontalAlignment="Left"
                                  SelectedValue="{Binding SelectedLanguage}"
                                  SelectedValuePath="Tag">
                            <ComboBoxItem Tag="zh-CN" Content="{DynamicResource Settings.Chinese}" />
                            <ComboBoxItem Tag="en-US" Content="{DynamicResource Settings.English}" />
                        </ComboBox>
                        <CheckBox Margin="0,24,0,8" IsChecked="{Binding StartWithWindows}"
                                  Content="登录 Windows 后自动在后台启动并恢复监控" />
                        <CheckBox Margin="0,4,0,8" IsChecked="{Binding CloseToTray}"
                                  Content="关闭主窗口时收起到后台" />
                        <TextBlock Margin="0,20,0,6" Text="{DynamicResource Settings.CardOpacity}" />
                        <Slider Minimum="0" Maximum="100" Value="{Binding CardOpacityPercent}" TickFrequency="1" />
                        <TextBlock Text="{Binding CardOpacityPercent, StringFormat={}{0:F0}%}" />
                        <TextBlock Margin="0,18,0,6" Text="{DynamicResource Settings.QuickEditChord}" />
                        <Button Content="{Binding EditChordDisplay}" PreviewMouseDown="EditChord_PreviewMouseDown"
                                HorizontalAlignment="Left" />
                        <TextBlock Foreground="{StaticResource MutedTextBrush}" Text="{DynamicResource Settings.QuickEditHint}" />
                    </StackPanel>
                </Border>
            </TabItem>

            <TabItem Header="{DynamicResource Settings.Palette}" Visibility="Collapsed">
                <Grid Margin="12">
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="190" />
                        <ColumnDefinition Width="16" />
                        <ColumnDefinition Width="*" />
                        <ColumnDefinition Width="16" />
                        <ColumnDefinition Width="280" />
                    </Grid.ColumnDefinitions>

                    <Border Grid.Column="0" Style="{StaticResource CardStyle}">
                        <StackPanel>
                            <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource Settings.ColorRole}" />
                            <ListBox ItemsSource="{Binding Roles}" SelectedItem="{Binding SelectedRole}">
                                <ListBox.ItemTemplate>
                                    <DataTemplate>
                                        <Grid Margin="2,5">
                                            <Grid.ColumnDefinitions>
                                                <ColumnDefinition Width="28" />
                                                <ColumnDefinition Width="*" />
                                            </Grid.ColumnDefinitions>
                                            <Border Width="20" Height="20" CornerRadius="4" Background="{Binding Picker.SelectedBrush}" />
                                            <TextBlock Grid.Column="1" Margin="8,0,0,0" VerticalAlignment="Center" Text="{Binding DisplayName}" />
                                        </Grid>
                                    </DataTemplate>
                                </ListBox.ItemTemplate>
                            </ListBox>
                        </StackPanel>
                    </Border>

                    <Border Grid.Column="2" Style="{StaticResource CardStyle}">
                        <StackPanel DataContext="{Binding SelectedRole.Picker}">
                            <Border Height="54" Margin="0,0,0,10" CornerRadius="8" Background="{Binding SelectedBrush}" />
                            <Label Style="{StaticResource FieldLabelStyle}" Content="{DynamicResource Settings.Hue}" />
                            <Slider Minimum="0" Maximum="359" Value="{Binding Hue}" />
                            <Label Style="{StaticResource FieldLabelStyle}" Content="{DynamicResource Settings.Saturation}" />
                            <Slider Minimum="0" Maximum="1" Value="{Binding Saturation}" />
                            <Label Style="{StaticResource FieldLabelStyle}" Content="{DynamicResource Settings.Value}" />
                            <Slider Minimum="0" Maximum="1" Value="{Binding Value}" />
                            <Label Style="{StaticResource FieldLabelStyle}" Content="{DynamicResource Settings.Hex}" />
                            <TextBox Text="{Binding Hex, UpdateSourceTrigger=PropertyChanged}" />
                        </StackPanel>
                    </Border>

                    <ScrollViewer Grid.Column="4" VerticalScrollBarVisibility="Auto">
                        <StackPanel>
                            <Border Style="{StaticResource CardStyle}">
                                <StackPanel>
                                    <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource Section.LivePreview}" />
                                    <Image Width="180" Height="180" Source="{Binding PreviewImage}" Stretch="Uniform" />
                                </StackPanel>
                            </Border>
                            <Border Style="{StaticResource CardStyle}">
                                <StackPanel>
                                    <TextBlock Style="{StaticResource SectionTitleStyle}" Text="{DynamicResource Settings.Recommended}" />
                                    <ItemsControl ItemsSource="{Binding RecommendedPalettes}">
                                        <ItemsControl.ItemsPanel><ItemsPanelTemplate><WrapPanel /></ItemsPanelTemplate></ItemsControl.ItemsPanel>
                                        <ItemsControl.ItemTemplate>
                                            <DataTemplate>
                                                <Button Padding="4" Margin="0,0,6,6" Click="Recommended_Click" ToolTip="{DynamicResource Settings.Recommended}">
                                                    <StackPanel Orientation="Horizontal">
                                                        <Border Width="20" Height="28" Background="{Binding GradientStart}" />
                                                        <Border Width="20" Height="28" Background="{Binding GradientEnd}" />
                                                        <Border Width="8" Height="28" Background="{Binding Stroke}" />
                                                        <Border Width="8" Height="28" Background="{Binding Glow}" />
                                                    </StackPanel>
                                                </Button>
                                            </DataTemplate>
                                        </ItemsControl.ItemTemplate>
                                    </ItemsControl>
                                    <TextBlock Margin="0,10,0,6" FontWeight="SemiBold" Text="{DynamicResource Settings.Recent}" />
                                    <ItemsControl ItemsSource="{Binding RecentColors}">
                                        <ItemsControl.ItemsPanel><ItemsPanelTemplate><WrapPanel /></ItemsPanelTemplate></ItemsControl.ItemsPanel>
                                        <ItemsControl.ItemTemplate>
                                            <DataTemplate>
                                                <Button Width="30" Height="30" Padding="3" Margin="0,0,6,6" Click="Recent_Click">
                                                    <Border Background="{Binding}" CornerRadius="3" />
                                                </Button>
                                            </DataTemplate>
                                        </ItemsControl.ItemTemplate>
                                    </ItemsControl>
                                </StackPanel>
                            </Border>
                        </StackPanel>
                    </ScrollViewer>
                </Grid>
            </TabItem>
        </TabControl>

        <StackPanel Grid.Row="1" Margin="0,14,0,0" HorizontalAlignment="Right" Orientation="Horizontal">
            <Button Content="{DynamicResource Common.Cancel}" Click="Cancel_Click" />
            <Button Style="{StaticResource PrimaryButtonStyle}" Content="{DynamicResource Common.Save}"
                    IsEnabled="{Binding CanSave}" Click="Save_Click" />
        </StackPanel>
    </Grid>
</controls:RoundedWindow>
```

## src/FolderThemeStudio.App/SettingsWindow.xaml.cs

```csharp
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
```

## src/FolderThemeStudio.App/Styles/AppleStyles.xaml

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:sys="clr-namespace:System;assembly=System.Runtime"
                    xmlns:automation="clr-namespace:System.Windows.Automation;assembly=PresentationCore"
                    xmlns:controls="clr-namespace:FolderThemeStudio.App.Controls">
    <SolidColorBrush x:Key="WindowBackgroundBrush" Color="#F5F5F7"/>
    <SolidColorBrush x:Key="CardBrush" Color="#FFFFFF"/>
    <SolidColorBrush x:Key="TextBrush" Color="#1D1D1F"/>
    <SolidColorBrush x:Key="MutedTextBrush" Color="#6E6E73"/>
    <SolidColorBrush x:Key="BorderBrush" Color="#E5E5EA"/>
    <SolidColorBrush x:Key="AccentBrush" Color="#007AFF"/>
    <SolidColorBrush x:Key="AccentHoverBrush" Color="#0063D1"/>
    <SolidColorBrush x:Key="FocusBrush" Color="#007AFF"/>
    <BooleanToVisibilityConverter x:Key="BoolToVisibility"/>
    <Style x:Key="VisibleFocusStyle" TargetType="Control">
        <Setter Property="Template"><Setter.Value><ControlTemplate>
            <Border Margin="-3" BorderBrush="{StaticResource FocusBrush}" BorderThickness="2" CornerRadius="11"/>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="Window">
        <Setter Property="Background" Value="{StaticResource WindowBackgroundBrush}"/>
        <Setter Property="Foreground" Value="{StaticResource TextBrush}"/>
        <Setter Property="FontFamily" Value="Segoe UI Variable Text, Segoe UI"/>
        <Setter Property="FontSize" Value="13"/>
        <Setter Property="TextOptions.TextFormattingMode" Value="Display"/>
        <Setter Property="TextOptions.TextRenderingMode" Value="ClearType"/>
    </Style>
    <Style TargetType="controls:RoundedWindow" BasedOn="{StaticResource {x:Type Window}}"/>
    <Style TargetType="Button">
        <Setter Property="MinHeight" Value="36"/><Setter Property="Padding" Value="14,8"/>
        <Setter Property="Margin" Value="0,0,8,8"/><Setter Property="Background" Value="#EEEEF2"/>
        <Setter Property="BorderBrush" Value="Transparent"/><Setter Property="BorderThickness" Value="1"/>
        <Setter Property="Foreground" Value="{StaticResource TextBrush}"/><Setter Property="Cursor" Value="Hand"/>
        <Setter Property="FocusVisualStyle" Value="{StaticResource VisibleFocusStyle}"/>
        <Setter Property="HorizontalContentAlignment" Value="Center"/><Setter Property="VerticalContentAlignment" Value="Center"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="Button">
            <Border x:Name="Surface" Background="{TemplateBinding Background}" BorderBrush="{TemplateBinding BorderBrush}"
                    BorderThickness="{TemplateBinding BorderThickness}" CornerRadius="10" Padding="{TemplateBinding Padding}">
                <ContentPresenter HorizontalAlignment="{TemplateBinding HorizontalContentAlignment}" VerticalAlignment="{TemplateBinding VerticalContentAlignment}" RecognizesAccessKey="True"/>
            </Border>
            <ControlTemplate.Triggers>
                <Trigger Property="IsMouseOver" Value="True"><Setter TargetName="Surface" Property="Opacity" Value="0.83"/></Trigger>
                <Trigger Property="IsPressed" Value="True"><Setter TargetName="Surface" Property="Opacity" Value="0.64"/></Trigger>
                <Trigger Property="IsEnabled" Value="False"><Setter TargetName="Surface" Property="Opacity" Value="0.42"/></Trigger>
            </ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style x:Key="PrimaryButtonStyle" TargetType="Button" BasedOn="{StaticResource {x:Type Button}}">
        <Setter Property="Background" Value="{StaticResource AccentBrush}"/><Setter Property="Foreground" Value="White"/>
        <Setter Property="MinHeight" Value="40"/><Setter Property="Padding" Value="18,9"/><Setter Property="FontWeight" Value="SemiBold"/>
    </Style>
    <Style x:Key="SidebarButtonStyle" TargetType="Button" BasedOn="{StaticResource {x:Type Button}}">
        <Setter Property="Background" Value="Transparent"/><Setter Property="Margin" Value="0,2,0,2"/>
        <Setter Property="HorizontalContentAlignment" Value="Left"/><Setter Property="Padding" Value="12,10"/>
        <Setter Property="FontSize" Value="13"/>
        <Style.Triggers><Trigger Property="IsMouseOver" Value="True"><Setter Property="Background" Value="#E6E6EC"/></Trigger></Style.Triggers>
    </Style>
    <Style x:Key="FeatureCardButtonStyle" TargetType="Button" BasedOn="{StaticResource {x:Type Button}}">
        <Setter Property="Background" Value="White"/><Setter Property="BorderBrush" Value="#ECECF0"/>
        <Setter Property="Padding" Value="22"/><Setter Property="HorizontalContentAlignment" Value="Stretch"/>
        <Setter Property="VerticalContentAlignment" Value="Top"/><Setter Property="Margin" Value="0"/>
        <Setter Property="MinHeight" Value="164"/>
    </Style>
    <Style TargetType="TextBox">
        <Setter Property="MinHeight" Value="36"/><Setter Property="Padding" Value="10,8"/>
        <Setter Property="Background" Value="#F7F7F9"/><Setter Property="Foreground" Value="{StaticResource TextBrush}"/>
        <Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/><Setter Property="BorderThickness" Value="1"/>
        <Setter Property="VerticalContentAlignment" Value="Center"/><Setter Property="FocusVisualStyle" Value="{StaticResource VisibleFocusStyle}"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="TextBox">
            <Border x:Name="Surface" CornerRadius="8" Background="{TemplateBinding Background}" BorderBrush="{TemplateBinding BorderBrush}" BorderThickness="{TemplateBinding BorderThickness}">
                <ScrollViewer x:Name="PART_ContentHost" Margin="{TemplateBinding Padding}"/>
            </Border>
            <ControlTemplate.Triggers>
                <Trigger Property="IsKeyboardFocused" Value="True"><Setter TargetName="Surface" Property="BorderBrush" Value="{StaticResource AccentBrush}"/></Trigger>
                <Trigger Property="IsEnabled" Value="False"><Setter TargetName="Surface" Property="Opacity" Value="0.45"/></Trigger>
            </ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="ComboBox">
        <Setter Property="MinHeight" Value="36"/><Setter Property="Padding" Value="11,7"/>
        <Setter Property="Background" Value="#F7F7F9"/><Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
        <Setter Property="BorderThickness" Value="1"/><Setter Property="Foreground" Value="{StaticResource TextBrush}"/>
        <Setter Property="FocusVisualStyle" Value="{StaticResource VisibleFocusStyle}"/>
        <Setter Property="ScrollViewer.CanContentScroll" Value="True"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="ComboBox">
            <Grid>
                <ToggleButton Style="{x:Null}" Focusable="False" ClickMode="Press"
                              IsChecked="{Binding IsDropDownOpen, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}">
                    <ToggleButton.Template><ControlTemplate TargetType="ToggleButton">
                        <Border CornerRadius="8" Background="{Binding Background, RelativeSource={RelativeSource AncestorType=ComboBox}}"
                                BorderBrush="{Binding BorderBrush, RelativeSource={RelativeSource AncestorType=ComboBox}}" BorderThickness="1">
                            <TextBlock Text="⌄" FontSize="17" Foreground="{StaticResource MutedTextBrush}" HorizontalAlignment="Right" VerticalAlignment="Center" Margin="0,0,12,3"/>
                        </Border>
                    </ControlTemplate></ToggleButton.Template>
                </ToggleButton>
                <ContentPresenter Margin="11,7,32,7" VerticalAlignment="Center" IsHitTestVisible="False"
                                  Content="{TemplateBinding SelectionBoxItem}" ContentTemplate="{TemplateBinding SelectionBoxItemTemplate}" ContentTemplateSelector="{TemplateBinding ItemTemplateSelector}"/>
                <Popup x:Name="PART_Popup" Placement="Bottom" AllowsTransparency="True" Focusable="False" IsOpen="{TemplateBinding IsDropDownOpen}" PopupAnimation="Fade">
                    <Border Margin="0,4,0,0" CornerRadius="10" Background="White" BorderBrush="{StaticResource BorderBrush}" BorderThickness="1"
                            MinWidth="{Binding ActualWidth, RelativeSource={RelativeSource TemplatedParent}}" Padding="5">
                        <ScrollViewer MaxHeight="300"><ItemsPresenter KeyboardNavigation.DirectionalNavigation="Contained"/></ScrollViewer>
                    </Border>
                </Popup>
            </Grid>
            <ControlTemplate.Triggers><Trigger Property="IsEnabled" Value="False"><Setter Property="Opacity" Value="0.45"/></Trigger></ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="ComboBoxItem">
        <Setter Property="Padding" Value="10,7"/><Setter Property="HorizontalContentAlignment" Value="Stretch"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="ComboBoxItem">
            <Border x:Name="Surface" CornerRadius="6" Padding="{TemplateBinding Padding}" Background="Transparent"><ContentPresenter/></Border>
            <ControlTemplate.Triggers>
                <Trigger Property="IsHighlighted" Value="True"><Setter TargetName="Surface" Property="Background" Value="#EBF3FF"/></Trigger>
                <Trigger Property="IsSelected" Value="True"><Setter TargetName="Surface" Property="Background" Value="#E0EDFF"/></Trigger>
            </ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="Slider">
        <Setter Property="MinHeight" Value="28"/><Setter Property="FocusVisualStyle" Value="{StaticResource VisibleFocusStyle}"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="Slider">
            <Grid VerticalAlignment="Center" Height="26" Margin="2,0">
                <Track x:Name="PART_Track" Height="26" IsDirectionReversed="{TemplateBinding IsDirectionReversed}">
                    <Track.DecreaseRepeatButton><RepeatButton Command="Slider.DecreaseLarge" Focusable="False" Style="{x:Null}">
                        <RepeatButton.Template><ControlTemplate><Border Height="4" CornerRadius="2" Background="{StaticResource AccentBrush}"/></ControlTemplate></RepeatButton.Template>
                    </RepeatButton></Track.DecreaseRepeatButton>
                    <Track.IncreaseRepeatButton><RepeatButton Command="Slider.IncreaseLarge" Focusable="False" Style="{x:Null}">
                        <RepeatButton.Template><ControlTemplate><Border Height="4" CornerRadius="2" Background="#D6D6DC"/></ControlTemplate></RepeatButton.Template>
                    </RepeatButton></Track.IncreaseRepeatButton>
                    <Track.Thumb><Thumb Width="18" Height="18"><Thumb.Template><ControlTemplate>
                        <Ellipse Fill="White" Stroke="#CFCFD5" StrokeThickness="1"><Ellipse.Effect><DropShadowEffect BlurRadius="5" ShadowDepth="1" Opacity="0.15"/></Ellipse.Effect></Ellipse>
                    </ControlTemplate></Thumb.Template></Thumb></Track.Thumb>
                </Track>
            </Grid>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="CheckBox">
        <Setter Property="VerticalContentAlignment" Value="Center"/><Setter Property="FocusVisualStyle" Value="{StaticResource VisibleFocusStyle}"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="CheckBox">
            <DockPanel>
                <Border x:Name="CheckSurface" Width="17" Height="17" CornerRadius="5" Background="White" BorderBrush="#C7C7CC" BorderThickness="1" Margin="0,0,9,0" VerticalAlignment="Center">
                    <TextBlock x:Name="CheckMark" Text="✓" Visibility="Collapsed" Foreground="White" FontSize="12" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
                </Border>
                <ContentPresenter VerticalAlignment="{TemplateBinding VerticalContentAlignment}" RecognizesAccessKey="True"/>
            </DockPanel>
            <ControlTemplate.Triggers>
                <Trigger Property="IsChecked" Value="True"><Setter TargetName="CheckSurface" Property="Background" Value="{StaticResource AccentBrush}"/><Setter TargetName="CheckSurface" Property="BorderBrush" Value="{StaticResource AccentBrush}"/><Setter TargetName="CheckMark" Property="Visibility" Value="Visible"/></Trigger>
                <Trigger Property="IsEnabled" Value="False"><Setter Property="Opacity" Value="0.45"/></Trigger>
            </ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="RadioButton">
        <Setter Property="FocusVisualStyle" Value="{StaticResource VisibleFocusStyle}"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="RadioButton">
            <DockPanel Margin="{TemplateBinding Padding}">
                <Border x:Name="Ring" Width="17" Height="17" CornerRadius="9" BorderThickness="1" BorderBrush="#C7C7CC" Background="White" Margin="0,0,8,0" VerticalAlignment="Center">
                    <Ellipse x:Name="Dot" Width="9" Height="9" Fill="{StaticResource AccentBrush}" Visibility="Collapsed"/>
                </Border>
                <ContentPresenter VerticalAlignment="Center" RecognizesAccessKey="True"/>
            </DockPanel>
            <ControlTemplate.Triggers><Trigger Property="IsChecked" Value="True"><Setter TargetName="Dot" Property="Visibility" Value="Visible"/><Setter TargetName="Ring" Property="BorderBrush" Value="{StaticResource AccentBrush}"/></Trigger></ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="Expander">
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="Expander">
            <StackPanel>
                <ToggleButton Style="{x:Null}" HorizontalContentAlignment="Left" Focusable="True" Padding="0,8"
                              IsChecked="{Binding IsExpanded, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}">
                    <ToggleButton.Template><ControlTemplate TargetType="ToggleButton">
                        <Border Background="Transparent" Padding="{TemplateBinding Padding}"><DockPanel>
                            <TextBlock x:Name="Arrow" Text="›" FontSize="18" Foreground="{StaticResource MutedTextBrush}" Margin="0,0,8,0" RenderTransformOrigin="0.5,0.5"/>
                            <ContentPresenter VerticalAlignment="Center" Content="{Binding Header, RelativeSource={RelativeSource AncestorType=Expander}}"/>
                        </DockPanel></Border>
                        <ControlTemplate.Triggers><Trigger Property="IsChecked" Value="True"><Setter TargetName="Arrow" Property="RenderTransform"><Setter.Value><RotateTransform Angle="90"/></Setter.Value></Setter></Trigger></ControlTemplate.Triggers>
                    </ControlTemplate></ToggleButton.Template>
                </ToggleButton>
                <ContentPresenter x:Name="ExpandedContent" Visibility="Collapsed"/>
            </StackPanel>
            <ControlTemplate.Triggers><Trigger Property="IsExpanded" Value="True"><Setter TargetName="ExpandedContent" Property="Visibility" Value="Visible"/></Trigger></ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="ListBoxItem">
        <Setter Property="HorizontalContentAlignment" Value="Stretch"/><Setter Property="Padding" Value="4"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="ListBoxItem">
            <Border x:Name="Surface" Padding="{TemplateBinding Padding}" CornerRadius="8" Background="Transparent"><ContentPresenter/></Border>
            <ControlTemplate.Triggers>
                <Trigger Property="IsSelected" Value="True"><Setter TargetName="Surface" Property="Background" Value="#E6F0FF"/></Trigger>
                <Trigger Property="IsMouseOver" Value="True"><Setter TargetName="Surface" Property="Background" Value="#F0F0F4"/></Trigger>
            </ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style x:Key="ColorSwatchItemStyle" TargetType="ListBoxItem">
        <Setter Property="automation:AutomationProperties.Name" Value="{Binding Name}"/>
        <Setter Property="Margin" Value="2"/><Setter Property="Padding" Value="4"/><Setter Property="ToolTip" Value="{Binding Name}"/>
        <Setter Property="FocusVisualStyle" Value="{StaticResource VisibleFocusStyle}"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="ListBoxItem">
            <Border x:Name="Ring" Padding="{TemplateBinding Padding}" BorderThickness="2" BorderBrush="Transparent" CornerRadius="24"><ContentPresenter/></Border>
            <ControlTemplate.Triggers>
                <Trigger Property="IsSelected" Value="True"><Setter TargetName="Ring" Property="BorderBrush" Value="{StaticResource AccentBrush}"/></Trigger>
                <Trigger Property="IsMouseOver" Value="True"><Setter TargetName="Ring" Property="Background" Value="#ECECF0"/></Trigger>
            </ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <DataTemplate x:Key="ColorSwatchTemplate"><Ellipse Width="26" Height="26" Fill="{Binding Hex}"/></DataTemplate>
    <Style TargetType="TabItem">
        <Setter Property="Padding" Value="14,8"/><Setter Property="Margin" Value="0,0,4,0"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="TabItem">
            <Border x:Name="Surface" CornerRadius="9" Padding="{TemplateBinding Padding}" Background="Transparent"><ContentPresenter ContentSource="Header" RecognizesAccessKey="True"/></Border>
            <ControlTemplate.Triggers><Trigger Property="IsSelected" Value="True"><Setter TargetName="Surface" Property="Background" Value="White"/><Setter Property="Foreground" Value="{StaticResource AccentBrush}"/></Trigger></ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="TabControl">
        <Setter Property="Background" Value="Transparent"/><Setter Property="BorderThickness" Value="0"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="TabControl">
            <Grid><Grid.RowDefinitions><RowDefinition Height="Auto"/><RowDefinition Height="*"/></Grid.RowDefinitions>
                <Border Background="#EAEAEE" CornerRadius="11" Padding="3" HorizontalAlignment="Left" Margin="0,0,0,12"><TabPanel IsItemsHost="True"/></Border>
                <ContentPresenter Grid.Row="1" ContentSource="SelectedContent"/>
            </Grid>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style x:Key="CardStyle" TargetType="Border">
        <Setter Property="Background" Value="{StaticResource CardBrush}"/><Setter Property="BorderBrush" Value="#ECECF0"/>
        <Setter Property="BorderThickness" Value="1"/><Setter Property="CornerRadius" Value="18"/>
        <Setter Property="Padding" Value="22"/><Setter Property="Margin" Value="0,0,0,16"/>
    </Style>
    <Style x:Key="SectionTitleStyle" TargetType="TextBlock">
        <Setter Property="FontSize" Value="17"/><Setter Property="FontWeight" Value="SemiBold"/><Setter Property="Margin" Value="0,0,0,12"/>
    </Style>
    <Style x:Key="FieldLabelStyle" TargetType="Label">
        <Setter Property="Padding" Value="0"/><Setter Property="Margin" Value="0,9,0,5"/><Setter Property="Foreground" Value="{StaticResource MutedTextBrush}"/>
    </Style>
    <Style TargetType="ListBox">
        <Setter Property="Background" Value="#FAFAFC"/><Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
        <Setter Property="BorderThickness" Value="1"/><Setter Property="ScrollViewer.CanContentScroll" Value="True"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="ListBox">
            <Border CornerRadius="9" Background="{TemplateBinding Background}" BorderBrush="{TemplateBinding BorderBrush}" BorderThickness="{TemplateBinding BorderThickness}" Padding="4">
                <ScrollViewer Focusable="False" Padding="{TemplateBinding Padding}"><ItemsPresenter/></ScrollViewer>
            </Border>
        </ControlTemplate></Setter.Value></Setter>
    </Style>
    <Style TargetType="ScrollBar">
        <Setter Property="Width" Value="9"/><Setter Property="Background" Value="Transparent"/>
        <Setter Property="Template"><Setter.Value><ControlTemplate TargetType="ScrollBar">
            <Grid Background="Transparent">
                <Track x:Name="PART_Track" Orientation="{TemplateBinding Orientation}" IsDirectionReversed="True"
                       Minimum="{TemplateBinding Minimum}" Maximum="{TemplateBinding Maximum}" ViewportSize="{TemplateBinding ViewportSize}"
                       Value="{Binding Value, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}">
                    <Track.DecreaseRepeatButton><RepeatButton x:Name="BeforeThumb" Command="ScrollBar.PageUpCommand" Focusable="False" Style="{x:Null}">
                        <RepeatButton.Template><ControlTemplate><Border Background="Transparent"/></ControlTemplate></RepeatButton.Template>
                    </RepeatButton></Track.DecreaseRepeatButton>
                    <Track.IncreaseRepeatButton><RepeatButton x:Name="AfterThumb" Command="ScrollBar.PageDownCommand" Focusable="False" Style="{x:Null}">
                        <RepeatButton.Template><ControlTemplate><Border Background="Transparent"/></ControlTemplate></RepeatButton.Template>
                    </RepeatButton></Track.IncreaseRepeatButton>
                    <Track.Thumb><Thumb x:Name="ScrollThumb" MinHeight="18"><Thumb.Template><ControlTemplate TargetType="Thumb">
                        <Border x:Name="Surface" CornerRadius="4" Margin="2" Background="#BCBCC2"/>
                        <ControlTemplate.Triggers><Trigger Property="IsMouseOver" Value="True"><Setter TargetName="Surface" Property="Background" Value="#98989D"/></Trigger></ControlTemplate.Triggers>
                    </ControlTemplate></Thumb.Template></Thumb></Track.Thumb>
                </Track>
            </Grid>
            <ControlTemplate.Triggers>
                <Trigger Property="Orientation" Value="Horizontal">
                    <Setter TargetName="PART_Track" Property="IsDirectionReversed" Value="False"/>
                    <Setter TargetName="BeforeThumb" Property="Command" Value="ScrollBar.PageLeftCommand"/>
                    <Setter TargetName="AfterThumb" Property="Command" Value="ScrollBar.PageRightCommand"/>
                    <Setter TargetName="ScrollThumb" Property="MinHeight" Value="0"/><Setter TargetName="ScrollThumb" Property="MinWidth" Value="18"/>
                </Trigger>
            </ControlTemplate.Triggers>
        </ControlTemplate></Setter.Value></Setter>
        <Style.Triggers><Trigger Property="Orientation" Value="Horizontal"><Setter Property="Height" Value="9"/><Setter Property="Width" Value="{x:Static sys:Double.NaN}"/></Trigger></Style.Triggers>
    </Style>
</ResourceDictionary>
```

## src/FolderThemeStudio.App/TutorialWindow.xaml

```xml
<controls:RoundedWindow x:Class="FolderThemeStudio.App.TutorialWindow" Style="{StaticResource {x:Type Window}}"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:controls="clr-namespace:FolderThemeStudio.App.Controls"
        Title="{DynamicResource Tutorial.Title}"
        Width="650" Height="430" MinWidth="520" MinHeight="360"
        WindowStartupLocation="CenterOwner" ResizeMode="CanResizeWithGrip">
    <Window.Icon>Assets/FolderThemeStudio.png</Window.Icon>
    <Grid Margin="24">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>
        <Grid>
            <Grid.ColumnDefinitions><ColumnDefinition Width="*" /><ColumnDefinition Width="Auto" /></Grid.ColumnDefinitions>
            <TextBlock FontSize="24" FontWeight="SemiBold" Text="{DynamicResource Tutorial.Title}" />
            <TextBlock Grid.Column="1" VerticalAlignment="Center" Foreground="{StaticResource MutedTextBrush}" Text="{Binding StepNumber}" />
        </Grid>
        <Border Grid.Row="1" Margin="0,20" Style="{StaticResource CardStyle}">
            <StackPanel VerticalAlignment="Center">
                <TextBlock FontSize="26" FontWeight="SemiBold" Text="{Binding CurrentStep.Title}" TextWrapping="Wrap" />
                <TextBlock Margin="0,16,0,0" FontSize="17" Foreground="{StaticResource MutedTextBrush}"
                           Text="{Binding CurrentStep.Body}" TextWrapping="Wrap" />
            </StackPanel>
        </Border>
        <Grid Grid.Row="2">
            <Grid.ColumnDefinitions><ColumnDefinition Width="Auto" /><ColumnDefinition Width="*" /><ColumnDefinition Width="Auto" /></Grid.ColumnDefinitions>
            <Button Command="{Binding SkipCommand}" Content="{DynamicResource Common.Skip}" />
            <StackPanel Grid.Column="2" Orientation="Horizontal">
                <Button Command="{Binding PreviousCommand}" Content="{DynamicResource Common.Previous}" />
                <Button Command="{Binding NextCommand}" Content="{DynamicResource Common.Next}" />
                <Button Style="{StaticResource PrimaryButtonStyle}" Command="{Binding FinishCommand}" Content="{DynamicResource Common.Finish}" />
            </StackPanel>
        </Grid>
    </Grid>
</controls:RoundedWindow>
```

## src/FolderThemeStudio.App/TutorialWindow.xaml.cs

```csharp
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
```

## src/FolderThemeStudio.App/ViewModels/AutoStyleRulesViewModel.cs

```csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.App.ViewModels;

public sealed class AutoStyleRulesViewModel : INotifyPropertyChanged
{
    private readonly IMonitoringRuleStore store;
    private readonly IFolderMonitoringCoordinator monitoring;
    private readonly IMonitoringIconAssetService assets;
    private readonly IMainViewModelDialogs dialogs;
    private readonly ILocalizationService localization;
    private readonly IViewModelDispatcher dispatcher;
    private readonly FolderTheme editorTheme;
    private readonly List<MonitoringRule> roots = [];
    private string? selectedRootPath;
    private AutoStyleRuleItem? selectedRule;
    private string keyword = string.Empty;
    private string? selectedImagePath;
    private BitmapSource? iconPreview;
    private string statusKey = "AutoRules.Status.Ready";
    private bool useEditorTheme = true;
    private bool enabled = true;
    private bool isBusy;
    private string? editingRuleId;

    public AutoStyleRulesViewModel(
        IMonitoringRuleStore store,
        IFolderMonitoringCoordinator monitoring,
        IMonitoringIconAssetService assets,
        IMainViewModelDialogs dialogs,
        ILocalizationService localization,
        IViewModelDispatcher dispatcher,
        FolderTheme editorTheme)
    {
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        this.monitoring = monitoring ?? throw new ArgumentNullException(nameof(monitoring));
        this.assets = assets ?? throw new ArgumentNullException(nameof(assets));
        this.dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));
        this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        this.editorTheme = editorTheme ?? throw new ArgumentNullException(nameof(editorTheme));

        AddNewRuleCommand = new RelayCommand(_ => BeginNewRule());
        SaveRuleCommand = new RelayCommand(_ => _ = SaveRuleAsync(), _ => CanSaveRule);
        CancelEditCommand = new RelayCommand(_ => CancelEdit());
        DeleteRuleCommand = new RelayCommand(parameter => _ = DeleteRuleAsync(parameter as string), parameter => parameter is string);
        MoveUpCommand = new RelayCommand(parameter => _ = MoveRuleAsync(parameter as string, -1), parameter => CanMove(parameter as string, -1));
        MoveDownCommand = new RelayCommand(parameter => _ = MoveRuleAsync(parameter as string, 1), parameter => CanMove(parameter as string, 1));
        ToggleEnabledCommand = new RelayCommand(parameter => _ = ToggleRuleAsync(parameter as string), parameter => parameter is string);
        ChooseImageCommand = new RelayCommand(_ => _ = ChooseImageAsync());
        localization.LanguageChanged += LanguageChanged;
        UpdatePreview();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<string> RootPaths { get; } = [];
    public ObservableCollection<AutoStyleRuleItem> Rules { get; } = [];
    public ICommand AddNewRuleCommand { get; }
    public ICommand SaveRuleCommand { get; }
    public ICommand CancelEditCommand { get; }
    public ICommand DeleteRuleCommand { get; }
    public ICommand MoveUpCommand { get; }
    public ICommand MoveDownCommand { get; }
    public ICommand ToggleEnabledCommand { get; }
    public ICommand ChooseImageCommand { get; }

    public string? SelectedRootPath
    {
        get => selectedRootPath;
        set
        {
            if (!SetField(ref selectedRootPath, value)) return;
            LoadSelectedRootRules();
            OnPropertyChanged(nameof(FallbackIconName));
            RefreshCommands();
        }
    }

    public AutoStyleRuleItem? SelectedRule
    {
        get => selectedRule;
        set
        {
            if (!SetField(ref selectedRule, value)) return;
            if (value is null) return;
            editingRuleId = value.Id;
            Keyword = value.Keyword;
            enabled = value.Enabled;
            OnPropertyChanged(nameof(Enabled));
            UseEditorTheme = false;
            SelectedImagePath = value.IcoPath;
            UpdatePreview();
            RefreshCommands();
        }
    }

    public string Keyword
    {
        get => keyword;
        set
        {
            if (SetField(ref keyword, value)) RefreshCommands();
        }
    }

    public bool Enabled
    {
        get => enabled;
        set => SetField(ref enabled, value);
    }

    public bool UseEditorTheme
    {
        get => useEditorTheme;
        set
        {
            if (!SetField(ref useEditorTheme, value)) return;
            UpdatePreview();
            RefreshCommands();
        }
    }

    public string? SelectedImagePath
    {
        get => selectedImagePath;
        private set
        {
            if (SetField(ref selectedImagePath, value))
            {
                if (!UseEditorTheme) UpdatePreview();
                RefreshCommands();
            }
        }
    }

    public BitmapSource? IconPreview
    {
        get => iconPreview;
        private set => SetField(ref iconPreview, value);
    }

    public string StatusText => localization.Get(statusKey);
    public string FallbackIconName =>
        Path.GetFileName(roots.FirstOrDefault(item => string.Equals(item.RootPath, SelectedRootPath, StringComparison.OrdinalIgnoreCase))?.IcoPath)
        ?? localization.Get("AutoRules.Status.NoRoots");
    public bool CanSaveRule => !IsBusy && !string.IsNullOrWhiteSpace(SelectedRootPath) &&
                               !string.IsNullOrWhiteSpace(Keyword) && (UseEditorTheme || !string.IsNullOrWhiteSpace(SelectedImagePath));
    public bool CanEdit => !IsBusy;
    public bool IsBusy
    {
        get => isBusy;
        private set
        {
            if (SetField(ref isBusy, value)) RefreshCommands();
        }
    }

    public async Task LoadAsync(CancellationToken token = default)
    {
        IsBusy = true;
        try
        {
            var loaded = await store.LoadAsync(token).ConfigureAwait(false);
            roots.Clear();
            roots.AddRange(loaded.Rules.OrderBy(rule => rule.RootPath, StringComparer.OrdinalIgnoreCase));
            await dispatcher.InvokeAsync(() =>
            {
                RootPaths.Clear();
                foreach (var root in roots) RootPaths.Add(root.RootPath);
                SelectedRootPath = RootPaths.FirstOrDefault();
                statusKey = RootPaths.Count == 0 ? "AutoRules.Status.NoRoots" : "AutoRules.Status.Ready";
                OnPropertyChanged(nameof(StatusText));
            });
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("AutoRules.Status.Error"));
        }
        finally { await dispatcher.InvokeAsync(() => IsBusy = false); }
    }

    public void BeginNewRule()
    {
        editingRuleId = null;
        SelectedRule = null;
        Keyword = string.Empty;
        Enabled = true;
        SelectedImagePath = null;
        UseEditorTheme = true;
        RefreshCommands();
    }

    public void CancelEdit()
    {
        if (selectedRule is null)
        {
            BeginNewRule();
            return;
        }

        editingRuleId = selectedRule.Id;
        Keyword = selectedRule.Keyword;
        enabled = selectedRule.Enabled;
        OnPropertyChanged(nameof(Enabled));
        useEditorTheme = false;
        OnPropertyChanged(nameof(UseEditorTheme));
        selectedImagePath = selectedRule.IcoPath;
        OnPropertyChanged(nameof(SelectedImagePath));
        UpdatePreview();
        RefreshCommands();
    }

    public async Task ChooseImageAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        var path = await dialogs.PickImageImportPathAsync().ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(path)) return;
        await dispatcher.InvokeAsync(() =>
        {
            UseEditorTheme = false;
            SelectedImagePath = path;
            SetStatus("AutoRules.Status.ImageSelected");
        });
    }

    public async Task<bool> SaveRuleAsync(CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(SelectedRootPath))
        {
            SetStatus("AutoRules.Status.NoRoots");
            return false;
        }
        if (string.IsNullOrWhiteSpace(Keyword))
        {
            SetStatus("AutoRules.Status.KeywordRequired");
            return false;
        }
        if (!UseEditorTheme && string.IsNullOrWhiteSpace(SelectedImagePath))
        {
            SetStatus("AutoRules.Status.ImageRequired");
            return false;
        }

        var rootPath = SelectedRootPath!;
        var root = roots.FirstOrDefault(item => string.Equals(item.RootPath, rootPath, StringComparison.OrdinalIgnoreCase));
        if (root is null)
        {
            SetStatus("AutoRules.Status.NoRoots");
            return false;
        }
        var request = new SaveRuleRequest(
            rootPath,
            root,
            editingRuleId ?? Guid.NewGuid().ToString("N"),
            Keyword.Trim(),
            Enabled,
            UseEditorTheme,
            SelectedImagePath,
            Rules.Select(item => item.ToRule()).ToList());

        IsBusy = true;
        try
        {
            var original = request.Rules.FirstOrDefault(item => item.Id == request.RuleId);
            var reuseOriginalIcon = original is not null && !request.UseEditorTheme &&
                !string.IsNullOrWhiteSpace(request.SelectedImagePath) &&
                File.Exists(original.IcoPath) && PathsEqual(request.SelectedImagePath, original.IcoPath);
            var icoPath = reuseOriginalIcon
                ? original!.IcoPath
                : await PersistSelectedIconAsync(request.UseEditorTheme, request.SelectedImagePath, token).ConfigureAwait(false);

            var updatedRules = request.Rules.ToList();
            var savedRule = new FolderNameStyleRule(request.RuleId, request.Keyword, icoPath, request.Enabled, updatedRules.Count);
            var oldIndex = updatedRules.FindIndex(item => item.Id == request.RuleId);
            if (oldIndex >= 0) updatedRules[oldIndex] = savedRule;
            else updatedRules.Add(savedRule);
            updatedRules = updatedRules.Select((item, index) => item with { Order = index }).ToList();
            var updatedRoot = request.Root with { NameRules = updatedRules, UpdatedAtUtc = DateTimeOffset.UtcNow };

            await monitoring.ReplaceRuleAsync(updatedRoot, token).ConfigureAwait(false);
            await dispatcher.InvokeAsync(() =>
            {
                var rootIndex = roots.FindIndex(item => string.Equals(item.RootPath, request.RootPath, StringComparison.OrdinalIgnoreCase));
                if (rootIndex >= 0) roots[rootIndex] = updatedRoot;
                if (string.Equals(SelectedRootPath, request.RootPath, StringComparison.OrdinalIgnoreCase))
                {
                    LoadSelectedRootRules();
                    SelectedRule = Rules.FirstOrDefault(item => item.Id == request.RuleId);
                }
                SetStatus("AutoRules.Status.Saved");
            });
            return true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or ArgumentException or NotSupportedException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("AutoRules.Status.Error"));
            return false;
        }
        finally { await dispatcher.InvokeAsync(() => IsBusy = false); }
    }

    public async Task DeleteRuleAsync(string? id, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        var index = Rules.ToList().FindIndex(rule => rule.Id == id);
        if (index < 0) return;
        Rules.RemoveAt(index);
        OnPropertyChanged(nameof(IsEmpty));
        if (editingRuleId == id) BeginNewRule();
        await NormalizeAndPersistAsync(token).ConfigureAwait(false);
        await dispatcher.InvokeAsync(() => SetStatus("AutoRules.Status.Deleted"));
    }

    public async Task MoveRuleAsync(string? id, int offset, CancellationToken token = default)
    {
        if (offset is not (-1 or 1) || string.IsNullOrWhiteSpace(id)) return;
        var index = Rules.ToList().FindIndex(rule => rule.Id == id);
        var target = index + offset;
        if (index < 0 || target < 0 || target >= Rules.Count) return;
        var item = Rules[index];
        Rules.RemoveAt(index);
        Rules.Insert(target, item);
        await NormalizeAndPersistAsync(token).ConfigureAwait(false);
    }

    public async Task ToggleRuleAsync(string? id, CancellationToken token = default)
    {
        var item = Rules.FirstOrDefault(rule => rule.Id == id);
        if (item is null) return;
        item.Enabled = !item.Enabled;
        await NormalizeAndPersistAsync(token).ConfigureAwait(false);
    }

    private async Task NormalizeAndPersistAsync(CancellationToken token)
    {
        MonitoringRule? updated = null;
        int rootIndex = -1;
        await dispatcher.InvokeAsync(() =>
        {
            var root = roots.FirstOrDefault(item => string.Equals(item.RootPath, SelectedRootPath, StringComparison.OrdinalIgnoreCase));
            if (root is null) return;
            for (var index = 0; index < Rules.Count; index++) Rules[index].Order = index;
            updated = root with
            {
                NameRules = Rules.Select(item => item.ToRule()).ToArray(),
                UpdatedAtUtc = DateTimeOffset.UtcNow
            };
            rootIndex = roots.FindIndex(item => string.Equals(item.RootPath, updated.RootPath, StringComparison.OrdinalIgnoreCase));
        });
        if (updated is null) return;
        await monitoring.ReplaceRuleAsync(updated, token).ConfigureAwait(false);
        await dispatcher.InvokeAsync(() => roots[rootIndex] = updated);
    }

    private async Task<string> PersistSelectedIconAsync(bool useTheme, string? imagePath, CancellationToken token)
    {
        var frames = new Dictionary<int, byte[]>();
        foreach (var size in FolderIconRenderer.RequiredSizes)
        {
            token.ThrowIfCancellationRequested();
            var bitmap = useTheme
                ? FolderIconRenderer.Render(editorTheme, size)
                : ImportedIconRenderer.Render(imagePath!, size);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using var stream = new MemoryStream();
            encoder.Save(stream);
            frames.Add(size, stream.ToArray());
        }
        var ico = IcoEncoder.Encode(frames);
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), "FolderThemeStudio", "auto-rules");
        Directory.CreateDirectory(temporaryDirectory);
        var temporaryPath = Path.Combine(temporaryDirectory, Guid.NewGuid().ToString("N") + ".ico");
        try
        {
            await File.WriteAllBytesAsync(temporaryPath, ico, token).ConfigureAwait(false);
            return await assets.PersistAsync(temporaryPath, token).ConfigureAwait(false);
        }
        finally { if (File.Exists(temporaryPath)) File.Delete(temporaryPath); }
    }

    private void LoadSelectedRootRules()
    {
        Rules.Clear();
        selectedRule = null;
        editingRuleId = null;
        var root = roots.FirstOrDefault(item => string.Equals(item.RootPath, SelectedRootPath, StringComparison.OrdinalIgnoreCase));
        if (root is not null)
        {
            foreach (var rule in root.NameRules.OrderBy(item => item.Order))
                Rules.Add(AutoStyleRuleItem.FromRule(rule, LoadIcoPreview(rule.IcoPath)));
        }
        BeginNewRule();
        OnPropertyChanged(nameof(IsEmpty));
        RefreshCommands();
    }

    private bool CanMove(string? id, int offset)
    {
        var index = Rules.ToList().FindIndex(rule => rule.Id == id);
        return index >= 0 && index + offset >= 0 && index + offset < Rules.Count;
    }

    public bool IsEmpty => Rules.Count == 0;

    private void UpdatePreview()
    {
        try
        {
            var bitmap = UseEditorTheme
                ? FolderIconRenderer.Render(editorTheme, 64)
                : string.IsNullOrWhiteSpace(SelectedImagePath) ? null : ImportedIconRenderer.Render(SelectedImagePath, 64);
            IconPreview = bitmap;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
        {
            IconPreview = null;
        }
    }

    private static BitmapSource? LoadIcoPreview(string path)
    {
        if (!File.Exists(path)) return null;
        try
        {
            using var stream = File.OpenRead(path);
            var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            var frame = decoder.Frames[0];
            frame.Freeze();
            return frame;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or NotSupportedException or ArgumentException)
        {
            return null;
        }
    }

    private static bool PathsEqual(string left, string right)
    {
        try
        {
            return string.Equals(Path.GetFullPath(left), Path.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception exception) when (exception is ArgumentException or IOException or NotSupportedException)
        {
            return false;
        }
    }

    private void SetStatus(string key)
    {
        statusKey = key;
        OnPropertyChanged(nameof(StatusText));
    }

    private void LanguageChanged(object? sender, EventArgs e) => OnPropertyChanged(nameof(StatusText));

    private void RefreshCommands()
    {
        OnPropertyChanged(nameof(CanSaveRule));
        OnPropertyChanged(nameof(CanEdit));
        (SaveRuleCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (MoveUpCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (MoveDownCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private sealed record SaveRuleRequest(
        string RootPath,
        MonitoringRule Root,
        string RuleId,
        string Keyword,
        bool Enabled,
        bool UseEditorTheme,
        string? SelectedImagePath,
        IReadOnlyList<FolderNameStyleRule> Rules);
}

public sealed class AutoStyleRuleItem : INotifyPropertyChanged
{
    private bool enabled;
    private int order;

    public AutoStyleRuleItem(string id, string keyword, string icoPath, bool enabled, int order, BitmapSource? preview)
    {
        Id = id;
        Keyword = keyword;
        IcoPath = icoPath;
        this.enabled = enabled;
        this.order = order;
        Preview = preview;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public string Id { get; }
    public string Keyword { get; }
    public string IcoPath { get; }
    public string IconName => Path.GetFileName(IcoPath);
    public BitmapSource? Preview { get; }
    public bool Enabled { get => enabled; set { if (enabled != value) { enabled = value; PropertyChanged?.Invoke(this, new(nameof(Enabled))); } } }
    public int Order { get => order; set { if (order != value) { order = value; PropertyChanged?.Invoke(this, new(nameof(Order))); } } }

    internal FolderNameStyleRule ToRule() => new(Id, Keyword, IcoPath, Enabled, Order);
    internal static AutoStyleRuleItem FromRule(FolderNameStyleRule rule, BitmapSource? preview) =>
        new(rule.Id, rule.Keyword, rule.IcoPath, rule.Enabled, rule.Order, preview);
}
```

## src/FolderThemeStudio.App/ViewModels/ColorPickerViewModel.cs

```csharp
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace FolderThemeStudio.App.ViewModels;

public sealed class ColorPickerViewModel : INotifyPropertyChanged
{
    private double hue;
    private double saturation;
    private double value;
    private string hex = "#000000";
    private bool isValid;
    private bool updating;
    private System.Windows.Media.Brush selectedBrush = System.Windows.Media.Brushes.Black;
    private System.Windows.Media.Brush hueBrush = System.Windows.Media.Brushes.Red;

    public ColorPickerViewModel(string initialHex)
    {
        if (!TrySetHex(initialHex))
        {
            throw new ArgumentException("Initial color must use RGB hexadecimal format.", nameof(initialHex));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public double Hue
    {
        get => hue;
        set
        {
            var normalized = ((value % 360) + 360) % 360;
            if (SetField(ref hue, normalized) && !updating) UpdateHexFromHsv();
        }
    }

    public double Saturation
    {
        get => saturation;
        set
        {
            if (SetField(ref saturation, Math.Clamp(value, 0, 1)) && !updating) UpdateHexFromHsv();
        }
    }

    public double Value
    {
        get => value;
        set
        {
            if (SetField(ref this.value, Math.Clamp(value, 0, 1)) && !updating) UpdateHexFromHsv();
        }
    }

    public string Hex
    {
        get => hex;
        set
        {
            if (TryNormalizeHex(value, out var normalized))
            {
                SetFromHex(normalized);
                return;
            }

            SetField(ref hex, value ?? string.Empty);
            SetField(ref isValid, false, nameof(IsValid));
        }
    }

    public bool IsValid => isValid;
    public System.Windows.Media.Brush SelectedBrush => selectedBrush;
    public System.Windows.Media.Brush HueBrush => hueBrush;

    public bool TrySetHex(string value)
    {
        if (!TryNormalizeHex(value, out var normalized))
        {
            return false;
        }

        SetFromHex(normalized);
        return true;
    }

    public void SetPalettePoint(double x, double y, double width, double height)
    {
        var selected = VisualColorMath.FromPalettePoint(x, y, width, height);
        Saturation = selected.Saturation;
        Value = selected.Value;
    }

    public void SetHuePoint(double y, double height) =>
        Hue = VisualColorMath.HueFromPoint(y, height);

    private void SetFromHex(string normalized)
    {
        var red = byte.Parse(normalized.AsSpan(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var green = byte.Parse(normalized.AsSpan(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var blue = byte.Parse(normalized.AsSpan(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        RgbToHsv(red, green, blue, out var nextHue, out var nextSaturation, out var nextValue);

        updating = true;
        try
        {
            SetField(ref hue, nextHue, nameof(Hue));
            SetField(ref saturation, nextSaturation, nameof(Saturation));
            SetField(ref value, nextValue, nameof(Value));
            SetField(ref hex, normalized, nameof(Hex));
            SetField(ref isValid, true, nameof(IsValid));
            SetBrush(red, green, blue);
            SetHueBrush(nextHue);
        }
        finally
        {
            updating = false;
        }
    }

    private void UpdateHexFromHsv()
    {
        HsvToRgb(Hue, Saturation, Value, out var red, out var green, out var blue);
        var normalized = $"#{red:X2}{green:X2}{blue:X2}";
        SetField(ref hex, normalized, nameof(Hex));
        SetField(ref isValid, true, nameof(IsValid));
        SetBrush(red, green, blue);
        SetHueBrush(Hue);
    }

    private void SetHueBrush(double selectedHue)
    {
        HsvToRgb(selectedHue, 1, 1, out var red, out var green, out var blue);
        var brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(red, green, blue));
        brush.Freeze();
        hueBrush = brush;
        OnPropertyChanged(nameof(HueBrush));
    }

    private void SetBrush(byte red, byte green, byte blue)
    {
        var brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(red, green, blue));
        brush.Freeze();
        selectedBrush = brush;
        OnPropertyChanged(nameof(SelectedBrush));
    }

    private static bool TryNormalizeHex(string? input, out string normalized)
    {
        var value = (input ?? string.Empty).Trim();
        if (!value.StartsWith('#')) value = "#" + value;
        if (value.Length == 4)
        {
            value = $"#{value[1]}{value[1]}{value[2]}{value[2]}{value[3]}{value[3]}";
        }

        normalized = value.ToUpperInvariant();
        return normalized.Length == 7 && normalized.AsSpan(1).ToString().All(Uri.IsHexDigit);
    }

    private static void RgbToHsv(byte red, byte green, byte blue, out double hue, out double saturation, out double value)
    {
        var r = red / 255d;
        var g = green / 255d;
        var b = blue / 255d;
        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var delta = max - min;
        value = max;
        saturation = max == 0 ? 0 : delta / max;
        if (delta == 0) hue = 0;
        else if (max == r) hue = 60 * (((g - b) / delta) % 6);
        else if (max == g) hue = 60 * (((b - r) / delta) + 2);
        else hue = 60 * (((r - g) / delta) + 4);
        if (hue < 0) hue += 360;
    }

    private static void HsvToRgb(double hue, double saturation, double value, out byte red, out byte green, out byte blue)
    {
        var chroma = value * saturation;
        var x = chroma * (1 - Math.Abs(((hue / 60) % 2) - 1));
        var m = value - chroma;
        var sector = (int)Math.Floor(hue / 60) % 6;
        var (r, g, b) = sector switch
        {
            0 => (chroma, x, 0d),
            1 => (x, chroma, 0d),
            2 => (0d, chroma, x),
            3 => (0d, x, chroma),
            4 => (x, 0d, chroma),
            _ => (chroma, 0d, x),
        };
        red = (byte)Math.Round((r + m) * 255);
        green = (byte)Math.Round((g + m) * 255);
        blue = (byte)Math.Round((b + m) * 255);
    }

    private bool SetField<T>(ref T field, T next, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, next)) return false;
        field = next;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

## src/FolderThemeStudio.App/ViewModels/FolderCategoriesViewModel.cs

```csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.App.Settings;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace FolderThemeStudio.App.ViewModels;

public sealed class FolderCategoriesViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly IFolderCategoryCatalog catalog;
    private readonly IFolderHoverMonitorService monitor;
    private readonly IMainViewModelDialogs dialogs;
    private readonly ILocalizationService localization;
    private readonly IViewModelDispatcher dispatcher;
    private FolderCategoryEntry? selectedEntry;
    private string folderPath = string.Empty;
    private string category = string.Empty;
    private string note = string.Empty;
    private string statusKey = "FolderCategories.Status.Ready";
    private bool isBusy;

    public FolderCategoriesViewModel(
        IFolderCategoryCatalog catalog,
        IFolderHoverMonitorService monitor,
        IMainViewModelDialogs dialogs,
        ILocalizationService localization,
        IViewModelDispatcher dispatcher)
    {
        this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        this.monitor = monitor ?? throw new ArgumentNullException(nameof(monitor));
        this.dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));
        this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        GradientStartPicker = new ColorPickerViewModel("#2868D7");
        GradientEndPicker = new ColorPickerViewModel("#2868D7");
        GradientStartPicker.PropertyChanged += PickerChanged;
        GradientEndPicker.PropertyChanged += PickerChanged;
        PresetColors = new ObservableCollection<string>(AppleColors.All.Select(color => color.Hex));
        PickFolderCommand = new RelayCommand(_ => _ = PickFolderAsync(), _ => !IsBusy);
        NewEntryCommand = new RelayCommand(_ => BeginNewEntry(), _ => !IsBusy);
        SaveCommand = new RelayCommand(_ => _ = SaveAsync(), _ => CanSave);
        RemoveCommand = new RelayCommand(_ => _ = RemoveAsync(), _ => !IsBusy && SelectedEntry is not null);
        localization.LanguageChanged += LanguageChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<FolderCategoryEntry> Entries { get; } = [];
    public ObservableCollection<string> PresetColors { get; }
    public ColorPickerViewModel GradientStartPicker { get; }
    public ColorPickerViewModel GradientEndPicker { get; }
    public ICommand PickFolderCommand { get; }
    public ICommand NewEntryCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand RemoveCommand { get; }

    public void Dispose()
    {
        localization.LanguageChanged -= LanguageChanged;
        GradientStartPicker.PropertyChanged -= PickerChanged;
        GradientEndPicker.PropertyChanged -= PickerChanged;
    }

    public FolderCategoryEntry? SelectedEntry
    {
        get => selectedEntry;
        set
        {
            if (!SetField(ref selectedEntry, value)) return;
            if (value is null)
            {
                RefreshCommands();
                return;
            }
            FolderPath = value.FolderPath;
            Category = value.Category;
            Note = value.Note;
            GradientStartPicker.Hex = value.GradientStartHex;
            GradientEndPicker.Hex = value.GradientEndHex;
            RefreshCommands();
        }
    }

    public string FolderPath { get => folderPath; private set { if (SetField(ref folderPath, value)) { OnPropertyChanged(nameof(FolderPathDisplay)); OnPropertyChanged(nameof(FolderName)); RefreshCommands(); } } }
    public string FolderName => Path.GetFileName(Path.TrimEndingDirectorySeparator(FolderPath));
    public string FolderPathDisplay => string.IsNullOrWhiteSpace(FolderPath) ? localization.Get("FolderCategories.Form.NoFolder") : FolderPath;
    public string Category { get => category; set { if (SetField(ref category, value)) RefreshCommands(); } }
    public string Note { get => note; set { if (SetField(ref note, value)) RefreshCommands(); } }
    public string ColorHex
    {
        get => GradientStartPicker.Hex;
        set
        {
            if (value is null) return;
            GradientStartPicker.Hex = value;
            GradientEndPicker.Hex = value;
            OnPropertyChanged(nameof(AccentPreview));
            RefreshCommands();
        }
    }
    public Brush AccentPreview
    {
        get
        {
            try
            {
                return new LinearGradientBrush(
                    (Color)ColorConverter.ConvertFromString(GradientStartPicker.Hex),
                    (Color)ColorConverter.ConvertFromString(GradientEndPicker.Hex),
                    new System.Windows.Point(0, 0), new System.Windows.Point(1, 1));
            }
            catch (Exception exception) when (exception is FormatException or ArgumentException or NotSupportedException)
            {
                return new SolidColorBrush(Color.FromRgb(40, 104, 215));
            }
        }
    }
    public string StatusText => localization.Get(statusKey);
    public bool IsEmpty => Entries.Count == 0;
    public bool CanEdit => !IsBusy;
    public bool CanSave => !IsBusy && !string.IsNullOrWhiteSpace(FolderPath) &&
                           !string.IsNullOrWhiteSpace(Category) && Category.Trim().Length <= 32 && Note.Trim().Length <= 140 &&
                           IsColorValid(GradientStartPicker.Hex) && IsColorValid(GradientEndPicker.Hex);
    public bool IsBusy
    {
        get => isBusy;
        private set
        {
            if (SetField(ref isBusy, value)) RefreshCommands();
        }
    }

    public async Task LoadAsync(CancellationToken token = default)
    {
        IsBusy = true;
        try
        {
            var entries = await catalog.LoadAsync(token).ConfigureAwait(false);
            await dispatcher.InvokeAsync(() =>
            {
                ReplaceEntries(entries);
                SetStatus("FolderCategories.Status.Ready");
            });
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or ArgumentException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("FolderCategories.Status.Error"));
        }
        finally { IsBusy = false; }
    }

    public async Task PickFolderAsync(CancellationToken token = default)
    {
        if (IsBusy) return;
        var path = await dialogs.PickFolderAsync();
        if (string.IsNullOrWhiteSpace(path)) return;
        SelectFolder(path);
    }

    public void SelectFolder(string path)
    {
        var existing = Entries.FirstOrDefault(item => string.Equals(item.FolderPath, Path.GetFullPath(path), StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
        {
            SelectedEntry = existing;
            SetStatus("FolderCategories.Status.Selected");
            return;
        }

        SelectedEntry = null;
        FolderPath = Path.GetFullPath(path);
        Category = string.Empty;
        Note = string.Empty;
        ColorHex = PresetColors[0];
        SetStatus("FolderCategories.Status.New");
    }

    public void BeginNewEntry()
    {
        SelectedEntry = null;
        FolderPath = string.Empty;
        Category = string.Empty;
        Note = string.Empty;
        ColorHex = PresetColors[0];
        SetStatus("FolderCategories.Status.Ready");
    }

    public void SelectFolderForQuickNote(string path)
    {
        SelectFolder(path);
        if (string.IsNullOrWhiteSpace(Category))
        {
            var name = FolderName;
            Category = string.IsNullOrWhiteSpace(name) ? "Folder" : name[..Math.Min(name.Length, 32)];
        }
    }

    public void ChoosePresetColor(string? color)
    {
        if (color is not null && PresetColors.Contains(color, StringComparer.OrdinalIgnoreCase)) ColorHex = color;
    }

    public async Task<bool> SaveAsync(CancellationToken token = default)
    {
        if (!CanSave) return false;
        IsBusy = true;
        try
        {
            var entry = new FolderCategoryEntry(FolderPath, Category.Trim(), Note.Trim(), GradientStartPicker.Hex.Trim(), GradientEndPicker.Hex.Trim());
            await catalog.UpsertAsync(entry, token).ConfigureAwait(false);
            var updated = await catalog.LoadAsync(token).ConfigureAwait(false);
            await monitor.ReloadCatalogAsync(token).ConfigureAwait(false);
            await dispatcher.InvokeAsync(() =>
            {
                ReplaceEntries(updated);
                SelectedEntry = Entries.FirstOrDefault(item => string.Equals(item.FolderPath, entry.FolderPath, StringComparison.OrdinalIgnoreCase));
                SetStatus("FolderCategories.Status.Saved");
            });
            return true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or ArgumentException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("FolderCategories.Status.Error"));
            return false;
        }
        finally { await dispatcher.InvokeAsync(() => IsBusy = false); }
    }

    public async Task RemoveAsync(CancellationToken token = default)
    {
        if (IsBusy || SelectedEntry is not { } selected) return;
        IsBusy = true;
        try
        {
            await catalog.RemoveAsync(selected.FolderPath, token).ConfigureAwait(false);
            var updated = await catalog.LoadAsync(token).ConfigureAwait(false);
            await monitor.ReloadCatalogAsync(token).ConfigureAwait(false);
            await dispatcher.InvokeAsync(() =>
            {
                ReplaceEntries(updated);
                BeginNewEntry();
                SetStatus("FolderCategories.Status.Removed");
            });
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or ArgumentException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("FolderCategories.Status.Error"));
        }
        finally { await dispatcher.InvokeAsync(() => IsBusy = false); }
    }

    private void ReplaceEntries(IEnumerable<FolderCategoryEntry> entries)
    {
        Entries.Clear();
        foreach (var entry in entries.OrderBy(item => item.FolderPath, StringComparer.OrdinalIgnoreCase)) Entries.Add(entry);
        OnPropertyChanged(nameof(IsEmpty));
    }

    private static bool IsColorValid(string value) => value.Length == 7 && value[0] == '#' && value.AsSpan(1).ToString().All(Uri.IsHexDigit);

    private void SetStatus(string key)
    {
        statusKey = key;
        OnPropertyChanged(nameof(StatusText));
    }

    private void LanguageChanged(object? sender, EventArgs e) => OnPropertyChanged(nameof(StatusText));

    private void PickerChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(ColorPickerViewModel.Hex)) return;
        OnPropertyChanged(nameof(ColorHex));
        OnPropertyChanged(nameof(AccentPreview));
        RefreshCommands();
    }

    private void RefreshCommands()
    {
        OnPropertyChanged(nameof(CanSave));
        OnPropertyChanged(nameof(CanEdit));
        (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (PickFolderCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (NewEntryCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (RemoveCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
```

## src/FolderThemeStudio.App/ViewModels/MainViewModel.cs

```csharp
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Themes;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.Monitoring;

namespace FolderThemeStudio.App.ViewModels;

public interface IThemeApplicationCoordinator
{
    Task<ApplyPlan> PlanAsync(ApplyRequest request, CancellationToken token);
    Task<ApplyResult> ApplyAsync(ApplyPlan plan, IProgress<ApplyProgress> progress, CancellationToken token);
    Task<RestoreResult> RestoreLatestAsync(IProgress<ApplyProgress> progress, CancellationToken token);
}

public interface IMainViewModelDialogs
{
    Task<bool> ConfirmAsync(string title, string message);
    Task<string?> PickFolderAsync();
    Task<string?> PickThemeImportPathAsync();
    Task<string?> PickImageImportPathAsync();
    Task<string?> PickThemeExportPathAsync(string suggestedFileName);
    void CopyText(string text);
    Task ExportTextAsync(string suggestedFileName, string text);
    void ShowError(string title, string message);
}

public interface IThemeCatalog
{
    Task<ThemeLoadResult> LoadAsync();
    Task SaveAsync(FolderTheme theme);
    Task<ThemeImportResult> ImportAsync(string sourcePath);
    Task ExportAsync(Guid themeId, string destinationPath);
    Task DeleteAsync(Guid themeId);
}

public interface IPreviewRenderer
{
    BitmapSource Render(IconSource source, int size);
}

public interface IPreviewDelay
{
    Task DelayAsync(TimeSpan delay, CancellationToken token);
}

public interface IPreviewWorkScheduler : IDisposable
{
    Task<T> RunAsync<T>(Func<T> work, CancellationToken token);
}

public interface IViewModelDispatcher
{
    Task InvokeAsync(Action action);
}

public enum ResultKind
{
    Success,
    RolledBack,
    StateUncertain,
    Skipped,
    Failed
}

public enum IconEditorMode
{
    BuiltIn,
    ImportedImage
}

public sealed record OperationResultItem(ResultKind Kind, string? Path, string Detail)
{
    public string StatusLabel => Kind switch
    {
        ResultKind.Success => "Succeeded",
        ResultKind.RolledBack => "Rolled back",
        ResultKind.StateUncertain => "State uncertain",
        ResultKind.Skipped => "Skipped",
        _ => "Failed"
    };
}

public sealed class PreviewImageViewModel(int size) : INotifyPropertyChanged
{
    private BitmapSource? image;

    public int Size { get; } = size;

    public BitmapSource? Image
    {
        get => image;
        internal set
        {
            if (ReferenceEquals(image, value))
            {
                return;
            }

            image = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}

public sealed class MainViewModel : INotifyPropertyChanged, IDisposable
{
    private static readonly TimeSpan PreviewDebounce = TimeSpan.FromMilliseconds(100);
    private static readonly int[] PreviewSizes = [16, 32, 64, 256];

    private readonly IThemeApplicationCoordinator coordinator;
    private readonly IMainViewModelDialogs dialogs;
    private readonly IThemeCatalog themes;
    private readonly IPreviewRenderer previewRenderer;
    private readonly IPreviewDelay previewDelay;
    private readonly IViewModelDispatcher dispatcher;
    private readonly IPreviewWorkScheduler previewWorkScheduler;
    private readonly IImportedImageService importedImageService;
    private readonly IFolderMonitoringCoordinator folderMonitoring;
    private readonly IMonitoringIconAssetService monitoringAssets;
    private readonly RelayCommand applyCommand;
    private readonly RelayCommand restoreCommand;
    private readonly RelayCommand planCommand;
    private readonly RelayCommand addRootCommand;
    private readonly RelayCommand removeRootCommand;
    private readonly RelayCommand saveThemeCommand;
    private readonly RelayCommand importThemeCommand;
    private readonly RelayCommand exportThemeCommand;
    private readonly RelayCommand deleteThemeCommand;
    private readonly RelayCommand copyFailedPathsCommand;
    private readonly RelayCommand exportFailedPathsCommand;
    private readonly RelayCommand chooseImportedImageCommand;
    private readonly RelayCommand clearImportedImageCommand;
    private readonly RelayCommand toggleMonitoringCommand;
    private readonly RelayCommand removeMonitoringRuleCommand;
    private CancellationTokenSource? previewCancellation;
    private CancellationTokenSource? operationCancellation;
    private Task previewCompletion = Task.CompletedTask;
    private Guid themeId;
    private string themeName;
    private string gradientStart;
    private string gradientEnd;
    private double gradientAngle;
    private double opacity;
    private double cornerRadius;
    private string strokeColor;
    private double strokeOpacity;
    private double strokeWidth;
    private double highlightStrength;
    private string glowColor;
    private double glowStrength;
    private double glowRadius;
    private double shadowStrength;
    private double shadowOffset;
    private MaterialStyle material;
    private StickerStyle sticker;
    private string stickerColor;
    private double stickerX;
    private double stickerY;
    private double stickerScale;
    private double stickerRotation;
    private string desktopBackground = "DefaultWallpaper";
    private double desktopIconSize = 64;
    private FolderTheme? selectedSavedTheme;
    private ApplicationMode selectedMode;
    private bool isThemeValid;
    private string validationMessage = string.Empty;
    private bool isPreviewRendering;
    private bool isPlanning;
    private bool isApplying;
    private bool isRestoring;
    private bool isManagingThemes;
    private int planAllowedCount;
    private int planSkippedCount;
    private string planDetails = "Plan not calculated yet.";
    private string progressMessage = "Ready.";
    private int progressCompleted;
    private int progressTotal;
    private string operationStatus = "No changes have been applied.";
    private string resultSummary = "No results yet.";
    private IReadOnlyList<string> serviceValidationErrors = [];
    private long inputRevision;
    private ApplyPlan? reviewedPlan;
    private ApplyRequest? reviewedRequest;
    private long reviewedPlanRevision = -1;
    private IconEditorMode selectedIconEditorMode;
    private string? importedImageAssetPath;
    private BitmapSource? importedImagePreview;
    private bool syncingPalette;
    private bool continueApplyingToNewFolders;

    public MainViewModel(
        IThemeApplicationCoordinator coordinator,
        IMainViewModelDialogs dialogs,
        IThemeCatalog themes,
        IPreviewRenderer previewRenderer,
        IPreviewDelay previewDelay,
        IViewModelDispatcher dispatcher,
        IPreviewWorkScheduler? previewWorkScheduler = null,
        IImportedImageService? importedImageService = null,
        IFolderMonitoringCoordinator? folderMonitoring = null,
        IMonitoringIconAssetService? monitoringAssets = null)
    {
        this.coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
        this.dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));
        this.themes = themes ?? throw new ArgumentNullException(nameof(themes));
        this.previewRenderer = previewRenderer ?? throw new ArgumentNullException(nameof(previewRenderer));
        this.previewDelay = previewDelay ?? throw new ArgumentNullException(nameof(previewDelay));
        this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        this.previewWorkScheduler = previewWorkScheduler ?? new StaPreviewWorkScheduler();
        this.importedImageService = importedImageService ?? new ImportedImageService();
        this.folderMonitoring = folderMonitoring ?? new NullFolderMonitoringCoordinator();
        this.monitoringAssets = monitoringAssets ?? new PassthroughMonitoringIconAssetService();
        this.folderMonitoring.OutcomeProduced += MonitoringOutcomeProduced;

        var initial = FolderTheme.IceBlue;
        themeId = initial.Id;
        themeName = initial.Name;
        gradientStart = initial.GradientStart;
        gradientEnd = initial.GradientEnd;
        gradientAngle = initial.GradientAngle;
        opacity = initial.Opacity;
        cornerRadius = initial.CornerRadius;
        strokeColor = initial.StrokeColor;
        strokeOpacity = initial.StrokeOpacity;
        strokeWidth = initial.StrokeWidth;
        highlightStrength = initial.HighlightStrength;
        glowColor = initial.GlowColor;
        glowStrength = initial.GlowStrength;
        glowRadius = initial.GlowRadius;
        shadowStrength = initial.ShadowStrength;
        shadowOffset = initial.ShadowOffset;
        material = initial.Material;
        sticker = initial.Sticker;
        stickerColor = initial.StickerColor;
        stickerX = initial.StickerX;
        stickerY = initial.StickerY;
        stickerScale = initial.StickerScale;
        stickerRotation = initial.StickerRotation;
        selectedSavedTheme = initial;
        PaletteEditor = new PaletteEditorViewModel(new FolderPalette(
            initial.GradientStart, initial.GradientEnd, initial.StrokeColor, initial.GlowColor));
        PaletteEditor.PaletteChanged += PaletteEditorChanged;

        Previews = new ObservableCollection<PreviewImageViewModel>(
            PreviewSizes.Select(size => new PreviewImageViewModel(size)));
        SavedThemes = [initial];
        SelectedRoots.CollectionChanged += SelectedRootsChanged;

        applyCommand = new RelayCommand(_ => _ = ApplyAsync(), _ => CanApply);
        restoreCommand = new RelayCommand(_ => _ = RestoreAsync(), _ => !IsBusy);
        planCommand = new RelayCommand(_ => _ = PlanAsync(), _ => CanPlan);
        addRootCommand = new RelayCommand(_ => _ = AddRootAsync(), _ => !IsBusy);
        removeRootCommand = new RelayCommand(RemoveRoot, parameter => !IsBusy && parameter is string);
        saveThemeCommand = new RelayCommand(_ => _ = SaveThemeAsync(), _ => IsThemeValid && !IsBusy);
        importThemeCommand = new RelayCommand(_ => _ = ImportThemeAsync(), _ => !IsBusy);
        exportThemeCommand = new RelayCommand(_ => _ = ExportThemeAsync(), _ => SelectedSavedTheme is not null && !IsBusy);
        deleteThemeCommand = new RelayCommand(_ => _ = DeleteThemeAsync(), _ => SelectedSavedTheme is not null && !IsBusy);
        copyFailedPathsCommand = new RelayCommand(_ => CopyFailedPaths(), _ => FailedResults.Count > 0);
        exportFailedPathsCommand = new RelayCommand(_ => _ = ExportFailedPathsAsync(), _ => FailedResults.Count > 0);
        chooseImportedImageCommand = new RelayCommand(_ => _ = ChooseImportedImageAsync(), _ => !IsBusy);
        clearImportedImageCommand = new RelayCommand(_ => ClearImportedImage(), _ => !IsBusy && importedImageAssetPath is not null);
        toggleMonitoringCommand = new RelayCommand(_ => _ = ToggleMonitoringAsync(), _ => !IsBusy);
        removeMonitoringRuleCommand = new RelayCommand(parameter => _ = RemoveMonitoringRuleAsync(parameter as string), parameter => !IsBusy && parameter is string);

        RefreshValidation();
        QueuePreviewRefresh();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<PreviewImageViewModel> Previews { get; }
    public ObservableCollection<FolderTheme> SavedThemes { get; }
    public ObservableCollection<string> SelectedRoots { get; } = [];
    public ObservableCollection<OperationResultItem> Results { get; } = [];
    public PaletteEditorViewModel PaletteEditor { get; }

    public IconEditorMode SelectedIconEditorMode
    {
        get => selectedIconEditorMode;
        set
        {
            if (!SetField(ref selectedIconEditorMode, value)) return;
            OnPropertyChanged(nameof(IsBuiltInEditor));
            OnPropertyChanged(nameof(IsImportedImageEditor));
            IconSourceChanged();
        }
    }

    public bool IsBuiltInEditor
    {
        get => SelectedIconEditorMode == IconEditorMode.BuiltIn;
        set { if (value) SelectedIconEditorMode = IconEditorMode.BuiltIn; }
    }

    public bool IsImportedImageEditor
    {
        get => SelectedIconEditorMode == IconEditorMode.ImportedImage;
        set { if (value) SelectedIconEditorMode = IconEditorMode.ImportedImage; }
    }

    public string? ImportedImageAssetPath
    {
        get => importedImageAssetPath;
        private set => SetField(ref importedImageAssetPath, value);
    }

    public BitmapSource? ImportedImagePreview
    {
        get => importedImagePreview;
        private set => SetField(ref importedImagePreview, value);
    }

    public IReadOnlyList<OperationResultItem> FailedResults =>
        Results.Where(result => result.Kind is ResultKind.Failed or ResultKind.StateUncertain).ToArray();

    public IReadOnlyList<ApplicationMode> AvailableModes { get; } =
        [ApplicationMode.Global, ApplicationMode.Compatible];

    public bool IsCompatibleMode => SelectedMode == ApplicationMode.Compatible;
    public bool ContinueApplyingToNewFolders
    {
        get => continueApplyingToNewFolders;
        set => SetField(ref continueApplyingToNewFolders, value);
    }
    public string MonitoringStatusText => folderMonitoring.Status.Paused
        ? $"监控已暂停（{folderMonitoring.Status.TotalRules} 条规则）"
        : $"正在监控 {folderMonitoring.Status.ActiveRules} 个目录；{folderMonitoring.Status.InactiveRules} 个目录暂不可用";
    public int ActiveMonitoringRuleCount => folderMonitoring.Status.ActiveRules;

    public void ApplyPalette(FolderPalette palette)
    {
        ArgumentNullException.ThrowIfNull(palette);
        syncingPalette = true;
        try
        {
            PaletteEditor.Apply(palette);
            GradientStart = palette.GradientStart;
            GradientEnd = palette.GradientEnd;
            StrokeColor = palette.Stroke;
            GlowColor = palette.Glow;
        }
        finally { syncingPalette = false; }
    }

    public FolderTheme CurrentTheme => new(
        FolderTheme.CurrentSchemaVersion,
        themeId,
        ThemeName,
        GradientStart,
        GradientEnd,
        GradientAngle,
        Opacity,
        CornerRadius,
        StrokeColor,
        StrokeOpacity,
        StrokeWidth,
        HighlightStrength,
        GlowColor,
        GlowStrength,
        GlowRadius,
        ShadowStrength,
        ShadowOffset,
        Material,
        Sticker,
        StickerColor,
        StickerX,
        StickerY,
        StickerScale,
        StickerRotation);

    public FolderTheme? SelectedSavedTheme
    {
        get => selectedSavedTheme;
        set
        {
            if (!SetField(ref selectedSavedTheme, value) || value is null)
            {
                return;
            }

            LoadTheme(value);
        }
    }

    public string ThemeName
    {
        get => themeName;
        set => SetThemeField(ref themeName, value ?? string.Empty);
    }

    public string GradientStart
    {
        get => gradientStart;
        set => SetThemeField(ref gradientStart, value ?? string.Empty);
    }

    public string GradientEnd
    {
        get => gradientEnd;
        set => SetThemeField(ref gradientEnd, value ?? string.Empty);
    }

    public double GradientAngle
    {
        get => gradientAngle;
        set => SetThemeField(ref gradientAngle, value);
    }

    public double Opacity
    {
        get => opacity;
        set => SetThemeField(ref opacity, value);
    }

    public double CornerRadius
    {
        get => cornerRadius;
        set => SetThemeField(ref cornerRadius, value);
    }

    public string StrokeColor
    {
        get => strokeColor;
        set => SetThemeField(ref strokeColor, value ?? string.Empty);
    }

    public double StrokeOpacity
    {
        get => strokeOpacity;
        set => SetThemeField(ref strokeOpacity, value);
    }

    public double StrokeWidth
    {
        get => strokeWidth;
        set => SetThemeField(ref strokeWidth, value);
    }

    public double HighlightStrength
    {
        get => highlightStrength;
        set => SetThemeField(ref highlightStrength, value);
    }

    public string GlowColor
    {
        get => glowColor;
        set => SetThemeField(ref glowColor, value ?? string.Empty);
    }

    public double GlowStrength
    {
        get => glowStrength;
        set => SetThemeField(ref glowStrength, value);
    }

    public double GlowRadius
    {
        get => glowRadius;
        set => SetThemeField(ref glowRadius, value);
    }

    public double ShadowStrength
    {
        get => shadowStrength;
        set => SetThemeField(ref shadowStrength, value);
    }

    public double ShadowOffset
    {
        get => shadowOffset;
        set => SetThemeField(ref shadowOffset, value);
    }

    public MaterialStyle Material
    {
        get => material;
        set => SetThemeField(ref material, value);
    }

    public StickerStyle Sticker
    {
        get => sticker;
        set => SetThemeField(ref sticker, value);
    }

    public string StickerColor
    {
        get => stickerColor;
        set => SetThemeField(ref stickerColor, value ?? string.Empty);
    }

    public double StickerX
    {
        get => stickerX;
        set => SetThemeField(ref stickerX, value);
    }

    public double StickerY
    {
        get => stickerY;
        set => SetThemeField(ref stickerY, value);
    }

    public double StickerScale
    {
        get => stickerScale;
        set => SetThemeField(ref stickerScale, value);
    }

    public double StickerRotation
    {
        get => stickerRotation;
        set => SetThemeField(ref stickerRotation, value);
    }

    public string DesktopBackground
    {
        get => desktopBackground;
        set
        {
            if (value is not ("DefaultWallpaper" or "#1B2A4A" or "#4A2D51" or "#204A46"))
                throw new ArgumentOutOfRangeException(nameof(value));
            SetField(ref desktopBackground, value);
        }
    }

    public double DesktopIconSize
    {
        get => desktopIconSize;
        set => SetField(ref desktopIconSize, Math.Clamp(value, 64, 128));
    }

    public ApplicationMode SelectedMode
    {
        get => selectedMode;
        set
        {
            if (!SetField(ref selectedMode, value))
            {
                return;
            }

            inputRevision++;
            OnPropertyChanged(nameof(IsCompatibleMode));
            InvalidatePlan();
            RefreshValidation();
        }
    }

    public bool IsThemeValid
    {
        get => isThemeValid;
        private set => SetField(ref isThemeValid, value);
    }

    public string ValidationMessage
    {
        get => validationMessage;
        private set => SetField(ref validationMessage, value);
    }

    public bool IsPreviewRendering
    {
        get => isPreviewRendering;
        private set
        {
            if (SetField(ref isPreviewRendering, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public bool IsPlanning
    {
        get => isPlanning;
        private set
        {
            if (SetField(ref isPlanning, value))
            {
                RaiseBusyState();
            }
        }
    }

    public bool IsApplying
    {
        get => isApplying;
        private set
        {
            if (SetField(ref isApplying, value))
            {
                RaiseBusyState();
            }
        }
    }

    public bool IsRestoring
    {
        get => isRestoring;
        private set
        {
            if (SetField(ref isRestoring, value))
            {
                RaiseBusyState();
            }
        }
    }

    public bool IsManagingThemes
    {
        get => isManagingThemes;
        private set
        {
            if (SetField(ref isManagingThemes, value))
            {
                RaiseBusyState();
            }
        }
    }

    public bool IsBusy => IsPlanning || IsApplying || IsRestoring || IsManagingThemes;
    public bool AreInputsEnabled => !IsBusy;
    public bool CanApply =>
        HasValidIconSource &&
        HasRequiredRoots &&
        !IsBusy &&
        !IsPreviewRendering &&
        reviewedPlan is { CanApply: true } &&
        reviewedRequest is not null &&
        reviewedPlanRevision == inputRevision;
    public bool CanPlan => HasValidIconSource && HasRequiredRoots && !IsBusy && !IsPreviewRendering;
    public bool HasProgressRange => ProgressTotal > 0;

    public int PlanAllowedCount
    {
        get => planAllowedCount;
        private set => SetField(ref planAllowedCount, value);
    }

    public int PlanSkippedCount
    {
        get => planSkippedCount;
        private set => SetField(ref planSkippedCount, value);
    }

    public string PlanDetails
    {
        get => planDetails;
        private set => SetField(ref planDetails, value);
    }

    public string ProgressMessage
    {
        get => progressMessage;
        private set => SetField(ref progressMessage, value);
    }

    public int ProgressCompleted
    {
        get => progressCompleted;
        private set => SetField(ref progressCompleted, value);
    }

    public int ProgressTotal
    {
        get => progressTotal;
        private set
        {
            if (SetField(ref progressTotal, value))
            {
                OnPropertyChanged(nameof(HasProgressRange));
            }
        }
    }

    public string OperationStatus
    {
        get => operationStatus;
        private set => SetField(ref operationStatus, value);
    }

    public string ResultSummary
    {
        get => resultSummary;
        private set => SetField(ref resultSummary, value);
    }

    public ICommand ApplyCommand => applyCommand;
    public ICommand RestoreCommand => restoreCommand;
    public ICommand PlanCommand => planCommand;
    public ICommand AddRootCommand => addRootCommand;
    public ICommand RemoveRootCommand => removeRootCommand;
    public ICommand SaveThemeCommand => saveThemeCommand;
    public ICommand ImportThemeCommand => importThemeCommand;
    public ICommand ExportThemeCommand => exportThemeCommand;
    public ICommand DeleteThemeCommand => deleteThemeCommand;
    public ICommand CopyFailedPathsCommand => copyFailedPathsCommand;
    public ICommand ExportFailedPathsCommand => exportFailedPathsCommand;
    public ICommand ChooseImportedImageCommand => chooseImportedImageCommand;
    public ICommand ClearImportedImageCommand => clearImportedImageCommand;
    public ICommand ToggleMonitoringCommand => toggleMonitoringCommand;
    public ICommand RemoveMonitoringRuleCommand => removeMonitoringRuleCommand;

    public async Task ChooseImportedImageAsync()
    {
        if (IsBusy) return;
        var sourcePath = await dialogs.PickImageImportPathAsync();
        if (string.IsNullOrWhiteSpace(sourcePath)) return;
        var result = await importedImageService.ImportAsync(sourcePath, CancellationToken.None);
        if (!result.Success || result.AssetPath is null || result.Preview is null)
        {
            dialogs.ShowError("无法导入图片", result.Error ?? "请选择有效的 PNG、JPG、BMP 或 ICO 图片。");
            return;
        }

        ImportedImageAssetPath = result.AssetPath;
        ImportedImagePreview = result.Preview;
        selectedIconEditorMode = IconEditorMode.ImportedImage;
        OnPropertyChanged(nameof(SelectedIconEditorMode));
        OnPropertyChanged(nameof(IsBuiltInEditor));
        OnPropertyChanged(nameof(IsImportedImageEditor));
        IconSourceChanged();
    }

    private void ClearImportedImage()
    {
        ImportedImageAssetPath = null;
        ImportedImagePreview = null;
        IconSourceChanged();
    }

    public async Task InitializeAsync()
    {
        await folderMonitoring.StartAsync();
        RaiseMonitoringState();
        try
        {
            var loaded = await RefreshThemeCatalogAsync();
            var selected = loaded.Themes.FirstOrDefault(theme => theme.Id == themeId);
            if (selected is not null)
            {
                selectedSavedTheme = selected;
                OnPropertyChanged(nameof(SelectedSavedTheme));
                RaiseCommandStates();
            }

            if (loaded.Diagnostics.Count > 0)
            {
                OperationStatus = $"Loaded themes with {loaded.Diagnostics.Count} diagnostic message(s).";
            }
        }
        catch (Exception exception)
        {
            OperationStatus = "Saved themes could not be loaded; the built-in theme remains available.";
            dialogs.ShowError("Could not load themes", exception.Message);
        }

        await WaitForPreviewAsync();
    }

    public Task WaitForPreviewAsync() => previewCompletion;

    public async Task PlanAsync()
    {
        if (!CanPlan)
        {
            return;
        }

        var plannedRevision = inputRevision;
        var request = CreateApplyRequest();
        ClearReviewedPlan();
        IsPlanning = true;
        OperationStatus = "Calculating a write-free application plan…";
        try
        {
            var plan = await coordinator.PlanAsync(request, CancellationToken.None);
            if (plannedRevision != inputRevision)
            {
                OperationStatus = "Inputs changed while planning. Review the current settings and calculate the plan again.";
                return;
            }
            reviewedPlan = plan;
            reviewedRequest = request;
            reviewedPlanRevision = plannedRevision;
            UpdatePlan(plan);
            RaiseCommandStates();
            OperationStatus = plan.CanApply
                ? "Plan ready. No system changes were made."
                : "The plan found issues that must be fixed before applying.";
        }
        catch (Exception exception)
        {
            OperationStatus = "Planning failed. No system changes were made.";
            dialogs.ShowError("Planning failed", exception.Message);
        }
        finally
        {
            IsPlanning = false;
        }
    }

    public async Task ApplyAsync()
    {
        if (!CanApply)
        {
            return;
        }

        var plannedRevision = reviewedPlanRevision;
        var request = reviewedRequest!;
        var plan = reviewedPlan!;
        operationCancellation?.Dispose();
        operationCancellation = new CancellationTokenSource();
        var token = operationCancellation.Token;
        IsApplying = true;
        Results.Clear();
        RefreshResultCommands();
        ResultSummary = "No results yet.";

        try
        {
            if (plannedRevision != inputRevision)
            {
                ClearReviewedPlan();
                OperationStatus = "Inputs changed after planning. Review a fresh plan before applying.";
                return;
            }

            var confirmed = await dialogs.ConfirmAsync(
                plan.Mode == ApplicationMode.Global ? "Apply for current user?" : "Apply to selected folders?",
                BuildApplyConfirmation(plan, request.ExplicitRoots ?? []));
            if (!confirmed)
            {
                OperationStatus = "Apply cancelled before any system changes.";
                return;
            }

            if (plannedRevision != inputRevision)
            {
                ClearReviewedPlan();
                OperationStatus = "Inputs changed during confirmation. Review a fresh plan before applying.";
                return;
            }

            OperationStatus = "Applying the folder theme…";
            ClearReviewedPlan();
            var progress = new DispatcherProgress<ApplyProgress>(dispatcher, ReportProgress);
            var result = await coordinator.ApplyAsync(plan, progress, token);
            if (result.Success && plan.Mode == ApplicationMode.Compatible && ContinueApplyingToNewFolders &&
                !string.IsNullOrWhiteSpace(result.IconArtifactPath))
            {
                var durableIco = await monitoringAssets.PersistAsync(result.IconArtifactPath, token);
                foreach (var root in request.ExplicitRoots ?? [])
                    await folderMonitoring.ReplaceFallbackIconAsync(root, durableIco, token);
                RaiseMonitoringState();
            }
            ShowApplyResult(result);
        }
        catch (OperationCanceledException)
        {
            OperationStatus = "Apply cancelled. Review the result list for any completed work.";
        }
        catch (Exception exception)
        {
            OperationStatus = "Apply failed before a complete result was available.";
            dialogs.ShowError("Apply failed", exception.Message);
        }
        finally
        {
            IsApplying = false;
        }
    }

    public async Task RestoreAsync()
    {
        if (IsBusy)
        {
            return;
        }

        var confirmed = await dialogs.ConfirmAsync(
            "Restore latest backup?",
            "Restore latest will use the most recent recovery snapshot. Successful reversions remain reported even if another path cannot be restored. Continue?");
        if (!confirmed)
        {
            OperationStatus = "Restore cancelled before any system changes.";
            return;
        }

        operationCancellation?.Dispose();
        operationCancellation = new CancellationTokenSource();
        IsRestoring = true;
        Results.Clear();
        RefreshResultCommands();
        try
        {
            var progress = new DispatcherProgress<ApplyProgress>(dispatcher, ReportProgress);
            var result = await coordinator.RestoreLatestAsync(progress, operationCancellation.Token);
            ShowRestoreResult(result);
        }
        catch (OperationCanceledException)
        {
            OperationStatus = "Restore cancelled. Successfully reverted items remain reverted.";
        }
        catch (Exception exception)
        {
            OperationStatus = "Restore partially completed or failed; review the error details.";
            dialogs.ShowError("Restore failed", exception.Message);
        }
        finally
        {
            IsRestoring = false;
        }
    }

    public async Task ExportFailedPathsAsync()
    {
        var text = BuildFailureText();
        if (text.Length > 0)
        {
            try
            {
                await dialogs.ExportTextAsync("folder-theme-failures.txt", text);
            }
            catch (Exception exception)
            {
                dialogs.ShowError("Could not export failure report", exception.Message);
            }
        }
    }

    public void Dispose()
    {
        folderMonitoring.OutcomeProduced -= MonitoringOutcomeProduced;
        folderMonitoring.Dispose();
        PaletteEditor.PaletteChanged -= PaletteEditorChanged;
        SelectedRoots.CollectionChanged -= SelectedRootsChanged;
        previewCancellation?.Cancel();
        previewCancellation?.Dispose();
        operationCancellation?.Cancel();
        operationCancellation?.Dispose();
        previewWorkScheduler.Dispose();
    }

    private bool HasRequiredRoots =>
        SelectedMode != ApplicationMode.Compatible || SelectedRoots.Count > 0;

    private async Task AddRootAsync()
    {
        var path = await dialogs.PickFolderAsync();
        if (!string.IsNullOrWhiteSpace(path) &&
            !SelectedRoots.Contains(path, StringComparer.OrdinalIgnoreCase))
        {
            SelectedRoots.Add(path);
        }
    }

    private void RemoveRoot(object? parameter)
    {
        if (parameter is string path)
        {
            SelectedRoots.Remove(path);
        }
    }

    public async Task SaveThemeAsync()
    {
        if (!IsThemeValid || IsBusy)
        {
            return;
        }

        var savedRevision = inputRevision;
        var theme = CurrentTheme;
        if (theme.Id == FolderTheme.IceBlue.Id)
        {
            theme = theme with { Id = Guid.NewGuid() };
        }

        IsManagingThemes = true;
        try
        {
            await themes.SaveAsync(theme);
            var loaded = await RefreshThemeCatalogAsync();
            if (savedRevision != inputRevision)
            {
                OperationStatus = $"Saved theme ‘{theme.Name}’; current edits remain unsaved.";
                return;
            }

            var saved = loaded.Themes.First(item => item.Id == theme.Id);
            if (themeId != saved.Id)
            {
                themeId = saved.Id;
                inputRevision++;
                InvalidatePlan();
                OnPropertyChanged(nameof(CurrentTheme));
            }
            selectedSavedTheme = saved;
            OnPropertyChanged(nameof(SelectedSavedTheme));
            RaiseCommandStates();
            OperationStatus = $"Saved theme ‘{saved.Name}’.";
        }
        catch (Exception exception)
        {
            dialogs.ShowError("Could not save theme", exception.Message);
        }
        finally
        {
            IsManagingThemes = false;
        }
    }

    public async Task ImportThemeAsync()
    {
        if (IsBusy)
        {
            return;
        }

        var sourcePath = await dialogs.PickThemeImportPathAsync();
        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            return;
        }

        IsManagingThemes = true;
        try
        {
            var result = await themes.ImportAsync(sourcePath);
            if (!result.Success || result.Theme is null)
            {
                dialogs.ShowError(
                    "Could not import theme",
                    result.Errors.Count == 0 ? "The selected file did not contain a valid theme." : string.Join(Environment.NewLine, result.Errors));
                return;
            }

            var loaded = await RefreshThemeCatalogAsync();
            var imported = loaded.Themes.First(theme => theme.Id == result.Theme.Id);
            SelectedSavedTheme = imported;
            OperationStatus = $"Imported theme ‘{imported.Name}’.";
        }
        catch (Exception exception)
        {
            dialogs.ShowError("Could not import theme", exception.Message);
        }
        finally
        {
            IsManagingThemes = false;
        }
    }

    public async Task ExportThemeAsync()
    {
        if (IsBusy || SelectedSavedTheme is not { } selected)
        {
            return;
        }

        var destinationPath = await dialogs.PickThemeExportPathAsync($"{MakeSafeFileName(selected.Name)}.json");
        if (string.IsNullOrWhiteSpace(destinationPath))
        {
            return;
        }

        IsManagingThemes = true;
        try
        {
            await themes.ExportAsync(selected.Id, destinationPath);
            OperationStatus = $"Exported theme ‘{selected.Name}’.";
        }
        catch (Exception exception)
        {
            dialogs.ShowError("Could not export theme", exception.Message);
        }
        finally
        {
            IsManagingThemes = false;
        }
    }

    public async Task DeleteThemeAsync()
    {
        if (IsBusy || SelectedSavedTheme is not { } selected)
        {
            return;
        }

        if (selected.Id == FolderTheme.IceBlue.Id)
        {
            dialogs.ShowError("Cannot delete built-in theme", "The built-in IceBlue theme cannot be deleted.");
            return;
        }

        var confirmed = await dialogs.ConfirmAsync(
            "Delete personal theme?",
            $"Delete the saved theme ‘{selected.Name}’? This removes its theme file and cannot be undone.");
        if (!confirmed)
        {
            return;
        }

        IsManagingThemes = true;
        try
        {
            await themes.DeleteAsync(selected.Id);
            var loaded = await RefreshThemeCatalogAsync();
            var fallback = loaded.Themes.First(theme => theme.Id == FolderTheme.IceBlue.Id);
            SelectedSavedTheme = fallback;
            OperationStatus = $"Deleted theme ‘{selected.Name}’.";
        }
        catch (Exception exception)
        {
            dialogs.ShowError("Could not delete theme", exception.Message);
        }
        finally
        {
            IsManagingThemes = false;
        }
    }

    private async Task<ThemeLoadResult> RefreshThemeCatalogAsync()
    {
        var loaded = await themes.LoadAsync();
        SavedThemes.Clear();
        foreach (var item in loaded.Themes)
        {
            SavedThemes.Add(item);
        }
        return loaded;
    }

    private static string MakeSafeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var safe = new string(name.Select(character => invalid.Contains(character) ? '-' : character).ToArray()).Trim();
        return safe.Length == 0 ? "folder-theme" : safe;
    }

    private void LoadTheme(FolderTheme theme)
    {
        themeId = theme.Id;
        themeName = theme.Name;
        gradientStart = theme.GradientStart;
        gradientEnd = theme.GradientEnd;
        gradientAngle = theme.GradientAngle;
        opacity = theme.Opacity;
        cornerRadius = theme.CornerRadius;
        strokeColor = theme.StrokeColor;
        strokeOpacity = theme.StrokeOpacity;
        strokeWidth = theme.StrokeWidth;
        highlightStrength = theme.HighlightStrength;
        glowColor = theme.GlowColor;
        glowStrength = theme.GlowStrength;
        glowRadius = theme.GlowRadius;
        shadowStrength = theme.ShadowStrength;
        shadowOffset = theme.ShadowOffset;
        material = theme.Material;
        sticker = theme.Sticker;
        stickerColor = theme.StickerColor;
        stickerX = theme.StickerX;
        stickerY = theme.StickerY;
        stickerScale = theme.StickerScale;
        stickerRotation = theme.StickerRotation;
        inputRevision++;

        foreach (var property in ThemePropertyNames)
        {
            OnPropertyChanged(property);
        }
        OnPropertyChanged(nameof(CurrentTheme));
        serviceValidationErrors = [];
        InvalidatePlan();
        RefreshValidation();
        QueuePreviewRefresh();
    }

    private static readonly string[] ThemePropertyNames =
    [
        nameof(ThemeName), nameof(GradientStart), nameof(GradientEnd), nameof(GradientAngle),
        nameof(Opacity), nameof(CornerRadius), nameof(StrokeColor), nameof(StrokeOpacity),
        nameof(StrokeWidth), nameof(HighlightStrength), nameof(GlowColor), nameof(GlowStrength),
        nameof(GlowRadius), nameof(ShadowStrength), nameof(ShadowOffset),
        nameof(Material), nameof(Sticker), nameof(StickerColor), nameof(StickerX),
        nameof(StickerY), nameof(StickerScale), nameof(StickerRotation)
    ];

    private void SelectedRootsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        inputRevision++;
        InvalidatePlan();
        RefreshValidation();
    }

    private ApplyRequest CreateApplyRequest() => new(
        CurrentIconSource,
        SelectedMode,
        SelectedMode == ApplicationMode.Compatible ? SelectedRoots.ToArray() : null);

    private void UpdatePlan(ApplyPlan plan)
    {
        PlanAllowedCount = plan.AllowedCount;
        PlanSkippedCount = plan.SkippedCount;
        serviceValidationErrors = plan.ValidationErrors;
        RefreshValidation();

        if (plan.Mode == ApplicationMode.Global)
        {
            PlanDetails = "Current-user Shell Icons values 3 and 4 will be updated; no folder tree is scanned.";
            return;
        }

        var reasons = plan.SkippedReasons.Count == 0
            ? "No skipped folders."
            : string.Join(", ", plan.SkippedReasons.Select(pair => $"{pair.Key}: {pair.Value}"));
        PlanDetails = $"{plan.AllowedCount} allowed, {plan.SkippedCount} skipped. {reasons}";
    }

    private static string BuildApplyConfirmation(ApplyPlan plan, IReadOnlyList<string> explicitRoots)
    {
        if (plan.Mode == ApplicationMode.Global)
        {
            return $"For the current Windows user, HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Icons values 3 and 4 will be changed to the rendered icon for ‘{plan.Theme.Name}’. The original mappings are backed up first and remain available through Restore latest. Continue?";
        }

        var roots = string.Join(Environment.NewLine, explicitRoots.Select(root => $"• {root}"));
        return $"Only these explicit folder roots will be considered; compatible mode never scans a whole drive:{Environment.NewLine}{roots}{Environment.NewLine}{Environment.NewLine}{plan.AllowedCount} allowed; {plan.SkippedCount} skipped. Existing Desktop.ini files keep unrelated settings while icon fields are updated. Known folders, network paths, reparse points, and unwritable folders are skipped. Continue?";
    }

    private void ShowApplyResult(ApplyResult result)
    {
        Results.Clear();
        var rolledBack = !result.Success && result.RollbackAttempted && result.RollbackSucceeded;
        var rollbackIncomplete = !result.Success && result.RollbackAttempted && !result.RollbackSucceeded;
        foreach (var item in result.Successful)
        {
            Results.Add(new OperationResultItem(
                rollbackIncomplete
                    ? ResultKind.StateUncertain
                    : rolledBack ? ResultKind.RolledBack : ResultKind.Success,
                item.Path,
                rollbackIncomplete
                    ? $"{item.Detail ?? "Changed"}; rollback incomplete, final state uncertain"
                    : rolledBack ? $"{item.Detail ?? "Changed"}; then rolled back" : item.Detail ?? "Applied"));
        }
        foreach (var item in result.Skipped)
        {
            Results.Add(new OperationResultItem(ResultKind.Skipped, item.Path, item.Detail ?? "Skipped"));
        }
        foreach (var failure in result.Failures)
        {
            Results.Add(new OperationResultItem(ResultKind.Failed, failure.Path, failure.Error));
        }

        ResultSummary = rollbackIncomplete
            ? $"State uncertain after incomplete rollback: {result.Successful.Count} changed  Skipped: {result.Skipped.Count}  Failed: {result.Failures.Count}"
            : rolledBack
                ? $"Changed then rolled back: {result.Successful.Count}  Skipped: {result.Skipped.Count}  Failed: {result.Failures.Count}"
                : $"Applied: {result.Successful.Count}  Skipped: {result.Skipped.Count}  Failed: {result.Failures.Count}";
        OperationStatus = result.Success
            ? "Apply completed successfully. A recovery snapshot is available."
            : rollbackIncomplete
                ? "Apply failed and rollback incomplete; system state is uncertain. Review every affected path and failure."
                : rolledBack
                ? "Apply did not complete; the recorded changes were rolled back."
                : "Apply partially completed or failed. Review each result below.";
        RefreshResultCommands();
    }

    private void ShowRestoreResult(RestoreResult result)
    {
        Results.Clear();
        foreach (var item in result.Successful)
        {
            Results.Add(new OperationResultItem(ResultKind.Success, item.Path, item.Detail ?? "Restored"));
        }
        foreach (var item in result.Skipped)
        {
            Results.Add(new OperationResultItem(ResultKind.Skipped, item.Path, item.Detail ?? "Already absent"));
        }
        foreach (var item in result.Changed)
        {
            Results.Add(new OperationResultItem(ResultKind.StateUncertain, item.Path, item.Detail ?? "Changed since apply; ownership remains unresolved"));
        }

        var changedPaths = result.Changed
            .Select(item => item.Path)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var failure in result.Failures)
        {
            if (failure.Path is not null && changedPaths.Contains(failure.Path)) continue;
            Results.Add(new OperationResultItem(ResultKind.Failed, failure.Path, failure.Error));
        }

        ResultSummary = $"Restored: {result.Successful.Count}  Already absent: {result.Skipped.Count}  Changed: {result.Changed.Count}  Failed: {result.Failures.Count - result.Changed.Count}";
        OperationStatus = result.Success
            ? result.AlreadyRestored
                ? "The latest backup had already been restored; no additional changes were made."
                : "Restore completed successfully."
            : "Restore partially completed or failed. Successful reversions remain in place; review the failures below.";
        RefreshResultCommands();
    }

    private void CopyFailedPaths()
    {
        var paths = FailedResults
            .Select(result => result.Path)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Distinct(StringComparer.OrdinalIgnoreCase);
        var text = string.Join(Environment.NewLine, paths);
        if (text.Length > 0)
        {
            try
            {
                dialogs.CopyText(text);
            }
            catch (Exception exception)
            {
                dialogs.ShowError("Could not copy failed paths", exception.Message);
            }
        }
    }

    private string BuildFailureText()
    {
        var builder = new StringBuilder();
        foreach (var result in FailedResults)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            builder.Append(result.Path ?? "(no path)");
            builder.Append('\t');
            builder.Append(result.Detail);
        }
        return builder.ToString();
    }

    private void ReportProgress(ApplyProgress progress)
    {
        ProgressMessage = $"{progress.Phase}: {progress.Message}";
        ProgressCompleted = progress.Completed;
        ProgressTotal = progress.Total;
    }

    private void RefreshValidation()
    {
        var validation = ThemeValidator.Validate(CurrentTheme);
        IsThemeValid = validation.IsValid;
        if (SelectedIconEditorMode == IconEditorMode.ImportedImage && importedImageAssetPath is null)
        {
            ValidationMessage = "请先选择一张 PNG、JPG、BMP 或 ICO 图片。";
        }
        else if ((SelectedIconEditorMode == IconEditorMode.BuiltIn && !validation.IsValid) || serviceValidationErrors.Count > 0)
        {
            var errors = validation.Errors.Concat(serviceValidationErrors).Distinct(StringComparer.Ordinal);
            ValidationMessage = $"Fix these fields before applying: {string.Join(", ", errors)}.";
        }
        else if (!HasRequiredRoots)
        {
            ValidationMessage = "Compatibility mode requires at least one explicit folder root. No whole-drive root is selected automatically.";
        }
        else
        {
            ValidationMessage = "图标已准备好，可以预览或计算方案。";
        }

        RaiseCommandStates();
    }

    private void InvalidatePlan()
    {
        ClearReviewedPlan();
        PlanAllowedCount = 0;
        PlanSkippedCount = 0;
        PlanDetails = "Plan not calculated yet.";
        serviceValidationErrors = [];
    }

    private void ClearReviewedPlan()
    {
        reviewedPlan = null;
        reviewedRequest = null;
        reviewedPlanRevision = -1;
        RaiseCommandStates();
    }

    private void QueuePreviewRefresh()
    {
        previewCancellation?.Cancel();
        previewCancellation?.Dispose();
        var cancellation = new CancellationTokenSource();
        previewCancellation = cancellation;
        IsPreviewRendering = true;
        previewCompletion = RefreshPreviewsAsync(cancellation);
    }

    private async Task RefreshPreviewsAsync(CancellationTokenSource cancellation)
    {
        try
        {
            await previewDelay.DelayAsync(PreviewDebounce, cancellation.Token);
            cancellation.Token.ThrowIfCancellationRequested();
            if (!HasValidIconSource)
            {
                await dispatcher.InvokeAsync(() =>
                {
                    foreach (var preview in Previews) preview.Image = null;
                });
                return;
            }
            var source = CurrentIconSource;
            var sizes = Previews.Select(preview => preview.Size).ToArray();
            var rendered = await previewWorkScheduler.RunAsync(() =>
            {
                var images = new List<(int Size, BitmapSource Image)>();
                foreach (var size in sizes)
                {
                    cancellation.Token.ThrowIfCancellationRequested();
                    var image = previewRenderer.Render(source, size);
                    if (image.CanFreeze && !image.IsFrozen) image.Freeze();
                    images.Add((size, image));
                }

                return (IReadOnlyList<(int Size, BitmapSource Image)>)images;
            }, cancellation.Token);
            await dispatcher.InvokeAsync(() =>
            {
                cancellation.Token.ThrowIfCancellationRequested();
                foreach (var item in rendered)
                {
                    Previews.Single(preview => preview.Size == item.Size).Image = item.Image;
                }
            });
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            await dispatcher.InvokeAsync(() =>
            {
                OperationStatus = "Preview rendering failed. Fix the theme values and try again.";
                dialogs.ShowError("Preview failed", exception.Message);
            });
        }
        finally
        {
            if (ReferenceEquals(previewCancellation, cancellation))
            {
                await dispatcher.InvokeAsync(() => IsPreviewRendering = false);
            }
        }
    }

    private void RefreshResultCommands()
    {
        OnPropertyChanged(nameof(FailedResults));
        copyFailedPathsCommand.RaiseCanExecuteChanged();
        exportFailedPathsCommand.RaiseCanExecuteChanged();
    }

    private void RaiseBusyState()
    {
        OnPropertyChanged(nameof(IsBusy));
        OnPropertyChanged(nameof(AreInputsEnabled));
        OnPropertyChanged(nameof(CanApply));
        OnPropertyChanged(nameof(CanPlan));
        RaiseCommandStates();
    }

    private void RaiseCommandStates()
    {
        OnPropertyChanged(nameof(CanApply));
        OnPropertyChanged(nameof(CanPlan));
        applyCommand?.RaiseCanExecuteChanged();
        restoreCommand?.RaiseCanExecuteChanged();
        planCommand?.RaiseCanExecuteChanged();
        addRootCommand?.RaiseCanExecuteChanged();
        removeRootCommand?.RaiseCanExecuteChanged();
        saveThemeCommand?.RaiseCanExecuteChanged();
        importThemeCommand?.RaiseCanExecuteChanged();
        exportThemeCommand?.RaiseCanExecuteChanged();
        deleteThemeCommand?.RaiseCanExecuteChanged();
        chooseImportedImageCommand?.RaiseCanExecuteChanged();
        clearImportedImageCommand?.RaiseCanExecuteChanged();
    }

    private bool SetThemeField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!SetField(ref field, value, propertyName))
        {
            return false;
        }

        inputRevision++;
        if (selectedSavedTheme is not null)
        {
            selectedSavedTheme = null;
            OnPropertyChanged(nameof(SelectedSavedTheme));
        }
        OnPropertyChanged(nameof(CurrentTheme));
        if (!syncingPalette)
        {
            var picker = propertyName switch
            {
                nameof(GradientStart) => PaletteEditor.GradientStart,
                nameof(GradientEnd) => PaletteEditor.GradientEnd,
                nameof(StrokeColor) => PaletteEditor.Stroke,
                nameof(GlowColor) => PaletteEditor.Glow,
                _ => null
            };
            if (value is string color) picker?.TrySetHex(color);
        }
        serviceValidationErrors = [];
        InvalidatePlan();
        RefreshValidation();
        QueuePreviewRefresh();
        return true;
    }

    public async Task ToggleMonitoringAsync()
    {
        if (folderMonitoring.Status.Paused) await folderMonitoring.ResumeAsync();
        else await folderMonitoring.PauseAsync();
        RaiseMonitoringState();
    }

    private async Task RemoveMonitoringRuleAsync(string? root)
    {
        if (string.IsNullOrWhiteSpace(root)) return;
        await folderMonitoring.RemoveRuleAsync(root);
        RaiseMonitoringState();
    }

    private void MonitoringOutcomeProduced(object? sender, MonitoringOutcome outcome) =>
        _ = dispatcher.InvokeAsync(() =>
        {
            Results.Add(new OperationResultItem(outcome.Success ? ResultKind.Success : ResultKind.Failed, outcome.Path,
                outcome.Success ? "后台监控已应用图标" : outcome.Error ?? "后台监控应用失败"));
            RefreshResultCommands();
        });

    private void RaiseMonitoringState()
    {
        OnPropertyChanged(nameof(MonitoringStatusText));
        OnPropertyChanged(nameof(ActiveMonitoringRuleCount));
        toggleMonitoringCommand.RaiseCanExecuteChanged();
        removeMonitoringRuleCommand.RaiseCanExecuteChanged();
    }

    private bool HasValidIconSource => SelectedIconEditorMode == IconEditorMode.ImportedImage
        ? importedImageAssetPath is not null
        : IsThemeValid;

    private IconSource CurrentIconSource => SelectedIconEditorMode == IconEditorMode.ImportedImage
        ? IconSource.ImportedImage(importedImageAssetPath ?? throw new InvalidOperationException("Choose an image first."))
        : IconSource.BuiltIn(CurrentTheme);

    private void IconSourceChanged()
    {
        inputRevision++;
        InvalidatePlan();
        RefreshValidation();
        QueuePreviewRefresh();
        clearImportedImageCommand.RaiseCanExecuteChanged();
    }

    private void PaletteEditorChanged(object? sender, EventArgs e)
    {
        if (!syncingPalette) ApplyPalette(PaletteEditor.Palette);
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private sealed class DispatcherProgress<T>(
        IViewModelDispatcher dispatcher,
        Action<T> report) : IProgress<T>
    {
        public void Report(T value) => _ = dispatcher.InvokeAsync(() => report(value));
    }
}

public sealed class ThemeApplicationCoordinator(ThemeApplicationService service) : IThemeApplicationCoordinator
{
    public Task<ApplyPlan> PlanAsync(ApplyRequest request, CancellationToken token) =>
        service.PlanAsync(request, token);

    public Task<ApplyResult> ApplyAsync(ApplyPlan plan, IProgress<ApplyProgress> progress, CancellationToken token) =>
        service.ApplyAsync(plan, progress, token);

    public Task<RestoreResult> RestoreLatestAsync(IProgress<ApplyProgress> progress, CancellationToken token) =>
        service.RestoreLatestAsync(progress, token);
}

public sealed class ThemeCatalog(ThemeStore store) : IThemeCatalog
{
    public Task<ThemeLoadResult> LoadAsync() => store.LoadAsync();
    public Task SaveAsync(FolderTheme theme) => store.SaveAsync(theme);
    public Task<ThemeImportResult> ImportAsync(string sourcePath) => store.ImportAsync(sourcePath);
    public Task ExportAsync(Guid themeId, string destinationPath) => store.ExportAsync(themeId, destinationPath);
    public Task DeleteAsync(Guid themeId) => store.DeleteAsync(themeId);
}

public sealed class FolderPreviewRenderer : IPreviewRenderer
{
    public BitmapSource Render(IconSource source, int size) => source.Kind switch
    {
        IconSourceKind.BuiltIn => FolderIconRenderer.Render(source.Theme!, size),
        IconSourceKind.ImportedImage => ImportedIconRenderer.Render(source.ImportedImagePath!, size),
        _ => throw new ArgumentOutOfRangeException(nameof(source))
    };
}

public sealed class TaskPreviewDelay : IPreviewDelay
{
    public Task DelayAsync(TimeSpan delay, CancellationToken token) => Task.Delay(delay, token);
}

public sealed class StaPreviewWorkScheduler : IPreviewWorkScheduler
{
    private readonly BlockingCollection<IWorkItem> queue = [];
    private readonly Thread thread;
    private int disposed;

    public StaPreviewWorkScheduler()
    {
        thread = new Thread(ProcessQueue)
        {
            IsBackground = true,
            Name = "Folder Theme Studio preview renderer"
        };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
    }

    public Task<T> RunAsync<T>(Func<T> work, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(work);
        ObjectDisposedException.ThrowIf(disposed != 0, this);
        token.ThrowIfCancellationRequested();
        var item = new WorkItem<T>(work, token);
        queue.Add(item, token);
        return item.Task;
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref disposed, 1) == 0) queue.CompleteAdding();
    }

    private void ProcessQueue()
    {
        foreach (var item in queue.GetConsumingEnumerable()) item.Execute();
    }

    private interface IWorkItem { void Execute(); }

    private sealed class WorkItem<T>(Func<T> work, CancellationToken token) : IWorkItem
    {
        private readonly TaskCompletionSource<T> completion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        internal Task<T> Task => completion.Task;

        public void Execute()
        {
            if (token.IsCancellationRequested)
            {
                completion.TrySetCanceled(token);
                return;
            }

            try { completion.TrySetResult(work()); }
            catch (OperationCanceledException exception) when (exception.CancellationToken == token)
            { completion.TrySetCanceled(token); }
            catch (Exception exception) { completion.TrySetException(exception); }
        }
    }
}

public sealed class WpfViewModelDispatcher : IViewModelDispatcher
{
    public Task InvokeAsync(Action action)
    {
        var application = System.Windows.Application.Current;
        if (application is null || application.Dispatcher.CheckAccess())
        {
            action();
            return Task.CompletedTask;
        }

        return application.Dispatcher.InvokeAsync(action).Task;
    }
}
```

## src/FolderThemeStudio.App/ViewModels/PaletteEditorViewModel.cs

```csharp
using System.ComponentModel;
using FolderThemeStudio.App.Settings;

namespace FolderThemeStudio.App.ViewModels;

public sealed record PaletteEditorRole(string DisplayName, ColorPickerViewModel Picker);

public sealed class PaletteEditorViewModel : INotifyPropertyChanged
{
    private PaletteEditorRole selectedRole;
    public PaletteEditorViewModel(FolderPalette initial)
    {
        GradientStart = new(initial.GradientStart);
        GradientEnd = new(initial.GradientEnd);
        Stroke = new(initial.Stroke);
        Glow = new(initial.Glow);
        Roles =
        [
            new("渐变起始色", GradientStart),
            new("渐变结束色", GradientEnd),
            new("边框颜色", Stroke),
            new("发光颜色", Glow)
        ];
        selectedRole = Roles[0];
        foreach (var role in Roles) role.Picker.PropertyChanged += PickerChanged;
    }

    public ColorPickerViewModel GradientStart { get; }
    public ColorPickerViewModel GradientEnd { get; }
    public ColorPickerViewModel Stroke { get; }
    public ColorPickerViewModel Glow { get; }
    public IReadOnlyList<PaletteEditorRole> Roles { get; }
    public PaletteEditorRole SelectedRole
    {
        get => selectedRole;
        set
        {
            if (Equals(selectedRole, value)) return;
            selectedRole = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedRole)));
        }
    }
    public FolderPalette Palette => new(GradientStart.Hex, GradientEnd.Hex, Stroke.Hex, Glow.Hex);
    public event EventHandler? PaletteChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public void Apply(FolderPalette palette)
    {
        GradientStart.TrySetHex(palette.GradientStart);
        GradientEnd.TrySetHex(palette.GradientEnd);
        Stroke.TrySetHex(palette.Stroke);
        Glow.TrySetHex(palette.Glow);
    }

    private void PickerChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ColorPickerViewModel.Hex) &&
            GradientStart.IsValid && GradientEnd.IsValid && Stroke.IsValid && Glow.IsValid)
        {
            PaletteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
```

## src/FolderThemeStudio.App/ViewModels/RelayCommand.cs

```csharp
using System.Windows.Input;

namespace FolderThemeStudio.App.ViewModels;

public sealed class RelayCommand(
    Action<object?> execute,
    Func<object?, bool>? canExecute = null) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => execute(parameter);

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
```

## src/FolderThemeStudio.App/ViewModels/SettingsViewModel.cs

```csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.App.ViewModels;

public sealed record PaletteRole(string DisplayName, ColorPickerViewModel Picker);

public sealed class SettingsViewModel : INotifyPropertyChanged
{
    private readonly AppSettings initial;
    private readonly IAppSettingsStore store;
    private readonly ILocalizationService localization;
    private readonly Action<FolderPalette> applyPalette;
    private readonly IPreviewRenderer previewRenderer;
    private string selectedLanguage;
    private PaletteRole? selectedRole;
    private BitmapSource? previewImage;
    private bool startWithWindows;
    private bool closeToTray;
    private double cardOpacityPercent;
    private FolderEditChord editChord;

    public SettingsViewModel(
        AppSettings initial,
        IAppSettingsStore store,
        ILocalizationService localization,
        Action<FolderPalette> applyPalette,
        IPreviewRenderer previewRenderer)
    {
        this.initial = initial ?? throw new ArgumentNullException(nameof(initial));
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        this.applyPalette = applyPalette ?? throw new ArgumentNullException(nameof(applyPalette));
        this.previewRenderer = previewRenderer ?? throw new ArgumentNullException(nameof(previewRenderer));
        selectedLanguage = initial.Language;
        startWithWindows = initial.StartWithWindows;
        closeToTray = initial.CloseToTray;
        cardOpacityPercent = initial.CardOpacity * 100;
        editChord = initial.EditChord;
        GradientStart = new ColorPickerViewModel(initial.Palette.GradientStart);
        GradientEnd = new ColorPickerViewModel(initial.Palette.GradientEnd);
        Stroke = new ColorPickerViewModel(initial.Palette.Stroke);
        Glow = new ColorPickerViewModel(initial.Palette.Glow);
        Roles =
        [
            new PaletteRole(localization.Get("Settings.GradientStart"), GradientStart),
            new PaletteRole(localization.Get("Settings.GradientEnd"), GradientEnd),
            new PaletteRole(localization.Get("Settings.Stroke"), Stroke),
            new PaletteRole(localization.Get("Settings.Glow"), Glow),
        ];
        selectedRole = Roles[0];
        RecentColors = new ObservableCollection<string>(initial.RecentColors);
        RecommendedPalettes =
        [
            FolderPalette.Default,
            new("#A78BFA", "#6366F1", "#DDD6FE", "#8B5CF6"),
            new("#34D399", "#059669", "#A7F3D0", "#10B981"),
            new("#FBBF24", "#F97316", "#FEF3C7", "#FB923C"),
            new("#FB7185", "#DB2777", "#FECDD3", "#F472B6"),
            new("#94A3B8", "#334155", "#E2E8F0", "#64748B"),
        ];
        foreach (var picker in Roles.Select(role => role.Picker)) picker.PropertyChanged += PickerChanged;
        RefreshPreview();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ColorPickerViewModel GradientStart { get; }
    public ColorPickerViewModel GradientEnd { get; }
    public ColorPickerViewModel Stroke { get; }
    public ColorPickerViewModel Glow { get; }
    public IReadOnlyList<PaletteRole> Roles { get; }
    public IReadOnlyList<FolderPalette> RecommendedPalettes { get; }
    public ObservableCollection<string> RecentColors { get; }

    public string SelectedLanguage
    {
        get => selectedLanguage;
        set => SetField(ref selectedLanguage, value is "en-US" ? "en-US" : "zh-CN");
    }

    public PaletteRole? SelectedRole
    {
        get => selectedRole;
        set => SetField(ref selectedRole, value);
    }

    public BitmapSource? PreviewImage
    {
        get => previewImage;
        private set => SetField(ref previewImage, value);
    }

    public bool CanSave => Roles.All(role => role.Picker.IsValid);

    public bool StartWithWindows
    {
        get => startWithWindows;
        set => SetField(ref startWithWindows, value);
    }

    public bool CloseToTray
    {
        get => closeToTray;
        set => SetField(ref closeToTray, value);
    }

    public double CardOpacityPercent
    {
        get => cardOpacityPercent;
        set => SetField(ref cardOpacityPercent, Math.Clamp(value, 0, 100));
    }

    public FolderEditChord EditChord
    {
        get => editChord;
        set
        {
            if (!FolderEditChord.TryNormalize(value, out var normalized)) return;
            if (SetField(ref editChord, normalized)) OnPropertyChanged(nameof(EditChordDisplay));
        }
    }

    public string EditChordDisplay => EditChord.ToString();

    public async Task<bool> SaveAsync()
    {
        if (!CanSave) return false;
        var palette = CurrentPalette();
        var recent = new[] { palette.GradientStart, palette.GradientEnd, palette.Stroke, palette.Glow }
            .Concat(initial.RecentColors)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(12)
            .ToArray();
        var settings = new AppSettings(
            SelectedLanguage,
            initial.TutorialCompleted,
            palette,
            recent,
            StartWithWindows,
            CloseToTray,
            initial.MonitoringPaused,
            CardOpacityPercent / 100) { EditChord = EditChord };
        await store.SaveAsync(settings);
        localization.ApplyLanguage(SelectedLanguage);
        applyPalette(palette);
        return true;
    }

    public void Cancel() { }

    public void ApplyRecommended(FolderPalette palette)
    {
        GradientStart.TrySetHex(palette.GradientStart);
        GradientEnd.TrySetHex(palette.GradientEnd);
        Stroke.TrySetHex(palette.Stroke);
        Glow.TrySetHex(palette.Glow);
    }

    public void ApplyRecent(string color)
    {
        SelectedRole?.Picker.TrySetHex(color);
    }

    private FolderPalette CurrentPalette() => new(GradientStart.Hex, GradientEnd.Hex, Stroke.Hex, Glow.Hex);

    private void PickerChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CanSave));
        if (e.PropertyName == nameof(ColorPickerViewModel.Hex) && CanSave) RefreshPreview();
    }

    private void RefreshPreview()
    {
        var palette = CurrentPalette();
        var theme = FolderTheme.IceBlue with
        {
            GradientStart = palette.GradientStart,
            GradientEnd = palette.GradientEnd,
            StrokeColor = palette.Stroke,
            GlowColor = palette.Glow,
        };
        PreviewImage = previewRenderer.Render(FolderThemeStudio.Core.Application.IconSource.BuiltIn(theme), 256);
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

## src/FolderThemeStudio.App/ViewModels/TutorialViewModel.cs

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Settings;

namespace FolderThemeStudio.App.ViewModels;

public sealed record TutorialStep(string Title, string Body);

public sealed class TutorialViewModel : INotifyPropertyChanged
{
    private readonly AppSettings settings;
    private readonly IAppSettingsStore store;
    private readonly RelayCommand previousCommand;
    private readonly RelayCommand nextCommand;
    private int stepIndex;

    public TutorialViewModel(AppSettings settings, IAppSettingsStore store, ILocalizationService localization)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        ArgumentNullException.ThrowIfNull(localization);
        Steps = Enumerable.Range(1, 3)
            .Select(index => new TutorialStep(
                localization.Get($"Tutorial.Step{index}.Title"),
                localization.Get($"Tutorial.Step{index}.Body")))
            .ToArray();
        previousCommand = new RelayCommand(_ => StepIndex--, _ => StepIndex > 0);
        nextCommand = new RelayCommand(_ => StepIndex++, _ => StepIndex < Steps.Count - 1);
        SkipCommand = new RelayCommand(_ => _ = CompleteAsync(skipped: true));
        FinishCommand = new RelayCommand(_ => _ = CompleteAsync(skipped: false), _ => StepIndex == Steps.Count - 1);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? RequestClose;
    public IReadOnlyList<TutorialStep> Steps { get; }
    public ICommand PreviousCommand => previousCommand;
    public ICommand NextCommand => nextCommand;
    public ICommand SkipCommand { get; }
    public ICommand FinishCommand { get; }
    public bool CloseRequested { get; private set; }

    public int StepIndex
    {
        get => stepIndex;
        private set
        {
            var bounded = Math.Clamp(value, 0, Steps.Count - 1);
            if (stepIndex == bounded) return;
            stepIndex = bounded;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CurrentStep));
            OnPropertyChanged(nameof(StepNumber));
            previousCommand.RaiseCanExecuteChanged();
            nextCommand.RaiseCanExecuteChanged();
            (FinishCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public TutorialStep CurrentStep => Steps[StepIndex];
    public string StepNumber => $"{StepIndex + 1} / {Steps.Count}";

    public async Task CompleteAsync(bool skipped)
    {
        if (CloseRequested) return;
        await store.SaveAsync(settings with { TutorialCompleted = true });
        CloseRequested = true;
        OnPropertyChanged(nameof(CloseRequested));
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

## src/FolderThemeStudio.App/ViewModels/VisualColorMath.cs

```csharp
namespace FolderThemeStudio.App.ViewModels;

public static class VisualColorMath
{
    public static (double Saturation, double Value) FromPalettePoint(
        double x,
        double y,
        double width,
        double height)
    {
        if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

        return (
            Math.Clamp(x / width, 0, 1),
            1 - Math.Clamp(y / height, 0, 1));
    }

    public static double HueFromPoint(double y, double height)
    {
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
        return Math.Clamp(y / height, 0, 1) * 359;
    }
}
```

## src/FolderThemeStudio.Core/Application/ApplyModels.cs

```csharp
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Application;

public enum ApplicationMode
{
    Global,
    Compatible
}

public sealed record ApplyRequest(
    IconSource IconSource,
    ApplicationMode Mode,
    IReadOnlyList<string>? ExplicitRoots = null)
{
    public ApplyRequest(FolderTheme theme, ApplicationMode mode, IReadOnlyList<string>? explicitRoots = null)
        : this(IconSource.BuiltIn(theme), mode, explicitRoots)
    {
    }

    public FolderTheme Theme => IconSource.Theme ?? FolderTheme.IceBlue;
}

public sealed record ApplyPlan(
    IconSource IconSource,
    ApplicationMode Mode,
    IReadOnlyList<FolderPlanItem> Folders,
    IReadOnlyList<string> ValidationErrors)
{
    public ApplyPlan(
        FolderTheme theme,
        ApplicationMode mode,
        IReadOnlyList<FolderPlanItem> folders,
        IReadOnlyList<string> validationErrors)
        : this(IconSource.BuiltIn(theme), mode, folders, validationErrors)
    {
    }

    public FolderTheme Theme => IconSource.Theme ?? FolderTheme.IceBlue;
    public bool CanApply => ValidationErrors.Count == 0;
    public int AllowedCount => Folders.Count(item => item.Decision == FolderDecision.Allowed);
    public int SkippedCount => Folders.Count - AllowedCount;
    public IReadOnlyDictionary<FolderDecision, int> SkippedReasons => Folders
        .Where(item => item.Decision != FolderDecision.Allowed)
        .GroupBy(item => item.Decision)
        .ToDictionary(group => group.Key, group => group.Count());
}

public enum ApplyPhase
{
    Validation,
    Rendering,
    Snapshot,
    SystemMutation,
    Refresh,
    Rollback,
    Restore
}

public sealed record ApplyProgress(ApplyPhase Phase, string Message, int Completed = 0, int Total = 0);

public sealed record ApplyRecord(string? Path, string? Detail);

public sealed record ApplyFailure(ApplyPhase Phase, string Error, string? Path = null);

public sealed record ApplyResult(
    bool Success,
    Guid? OperationId,
    IReadOnlyList<ApplyRecord> Successful,
    IReadOnlyList<ApplyRecord> Skipped,
    IReadOnlyList<ApplyFailure> Failures,
    bool RollbackAttempted,
    bool RollbackSucceeded)
{
    public string? IconArtifactPath { get; init; }
}

public sealed record RestoreResult(
    bool Success,
    bool AlreadyRestored,
    Guid? OperationId,
    IReadOnlyList<ApplyFailure> Failures)
{
    public IReadOnlyList<ApplyRecord> Successful { get; init; } = [];
    public IReadOnlyList<ApplyRecord> Skipped { get; init; } = [];
    public IReadOnlyList<ApplyRecord> Changed { get; init; } = [];
}
```

## src/FolderThemeStudio.Core/Application/IconSource.cs

```csharp
using System.IO;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Application;

public enum IconSourceKind
{
    BuiltIn,
    ImportedImage
}

public sealed record IconSource
{
    private IconSource(IconSourceKind kind, FolderTheme? theme, string? importedImagePath)
    {
        Kind = kind;
        Theme = theme;
        ImportedImagePath = importedImagePath;
    }

    public IconSourceKind Kind { get; }
    public FolderTheme? Theme { get; }
    public string? ImportedImagePath { get; }

    public static IconSource BuiltIn(FolderTheme theme) =>
        new(IconSourceKind.BuiltIn, theme ?? throw new ArgumentNullException(nameof(theme)), null);

    public static IconSource ImportedImage(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return new IconSource(IconSourceKind.ImportedImage, null, Path.GetFullPath(path));
    }
}
```

## src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs

```csharp
using System.IO;
using System.Windows.Media.Imaging;
using FolderThemeStudio.Core.Recovery;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Application;

internal interface IThemeValidationService
{
    ThemeValidationResult Validate(FolderTheme theme);
}

internal interface IIconArtifactService
{
    Task<IconArtifactResult> CreateAndVerifyAsync(IconSource source, Guid operationId, CancellationToken token);
}

internal interface IKnownFolderPlanningService
{
    IAsyncEnumerable<FolderPlanItem> PlanTreeAsync(string root, CancellationToken token);
}

internal interface IApplicationBackupService
{
    Task CreateAsync(OperationSnapshot snapshot, CancellationToken token);
    Task<OperationSnapshot?> LoadLatestAsync(CancellationToken token);
    Task<IApplicationRestoreLease> AcquireRestoreLeaseAsync(Guid operationId, CancellationToken token);
}

internal interface IApplicationRestoreLease : IAsyncDisposable
{
    bool AlreadyRestored { get; }
    Task MarkCompletedAsync(DateTimeOffset completedAtUtc, CancellationToken token);
}

internal interface IGlobalIconOperations
{
    GlobalSnapshotResult CaptureSnapshot();
    OperationResult Apply(string icoPath);
    Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token);
}

internal interface ICompatibleFolderOperations
{
    Task<CompatibleOperationResult> ApplyAsync(
        IReadOnlyList<FolderPlanItem> plan,
        string icoPath,
        Guid operationId,
        CancellationToken token);

    Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token);
}

internal interface IApplicationShellRefreshService
{
    OperationResult Refresh();
}

internal sealed record IconArtifactResult(bool IsSuccess, string? Path, string? Error)
{
    internal static IconArtifactResult Success(string path) => new(true, path, null);
    internal static IconArtifactResult Failure(string error) => new(false, null, error);
}

internal sealed record OperationResult(bool IsSuccess, string? Error)
{
    internal static OperationResult Success() => new(true, null);
    internal static OperationResult Failure(string error) => new(false, error);
}

internal sealed record GlobalSnapshotResult(
    bool IsSuccess,
    RegistryValueSnapshot? RegistryValue3,
    RegistryValueSnapshot? RegistryValue4,
    string? Error);

internal sealed record CompatibleOperationResult(
    bool IsSuccess,
    int Applied,
    int Skipped,
    int Failed,
    bool Cancelled,
    string? Error)
{
    internal IReadOnlyList<ApplyRecord> Successful { get; init; } = [];
    internal IReadOnlyList<ApplyRecord> SkippedItems { get; init; } = [];
    internal IReadOnlyList<ApplyFailure> Failures { get; init; } = [];
    internal IReadOnlyList<ApplyFailure> Unresolved { get; init; } = [];
}

internal sealed record RestoreOperationResult(
    bool IsSuccess,
    IReadOnlyList<ApplyRecord> Successful,
    IReadOnlyList<ApplyRecord> Skipped,
    IReadOnlyList<ApplyRecord> Changed,
    IReadOnlyList<ApplyFailure> Failures)
{
    internal static RestoreOperationResult Success() => new(true, [], [], [], []);
    internal static RestoreOperationResult Failure(string error, string? path = null) =>
        new(false, [], [], [], [new ApplyFailure(ApplyPhase.Restore, error, path)]);
}

public sealed class ThemeApplicationService
{
    private readonly IThemeValidationService validator;
    private readonly IIconArtifactService icons;
    private readonly IKnownFolderPlanningService planner;
    private readonly IApplicationBackupService backup;
    private readonly IGlobalIconOperations global;
    private readonly ICompatibleFolderOperations compatible;
    private readonly IApplicationShellRefreshService refresh;
    private readonly Func<Guid> newOperationId;
    private readonly Func<DateTimeOffset> utcNow;

    public ThemeApplicationService()
    {
        var backupService = new BackupService();
        validator = new DefaultThemeValidationService();
        icons = new DefaultIconArtifactService();
        planner = new DefaultKnownFolderPlanningService(new KnownFolderService());
        backup = new DefaultApplicationBackupService(backupService);
        global = new DefaultGlobalIconOperations(new GlobalIconService());
        compatible = new DefaultCompatibleFolderOperations(new CompatibleFolderService(backupService));
        refresh = new DefaultApplicationShellRefreshService(new ShellRefreshService());
        newOperationId = Guid.NewGuid;
        utcNow = () => DateTimeOffset.UtcNow;
    }

    internal ThemeApplicationService(
        IThemeValidationService validator,
        IIconArtifactService icons,
        IKnownFolderPlanningService planner,
        IApplicationBackupService backup,
        IGlobalIconOperations global,
        ICompatibleFolderOperations compatible,
        IApplicationShellRefreshService refresh,
        Func<Guid> newOperationId,
        Func<DateTimeOffset> utcNow)
    {
        this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this.icons = icons ?? throw new ArgumentNullException(nameof(icons));
        this.planner = planner ?? throw new ArgumentNullException(nameof(planner));
        this.backup = backup ?? throw new ArgumentNullException(nameof(backup));
        this.global = global ?? throw new ArgumentNullException(nameof(global));
        this.compatible = compatible ?? throw new ArgumentNullException(nameof(compatible));
        this.refresh = refresh ?? throw new ArgumentNullException(nameof(refresh));
        this.newOperationId = newOperationId ?? throw new ArgumentNullException(nameof(newOperationId));
        this.utcNow = utcNow ?? throw new ArgumentNullException(nameof(utcNow));
    }

    public async Task<ApplyPlan> PlanAsync(ApplyRequest request, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.IconSource);
        token.ThrowIfCancellationRequested();

        var validationErrors = request.IconSource.Kind == IconSourceKind.BuiltIn
            ? validator.Validate(request.IconSource.Theme!).Errors
            : [];
        if (validationErrors.Count > 0)
        {
            return new ApplyPlan(request.IconSource, request.Mode, [], validationErrors);
        }

        if (request.Mode == ApplicationMode.Global)
        {
            return new ApplyPlan(request.IconSource, request.Mode, [], []);
        }

        if (request.ExplicitRoots is null || request.ExplicitRoots.Count == 0)
        {
            return new ApplyPlan(
                request.IconSource,
                request.Mode,
                [],
                ["Compatibility mode requires at least one explicit root."]);
        }

        var folders = new List<FolderPlanItem>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var root in request.ExplicitRoots)
        {
            token.ThrowIfCancellationRequested();
            await foreach (var item in planner.PlanTreeAsync(root, token).WithCancellation(token).ConfigureAwait(false))
            {
                var rootedItem = item with
                {
                    SelectedRoot = item.SelectedRoot ?? root,
                    CanonicalPath = item.CanonicalPath ?? item.Path,
                    CanonicalRoot = item.CanonicalRoot ?? root
                };
                if (seen.Add(rootedItem.Path))
                {
                    folders.Add(rootedItem);
                }
            }
        }

        return new ApplyPlan(request.IconSource, request.Mode, folders, []);
    }

    public async Task<ApplyResult> ApplyAsync(
        ApplyPlan plan,
        IProgress<ApplyProgress> progress,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(progress);

        if (!plan.CanApply)
        {
            return FailedBeforeSnapshot(
                plan.ValidationErrors.Select(error => new ApplyFailure(ApplyPhase.Validation, error)).ToArray());
        }

        var operationId = newOperationId();
        progress.Report(new ApplyProgress(ApplyPhase.Rendering, "Rendering and verifying the icon."));
        IconArtifactResult artifact;
        try
        {
            artifact = await icons.CreateAndVerifyAsync(plan.IconSource, operationId, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Rendering, "Icon generation was cancelled.")]);
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Rendering, exception.Message)]);
        }

        if (!artifact.IsSuccess || string.IsNullOrWhiteSpace(artifact.Path))
        {
            return FailedBeforeSnapshot([
                new ApplyFailure(ApplyPhase.Rendering, artifact.Error ?? "Icon generation did not produce a verified ICO.")]);
        }

        SnapshotBuildResult snapshotResult;
        try
        {
            snapshotResult = BuildSnapshot(plan.Mode, operationId);
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Snapshot, exception.Message)]);
        }
        if (!snapshotResult.IsSuccess || snapshotResult.Snapshot is null)
        {
            return FailedBeforeSnapshot([
                new ApplyFailure(ApplyPhase.Snapshot, snapshotResult.Error ?? "Could not capture the original system state.")]);
        }

        progress.Report(new ApplyProgress(ApplyPhase.Snapshot, "Saving the recovery snapshot."));
        try
        {
            await backup.CreateAsync(snapshotResult.Snapshot, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Snapshot, "Snapshot creation was cancelled.")]);
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return FailedBeforeSnapshot([new ApplyFailure(ApplyPhase.Snapshot, exception.Message)]);
        }

        progress.Report(new ApplyProgress(ApplyPhase.SystemMutation, "Applying the folder icon."));
        var mutation = await ApplyMutationAsync(plan, artifact.Path, operationId, token).ConfigureAwait(false);
        if (!mutation.IsSuccess)
        {
            return await RollBackFailedApplyAsync(
                plan,
                snapshotResult.Snapshot,
                operationId,
                mutation,
                progress).ConfigureAwait(false);
        }

        progress.Report(new ApplyProgress(ApplyPhase.Refresh, "Refreshing Windows folder icons."));
        var refreshResult = TryRefresh();
        if (!refreshResult.IsSuccess)
        {
            var refreshFailure = new MutationResult(
                false,
                mutation.Successful,
                mutation.Skipped,
                [new ApplyFailure(ApplyPhase.Refresh, refreshResult.Error ?? "Shell refresh failed.")]);
            return await RollBackFailedApplyAsync(
                plan,
                snapshotResult.Snapshot,
                operationId,
                refreshFailure,
                progress).ConfigureAwait(false);
        }

        return new ApplyResult(
            true,
            operationId,
            mutation.Successful,
            mutation.Skipped,
            [],
            false,
            false)
        {
            IconArtifactPath = artifact.Path
        };
    }

    public async Task<RestoreResult> RestoreLatestAsync(
        IProgress<ApplyProgress> progress,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(progress);
        OperationSnapshot? snapshot;
        try
        {
            snapshot = await backup.LoadLatestAsync(token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return new RestoreResult(false, false, null, [new ApplyFailure(ApplyPhase.Restore, "Restore was cancelled.")]);
        }
        catch (Exception exception)
        {
            return new RestoreResult(false, false, null, [new ApplyFailure(ApplyPhase.Restore, exception.Message)]);
        }

        if (snapshot is null)
        {
            return new RestoreResult(true, true, null, []);
        }

        IApplicationRestoreLease restoreLease;
        try
        {
            restoreLease = await backup.AcquireRestoreLeaseAsync(snapshot.Id, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message.Length == 0 ? "Restore was cancelled." : exception.Message);
        }
        catch (Exception exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message);
        }

        await using var heldRestoreLease = restoreLease;
        if (heldRestoreLease.AlreadyRestored)
        {
            return new RestoreResult(true, true, snapshot.Id, []);
        }

        progress.Report(new ApplyProgress(ApplyPhase.Restore, "Restoring the latest saved state."));
        RestoreOperationResult restored;
        try
        {
            restored = await RestoreMatchingAsync(snapshot, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message.Length == 0 ? "Restore was cancelled." : exception.Message);
        }
        catch (Exception exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message);
        }

        if (!restored.IsSuccess)
        {
            return new RestoreResult(false, false, snapshot.Id, restored.Failures)
            {
                Successful = restored.Successful,
                Skipped = restored.Skipped,
                Changed = restored.Changed
            };
        }

        progress.Report(new ApplyProgress(ApplyPhase.Refresh, "Refreshing Windows folder icons."));
        OperationResult refreshed;
        try
        {
            token.ThrowIfCancellationRequested();
            refreshed = refresh.Refresh();
        }
        catch (OperationCanceledException exception)
        {
            return RestoreFailure(
                snapshot.Id,
                exception.Message.Length == 0 ? "Restore was cancelled before refresh." : exception.Message,
                restored,
                ApplyPhase.Refresh);
        }
        catch (Exception exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message, restored, ApplyPhase.Refresh);
        }

        if (!refreshed.IsSuccess)
        {
            return RestoreFailure(
                snapshot.Id,
                refreshed.Error ?? "Shell refresh failed.",
                restored,
                ApplyPhase.Refresh);
        }

        try
        {
            token.ThrowIfCancellationRequested();
            await heldRestoreLease.MarkCompletedAsync(utcNow(), token).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            return RestoreFailure(
                snapshot.Id,
                exception.Message.Length == 0 ? "Restore completion was cancelled." : exception.Message,
                restored);
        }
        catch (Exception exception)
        {
            return RestoreFailure(snapshot.Id, exception.Message, restored);
        }

        return new RestoreResult(true, false, snapshot.Id, [])
        {
            Successful = restored.Successful,
            Skipped = restored.Skipped,
            Changed = restored.Changed
        };
    }

    private SnapshotBuildResult BuildSnapshot(ApplicationMode mode, Guid operationId)
    {
        if (mode == ApplicationMode.Compatible)
        {
            return SnapshotBuildResult.Success(new OperationSnapshot(
                operationId,
                utcNow(),
                new RegistryValueSnapshot("3", false, null),
                new RegistryValueSnapshot("4", false, null),
                [],
                false)
            {
                ApplicationMode = ApplicationMode.Compatible
            });
        }

        var captured = global.CaptureSnapshot();
        if (!captured.IsSuccess || captured.RegistryValue3 is null || captured.RegistryValue4 is null)
        {
            return SnapshotBuildResult.Failure(captured.Error ?? "Could not read the current folder icon mappings.");
        }

        return SnapshotBuildResult.Success(new OperationSnapshot(
            operationId,
            utcNow(),
            captured.RegistryValue3,
            captured.RegistryValue4,
            [],
            false)
        {
            ApplicationMode = ApplicationMode.Global
        });
    }

    private async Task<MutationResult> ApplyMutationAsync(
        ApplyPlan plan,
        string icoPath,
        Guid operationId,
        CancellationToken token)
    {
        try
        {
            if (plan.Mode == ApplicationMode.Global)
            {
                var globalResult = global.Apply(icoPath);
                return globalResult.IsSuccess
                    ? new MutationResult(true, [new ApplyRecord(null, "Global folder icon mapping applied.")], [], [])
                    : new MutationResult(false, [], [], [
                        new ApplyFailure(ApplyPhase.SystemMutation, globalResult.Error ?? "Global icon application failed.")]);
            }

            var result = await compatible.ApplyAsync(plan.Folders, icoPath, operationId, token).ConfigureAwait(false);
            var skipped = result.SkippedItems;
            var successful = result.Successful;
            var failures = result.Failures.Count > 0
                ? result.Failures
                : result.IsSuccess && result.Failed == 0 && !result.Cancelled
                    ? []
                    : [new ApplyFailure(
                        ApplyPhase.SystemMutation,
                        result.Error ?? (result.Cancelled
                            ? "Compatibility application was cancelled."
                            : $"Compatibility application failed for {result.Failed} folder(s)."))];
            return new MutationResult(failures.Count == 0, successful, skipped, failures)
            {
                Unresolved = result.Unresolved
            };
        }
        catch (OperationCanceledException)
        {
            return new MutationResult(false, [], [], [
                new ApplyFailure(ApplyPhase.SystemMutation, "System mutation was cancelled.")]);
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return new MutationResult(false, [], [], [new ApplyFailure(ApplyPhase.SystemMutation, exception.Message)]);
        }
    }

    private async Task<ApplyResult> RollBackFailedApplyAsync(
        ApplyPlan plan,
        OperationSnapshot snapshot,
        Guid operationId,
        MutationResult mutation,
        IProgress<ApplyProgress> progress)
    {
        progress.Report(new ApplyProgress(ApplyPhase.Rollback, "Rolling back the failed operation."));
        RestoreOperationResult rollback;
        var rollbackCompletedDurably = false;
        try
        {
            await using var restoreLease = await backup.AcquireRestoreLeaseAsync(operationId, CancellationToken.None).ConfigureAwait(false);
            if (restoreLease.AlreadyRestored)
            {
                rollback = RestoreOperationResult.Success();
                rollbackCompletedDurably = mutation.Unresolved.Count == 0;
            }
            else
            {
                rollback = plan.Mode == ApplicationMode.Global
                    ? await global.RestoreAsync(snapshot, CancellationToken.None).ConfigureAwait(false)
                    : await compatible.RestoreAsync(snapshot, CancellationToken.None).ConfigureAwait(false);
                if (rollback.IsSuccess)
                {
                    var refreshed = TryRefresh();
                    if (!refreshed.IsSuccess)
                    {
                        rollback = RestoreOperationResult.Failure(refreshed.Error ?? "Shell refresh after rollback failed.");
                    }
                    else if (mutation.Unresolved.Count == 0)
                    {
                        await restoreLease.MarkCompletedAsync(utcNow(), CancellationToken.None).ConfigureAwait(false);
                        rollbackCompletedDurably = true;
                    }
                }
            }
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            rollback = RestoreOperationResult.Failure(exception.Message);
        }

        var failures = mutation.Failures.ToList();
        failures.AddRange(mutation.Unresolved.Select(failure => failure with { Phase = ApplyPhase.Rollback }));
        if (!rollback.IsSuccess || !rollbackCompletedDurably)
        {
            if (rollback.Failures.Count == 0 && mutation.Unresolved.Count == 0)
            {
                failures.Add(new ApplyFailure(ApplyPhase.Rollback, "Rollback did not reach durable completion."));
            }
            else
            {
                failures.AddRange(rollback.Failures.Select(failure => failure with { Phase = ApplyPhase.Rollback }));
            }
        }

        return new ApplyResult(
            false,
            operationId,
            mutation.Successful,
            mutation.Skipped,
            failures,
            true,
            rollback.IsSuccess && rollbackCompletedDurably);
    }

    private Task<RestoreOperationResult> RestoreMatchingAsync(OperationSnapshot snapshot, CancellationToken token) =>
        snapshot.ApplicationMode == ApplicationMode.Global
            ? global.RestoreAsync(snapshot, token)
            : compatible.RestoreAsync(snapshot, token);

    private OperationResult TryRefresh()
    {
        try
        {
            return refresh.Refresh();
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return OperationResult.Failure(exception.Message);
        }
    }

    private static ApplyResult FailedBeforeSnapshot(IReadOnlyList<ApplyFailure> failures) =>
        new(false, null, [], [], failures, false, false);

    private static RestoreResult RestoreFailure(Guid? operationId, string error, ApplyPhase phase = ApplyPhase.Restore) =>
        new(false, false, operationId, [new ApplyFailure(phase, error)]);

    private static RestoreResult RestoreFailure(
        Guid? operationId,
        string error,
        RestoreOperationResult restored,
        ApplyPhase phase = ApplyPhase.Restore) =>
        new RestoreResult(false, false, operationId, [new ApplyFailure(phase, error)])
        {
            Successful = restored.Successful,
            Skipped = restored.Skipped,
            Changed = restored.Changed
        };

    private static bool IsOperationalFailure(Exception exception) =>
        exception is IOException
            or UnauthorizedAccessException
            or System.Security.SecurityException
            or InvalidOperationException
            or ArgumentException;

    private sealed record SnapshotBuildResult(bool IsSuccess, OperationSnapshot? Snapshot, string? Error)
    {
        internal static SnapshotBuildResult Success(OperationSnapshot snapshot) => new(true, snapshot, null);
        internal static SnapshotBuildResult Failure(string error) => new(false, null, error);
    }

    private sealed record MutationResult(
        bool IsSuccess,
        IReadOnlyList<ApplyRecord> Successful,
        IReadOnlyList<ApplyRecord> Skipped,
        IReadOnlyList<ApplyFailure> Failures)
    {
        internal IReadOnlyList<ApplyFailure> Unresolved { get; init; } = [];
    }
}

internal sealed class DefaultThemeValidationService : IThemeValidationService
{
    public ThemeValidationResult Validate(FolderTheme theme) => ThemeValidator.Validate(theme);
}

internal sealed class DefaultKnownFolderPlanningService(KnownFolderService service) : IKnownFolderPlanningService
{
    public IAsyncEnumerable<FolderPlanItem> PlanTreeAsync(string root, CancellationToken token) =>
        service.PlanTreeAsync(root, token);
}

internal sealed class DefaultApplicationBackupService(BackupService service) : IApplicationBackupService
{
    public Task CreateAsync(OperationSnapshot snapshot, CancellationToken token) => service.CreateAsync(snapshot, token);
    public Task<OperationSnapshot?> LoadLatestAsync(CancellationToken token) => service.LoadLatestAsync(token);

    public async Task<IApplicationRestoreLease> AcquireRestoreLeaseAsync(Guid operationId, CancellationToken token) =>
        new DefaultApplicationRestoreLease(await service.AcquireRestoreLeaseAsync(operationId, token).ConfigureAwait(false));
}

internal sealed class DefaultApplicationRestoreLease(BackupService.RestoreLease lease) : IApplicationRestoreLease
{
    public bool AlreadyRestored => lease.AlreadyRestored;

    public Task MarkCompletedAsync(DateTimeOffset completedAtUtc, CancellationToken token) =>
        lease.MarkCompletedAsync(completedAtUtc, token);

    public ValueTask DisposeAsync() => lease.DisposeAsync();
}

internal sealed class DefaultGlobalIconOperations(GlobalIconService service) : IGlobalIconOperations
{
    public GlobalSnapshotResult CaptureSnapshot()
    {
        var result = service.CaptureSnapshot();
        return new GlobalSnapshotResult(
            result.Success,
            result.RegistryValue3,
            result.RegistryValue4,
            result.Error);
    }

    public OperationResult Apply(string icoPath)
    {
        var result = service.ApplyWithoutRefresh(icoPath);
        return result.Success ? OperationResult.Success() : OperationResult.Failure(result.Error ?? "Global application failed.");
    }

    public Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var result = service.RestoreWithoutRefresh(snapshot);
        return Task.FromResult(result.Success
            ? RestoreOperationResult.Success()
            : RestoreOperationResult.Failure(result.Error ?? "Global restore failed."));
    }
}

internal sealed class DefaultCompatibleFolderOperations(CompatibleFolderService service) : ICompatibleFolderOperations
{
    public async Task<CompatibleOperationResult> ApplyAsync(
        IReadOnlyList<FolderPlanItem> plan,
        string icoPath,
        Guid operationId,
        CancellationToken token)
    {
        var result = await service.ApplyWithExistingSnapshotAsync(plan, icoPath, operationId, token).ConfigureAwait(false);
        var success = result.Failed == 0 && !result.Cancelled;
        return new CompatibleOperationResult(
            success,
            result.Applied,
            result.Skipped,
            result.Failed,
            result.Cancelled,
            success ? null : "One or more compatibility folder operations failed.")
        {
            Successful = result.Outcomes
                .Where(outcome => outcome.Kind == CompatibleApplyOutcomeKind.Applied)
                .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
                .ToArray(),
            SkippedItems = result.Outcomes
                .Where(outcome => outcome.Kind == CompatibleApplyOutcomeKind.Skipped)
                .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
                .ToArray(),
            Failures = result.Outcomes
                .Where(outcome => outcome.Kind == CompatibleApplyOutcomeKind.Failed)
                .Select(outcome => new ApplyFailure(
                    ApplyPhase.SystemMutation,
                    outcome.Detail ?? "Compatibility folder application failed.",
                    outcome.Path))
                .ToArray(),
            Unresolved = result.Unresolved
                .Select(outcome => new ApplyFailure(
                    ApplyPhase.Rollback,
                    outcome.Detail ?? "Compatibility folder mutation remains unresolved.",
                    outcome.Path))
                .ToArray()
        };
    }

    public async Task<RestoreOperationResult> RestoreAsync(OperationSnapshot snapshot, CancellationToken token)
    {
        var result = await service.RestoreAsync(snapshot.Id, token).ConfigureAwait(false);
        var successful = result.Outcomes
            .Where(outcome => outcome.Kind == CompatibleRestoreOutcomeKind.Restored)
            .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
            .ToArray();
        var skipped = result.Outcomes
            .Where(outcome => outcome.Kind == CompatibleRestoreOutcomeKind.Missing)
            .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
            .ToArray();
        var changed = result.Outcomes
            .Where(outcome => outcome.Kind == CompatibleRestoreOutcomeKind.ChangedSinceApply)
            .Select(outcome => new ApplyRecord(outcome.Path, outcome.Detail))
            .ToArray();
        var failures = result.Outcomes
            .Where(outcome => outcome.Kind == CompatibleRestoreOutcomeKind.Failed)
            .Select(outcome => new ApplyFailure(
                ApplyPhase.Restore,
                outcome.Detail ?? "Compatibility folder restore failed.",
                outcome.Path))
            .Concat(changed.Select(outcome => new ApplyFailure(
                ApplyPhase.Restore,
                outcome.Detail ?? "Desktop.ini changed since apply.",
                outcome.Path)))
            .ToList();
        if (result.Cancelled)
        {
            failures.Add(new ApplyFailure(ApplyPhase.Restore, "Compatibility restore was cancelled."));
        }

        return new RestoreOperationResult(result.IsFullyRestored, successful, skipped, changed, failures);
    }
}

internal sealed class DefaultApplicationShellRefreshService(ShellRefreshService service) : IApplicationShellRefreshService
{
    public OperationResult Refresh()
    {
        var result = service.RefreshIcons();
        return result.Success
            ? OperationResult.Success()
            : OperationResult.Failure(result.Error ?? "Shell refresh failed.");
    }
}

internal sealed class DefaultIconArtifactService : IIconArtifactService
{
    private readonly string iconsRoot;

    internal DefaultIconArtifactService()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio",
            "icons"))
    {
    }

    internal DefaultIconArtifactService(string iconsRoot)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(iconsRoot);
        this.iconsRoot = Path.GetFullPath(iconsRoot);
    }

    public async Task<IconArtifactResult> CreateAndVerifyAsync(
        IconSource source,
        Guid operationId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(source);
        var frames = new Dictionary<int, byte[]>();
        foreach (var size in FolderIconRenderer.RequiredSizes)
        {
            token.ThrowIfCancellationRequested();
            var bitmap = source.Kind switch
            {
                IconSourceKind.BuiltIn => FolderIconRenderer.Render(source.Theme!, size),
                IconSourceKind.ImportedImage => ImportedIconRenderer.Render(source.ImportedImagePath!, size),
                _ => throw new ArgumentOutOfRangeException(nameof(source))
            };
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using var stream = new MemoryStream();
            encoder.Save(stream);
            frames.Add(size, stream.ToArray());
        }

        var ico = IcoEncoder.Encode(frames);
        VerifyIco(ico);

        Directory.CreateDirectory(iconsRoot);
        var destination = Path.Combine(iconsRoot, $"{operationId:D}.ico");
        var temporary = destination + ".tmp";
        try
        {
            await File.WriteAllBytesAsync(temporary, ico, token).ConfigureAwait(false);
            File.Move(temporary, destination, overwrite: true);
            var persisted = await File.ReadAllBytesAsync(destination, token).ConfigureAwait(false);
            if (!persisted.AsSpan().SequenceEqual(ico))
            {
                throw new IOException("The persisted ICO did not match the verified icon payload.");
            }
        }
        finally
        {
            if (File.Exists(temporary)) File.Delete(temporary);
        }

        return IconArtifactResult.Success(destination);
    }

    private static void VerifyIco(byte[] ico)
    {
        using var stream = new MemoryStream(ico, writable: false);
        using var reader = new BinaryReader(stream);
        if (reader.ReadUInt16() != 0
            || reader.ReadUInt16() != 1
            || reader.ReadUInt16() != FolderIconRenderer.RequiredSizes.Length)
        {
            throw new InvalidDataException("The generated ICO header is invalid.");
        }

        foreach (var size in FolderIconRenderer.RequiredSizes)
        {
            var width = reader.ReadByte();
            var height = reader.ReadByte();
            reader.ReadBytes(14);
            var expected = size == 256 ? 0 : size;
            if (width != expected || height != expected)
            {
                throw new InvalidDataException("The generated ICO frame table is invalid.");
            }
        }
    }
}
```

## src/FolderThemeStudio.Core/FolderThemeStudio.Core.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-windows10.0.19041.0</TargetFramework>
    <UseWPF>true</UseWPF>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

## src/FolderThemeStudio.Core/Recovery/BackupModels.cs

```csharp
using System.IO;
using System.Text.RegularExpressions;
using FolderThemeStudio.Core.Application;
using Microsoft.Win32;

namespace FolderThemeStudio.Core.Recovery;

public sealed record RegistryValueSnapshot(
    string Name,
    bool Existed,
    string? Value,
    RegistryValueKind? Kind = null)
{
    internal void Validate(string expectedName)
    {
        if (!string.Equals(Name, expectedName, StringComparison.Ordinal))
        {
            throw new ArgumentException($"The snapshot must describe registry value '{expectedName}'.", nameof(Name));
        }

        if (Existed != (Value is not null) || Existed != (Kind is not null))
        {
            throw new ArgumentException("A registry value is present exactly when its raw value and kind are both saved.", nameof(Value));
        }

        if (Kind is not null && Kind is not (RegistryValueKind.String or RegistryValueKind.ExpandString))
        {
            throw new ArgumentException("Only REG_SZ and REG_EXPAND_SZ folder mappings are supported.", nameof(Kind));
        }
    }
}

public enum FolderMutationPhase
{
    FileCreated,
    FolderAttributeIntentPersisted,
    Completed,
    OriginalCaptured
}

public sealed record FolderMutationRecord(
    string FolderPath,
    FileAttributes OriginalFolderAttributes,
    string CreatedFilePath,
    string ExpectedDesktopIniSha256,
    FileAttributes AddedFolderAttributes,
    bool IsCompleted)
{
    private static readonly Regex Sha256 = new("^[0-9A-Fa-f]{64}$", RegexOptions.CultureInvariant);

    public FolderMutationPhase Phase { get; init; } = IsCompleted
        ? FolderMutationPhase.Completed
        : FolderMutationPhase.FileCreated;
    public bool FileOriginallyExisted { get; init; }
    public string? OriginalDesktopIniBase64 { get; init; }
    public FileAttributes? OriginalDesktopIniAttributes { get; init; }

    internal void Validate()
    {
        if (string.IsNullOrWhiteSpace(FolderPath))
        {
            throw new ArgumentException("A mutation must identify its folder.", nameof(FolderPath));
        }

        if (string.IsNullOrWhiteSpace(CreatedFilePath))
        {
            throw new ArgumentException("A mutation must identify the file created by the tool.", nameof(CreatedFilePath));
        }

        if (!IsDesktopIniInsideFolder(FolderPath, CreatedFilePath))
        {
            throw new ArgumentException("The tool-owned file must be desktop.ini directly inside the recorded folder.", nameof(CreatedFilePath));
        }

        if (!Sha256.IsMatch(ExpectedDesktopIniSha256))
        {
            throw new ArgumentException("The expected Desktop.ini hash must be a SHA-256 value.", nameof(ExpectedDesktopIniSha256));
        }

        if ((AddedFolderAttributes & ~FileAttributes.ReadOnly) != 0)
        {
            throw new ArgumentException("A compatibility mutation may record only the ReadOnly folder attribute bit.", nameof(AddedFolderAttributes));
        }

        if (!Enum.IsDefined(Phase))
        {
            throw new ArgumentException("A mutation phase is invalid.", nameof(Phase));
        }

        if (FileOriginallyExisted)
        {
            if (string.IsNullOrWhiteSpace(OriginalDesktopIniBase64) || OriginalDesktopIniAttributes is null)
                throw new ArgumentException("An existing Desktop.ini mutation requires original bytes and attributes.");
            try { _ = Convert.FromBase64String(OriginalDesktopIniBase64); }
            catch (FormatException exception) { throw new ArgumentException("Original Desktop.ini bytes must be base64.", exception); }
        }
        else if (OriginalDesktopIniBase64 is not null || OriginalDesktopIniAttributes is not null)
        {
            throw new ArgumentException("A newly created Desktop.ini cannot include an original-file snapshot.");
        }

        if ((Phase is FolderMutationPhase.FileCreated or FolderMutationPhase.OriginalCaptured) && AddedFolderAttributes != 0)
        {
            throw new ArgumentException("A file-created mutation cannot claim folder attribute changes.", nameof(AddedFolderAttributes));
        }

        if (IsCompleted != (Phase == FolderMutationPhase.Completed))
        {
            throw new ArgumentException("Mutation completion must agree with its durable phase.", nameof(IsCompleted));
        }

        if ((AddedFolderAttributes & OriginalFolderAttributes) != 0)
        {
            throw new ArgumentException("A mutation cannot claim an attribute bit that was already present.", nameof(AddedFolderAttributes));
        }
    }

    private static bool IsDesktopIniInsideFolder(string folderPath, string createdFilePath)
    {
        try
        {
            var canonicalFolder = GetCanonicalRootedPath(folderPath);
            var canonicalFile = GetCanonicalRootedPath(createdFilePath);
            return canonicalFolder is not null
                && canonicalFile is not null
                && string.Equals(canonicalFile, Path.Combine(canonicalFolder, "desktop.ini"), StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException)
        {
            return false;
        }
    }

    private static string? GetCanonicalRootedPath(string path)
    {
        if (!Path.IsPathRooted(path))
        {
            return null;
        }

        var fullPath = Path.GetFullPath(path);
        var canonicalPath = Path.TrimEndingDirectorySeparator(fullPath);
        return string.Equals(path, canonicalPath, StringComparison.OrdinalIgnoreCase)
            ? canonicalPath
            : null;
    }
}

public sealed record OperationSnapshot(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    RegistryValueSnapshot RegistryValue3,
    RegistryValueSnapshot RegistryValue4,
    IReadOnlyList<FolderMutationRecord> Mutations,
    bool IsCompleted)
{
    public ApplicationMode ApplicationMode { get; init; } = ApplicationMode.Global;

    internal void Validate()
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("An operation snapshot requires a non-empty identifier.", nameof(Id));
        }

        if (!Enum.IsDefined(ApplicationMode))
        {
            throw new ArgumentException("The snapshot application mode is invalid.", nameof(ApplicationMode));
        }

        RegistryValue3.Validate("3");
        RegistryValue4.Validate("4");
        foreach (var mutation in Mutations)
        {
            mutation.Validate();
        }
    }

    public bool Equals(OperationSnapshot? other) =>
        other is not null
        && Id == other.Id
        && CreatedAtUtc == other.CreatedAtUtc
        && EqualityComparer<RegistryValueSnapshot>.Default.Equals(RegistryValue3, other.RegistryValue3)
        && EqualityComparer<RegistryValueSnapshot>.Default.Equals(RegistryValue4, other.RegistryValue4)
        && ApplicationMode == other.ApplicationMode
        && IsCompleted == other.IsCompleted
        && Mutations.SequenceEqual(other.Mutations);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Id);
        hash.Add(CreatedAtUtc);
        hash.Add(RegistryValue3);
        hash.Add(RegistryValue4);
        hash.Add(ApplicationMode);
        hash.Add(IsCompleted);
        foreach (var mutation in Mutations)
        {
            hash.Add(mutation);
        }

        return hash.ToHashCode();
    }
}
```

## src/FolderThemeStudio.Core/Recovery/BackupService.cs

```csharp
using System.IO;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Collections.Concurrent;

namespace FolderThemeStudio.Core.Recovery;

internal interface IFileSystem
{
    void CreateDirectory(string path);
    bool FileExists(string path);
    Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken);
    Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken);
    void MoveFile(string sourcePath, string destinationPath);
    void ReplaceFile(string sourcePath, string destinationPath);
    void DeleteFile(string path);
}

internal interface IRestoreProcessLock
{
    Task<IAsyncDisposable> AcquireAsync(string lockPath, CancellationToken cancellationToken);
}

internal sealed class FileRestoreProcessLock : IRestoreProcessLock
{
    public async Task<IAsyncDisposable> AcquireAsync(string lockPath, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lockPath);
        var fullPath = Path.GetFullPath(lockPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)
            ?? throw new InvalidOperationException("A restore lock requires a parent directory."));

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var stream = new FileStream(
                    fullPath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    1,
                    FileOptions.WriteThrough);
                return new FileRestoreProcessLease(stream);
            }
            catch (IOException)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(40), cancellationToken).ConfigureAwait(false);
            }
        }
    }

    private sealed class FileRestoreProcessLease(FileStream stream) : IAsyncDisposable
    {
        public ValueTask DisposeAsync() => stream.DisposeAsync();
    }
}

internal sealed class NoOpRestoreProcessLock : IRestoreProcessLock
{
    public Task<IAsyncDisposable> AcquireAsync(string lockPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IAsyncDisposable>(new Lease());
    }

    private sealed class Lease : IAsyncDisposable
    {
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}

public sealed class BackupService
{
    private const int CurrentSchemaVersion = 3;
    private const string SnapshotFileName = "snapshot.v1.json";
    private const string LatestFileName = "latest.v1.json";
    private const string RestoreFileName = "restore.v1.json";
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> RestoreGates = new(StringComparer.OrdinalIgnoreCase);
    private readonly IFileSystem fileSystem;
    private readonly IRestoreProcessLock restoreProcessLock;
    private readonly string backupsRoot;
    private readonly SemaphoreSlim gate = new(1, 1);

    public BackupService()
        : this(new WindowsFileSystem(), Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio",
            "backups"), new FileRestoreProcessLock())
    {
    }

    internal static BackupService CreateForTesting(IFileSystem fileSystem, string backupsRoot) =>
        new(fileSystem, backupsRoot, new NoOpRestoreProcessLock());

    internal static BackupService CreateForTesting(
        IFileSystem fileSystem,
        string backupsRoot,
        IRestoreProcessLock restoreProcessLock) =>
        new(fileSystem, backupsRoot, restoreProcessLock);

    private BackupService(IFileSystem fileSystem, string backupsRoot, IRestoreProcessLock restoreProcessLock)
    {
        this.fileSystem = fileSystem;
        this.backupsRoot = backupsRoot;
        this.restoreProcessLock = restoreProcessLock;
    }

    public async Task CreateAsync(OperationSnapshot snapshot, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        snapshot.Validate();

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshotPath = GetSnapshotPath(snapshot.Id);
            if (fileSystem.FileExists(snapshotPath))
            {
                var existing = await LoadByIdAsync(snapshot.Id, cancellationToken).ConfigureAwait(false);
                if (existing != snapshot)
                {
                    throw new InvalidOperationException($"A different snapshot for operation '{snapshot.Id}' already exists.");
                }

                await WriteJsonAtomicallyAsync(GetLatestPath(), new LatestDocument(CurrentSchemaVersion, snapshot.Id), cancellationToken).ConfigureAwait(false);
                return;
            }

            fileSystem.CreateDirectory(GetOperationDirectory(snapshot.Id));
            await WriteJsonAtomicallyAsync(snapshotPath, new SnapshotDocument(CurrentSchemaVersion, snapshot), cancellationToken).ConfigureAwait(false);
            await WriteJsonAtomicallyAsync(GetLatestPath(), new LatestDocument(CurrentSchemaVersion, snapshot.Id), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task AppendMutationAsync(Guid operationId, FolderMutationRecord mutation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(mutation);
        mutation.Validate();

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshot = await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
            var existing = snapshot.Mutations.SingleOrDefault(candidate => string.Equals(
                candidate.CreatedFilePath,
                mutation.CreatedFilePath,
                StringComparison.OrdinalIgnoreCase));
            if (existing is not null)
            {
                if (existing == mutation)
                {
                    return;
                }

                throw new InvalidOperationException("An operation cannot claim the same Desktop.ini path with different ownership data.");
            }

            var updated = snapshot with { Mutations = snapshot.Mutations.Append(mutation).ToArray() };
            await WriteJsonAtomicallyAsync(GetSnapshotPath(operationId), new SnapshotDocument(CurrentSchemaVersion, updated), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task CompleteAsync(Guid operationId, CancellationToken cancellationToken = default)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshot = await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
            var updated = snapshot with { IsCompleted = true };
            await WriteJsonAtomicallyAsync(GetSnapshotPath(operationId), new SnapshotDocument(CurrentSchemaVersion, updated), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task CompleteMutationAsync(
        Guid operationId,
        string createdFilePath,
        FileAttributes addedFolderAttributes,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(createdFilePath);

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshot = await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
            var matchingIndexes = snapshot.Mutations
                .Select((mutation, index) => (mutation, index))
                .Where(candidate => string.Equals(
                    candidate.mutation.CreatedFilePath,
                    createdFilePath,
                    StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (matchingIndexes.Length != 1)
            {
                throw new InvalidOperationException("A mutation completion must identify exactly one owned Desktop.ini record.");
            }

            var (existing, index) = matchingIndexes[0];
            var completed = existing with
            {
                AddedFolderAttributes = addedFolderAttributes,
                IsCompleted = true,
                Phase = FolderMutationPhase.Completed
            };
            completed.Validate();
            if (existing == completed)
            {
                return;
            }

            if (existing.IsCompleted)
            {
                throw new InvalidOperationException("A completed mutation cannot be completed again with different attribute ownership.");
            }

            var mutations = snapshot.Mutations.ToArray();
            mutations[index] = completed;
            var updated = snapshot with { Mutations = mutations };
            await WriteJsonAtomicallyAsync(GetSnapshotPath(operationId), new SnapshotDocument(CurrentSchemaVersion, updated), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public Task PersistFolderAttributeIntentAsync(
        Guid operationId,
        string createdFilePath,
        FileAttributes addedFolderAttributes,
        CancellationToken cancellationToken = default) =>
        UpdateMutationPhaseAsync(
            operationId,
            createdFilePath,
            FolderMutationPhase.FolderAttributeIntentPersisted,
            addedFolderAttributes,
            cancellationToken);

    public Task ResetMutationToFileCreatedAsync(
        Guid operationId,
        string createdFilePath,
        CancellationToken cancellationToken = default) =>
        UpdateMutationPhaseAsync(
            operationId,
            createdFilePath,
            FolderMutationPhase.FileCreated,
            0,
            cancellationToken);

    private async Task UpdateMutationPhaseAsync(
        Guid operationId,
        string createdFilePath,
        FolderMutationPhase phase,
        FileAttributes addedFolderAttributes,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(createdFilePath);
        if (phase == FolderMutationPhase.Completed)
        {
            throw new ArgumentException("Use CompleteMutationAsync for completed mutations.", nameof(phase));
        }

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var snapshot = await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
            var matches = snapshot.Mutations
                .Select((mutation, index) => (mutation, index))
                .Where(candidate => string.Equals(candidate.mutation.CreatedFilePath, createdFilePath, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (matches.Length != 1)
            {
                throw new InvalidOperationException("A mutation phase update must identify exactly one owned Desktop.ini record.");
            }

            var (existing, index) = matches[0];
            if (existing.IsCompleted)
            {
                throw new InvalidOperationException("A completed mutation cannot return to an incomplete phase.");
            }

            var updatedMutation = existing with
            {
                AddedFolderAttributes = addedFolderAttributes,
                IsCompleted = false,
                Phase = phase
            };
            updatedMutation.Validate();
            if (updatedMutation == existing) return;

            var mutations = snapshot.Mutations.ToArray();
            mutations[index] = updatedMutation;
            var updated = snapshot with { Mutations = mutations };
            await WriteJsonAtomicallyAsync(GetSnapshotPath(operationId), new SnapshotDocument(CurrentSchemaVersion, updated), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<OperationSnapshot?> LoadLatestAsync(CancellationToken cancellationToken = default)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (!fileSystem.FileExists(GetLatestPath()))
            {
                return null;
            }

            var latest = await DeserializeAsync<LatestDocument>(GetLatestPath(), cancellationToken).ConfigureAwait(false);
            EnsureVersion(latest.Version);
            return await LoadByIdAsync(latest.OperationId, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<OperationSnapshot> LoadAsync(Guid operationId, CancellationToken cancellationToken = default)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return await LoadByIdAsync(operationId, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<RestoreLease> AcquireRestoreLeaseAsync(
        Guid operationId,
        CancellationToken cancellationToken = default)
    {
        if (operationId == Guid.Empty)
        {
            throw new ArgumentException("An operation identifier is required.", nameof(operationId));
        }

        var restoreGate = RestoreGates.GetOrAdd(GetRestoreGateKey(operationId), _ => new SemaphoreSlim(1, 1));
        await restoreGate.WaitAsync(cancellationToken).ConfigureAwait(false);
        IAsyncDisposable? processLease = null;
        try
        {
            fileSystem.CreateDirectory(GetOperationDirectory(operationId));
            processLease = await restoreProcessLock
                .AcquireAsync(GetRestoreLockPath(operationId), cancellationToken)
                .ConfigureAwait(false);
            var restorePath = GetRestorePath(operationId);
            var alreadyRestored = false;
            if (fileSystem.FileExists(restorePath))
            {
                var document = await DeserializeAsync<RestoreDocument>(restorePath, cancellationToken).ConfigureAwait(false);
                EnsureVersion(document.Version);
                if (document.OperationId != operationId)
                {
                    throw new InvalidDataException("The restore marker does not match the requested operation.");
                }

                alreadyRestored = true;
            }

            return new RestoreLease(this, operationId, restoreGate, processLease, alreadyRestored);
        }
        catch
        {
            if (processLease is not null) await processLease.DisposeAsync().ConfigureAwait(false);
            restoreGate.Release();
            throw;
        }
    }

    private async Task MarkRestoreCompletedAsync(
        Guid operationId,
        DateTimeOffset completedAtUtc,
        CancellationToken cancellationToken)
    {
        if (completedAtUtc == default || completedAtUtc.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("A restore completion requires a UTC timestamp.", nameof(completedAtUtc));
        }

        await WriteJsonAtomicallyAsync(
            GetRestorePath(operationId),
            new RestoreDocument(CurrentSchemaVersion, operationId, completedAtUtc),
            cancellationToken).ConfigureAwait(false);
    }

    private async Task<OperationSnapshot> LoadByIdAsync(Guid operationId, CancellationToken cancellationToken)
    {
        if (operationId == Guid.Empty)
        {
            throw new ArgumentException("An operation identifier is required.", nameof(operationId));
        }

        var document = await DeserializeAsync<SnapshotDocument>(GetSnapshotPath(operationId), cancellationToken).ConfigureAwait(false);
        EnsureVersion(document.Version);
        try
        {
            document.Snapshot.Validate();
            return document.Snapshot;
        }
        catch (Exception exception) when (exception is ArgumentException or NullReferenceException)
        {
            throw new InvalidDataException("Backup snapshot contains invalid required values.", exception);
        }
    }

    private async Task<T> DeserializeAsync<T>(string path, CancellationToken cancellationToken)
    {
        var contents = await fileSystem.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
        try
        {
            using var document = JsonDocument.Parse(contents);
            ValidateDocument<T>(document.RootElement);
            return JsonSerializer.Deserialize<T>(contents, JsonOptions)
                ?? throw new InvalidDataException($"Backup data at '{path}' is empty or malformed.");
        }
        catch (Exception exception) when (exception is JsonException or FormatException or ArgumentException or InvalidOperationException)
        {
            throw new InvalidDataException($"Backup data at '{path}' is empty or malformed.", exception);
        }
    }

    private static void ValidateDocument<T>(JsonElement root)
    {
        RequireObject(root, "document");
        var version = ReadInt32(root, "Version");

        if (typeof(T) == typeof(LatestDocument))
        {
            var operationId = RequireString(root, "OperationId");
            if (!Guid.TryParse(operationId, out var parsedOperationId) || parsedOperationId == Guid.Empty)
            {
                throw new InvalidDataException("The latest backup pointer has an invalid operation identifier.");
            }

            return;
        }

        if (typeof(T) == typeof(RestoreDocument))
        {
            var operationId = RequireString(root, "OperationId");
            if (!Guid.TryParse(operationId, out var parsedOperationId) || parsedOperationId == Guid.Empty)
            {
                throw new InvalidDataException("The restore marker has an invalid operation identifier.");
            }

            var completedAt = RequireString(root, "CompletedAtUtc");
            if (!DateTimeOffset.TryParse(
                    completedAt,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind,
                    out var parsedCompletedAt)
                || parsedCompletedAt == default
                || parsedCompletedAt.Offset != TimeSpan.Zero)
            {
                throw new InvalidDataException("The restore marker requires a valid UTC completion timestamp.");
            }

            return;
        }

        if (typeof(T) != typeof(SnapshotDocument))
        {
            throw new InvalidOperationException($"Unsupported backup document type '{typeof(T).Name}'.");
        }

        var snapshot = RequireObjectProperty(root, "Snapshot");
        var snapshotId = RequireString(snapshot, "Id");
        if (!Guid.TryParse(snapshotId, out var parsedSnapshotId) || parsedSnapshotId == Guid.Empty)
        {
            throw new InvalidDataException("A snapshot requires a non-empty identifier.");
        }

        var createdAt = RequireString(snapshot, "CreatedAtUtc");
        if (!DateTimeOffset.TryParse(createdAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out _))
        {
            throw new InvalidDataException("A snapshot requires a valid creation timestamp.");
        }

        ValidateRegistryValue(RequireObjectProperty(snapshot, "RegistryValue3"), version);
        ValidateRegistryValue(RequireObjectProperty(snapshot, "RegistryValue4"), version);
        var mutations = RequireArrayProperty(snapshot, "Mutations");
        foreach (var mutation in mutations.EnumerateArray())
        {
            ValidateMutation(mutation, version);
        }

        RequireBoolean(snapshot, "IsCompleted");
    }

    private static void ValidateRegistryValue(JsonElement registryValue, int version)
    {
        RequireString(registryValue, "Name");
        RequireBoolean(registryValue, "Existed");
        var value = RequireProperty(registryValue, "Value");
        if (value.ValueKind is not (JsonValueKind.String or JsonValueKind.Null))
        {
            throw new InvalidDataException("A registry value must be a string or null.");
        }


        if (version >= 2)
        {
            var kind = RequireProperty(registryValue, "Kind");
            if (kind.ValueKind is not (JsonValueKind.Number or JsonValueKind.Null)
                || (kind.ValueKind == JsonValueKind.Number && !kind.TryGetInt32(out _)))
            {
                throw new InvalidDataException("A registry value kind must be a 32-bit number or null.");
            }
        }
    }

    private static void ValidateMutation(JsonElement mutation, int version)
    {
        RequireObject(mutation, "mutation");
        RequireString(mutation, "FolderPath");
        RequireInt32(mutation, "OriginalFolderAttributes");
        RequireString(mutation, "CreatedFilePath");
        RequireString(mutation, "ExpectedDesktopIniSha256");
        RequireInt32(mutation, "AddedFolderAttributes");
        RequireBoolean(mutation, "IsCompleted");
        if (version >= 2) RequireInt32(mutation, "Phase");
        if (version >= 3)
        {
            RequireBoolean(mutation, "FileOriginallyExisted");
            var original = RequireProperty(mutation, "OriginalDesktopIniBase64");
            if (original.ValueKind is not (JsonValueKind.String or JsonValueKind.Null))
                throw new InvalidDataException("Original Desktop.ini bytes must be a string or null.");
            var attributes = RequireProperty(mutation, "OriginalDesktopIniAttributes");
            if (attributes.ValueKind is not (JsonValueKind.Number or JsonValueKind.Null)
                || (attributes.ValueKind == JsonValueKind.Number && !attributes.TryGetInt32(out _)))
                throw new InvalidDataException("Original Desktop.ini attributes must be a number or null.");
        }
    }

    private static JsonElement RequireObjectProperty(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        RequireObject(property, name);
        return property;
    }

    private static JsonElement RequireArrayProperty(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidDataException($"Backup field '{name}' must be an array.");
        }

        return property;
    }

    private static string RequireString(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind != JsonValueKind.String || property.GetString() is not { } value)
        {
            throw new InvalidDataException($"Backup field '{name}' must be a string.");
        }

        return value;
    }

    private static void RequireInt32(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind != JsonValueKind.Number || !property.TryGetInt32(out _))
        {
            throw new InvalidDataException($"Backup field '{name}' must be a 32-bit number.");
        }
    }

    private static int ReadInt32(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind != JsonValueKind.Number || !property.TryGetInt32(out var value))
        {
            throw new InvalidDataException($"Backup field '{name}' must be a 32-bit number.");
        }

        return value;
    }

    private static void RequireBoolean(JsonElement parent, string name)
    {
        var property = RequireProperty(parent, name);
        if (property.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
        {
            throw new InvalidDataException($"Backup field '{name}' must be a boolean.");
        }
    }

    private static JsonElement RequireProperty(JsonElement parent, string name)
    {
        if (!parent.TryGetProperty(name, out var property))
        {
            throw new InvalidDataException($"Backup data is missing required field '{name}'.");
        }

        return property;
    }

    private static void RequireObject(JsonElement element, string name)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException($"Backup {name} must be an object.");
        }
    }

    private async Task WriteJsonAtomicallyAsync<T>(string destinationPath, T document, CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(destinationPath)
            ?? throw new InvalidOperationException("Backup paths must have a parent directory.");
        fileSystem.CreateDirectory(directory);
        var temporaryPath = Path.Combine(directory, $".{Path.GetFileName(destinationPath)}.{Guid.NewGuid():N}.tmp");

        try
        {
            await fileSystem.WriteAllTextAsync(temporaryPath, JsonSerializer.Serialize(document, JsonOptions), cancellationToken).ConfigureAwait(false);
            if (fileSystem.FileExists(destinationPath))
            {
                fileSystem.ReplaceFile(temporaryPath, destinationPath);
            }
            else
            {
                fileSystem.MoveFile(temporaryPath, destinationPath);
            }
        }
        finally
        {
            fileSystem.DeleteFile(temporaryPath);
        }
    }

    private string GetOperationDirectory(Guid operationId) => Path.Combine(backupsRoot, operationId.ToString("D"));
    private string GetSnapshotPath(Guid operationId) => Path.Combine(GetOperationDirectory(operationId), SnapshotFileName);
    private string GetRestorePath(Guid operationId) => Path.Combine(GetOperationDirectory(operationId), RestoreFileName);
    private string GetRestoreLockPath(Guid operationId) => Path.Combine(GetOperationDirectory(operationId), "restore.lock");
    private string GetLatestPath() => Path.Combine(backupsRoot, LatestFileName);
    private string GetRestoreGateKey(Guid operationId) =>
        $"{Path.TrimEndingDirectorySeparator(Path.GetFullPath(backupsRoot))}|{operationId:D}";

    private static void EnsureVersion(int version)
    {
        if (version is < 1 or > CurrentSchemaVersion)
        {
            throw new InvalidDataException($"Unsupported backup schema version '{version}'.");
        }
    }

    private sealed record SnapshotDocument(int Version, OperationSnapshot Snapshot);
    private sealed record LatestDocument(int Version, Guid OperationId);
    private sealed record RestoreDocument(int Version, Guid OperationId, DateTimeOffset CompletedAtUtc);

    public sealed class RestoreLease : IAsyncDisposable
    {
        private readonly BackupService owner;
        private readonly SemaphoreSlim restoreGate;
        private readonly IAsyncDisposable processLease;
        private int disposed;

        internal RestoreLease(
            BackupService owner,
            Guid operationId,
            SemaphoreSlim restoreGate,
            IAsyncDisposable processLease,
            bool alreadyRestored)
        {
            this.owner = owner;
            OperationId = operationId;
            this.restoreGate = restoreGate;
            this.processLease = processLease;
            AlreadyRestored = alreadyRestored;
        }

        public Guid OperationId { get; }
        public bool AlreadyRestored { get; private set; }

        public async Task MarkCompletedAsync(
            DateTimeOffset completedAtUtc,
            CancellationToken cancellationToken = default)
        {
            ObjectDisposedException.ThrowIf(disposed != 0, this);
            if (AlreadyRestored)
            {
                return;
            }

            await owner.MarkRestoreCompletedAsync(OperationId, completedAtUtc, cancellationToken).ConfigureAwait(false);
            AlreadyRestored = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref disposed, 1) == 0)
            {
                try { await processLease.DisposeAsync().ConfigureAwait(false); }
                finally { restoreGate.Release(); }
            }
        }
    }
}

internal sealed class WindowsFileSystem : IFileSystem
{
    public void CreateDirectory(string path) => Directory.CreateDirectory(path);
    public bool FileExists(string path) => File.Exists(path);
    public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken) => File.ReadAllTextAsync(path, cancellationToken);

    public async Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous | FileOptions.WriteThrough);
        await using var writer = new StreamWriter(stream, new UTF8Encoding(false), 4096, leaveOpen: true);
        await writer.WriteAsync(contents.AsMemory(), cancellationToken).ConfigureAwait(false);
        await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    public void MoveFile(string sourcePath, string destinationPath) => File.Move(sourcePath, destinationPath);
    public void ReplaceFile(string sourcePath, string destinationPath) => File.Replace(sourcePath, destinationPath, destinationBackupFileName: null);
    public void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
```

## src/FolderThemeStudio.Core/Rendering/FolderIconRenderer.cs

```csharp
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Rendering;

public static class FolderIconRenderer
{
    public static readonly int[] RequiredSizes = [16, 20, 24, 32, 48, 64, 128, 256];

    public static BitmapSource Render(FolderTheme theme, int size)
    {
        ThemeValidator.ThrowIfInvalid(theme);
        if (!RequiredSizes.Contains(size))
        {
            throw new ArgumentOutOfRangeException(nameof(size));
        }

        var target = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Pbgra32);
        target.Render(DrawFolder(theme, size, simplify: size <= 32));
        target.Freeze();
        return target;
    }

    private static DrawingVisual DrawFolder(FolderTheme theme, int size, bool simplify)
    {
        if (theme.Material == MaterialStyle.Satin) return DrawSatinFolder(theme, size, simplify);
        var visual = new DrawingVisual();
        using var context = visual.RenderOpen();
        context.PushTransform(new ScaleTransform(size, size));
        var inset = 1d / size;
        context.PushClip(new RectangleGeometry(new Rect(inset, inset, 1d - (2d * inset), 1d - (2d * inset))));

        var radius = Math.Min(theme.CornerRadius / size, 0.12);
        var outlineWidth = simplify ? 1d / size : Math.Max(theme.StrokeWidth / size, 1d / size);
        var body = new Rect(0.09, 0.30, 0.82, 0.55);
        var tab = new Rect(0.15, 0.18, 0.36, 0.24);
        var fill = CreateGradient(theme);
        var outline = new Pen(CreateBrush(theme.StrokeColor, theme.StrokeOpacity), outlineWidth);

        if (!simplify && theme.GlowRadius > 0 && theme.GlowStrength > 0)
        {
            var glowWidth = Math.Max(outlineWidth * 2, theme.GlowRadius / size);
            context.DrawRoundedRectangle(null, new Pen(CreateBrush(theme.GlowColor, theme.GlowStrength * 0.3), glowWidth), body, radius, radius);
        }

        context.DrawRoundedRectangle(CreateGradient(theme, 0.9), outline, tab, radius, radius);

        if (theme.ShadowStrength > 0 && theme.ShadowOffset > 0)
        {
            var shadow = body;
            shadow.Offset(0, theme.ShadowOffset / size);
            context.DrawRoundedRectangle(CreateBrush("#000000", theme.ShadowStrength * 0.5), null, shadow, radius, radius);
        }

        context.DrawRoundedRectangle(fill, outline, body, radius, radius);

        var separatorY = body.Top + 0.09;
        context.DrawLine(new Pen(CreateBrush(theme.StrokeColor, theme.StrokeOpacity * 0.7), outlineWidth),
            new Point(body.Left + 0.035, separatorY), new Point(body.Right - 0.035, separatorY));

        var highlightWidth = Math.Max(theme.HighlightStrength * 0.035, 0);
        if (!simplify || highlightWidth * size >= 1)
        {
            var highlight = new StreamGeometry();
            using (var geometryContext = highlight.Open())
            {
                geometryContext.BeginFigure(new Point(body.Left + 0.10, body.Top + 0.12), true, true);
                geometryContext.LineTo(new Point(body.Right - 0.18, body.Top + 0.12), true, false);
                geometryContext.LineTo(new Point(body.Right - 0.33, body.Bottom - 0.10), true, false);
                geometryContext.LineTo(new Point(body.Left + 0.02, body.Bottom - 0.10), true, false);
            }

            highlight.Freeze();
            context.DrawGeometry(CreateBrush("#FFFFFF", theme.HighlightStrength * 0.28), null, highlight);
        }

        MaterialPainter.Draw(context, theme, body, radius, size, simplify);
        StickerPainter.Draw(context, theme, body, size, simplify);

        context.Pop();
        context.Pop();
        return visual;
    }

    private static DrawingVisual DrawSatinFolder(FolderTheme theme, int size, bool simplify)
    {
        var visual = new DrawingVisual();
        using var context = visual.RenderOpen();
        context.PushTransform(new ScaleTransform(size, size));
        var inset = 1d / size;
        context.PushClip(new RectangleGeometry(new Rect(inset, inset, 1 - 2 * inset, 1 - 2 * inset)));
        var radius = Math.Clamp(theme.CornerRadius / 256, .025, .09);
        var back = Geometry.Parse("M .10,.29 L .10,.245 Q .10,.19 .155,.19 L .32,.19 Q .35,.19 .375,.22 L .415,.26 L .845,.26 Q .90,.26 .90,.315 L .90,.78 Q .90,.83 .85,.83 L .15,.83 Q .10,.83 .10,.78 Z");
        back.Freeze();
        context.DrawGeometry(CreateGradient(theme, .82), null, back);

        var face = new StreamGeometry();
        using (var path = face.Open())
        {
            path.BeginFigure(new Point(.09 + radius, .34), true, true);
            path.LineTo(new Point(.91 - radius, .34), true, false);
            path.QuadraticBezierTo(new Point(.925, .34), new Point(.915, .34 + radius), true, false);
            path.LineTo(new Point(.885, .87 - radius), true, false);
            path.QuadraticBezierTo(new Point(.88, .87), new Point(.88 - radius, .87), true, false);
            path.LineTo(new Point(.12 + radius, .87), true, false);
            path.QuadraticBezierTo(new Point(.12, .87), new Point(.115, .87 - radius), true, false);
            path.LineTo(new Point(.085, .34 + radius), true, false);
            path.QuadraticBezierTo(new Point(.075, .34), new Point(.09 + radius, .34), true, false);
        }
        face.Freeze();
        if (theme.ShadowStrength > 0 && theme.ShadowOffset > 0)
        {
            for (var layer = simplify ? 1 : 3; layer > 0; layer--)
            {
                context.PushTransform(new TranslateTransform(0, theme.ShadowOffset / 256 + layer * .003));
                context.DrawGeometry(CreateBrush("#17172A", theme.ShadowStrength * .13), null, face);
                context.Pop();
            }
        }
        if (!simplify && theme.GlowStrength > 0 && theme.GlowRadius > 0)
            context.DrawGeometry(null, new Pen(CreateBrush(theme.GlowColor, theme.GlowStrength * .18), theme.GlowRadius / size), face);
        context.DrawGeometry(CreateGradient(theme), new Pen(CreateBrush(theme.StrokeColor, theme.StrokeOpacity * .5),
            simplify ? 1d / size : Math.Max(.5 / size, theme.StrokeWidth / size)), face);
        context.PushClip(face);
        var sheen = new LinearGradientBrush(
            CreateColor("#FFFFFF", theme.HighlightStrength * .65), CreateColor("#FFFFFF", 0), new Point(0, 0), new Point(0, 1));
        context.DrawRectangle(sheen, null, new Rect(.075, .34, .85, .36));
        if (!simplify)
        {
            context.DrawLine(new Pen(CreateBrush("#FFFFFF", theme.HighlightStrength * .75), 1d / size),
                new Point(.15, .351), new Point(.85, .351));
        }
        context.Pop();
        StickerPainter.Draw(context, theme, new Rect(.09, .30, .82, .55), size, simplify);
        context.Pop();
        context.Pop();
        return visual;
    }

    private static LinearGradientBrush CreateGradient(FolderTheme theme, double opacityMultiplier = 1)
    {
        var radians = theme.GradientAngle * Math.PI / 180;
        var x = Math.Cos(radians) * 0.5;
        var y = Math.Sin(radians) * 0.5;
        var brush = new LinearGradientBrush(
            CreateColor(theme.GradientStart, theme.Opacity * opacityMultiplier),
            CreateColor(theme.GradientEnd, theme.Opacity * opacityMultiplier),
            new Point(0.5 - x, 0.5 - y),
            new Point(0.5 + x, 0.5 + y));
        brush.Freeze();
        return brush;
    }

    private static SolidColorBrush CreateBrush(string color, double opacity)
    {
        var brush = new SolidColorBrush(CreateColor(color, opacity));
        brush.Freeze();
        return brush;
    }

    private static Color CreateColor(string value, double opacity)
    {
        var color = (Color)ColorConverter.ConvertFromString(value)!;
        color.A = (byte)Math.Round(Math.Clamp(opacity, 0, 1) * byte.MaxValue);
        return color;
    }
}
```

## src/FolderThemeStudio.Core/Rendering/IcoEncoder.cs

```csharp
using System.IO;
using System.Windows.Media.Imaging;

namespace FolderThemeStudio.Core.Rendering;

public static class IcoEncoder
{
    private static readonly byte[] PngSignature = [137, 80, 78, 71, 13, 10, 26, 10];

    public static byte[] Encode(IReadOnlyDictionary<int, byte[]> pngFrames)
    {
        ArgumentNullException.ThrowIfNull(pngFrames);
        ValidateFrameSet(pngFrames);

        var orderedFrames = FolderIconRenderer.RequiredSizes
            .Select(size => new KeyValuePair<int, byte[]>(size, pngFrames[size]))
            .ToArray();
        var payloadOffset = checked(6 + (orderedFrames.Length * 16));

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
        writer.Write((ushort)0);
        writer.Write((ushort)1);
        writer.Write((ushort)orderedFrames.Length);

        foreach (var frame in orderedFrames)
        {
            writer.Write((byte)(frame.Key == 256 ? 0 : frame.Key));
            writer.Write((byte)(frame.Key == 256 ? 0 : frame.Key));
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((ushort)1);
            writer.Write((ushort)32);
            writer.Write(frame.Value.Length);
            writer.Write(payloadOffset);
            payloadOffset = checked(payloadOffset + frame.Value.Length);
        }

        foreach (var frame in orderedFrames)
        {
            writer.Write(frame.Value);
        }

        writer.Flush();
        return stream.ToArray();
    }

    private static void ValidateFrameSet(IReadOnlyDictionary<int, byte[]> pngFrames)
    {
        if (pngFrames.Count != FolderIconRenderer.RequiredSizes.Length ||
            pngFrames.Keys.Any(size => !FolderIconRenderer.RequiredSizes.Contains(size)))
        {
            throw new ArgumentException("Frames must contain exactly one PNG for every required size.", nameof(pngFrames));
        }

        foreach (var size in FolderIconRenderer.RequiredSizes)
        {
            if (!pngFrames.TryGetValue(size, out var frame))
            {
                throw new ArgumentException("Frames must contain exactly one PNG for every required size.", nameof(pngFrames));
            }

            ValidatePngFrame(size, frame);
        }
    }

    private static void ValidatePngFrame(int size, byte[] frame)
    {
        if (frame is null || frame.Length < PngSignature.Length || !frame.AsSpan(0, PngSignature.Length).SequenceEqual(PngSignature))
        {
            throw new ArgumentException("Every frame must be a valid PNG payload.", nameof(frame));
        }

        try
        {
            using var stream = new MemoryStream(frame, writable: false);
            var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            if (decoder.Frames.Count != 1 || decoder.Frames[0].PixelWidth != size || decoder.Frames[0].PixelHeight != size)
            {
                throw new ArgumentException("PNG dimensions must match the frame size.", nameof(frame));
            }
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new ArgumentException("Every frame must be a valid PNG payload.", nameof(frame), exception);
        }
    }
}
```

## src/FolderThemeStudio.Core/Rendering/ImportedIconRenderer.cs

```csharp
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FolderThemeStudio.Core.Rendering;

public static class ImportedIconRenderer
{
    public static BitmapSource Render(string normalizedPngPath, int size)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(normalizedPngPath);
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

        var source = Load(normalizedPngPath, size);
        var scale = Math.Min(size / (double)source.PixelWidth, size / (double)source.PixelHeight);
        var width = source.PixelWidth * scale;
        var height = source.PixelHeight * scale;
        var x = (size - width) / 2;
        var y = (size - height) / 2;

        var visual = new DrawingVisual();
        RenderOptions.SetBitmapScalingMode(visual, BitmapScalingMode.HighQuality);
        using (var context = visual.RenderOpen())
        {
            context.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, size, size));
            context.DrawImage(source, new Rect(x, y, width, height));
        }

        var target = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Pbgra32);
        target.Render(visual);
        target.Freeze();
        return target;
    }

    private static BitmapSource Load(string path, int targetSize)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);
        var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
        var source = decoder.Frames
            .Where(frame => frame.PixelWidth >= targetSize && frame.PixelHeight >= targetSize)
            .OrderBy(frame => checked((long)frame.PixelWidth * frame.PixelHeight))
            .ThenByDescending(frame => frame.Format.BitsPerPixel)
            .FirstOrDefault()
            ?? decoder.Frames
                .OrderByDescending(frame => checked((long)frame.PixelWidth * frame.PixelHeight))
                .ThenByDescending(frame => frame.Format.BitsPerPixel)
                .First();
        source.Freeze();
        return source;
    }
}
```

## src/FolderThemeStudio.Core/Rendering/ImportedImageService.cs

```csharp
using System.IO;
using System.Security.Cryptography;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FolderThemeStudio.Core.Rendering;

public sealed record ImportedImageResult(
    bool Success,
    string? AssetPath,
    BitmapSource? Preview,
    string? Error)
{
    public static ImportedImageResult Succeeded(string assetPath, BitmapSource preview) =>
        new(true, assetPath, preview, null);

    public static ImportedImageResult Failure(string error) =>
        new(false, null, null, error);
}

public interface IImportedImageService
{
    Task<ImportedImageResult> ImportAsync(string sourcePath, CancellationToken token);
}

public sealed class ImportedImageService : IImportedImageService
{
    private static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
    private static readonly byte[] IcoSignature = [0x00, 0x00, 0x01, 0x00];
    private readonly string importsRoot;

    public ImportedImageService()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio",
            "imports"))
    {
    }

    public ImportedImageService(string importsRoot)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(importsRoot);
        this.importsRoot = Path.GetFullPath(importsRoot);
    }

    public async Task<ImportedImageResult> ImportAsync(string sourcePath, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            return ImportedImageResult.Failure("An image path is required.");
        }

        try
        {
            token.ThrowIfCancellationRequested();
            var fullSourcePath = Path.GetFullPath(sourcePath);
            var bytes = await File.ReadAllBytesAsync(fullSourcePath, token).ConfigureAwait(false);
            var detected = DetectFormat(bytes);
            var expected = FormatForExtension(Path.GetExtension(fullSourcePath));
            if (detected is null || expected is null || detected != expected)
            {
                return ImportedImageResult.Failure("Choose a valid PNG, JPEG, BMP, or ICO image whose extension matches its contents.");
            }

            var normalized = Decode(bytes, detected);
            var pngBytes = EncodePng(normalized);
            var hash = Convert.ToHexString(SHA256.HashData(pngBytes)).ToLowerInvariant();
            Directory.CreateDirectory(importsRoot);
            var destination = Path.Combine(importsRoot, hash + ".png");
            if (!File.Exists(destination))
            {
                var temporary = Path.Combine(importsRoot, $".{hash}.{Guid.NewGuid():N}.tmp");
                try
                {
                    await File.WriteAllBytesAsync(temporary, pngBytes, token).ConfigureAwait(false);
                    try
                    {
                        File.Move(temporary, destination);
                    }
                    catch (IOException) when (File.Exists(destination))
                    {
                        // Another import persisted the identical content first.
                    }
                }
                finally
                {
                    if (File.Exists(temporary)) File.Delete(temporary);
                }
            }

            var preview = Decode(
                await File.ReadAllBytesAsync(destination, token).ConfigureAwait(false),
                "png");
            return ImportedImageResult.Succeeded(destination, preview);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception) when (exception is IOException
            or UnauthorizedAccessException
            or System.Security.SecurityException
            or NotSupportedException
            or ArgumentException
            or InvalidOperationException
            or FileFormatException)
        {
            return ImportedImageResult.Failure(exception.Message);
        }
    }

    private static string? DetectFormat(ReadOnlySpan<byte> bytes)
    {
        if (bytes.StartsWith(PngSignature)) return "png";
        if (bytes.StartsWith(IcoSignature)) return "ico";
        if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF) return "jpeg";
        if (bytes.Length >= 2 && bytes[0] == 0x42 && bytes[1] == 0x4D) return "bmp";
        return null;
    }

    private static string? FormatForExtension(string extension) => extension.ToLowerInvariant() switch
    {
        ".png" => "png",
        ".jpg" or ".jpeg" => "jpeg",
        ".bmp" => "bmp",
        ".ico" => "ico",
        _ => null
    };

    private static BitmapSource Decode(byte[] bytes, string format)
    {
        using var stream = new MemoryStream(bytes, writable: false);
        var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
        if (decoder.Frames.Count == 0) throw new FileFormatException("The image has no decodable frame.");
        var source = format == "ico"
            ? decoder.Frames
                .OrderByDescending(frame => checked((long)frame.PixelWidth * frame.PixelHeight))
                .ThenByDescending(frame => frame.Format.BitsPerPixel)
                .First()
            : decoder.Frames[0];
        var converted = new FormatConvertedBitmap(source, PixelFormats.Pbgra32, null, 0);
        converted.Freeze();
        return converted;
    }

    private static byte[] EncodePng(BitmapSource source)
    {
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(source));
        using var stream = new MemoryStream();
        encoder.Save(stream);
        return stream.ToArray();
    }
}
```

## src/FolderThemeStudio.Core/Rendering/MaterialPainter.cs

```csharp
using System.Windows;
using System.Windows.Media;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Rendering;

internal static class MaterialPainter
{
    public static void Draw(DrawingContext context, FolderTheme theme, Rect face, double radius, int size, bool simplify)
    {
        if (theme.Material == MaterialStyle.Glass) return;

        switch (theme.Material)
        {
            case MaterialStyle.Clay:
                context.DrawRoundedRectangle(Brush("#FFF4E8", 0.36), null, face, radius, radius);
                var inner = new Rect(face.Left + .018, face.Top + .025, face.Width - .036, face.Height - .05);
                context.DrawRoundedRectangle(null, new Pen(Brush("#FFFFFF", .48), 1d / size), inner, radius, radius);
                context.DrawLine(new Pen(Brush("#3F3451", .24), Math.Max(1d / size, .012)),
                    new Point(face.Left + .08, face.Bottom - .035), new Point(face.Right - .08, face.Bottom - .035));
                break;
            case MaterialStyle.FrostedGlass:
                context.DrawRoundedRectangle(Brush("#FFFFFF", .32), null, face, radius, radius);
                if (!simplify)
                {
                    context.DrawRoundedRectangle(null, new Pen(Brush("#FFFFFF", .55), 1d / size),
                        new Rect(face.Left + .015, face.Top + .015, face.Width - .03, face.Height - .03), radius, radius);
                    context.DrawLine(new Pen(Brush("#FFFFFF", .18), .045),
                        new Point(face.Left + .10, face.Top + .16), new Point(face.Right - .22, face.Bottom - .14));
                }
                break;
            case MaterialStyle.Paper:
                context.DrawRoundedRectangle(Brush("#FFE3AC", .48), null, face, radius, radius);
                if (!simplify)
                {
                    for (var y = face.Top + .17; y < face.Bottom - .04; y += .09)
                    {
                        context.DrawLine(new Pen(Brush("#745D54", .19), 1d / size),
                            new Point(face.Left + .06, y), new Point(face.Right - .06, y));
                    }
                }
                break;
        }
    }

    private static SolidColorBrush Brush(string hex, double opacity)
    {
        var color = (Color)ColorConverter.ConvertFromString(hex)!;
        color.A = (byte)Math.Round(opacity * byte.MaxValue);
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        return brush;
    }
}
```

## src/FolderThemeStudio.Core/Rendering/StickerPainter.cs

```csharp
using System.Windows;
using System.Windows.Media;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.Core.Rendering;

public static class StickerPainter
{
    public static Point ClampPosition(Point relative, double scale, Rect face)
    {
        var diameter = scale * Math.Min(face.Width, face.Height);
        var padX = diameter / (2 * face.Width) + .02;
        var padY = diameter / (2 * face.Height) + .02;
        return new Point(
            Math.Clamp(relative.X, Math.Max(StickerPlacement.MinimumCenter, padX), Math.Min(StickerPlacement.MaximumCenter, 1 - padX)),
            Math.Clamp(relative.Y, Math.Max(StickerPlacement.MinimumCenter, padY), Math.Min(StickerPlacement.MaximumCenter, 1 - padY)));
    }

    public static void Draw(DrawingContext context, FolderTheme theme, Rect face, int size, bool simplify)
    {
        if (theme.Sticker == StickerStyle.None) return;

        var relative = ClampPosition(new Point(theme.StickerX, theme.StickerY), theme.StickerScale, face);
        var diameter = theme.StickerScale * Math.Min(face.Width, face.Height);
        var center = new Point(face.Left + face.Width * relative.X, face.Top + face.Height * relative.Y);
        var color = (Color)ColorConverter.ConvertFromString(theme.StickerColor)!;
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        var border = simplify ? null : new Pen(Brush("#34405C", .36), Math.Max(.01, 1d / size / diameter));

        context.PushClip(new RectangleGeometry(new Rect(face.Left + .01, face.Top + .01, face.Width - .02, face.Height - .02)));
        context.PushTransform(new TranslateTransform(center.X, center.Y));
        context.PushTransform(new RotateTransform(theme.StickerRotation));
        context.PushTransform(new ScaleTransform(diameter, diameter));

        switch (theme.Sticker)
        {
            case StickerStyle.Star:
                context.DrawGeometry(brush, border, Star());
                break;
            case StickerStyle.Flower:
                for (var petal = 0; petal < 5; petal++)
                {
                    var angle = petal * Math.PI * 2 / 5;
                    context.DrawEllipse(brush, border, new Point(Math.Sin(angle) * .28, -Math.Cos(angle) * .28), .2, .2);
                }
                context.DrawEllipse(Brush("#FFD568", 1), null, new Point(0, 0), .16, .16);
                break;
            case StickerStyle.Heart:
                context.DrawGeometry(brush, border, Heart());
                break;
            case StickerStyle.Paw:
                context.DrawEllipse(brush, border, new Point(0, .18), .32, .25);
                if (!simplify)
                {
                    foreach (var toe in new[] { new Point(-.32, -.20), new Point(-.10, -.34), new Point(.13, -.34), new Point(.34, -.20) })
                        context.DrawEllipse(brush, border, toe, .115, .145);
                }
                break;
        }

        context.Pop();
        context.Pop();
        context.Pop();
        context.Pop();
    }

    private static StreamGeometry Star()
    {
        var geometry = new StreamGeometry();
        using var writer = geometry.Open();
        var points = Enumerable.Range(0, 10).Select(index =>
        {
            var angle = -Math.PI / 2 + index * Math.PI / 5;
            var radius = index % 2 == 0 ? .48 : .22;
            return new Point(Math.Cos(angle) * radius, Math.Sin(angle) * radius);
        }).ToArray();
        writer.BeginFigure(points[0], true, true);
        writer.PolyLineTo(points.Skip(1).ToArray(), true, false);
        geometry.Freeze();
        return geometry;
    }

    private static StreamGeometry Heart()
    {
        var geometry = new StreamGeometry();
        using var writer = geometry.Open();
        writer.BeginFigure(new Point(0, .46), true, true);
        writer.BezierTo(new Point(-.55, .04), new Point(-.60, -.28), new Point(-.26, -.39), true, false);
        writer.BezierTo(new Point(-.08, -.46), new Point(0, -.31), new Point(0, -.22), true, false);
        writer.BezierTo(new Point(0, -.31), new Point(.08, -.46), new Point(.26, -.39), true, false);
        writer.BezierTo(new Point(.60, -.28), new Point(.55, .04), new Point(0, .46), true, false);
        geometry.Freeze();
        return geometry;
    }

    private static SolidColorBrush Brush(string value, double opacity)
    {
        var color = (Color)ColorConverter.ConvertFromString(value)!;
        color.A = (byte)Math.Round(opacity * byte.MaxValue);
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        return brush;
    }
}
```

## src/FolderThemeStudio.Core/SystemIntegration/CompatibleFolderFileSystem.cs

```csharp
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FolderThemeStudio.Core.SystemIntegration;

internal readonly record struct AtomicDesktopIniWriteResult(bool Created, string? ExpectedSha256)
{
    internal static AtomicDesktopIniWriteResult SuccessfullyCreated(string expectedSha256) => new(true, expectedSha256);
    internal static AtomicDesktopIniWriteResult DestinationExists() => new(false, null);
}

internal interface ICompatibleFolderFileSystem
{
    bool FileExists(string path);
    bool DirectoryExists(string path);
    FileAttributes GetAttributes(string path);
    void SetAttributes(string path, FileAttributes attributes);
    byte[] ReadAllBytes(string path);
    Task<AtomicDesktopIniWriteResult> WriteDesktopIniAtomicallyAsync(string destinationPath, string contents);
    Task<string> ReplaceDesktopIniAtomicallyAsync(string destinationPath, byte[] bytes);
}

internal sealed class WindowsCompatibleFolderFileSystem : ICompatibleFolderFileSystem
{
    public bool FileExists(string path) => File.Exists(path);
    public bool DirectoryExists(string path) => Directory.Exists(path);
    public FileAttributes GetAttributes(string path) => File.GetAttributes(path);
    public void SetAttributes(string path, FileAttributes attributes) => File.SetAttributes(path, attributes);
    public byte[] ReadAllBytes(string path) => File.ReadAllBytes(path);

    public async Task<AtomicDesktopIniWriteResult> WriteDesktopIniAtomicallyAsync(string destinationPath, string contents)
    {
        var directory = Path.GetDirectoryName(destinationPath)
            ?? throw new InvalidOperationException("Desktop.ini must have a parent folder.");
        var temporaryPath = Path.Combine(directory, $".desktop.ini.{Guid.NewGuid():N}.tmp");
        var encoding = new UnicodeEncoding(false, true, true);
        var preamble = encoding.GetPreamble();
        var contentBytes = encoding.GetBytes(contents);
        var bytes = new byte[preamble.Length + contentBytes.Length];
        preamble.CopyTo(bytes, 0);
        contentBytes.CopyTo(bytes, preamble.Length);
        var expectedHash = Convert.ToHexString(SHA256.HashData(bytes));

        try
        {
            await using (var stream = new FileStream(
                temporaryPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                4096,
                FileOptions.Asynchronous | FileOptions.WriteThrough))
            {
                await stream.WriteAsync(bytes).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                stream.Flush(flushToDisk: true);
            }

            try
            {
                File.Move(temporaryPath, destinationPath);
            }
            catch (IOException) when (File.Exists(destinationPath))
            {
                return AtomicDesktopIniWriteResult.DestinationExists();
            }

            return AtomicDesktopIniWriteResult.SuccessfullyCreated(expectedHash);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    public async Task<string> ReplaceDesktopIniAtomicallyAsync(string destinationPath, byte[] bytes)
    {
        var directory = Path.GetDirectoryName(destinationPath)
            ?? throw new InvalidOperationException("Desktop.ini must have a parent folder.");
        var temporaryPath = Path.Combine(directory, $".desktop.ini.{Guid.NewGuid():N}.tmp");
        var expectedHash = Convert.ToHexString(SHA256.HashData(bytes));
        var originalAttributes = File.GetAttributes(destinationPath);
        try
        {
            await using (var stream = new FileStream(
                temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096,
                FileOptions.Asynchronous | FileOptions.WriteThrough))
            {
                await stream.WriteAsync(bytes).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                stream.Flush(flushToDisk: true);
            }
            File.SetAttributes(destinationPath, FileAttributes.Normal);
            try
            {
                File.Move(temporaryPath, destinationPath, overwrite: true);
            }
            catch
            {
                if (File.Exists(destinationPath)) File.SetAttributes(destinationPath, originalAttributes);
                throw;
            }
            return expectedHash;
        }
        finally
        {
            if (File.Exists(temporaryPath)) File.Delete(temporaryPath);
        }
    }
}
```

## src/FolderThemeStudio.Core/SystemIntegration/CompatibleFolderService.cs

```csharp
using System.IO;
using System.Security.Cryptography;
using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Recovery;

namespace FolderThemeStudio.Core.SystemIntegration;

public sealed record CompatibleApplySummary(
    Guid OperationId,
    int Applied,
    int Skipped,
    int Failed,
    bool Cancelled)
{
    public IReadOnlyList<CompatibleApplyOutcome> Outcomes { get; init; } = [];
    public IReadOnlyList<CompatibleApplyOutcome> Unresolved { get; init; } = [];
}

public enum CompatibleApplyOutcomeKind { Applied, Skipped, Failed }
public sealed record CompatibleApplyOutcome(string Path, CompatibleApplyOutcomeKind Kind, string? Detail);

public sealed record CompatibleRestoreSummary(
    Guid OperationId,
    int Restored,
    int ChangedSinceApply,
    int Missing,
    int Failed,
    bool Cancelled)
{
    public IReadOnlyList<CompatibleRestoreOutcome> Outcomes { get; init; } = [];
    public bool IsFullyRestored => ChangedSinceApply == 0 && Failed == 0 && !Cancelled;
}

public enum CompatibleRestoreOutcomeKind { Restored, Missing, ChangedSinceApply, Failed }
public sealed record CompatibleRestoreOutcome(string Path, CompatibleRestoreOutcomeKind Kind, string? Detail);

public sealed class CompatibleFolderService
{
    private const FileAttributes DesktopIniAttributes = FileAttributes.Hidden | FileAttributes.System;
    private const FileAttributes FolderAttributes = FileAttributes.ReadOnly;
    private readonly BackupService backupService;
    private readonly IOwnedFileDeletionService ownedFileDeletion;
    private readonly ICompatibleFolderFileSystem folderFileSystem;
    private readonly KnownFolderService folderPolicy;

    public CompatibleFolderService(BackupService backupService)
        : this(backupService, new WindowsOwnedFileDeletionService(), new WindowsCompatibleFolderFileSystem(), new KnownFolderService())
    {
    }

    internal static CompatibleFolderService CreateForTesting(
        BackupService backupService,
        IOwnedFileDeletionService ownedFileDeletion) =>
        new(backupService, ownedFileDeletion, new WindowsCompatibleFolderFileSystem(), new KnownFolderService());

    internal static CompatibleFolderService CreateForTesting(
        BackupService backupService,
        IOwnedFileDeletionService ownedFileDeletion,
        ICompatibleFolderFileSystem folderFileSystem) =>
        new(backupService, ownedFileDeletion, folderFileSystem, new KnownFolderService());

    internal static CompatibleFolderService CreateForTesting(
        BackupService backupService,
        IOwnedFileDeletionService ownedFileDeletion,
        ICompatibleFolderFileSystem folderFileSystem,
        KnownFolderService folderPolicy) =>
        new(backupService, ownedFileDeletion, folderFileSystem, folderPolicy);

    private CompatibleFolderService(
        BackupService backupService,
        IOwnedFileDeletionService ownedFileDeletion,
        ICompatibleFolderFileSystem folderFileSystem,
        KnownFolderService folderPolicy)
    {
        this.backupService = backupService ?? throw new ArgumentNullException(nameof(backupService));
        this.ownedFileDeletion = ownedFileDeletion ?? throw new ArgumentNullException(nameof(ownedFileDeletion));
        this.folderFileSystem = folderFileSystem ?? throw new ArgumentNullException(nameof(folderFileSystem));
        this.folderPolicy = folderPolicy ?? throw new ArgumentNullException(nameof(folderPolicy));
    }

    public async Task<CompatibleApplySummary> ApplyAsync(
        IReadOnlyList<FolderPlanItem> plan,
        string icoPath,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentException.ThrowIfNullOrWhiteSpace(icoPath);

        var operationId = Guid.NewGuid();
        var snapshot = new OperationSnapshot(
            operationId,
            DateTimeOffset.UtcNow,
            new RegistryValueSnapshot("3", false, null),
            new RegistryValueSnapshot("4", false, null),
            [],
            false)
        {
            ApplicationMode = ApplicationMode.Compatible
        };
        await backupService.CreateAsync(snapshot, CancellationToken.None).ConfigureAwait(false);

        return await ApplyWithExistingSnapshotAsync(plan, icoPath, operationId, token).ConfigureAwait(false);
    }

    public async Task<CompatibleApplySummary> ApplyWithExistingSnapshotAsync(
        IReadOnlyList<FolderPlanItem> plan,
        string icoPath,
        Guid operationId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentException.ThrowIfNullOrWhiteSpace(icoPath);
        if (operationId == Guid.Empty)
        {
            throw new ArgumentException("An operation identifier is required.", nameof(operationId));
        }

        var fullIcoPath = Path.GetFullPath(icoPath);

        var applied = 0;
        var skipped = 0;
        var failed = 0;
        var cancelled = false;
        var hasUntrackedMutation = false;
        var outcomes = new List<CompatibleApplyOutcome>();
        var unresolved = new List<CompatibleApplyOutcome>();

        foreach (var item in plan)
        {
            if (token.IsCancellationRequested)
            {
                cancelled = true;
                break;
            }

            if (item.Decision != FolderDecision.Allowed)
            {
                skipped++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Skipped, item.Decision.ToString()));
                continue;
            }

            var revalidated = folderPolicy.RevalidateForMutation(item);
            if (!revalidated.IsAllowed || string.IsNullOrWhiteSpace(revalidated.CanonicalPath))
            {
                skipped++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Skipped, revalidated.Detail));
                continue;
            }

            var folderPath = NormalizeDirectoryPath(revalidated.CanonicalPath);
            var desktopIniPath = Path.Combine(folderPath, "desktop.ini");
            var fileOriginallyExisted = folderFileSystem.FileExists(desktopIniPath);
            FileAttributes originalFolderAttributes;
            FileAttributes? originalDesktopIniAttributes = null;
            byte[]? originalDesktopIniBytes = null;
            string expectedHash;
            FolderMutationRecord incompleteMutation;
            try
            {
                originalFolderAttributes = folderFileSystem.GetAttributes(folderPath);
                if (fileOriginallyExisted)
                {
                    originalDesktopIniAttributes = folderFileSystem.GetAttributes(desktopIniPath);
                    originalDesktopIniBytes = folderFileSystem.ReadAllBytes(desktopIniPath);
                    var merge = DesktopIniDocument.Merge(originalDesktopIniBytes, fullIcoPath);
                    if (!merge.Success || merge.Bytes is null)
                        throw new InvalidDataException(merge.Error ?? "Desktop.ini could not be merged safely.");
                    expectedHash = Convert.ToHexString(SHA256.HashData(merge.Bytes));
                    incompleteMutation = new FolderMutationRecord(
                        folderPath, originalFolderAttributes, desktopIniPath, expectedHash, 0, false)
                    {
                        Phase = FolderMutationPhase.OriginalCaptured,
                        FileOriginallyExisted = true,
                        OriginalDesktopIniBase64 = Convert.ToBase64String(originalDesktopIniBytes),
                        OriginalDesktopIniAttributes = originalDesktopIniAttributes
                    };
                    await backupService.AppendMutationAsync(operationId, incompleteMutation, CancellationToken.None).ConfigureAwait(false);
                    await folderFileSystem.ReplaceDesktopIniAtomicallyAsync(desktopIniPath, merge.Bytes).ConfigureAwait(false);
                    await backupService.ResetMutationToFileCreatedAsync(operationId, desktopIniPath, CancellationToken.None).ConfigureAwait(false);
                }
                else
                {
                    var contents = $"[.ShellClassInfo]\r\nIconFile={fullIcoPath}\r\nIconIndex=0\r\n";
                    var writeResult = await folderFileSystem.WriteDesktopIniAtomicallyAsync(desktopIniPath, contents).ConfigureAwait(false);
                    if (!writeResult.Created)
                    {
                        skipped++;
                        outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Skipped, "Desktop.ini appeared concurrently."));
                        continue;
                    }
                    expectedHash = writeResult.ExpectedSha256
                        ?? throw new InvalidOperationException("A successful Desktop.ini write must return its ownership hash.");
                    incompleteMutation = new FolderMutationRecord(
                        folderPath, originalFolderAttributes, desktopIniPath, expectedHash, 0, false);
                }
            }
            catch (Exception exception) when (IsOperationalFailure(exception))
            {
                failed++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Failed, exception.Message));
                continue;
            }

            try
            {
                if (!fileOriginallyExisted)
                    await backupService.AppendMutationAsync(operationId, incompleteMutation, CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception appendException) when (IsOperationalFailure(appendException))
            {
                var cleanupResolved = false;
                try
                {
                    var deletion = ownedFileDeletion.DeleteIfHashMatches(desktopIniPath, expectedHash);
                    cleanupResolved = deletion is OwnedFileDeletionOutcome.Deleted or OwnedFileDeletionOutcome.Missing;
                }
                catch (Exception deletionException) when (IsOperationalFailure(deletionException))
                {
                    cleanupResolved = false;
                }

                if (!cleanupResolved)
                {
                    try
                    {
                        await backupService.AppendMutationAsync(operationId, incompleteMutation, CancellationToken.None).ConfigureAwait(false);
                    }
                    catch (Exception retryException) when (IsOperationalFailure(retryException))
                    {
                        hasUntrackedMutation = true;
                        unresolved.Add(new CompatibleApplyOutcome(
                            item.Path,
                            CompatibleApplyOutcomeKind.Failed,
                            "Desktop.ini could not be safely removed or durably recorded."));
                    }
                }

                failed++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Failed, appendException.Message));
                continue;
            }

            FileAttributes addedFolderAttributes;
            try
            {
                var currentFileAttributes = folderFileSystem.GetAttributes(desktopIniPath);
                folderFileSystem.SetAttributes(desktopIniPath, currentFileAttributes | DesktopIniAttributes);

                var currentFolderAttributes = folderFileSystem.GetAttributes(folderPath);
                addedFolderAttributes = FolderAttributes & ~currentFolderAttributes;
                await backupService.PersistFolderAttributeIntentAsync(
                    operationId,
                    desktopIniPath,
                    addedFolderAttributes,
                    CancellationToken.None).ConfigureAwait(false);
                folderFileSystem.SetAttributes(folderPath, currentFolderAttributes | FolderAttributes);
            }
            catch (Exception exception) when (IsOperationalFailure(exception))
            {
                failed++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Failed, exception.Message));
                continue;
            }

            try
            {
                await backupService.CompleteMutationAsync(
                    operationId,
                    desktopIniPath,
                    addedFolderAttributes,
                    CancellationToken.None).ConfigureAwait(false);
                applied++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Applied, null));
            }
            catch (Exception exception) when (IsOperationalFailure(exception))
            {
                var compensationSucceeded = TryRevertAddedAttributes(folderPath, addedFolderAttributes);
                if (compensationSucceeded)
                {
                    try
                    {
                        await backupService.ResetMutationToFileCreatedAsync(
                            operationId,
                            desktopIniPath,
                            CancellationToken.None).ConfigureAwait(false);
                    }
                    catch (Exception resetException) when (IsOperationalFailure(resetException))
                    {
                        hasUntrackedMutation = true;
                    }
                }
                else
                {
                    if (!TryGetRemainingAddedAttributes(
                        folderPath,
                        addedFolderAttributes,
                        out var remainingAddedFolderAttributes))
                    {
                        hasUntrackedMutation = true;
                    }
                    else
                    {
                        try
                        {
                            await backupService.CompleteMutationAsync(
                                operationId,
                                desktopIniPath,
                                remainingAddedFolderAttributes,
                                CancellationToken.None).ConfigureAwait(false);
                        }
                        catch (Exception retryException) when (IsOperationalFailure(retryException))
                        {
                            hasUntrackedMutation = true;
                        }
                    }
                }

                failed++;
                outcomes.Add(new CompatibleApplyOutcome(item.Path, CompatibleApplyOutcomeKind.Failed, exception.Message));
            }
        }

        if (!cancelled && !hasUntrackedMutation)
        {
            await backupService.CompleteAsync(operationId, CancellationToken.None).ConfigureAwait(false);
        }

        return new CompatibleApplySummary(operationId, applied, skipped, failed, cancelled)
        {
            Outcomes = outcomes,
            Unresolved = unresolved
        };
    }

    public async Task<CompatibleRestoreSummary> RestoreAsync(Guid operationId, CancellationToken token)
    {
        var snapshot = await backupService.LoadAsync(operationId, CancellationToken.None).ConfigureAwait(false);
        var restored = 0;
        var changedSinceApply = 0;
        var missing = 0;
        var failed = 0;
        var cancelled = false;
        var outcomes = new List<CompatibleRestoreOutcome>();

        foreach (var mutation in snapshot.Mutations)
        {
            if (token.IsCancellationRequested)
            {
                cancelled = true;
                break;
            }

            try
            {
                if (mutation.FileOriginallyExisted)
                {
                    if (!folderFileSystem.FileExists(mutation.CreatedFilePath))
                    {
                        changedSinceApply++;
                        outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.ChangedSinceApply, "The original Desktop.ini is now missing."));
                        continue;
                    }

                    var currentBytes = folderFileSystem.ReadAllBytes(mutation.CreatedFilePath);
                    var currentHash = Convert.ToHexString(SHA256.HashData(currentBytes));
                    var originalBytes = Convert.FromBase64String(mutation.OriginalDesktopIniBase64!);
                    var originalHash = Convert.ToHexString(SHA256.HashData(originalBytes));
                    if (string.Equals(currentHash, mutation.ExpectedDesktopIniSha256, StringComparison.OrdinalIgnoreCase))
                    {
                        await folderFileSystem.ReplaceDesktopIniAtomicallyAsync(mutation.CreatedFilePath, originalBytes).ConfigureAwait(false);
                        folderFileSystem.SetAttributes(mutation.CreatedFilePath, mutation.OriginalDesktopIniAttributes!.Value);
                    }
                    else if (!string.Equals(currentHash, originalHash, StringComparison.OrdinalIgnoreCase))
                    {
                        changedSinceApply++;
                        outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.ChangedSinceApply, "Desktop.ini changed since apply."));
                        continue;
                    }

                    if (RequiresFolderAttributeRestore(mutation)) RestoreChangedFolderAttributes(mutation);
                    restored++;
                    outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.Restored, null));
                    continue;
                }

                var deletion = ownedFileDeletion.DeleteIfHashMatches(
                    mutation.CreatedFilePath,
                    mutation.ExpectedDesktopIniSha256);
                if (deletion == OwnedFileDeletionOutcome.Missing)
                {
                    if (RequiresFolderAttributeRestore(mutation)) RestoreChangedFolderAttributes(mutation);
                    missing++;
                    outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.Missing, "Owned Desktop.ini was already missing; owned attributes were resolved."));
                }
                else if (deletion == OwnedFileDeletionOutcome.HashMismatch)
                {
                    changedSinceApply++;
                    outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.ChangedSinceApply, "Desktop.ini changed since apply."));
                    continue;
                }
                else
                {
                    if (RequiresFolderAttributeRestore(mutation)) RestoreChangedFolderAttributes(mutation);
                    restored++;
                    outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.Restored, null));
                }
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException)
            {
                failed++;
                outcomes.Add(new CompatibleRestoreOutcome(mutation.FolderPath, CompatibleRestoreOutcomeKind.Failed, exception.Message));
            }
        }

        return new CompatibleRestoreSummary(operationId, restored, changedSinceApply, missing, failed, cancelled)
        {
            Outcomes = outcomes
        };
    }

    private static bool RequiresFolderAttributeRestore(FolderMutationRecord mutation) =>
        mutation.Phase is FolderMutationPhase.FolderAttributeIntentPersisted or FolderMutationPhase.Completed;

    private static string NormalizeDirectoryPath(string path) =>
        Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));

    private void RestoreChangedFolderAttributes(FolderMutationRecord mutation)
    {
        if (!folderFileSystem.DirectoryExists(mutation.FolderPath))
        {
            return;
        }

        if (mutation.AddedFolderAttributes == 0)
        {
            return;
        }

        var currentAttributes = folderFileSystem.GetAttributes(mutation.FolderPath);
        folderFileSystem.SetAttributes(
            mutation.FolderPath,
            currentAttributes & ~mutation.AddedFolderAttributes);
    }

    private bool TryRevertAddedAttributes(string path, FileAttributes addedAttributes)
    {
        if (addedAttributes == 0)
        {
            return true;
        }

        try
        {
            var currentAttributes = folderFileSystem.GetAttributes(path);
            folderFileSystem.SetAttributes(path, currentAttributes & ~addedAttributes);
            return true;
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            return false;
        }
    }

    private bool TryGetRemainingAddedAttributes(
        string path,
        FileAttributes addedAttributes,
        out FileAttributes remainingAddedAttributes)
    {
        try
        {
            remainingAddedAttributes = folderFileSystem.GetAttributes(path) & addedAttributes;
            return true;
        }
        catch (Exception exception) when (IsOperationalFailure(exception))
        {
            remainingAddedAttributes = 0;
            return false;
        }
    }

    private static bool IsOperationalFailure(Exception exception) =>
        exception is IOException
            or UnauthorizedAccessException
            or System.Security.SecurityException
            or InvalidOperationException
            or ArgumentException;
}
```

## src/FolderThemeStudio.Core/SystemIntegration/DesktopIniDocument.cs

```csharp
using System.Text;

namespace FolderThemeStudio.Core.SystemIntegration;

internal sealed record DesktopIniMergeResult(bool Success, byte[]? Bytes, string? Error)
{
    internal static DesktopIniMergeResult Succeeded(byte[] bytes) => new(true, bytes, null);
    internal static DesktopIniMergeResult Failure(string error) => new(false, null, error);
}

internal static class DesktopIniDocument
{
    private const string ShellSection = ".ShellClassInfo";

    internal static DesktopIniMergeResult Merge(byte[]? originalBytes, string icoPath)
    {
        if (string.IsNullOrWhiteSpace(icoPath) || icoPath.IndexOfAny(['\r', '\n', '\0']) >= 0)
            return DesktopIniMergeResult.Failure("The icon path is invalid.");

        try
        {
            var source = originalBytes ?? [];
            var decoded = Decode(source);
            if (!decoded.Success) return DesktopIniMergeResult.Failure(decoded.Error!);

            var newline = decoded.Text!.Length == 0 || decoded.Text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
            var hadTrailingNewline = decoded.Text.Length > 0 &&
                (decoded.Text.EndsWith("\n", StringComparison.Ordinal) || decoded.Text.EndsWith("\r", StringComparison.Ordinal));
            var normalized = decoded.Text.Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\r', '\n');
            var lines = normalized.Length == 0
                ? new List<string>()
                : normalized.Split('\n').ToList();
            if (hadTrailingNewline && lines.Count > 0 && lines[^1].Length == 0) lines.RemoveAt(lines.Count - 1);

            var sectionNames = new string?[lines.Count];
            string? currentSection = null;
            var shellSectionCount = 0;
            for (var index = 0; index < lines.Count; index++)
            {
                var trimmed = lines[index].Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith(';') || trimmed.StartsWith('#'))
                {
                    sectionNames[index] = currentSection;
                    continue;
                }

                if (trimmed.StartsWith('['))
                {
                    if (!trimmed.EndsWith(']') || trimmed.Length < 3 || trimmed[1..^1].Contains('[') || trimmed[1..^1].Contains(']'))
                        return DesktopIniMergeResult.Failure($"Unsafe section syntax on line {index + 1}.");
                    currentSection = trimmed[1..^1].Trim();
                    if (currentSection.Length == 0)
                        return DesktopIniMergeResult.Failure($"Empty section on line {index + 1}.");
                    if (currentSection.Equals(ShellSection, StringComparison.OrdinalIgnoreCase)) shellSectionCount++;
                    if (shellSectionCount > 1)
                        return DesktopIniMergeResult.Failure("Multiple .ShellClassInfo sections are not supported.");
                    sectionNames[index] = currentSection;
                    continue;
                }

                if (currentSection is null || !trimmed.Contains('='))
                    return DesktopIniMergeResult.Failure($"Unsafe key syntax on line {index + 1}.");
                sectionNames[index] = currentSection;
            }

            var output = new List<string>();
            if (shellSectionCount == 0)
            {
                output.AddRange(lines);
                output.Add($"[{ShellSection}]");
                output.Add($"IconFile={icoPath}");
                output.Add("IconIndex=0");
            }
            else
            {
                var inserted = false;
                for (var index = 0; index < lines.Count; index++)
                {
                    var line = lines[index];
                    var trimmed = line.Trim();
                    var inShell = sectionNames[index]?.Equals(ShellSection, StringComparison.OrdinalIgnoreCase) == true;
                    var startsNextSection = trimmed.StartsWith('[') &&
                        !trimmed[1..^1].Trim().Equals(ShellSection, StringComparison.OrdinalIgnoreCase);
                    if (!inserted && startsNextSection && output.Any(item => item.Trim().Equals($"[{ShellSection}]", StringComparison.OrdinalIgnoreCase)))
                    {
                        AddIconLines(output, icoPath);
                        inserted = true;
                    }

                    if (inShell && TryGetKey(trimmed, out var key) &&
                        key is "iconfile" or "iconindex" or "iconresource")
                        continue;
                    output.Add(line);
                }
                if (!inserted) AddIconLines(output, icoPath);
            }

            var resultText = string.Join(newline, output) + newline;
            return DesktopIniMergeResult.Succeeded(Encode(resultText, decoded.Encoding!, decoded.HasBom));
        }
        catch (Exception exception) when (exception is DecoderFallbackException or EncoderFallbackException or ArgumentException)
        {
            return DesktopIniMergeResult.Failure(exception.Message);
        }
    }

    private static void AddIconLines(List<string> lines, string icoPath)
    {
        lines.Add($"IconFile={icoPath}");
        lines.Add("IconIndex=0");
    }

    private static bool TryGetKey(string line, out string key)
    {
        var separator = line.IndexOf('=');
        key = separator > 0 ? line[..separator].Trim().ToLowerInvariant() : string.Empty;
        return separator > 0;
    }

    private static (bool Success, string? Text, Encoding? Encoding, bool HasBom, string? Error) Decode(byte[] bytes)
    {
        if (bytes.Contains((byte)0) && !(bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE))
            return (false, null, null, false, "The document contains unsupported NUL bytes.");

        Encoding encoding;
        var offset = 0;
        var hasBom = false;
        if (bytes.AsSpan().StartsWith(Encoding.Unicode.GetPreamble()))
        {
            encoding = new UnicodeEncoding(false, false, true);
            offset = 2;
            hasBom = true;
        }
        else if (bytes.AsSpan().StartsWith(Encoding.UTF8.GetPreamble()))
        {
            encoding = new UTF8Encoding(false, true);
            offset = 3;
            hasBom = true;
        }
        else
        {
            encoding = new UTF8Encoding(false, true);
        }

        try { return (true, encoding.GetString(bytes, offset, bytes.Length - offset), encoding, hasBom, null); }
        catch (DecoderFallbackException exception) { return (false, null, null, false, exception.Message); }
    }

    private static byte[] Encode(string text, Encoding encoding, bool includeBom)
    {
        var content = encoding.GetBytes(text);
        if (!includeBom) return content;
        var preamble = encoding.CodePage == Encoding.Unicode.CodePage
            ? Encoding.Unicode.GetPreamble()
            : Encoding.UTF8.GetPreamble();
        return preamble.Concat(content).ToArray();
    }
}
```

## src/FolderThemeStudio.Core/SystemIntegration/GlobalIconService.cs

```csharp
using FolderThemeStudio.Core.Recovery;
using Microsoft.Win32;

namespace FolderThemeStudio.Core.SystemIntegration;

public sealed record GlobalApplyResult(
    bool Success,
    RegistryValueSnapshot? RegistryValue3,
    RegistryValueSnapshot? RegistryValue4,
    ShellRefreshResult ShellRefresh,
    string? Error);

public sealed record GlobalRestoreResult(bool Success, ShellRefreshResult ShellRefresh, string? Error);

public sealed record GlobalSnapshotCaptureResult(
    bool Success,
    RegistryValueSnapshot? RegistryValue3,
    RegistryValueSnapshot? RegistryValue4,
    string? Error);

internal interface IRegistryStore
{
    bool TryGet(string name, out RegistryStoreValue? value);
    void Set(string name, string value, RegistryValueKind kind);
    void Delete(string name);
}

internal sealed record RegistryStoreValue(string RawValue, RegistryValueKind Kind);

public sealed class GlobalIconService
{
    private const string ClosedFolderValueName = "3";
    private const string OpenFolderValueName = "4";
    private readonly IRegistryStore registry;
    private readonly ShellRefreshService shellRefresh;

    public GlobalIconService()
        : this(new WindowsRegistryStore(), new ShellRefreshService())
    {
    }

    internal static GlobalIconService CreateForTesting(IRegistryStore registry, ShellRefreshService shellRefresh) =>
        new(registry, shellRefresh);

    private GlobalIconService(IRegistryStore registry, ShellRefreshService shellRefresh)
    {
        this.registry = registry;
        this.shellRefresh = shellRefresh;
    }

    public GlobalApplyResult Apply(string icoPath)
        => ApplyCore(icoPath, refreshShell: true);

    public GlobalApplyResult ApplyWithoutRefresh(string icoPath)
        => ApplyCore(icoPath, refreshShell: false);

    public GlobalSnapshotCaptureResult CaptureSnapshot()
    {
        try
        {
            return new GlobalSnapshotCaptureResult(true, Capture(ClosedFolderValueName), Capture(OpenFolderValueName), null);
        }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.Security.SecurityException or System.IO.IOException or InvalidOperationException or ArgumentException)
        {
            return new GlobalSnapshotCaptureResult(false, null, null, exception.Message);
        }
    }

    private GlobalApplyResult ApplyCore(string icoPath, bool refreshShell)
    {
        if (string.IsNullOrWhiteSpace(icoPath))
        {
            return ApplyFailure(null, null, "An icon path is required.");
        }

        RegistryValueSnapshot? value3 = null;
        RegistryValueSnapshot? value4 = null;
        try
        {
            value3 = Capture(ClosedFolderValueName);
            value4 = Capture(OpenFolderValueName);
            var mapping = $"{icoPath},0";
            registry.Set(ClosedFolderValueName, mapping, RegistryValueKind.String);
            registry.Set(OpenFolderValueName, mapping, RegistryValueKind.String);

            if (!HasValue(ClosedFolderValueName, mapping) || !HasValue(OpenFolderValueName, mapping))
            {
                return ApplyFailure(value3, value4, "Windows did not retain the requested folder icon mappings.");
            }
        }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.Security.SecurityException or System.IO.IOException or InvalidOperationException or ArgumentException)
        {
            return ApplyFailure(value3, value4, exception.Message);
        }

        return new GlobalApplyResult(
            true,
            value3,
            value4,
            refreshShell ? shellRefresh.RefreshIcons() : NotAttemptedRefresh(),
            null);
    }

    public GlobalRestoreResult Restore(OperationSnapshot snapshot)
        => RestoreCore(snapshot, refreshShell: true);

    public GlobalRestoreResult RestoreWithoutRefresh(OperationSnapshot snapshot)
        => RestoreCore(snapshot, refreshShell: false);

    private GlobalRestoreResult RestoreCore(OperationSnapshot snapshot, bool refreshShell)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        snapshot.Validate();

        try
        {
            Restore(snapshot.RegistryValue3);
            Restore(snapshot.RegistryValue4);

            if (!Matches(snapshot.RegistryValue3) || !Matches(snapshot.RegistryValue4))
            {
                return new GlobalRestoreResult(false, NotAttemptedRefresh(), "Windows did not retain the restored folder icon mappings.");
            }
        }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.Security.SecurityException or System.IO.IOException or InvalidOperationException or ArgumentException)
        {
            return new GlobalRestoreResult(false, NotAttemptedRefresh(), exception.Message);
        }

        return new GlobalRestoreResult(
            true,
            refreshShell ? shellRefresh.RefreshIcons() : NotAttemptedRefresh(),
            null);
    }

    private RegistryValueSnapshot Capture(string name)
    {
        var existed = registry.TryGet(name, out var value);
        if (existed && value is null)
        {
            throw new InvalidOperationException($"Registry value '{name}' exists without a string value.");
        }

        return new RegistryValueSnapshot(name, existed, value?.RawValue, value?.Kind);
    }

    private bool HasValue(string name, string expected) =>
        registry.TryGet(name, out var actual)
        && actual is not null
        && actual.Kind == RegistryValueKind.String
        && string.Equals(actual.RawValue, expected, StringComparison.Ordinal);

    private void Restore(RegistryValueSnapshot value)
    {
        if (value.Existed)
        {
            registry.Set(value.Name, value.Value!, value.Kind!.Value);
        }
        else
        {
            registry.Delete(value.Name);
        }
    }

    private bool Matches(RegistryValueSnapshot expected)
    {
        var exists = registry.TryGet(expected.Name, out var actual);
        return expected.Existed
            ? exists
                && actual is not null
                && actual.Kind == expected.Kind
                && string.Equals(actual.RawValue, expected.Value, StringComparison.Ordinal)
            : !exists;
    }

    private static GlobalApplyResult ApplyFailure(RegistryValueSnapshot? value3, RegistryValueSnapshot? value4, string error) =>
        new(false, value3, value4, NotAttemptedRefresh(), error);

    private static ShellRefreshResult NotAttemptedRefresh() => new(false, false, null);
}

internal sealed class WindowsRegistryStore : IRegistryStore
{
    private const string ShellIconsKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons";

    public bool TryGet(string name, out RegistryStoreValue? value)
    {
        using var key = Registry.CurrentUser.OpenSubKey(ShellIconsKeyPath, writable: false);
        if (key is null)
        {
            value = null;
            return false;
        }

        var rawValue = key.GetValue(name, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
        if (rawValue is null)
        {
            value = null;
            return false;
        }

        var kind = key.GetValueKind(name);
        if (kind is not (RegistryValueKind.String or RegistryValueKind.ExpandString))
        {
            throw new InvalidOperationException($"Registry value '{name}' is not REG_SZ or REG_EXPAND_SZ.");
        }

        value = new RegistryStoreValue(
            rawValue as string ?? throw new InvalidOperationException($"Registry value '{name}' is not a string."),
            kind);
        return true;
    }

    public void Set(string name, string value, RegistryValueKind kind)
    {
        using var key = Registry.CurrentUser.CreateSubKey(ShellIconsKeyPath, writable: true)
            ?? throw new InvalidOperationException("Could not open the current-user Shell Icons registry key.");
        if (kind is not (RegistryValueKind.String or RegistryValueKind.ExpandString))
        {
            throw new ArgumentOutOfRangeException(nameof(kind));
        }

        key.SetValue(name, value, kind);
    }

    public void Delete(string name)
    {
        using var key = Registry.CurrentUser.OpenSubKey(ShellIconsKeyPath, writable: true);
        key?.DeleteValue(name, throwOnMissingValue: false);
    }
}
```

## src/FolderThemeStudio.Core/SystemIntegration/KnownFolderService.cs

```csharp
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

[assembly: InternalsVisibleTo("FolderThemeStudio.Core.Tests")]

namespace FolderThemeStudio.Core.SystemIntegration;

public enum FolderDecision
{
    Allowed,
    KnownFolder,
    ProtectedRoot,
    NetworkPath,
    ReparsePoint,
    NoWriteAccess,
    ExistingCustomization
}

public sealed record FolderPlanItem(
    string Path,
    FolderDecision Decision,
    string? SelectedRoot = null,
    string? CanonicalPath = null,
    string? CanonicalRoot = null);

internal sealed record FolderMutationValidation(
    bool IsAllowed,
    string? CanonicalPath,
    string Detail);

internal enum DesktopIniStatus { Absent, Present, Indeterminate }

internal sealed record FolderInspection(
    string CanonicalPath,
    FolderDecision? Rejection,
    System.IO.DriveType DriveType,
    System.IO.FileAttributes Attributes,
    DesktopIniStatus DesktopIniStatus,
    bool CanWrite);

internal interface IFolderSystem
{
    FolderInspection Inspect(string path);
    IEnumerable<string> EnumerateDirectories(string path, System.IO.EnumerationOptions options);
}

public sealed class KnownFolderService
{
    private const uint FileAddFile = 0x0002;
    private const uint FileShareReadWriteDelete = 0x0007;
    private const uint OpenExisting = 3;
    private const uint FileFlagBackupSemantics = 0x02000000;

    private static readonly Guid[] KnownFolderIds =
    [
        new("B4BFCC3A-DB2C-424C-B029-7FE99A87C641"), // Desktop
        new("374DE290-123F-4565-9164-39C4925E467B"), // Downloads
        new("FDD39AD0-238F-46AF-ADB4-6C85480369C7"), // Documents
        new("33E28130-4E1E-4676-835A-98395C3BC3BB"), // Pictures
        new("4BD8D571-6D19-48D3-BE97-422220080E43"), // Music
        new("18989B1D-99B5-455B-841C-AB7C74E4DDFC"), // Videos
        new("1777F761-68AD-4D8A-87BD-30B759FA33DD"), // Favorites
        new("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"), // Saved Games
        new("A52BBA46-E9E1-435F-B3D9-28DAA648C0F6"), // OneDrive
        new("DFDF76A2-C82A-4D63-906A-5644AC457385"), // Public
        new("C4AA340D-F20F-4863-AFEF-F87EF2E6BA25"), // Public Desktop
        new("ED4824AF-DCE4-45A8-81E2-FC7965083634"), // Public Documents
        new("3D644C9B-1FB8-4F30-9B45-F670235F79C0"), // Public Downloads
        new("B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5"), // Public Pictures
        new("3214FAB5-9757-4298-BB61-92A9DEAA44FF"), // Public Music
        new("2400183A-6185-49FB-A2D8-4A392A602BA3")  // Public Videos
    ];

    private readonly HashSet<string> knownFolderPaths;
    private readonly string[] protectedRoots;
    private readonly IFolderSystem folderSystem;

    public KnownFolderService()
        : this(new WindowsFolderSystem(), null)
    {
    }

    internal static KnownFolderService CreateForTesting(IFolderSystem folderSystem, IEnumerable<string>? injectedKnownFolderPaths) =>
        new(folderSystem, injectedKnownFolderPaths);

    private KnownFolderService(IFolderSystem folderSystem, IEnumerable<string>? injectedKnownFolderPaths)
    {
        this.folderSystem = folderSystem;
        knownFolderPaths = CanonicalizePaths(ResolveKnownFolderPaths().Concat(injectedKnownFolderPaths ?? []));
        protectedRoots = CanonicalizePaths(GetProtectedRoots()).ToArray();
    }

    public FolderDecision Evaluate(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return FolderDecision.NoWriteAccess;
        }

        FolderInspection inspection;
        try
        {
            inspection = folderSystem.Inspect(path);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException or System.IO.IOException or UnauthorizedAccessException)
        {
            return FolderDecision.NoWriteAccess;
        }

        if (inspection.Rejection is { } rejection)
        {
            return rejection;
        }

        var canonicalPath = inspection.CanonicalPath;
        if (inspection.DriveType == System.IO.DriveType.Network || IsVolumeRoot(canonicalPath) || protectedRoots.Any(root => IsAtOrBelow(canonicalPath, root)))
        {
            return inspection.DriveType == System.IO.DriveType.Network ? FolderDecision.NetworkPath : FolderDecision.ProtectedRoot;
        }

        if (knownFolderPaths.Contains(canonicalPath))
        {
            return FolderDecision.KnownFolder;
        }

        if ((inspection.Attributes & System.IO.FileAttributes.Directory) == 0)
        {
            return FolderDecision.NoWriteAccess;
        }

        if ((inspection.Attributes & System.IO.FileAttributes.ReparsePoint) != 0)
        {
            return FolderDecision.ReparsePoint;
        }

        return (inspection.DesktopIniStatus is DesktopIniStatus.Absent or DesktopIniStatus.Present) && inspection.CanWrite
            ? FolderDecision.Allowed
            : FolderDecision.NoWriteAccess;
    }

    internal FolderMutationValidation RevalidateForMutation(FolderPlanItem item)
    {
        if (string.IsNullOrWhiteSpace(item.SelectedRoot)
            || string.IsNullOrWhiteSpace(item.CanonicalRoot)
            || string.IsNullOrWhiteSpace(item.CanonicalPath))
        {
            return new FolderMutationValidation(false, null, "MissingRootProvenance");
        }

        try
        {
            var rootInspection = folderSystem.Inspect(item.SelectedRoot);
            var targetInspection = folderSystem.Inspect(item.Path);
            var rootDecision = EvaluateBasePolicy(rootInspection, includeCustomization: false);
            if (rootDecision is not (FolderDecision.Allowed or FolderDecision.KnownFolder))
            {
                return new FolderMutationValidation(false, null, rootDecision.ToString());
            }

            var targetDecision = EvaluateBasePolicy(targetInspection, includeCustomization: false);
            if (targetDecision != FolderDecision.Allowed)
            {
                return new FolderMutationValidation(false, null, targetDecision.ToString());
            }

            if (!string.Equals(rootInspection.CanonicalPath, item.CanonicalRoot, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(targetInspection.CanonicalPath, item.CanonicalPath, StringComparison.OrdinalIgnoreCase)
                || !IsAtOrBelow(targetInspection.CanonicalPath, rootInspection.CanonicalPath))
            {
                return new FolderMutationValidation(false, null, FolderDecision.ProtectedRoot.ToString());
            }

            return new FolderMutationValidation(true, targetInspection.CanonicalPath, FolderDecision.Allowed.ToString());
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException or System.IO.IOException or UnauthorizedAccessException or KeyNotFoundException)
        {
            return new FolderMutationValidation(false, null, FolderDecision.NoWriteAccess.ToString());
        }
    }

    private FolderDecision EvaluateBasePolicy(FolderInspection inspection, bool includeCustomization)
    {
        if (inspection.Rejection is { } rejection) return rejection;
        var canonicalPath = inspection.CanonicalPath;
        if (inspection.DriveType == System.IO.DriveType.Network) return FolderDecision.NetworkPath;
        if (IsVolumeRoot(canonicalPath) || protectedRoots.Any(root => IsAtOrBelow(canonicalPath, root))) return FolderDecision.ProtectedRoot;
        if (knownFolderPaths.Contains(canonicalPath)) return FolderDecision.KnownFolder;
        if ((inspection.Attributes & System.IO.FileAttributes.Directory) == 0) return FolderDecision.NoWriteAccess;
        if ((inspection.Attributes & System.IO.FileAttributes.ReparsePoint) != 0) return FolderDecision.ReparsePoint;
        if (inspection.DesktopIniStatus == DesktopIniStatus.Indeterminate || !inspection.CanWrite) return FolderDecision.NoWriteAccess;
        return FolderDecision.Allowed;
    }

    public async IAsyncEnumerable<FolderPlanItem> PlanTreeAsync(
        string root,
        [EnumeratorCancellation] CancellationToken token)
    {
        if (!TryNormalizePath(root, out var normalizedRoot))
        {
            yield return new FolderPlanItem(root, FolderDecision.NoWriteAccess);
            yield break;
        }

        var pending = new Queue<string>();
        pending.Enqueue(normalizedRoot);
        var enumerationOptions = new System.IO.EnumerationOptions
        {
            RecurseSubdirectories = false,
            IgnoreInaccessible = true,
            AttributesToSkip = System.IO.FileAttributes.ReparsePoint,
            ReturnSpecialDirectories = false
        };
        var reportingEnumerationOptions = new System.IO.EnumerationOptions
        {
            RecurseSubdirectories = false,
            IgnoreInaccessible = true,
            AttributesToSkip = 0,
            ReturnSpecialDirectories = false
        };

        while (pending.TryDequeue(out var current))
        {
            token.ThrowIfCancellationRequested();
            var currentDecision = Evaluate(current);
            var isSelectedKnownFolder = currentDecision == FolderDecision.KnownFolder
                && string.Equals(current, normalizedRoot, StringComparison.OrdinalIgnoreCase);

            if (currentDecision != FolderDecision.Allowed && !isSelectedKnownFolder)
            {
                yield return CreatePlanItem(current, currentDecision, normalizedRoot);
                continue;
            }

            var skipped = new List<FolderPlanItem>();
            var reportedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!TryScanChildren(current, enumerationOptions, enqueueAllowedChildren: true, token, pending, skipped, reportedPaths)
                || !TryScanChildren(current, reportingEnumerationOptions, enqueueAllowedChildren: false, token, pending, skipped, reportedPaths))
            {
                yield return CreatePlanItem(
                    current,
                    currentDecision == FolderDecision.Allowed ? FolderDecision.NoWriteAccess : currentDecision,
                    normalizedRoot);
                continue;
            }

            yield return CreatePlanItem(current, currentDecision, normalizedRoot);
            foreach (var skippedItem in skipped) yield return skippedItem;
            await Task.Yield();
        }
    }

    private FolderPlanItem CreatePlanItem(string path, FolderDecision decision, string selectedRoot)
    {
        try
        {
            return new FolderPlanItem(
                path,
                decision,
                selectedRoot,
                folderSystem.Inspect(path).CanonicalPath,
                folderSystem.Inspect(selectedRoot).CanonicalPath);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException or System.IO.IOException or UnauthorizedAccessException)
        {
            return new FolderPlanItem(path, decision, selectedRoot);
        }
    }

    private bool TryScanChildren(
        string current,
        System.IO.EnumerationOptions options,
        bool enqueueAllowedChildren,
        CancellationToken token,
        Queue<string> pending,
        List<FolderPlanItem> skipped,
        HashSet<string> reportedPaths)
    {
        try
        {
            foreach (var child in folderSystem.EnumerateDirectories(current, options))
            {
                token.ThrowIfCancellationRequested();
                var decision = Evaluate(child);
                if (decision == FolderDecision.Allowed)
                {
                    if (enqueueAllowedChildren)
                    {
                        pending.Enqueue(child);
                    }
                }
                else if (reportedPaths.Add(child))
                {
                    skipped.Add(new FolderPlanItem(child, decision));
                }
            }

            return true;
        }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.IO.DirectoryNotFoundException or System.IO.IOException or System.Security.SecurityException)
        {
            return false;
        }
    }

    private static IEnumerable<string> ResolveKnownFolderPaths()
    {
        foreach (var folderId in KnownFolderIds)
        {
            if (SHGetKnownFolderPath(folderId, 0, IntPtr.Zero, out var pathPointer) != 0 || pathPointer == IntPtr.Zero)
            {
                continue;
            }

            try
            {
                var path = Marshal.PtrToStringUni(pathPointer);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    yield return path;
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(pathPointer);
            }
        }
    }

    private static IEnumerable<string> GetProtectedRoots()
    {
        var candidates = new[]
        {
            Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            Environment.SystemDirectory,
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
        };

        return candidates
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(NormalizePath)
            .Distinct(StringComparer.OrdinalIgnoreCase);
    }

    private HashSet<string> CanonicalizePaths(IEnumerable<string> paths)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var path in paths.Where(path => !string.IsNullOrWhiteSpace(path)))
        {
            try
            {
                result.Add(NormalizePath(path));
                result.Add(folderSystem.Inspect(path).CanonicalPath);
            }
            catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException or System.IO.IOException or UnauthorizedAccessException)
            {
                // The lexical path remains protected when physical resolution is unavailable.
            }
        }

        return result;
    }

    internal static bool CanCreateFileIn(string path)
    {
        using var handle = CreateFile(path, FileAddFile, FileShareReadWriteDelete, IntPtr.Zero, OpenExisting, FileFlagBackupSemantics, IntPtr.Zero);
        return !handle.IsInvalid;
    }

    private static bool IsAtOrBelow(string path, string root) =>
        path.Equals(root, StringComparison.OrdinalIgnoreCase)
        || path.StartsWith(root + System.IO.Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
        || path.StartsWith(root + System.IO.Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);

    internal static bool IsNetworkPath(string path) =>
        path.StartsWith(@"\\?\UNC\", StringComparison.OrdinalIgnoreCase)
        || (path.StartsWith(@"\\", StringComparison.Ordinal) && !path.StartsWith(@"\\?\", StringComparison.Ordinal));

    private static bool IsVolumeRoot(string path) =>
        path.Equals(System.IO.Path.GetPathRoot(path), StringComparison.OrdinalIgnoreCase);

    private static bool TryNormalizePath(string path, out string normalizedPath)
    {
        try
        {
            normalizedPath = NormalizePath(path);
            return true;
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException)
        {
            normalizedPath = string.Empty;
            return false;
        }
    }

    internal static string NormalizePath(string path)
    {
        var fullPath = System.IO.Path.GetFullPath(path);
        var root = System.IO.Path.GetPathRoot(fullPath) ?? string.Empty;
        return fullPath.Length > root.Length
            ? fullPath.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar)
            : fullPath;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern int SHGetKnownFolderPath(
        [In] Guid rfid,
        uint dwFlags,
        IntPtr hToken,
        out IntPtr ppszPath);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(
        string fileName,
        uint desiredAccess,
        uint shareMode,
        IntPtr securityAttributes,
        uint creationDisposition,
        uint flagsAndAttributes,
        IntPtr templateFile);
}

internal sealed class WindowsFolderSystem : IFolderSystem
{
    private const uint FileReadAttributes = 0x0080;
    private const uint FileShareReadWriteDelete = 0x0007;
    private const uint OpenExisting = 3;
    private const uint FileFlagBackupSemantics = 0x02000000;

    public FolderInspection Inspect(string path)
    {
        if (KnownFolderService.IsNetworkPath(path))
            return Rejected(path, FolderDecision.NetworkPath);

        string lexicalPath;
        try { lexicalPath = KnownFolderService.NormalizePath(NormalizeExtendedPath(path)); }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or System.Security.SecurityException)
        { return Rejected(path, FolderDecision.NoWriteAccess); }

        var root = System.IO.Path.GetPathRoot(lexicalPath);
        try
        {
            if (root is not null && new System.IO.DriveInfo(root).DriveType == System.IO.DriveType.Network)
                return Rejected(lexicalPath, FolderDecision.NetworkPath);
        }
        catch (System.IO.IOException) { return Rejected(lexicalPath, FolderDecision.NoWriteAccess); }

        foreach (var ancestor in Ancestors(lexicalPath))
        {
            System.IO.FileAttributes attributes;
            try { attributes = System.IO.File.GetAttributes(ancestor); }
            catch (Exception exception) when (exception is UnauthorizedAccessException or System.IO.IOException)
            { return Rejected(lexicalPath, FolderDecision.NoWriteAccess); }
            if ((attributes & System.IO.FileAttributes.ReparsePoint) != 0)
                return Rejected(lexicalPath, FolderDecision.ReparsePoint);
        }

        var attributesForPath = System.IO.File.GetAttributes(lexicalPath);
        if ((attributesForPath & System.IO.FileAttributes.Directory) == 0)
            return Rejected(lexicalPath, FolderDecision.NoWriteAccess);
        if (!TryGetFinalPath(lexicalPath, out var canonicalPath))
            return Rejected(lexicalPath, FolderDecision.NoWriteAccess);

        var desktopIniStatus = ProbeDesktopIni(canonicalPath);
        return new FolderInspection(canonicalPath, null, System.IO.DriveType.Fixed, attributesForPath, desktopIniStatus, KnownFolderService.CanCreateFileIn(canonicalPath));
    }

    public IEnumerable<string> EnumerateDirectories(string path, System.IO.EnumerationOptions options) =>
        System.IO.Directory.EnumerateDirectories(path, "*", options);

    private static FolderInspection Rejected(string path, FolderDecision decision) =>
        new(path, decision, System.IO.DriveType.Unknown, 0, DesktopIniStatus.Indeterminate, false);

    private static IEnumerable<string> Ancestors(string path)
    {
        var current = new System.IO.DirectoryInfo(path);
        while (current is not null)
        {
            yield return current.FullName;
            current = current.Parent;
        }
    }

    private static DesktopIniStatus ProbeDesktopIni(string directory)
    {
        try
        {
            using var stream = new System.IO.FileStream(System.IO.Path.Combine(directory, "desktop.ini"), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite | System.IO.FileShare.Delete);
            return DesktopIniStatus.Present;
        }
        catch (System.IO.FileNotFoundException) { return DesktopIniStatus.Absent; }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.IO.IOException) { return DesktopIniStatus.Indeterminate; }
    }

    private static string NormalizeExtendedPath(string path) =>
        path.StartsWith(@"\\?\", StringComparison.Ordinal) ? path[4..] : path;

    private static bool TryGetFinalPath(string path, out string canonicalPath)
    {
        using var handle = CreateFile(path, FileReadAttributes, FileShareReadWriteDelete, IntPtr.Zero, OpenExisting, FileFlagBackupSemantics, IntPtr.Zero);
        if (handle.IsInvalid) { canonicalPath = string.Empty; return false; }
        var buffer = new StringBuilder(512);
        var length = GetFinalPathNameByHandle(handle, buffer, (uint)buffer.Capacity, 0);
        if (length == 0) { canonicalPath = string.Empty; return false; }
        if (length >= buffer.Capacity)
        {
            buffer.EnsureCapacity((int)length + 1);
            length = GetFinalPathNameByHandle(handle, buffer, (uint)buffer.Capacity, 0);
            if (length == 0) { canonicalPath = string.Empty; return false; }
        }
        canonicalPath = KnownFolderService.NormalizePath(NormalizeExtendedPath(buffer.ToString()));
        return true;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(string fileName, uint desiredAccess, uint shareMode, IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes, IntPtr templateFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern uint GetFinalPathNameByHandle(Microsoft.Win32.SafeHandles.SafeFileHandle file, StringBuilder path, uint length, uint flags);
}
```

## src/FolderThemeStudio.Core/SystemIntegration/MonitoredFolderIconService.cs

```csharp
using System.IO;

namespace FolderThemeStudio.Core.SystemIntegration;

public sealed record MonitoredApplyOutcome(bool Success, string Path, string? Error)
{
    public static MonitoredApplyOutcome Succeeded(string path) => new(true, path, null);
    public static MonitoredApplyOutcome Failure(string path, string error) => new(false, path, error);
}

public interface IMonitoredFolderIconService
{
    Task<MonitoredApplyOutcome> ApplyAsync(string folderPath, string icoPath, CancellationToken token);
}

public sealed class MonitoredFolderIconService : IMonitoredFolderIconService
{
    private readonly KnownFolderService policy = new();
    private readonly ICompatibleFolderFileSystem files = new WindowsCompatibleFolderFileSystem();

    public async Task<MonitoredApplyOutcome> ApplyAsync(string folderPath, string icoPath, CancellationToken token)
    {
        try
        {
            token.ThrowIfCancellationRequested();
            var canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(folderPath));
            if (policy.Evaluate(canonical) != FolderDecision.Allowed)
                return MonitoredApplyOutcome.Failure(canonical, "Folder is not an allowed ordinary folder.");
            var fullIco = Path.GetFullPath(icoPath);
            if (!File.Exists(fullIco)) return MonitoredApplyOutcome.Failure(canonical, "Monitoring icon asset is missing.");

            var ini = Path.Combine(canonical, "desktop.ini");
            var existed = files.FileExists(ini);
            var merge = DesktopIniDocument.Merge(existed ? files.ReadAllBytes(ini) : null, fullIco);
            if (!merge.Success || merge.Bytes is null)
                return MonitoredApplyOutcome.Failure(canonical, merge.Error ?? "Desktop.ini could not be merged.");

            if (existed)
            {
                await files.ReplaceDesktopIniAtomicallyAsync(ini, merge.Bytes).ConfigureAwait(false);
            }
            else
            {
                var text = System.Text.Encoding.UTF8.GetString(merge.Bytes);
                var created = await files.WriteDesktopIniAtomicallyAsync(ini, text).ConfigureAwait(false);
                if (!created.Created)
                {
                    merge = DesktopIniDocument.Merge(files.ReadAllBytes(ini), fullIco);
                    if (!merge.Success || merge.Bytes is null)
                        return MonitoredApplyOutcome.Failure(canonical, merge.Error ?? "Concurrent Desktop.ini could not be merged.");
                    await files.ReplaceDesktopIniAtomicallyAsync(ini, merge.Bytes).ConfigureAwait(false);
                }
            }

            files.SetAttributes(ini, files.GetAttributes(ini) | FileAttributes.Hidden | FileAttributes.System);
            files.SetAttributes(canonical, files.GetAttributes(canonical) | FileAttributes.ReadOnly);
            return MonitoredApplyOutcome.Succeeded(canonical);
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException or ArgumentException or InvalidOperationException)
        {
            return MonitoredApplyOutcome.Failure(folderPath, exception.Message);
        }
    }
}
```

## src/FolderThemeStudio.Core/SystemIntegration/OwnedFileDeletionService.cs

```csharp
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Win32.SafeHandles;

namespace FolderThemeStudio.Core.SystemIntegration;

internal enum OwnedFileDeletionOutcome
{
    Deleted,
    Missing,
    HashMismatch
}

internal interface IOwnedFileDeletionService
{
    OwnedFileDeletionOutcome DeleteIfHashMatches(string path, string expectedSha256);
}

internal sealed class WindowsOwnedFileDeletionService : IOwnedFileDeletionService
{
    private const uint GenericRead = 0x80000000;
    private const uint DeleteAccess = 0x00010000;
    private const uint FileShareRead = 0x00000001;
    private const uint OpenExisting = 3;
    private const int FileDispositionInfoClass = 4;
    private const int ErrorFileNotFound = 2;
    private const int ErrorPathNotFound = 3;

    public OwnedFileDeletionOutcome DeleteIfHashMatches(string path, string expectedSha256)
    {
        using var handle = CreateFile(
            path,
            GenericRead | DeleteAccess,
            FileShareRead,
            IntPtr.Zero,
            OpenExisting,
            0,
            IntPtr.Zero);
        if (handle.IsInvalid)
        {
            var error = Marshal.GetLastWin32Error();
            if (error is ErrorFileNotFound or ErrorPathNotFound)
            {
                return OwnedFileDeletionOutcome.Missing;
            }

            throw new IOException($"Could not open the owned Desktop.ini for verified deletion: {new Win32Exception(error).Message}", new Win32Exception(error));
        }

        using var stream = new FileStream(handle, FileAccess.Read);
        var actualSha256 = Convert.ToHexString(SHA256.HashData(stream));
        if (!string.Equals(actualSha256, expectedSha256, StringComparison.OrdinalIgnoreCase))
        {
            return OwnedFileDeletionOutcome.HashMismatch;
        }

        var disposition = new FileDispositionInfo { DeleteFile = true };
        if (!SetFileInformationByHandle(
            handle,
            FileDispositionInfoClass,
            ref disposition,
            (uint)Marshal.SizeOf<FileDispositionInfo>()))
        {
            var error = Marshal.GetLastWin32Error();
            throw new IOException($"Could not mark the verified Desktop.ini handle for deletion: {new Win32Exception(error).Message}", new Win32Exception(error));
        }

        return OwnedFileDeletionOutcome.Deleted;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct FileDispositionInfo
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool DeleteFile;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateFile(
        string fileName,
        uint desiredAccess,
        uint shareMode,
        IntPtr securityAttributes,
        uint creationDisposition,
        uint flagsAndAttributes,
        IntPtr templateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetFileInformationByHandle(
        SafeFileHandle file,
        int fileInformationClass,
        ref FileDispositionInfo fileInformation,
        uint bufferSize);
}
```

## src/FolderThemeStudio.Core/SystemIntegration/ShellRefreshService.cs

```csharp
using System.Runtime.InteropServices;

namespace FolderThemeStudio.Core.SystemIntegration;

public sealed record ShellRefreshResult(bool Attempted, bool Success, string? Error);

internal interface IShellChangeNotifier
{
    void NotifyAssociationChanged();
}

public sealed class ShellRefreshService
{
    private readonly IShellChangeNotifier notifier;

    public ShellRefreshService()
        : this(new WindowsShellChangeNotifier())
    {
    }

    internal static ShellRefreshService CreateForTesting(IShellChangeNotifier notifier) => new(notifier);

    private ShellRefreshService(IShellChangeNotifier notifier)
    {
        this.notifier = notifier;
    }

    public ShellRefreshResult RefreshIcons()
    {
        try
        {
            notifier.NotifyAssociationChanged();
            return new ShellRefreshResult(true, true, null);
        }
        catch (Exception exception) when (exception is ExternalException or DllNotFoundException or EntryPointNotFoundException or InvalidOperationException)
        {
            return new ShellRefreshResult(true, false, exception.Message);
        }
    }
}

internal sealed class WindowsShellChangeNotifier : IShellChangeNotifier
{
    private const uint ShcneAssocChanged = 0x08000000;
    private const uint ShcnfIdList = 0x0000;

    public void NotifyAssociationChanged() => SHChangeNotify(ShcneAssocChanged, ShcnfIdList, IntPtr.Zero, IntPtr.Zero);

    [DllImport("shell32.dll")]
    private static extern void SHChangeNotify(uint eventId, uint flags, IntPtr item1, IntPtr item2);
}
```

## src/FolderThemeStudio.Core/Themes/FolderTheme.cs

```csharp
namespace FolderThemeStudio.Core.Themes;

public enum MaterialStyle { Glass, Clay, FrostedGlass, Paper, Satin }
public enum StickerStyle { None, Star, Flower, Heart, Paw }

public static class StickerPlacement
{
    public const double MinimumCenter = 0.15;
    public const double MaximumCenter = 0.85;
    public const double MinimumScale = 0.15;
    public const double MaximumScale = 0.60;
    public const double MinimumRotation = -180;
    public const double MaximumRotation = 180;
}

public sealed record FolderTheme(
    int SchemaVersion,
    Guid Id,
    string Name,
    string GradientStart,
    string GradientEnd,
    double GradientAngle,
    double Opacity,
    double CornerRadius,
    string StrokeColor,
    double StrokeOpacity,
    double StrokeWidth,
    double HighlightStrength,
    string GlowColor,
    double GlowStrength,
    double GlowRadius,
    double ShadowStrength,
    double ShadowOffset,
    MaterialStyle Material = MaterialStyle.Glass,
    StickerStyle Sticker = StickerStyle.None,
    string StickerColor = "#FFFFFF",
    double StickerX = 0.5,
    double StickerY = 0.5,
    double StickerScale = 0.35,
    double StickerRotation = 0)
{
    public const int CurrentSchemaVersion = 2;

    public FolderTheme UpgradeV1() => SchemaVersion == 1
        ? this with
        {
            SchemaVersion = CurrentSchemaVersion,
            Material = MaterialStyle.Glass,
            Sticker = StickerStyle.None,
            StickerColor = "#FFFFFF",
            StickerX = 0.5,
            StickerY = 0.5,
            StickerScale = 0.35,
            StickerRotation = 0
        }
        : this;

    public static FolderTheme IceBlue { get; } = new(
        CurrentSchemaVersion, Guid.Parse("7fcb41ba-7c24-4914-9353-2f90be4bc4f0"), "玻璃霓虹 / 冰川蓝",
        "#65D9FF", "#478CFF", 135, .82, 16, "#B9F5FF", .90, 2,
        .58, "#43BFFF", .45, 8, .18, 6);
}

public sealed record ThemeValidationResult(IReadOnlyList<string> Errors)
{
    public bool IsValid => Errors.Count == 0;
}
```

## src/FolderThemeStudio.Core/Themes/ThemeStore.cs

```csharp
using System.IO;
using System.Text;
using System.Text.Json;

namespace FolderThemeStudio.Core.Themes;

public sealed record ThemeLoadResult(
    IReadOnlyList<FolderTheme> Themes,
    IReadOnlyList<string> Diagnostics);

public sealed record ThemeImportResult(
    FolderTheme? Theme,
    IReadOnlyList<string> Errors)
{
    public bool Success => Theme is not null && Errors.Count == 0;
}

public sealed class ThemeStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private readonly string themesDirectory;

    public ThemeStore(string? themesDirectory = null)
    {
        this.themesDirectory = Path.GetFullPath(themesDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio",
            "themes"));
    }

    public async Task<ThemeLoadResult> LoadAsync()
    {
        var themes = new List<FolderTheme> { FolderTheme.IceBlue };
        var diagnostics = new List<string>();
        var themeIds = new HashSet<Guid> { FolderTheme.IceBlue.Id };

        if (!Directory.Exists(themesDirectory))
        {
            return new ThemeLoadResult(themes, diagnostics);
        }

        try
        {
            foreach (var filePath in Directory.EnumerateFiles(themesDirectory, "*.json")
                         .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase))
            {
                var theme = await TryReadThemeAsync(filePath, diagnostics);

                if (theme is null)
                {
                    continue;
                }

                if (!themeIds.Add(theme.Id))
                {
                    diagnostics.Add($"Skipped '{Path.GetFileName(filePath)}' because its theme ID is already in use.");
                    continue;
                }

                themes.Add(theme);
            }
        }
        catch (IOException exception)
        {
            diagnostics.Add($"Could not enumerate stored themes: {exception.Message}");
        }
        catch (UnauthorizedAccessException exception)
        {
            diagnostics.Add($"Could not enumerate stored themes: {exception.Message}");
        }

        return new ThemeLoadResult(themes, diagnostics);
    }

    public async Task SaveAsync(FolderTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);
        ThemeValidator.ThrowIfInvalid(theme);

        if (theme.Id == FolderTheme.IceBlue.Id)
        {
            throw new InvalidOperationException("The built-in theme cannot be saved as a user theme.");
        }

        await WriteThemeAsync(theme, GetThemePath(theme.Id));
    }

    public Task DeleteAsync(Guid themeId)
    {
        if (themeId == FolderTheme.IceBlue.Id)
        {
            throw new InvalidOperationException("The built-in theme cannot be deleted.");
        }

        var themePath = GetThemePath(themeId);
        if (File.Exists(themePath))
        {
            File.Delete(themePath);
        }

        return Task.CompletedTask;
    }

    public async Task ExportAsync(Guid themeId, string destinationPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationPath);

        var theme = (await LoadAsync()).Themes.SingleOrDefault(candidate => candidate.Id == themeId)
            ?? throw new KeyNotFoundException("The requested theme was not found.");

        await WriteThemeAsync(theme, Path.GetFullPath(destinationPath));
    }

    public async Task<ThemeImportResult> ImportAsync(string sourcePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);

        var errors = new List<string>();
        var theme = await TryReadThemeAsync(Path.GetFullPath(sourcePath), errors);

        if (theme is null)
        {
            return new ThemeImportResult(null, errors);
        }

        var existingThemes = await LoadAsync();
        var existingIds = existingThemes.Themes.Select(candidate => candidate.Id).ToHashSet();

        if (existingIds.Contains(theme.Id))
        {
            do
            {
                theme = theme with { Id = Guid.NewGuid() };
            }
            while (existingIds.Contains(theme.Id));
        }

        await SaveAsync(theme);
        return new ThemeImportResult(theme, []);
    }

    private async Task<FolderTheme?> TryReadThemeAsync(string path, ICollection<string> diagnostics)
    {
        try
        {
            await using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 81920,
                FileOptions.Asynchronous | FileOptions.SequentialScan);
            var theme = await JsonSerializer.DeserializeAsync<FolderTheme>(stream, SerializerOptions);

            if (theme is null)
            {
                diagnostics.Add($"Skipped '{Path.GetFileName(path)}' because it does not contain a theme.");
                return null;
            }

            theme = theme.UpgradeV1();
            var validation = ThemeValidator.Validate(theme);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    diagnostics.Add(error);
                }
                return null;
            }

            return theme;
        }
        catch (JsonException exception)
        {
            diagnostics.Add($"Skipped '{Path.GetFileName(path)}' because it is not valid JSON: {exception.Message}");
        }
        catch (IOException exception)
        {
            diagnostics.Add($"Could not read '{Path.GetFileName(path)}': {exception.Message}");
        }
        catch (UnauthorizedAccessException exception)
        {
            diagnostics.Add($"Could not read '{Path.GetFileName(path)}': {exception.Message}");
        }

        return null;
    }

    private async Task WriteThemeAsync(FolderTheme theme, string destinationPath)
    {
        var destinationDirectory = Path.GetDirectoryName(destinationPath)
            ?? throw new ArgumentException("The destination path must include a directory.", nameof(destinationPath));
        Directory.CreateDirectory(destinationDirectory);

        var temporaryPath = $"{destinationPath}.{Guid.NewGuid():N}.tmp";

        try
        {
            await using (var stream = new FileStream(
                temporaryPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                await JsonSerializer.SerializeAsync(stream, theme, SerializerOptions);
                await stream.FlushAsync();
                stream.Flush(flushToDisk: true);
            }

            if (File.Exists(destinationPath))
            {
                File.Replace(temporaryPath, destinationPath, destinationBackupFileName: null);
            }
            else
            {
                File.Move(temporaryPath, destinationPath);
            }
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    private string GetThemePath(Guid themeId) =>
        Path.Combine(themesDirectory, $"{themeId:N}.json");
}
```

## src/FolderThemeStudio.Core/Themes/ThemeValidator.cs

```csharp
using System.Text.RegularExpressions;

namespace FolderThemeStudio.Core.Themes;

public static class ThemeValidator
{
    private static readonly Regex HexColorPattern = new(
        "^#[0-9A-Fa-f]{6}$",
        RegexOptions.CultureInvariant);

    public static ThemeValidationResult Validate(FolderTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        var errors = new List<string>();

        ValidateSchemaVersion(theme, errors);
        ValidateName(theme, errors);
        ValidateColors(theme, errors);
        ValidateAngle(theme, errors);
        ValidateUnitRangeProperties(theme, errors);
        ValidateGeometryProperties(theme, errors);
        ValidateAppearance(theme, errors);

        return new ThemeValidationResult(errors);
    }

    public static void ThrowIfInvalid(FolderTheme theme)
    {
        var result = Validate(theme);

        if (!result.IsValid)
        {
            throw new ArgumentException(
                $"The theme contains invalid properties: {string.Join(", ", result.Errors)}.",
                nameof(theme));
        }
    }

    private static void ValidateSchemaVersion(FolderTheme theme, ICollection<string> errors)
    {
        if (theme.SchemaVersion != FolderTheme.CurrentSchemaVersion)
        {
            errors.Add(nameof(FolderTheme.SchemaVersion));
        }
    }

    private static void ValidateName(FolderTheme theme, ICollection<string> errors)
    {
        if (string.IsNullOrWhiteSpace(theme.Name))
        {
            errors.Add(nameof(FolderTheme.Name));
        }
    }

    private static void ValidateColors(FolderTheme theme, ICollection<string> errors)
    {
        AddIfInvalidColor(theme.GradientStart, nameof(FolderTheme.GradientStart), errors);
        AddIfInvalidColor(theme.GradientEnd, nameof(FolderTheme.GradientEnd), errors);
        AddIfInvalidColor(theme.StrokeColor, nameof(FolderTheme.StrokeColor), errors);
        AddIfInvalidColor(theme.GlowColor, nameof(FolderTheme.GlowColor), errors);
        AddIfInvalidColor(theme.StickerColor, nameof(FolderTheme.StickerColor), errors);
    }

    private static void ValidateAngle(FolderTheme theme, ICollection<string> errors)
    {
        if (!IsInRange(theme.GradientAngle, 0, 360))
        {
            errors.Add(nameof(FolderTheme.GradientAngle));
        }
    }

    private static void ValidateUnitRangeProperties(FolderTheme theme, ICollection<string> errors)
    {
        AddIfOutsideUnitRange(theme.Opacity, nameof(FolderTheme.Opacity), errors);
        AddIfOutsideUnitRange(theme.StrokeOpacity, nameof(FolderTheme.StrokeOpacity), errors);
        AddIfOutsideUnitRange(theme.HighlightStrength, nameof(FolderTheme.HighlightStrength), errors);
        AddIfOutsideUnitRange(theme.GlowStrength, nameof(FolderTheme.GlowStrength), errors);
        AddIfOutsideUnitRange(theme.ShadowStrength, nameof(FolderTheme.ShadowStrength), errors);
    }

    private static void ValidateGeometryProperties(FolderTheme theme, ICollection<string> errors)
    {
        AddIfNegative(theme.CornerRadius, nameof(FolderTheme.CornerRadius), errors);
        AddIfNegative(theme.StrokeWidth, nameof(FolderTheme.StrokeWidth), errors);
        AddIfNegative(theme.GlowRadius, nameof(FolderTheme.GlowRadius), errors);
        AddIfNegative(theme.ShadowOffset, nameof(FolderTheme.ShadowOffset), errors);
    }

    private static void ValidateAppearance(FolderTheme theme, ICollection<string> errors)
    {
        if (!Enum.IsDefined(theme.Material)) errors.Add(nameof(FolderTheme.Material));
        if (!Enum.IsDefined(theme.Sticker)) errors.Add(nameof(FolderTheme.Sticker));
        if (!IsInRange(theme.StickerX, StickerPlacement.MinimumCenter, StickerPlacement.MaximumCenter))
            errors.Add(nameof(FolderTheme.StickerX));
        if (!IsInRange(theme.StickerY, StickerPlacement.MinimumCenter, StickerPlacement.MaximumCenter))
            errors.Add(nameof(FolderTheme.StickerY));
        if (!IsInRange(theme.StickerScale, StickerPlacement.MinimumScale, StickerPlacement.MaximumScale))
            errors.Add(nameof(FolderTheme.StickerScale));
        if (!IsInRange(theme.StickerRotation, StickerPlacement.MinimumRotation, StickerPlacement.MaximumRotation))
            errors.Add(nameof(FolderTheme.StickerRotation));
    }

    private static void AddIfInvalidColor(string? color, string propertyName, ICollection<string> errors)
    {
        if (color is null || !HexColorPattern.IsMatch(color))
        {
            errors.Add(propertyName);
        }
    }

    private static void AddIfOutsideUnitRange(double value, string propertyName, ICollection<string> errors)
    {
        if (!IsInRange(value, 0, 1))
        {
            errors.Add(propertyName);
        }
    }

    private static void AddIfNegative(double value, string propertyName, ICollection<string> errors)
    {
        if (!double.IsFinite(value) || value < 0)
        {
            errors.Add(propertyName);
        }
    }

    private static bool IsInRange(double value, double minimum, double maximum) =>
        double.IsFinite(value) && value >= minimum && value <= maximum;
}
```
