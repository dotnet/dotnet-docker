#!/usr/bin/env pwsh
# Lists all build runs in the last N hours for pipelines under a given folder.
# Usage:
#   ./Get-RecentBuilds.ps1 -Organization dnceng -Project internal -Folder dotnet/docker-tools
#   ./Get-RecentBuilds.ps1 -Organization dnceng -Project internal -Folder dotnet/docker-tools -Hours 48

[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $Organization,
    [Parameter(Mandatory)][string] $Project,
    [Parameter(Mandatory)][string] $Folder,
    [int] $Hours = 24
)

$ErrorActionPreference = "Stop"

. "$PSScriptRoot/AzureDevOps.ps1"

$normalizedFolder = "\" + ($Folder.Trim('\', '/') -replace '/', '\')
$minTime = [DateTime]::UtcNow.AddHours(-$Hours).ToString("o")

$definitions = Invoke-AzDORestMethod `
    -Organization $Organization `
    -Project $Project `
    -Endpoint "build/definitions" `
    -QueryParams @{ path = $normalizedFolder }

if (-not $definitions.value -or $definitions.value.Count -eq 0) {
    Write-Host "## No pipelines found in $normalizedFolder"
    return
}

$definitionIds = ($definitions.value | ForEach-Object { $_.id }) -join ","

$builds = Invoke-AzDORestMethod `
    -Organization $Organization `
    -Project $Project `
    -Endpoint "build/builds" `
    -QueryParams @{
        definitions = $definitionIds
        minTime     = $minTime
        queryOrder  = "finishTimeDescending"
    }

Write-Host "## Builds in $normalizedFolder (last $Hours hours)"
Write-Host ""
Write-Host "Found $($builds.value.Count) build(s) across $($definitions.value.Count) pipeline(s)."
Write-Host ""

if ($builds.value.Count -gt 0) {
    Write-Host "Pipeline | State | Build | Branch | Finished | Link"
    Write-Host "--- | --- | --- | --- | --- | ---"
    foreach ($build in $builds.value) {
        $state = if ($build.status -eq "completed") { $build.result } else { $build.status }
        $url = "https://dev.azure.com/$Organization/$Project/_build/results?buildId=$($build.id)"
        Write-Host "$($build.definition.name) | $state | $($build.id) | $($build.sourceBranch) | $($build.finishTime) | $url"
    }
}
