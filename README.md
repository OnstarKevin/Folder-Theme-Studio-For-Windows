# Folder Theme Studio Beta 18 — 宣传片可编辑工程

主合成：`FolderThemeStudioLaunch`，1920 × 1080，30 fps，1800 帧（60 秒）。

`npm run dev` 启动 Remotion Studio。`src/scenes/` 中的七个独立场景均在 Studio 的 Scenes 文件夹注册，可双击主时间轴的场景进入独立时间轴。

| 时间 | 场景 | 内容 |
| --- | --- | --- |
| 0–6 s | Opening | 品牌、文件夹近景、全新壁纸 |
| 6–16 s | Materials | 十二配色、真实材质导出、星形贴纸 |
| 16–26 s | Notes | 分类、快捷备注、统一信息便签 |
| 26–34 s | LiveFolders | 最近文件、拖放子目录 |
| 34–44 s | Island | 260 × 64 迷你灵动岛、音乐控制 |
| 44–52 s | Notifications | 六秒通知、消息去重、弹性拖动 |
| 52–60 s | Finale | Beta 18 真实界面、下载 CTA |

音乐只使用 `public/reference-3-strikes.m4a` 一条参考原音轨，不循环。开始短淡入，56.7–57.69 秒淡出，结尾自然留白。无配音。

画面采用本地系统字体，所有动画由 Remotion 帧驱动。`public/folders/` 为产品原生渲染器导出的透明 PNG；真实软件截图保存在 public。品牌开场和十二色特写为可编辑 SVG；功能演示保留“示例数据”标注。未展示未上线的资源管理器背景虚化功能。

导出命令：

```sh
npx remotion render src/index.ts FolderThemeStudioLaunch out/Folder-Studio-Beta18-Launch.mp4 --codec=h264
```

本机若没有 Remotion 浏览器缓存，可使用已安装浏览器的 `--browser-executable` 参数。

