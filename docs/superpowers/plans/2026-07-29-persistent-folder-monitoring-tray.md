# Persistent Folder Monitoring and Tray Background Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Let compatible mode safely restyle folders with existing `desktop.ini`, recursively style newly created folders using persistent rules, resume at Windows sign-in, and keep the app available from the notification area after its window closes.

**Architecture:** Add a byte-preserving Desktop.ini merge/recovery layer in Core, a versioned monitoring-rule store plus watcher coordinator in App, and isolated startup/tray adapters around WPF lifecycle. Manual compatible apply and background monitoring share one folder-icon mutation service so overwrite, merge, validation, and error behavior cannot diverge.

**Tech Stack:** .NET 8, WPF, `FileSystemWatcher`, `System.Text.Json`, current-user Windows Registry, `System.Windows.Forms.NotifyIcon`, xUnit, Inno Setup 6.

## Global Constraints

- Existing special-folder, protected-root, network-path, reparse-point, and write-access exclusions remain active.
- Existing `desktop.ini` is merged; unrelated content must not be erased.
- Unsafe parsing leaves the original file untouched and returns a visible per-folder failure.
- Monitoring is recursive and restricted to explicitly configured canonical roots.
- Closing hides to the notification area by default; only explicit Exit shuts down.
- Current-user sign-in startup is enabled by default and uses `--background`.
- No Windows service and no administrator-only startup mechanism.
- Use test-first red/green cycles and preserve unrelated user worktree changes.
- Release target is `v0.1.0-beta.6`, file version `0.1.0.6`, assembly version remains `0.1.0.0`.

---

### Task 1: Byte-Preserving Desktop.ini Merge

**Files:**
- Create: `src/FolderThemeStudio.Core/SystemIntegration/DesktopIniDocument.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/SystemIntegration/DesktopIniDocumentTests.cs`

**Interfaces:**
- Produces: `DesktopIniMergeResult DesktopIniDocument.Merge(byte[]? originalBytes, string icoPath)`.
- `DesktopIniMergeResult` contains `bool Success`, `byte[]? Bytes`, and `string? Error`.
- Later tasks use this function for both manual and monitored mutations.

- [ ] **Step 1: Write failing merge tests**

Cover absent input, existing `[.ShellClassInfo]`, `IconResource`, `IconFile`, `IconIndex`, unrelated sections/keys/comments, CRLF/LF preservation, UTF-8 BOM, UTF-16 LE, malformed section headers, and idempotent reapplication. Assert that only icon fields change and malformed input returns failure with no bytes.

```csharp
[Fact]
public void Merge_PreservesUnrelatedKeysAndReplacesIconValues()
{
    var original = Encoding.Unicode.GetBytes(
        "[.ShellClassInfo]\r\nLocalizedResourceName=@shell32.dll,-1\r\nIconFile=old.ico\r\nIconIndex=2\r\n[ViewState]\r\nMode=\r\n");

    var result = DesktopIniDocument.Merge(original, @"C:\Icons\new.ico");

    Assert.True(result.Success);
    var text = Encoding.Unicode.GetString(result.Bytes!);
    Assert.Contains("LocalizedResourceName=@shell32.dll,-1", text);
    Assert.Contains("IconFile=C:\\Icons\\new.ico", text);
    Assert.Contains("IconIndex=0", text);
    Assert.Contains("[ViewState]\r\nMode=", text);
    Assert.DoesNotContain("old.ico", text);
}
```

- [ ] **Step 2: Run the focused tests and verify RED**

Run: `dotnet test FolderThemeStudio.sln --no-restore --filter "FullyQualifiedName~DesktopIniDocumentTests"`

Expected: FAIL because `DesktopIniDocument` does not exist.

- [ ] **Step 3: Implement the minimal parser and merge**

Detect BOM/encoding and line ending, parse section boundaries and key names case-insensitively, preserve untouched raw lines, replace icon keys in `.ShellClassInfo`, append the section if absent, and encode with the original encoding/BOM. Reject NUL-containing input, undecodable bytes, duplicate malformed headers, and lines that cannot be classified safely.

- [ ] **Step 4: Run focused tests and verify GREEN**

Run the Step 2 command. Expected: all `DesktopIniDocumentTests` pass.

- [ ] **Step 5: Commit**

```powershell
git add src/FolderThemeStudio.Core/SystemIntegration/DesktopIniDocument.cs tests/FolderThemeStudio.Core.Tests/SystemIntegration/DesktopIniDocumentTests.cs
git commit -m "feat: merge icon fields in desktop ini"
```

