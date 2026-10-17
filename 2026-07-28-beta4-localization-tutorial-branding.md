# Folder Theme Studio Beta4 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ship an upload-ready Beta4 with Chinese/English UI, a first-run tutorial, top help/settings actions, a visual folder palette editor, and consistent new branding.

**Architecture:** Keep the existing WPF/core split. Add small app-layer settings and localization services, bind XAML through dynamic resources, and let dedicated settings/tutorial view models update the existing `MainViewModel`. Generate branding from one checked-in vector specification and reuse the resulting ICO/PNG everywhere.

**Tech Stack:** .NET 8, WPF/XAML, System.Text.Json, xUnit, PowerShell, Inno Setup 6.

## Global Constraints

- Release version is `v0.1.0-beta.4`; assembly version remains `0.1.0.0`; file version is `0.1.0.4`.
- Languages are Simplified Chinese (`zh-CN`, default) and English (`en-US`), switchable at runtime.
- The palette controls only folder gradient start/end, stroke, and glow colors.
- Existing registry, recovery, folder ownership, shell refresh, and runtime-download protections remain unchanged.
- Deliver setup EXE, setup SHA-256, source ZIP, source SHA-256, bilingual docs, and release notes; do not push or create a GitHub Release.

---

### Task 1: Persistent settings and runtime localization

**Files:**
- Create: `src/FolderThemeStudio.App/Settings/AppSettings.cs`
- Create: `src/FolderThemeStudio.App/Settings/JsonAppSettingsStore.cs`
- Create: `src/FolderThemeStudio.App/Localization/LocalizationService.cs`
- Create: `src/FolderThemeStudio.App/Localization/Strings.en-US.xaml`
- Create: `src/FolderThemeStudio.App/Localization/Strings.zh-CN.xaml`
- Modify: `src/FolderThemeStudio.App/App.xaml`
- Modify: `src/FolderThemeStudio.App/App.xaml.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/AppSettingsTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/LocalizationServiceTests.cs`

**Interfaces:**
- Produces: `FolderPalette(string GradientStart, string GradientEnd, string Stroke, string Glow)`.
- Produces: `AppSettings(string Language, bool TutorialCompleted, FolderPalette Palette, IReadOnlyList<string> RecentColors)`.
- Produces: `IAppSettingsStore.LoadAsync()` and `SaveAsync(AppSettings)`.
- Produces: `ILocalizationService.CurrentLanguage`, `ApplyLanguage(string)`, and `Get(string, params object[])`.

- [ ] **Step 1: Write failing settings tests**

Assert missing/corrupt JSON returns `AppSettings.Default`, valid settings round-trip under an injected temporary path, language is limited to `zh-CN`/`en-US`, and palette colors are normalized `#RRGGBB` values.

- [ ] **Step 2: Run the focused tests and confirm failure**

Run: `dotnet test FolderThemeStudio.sln -c Release --filter "FullyQualifiedName~AppSettingsTests"`
Expected: FAIL because settings types do not exist.

- [ ] **Step 3: Implement the settings records and store**

Use `%LOCALAPPDATA%\FolderThemeStudio\settings.json`, `System.Text.Json`, a same-directory temporary file plus `File.Move(..., overwrite: true)`, and defaults `zh-CN`, tutorial incomplete, IceBlue palette, empty recent colors. Keep at most 12 distinct recent colors.

- [ ] **Step 4: Write and run failing localization tests**

Assert both dictionaries contain identical keys, `ApplyLanguage("en-US")` changes `CurrentLanguage`, invalid language falls back to `zh-CN`, and `Get("App.Title")` returns the selected translation.

Run: `dotnet test FolderThemeStudio.sln -c Release --filter "FullyQualifiedName~LocalizationServiceTests"`
Expected: FAIL because localization service and dictionaries do not exist.

- [ ] **Step 5: Implement runtime localization**

Load exactly one language dictionary into `Application.Resources.MergedDictionaries`; expose `Get` for view-model text and use `{DynamicResource <key>}` for XAML. Include every visible label, accessibility name, dialog title, status, validation, result label, tutorial string, and settings string in both dictionaries.

- [ ] **Step 6: Run focused and full tests, then commit**

Run: `dotnet test FolderThemeStudio.sln -c Release`
Expected: all tests pass.

Commit: `feat: add persistent bilingual app settings`

---

### Task 2: Visual palette settings window

**Files:**
- Create: `src/FolderThemeStudio.App/ViewModels/ColorPickerViewModel.cs`
- Create: `src/FolderThemeStudio.App/ViewModels/SettingsViewModel.cs`
- Create: `src/FolderThemeStudio.App/SettingsWindow.xaml`
- Create: `src/FolderThemeStudio.App/SettingsWindow.xaml.cs`
- Modify: `src/FolderThemeStudio.App/ViewModels/MainViewModel.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/ColorPickerViewModelTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/SettingsViewModelTests.cs`

