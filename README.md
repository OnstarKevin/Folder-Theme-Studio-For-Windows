# 文件夹主题工坊 / Folder Theme Studio

<p align="center">
  <img src="assets/brand/raster/folder-theme-studio-256.png" width="160" alt="Folder Theme Studio logo">
</p>

<p align="center">
  <img src="https://img.shields.io/badge/version-v0.1.0--beta.8-1769e0" alt="Version v0.1.0-beta.8">
  <img src="https://img.shields.io/badge/platform-Windows%2010%2F11%20x64-0078d4" alt="Windows 10/11 x64">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-Apache--2.0-5c6bc0" alt="Apache-2.0 license"></a>
</p>

<p align="center">
  为普通 Windows 文件夹设计统一、可持续、可恢复的个性化图标。<br>
  Design consistent, persistent, and recoverable icons for ordinary Windows folders.
</p>

---

## 简体中文

Folder Theme Studio 是一款面向 Windows 10/11 x64 的开源文件夹图标定制工具。它只处理普通文件夹图标，不替换文件资源管理器、不修改文件类型图标，也不会把系统特殊文件夹当作普通目录处理。

你可以通过可视化调色板设计内置文件夹样式，也可以导入自己的图片作为完整图标。应用前，软件会先计算方案并展示允许与跳过的项目；应用后可通过备份恢复。对于需要长期统一风格的目录，还能启用递归监控，让之后新建的文件夹自动继承所选样式。

### 功能亮点

- **二维可视化调色板**：直接选择色相、饱和度和亮度，并分别调整渐变起始色、渐变结束色、描边和发光颜色。
- **图片图标导入**：支持 PNG、JPG、JPEG、BMP、ICO；自动保持比例、居中并补充透明边距。
- **无需保存即可使用**：刚完成的样式编辑或刚导入的图片会立即成为当前图标，保存个人主题仅用于以后复用。
- **多尺寸实时预览**：同步检查 16、32、64、256 px 效果。
- **两种应用模式**：全局模式统一普通文件夹映射；兼容模式只处理明确选择的目录。
- **已有样式可再次修改**：仅合并 `Desktop.ini` 中的图标字段，保留其他设置、注释、编码和换行格式。
- **递归持久监控**：为指定根目录保持样式，新建文件夹自动继承当前图标。
- **悬浮分类卡片**：为桌面或资源管理器中的真实文件夹添加分类和备注；卡片固定在文件夹旁，支持双色渐变强调色与统一透明度。
- **备份与恢复**：应用前记录原有配置；符合恢复条件时精确还原原始 `Desktop.ini` 内容和属性。
- **后台运行**：关闭主窗口默认收起到系统托盘；支持登录 Windows 后自动在后台启动并恢复监控。
- **中英双语界面**：简体中文与 English 可在设置中切换。
- **原生圆角窗口**：主窗口、设置和教程在受支持的 Windows 版本上使用原生圆角，同时保留系统标题栏与窗口操作。

### Beta 8 更新

- 桌面和资源管理器中的文件夹均支持悬浮分类卡片；Alt＋鼠标右键可快速编辑，也可在设置中更改操作组合。
- 卡片增加双色渐变强调色和统一透明度，DIY 面板滚动更快。
- 保留 ICO 导入、递归监控、系统托盘与登录后后台自启能力。

完整变更参见 [v0.1.0-beta.8 发布说明](docs/releases/v0.1.0-beta.8.md)。

### 安装

1. 从项目 Release 下载 `FolderThemeStudio-v0.1.0-beta.8-setup.exe` 及同名 `.sha256` 文件。
2. 核对安装包 SHA-256。
3. 运行安装器。若系统缺少 x64 `.NET 8 Desktop Runtime`，安装器会联网下载并安装。

> 当前发布的是未签名 Beta 版本，Windows 可能显示 SmartScreen 提示。建议首次使用时先在一次性测试目录中通过兼容模式验证效果。

### 快速开始

1. 在“图标来源”中选择内置文件夹样式，或导入 PNG、JPG、JPEG、BMP、ICO 图片。
2. 调整颜色并检查 16、32、64、256 px 实时预览。
   当前编辑或刚导入的图标无需保存个人主题即可继续使用。
