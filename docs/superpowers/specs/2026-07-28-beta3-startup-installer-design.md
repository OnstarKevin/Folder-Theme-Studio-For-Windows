# Folder Theme Studio Beta 3 Startup and Online Installer Design

## Goal

Ship a usable Beta 3 that fixes the WPF startup crash and replaces the large self-contained executable with a small framework-dependent Windows installer that installs the required .NET 8 Desktop Runtime when missing.

## Startup fix

`ProgressBar.Value` in `MainWindow.xaml` must bind to `MainViewModel.ProgressCompleted` with `Mode=OneWay`. A regression test must inspect the real XAML and fail if this binding is absent or writable again. The complete Release suite must remain green.

## Publishing model

Publish `win-x64` as a framework-dependent single-file WPF application. Disable ReadyToRun and trimming. The application remains x64-only and targets `net8.0-windows10.0.19041.0`. The resulting executable must contain application code but not the .NET runtime.

## Online installer

Use stable Inno Setup 6 to produce one unsigned Beta 3 setup executable. Install per machine under Program Files, create Start Menu and optional desktop shortcuts, and register an uninstaller.

Before the application can be launched, the installer checks the standard x64 .NET installation directory for a compatible `Microsoft.WindowsDesktop.App` 8.x runtime. If absent, it downloads a pinned Microsoft .NET 8 Desktop Runtime x64 installer over HTTPS, verifies the expected SHA-256 recorded by the release build, and runs it with `/install /quiet /norestart`. Exit codes `0` and `3010` are successful; every other result aborts with a clear message. A failed download or verification must never launch or advertise a partially usable application.

The setup requires internet access and administrator approval when .NET must be installed. The runtime remains installed if Folder Theme Studio is later uninstalled because other applications may use it.

## Release output

Release as `v0.1.0-beta.3`. Produce:

- `FolderThemeStudio-v0.1.0-beta.3-setup.exe`
- its lowercase SHA-256 record
- a clean source ZIP and SHA-256 record
- Beta 3 release notes and updated README, changelog, contribution, security, and packaging documentation

The old standalone Beta 2 artifacts remain untouched.

## Verification

Use test-driven development: first demonstrate that the XAML regression test fails, then add the one-way binding and confirm it passes. Run the entire Release suite. Inspect the published executable and setup metadata, confirm the installer contains only the framework-dependent app payload, verify all hashes, compare Beta 2 and Beta 3 sizes, and keep the Git worktree clean. Do not launch the GUI, install the setup, download/install the runtime on the user's system, or modify real folder/registry state during automated verification.
