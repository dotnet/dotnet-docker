Set-StrictMode -Version Latest
$ErrorActionPreference="Stop"

$dockerRepo="microsoft/dotnet"
$repoRoot = Split-Path -Parent $PSScriptRoot

if ($env:DEBUGTEST -eq $null) {
    $optionalDockerRunArgs="--rm"
}
else {
    $optionalDockerRunArgs=""
}

pushd $repoRoot

# Loop through each sdk Dockerfile in the repo and test the sdk and runtime images.
Get-ChildItem -Recurse -Filter Dockerfile | where DirectoryName -like "*\nanoserver\sdk\*" | foreach {
    $sdkTag = $_.DirectoryName.Replace($repoRoot, '').Replace("\nanoserver", '').TrimStart('\').Replace('\', '-') + "-nanoserver"

    $fullSdkTag="$($dockerRepo):$sdkTag"
    $baseTag="$fullSdkTag".Replace("-sdk", '').Replace("-msbuild", '').Replace("-projectjson", '').Replace("-nanoserver", '')

    $timeStamp = Get-Date -Format FileDateTime
    $appName="app$timeStamp"
    $appDir="${repoRoot}\.test-assets\${appName}"

    New-Item $appDir -type directory | Out-Null

    Write-Host "----- Testing $fullSdkTag -----"
    docker run -t $optionalDockerRunArgs -v "$($appDir):c:\$appName" -v "$repoRoot\test:c:\test" --name "sdk-test-$appName" --entrypoint powershell "$fullSdkTag" c:\test\create-run-publish-app.ps1 "c:\$appName" "${sdkTag}"
    if (-NOT $?) {
        throw  "Testing $full_sdk_tag failed"
    }

    Write-Host "----- Testing $baseTag-runtime-nanoserver -----"
    docker run -t $optionalDockerRunArgs -v "$($appDir):c:\$appName" --name "runtime-test-$appName" --entrypoint dotnet "$baseTag-runtime-nanoserver" "C:\$appName\publish\$appName.dll"
    if (-NOT $?) {
        throw  "Testing $baseTag-runtime failed"
    }
}

popd
