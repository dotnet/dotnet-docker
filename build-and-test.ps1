[cmdletbinding()]
param(
    [switch]$UseImageCache
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$dirSeparator = [IO.Path]::DirectorySeparatorChar
$dockerRepo = (Get-Content "manifest.json" | ConvertFrom-Json).Repos[0].Name

if ($UseImageCache) {
    $optionalDockerBuildArgs = ""
}
else {
    $optionalDockerBuildArgs = "--no-cache"
}

$platform = docker version -f "{{ .Server.Os }}"

if ($platform -eq "windows") {
    $imageOs = "nanoserver"
}
else {
    $imageOs = "jessie"
}

pushd $PSScriptRoot

$tags = [System.Collections.ArrayList]@()
Get-ChildItem -Recurse -Filter Dockerfile |
    where {$_.DirectoryName.TrimStart($PSScriptRoot) -like "*$dirSeparator$imageOs*"} |
    # sort in descending order to ensure runtime-deps get built before runtime to satisfy dependency
    Sort-Object {$_.DirectoryName} -Descending |
    foreach {
        $tag = "${dockerRepo}:" +
            $_.DirectoryName.
                Replace("$PSScriptRoot$dirSeparator", '').
                Replace("$dirSeparator$imageOs", '').
                Replace($dirSeparator, '-')
        $tags.Add($tag) | Out-Null
        Write-Host "--- Building $tag from $($_.DirectoryName) ---"
        docker build $optionalDockerBuildArgs -t $tag $_.DirectoryName
        if (-NOT $?) {
            throw "Failed building $tag"
        }
    }

popd

./test/run-test.ps1 -UseImageCache:$UseImageCache

Write-Host "Tags built and tested:`n$($tags | Out-String)"
