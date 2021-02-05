#!/usr/bin/env pwsh
param(
    # Version of .NET Core to filter by
    [string]$Version = "*",

    # Name of OS to filter by
    [string]$OS,

    # Type of architecture to filter by
    [string]$Architecture,

    # Additional custom path filters (overrides Version)
    [string[]]$Paths,

    # Additional args to pass to ImageBuilder
    [string]$OptionalImageBuilderArgs,

    # Execution mode of the script
    [ValidateSet("BuildAndTest", "Build", "Test")]
    [string]$Mode = "BuildAndTest",

    # Categories of tests to run
    [ValidateSet("runtime", "runtime-deps", "aspnet", "sdk", "pre-build", "sample", "image-size", "monitor")]
    [string[]]$TestCategories = @("runtime", "runtime-deps", "aspnet", "sdk", "pre-build", "sample", "image-size", "monitor")
)

if (($Mode -eq "BuildAndTest" -or $Mode -eq "Test") -and $TestCategories.Contains("pre-build")) {
    & ./tests/run-tests.ps1 -TestCategories "pre-build"
}

if ($Mode -eq "BuildAndTest" -or $Mode -eq "Build") {
    # Build the product images
    & ./eng/common/build.ps1 `
        -Version $Version `
        -OS $OS `
        -Architecture $Architecture `
        -Paths $Paths `
        -OptionalImageBuilderArgs $OptionalImageBuilderArgs

    $activeOS = docker version -f "{{ .Server.Os }}"
    if ($activeOS -eq "windows" -and -not $OS) {
        Write-Host "Setting OS to match local Windows host version"
        $windowsReleaseId = (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion").ReleaseId
        $OS = "nanoserver-$windowsReleaseId"
    }

    # Build the sample images
    & ./eng/common/build.ps1 `
        -Version $Version `
        -OS $OS `
        -Architecture $Architecture `
        -Paths $Paths `
        -OptionalImageBuilderArgs $OptionalImageBuilderArgs `
        -Manifest manifest.samples.json
}
if ($Mode -eq "BuildAndTest" -or $Mode -eq "Test") {

    $localTestCategories = $TestCategories

    if ($Version -ne "*" -and $TestCategories.Contains("sample")) {
        $localTestCategories = $TestCategories | where { $_ -ne "sample" }
        Write-Warning "Skipping sample image testing since Version was set"
    }

    & ./tests/run-tests.ps1 `
        -Version $Version `
        -OS $OS `
        -Architecture $Architecture `
        -TestCategories $localTestCategories
}
