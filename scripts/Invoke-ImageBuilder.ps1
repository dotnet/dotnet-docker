#!/usr/bin/env pwsh
[cmdletbinding()]
param(
    [string]$ImageBuilderArgs
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Log {
    param ([string] $Message)

    Write-Output $Message
}

function Exec {
    param ([string] $Cmd)

    Log "Executing: '$Cmd'"
    Invoke-Expression $Cmd
    if ($LASTEXITCODE -ne 0) {
        throw "Failed: '$Cmd'"
    }
}

$windowsImageBuilder = 'microsoft/dotnet-buildtools-prereqs:image-builder-nanoserver-20190209204624'
$linuxImageBuilder = 'microsoft/dotnet-buildtools-prereqs:image-builder-debian-20190210044705'
$imageBuilderContainerName = "ImageBuilder-$(Get-Date -Format yyyyMMddhhmmss)"

pushd $PSScriptRoot/../
try {
    $activeOS = docker version -f "{{ .Server.Os }}"
    if ($activeOS -eq "linux") {
        # On Linux, ImageBuilder is run within a container.  The local repo is copied into a Docker volume
        # in order to support running with a remote Docker server.
        ./scripts/Invoke-WithRetry "docker pull $linuxImageBuilder"
        $repoVolume = "repo-$(Get-Date -Format yyyyMMddhhmmss)"
        Exec "docker create -v ${repoVolume}:/repo --name $imageBuilderContainerName $linuxImageBuilder"
        Exec "docker cp . ${imageBuilderContainerName}:/repo"
        Exec "docker container rm -f $imageBuilderContainerName"
        $imageBuilderCmd = "docker run --rm -v /var/run/docker.sock:/var/run/docker.sock -v ${repoVolume}:/repo -w /repo $linuxImageBuilder"
    }
    else {
        # On Windows, ImageBuilder is run locally due to limitations with running Docker client within a container.
        $imageBuilderFolder = ".Microsoft.DotNet.ImageBuilder"
        $imageBuilderCmd = [System.IO.Path]::Combine($imageBuilderFolder, "image-builder", "Microsoft.DotNet.ImageBuilder.exe")
        if (-not (Test-Path -Path "$imageBuilderCmd" -PathType Leaf)) {
            ./scripts/Invoke-WithRetry "docker pull $windowsImageBuilder"
            Exec "docker create --name $imageBuilderContainerName $windowsImageBuilder"
            New-Item -Path "$imageBuilderFolder" -ItemType Directory -Force
            Exec "docker cp ${imageBuilderContainerName}:/image-builder $imageBuilderFolder"
            Exec "docker container rm -f $imageBuilderContainerName"
        }
    }

    Exec "$imageBuilderCmd $ImageBuilderArgs"

    if ($activeOS -eq "linux") {
        Exec "docker volume rm -f $repoVolume"
    }
}
finally {
    popd
}
