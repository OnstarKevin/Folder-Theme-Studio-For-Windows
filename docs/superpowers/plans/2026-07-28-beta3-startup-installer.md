# Beta 3 Startup and Online Installer Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fix the WPF startup crash and publish a small Beta 3 setup executable that installs .NET 8 Desktop Runtime when required.

**Architecture:** Protect the real XAML and publish properties with repository contract tests. Publish a framework-dependent x64 single-file app, then compile an Inno Setup web installer whose pinned Microsoft runtime download is verified before silent installation.

**Tech Stack:** .NET 8, WPF, xUnit, PowerShell, Inno Setup 6.7.x.

## Global Constraints

- Release version is `v0.1.0-beta.3`; assembly version stays `0.1.0.0` and file version becomes `0.1.0.3`.
- Target Windows 10/11 x64 and `net8.0-windows10.0.19041.0`.
- Publish framework-dependent and single-file with ReadyToRun and trimming disabled.
- Runtime prerequisite is Microsoft Windows Desktop Runtime x64 `8.0.29` from `https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/8.0.29/windowsdesktop-runtime-8.0.29-win-x64.exe`.
- Verify the downloaded prerequisite against Microsoft's published SHA-512 `02d272ee678f5bc8be522b0b8adaf2a3c9d35d1044d7737ea48e477928a839ad5344724b242b210afdc64775d8cf43655db58396fcf53dd185e23ba2b888ec44`, then record and embed its computed SHA-256.
- Do not launch the GUI, install the setup/runtime, or mutate real folders and registry state during automated verification.

---

### Task 1: Reproduce and fix the startup binding crash

**Files:**
- Create: `tests/FolderThemeStudio.Core.Tests/UiContractTests.cs`
- Modify: `src/FolderThemeStudio.App/MainWindow.xaml:225`

**Interfaces:**
- Consumes: the real `MainWindow.xaml` file.
- Produces: a regression contract requiring `ProgressCompleted` to be one-way.

- [ ] **Step 1: Write the failing regression test**

```csharp
[Fact]
public void ProgressValueBinding_IsExplicitlyOneWay()
{
    var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    var document = XDocument.Load(Path.Combine(root, "src", "FolderThemeStudio.App", "MainWindow.xaml"));
    var progress = document.Descendants().Single(element =>
        element.Name.LocalName == "ProgressBar" &&
        element.Attributes().Any(attribute => attribute.Name.LocalName == "Name" && attribute.Value == "Operation progress"));
    var value = progress.Attribute("Value")?.Value;
    Assert.Contains("ProgressCompleted", value);
    Assert.Contains("Mode=OneWay", value);
}
```

- [ ] **Step 2: Run only this test and verify RED**

Run: `dotnet test tests/FolderThemeStudio.Core.Tests/FolderThemeStudio.Core.Tests.csproj -c Release --no-restore --filter ProgressValueBinding_IsExplicitlyOneWay`

Expected: FAIL because the existing binding omits `Mode=OneWay`.

- [ ] **Step 3: Make the minimal XAML fix**

```xml
Value="{Binding ProgressCompleted, Mode=OneWay}"
```

- [ ] **Step 4: Verify GREEN and commit**

Run the focused test, then the complete Release suite. Commit `fix: prevent progress binding startup crash`.

---

### Task 2: Build the framework-dependent online installer

**Files:**
- Create: `build/FolderThemeStudio.iss`
- Create: `tests/FolderThemeStudio.Core.Tests/ReleaseContractTests.cs`
- Modify: `src/FolderThemeStudio.App/FolderThemeStudio.App.csproj`
- Modify: `build/Package-Release.ps1`

**Interfaces:**
- Consumes: framework-dependent publish output, Inno `ISCC.exe`, the pinned runtime URL and both hashes.
- Produces: `artifacts/FolderThemeStudio-v0.1.0-beta.3-setup.exe` and `.sha256`.

- [ ] **Step 1: Write failing release contract tests**

