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

$DotnetInstallScriptPath = Join-Path -Path $InstallPath -ChildPath $DotnetInstallScript

if (!(Test-Path $DotnetInstallScriptPath)) {
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12;
    & "$PSScriptRoot/Invoke-WithRetry.ps1" "Invoke-WebRequest 'https://dot.net/v1/$DotnetInstallScript' -OutFile $DotnetInstallScriptPath"
}

$DotnetChannel = "7.0"

$InstallFailed = $false
if ($IsRunningOnUnix) {
    & chmod +x $DotnetInstallScriptPath
    & $DotnetInstallScriptPath --channel $DotnetChannel --install-dir $InstallPath
    $InstallFailed = ($LASTEXITCODE -ne 0)
}
else {
    & $DotnetInstallScriptPath -Channel $DotnetChannel -InstallDir $InstallPath
    $InstallFailed = (-not $?)
}

# See https://github.com/NuGet/NuGet.Client/pull/4259
$Env:NUGET_EXPERIMENTAL_CHAIN_BUILD_RETRY_POLICY = "3,1000"

if ($InstallFailed) { throw "Failed to install the .NET Core SDK" }