---

### Task 2: Exact Backup and Restore for Existing Desktop.ini

**Files:**
- Modify: `src/FolderThemeStudio.Core/Recovery/BackupModels.cs`
- Modify: `src/FolderThemeStudio.Core/Recovery/BackupService.cs`
- Modify: `src/FolderThemeStudio.Core/SystemIntegration/CompatibleFolderFileSystem.cs`
- Modify: `src/FolderThemeStudio.Core/SystemIntegration/CompatibleFolderService.cs`
- Modify: `src/FolderThemeStudio.Core/SystemIntegration/KnownFolderService.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Recovery/BackupServiceTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/SystemIntegration/CompatibleFolderServiceTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/SystemIntegration/KnownFolderServiceTests.cs`

**Interfaces:**
- Extend `FolderMutationRecord` with `bool FileOriginallyExisted`, `string? OriginalDesktopIniBase64`, and `FileAttributes? OriginalDesktopIniAttributes`.
- Add `DesktopIniSnapshot ICompatibleFolderFileSystem.CaptureDesktopIni(string path)`.
- Replace create-only write with `Task<AtomicDesktopIniWriteResult> WriteDesktopIniAtomicallyAsync(string path, byte[] bytes, bool overwrite)`.

- [ ] **Step 1: Write failing planning, apply, and restore tests**

Assert that an ordinary writable folder with `desktop.ini` is `Allowed`; compatible apply preserves unrelated bytes and records the original; restoring a pre-existing file writes back exact bytes/attributes; restoring a newly created file still uses hash-owned deletion; a malformed file produces failure without mutation.

- [ ] **Step 2: Run focused tests and verify RED**

Run: `dotnet test FolderThemeStudio.sln --no-restore --filter "FullyQualifiedName~CompatibleFolderServiceTests|FullyQualifiedName~KnownFolderServiceTests|FullyQualifiedName~BackupServiceTests"`

Expected: failures showing `ExistingCustomization` and missing original-file snapshot fields.

- [ ] **Step 3: Extend the durable recovery schema**

Validate that original bytes/attributes are present exactly when `FileOriginallyExisted` is true. Preserve backward compatibility for old snapshots by treating absent fields as a tool-created file. Keep canonical path and SHA-256 validation.

- [ ] **Step 4: Replace the create-only mutation flow**

Remove the `desktop.ini` skip from planning and revalidation. Capture original bytes/attributes, call `DesktopIniDocument.Merge`, persist the original snapshot before mutation, atomically replace or create the file, then complete attribute phases. On pre-snapshot or parse failure, do not write.

- [ ] **Step 5: Restore according to ownership type**

For pre-existing files, restore exact original bytes and attributes after confirming the current hash equals the recorded post-write hash. For created files, retain current hash-matched deletion. Hash mismatch remains `ChangedSinceApply`.

- [ ] **Step 6: Run focused and application-service tests**

Run: `dotnet test FolderThemeStudio.sln --no-restore --filter "FullyQualifiedName~CompatibleFolderServiceTests|FullyQualifiedName~KnownFolderServiceTests|FullyQualifiedName~BackupServiceTests|FullyQualifiedName~ThemeApplicationServiceTests"`

Expected: all selected tests pass and `ExistingCustomization` is no longer a skip reason for ordinary compatible targets.

- [ ] **Step 7: Commit**

```powershell
git add src/FolderThemeStudio.Core/Recovery src/FolderThemeStudio.Core/SystemIntegration tests/FolderThemeStudio.Core.Tests/Recovery tests/FolderThemeStudio.Core.Tests/SystemIntegration tests/FolderThemeStudio.Core.Tests/Application/ThemeApplicationServiceTests.cs
git commit -m "feat: safely restyle existing customized folders"
```

---

### Task 3: Versioned Monitoring Rules and Durable Icon Assets

**Files:**
- Create: `src/FolderThemeStudio.App/Monitoring/MonitoringRule.cs`
- Create: `src/FolderThemeStudio.App/Monitoring/JsonMonitoringRuleStore.cs`
- Create: `src/FolderThemeStudio.App/Monitoring/MonitoringIconAssetService.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/MonitoringRuleStoreTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/MonitoringIconAssetServiceTests.cs`

**Interfaces:**
- `MonitoringRule(int SchemaVersion, string RootPath, string IcoPath, DateTimeOffset UpdatedAtUtc)`.
- `IMonitoringRuleStore.LoadAsync`, `UpsertAsync`, `RemoveAsync`, and `SetPausedAsync`.
- `IMonitoringIconAssetService.PersistAsync(string generatedIcoPath, CancellationToken)` returns a hash-named ICO under `%LOCALAPPDATA%\FolderThemeStudio\monitoring-icons`.

