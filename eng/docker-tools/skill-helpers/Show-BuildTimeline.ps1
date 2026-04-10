#!/usr/bin/env pwsh
# Prints the build timeline as an indented tree with result indicators.
# Usage:
#   ./Show-BuildTimeline.ps1 -Organization dnceng -Project internal -BuildId 12345
#   ./Show-BuildTimeline.ps1 -Organization dnceng -Project internal -BuildId 12345 -ShowAllTasks

[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $Organization,
    [Parameter(Mandatory)][string] $Project,
    [Parameter(Mandatory)][int]    $BuildId,
    [switch] $ShowAllTasks
)

$ErrorActionPreference = "Stop"

. "$PSScriptRoot/AzureDevOps.ps1"

$build = Invoke-AzDORestMethod `
    -Organization $Organization `
    -Project $Project `
    -Endpoint "build/builds/$BuildId"

Write-Host "# Build $BuildId - $($build.definition.name)"
Write-Host ""
Write-Host "- Status:  $($build.status) $(if ($build.result) { "($($build.result))" })"
Write-Host "- Branch:  $($build.sourceBranch)"
Write-Host "- Queued:  $($build.queueTime)"
Write-Host "- URL:     $($build._links.web.href)"
Write-Host ""

$timeline = Invoke-AzDORestMethod `
    -Organization $Organization `
    -Project $Project `
    -Endpoint "build/builds/$BuildId/timeline"

$records = $timeline.records

# Build a lookup of children grouped by parentId
$childrenOf = @{}
foreach ($record in $records) {
    $parentId = $record.parentId
    if (-not $parentId) { $parentId = "" }
    if (-not $childrenOf.ContainsKey($parentId)) {
        $childrenOf[$parentId] = [System.Collections.Generic.List[object]]::new()
    }
    $childrenOf[$parentId].Add($record)
}

# Sort children by order within each group
foreach ($key in @($childrenOf.Keys)) {
    $childrenOf[$key] = $childrenOf[$key] | Sort-Object { $_.order }
}

function Write-TimelineNode([string] $nodeId, [int] $depth) {
    $children = $childrenOf[$nodeId]
    if (-not $children) { return }

    foreach ($child in $children) {
        $isTask = $child.type -eq "Task"
        $isFailing = $child.result -in @("failed", "canceled", "abandoned") -or $child.state -eq "inProgress"
        if ($isTask -and -not $ShowAllTasks -and -not $isFailing) { continue }

        $indent = "  " * $depth
        $status = if ($child.result) { $child.result } else { $child.state }

        $logId = $child.log.id
        $logLabel = if ($logId) { " #$logId" } else { "" }
        Write-Host "${indent}- $($child.type)$logLabel | $($child.name) | $status"
        Write-TimelineNode $child.id ($depth + 1)
    }
}

Write-Host "## Build Timeline"
Write-Host ""
Write-TimelineNode "" 0
Write-Host ""
