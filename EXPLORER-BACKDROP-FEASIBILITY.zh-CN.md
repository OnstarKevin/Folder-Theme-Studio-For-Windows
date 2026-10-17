# 资源管理器背景：可行性检查（2026-10-17）

本轮结论：原生 Explorer 的毛玻璃值得做成独立、默认关闭的实验模块；**尚未把自定义图片背景或毛玻璃作为已实现功能发布**。没有安装、注册或注入第三方 DLL，没有更改 Explorer 的窗口样式。

## 本机只读结果

- Windows 构建 26200.9550（25H2）。兼容判断使用构建号；注册表 ProductName 仍可能显示旧产品名称。
- 检测到原生 `CabinetWClass` 文件管理器窗口。
- `DwmGetWindowAttribute(DWMWA_SYSTEMBACKDROP_TYPE)` 成功，返回 4（TabbedWindow，Mica Alt）。**该窗口已经有系统材质属性**，不能据此断言文件列表背景透明。
- 子窗口同时包含 `DUIViewWndClassName`、`DirectUIHWND`、`SHELLDLL_DefView` 和 `Microsoft.UI.Content.DesktopChildSiteBridge`，需要同时考虑经典列表与 WinUI 容器。
- 实测只说明窗口结构和属性读取成功，不代表扩展在该构建上经过启用、渲染、崩溃恢复或卸载验证。原始探测数据在本地 `artifacts/backdrop-probe/result.json`，不含窗口标题、文件名或路径。

## 路线比较

| 路线 | 能做到什么 | 结论 |
| --- | --- | --- |
| 仅改变 Explorer 整窗透明度 | 所有文字、图标也一起变淡，无法得到独立背景虚化 | 不作为产品方案 |
| 只设置 DWM backdrop | Windows 11 的 Mica/Acrylic 材质；不能自动清除不透明子控件背景 | 可用于本软件预览，但不足以证明原生 Explorer 效果 |
| 独立原生扩展处理 Explorer 背景 | 可清除列表/地址栏/WinUI 背景，应用系统材质和混色 | 后续实验路线；需验证特定 Windows 构建 |
| 自定义图片加模糊 | 图片解码、缩放、分层绘制和缓存，并保持文件列表文字清晰 | 独立第二阶段，不能以“已支持毛玻璃”推断已支持 |

## 下一阶段建议

先实现单独的扩展兼容原型，限定测试系统与架构，不随主软件默认加载。设置支持效果关闭/Acrylic/Mica、混色强度、恢复默认，并验证 Explorer 多标签页、浅深色、高 DPI、多屏、睡眠唤醒、崩溃后的卸载路径。通过测试后再接入软件首页入口。

图片背景随后提供图片选择、模糊强度、遮罩明暗、文字对比度预览。将“每个文件夹独立背景”和“所有 Explorer 窗口统一背景”分别设计；当前未选择或实现这两种作用范围。

## 依据

- [Microsoft DWM_SYSTEMBACKDROP_TYPE](https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwm_systembackdrop_type)：该系统材质接口最低要求 Windows 11 build 22621。
- [ExplorerBlurMica](https://github.com/Maplespe/ExplorerBlurMica)：项目介绍支持 Blur/Acrylic/Mica、混色和清除 WinUI 背景，安装需要注册原生 DLL，并有卸载与故障恢复说明。它的 README 没有把自定义图片背景列为功能。此次仅阅读其公开文档，没有复制、链接或捆绑其代码与二进制；未来若采用需核对 LGPL-3.0 分发要求。
- [Microsoft Desktop.ini 自定义](https://learn.microsoft.com/en-us/windows/win32/shell/how-to-customize-folders-with-desktop-ini)：当前文档中的文件夹图标/提示配置不能替代上述背景渲染模块。