- [ ] **Step 1: Write failing persistence tests**

Test atomic JSON save, canonical case-insensitive root replacement, malformed-rule isolation with diagnostics, missing-root retention, pause persistence, content-addressed ICO copy, and reuse of identical assets.

- [ ] **Step 2: Run tests and verify RED**

Run: `dotnet test FolderThemeStudio.sln --no-restore --filter "FullyQualifiedName~MonitoringRuleStoreTests|FullyQualifiedName~MonitoringIconAssetServiceTests"`

Expected: FAIL because monitoring persistence types do not exist.

- [ ] **Step 3: Implement stores with injected paths and file adapters**

Use schema version `1`, full canonical paths, case-insensitive root identity, temporary-file replacement, and per-entry diagnostics. Never delete an ICO still referenced by a rule.

- [ ] **Step 4: Run focused tests and verify GREEN**

Run the Step 2 command. Expected: all focused tests pass.

- [ ] **Step 5: Commit**

```powershell
git add src/FolderThemeStudio.App/Monitoring tests/FolderThemeStudio.Core.Tests/Application/MonitoringRuleStoreTests.cs tests/FolderThemeStudio.Core.Tests/Application/MonitoringIconAssetServiceTests.cs
git commit -m "feat: persist recursive monitoring rules"
```

---

### Task 4: Recursive Watcher Coordinator

**Files:**
- Create: `src/FolderThemeStudio.App/Monitoring/FolderMonitoringCoordinator.cs`
- Create: `src/FolderThemeStudio.App/Monitoring/DirectoryWatcher.cs`
- Create: `src/FolderThemeStudio.Core/SystemIntegration/MonitoredFolderIconService.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/FolderMonitoringCoordinatorTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/SystemIntegration/MonitoredFolderIconServiceTests.cs`

**Interfaces:**
- `IFolderMonitoringCoordinator.StartAsync`, `PauseAsync`, `ResumeAsync`, `ReplaceRuleAsync`, `RemoveRuleAsync`, and `MonitoringStatus Status`.
- `IMonitoredFolderIconService.ApplyAsync(string folderPath, string icoPath, CancellationToken)` returns `MonitoredApplyOutcome` without creating a manual recovery operation.
- Coordinator publishes `MonitoringOutcome` events for UI history.

- [ ] **Step 1: Write failing coordinator tests with fake watcher/time**

Test recursive watcher configuration, event canonicalization, burst de-duplication, three bounded readiness attempts, safe-path revalidation, pause/resume, rule replacement, missing-root retry, watcher overflow restart, and isolation between roots.

- [ ] **Step 2: Write failing monitored-mutation tests**

Assert the service uses the same Desktop.ini merge, preserves unrelated content, adds Hidden/System plus folder ReadOnly attributes, and returns failure without mutation for unsafe input.

- [ ] **Step 3: Run focused tests and verify RED**

Run: `dotnet test FolderThemeStudio.sln --no-restore --filter "FullyQualifiedName~FolderMonitoringCoordinatorTests|FullyQualifiedName~MonitoredFolderIconServiceTests"`

- [ ] **Step 4: Implement monitored folder mutation**

Extract the shared merge/write operation from compatible apply into a focused service. Manual apply supplies a recovery recorder; monitoring supplies a no-manual-snapshot recorder and reports outcomes. Both retain policy revalidation.

- [ ] **Step 5: Implement the watcher coordinator**

Create one `FileSystemWatcher` per active root with `IncludeSubdirectories=true`, `NotifyFilter=DirectoryName`, and Created/Renamed handling. Feed a single async queue keyed by canonical path; debounce event bursts and retry at 250 ms, 1 s, and 3 s. On overflow, recreate only the affected watcher and enumerate directories newer than its last successful timestamp under that configured root.

- [ ] **Step 6: Run focused tests and verify GREEN**

Run the Step 3 command. Expected: all focused tests pass.

- [ ] **Step 7: Commit**

```powershell
git add src/FolderThemeStudio.App/Monitoring src/FolderThemeStudio.Core/SystemIntegration tests/FolderThemeStudio.Core.Tests/Application/FolderMonitoringCoordinatorTests.cs tests/FolderThemeStudio.Core.Tests/SystemIntegration/MonitoredFolderIconServiceTests.cs
git commit -m "feat: monitor new folders recursively"
```

