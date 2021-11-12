#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

channel=$1

sudo apt-get update && \
    sudo apt-get install -y --no-install-recommends libxml2-utils

# Download the SDK and resolve the redirected URL
sdkUrl=$(curl -w %{url_effective} -sSLo sdk.zip https://aka.ms/dotnet/$channel/dotnet-sdk-win-x64.zip)

unzip -p sdk.zip "sdk/*/.version" > sdkversion

commitSha=$(cat sdkversion | head -1)
commitSha=${commitSha%?} # Remove last character (newline)

# Resolve the SDK build version from the SDK URL
# Example URL: https://dotnetcli.azureedge.net/dotnet/Sdk/6.0.100-rtm.21522.1/dotnet-sdk-6.0.100-win-x64.zip
#   The sed command below extracts the 6.0.100-rtm.21522.1 from the URL
# We don't want the version contained in the SDK's .version file because that may be a stable branding version
sdkVer=$(echo $sdkUrl | sed 's|.*/Sdk/\(.*\)/.*|\1|g')

rm sdkversion

curl -fSLo versionDetails.xml https://raw.githubusercontent.com/dotnet/installer/$commitSha/eng/Version.Details.xml

runtimeVer=$(xmllint --xpath "string(//ProductDependencies/Dependency[starts-with(@Name,'VS.Redist.Common.NetCore.SharedFramework.x64')]/@Version)" versionDetails.xml)
aspnetVer=$(xmllint --xpath "string(//ProductDependencies/Dependency[starts-with(@Name,'VS.Redist.Common.AspNetCore.SharedFramework.x64')]/@Version)" versionDetails.xml)

rm sdk.zip
rm versionDetails.xml

echo "##vso[task.setvariable variable=sdkVer]$sdkVer"
echo "##vso[task.setvariable variable=runtimeVer]$runtimeVer"
echo "##vso[task.setvariable variable=aspnetVer]$aspnetVer"
