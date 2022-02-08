#!/usr/bin/env sh

while getopts "ho:" opt; do
    case $opt in
        o)
            outputPath=$OPTARG
            shift 2
            ;;
    esac
done

version=$1
imageTag=$2
dockerfilePath=$3
shift 2
packagelist=$@

scriptDir="$(dirname $(realpath $0))"
commonPackageScriptsDir="$scriptDir/../../common/package-scripts"
customPkgManagerArgs="--releasever=$version --installroot /staging"

if [ -n "$outputPath" ]; then
    outputPathArg="-o $outputPath"
fi

# Get the image tag of the installer stage. This is the image that will be used
# for querying package versions.
imageTag=$(cat $dockerfilePath |  tr -d '\r' | sed -n 's/FROM\s\(\S*\)\sAS\sinstaller/\1/p')

$commonPackageScriptsDir/get-upgradable-packages.sh \
    -a "$customPkgManagerArgs" \
    $outputPathArg \
    $imageTag \
    $dockerfilePath \
    $packagelist
