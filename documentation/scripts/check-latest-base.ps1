#!/usr/bin/env pwsh

<#
.SYNOPSIS
Checks whether a given image is based on the latest version of a .NET image tag.
#>
[cmdletbinding()]
param(
    # The image to be checked on whether it is based on the latest version of a .NET image tag.
    [Parameter(Mandatory = $true)]
    [string]
    $ImageDigest,

    # The .NET image tag to be checked against.
    [Parameter(Mandatory = $true)]
    [string]
    $DotNetImageTag
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

if (-not $ImageDigest.Contains("@sha256:")) {
    Write-Error "The value '$ImageDigest' must be a fully-qualified image digest, not an image tag. This ensures that the image is not subject to change from when it was scanned."
    return
}

Write-Host "Pulling $ImageDigest..."
docker pull $ImageDigest
if (-not $?) {
    Write-Error "Failed to pull $ImageDigest"
    return
}

$imageConfig = $(docker inspect $ImageDigest | ConvertFrom-Json)
if (-not $?) {
    Write-Error "Failed to get configuration of $ImageDigest"
    return
}

Write-Host
Write-Host "Getting manifest of $DotNetImageTag..."
$baseTagManifest = $(docker manifest inspect $DotNetImageTag | ConvertFrom-Json)
if (-not $?) {
    Write-Error "Failed to get manifest of $DotNetImageTag"
    return
}

$mediaType = $baseTagManifest.mediaType

$basePullImage = ""

if ($mediaType.Contains("list")) {
    Write-Host
    Write-Host "Resolving multi-arch tag $DotNetImageTag to matching platform..."

    $baseImageDigest = $baseTagManifest.manifests |
        Where-Object { $_.platform.Architecture -eq $imageConfig.Architecture -and $_.platform.Os -eq $imageConfig.Os } |
        Select-Object -ExpandProperty digest

    if (-not $baseImageDigest) {
        Write-Error "ERROR: Could not find a matching platform for the given image."
        return 1
    }

    # If the image name contains a tag separater, replace the tag with the digest; otherwise, append the digest
    $colonIndex = $DotNetImageTag.IndexOf(":")
    if ($colonIndex -ge 0) {
        $basePullImage = $DotNetImageTag.Substring(0, $colonIndex) + "@$baseImageDigest"
    }
    else {
        $basePullImage = "$DotNetImageTag@$baseImageDigest"
    }
}
else {
    $basePullImage = $DotNetImageTag
}

Write-Host
Write-Host "Pulling $basePullImage..."
docker pull $basePullImage
if (-not $?) {
    Write-Error "Failed to pull $basePullImage"
    return
}

$baseImageConfig = $(docker inspect $basePullImage | ConvertFrom-Json)
if (-not $?) {
    Write-Error "Failed to get configuration of $basePullImage"
    return
}

# Validate that the base image is the same platform as the specified image to check
if ($imageConfig.Os -ne $baseImageConfig.Os -or $imageConfig.Architecture -ne $baseImageConfig.Architecture) {
    Write-Host
    Write-Error "ERROR: The given image is not built on the same platform as the .NET base image."
    return 1
}

Write-Host
Write-Host "Comparing image layers..."
$baseLayerCount = $baseImageConfig.RootFS.Layers.Length
$layerDiffs = Compare-Object $baseImageConfig.RootFS.Layers $imageConfig.RootFS.Layers[0..$($baseLayerCount - 1)]
if (-not $layerDiffs) {
    $result = $true
}
else {
    $result = $false
}

Write-Host
Write-Host "Result:"
Write-Output $result
