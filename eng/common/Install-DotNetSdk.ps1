#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

<#
.SYNOPSIS
Install the .NET Core SDK at the specified path.

.PARAMETER InstallPath
The path where the .NET Core SDK is to be installed.

#>
[cmdletbinding()]
param(
    [string]
    $InstallPath
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if (!(Test-Path "$InstallPath")) {
    mkdir "$InstallPath" | Out-Null
}

$IsRunningOnUnix = $PSVersionTable.contains("Platform") -and $PSVersionTable.Platform -eq "Unix"
if ($IsRunningOnUnix) {
    $DotnetInstallScript = "dotnet-install.sh"
}
else {
    $DotnetInstallScript = "dotnet-install.ps1"
}

if (!(Test-Path $DotnetInstallScript)) {
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12;
    Invoke-WebRequest "https://dot.net/v1/$DotnetInstallScript" -OutFile $InstallPath/$DotnetInstallScript
}

$DotnetChannel = "5.0"

$InstallFailed = $false
if ($IsRunningOnUnix) {
    & chmod +x $InstallPath/$DotnetInstallScript
    & $InstallPath/$DotnetInstallScript --channel $DotnetChannel --version "latest" --install-dir $InstallPath
    $InstallFailed = ($LASTEXITCODE -ne 0)
}
else {
    & $InstallPath/$DotnetInstallScript -Channel $DotnetChannel -Version "latest" -InstallDir $InstallPath
    $InstallFailed = (-not $?)
}

if ($InstallFailed) { throw "Failed to install the .NET Core SDK" }
