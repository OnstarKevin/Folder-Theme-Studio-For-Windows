# Folder Theme Studio Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a reversible Windows desktop application that renders highly customizable glass-neon folder icons and applies them only to ordinary folders.

**Architecture:** A WPF presentation project depends on a testable application/core library. System mutations are isolated behind services for rendering, theme persistence, known-folder policy, backups, global Shell icon mapping, per-folder compatibility mode, and Shell refresh. An application coordinator validates and plans each operation before it writes anything, and records enough ownership metadata to restore only changes made by this tool.

**Tech Stack:** .NET 8, C# 12, WPF, `System.Text.Json`, Windows Registry APIs, Win32 Known Folder and Shell notification APIs, xUnit.

## Global Constraints

- Support Windows 10 and Windows 11 on x64.
- Use .NET 8 and WPF; publish a self-contained Windows executable.
- Work in the current-user scope by default and request elevation only for an explicitly selected operation that requires it.
- Change ordinary folder icons only; never change File Explorer chrome, file-type icons, or Windows special-folder icons.
- The built-in default preset is “玻璃霓虹 / 冰川蓝”.
- Generate ICO frames at 16, 20, 24, 32, 48, 64, 128, and 256 pixels.
- Never scan an entire drive by default; compatibility mode processes only roots explicitly selected by the user.
- Never overwrite or delete a pre-existing `Desktop.ini` that the tool does not own.
- The local machine currently has no .NET SDK installed. Before Task 1, install the official .NET 8 SDK and verify `dotnet --list-sdks` shows an `8.0.x` SDK.

## Planned File Structure

```text
FolderThemeStudio.sln
src/
  FolderThemeStudio.Core/
    FolderThemeStudio.Core.csproj
    Themes/FolderTheme.cs
    Themes/ThemeValidator.cs
    Themes/ThemeStore.cs
    Rendering/FolderIconRenderer.cs
    Rendering/IcoEncoder.cs
    SystemIntegration/KnownFolderService.cs
    SystemIntegration/ShellRefreshService.cs
    SystemIntegration/GlobalIconService.cs
    SystemIntegration/CompatibleFolderService.cs
    Recovery/BackupModels.cs
    Recovery/BackupService.cs
    Application/ApplyModels.cs
    Application/ThemeApplicationService.cs
  FolderThemeStudio.App/
    FolderThemeStudio.App.csproj
    App.xaml
    App.xaml.cs
    MainWindow.xaml
    MainWindow.xaml.cs
    ViewModels/MainViewModel.cs
    ViewModels/RelayCommand.cs
    Services/DialogService.cs
tests/
  FolderThemeStudio.Core.Tests/
    FolderThemeStudio.Core.Tests.csproj
    Themes/ThemeValidatorTests.cs
    Themes/ThemeStoreTests.cs
    Rendering/IcoEncoderTests.cs
    Rendering/FolderIconRendererTests.cs
    SystemIntegration/KnownFolderServiceTests.cs
    SystemIntegration/GlobalIconServiceTests.cs
    SystemIntegration/CompatibleFolderServiceTests.cs
    Recovery/BackupServiceTests.cs
    Application/ThemeApplicationServiceTests.cs
docs/manual-test-checklist.md
```

---

### Task 1: Solution skeleton and validated theme model

**Files:**
- Create: `FolderThemeStudio.sln`
- Create: `src/FolderThemeStudio.Core/FolderThemeStudio.Core.csproj`
- Create: `src/FolderThemeStudio.App/FolderThemeStudio.App.csproj`
- Create: `tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj`
- Create: `src/FolderThemeStudio.Core/Themes/FolderTheme.cs`
- Create: `src/FolderThemeStudio.Core/Themes/ThemeValidator.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Themes/ThemeValidatorTests.cs`

**Interfaces:**
- Produces: `FolderTheme`, `ThemeValidationResult`, `ThemeValidator.Validate(FolderTheme)`, and `ThemeValidator.ThrowIfInvalid(FolderTheme)` for every later task.

- [ ] **Step 1: Scaffold the solution and projects**

