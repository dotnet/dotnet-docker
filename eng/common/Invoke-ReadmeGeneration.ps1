#!/usr/bin/env pwsh
param (
    [string] $Repo,
    [string] $ReadmePath,
    [string] $Manifest,
    [string] $GitRepo,
    [string] $Branch = "master",
    [switch] $ReuseImageBuilderImage,
    [switch] $Validate
)

$ErrorActionPreference = 'Stop'
$repoRoot = (Get-Item "$PSScriptRoot").Parent.Parent.FullName

function Log {
    param ([string] $Message)

    Write-Output $Message
}

function Exec {
    param ([string] $Cmd)

    Log "Executing: '$Cmd'"
    Invoke-Expression $Cmd
    if ($LASTEXITCODE -ne 0) {
        throw "Failed: '$Cmd'"
    }
}

$onTagsGenerated = {
    param($ContainerName)

    if (-Not $Validate) {
        Exec "docker cp ${ContainerName}:/repo/$ReadmePath $repoRoot/$ReadmePath"
    }
}

if ($Validate) {
    $customImageBuilderArgs = " --validate"
}

$imageBuilderArgs = "generateTagsReadme" `
    + " --manifest $Manifest" `
    + " --repo $Repo" `
    + " --source-branch $Branch" `
    + $customImageBuilderArgs `
    + " $GitRepo"

& $PSScriptRoot/Invoke-ImageBuilder.ps1 `
    -ImageBuilderArgs $imageBuilderArgs `
    -ReuseImageBuilderImage:$ReuseImageBuilderImage `
    -OnCommandExecuted $onTagsGenerated
