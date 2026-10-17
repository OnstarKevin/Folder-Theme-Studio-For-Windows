# beta.17 上传与试用

1. 直接运行 `02-Release-Assets/FolderThemeStudio-v0.1.0-beta.17-setup.exe` 试用新版。
2. 将 `01-Repository-Source` 内的内容更新到 GitHub 仓库根目录，保留目录结构，不上传包装目录。
3. 创建预发布 Release，标签 `v0.1.0-beta.17`，标题 `Folder Theme Studio beta.17 — 新消息提醒与弹性拖动`。
4. 正文使用 `02-Release-Assets/RELEASE-NOTES.md`，附件上传安装 EXE、源码 ZIP 与各自 `.sha256`。
5. `Get-FileHash 文件名 -Algorithm SHA256` 可对照校验文件。

资料已在本地生成，不代表 GitHub 已发布。当前介绍网站仍为 beta.16，本轮未变更线上下载；`website` 保留其相符的 beta.16 安装包和校验值，避免把开发试用包冒充已上线版本。

资源管理器背景本轮仅完成只读可行性检查；自定义图片、背景虚化尚未纳入应用。参见 `docs/EXPLORER-BACKDROP-FEASIBILITY.zh-CN.md`。