```powershell
dotnet new sln -n FolderThemeStudio
dotnet new classlib -n FolderThemeStudio.Core -o src/FolderThemeStudio.Core -f net8.0
dotnet new wpf -n FolderThemeStudio.App -o src/FolderThemeStudio.App -f net8.0
dotnet new xunit -n FolderThemeStudio.Core.Tests -o tests/FolderThemeStudio.Core.Tests -f net8.0
dotnet sln add src/FolderThemeStudio.Core/FolderThemeStudio.Core.csproj src/FolderThemeStudio.App/FolderThemeStudio.App.csproj tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj
dotnet add src/FolderThemeStudio.App/FolderThemeStudio.App.csproj reference src/FolderThemeStudio.Core/FolderThemeStudio.Core.csproj
dotnet add tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj reference src/FolderThemeStudio.Core/FolderThemeStudio.Core.csproj
dotnet add tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj reference src/FolderThemeStudio.App/FolderThemeStudio.App.csproj
```

Set both production projects to `net8.0-windows10.0.19041.0`, enable nullable and implicit usings, and set `<UseWPF>true</UseWPF>` in the core project so the renderer can use WPF imaging without adding a graphics dependency.

- [ ] **Step 2: Write failing validation tests**

```csharp
[Fact]
public void IceBluePreset_IsValid() =>
    Assert.True(ThemeValidator.Validate(FolderTheme.IceBlue).IsValid);

[Theory]
[InlineData(-0.01)]
[InlineData(1.01)]
public void OpacityOutsideUnitRange_IsRejected(double value)
{
    var theme = FolderTheme.IceBlue with { Opacity = value };
    Assert.Contains(nameof(FolderTheme.Opacity), ThemeValidator.Validate(theme).Errors);
}
```

- [ ] **Step 3: Run the focused tests and confirm failure**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests --filter ThemeValidatorTests`

Expected: FAIL because `FolderTheme` and `ThemeValidator` do not exist.

- [ ] **Step 4: Implement the immutable theme model and validation**

```csharp
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
    double ShadowOffset)
{
    public const int CurrentSchemaVersion = 1;
    public static FolderTheme IceBlue { get; } = new(
        1, Guid.Parse("7fcb41ba-7c24-4914-9353-2f90be4bc4f0"), "玻璃霓虹 / 冰川蓝",
        "#65D9FF", "#478CFF", 135, .82, 16, "#B9F5FF", .90, 2,
        .58, "#43BFFF", .45, 8, .18, 6);
}

public sealed record ThemeValidationResult(IReadOnlyList<string> Errors)
{
    public bool IsValid => Errors.Count == 0;
}
```

Validate schema version, non-empty name, `#RRGGBB` colors, angles from 0 through 360, unit-range opacity/strength values, and non-negative geometry values. Return property names in `Errors` so the UI can focus invalid controls.

- [ ] **Step 5: Run all tests and commit**

Run: `dotnet test`

Expected: PASS.

```powershell
git add FolderThemeStudio.sln src tests
git commit -m "feat: add validated folder theme model"
```

### Task 2: Resolution-independent renderer and multi-frame ICO encoder

**Files:**
- Create: `src/FolderThemeStudio.Core/Rendering/FolderIconRenderer.cs`
- Create: `src/FolderThemeStudio.Core/Rendering/IcoEncoder.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Rendering/FolderIconRendererTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Rendering/IcoEncoderTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Rendering/TestPng.cs`

**Interfaces:**
- Consumes: `FolderTheme` and `ThemeValidator.Validate` from Task 1.
- Produces: `BitmapSource FolderIconRenderer.Render(FolderTheme theme, int size)` and `byte[] IcoEncoder.Encode(IReadOnlyDictionary<int, byte[]> pngFrames)`.

- [ ] **Step 1: Write failing renderer and ICO structure tests**

