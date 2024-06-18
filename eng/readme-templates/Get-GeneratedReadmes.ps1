#!/usr/bin/env pwsh
param(
    [switch] $Validate,
    [string] $Branch
)

$ErrorActionPreference = 'Stop'
Import-Module -force $PSScriptRoot/../DependencyManagement.psm1

if ($Validate) {
    $customImageBuilderArgs = " --validate"
}

$repoRoot = (Get-Item "$PSScriptRoot").Parent.Parent.FullName

function CopyReadme([string]$containerName, [string]$readmeRelativePath) {
    $readmeDir = Split-Path $readmeRelativePath -Parent
    Exec "docker cp ${containerName}:/repo/$readmeRelativePath $repoRoot/$readmeDir"
}

$onDockerfilesGenerated = {
    param($ContainerName)

    if (-Not $Validate) {
        CopyReadme $ContainerName "README.aspire-dashboard.md"
        CopyReadme $ContainerName "README.aspnet.md"
        CopyReadme $ContainerName "README.md"
        CopyReadme $ContainerName "README.monitor.md"
        CopyReadme $ContainerName "README.monitor-base.md"
        CopyReadme $ContainerName "README.runtime-deps.md"
        CopyReadme $ContainerName "README.runtime.md"
        CopyReadme $ContainerName "README.samples.md"
        CopyReadme $ContainerName "README.sdk.md"

        CopyReadme $ContainerName ".mar/README.aspire-dashboard.md"
        CopyReadme $ContainerName ".mar/README.aspnet.md"
        CopyReadme $ContainerName ".mar/README.md"
        CopyReadme $ContainerName ".mar/README.monitor.md"
        CopyReadme $ContainerName ".mar/README.monitor-base.md"
        CopyReadme $ContainerName ".mar/README.runtime-deps.md"
        CopyReadme $ContainerName ".mar/README.runtime.md"
        CopyReadme $ContainerName ".mar/README.samples.md"
        CopyReadme $ContainerName ".mar/README.sdk.md"

        CopyReadme $ContainerName ".mar/README.aspire-dashboard.portal.md"
        CopyReadme $ContainerName ".mar/README.aspnet.portal.md"
        CopyReadme $ContainerName ".mar/README.monitor.portal.md"
        CopyReadme $ContainerName ".mar/README.monitor-base.portal.md"
        CopyReadme $ContainerName ".mar/README.runtime-deps.portal.md"
        CopyReadme $ContainerName ".mar/README.runtime.portal.md"
        CopyReadme $ContainerName ".mar/README.samples.portal.md"
        CopyReadme $ContainerName ".mar/README.sdk.portal.md"
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
    $Branch = Get-Branch
}

Invoke-GenerateReadme "manifest.json" $Branch
Invoke-GenerateReadme "manifest.samples.json" "main"
