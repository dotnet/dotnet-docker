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
    [string]$OSFilter = "*",

    # Type of architecture to filter by
    [string]$ArchitectureFilter = "amd64",

    # Custom path filters that override the other filter options
    [string]$PathFilters,

    # Additional args to pass to ImageBuilder
    [string]$OptionalImageBuilderArgs,

    # Whether to exclude architecture from the generated path filter
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
    if (-not $PathFilters) {
        $PathFilters = "$VersionFilter/*/$OSFilter"
        if (-not $ExcludeArchitecture) {
            $PathFilters += "/$ArchitectureFilter"
        }

        $PathFilters = "--path '$PathFilters'"
    }

    ./eng/common/Invoke-ImageBuilder.ps1 "build $PathFilters $OptionalImageBuilderArgs"
}
finally {
    popd
}
