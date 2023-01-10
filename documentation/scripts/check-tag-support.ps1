#!/usr/bin/env pwsh

<#
.SYNOPSIS
Checks whether a .NET container image tag is currently supported.
#>
[cmdletbinding()]
param(
    # The full .NET container image tag in the form of "mcr.microsoft.com/dotnet/<image-type>:<tag>"
    [Parameter(Mandatory = $true)]
    [string]
    $ImageTag
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

function hasProperty($object, $propertyName) {
    return $null -ne $object.psobject.Properties[$propertyName]
}

# Parse the fully-qualified image tag into its components
$components = $ImageTag -split '/'

if ($components[0] -ne "mcr.microsoft.com") {
    throw "The image tag must be from the 'mcr.microsoft.com' registry"
}

if ($components[1] -ne "dotnet") {
    throw "The image tag must be from the 'dotnet' repository"
}

$imageComponents = $components[2] -split ':'

$imageType = $imageComponents[0]
$imageTypes = @("aspnet", "runtime", "runtime-deps", "sdk", "monitor")
if (-not $imageTypes.Contains($imageType)) {
    throw "The image tag must use one of the following image types: $($imageTypes -join ', ')"
}

# If no tag is specified, default to the latest tag. By default, the latest tag is supported.
if ($imageComponents.Count -eq 1) {
    return $true
}

$tag = $imageComponents[1]
$imageInfo = Invoke-RestMethod -Uri "https://raw.githubusercontent.com/dotnet/versions/main/build-info/docker/image-info.dotnet-dotnet-docker-main.json"

foreach ($repo in $imageInfo.repos) {
    if ($repo.repo -eq "dotnet/$imageType") {
        foreach ($image in $repo.images) {
            if ((hasProperty $image "manifest") -and $image.manifest.sharedTags -contains $tag) {
                return $true
            }
            foreach ($platform in $image.platforms) {
                if ((hasProperty $platform "simpleTags") -and $platform.simpleTags -contains $tag) {
                    return $true
                }
            }
        }
    }
}

return $false
