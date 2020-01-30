#!/usr/bin/env pwsh
[cmdletbinding(
    DefaultParameterSetName = 'BuildAndTest'
)]
param(
    [string]$VersionFilter = "*",
    [string]$OSFilter = "*",
    [string]$ArchitectureFilter = "amd64",
    [Parameter(ParameterSetName = "Build")]
    [Parameter(ParameterSetName = "BuildAndTest")]
    [string]$OptionalImageBuilderArgs,
    [Parameter(ParameterSetName = "Build")]
    [switch]$BuildOnly,
    [Parameter(ParameterSetName = "Test")]
    [switch]$TestOnly,
    [Parameter(ParameterSetName = "Test")]
    [Parameter(ParameterSetName = "BuildAndTest")]
    [ValidateSet("runtime", "runtime-deps", "aspnet", "sdk", "sample", "image-size")]
    [string[]]$TestCategories = @("runtime", "runtime-deps", "aspnet", "sdk", "sample", "image-size")
)

if ($PSCmdlet.ParameterSetName -eq 'BuildAndTest') {
    $build = $true
    $test = $true
}
else {
    $build = $BuildOnly
    $test = $TestOnly
}

if ($build) {
    & ./eng/common/build.ps1 `
        -VersionFilter $VersionFilter `
        -OSFilter $OSFilter `
        -ArchitectureFilter $ArchitectureFilter `
        -OptionalImageBuilderArgs $OptionalImageBuilderArgs
}
if ($test) {
    & ./tests/run-tests.ps1 `
        -VersionFilter $VersionFilter `
        -OSFilter $OSFilter `
        -ArchitectureFilter $ArchitectureFilter `
        -TestCategories $TestCategories
}