Parse the real `.csproj` and assert `SelfContained=false`, `PublishSingleFile=true`, `PublishReadyToRun=false`, `PublishTrimmed=false`, `Version=0.1.0-beta.3`, and `FileVersion=0.1.0.3`. Read `build/FolderThemeStudio.iss` and assert it contains the pinned runtime URL, `DownloadTemporaryFile`, `/install /quiet /norestart`, success codes `0` and `3010`, and the `Microsoft.WindowsDesktop.App` 8.x detection path.

- [ ] **Step 2: Run the release tests and verify RED**

Expected: FAIL on the old self-contained project properties and missing `.iss` file.

- [ ] **Step 3: Change publish properties**

```xml
<SelfContained>false</SelfContained>
<PublishSingleFile>true</PublishSingleFile>
<PublishReadyToRun>false</PublishReadyToRun>
<PublishTrimmed>false</PublishTrimmed>
<Version>0.1.0-beta.3</Version>
<FileVersion>0.1.0.3</FileVersion>
```

- [ ] **Step 4: Add the Inno Setup definition**

Use `ArchitecturesAllowed=x64compatible`, `ArchitecturesInstallIn64BitMode=x64compatible`, `PrivilegesRequired=admin`, `{autopf}\Folder Theme Studio`, LZMA2 compression, Start Menu and optional desktop shortcuts. In `PrepareToInstall`, enumerate `{autopf}\dotnet\shared\Microsoft.WindowsDesktop.App\8.*`; if absent, call `DownloadTemporaryFile(RuntimeUrl, RuntimeFileName, RuntimeSha256, @OnDownloadProgress)`, execute the downloaded file with `/install /quiet /norestart`, and accept only exit code `0` or `3010` before rechecking the runtime directory.

- [ ] **Step 5: Rewrite the package script**

Publish with `--self-contained false /p:PublishSingleFile=true /p:PublishReadyToRun=false`. Require an `-InnoCompilerPath`, verify/download the pinned runtime prerequisite beneath `[IO.Path]::GetTempPath()`, compare its SHA-512 with Microsoft's release metadata value, compute SHA-256, and invoke `ISCC.exe` with `/DAppVersion=0.1.0-beta.3`, `/DSourceDir=$stagingPath`, `/DOutputDir=$artifactsRoot`, `/DRuntimeUrl=$runtimeUrl`, and `/DRuntimeSha256=$runtimeSha256`. Hash the final setup and remove only validated staging/temp paths.

- [ ] **Step 6: Verify contracts, compile, inspect, and commit**

Run focused and full Release tests. Install stable Inno Setup 6 only after approval, compile without launching the setup, verify `MZ`, version metadata, embedded app payload, checksum, and that setup size is smaller than the Beta 2 ZIP. Commit `build: add lightweight beta3 online installer`.

---

### Task 3: Refresh release documentation and source package

**Files:**
- Create: `docs/releases/v0.1.0-beta.3.md`
- Modify: `README.md`
- Modify: `CHANGELOG.md`
- Modify: `CONTRIBUTING.md`
- Modify: `SECURITY.md`

**Interfaces:**
- Consumes: verified setup name, size, runtime requirement, and hashes.
- Produces: upload-ready setup/source artifacts and accurate Beta 3 instructions.

- [ ] **Step 1: Document Beta 3**

State that the startup crash is fixed, the setup downloads Microsoft .NET 8 Desktop Runtime 8.0.29 only when missing, internet/admin access may be required, binaries are unsigned, and the runtime is not removed during uninstall.

- [ ] **Step 2: Generate release outputs**

Create the setup checksum, `FolderThemeStudio-v0.1.0-beta.3-source.zip`, and its checksum. Exclude `.git`, `.worktrees`, `.superpowers`, `artifacts`, `bin`, and `obj` from the source archive.

- [ ] **Step 3: Perform final verification**

Run all Release tests, `git diff --check`, verify recorded hashes against both files, inspect ZIP entries, compare sizes, and confirm `git status --short --branch` is clean after committing `release: prepare v0.1.0 beta3`.
