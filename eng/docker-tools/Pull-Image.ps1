#!/usr/bin/env pwsh

[cmdletbinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$Image,

    [Parameter(Mandatory = $false)]
    [int]$Retries = 2,

    [Parameter(Mandatory = $false)]
    [int]$WaitFactor = 6
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

& "$PSScriptRoot/Invoke-WithRetry.ps1" "docker pull $Image" -Retries $Retries -WaitFactor $WaitFactor
