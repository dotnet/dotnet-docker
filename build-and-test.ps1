#!/usr/bin/env pwsh
param(
    # Version of .NET Core to filter by
    [string]$VersionFilter = "*",

    # Name of OS to filter by
    [string]$OSFilter,

    # Type of architecture to filter by
    [string]$ArchitectureFilter,

    # Additional custom path filters (overrides VersionFilter)
    [string]$PathFilters,

    # Additional args to pass to ImageBuilder
    [string]$OptionalImageBuilderArgs,

    # Execution mode of the script
    [ValidateSet("BuildAndTest", "Build", "Test")]
    [string]$Mode = "BuildAndTest",

    # Categories of tests to run
    [ValidateSet("runtime", "runtime-deps", "aspnet", "sdk", "sample", "image-size")]
    [string[]]$TestCategories = @("runtime", "runtime-deps", "aspnet", "sdk", "sample", "image-size")
)

if ($Mode -eq "BuildAndTest" -or $Mode -eq "Build") {
    # Build the product images
    & ./eng/common/build.ps1 `
        -VersionFilter $VersionFilter `
        -OSFilter $OSFilter `
        -ArchitectureFilter $ArchitectureFilter `
        -PathFilters $PathFilters `
        -OptionalImageBuilderArgs $OptionalImageBuilderArgs

    $activeOS = docker version -f "{{ .Server.Os }}"
    if ($activeOS -eq "windows" -and -not $OSFilter) {
        Write-Host "Setting OSFilter to match local Windows host version"
        $windowsReleaseId = (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion").ReleaseId
        $OSFilter = "nanoserver-$windowsReleaseId"
    }

    # Build the sample images
    & ./eng/common/build.ps1 `
        -VersionFilter $VersionFilter `
        -OSFilter $OSFilter `
        -ArchitectureFilter $ArchitectureFilter `
        -PathFilters $PathFilters `
        -OptionalImageBuilderArgs $OptionalImageBuilderArgs `
        -Manifest manifest.samples.json
}
if ($Mode -eq "BuildAndTest" -or $Mode -eq "Test") {

    $localTestCategories = $TestCategories

    if ($VersionFilter -ne "*" -and $TestCategories.Contains("sample")) {
        $localTestCategories = $TestCategories | where { $_ -ne "sample"}
        Write-Warning "Skipping sample image testing since VersionFilter was set"
    }

    & ./tests/run-tests.ps1 `
        -VersionFilter $VersionFilter `
        -OSFilter $OSFilter `
        -ArchitectureFilter $ArchitectureFilter `
        -TestCategories $localTestCategories
}
