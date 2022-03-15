#!/usr/bin/env sh

# Custom script to get the installed packages for the distroless version of CBL-Mariner.
# This script is tied to the implementation of the distroless CBL-Mariner Dockerfiles.

version=$1
imageTag=$2
dockerfilePath=$3

scriptDir="$(dirname $(realpath $0))"

# This script relies on the staging location of the packages that is generated by the
# runtime-deps Dockerfile. The runtime and aspnet Dockerfiles do not install additional package and thus
# do not have have their own staging location. Because of this, it's not possible to query for the packages
# that exist for the runtime and aspnet Dockerfiles. But since they don't install their own packages, the
# runtime-deps Dockerfile can be used instead to query for the packages that are installed. So regardless of
# which Dockerfile is used, the runtime-deps Dockerfile is used to query for the packages that are installed.
# To always target the runtime-deps Dockerfile, "runtime" and "aspnet" will be replaced with "runtime-deps"
# by the base cbl-mariner-distroless script. To prepare for that change to the path, we need to replace
# 7.0 with 6.0 in the path because 7.0 gets its runtime-deps from the 6.0 location.

# Temporarily doesn't apply in main branch while 7.0 dir contains its own runtime-deps Dockerfiles
# dockerfilePath="$(echo $dockerfilePath | sed 's/\/7.0\//\/6.0\//g')"

$scriptDir/../get-installed-packages.sh $version $imageTag $dockerfilePath
