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
    [Parameter(Mandatory = $false, ParameterSetName = 'DotnetSdk')]
    [string]
    $SdkVersion,

    # Build version of ASP.NET Core
    [Parameter(Mandatory = $false, ParameterSetName = 'DotnetSdk')]
    [string]
    $AspnetVersion,

    # Build version of the .NET runtime
    [Parameter(Mandatory = $false, ParameterSetName = 'DotnetSdk')]
    [string]
    $RuntimeVersion,

    # Build verison of the .NET Monitor tool
    [Parameter(Mandatory = $false, ParameterSetName = 'DotnetMonitor')]
    [string]
    $MonitorVersion,

    # Build verison of the .NET Aspire Dashboard
    [Parameter(Mandatory = $false, ParameterSetName = 'DotnetAspireDashboard')]
    [string]
    $AspireVersion,

    # Compute the checksum if a published checksum cannot be found
    [Switch]
    $ComputeShas,

    # Use stable branding version numbers to compute paths
    [Switch]
    $UseStableBranding,

    # When set, only prints out an Azure DevOps variable with the value of the args to pass to update-dependencies.
    [string]
    $AzdoVariableName,

    # File containing checksums for each product asset; used to override the behavior of locating the checksums from blob storage accounts.
    [string]
    $ChecksumsFile,

    # The release state of the product assets
    [ValidateSet("Prerelease", "Release")]
    [string]
    $ReleaseState,

    # PAT used to access internal AzDO build artifacts
    [string]
    $InternalAccessToken,

    # Base Url for internal AzDO build artifacts
    [string]
    $InternalBaseUrl
)

Import-Module -force $PSScriptRoot/DependencyManagement.psm1

$updateDepsArgs = @($ProductVersion)

if ($SdkVersion) {
    $updateDepsArgs += @("--product-version", "sdk=$SdkVersion")
}

if ($AspnetVersion) {
    $updateDepsArgs += @("--product-version", "aspnet=$AspnetVersion", "--product-version", "aspnet-composite=$AspnetVersion")

    if (!$InternalBaseUrl) {
        # rpm packages are only needed for 6.0 which isn't supported for internal testing scenarios
        $updateDepsArgs += @("--product-version", "aspnet-runtime-targeting-pack=$AspnetVersion")
    }
}

if ($RuntimeVersion) {
    $updateDepsArgs += @("--product-version", "runtime=$RuntimeVersion")

    if (!$InternalBaseUrl) {
        # rpm packages are only needed for 6.0 which isn't supported for internal testing scenarios
        $updateDepsArgs += @("--product-version", "runtime-apphost-pack=$RuntimeVersion", "--product-version", "runtime-targeting-pack=$RuntimeVersion", "--product-version", "runtime-host=$RuntimeVersion", "--product-version", "runtime-hostfxr=$RuntimeVersion", "--product-version", "netstandard-targeting-pack-2.1.0", "--product-version", "runtime-deps-cm.1=$RuntimeVersion", "--product-version", "runtime-deps-cm.2=$RuntimeVersion")
    }
}

if ($MonitorVersion) {
    $updateDepsArgs += @("--product-version", "monitor=$MonitorVersion")

    $productMajorVersion = $ProductVersion.Split('.', 2)[0]
    if ($productMajorVersion -ge 8) {
        $updateDepsArgs += @("--product-version", "monitor-base=$MonitorVersion")
        $updateDepsArgs += @("--product-version", "monitor-ext-azureblobstorage=$MonitorVersion")
        $updateDepsArgs += @("--product-version", "monitor-ext-s3storage=$MonitorVersion")
    }
}

if ($AspireVersion) {
    $updateDepsArgs += @("--product-version", "aspire-dashboard=$AspireVersion")
    $productMajorVersion = $ProductVersion.Split('.', 2)[0]
}

if ($ComputeShas) {
    $updateDepsArgs += "--compute-shas"
}

if ($ChecksumsFile) {
    $updateDepsArgs += "--checksums-file=$ChecksumsFile"
}

if ($UseStableBranding) {
    $updateDepsArgs += "--stable-branding"
}

if ($ReleaseState) {
    $updateDepsArgs += "--release-state=$ReleaseState"
}

if ($InternalAccessToken) {
    $updateDepsArgs += "--internal-access-token=$InternalAccessToken"
}

if ($InternalBaseUrl) {
    $updateDepsArgs += "--internal-base-url=$InternalBaseUrl"
}

$versionSourceName = switch ($PSCmdlet.ParameterSetName) {
    "DotnetSdk" { "dotnet/sdk" }
    "DotnetMonitor" { "dotnet/dotnet-monitor/$ProductVersion" }
    "DotnetAspireDashboard" { "dotnet/aspire-dashboard/$ProductVersion" }
    default { Write-Error -Message "Unknown version source" -ErrorAction Stop }
}

if ($versionSourceName) {
    $updateDepsArgs += "--version-source-name=$versionSourceName"
}

$updateDepsArgs += "--source-branch=$(Get-Branch)"

if ($AzdoVariableName) {
    Write-Host "##vso[task.setvariable variable=$AzdoVariableName]$updateDepsArgs"
}
else {
    & dotnet run --project $PSScriptRoot/update-dependencies/update-dependencies.csproj @updateDepsArgs
}
