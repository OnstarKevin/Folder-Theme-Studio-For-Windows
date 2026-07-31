[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$RepositoryRoot,
    [Parameter(Mandatory)]
    [string]$SourceStagingPath,
    [Parameter(Mandatory)]
    [string]$DestinationPath
)

$ErrorActionPreference = 'Stop'
$repositoryRootPath = [IO.Path]::GetFullPath($RepositoryRoot)
$sourceStagingFullPath = [IO.Path]::GetFullPath($SourceStagingPath)
$destinationFullPath = [IO.Path]::GetFullPath($DestinationPath)
$excludedSegments = @('.git', '.worktrees', '.superpowers', 'artifacts', 'bin', 'obj', 'TestResults', 'node_modules', '.hyperframes', 'renders')
$repositoryPrefix = $repositoryRootPath.TrimEnd([IO.Path]::DirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
$minimumZipTimestamp = [datetime]'1980-01-01T00:00:00Z'
$maximumZipTimestamp = [datetime]'2107-12-31T23:59:58Z'

if (Test-Path -LiteralPath $sourceStagingFullPath) {
    throw "Source staging path already exists: $sourceStagingFullPath"
}
if (Test-Path -LiteralPath $destinationFullPath) {
    throw "Destination archive already exists: $destinationFullPath"
}

New-Item -ItemType Directory -Path $sourceStagingFullPath | Out-Null
foreach ($file in Get-ChildItem -LiteralPath $repositoryRootPath -File -Recurse -Force) {
    if ($file.Name.Equals('desktop.ini', [StringComparison]::OrdinalIgnoreCase)) { continue }
    if (-not $file.FullName.StartsWith($repositoryPrefix, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Source file resolved outside repository root: $($file.FullName)"
    }

    $relative = $file.FullName.Substring($repositoryPrefix.Length)
    $segments = $relative -split '[\\/]'
    if (@($segments | Where-Object { $excludedSegments -contains $_ }).Count -ne 0) { continue }

    $destination = Join-Path $sourceStagingFullPath $relative
    $destinationDirectory = Split-Path -Parent $destination
    if (-not (Test-Path -LiteralPath $destinationDirectory)) {
        New-Item -ItemType Directory -Path $destinationDirectory -Force | Out-Null
    }
    Copy-Item -LiteralPath $file.FullName -Destination $destination

    $copiedFile = Get-Item -LiteralPath $destination
    if ($copiedFile.LastWriteTimeUtc -lt $minimumZipTimestamp) {
        $copiedFile.LastWriteTimeUtc = $minimumZipTimestamp
    }
    elseif ($copiedFile.LastWriteTimeUtc -gt $maximumZipTimestamp) {
        $copiedFile.LastWriteTimeUtc = $maximumZipTimestamp
    }
}

Compress-Archive -Path (Join-Path $sourceStagingFullPath '*') -DestinationPath $destinationFullPath -CompressionLevel Optimal
