# Persistent Folder Monitoring and Tray Background Design

## Goal

Allow compatible mode to update folders that already contain `desktop.ini`, preserve unrelated folder metadata, recursively apply the selected icon to newly created folders under chosen roots, restore monitoring after sign-in, and keep the application running in the Windows notification area when its main window is closed.

## Existing Behavior and Root Cause

Compatible planning currently classifies every folder containing `desktop.ini` as `ExistingCustomization`. The apply service performs the same check immediately before mutation. Consequently, a folder changed by Folder Theme Studio becomes ineligible for a later style change because the application cannot distinguish its own file from another customization.

The new behavior removes `ExistingCustomization` as a skip reason for compatible-mode icon updates. Existing configuration is merged instead of erased.

## Desktop.ini Merge and Recovery

- Folders that pass the existing special-folder, protected-root, network, reparse-point, and write-access checks remain eligible even when `desktop.ini` exists.
- When no `desktop.ini` exists, create one containing the selected icon configuration.
- When `desktop.ini` exists, preserve all unrelated sections, keys, comments, encoding where safely detectable, and line endings. Replace or add only icon-related values in `[.ShellClassInfo]`: `IconResource`, `IconFile`, and `IconIndex`.
- Use a deterministic merge component that returns the new bytes and an explicit failure if the document cannot be safely parsed. A parse failure leaves the original file untouched and is reported as a per-folder failure.
- Before writing, persist the original file bytes, attributes, folder attributes, and whether the file originally existed. Writes remain atomic.
- Recovery restores the exact original bytes and attributes for pre-existing files. Files created by Folder Theme Studio are deleted only when their recorded hash still matches. Folder attributes added by the application are removed during recovery.
- Reapplying a style creates a new recovery operation, so restoring the latest operation returns every affected folder to its immediately previous state.

## Persistent Recursive Monitoring

- Compatible-mode application exposes a `Continue applying to new folders` option. When enabled and an apply succeeds, each explicit root receives a persistent monitoring rule associated with the applied icon source.
- A rule stores the canonical root path and a durable icon asset path. Built-in themes are rendered to a durable ICO before the rule is saved; imported images use the generated durable ICO. Rules never depend on transient preview state.
- Monitoring uses recursive `FileSystemWatcher` instances with directory creation notifications only. Events are normalized, de-duplicated, and queued.
- A new directory is processed after a short readiness delay. Transient sharing, attribute, or creation-order errors are retried with bounded delays. A path is applied at most once per event burst.
- Newly created folders at any depth under a monitored root receive the rule's current icon configuration through the same merge service used by manual compatible apply.
- Reapplying a different icon to an existing monitored root replaces that root's rule. Removing a root from persistent monitoring stops its watcher without altering folders already changed.
- Missing or temporarily inaccessible roots remain saved but inactive. The monitor periodically attempts to attach again and exposes the inactive state to the UI.
- Automatic applications write concise success/failure history for display in the existing results area. They do not show modal error dialogs for individual folders.

## Persistence Model

- Store monitoring rules in `%LOCALAPPDATA%\FolderThemeStudio\monitoring-rules.json` using versioned JSON and atomic replacement.
- Reject malformed rules individually, retain valid rules, and surface diagnostics in the main window.
- Store user preferences for `Start with Windows`, `Close to tray`, and `Monitoring paused` in the existing app settings document.
- Pausing monitoring is persistent but does not delete rules.

## Notification Area and Window Lifecycle

- Closing the main window hides it instead of shutting down when `Close to tray` is enabled. The first hide displays a notification that recursive monitoring remains active.
- A notification-area icon supports double-click to show and activate the main window.
- Its menu contains `Open Folder Theme Studio`, `Pause/Resume monitoring`, and `Exit`.
- `Exit` is the only tray action that disposes watchers, view models, and the tray icon before calling application shutdown.
- Settings exposes `Start with Windows` and `Close to tray`, both enabled by default for this release.
- The application uses explicit shutdown mode so hiding the last window does not terminate the process.

## Sign-in Startup

- Manage a current-user startup value under `HKCU\Software\Microsoft\Windows\CurrentVersion\Run`; no administrator privilege is required.
- The startup command quotes the executable path and adds `--background`.
- A background launch initializes settings and monitoring, creates the tray icon, and does not show the main window or first-run tutorial.
- Disabling the preference removes only the exact value owned by Folder Theme Studio.
- Normal interactive launch continues to show the main window.

## Main Window Changes

- In compatible mode, show the `Continue applying to new folders` option near the explicit root list.
- Show monitoring state: running, paused, inactive roots, and the number of active rules.
- Provide a management action to remove selected roots from persistent monitoring.
- Automatic monitoring outcomes feed the existing result list without interrupting current manual operations.

## Error Handling

- Unsafe or unparseable `desktop.ini` files are never overwritten.
- Watcher overflow triggers a controlled watcher restart and a scan for directories created since the last known timestamp; it does not scan outside configured roots.
- Expected I/O, access, path, and disposal errors become rule diagnostics or per-folder results.
- Unexpected monitor failures isolate the affected rule and leave other roots running.

## Testing

- Unit-test merge behavior for absent files, existing icon fields, unrelated keys, comments, line endings, malformed input, and idempotent reapplication.
- Test compatible planning and apply no longer skip solely because `desktop.ini` exists.
- Test exact recovery of pre-existing file bytes and attributes, plus deletion of application-created files.
- Test recursive event de-duplication, bounded retry, rule replacement, pause/resume, inactive-root reattachment, and persistence reload.
- Test close-to-tray cancellation, explicit exit, reopen activation, background startup, and current-user startup value ownership.
- Run the complete Release test suite and rebuild installer/source artifacts with updated checksums.

## Non-Goals

- No Windows service is installed.
- Monitoring runs only while the user-session application process is active; sign-in startup ensures it normally starts automatically.
- Global-mode registry behavior is unchanged.
- Existing system-folder and unsafe-path exclusions remain in force.
