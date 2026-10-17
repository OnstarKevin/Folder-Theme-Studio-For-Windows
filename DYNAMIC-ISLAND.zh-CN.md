# 内置桌面灵动岛：运行与开发说明

灵动岛直接集成在 Folder Theme Studio 中。从首页或左侧“桌面灵动岛”打开，不需要启动另一个软件。

## 已实现与范围

| 功能 | 实现方式 |
| --- | --- |
| 无边框、圆角、深色半透明、置顶、隐藏任务栏 | WPF 独立窗口；固定 260×64 逻辑像素，黑色胶囊主体约 248×52；初次显示不抢焦点 |
| 左键拖动，恢复位置 | DragMove；保存 WPF 逻辑像素；窗口尺寸变化时约束到当前显示器工作区 |
| 滚轮调节透明度 | 每格 5%，限制 20%–100%，合并保存 |
| 时间 | 每秒独立刷新 HH:mm:ss，不受媒体查询影响 |
| CPU | GetSystemTimes 两次采样的增量，Kernel 包含 Idle，第一次采样显示 — |
| 内存 | GlobalMemoryStatusEx 物理内存可用量与总量 |
| 剪贴板 | AddClipboardFormatListener + WM_CLIPBOARDUPDATE；前 80 个文本元素，不保存历史 |
| 音乐 | Windows.Media.Control，优先正在播放的会话；歌名、歌手、专辑、应用来源、状态、进度和时长；右键播放/暂停/上下一首 |
| 歌词 | 根据媒体播放进度显示 LRC 当前行；迷你模式合并为单行摘要并省略长文本，支持多时间戳与 offset |
| 通知 | UserNotificationListener 用户主动授权，系统不支持时显示限制；可选令牌保护的本地消息接口 |
| 设置 | 模块开关、1–60 秒采样间隔、透明度、歌词目录、与主软件共用开机启动 |
| 生命周期 | 单实例窗口，关闭解除监听并取消刷新；退出主软件保存最后位置与启用状态 |

beta.16 在线歌词默认关闭；开启后后台按歌曲/歌手查询，不上传剪贴板和通知。原生系统通知访问取决于 Windows 授权及包身份；本地消息接口是独立可用的补充。播放器没有发布 SMTC 会话时显示等待状态。

音乐、剪贴板和通知摘要每 5 秒轮换，长文本省略；悬停查看当前歌曲详情、歌词译文及数据来源。胶囊始终保持 260×64。

## 日常使用

1. 首页 → 桌面灵动岛。
2. 左键拖动；滚轮上/下调节透明度。
3. 右键 → 设置，选择显示模块。剪贴板默认关闭，用户开启后才读取。
4. 右键 → 关闭灵动岛，下次启动不会自动显示；右键 → 退出软件则保留灵动岛启用状态，下次启动恢复。
5. 设置中的开机启动与主软件共用同一注册表 Run 项，不增加重复启动入口。

配置位于 `%LOCALAPPDATA%\FolderThemeStudio\dynamic-island.json`。损坏的配置会回退到默认值。拖动与滚轮操作在短暂停顿后合并保存，写入失败会显示提示。剪贴板及通知内容不写入这个文件。

## Visual Studio 2022

安装 Visual Studio 2022 17.8 或以上版本，选择“.NET 桌面开发”工作负载，并安装 .NET 8 SDK。打开仓库根目录的 `FolderThemeStudio.sln`，将 `FolderThemeStudio.App` 设为启动项目，选择 x64，按 F5。从首页打开灵动岛即可调试。

项目目标是 `net8.0-windows10.0.19041.0`，Windows 10 2004 或更新版本。WPF、Win32 和 WinRT 投影都由 SDK 提供，灵动岛不需要额外第三方 NuGet 包。

```powershell
dotnet restore FolderThemeStudio.sln
dotnet build FolderThemeStudio.sln -c Debug
dotnet run --project src/FolderThemeStudio.App/FolderThemeStudio.App.csproj -c Debug
dotnet test FolderThemeStudio.sln -c Release
```

仓库 `NuGet.Config` 已设置官方 nuget.org。测试工程会恢复 Microsoft.NET.Test.Sdk、xunit 和 xunit.runner.visualstudio；不是灵动岛运行时依赖。遇到共享编译服务的写入权限错误，可在 build/test 命令中加入 `-p:UseSharedCompilation=false`。

## 完整源码结构

