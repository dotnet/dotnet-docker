#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

curl -SLo dotnet-monitor.nupkg.version https://aka.ms/dotnet/dotnet-monitor.nupkg.version

# Read version file and remove newlines
monitorVer=$(tr -d '\r\n' < dotnet-monitor.nupkg.version)

rm dotnet-monitor.nupkg.version

echo "##vso[task.setvariable variable=monitorVer]$monitorVer"
