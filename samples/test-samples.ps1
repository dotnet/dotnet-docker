#!/usr/bin/env pwsh

function Log([string]$s) {
    [console]::WriteLine("###### $s")
}

function Check([string]$s) {
    if ($LASTEXITCODE -ne 0) {
        Log "Failed: $s"
        throw "Error case -- see failed step"
    }
}

function IsValidConfiguration([string] $Dockerfile)
{
    $DockerOS = docker version -f "{{ .Server.Os }}"
    if ($DockerOS -eq "windows") {
        if ($DockerFile.Contains("debian") -or $DockerFile.Contains("alpine"))
        {
            Log "Not valid in this configuration"
            return $False
        }
    }
    else {
        if ($DockerFile.Contains("nanoserver")) {
            Log "Not valid in this configuration"
            return $False
        }
    }

    if ($DockerFile.Contains("arm")){
        Log "Not valid in this configuration"
        return $False
    }

    return $True
}

function Build(
        [string] $BuildContext,
        [string] $Dockerfile,
        [string] $Stage = "None"){
    
    $IsValid = IsValidConfiguration $Dockerfile
    Log "Build: $Dockerfile"
    if (-not $IsValid)
    {
        return
    }

    Log "Building"
    $Random = Get-Random -minimum 100 -maximum 1000
    $Tag = "test$($Random)"
    docker build --pull -t $Tag -f $DockerFile $BuildContext
    Check "docker build failed"
}

function BuildAndTest(
        [string] $BuildContext,
        [string] $Dockerfile,
        [string] $Stage = "None"){

    Log "BuildAndTest: $Dockerfile"
    $IsValid = IsValidConfiguration $Dockerfile
    if (-not $IsValid)
    {
        return
    }

    Log "Building"
    $Random = Get-Random -minimum 100 -maximum 1000
    $StageArgument = ""
    $Tag = "test$($Random)"
    if ($Stage -ne "None") {
        docker build --pull --target $Stage -t $Tag -f $DockerFile $BuildContext
    }
    else {
        docker build --pull -t $Tag -f $DockerFile $BuildContext
    }
    Check "docker build failed"
    Log "Testing"
    docker run --rm -it $Tag
    Check "docker run failed"
    docker rmi -f $Tag
}

$dotnetBuildContext = Join-Path "." "dotnetapp"
$aspnetBuildContext = Join-Path "." "aspnetapp"
# test dotnetapp
gci $dotnetBuildContext -Filter Dockerfile* | ForEach-Object {BuildAndTest $dotnetBuildContext $_.FullName}

# test dotnetapp "testrunner" stage
gci $dotnetBuildContext -Filter Dockerfile* | ForEach-Object {BuildAndTest $dotnetBuildContext $_.FullName "testrunner"}

# test aspnetapp
gci $aspnetBuildContext -Filter Dockerfile* | ForEach-Object {Build $aspnetBuildContext $_.FullName}
