#!/usr/bin/env pwsh
param(
    [string]$Branch
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Path "$PSScriptRoot" -Parent

function Log {
    param ([string] $Message)

    Write-Output $Message
}

function Exec {
    param ([string] $Cmd)

    Log "Executing: '$Cmd'"
    Invoke-Expression $Cmd
    if ($LASTEXITCODE -ne 0) {
        throw "Failed: '$Cmd'"
    }
}

function GenerateDoc {
    param (
        [string] $Repo,
        [string] $ReadmePath,
        [string] $Manifest,
        [string] $Branch,
        [switch] $ReuseImageBuilderImage
    )

    $onTagsGenerated = {
        param($ContainerName)
        Exec "docker cp ${ContainerName}:/repo/$ReadmePath $repoRoot/$ReadmePath"
    }

    $imageBuilderArgs = "generateTagsReadme" `
        + " --manifest $Manifest" `
        + " --repo $Repo" `
        + " https://github.com/dotnet/dotnet-docker/blob/${Branch}"

    & "$PSScriptRoot/Invoke-ImageBuilder.ps1" `
        -ImageBuilderArgs $imageBuilderArgs `
        -ReuseImageBuilderImage:$ReuseImageBuilderImage `
        -OnCommandExecuted $onTagsGenerated
}

if (!$Branch) {
    $manifestJson = Get-Content ${repoRoot}/manifest.json | ConvertFrom-Json
    if ($manifestJson.Repos[0].Name.Contains("nightly")) {
        $Branch = "nightly"
        $coreRepoName = "core-nightly"
    }
    else {
        $Branch = "master"
        $coreRepoName = "core"
    }
}

GenerateDoc dotnet/$coreRepoName/runtime-deps README.runtime-deps.md manifest.json $Branch
GenerateDoc dotnet/$coreRepoName/runtime README.runtime.md manifest.json $Branch -ReuseImageBuilderImage
GenerateDoc dotnet/$coreRepoName/aspnet README.aspnet.md manifest.json $Branch -ReuseImageBuilderImage
GenerateDoc dotnet/$coreRepoName/sdk README.sdk.md manifest.json $Branch -ReuseImageBuilderImage
GenerateDoc dotnet/core/samples README.samples.md manifest.samples.json master -ReuseImageBuilderImage