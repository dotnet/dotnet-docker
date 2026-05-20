#!/usr/bin/env pwsh
# Shows all PR checks as a summary table, then expands AzDO build timelines for any
# checks that point at Azure Pipelines (https://dev.azure.com/...).
# Requires `gh` CLI authenticated against the target repo.
#
# Usage:
#   ./Show-PullRequestBuilds.ps1 -PullRequest 2100
#   ./Show-PullRequestBuilds.ps1 -PullRequest 2100 -Repo dotnet/docker-tools
#   ./Show-PullRequestBuilds.ps1 -PullRequest 2100 -ShowAllTasks

[CmdletBinding()]
param(
    [Parameter(Mandatory)][int]    $PullRequest,
    [string] $Repo,
    [switch] $ShowAllTasks
)

$ErrorActionPreference = "Stop"

$ghArgs = @("pr", "view", $PullRequest, "--json", "statusCheckRollup")
if ($Repo) { $ghArgs += @("--repo", $Repo) }

$checksJson = & gh @ghArgs 2>&1
if ($LASTEXITCODE -ne 0) {
    throw "gh pr view failed: $checksJson"
}

$checks = ($checksJson | ConvertFrom-Json).statusCheckRollup

# statusCheckRollup mixes two shapes:
#   CheckRun:      { name, status, conclusion, detailsUrl, workflowName }
#   StatusContext: { context, state, targetUrl, description }
# Normalize them.
$normalized = foreach ($check in $checks) {
    if ($check.PSObject.Properties.Name -contains "context") {
        [pscustomobject]@{
            Name  = $check.context
            State = $check.state
            Url   = $check.targetUrl
        }
    }
    else {
        $state = if ($check.conclusion) { $check.conclusion } else { $check.status }
        [pscustomobject]@{
            Name  = $check.name
            State = $state
            Url   = $check.detailsUrl
        }
    }
}

# AzDO build results URLs look like:
#   https://dev.azure.com/<org>/<project>/_build/results?buildId=<id>...
$pattern = '^https?://dev\.azure\.com/(?<org>[^/]+)/(?<project>[^/]+)/_build/results\?.*buildId=(?<buildId>\d+)'

$builds = @()
foreach ($check in $normalized) {
    if (-not $check.Url) { continue }
    $match = [regex]::Match($check.Url, $pattern)
    if (-not $match.Success) { continue }

    $builds += [pscustomobject]@{
        Org     = $match.Groups["org"].Value
        Project = $match.Groups["project"].Value
        BuildId = [int]$match.Groups["buildId"].Value
    }
}

# Deduplicate by buildId (a single build can produce multiple check-run rows).
$builds = $builds | Sort-Object BuildId -Unique

$title = if ($Repo) { "$Repo#$PullRequest" } else { "PR #$PullRequest" }
Write-Host "## Checks for $title"
Write-Host ""
Write-Host "$($normalized.Count) check(s); $($builds.Count) Azure Pipelines build(s)."
Write-Host ""

if ($normalized.Count -gt 0) {
    Write-Host "Check | State | URL"
    Write-Host "--- | --- | ---"
    foreach ($check in $normalized | Sort-Object Name) {
        Write-Host "$($check.Name) | $($check.State) | $($check.Url)"
    }
    Write-Host ""
}

if ($builds.Count -eq 0) { return }

$timelineScript = "$PSScriptRoot/Show-BuildTimeline.ps1"

foreach ($build in $builds) {
    Write-Host "---"
    Write-Host ""
    & $timelineScript `
        -Organization $build.Org `
        -Project $build.Project `
        -BuildId $build.BuildId `
        -ShowAllTasks:$ShowAllTasks
}
