#!/usr/bin/env pwsh

# Load common image names
$imageNameVars = & $PSScriptRoot/Get-ImageNameVars.ps1
foreach ($varName in $imageNameVars.Keys) { 
    Set-Variable -Name $varName -Value $imageNameVars[$varName] -Scope Global
}

& docker inspect ${imageNames.imagebuilderName} | Out-Null
if (-not $?) {
    Write-Output "Pulling"
    & $PSScriptRoot/Invoke-WithRetry.ps1 "docker pull ${imageNames.imagebuilderName}"
}
