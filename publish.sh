#!/usr/bin/env bash
#
# $1 is the version number

set -e

if [ $# -eq 0 ] ; then
    VERSION="0.0.1.alpha"
else
    VERSION=$1
fi

echo Pushing Docker images: msimons/dotnet:latest, msimons/dotnet:$VERSION
docker push msimons/dotnet
docker push msimons/dotnet:$VERSION

echo Pushing Docker images: msimons/dotnet:onbuild, msimons/dotnet:${VERSION}-onbuild
docker push msimons/dotnet:onbuild
docker push msimons/dotnet:${VERSION}-onbuild
