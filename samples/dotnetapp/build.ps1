#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# If this script fails, it is probably because docker drive sharing isn't enabled

function Log {
    Param ([string] $s)
    Write-Output "###### $s"
}

function Check {
    Param ([string] $s)
    if ($LASTEXITCODE -ne 0) { 
        Log s
        throw "$s failed"
    }
}

$IsRunningOnUnix = $PSVersionTable.contains("Platform") -and $PSVersionTable.Platform -eq "Unix"
$DockerOS = docker version -f "{{ .Server.Os }}"
$ImageName = "dotnetapp"
$TestImageName = "dotnetapp:test"

Log "Building docker image"
docker build --pull -t $ImageName .
Check "docker build"

Log "Building test docker image."
docker build --pull --target testrunner -t $TestImageName .
Check "docker build"

$TestResults = "TestResults"
$TestResultsDir = Join-Path $PSScriptRoot $TestResults
$TestResultsDirExists = (Test-Path -Path $TestResultsDir)

Log "Check if $TestResults directory exists: $TestResultsDirExists"

if (!$TestResultsDirExists) {
    Log "Create $TestResults folder"
    mkdir $TestResultsDir
    gci . TestResults -ad
}
Log $TestResultsDir

if ($DockerOS -eq "linux") {
    Log "Environment: Linux containers"
    docker run --rm -v ${TestResultsDir}:/app/tests/${TestResults} $TestImageName        
}
else {
    Log "Environment: Windows containers"
    docker run --rm -v ${TestResultsDir}:C:\app\tests\${TestResults} $TestImageName
}

$testfiles = gci $TestResultsDir *.trx | Sort-Object -Property LastWriteTime | select -last 1

Log "Docker image built: $ImageName"
Log "Test log file:"
Write-Output $testfiles
