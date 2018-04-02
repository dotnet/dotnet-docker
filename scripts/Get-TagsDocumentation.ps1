#!/usr/bin/env pwsh
param(
    [string]$Branch,
    [string]$Manifest='manifest.json',
    [string]$Template='./scripts/TagsDocumentationTemplate.md',
    [string]$ImageBuilderImageName='microsoft/dotnet-buildtools-prereqs:image-builder-debian-20180312170813'
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

if ($Template) {
    $templateOption = "--template $Template"
}

& docker pull $ImageBuilderImageName

$dockerRunCmd = "docker run --rm" `
    + " -v /var/run/docker.sock:/var/run/docker.sock" `
    + " -v ${repoRoot}:/repo" `
    + " -w /repo" `
    + " $ImageBuilderImageName" `
    + " generateTagsReadme --update-readme --manifest $Manifest $templateOption https://github.com/dotnet/dotnet-docker/blob/${Branch}"
Invoke-Expression $dockerRunCmd