```csharp
public static readonly int[] RequiredSizes = [16, 20, 24, 32, 48, 64, 128, 256];

[Theory]
[MemberData(nameof(Sizes))]
public void Render_ReturnsRequestedSquareBitmap(int size)
{
    var bitmap = FolderIconRenderer.Render(FolderTheme.IceBlue, size);
    Assert.Equal(size, bitmap.PixelWidth);
    Assert.Equal(size, bitmap.PixelHeight);
}

[Fact]
public void Encode_WritesOneDirectoryEntryPerRequiredSize()
{
    var frames = RequiredSizes.ToDictionary(x => x, TestPng.Create);
    var ico = IcoEncoder.Encode(frames);
    Assert.Equal(RequiredSizes.Length, BitConverter.ToUInt16(ico, 4));
}
```

Add `TestPng.Create(int size)` as a test-only helper that returns a valid square transparent PNG using `WriteableBitmap` and `PngBitmapEncoder`.

- [ ] **Step 2: Run tests and confirm failure**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests --filter "FolderIconRendererTests|IcoEncoderTests"`

Expected: FAIL because the renderer and encoder do not exist.

- [ ] **Step 3: Implement folder geometry with a small-size branch**

Use a `DrawingVisual` with normalized coordinates. Draw the back tab, translucent body, outline, top separator, diagonal highlight, shadow, and glow. For sizes `<= 32`, set glow radius to zero, clamp the outline to one device pixel, and omit any highlight thinner than one pixel. Freeze the resulting `BitmapSource` so it can safely cross threads.

```csharp
public static BitmapSource Render(FolderTheme theme, int size)
{
    ThemeValidator.ThrowIfInvalid(theme);
    if (!RequiredSizes.Contains(size)) throw new ArgumentOutOfRangeException(nameof(size));
    var visual = DrawFolder(theme, size, simplify: size <= 32);
    var target = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Pbgra32);
    target.Render(visual);
    target.Freeze();
    return target;
}
```

- [ ] **Step 4: Implement PNG encoding and ICO packing**

Encode each bitmap with `PngBitmapEncoder`. Build the ICO header (`reserved=0`, `type=1`, `count=8`), eight 16-byte directory entries, then append PNG payloads. Encode 256 as width/height byte `0`; reject missing, extra, duplicate, or invalid-sized frames.

- [ ] **Step 5: Add pixel-boundary and decode tests, run, and commit**

Add a test that reads every ICO directory offset and verifies the PNG signature, plus a test asserting the alpha bounds remain inside each bitmap.

Run: `dotnet test`

Expected: PASS.

```powershell
git add src/FolderThemeStudio.Core/Rendering tests/FolderThemeStudio.Core.Tests/Rendering
git commit -m "feat: render glass folder icons and encode ico files"
```

### Task 3: Versioned theme persistence and import/export

**Files:**
- Create: `src/FolderThemeStudio.Core/Themes/ThemeStore.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Themes/ThemeStoreTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/TestSupport/TemporaryDirectory.cs`

**Interfaces:**
- Consumes: `FolderTheme`, `ThemeValidator.Validate`.
- Produces: `Task<ThemeLoadResult> ThemeStore.LoadAsync()`, `SaveAsync(FolderTheme)`, `DeleteAsync(Guid)`, `ExportAsync(Guid, string)`, and `Task<ThemeImportResult> ImportAsync(string)`.

- [ ] **Step 1: Write failing round-trip, invalid import, and built-in protection tests**

```csharp
[Fact]
public async Task SaveThenLoad_RoundTripsTheme()
{
    var store = new ThemeStore(temp.Path);
    await store.SaveAsync(FolderTheme.IceBlue with { Id = Guid.NewGuid(), Name = "Mine" });
    Assert.Equal("Mine", (await store.LoadAsync()).Themes.Single(x => x.Name == "Mine").Name);
}

[Fact]
public async Task DeleteBuiltIn_IsRejected() =>
    await Assert.ThrowsAsync<InvalidOperationException>(() =>
        new ThemeStore(temp.Path).DeleteAsync(FolderTheme.IceBlue.Id));
```

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests --filter ThemeStoreTests`

Expected: FAIL because `ThemeStore` does not exist.

- [ ] **Step 3: Implement atomic JSON persistence**

