# Beta 8 以来：宣传片与素材

## 交付

- 1920×1080、30 fps、60 秒 MP4，主文件在 `artifacts/FolderThemeStudio-Beta8-18-Promo.mp4`。
- 网站版在 `website/assets/video/folder-theme-studio-launch.mp4`，保留音轨，按播放而不自动出声。
- 可编辑工程在 `promo-film`。`npm ci` 后运行 `npm run dev`；完整片 Composition ID 为 `FolderThemeStudioLaunch`，各场景也可单独预览。
- 修改场景后使用 `npx remotion render src/index.ts FolderThemeStudioLaunch out/launch.mp4 --codec=h264 --crf=20` 导出。软件安装 EXE 不携带宣传片运行环境。

## 参考与来源

- 新视频封面：`assets/brand/promo-cover-beta18.png`。沿用产品冰山背景、玻璃文件夹和迷你灵动岛，使用大字与冰蓝/柔粉配色；网站视频海报同步使用该封面。封面是宣传视觉合成，不代表新增的软件界面。

- 视觉节奏参考：[假如把爸爸的钥匙圈做成苹果风格](https://www.bilibili.com/video/BV1XHBXBaENH/)。使用其公开预览分镜进行风格分析，没有把该片画面搬入本片。
- 用户指定同款配乐。Bilibili 的该片 `bgm_info` 标注《3 Strikes》，music_id `MA425508892059918427`。本工程使用参考片公开音频流，原音频约 57.69 秒，保留原片的混音/剪辑走向，不宣称这是无其他声音的独立母带。
- 默认背景来自用户附件 `如故_1_original.png`，原始 2944×1648，裁切区域 `(8,0,2928,1647)`，没有 AI 重绘。高清母版保留不变，软件内置资源等比缩小到 1920×1080 以控制安装体积。用于软件、影片与网站的同一视觉。
- 产品截图使用示例数据；图标/材质由现有 FolderIconRenderer 导出，宣传动画另用代码绘制。演示歌词为自写示例，不是参考曲目的歌词。

资源管理器自定义图片背景与虚化尚未上线，不出现在影片的功能承诺中。
