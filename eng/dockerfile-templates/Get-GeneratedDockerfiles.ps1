#!/usr/bin/env pwsh
param(
    [switch]$Validate,
    [string]$Branch
)

Import-Module -force $PSScriptRoot/../DependencyManagement.psm1

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
    $Branch = Get-Branch
}

& $PSScriptRoot/../common/Invoke-ImageBuilder.ps1 `
    -ImageBuilderArgs "generateDockerfiles --architecture '*' --os-type '*'$customImageBuilderArgs --var branch=$Branch" `
    -OnCommandExecuted $onDockerfilesGenerated
