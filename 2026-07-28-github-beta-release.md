# Folder Theme Studio GitHub Beta Release Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Turn the existing Folder Theme Studio branch into an Apache-2.0 licensed GitHub-ready repository and create a reproducible `v0.1.0-beta.1` Windows x64 release bundle.

**Architecture:** Repository metadata and public documentation describe the beta honestly; GitHub Actions reproduce Release tests and publishing on Windows; a source-controlled PowerShell packager creates the same ZIP and SHA-256 assets locally. Large binaries remain outside Git under ignored `artifacts/`.

**Tech Stack:** .NET 8, C# 12, WPF, PowerShell 7/Windows PowerShell, GitHub Actions YAML, Markdown, Apache License 2.0.

## Global Constraints

- Release version is exactly `v0.1.0-beta.1` and is identified as a prerelease.
- License is Apache-2.0 using the unmodified official license text.
- Do not commit the approximately 239 MB executable or generated release ZIP.
- Preserve the two accepted recovery limitations in README, changelog, release notes, and issue-report guidance.
- Do not claim manual Windows 10/11, Explorer, DPI, GUI, accessibility, registry, or real-folder verification.
- Do not launch the GUI or mutate the registry, user folders, or Shell settings during packaging.
- GitHub automation must build and test on `windows-latest` and publish self-contained `win-x64` assets only for version tags.
- Binaries are unsigned; documentation and release notes must say so.

---

### Task 1: Public repository metadata and documentation

**Files:**
- Create: `LICENSE`
- Create: `CHANGELOG.md`
- Create: `CONTRIBUTING.md`
- Create: `SECURITY.md`
- Create: `.editorconfig`
- Create: `.gitattributes`
- Create: `.github/ISSUE_TEMPLATE/bug_report.yml`
- Create: `.github/ISSUE_TEMPLATE/config.yml`
- Create: `docs/releases/v0.1.0-beta.1.md`
- Modify: `.gitignore`
- Modify: `README.md`

**Interfaces:**
- Consumes: accepted release design and existing safety checklist.
- Produces: public-facing repository contract, issue-report schema, and release-note file consumed by Task 2 and Task 3.

- [ ] **Step 1: Add a failing repository-policy check**

Run this PowerShell assertion before creating the files:

```powershell
$required = @(
  'LICENSE', 'CHANGELOG.md', 'CONTRIBUTING.md', 'SECURITY.md',
  '.editorconfig', '.gitattributes',
  '.github/ISSUE_TEMPLATE/bug_report.yml',
  '.github/ISSUE_TEMPLATE/config.yml',
  'docs/releases/v0.1.0-beta.1.md'
)
$missing = $required | Where-Object { -not (Test-Path -LiteralPath $_) }
if ($missing.Count -eq 0) { throw 'Expected repository metadata to be absent before implementation.' }
```

Expected: PASS as a red-state assertion because required files are currently absent.

- [ ] **Step 2: Add license and repository text/config files**

Use the official Apache License 2.0 text beginning with:

```text
Apache License
Version 2.0, January 2004
http://www.apache.org/licenses/
```

Add `.editorconfig` defaults for UTF-8, final newlines, four-space C# indentation, two-space YAML indentation, and CRLF for PowerShell. Add `.gitattributes` with `* text=auto`, LF for Markdown/YAML/JSON, CRLF for `*.ps1`, and binary declarations for `*.ico`, `*.png`, `*.zip`, and `*.exe`.

Append these ignored outputs to `.gitignore`:

```gitignore
artifacts/
*.nupkg
*.snupkg
```

- [ ] **Step 3: Rewrite the README and release documentation**

README sections must appear in this order: beta warning, features, screenshots/manual-verification status, scope exclusions, download/run, two apply modes, safety/recovery, known issues, build/test/package, contributing/security/license. Include both accepted recovery limitations verbatim in meaning, an unsigned-binary warning, and links to all repository documents.

`CHANGELOG.md` uses Keep a Changelog headings and contains `[0.1.0-beta.1] - 2026-07-28`. `docs/releases/v0.1.0-beta.1.md` identifies the release as prerelease, lists automated verification separately from unperformed manual checks, and names both accepted issues. `SECURITY.md` says only the beta version is currently supported and asks reporters not to include unrelated personal paths or live backup snapshots.

- [ ] **Step 4: Add privacy-conscious issue templates**

`bug_report.yml` must request Windows version/build, app version, selected mode, Explorer view/DPI when relevant, sanitized steps, sanitized failure text, and confirmation that personal paths/backups were removed. It must not ask users to upload live recovery snapshots. Disable blank issues in `config.yml`.

