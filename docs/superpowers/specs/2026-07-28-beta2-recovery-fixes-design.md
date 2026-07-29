# Beta 2 Recovery Fixes Design

## Goal

Fix the two accepted recovery defects from `v0.1.0-beta.1`, then publish replacement artifacts as `v0.1.0-beta.2`.

## Fix 1: Fail Closed for Untracked Desktop.ini Compensation

When compatibility apply creates `Desktop.ini` but cannot append its ownership record, compensation must inspect the handle-bound deletion result. `Deleted` and `Missing` are resolved outcomes. `HashMismatch`, deletion exceptions, or failure to durably append the unresolved mutation mean the apply contains an unresolved change.

That unresolved state must propagate from `CompatibleFolderService` through the application mutation result. Automatic rollback may restore recorded changes, but it must report rollback failure and must not write the durable restore-completion marker while any untracked change remains. The result must include the affected folder path and an actionable explanation. Existing user or replacement files remain untouched.

## Fix 2: Preserve Restore Outcomes After Post-Restore Failure

Once registry or folder restoration returns per-path `Successful`, `Skipped`, and `Changed` outcomes, every later failure result must retain them. This includes cancellation or exceptions before Shell refresh, a returned or thrown Shell refresh failure, and cancellation or failure while writing the durable completion marker.

A shared result-construction helper will combine the post-restore failure with the already completed outcome lists. The operation remains retryable because no completion marker is written on these failures.

## Tests

- A compatibility-service regression test simulates ownership append failure followed by hash mismatch and asserts an unresolved mutation is reported.
- A coordinator regression test asserts automatic rollback does not mark completion when the mutation result contains an unresolved path.
- Coordinator tests assert successful/skipped/changed paths survive refresh failure, refresh exception, and completion-marker failure.
- Existing recovery and UI tests must remain green.

All tests use fakes or disposable test directories. No real registry, personal folder, Shell setting, or GUI launch is permitted.

## Beta 2 Release

- Change public release references from `v0.1.0-beta.1` to `v0.1.0-beta.2`.
- Add a Beta 2 changelog entry stating that both recovery defects are fixed.
- Remove those two items from current known issues; retain the unperformed manual Windows/Explorer/DPI/accessibility verification warning.
- Update the packager default and release-notes input to Beta 2.
- Run the complete Release test suite, build the self-contained `win-x64` release, and generate program/source ZIP files plus SHA-256 records.
- Keep Beta 1 artifacts locally; do not overwrite them.

## Out of Scope

- Broad transaction-log redesign.
- GUI changes unrelated to recovery status propagation.
- Code signing, remote GitHub operations, or manual Windows system testing.
