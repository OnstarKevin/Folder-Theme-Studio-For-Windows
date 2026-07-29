# Beta 2 Recovery Fixes Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fix the two remaining recovery-reporting defects and publish verified `v0.1.0-beta.2` source and Windows assets.

**Architecture:** Compatibility apply propagates unresolved, untracked mutations to the coordinator so automatic rollback cannot be marked complete. Restore failure construction retains all per-path outcomes already produced before a later refresh or marker failure. Public documentation and the packager move to Beta 2 after regression tests pass.

**Tech Stack:** .NET 8, C# 12, xUnit, WPF, PowerShell.

## Global Constraints

- Do not touch real registry keys, personal folders, Shell settings, or launch the GUI.
- Existing or replacement `Desktop.ini` files must never be deleted after a hash mismatch.
- A durable restore-completion marker is forbidden while any mutation is unresolved.
- Preserve `Successful`, `Skipped`, and `Changed` records on every post-restore failure.
- Release version is exactly `v0.1.0-beta.2`; Beta 1 artifacts remain untouched.

---

### Task 1: Fail-closed compatibility compensation and durable rollback reporting

**Files:**
- Modify: `src/FolderThemeStudio.Core/SystemIntegration/CompatibleFolderService.cs`
- Modify: `src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/SystemIntegration/CompatibleFolderServiceTests.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/Application/ThemeApplicationServiceTests.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/TestSupport/ApplicationFixture.cs`

**Interfaces:**
- Extend `CompatibleApplySummary` with `IReadOnlyList<CompatibleApplyOutcome> Unresolved`.
- Extend the internal `MutationResult` with `IReadOnlyList<CompatibleApplyOutcome> Unresolved`.
- Automatic rollback consumes that collection and refuses `MarkCompletedAsync` when it is non-empty.

- [ ] **Step 1: Add failing service regression test**

Add a deletion fake that returns `OwnedFileDeletionOutcome.HashMismatch` after the first backup append fails. Assert the replacement file remains, `result.Failed == 1`, and `result.Unresolved` contains the exact folder path.

- [ ] **Step 2: Confirm the service test is RED**

Run:

```powershell
dotnet test tests\FolderThemeStudio.Core.Tests -c Release --filter "Apply_WhenInitialOwnershipAppendFailsAndCleanupHashMismatches"
```

Expected: compile failure because the unresolved outcome contract is absent, or assertion failure because the deletion result is ignored.

- [ ] **Step 3: Implement service propagation**

In the initial append-failure branch, inspect `DeleteIfHashMatches`. Treat `Deleted` and `Missing` as resolved cleanup. For `HashMismatch`, deletion exceptions, or a failed second append, add a failed `CompatibleApplyOutcome` to the unresolved collection and keep `hasUntrackedMutation = true`. Never modify the replacement file.

- [ ] **Step 4: Add failing coordinator rollback test**

Configure the compatibility fake to return one unresolved path. Assert `ApplyResult.RollbackAttempted` is true, `RollbackSucceeded` is false, the exact path appears in failures, and the fake restore lease never marks completion.

- [ ] **Step 5: Confirm coordinator test is RED**

Run:

```powershell
dotnet test tests\FolderThemeStudio.Core.Tests -c Release --filter "FailedCompatibleApply_WithUnresolvedMutation_DoesNotMarkRollbackComplete"
```

Expected: FAIL because rollback currently marks an empty recorded snapshot complete.

- [ ] **Step 6: Implement coordinator fail-closed rollback**

Propagate unresolved paths through the internal mutation result. After recorded rollback succeeds, merge an `ApplyPhase.Rollback` failure for every unresolved path, keep `RollbackSucceeded = false`, and skip `MarkCompletedAsync`. Preserve existing behavior when the unresolved list is empty.

- [ ] **Step 7: Run focused tests and commit**

```powershell
dotnet test tests\FolderThemeStudio.Core.Tests -c Release --filter "CompatibleFolderServiceTests|ThemeApplicationServiceTests"
git add src/FolderThemeStudio.Core tests/FolderThemeStudio.Core.Tests
git commit -m "fix: fail closed on unresolved folder rollback"
```

Expected: focused tests pass.

### Task 2: Preserve per-path outcomes after restore-stage failures

**Files:**
- Modify: `src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs`
- Modify: `tests/FolderThemeStudio.Core.Tests/Application/ThemeApplicationServiceTests.cs`

