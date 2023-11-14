#!/usr/bin/env pwsh

<#
.SYNOPSIS
Returns the various component versions of the latest .NET build.
#>
[cmdletbinding()]
param(
    # The release channel to use for determining the latest .NET build.
    [Parameter(ParameterSetName = "Channel")]
    [string]
    $Channel,

    [Parameter(ParameterSetName = "Explicit")]
    # SDK versions to target
    [string[]]
    $SdkVersions,

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

function GetLatestSdkVersionInfoFromChannel([string]$queryString) {
    $sdkFile = "dotnet-sdk-win-x64.zip"
    $akaMsUrl = "https://aka.ms/dotnet/$Channel/$sdkFile$queryString"
    Write-Host "Querying $akaMsUrl"
    $response = Invoke-WebRequest -Uri $akaMsUrl -Method Head
    $sdkUrl = $response.BaseResponse.RequestMessage.RequestUri.AbsoluteUri
    Write-Host "Resolved SDK URL: $sdkUrl"

    return GetSdkVersionInfo $sdkUrl
}

function GetSdkVersionInfo([string]$sdkUrl) {
    New-Item -Path $tempDir -ItemType Directory -Force | Out-Null

    try {
        # Download the SDK
        Write-Host "Downloading SDK from $sdkUrl"
        $sdkOutPath = "$tempDir/sdk.zip"
        Invoke-WebRequest -Uri $sdkUrl -OutFile $sdkOutPath

        # Extract .version file from SDK zip
        $zipFile = [IO.Compression.ZipFile]::OpenRead($sdkOutPath)
        try {
            $zipFile.Entries | Where-Object { $_.FullName -like "sdk/*/.version" } | ForEach-Object {
                [IO.Compression.ZipFileExtensions]::ExtractToFile($_, "$tempDir/$($_.Name)")
            }
        }
        finally {
            $zipFile.Dispose()
        }

        # Get the commit SHA from the .version file. Example contents:
        #
        # Unstable version:
        # f54f69f9aade0a00936aca343c3eb493d5edbc05
        # 8.0.100-rtm.23512.13
        # win-x64
        # 8.0.100-rtm.23512.13
        #
        # Stable version:
        # eb26aacfecb08dd87b418f4cfaf7faa10eb1900f
        # 7.0.401
        # win-x64
        # 7.0.401-servicing.23425.30
        #
        $versionInfoText = $(Get-Content "$tempDir/.version")

        $commitSha = $versionInfoText[0].Trim()
        $shortVersion = $versionInfoText[1].Trim()
        $rid = $versionInfoText[2].Trim()
        $fullVersion = $versionInfoText[3].Trim()
        $isStableVersion = $fullVersion -ne $shortVersion

        return [PSCustomObject]@{
            CommitSha = $commitSha
            Version = $fullVersion
            IsStableVersion = $isStableVersion
            Rid = $rid
        }
    }
    finally {
        Remove-Item $tempDir -Force -Recurse -ErrorAction Ignore
    }
}

function ResolveSdkUrl([string]$sdkVersion, [string]$queryString, [bool]$useStableBranding) {
    if ($useStableBranding) {
        $sdkStableVersion = ($sdkVersion -split "-")[0]
    }
    else {
        $sdkStableVersion = $sdkVersion
    }

    $zipFile = "dotnet-sdk-$sdkStableVersion-win-x64.zip"

    $containerVersion = $sdkVersion.Replace(".", "-")

    if ($UseInternalBuild) {
        $sdkUrl = "https://dotnetstage.blob.core.windows.net/$containerVersion-internal/Sdk/$sdkVersion/$zipFile$queryString"
    }
    else {
        $sdkUrl = "https://dotnetbuilds.blob.core.windows.net/public/Sdk/$sdkVersion/$zipFile"
    }
    return $sdkUrl
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

if ($UseInternalBuild) {
    if ($Channel)
    {
        $Channel = "internal/$Channel"
    }

    $queryString = "$BlobStorageSasQueryString"
}
else {
    $queryString = ""
}

$sdkVersionInfos = @()

if ($Channel) {
    $sdkVersionInfo = GetLatestSdkVersionInfoFromChannel $queryString
    $sdkVersionInfos += $sdkVersionInfo
}

foreach ($sdkVersion in $SdkVersions)
{
    $useStableBranding = & $PSScriptRoot/Get-IsStableBranding.ps1 -Version $sdkVersion
    $sdkUrl = ResolveSdkUrl $sdkVersion $queryString $useStableBranding
    $sdkVersionInfo = GetSdkVersionInfo $sdkUrl
    $sdkVersionInfos += $sdkVersionInfo
}

Write-Host "Resolved SDK versions: $SdkVersions"
$versionInfos = @()
foreach ($sdkVersionInfo in $SdkVersionInfos) {
    $versionDetails = GetVersionDetails $sdkVersionInfo.CommitSha

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

    $sdkVersionParts = $sdkVersionInfo.Version -split "\."
    $dockerfileVersion = "$($sdkVersionParts[0]).$($sdkVersionParts[1])"

    Write-Host "Dockerfile version: $dockerfileVersion"
    Write-Host "SDK version: $($sdkVersionInfo.Version)"
    Write-Host "Runtime version: $runtimeVersion"
    Write-Host "ASP.NET Core version: $aspnetVersion"
    Write-Host

    $versionInfos += @{
        DockerfileVersion = $dockerfileVersion
        SdkVersion = $sdkVersionInfo.Version
        RuntimeVersion = $runtimeVersion
        AspnetVersion = $aspnetVersion
        StableBranding = $sdkVersionInfo.IsStableVersion
    }
}

Write-Output "##vso[task.setvariable variable=versionInfos]$($versionInfos | ConvertTo-Json -Compress -AsArray)"
