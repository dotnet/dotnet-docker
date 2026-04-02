#!/usr/bin/env pwsh
# Retrieves a build log by log ID.
# Usage:
#   ./Get-BuildLog.ps1 -Organization dnceng -Project internal -BuildId 12345 -LogId 47

[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $Organization,
    [Parameter(Mandatory)][string] $Project,
    [Parameter(Mandatory)][int]    $BuildId,
    [Parameter(Mandatory)][int]    $LogId
)

$ErrorActionPreference = "Stop"

. "$PSScriptRoot/AzureDevOps.ps1"

Invoke-AzDORestMethod `
    -Organization $Organization `
    -Project $Project `
    -Endpoint "build/builds/$BuildId/logs/$LogId"
