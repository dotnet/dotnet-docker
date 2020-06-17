#!/usr/bin/env pwsh

<#
    .SYNOPSIS
        Builds the .NET Core Dockerfiles
#>

[cmdletbinding()]
param(
    # Version of .NET Core to filter by
    [string]$Version = "*",

    # Name of OS to filter by
    [string]$OS,

    # Type of architecture to filter by
    [string]$Architecture,

    # Additional custom path filters (overrides Version)
    [string[]]$Paths,

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

    if ($OS) {
        $args += " --os-version $OS"
    }

    if ($Architecture) {
        $args += " --architecture $Architecture"
    }

    if ($Paths) {
        $args += " --path " + ($Paths -join " --path ")
    }
    else {
        $args += " --path 'src/*/$Version/*'"
    }

    if ($Manifest) {
        $args += " --manifest $Manifest"
    }

    ./eng/common/Invoke-ImageBuilder.ps1 "build $args"
}
finally {
    popd
}
