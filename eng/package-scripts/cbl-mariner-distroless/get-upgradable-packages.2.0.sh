#!/usr/bin/env sh

scriptDir=$(dirname $(realpath $0))

while getopts "o:" opt; do
    case $opt in
        o)
            outputPath=$OPTARG
            shift 2
            ;;
    esac
done

if [ -n "$outputPath" ]; then
    outputPathArg="-o $outputPath"
fi

$scriptDir/get-upgradable-packages.sh $outputPathArg 2.0 $@
