# Personalized Folder Appearance Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add material styles, one editable sticker, and a simulated desktop preview to Folder Theme Studio.

**Architecture:** Extend the persisted theme model and existing WPF icon renderer so preview and ICO share one source. Keep simulated desktop backgrounds in the view only. Preserve the current application, backup, and restore pipeline.

**Tech Stack:** C#/.NET 8, WPF, xUnit.

**Spec:** `docs/superpowers/specs/2026-10-01-personalized-folder-appearance-design.md`

## Global Constraints

- Materials: Glass (current appearance), Clay, FrostedGlass, Paper.
- Stickers: None, Star, Flower, Heart, Paw; exactly one per theme.
- Sticker coordinates are relative to the folder face: center X/Y each 0.15–0.85, scale 0.15–0.60 of face width, rotation -180–180 degrees; default is white, center (0.5, 0.5), scale 0.35, rotation 0.
- Old version 1 themes load as Glass with no sticker; new writes use version 2.
- A material or sticker edit invalidates the reviewed apply plan; preview background changes do not.
- 16–32px rendering simplifies texture and sticker detail; 16–256px preview and ICO use the same renderer.
- Do not change apply, backup, restore, or Windows icon integration behavior.

## Review Focus

- A version 1 user theme must load with the same pixels and migrate before validation (Task 1).
- Malformed version 2 enum values or non-finite sticker numbers must fail validation (Task 1).
- A sticker dragged beyond the folder face must be clamped to the safe area (Task 3).
- Rapid drag edits must not let an older preview replace a newer image (Task 4).
- A desktop background change must leave the reviewed apply plan valid (Task 5).

---

### Task 1: Theme schema and migration

**Files:** Modify `src/FolderThemeStudio.Core/Themes/FolderTheme.cs`, `ThemeValidator.cs`, `ThemeStore.cs`; test `tests/FolderThemeStudio.Core.Tests/Themes/ThemeValidatorTests.cs`, `ThemeStoreTests.cs`.

**Interfaces:** Add `MaterialStyle` and `StickerStyle` enums. Add trailing `FolderTheme` values `MaterialStyle Material`, `StickerStyle Sticker`, `string StickerColor`, `double StickerX`, `StickerY`, `StickerScale`, `StickerRotation`; defaults preserve the current appearance. Add `FolderTheme.UpgradeV1(): FolderTheme` for import and load.

- [ ] Write tests: version 1 theme imports/loads as version 2 with Glass/None and pixel-equivalent original fields; version 2 round-trips all new fields; unknown enums, invalid color, NaN/infinite numbers, and out-of-range values fail by field name.
- [ ] Run focused theme tests to see the new cases fail: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --filter "FullyQualifiedName~Themes"`.
- [ ] Implement defaults, version 1 migration at the store read boundary, and validation. Use normalized sticker center within the folder face and bounded scale/rotation; make the bounds explicit constants shared with the editor.
- [ ] Run the focused theme tests to pass; commit the theme and test files.

### Task 2: Material rendering

**Files:** Modify `src/FolderThemeStudio.Core/Rendering/FolderIconRenderer.cs`; create `src/FolderThemeStudio.Core/Rendering/MaterialPainter.cs`; test `tests/FolderThemeStudio.Core.Tests/Rendering/FolderIconRendererTests.cs`.

**Interfaces:** `MaterialPainter.Draw(DrawingContext context, FolderTheme theme, Rect body, double radius, int size, bool simplify)` draws the face after the tab and shadow. The Glass branch retains the original drawing commands and ordering.

- [ ] Before refactoring, save deterministic pixel fixtures for current Glass at 16, 32, 64, and 256px; add assertions that Glass matches them and that Clay, FrostedGlass, and Paper differ while keeping transparent margins at 16px.
- [ ] Run focused renderer tests to see new cases fail.
- [ ] Extract the current face painting unchanged into the Glass branch, then draw the three new materials with WPF vectors; skip fine texture when `simplify` is true.
- [ ] Run focused renderer tests to pass; commit renderer and test files.

### Task 3: Sticker rendering

**Files:** Create `src/FolderThemeStudio.Core/Rendering/StickerPainter.cs`; modify `FolderIconRenderer.cs`; test `tests/FolderThemeStudio.Core.Tests/Rendering/FolderIconRendererTests.cs`.

**Interfaces:** `StickerPainter.Draw(DrawingContext context, FolderTheme theme, Rect face, int size, bool simplify)` uses built-in vector geometries and the theme's normalized position, scale, rotation, and color. Provide `StickerPainter.ClampPosition(...)` or an equivalent pure geometry helper for editor drag handling.

- [ ] Add tests for no-sticker pixel identity, each sticker's visible pixels, 16px simplification, transparent margins, and out-of-face drag clamping.
- [ ] Run focused renderer tests to see new cases fail.
- [ ] Implement the vector geometry and shared clamping helper; add one sticker draw call after the folder face.
- [ ] Run focused renderer tests to pass; commit renderer and test files.

### Task 4: Editor controls and live preview

**Files:** Modify `src/FolderThemeStudio.App/ViewModels/MainViewModel.cs`, `src/FolderThemeStudio.App/MainWindow.xaml`, `MainWindow.xaml.cs`, both `src/FolderThemeStudio.App/Localization/Strings.*.xaml`; test `tests/FolderThemeStudio.Core.Tests/Application/MainViewModelTests.cs`, `MainWindowAccessibilityTests.cs`.

**Interfaces:** Expose material/sticker selections and sticker color, X/Y, scale, rotation properties on `MainViewModel`; `CurrentTheme` includes them. Add pointer handling on the existing large preview, mapping pointer coordinates through the same folder-face bounds used by `StickerPainter`.

- [ ] Add view-model tests that every material/sticker edit updates `CurrentTheme`, refreshes preview, and invalidates the reviewed apply plan; add a rapid-edit test confirming only the latest render is shown.
- [ ] Run focused view-model tests to see new cases fail.
- [ ] Add properties, controls, labels, and drag behavior; reuse the existing 100ms preview debounce and cancellation.
- [ ] Run focused view-model and accessibility tests to pass; commit UI, localization, and test files.

### Task 5: Simulated desktop and end-to-end check

**Files:** Modify `MainWindow.xaml`, `MainViewModel.cs`, both `Strings.*.xaml`, `docs/USER-GUIDE.zh-CN.md`, `docs/USER-GUIDE.en-US.md`; test `MainViewModelTests.cs`, `MainWindowTests.cs`.

**Interfaces:** Expose a preview-only desktop background choice and size; bind existing `Previews` images to a sample desktop with sample folder names. Changing either choice must not change `CurrentTheme` or `inputRevision`.

- [ ] Add tests for preview-only choices, preserved reviewed plan, and window creation with the new desktop section.
- [ ] Run focused application tests to see new cases fail.
- [ ] Add the simulated desktop view and update both guides.
- [ ] Run the full suite and Release build: `dotnet restore FolderThemeStudio.sln`, `dotnet test FolderThemeStudio.sln -c Release --no-restore`, `dotnet build FolderThemeStudio.sln -c Release --no-restore`. If restore cannot acquire `Microsoft.Windows.SDK.NET.Ref`, report the exact blocker and run all locally available checks.
- [ ] Inspect representative materials and stickers at 16/32/256px and on each desktop background; commit the UI, guide, and test files.
