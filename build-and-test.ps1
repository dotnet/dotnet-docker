#!/usr/bin/env pwsh
[cmdletbinding()]
param(
    [string]$VersionFilter = "*",
    [string]$OSFilter = "*",
    [string]$ArchitectureFilter = "amd64",
    [string]$OptionalImageBuilderArgs,
    [switch]$SkipTesting = $false,
	[switch]$IsLocalRun = $true
)

& ./eng/common/build-and-test.ps1 `
    -VersionFilter $VersionFilter `
    -OSFilter $OSFilter `
    -ArchitectureFilter $ArchitectureFilter `
    -OptionalImageBuilderArgs $OptionalImageBuilderArgs `
    -SkipTesting:$SkipTesting `
	-IsLocalRun:$IsLocalRun
