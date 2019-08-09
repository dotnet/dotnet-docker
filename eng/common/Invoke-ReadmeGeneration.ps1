#!/usr/bin/env pwsh
param (
    [string] $Repo,
    [string] $ReadmePath,
    [string] $Manifest,
    [string] $GitRepo,
    [string] $Branch = "master",
    [switch] $ReuseImageBuilderImage
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
    Exec "docker cp ${ContainerName}:/repo/$ReadmePath $repoRoot/$ReadmePath"
}

$imageBuilderArgs = "generateTagsReadme" `
    + " --manifest $Manifest" `
    + " --repo $Repo" `
    + " $GitRepo/blob/${Branch}"

& "$PSScriptRoot/Invoke-ImageBuilder.ps1" `
    -ImageBuilderArgs $imageBuilderArgs `
    -ReuseImageBuilderImage:$ReuseImageBuilderImage `
    -OnCommandExecuted $onTagsGenerated
