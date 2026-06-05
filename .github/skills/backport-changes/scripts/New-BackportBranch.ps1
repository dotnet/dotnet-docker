#!/usr/bin/env pwsh

# Determines the latest public release branch and creates a working branch for
# backporting based on it. Combines steps 1 and 2 of the backport workflow:
# detect the public remote, fetch it, find the latest release branch, and create
# a `backport-<release>` branch off its up-to-date tip.

[CmdletBinding()]
param(
    # Optional release branch override (e.g. "release/2026-05B"). Defaults to the
    # most recently created public release branch.
    [string] $ReleaseBranch
)

. "$PSScriptRoot/../../shared/GitHelpers.ps1"

# Require a clean working tree so uncommitted changes are not carried onto the
# new branch.
$status = git status --porcelain
if (-not [string]::IsNullOrWhiteSpace($status)) {
    throw "Working tree is not clean. Commit or stash your changes before creating the backport branch."
}

if ([string]::IsNullOrWhiteSpace($ReleaseBranch)) {
    # Get-LatestPublicReleaseBranch resolves the public remote, fetches it, and
    # returns a remote-qualified ref (e.g. "upstream/release/2026-06B").
    $startPoint = Get-LatestPublicReleaseBranch
    Write-Host "Latest public release branch: $startPoint"
} else {
    $remote = Get-PublicRemoteName
    Write-Host "Fetching latest from '$remote'..."
    git fetch $remote 2>&1 | Out-Null
    $startPoint = "$remote/$ReleaseBranch"
}

if (-not (git rev-parse --verify --quiet "$startPoint")) {
    throw "Start point '$startPoint' was not found on the public remote."
}

$releaseName = $startPoint -replace '^.*release/', ''
$workingBranch = "backport-$releaseName"

if (git rev-parse --verify --quiet "refs/heads/$workingBranch") {
    throw "Branch '$workingBranch' already exists. Delete it or check it out before re-running."
}

# Use --no-track so the working branch does not adopt the public release branch
# as its upstream.
git switch --no-track -c $workingBranch $startPoint

Write-Host ""
Write-Host "Created working branch '$workingBranch' based on '$startPoint'."
Write-Host "Next: get backport candidates with scripts/Get-BackportPRs.ps1."
