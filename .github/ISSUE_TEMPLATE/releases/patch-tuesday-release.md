# Patch Tuesday Release

## Tasks

1. - [ ] Run the [`Get-BaseImageStatus.ps1`](https://github.com/dotnet/dotnet-docker/blob/main/eng/common/Get-BaseImageStatus.ps1) script and wait until the Windows images have been updated as part of the Windows Patch Tuesday release process. This script will display when the dependent Windows images were last updated. Wait until all the images show that they have been recently updated. "Recently updated" amounts to be having been updated within the past week or so; images from a month ago should be considered to be the old version.

          ./eng/common/Get-BaseImageStatus.ps1 -Continuous
1. - [ ] Run [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link) with the following parameters:

          imageBuilder.pathArgs: --path '*nanoserver*' --path '*windowsservercore*'
1. - [ ] Confirm successful run of dotnet-docker pipeline
1. - [ ] Run [dotnet-docker-nightly pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=359) (internal MSFT link) with the following parameters:

          imageBuilder.pathArgs: --path '*nanoserver*' --path '*windowsservercore*'
1. - [ ] Confirm successful run of dotnet-docker-nightly pipeline
1. - [ ] Confirm run for [dotnet-docker-samples](https://dev.azure.com/dnceng/internal/_build?definitionId=376) (internal MSFT link) was queued. This will be queued automatically by [dotnet-docker-tools-check-base-image-updates](https://dev.azure.com/dnceng/internal/_build?definitionId=536) when it detects that the product images have been updated (detection runs on a schedule). Alternatively, you can manually queue the samples build.
1. - [ ] Confirm successful run of dotnet-docker-samples pipeline
