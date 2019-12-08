#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
param(
  [parameter(Mandatory=$true)]  [System.IO.FileInfo]$path
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
$Nano = "nanoserver"

if ($DockerOS -eq "linux") {
    Log "Environment: Linux containers"
}
else {
    Log "Environment: Windows containers"
}



Foreach ($File in Get-ChildItem $path Dockerfile*) 
{

    if ($DockerOS -eq "linux" -And $File.Name.Contains($nano)) {
        Continue
    }
    else if ($DockerOS -eq "windows" -And $File.Name.Contains($nano)) {
    }
    else if ($File.Name -eq "Dockerfile") {    
    }
    else
    {
        Continue
    }

    Log "Building $File"
    docker build --pull -t TestBuildImage -f $File .
    docker rmi TestBuildImage

}

PrintElapsedTime
