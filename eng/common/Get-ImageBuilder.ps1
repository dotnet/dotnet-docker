#!/usr/bin/env pwsh

# Load common image names
Get-Content $PSScriptRoot/templates/variables/docker-images.yml |
Where-Object { $_.Trim() -notlike 'variables:' } |
ForEach-Object { 
    $parts = $_.Split(':', 2)
    Set-Variable -Name $parts[0].Trim() -Value $parts[1].Trim() -Scope Global
}

& docker inspect ${imageNames.imagebuilder} | Out-Null
if (-not $?) {
    Write-Output "Pulling"
    & $PSScriptRoot/Invoke-WithRetry.ps1 "docker pull ${imageNames.imagebuilder}"
}
