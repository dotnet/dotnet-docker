#!/usr/bin/env pwsh
[cmdletbinding()]
param(
    [string]$VersionFilter = "*",
    [string]$OSFilter = "*",
    [string]$ArchitectureFilter = "amd64",
    [string]$OptionalImageBuilderArgs,
    [string]$OptionalTestArgs,
    [switch]$SkipTesting = $false,
    [switch]$ExcludeArchitecture = $false
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Log {
    param ([string] $Message)

    Write-Output $Message
}

function Exec {
    param ([string] $Cmd)

    Log "Executing: '$Cmd'"
    Invoke-Expression $Cmd
    if ($LASTEXITCODE -ne 0) {
        throw "Failed: '$Cmd'"
    }
}

pushd $PSScriptRoot/../..
try {
    $pathFilter = "$VersionFilter/*/$OSFilter"
    if (-not $ExcludeArchitecture) {
        $pathFilter += "/$ArchitectureFilter"
    }

    ./eng/common/Invoke-ImageBuilder.ps1 "build --path '$pathFilter' $OptionalImageBuilderArgs"

    if (-not $SkipTesting) {
        if (Test-Path ./tests/run-tests.ps1) {
            if (-not $ExcludeArchitecture) {
                $OptionalTestArgs += " -ArchitectureFilter $ArchitectureFilter"
            }

            Exec "./tests/run-tests.ps1 -VersionFilter $VersionFilter -OSFilter $OSFilter $OptionalTestArgs"
        } else {
          Write-Warning "Test script file './tests/run-tests.ps1' not found."
        }
    }
}
finally {
    popd
}
