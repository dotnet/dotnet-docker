#!/usr/bin/env pwsh
[cmdletbinding()]
param(
    [string]$VersionFilter = "*",
    [string]$OSFilter = "*",
    [string]$ArchitectureFilter = "amd64",
    [string]$ImageBuilderCustomArgs,
    [switch]$SkipTesting = $false
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

pushd $PSScriptRoot
try {
    ./scripts/Invoke-ImageBuilder.ps1 "build --path '$VersionFilter/*/$OSFilter/$ArchitectureFilter' $ImageBuilderCustomArgs"

    if (-not $SkipTesting) {
        ./tests/run-tests.ps1 `
            -VersionFilter $VersionFilter `
            -ArchitectureFilter $ArchitectureFilter `
            -OSFilter $OSFilter -IsLocalRun
    }
}
finally {
    popd
}
