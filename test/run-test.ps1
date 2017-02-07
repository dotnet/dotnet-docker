[cmdletbinding()]
param(
    [switch]$UseImageCache
)

function Exec([scriptblock]$cmd, [string]$errorMessage = "Error executing command: " + $cmd) {
    & $cmd
    if ($LastExitCode -ne 0) {
        throw $errorMessage
    }
}

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if ($UseImageCache) {
    $optionalDockerBuildArgs=""
}
else {
    $optionalDockerBuildArgs = "--no-cache"
}

$dockerRepo="microsoft/dotnet"
$dirSeparator = [IO.Path]::DirectorySeparatorChar
$repoRoot = Split-Path -Parent $PSScriptRoot
$platform = docker version -f "{{ .Server.Os }}"

if ($platform -eq "windows") {
    $imageOs = "nanoserver"
    $tagSuffix = "-nanoserver"
    $containerRoot = "C:\"
    $platformDirSeparator = '\'
}
else {
    $imageOs = "debian"
    $tagSuffix = ""
    $containerRoot = "/"
    $platformDirSeparator = '/'
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
        $appName="app$timeStamp".ToLower()

        $buildImage = "sdk-build-$appName"
        $dotnetNewParam = ""
        if ($sdkTag -like "*1.1-sdk-msbuild*") {
            $dotnetNewParam = "console --framework netcoreapp1.1"
        }
        elseif ($sdkTag -like "*1.0-sdk-msbuild*") {
            $dotnetNewParam = "console --framework netcoreapp1.0"
        }
        $dockerFilesPath = "${repoRoot}${dirSeparator}test${dirSeparator}"

        Write-Host "----- Testing create, restore and build with $fullSdkTag with image $buildImage -----"
        exec { (Get-Content ${dockerFilesPath}Dockerfile.test).Replace("{image}", $fullSdkTag).Replace("{dotnetNewParam}", $dotnetNewParam) `
            | docker build $optionalDockerBuildArgs -t $buildImage -
        }

        Write-Host "----- Running app built on $fullSdkTag -----"
        exec { docker run --rm $buildImage dotnet run }

        Try {
            $framworkDepVol = "framework-dep-publish-$appName"
            Write-Host "----- Publishing framework-dependant app built on $fullSdkTag to volume $framworkDepVol -----"
            exec { docker run --rm -v ${framworkDepVol}:"${containerRoot}volume" $buildImage dotnet publish -o ${containerRoot}volume }

            Write-Host "----- Testing on $baseTag-runtime$tagSuffix with $sdkTag framework-dependent app -----"
            exec { docker run --rm `
                -v ${framworkDepVol}":${containerRoot}volume" `
                --entrypoint dotnet `
                "$baseTag-runtime$tagSuffix" `
                "${containerRoot}volume${platformDirSeparator}test.dll"
            }
        }
        Finally {
            docker volume rm $framworkDepVol
        }

        if ($platform -eq "linux") {
            if ($sdkTag -like "*projectjson*") {
                $projectType = "projectjson"
            }
            else {
                $projectType = "msbuild"
            }

            $selfContainedImage = "self-contained-build-${buildImage}"
            Write-Host "----- Creating publish-image for self-contained app built on $fullSdkTag -----"
            exec {
                (Get-Content ${dockerFilesPath}Dockerfile.linux.${projectType}.publish).Replace("{image}", $buildImage) `
                    | docker build $optionalDockerBuildArgs -t $selfContainedImage -
            }

            Try {
                $selfContainedVol = "self-contained-publish-$appName"
                Write-Host "----- Publishing self-contained published app built on $fullSdkTag to volume $selfContainedVol using image $selfContainedImage -----"
                # REMARK: This structure seems to be required because of some PowerShell parameter passing weirdness
                if ($projectType -eq "projectjson") {
                    exec { docker run --rm `
                        -v ${selfContainedVol}":${containerRoot}volume" `
                        --entrypoint dotnet `
                        $selfContainedImage `
                        publish -o ${containerRoot}volume
                    }
                }
                else {
                    exec { docker run --rm `
                        -v ${selfContainedVol}":${containerRoot}volume" `
                        --entrypoint dotnet `
                        $selfContainedImage `
                        publish -r debian.8-x64 -o ${containerRoot}volume
                    }
                }

                Write-Host "----- Testing $baseTag-runtime-deps$tagSuffix with $sdkTag self-contained app -----"
                exec { docker run -t --rm -v ${selfContainedVol}":${containerRoot}volume" ${baseTag}-runtime-deps$tagSuffix ${containerRoot}volume${platformDirSeparator}test }
            }
            Finally {
                docker volume rm $selfContainedVol
            }
        }
    }

popd
