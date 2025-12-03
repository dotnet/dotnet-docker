#!/usr/bin/env pwsh

# Executes a command and retries if it fails.
[cmdletbinding()]
param (
        [Parameter(Mandatory = $true)][string]$Cmd,
        [int]$Retries = 2,
        [int]$WaitFactor = 6
    )

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$count = 0
$completed = $false

Write-Output "Executing '$Cmd'"

while (-not $completed) {
    try {
        Invoke-Expression $Cmd
        if (-not $(Test-Path variable:LASTEXITCODE) -or $LASTEXITCODE -eq 0) {
            $completed = $true
            continue
        }
    }
    catch {
    }

    $count++

    if ($count -lt $Retries) {
        $wait = [Math]::Pow($WaitFactor, $count - 1)
        Write-Output "Retry $count/$Retries, retrying in $wait seconds..."
        Start-Sleep $wait
    }
    else {
        Write-Output "Retry $count/$Retries, no more retries left."
        throw "Failed to execute '$Cmd'"
    }
}
