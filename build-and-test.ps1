[cmdletbinding()]
param(
   [string]$OS="windowsservercore"
)

Set-StrictMode -Version Latest
$ErrorActionPreference="Stop"

$dockerRepo="microsoft/dotnet"

pushd $PSScriptRoot

Get-ChildItem -Recurse -Filter Dockerfile | where DirectoryName -like "*\$OS*" | sort DirectoryName | foreach {
    $tag = "$($dockerRepo):" + $_.DirectoryName.Replace($PSScriptRoot, '').TrimStart('\').Replace('\', '-') -replace "${OS}$", "${OS}-sdk"
    Write-Host "----- Building $tag -----"
    docker build --no-cache -t $tag $_.DirectoryName
    if (-NOT $?) {
        throw "Failed building $tag"
    }
}

./test/run-test.ps1 -OS $OS

popd
