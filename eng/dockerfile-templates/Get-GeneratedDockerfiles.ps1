#!/usr/bin/env pwsh
param(
    # Paths to the Dockerfiles to generate.
    [string[]]$Paths
)

if ($Paths) {
    $pathArgs = " --path " + ($Paths -join " --path ")
}

$repoRoot = (Get-Item "$PSScriptRoot").Parent.Parent.FullName

$onDockerfilesGenerated = {
    param($ContainerName)
    Exec "docker cp ${ContainerName}:/repo/src $repoRoot"
}

& $PSScriptRoot/../common/Invoke-ImageBuilder.ps1 `
    -ImageBuilderArgs "generateDockerfiles --architecture * --os-type *$pathArgs" `
    -OnCommandExecuted $onDockerfilesGenerated
