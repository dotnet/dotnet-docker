#!/usr/bin/env sh

# This is the main script for retrieving the list of upgradable packages.

usage() {
    echo "Usage: $0 [<OPTIONS>...] IMAGETAG DOCKERFILEPATH [PACKAGEVERSION...]"
    echo
    echo "Queries a container to determine which packages are upgradable"
    echo 
    echo "Options:"
    echo "  -h: Print this help message"
    echo "  -a: Additional package manager args"
    echo "  -o: Output path for a formatted list of upgradable packages"
    echo "Arguments:"
    echo "  IMAGETAG: The tag of the image to query"
    echo "  DOCKERFILEPATH: The path to the image's Dockerfile"
    echo "  PACKAGEVERSION: A package version to check whether its upgradable (format: NAME=VERSION)"
}

while getopts "ha:o:" opt; do
    case $opt in
        h)
            usage
            exit 0
            ;;
        a)
            pkgManagerArgs=$OPTARG
            shift 2
            ;;
        o)
            outputPath=$OPTARG
            shift 2
            ;;
        \?)
            echo "Invalid option: -$OPTARG" >&2
            usage
            exit 1
            ;;
        :)
            echo "Option -$OPTARG requires an argument." >&2
            usage
            exit 1
            ;;
    esac
done

imageTag=$1
dockerfilePath=$2
shift 2
packagelist=$@

scriptDir="$(dirname $(realpath $0))"
containerName="container$RANDOM"
containerOutputPath="/$containerName.txt"

scriptsWrapperImageTag="tag$RANDOM"
docker build -t $scriptsWrapperImageTag -f $scriptDir/Dockerfile.scripts --build-arg BASETAG=$imageTag $scriptDir 1>/dev/null 2>/dev/null

docker run \
    --name $containerName \
    --entrypoint /bin/sh \
    --user root \
    $scriptsWrapperImageTag \
    -c "/scripts/get-upgradable-packages.container.sh $containerOutputPath \"$pkgManagerArgs\" $packagelist"

if [ -n "$outputPath" ]; then
    docker cp $containerName:$containerOutputPath $outputPath
fi

docker rm $containerName 1>/dev/null
