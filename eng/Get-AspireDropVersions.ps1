<#
.SYNOPSIS
Returns the various component versions of the latest .NET build.
#>
[cmdletbinding()]
param(
    # The release channel to use for determining the latest .NET build.
    [Parameter(ParameterSetName = "Channel")]
    [string]
    $Channel,

    [Parameter(ParameterSetName = "Explicit")]
    # Aspire version to target
    [string]
    $AspireVersion
)

$ErrorActionPreference = 'Stop'
Import-Module -force $PSScriptRoot/DependencyManagement.psm1

if ($Channel) {
    # Example channel: '8.0/daily'
    $akaMsUrl = "https://aka.ms/dotnet/${Channel}/aspire-dashboard-linux-x64.zip"
    $versionSpecificUrl = Resolve-DotnetProductUrl $akaMsUrl

    # Assume the versionSpecificUrl is a string like
    # https://dotnetbuilds.azureedge.net/public/aspire/8.0.0-preview.X.YYYYY.Z/aspire-dashboard-linux-x64.zip
    $aspireVersion = $versionSpecificUrl -replace '^.*/aspire/([^/]+)/.*$', '$1'

    if (Get-IsStableBranding $aspireVersion) {
        # The stable version for 8.0.0-preview.4.24105.1 is 8.0.0-preview.4
        $aspireVersion = $aspireVersion -replace '^(.*?)-.*$', '$1'
    }
} else {
    $aspireVersion = $AspireVersion
}

# Grab the major.minor version from the Aspire version string
$dockerfileVersion = $aspireVersion -replace '^(\d+\.\d+).*$', '$1'

Write-Output "##vso[task.setvariable variable=dockerfileVersion]$dockerfileVersion"
Write-Output "##vso[task.setvariable variable=aspireVersion]$aspireVersion"
