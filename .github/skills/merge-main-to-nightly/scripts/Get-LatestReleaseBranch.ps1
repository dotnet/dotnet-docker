#!/usr/bin/env pwsh
#
# Get-LatestReleaseBranch.ps1 — Find the latest public and internal release branches
# in dotnet/dotnet-docker by fetching from upstream/dnceng and listing by creation date.
#

Write-Host "Fetching from 'upstream' and 'dnceng' remotes..."
git fetch upstream 2>&1 | Out-Null
git fetch dnceng 2>&1 | Out-Null

Write-Host ""
Write-Host "## Public release branches (most recent first)"
Write-Host ""
git branch -r --list 'upstream/release/*' --sort=-creatordate | Select-Object -First 5

Write-Host ""
Write-Host "## Internal release branches (most recent first)"
Write-Host ""
git branch -r --list 'dnceng/internal/release/*' --sort=-creatordate | Select-Object -First 5
