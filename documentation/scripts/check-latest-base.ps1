#!/usr/bin/env pwsh

<#
.SYNOPSIS
Checks whether a given image is based on the latest version of a .NET base image tag.
#>
[cmdletbinding()]
param(
    # The image to be checked on whether it is based on the latest version of a .NET base image tag.
    [Parameter(Mandatory = $true)]
    [string]
    $ImageTag,

    # The .NET base image tag to be checked against.
    [Parameter(Mandatory = $true)]
    [string]
    $DotNetBaseImageTag
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

Write-Host "Pulling $ImageTag..."
docker pull $ImageTag

$imageConfig = $(docker inspect $ImageTag | ConvertFrom-Json)

Write-Host
Write-Host "Getting manifest of $DotNetBaseImageTag..."
$baseTagManifest = $(docker manifest inspect $DotNetBaseImageTag | ConvertFrom-Json)
$mediaType = $baseTagManifest.mediaType

$basePullImage = ""

if ($mediaType.Contains("list")) {
    Write-Host
    Write-Host "Resolving multi-arch tag $DotNetBaseImageTag to matching platform..."

    $baseImageDigest = $baseTagManifest.manifests
        | Where-Object { $_.platform.Architecture -eq $imageConfig.Architecture -and $_.platform.Os -eq $imageConfig.Os }
        | Select-Object -ExpandProperty digest

    if (-not $baseImageDigest) {
        Write-Error "ERROR: Could not find a matching platform for the given image."
        return 1
    }

    # If the image name contains a tag separater, replace the tag with the digest; otherwise, append the digest
    $colonIndex = $DotNetBaseImageTag.IndexOf(":")
    if ($colonIndex -ge 0) {
        $basePullImage = $DotNetBaseImageTag.Substring(0, $colonIndex) + "@$baseImageDigest"
    }
    else {
        $basePullImage = "$DotNetBaseImageTag@$baseImageDigest"
    }
}
else {
    $basePullImage = $DotNetBaseImageTag
}

Write-Host
Write-Host "Pulling $basePullImage..."
docker pull $basePullImage
$baseImageConfig = $(docker inspect $basePullImage | ConvertFrom-Json)

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
