#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

channel=$1
useInternalBuild=$2
blobStorageSasQueryString=$3
azdoVersionsRepoInfoAccessToken=$4

sudo apt-get update && \
    sudo apt-get install -y --no-install-recommends libxml2-utils

if [ "$useInternalBuild" = "true" ]; then
    channel="internal/$channel"
    queryString="$blobStorageSasQueryString"
else
    queryString=""
fi

akaMsUrl="https://aka.ms/dotnet/$channel/dotnet-sdk-win-x64.zip$queryString"

echo "Downloading SDK from $akaMsUrl"

# Download the SDK and resolve the redirected URL
sdkUrl=$(curl -w %{url_effective} -sSLo sdk.zip $akaMsUrl)

unzip -p sdk.zip "sdk/*/.version" > sdkversion

commitSha=$(cat sdkversion | head -1)
commitSha=${commitSha%?} # Remove last character (newline)

# Resolve the SDK build version from the SDK URL
# Example URL: https://dotnetcli.azureedge.net/dotnet/Sdk/6.0.100-rtm.21522.1/dotnet-sdk-6.0.100-win-x64.zip
#   The sed command below extracts the 6.0.100-rtm.21522.1 from the URL
# We don't want the version contained in the SDK's .version file because that may be a stable branding version
# The sed command doesn't support non-greedy matching so we have to be careful with the trailing slash after
# the version because there may be more than one slash if a SAS token is present. To handle this, we just look
# for the beginning of the filename with starts with "dotnet".
sdkVer=$(echo $sdkUrl | sed 's|.*/Sdk/\(.*\)/dotnet.*|\1|g')

rm sdkversion

if [ -z "$sdkVer" ]; then
    echo "Unable to resolve the SDK version" >&2
    exit 1
fi

versionDetailsPath="eng/Version.Details.xml"
if [ "$useInternalBuild" = "true" ]; then
    dotnetInstallerRepoId="c20f712b-f093-40de-9013-d6b084c1ff30"
    versionDetailsUrl="https://dev.azure.com/dnceng/internal/_apis/git/repositories/$dotnetInstallerRepoId/items?scopePath=/$versionDetailsPath&api-version=6.0&version=$commitSha&versionType=commit"
    requestUserArgs="-u :$azdoVersionsRepoInfoAccessToken"
else
    versionDetailsUrl="https://raw.githubusercontent.com/dotnet/installer/$commitSha/$versionDetailsPath"
    requestUserArgs=""
fi

curl $requestUserArgs -fSLo versionDetails.xml $versionDetailsUrl

getDependencyVersion() {
    local dependencyName=$1
    xmllint --xpath "string(//ProductDependencies/Dependency[starts-with(@Name,'$dependencyName')]/@Version)" versionDetails.xml
}

runtimeVer=$(getDependencyVersion "VS.Redist.Common.NetCore.SharedFramework.x64")
if [ -z "$runtimeVer" ]; then
    runtimeVer=$(getDependencyVersion "Microsoft.NETCore.App.Internal")
fi

aspnetVer=$(getDependencyVersion "VS.Redist.Common.AspNetCore.SharedFramework.x64")

rm sdk.zip
rm versionDetails.xml

if [ -z "$runtimeVer" ]; then
    echo "Unable to resolve the runtime version" >&2
    exit 1
fi

if [ -z "$aspnetVer" ]; then
    echo "Unable to resolve the ASP.NET Core runtime version" >&2
    exit 1
fi

echo "##vso[task.setvariable variable=sdkVer]$sdkVer"
echo "##vso[task.setvariable variable=runtimeVer]$runtimeVer"
echo "##vso[task.setvariable variable=aspnetVer]$aspnetVer"
