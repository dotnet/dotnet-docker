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
    [ValidateSet("runtime", "runtime-deps", "aspnet", "sdk", "pre-build", "sample", "monitor", "aspire-dashboard")]
    [string[]]$TestCategories = @("runtime", "runtime-deps", "aspnet", "sdk", "monitor", "aspire-dashboard")
)

[System.Collections.ArrayList]$TestCategories = $TestCategories

if ($Version -notmatch '^\d+\.\d+(\.[\d*])?|\*$') {
    Write-Error "Error: Input version '$Version' is not in the expected format of X.Y or X.Y.*"
    exit 1

    # If we call the script with a format like "8.0", add the trailing ".*" that ImageBuilder expects
    if ($Version.Split('.').Count -lt 3) {
        $Version += ".*"
    }
}

if (($Mode -eq "BuildAndTest" -or $Mode -eq "Test")) {

    Write-Host "`nTests will run with TestCategories: $TestCategories"

    if ($TestCategories.Contains("pre-build"))
    {
        & ./tests/run-tests.ps1 -TestCategories "pre-build" -Version "*"
        $TestCategories.Remove("pre-build")
    }
    else
    {
        Write-Host "Skipping pre-build validation tests."
        Write-Host "To run pre-build tests, use the 'pre-build' test category.`n"
    }
}

if ($Mode -eq "BuildAndTest" -or $Mode -eq "Build") {
    # Build the product images
    & ./eng/docker-tools/build.ps1 `
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

    $buildSamples = $Paths -match "samples"
    if ($buildSamples)
    {
        # Build the sample images
        & ./eng/docker-tools/build.ps1 `
            -Version $Version `
            -OS @($OS) `
            -Architecture $Architecture `
            -Paths $Paths `
            -OptionalImageBuilderArgs $OptionalImageBuilderArgs `
            -Manifest manifest.samples.json
    }
    else
    {
        Write-Host "Skipping samples builds since no input paths contained samples"
    }
}

if ($Mode -eq "BuildAndTest" -or $Mode -eq "Test") {

    $VersionParts = $Version.Split('.')
    $TestVersion = $VersionParts[0] + "." + $VersionParts[1]

    $localTestCategories = $TestCategories

    if ($Version -ne "*" -and $TestCategories.Contains("sample")) {
        $localTestCategories = $TestCategories | where { $_ -ne "sample" }
        Write-Warning "Skipping sample image testing since Version was set"
    }


    if ($Paths -ne $null -and $Paths.Count -gt 0) {
        & ./tests/run-tests.ps1 `
            -Architecture $Architecture `
            -Paths $Paths `
            -TestCategories $localTestCategories
    } else {
        & ./tests/run-tests.ps1 `
            -Version $TestVersion `
            -OSVersions @($OS) `
            -Architecture $Architecture `
            -TestCategories @localTestCategories
    }
}
