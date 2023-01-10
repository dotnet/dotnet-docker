#!/usr/bin/env pwsh

<#
.SYNOPSIS
Checks if a package is installed in the specified container image.
#>
[cmdletbinding()]
param(
    # Name of the package to be checked.
    [Parameter(Mandatory = $true)]
    [string]
    $PackageName,

    # The image to be checked on whether it contains the specified package.
    [Parameter(Mandatory = $true)]
    [string]
    $ImageName
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

Write-Host "Scanning $ImageName..."
$scan = $(docker run --rm anchore/syft packages $ImageName -q --output json | ConvertFrom-Json)
if (-not $?) {
    Write-Error "Failed to scan packages of $ImageName"
    return
}

$matchingPackage = $($scan.artifacts | Where-Object { $_.name -eq $PackageName })

if ($matchingPackage) {    
    $result = $true
}
else {
    $result = $false
}

Write-Host
Write-Output $result
