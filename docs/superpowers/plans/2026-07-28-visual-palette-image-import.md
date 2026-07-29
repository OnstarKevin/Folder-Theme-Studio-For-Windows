# Visual Palette and Image Icon Import Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a mouse-driven two-dimensional color palette to the built-in folder editor and a separate static-image icon mode that imports PNG/JPEG/BMP files and turns them into complete multi-size Windows folder icons.

**Architecture:** Introduce an explicit icon-source value carried by planning and artifact generation, while leaving global/compatible application and recovery unchanged. Extract testable color-coordinate math, add a reusable WPF visual picker, and isolate imported-image validation, normalization, persistence, and rendering in focused services. The main view model owns the two editor modes and invalidates reviewed plans whenever the selected source changes.

**Tech Stack:** .NET 8, C# 12, WPF, `BitmapDecoder`, `PngBitmapEncoder`, `RenderTargetBitmap`, xUnit, Inno Setup 6.

## Global Constraints

- Target Windows 10/11 x64 and remain framework-dependent on .NET 8 Desktop Runtime.
- Support only PNG, JPG/JPEG, and BMP imports; do not display or accept GIF.
- Imported images become the complete icon, preserve aspect ratio, are never cropped, and use transparent square padding.
- Keep built-in themes and imported images as separate modes; imported images do not enter the saved-theme catalog.
- Do not implement calculation-plan progress in this change.
- Preserve existing global/compatible folder filtering, backup, restore, and Shell refresh behavior.
- Use test-first red/green cycles and stage only intentional source files; existing user-created `desktop.ini` files are never modified or committed.

---

### Task 1: Explicit icon-source model through planning

**Files:**
- Create: `src/FolderThemeStudio.Core/Application/IconSource.cs`
- Modify: `src/FolderThemeStudio.Core/Application/ApplyModels.cs`
- Modify: `src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs`
- Modify: `src/FolderThemeStudio.App/ViewModels/MainViewModel.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/Application/ThemeApplicationServiceTests.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/TestSupport/ApplicationFixture.cs`

**Interfaces:**
- Produces: `IconSourceKind`, `IconSource.BuiltIn(FolderTheme)`, `IconSource.ImportedImage(string)`.
- Changes: `ApplyRequest.IconSource`, `ApplyPlan.IconSource`, and `IIconArtifactService.CreateAndVerifyAsync(IconSource, Guid, CancellationToken)`.
- Preserves: `ApplyPlan.Theme` as a compatibility convenience returning the built-in theme or `FolderTheme.IceBlue` for imported sources until all existing result text is source-aware.

- [ ] **Step 1: Write failing model and coordinator tests**

```csharp
[Fact]
public async Task Plan_ImportedImage_DoesNotRejectUnusedBuiltInThemeFields()
{
    var fixture = new ApplicationFixture();
    var source = IconSource.ImportedImage(@"C:\Imports\photo.png");
    var plan = await fixture.Service.PlanAsync(new ApplyRequest(source, ApplicationMode.Global), CancellationToken.None);
    Assert.True(plan.CanApply);
    Assert.Equal(source, plan.IconSource);
}

[Fact]
public async Task Apply_PassesReviewedIconSourceToArtifactService()
{
    var fixture = new ApplicationFixture();
    var source = IconSource.ImportedImage(@"C:\Imports\photo.png");
    await fixture.Service.ApplyAsync(new ApplyPlan(source, ApplicationMode.Global, [], []), fixture.Progress, CancellationToken.None);
    Assert.Equal(source, fixture.Icons.LastSource);
}
```

- [ ] **Step 2: Run the focused tests and verify RED**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~ThemeApplicationServiceTests"`

Expected: FAIL because `IconSource` and source-aware constructors do not exist.

- [ ] **Step 3: Implement the minimal source model and flow**

```csharp
public enum IconSourceKind { BuiltIn, ImportedImage }

public sealed record IconSource
{
    private IconSource(IconSourceKind kind, FolderTheme? theme, string? importedImagePath) =>
        (Kind, Theme, ImportedImagePath) = (kind, theme, importedImagePath);
    public IconSourceKind Kind { get; }
    public FolderTheme? Theme { get; }
    public string? ImportedImagePath { get; }
    public static IconSource BuiltIn(FolderTheme theme) => new(IconSourceKind.BuiltIn, theme, null);
    public static IconSource ImportedImage(string path) => new(IconSourceKind.ImportedImage, null, Path.GetFullPath(path));
}
```

