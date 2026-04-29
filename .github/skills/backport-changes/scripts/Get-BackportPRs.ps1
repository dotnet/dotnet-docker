#!/usr/bin/env pwsh

# Lists pull requests with the 'needs-backport' label in dotnet/dotnet-docker.
# Separates PRs merged before the most recent Patch Tuesday (likely stale labels).

function Get-PatchTuesday ([int]$Offset = 0) {
    $target = [datetime]::Today.AddMonths($Offset)
    $firstOfMonth = [datetime]::new($target.Year, $target.Month, 1)
    $firstTuesday = $firstOfMonth
    while ($firstTuesday.DayOfWeek -ne [DayOfWeek]::Tuesday) {
        $firstTuesday = $firstTuesday.AddDays(1)
    }
    return $firstTuesday.AddDays(7)
}

function Write-PRTable ($prs) {
    Write-Host "| # | Title | Author | State | Created | Merged |"
    Write-Host "|---|-------|--------|-------|---------|--------|"
    foreach ($pr in $prs) {
        $created = ([datetime]$pr.createdAt).ToString("yyyy-MM-dd")
        $merged = if ($pr.mergedAt) { ([datetime]$pr.mergedAt).ToString("yyyy-MM-dd") } else { "" }
        $state = $pr.state
        $author = $pr.author.login
        Write-Host "| #$($pr.number) | $($pr.title) | $author | $state | $created | $merged |"
    }
}

$prs = gh pr list --repo dotnet/dotnet-docker --label needs-backport --state all --json number,title,state,createdAt,mergedAt,author --limit 100 | ConvertFrom-Json
$prs = $prs | Sort-Object createdAt

if ($prs.Count -eq 0) {
    Write-Host "No PRs found with the 'needs-backport' label."
    return
}

$patchTuesday = Get-PatchTuesday -1
$current = @()
$stale = @()

foreach ($pr in $prs) {
    if ($pr.mergedAt -and ([datetime]$pr.mergedAt) -lt $patchTuesday) {
        $stale += $pr
    } else {
        $current += $pr
    }
}

Write-Host "## Needs Backport"
Write-Host ""
Write-Host "The following pull requests are candidates to be backported from the nightly branch to the release branch."
Write-Host ""
if ($current.Count -gt 0) {
    Write-PRTable $current
} else {
    Write-Host "_None._"
}

if ($stale.Count -gt 0) {
    Write-Host ""
    Write-Host "## Possibly Stale (merged before $($patchTuesday.ToString("yyyy-MM-dd")))"
    Write-Host ""
    Write-Host 'The following pull requests have the `needs-backport` label, but were merged before the most recent release.'
    Write-Host 'They may already be backported but did not have their label removed.'
    Write-Host ""
    Write-PRTable $stale
}
