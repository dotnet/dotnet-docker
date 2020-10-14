#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

channel=$1

sudo apt-get update && \
    sudo apt-get install -y --no-install-recommends libxml2-utils

curl -SLo sdk.zip https://aka.ms/dotnet/$channel/Sdk/dotnet-sdk-win-x64.zip

commitSha=$(unzip -p sdk.zip "sdk/*/.version" | cat | head -1)
# Remove last character (end-of-line)
commitSha=${commitSha%?}
sdkVer=$(unzip -p sdk.zip "sdk/*/.version" | cat | head -2 | tail -1)

curl -SLo versionDetails.xml https://raw.githubusercontent.com/dotnet/installer/$commitSha/eng/Version.Details.xml

runtimeVer=$(xmllint --xpath string\(//ProductDependencies/Dependency[@Name=\'Microsoft.NETCore.App.Internal\']/@Version\) versionDetails.xml)
aspnetVer=$(xmllint --xpath string\(//ProductDependencies/Dependency[contains\(@Name,\'VS.Redist.Common.AspNetCore.TargetingPack.x64\'\)]/@Version\) versionDetails.xml)

rm sdk.zip
rm versionDetails.xml

echo "##vso[task.setvariable variable=sdkVer]$sdkVer"
echo "##vso[task.setvariable variable=runtimeVer]$runtimeVer"
echo "##vso[task.setvariable variable=aspnetVer]$aspnetVer"