Store user themes under `%LocalAppData%\FolderThemeStudio\themes`. Write UTF-8 JSON to a sibling `.tmp`, flush, then replace the destination. Import parses, validates, assigns a new ID on collision, and never writes invalid content. `LoadAsync` always returns the built-in preset first and skips corrupt user files while returning diagnostics through `ThemeLoadResult`.

`TemporaryDirectory` creates a unique directory under `Path.GetTempPath()` and exposes its absolute `Path`; disposal removes only that owned directory after normalizing and verifying it is still beneath the test root.

- [ ] **Step 4: Run all tests and commit**

Run: `dotnet test`

Expected: PASS.

```powershell
git add src/FolderThemeStudio.Core/Themes tests/FolderThemeStudio.Core.Tests/Themes
git commit -m "feat: persist and exchange folder themes"
```

### Task 4: Ordinary-folder policy and traversal planning

**Files:**
- Create: `src/FolderThemeStudio.Core/SystemIntegration/KnownFolderService.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/SystemIntegration/KnownFolderServiceTests.cs`

**Interfaces:**
- Produces: `FolderDecision KnownFolderService.Evaluate(string path)` and `IAsyncEnumerable<FolderPlanItem> PlanTreeAsync(string root, CancellationToken token)`.
- `FolderDecision` is `Allowed`, `KnownFolder`, `ProtectedRoot`, `NetworkPath`, `ReparsePoint`, `NoWriteAccess`, or `ExistingCustomization`.

- [ ] **Step 1: Write failing classification tests**

```csharp
[Theory]
[InlineData(@"C:\Windows", FolderDecision.ProtectedRoot)]
[InlineData(@"\\server\share", FolderDecision.NetworkPath)]
public void Evaluate_RejectsUnsafeRoots(string path, FolderDecision expected) =>
    Assert.Equal(expected, service.Evaluate(path));

[Fact]
public void Evaluate_RejectsInjectedKnownFolder() =>
    Assert.Equal(FolderDecision.KnownFolder, service.Evaluate(@"C:\Users\Test\Downloads"));
```

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests --filter KnownFolderServiceTests`

Expected: FAIL because policy types do not exist.

- [ ] **Step 3: Implement canonical path and Known Folder checks**

Resolve known-folder paths with `SHGetKnownFolderPath` for Desktop, Downloads, Documents, Pictures, Music, Videos, Favorites, Saved Games, OneDrive, and Public equivalents. Normalize with `Path.GetFullPath`, trim separators only after preserving volume roots, and compare using `OrdinalIgnoreCase`. Reject UNC paths, protected roots, and directories with `FileAttributes.ReparsePoint`.

- [ ] **Step 4: Implement cancellable, non-following traversal**

Use `EnumerationOptions { RecurseSubdirectories = false, IgnoreInaccessible = true, AttributesToSkip = FileAttributes.ReparsePoint }`. Evaluate each child before enqueueing it. Report skipped items as plan results; do not silently cross a rejected directory.

- [ ] **Step 5: Run all tests and commit**

Run: `dotnet test`

Expected: PASS.

```powershell
git add src/FolderThemeStudio.Core/SystemIntegration/KnownFolderService.cs tests/FolderThemeStudio.Core.Tests/SystemIntegration/KnownFolderServiceTests.cs
git commit -m "feat: identify and exclude special folders"
```

### Task 5: Recovery snapshots and ownership records

**Files:**
- Create: `src/FolderThemeStudio.Core/Recovery/BackupModels.cs`
- Create: `src/FolderThemeStudio.Core/Recovery/BackupService.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Recovery/BackupServiceTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/TestSupport/InMemoryFileSystem.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/TestSupport/SnapshotFactory.cs`

**Interfaces:**
- Produces: `OperationSnapshot`, `FolderMutationRecord`, `BackupService.CreateAsync`, `AppendMutationAsync`, `CompleteAsync`, and `LoadLatestAsync`.

- [ ] **Step 1: Write failing atomicity and round-trip tests**

```csharp
[Fact]
public async Task SnapshotRoundTrip_PreservesOriginalRegistryValuesAndFolderAttributes()
{
    var snapshot = SnapshotFactory.WithRegistryAndFolderMutation();
    await service.CreateAsync(snapshot);
    Assert.Equal(snapshot, await service.LoadLatestAsync());
}

