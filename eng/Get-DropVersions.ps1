#!/usr/bin/env pwsh

<#
.SYNOPSIS
Returns the various component versions of the latest .NET build.
#>
[cmdletbinding()]
param(
    # The release channel to use for determining the latest .NET build.
    [Parameter(Mandatory)]
    [string]
    $Channel,

    # Whether to target an internal .NET build
    [switch]
    $UseInternalBuild,

    # SAS query string used to access the internal blob storage location of the build
    [string]
    $BlobStorageSasQueryString,

    # PAT used to access the versions repo in AzDO
    [string]
    $AzdoVersionsRepoInfoAccessToken
)

function GetSdkInfo() {
    if ($UseInternalBuild) {
        $Channel = "internal/$Channel"
        $queryString = "$BlobStorageSasQueryString"
    }
    else {
        $queryString = ""
    }

    # Download the SDK
    $sdkFile = "dotnet-sdk-win-x64.zip"
    $akaMsUrl = "https://aka.ms/dotnet/$Channel/$sdkFile$queryString"
    Write-Host "Downloading SDK from $akaMsUrl"
    $sdkOutPath = "$tempDir/$sdkFile"
    $response = Invoke-WebRequest -Uri $akaMsUrl -OutFile $sdkOutPath -PassThru
    $sdkUrl = $response.BaseResponse.RequestMessage.RequestUri.AbsoluteUri
    Write-Host "Resolved SDK URL: $sdkUrl"

    # Extract .version file from SDK zip
    $zipFile = [IO.Compression.ZipFile]::OpenRead("$tempDir/$sdkFile")
    try {
        $zipFile.Entries | Where-Object { $_.FullName -like "sdk/*/.version" } | ForEach-Object {
            [IO.Compression.ZipFileExtensions]::ExtractToFile($_, "$tempDir/$($_.Name)")
        }
    }
    finally {
        $zipFile.Dispose()
    }

    # Get the commit SHA from the .version file
    $commitSha = $(Get-Content "$tempDir/.version")[0].Trim()

    # Resolve the SDK build version from the SDK URL
    # Example URL: https://dotnetcli.azureedge.net/dotnet/Sdk/6.0.100-rtm.21522.1/dotnet-sdk-6.0.100-win-x64.zip
    #   The command below extracts the 6.0.100-rtm.21522.1 from the URL
    $sdkUrlPath = "/Sdk/"
    $versionUrlPath = $sdkUrl.Substring($sdkUrl.IndexOf($sdkUrlPath) + $sdkUrlPath.Length)
    $sdkVersion = $versionUrlPath.Substring(0, $versionUrlPath.IndexOf("/"))

    return @{
        SdkVersion = $sdkVersion;
        CommitSha = $commitSha;
    }
}

function GetVersionDetails([string]$commitSha) {
    $versionDetailsPath="eng/Version.Details.xml"
    
    if ($UseInternalBuild) {
        $dotnetInstallerRepoId="c20f712b-f093-40de-9013-d6b084c1ff30"
        $versionDetailsUrl="https://dev.azure.com/dnceng/internal/_apis/git/repositories/$dotnetInstallerRepoId/items?scopePath=/$versionDetailsPath&api-version=6.0&version=$commitSha&versionType=commit"
        $base64AccessToken = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes(":$AzdoVersionsRepoInfoAccessToken"))
        $headers = @{
            "Authorization" = "Basic $base64AccessToken"
        }
    }
    else {
        $versionDetailsUrl="https://raw.githubusercontent.com/dotnet/installer/$commitSha/$versionDetailsPath"
        $headers = @{}
    }

    Write-Host "Downloading version details from $versionDetailsUrl"
    $versionDetails = [xml](Invoke-RestMethod -Uri $versionDetailsUrl -Headers $headers)
    return $versionDetails
}

function GetDependencyVersion([string]$dependencyName, [xml]$versionDetails) {
    $result = Select-Xml -XPath "//ProductDependencies/Dependency[starts-with(@Name,'$dependencyName')]/@Version" -Xml $versionDetails
    return $result.Node.Value
}

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'
Set-StrictMode -Version 2.0

$tempDir = "$([System.IO.Path]::GetTempPath())/dotnet-docker-get-dropversions"
New-Item -Path $tempDir -ItemType Directory -Force | Out-Null

try {
    $sdkInfo = GetSdkInfo
    $versionDetails = GetVersionDetails $sdkInfo.CommitSha

    $runtimeVersion = GetDependencyVersion "VS.Redist.Common.NetCore.SharedFramework.x64" $versionDetails

    $aspnetVersion = GetDependencyVersion "VS.Redist.Common.AspNetCore.SharedFramework.x64" $versionDetails

    if (-not $runtimeVersion) {
        Write-Error "Unable to resolve the runtime version"
        exit 1
    }

    if (-not $aspnetVersion) {
        Write-Error "Unable to resolve the ASP.NET Core runtime version"
        exit 1
    }

    Write-Output "##vso[task.setvariable variable=sdkVer]$($sdkInfo.SdkVersion)"
    Write-Output "##vso[task.setvariable variable=runtimeVer]$runtimeVersion"
    Write-Output "##vso[task.setvariable variable=aspnetVer]$aspnetVersion"
}
finally {
    Remove-Item $tempDir -Force -Recurse -ErrorAction Ignore
}
