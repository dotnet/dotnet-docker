#!/usr/bin/env pwsh

function Log([string]$s) {
    Write-Output "###### $s"
}

function Check([string]$s) {
    if ($LASTEXITCODE -ne 0) {
        Log "Failed: $s"
        throw "Error case -- see failed step"
    }
}

function IsValidConfiguration()
{
    $DockerOS = docker version -f "{{ .Server.Os }}"
    if ($DockerOS -eq "windows") {
        if ($DockerFile.Contains("debian") -or $DockerFile.Contains("alpine"))
        {
            Log "Not supported on this OS: $Dockerfile"
            return $false
        }
    }
    else {
        if ($DockerFile.Contains("nanoserver")) {
            Log "Not supported on this OS: $Dockerfile"
            return $false
        }
    }

    if ($DockerFile.Contains("arm32")){
        Log "Not supported on this OS: $Dockerfile"
        return $false
    }

    return $true
}

function Build(
        [string] $BuildContext,
        [string] $Dockerfile){
    
    if (IsValidConfiguration -eq $false)
    {
        return
    }

    Log "Testing: $Dockerfile"
    $Random = Get-Random -minimum 100 -maximum 1000
    $Tag = "test$($Random)"
    docker build --pull -t $Tag -f $DockerFile $BuildContext
    Check "docker build failed"
    return $Tag
}

function BuildAndTest(
        [string] $BuildContext,
        [string] $Dockerfile){
    
    if (IsValidConfiguration -eq $false)
    {
        return
    }

    $Tag = Build $BuildContext $DockerFile
    docker run --rm -it $Tag
    Check "docker run failed"
    docker rmi -f $Tag
}

function TestAdHocImages()
{
    $Random = Get-Random -minimum 100 -maximum 1000
    $Dockerfile = Join-Path $BuildContext "Dockerfile"
    $Tag = "test$($Random)"
    docker build --pull -t $Tag -f $Dockerfile --target testunner $BuildContext
    Check "docker build failed"
    docker run --rm $Tag
    Check "docker run failed"
}



$BuildContext = Join-Path "." "dotnetapp"

gci $BuildContext -Filter Dockerfile* | ForEach-Object {BuildAndTest $BuildContext $_.FullName}

TestAdHocImages

