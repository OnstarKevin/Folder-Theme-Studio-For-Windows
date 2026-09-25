#ifndef AppVersion
  #define AppVersion "0.1.0-beta.8"
#endif
#ifndef SourceDir
  #define SourceDir "..\artifacts\publish"
#endif
#ifndef OutputDir
  #define OutputDir "..\artifacts"
#endif
#ifndef RuntimeUrl
  #define RuntimeUrl "https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/8.0.29/windowsdesktop-runtime-8.0.29-win-x64.exe"
#endif
#ifndef RuntimeSha256
  #define RuntimeSha256 "c0ffa16efeb7ef3ac8100a6a9d7089d9c2904ee89f1815557a79a91be584f775"
#endif

#define AppName "Folder Theme Studio"
#define AppExeName "FolderThemeStudio.App.exe"
#define RuntimeFileName "windowsdesktop-runtime-8.0.29-win-x64.exe"

[Setup]
AppId={{2D1D251D-B170-4B07-AFC5-3B16284D3DD7}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher=Folder Theme Studio contributors
DefaultDirName={autopf}\Folder Theme Studio
DefaultGroupName=Folder Theme Studio
DisableProgramGroupPage=yes
LicenseFile={#SourceDir}\LICENSE
OutputDir={#OutputDir}
OutputBaseFilename=FolderThemeStudio-v{#AppVersion}-setup
SetupIconFile=..\src\FolderThemeStudio.App\Assets\FolderThemeStudio.ico
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
UninstallDisplayIcon={app}\{#AppExeName}
VersionInfoVersion=0.1.0.8
VersionInfoProductVersion=0.1.0.8
VersionInfoDescription=Folder Theme Studio online installer

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "chinesesimplified"; MessagesFile: "compiler:Default.isl,ChineseSimplified.isl"

[CustomMessages]
english.DesktopTask=Create a desktop shortcut
chinesesimplified.DesktopTask=创建桌面快捷方式
english.AdditionalTasks=Additional shortcuts:
chinesesimplified.AdditionalTasks=其他快捷方式：
english.LaunchApp=Launch Folder Theme Studio
chinesesimplified.LaunchApp=启动文件夹主题工坊
english.DownloadingRuntime=Downloading Microsoft .NET 8 Desktop Runtime (%d%%)...
chinesesimplified.DownloadingRuntime=正在下载 Microsoft .NET 8 桌面运行时（%d%%）…
english.DownloadingRuntimeUnknown=Downloading Microsoft .NET 8 Desktop Runtime...
chinesesimplified.DownloadingRuntimeUnknown=正在下载 Microsoft .NET 8 桌面运行时…

[Tasks]
Name: "desktopicon"; Description: "{cm:DesktopTask}"; GroupDescription: "{cm:AdditionalTasks}"; Flags: unchecked

[Files]
Source: "{#SourceDir}\FolderThemeStudio.App.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\LICENSE"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\RELEASE-NOTES.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\START-HERE.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\USER-GUIDE.zh-CN.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\USER-GUIDE.en-US.md"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Folder Theme Studio"; Filename: "{app}\{#AppExeName}"
Name: "{autodesktop}\Folder Theme Studio"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchApp}"; Flags: nowait postinstall skipifsilent; Check: CanLaunchApplication

[Code]
const
  RuntimeUrl = '{#RuntimeUrl}';
  RuntimeSha256 = '{#RuntimeSha256}';
  RuntimeFileName = '{#RuntimeFileName}';

var
  RuntimeRestartRequired: Boolean;

function HasDesktopRuntime8: Boolean;
var
  RuntimeRoot: String;
  FindRec: TFindRec;
begin
  Result := False;
  RuntimeRoot := ExpandConstant('{autopf}\dotnet\shared\Microsoft.WindowsDesktop.App');
  if FindFirst(AddBackslash(RuntimeRoot) + '8.*', FindRec) then
  begin
    try
      repeat
        if (FindRec.Attributes and FILE_ATTRIBUTE_DIRECTORY) <> 0 then
        begin
          Result := True;
          Exit;
        end;
      until not FindNext(FindRec);
    finally
      FindClose(FindRec);
    end;
  end;
end;

function OnDownloadProgress(const Url, FileName: String; const Progress, ProgressMax: Int64): Boolean;
begin
  if ProgressMax > 0 then
    WizardForm.StatusLabel.Caption := Format(CustomMessage('DownloadingRuntime'), [(Progress * 100) div ProgressMax])
  else
    WizardForm.StatusLabel.Caption := CustomMessage('DownloadingRuntimeUnknown');
  Result := True;
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  RuntimePath: String;
  ResultCode: Integer;
begin
  Result := '';
  if HasDesktopRuntime8 then
    Exit;

  try
    DownloadTemporaryFile(RuntimeUrl, RuntimeFileName, RuntimeSha256, @OnDownloadProgress);
    RuntimePath := ExpandConstant('{tmp}\') + RuntimeFileName;
    if not Exec(RuntimePath, '/install /quiet /norestart', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
    begin
      Result := 'Unable to start the Microsoft .NET 8 Desktop Runtime installer.';
      Exit;
    end;

    if (ResultCode <> 0) and (ResultCode <> 3010) then
    begin
      Result := Format('Microsoft .NET 8 Desktop Runtime installation failed with exit code %d.', [ResultCode]);
      Exit;
    end;

    if ResultCode = 3010 then
    begin
      RuntimeRestartRequired := True;
      NeedsRestart := True;
    end;

    if (ResultCode = 0) and (not HasDesktopRuntime8) then
      Result := 'Microsoft .NET 8 Desktop Runtime installation completed, but the required 8.x x64 runtime was not detected.';
  except
    Result := 'Unable to download or verify Microsoft .NET 8 Desktop Runtime: ' + GetExceptionMessage;
  end;
end;

function CanLaunchApplication: Boolean;
begin
  Result := (not RuntimeRestartRequired) and HasDesktopRuntime8;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
    RegDeleteValue(HKCU, 'Software\Microsoft\Windows\CurrentVersion\Run', 'FolderThemeStudio');
end;
