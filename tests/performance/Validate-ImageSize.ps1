#!/usr/bin/env pwsh
param(
    [switch]$UpdateBaselines,
    [switch]$UseLocalImages,
    [string]$ImageBuilderCustomArgs = "--architecture '*'"
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
    $validateImageSizeArgs = "./tests/performance/ImageSize$branch.$activeOS.json $ImageBuilderCustomArgs"
    if ($UpdateBaselines) {
        $validateImageSizeArgs += " --update"
    }
    if (!$UseLocalImages) {
        $validateImageSizeArgs += " --pull"
    }

    ./eng/common/Invoke-ImageBuilder.ps1 "validateImageSize $validateImageSizeArgs"
}
finally {
    popd
}