---

### Task 5: Manual Apply Integration and Monitoring UI

**Files:**
- Modify: `src/FolderThemeStudio.Core/Application/ApplyModels.cs`
- Modify: `src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs`
- Modify: `src/FolderThemeStudio.App/ViewModels/MainViewModel.cs`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml`
- Modify: `src/FolderThemeStudio.App/Localization/Strings.zh-CN.xaml`
- Modify: `src/FolderThemeStudio.App/Localization/Strings.en-US.xaml`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/MainViewModelTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/MainWindowTests.cs`

**Interfaces:**
- Add `string? IconArtifactPath` to successful `ApplyResult` so App can persist the verified ICO.
- Add `bool ContinueApplyingToNewFolders`, `MonitoringStatusText`, `ActiveMonitoringRuleCount`, `RemoveMonitoringRuleCommand`, and pause/resume command state to `MainViewModel`.

- [ ] **Step 1: Write failing view-model tests**

Test that compatible success plus enabled persistence saves/replaces rules for explicit roots, global apply never saves rules, failed/skipped roots do not become rules, source/root changes invalidate the plan, removing a rule stops monitoring, and monitoring outcomes appear in Results without modal dialogs.

- [ ] **Step 2: Run focused tests and verify RED**

Run: `dotnet test FolderThemeStudio.sln --no-restore --filter "FullyQualifiedName~MainViewModelTests|FullyQualifiedName~MainWindowTests"`

- [ ] **Step 3: Expose verified artifact and connect rules**

Return the verified ICO path from application service. After compatible apply, persist it through `MonitoringIconAssetService`, upsert only successful explicit roots, then refresh coordinator/status. Keep the option disabled and hidden in global mode.

- [ ] **Step 4: Add localized controls**

Place `Continue applying to new folders` beside compatible roots, show running/paused/inactive counts, and add pause/resume plus remove-selected-rule actions. Keep layout usable in stacked mode.

- [ ] **Step 5: Run focused tests and verify GREEN**

Run the Step 2 command. Expected: all selected tests pass.

- [ ] **Step 6: Commit**

```powershell
git add src/FolderThemeStudio.Core/Application src/FolderThemeStudio.App/ViewModels/MainViewModel.cs src/FolderThemeStudio.App/MainWindow.xaml src/FolderThemeStudio.App/Localization tests/FolderThemeStudio.Core.Tests/Application/MainViewModelTests.cs tests/FolderThemeStudio.Core.Tests/Application/MainWindowTests.cs
git commit -m "feat: manage persistent monitoring from compatible mode"
```

---

### Task 6: Tray Lifecycle, Settings, and Sign-In Startup

