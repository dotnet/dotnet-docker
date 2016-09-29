[cmdletbinding()]
param(
   [string]$OS="windowsservercore"
)

Set-StrictMode -Version Latest
$ErrorActionPreference="Stop"

$dockerRepo="microsoft/dotnet"
$repoRoot = Split-Path -Parent $PSScriptRoot

# Maps development image versions to the corresponding runtime image version
$versionMappings=@{"1.0.0-preview2" = "1.0"}

if ($env:DEBUGTEST -eq $null) {
    $optionalDockerRunArgs="--rm"
}
else {
    $optionalDockerRunArgs=""
}

pushd $repoRoot

# Loop through each sdk Dockerfile in the repo.  If it has an entry in $versionMappings, then test the sdk, core, and onbuild images; if not, fail.
Get-ChildItem -Recurse -Filter Dockerfile | where DirectoryName -like "*\$OS" | foreach {
    $developmentImageVersion = $_.Directory.Parent.Name
    if ($versionMappings.ContainsKey($developmentImageVersion)) {
        $runtimeImageVersion = $versionMappings[$developmentImageVersion]
    }
    else {
        $runtimeImageVersion = $developmentImageVersion
    }

    $developmentTagBase="$($dockerRepo):$developmentImageVersion-$OS"
    $runtimeTagBase="$($dockerRepo):$runtimeImageVersion-$OS"

    $timeStamp = Get-Date -Format FileDateTime
    $appName="app$timeStamp"
    $appDir="${repoRoot}\.test-assets\${appName}"

    New-Item $appDir -type directory | Out-Null

    Write-Host "----- Testing $developmentTagBase-sdk -----"
    docker run -t $optionalDockerRunArgs -v "$($appDir):c:\$appName" -v "$repoRoot\test:c:\test" --name "sdk-test-$appName" --entrypoint powershell "$developmentTagBase-sdk" c:\test\create-run-publish-app.ps1 "c:\$appName"
    if (-NOT $?) {
        throw  "Testing $developmentTagBase-sdk failed"
    }

    Write-Host "----- Testing $runtimeTagBase-core -----"
    docker run -t $optionalDockerRunArgs -v "$($appDir):c:\$appName" --name "core-test-$appName" --entrypoint dotnet "$runtimeTagBase-core" "C:\$appName\publish\$appName.dll"
    if (-NOT $?) {
        throw  "Testing $runtimeTagBase-core failed"
    }

    Write-Host "----- Testing $developmentTagBase-onbuild -----"
    pushd $appDir
    $onbuildTag = "$appName-onbuild".ToLowerInvariant()
    New-Item -Name Dockerfile -Value "FROM $developmentTagBase-onbuild" | Out-Null
    docker build -t $onbuildTag .
    popd
    if (-NOT $?) {
        throw  "Failed building $onbuildTag"
    }

    docker run -t $optionalDockerRunArgs --name "onbuild-test-$appName" $onbuildTag
    if (-NOT $?) {
        throw "Testing $developmentTagBase-onbuild failed"
    }

    if ($env:DEBUGTEST -eq $null) {
        docker rmi $onbuildTag
        if (-NOT $?) {
            throw "Failed to delete $onbuildTag image"
        }
    }
}

popd
