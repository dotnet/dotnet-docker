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

function CopyReadme([string]$containerName, [string]$readmeRelativePath) {
    $readmeDir = Split-Path $readmeRelativePath -Parent
    Exec "docker cp ${containerName}:/repo/$readmeRelativePath $repoRoot/$readmeDir"
}

$onDockerfilesGenerated = {
    param($ContainerName)

    if (-Not $Validate) {
        CopyReadme $ContainerName "README.aspnet.md"
        CopyReadme $ContainerName "README.md"
        CopyReadme $ContainerName "README.monitor.md"
        CopyReadme $ContainerName "README.monitor-base.md"
        CopyReadme $ContainerName "README.runtime-deps.md"
        CopyReadme $ContainerName "README.runtime.md"
        CopyReadme $ContainerName "README.samples.md"
        CopyReadme $ContainerName "README.sdk.md"

        CopyReadme $ContainerName ".mar/portal/README.aspnet.portal.md"
        CopyReadme $ContainerName ".mar/portal/README.monitor.portal.md"
        CopyReadme $ContainerName ".mar/portal/README.monitor-base.portal.md"
        CopyReadme $ContainerName ".mar/portal/README.runtime-deps.portal.md"
        CopyReadme $ContainerName ".mar/portal/README.runtime.portal.md"
        CopyReadme $ContainerName ".mar/portal/README.samples.portal.md"
        CopyReadme $ContainerName ".mar/portal/README.sdk.portal.md"
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
    $Branch = & $PSScriptRoot/../Get-Branch.ps1
}

Invoke-GenerateReadme "manifest.json" $Branch
Invoke-GenerateReadme "manifest.samples.json" "main"