**Interfaces:**
- Consumes: `AppSettings`, `FolderPalette`, `IAppSettingsStore`, `ILocalizationService`.
- Produces: `ColorPickerViewModel.Hue`, `Saturation`, `Value`, `Hex`, `SelectedBrush`, and `TrySetHex(string)`.
- Produces: `SettingsViewModel.SaveAsync()` returning a validated `AppSettings` and `MainViewModel.ApplyPalette(FolderPalette)`.

- [ ] **Step 1: Write failing HSV/hex tests**

Cover red `(0,1,1) -> #FF0000`, cyan `(180,1,1) -> #00FFFF`, lowercase/three-digit hex normalization, invalid input rejection, and round-trip tolerance of one RGB unit.

- [ ] **Step 2: Run tests and confirm failure**

Run: `dotnet test FolderThemeStudio.sln -c Release --filter "FullyQualifiedName~ColorPickerViewModelTests"`
Expected: FAIL because the picker does not exist.

- [ ] **Step 3: Implement the picker and settings view model**

Implement RGB/HSV conversion without a new package. Maintain four roles (`GradientStart`, `GradientEnd`, `Stroke`, `Glow`), six recommended palettes, at most 12 recent colors, a preview `FolderTheme`, localized validation, save/cancel isolation, and language switching through `ILocalizationService`.

- [ ] **Step 4: Write settings behavior tests**

Assert Cancel leaves `MainViewModel` and disk unchanged; Save persists language/palette, adds recent colors, and applies all four colors; invalid hex disables Save; changing a picker value updates the preview theme.

- [ ] **Step 5: Build the settings UI**

Create tabs “常规/General” and “图标调色板/Icon Palette”. The palette tab has role selectors on the left, HSV sliders and hex input in the center, live 256px folder preview plus recommended/recent swatches on the right, and localized Save/Cancel buttons. Bind preview rendering to `FolderIconRenderer` via the existing `IPreviewRenderer` abstraction.

- [ ] **Step 6: Verify and commit**

Run: `dotnet test FolderThemeStudio.sln -c Release`
Expected: all tests pass.

Commit: `feat: add visual folder palette settings`

---

### Task 3: Top bar and first-run tutorial

**Files:**
- Create: `src/FolderThemeStudio.App/ViewModels/TutorialViewModel.cs`
- Create: `src/FolderThemeStudio.App/TutorialWindow.xaml`
- Create: `src/FolderThemeStudio.App/TutorialWindow.xaml.cs`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml.cs`
- Modify: `src/FolderThemeStudio.App/App.xaml.cs`
- Modify: `src/FolderThemeStudio.App/ViewModels/MainViewModel.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/TutorialViewModelTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/MainWindowTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/MainWindowAccessibilityTests.cs`

**Interfaces:**
- Consumes: current `AppSettings`, stores tutorial completion through `IAppSettingsStore`.
- Produces: `TutorialViewModel.StepIndex`, `StepCount`, `NextCommand`, `PreviousCommand`, `SkipCommand`, `FinishCommand`.
- Produces: top-level handlers `OpenTutorial` and `OpenSettings`.

- [ ] **Step 1: Write failing tutorial tests**

Assert five localized steps, bounded previous/next navigation, Skip and Finish both persist `TutorialCompleted=true`, and manual reopening does not reset other settings.

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test FolderThemeStudio.sln -c Release --filter "FullyQualifiedName~TutorialViewModelTests"`
Expected: FAIL because tutorial types do not exist.

- [ ] **Step 3: Implement tutorial behavior and window**

Use five steps: choose theme, tune palette, inspect preview, apply safely, restore backup. Show automatically after `MainWindow` initialization only when `TutorialCompleted` is false; the Help button always reopens it.

- [ ] **Step 4: Replace the main header and localize the full main window**

Use a two-column top bar: Logo plus `文件夹主题工坊` and `Folder Theme Studio` on the left; localized Tutorial and Settings buttons on the right. Replace all hardcoded visible/accessibility text and all view-model-created status/dialog strings with localization keys. Preserve responsive stacking below 900px.

- [ ] **Step 5: Extend real-window and accessibility tests**

Show the actual window on STA, assert it reaches `ApplicationIdle`, locate top buttons by automation name, switch language and verify title/button resources update, and confirm existing progress binding remains OneWay.

- [ ] **Step 6: Verify and commit**

Run: `dotnet test FolderThemeStudio.sln -c Release`
Expected: all tests pass and the real hidden WPF window opens without binding errors.

Commit: `feat: add localized tutorial and top navigation`

---

### Task 4: Folder magic-wand branding

