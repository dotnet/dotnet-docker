#!/usr/bin/env bash

set -e

marinerRepoTag="$1"
outputPath="$2"

# Necessary to avoid having the toolkit think it's running in a CDPx build environment which would cause this build to not work
rm /.dockerenv

git clone https://github.com/microsoft/CBL-Mariner.git
pushd /CBL-Mariner/toolkit
git checkout $marinerRepoTag
make package-toolkit REBUILD_TOOLS=y
popd

# Set up a separate build location from the main repo
mkdir -p /builder/SPECS
pushd /builder
cp ../CBL-Mariner/out/toolkit-*.tar.gz ./
tar -xzvf toolkit-*.tar.gz
cd toolkit

make image CONFIG_FILE=/scripts/imageconfig.json

mv /builder/out/images/imageconfig/dotnet-runtime-deps-*.tar.gz $outputPath
