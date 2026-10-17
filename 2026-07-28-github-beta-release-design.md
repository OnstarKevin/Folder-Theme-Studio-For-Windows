# Folder Theme Studio GitHub Beta Release Design

## Goal

Prepare Folder Theme Studio as an Apache-2.0 licensed GitHub repository and produce a reproducible `v0.1.0-beta.1` Windows release bundle. The repository must be suitable for public review while clearly identifying the two accepted recovery limitations and all unperformed manual Windows checks.

## Release Positioning

- Version: `v0.1.0-beta.1`.
- Status: prerelease/beta, not a stable production release.
- Platform: Windows 10/11 x64, self-contained .NET 8 WPF application.
- The release notes and README must not claim that Windows, Explorer, DPI, GUI accessibility, registry, or real-folder behavior has been manually verified.
- The following known issues must be disclosed:
  1. An exceptionally narrow failure path can report rollback completion while an untracked or changed `Desktop.ini` remains.
  2. Per-folder restore results can be lost from the returned result if Shell refresh or durable completion-marker persistence subsequently fails.

## Repository Layout

Keep production source under `src/`, tests under `tests/`, user documentation under `docs/`, and the solution at the repository root. Add:

- `LICENSE` containing the unmodified Apache License 2.0 text.
- `CHANGELOG.md` with the beta release and known limitations.
- `CONTRIBUTING.md` with build, test, safety, and pull-request guidance.
- `SECURITY.md` with private vulnerability-reporting guidance and supported-version status.
- `.editorconfig` with conservative C# and text-file defaults.
- `.gitattributes` for consistent text normalization.
- `.github/workflows/ci.yml` for Windows Release build and test on pushes and pull requests.
- `.github/workflows/release.yml` for tag-triggered self-contained publishing and GitHub prerelease asset creation.
- `.github/ISSUE_TEMPLATE/bug_report.yml` and `config.yml` for structured, privacy-conscious reports.
- `docs/releases/v0.1.0-beta.1.md` as the release notes source.

The existing implementation specifications and plans remain in `docs/superpowers/` as engineering history.

## Artifact Policy

The approximately 239 MB executable must not be committed to Git because it exceeds GitHub's normal per-file limit. Local output goes under ignored `artifacts/` and contains:

- `FolderThemeStudio-v0.1.0-beta.1-win-x64.zip`
- `FolderThemeStudio-v0.1.0-beta.1-win-x64.sha256`
- an unpacked staging directory only while packaging

The ZIP contains the complete self-contained publish directory, the Apache-2.0 license, release notes, and a short start/readme file. SHA-256 is calculated from the final ZIP bytes. Temporary staging is removed after successful packaging; the ZIP and checksum remain available locally for upload to a GitHub Release.

## Automation

### Continuous Integration

On pushes and pull requests, a Windows runner will:

1. check out the repository;
2. install the .NET 8 SDK;
3. restore dependencies;
4. run the complete Release test suite;
5. build the WPF application in Release mode.

CI must not launch the GUI or mutate the runner's registry or user folders.

### Tagged Prerelease

Tags matching `v*` trigger the release workflow. It will run the Release tests, publish self-contained `win-x64`, stage the license and release notes, create a ZIP and SHA-256 file, and upload both to a GitHub prerelease. The workflow uses the repository-provided `GITHUB_TOKEN`; it does not require a code-signing certificate and must disclose that binaries are unsigned.

## Documentation

The README will lead with purpose, screenshots status, beta warning, ordinary-folder-only scope, build instructions, release download/use instructions, safety and recovery behavior, known issues, privacy-safe bug reporting, and links to the license/contribution/security documents.

The release notes will summarize the visual editor, quick and compatibility modes, tests, packaging, known issues, unsigned-binary warning, and unperformed manual checks. Documentation must distinguish automated verification from manual verification.

## Verification

Before delivery:

1. run the complete Release test suite;
2. validate the solution builds in Release mode;
3. publish the self-contained `win-x64` application;
4. package the ZIP and calculate SHA-256;
5. inspect the ZIP entries, executable MZ header, checksum match, and absence of `.deps.json`/`.runtimeconfig.json` for the bundled app;
6. validate YAML structure and scan all public docs for placeholders, contradictory stability claims, missing known issues, and accidental personal paths;
7. confirm `git status --short` is clean after committing repository changes, with `artifacts/` ignored.

No GUI launch or real system mutation is part of this release preparation because the user stopped GUI automation and has accepted deferring those checks.

## Out of Scope

- Fixing the two accepted recovery issues.
- Code signing, MSI/MSIX installers, Microsoft Store packaging, or SmartScreen reputation.
- Creating or pushing a remote GitHub repository.
- Publishing a GitHub Release from this environment.
- Claiming stable-release readiness.