3. 选择全局模式，或在兼容模式中添加一个或多个明确根目录。
4. 点击“计算方案”，检查允许、跳过和失败项目后再应用。
5. 如需让新文件夹继续继承样式，启用递归监控；需要撤销时使用“恢复最近备份”。

### 应用模式

| 模式 | 适合场景 | 行为 |
| --- | --- | --- |
| 全局模式 | 希望统一当前用户普通文件夹的默认外观 | 修改普通文件夹图标映射，不扫描整个磁盘 |
| 兼容模式 | 只想修改项目、素材库或指定目录 | 仅处理明确添加的根目录；跳过特殊、受保护、网络、重解析或不可写路径 |
| 兼容模式 + 递归监控 | 希望目录中的新文件夹长期保持同一风格 | 监控所选根目录，新建文件夹自动应用已保存的图标 |

### 使用须知

- 软件只替换普通文件夹图标，不替换文件资源管理器本身。
- GIF 不受支持；动态图也不适合作为 Windows 文件夹图标。
- Windows 图标缓存可能不会立即刷新，可重新打开文件夹窗口或稍后查看。
- 托盘菜单可打开主窗口、暂停/恢复监控或彻底退出。
- 开机自启和关闭到托盘均可在“设置”中关闭。

### 推荐图标资源