Validate `FolderTheme` only for `BuiltIn`; require a non-empty normalized asset path for `ImportedImage`. Pass the exact reviewed source into artifact creation.

- [ ] **Step 4: Run focused tests and the existing application tests**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~ThemeApplicationServiceTests|FullyQualifiedName~MainViewModelTests"`

Expected: PASS with zero failures.

- [ ] **Step 5: Commit**

```powershell
git add src/FolderThemeStudio.Core/Application/IconSource.cs src/FolderThemeStudio.Core/Application/ApplyModels.cs src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs src/FolderThemeStudio.App/ViewModels/MainViewModel.cs tests/FolderThemeStudio.Core.Tests/Application/ThemeApplicationServiceTests.cs tests/FolderThemeStudio.Core.Tests/TestSupport/ApplicationFixture.cs
git commit -m "refactor: add explicit icon sources"
```

---

### Task 2: Testable two-dimensional color selection

**Files:**
- Create: `src/FolderThemeStudio.App/ViewModels/VisualColorMath.cs`
- Modify: `src/FolderThemeStudio.App/ViewModels/ColorPickerViewModel.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Application/VisualColorMathTests.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/Application/ColorPickerViewModelTests.cs`

**Interfaces:**
- Produces: `VisualColorMath.FromPalettePoint(double x, double y, double width, double height)` returning `(double Saturation, double Value)`.
- Produces: `VisualColorMath.HueFromPoint(double y, double height)` and `ColorPickerViewModel.SetPalettePoint(...)` / `SetHuePoint(...)`.

- [ ] **Step 1: Write failing coordinate tests with hand-derived values**

```csharp
[Theory]
[InlineData(0, 0, 200, 100, 0, 1)]
[InlineData(100, 50, 200, 100, 0.5, 0.5)]
[InlineData(200, 100, 200, 100, 1, 0)]
public void FromPalettePoint_MapsAndClampsCoordinates(
    double x, double y, double width, double height, double expectedSaturation, double expectedValue)
{
    var actual = VisualColorMath.FromPalettePoint(x, y, width, height);
    Assert.Equal(expectedSaturation, actual.Saturation, 6);
    Assert.Equal(expectedValue, actual.Value, 6);
}
```

Add zero-size tests expecting `ArgumentOutOfRangeException`, and verify a midpoint update changes `Hex` and `SelectedBrush`.

- [ ] **Step 2: Run focused tests and verify RED**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~VisualColorMathTests|FullyQualifiedName~ColorPickerViewModelTests"`

Expected: FAIL because visual coordinate APIs are missing.

- [ ] **Step 3: Implement coordinate conversion and view-model commands**

```csharp
internal static (double Saturation, double Value) FromPalettePoint(double x, double y, double width, double height) =>
    width <= 0 || height <= 0
        ? throw new ArgumentOutOfRangeException(nameof(width), "Palette dimensions must be positive.")
        : (Math.Clamp(x / width, 0, 1), 1 - Math.Clamp(y / height, 0, 1));
```

Keep HSV/RGB conversion centralized in `ColorPickerViewModel`; visual inputs update the same `Hue`, `Saturation`, and `Value` properties as hexadecimal input.

- [ ] **Step 4: Run focused tests**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~VisualColorMathTests|FullyQualifiedName~ColorPickerViewModelTests"`

Expected: PASS with zero failures.

- [ ] **Step 5: Commit**

```powershell
git add src/FolderThemeStudio.App/ViewModels/VisualColorMath.cs src/FolderThemeStudio.App/ViewModels/ColorPickerViewModel.cs tests/FolderThemeStudio.Core.Tests/Application/VisualColorMathTests.cs tests/FolderThemeStudio.Core.Tests/Application/ColorPickerViewModelTests.cs
git commit -m "feat: add visual color coordinate model"
```

---

### Task 3: Imported image validation, persistence, and rendering

