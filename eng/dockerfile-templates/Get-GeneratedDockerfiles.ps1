#!/usr/bin/env pwsh
param(
    [switch]$Validate,
    [string]$Branch,
    [string]$OutputDirectory,
    [string]$CustomImageBuilderArgs
)

Import-Module -force $PSScriptRoot/../DependencyManagement.psm1

if (-Not $CustomImageBuilderArgs) {
    $CustomImageBuilderArgs = ""
}

if ($Validate) {
    $CustomImageBuilderArgs += " --validate"
}

if (-Not $OutputDirectory) {
    $repoRoot = (Get-Item "$PSScriptRoot").Parent.Parent.FullName
    $OutputDirectory = $repoRoot
}

$onDockerfilesGenerated = {
    param($ContainerName)

    # On Windows, ImageBuilder is run locally due to limitations with running Docker client within a container.
    # Remove linux condition when https://github.com/dotnet/docker-tools/issues/159 is resolved
    if ($(Get-DockerOs) -eq "linux" -and -not $Validate) {
        Exec "docker cp ${ContainerName}:/repo/src $OutputDirectory"
    }
}

if (!$Branch) {
    $Branch = Get-Branch
}

& $PSScriptRoot/../docker-tools/Invoke-ImageBuilder.ps1 `
    -ImageBuilderArgs "generateDockerfiles $CustomImageBuilderArgs --var branch=$Branch --no-version-logging" `
    -OnCommandExecuted $onDockerfilesGenerated
