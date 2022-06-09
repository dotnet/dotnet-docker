#!/usr/bin/env pwsh

<#
.SYNOPSIS
Updates dependencies for the specified .NET version.
#>
[cmdletbinding()]
param(
    # The major/minor version of the product (e.g. 6.0).
    [Parameter(Mandatory = $true, Position = 0)]
    [string]
    $ProductVersion,

    # Build version of the SDK
    [Parameter(Mandatory = $false, ParameterSetName = 'DotnetInstaller')]
    [string]
    $SdkVersion,

    # Build version of ASP.NET Core
    [Parameter(Mandatory = $false, ParameterSetName = 'DotnetInstaller')]
    [string]
    $AspnetVersion,

    # Build version of the .NET runtime
    [Parameter(Mandatory = $false, ParameterSetName = 'DotnetInstaller')]
    [string]
    $RuntimeVersion,

    # Build verison of the .NET Monitor tool
    [Parameter(Mandatory = $false, ParameterSetName = 'DotnetMonitor')]
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

if ($SdkVersion) {
    $updateDepsArgs += @("--product-version", "sdk=$SdkVersion")
}

if ($AspnetVersion) {
    $updateDepsArgs += @("--product-version", "aspnet=$AspnetVersion", "--product-version", "aspnet-runtime-targeting-pack=$AspnetVersion")
}

if ($RuntimeVersion) {
    $updateDepsArgs += @("--product-version", "runtime=$RuntimeVersion", "--product-version", "runtime-apphost-pack=$RuntimeVersion", "--product-version", "runtime-targeting-pack=$RuntimeVersion", "--product-version", "runtime-host=$RuntimeVersion", "--product-version", "runtime-hostfxr=$RuntimeVersion", "--product-version", "netstandard-targeting-pack-2.1.0", "--product-version", "runtime-deps-cm.1=$RuntimeVersion", "--product-version", "runtime-deps-cm.2=$RuntimeVersion")
}

if ($MonitorVersion) {
    $updateDepsArgs += @("--product-version", "monitor=$MonitorVersion")
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

$versionSourceName = switch ($PSCmdlet.ParameterSetName) {
    "DotnetInstaller" { "dotnet/installer" }
    "DotnetMonitor" { "dotnet/dotnet-monitor" }
    default { Write-Error -Message "Unknown version source" -ErrorAction Stop }
}

if ($versionSourceName) {
    $updateDepsArgs += "--version-source-name=$versionSourceName"
}

$branch = & $PSScriptRoot/Get-Branch.ps1
$updateDepsArgs += "--source-branch=$branch"

if ($PrintArgsVariableOnly) {
    Write-Host "##vso[task.setvariable variable=updateDepsArgs]$updateDepsArgs"
}
else {
    & dotnet run --project $PSScriptRoot/update-dependencies/update-dependencies.csproj @updateDepsArgs
}
