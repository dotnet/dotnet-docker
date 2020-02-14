#!/usr/bin/env pwsh

<#
    .SYNOPSIS
        Builds the .NET Core Dockerfiles
#>

[cmdletbinding()]
param(
    # Version of .NET Core to filter by
    [string]$VersionFilter = "*",

    # Name of OS to filter by
    [string]$OSFilter,

    # Type of architecture to filter by
    [string]$ArchitectureFilter,

    # Additional custom path filters (overrides VersionFilter)
    [string]$PathFilters,

    # Path to manifest file
    [string]$Manifest = "manifest.json",

    # Additional args to pass to ImageBuilder
    [string]$OptionalImageBuilderArgs
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
    $args = $OptionalImageBuilderArgs

    if ($OSFilter) {
        $args += " --os-version $OSFilter"
    }

    if ($ArchitectureFilter) {
        $args += " --architecture $ArchitectureFilter"
    }

    if ($PathFilters) {
        $args += " $PathFilters"
    }
    else {
        $args += " --path '$VersionFilter/*'"
    }

    if ($Manifest) {
        $args += " --manifest $Manifest"
    }

    ./eng/common/Invoke-ImageBuilder.ps1 "build $args"
}
finally {
    popd
}
