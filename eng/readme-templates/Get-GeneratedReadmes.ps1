#!/usr/bin/env pwsh
param(
    [switch] $Validate,
    [string] $Branch
)

$ErrorActionPreference = 'Stop'

if ($Validate) {
    $customImageBuilderArgs = " --validate"
}

$repoRoot = (Get-Item "$PSScriptRoot").Parent.Parent.FullName

$onDockerfilesGenerated = {
    param($ContainerName)

    if (-Not $Validate) {
        Exec "docker cp ${ContainerName}:/repo/README.aspnet.md $repoRoot"
        Exec "docker cp ${ContainerName}:/repo/README.md $repoRoot"
        Exec "docker cp ${ContainerName}:/repo/README.monitor.md $repoRoot"
        Exec "docker cp ${ContainerName}:/repo/README.runtime-deps.md $repoRoot"
        Exec "docker cp ${ContainerName}:/repo/README.runtime.md $repoRoot"
        Exec "docker cp ${ContainerName}:/repo/README.samples.md $repoRoot"
        Exec "docker cp ${ContainerName}:/repo/README.sdk.md $repoRoot"
    }
}

function Invoke-GenerateReadme {
    param ([string] $Manifest, [string] $SourceBranch)

    & $PSScriptRoot/../common/Invoke-ImageBuilder.ps1 `
        -ImageBuilderArgs `
            "generateReadmes --manifest $Manifest --source-branch $SourceBranch$customImageBuilderArgs --var branch=$SourceBranch 'https://github.com/dotnet/dotnet-docker'" `
        -OnCommandExecuted $onDockerfilesGenerated
}

if (!$Branch) {
    $manifestJson = Get-Content ${repoRoot}/manifest.json | ConvertFrom-Json
    if ($manifestJson.Repos[0].Name.Contains("nightly")) {
        $Branch = "nightly"
    }
    else {
        $Branch = "main"
    }
}

Invoke-GenerateReadme "manifest.json" $Branch
Invoke-GenerateReadme "manifest.samples.json" "main"
