# 参与贡献

开发需要 Windows x64、.NET 8 SDK；生成安装器还需要 Inno Setup 6。

```powershell
dotnet restore FolderThemeStudio.sln
dotnet test FolderThemeStudio.sln -c Release
dotnet build src\FolderThemeStudio.App\FolderThemeStudio.App.csproj -c Release
powershell -NoProfile -ExecutionPolicy Bypass -File .\build\Package-Release.ps1 -Version v0.1.0-beta.7 -InnoCompilerPath "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"
```

涉及注册表、`Desktop.ini`、路径遍历或恢复逻辑的改动必须包含回归测试，不得使用真实个人目录作为测试目标。不要在 Issue、日志或截图中公开个人路径、恢复快照或用户名。

`artifacts/` 不提交到 Git；其中的安装器、源码包和校验文件用于上传 GitHub Release。
