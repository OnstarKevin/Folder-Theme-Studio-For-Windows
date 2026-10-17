# Island Media Integration Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** 补齐灵动岛在线歌词、媒体信息/控制及消息接口，生成 beta.16 GitHub 资料并更新现有介绍网站。
**Architecture:** 保留 WPF/WinRT，独立歌词后台协调器和 localhost 通知桥，按接口注入验证。网络数据只影响对应模块；窗口保持固定小尺寸。
**Tech Stack:** C# .NET 8 WPF, HttpClient, WinRT SMTC/UserNotificationListener, TcpListener, existing Sites static source.
**Spec:** ../specs/2026-10-04-island-media-integration.md

## Constraints and review focus
- 260×64 固定窗口；本地 LRC 优先；在线歌词可关闭；不保存剪贴板/通知历史。
- 验证服务异常/畸形响应、快切歌、无歌手视频、网络超时、通知请求来源和窗口关闭。
- 不提交用户的旧未完成工作，不代为上传 GitHub；网站更新已获用户授权。

## Task 1: Media and lyrics
- [x] 添加 OnlineLyricsSource / LyricsCoordinator、媒体元信息和控制接口。
- [x] 先写歌词匹配、缓存/取消、模块不阻塞与控制失败测试，再实现并验证。
- [x] 设置加入在线歌词开关，迷你摘要轮换与右键媒体控制。

## Task 2: Notifications
- [x] 实现 localhost 带令牌消息桥及设置，原生通知按授权结果降级。
- [x] 验证真实 TCP 请求、鉴权、畸形数据、关闭/重新启用。

## Task 3: Release and website
- [x] 更新 beta.16 版本、README、指南、参考致谢及中英推广文案。
- [x] 一次完整回归、一次新上下文代码审查，修复重要问题。
- [x] 生成安装包、源码包、GitHub 上传文件夹，校验版本与哈希。
- [x] 更新既有 Sites 源码与下载资产，发布并确认成功状态。

## Ledger
- Ruling: 在用户已授权的现有工作树内执行，不另外创建工作树或修改旧发布文件夹。
- Ruling: 参考项目仅用作实现依据；保留标准证书校验和明确的权限/功能边界。

- Verification: 408 项完整回归通过；真实 TCP 鉴权与关闭测试通过；公开示例歌曲在线歌词实测成功。
- Review: 新上下文审查提出的歌曲时长匹配、取消旧请求和控制错误提示已修复，复查无剩余问题。
