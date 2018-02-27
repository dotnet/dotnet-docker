# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[cmdletbinding()]
param(
    [string]$UpdateDependenciesParams,
    [switch]$CleanupDocker
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$imageName = "update-dependencies"

try {
    $repoRoot = Split-Path -Path "$PSScriptRoot" -Parent

    & docker build -t $imageName -f $PSScriptRoot\update-dependencies\Dockerfile --pull $repoRoot
    if ($LastExitCode -ne 0) {
        throw "Failed to build the update dependencies tool"
    }

    Invoke-Expression "docker run --rm --user ContainerAdministrator $imageName $UpdateDependenciesParams"
    if ($LastExitCode -ne 0) {
        throw "Failed to update dependencies"
    }
}
finally {
    if ($CleanupDocker) {
        & docker rmi -f $imageName
        & docker system prune -f
    }
}