```text
FolderThemeStudio.sln
NuGet.Config
src/FolderThemeStudio.App/
  App.xaml / App.xaml.cs                         # 主软件启动、托盘与灵动岛恢复
  MainWindow.xaml / MainWindow.xaml.cs           # 首页入口与共用开机启动
  FolderThemeStudio.App.csproj                   # .NET 8 / WPF / WinRT 目标
  DynamicIsland/
    IslandSettings.cs                           # 配置、校验、原子保存
    SystemMetricsSource.cs                      # CPU 和内存 Win32 API
    ClipboardTextListener.cs                    # 剪贴板事件、重试与 Unicode 预览
    LrcLyricsSource.cs                          # 可替换的歌词接口与 LRC 实现
    OnlineLyricsSource.cs / LyricsCoordinator.cs # 多来源网络获取与后台缓存
    LocalNotificationBridge.cs / IslandNotificationHub.cs # 本地消息与系统通知汇合
    WindowsMediaSource.cs                       # 系统媒体会话实现
    WindowsNotificationSource.cs                # 通知接口与 MSIX WinRT 实现
    IslandViewModel.cs                          # 可测试的数据刷新与生命周期
    DynamicIslandWindow.xaml / .xaml.cs          # 灵动岛外观、拖动、滚轮、滚动歌词
    DynamicIslandSettingsWindow.xaml / .xaml.cs  # 模块与刷新设置
    DynamicIslandController.cs                  # 单窗口管理与主软件集成
tests/FolderThemeStudio.Core.Tests/Application/
  DynamicIslandTests.cs                          # 原生采样、边界、存储及 UI 回归
docs/
  DYNAMIC-ISLAND.zh-CN.md
  DYNAMIC-ISLAND-SOURCE.md                        # 项目源文件逐个完整代码
```

实际项目和源码 ZIP 中包含现有软件所有代码，以上只列出入口和灵动岛相关文件。逐文件代码文档为本次发布时的快照，后续开发应直接编辑 src 下的原文件。

## 网易云、QQ 音乐、Spotify 与歌词

在灵动岛设置勾选“在线获取歌词”。本地 LRC 优先；没有本地歌词时后台查询落月 QQ/网易、网易云与 LRCLIB。网易云会话优先网易来源。只检索完整歌名/歌手，不给没有歌手的视频标题搜索歌词。已知服务时长与播放时长不匹配时跳过候选；未提供时长的服务只能核对歌名和歌手。没有同步时间戳的内容不会当作同步歌词显示。

换歌时取消旧查询；最多缓存 24 首，成功结果 12 小时，空结果 3 分钟；缓存只在内存。每来源 3 秒超时，不阻塞其他信息刷新。请求采用标准 TLS 校验，服务停用/网络失败可继续使用本地 LRC。歌词与译文在悬停摘要中查看，胶囊显示当前歌词单行预览。

完整参考致谢及许可见 [THIRD-PARTY-NOTICES](../THIRD-PARTY-NOTICES.md)。已实测公开示例“晴天 / 周杰伦”能够通过落月 QQ 返回同步歌词；这不代表曲库中每首歌曲都可用。

开启播放器设置中的“系统媒体控制/系统媒体传输控制”或同类功能。Spotify 及其他发布 SMTC 会话的播放器可由当前系统媒体会话读取；网易云、QQ 音乐是否发布会话取决于客户端版本和配置。没有会话时，不会假装正在播放。

