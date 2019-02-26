#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

[cmdletbinding()]
param(
    [string]$VersionFilter,
    [string]$ArchitectureFilter,
    [string]$OSFilter,
    [string]$Registry,
    [string]$RepoPrefix,
    [switch]$DisableHttpVerification,
    [switch]$IsLocalRun
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$DotnetInstallDir = "$PSScriptRoot/../.dotnet"

if (!(Test-Path "$DotnetInstallDir")) {
    mkdir "$DotnetInstallDir" | Out-Null
}

# Install the .NET Core SDK
$IsRunningOnUnix = $PSVersionTable.contains("Platform") -and $PSVersionTable.Platform -eq "Unix"
if ($IsRunningOnUnix) {
    $DotnetInstallScript = "dotnet-install.sh"
}
else {
    $DotnetInstallScript = "dotnet-install.ps1"
}

if (!(Test-Path $DotnetInstallScript)) {
    $DOTNET_INSTALL_SCRIPT_URL = "https://raw.githubusercontent.com/dotnet/cli/release/2.1/scripts/obtain/$DotnetInstallScript"
    Invoke-WebRequest $DOTNET_INSTALL_SCRIPT_URL -OutFile $DotnetInstallDir/$DotnetInstallScript
}

if ($IsRunningOnUnix) {
    & chmod +x $DotnetInstallDir/$DotnetInstallScript
    & $DotnetInstallDir/$DotnetInstallScript --channel "2.1" --version "latest" --architecture x64 --install-dir $DotnetInstallDir
}
else {
    & $DotnetInstallDir/$DotnetInstallScript -Channel "2.1" -Version "latest" -Architecture x64 -InstallDir $DotnetInstallDir
}

if ($LASTEXITCODE -ne 0) { throw "Failed to install the .NET Core SDK" }

Push-Location "$PSScriptRoot\Microsoft.DotNet.Docker.Tests"

Try {
    # Run Tests
    if ([string]::IsNullOrWhiteSpace($ArchitectureFilter)) {
        $ArchitectureFilter = "amd64"
    }
    if ($DisableHttpVerification) {
        $env:DISABLE_HTTP_VERIFICATION = 1
    }
    if ($IsLocalRun) {
        $env:LOCAL_RUN = 1
    }

    $env:IMAGE_ARCH_FILTER = $ArchitectureFilter
    $env:IMAGE_OS_FILTER = $OSFilter
    $env:IMAGE_VERSION_FILTER = $VersionFilter
    $env:REGISTRY = $Registry
    $env:REPO_PREFIX = $RepoPrefix

    $env:DOTNET_CLI_TELEMETRY_OPTOUT = 1
    $env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 1
    $env:DOTNET_MULTILEVEL_LOOKUP = '0'

    & $DotnetInstallDir/dotnet test --logger:trx

    if ($LASTEXITCODE -ne 0) { throw "Tests Failed" }
}
Finally {
    Pop-Location
}
