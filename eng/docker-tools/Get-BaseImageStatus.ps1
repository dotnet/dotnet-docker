#!/usr/bin/env pwsh

<#
.SYNOPSIS
Outputs the status of external base images referenced in the Dockerfiles.
#>
[cmdletbinding()]
param(
    # Path to the manifest file to use
    [string]
    $Manifest = "manifest.json",

    # Architecture to filter Dockerfiles to
    [string]
    $Architecture = "*",

    # A value indicating whether to run the script continously
    [switch]
    $Continuous,

    # Number of seconds to wait between each iteration
    [int]
    $ContinuousDelay = 10
)

Set-StrictMode -Version Latest

$imageBuilderArgs = "getBaseImageStatus --manifest $Manifest --architecture $Architecture"
if ($Continuous) {
    $imageBuilderArgs += " --continuous --continuous-delay $ContinuousDelay"
}

& "$PSScriptRoot/Invoke-ImageBuilder.ps1" -ImageBuilderArgs $imageBuilderArgs
