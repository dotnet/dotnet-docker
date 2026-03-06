#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

<#
.SYNOPSIS
    Updates the internal Dockerfile baselines by running the snapshot tests and accepting the changes.

.DESCRIPTION
    This script automates the workflow described in CONTRIBUTING.md for verifying internal Dockerfiles.
    It runs the VerifyInternalDockerfilesOutput pre-build tests to generate updated baseline files,
    then accepts the changes by renaming .received.txt files to .approved.txt.

    Steps performed:
    1. Runs the pre-build tests filtered to VerifyInternalDockerfilesOutput.
    2. If the tests fail (baselines changed), accepts the new baselines.
    3. Displays the git diff of updated baseline files for review.

.EXAMPLE
    ./tests/update-internal-baselines.ps1
    Runs the internal Dockerfile tests and accepts any baseline changes.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = (Get-Item "$PSScriptRoot").Parent.FullName

Write-Host "`nStep 1: Running VerifyInternalDockerfilesOutput tests...`n" -ForegroundColor Cyan

$testFailed = $false
try {
    & "$repoRoot/tests/run-tests.ps1" `
        -Paths "*" `
        -TestCategories "pre-build" `
        -CustomTestFilter "VerifyInternalDockerfilesOutput"
}
catch {
    $testFailed = $true
    Write-Host "`nTests reported differences in internal Dockerfile baselines.`n" -ForegroundColor Yellow
}

$receivedFiles = Get-ChildItem `
    -Recurse `
    -Path "$PSScriptRoot/Microsoft.DotNet.Docker.Tests/Baselines" `
    -Filter "*.received.txt" `
    -ErrorAction SilentlyContinue

if (-not $receivedFiles -or $receivedFiles.Count -eq 0) {
    if ($testFailed) {
        Write-Host "Tests failed but no .received.txt files were found. Review test output for errors." -ForegroundColor Red
        exit 1
    }
    Write-Host "`nBaselines are up to date. No changes needed.`n" -ForegroundColor Green
    exit 0
}

Write-Host "`nFound $($receivedFiles.Count) baseline file(s) with changes:`n" -ForegroundColor Cyan
$receivedFiles | ForEach-Object { Write-Host "  $_" }

Write-Host "`nStep 2: Accepting baseline changes...`n" -ForegroundColor Cyan
& "$repoRoot/tests/accept-changes.ps1"
Write-Host "Accepted all baseline changes.`n" -ForegroundColor Green

Write-Host "Step 3: Reviewing changes...`n" -ForegroundColor Cyan
git -C $repoRoot --no-pager diff -- "$PSScriptRoot/Microsoft.DotNet.Docker.Tests/Baselines/"
Write-Host "`nDone. Review the diff above and commit the updated baselines if acceptable.`n" -ForegroundColor Green
