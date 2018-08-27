#!/usr/bin/env pwsh

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Log {
    param ([string] $Message)

    Write-Output "###### $Message"
}

function Exec {
    param ([string] $Cmd)

    Log "$Cmd"
    Invoke-Expression $Cmd
    if ($LASTEXITCODE -ne 0) {
        throw "Failed: $Cmd"
    }
}

function Get-DockerfileItem {
    param ([string] $SampleType)

    $exclusions = [System.Collections.ArrayList]@()
    $dockerOS = docker version -f "{{ .Server.Os }}"
    if ($dockerOS -eq "windows") {
        $null = $exclusions.Add("*debian*")
        $null = $exclusions.Add("*alpine*")
    }
    else {
        $null = $exclusions.Add("*nanoserver*")
    }

    $dockerArch = docker info -f "{{ .Architecture }}"
    if ($dockerArch -eq "x86_64") {
        $null = $exclusions.Add("*arm*")
    }
    else {
        $null = $exclusions.Add("*x64*")
    }

    $dockerfilePath = [io.path]::combine($PSScriptRoot, $SampleType, "Dockerfile*")
    Get-ChildItem -Path $dockerfilePath -Exclude $exclusions
}

function Validate_Dockerfile {
    param ([string] $Dockerfile, [string] $Stage = $null, [switch] $SkipRunning)

    Log "Validating Dockerfile: $Dockerfile"

    $tag = "test" + [DateTime]::Now.ToString("yyyyMMdd-HHmmss")
    $buildContext = [System.IO.Path]::GetDirectoryName($Dockerfile)
    $optionalBuildArgs = ""
    if ($Stage) {
        $optionalBuildArgs += "--target $Stage"
    }

    Exec "docker build --pull -t $tag -f $DockerFile $optionalBuildArgs $buildContext"

    if (!$SkipRunning) {
        Exec "docker run --rm -it $tag"
    }

    docker rmi -f $tag
}

# test dotnetapp samples
Get-DockerfileItem "dotnetapp" | ForEach-Object {
    Validate_Dockerfile $_.FullName
    Validate_Dockerfile $_.FullName "testrunner"
}

# test aspnetapp samples
Get-DockerfileItem "aspnetapp" | ForEach-Object { Validate_Dockerfile $_.FullName -SkipRunning }
