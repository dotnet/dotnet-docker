#!/usr/bin/env pwsh
param(
    [string] $Branch,
    [switch] $Validate
)

$ErrorActionPreference = 'Stop'

if (!$Branch) {
    $repoRoot = (Get-Item "$PSScriptRoot").Parent.FullName
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

function GenerateReadme {
    param (
        [string] $Repo,
        [string] $Readme,
        [switch] $FirstCall
    )

    if ($Repo.Contains("samples")) {
        $branchArg = "master"
        $manifest = "manifest.samples.json"
    }
    else {
        $branchArg = $Branch
        $manifest = "manifest.json"
    }

    if (-not $FirstCall) {
        $optionalArgs = "-ReuseImageBuilderImage"
    }

    & $PSScriptRoot/common/Invoke-ReadmeGeneration.ps1 `
        $Repo $Readme $manifest "https://github.com/dotnet/dotnet-docker" $branchArg -Validate:$Validate $optionalArgs
}

GenerateReadme -Repo dotnet/$($coreRepoName)runtime-deps -Readme README.runtime-deps.md -FirstCall
GenerateReadme -Repo dotnet/$($repoName)runtime-deps -Readme README.runtime-deps.preview.md
GenerateReadme -Repo dotnet/$($coreRepoName)runtime -Readme README.runtime.md
GenerateReadme -Repo dotnet/$($repoName)runtime -Readme README.runtime.preview.md
GenerateReadme -Repo dotnet/$($coreRepoName)aspnet -Readme README.aspnet.md
GenerateReadme -Repo dotnet/$($repoName)aspnet -Readme README.aspnet.preview.md
GenerateReadme -Repo dotnet/$($coreRepoName)sdk -Readme README.sdk.md
GenerateReadme -Repo dotnet/$($repoName)sdk -Readme README.sdk.preview.md
GenerateReadme -Repo dotnet/core/samples -Readme README.samples.md
