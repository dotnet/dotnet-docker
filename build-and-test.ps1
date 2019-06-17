#!/usr/bin/env pwsh
[cmdletbinding()]
param(
    [string]$VersionFilter = "*",
    [string]$OSFilter = "*",
    [string]$ArchitectureFilter = "amd64",
    [string]$ImageBuilderCustomArgs,
    [switch]$SkipTesting = $false
)

& ./eng/common/build-and-test.ps1 `
    -VersionFilter $VersionFilter `
    -OSFilter $OSFilter `
    -ArchitectureFilter $ArchitectureFilter `
    -ImageBuilderCustomArgs $ImageBuilderCustomArgs `
    -SkipTesting:$SkipTesting
