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

function Resolve-DotnetProductUrl([string] $akaMsUrl) {
    Write-Host "Querying $akaMsUrl"
    $response = Invoke-WebRequest -Uri $akaMsUrl -Method Head
    $resolvedUrl = $response.BaseResponse.RequestMessage.RequestUri.AbsoluteUri
    Write-Host "Resolved URL: $resolvedUrl"
    return $resolvedUrl
}
