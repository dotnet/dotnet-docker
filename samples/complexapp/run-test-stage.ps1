#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# If this script fails, it is probably because docker drive sharing isn't enabled

param(
    [System.IO.DirectoryInfo]$Path
)

$Time = [System.Diagnostics.Stopwatch]::StartNew()

function PrintElapsedTime {
    Log $([string]::Format("Elapsed time: {0}.{1}",$Time.Elapsed.Seconds,$Time.Elapsed.Milliseconds))
}

function Log {
    Param ([string] $s)
    Write-Output "###### $s"
}

function Check {
    Param ([string] $s)
    if ($LASTEXITCODE -ne 0) { 
        Log "Failed: $s"
        throw "Error case -- see failed step"
    }
}

$DockerOS = docker version -f "{{ .Server.Os }}"
$ImageName = "complexapp"
$TestImageName = "complexapp:test"
$TestStage = "test"

if (!$Path)
{
    $Path = "."
}

PrintElapsedTime
Log "Build application image"
docker build --pull -t $ImageName $Path
PrintElapsedTime
Check "docker build (application)"

Log "Build test runner image"
docker build --target $TestStage -t $TestImageName $Path
PrintElapsedTime
Check "docker build (test runner)"

$TestResults = "TestResults"
$TestResultsDir = Join-Path $PSScriptRoot $TestResults
$TestResultsDirExists = (Test-Path -Path $TestResultsDir)

Log "Check if $TestResults directory exists: $TestResultsDirExists"

if (!$TestResultsDirExists) {
    Log "Create $TestResults folder"
    mkdir $TestResultsDir
    Get-ChildItem . TestResults -ad
}
Log $TestResultsDir

Log "Run test container with test runner image"

if ($DockerOS -eq "linux") {
    Log "Environment: Linux containers"
    docker run --rm -v ${TestResultsDir}:/app/tests/${TestResults} $TestImageName        
}
else {
    Log "Environment: Windows containers"
    docker run --rm -v ${TestResultsDir}:C:\app\tests\${TestResults} $TestImageName
}

PrintElapsedTime

$testfiles = Get-ChildItem $TestResultsDir *.trx | Sort-Object -Property LastWriteTime | Select-Object -last 1

Log "Docker image built: $ImageName"
Log "Test log file:"
Write-Output $testfiles
