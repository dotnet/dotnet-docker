#!/usr/bin/env pwsh

<#
.SYNOPSIS
Example script that updates the bundled docker-tools infrastructure in the current repo to a specific
ImageBuilder image.

.DESCRIPTION
ImageBuilder ships a copy of the eng/docker-tools infrastructure and writes it back to disk via its
'update' command. The reference that 'update' records into
eng/docker-tools/templates/variables/docker-images.yml is supplied as an argument rather than being
baked into the build, so the caller decides exactly which image the repo should pin to.

This example resolves the multi-platform (manifest list / image index) digest of an ImageBuilder image
(the published 'latest' tag by default) and passes that digest reference to the 'update' command, which
runs inside the same image with the repository mounted so it can rewrite eng/docker-tools on disk.

.PARAMETER ImageBuilderImage
The ImageBuilder image to resolve and run. Defaults to the published 'latest' tag.

.PARAMETER RepoRoot
The root of the git repository to update. Defaults to the current directory.

.NOTES
To exercise an unpublished ImageBuilder (for example, the 'update' command before it is released),
build the image, push it to a registry it can be pulled from, and pass its reference via
-ImageBuilderImage. The digest is read from the registry, so the image must be pushed first.
#>
[CmdletBinding()]
param(
    [string]
    $ImageBuilderImage = "mcr.microsoft.com/dotnet-buildtools/image-builder:latest",

    [string]
    $RepoRoot = (Get-Location).Path
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Exec {
    param ([string] $Cmd)

    Write-Output "Executing: '$Cmd'"
    Invoke-Expression $Cmd
    if ($LASTEXITCODE -ne 0) {
        throw "Failed: '$Cmd'"
    }
}

# Strip any existing tag or digest so the resolved digest can be appended to the bare repository name.
# A tag is a ':' within the final path segment; a registry host's ':port' precedes the last '/', so it
# must not be mistaken for a tag.
function Get-RepositoryName {
    param ([string] $Reference)

    $withoutDigest = $Reference.Split('@')[0]
    $lastSlash = $withoutDigest.LastIndexOf('/')
    $lastColon = $withoutDigest.LastIndexOf(':')
    if ($lastColon -gt $lastSlash) {
        return $withoutDigest.Substring(0, $lastColon)
    }

    return $withoutDigest
}

# Resolve the multi-platform digest. 'docker buildx imagetools inspect' reads the top-level manifest
# straight from the registry, so for a multi-arch image this is the manifest list (image index) digest
# rather than a single platform's digest. Pinning the index keeps the reference valid on every
# platform the pipeline runs on.
$digest = (docker buildx imagetools inspect $ImageBuilderImage --format '{{.Manifest.Digest}}')
if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($digest)) {
    throw "Unable to resolve a multi-platform digest for '$ImageBuilderImage'."
}

$repository = Get-RepositoryName $ImageBuilderImage
$imageBuilderRef = "$repository@$($digest.Trim())"

Write-Output "Resolved ImageBuilder reference: $imageBuilderRef"

# Run 'update' from the resolved digest, mounting the repository so it can write eng/docker-tools to
# disk. The command must run from the repository root, which is why $RepoRoot is the mounted working
# directory. Running by the same digest that gets recorded keeps the writer and the pinned reference
# identical.
Exec ("docker run --rm " `
    + "-v `"${RepoRoot}:/repo`" " `
    + "-w /repo " `
    + "$imageBuilderRef " `
    + "update $imageBuilderRef")
