#!/usr/bin/env pwsh
param(
    [string]$Branch,
    [string]$Manifest='manifest.json',
    [string]$ReadMeTemplate='./scripts/TagsDocumentationTemplate.md',
    [string]$TagsTemplate='./scripts/FullTagsDocumentationTemplate.md',
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
    param ([string] $Template, [string] $ReadmePath = $null)

    if ($Template) {
        $templateOption = "--template $Template"
    }

    if ($ReadmePath) {
        $readmePathOption = "--readme-path $ReadmePath"
    }

    $dockerRunCmd = "docker run --rm" `
    + " -v /var/run/docker.sock:/var/run/docker.sock" `
    + " -v ${repoRoot}:/repo" `
    + " -w /repo" `
    + " $ImageBuilderImageName" `
    + " generateTagsReadme --update-readme --manifest $Manifest $readmePathOption $templateOption https://github.com/dotnet/dotnet-docker/blob/${Branch}"
    Invoke-Expression $dockerRunCmd
}

& docker pull $ImageBuilderImageName

GenerateDoc $ReadMeTemplate

if ($TagsTemplate) {
    GenerateDoc ./scripts/FullTagsDocumentationTemplate.md TAGS.md
}