**Files:**
- Create: `src/FolderThemeStudio.Core/Rendering/ImportedImageService.cs`
- Create: `src/FolderThemeStudio.Core/Rendering/ImportedIconRenderer.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Rendering/ImportedImageServiceTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Rendering/ImportedIconRendererTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Rendering/TestImageFiles.cs`

**Interfaces:**
- Produces: `Task<ImportedImageResult> ImportedImageService.ImportAsync(string sourcePath, CancellationToken)`.
- Produces: `IImportedImageService` with the same `ImportAsync` signature so the main view model can use a deterministic test double.
- Produces: `BitmapSource ImportedIconRenderer.Render(string normalizedPngPath, int size)`.
- `ImportedImageResult` contains `Success`, `AssetPath`, `Preview`, and `Error`.
- Test utility: `TestImageFiles.Write(string fileName, int width, int height, string directory)` writes a real encoder-backed PNG, JPEG, or BMP fixture; `AssertTransparent` and `AssertOpaque` read literal BGRA pixels from the rendered bitmap.

- [ ] **Step 1: Write failing real-file import and layout tests**

```csharp
[Theory]
[InlineData("sample.png")]
[InlineData("sample.jpg")]
[InlineData("sample.bmp")]
public async Task ImportAsync_SupportedImage_NormalizesToPersistentPng(string fileName)
{
    using var temp = new TemporaryDirectory();
    var source = TestImageFiles.Write(fileName, width: 80, height: 40, temp.Path);
    var result = await new ImportedImageService(Path.Combine(temp.Path, "imports"))
        .ImportAsync(source, CancellationToken.None);
    Assert.True(result.Success);
    Assert.EndsWith(".png", result.AssetPath, StringComparison.OrdinalIgnoreCase);
    Assert.True(File.Exists(result.AssetPath));
}

[Fact]
public void Render_WideImage_IsCenteredWithoutCropping()
{
    var bitmap = ImportedIconRenderer.Render(wideTransparentFixture, 64);
    Assert.Equal(64, bitmap.PixelWidth);
    AssertTransparent(bitmap, x: 32, y: 4);
    AssertOpaque(bitmap, x: 32, y: 32);
}
```

Add failures for GIF content, mismatched extension/content, corrupt data, and import failure preserving the previous selection at the view-model boundary.

- [ ] **Step 2: Run rendering tests and verify RED**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~ImportedImage|FullyQualifiedName~ImportedIconRenderer"`

Expected: FAIL because services do not exist.

- [ ] **Step 3: Implement signature detection and normalized persistence**

Recognize literal signatures: PNG `89 50 4E 47 0D 0A 1A 0A`, JPEG `FF D8 FF`, BMP `42 4D`. Require the extension to match the detected format. Decode with `BitmapCacheOption.OnLoad`, convert to `Pbgra32`, encode as PNG, write to a temporary file, atomically move to a SHA-256-named asset, then reopen it before returning success.

- [ ] **Step 4: Implement aspect-fit rendering**

```csharp
var scale = Math.Min(size / (double)source.PixelWidth, size / (double)source.PixelHeight);
var width = source.PixelWidth * scale;
var height = source.PixelHeight * scale;
var x = (size - width) / 2;
var y = (size - height) / 2;
context.DrawImage(source, new Rect(x, y, width, height));
```

Render onto a transparent `RenderTargetBitmap` using high-quality scaling and freeze the result.

- [ ] **Step 5: Run focused rendering tests**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~ImportedImage|FullyQualifiedName~ImportedIconRenderer"`

Expected: PASS with zero failures.

- [ ] **Step 6: Commit**

```powershell
git add src/FolderThemeStudio.Core/Rendering/ImportedImageService.cs src/FolderThemeStudio.Core/Rendering/ImportedIconRenderer.cs tests/FolderThemeStudio.Core.Tests/Rendering/ImportedImageServiceTests.cs tests/FolderThemeStudio.Core.Tests/Rendering/ImportedIconRendererTests.cs tests/FolderThemeStudio.Core.Tests/Rendering/TestImageFiles.cs
git commit -m "feat: import static images as icon assets"
```

---

### Task 4: Source-aware ICO generation