[Fact]
public async Task InterruptedTemporaryWrite_DoesNotReplaceLastGoodSnapshot()
{
    await service.CreateAsync(SnapshotFactory.First());
    fileSystem.FailNextReplace = true;
    await Assert.ThrowsAsync<IOException>(() => service.CreateAsync(SnapshotFactory.Second()));
    Assert.Equal(SnapshotFactory.First().Id, (await service.LoadLatestAsync())!.Id);
}
```

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests --filter BackupServiceTests`

Expected: FAIL because recovery types do not exist.

- [ ] **Step 3: Implement versioned snapshots and append-only mutation ownership**

Persist under `%LocalAppData%\FolderThemeStudio\backups\<operation-id>`. A snapshot records whether registry values `3` and `4` existed and their exact values. Each compatibility record stores folder path, original folder attributes, created file path, expected SHA-256 of tool-owned `Desktop.ini`, and completion state. Use atomic JSON replacement and never infer ownership merely from a filename.

The test-only `InMemoryFileSystem` implements the same internal `IFileSystem` abstraction used by `BackupService`; `SnapshotFactory` returns deterministic records with fixed GUIDs and timestamps so equality assertions are stable.

- [ ] **Step 4: Run all tests and commit**

Run: `dotnet test`

Expected: PASS.

```powershell
git add src/FolderThemeStudio.Core/Recovery tests/FolderThemeStudio.Core.Tests/Recovery
git commit -m "feat: record reversible icon mutations"
```

### Task 6: Global quick mode and Shell refresh

**Files:**
- Create: `src/FolderThemeStudio.Core/SystemIntegration/ShellRefreshService.cs`
- Create: `src/FolderThemeStudio.Core/SystemIntegration/GlobalIconService.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/SystemIntegration/GlobalIconServiceTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/TestSupport/InMemoryRegistryStore.cs`

**Interfaces:**
- Consumes: `OperationSnapshot` and generated ICO path.
- Produces: `GlobalApplyResult Apply(string icoPath)`, `GlobalRestoreResult Restore(OperationSnapshot snapshot)`, and `ShellRefreshResult ShellRefreshService.RefreshIcons()`.

- [ ] **Step 1: Write failing registry adapter tests**

```csharp
[Fact]
public void Apply_WritesClosedAndOpenFolderMappings()
{
    var result = service.Apply(@"C:\Themes\ice.ico");
    Assert.True(result.Success);
    Assert.Equal(@"C:\Themes\ice.ico,0", registry.Get("3"));
    Assert.Equal(@"C:\Themes\ice.ico,0", registry.Get("4"));
}

[Fact]
public void Restore_ReinstatesMissingAndExistingValuesExactly()
{
    service.Restore(SnapshotFactory.MixedRegistryState());
    Assert.False(registry.Exists("3"));
    Assert.Equal("original.dll,4", registry.Get("4"));
}
```

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests --filter GlobalIconServiceTests`

Expected: FAIL because services do not exist.

- [ ] **Step 3: Implement injectable Registry access**

Use `HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons`; capture the exact pre-write state before setting string values `3` and `4` to `<icoPath>,0`. Return a failure result on access or verification failure. Never fall back to HKLM and never elevate automatically.

Define internal `IRegistryStore` with `TryGet(string name, out string? value)`, `Set(string name, string value)`, and `Delete(string name)`. Production uses `Microsoft.Win32.Registry`; tests use `InMemoryRegistryStore`.

- [ ] **Step 4: Implement Shell refresh notification**

P/Invoke `SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero)`. Report notification failure separately from registry success so the UI can offer sign-out/restart guidance without claiming the write failed.

- [ ] **Step 5: Run all tests and commit**

Run: `dotnet test`

Expected: PASS.

```powershell
git add src/FolderThemeStudio.Core/SystemIntegration tests/FolderThemeStudio.Core.Tests/SystemIntegration
git commit -m "feat: apply and restore global folder icon mapping"
```

### Task 7: Compatibility mode with non-destructive Desktop.ini ownership

**Files:**
- Create: `src/FolderThemeStudio.Core/SystemIntegration/CompatibleFolderService.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/SystemIntegration/CompatibleFolderServiceTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/TestSupport/TemporaryFolderFixture.cs`

**Interfaces:**
- Consumes: `FolderPlanItem`, ICO path, `BackupService`.
- Produces: `Task<CompatibleApplySummary> ApplyAsync(IReadOnlyList<FolderPlanItem> plan, string icoPath, CancellationToken token)` and `Task<CompatibleRestoreSummary> RestoreAsync(Guid operationId, CancellationToken token)`.

- [ ] **Step 1: Write failing apply, skip, restore, and cancellation tests**

```csharp
[Fact]
public async Task ExistingUnownedDesktopIni_IsSkippedWithoutModification()
{
    fixture.WriteDesktopIni("[.ShellClassInfo]\r\nInfoTip=Keep me\r\n");
    var before = fixture.ReadDesktopIni();
    var result = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
    Assert.Equal(1, result.Skipped);
    Assert.Equal(before, fixture.ReadDesktopIni());
}

