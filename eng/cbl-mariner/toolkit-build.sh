#!/usr/bin/env sh

set -e

marinerRepoTag="$1"
outputPath="$2"
runtimeDepsPackageListPath="$3"
script=$(readlink -f "$0")
scriptDir=$(dirname "$script")
repoRoot=$(git rev-parse --show-toplevel)

# Necessary to avoid having the toolkit think it's running in a CDPx build environment which would cause this build to not work
rm /.dockerenv

git clone --depth 1 --branch $marinerRepoTag https://github.com/microsoft/CBL-Mariner.git /CBL-Mariner
cd /CBL-Mariner/toolkit
make package-toolkit REBUILD_TOOLS=y

# Set up a separate build location outside of the repo
mkdir -p /builder/SPECS
cd /builder
cp /CBL-Mariner/out/toolkit-*.tar.gz ./
tar -xzvf toolkit-*.tar.gz
cd toolkit

configPath="$scriptDir/imageconfig.json"

# Add the runtime-deps package list path to the image config
echo $(jq --arg new $repoRoot$runtimeDepsPackageListPath '.SystemConfigs[].PackageLists? += [$new]' $configPath) > $configPath

# Build the distroless tarball
make image CONFIG_FILE=$configPath
mv /builder/out/images/imageconfig/dotnet-runtime-deps-*.tar.gz $outputPath
