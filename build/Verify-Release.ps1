[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$PublishedPath,
    [string]$SetupPath,
    [string]$SourceZipPath
)

$ErrorActionPreference = 'Stop'
$publishedRoot = [IO.Path]::GetFullPath($PublishedPath)
$executablePath = Join-Path $publishedRoot 'FolderThemeStudio.App.exe'
if (-not (Test-Path -LiteralPath $executablePath)) {
    throw "Published executable not found: $executablePath"
}

$executable = Get-Item -LiteralPath $executablePath
if ($executable.Length -ge 30MB) {
    throw "Published executable must be smaller than 30 MiB, got $($executable.Length) bytes."
}

$bundledRuntimeFiles = @(
    'coreclr.dll'
    'clrjit.dll'
    'hostfxr.dll'
    'System.Private.CoreLib.dll'
) | Where-Object { Test-Path -LiteralPath (Join-Path $publishedRoot $_) }
if ($bundledRuntimeFiles.Count -ne 0) {
    throw "Framework-dependent output contains bundled runtime files: $($bundledRuntimeFiles -join ', ')"
}

if ($SetupPath) {
    $setup = Get-Item -LiteralPath ([IO.Path]::GetFullPath($SetupPath))
    if ($setup.Length -ge 25MB) {
        throw "Online setup must be smaller than 25 MiB, got $($setup.Length) bytes."
    }

    $signature = [IO.File]::ReadAllBytes($setup.FullName)[0..1]
    if ($signature[0] -ne 77 -or $signature[1] -ne 90) {
        throw 'Setup executable does not have an MZ signature.'
    }
}

if ($SourceZipPath) {
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $sourceZip = [IO.Path]::GetFullPath($SourceZipPath)
    $archive = [IO.Compression.ZipFile]::OpenRead($sourceZip)
    try {
        $entryNames = @($archive.Entries | ForEach-Object { $_.FullName.Replace('\', '/') })
        foreach ($requiredEntry in @('README.md', 'LICENSE', 'docs/USER-GUIDE.zh-CN.md', 'docs/USER-GUIDE.en-US.md')) {
            if ($entryNames -notcontains $requiredEntry) { throw "Source archive is missing $requiredEntry." }
        }
        $forbidden = @($entryNames | Where-Object { $_ -match '(^|/)(\.git|\.worktrees|\.superpowers|artifacts|bin|obj|TestResults)(/|$)' })
        if ($forbidden.Count -ne 0) { throw "Source archive contains excluded paths: $($forbidden -join ', ')" }
        $desktopIni = @($entryNames | Where-Object { $_ -match '(^|/)desktop\.ini$' })
        if ($desktopIni.Count -ne 0) { throw "Source archive contains desktop.ini: $($desktopIni -join ', ')" }
    }
    finally {
        $archive.Dispose()
    }
}

[pscustomobject]@{
    PublishedExecutableBytes = $executable.Length
    SetupBytes = if ($SetupPath) { $setup.Length } else { $null }
} | ConvertTo-Json -Compress
