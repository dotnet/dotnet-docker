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

    # On Windows, ImageBuilder is run locally due to limitations with running Docker client within a container.
    # Remove linux condition when https://github.com/dotnet/docker-tools/issues/159 is resolved.
    if ($(Get-DockerOs) -eq "linux" -and -not $Validate) {
        CopyReadme $ContainerName "README.aspire-dashboard.md"
        CopyReadme $ContainerName "README.aspnet.md"
        CopyReadme $ContainerName "README.md"
        CopyReadme $ContainerName "README.monitor.md"
        CopyReadme $ContainerName "README.monitor-base.md"
        CopyReadme $ContainerName "README.yarp.md"
        CopyReadme $ContainerName "README.runtime-deps.md"
        CopyReadme $ContainerName "README.runtime.md"
        CopyReadme $ContainerName "README.samples.md"
        CopyReadme $ContainerName "README.sdk.md"

        CopyReadme $ContainerName ".portal-docs/docker-hub/README.aspire-dashboard.md"
        CopyReadme $ContainerName ".portal-docs/docker-hub/README.aspnet.md"
        CopyReadme $ContainerName ".portal-docs/docker-hub/README.monitor.md"
        CopyReadme $ContainerName ".portal-docs/docker-hub/README.monitor-base.md"
        CopyReadme $ContainerName ".portal-docs/docker-hub/README.yarp.md"
        CopyReadme $ContainerName ".portal-docs/docker-hub/README.runtime-deps.md"
        CopyReadme $ContainerName ".portal-docs/docker-hub/README.runtime.md"
        CopyReadme $ContainerName ".portal-docs/docker-hub/README.samples.md"
        CopyReadme $ContainerName ".portal-docs/docker-hub/README.sdk.md"

        CopyReadme $ContainerName ".portal-docs/mar/README.aspire-dashboard.portal.md"
        CopyReadme $ContainerName ".portal-docs/mar/README.aspnet.portal.md"
        CopyReadme $ContainerName ".portal-docs/mar/README.monitor.portal.md"
        CopyReadme $ContainerName ".portal-docs/mar/README.monitor-base.portal.md"
        CopyReadme $ContainerName ".portal-docs/mar/README.yarp.portal.md"
        CopyReadme $ContainerName ".portal-docs/mar/README.runtime-deps.portal.md"
        CopyReadme $ContainerName ".portal-docs/mar/README.runtime.portal.md"
        CopyReadme $ContainerName ".portal-docs/mar/README.samples.portal.md"
        CopyReadme $ContainerName ".portal-docs/mar/README.sdk.portal.md"
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
