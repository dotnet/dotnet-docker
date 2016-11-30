[cmdletbinding()]
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("win", "linux")]
    [string]$Platform
)

Set-StrictMode -Version Latest
$ErrorActionPreference="Stop"

$dockerRepo="microsoft/dotnet"
$dirSeparator = [IO.Path]::DirectorySeparatorChar
$repoRoot = Split-Path -Parent $PSScriptRoot

if ($Platform -eq "win") {
    $imageOs = "nanoserver"
    $tagSuffix = "-nanoserver"
    $testScriptSuffix = "ps1"
    $containerRoot = "C:\"
    $sdkRunArg = "powershell"
}
else {
    $imageOs = "debian"
    $tagSuffix = ""
    $testScriptSuffix = "sh"
    $containerRoot = "/"
    $sdkRunArg = ""
}

pushd $repoRoot

# Loop through each sdk Dockerfile in the repo and test the sdk and runtime images.
Get-ChildItem -Recurse -Filter Dockerfile |
    where DirectoryName -like "*${dirSeparator}${imageOs}${dirSeparator}sdk${dirSeparator}*" |
    foreach {
        $sdkTag = $_.DirectoryName.
                Replace("$repoRoot$dirSeparator", '').
                Replace("$dirSeparator$imageOs", '').
                Replace($dirSeparator, '-') +
            $tagSuffix

        $fullSdkTag="${dockerRepo}:${sdkTag}"
        $baseTag=$fullSdkTag.
            TrimEnd($tagSuffix).
            TrimEnd("-msbuild").
            TrimEnd("-projectjson").
            TrimEnd("-sdk")

        $timeStamp = Get-Date -Format FileDateTime
        $appName="app$timeStamp"
        $appDir="$repoRoot$dirSeparator.test-assets$dirSeparator$appName"

        New-Item $appDir -type directory | Out-Null

        Write-Host "----- Testing $fullSdkTag -----"
        docker run -t --rm `
            -v "${appDir}:${containerRoot}${appName}" `
            -v "${repoRoot}${dirSeparator}test:${containerRoot}test" `
            --name "sdk-test-$appName" `
            $fullSdkTag $sdkRunArg "${containerRoot}test${dirSeparator}create-run-publish-app.${testScriptSuffix}" "${containerRoot}${appName}" $sdkTag
        if (-NOT $?) {
            throw  "Testing $fullSdkTag failed"
        }

        Write-Host "----- Testing $baseTag-runtime$tagSuffix with $sdkTag app -----"
        docker run -t --rm `
            -v "${appDir}:${containerRoot}${appName}" `
            --name "runtime-test-$appName" `
            --entrypoint dotnet `
            "$baseTag-runtime$tagSuffix" `
            "${containerRoot}${appName}${dirSeparator}publish${dirSeparator}framework-dependent${dirSeparator}${appName}.dll"
        if (-NOT $?) {
            throw  "Testing $baseTag-runtime$tagSuffix failed"
        }

        if ($Platform -eq "linux") {
            Write-Host "----- Testing $baseTag-runtime-deps$tagSuffix with $sdkTag app -----"
            docker run -t --rm `
                -v "${appDir}:${containerRoot}${appName}" `
                --name "runtime-deps-test-$appName" `
                --entrypoint "${containerRoot}${appName}${dirSeparator}publish${dirSeparator}self-contained${dirSeparator}${appName}" `
                "$baseTag-runtime-deps$tagSuffix"
            if (-NOT $?) {
                throw  "Testing $baseTag-runtime-deps$tagSuffix failed"
            }
        }
    }

popd
