# Folder Theme Studio 文件夹主题工坊

用来自定义 Windows 文件夹图标的桌面小工具：内置渐变样式与可视化调色板，也可以导入自己的图片 / ICO，一键把这套外观应用到普通文件夹，并自带备份与恢复。附带递归监控、自动样式规则、悬停分类卡片和托盘常驻。

- 平台：Windows 10 2004（10.0.19041）及以上，x64
- 技术栈：C# · WPF · .NET 8
- 许可证：Apache License 2.0

> Beta 阶段：安装包未做代码签名。程序只替换**普通文件夹**图标，不修改文件资源管理器界面、文件类型图标、快捷方式或系统特殊文件夹图标。

## 功能

- **内置样式 + 可视化调色板**：设置渐变起止色、边框色与发光色，可直接在色板上取色或输入十六进制；右侧同步预览 16 / 32 / 64 / 256 px。
- **导入图片图标**：支持 PNG、JPG、JPEG、BMP、ICO（不支持 GIF）；多尺寸 ICO 自动采用最大、色深最高的图层，保留透明通道并补透明边距。
- **全局模式 / 兼容模式**：全局模式统一修改当前用户的普通文件夹图标映射，不扫描整个磁盘；兼容模式只处理手动添加的根目录，已有 `Desktop.ini` 配置会被安全合并，特殊、受保护或不可写目录自动跳过。
- **备份与恢复**：应用前自动生成快照，「恢复最近备份」可撤销最近一次应用，精确还原原有 `Desktop.ini` 字节与属性。
- **递归监控**：勾选「让新文件夹继续使用此样式」后，所选根目录下新建的文件夹自动继承当前外观。
- **自动样式规则**：为已监控目录按名称关键词（不区分大小写的包含匹配）指定图标，可选当前样式或导入图片 / ICO，列表越靠前优先级越高。
- **文件夹分类卡片**：给文件夹填写分类、备注与双色渐变强调色，在桌面或资源管理器悬停约半秒即可查看；卡片固定在文件夹旁，不改图标也不改文件内容。
- **快捷编辑**：默认 Alt + 鼠标右键直接打开编辑窗口，修饰键组合可在设置中更改。
- **托盘常驻**：关闭主窗口默认收起到托盘，可暂停 / 恢复监控；登录 Windows 后自动在后台恢复监控。
- **中英双语界面**，主窗口、设置与教程窗口在受支持的系统上使用原生圆角并保留标准标题栏行为。

## 快速开始

从 [Releases](https://github.com/OnstarKevin/Folder-Theme-Studio-For-Windows/releases) 下载 `FolderThemeStudio-v*-setup.exe` 运行即可。缺少 .NET 8 桌面运行时时，安装器会从固定的微软 HTTPS 地址下载对应安装包，校验 SHA-256 后静默安装。

详细用法见 [中文使用教程](docs/USER-GUIDE.zh-CN.md) 或 [English user guide](docs/USER-GUIDE.en-US.md)。

## 从源码构建

需要 Windows x64、.NET 8 SDK；生成安装器还需要 Inno Setup 6。

```powershell
dotnet restore FolderThemeStudio.sln
dotnet test FolderThemeStudio.sln -c Release
dotnet build src\FolderThemeStudio.App\FolderThemeStudio.App.csproj -c Release

# 生成在线安装器与源码包，输出到 artifacts/
powershell -NoProfile -ExecutionPolicy Bypass -File .\build\Package-Release.ps1 `
  -Version v0.1.0-beta.8 `
  -InnoCompilerPath "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"
```

发布流程、回归测试要求与禁止提交的内容见 [CONTRIBUTING.md](CONTRIBUTING.md)。

## 仓库结构

| 路径 | 内容 |
| --- | --- |
| `src/FolderThemeStudio.App/` | WPF 界面层：窗口、ViewModel、本地化、监控与托盘服务 |
| `src/FolderThemeStudio.Core/` | 核心逻辑：主题应用、备份恢复、图标渲染、系统集成 |
| `tests/FolderThemeStudio.Core.Tests/` | xUnit 测试，含 `Desktop.ini` 合并、备份恢复、ICO 编码等回归用例 |
| `tools/FolderThemeStudio.BrandAssets/` | 品牌图与多尺寸 ICO 生成工具 |
| `build/` | 打包脚本、校验脚本与 Inno Setup 安装器脚本 |
| `assets/brand/` | 品牌 SVG 源文件与各尺寸 PNG |
| `docs/` | 用户教程与版本说明 |

## 安全

- 只修改当前用户作用域内的普通文件夹图标配置，不扫描整个磁盘，不改文件类型图标与特殊文件夹。
- 涉及注册表、`Desktop.ini`、路径遍历或恢复逻辑的改动必须附带回归测试，且不得以真实个人目录作为测试目标。
- 漏洞报告方式见 [SECURITY.md](SECURITY.md)。

## License

[Apache License 2.0](LICENSE)

---

## English

**Folder Theme Studio** is a Windows desktop app for restyling ordinary folder icons. Pick a built-in gradient style or import your own PNG / JPG / BMP / ICO, preview it live at 16–256 px, then apply it with automatic backup and one-click restore.

It also supports recursive folder monitoring (new folders inherit the style), keyword-based auto style rules, hover category cards, an Alt + right-click quick editor, and a tray-resident background mode — with a bilingual Chinese / English UI.

Built with C# / WPF / .NET 8, targeting Windows 10 2004 (10.0.19041) or later on x64. See the [English user guide](docs/USER-GUIDE.en-US.md) for usage, [CONTRIBUTING.md](CONTRIBUTING.md) for build and release steps, and [SECURITY.md](SECURITY.md) for reporting issues. Licensed under the [Apache License 2.0](LICENSE).