本实现直接使用系统 API，不依赖用户提到的 Now Playing 项目，也不假设一个未经验证的默认 HTTP 端口。媒体 API 提供元数据和时间轴，不提供歌词。[Microsoft 媒体会话文档](https://learn.microsoft.com/en-us/uwp/api/windows.media.control.globalsystemmediatransportcontrolssessionmanager.requestasync?view=winrt-26100)、[时间轴文档](https://learn.microsoft.com/en-us/uwp/api/windows.media.control.globalsystemmediatransportcontrolssessiontimelineproperties?view=winrt-26100)。

把有权使用的 UTF-8 LRC 文件放在所选目录，以 `歌名.lrc` 或 `歌手 - 歌名.lrc` 命名，需和媒体元数据匹配。先查找歌手与歌名组合，再查找歌名。单个文件上限 2 MiB。示例文件：

```lrc
[ar:示例歌手]
[ti:City Lights]
[offset:0]
[00:00.00]第一句歌词
[00:12.50]第二句歌词
[00:24.00][00:48.00]重复歌词
```

`IIslandLyricsSource` 可以替换为自己的本地 HTTP/授权歌词服务，返回 `LyricLine` 列表；`IIslandMediaSource` 可替换为其他 Now Playing 适配器。接口和当前实现的完整代码均包含在项目中。

## 通知授权与 MSIX 扩展

点击“请求通知读取权限”后尝试 Windows 授权，只有 Allowed 才读取。普通 EXE 的权限能力因系统而异；系统拒绝或不支持时会显示实际状态，可使用本地消息接口。MSIX 能力声明仍是受支持的打包路线；本次 EXE 发布没有验证完整 MSIX 授权流程。

在 VS 2022 添加 Windows Application Packaging Project，引用现有 WPF 项目并指定入口，配置匹配证书的包身份及签名。包装工程需要 Windows SDK/打包工作负载。在该工程生成的完整 `Package.appxmanifest` 中声明命名空间及能力：

```xml
<!-- 在 Package 根元素添加命名空间并合并到其 IgnorableNamespaces 中。 -->
xmlns:uap3="http://schemas.microsoft.com/appx/manifest/uap/windows10/3"
xmlns:uap7="http://schemas.microsoft.com/appx/manifest/uap/windows10/7"
xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities"

<!-- 合并到已有 Capabilities，不要用此段替换完整清单。 -->
<Capabilities>
  <uap3:Capability Name="userNotificationListener" />
  <uap7:Capability Name="globalMediaControl" />
  <rescap:Capability Name="runFullTrust" />
</Capabilities>
```

安装签名的 MSIX 后，从灵动岛设置点击“请求通知读取权限”，由 Windows 显示授权流程。拒绝后需在 Windows 设置中重新允许访问。适配器每次读取检查权限，权限被撤销时立即停止提供通知；关闭窗口时解绑通知事件。不删除或清空系统通知。

能力与 UI 线程授权要求见 [Microsoft 通知监听说明](https://learn.microsoft.com/en-us/windows/apps/develop/notifications/app-notifications/notification-listener)、[uap3 能力定义](https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap3-capability-manual)及 [媒体能力定义](https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/app-capability-declarations)。

## 本地消息接口

设置中同时开启“通知与消息”和“启用本地消息接口”，默认端口 47301。点击“复制接口令牌”。接口只监听 127.0.0.1，端口占用时换一个端口后保存。窗口关闭或关闭模块会停止监听。消息仅在内存保留 90 秒，不写历史。

```powershell
$token = '粘贴设置面板复制的本地令牌'
$body = @{ title = '下载完成'; body = '你的文件已经准备好'; source = '下载助手' } | ConvertTo-Json
Invoke-RestMethod -Uri 'http://127.0.0.1:47301/api/notifications' -Method Post `
  -Headers @{ Authorization = "Bearer $token" } -ContentType 'application/json; charset=utf-8' `
  -Body ([Text.Encoding]::UTF8.GetBytes($body))
```

成功返回 `{"ok":true}`。无需管理员、无需 URL ACL、不开放局域网；拒绝带浏览器 Origin 的请求和大于 16 KiB 的正文。这个接口接收脚本/配套程序的消息，不能冒充自动监听所有 Windows 通知。本机令牌保存在应用自己的配置文件中，请不要上传该配置。

## beta.17：新消息与拖动体验

- 只提醒本次启用后的新通知，不回放通知中心的历史。按来源、时间、正文等信息生成内存指纹去重；每条提醒 6 秒，最多排队 8 条，突发消息优先保留较新的条目。90 秒前的排队消息丢弃，关闭模块清空队列。
- 新消息暂时优先占用迷你摘要，随后恢复音乐/剪贴板。来源名称和文字徽标帮助区分消息；不是应用官方 Logo。悬停可查看来源与待显示数量。
- 在“屏蔽消息来源”填写应用显示名称或 AppUserModelId，多个来源用分号分隔，大小写不敏感、完整匹配。本地脚本可提供 `source`，对应 ID 为 `local:来源名称`。改变筛选会清空当前提醒，避免继续显示已屏蔽内容。
- 原生通知改为批量读取，本地接口缓存突发消息；没有发送到 Windows 通知中心的应用内消息不会被读取。本地接口也不会自动读取微信/QQ 聊天内容。原生授权仍受系统及包身份限制。
- “弹性拖动”只作用于胶囊外观：轻微形变、倾斜和阴影，松手回弹，位置保持跟手。遵循系统动画设置，可在灵动岛设置关闭；“靠近屏幕边缘时吸附”独立开关，触发距离 18 逻辑像素。
- 仍为 260×64，长文本单行省略，不自动展开面板。
- 原生资源管理器背景状态见 [可行性报告](EXPLORER-BACKDROP-FEASIBILITY.zh-CN.md)，目前尚无自定义图片/虚化功能入口。
