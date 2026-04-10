#!/usr/bin/env pwsh
# Lightweight wrapper for authenticated Azure DevOps REST API calls.
# Uses `az account get-access-token` for bearer token auth.
#
# Usage:
#   . ./AzureDevOps.ps1
#   $response = Invoke-AzDORestMethod -Organization myorg -Project myproject `
#       -Endpoint "pipelines/42/runs" -Method POST -Body @{ resources = @{} }

$ErrorActionPreference = "Stop"

function Get-AzDOAccessToken {
    <#
    .SYNOPSIS
        Returns a bearer token for Azure DevOps.
    #>

    # Well-known Entra ID application ID for Azure DevOps
    $tokenJson = az account get-access-token --resource "499b84ac-1321-427f-aa17-267ca6975798" 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to get access token. Run 'az login' first. Output: $tokenJson"
    }

    $parsed = $tokenJson | ConvertFrom-Json
    return $parsed.accessToken
}

function Invoke-AzDORestMethod {
    <#
    .SYNOPSIS
        Calls an Azure DevOps REST API endpoint with automatic authentication.
    .PARAMETER Organization
        Azure DevOps organization name (not the full URL).
    .PARAMETER Project
        Azure DevOps project name.
    .PARAMETER Endpoint
        API path after _apis/ (e.g. "pipelines/42/runs", "build/builds").
    .PARAMETER Method
        HTTP method. Defaults to GET.
    .PARAMETER Body
        Request body as a hashtable. Automatically converted to JSON.
    .PARAMETER ApiVersion
        API version. Defaults to 7.1.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Organization,
        [Parameter(Mandatory)][string] $Project,
        [Parameter(Mandatory)][string] $Endpoint,
        [string] $Method = "GET",
        [hashtable] $Body,
        [string] $ApiVersion = "7.1"
    )

    $token = Get-AzDOAccessToken
    $headers = @{
        Authorization  = "Bearer $token"
        "Content-Type" = "application/json"
    }

    $uri = "https://dev.azure.com/$Organization/$Project/_apis/$($Endpoint)?api-version=$ApiVersion"

    $params = @{
        Uri     = $uri
        Headers = $headers
        Method  = $Method
    }

    if ($Body) {
        $params.Body = $Body | ConvertTo-Json -Depth 10
    }

    return Invoke-RestMethod @params
}