想导入现成的文件夹图标，可以访问第三方项目 [Folder11 Ico](https://github.com/icon11-community/Folder-Ico) 下载 `.ico` 文件，再使用本软件的图片图标导入功能。图标由该项目提供，使用前请查看其说明和授权条款。

### 文档

- [中文使用教程](docs/USER-GUIDE.zh-CN.md)
- [English User Guide](docs/USER-GUIDE.en-US.md)
- [Beta 8 发布说明](docs/releases/v0.1.0-beta.8.md)
- [双语项目宣传文章](docs/PROMOTION.zh-en.md)
- [人工测试清单](docs/manual-test-checklist.md)
- [安全策略](SECURITY.md)
- [参与贡献](CONTRIBUTING.md)

### 从源码构建

开发环境需要 Windows x64 和 .NET 8 SDK；生成安装器还需要 Inno Setup 6。

```powershell
dotnet restore FolderThemeStudio.sln
dotnet test FolderThemeStudio.sln -c Release
dotnet build src\FolderThemeStudio.App\FolderThemeStudio.App.csproj -c Release
powershell -NoProfile -ExecutionPolicy Bypass -File .\build\Package-Release.ps1 -Version v0.1.0-beta.8 -InnoCompilerPath "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"
```

欢迎通过 Issue 提交可复现的问题，也欢迎先阅读 [参与贡献](CONTRIBUTING.md) 后改进界面、文档、兼容性或测试。涉及敏感问题时，请遵循 [安全策略](SECURITY.md)。

### 许可证

本项目基于 [Apache License 2.0](LICENSE) 开源。

---

## English

Folder Theme Studio is an open-source folder icon customization tool for Windows 10/11 x64. It targets ordinary folder icons only: it does not replace File Explorer, alter file-type icons, or treat protected system locations as ordinary folders.

You can build a folder style with a visual color palette or import an image as the complete icon. Before changing anything, the app calculates a plan and reports what is allowed or skipped; afterwards, its backup workflow can restore the previous state. For directories that need a lasting visual identity, recursive monitoring automatically applies the selected style to newly created folders.

### Highlights

- **Two-dimensional visual color palette**: pick hue, saturation, and brightness directly, then tune gradient start, gradient end, stroke, and glow independently.
- **Image icon import**: supports PNG, JPG, JPEG, BMP, and ICO, preserving aspect ratio while centering the image on transparent padding.
- **No save required for current use**: new style edits and imported images become the current icon immediately; saving a personal theme is only for later reuse.
- **Live multi-size preview**: inspect the result at 16, 32, 64, and 256 px.
- **Two application modes**: Global mode updates the ordinary-folder mapping; Compatible mode stays within explicitly selected roots.
- **Restyle existing custom folders**: only icon-related fields in `Desktop.ini` are merged, while unrelated settings, comments, encoding, and line endings are retained.
- **Persistent recursive monitoring**: keep a style attached to a selected root so new folders inherit its icon automatically.
- **Folder hover cards**: add a category and note to a real desktop or Explorer folder; the stationary card supports a two-color accent and shared opacity.
- **Backup and restore**: capture pre-existing configuration before applying and restore the original `Desktop.ini` bytes and attributes when recovery conditions are met.
- **Background operation**: closing the main window minimizes to the system tray by default; monitoring can resume automatically in the background after Windows sign-in.
- **Bilingual interface**: switch between Simplified Chinese and English in Settings.
- **Native rounded windows**: Main, Settings, and Tutorial use native rounded corners on supported Windows versions while retaining standard system window controls.

### What is new in Beta 8

- Added hover cards on desktop and Explorer folders; Alt + right-click opens quick editing, and the gesture is configurable.
- Cards support a two-color gradient accent and shared opacity; the DIY panel scrolls faster.
- Retains ICO imports, persistent recursive monitoring, system-tray controls, and background startup.

See the [v0.1.0-beta.8 release notes](docs/releases/v0.1.0-beta.8.md) for the complete summary.

### Installation

1. Download `FolderThemeStudio-v0.1.0-beta.8-setup.exe` and its matching `.sha256` file from the project Release.
2. Verify the installer SHA-256.
3. Run the installer. If the x64 `.NET 8 Desktop Runtime` is missing, the installer downloads and installs it over the network.

> This is an unsigned Beta build, so Windows may display a SmartScreen warning. For a first run, use Compatible mode with a disposable test directory and verify the result before applying more broadly.

### Quick start

1. Choose a built-in folder style under Icon source, or import a PNG, JPG, JPEG, BMP, or ICO image.
2. Tune the colors and inspect the live 16, 32, 64, and 256 px previews.
   Current edits and newly imported icons can be used without saving a personal theme.
3. Choose Global mode, or add one or more explicit roots in Compatible mode.
4. Select Calculate plan, review allowed, skipped, and failed items, and then apply.
5. Enable recursive monitoring if future folders should inherit the style; use Restore latest backup when you need to undo the last application.

### Application modes

| Mode | Best for | Behavior |
| --- | --- | --- |
| Global | Giving ordinary folders a consistent default appearance for the current user | Updates the ordinary-folder icon mapping without scanning the whole drive |
| Compatible | Styling only a project, asset library, or selected directory | Processes explicit roots and skips special, protected, network, reparse-point, or unwritable paths |
| Compatible + recursive monitoring | Keeping future folders in a directory visually consistent | Watches selected roots and applies a persisted icon to newly created folders |

### Important notes

- The app changes ordinary folder icons only; it does not replace File Explorer.
- GIF is not supported, and animated images are not suitable as Windows folder icons.
- Windows icon caching can delay visible changes; reopen the folder window or check again later.
- The tray menu can open the main window, pause/resume monitoring, or exit completely.
- Startup after sign-in and close-to-tray behavior can both be disabled in Settings.

### Recommended icon resource

For ready-made folder icons, download `.ico` files from the third-party [Folder11 Ico](https://github.com/icon11-community/Folder-Ico) project and import them with this app's image icon import feature. Check the project's documentation and licensing terms before use.

### Documentation

- [中文使用教程](docs/USER-GUIDE.zh-CN.md)
- [English User Guide](docs/USER-GUIDE.en-US.md)
- [Beta 8 release notes](docs/releases/v0.1.0-beta.8.md)
- [Bilingual project announcement](docs/PROMOTION.zh-en.md)
- [Manual test checklist](docs/manual-test-checklist.md)
- [Security policy](SECURITY.md)
- [Contributing guide](CONTRIBUTING.md)

### Build from source

Development requires Windows x64 and the .NET 8 SDK. Inno Setup 6 is also required to create the installer.

```powershell
dotnet restore FolderThemeStudio.sln
dotnet test FolderThemeStudio.sln -c Release
dotnet build src\FolderThemeStudio.App\FolderThemeStudio.App.csproj -c Release
powershell -NoProfile -ExecutionPolicy Bypass -File .\build\Package-Release.ps1 -Version v0.1.0-beta.8 -InnoCompilerPath "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"
```

Reproducible bug reports are welcome through Issues. Read the [contributing guide](CONTRIBUTING.md) before proposing interface, documentation, compatibility, or test improvements. For sensitive reports, follow the [security policy](SECURITY.md).

### License

Folder Theme Studio is released under the [Apache License 2.0](LICENSE).
