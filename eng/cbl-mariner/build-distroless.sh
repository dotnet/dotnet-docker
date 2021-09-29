#!/usr/bin/env sh

set -e

marinerRepoTag="$1"
outputPath="$2"
script=$(readlink -f "$0")
scriptDir=$(dirname "$script")

# Run container to build distroless tarball
docker build --pull -t mariner-prereqs $scriptDir
containerOutputPath="/dotnet-runtime-deps.tar.gz"
docker run --name mariner-build --privileged mariner-prereqs ./toolkit-build.sh $marinerRepoTag $containerOutputPath

# Copy tarball out of container and clean up
docker cp mariner-build:$containerOutputPath $outputPath
docker rm mariner-build
