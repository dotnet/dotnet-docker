Set-StrictMode -Version Latest
$ErrorActionPreference="Stop"

$dockerRepo="microsoft/dotnet"
$repoRoot = Split-Path -Parent $PSScriptRoot

# Maps development image versions to the corresponding runtime image version
$versionMappings=@{"1.0.0-preview2" = "1.0"; "1.0.0-preview2.1" = "1.1.0-preview1"}

if ($env:DEBUGTEST -eq $null) {
    $optionalDockerRunArgs="--rm"
}
else {
    $optionalDockerRunArgs=""
}

pushd $repoRoot

# Loop through each sdk Dockerfile in the repo.  If it has an entry in $versionMappings, then test the sdk, runtime, and onbuild images; if not, fail.
Get-ChildItem -Recurse -Filter Dockerfile | where DirectoryName -like "*\nanoserver" | foreach {
    $developmentImageVersion = $_.Directory.Parent.Name
    if ($versionMappings.ContainsKey($developmentImageVersion)) {
        $runtimeImageVersion = $versionMappings[$developmentImageVersion]
    }
    else {
        $runtimeImageVersion = $developmentImageVersion
    }

    $developmentTagBase="$($dockerRepo):$developmentImageVersion-nanoserver"
    $runtimeTagBase="$($dockerRepo):$runtimeImageVersion-nanoserver"

    $timeStamp = Get-Date -Format FileDateTime
    $appName="app$timeStamp"
    $appDir="${repoRoot}\.test-assets\${appName}"

    New-Item $appDir -type directory | Out-Null

    Write-Host "----- Testing $developmentTagBase-sdk -----"
    docker run -t $optionalDockerRunArgs -v "$($appDir):c:\$appName" -v "$repoRoot\test:c:\test" --name "sdk-test-$appName" --entrypoint powershell "$developmentTagBase-sdk" c:\test\create-run-publish-app.ps1 "c:\$appName"
    if (-NOT $?) {
        throw  "Testing $developmentTagBase-sdk failed"
    }

    Write-Host "----- Testing $runtimeTagBase-runtime -----"
    docker run -t $optionalDockerRunArgs -v "$($appDir):c:\$appName" --name "runtime-test-$appName" --entrypoint dotnet "$runtimeTagBase-runtime" "C:\$appName\publish\$appName.dll"
    if (-NOT $?) {
        throw  "Testing $runtimeTagBase-runtime failed"
    }
}

popd
