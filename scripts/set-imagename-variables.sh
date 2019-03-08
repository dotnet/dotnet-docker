#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

# Generates Azure DevOps variables for the set of statically defined image names being used
# in the pipeline.  The names of the variables have a prefix of 'imageNames.' followed by the
# name of the corresponding file in the image-names folder.

for filePath in scripts/image-names/*.txt; do
    fileName=$(basename $filePath)
    fileNameWithoutExt=${fileName%.*}
    echo "##vso[task.setvariable variable=imageNames.$fileNameWithoutExt]$(cat $filePath)"
done