**Interfaces:**
- Add a private helper that creates a failed `RestoreResult` from an existing `RestoreOperationResult`, retaining `Successful`, `Skipped`, and `Changed`.

- [ ] **Step 1: Add failing outcome-preservation tests**

Configure compatibility restore with one successful, one skipped, and one changed path. Add separate tests for a returned refresh failure, a thrown refresh exception, and completion-marker failure. Each result must be unsuccessful while retaining all three path collections.

- [ ] **Step 2: Confirm tests are RED**

```powershell
dotnet test tests\FolderThemeStudio.Core.Tests -c Release --filter "FullyQualifiedName~PreservesCompletedOutcomes"
```

Expected: FAIL because current post-restore failure branches return empty path collections.

- [ ] **Step 3: Implement one result-construction path**

Replace post-restore calls to the pathless `RestoreFailure` helper with a helper accepting `RestoreOperationResult restored`, failure phase, and message. Set `Successful = restored.Successful`, `Skipped = restored.Skipped`, and `Changed = restored.Changed` for cancellation, exceptions, returned refresh failure, and marker failure.

- [ ] **Step 4: Run focused and full tests, then commit**

```powershell
dotnet test tests\FolderThemeStudio.Core.Tests -c Release --filter ThemeApplicationServiceTests
dotnet test FolderThemeStudio.sln -c Release --no-restore
git add src/FolderThemeStudio.Core/Application/ThemeApplicationService.cs tests/FolderThemeStudio.Core.Tests/Application/ThemeApplicationServiceTests.cs
git commit -m "fix: preserve partial restore outcomes"
```

Expected: focused and full Release tests pass with zero failures.

### Task 3: Update and publish Beta 2 artifacts

**Files:**
- Modify: `README.md`
- Modify: `CHANGELOG.md`
- Modify: `CONTRIBUTING.md`
- Modify: `build/Package-Release.ps1`
- Create: `docs/releases/v0.1.0-beta.2.md`
- Generate ignored: `artifacts/FolderThemeStudio-v0.1.0-beta.2-win-x64.zip`
- Generate ignored: `artifacts/FolderThemeStudio-v0.1.0-beta.2-win-x64.sha256`
- Generate ignored: `artifacts/FolderThemeStudio-v0.1.0-beta.2-source.zip`
- Generate ignored: `artifacts/FolderThemeStudio-v0.1.0-beta.2-source.sha256`

**Interfaces:**
- `build/Package-Release.ps1` defaults to `v0.1.0-beta.2` and consumes `docs/releases/v0.1.0-beta.2.md`.

- [ ] **Step 1: Update public version and release notes**

Change current download/package commands to Beta 2. Add a Beta 2 changelog entry describing both recovery fixes. The Beta 2 release notes must retain the unsigned-binary and unperformed manual-verification warnings, but must not list the fixed defects as current known issues.

- [ ] **Step 2: Verify version consistency**

```powershell
rg -n "v0\.1\.0-beta\.1" README.md CONTRIBUTING.md build docs/releases/v0.1.0-beta.2.md
rg -n "v0\.1\.0-beta\.2" README.md CHANGELOG.md CONTRIBUTING.md build docs/releases/v0.1.0-beta.2.md
```

Expected: no Beta 1 references in current commands; Beta 2 appears in every current release file. Historical Beta 1 release notes remain unchanged.

- [ ] **Step 3: Commit Beta 2 metadata**

```powershell
git add README.md CHANGELOG.md CONTRIBUTING.md build/Package-Release.ps1 docs/releases/v0.1.0-beta.2.md
git commit -m "release: prepare v0.1.0 beta2"
```

- [ ] **Step 4: Run final Release verification and package**

```powershell
dotnet test FolderThemeStudio.sln -c Release --no-restore
powershell -NoProfile -ExecutionPolicy Bypass -File .\build\Package-Release.ps1 -Version v0.1.0-beta.2 -DotNetPath C:\tmp\folder-theme-dotnet8\dotnet.exe -SkipTests
git archive --format=zip --output=artifacts\FolderThemeStudio-v0.1.0-beta.2-source.zip HEAD
```

Calculate and write the source ZIP SHA-256. Verify both hashes, required release entries, executable `MZ` header, no bundled `.deps.json`/`.runtimeconfig.json`, no ignored/internal entries in the source ZIP, and a clean Git status.
