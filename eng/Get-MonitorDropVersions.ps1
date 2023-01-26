#!/usr/bin/env pwsh

<#
.SYNOPSIS
Returns the various component versions of the latest .NET Monitor build.
#>
[cmdletbinding()]
param(
    # The release channel to use for determining the latest .NET Monitor build.
    [Parameter(Mandatory=$true, ParameterSetName="WithChannel")]
    [string]
    $Channel,

    # A file containing the latest .NET Monitor build version.
    [Parameter(Mandatory=$true, ParameterSetName="WithBuildVersionFile")]
    [ValidateScript({if (-not (Test-Path -PathType Leaf -Path $(Resolve-Path $_).Path)) { throw "Could not find file $_"} $true})]
    [string]
    $BuildVersionFilePath
)

if ($Channel) {
    $url = "https://aka.ms/dotnet/diagnostics/monitor$Channel/dotnet-monitor.nupkg.buildversion"
    $monitorVersion = $(Invoke-RestMethod $url).Trim()
} else {
    $monitorVersion = $(Get-Content $BuildVersionFilePath).Trim()
}

$versionSplit=$monitorVersion.Split('.', 3)
$majorMinorVersion="$($versionSplit[0]).$($versionSplit[1])"

$stableBranding = & $PSScriptRoot/Get-IsStableBranding.ps1 -Version $monitorVersion

Write-Output "##vso[task.setvariable variable=monitorMajorMinorVersion]$majorMinorVersion"
Write-Output "##vso[task.setvariable variable=monitorVer]$monitorVersion"
Write-Output "##vso[task.setvariable variable=stableBranding]$stableBranding"