[Fact]
public async Task Restore_DeletesOnlyMatchingOwnedFileAndRestoresAttributes()
{
    var summary = await service.ApplyAsync(fixture.Plan, fixture.IcoPath, CancellationToken.None);
    await service.RestoreAsync(summary.OperationId, CancellationToken.None);
    Assert.False(File.Exists(fixture.DesktopIniPath));
    Assert.Equal(fixture.OriginalAttributes, File.GetAttributes(fixture.FolderPath));
}
```

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests --filter CompatibleFolderServiceTests`

Expected: FAIL because `CompatibleFolderService` does not exist.

- [ ] **Step 3: Implement atomic per-folder application**

For each allowed folder with no `Desktop.ini`, write this UTF-16 content to a temporary sibling file, flush, rename, then set the file to Hidden/System and the folder to ReadOnly while preserving original attributes:

```csharp
var desktopIni = $"[.ShellClassInfo]\r\nIconFile={Path.GetFullPath(icoPath)}\r\nIconIndex=0\r\n";
```

Record the mutation immediately after each atomic write. Stop scheduling new items after cancellation but finish the current file operation.

`TemporaryFolderFixture` creates a unique directory under the test runner's temporary root and exposes `FolderPath`, `DesktopIniPath`, `OriginalAttributes`, `IcoPath`, `Plan`, `WriteDesktopIni`, and `ReadDesktopIni`; its `Dispose` restores attributes before removing only its own temporary directory.

- [ ] **Step 4: Implement hash-guarded restore**

Before deletion, verify the current file SHA-256 matches the recorded tool-created hash. If it differs, skip and report `ChangedSinceApply`. Restore only attribute bits changed by the recorded operation.

- [ ] **Step 5: Run all tests and commit**

Run: `dotnet test`

Expected: PASS.

```powershell
git add src/FolderThemeStudio.Core/SystemIntegration/CompatibleFolderService.cs tests/FolderThemeStudio.Core.Tests/SystemIntegration/CompatibleFolderServiceTests.cs
git commit -m "feat: add reversible compatibility mode"
```

### Task 8: Transactional application coordinator

**Files:**
- Create: `src/FolderThemeStudio.Core/Application/ApplyModels.cs`
- Create: `src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Application/ThemeApplicationServiceTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/TestSupport/ApplicationFixture.cs`

**Interfaces:**
- Consumes: validator, renderer, ICO encoder, policy, backup, global, compatibility, and refresh services.
- Produces: `Task<ApplyPlan> PlanAsync(ApplyRequest request, CancellationToken token)`, `Task<ApplyResult> ApplyAsync(ApplyPlan plan, IProgress<ApplyProgress> progress, CancellationToken token)`, and `Task<RestoreResult> RestoreLatestAsync(IProgress<ApplyProgress> progress, CancellationToken token)`.

