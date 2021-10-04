#!/usr/bin/env sh

set -e

marinerRepoTag="$1"
outputPath="$2"
script=$(readlink -f "$0")
scriptDir=$(dirname "$script")

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

make image CONFIG_FILE=$scriptDir/imageconfig.json

mv /builder/out/images/imageconfig/dotnet-runtime-deps-*.tar.gz $outputPath
