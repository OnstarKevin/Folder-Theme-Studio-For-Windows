# beta.18 上传与试用

最新整理目录为 `GitHub-Upload-v0.1.0-beta.18-Ready`，先阅读其中的 `00-START-HERE.md`。
目录中的 `03-Website` 提供独立静态站点包和 Sites 部署包，`04-Promo` 提供宣传片、新封面与默认背景原图。
新增封面使用原创生成的玻璃文件夹视觉；产品示意不代表额外功能。宣传片音乐沿用参考视频的《3 Strikes》，公开分发前请自行确认音乐和原始背景图片的使用授权。

1. 直接运行 `02-Release-Assets/FolderThemeStudio-v0.1.0-beta.18-setup.exe` 试用新版。
2. 将 `01-Repository-Source` 内的内容更新到 GitHub 仓库根目录，保留目录结构，不上传包装目录。
3. 创建预发布 Release，标签 `v0.1.0-beta.18`，标题 `Folder Theme Studio beta.18 — 新背景与产品影片`。
4. 正文使用 `02-Release-Assets/RELEASE-NOTES.md`，附件上传安装 EXE、源码 ZIP 与各自 `.sha256`。
5. `Get-FileHash 文件名 -Algorithm SHA256` 可对照校验文件。

资料已在本地生成，不代表 GitHub 已发布。本轮同步更新产品介绍网站，新增宣传片与 Beta 18 下载；上线结果以部署返回状态为准。

资源管理器背景本轮仅完成只读可行性检查；自定义图片、背景虚化尚未纳入应用。参见 `docs/EXPLORER-BACKDROP-FEASIBILITY.zh-CN.md`。
