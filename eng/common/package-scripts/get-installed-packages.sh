#!/usr/bin/env sh

# This is the main script for retrieving the list of installed packages.

usage() {
    echo "Usage: $0 [<OPTIONS>...] IMAGETAG DOCKERFILEPATH"
    echo
    echo "Queries a container to determine which packages are installed"
    echo 
    echo "Options:"
    echo "  -h: Print this help message"
    echo "  -a: Additional package manager args"
    echo "Arguments:"
    echo "  IMAGETAG: The tag of the image to query"
    echo "  DOCKERFILEPATH: The path to the image's Dockerfile"
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
scriptDir=$(dirname $(realpath $0))

docker run \
    --rm \
    -i \
    -e PKG_MANAGER_ARGS="$pkgManagerArgs" \
    --entrypoint /bin/sh \
    --user root \
    $imageTag \
    < $scriptDir/get-installed-packages.container.sh
