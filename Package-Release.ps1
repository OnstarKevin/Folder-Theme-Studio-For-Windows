[CmdletBinding()]
param(
    [ValidatePattern('^v\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?$')]
    [string]$Version = 'v0.1.0-beta.18',
    [string]$DotNetPath = 'dotnet',
    [Parameter(Mandatory)]
    [string]$InnoCompilerPath,
    [switch]$SkipTests
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$artifactsRoot = [System.IO.Path]::GetFullPath((Join-Path $repositoryRoot 'artifacts'))
$stagingPath = [System.IO.Path]::GetFullPath((Join-Path $artifactsRoot ".staging-$Version-win-x64"))
$setupName = "FolderThemeStudio-$Version-setup.exe"
$setupPath = [System.IO.Path]::GetFullPath((Join-Path $artifactsRoot $setupName))
$checksumPath = [System.IO.Path]::GetFullPath((Join-Path $artifactsRoot "FolderThemeStudio-$Version-setup.sha256"))
$sourceStagingPath = [System.IO.Path]::GetFullPath((Join-Path $artifactsRoot ".source-$Version"))
$sourceName = "FolderThemeStudio-$Version-source.zip"
$sourcePath = [System.IO.Path]::GetFullPath((Join-Path $artifactsRoot $sourceName))
$sourceChecksumPath = [System.IO.Path]::GetFullPath((Join-Path $artifactsRoot "FolderThemeStudio-$Version-source.sha256"))
$projectPath = Join-Path $repositoryRoot 'src\FolderThemeStudio.App\FolderThemeStudio.App.csproj'
$solutionPath = Join-Path $repositoryRoot 'FolderThemeStudio.sln'
$releaseNotesPath = Join-Path $repositoryRoot "docs\releases\$Version.md"
$installerPath = Join-Path $repositoryRoot 'build\FolderThemeStudio.iss'
$verificationPath = Join-Path $repositoryRoot 'build\Verify-Release.ps1'
$sourceArchiveScriptPath = Join-Path $repositoryRoot 'build\New-SourceArchive.ps1'
$runtimeUrl = 'https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/8.0.29/windowsdesktop-runtime-8.0.29-win-x64.exe'
$runtimeSha256 = 'c0ffa16efeb7ef3ac8100a6a9d7089d9c2904ee89f1815557a79a91be584f775'

function Assert-ChildPath([string]$Path, [string]$Parent) {
    $prefix = $Parent.TrimEnd([System.IO.Path]::DirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar
    if (-not $Path.StartsWith($prefix, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Unsafe path outside artifacts directory: $Path"
    }
}

$dotnetCommand = Get-Command $DotNetPath -ErrorAction Stop
$resolvedDotNetPath = $dotnetCommand.Source
$resolvedInnoPath = (Resolve-Path -LiteralPath $InnoCompilerPath -ErrorAction Stop).Path

foreach ($required in @($projectPath, $solutionPath, $releaseNotesPath, $installerPath, $verificationPath, $sourceArchiveScriptPath, (Join-Path $repositoryRoot 'LICENSE'), (Join-Path $repositoryRoot 'README.md'), (Join-Path $repositoryRoot 'docs\USER-GUIDE.zh-CN.md'), (Join-Path $repositoryRoot 'docs\USER-GUIDE.en-US.md'))) {
    if (-not (Test-Path -LiteralPath $required)) { throw "Required input not found: $required" }
}

New-Item -ItemType Directory -Path $artifactsRoot -Force | Out-Null
Assert-ChildPath $stagingPath $artifactsRoot
Assert-ChildPath $setupPath $artifactsRoot
Assert-ChildPath $checksumPath $artifactsRoot
Assert-ChildPath $sourceStagingPath $artifactsRoot
Assert-ChildPath $sourcePath $artifactsRoot
Assert-ChildPath $sourceChecksumPath $artifactsRoot

if (Test-Path -LiteralPath $stagingPath) { Remove-Item -LiteralPath $stagingPath -Recurse -Force }
if (Test-Path -LiteralPath $sourceStagingPath) { Remove-Item -LiteralPath $sourceStagingPath -Recurse -Force }
New-Item -ItemType Directory -Path $stagingPath | Out-Null

try {
    if (-not $SkipTests) {
        & $resolvedDotNetPath test $solutionPath -c Release --no-restore
        if ($LASTEXITCODE -ne 0) { throw "Release tests failed with exit code $LASTEXITCODE." }
    }

    & $resolvedDotNetPath publish $projectPath -c Release -r win-x64 --self-contained false --no-restore -o $stagingPath /p:PublishSingleFile=true /p:PublishReadyToRun=false
    if ($LASTEXITCODE -ne 0) { throw "Publish failed with exit code $LASTEXITCODE." }

    $executablePath = Join-Path $stagingPath 'FolderThemeStudio.App.exe'
    if (-not (Test-Path -LiteralPath $executablePath)) { throw 'Published executable was not created.' }

    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'LICENSE') -Destination (Join-Path $stagingPath 'LICENSE')
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'README.md') -Destination (Join-Path $stagingPath 'README.md')
    Copy-Item -LiteralPath $releaseNotesPath -Destination (Join-Path $stagingPath 'RELEASE-NOTES.md')
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'docs\USER-GUIDE.zh-CN.md') -Destination (Join-Path $stagingPath 'USER-GUIDE.zh-CN.md')
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'docs\USER-GUIDE.en-US.md') -Destination (Join-Path $stagingPath 'USER-GUIDE.en-US.md')
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'docs\DYNAMIC-ISLAND.zh-CN.md') -Destination (Join-Path $stagingPath 'DYNAMIC-ISLAND.zh-CN.md')
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'docs\EXPLORER-BACKDROP-FEASIBILITY.zh-CN.md') -Destination (Join-Path $stagingPath 'EXPLORER-BACKDROP-FEASIBILITY.zh-CN.md')
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'THIRD-PARTY-NOTICES.md') -Destination (Join-Path $stagingPath 'THIRD-PARTY-NOTICES.md')
    New-Item -ItemType Directory -Path (Join-Path $stagingPath 'licenses') -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'licenses\NotchPeninsula-Apache-2.0.txt') -Destination (Join-Path $stagingPath 'licenses\NotchPeninsula-Apache-2.0.txt')

    @(
        "Folder Theme Studio $Version"
        ''
        'This is an unsigned beta; test compatibility mode in a disposable folder first.'
        'The installer downloads Microsoft .NET 8 Desktop Runtime when it is missing.'
        'Read README.md and RELEASE-NOTES.md before launching Folder Theme Studio.'
    ) | Set-Content -LiteralPath (Join-Path $stagingPath 'START-HERE.txt') -Encoding UTF8

    if (Test-Path -LiteralPath $setupPath) { Remove-Item -LiteralPath $setupPath -Force }
    if (Test-Path -LiteralPath $checksumPath) { Remove-Item -LiteralPath $checksumPath -Force }

    & $verificationPath -PublishedPath $stagingPath
    if ($LASTEXITCODE -ne 0) { throw "Published output verification failed with exit code $LASTEXITCODE." }

    $appVersion = $Version.Substring(1)
    & $resolvedInnoPath "/DAppVersion=$appVersion" "/DSourceDir=$stagingPath" "/DOutputDir=$artifactsRoot" "/DRuntimeUrl=$runtimeUrl" "/DRuntimeSha256=$runtimeSha256" $installerPath
    if ($LASTEXITCODE -ne 0) { throw "Installer compilation failed with exit code $LASTEXITCODE." }
    if (-not (Test-Path -LiteralPath $setupPath)) { throw 'Installer executable was not created.' }

    & $verificationPath -PublishedPath $stagingPath -SetupPath $setupPath
    if ($LASTEXITCODE -ne 0) { throw "Installer verification failed with exit code $LASTEXITCODE." }

    $hash = (Get-FileHash -LiteralPath $setupPath -Algorithm SHA256).Hash.ToLowerInvariant()
    "$hash  $setupName" | Set-Content -LiteralPath $checksumPath -Encoding ascii -NoNewline

    # 网站与 GitHub 资料使用同一份已验证安装包，避免版本和校验值不同步。
    $websitePath = Join-Path $repositoryRoot 'website'
    # 只有网站明确引用当前版本时才更新下载区，避免本地试用版本覆盖线上旧版的校验值。
    if ((Test-Path -LiteralPath (Join-Path $websitePath 'index.html')) -and [IO.File]::ReadAllText((Join-Path $websitePath 'index.html')).Contains($setupName)) {
        $downloadsPath = Join-Path $websitePath 'downloads'
        New-Item -ItemType Directory -Path $downloadsPath -Force | Out-Null
        Copy-Item -LiteralPath $setupPath, $checksumPath -Destination $downloadsPath
        Copy-Item -LiteralPath $releaseNotesPath -Destination (Join-Path $downloadsPath 'RELEASE-NOTES.md')
        Copy-Item -LiteralPath (Join-Path $repositoryRoot 'docs\DYNAMIC-ISLAND.zh-CN.md') -Destination (Join-Path $downloadsPath 'DYNAMIC-ISLAND.zh-CN.md')
        $websiteIndex = Join-Path $websitePath 'index.html'
        $html = [IO.File]::ReadAllText($websiteIndex)
        $html = [regex]::Replace($html, '(<code id="checksum-value"[^>]*>)[a-f0-9]{64}(</code>)', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $match.Groups[1].Value + $hash + $match.Groups[2].Value })
        $setupSize = ((Get-Item -LiteralPath $setupPath).Length / 1MB).ToString('0.0', [Globalization.CultureInfo]::InvariantCulture)
        $html = [regex]::Replace($html, '<li>\d+(?:\.\d+)? (?:MB|MiB)</li>', "<li>$setupSize MiB</li>")
        [IO.File]::WriteAllText($websiteIndex, $html, [Text.UTF8Encoding]::new($false))
    }

    foreach ($oldSource in @($sourcePath, $sourceChecksumPath)) {
        if (Test-Path -LiteralPath $oldSource) { Remove-Item -LiteralPath $oldSource -Force }
    }
    & $sourceArchiveScriptPath -RepositoryRoot $repositoryRoot -SourceStagingPath $sourceStagingPath -DestinationPath $sourcePath
    $sourceHash = (Get-FileHash -LiteralPath $sourcePath -Algorithm SHA256).Hash.ToLowerInvariant()
    "$sourceHash  $sourceName" | Set-Content -LiteralPath $sourceChecksumPath -Encoding ascii -NoNewline
    & $verificationPath -PublishedPath $stagingPath -SetupPath $setupPath -SourceZipPath $sourcePath
    if ($LASTEXITCODE -ne 0) { throw "Source archive verification failed with exit code $LASTEXITCODE." }
    Write-Host "Created $setupPath"
    Write-Host "SHA-256 $hash"
    Write-Host "Created $sourcePath"
    Write-Host "SHA-256 $sourceHash"
}
finally {
    if (Test-Path -LiteralPath $stagingPath) {
        Assert-ChildPath $stagingPath $artifactsRoot
        Remove-Item -LiteralPath $stagingPath -Recurse -Force
    }
    if (Test-Path -LiteralPath $sourceStagingPath) {
        Assert-ChildPath $sourceStagingPath $artifactsRoot
        Remove-Item -LiteralPath $sourceStagingPath -Recurse -Force
    }
}
