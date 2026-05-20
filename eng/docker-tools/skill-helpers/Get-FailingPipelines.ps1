#!/usr/bin/env pwsh
# Lists pipeline definitions in a folder whose most recent completed build did not succeed.
# Usage:
#   ./Get-FailingPipelines.ps1 -Organization dnceng -Project internal -Folder dotnet/docker-tools

[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $Organization,
    [Parameter(Mandatory)][string] $Project,
    [Parameter(Mandatory)][string] $Folder,
    [switch] $IncludeWarnings
)

$ErrorActionPreference = "Stop"

. "$PSScriptRoot/AzureDevOps.ps1"

# Normalize folder: accept "dotnet/docker-tools" or "\dotnet\docker-tools".
$normalizedFolder = "\" + ($Folder.Trim('\', '/') -replace '/', '\')

$failingResults = @("failed", "canceled")
if ($IncludeWarnings) {
    $failingResults += "partiallySucceeded"
}

$definitions = Invoke-AzDORestMethod `
    -Organization $Organization `
    -Project $Project `
    -Endpoint "build/definitions" `
    -QueryParams @{
        path                = $normalizedFolder
        includeLatestBuilds = "true"
    }

$failing = @()
foreach ($def in $definitions.value) {
    $latest = $def.latestCompletedBuild
    if (-not $latest) { continue }
    if ($failingResults -notcontains $latest.result) { continue }

    $failing += [pscustomobject]@{
        Definition  = $def.name
        Result      = $latest.result
        BuildId     = $latest.id
        BuildNumber = $latest.buildNumber
        Branch      = $latest.sourceBranch
        FinishTime  = $latest.finishTime
        Url         = "https://dev.azure.com/$Organization/$Project/_build/results?buildId=$($latest.id)"
    }
}

Write-Host "## Failing pipelines in $normalizedFolder"
Write-Host ""
Write-Host "Found $($failing.Count) of $($definitions.value.Count) pipeline(s) with a failing latest run."
Write-Host ""

if ($failing.Count -gt 0) {
    Write-Host "Pipeline | Result | Build | Branch | Finished | Link"
    Write-Host "--- | --- | --- | --- | --- | ---"
    foreach ($item in $failing | Sort-Object Definition) {
        Write-Host "$($item.Definition) | $($item.Result) | $($item.BuildId) | $($item.Branch) | $($item.FinishTime) | $($item.Url)"
    }
}
