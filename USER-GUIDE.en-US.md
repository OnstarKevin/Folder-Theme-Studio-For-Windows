# Folder Theme Studio User Guide

## Install and start

1. Run `FolderThemeStudio-v0.1.0-beta.18-setup.exe`.
2. Stay online if the installer needs to install the x64 .NET 8 Desktop Runtime.
3. Home brings Desktop Island, Icon studio, Live folders, Automatic styles, and Backup & restore together. Tutorial and Settings are at the bottom of the sidebar.

## Desktop Island

Open Desktop Island from Home or the sidebar. Drag to move, scroll to adjust opacity, and right-click for settings or to close the island. Settings control modules, refresh interval, and shared application startup. Lyrics use local LRC files. Notification listening requires MSIX packaging and permission. See [the development and configuration guide](DYNAMIC-ISLAND.zh-CN.md).

## Start in three steps

1. Open Icon studio from Home. Choose one of twelve Apple-inspired colors and a material, or import an image. Expand Advanced options to customize gradients, stroke, and glow.
2. Add the folders you want to change. The default scope is selected folders only; the current-user global scope is also available.
3. Calculate the plan, review it, then apply. Saving a personal theme is optional and only needed for reuse. Open Backup & restore from Home to undo the latest application.

## Materials, stickers, and simulated desktop

Choose satin, glass, clay, frosted glass, or paper. Expand Stickers to adjust a sticker's color, size, and rotation or drag it on the preview. Expand size previews or the simulated desktop to inspect the result. Desktop preview choices do not change the saved theme or application plan.

## Import an image icon


1. Choose Imported image icon under Icon source.
2. Select a PNG, JPG, JPEG, BMP, or ICO image. GIF is not supported. For a multi-size ICO, the largest available layer is selected automatically.
3. The whole image becomes the icon; its aspect ratio is preserved, it is centered, and transparent padding is added.
4. Review the preview, calculate the plan, and apply it.
5. The newly imported image or ICO becomes the current icon immediately; no separate save is required.

## Live folders

Pause over an ordinary desktop or Explorer folder for about half a second to preview its five most recently modified regular files. Drag regular files onto the card, then drop them on This folder or a direct child folder to move them. Folders, links, and same-name conflicts are not moved or overwritten.

The same card shows created/modified dates, direct content counts, and a size summary including subfolders. The matching Windows info tip is temporarily dismissed while the folder is recognized; leaving or clicking ends this behavior without changing global preferences. Large directories, unreadable content, or links are marked as partially counted.

## Apply and restore


- Global mode updates ordinary-folder icon mappings for the current user without scanning the whole drive.
- Compatible mode processes only explicit roots. Existing customizations are merged safely; special, protected, or unwritable folders are still skipped.
- Enable “Keep applying this style to new folders” to recursively monitor selected roots and style newly created folders automatically.
- Open Automatic Rules from Home or the sidebar to add a name keyword and icon for a monitored root. Use the current editor style or import an image/ICO.
- Matching is case-insensitive substring matching; rules nearer the top take priority. Unmatched folders keep that root's existing monitoring icon.
- Rules recursively apply to folders created later under the selected root. Rename events keep the existing default-icon behavior; existing folders are not batch-rewritten.
- Open Folder Hover Cards from Home or the sidebar to manage categories, notes, and accent colors. Apple-inspired color swatches are available here too.
- Alt + right-click opens a compact note editor. The clicked folder is selected automatically. Ctrl + Enter saves and closes; Esc cancels. Category and color options are collapsed. Change the shortcut in Settings. This dedicated shortcut replaces the Windows context menu; ordinary right-clicks retain their native behavior.
- The opacity setting applies to all note cards. Category data is stored for the current Windows user. If a folder moves, add its new path. Files, shortcuts, and virtual items do not show cards.
- Closing the main window minimizes to the tray by default. The tray can pause/resume monitoring or exit completely. Monitoring resumes in the background after Windows sign-in; both behaviors can be disabled in Settings.
- Recalculate the plan after changing any input, then apply.
- Restore latest backup undoes the most recent application.
- Main, Settings, and Tutorial use native rounded corners on supported Windows versions while retaining normal title-bar, resizing, and snapping behavior.

## Troubleshooting

- Nothing happens after double-clicking: install the x64 .NET 8 Desktop Runtime or rerun the online installer.
- Icons do not refresh immediately: wait for the Windows icon cache or reopen Explorer windows.
- SmartScreen warning: this Beta is unsigned; verify the downloaded SHA-256.
- The app changes ordinary folder icons only; it does not replace File Explorer.
