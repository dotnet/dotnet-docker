#!/usr/bin/env pwsh

<#
.SYNOPSIS
Returns the various component versions of the latest .NET Monitor build.
#>
[cmdletbinding()]
param(
    # The release channel to use for determining the latest .NET Monitor build.
    [Parameter(Mandatory)]
    [string]
    $Channel
)

$url = "https://aka.ms/dotnet/diagnostics/monitor$Channel/dotnet-monitor.nupkg.buildversion"
$monitorVersion = $(Invoke-RestMethod $url).Trim()

Write-Output "##vso[task.setvariable variable=monitorVer]$monitorVersion"
