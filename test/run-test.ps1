[cmdletbinding()]
param(
    [switch]$UseImageCache,
    [string]$Filter,
    [string]$Architecture
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Exec([scriptblock]$cmd, [string]$errorMessage = "Error executing command: " + $cmd) {
    & $cmd
    if ($LastExitCode -ne 0) {
        throw $errorMessage
    }
}

function Get-ActivePlatformImages([PSCustomObject]$manifestRepo, [string]$activeOS) {
    return $manifestRepo.Images |
        ForEach-Object { $_.Platforms } |
        Where-Object { $_.os -eq "$activeOS" } |
        Where-Object { ( [string]::IsNullOrEmpty($Architecture) -and -not [bool]($_.PSobject.Properties.name -match "architecture"))`
            -or ( [bool]($_.PSobject.Properties.name -match "architecture") -and $_.architecture -eq "$Architecture" ) }
}

function Get-RuntimeTag([string]$sdkDockerfilePath, [string]$runtimeType, [string]$activeOS, [PSCustomObject]$manifestRepo) {
    $runtimeDockerfilePath = $sdkDockerfilePath.Replace("sdk", $runtimeType)
    $platforms = Get-ActivePlatformImages $manifestRepo $activeOS |
        Where-Object { $_.Dockerfile -eq $runtimeDockerfilePath }
    return $manifestRepo.Name + ':' + ([array]($_.Tags | ForEach-Object { $_.PSobject.Properties }))[0].Name
}

if ($UseImageCache) {
    $optionalDockerBuildArgs = ""
}
else {
    $optionalDockerBuildArgs = "--no-cache"
}

$dirSeparator = [IO.Path]::DirectorySeparatorChar
$repoRoot = Split-Path -Parent $PSScriptRoot
$manifestPath = [IO.Path]::combine(${repoRoot}, "manifest.json")
$manifestRepo = (Get-Content $manifestPath | ConvertFrom-Json).Repos[0]
$testFilesPath = "$PSScriptRoot$dirSeparator"
$activeOS = docker version -f "{{ .Server.Os }}"

# update as appropriate (e.g. "2.0-sdk") whenever pre-release packages are referenced prior to being available on NuGet.org.
$includePrereleasePackageSourceForSdkTag = $null

if ($activeOS -eq "windows") {
    $containerRoot = "C:\"
    $platformDirSeparator = '\'
}
else {
    $containerRoot = "/"
    $platformDirSeparator = '/'
}

# Loop through each sdk Dockerfile in the repo and test the sdk and runtime images.
Get-ActivePlatformImages $manifestRepo $activeOS |
    Where-Object { [string]::IsNullOrEmpty($Filter) -or $_.dockerfile -like "$Filter*" } |
    Where-Object { $_.Dockerfile.Contains('sdk') } |
    ForEach-Object {
        $sdkTag = ([array]($_.Tags | ForEach-Object { $_.PSobject.Properties }))[0].Name
        $fullSdkTag = "$($manifestRepo.Name):${sdkTag}"

        $timeStamp = Get-Date -Format FileDateTime
        $appName = "app$timeStamp".ToLower()
        $buildImage = "sdk-build-$appName"
        $dotnetNewParam = "console --framework netcoreapp$($sdkTag.Split('-')[0].Substring(0,3))"

        $optionalRestoreParams = ""
        if ($sdkTag -like $includePrereleasePackageSourceForSdkTag) {
            $optionalRestoreParams = "-s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json -s https://api.nuget.org/v3/index.json"
        }

        Write-Host "----- Testing create, restore and build with $fullSdkTag with image $buildImage -----"
        Try {
            exec { (Get-Content ${testFilesPath}Dockerfile.test).
                    Replace("{image}", $fullSdkTag).
                    Replace("{dotnetNewParam}", $dotnetNewParam).
                    Replace("{optionalRestoreParams}", $optionalRestoreParams) `
                | docker build $optionalDockerBuildArgs -t $buildImage -
            }

            Write-Host "----- Running app built on $fullSdkTag -----"
            exec { docker run --rm $buildImage dotnet run }

            $framworkDepVol = "framework-dep-publish-$appName"
            Write-Host "----- Publishing framework-dependant app built on $fullSdkTag to volume $framworkDepVol -----"
            Try {
                exec { docker run --rm `
                    -v ${framworkDepVol}:"${containerRoot}volume" `
                    $buildImage `
                    dotnet publish -o ${containerRoot}volume
                }

                $fullRuntimeTag = Get-RuntimeTag $_.Dockerfile "runtime" $activeOS $manifestRepo
                Write-Host "----- Testing on $fullRuntimeTag with $sdkTag framework-dependent app -----"
                exec { docker run --rm `
                    -v ${framworkDepVol}":${containerRoot}volume" `
                    "$fullRuntimeTag" `
                    dotnet "${containerRoot}volume${platformDirSeparator}test.dll"
                }
            }
            Finally {
                docker volume rm $framworkDepVol
            }

            if ($activeOS -eq "linux") {
                $selfContainedImage = "self-contained-build-${buildImage}"
                Write-Host "----- Creating publish-image for self-contained app built on $fullSdkTag -----"
                Try {
                    exec { (Get-Content ${testFilesPath}Dockerfile.linux.publish).
                                Replace("{image}", $buildImage).
                                Replace("{optionalRestoreParams}", $optionalRestoreParams) `
                        | docker build $optionalDockerBuildArgs -t $selfContainedImage -
                    }

                    $selfContainedVol = "self-contained-publish-$appName"
                    Write-Host "----- Publishing self-contained published app built on $fullSdkTag to volume $selfContainedVol using image $selfContainedImage -----"
                    Try {
                        exec { docker run --rm `
                            -v ${selfContainedVol}":${containerRoot}volume" `
                            $selfContainedImage `
                            dotnet publish -r debian.8-x64 -o ${containerRoot}volume
                        }

                        $fullRuntimeDepsTag = Get-RuntimeTag $_.Dockerfile "runtime-deps" $activeOS $manifestRepo
                        Write-Host "----- Testing $fullRuntimeDepsTag with $sdkTag self-contained app -----"
                        exec { docker run -t --rm `
                            -v ${selfContainedVol}":${containerRoot}volume" `
                            $fullRuntimeDepsTag `
                            ${containerRoot}volume${platformDirSeparator}test
                        }
                    }
                    Finally {
                        docker volume rm $selfContainedVol
                    }
                }
                Finally {
                    docker image rm $selfContainedImage
                }
            }
        }
        Finally {
            docker image rm $buildImage
        }
    }
