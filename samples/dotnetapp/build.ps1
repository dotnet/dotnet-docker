#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

function Log {
    Param ([string] $s)
    Write-Output "###### $s"
}

function Check {
    Param ([string] $s)
    if ($LASTEXITCODE -ne 0) { throw "###### $s failed ######" }
}

$IsRunningOnUnix = $PSVersionTable.contains("Platform") -and $PSVersionTable.Platform -eq "Unix"
$DockerOS = docker version -f "{{ .Server.Os }}"
$ImageName = "dotnetapp"
$TestImageName = "dotnetapp:test"

Log "Building docker image"
docker build --pull -t $ImageName .
Check "docker build failed"

Log "Building test docker image."
docker build --pull --target testrunner -t $TestImageName .
Check "docker build failed"

$TestResultsName = "TestResults"
$TestResults = Join-Path $PSScriptRoot $TestResultsName
$TestResultsExists = (Test-Path -Path $TestResults)

Log "Check if $TestResultsName directory exists: $TestResultsExists"

if (!$TestResultsExists) {
    Log "Create $TestResultsName folder"
    mkdir $TestResults
    gci . TestResults -ad
}

if ($IsRunningOnUnix) {
    Log "Environment: Unix containers"
    docker run --rm -v ${TestResults}:/app/tests/${TestResultsName} $TestImageName
}
elseif ($DockerOS -eq "linux") {
    Log "Environment: Unix containers on Windows"
    docker run --rm -v ${TestResults}:/app/tests/${TestResultsName} $TestImageName        
}
else {
    Log "Environment: Windows containers"
    docker run --rm -v ${TestResults}:C:\app\tests\${TestResultsName} $TestImageName
}

$testfiles = gci $TestResults *.trx | Sort-Object -Property LastWriteTime | select -last 1

Log "Docker image built: $ImageName"
Log "Test log file:"
Write-Output $testfiles