- [ ] **Step 1: Write failing orchestration and rollback tests**

```csharp
[Fact]
public async Task Apply_CreatesIconThenSnapshotBeforeSystemWrite()
{
    await service.ApplyAsync(fixture.ValidGlobalPlan, fixture.Progress, CancellationToken.None);
    Assert.Equal(["render", "snapshot", "global-write", "refresh"], fixture.CallOrder);
}

[Fact]
public async Task FailedSystemWrite_AttemptsRollbackAndReportsBothResults()
{
    fixture.Global.FailApply = true;
    var result = await service.ApplyAsync(fixture.ValidGlobalPlan, fixture.Progress, CancellationToken.None);
    Assert.False(result.Success);
    Assert.True(result.RollbackAttempted);
    Assert.True(result.RollbackSucceeded);
}
```

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests --filter ThemeApplicationServiceTests`

Expected: FAIL because application models and coordinator do not exist.

- [ ] **Step 3: Implement plan-first execution**

`ApplyRequest` contains theme, mode, and optional explicit roots. `PlanAsync` validates the theme and, for compatibility mode, returns counts and skipped reasons without writing. `ApplyAsync` generates and verifies the ICO, persists the snapshot, applies the chosen mode, refreshes Shell icons, and returns separate success/skip/failure records.

- [ ] **Step 4: Implement rollback and idempotent restore**

On any post-snapshot mutation failure, call the matching restore service. A second restore call must report `AlreadyRestored` rather than fail. Never report overall success if rendering, snapshot creation, system write, verification, or required rollback fails.

`ApplicationFixture` supplies recording fakes for each service, a deterministic `ValidGlobalPlan`, and an `IProgress<ApplyProgress>` collector; every fake appends its operation name to a shared `CallOrder` list.

- [ ] **Step 5: Run all tests and commit**

Run: `dotnet test`

Expected: PASS.

```powershell
git add src/FolderThemeStudio.Core/Application tests/FolderThemeStudio.Core.Tests/Application
git commit -m "feat: coordinate safe theme application and rollback"
```

### Task 9: WPF editor, preview, apply flow, and accessibility

**Files:**
- Modify: `src/FolderThemeStudio.App/App.xaml`
- Modify: `src/FolderThemeStudio.App/App.xaml.cs`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml.cs`
- Create: `src/FolderThemeStudio.App/ViewModels/MainViewModel.cs`
- Create: `src/FolderThemeStudio.App/ViewModels/RelayCommand.cs`
- Create: `src/FolderThemeStudio.App/Services/DialogService.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/Application/MainViewModelTests.cs`
- Create: `tests/FolderThemeStudio.Core.Tests/TestSupport/MainViewModelFixture.cs`

**Interfaces:**
- Consumes: all Task 1–8 public interfaces.
- Produces: the usable desktop application.

- [ ] **Step 1: Write failing view-model state tests**

```csharp
[Fact]
public async Task EditingColor_RefreshesAllPreviewSizes()
{
    var vm = fixture.CreateViewModel();
    vm.GradientStart = "#22AAFF";
    await vm.WaitForPreviewAsync();
    Assert.Equal([16, 32, 64, 256], vm.Previews.Select(x => x.Size));
}

[Fact]
public void CompatibilityMode_RequiresExplicitRoot()
{
    var vm = fixture.CreateViewModel();
    vm.SelectedMode = ApplyMode.Compatible;
    Assert.False(vm.ApplyCommand.CanExecute(null));
}
```

