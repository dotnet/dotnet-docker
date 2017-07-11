[cmdletbinding()]
param(
    [switch]$UseImageCache,
    [string]$Filter
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Exec([scriptblock]$cmd, [string]$errorMessage = "Error executing command: " + $cmd) {
    & $cmd
    if ($LastExitCode -ne 0) {
        throw $errorMessage
    }
}

function Get-ActivePlatformImages([PSCustomObject]$manifestRepo, [string]$platform) {
    return $manifestRepo.Images |
        ForEach-Object { $_.Platforms } |
        Where-Object { [bool]($_.PSobject.Properties.name -match $platform) }
}

function Get-RuntimeTag([string]$sdkDockerfilePath, [string]$runtimeType, [string]$platform, [PSCustomObject]$manifestRepo) {
    $runtimeDockerfilePath = $sdkDockerfilePath.Replace("sdk", $runtimeType)
    $platforms = Get-ActivePlatformImages $manifestRepo $platform |
        Where-Object { $_.$platform.Dockerfile -eq $runtimeDockerfilePath }
    return $manifestRepo.Name + ':' + $platforms[0].$platform.Tags[0]
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
$platform = docker version -f "{{ .Server.Os }}"

# update as appropriate (e.g. "2.0-sdk") whenever pre-release packages are referenced prior to being available on NuGet.org.
$includePrereleasePackageSourceForSdkTag = $null

if ($platform -eq "windows") {
    $containerRoot = "C:\"
    $platformDirSeparator = '\'
}
else {
    $containerRoot = "/"
    $platformDirSeparator = '/'
}

# Loop through each sdk Dockerfile in the repo and test the sdk and runtime images.
Get-ActivePlatformImages $manifestRepo $platform |
    Where-Object { [string]::IsNullOrEmpty($Filter) -or $_.$platform.dockerfile -like "$Filter*" } |
    Where-Object { $_.$platform.Dockerfile.Contains('sdk') } |
    ForEach-Object {
        $sdkTag = $_.$platform.Tags[0]
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

                $fullRuntimeTag = Get-RuntimeTag $_.$platform.Dockerfile "runtime" $platform $manifestRepo
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

            if ($platform -eq "linux") {
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

                        if ($sdkTag -like "2.0*-sdk*") {
                            # Temporary workaround https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/dogfooding.md#option-2-self-contained
                            exec { docker run --rm `
                                -v ${selfContainedVol}":${containerRoot}volume" `
                                $selfContainedImage `
                                chmod u+x ${containerRoot}volume${platformDirSeparator}test
                            }
                        }

                        $fullRuntimeDepsTag = Get-RuntimeTag $_.$platform.Dockerfile "runtime-deps" $platform $manifestRepo
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