- [ ] **Step 5: Validate public repository policy**

Run:

```powershell
$required = @('LICENSE','CHANGELOG.md','CONTRIBUTING.md','SECURITY.md','.editorconfig','.gitattributes','.github/ISSUE_TEMPLATE/bug_report.yml','.github/ISSUE_TEMPLATE/config.yml','docs/releases/v0.1.0-beta.1.md')
$missing = $required | Where-Object { -not (Test-Path -LiteralPath $_) }
if ($missing) { throw "Missing: $($missing -join ', ')" }
rg -n 'v0\.1\.0-beta\.1|Apache-2\.0|unsigned|未验证|unverified|known issue|Known issue' README.md CHANGELOG.md SECURITY.md CONTRIBUTING.md docs/releases/v0.1.0-beta.1.md
rg -n 'C:\\Users\\|daiha' README.md CHANGELOG.md CONTRIBUTING.md SECURITY.md .github docs/releases
git diff --check
```

Expected: required files exist; positive scan finds release/licensing/safety wording; personal-path/placeholder scan finds no matches; `git diff --check` exits 0.

- [ ] **Step 6: Commit repository metadata**

```powershell
git add LICENSE CHANGELOG.md CONTRIBUTING.md SECURITY.md .editorconfig .gitattributes .gitignore README.md .github/ISSUE_TEMPLATE docs/releases/v0.1.0-beta.1.md
git commit -m "docs: prepare public beta repository"
```

### Task 2: GitHub CI and tagged prerelease automation

**Files:**
- Create: `.github/workflows/ci.yml`
- Create: `.github/workflows/release.yml`

**Interfaces:**
- Consumes: solution, app project, Apache license, and `docs/releases/v0.1.0-beta.1.md`.
- Produces: reproducible CI validation and GitHub prerelease assets named `FolderThemeStudio-v0.1.0-beta.1-win-x64.zip` and `.sha256`.

- [ ] **Step 1: Write workflow-contract assertions and confirm RED**

```powershell
if (Test-Path '.github/workflows/ci.yml') { throw 'CI workflow unexpectedly exists.' }
if (Test-Path '.github/workflows/release.yml') { throw 'Release workflow unexpectedly exists.' }
```

Expected: PASS because both workflows are absent.

- [ ] **Step 2: Create the CI workflow**

Create `.github/workflows/ci.yml` with `pull_request` and pushes to `main`, `workflow_dispatch`, `permissions: contents: read`, `windows-latest`, `actions/checkout`, `actions/setup-dotnet` with `dotnet-version: 8.0.x`, then:

```powershell
dotnet restore FolderThemeStudio.sln
dotnet test FolderThemeStudio.sln -c Release --no-restore
dotnet build src/FolderThemeStudio.App/FolderThemeStudio.App.csproj -c Release --no-restore
```

- [ ] **Step 3: Create the tag-triggered prerelease workflow**

Create `.github/workflows/release.yml` triggered by `v*` tags and `workflow_dispatch` with a required version input. Grant `contents: write`, validate that the chosen version equals `v0.1.0-beta.1` for this release, run Release tests, publish self-contained `win-x64`, copy `LICENSE`, `README.md`, and the release notes into staging, compress staging, calculate SHA-256 with `Get-FileHash`, upload both assets, then use `softprops/action-gh-release` with:

```yaml
prerelease: true
body_path: docs/releases/v0.1.0-beta.1.md
files: |
  artifacts/FolderThemeStudio-v0.1.0-beta.1-win-x64.zip
  artifacts/FolderThemeStudio-v0.1.0-beta.1-win-x64.sha256
```

Pin third-party actions to stable major versions and keep the token limited to this release job.

- [ ] **Step 4: Validate workflow structure**

Use the bundled Python YAML library if available, then run content assertions:

```powershell
python -c "import yaml; [yaml.safe_load(open(p, encoding='utf-8')) for p in ['.github/workflows/ci.yml','.github/workflows/release.yml']]"
rg -n 'windows-latest|dotnet test|dotnet publish|prerelease: true|contents: write|sha256' .github/workflows
git diff --check
```

Expected: YAML parses, required workflow terms are present, and whitespace validation passes.

- [ ] **Step 5: Commit GitHub automation**

```powershell
git add .github/workflows
git commit -m "ci: add Windows beta release workflows"
```

### Task 3: Reproducible local packager and final beta artifacts

**Files:**
- Create: `build/Package-Release.ps1`
- Modify: `CONTRIBUTING.md`
- Modify: `README.md`
- Generate ignored: `artifacts/FolderThemeStudio-v0.1.0-beta.1-win-x64.zip`
- Generate ignored: `artifacts/FolderThemeStudio-v0.1.0-beta.1-win-x64.sha256`

