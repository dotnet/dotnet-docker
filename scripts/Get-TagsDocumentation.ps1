#!/usr/bin/env pwsh
param(
    [string]$Branch,
    [string]$Manifest='manifest.json',
    [string]$ReadMeTemplate='./scripts/ReadmeTagsDocumentationTemplate.md',
    [string]$TagsTemplate='./scripts/TagsDocumentationTemplate.md',
    [string]$ImageBuilderImageName='microsoft/dotnet-buildtools-prereqs:image-builder-debian-20180821134221'
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Path "$PSScriptRoot" -Parent

if (!$Branch) {
    $manifestJson = Get-Content ${repoRoot}/${Manifest} | ConvertFrom-Json
    $dockerHubRepo = $manifestJson.Repos[0].Name.Split('/')[1]
    if ($dockerHubRepo -eq "dotnet") {
        $Branch = "master"
    }
    else {
        $Branch = "nightly"
    }
}

function GenerateDoc {
    param ([string] $Template, [string] $ReadmePath = $null, [switch] $SkipValidation)

    if ($Template) {
        $templateOption = "--template $Template"
    }

    if ($ReadmePath) {
        $readmePathOption = "--readme-path $ReadmePath"
    }

    if ($SkipValidation) {
        $skipValidationOption = "--skip-validation"
    }

    $dockerRunCmd = "docker run --rm" `
    + " -v /var/run/docker.sock:/var/run/docker.sock" `
    + " -v ${repoRoot}:/repo" `
    + " -w /repo" `
    + " $ImageBuilderImageName" `
    + " generateTagsReadme --update-readme --manifest $Manifest $readmePathOption $templateOption $skipValidationOption https://github.com/dotnet/dotnet-docker/blob/${Branch}"
    Invoke-Expression $dockerRunCmd

    if ($LastExitCode -ne 0) {
        throw "Failed to generate documentation"
    }
}

& docker pull $ImageBuilderImageName

GenerateDoc $ReadMeTemplate -SkipValidation

if ($TagsTemplate) {
    GenerateDoc $TagsTemplate TAGS.md
}
