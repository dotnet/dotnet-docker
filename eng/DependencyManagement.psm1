$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

# Common functions for .NET Docker dependency management

function Get-Branch() {
    $repoRoot = (Get-Item "$PSScriptRoot").Parent.FullName
    $manifestJson = Get-Content ${repoRoot}/manifest.json | ConvertFrom-Json
    if ($manifestJson.Repos[0].Name.Contains("nightly")) {
        return "nightly"
    }
    else {
        return "main"
    }
}

function Get-IsStableBranding([string] $version) {
    return $Version.Contains("-servicing") -or $Version.Contains("-rtm")
}

function Get-ProductReleaseState() {
    if ($(Get-Branch) -ieq 'main') {
        return 'Release'
    } else {
        return 'Prerelease'
    }
}

function Get-DockerOs() {
    return docker version -f "{{ .Server.Os }}"
}
