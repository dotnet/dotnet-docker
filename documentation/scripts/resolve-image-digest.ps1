#!/usr/bin/env pwsh

<#
.SYNOPSIS
Resolves a given image tag to a digest. 
#>
[cmdletbinding()]
param(
    # The image tag whose digest should be returned.
    [Parameter(Mandatory = $true)]
    [string]
    $ImageTag,

    # The desired OS type of the image
    [ValidateSet("linux", "windows")]
    [string]
    $Os,

    # The desired architecture of the image
    [string]
    [ValidateSet("amd64", "arm", "arm64")]
    $Architecture,

    # The desired OS version of the image (wildcards are allowed)
    [string]
    $OsVersion
)

$manifest = docker manifest inspect $ImageTag | ConvertFrom-Json
if (-not $?) {
    Write-Error "Failed to get manifest of $ImageTag"
    return
}

if ($manifest.config.digest) {
    Write-Output $manifest.config.digest
    return
}

$manifest = $manifest.manifests | Where-Object { $_.platform.architecture -eq $Architecture -and $_.platform.os -eq $Os -and ($_.platform."os.version" -eq $null -or $_.platform."os.version" -like $OsVersion) }
if ($manifest) {
    Write-Output $manifest.digest
}
else {
    Write-Error "Unable to resolve multi-arch image to a digest. Be sure to specify the desired OS and architecture. For Windows containers, include the OS version as well."
}