**Files:**
- Modify: `src/FolderThemeStudio.App/FolderThemeStudio.App.csproj`
- Modify: `src/FolderThemeStudio.App/App.xaml`
- Modify: `src/FolderThemeStudio.App/App.xaml.cs`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml.cs`
- Modify: `src/FolderThemeStudio.App/Settings/AppSettings.cs`
- Modify: `src/FolderThemeStudio.App/ViewModels/SettingsViewModel.cs`
- Modify: `src/FolderThemeStudio.App/SettingsWindow.xaml`
- Create: `src/FolderThemeStudio.App/Services/TrayIconService.cs`
- Create: `src/FolderThemeStudio.App/Services/StartupRegistrationService.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/AppSettingsTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/SettingsViewModelTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/MainWindowTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/StartupRegistrationServiceTests.cs`
- Test: `tests/FolderThemeStudio.Core.Tests/Application/TrayLifecycleTests.cs`

**Interfaces:**
- Settings add `bool StartWithWindows`, `bool CloseToTray`, and `bool MonitoringPaused`, with migration defaults `true`, `true`, and `false` for older JSON.
- `IStartupRegistrationService.SetEnabled(bool enabled, string executablePath)` owns `HKCU\...\Run\FolderThemeStudio` with quoted `--background` command.
- `ITrayIconService` exposes Open, ToggleMonitoring, and Exit events.

- [ ] **Step 1: Write failing settings migration and registry tests**

Assert old Beta5 JSON normalizes to startup/tray enabled; new values round-trip; enable writes the exact quoted command; disable removes only FolderThemeStudio's value; foreign/mismatched values are not deleted as owned state.

- [ ] **Step 2: Write failing window lifecycle tests**

Test close cancellation plus Hide when enabled, normal close when disabled, double-click/menu reopen and activate, pause/resume forwarding, explicit Exit disposal/shutdown, and `--background` startup not showing the window/tutorial.

- [ ] **Step 3: Run focused tests and verify RED**

Run: `dotnet test FolderThemeStudio.sln --no-restore --filter "FullyQualifiedName~AppSettingsTests|FullyQualifiedName~SettingsViewModelTests|FullyQualifiedName~StartupRegistrationServiceTests|FullyQualifiedName~TrayLifecycleTests|FullyQualifiedName~MainWindowTests"`

- [ ] **Step 4: Implement adapters and explicit application lifetime**

Enable Windows Forms in the App project, create/finally-dispose `NotifyIcon`, set WPF shutdown mode to explicit, intercept `Closing`, and add an `isExplicitExit` guard. Parse `--background` before showing UI. Explicit Exit disposes monitor, tray, and view model, then calls `Application.Shutdown()`.

- [ ] **Step 5: Add settings controls and save side effects**

Show localized checkboxes under General. Saving settings updates the current-user Run value immediately and updates window close behavior without restart. Paused monitoring persists through the coordinator/store.

- [ ] **Step 6: Run focused tests and verify GREEN**

Run the Step 3 command. Expected: all selected tests pass.

- [ ] **Step 7: Commit**

```powershell
git add src/FolderThemeStudio.App tests/FolderThemeStudio.Core.Tests/Application
git commit -m "feat: run monitoring from the notification area"
```

---

### Task 7: Beta6 Documentation and Release Artifacts

**Files:**
- Modify: `src/FolderThemeStudio.App/FolderThemeStudio.App.csproj`
- Modify: `build/FolderThemeStudio.iss`
- Modify: `build/Package-Release.ps1`
- Modify: `README.md`
- Modify: `CHANGELOG.md`
- Modify: `docs/USER-GUIDE.zh-CN.md`
- Modify: `docs/USER-GUIDE.en-US.md`
- Create: `docs/releases/v0.1.0-beta.6.md`
- Modify: `tests/FolderThemeStudio.Core.Tests/Application/ReleaseConfigurationTests.cs`

**Interfaces:**
- Produces `FolderThemeStudio-v0.1.0-beta.6-setup.exe`, setup checksum, source ZIP, and source checksum.

- [ ] **Step 1: Write failing Beta6 metadata tests**

Assert product version `0.1.0-beta.6`, file version `0.1.0.6`, package default `v0.1.0-beta.6`, tray dependency configuration, and installer uninstall/startup cleanup declarations.

- [ ] **Step 2: Run release tests and verify RED**

Run: `dotnet test FolderThemeStudio.sln -c Release --no-restore --filter "FullyQualifiedName~ReleaseConfigurationTests"`

- [ ] **Step 3: Update bilingual docs and metadata**

Document safe restyling of existing folders, exact recovery, recursive inheritance, monitor pause/removal, close-to-tray behavior, explicit exit, sign-in startup, and how to disable both preferences. Update installer defaults to Beta6 and ensure uninstall removes the owned Run value without touching monitoring rules unless the user chooses data removal.

- [ ] **Step 4: Run complete verification**

Run: `dotnet test FolderThemeStudio.sln -c Release --no-restore`

Expected: zero failed tests.

- [ ] **Step 5: Build and verify release artifacts**

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\build\Package-Release.ps1 -Version v0.1.0-beta.6 -DotNetPath C:\tmp\folder-theme-dotnet8\dotnet.exe -InnoCompilerPath "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"
```

Verify setup below 25 MiB, no forbidden/source `desktop.ini` entries, and fresh SHA-256 matches both checksum files.

- [ ] **Step 6: Commit**

```powershell
git add src/FolderThemeStudio.App/FolderThemeStudio.App.csproj build README.md CHANGELOG.md docs tests/FolderThemeStudio.Core.Tests/Application/ReleaseConfigurationTests.cs
git commit -m "release: prepare Folder Theme Studio beta6"
```

---

## Final Verification Checklist

- [ ] An already styled ordinary folder can be restyled without losing unrelated Desktop.ini content.
- [ ] Latest restore recreates the exact prior Desktop.ini bytes and attributes.
- [ ] New folders at every depth under configured roots inherit the current rule.
- [ ] Duplicate watcher events cause one effective write per burst.
- [ ] Paused, missing, replaced, and removed rules behave deterministically after restart.
- [ ] Closing hides to tray; Open restores; Exit alone terminates monitoring and process.
- [ ] `--background` sign-in launch restores rules without opening main window or tutorial.
- [ ] Full Release suite, package verification, source archive exclusions, and hashes pass.
