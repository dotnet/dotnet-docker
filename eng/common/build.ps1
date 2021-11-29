#!/usr/bin/env pwsh

<#
    .SYNOPSIS
        Builds the Dockerfiles
#>

[cmdletbinding()]
param(
    # Product versions to filter by
    [string[]]$Version = "*",

    # Names of OS to filter by
    [string[]]$OS,

    # Type of architecture to filter by
    [string]$Architecture,

    # Additional custom path filters
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

    if ($Version) {
        $args += ($Version | foreach { ' --version "{0}"' -f $_ })
    }

    if ($OS) {
        $args += ($OS | foreach { ' --os-version "{0}"' -f $_ })
    }

    if ($Architecture) {
        $args += ' --architecture "{0}"' -f $Architecture
    }

    if ($Paths) {
        $args += ($Paths | foreach { ' --path "{0}"' -f $_ })
    }

    if ($Manifest) {
        $args += ' --manifest "{0}"' -f $Manifest
    }

    ./eng/common/Invoke-ImageBuilder.ps1 "build $args"
}
finally {
    popd
}
