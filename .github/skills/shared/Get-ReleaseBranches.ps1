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

$upstreamRemote = Get-RemoteName -UrlPattern 'github\.com[:/]dotnet/'
$dncengRemote = Get-RemoteName -UrlPattern 'dev\.azure\.com/dnceng/'

Write-Host "Fetching from '$upstreamRemote' and '$dncengRemote' remotes..."
git fetch $upstreamRemote 2>&1 | Out-Null
git fetch $dncengRemote 2>&1 | Out-Null

Write-Host ""
Write-Host "## Public release branches (most recent first)"
Write-Host ""
git branch -r --list "$upstreamRemote/release/*" --sort=-creatordate | Select-Object -First 5

Write-Host ""
Write-Host "## Internal release branches (most recent first)"
Write-Host ""
git branch -r --list "$dncengRemote/internal/release/*" --sort=-creatordate | Select-Object -First 5
