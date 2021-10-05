#!/usr/bin/env sh

set -e

marinerRepoTag="$1"
outputPath="$2"
runtimeDepsPackageListPath="$3"
script=$(readlink -f "$0")
scriptDir=$(dirname "$script")

repoRoot=$(git rev-parse --show-toplevel)
containerRepoPath="/repo"

# Build an image with the CBL-Mariner prereqs that also contains the contents of this repo
docker build --pull -t mariner-prereqs -f $scriptDir/Dockerfile --build-arg REPO_PATH=$containerRepoPath $repoRoot

containerOutputPath="/dotnet-runtime-deps.tar.gz"
containerName="mariner-build-$(cat /proc/sys/kernel/random/uuid)"
toolkitScriptPath="${scriptDir#$repoRoot}/toolkit-build.sh"

# Run container to build distroless tarball
docker run --name $containerName --privileged mariner-prereqs $containerRepoPath$toolkitScriptPath $marinerRepoTag $containerOutputPath $runtimeDepsPackageListPath

# Copy tarball out of container and clean up
docker cp $containerName:$containerOutputPath $outputPath
docker rm $containerName
