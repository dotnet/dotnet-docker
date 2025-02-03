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

    if (-Not $Validate) {
        Exec "docker cp ${ContainerName}:/repo/src $OutputDirectory"
    }
}

if (!$Branch) {
    $Branch = Get-Branch
}

& $PSScriptRoot/../common/Invoke-ImageBuilder.ps1 `
    -ImageBuilderArgs "generateDockerfiles $CustomImageBuilderArgs --var branch=$Branch" `
    -OnCommandExecuted $onDockerfilesGenerated
