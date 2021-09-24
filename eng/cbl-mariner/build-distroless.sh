#!/usr/bin/env bash

set -e

marinerRepoTag="$1"
outputPath="$2"
scriptDir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
prereqsTag="mcr.microsoft.com/dotnet-buildtools/prereqs:ubuntu-20.04-cbl-mariner-20210924193732-ba78c9e"

# Update Docker with new configuration
# TODO: Can be removed when https://github.com/dotnet/dotnet-docker/issues/3142 is completed
sudo systemctl stop docker
sudo cp $scriptDir/daemon.json /etc/docker/daemon.json
sudo systemctl start docker

# Run container to build distroless tarball
docker pull $prereqsTag
containerOutputPath="/dotnet-runtime-deps.tar.gz"
docker run --name mariner-build -v $scriptDir:/scripts --privileged $prereqsTag /scripts/toolkit-build.sh $marinerRepoTag $containerOutputPath

# Copy tarball out of container and clean up
docker cp mariner-build:$containerOutputPath $outputPath
docker rm mariner-build

# Restore Docker with original configuration
# TODO: Can be removed when https://github.com/dotnet/dotnet-docker/issues/3142 is completed
sudo systemctl stop docker
sudo rm -rf /var/lib/docker/devicemapper
sudo rm /etc/docker/daemon.json
sudo systemctl start docker
