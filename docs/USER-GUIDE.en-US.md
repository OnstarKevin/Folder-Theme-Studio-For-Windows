# Folder Theme Studio User Guide

## Install and start

1. Run `FolderThemeStudio-v0.1.0-beta.7-setup.exe`.
2. Stay online if the installer needs to install the x64 .NET 8 Desktop Runtime.
3. Tutorial and Settings are always available at the top; language selection is in Settings.

## Visual color palette

1. Choose Built-in folder style under Icon source.
2. Select gradient start, gradient end, stroke, or glow in Visual color palette.
3. Pick saturation and brightness on the large two-dimensional palette, and hue on the vertical strip. Hex input remains available.
4. Check the synchronized 16, 32, 64, and 256 px previews.

## Import an image icon

1. Choose Imported image icon under Icon source.
2. Select a PNG, JPG, JPEG, BMP, or ICO image. GIF is not supported. For a multi-size ICO, the largest available layer is selected automatically.
3. The whole image becomes the icon; its aspect ratio is preserved, it is centered, and transparent padding is added.
4. Review the preview, calculate the plan, and apply it.

## Apply and restore

- Global mode updates ordinary-folder icon mappings for the current user without scanning the whole drive.
- Compatible mode processes only explicit roots. Existing customizations are merged safely; special, protected, or unwritable folders are still skipped.
- Enable “Keep applying this style to new folders” to recursively monitor selected roots and style newly created folders automatically.
- Closing the main window minimizes to the tray by default. The tray can pause/resume monitoring or exit completely. Monitoring resumes in the background after Windows sign-in; both behaviors can be disabled in Settings.
- Recalculate the plan after changing any input, then apply.
- Restore latest backup undoes the most recent application.

## Troubleshooting

- Nothing happens after double-clicking: install the x64 .NET 8 Desktop Runtime or rerun the online installer.
- Icons do not refresh immediately: wait for the Windows icon cache or reopen Explorer windows.
- SmartScreen warning: Beta6 is unsigned; verify the downloaded SHA-256.
- The app changes ordinary folder icons only; it does not replace File Explorer.
