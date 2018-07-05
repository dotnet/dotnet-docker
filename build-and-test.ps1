#!/usr/bin/env pwsh
[cmdletbinding()]
param(
    [switch]$UseImageCache,
    [string]$VersionFilter,
    [string]$ArchitectureFilter,
    [string]$OSFilter,
    [switch]$CleanupDocker
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Invoke-CleanupDocker($ActiveOS)
{
    if ($CleanupDocker) {
        if ("$ActiveOS" -eq "windows") {
            docker ps -a -q | ForEach-Object { docker rm -f $_ }

            # Windows base images are large, preserve them to avoid the overhead of pulling each time.
            docker images |
                Where-Object {
                    -Not ($_.StartsWith("microsoft/nanoserver ")`
                    -Or $_.StartsWith("microsoft/windowsservercore ")`
                    -Or $_.StartsWith("REPOSITORY ")) } |
                ForEach-Object { $_.Split(' ', [System.StringSplitOptions]::RemoveEmptyEntries)[2] } |
                Select-Object -Unique |
                ForEach-Object { docker rmi -f $_ }
        }
        else {
            docker system prune -a -f
        }
    }
}

$(docker version) | % { Write-Host "$_" }
$activeOS = docker version -f "{{ .Server.Os }}"
Invoke-CleanupDocker $activeOS

if ($UseImageCache) {
    $optionalDockerBuildArgs = ""
}
else {
    $optionalDockerBuildArgs = "--no-cache"
}

$manifest = Get-Content "manifest.json" | ConvertFrom-Json
$manifestRepo = $manifest.Repos[0]
$builtTags = @()

$buildFilter = "*"
if (-not [string]::IsNullOrEmpty($versionFilter))
{
    $buildFilter = "$versionFilter/$buildFilter"
}
if (-not [string]::IsNullOrEmpty($OsFilter))
{
    $buildFilter = "$buildFilter/$OsFilter/*"
}

try {
    $manifestRepo.Images |
    ForEach-Object {
        $images = $_
        $_.Platforms |
            Where-Object { $_.os -eq "$activeOS" } |
            Where-Object { [string]::IsNullOrEmpty($buildFilter) -or $_.dockerfile -like "$buildFilter" } |
            Where-Object { ( [string]::IsNullOrEmpty($ArchitectureFilter) -and -not [bool]($_.PSobject.Properties.name -match "architecture"))`
                -or ( [bool]($_.PSobject.Properties.name -match "architecture") -and $_.architecture -eq "$ArchitectureFilter" ) } |
            ForEach-Object {
                $dockerfilePath = $_.dockerfile
                $tags = [array]($_.Tags | ForEach-Object { $_.PSobject.Properties })
                $qualifiedTags = $tags | ForEach-Object { $manifestRepo.Name + ':' + $_.Name}
                $formattedTags = $qualifiedTags -join ', '
                Write-Host "--- Building $formattedTags from $dockerfilePath ---"
                Invoke-Expression "docker build $optionalDockerBuildArgs -t $($qualifiedTags -join ' -t ') $dockerfilePath"
                if ($LastExitCode -ne 0) {
                    throw "Failed building $formattedTags"
                }

                $builtTags += $formattedTags
            }
    }

    ./tests/run-tests.ps1 -VersionFilter $VersionFilter -ArchitectureFilter $ArchitectureFilter -OSFilter $OSFilter -IsLocalRun
    Write-Host "Tags built and tested:`n$($builtTags | Out-String)"
}
finally {
    Invoke-CleanupDocker $activeOS
}