**Interfaces:**
- Consumes: `dotnet`, app project, license, README, and release notes.
- Produces: `build/Package-Release.ps1 -Version v0.1.0-beta.1 -DotNetPath C:\tmp\folder-theme-dotnet8\dotnet.exe` and final GitHub Release upload assets.

- [ ] **Step 1: Define the packager contract and confirm RED**

```powershell
if (Test-Path 'build/Package-Release.ps1') { throw 'Packager unexpectedly exists.' }
```

Expected: PASS because the packager is absent.

- [ ] **Step 2: Implement the fail-fast packager**

The script parameters are:

```powershell
param(
  [ValidatePattern('^v\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?$')]
  [string]$Version = 'v0.1.0-beta.1',
  [string]$DotNetPath = 'dotnet',
  [switch]$SkipTests
)
```

Resolve the repository root from `$PSScriptRoot`, reject a dirty source prerequisite only when tracked files needed for packaging are missing, create `artifacts/.staging-$Version-win-x64`, run Release tests unless `-SkipTests`, publish to staging with `--self-contained true`, verify `FolderThemeStudio.App.exe`, copy `LICENSE`, `README.md`, and version release notes, create a `START-HERE.txt` with beta/unsigned/known-issue warnings, remove any existing same-version ZIP/checksum, compress staging, write lowercase SHA-256 plus two spaces plus filename, validate the checksum, and remove only the validated staging directory in `finally`.

The script must resolve and validate every deletion target beneath the repository's `artifacts` directory before `Remove-Item -Recurse`.

- [ ] **Step 3: Document local packaging**

README and CONTRIBUTING must show:

```powershell
& .\build\Package-Release.ps1 -Version v0.1.0-beta.1
```

They must explain that generated files are ignored by Git and should be attached to a GitHub prerelease rather than committed.

- [ ] **Step 4: Run source verification**

```powershell
$env:DOTNET_CLI_HOME = 'C:\tmp\folder-theme-dotnet-home'
$env:NUGET_PACKAGES = 'C:\tmp\folder-theme-nuget'
& 'C:\tmp\folder-theme-dotnet8\dotnet.exe' test FolderThemeStudio.sln -c Release --no-restore
```

Expected: 172 tests pass, 0 fail, 0 skip, unless later documentation-only work leaves the same test count.

- [ ] **Step 5: Create and inspect the final artifacts**

```powershell
& .\build\Package-Release.ps1 -Version v0.1.0-beta.1 -DotNetPath 'C:\tmp\folder-theme-dotnet8\dotnet.exe' -SkipTests
$zip = Resolve-Path 'artifacts\FolderThemeStudio-v0.1.0-beta.1-win-x64.zip'
$hash = (Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash.ToLowerInvariant()
$recorded = (Get-Content 'artifacts\FolderThemeStudio-v0.1.0-beta.1-win-x64.sha256' -Raw).Split(' ', [System.StringSplitOptions]::RemoveEmptyEntries)[0]
if ($hash -ne $recorded) { throw 'SHA-256 mismatch.' }
Add-Type -AssemblyName System.IO.Compression.FileSystem
$archive = [System.IO.Compression.ZipFile]::OpenRead($zip)
try {
  $names = $archive.Entries.FullName
  foreach ($required in @('FolderThemeStudio.App.exe','LICENSE','README.md','START-HERE.txt','RELEASE-NOTES.md')) {
    if ($names -notcontains $required) { throw "Missing archive entry: $required" }
  }
  if ($names -match '\.deps\.json$|\.runtimeconfig\.json$') { throw 'Unexpected framework metadata beside single-file bundle.' }
} finally { $archive.Dispose() }
```

Expected: checksum matches; all required entries exist; no app `.deps.json` or `.runtimeconfig.json` entry exists.

- [ ] **Step 6: Commit packager and final documentation**

```powershell
git add build/Package-Release.ps1 README.md CONTRIBUTING.md
git commit -m "build: add reproducible beta packager"
```

- [ ] **Step 7: Final repository and artifact verification**

```powershell
git status --short --branch
git check-ignore artifacts/FolderThemeStudio-v0.1.0-beta.1-win-x64.zip
git check-ignore artifacts/FolderThemeStudio-v0.1.0-beta.1-win-x64.sha256
git diff --check HEAD~3..HEAD
```

Expected: branch is clean; both artifacts are ignored; no whitespace errors exist in the release-preparation commits.
