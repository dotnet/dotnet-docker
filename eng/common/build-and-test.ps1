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

pushd $PSScriptRoot/../..
try {
    ./eng/common/Invoke-ImageBuilder.ps1 "build --path '$VersionFilter/*/$OSFilter/$ArchitectureFilter' $ImageBuilderCustomArgs"

    if (-not $SkipTesting) {
        if (Test-Path ./tests/run-tests.ps1) {
            ./tests/run-tests.ps1 `
                -VersionFilter $VersionFilter `
                -ArchitectureFilter $ArchitectureFilter `
                -OSFilter $OSFilter -IsLocalRun
        } else {
          Write-Warning "Test script file './tests/run-tests.ps1' not found."
        }
    }
}
finally {
    popd
}
