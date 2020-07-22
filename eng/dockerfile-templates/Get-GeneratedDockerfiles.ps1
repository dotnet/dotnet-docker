#!/usr/bin/env pwsh
param(
    [switch]$Validate
)

if ($Validate) {
    $customImageBuilderArgs = " --validate"
}

$repoRoot = (Get-Item "$PSScriptRoot").Parent.Parent.FullName

$onDockerfilesGenerated = {
    param($ContainerName)

    if (-Not $Validate) {
        Exec "docker cp ${ContainerName}:/repo/src $repoRoot"
    }
}

& $PSScriptRoot/../common/Invoke-ImageBuilder.ps1 `
    -ImageBuilderArgs "generateDockerfiles --architecture '*' --os-type '*'$customImageBuilderArgs" `
    -OnCommandExecuted $onDockerfilesGenerated