- [ ] **Step 2: Run focused tests and confirm failure**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests --filter MainViewModelTests`

Expected: FAIL because `MainViewModel` does not exist.

- [ ] **Step 3: Implement the view model and debounced previews**

Expose bindable theme properties, validation messages, four preview images, saved themes, selected mode, selected roots, planning counts, progress, and final result. Debounce slider/color changes by 100 ms and cancel obsolete renders. Disable Apply while validation, planning, rendering, application, or restore is active.

`MainViewModelFixture` provides an immediate test dispatcher, fake dialogs, deterministic preview renderer, and a `WaitForPreviewAsync` completion hook so tests never sleep or depend on wall-clock timing.

- [ ] **Step 4: Build the WPF layout**

Use a two-column layout at normal widths: editor and theme controls on the left; dual-background preview and application panel on the right. Stack into one column below 900 device-independent pixels. Provide keyboard labels, visible focus states, AutomationProperties names, 44-pixel primary action targets, system-font scaling, and no color-only status signals.

- [ ] **Step 5: Wire safe confirmations and results**

Before global apply, show the exact current-user mapping change and restoration availability. Before compatibility apply, show selected roots, allowed/skipped counts, and confirm no whole-drive default. Results list successful, skipped, and failed counts with copy/export for failed paths. Restore shows partial failures without hiding successful reversions.

- [ ] **Step 6: Run tests, build, launch smoke test, and commit**

Run: `dotnet test`

Run: `dotnet build src/FolderThemeStudio.App/FolderThemeStudio.App.csproj -c Release`

Run: `dotnet run --project src/FolderThemeStudio.App/FolderThemeStudio.App.csproj`

Expected: tests PASS, Release build succeeds, and the window opens with the ice-blue preset and four previews.

```powershell
git add src/FolderThemeStudio.App tests/FolderThemeStudio.Core.Tests/Application/MainViewModelTests.cs
git commit -m "feat: add accessible folder theme editor"
```

### Task 10: Packaging, manual safety verification, and release documentation

**Files:**
- Modify: `src/FolderThemeStudio.App/FolderThemeStudio.App.csproj`
- Create: `docs/manual-test-checklist.md`
- Create: `README.md`

**Interfaces:**
- Consumes: completed application.
- Produces: self-contained `win-x64` release and reproducible verification record.

- [ ] **Step 1: Add publish settings**

Set `RuntimeIdentifier=win-x64`, `SelfContained=true`, `PublishSingleFile=true`, `PublishReadyToRun=true`, `DebugType=embedded`, and a stable application icon. Do not trim WPF assemblies.

- [ ] **Step 2: Write the exact manual test checklist**

Include checkboxes for Windows 10 and 11; light/dark themes; 100%, 150%, and 200% DPI; all Explorer icon sizes; global apply, reapply, restart persistence, and restore; compatibility apply, cancellation, modified `Desktop.ini`, and restore; system special folders unchanged; file icons unchanged; denied directory behavior; corrupt backup behavior; and quick-mode recovery after a simulated missing icon file.

- [ ] **Step 3: Document usage and limitations**

README must explain the two modes, the undocumented nature of global quick mode, exact scope exclusions, backup location, restore behavior, compatibility-mode `Desktop.ini` ownership, and how to report failures without exposing unrelated paths.

- [ ] **Step 4: Run release verification**

Run: `dotnet test -c Release`

Run: `dotnet publish src/FolderThemeStudio.App/FolderThemeStudio.App.csproj -c Release -r win-x64 --self-contained true`

Run the published executable and complete the applicable rows in `docs/manual-test-checklist.md`. Do not mark untested Windows versions as passed.

Expected: tests PASS, publish succeeds, the app launches without a separately installed runtime, global restore returns the test profile to its original state, and the special-folder/file-icon checks remain unchanged.

- [ ] **Step 5: Commit the verified release configuration**

```powershell
git add src/FolderThemeStudio.App/FolderThemeStudio.App.csproj README.md docs/manual-test-checklist.md
git commit -m "docs: add release and safety verification workflow"
```

## Final Verification

- [ ] Run `dotnet test -c Release` and retain the passing summary.
- [ ] Run `dotnet publish src/FolderThemeStudio.App/FolderThemeStudio.App.csproj -c Release -r win-x64 --self-contained true` and retain the output path.
- [ ] Confirm `git status --short` is empty.
- [ ] Review the manual checklist and clearly identify any Windows version or DPI configuration not physically tested.
