#!/usr/bin/env bash
#
# $1 is the version number

set -e

if [ $# -eq 0 ] ; then
    VERSION="0.0.1-alpha"
else
    VERSION=$1
fi

echo Building Docker image: dotnet
docker build -t dotnet --no-cache ./$VERSION/
docker tag -f dotnet dotnet:$VERSION
docker tag -f dotnet microsoft/dotnet
docker tag -f dotnet microsoft/dotnet:$VERSION

echo Building Docker image: dotnet:onbuild
docker build -t dotnet:onbuild --no-cache ./$VERSION/onbuild
docker tag -f dotnet:onbuild dotnet:${VERSION}-onbuild
docker tag -f dotnet:onbuild microsoft/dotnet:onbuild
docker tag -f dotnet:onbuild microsoft/dotnet:${VERSION}-onbuild

