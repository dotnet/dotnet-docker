#!/usr/bin/env sh

# This is the main script for retrieving the list of upgradable packages.

imageTag=$1
dockerfilePath=$2
outputPath=$3

shift 3
packagelist=$@

scriptDir="$(dirname $(realpath $0))"
containerName="$(uuidgen)"
containerOutputPath="/$containerName.txt"

docker run --name $containerName -v $scriptDir:/scripts --entrypoint /bin/sh $imageTag -c "/scripts/get-upgradable-packages.container.sh $containerOutputPath $packagelist"

docker cp $containerName:$containerOutputPath $outputPath
docker rm $containerName 1>/dev/null
