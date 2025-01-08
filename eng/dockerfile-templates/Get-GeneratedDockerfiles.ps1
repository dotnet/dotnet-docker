#!/usr/bin/env pwsh
param(
    [switch]$Validate,
    [string]$Branch,
    [string]$OutputDirectory
)

Import-Module -force $PSScriptRoot/../DependencyManagement.psm1

if ($Validate) {
    $customImageBuilderArgs = " --validate"
}

if (-Not $OutputDirectory) {
    $repoRoot = (Get-Item "$PSScriptRoot").Parent.Parent.FullName
    $OutputDirectory = $repoRoot
}

$onDockerfilesGenerated = {
    param($ContainerName)

    if (-Not $Validate) {
        Exec "docker cp ${ContainerName}:/repo/src $OutputDirectory"
    }
}

if (!$Branch) {
    $Branch = Get-Branch
}

& $PSScriptRoot/../common/Invoke-ImageBuilder.ps1 `
    -ImageBuilderArgs "generateDockerfiles $customImageBuilderArgs --var branch=$Branch" `
    -OnCommandExecuted $onDockerfilesGenerated
