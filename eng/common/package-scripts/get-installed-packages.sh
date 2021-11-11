#!/usr/bin/env sh

# This is the main script for retrieving the list of installed packages.

imageTag=$1
dockerfilePath=$2
scriptDir=$(dirname $(realpath $0))

docker run --rm -i $imageTag /bin/sh < $scriptDir/get-installed-packages.container.sh
