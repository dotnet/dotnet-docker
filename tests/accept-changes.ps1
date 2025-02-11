#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

<#
.SYNOPSIS
    Accepts or discards changes in baseline files for .NET Docker tests.

.DESCRIPTION
    This script processes baseline files in the Microsoft.DotNet.Docker.Tests/Baselines directory.
    If the -Discard switch is specified, it deletes all .received.txt files.
    Otherwise, it renames all .received.txt files to .approved.txt.

.PARAMETER Discard
    If specified, the script will discard (delete) all .received.txt files.
    If not specified, the script will rename all .received.txt files to .approved.txt.

.EXAMPLE
    .\accept-changes.ps1
    This will rename all .received.txt files to .approved.txt in the Baselines directory.

.EXAMPLE
    .\accept-changes.ps1 -Discard
    This will delete all .received.txt files in the Baselines directory.
#>

param ([switch]$Discard)

$files = Get-ChildItem `
    -Recurse `
    -Path "$PSScriptRoot/Microsoft.DotNet.Docker.Tests/Baselines" `
    -Filter "*.received.txt"

if ($Discard) {
    $files | Remove-Item
} else {
    $files | Move-Item -Force -Destination { $_.DirectoryName + "/" + ($_.Name -replace '\.received\.txt$', '.approved.txt') }
}
