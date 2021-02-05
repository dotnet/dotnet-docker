#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

[cmdletbinding()]
param(
    [string]$Version,
    [string]$Architecture,
    [string]$OS,
    [string]$Registry,
    [string]$RepoPrefix,
    [switch]$DisableHttpVerification,
    [switch]$PullImages,
    [string]$ImageInfoPath,
    [ValidateSet("runtime", "runtime-deps", "aspnet", "sdk", "pre-build", "sample", "image-size", "monitor")]
    [string[]]$TestCategories = @("runtime", "runtime-deps", "aspnet", "sdk", "monitor")
)

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

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$EngCommonDir = "$PSScriptRoot/../eng/common"

$DotnetInstallDir = "$PSScriptRoot/../.dotnet"
& $EngCommonDir/Install-DotNetSdk.ps1 -InstallPath $DotnetInstallDir

# Ensure that ImageBuilder image is pulled because some tests require it
& $EngCommonDir/Get-ImageBuilder.ps1

$activeOS = docker version -f "{{ .Server.Os }}"

Push-Location "$PSScriptRoot\Microsoft.DotNet.Docker.Tests"

Try {
    # Run Tests
    if ([string]::IsNullOrWhiteSpace($Architecture)) {
        $Architecture = "amd64"
    }

    if ($DisableHttpVerification) {
        $env:DISABLE_HTTP_VERIFICATION = 1
    }
    else {
        $env:DISABLE_HTTP_VERIFICATION = $null
    }

    if ($PullImages) {
        $env:PULL_IMAGES = 1
    }
    else {
        $env:PULL_IMAGES = $null
    }

    $env:IMAGE_ARCH = $Architecture
    $env:IMAGE_OS = $OS
    $env:IMAGE_VERSION = $Version
    $env:REGISTRY = $Registry
    $env:REPO_PREFIX = $RepoPrefix
    $env:IMAGE_INFO_PATH = $ImageInfoPath
    $env:SOURCE_REPO_ROOT = (Get-Item "$PSScriptRoot").Parent.FullName

    $env:DOTNET_CLI_TELEMETRY_OPTOUT = 1
    $env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 1
    $env:DOTNET_MULTILEVEL_LOOKUP = '0'

    $testFilter = ""
    if ($TestCategories) {
        # Construct an expression that filters the test to each of the
        # selected TestCategories (using an OR operator between each category).
        # See https://docs.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests
        $TestCategories | foreach {
            # Skip pre-build tests on Windows because of missing pre-reqs (https://github.com/dotnet/dotnet-docker/issues/2261)
            if ($_ -eq "pre-build" -and $activeOS -eq "windows") {
                Write-Warning "Skipping pre-build tests for Windows containers"
            } else {
                if ($testFilter) {
                    $testFilter += "|"
                }

                $testFilter += "Category=$_"
            }
        }

        if (-not $testFilter) {
            exit;
        }

        $testFilter = "--filter `"$testFilter`""
    }

    Exec "$DotnetInstallDir/dotnet test $testFilter --logger:trx"

    if ($TestCategories.Contains('image-size')) {
        & ../performance/Validate-ImageSize.ps1 -PullImages:$PullImages -ValidationMode Integrity
    }
}
Finally {
    Pop-Location
}