**Files:**
- Modify: `src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/Application/ThemeApplicationServiceTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Application/IconArtifactServiceTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/TestSupport/IcoTestReader.cs`

**Interfaces:**
- Consumes: `IconSource` from Task 1 and `ImportedIconRenderer.Render` from Task 3.
- Produces: verified ICO output for all `FolderIconRenderer.RequiredSizes` from either source.
- Test utility: `IcoTestReader.ReadSizes(string icoPath)` parses the ICO directory entries without using production verification logic.

- [ ] **Step 1: Write failing imported-source artifact tests**

```csharp
[Fact]
public async Task CreateAndVerifyAsync_ImportedImage_WritesEveryRequiredFrame()
{
    using var temp = new TemporaryDirectory();
    var service = new DefaultIconArtifactService(temp.Path);
    var result = await service.CreateAndVerifyAsync(
        IconSource.ImportedImage(normalizedPng), Guid.NewGuid(), CancellationToken.None);
    Assert.True(result.IsSuccess);
    Assert.Equal(FolderIconRenderer.RequiredSizes, IcoTestReader.ReadSizes(result.Path!));
}
```

- [ ] **Step 2: Run artifact tests and verify RED**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~IconArtifactServiceTests"`

Expected: FAIL because the artifact service still accepts only `FolderTheme`.

- [ ] **Step 3: Select the renderer by source kind**

```csharp
var bitmap = source.Kind switch
{
    IconSourceKind.BuiltIn => FolderIconRenderer.Render(source.Theme!, size),
    IconSourceKind.ImportedImage => ImportedIconRenderer.Render(source.ImportedImagePath!, size),
    _ => throw new ArgumentOutOfRangeException(nameof(source))
};
```

Reuse existing PNG frame encoding, `IcoEncoder`, persisted-byte verification, and atomic output behavior.

- [ ] **Step 4: Run artifact and application tests**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~IconArtifactServiceTests|FullyQualifiedName~ThemeApplicationServiceTests"`

Expected: PASS with zero failures.

- [ ] **Step 5: Commit**

```powershell
git add src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs tests/FolderThemeStudio.Core.Tests/Application/ThemeApplicationServiceTests.cs tests/FolderThemeStudio.Core.Tests/Application/IconArtifactServiceTests.cs tests/FolderThemeStudio.Core.Tests/TestSupport/IcoTestReader.cs
git commit -m "feat: generate ico from either icon source"
```

---

### Task 5: Main-window dual editor and visual picker control

**Files:**
- Create: `src/FolderThemeStudio.App/Controls/VisualColorPicker.xaml`
- Create: `src/FolderThemeStudio.App/Controls/VisualColorPicker.xaml.cs`
- Create: `src/FolderThemeStudio.App/ViewModels/PaletteEditorViewModel.cs`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml.cs`
- Modify: `src/FolderThemeStudio.App/App.xaml.cs`
- Modify: `src/FolderThemeStudio.App/ViewModels/MainViewModel.cs`
- Modify: `src/FolderThemeStudio.App/Services/DialogService.cs`
- Modify: `src/FolderThemeStudio.App/SettingsWindow.xaml`
- Modify: `src/FolderThemeStudio.App/SettingsWindow.xaml.cs`
- Modify: `src/FolderThemeStudio.App/ViewModels/SettingsViewModel.cs`
- Modify: `src/FolderThemeStudio.App/Localization/Strings.zh-CN.xaml`
- Modify: `src/FolderThemeStudio.App/Localization/Strings.en-US.xaml`
- Modify: `tests/FolderThemeStudio.Core.Tests/Application/MainViewModelTests.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/Application/MainWindowTests.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/TestSupport/MainViewModelFixture.cs`

**Interfaces:**
- Produces: `IconEditorMode.BuiltIn`, `IconEditorMode.ImportedImage`, `SelectedIconEditorMode`, `ImportedImagePreview`, `ChooseImageCommand`, and `ClearImageCommand`.
- Produces: `PaletteEditorViewModel.Roles`, `SelectedRole`, and `PaletteChanged`, initialized from `FolderPalette` and shared by the main editor only.
- Adds: `IMainViewModelDialogs.PickImageImportPathAsync()` filtering `*.png;*.jpg;*.jpeg;*.bmp` only.
- Consumes: visual coordinate APIs from Task 2 and `ImportedImageService` from Task 3.
- Changes: `IPreviewRenderer.Render(IconSource source, int size)` so imported previews and final artifacts use the same renderer choice.

- [ ] **Step 1: Write failing view-model behavior tests**

```csharp
[Fact]
public async Task ChooseImage_SuccessSwitchesSourceAndInvalidatesReviewedPlan()
{
    var fixture = MainViewModelFixture.Create();
    await fixture.PlanCompatibleAsync();
    fixture.Dialogs.ImageImportPath = fixture.ValidPngPath;
    fixture.ViewModel.ChooseImageCommand.Execute(null);
    await fixture.ViewModel.WaitForImageImportAsync();
    Assert.Equal(IconEditorMode.ImportedImage, fixture.ViewModel.SelectedIconEditorMode);
    Assert.NotNull(fixture.ViewModel.ImportedImagePreview);
    Assert.False(fixture.ViewModel.CanApply);
}

