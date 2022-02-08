#!/usr/bin/env sh

scriptDir=$(dirname $(realpath $0))
$scriptDir/get-installed-packages.sh 1.0 $@
