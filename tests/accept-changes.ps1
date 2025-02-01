#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

param (
    [switch]$Discard
)

$files = Get-ChildItem `
    -Recurse `
    -Path "$PSScriptRoot/Microsoft.DotNet.Docker.Tests/Baselines" `
    -Filter "*.received.txt"

if ($Discard) {
    $files | Remove-Item
} else {
    $files | Rename-Item -NewName { $_.Name -replace '\.received\.txt$', '.approved.txt' }
}