[Fact]
public async Task ChooseImage_FailureKeepsPreviousValidImage()
{
    var fixture = MainViewModelFixture.WithImportedImage();
    var previous = fixture.ViewModel.ImportedImageAssetPath;
    fixture.Images.NextResult = ImportedImageResult.Failure("Invalid image");
    await fixture.ViewModel.ChooseImageAsync();
    Assert.Equal(previous, fixture.ViewModel.ImportedImageAssetPath);
}
```

Add tests that built-in color changes invalidate a plan and imported mode without an image cannot plan.

- [ ] **Step 2: Run main-window tests and verify RED**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~MainViewModelTests|FullyQualifiedName~MainWindowTests"`

Expected: FAIL because dual-mode properties and commands are missing.

- [ ] **Step 3: Implement the visual picker control**

Use a square with a white-to-current-hue horizontal gradient and transparent-to-black vertical overlay, a vertical rainbow hue strip, and Canvas-positioned circular indicators. Handle `PreviewMouseDown` and `PreviewMouseMove` while the left button is pressed; map positions through Task 2 APIs. Bind to the selected color role's `ColorPickerViewModel`.

- [ ] **Step 4: Implement source-mode view-model behavior**

Build `CurrentIconSource` from `CurrentTheme` in built-in mode or the persisted imported asset in image mode. Successful import switches to image mode; failure leaves all prior state untouched. Source changes call the same `InvalidatePlan()` path used by folder roots and theme changes. Extract the four existing color roles from `SettingsViewModel` into `PaletteEditorViewModel`; subscribe once to `PaletteChanged` so the main theme fields and preview update without duplicated color state.

- [ ] **Step 5: Restructure the main window and simplify settings**

Add two prominent mode tabs/buttons above the editor. The built-in panel contains role selection, `VisualColorPicker`, hexadecimal auxiliary input, existing parameter controls, and previews. The imported panel contains Select/Replace/Clear buttons and a 256-pixel final-layout preview. Remove the palette tab from `SettingsWindow`; retain language controls and settings persistence.

- [ ] **Step 6: Add bilingual UI strings**

Add exact resources for `IconMode.BuiltIn`, `IconMode.ImportedImage`, `Image.Select`, `Image.Replace`, `Image.Clear`, `Image.Empty`, `Image.Unsupported`, and visual-picker accessibility names in both language dictionaries.

- [ ] **Step 7: Run UI/view-model tests**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~MainViewModelTests|FullyQualifiedName~MainWindowTests|FullyQualifiedName~LocalizationServiceTests"`

Expected: PASS with zero failures.

- [ ] **Step 8: Commit**

