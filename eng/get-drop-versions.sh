#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

channel=$1
monitorChannel=$2

curl -SLo sdk.zip https://aka.ms/dotnet/$channel/Sdk/dotnet-sdk-win-x64.zip

sdkVer=$(unzip -p sdk.zip "sdk/*/.version" | cat | head -2 | tail -1)
runtimeVer=$(unzip -p sdk.zip "shared/Microsoft.NETCore.App/*/.version" | cat | tail -1)
aspnetVer=$(unzip -p sdk.zip "shared/Microsoft.AspNetCore.App/*/.version" | cat | tail -1)

rm sdk.zip

curl -SLo dotnet-monitor.nupkg https://aka.ms/dotnet/$monitorChannel/diagnostics/monitor5.0/dotnet-monitor.nupkg
# In nuspec, there is only one element named "version" and it is the version of the nupkg.
# All other uses of "version" are attributes on other elements. grep using Perl regex, reporting
# the first match, and only printing what was matched.
monitorVer=$(unzip -p dotnet-monitor.nupkg dotnet-monitor.nuspec | cat | grep -oPm1 "(?<=<version>)[^<]+")

rm dotnet-monitor.nupkg

echo "##vso[task.setvariable variable=sdkVer]$sdkVer"
echo "##vso[task.setvariable variable=runtimeVer]$runtimeVer"
echo "##vso[task.setvariable variable=aspnetVer]$aspnetVer"
echo "##vso[task.setvariable variable=monitorVer]$monitorVer"
