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
    [switch]$Build,
    [Parameter(ParameterSetName = "Test")]
    [switch]$Test,
    [Parameter(ParameterSetName = "Test")]
    [Parameter(ParameterSetName = "BuildAndTest")]
    [ValidateSet("runtime", "runtime-deps", "aspnet", "sdk", "image-size")]
    [string[]]$TestCategories = @("runtime", "runtime-deps", "aspnet", "sdk", "image-size")
)

if ($PSCmdlet.ParameterSetName -eq 'BuildAndTest') {
    $Build = $true
    $Test = $true
}

if ($Build) {
    & ./eng/common/build-and-test.ps1 `
        -VersionFilter $VersionFilter `
        -OSFilter $OSFilter `
        -ArchitectureFilter $ArchitectureFilter `
        -OptionalImageBuilderArgs $OptionalImageBuilderArgs `
        -SkipTesting:$Test
}
if ($Test) {
    & ./tests/run-tests.ps1 `
        -VersionFilter $VersionFilter `
        -OSFilter $OSFilter `
        -ArchitectureFilter $ArchitectureFilter `
        -TestCategories $TestCategories
}