**Files:**
- Create: `assets/brand/folder-theme-studio-logo.svg`
- Create: `tools/FolderThemeStudio.BrandAssets/FolderThemeStudio.BrandAssets.csproj`
- Create: `tools/FolderThemeStudio.BrandAssets/Program.cs`
- Create: `src/FolderThemeStudio.App/Assets/FolderThemeStudio.png`
- Replace: `src/FolderThemeStudio.App/Assets/FolderThemeStudio.ico`
- Modify: `src/FolderThemeStudio.App/FolderThemeStudio.App.csproj`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml`
- Modify: `build/FolderThemeStudio.iss`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/BrandAssetTests.cs`

**Interfaces:**
- Produces: one SVG source and deterministic 16/20/24/32/48/64/128/256 PNG frames plus multi-frame ICO.

- [ ] **Step 1: Write failing asset tests**

Assert the SVG contains the folder outline, wand line, and star paths; PNG is 256x256; ICO contains all eight required sizes; project, window, and installer reference the same branded assets.

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test FolderThemeStudio.sln -c Release --filter "FullyQualifiedName~BrandAssetTests"`
Expected: FAIL because the new source/assets are absent.

- [ ] **Step 3: Implement deterministic asset generation**

Draw a rounded blue-violet folder outline, diagonal magic wand, and two four-point stars on transparent background using WPF drawing primitives. Reuse `IcoEncoder` for the eight ICO frames. Check in the SVG source and generated 256px PNG/ICO; do not add a runtime dependency.

- [ ] **Step 4: Apply assets consistently**

Set WPF window/taskbar icon and `<ApplicationIcon>` to the new ICO, display the PNG in the top bar, and keep `SetupIconFile` on the same ICO. Add the PNG to README at repository-relative path.

- [ ] **Step 5: Verify and commit**

Run: `dotnet run --project tools/FolderThemeStudio.BrandAssets/FolderThemeStudio.BrandAssets.csproj`
Run: `dotnet test FolderThemeStudio.sln -c Release`
Expected: deterministic assets and all tests pass.

Commit: `feat: apply folder magic-wand branding`

---

### Task 5: Beta4 documentation, installer localization, and artifacts

**Files:**
- Modify: `src/FolderThemeStudio.App/FolderThemeStudio.App.csproj`
- Modify: `build/FolderThemeStudio.iss`
- Modify: `build/Package-Release.ps1`
- Modify: `build/Verify-Release.ps1`
- Modify: `README.md`
- Create: `docs/USER-GUIDE.zh-CN.md`
- Create: `docs/USER-GUIDE.en-US.md`
- Create: `docs/releases/v0.1.0-beta.4.md`
- Modify: `docs/manual-test-checklist.md`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/ReleaseConfigurationTests.cs`

**Interfaces:**
- Produces: `FolderThemeStudio-v0.1.0-beta.4-setup.exe`, setup `.sha256`, source `.zip`, and source `.sha256` in `artifacts/`.

- [ ] **Step 1: Write failing release configuration tests**

Assert project version `0.1.0-beta.4`, file version `0.1.0.4`, framework-dependent single-file settings, bilingual Inno languages/tasks, new icon reference, Beta4 package defaults, and creation of both setup/source checksums.

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test FolderThemeStudio.sln -c Release --filter "FullyQualifiedName~ReleaseConfigurationTests"`
Expected: FAIL while files still identify Beta3.

- [ ] **Step 3: Update version and installer**

Change version metadata to Beta4. Add Inno `english` and `chinesesimplified` languages with localized desktop shortcut/runtime download messages while preserving pinned .NET 8.0.29 URL/SHA-256 and current runtime detection.

- [ ] **Step 4: Produce bilingual user documentation**

Document installation, first-run tutorial, language switching, four-role palette editing, apply modes, restore, troubleshooting, unsigned beta warning, and GitHub artifact verification. Update README links and manual checklist for both languages, palette persistence, first-run state, high DPI, and real install/uninstall.

- [ ] **Step 5: Extend packaging for a clean source archive**

Create a deterministic source staging directory under `artifacts`, excluding `.git`, `.worktrees`, `.superpowers`, `artifacts`, `bin`, and `obj`; zip it, hash both archives, and validate setup `<25 MiB`, published EXE `<30 MiB`, source ZIP opens, required docs exist, and no excluded paths are present.

- [ ] **Step 6: Run complete verification**

Run: `dotnet restore FolderThemeStudio.sln`
Run: `dotnet test FolderThemeStudio.sln -c Release`
Run: `powershell -NoProfile -ExecutionPolicy Bypass -File .\build\Package-Release.ps1 -Version v0.1.0-beta.4 -InnoCompilerPath "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"`
Run: `Get-ChildItem artifacts\FolderThemeStudio-v0.1.0-beta.4-* | Get-FileHash -Algorithm SHA256`
Expected: tests pass; four upload assets exist; recorded checksums match; setup and EXE remain below limits.

- [ ] **Step 7: Smoke-test and commit**

Launch the published EXE, confirm the Chinese first-run tutorial appears, open Settings, switch to English, edit/save all four colors, restart, verify persistence, then close without applying a real folder change.

Commit: `release: package Folder Theme Studio beta4`

