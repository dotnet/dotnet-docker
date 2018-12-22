#!/usr/bin/env pwsh
param(
    [string]$Branch,
    [string]$ImageBuilderImageName='microsoft/dotnet-buildtools-prereqs:image-builder-debian-20181221161902'
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
    param ([string] $Template, [string] $ReadmePath, [string] $Manifest, [string] $Branch, [switch] $SkipValidation)

    if ($Template) {
        $templateOption = "--template $Template"
    }

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
            + " --readme-path $ReadmePath" `
            + " $templateOption" `
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
    $dockerHubRepo = $manifestJson.Repos[0].Name.Split('/')[1]
    if ($dockerHubRepo -eq "dotnet") {
        $Branch = "master"
    }
    else {
        $Branch = "nightly"
    }
}

Exec "docker pull $ImageBuilderImageName"

GenerateDoc './scripts/ReadmeTagsDocumentationTemplate.md' README.md './manifest.json' $Branch -SkipValidation
GenerateDoc './scripts/TagsDocumentationTemplate.md' TAGS.md './manifest.json' $Branch
GenerateDoc './scripts/SamplesReadmeTagsDocumentationTemplate.md' ./samples/README.DockerHub.md './manifest.samples.json' 'master'
