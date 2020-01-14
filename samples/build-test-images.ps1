#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
param(
  [parameter(Mandatory=$true)]  [System.IO.DirectoryInfo]$Path,
  [parameter(Mandatory=$false)] [bool]$RunApp
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
$dockerarch = docker version -f "{{ .Server.Arch }}"
$nano = "nanoserver"
$slim = "slim"
$totalCount = 0
$buildCount = 0
$runCount = 0

Log "Environment: $dockeros containers"
Log "Chip: $dockerarch"

Foreach ($file in Get-ChildItem $Path Dockerfile*) 
{
    $totalCount++
    if ($file.Name -eq "Dockerfile") {}
    elseif ($dockeros -eq "windows" -And $file.Name.Contains($nano)) {}
    elseif ($dockeros -eq "linux" -And $file.Name.Contains($nano)) {Continue}
    elseif ($dockeros -eq "windows") {Continue}
    
    # Self-contained apps must be built on matching architecture due to crossgen
    # This restriction will be lifted at some point
    if ($file.Name.Contains($slim) -And 
        (($dockerarch -eq "amd64" -And $file.Name.Contains("x64")) -or
         ($dockerarch -eq "arm64" -And $file.Name.Contains("arm64")) -or
         ($dockerarch -eq "arm32" -And $file.Name.Contains("arm32"))
        )
       ) {}
    elseif ($file.Name.Contains($slim)) {Continue}

    ## Limit building ARM32 

    $testimage = "testbuildimage"

    Log "Building $file on $dockeros + $dockerarch"
    docker build --pull -t $testimage -f $file $Path
    Check "Build image for $file"
    $buildCount++

    if ($RunApp)
    {
        if ($file.Name -eq "Dockerfile" -or
            ($dockerarch -eq "amd64" -And $file.Name.Contains("x64")) -or
            ($dockerarch -eq "arm64" -And $file.Name.Contains("arm64")) -or
            ($dockerarch -eq "arm32" -And $file.Name.Contains("arm32"))
        )
        {
            docker run --rm $testimage "Hello from $file"
            Check "Run image for $file"
            $runCount++
        }
    }
    docker rmi $testimage
}

Log "$totalCount Dockerfiles discovered"
Log "$buildCount images built"

if ($RunApp)
{
    Log "$runCount images run"
}

PrintElapsedTime
