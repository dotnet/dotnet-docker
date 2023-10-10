#!/usr/bin/env pwsh

<#
.SYNOPSIS
Returns the various component versions of the latest .NET Monitor build.
#>
[cmdletbinding()]
param(
    # A file containing the latest .NET Monitor build version.
    [Parameter(Mandatory=$true)]
    [ValidateScript({if (-not (Test-Path -PathType Leaf -Path $(Resolve-Path $_).Path)) { throw "Could not find file $_"} $true})]
    [string]
    $BuildVersionFilePath
)

$monitorVersion = $(Get-Content $BuildVersionFilePath).Trim()

$versionSplit=$monitorVersion.Split('.', 3)
$majorMinorVersion="$($versionSplit[0]).$($versionSplit[1])"

$stableBranding = & $PSScriptRoot/Get-IsStableBranding.ps1 -Version $monitorVersion

$versionInfos = @(
    @{
        DockerfileVersion = $majorMinorVersion
        MonitorVersion = $monitorVersion
        StableBranding = $stableBranding
        ComputeShas = $false
    }
)

Write-Output "##vso[task.setvariable variable=versionInfos]$($versionInfos | ConvertTo-Json -Compress -AsArray)"
