$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

# Common functions for .NET Docker dependency management

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
