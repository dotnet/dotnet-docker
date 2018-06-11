#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

function Log {
    Param ([string] $s)
    Write-Output "###### $s"
}

$IsRunningOnUnix = $PSVersionTable.contains("Platform") -and $PSVersionTable.Platform -eq "Unix"
$DockerOS = docker version -f "{{ .Server.Os }}"
$ImageName = "dotnetapp"
$TestImageName = "dotnetapp:test"

Log "Running Docker build"
docker build --pull -t $ImageName .
if ($LASTEXITCODE -ne 0) { throw "## docker build failed" }

Log "Run tests and collect results."
docker build --pull --target testrunner -t $TestImageName .
if ($LASTEXITCODE -ne 0) { throw "## docker build failed" }

if (!(Test-Path -Path "$PSScriptRoot\TestResults")) {
    Log "Create TestResults folder"
    mkdir $PSScriptRoot\TestResults
}

if ($IsRunningOnUnix) {
    Log "Environment: Unix containers"
    docker run --rm -v $PSScriptRoot/TestResults:/app/tests/TestResults $TestImageName
}
elseif ($DockerOS -eq "linux") {
    Log "Environment: Unix containers on Windows"
    docker run --rm -v $PSScriptRoot\TestResults:/app/tests/TestResults $TestImageName        
}
else {
    Log "Environment: Windows containers"
    docker run --rm -v $PSScriptRoot\TestResults:C:\app\tests\TestResults $TestImageName
}

if ($LASTEXITCODE -ne 0) { Log "Tests failed" }


$testfiles = gci $PSScriptRoot\TestResults\*.trx | sort LastWriteTime | select -last 1

Log "Docker image built: $ImageName"
Log "Test file:"
Write-Output $testfiles
