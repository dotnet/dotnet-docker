#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

channel=$1

sudo apt-get update && \
    sudo apt-get install -y --no-install-recommends libxml2-utils

curl -SLo sdk.zip https://aka.ms/dotnet/$channel/dotnet-sdk-win-x64.zip

unzip -p sdk.zip "sdk/*/.version" > sdkversion

commitSha=$(cat sdkversion | head -1)
commitSha=${commitSha%?} # Remove last character (newline)

sdkVer=$(cat sdkversion | head -2 | tail -1)

rm sdkversion

curl -SLo versionDetails.xml https://raw.githubusercontent.com/dotnet/installer/$commitSha/eng/Version.Details.xml

runtimeVer=$(xmllint --xpath "string(//ProductDependencies/Dependency[starts-with(@Name,'VS.Redist.Common.NetCore.SharedFramework.x64')]/@Version)" versionDetails.xml)
aspnetVer=$(xmllint --xpath "string(//ProductDependencies/Dependency[starts-with(@Name,'VS.Redist.Common.AspNetCore.SharedFramework.x64')]/@Version)" versionDetails.xml)

rm sdk.zip
rm versionDetails.xml

echo "##vso[task.setvariable variable=sdkVer]$sdkVer"
echo "##vso[task.setvariable variable=runtimeVer]$runtimeVer"
echo "##vso[task.setvariable variable=aspnetVer]$aspnetVer"
