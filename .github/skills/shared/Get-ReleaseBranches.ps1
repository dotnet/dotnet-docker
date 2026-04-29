#!/usr/bin/env pwsh

# Finds the latest public and internal release branches in dotnet/dotnet-docker
# by fetching from GitHub (public) and Azure DevOps (internal) and listing by
# branch creation date.

function Get-RemoteName {
	param(
		[Parameter(Mandatory = $true)]
		[string] $UrlPattern
	)

	$matchingRemotes = @(git remote | Where-Object {
		$remoteName = $_
		$remoteUrl = git remote get-url $remoteName 2>$null
		$remoteUrl -match $UrlPattern
	})

	if ($matchingRemotes.Count -eq 0) {
		throw "Unable to find a remote with a URL matching '$UrlPattern'."
	}

	if ($matchingRemotes.Count -gt 1) {
		throw "Found multiple remotes with URLs matching '$UrlPattern': $($matchingRemotes -join ', ')."
	}

	return $matchingRemotes[0]
}

# Show basic documentation
Write-Host "# Release branches"
Write-Host "Release branches follow the Windows release naming scheme."
Write-Host "Example: 2026-04B refers to the second week (B) of April 2026."

$gitHubRemote = Get-RemoteName -UrlPattern 'github\.com[:/]dotnet/'
$dncengRemote = Get-RemoteName -UrlPattern 'dev\.azure\.com/dnceng/'
$upstreamRemoteUrl = git remote get-url $gitHubRemote
$dncengRemoteUrl = git remote get-url $dncengRemote

Write-Host ""
Write-Host "## Repo configuration"
Write-Host "This repo is configured with the following remotes:"
Write-Host ""
Write-Host "Source | Remote Name | Remote URL"
Write-Host "--- | --- | ---"
Write-Host "Public | $gitHubRemote | $upstreamRemoteUrl"
Write-Host "Internal | $dncengRemote | $dncengRemoteUrl"

git fetch $gitHubRemote 2>&1 | Out-Null
git fetch $dncengRemote 2>&1 | Out-Null

$numberOfBranches = 5

Write-Host ""
Write-Host "## ${numberOfBranches} most recent public release branches"
git branch -r --list "$gitHubRemote/release/*" --sort=-creatordate `
    | Select-Object -First $numberOfBranches `
    | ForEach-Object { Write-Host "- $($_.Trim())" }

Write-Host ""
Write-Host "## ${numberOfBranches} most recent internal release branches"
git branch -r --list "$dncengRemote/internal/release/*" --sort=-creatordate `
    | Select-Object -First $numberOfBranches `
    | ForEach-Object { Write-Host "- $($_.Trim())" }
