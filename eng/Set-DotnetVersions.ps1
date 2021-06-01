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

    # Compute the checksum if a published checksum cannot be found
    [Switch]
    $ComputeShas,

    # When set, only prints out an Azure DevOps variable with the value of the args to pass to update-dependencies.
    [Switch]
    $PrintArgsVariableOnly
)

$updateDepsArgs = @($ProductVersion)

if ($SdkVersion) {
    $updateDepsArgs += @("--product-version", "sdk=$SdkVersion", "--product-version", "sdk-rpm=$SdkVersion")
}

if ($AspnetVersion) {
    $updateDepsArgs += @("--product-version", "aspnet=$AspnetVersion", "--product-version", "aspnet-rpm=$AspnetVersion", "--product-version", "aspnet-runtime-targeting-pack-rpm=$AspnetVersion")
}

if ($RuntimeVersion) {
    $updateDepsArgs += @("--product-version", "runtime=$RuntimeVersion", "--product-version", "runtime-rpm=$RuntimeVersion", "--product-version", "runtime-apphost-pack-rpm=$RuntimeVersion", "--product-version", "runtime-targeting-pack-rpm=$RuntimeVersion", "--product-version", "runtime-host-rpm=$RuntimeVersion", "--product-version", "runtime-hostfxr-rpm=$RuntimeVersion", "--product-version", "runtime-deps-cm.1-rpm=$RuntimeVersion", "--product-version", "netstandard-targeting-pack-2.1.0-rpm")
}

if ($ComputeShas) {
    $updateDepsArgs += "--compute-shas"
}

if ($PrintArgsVariableOnly) {
    Write-Host "##vso[task.setvariable variable=updateDepsArgs]$updateDepsArgs"
}
else {
    & dotnet run --project $PSScriptRoot/update-dependencies/update-dependencies.csproj @updateDepsArgs
}
