#!/usr/bin/env pwsh
param(
    [string]$Branch,
    [string]$ImageBuilderImageName='microsoft/dotnet-buildtools-prereqs:image-builder-debian-20190122231638'
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
    param ([string] $Template, [string] $Repo, [string] $ReadmePath, [string] $Manifest, [string] $Branch, [switch] $SkipValidation)

    if ($SkipValidation) {
        $skipValidationOption = "--skip-validation"
    }

    $imageBuilderContainerName = "imagebuilder-$(Get-Date -Format yyyyMMddhhmmss)"
    $createCmd = "docker create" `
        + " -v /var/run/docker.sock:/var/run/docker.sock" `
        + " -w /repo" `
        + " --name $imageBuilderContainerName" `
        + " $ImageBuilderImageName" `
            + " generateTagsReadme" `
            + " --update-readme" `
            + " --manifest $Manifest" `
            + " --repo $Repo" `
            + " --template ./scripts/documentation-templates/$Template" `
            + " $skipValidationOption" `
            + " https://github.com/dotnet/dotnet-docker/blob/${Branch}"
    Exec $createCmd
    try {
        Exec "docker cp $repoRoot/. ${imageBuilderContainerName}:/repo/"
        Exec "docker start -a $imageBuilderContainerName"
        Exec "docker cp ${imageBuilderContainerName}:/repo/$ReadmePath $repoRoot/$ReadmePath"
    }
    finally {
        Exec "docker rm -f $imageBuilderContainerName"
    }
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

Exec "docker pull $ImageBuilderImageName"

GenerateDoc runtime-deps-tags.md mcr.microsoft.com/dotnet/$coreRepoName/runtime-deps README.runtime-deps.md manifest.json $Branch
GenerateDoc runtime-tags.md mcr.microsoft.com/dotnet/$coreRepoName/runtime README.runtime.md manifest.json $Branch
GenerateDoc aspnet-tags.md mcr.microsoft.com/dotnet/$coreRepoName/aspnet README.aspnet.md manifest.json $Branch
GenerateDoc sdk-tags.md mcr.microsoft.com/dotnet/$coreRepoName/sdk README.sdk.md manifest.json $Branch
GenerateDoc samples-tags.md mcr.microsoft.com/dotnet/core/samples ./samples/README.DockerHub.md manifest.samples.json master
