#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

$IsRunningOnUnix = $PSVersionTable.contains("Platform") -and $PSVersionTable.Platform -eq "Unix"
$DockerOS = docker version -f "{{ .Server.Os }}"

# Run Docker Build
Write-Output "## Running Docker build"
docker build --pull -t dotnetapp .

# Capture tests if Docker build fails
if ($LASTEXITCODE -ne 0)
{
    Write-Output "## Docker build failed. Attempting to collect test results."
    docker build --pull --target testrunner -t dotnetapp:test .
    
    if ($LASTEXITCODE -ne 0)
    {
        Write-Output "#### Something is very wrong. You should check."
    }
    else {
        if (!(Test-Path -Path "TestResults"))
        {
            Write-Output "#### Create TestResults folder"
            mkdir TestResults
        }

        Write-Output "#### Running container with dotnetapp:test image"

        if ($IsRunningOnUnix) {
            Write-Output "###### Environment: Unix containers"
            docker run --rm -v /"$(pwd)/"/TestResults:/app/tests/TestResults dotnetapp:test
        }
        elseif ($DockerOS -eq "linux") {
            Write-Output "###### Environment: Unix containers on Windows"
            docker run --rm -v C:\git\dotnet-docker\samples\dotnetapp\TestResults:/app/tests/TestResults dotnetapp:test        
        }
        else {
            Write-Output "###### Environment: Windows containers"
            docker run --rm -v C:\git\dotnet-docker\samples\dotnetapp\TestResults:C:\app\tests\TestResults dotnetapp:test
        }

        $testfiles = Get-ChildItem .\TestResults\*.trx

        Write-Output "#### Printing list of test files"
        Write-Output $testfiles
    }
}
else {
    Write-Output "## Docker image built: dotnetapp"
}
