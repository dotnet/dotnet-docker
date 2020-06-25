#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

channel=$1

curl -SLo sdk.zip https://aka.ms/dotnet/$channel/Sdk/dotnet-sdk-win-x64.zip

sdkVer=$(unzip -p sdk.zip "sdk/*/.version" | cat | head -2 | tail -1)
runtimeVer=$(unzip -p sdk.zip "shared/Microsoft.NETCore.App/*/.version" | cat | tail -1)
aspnetVer=$(unzip -p sdk.zip "shared/Microsoft.AspNetCore.App/*/.version" | cat | tail -1)

rm sdk.zip

echo "##vso[task.setvariable variable=sdkVer]$sdkVer"
echo "##vso[task.setvariable variable=runtimeVer]$runtimeVer"
echo "##vso[task.setvariable variable=aspnetVer]$aspnetVer"
