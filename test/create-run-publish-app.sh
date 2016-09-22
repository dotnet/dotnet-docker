#!/usr/bin/env bash
set -e 	# Exit immediately upon failure

: ${1?"Need to pass sandbox directory as argument"}

cd $1
dotnet new
dotnet restore
dotnet run
dotnet publish -o publish
