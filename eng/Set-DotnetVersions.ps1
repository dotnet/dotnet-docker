#!/usr/bin/env pwsh

<#
.SYNOPSIS
Updates dependencies for the specified .NET version.
#>
[cmdletbinding()]
param(
    # The major/minor version of the product (e.g. 6.0).
    [Parameter(Mandatory = $true)]
    [string]
    $ProductVersion,

    # Build version of the SDK
    [string]
    $SdkVersion,

    # Build version of ASP.NET Core
    [string]
    $AspnetVersion,

    # Build version of the .NET runtime
    [string]
    $RuntimeVersion,

    # Build verison of the .NET Monitor tool
    [string]
    $MonitorVersion,

    # Compute the checksum if a published checksum cannot be found
    [Switch]
    $ComputeShas,

    # Use stable branding version numbers to compute paths
    [Switch]
    $UseStableBranding,

    # When set, only prints out an Azure DevOps variable with the value of the args to pass to update-dependencies.
    [Switch]
    $PrintArgsVariableOnly,

    # SAS query string used to access files in the binary blob container
    [string]
    $BinarySasQueryString,

    # SAS query string used to access files in the checksum blob container
    [string]
    $ChecksumSasQueryString
)

$updateDepsArgs = @($ProductVersion)

$versionSourceName = "";
if ($SdkVersion) {
    $updateDepsArgs += @("--product-version", "sdk=$SdkVersion")
    $versionSourceName = "dotnet/installer"
}

if ($AspnetVersion) {
    $updateDepsArgs += @("--product-version", "aspnet=$AspnetVersion", "--product-version", "aspnet-runtime-targeting-pack=$AspnetVersion")
    $versionSourceName = "dotnet/installer"
}

if ($RuntimeVersion) {
    $updateDepsArgs += @("--product-version", "runtime=$RuntimeVersion", "--product-version", "runtime-apphost-pack=$RuntimeVersion", "--product-version", "runtime-targeting-pack=$RuntimeVersion", "--product-version", "runtime-host=$RuntimeVersion", "--product-version", "runtime-hostfxr=$RuntimeVersion", "--product-version", "runtime-deps-cm.1=$RuntimeVersion", "--product-version", "netstandard-targeting-pack-2.1.0")
    $versionSourceName = "dotnet/installer"
}

if ($MonitorVersion) {
    $updateDepsArgs += @("--product-version", "monitor=$MonitorVersion")
    $versionSourceName = "dotnet/dotnet-monitor"
}

if ($ComputeShas) {
    $updateDepsArgs += "--compute-shas"
}

if ($BinarySasQueryString) {
    $updateDepsArgs += "--binary-sas=$BinarySasQueryString"
}

if ($ChecksumSasQueryString) {
    $updateDepsArgs += "--checksum-sas=$ChecksumSasQueryString"
}

if ($UseStableBranding) {
    $updateDepsArgs += "--stable-branding"
}

if ($versionSourceName) {
    $updateDepsArgs += "--version-source-name=$versionSourceName"
}

$branch = & $PSScriptRoot/Get-Branch.ps1
$updateDepsArgs += "--branch=$branch"

if ($PrintArgsVariableOnly) {
    Write-Host "##vso[task.setvariable variable=updateDepsArgs]$updateDepsArgs"
}
else {
    & dotnet run --project $PSScriptRoot/update-dependencies/update-dependencies.csproj @updateDepsArgs
}
