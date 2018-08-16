#!/usr/bin/env pwsh
[cmdletbinding()]
param(
    [Parameter(Mandatory = $true)][string]$Image
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Executes a command and retries if it fails.
function Exec {
    param (
        [Parameter(Mandatory = $true)][scriptblock]$cmd,
        [Parameter(Mandatory = $false)][int]$retries = 5,
        [Parameter(Mandatory = $false)][int]$waitFactor = 6
    )

    $count = 0
    $completed = $false

    while (-not $completed) {
        & $cmd
        $exit = $LASTEXITCODE
        $count++

        if ($exit -eq 0) {
            $completed = $true
        }
        else {
            if ($count -lt $retries) {
                $wait = [Math]::Pow($waitFactor, $count - 1)
                Write-Output "Retry $count/$retries exited $exit, retrying in $wait seconds..."
                Start-Sleep $wait
            }
            else {
                Write-Output "Retry $count/$retries exited $exit, no more retries left."
                $completed = $true
            }
        }
    }
}

Exec { docker pull $Image }
