#!/usr/bin/env pwsh

# Shows the file-level diff between HEAD and the nightly branch on the public
# (GitHub) remote. Run this from your backport working branch (with cherry-picks
# applied and committed) to verify the backport: every reported difference should
# be an expected divergence (see SKILL.md). Investigate anything that is not.
# The working tree must be clean.

. "$PSScriptRoot/../../shared/GitHelpers.ps1"

# Detect the public (GitHub) remote so we can fetch and diff against its nightly.
$gitHubRemote = Get-PublicRemoteName

# Require a clean working tree so that all cherry-picks and regenerated files are
# committed. This lets us diff HEAD (including new files) rather than the working
# tree, which would miss untracked files.
$status = git status --porcelain
if (-not [string]::IsNullOrWhiteSpace($status)) {
    throw "Working tree is not clean. Commit or stash your changes before verifying the backport, then re-run this script."
}

Write-Host "Fetching latest from '$gitHubRemote'..."
git fetch $gitHubRemote 2>&1 | Out-Null

$nightlyRef = "$gitHubRemote/nightly"
if (-not (git rev-parse --verify --quiet "$nightlyRef")) {
    throw "Branch '$nightlyRef' was not found on remote '$gitHubRemote'."
}

Write-Host ""
Write-Host "# Backport verification diff"
Write-Host ""
Write-Host "Comparing HEAD against nightly on the public remote."
Write-Host ""
Write-Host "- Nightly: ``$nightlyRef``"
Write-Host ""
Write-Host "Every difference below should be an expected divergence (see SKILL.md)."
Write-Host "Investigate anything that is not."
Write-Host ""
Write-Host "## Changed files (HEAD vs nightly)"
Write-Host ""

$nameStatus = git diff --name-status "$nightlyRef" HEAD
if ([string]::IsNullOrWhiteSpace($nameStatus)) {
    Write-Host "_No differences._"
} else {
    Write-Host '```'
    Write-Host ($nameStatus -join "`n")
    Write-Host '```'
}
