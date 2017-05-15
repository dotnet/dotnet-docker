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

$dirSeparator = [IO.Path]::DirectorySeparatorChar
$repoRoot = Split-Path -Parent $PSScriptRoot
$manifestPath = [IO.Path]::combine(${repoRoot}, "manifest.json")
$dockerRepo = (Get-Content $manifestPath | ConvertFrom-Json).DockerRepo
$testFilesPath = "$PSScriptRoot$dirSeparator"
$platform = docker version -f "{{ .Server.Os }}"

# update as appropriate (e.g. "2.0-sdk") whenever pre-release packages are referenced prior to being available on NuGet.org.
$includePrereleasePackageSourceForSdkTag = $null

if ($platform -eq "windows") {
    $imageOs = "nanoserver"
    $containerRoot = "C:\"
    $platformDirSeparator = '\'
}
else {
    $imageOs = "jessie"
    $containerRoot = "/"
    $platformDirSeparator = '/'
}

# Loop through each sdk Dockerfile in the repo and test the sdk and runtime images.
Get-ChildItem -Path $repoRoot -Recurse -Filter Dockerfile |
    where DirectoryName -like "*${dirSeparator}sdk${dirSeparator}${imageOs}" |
    foreach {
        $sdkTag = $_.DirectoryName.
                Replace("$repoRoot$dirSeparator", '').
                Replace("$dirSeparator$imageOs", '').
                Replace($dirSeparator, '-')
        $fullSdkTag = "${dockerRepo}:${sdkTag}"

        $timeStamp = Get-Date -Format FileDateTime
        $appName = "app$timeStamp".ToLower()
        $buildImage = "sdk-build-$appName"
        $dotnetNewParam = "console --framework netcoreapp$($sdkTag.Split('-')[0])"

        Write-Host "----- Testing create, restore and build with $fullSdkTag with image $buildImage -----"
        Try {
            exec { (Get-Content ${testFilesPath}Dockerfile.test).Replace("{image}", $fullSdkTag).Replace("{dotnetNewParam}", $dotnetNewParam) `
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

                $runtimeTag = $fullSdkTag.Replace("sdk", "runtime")
                Write-Host "----- Testing on $runtimeTag with $sdkTag framework-dependent app -----"
                exec { docker run --rm `
                    -v ${framworkDepVol}":${containerRoot}volume" `
                    "$runtimeTag" `
                    dotnet "${containerRoot}volume${platformDirSeparator}test.dll"
                }
            }
            Finally {
                docker volume rm $framworkDepVol
            }

            if ($platform -eq "linux") {
                $selfContainedImage = "self-contained-build-${buildImage}"
                $optionalRestoreParams = ""
                if ($sdkTag -like $includePrereleasePackageSourceForSdkTag) {
                    $optionalRestoreParams = "-s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json -s https://api.nuget.org/v3/index.json"
                }

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

                        if ($sdkTag -like "2.0-sdk") {
                            # Temporary workaround https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/dogfooding.md#option-2-self-contained
                            exec { docker run --rm `
                                -v ${selfContainedVol}":${containerRoot}volume" `
                                $selfContainedImage `
                                chmod u+x ${containerRoot}volume${platformDirSeparator}test
                            }
                        }

                        $runtimeDepsTag = $fullSdkTag.Replace("sdk", "runtime-deps")
                        Write-Host "----- Testing $runtimeDepsTag with $sdkTag self-contained app -----"
                        exec { docker run -t --rm `
                            -v ${selfContainedVol}":${containerRoot}volume" `
                            $runtimeDepsTag `
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