```powershell
git add src/FolderThemeStudio.App/Controls src/FolderThemeStudio.App/App.xaml.cs src/FolderThemeStudio.App/MainWindow.xaml src/FolderThemeStudio.App/MainWindow.xaml.cs src/FolderThemeStudio.App/ViewModels/MainViewModel.cs src/FolderThemeStudio.App/ViewModels/PaletteEditorViewModel.cs src/FolderThemeStudio.App/Services/DialogService.cs src/FolderThemeStudio.App/SettingsWindow.xaml src/FolderThemeStudio.App/SettingsWindow.xaml.cs src/FolderThemeStudio.App/ViewModels/SettingsViewModel.cs src/FolderThemeStudio.App/Localization tests/FolderThemeStudio.Core.Tests/Application/MainViewModelTests.cs tests/FolderThemeStudio.Core.Tests/Application/MainWindowTests.cs tests/FolderThemeStudio.Core.Tests/TestSupport/MainViewModelFixture.cs
git commit -m "feat: add dual icon editor with visual palette"
```

---

### Task 6: Beta 5 documentation, full verification, and release artifacts

**Files:**
- Modify: `src/FolderThemeStudio.App/FolderThemeStudio.App.csproj`
- Modify: `build/FolderThemeStudio.iss`
- Modify: `build/Package-Release.ps1`
- Modify: `README.md`
- Modify: `docs/USER-GUIDE.zh-CN.md`
- Modify: `docs/USER-GUIDE.en-US.md`
- Create: `docs/releases/v0.1.0-beta.5.md`
- Modify: `tests/FolderThemeStudio.Core.Tests/Application/ReleaseConfigurationTests.cs`

**Interfaces:**
- Produces: `FolderThemeStudio-v0.1.0-beta.5-setup.exe`, setup SHA-256, clean source ZIP, and source SHA-256.
- Sets product version `0.1.0-beta.5` and file version `0.1.0.5`.

- [ ] **Step 1: Write failing release configuration assertions**

```csharp
Assert.StartsWith("0.1.0-beta.5", version.ProductVersion);
Assert.Equal("0.1.0.5", version.FileVersion);
Assert.Contains("v0.1.0-beta.5", packageScript);
```

- [ ] **Step 2: Run release tests and verify RED**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~ReleaseConfigurationTests"`

Expected: FAIL while project metadata remains Beta 4.

- [ ] **Step 3: Update version and bilingual documentation**

Document the mouse-driven palette, PNG/JPEG/BMP full-icon import, aspect-fit behavior, absence of GIF support, and the requirement to recalculate a plan after changing source. Update installer defaults and package names to Beta 5. In `Package-Release.ps1`, skip any source file whose name equals `desktop.ini` (case-insensitive) in addition to the existing excluded path segments, so Windows icon metadata can never enter the source archive.

- [ ] **Step 4: Run the complete test suite**

Run: `dotnet test FolderThemeStudio.sln -c Release`

Expected: PASS with zero failed tests.

- [ ] **Step 5: Build and verify release artifacts**

Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\build\Package-Release.ps1 -Version v0.1.0-beta.5 -DotNetPath C:\tmp\folder-theme-dotnet8\dotnet.exe -InnoCompilerPath "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"
```

Verify the setup is below 25 MB, the source ZIP contains no `.git`, `.worktrees`, `.superpowers`, `artifacts`, `bin`, `obj`, `TestResults`, or `desktop.ini` entries, and both checksum files match freshly computed SHA-256 values.

- [ ] **Step 6: Commit release metadata**

```powershell
git add src/FolderThemeStudio.App/FolderThemeStudio.App.csproj build/FolderThemeStudio.iss build/Package-Release.ps1 README.md docs/USER-GUIDE.zh-CN.md docs/USER-GUIDE.en-US.md docs/releases/v0.1.0-beta.5.md tests/FolderThemeStudio.Core.Tests/Application/ReleaseConfigurationTests.cs
git commit -m "release: prepare Folder Theme Studio beta5"
```

---

## Final Verification Checklist

- [ ] Built-in mode exposes a visible 2D saturation/value palette and hue strip, with mouse click/drag selection.
- [ ] Imported-image mode is visually separate and accepts only PNG, JPEG, and BMP.
- [ ] Wide and tall images remain complete, centered, and transparently padded in every ICO frame.
- [ ] Switching mode, changing color, replacing, or clearing an image invalidates the reviewed plan.
- [ ] Existing global/compatible filtering, recovery, and restore tests remain green.
- [ ] Full suite, Release build, installer validation, source archive validation, and hashes pass from fresh commands.
