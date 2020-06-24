#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

channel=$1

curl -SLo dotnet-monitor.nupkg https://aka.ms/dotnet/$channel/diagnostics/monitor5.0/dotnet-monitor.nupkg
# In nuspec, there is only one element named "version" and it is the version of the nupkg.
# All other uses of "version" are attributes on other elements. grep using Perl regex, reporting
# the first match, and only printing what was matched.
monitorVer=$(unzip -p dotnet-monitor.nupkg dotnet-monitor.nuspec | cat | grep -oPm1 "(?<=<version>)[^<]+")

rm dotnet-monitor.nupkg

echo "##vso[task.setvariable variable=monitorVer]$monitorVer"
