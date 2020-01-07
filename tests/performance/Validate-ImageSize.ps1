#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

[cmdletbinding(
    DefaultParameterSetName = 'Validate'
)]
param(
    [switch]$PullImages,
    [string]$ImageBuilderCustomArgs = "--architecture '*'",
    [Parameter(ParameterSetName = 'Validate')]
    [switch]$BaselineIntegrityOnly,
    [Parameter(ParameterSetName = 'Update')]
    [switch]$UpdateBaselines,
    [Parameter(ParameterSetName = 'Update')]
    [switch]$UpdateAll
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

pushd $PSScriptRoot/../../
try {
    $manifestJson = Get-Content ./manifest.json | ConvertFrom-Json
    if ($manifestJson.Repos[0].Name.Contains("nightly")) {
        $branch = ".nightly"
    }
    
    $activeOS = docker version -f "{{ .Server.Os }}"
    $baselinePath = "./tests/performance/ImageSize$branch.$activeOS.json"
    $commandArgs = "$baselinePath $ImageBuilderCustomArgs"

    if ($PullImages) {
        $commandArgs += " --pull"
    }

    if ($UpdateBaselines) {
        $commandName = "updateImageSizeBaseline"
        if ($UpdateAll) {
            $commandArgs += " --all"
        }
    }
    else {
        $commandName = "validateImageSize"
        if ($BaselineIntegrityOnly) {
            $commandArgs += " --baseline-integrity-only"
        }
    }

    $onCommandExecuted = {
        param($ContainerName)
        if ($UpdateBaselines -and $activeOS -eq "linux") {
            Exec "docker cp ${ContainerName}:/repo/$baselinePath $baselinePath"
        }
    }

    ./eng/common/Invoke-ImageBuilder.ps1 "$commandName $commandArgs" -OnCommandExecuted $onCommandExecuted
}
finally {
    popd
}
