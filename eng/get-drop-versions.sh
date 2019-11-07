#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

channel=$1

curl -SLo sdk.zip https://dotnetcli.azureedge.net/dotnet/Sdk/$channel/dotnet-sdk-latest-win-x64.zip
unzip sdk.zip -d sdk
rm sdk.zip

sdkVersionsPath=$(find sdk/sdk -name .version)
sdkVer=$(cat $sdkVersionsPath | head -2 | tail -1)

runtimeVersionsPath=$(find sdk/shared/Microsoft.NETCore.App -name .version)
runtimeVer=$(cat $runtimeVersionsPath | tail -1)

aspnetVersionsPath=$(find sdk/shared/Microsoft.AspNetCore.App -name .version)
aspnetVer=$(cat $aspnetVersionsPath | tail -1)

rm -rf sdk

echo "##vso[task.setvariable variable=sdkVer]$sdkVer"
echo "##vso[task.setvariable variable=runtimeVer]$runtimeVer"
echo "##vso[task.setvariable variable=aspnetVer]$aspnetVer"
