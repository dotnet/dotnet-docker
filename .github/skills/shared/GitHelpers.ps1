#!/usr/bin/env pwsh

# Shared git helpers for the release/backport skills. Dot-source this file:
#   . "$PSScriptRoot/GitHelpers.ps1"

# Finds the single git remote whose URL matches the given pattern.
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

# Finds the public (GitHub dotnet) remote.
function Get-PublicRemoteName {
    return Get-RemoteName -UrlPattern 'github\.com[:/]dotnet/'
}

# Fetches the public remote and returns the most recently created public release
# branch as a remote-qualified ref (e.g. "upstream/release/2026-06B"). The remote
# prefix is kept intentionally so callers use the remote-tracking branch rather
# than a possibly-stale local branch.
function Get-LatestPublicReleaseBranch {
    $remote = Get-PublicRemoteName
    git fetch $remote 2>&1 | Out-Null

    $branch = git branch -r --list "$remote/release/*" --sort=-creatordate `
        | Select-Object -First 1

    if ([string]::IsNullOrWhiteSpace($branch)) {
        throw "No release branches found on remote '$remote'."
    }

    return $branch.Trim()
}
