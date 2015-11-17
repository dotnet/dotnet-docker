#!/usr/bin/env bash
#
# $1 is the version number

set -e

if [ $# -eq 0 ] ; then
    VERSION="1.0"
else
    VERSION=$1
fi

echo Building Docker image: dotnet
docker build -t dotnet --no-cache ./src/$VERSION/
docker tag -f dotnet dotnet:$VERSION
docker tag -f dotnet msimons/dotnet
docker tag -f dotnet msimons/dotnet:$VERSION

echo Building Docker image: dotnet:onbuild
docker build -t dotnet:onbuild --no-cache ./src/$VERSION/onbuild
docker tag -f dotnet:onbuild dotnet:${VERSION}-onbuild
docker tag -f dotnet:onbuild msimons/dotnet:onbuild
docker tag -f dotnet:onbuild msimons/dotnet:${VERSION}-onbuild

