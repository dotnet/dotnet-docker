#!/usr/bin/env pwsh
param(
    [string]$Branch
)

$ErrorActionPreference = 'Stop'
$repoRoot = (Get-Item "$PSScriptRoot").Parent.FullName

if (!$Branch) {
    $manifestJson = Get-Content ${repoRoot}/manifest.json | ConvertFrom-Json
    if ($manifestJson.Repos[0].Name.Contains("nightly")) {
        $Branch = "nightly"
        $coreRepoName = "core-nightly/"
        $repoName = "nightly/"
    }
    else {
        $Branch = "master"
        $coreRepoName = "core/"
        $repoName = ""
    }
}

$gitRepo = "https://github.com/dotnet/dotnet-docker"

& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/$($coreRepoName)runtime-deps README.runtime-deps.md manifest.json $gitRepo $Branch
& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/$($repoName)runtime-deps README.runtime-deps.preview.md manifest.json $gitRepo $Branch -ReuseImageBuilderImage
& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/$($coreRepoName)runtime README.runtime.md manifest.json $gitRepo $Branch -ReuseImageBuilderImage
& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/$($repoName)runtime README.runtime.preview.md manifest.json $gitRepo $Branch -ReuseImageBuilderImage
& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/$($coreRepoName)aspnet README.aspnet.md manifest.json $gitRepo $Branch -ReuseImageBuilderImage
& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/$($repoName)aspnet README.aspnet.preview.md manifest.json $gitRepo $Branch -ReuseImageBuilderImage
& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/$($coreRepoName)sdk README.sdk.md manifest.json $gitRepo $Branch -ReuseImageBuilderImage
& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/$($repoName)sdk README.sdk.preview.md manifest.json $gitRepo $Branch -ReuseImageBuilderImage
& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/core/samples README.samples.md manifest.samples.json $gitRepo -ReuseImageBuilderImage
& $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
    dotnet/$($repoName)monitor README.monitor.md manifest.json $gitRepo $Branch -ReuseImageBuilderImage
