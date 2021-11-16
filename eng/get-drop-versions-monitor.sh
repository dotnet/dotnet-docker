#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

channel=$1

curl -fSLo dotnet-monitor.nupkg.buildversion https://aka.ms/dotnet/diagnostics/monitor$channel/dotnet-monitor.nupkg.buildversion

# Read version file and remove newlines
monitorVer=$(tr -d '\r\n' < dotnet-monitor.nupkg.buildversion)

rm dotnet-monitor.nupkg.buildversion

echo "##vso[task.setvariable variable=monitorVer]$monitorVer"
