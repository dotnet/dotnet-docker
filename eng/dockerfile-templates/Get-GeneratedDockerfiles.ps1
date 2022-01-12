#!/usr/bin/env pwsh
param(
    [switch]$Validate,
    [string]$Branch
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

if (!$Branch) {
    $Branch = & $PSScriptRoot/../Get-Branch.ps1
}

& $PSScriptRoot/../common/Invoke-ImageBuilder.ps1 `
    -ImageBuilderArgs "generateDockerfiles --architecture '*' --os-type '*'$customImageBuilderArgs --var branch=$Branch" `
    -OnCommandExecuted $onDockerfilesGenerated
