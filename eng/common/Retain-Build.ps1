# Adapted from https://github.com/dotnet/arcade/blob/main/eng/common/retain-build.ps1
Param(
    [Parameter(Mandatory = $true)][int] $BuildId,
    [Parameter(Mandatory = $true)][string] $AzdoOrgUri, 
    [Parameter(Mandatory = $true)][string] $AzdoProject,
    [Parameter(Mandatory = $true)][string] $Token
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

function Get-AzDOHeaders(
    [string] $Token) {
    $base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(":${Token}"))
    $headers = @{"Authorization" = "Basic $base64AuthInfo" }
    return $headers
}

function Update-BuildRetention(
    [string] $AzdoOrgUri,
    [string] $AzdoProject,
    [int] $BuildId,
    [string] $Token) {
    $headers = Get-AzDOHeaders -Token $Token
    $requestBody = "{
        `"keepForever`": `"true`"
    }"

    $requestUri = "${AzdoOrgUri}/${AzdoProject}/_apis/build/builds/${BuildId}?api-version=6.0"
    Write-Host "Attempting to retain build using the following URI: ${requestUri} ..."

    try {
        Invoke-RestMethod -Uri $requestUri -Method Patch -Body $requestBody -Header $headers -contentType "application/json"
        Write-Host "Updated retention settings for build ${BuildId}."
    }
    catch {
        Write-Host "##[error] Failed to update retention settings for build: $($_.Exception.Response.StatusDescription)"
        exit 1
    }
}

Update-BuildRetention -AzdoOrgUri $AzdoOrgUri -AzdoProject $AzdoProject -BuildId $BuildId -Token $Token
exit 0
