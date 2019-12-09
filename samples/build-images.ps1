#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
param(
  [parameter(Mandatory=$true)]  [System.IO.FileInfo]$path,
  [parameter(Mandatory=$false)] [bool]$runapp
)


$time = [System.Diagnostics.Stopwatch]::StartNew()

function PrintElapsedTime {
    Log $([string]::Format("Elapsed time: {0}.{1}",$time.Elapsed.Seconds,$time.Elapsed.Milliseconds))
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

$dockeros = docker version -f "{{ .Server.Os }}"
$dockerarch = (docker version -f "{{ .Server.Arch }}")
$nano = "nanoserver"
$totalCount = 0
$buildCount = 0
$runCount = 0

Log "Environment: $dockeros containers"
Log "Chip: $dockerarch"

Foreach ($file in Get-ChildItem $path Dockerfile*) 
{
    $totalCount++
    if ($file.Name -eq "Dockerfile") {}
    elseif ($dockeros -eq "windows" -and ($file.Name.Contains($nano))) {}
    elseif ($dockeros -eq "linux" -and $file.Name.Contains($nano)) {
        Continue
    }

    $testimage = "testbuildimage"

    Log "Building $file"
    docker build --pull -t $testimage -f $file $path
    Check "Build image for $file"
    $buildCount++

    if ($runapp)
    {
        if ($file.Name -eq "Dockerfile" -or
            ($dockerarch -eq "amd64" -And $file.Name.Contains("x64")) -or
            ($dockerarch -eq "arm64" -And $file.Name.Contains("arm64")) -or
            ($dockerarch -eq "arm32" -And $file.Name.Contains("arm32"))
        )
        {
            docker run --rm -it $testimage "Hello from $file"
            $runCount++
        }

        Check "Run image for $file"
    }
    docker rmi $testimage
}

Log "$totalCount Dockerfiles discovered"
Log "$buildCount images built"

if ($runapp)
{
    Log "$runCount images run"
}

PrintElapsedTime
