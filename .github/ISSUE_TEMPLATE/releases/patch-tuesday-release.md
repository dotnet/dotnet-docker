# Patch Tuesday Release

## Tasks
1. - [ ] Wait for the following Windows images to have been updated as part of the Windows Patch Tuesday release process:

      - [ ] `mcr.microsoft.com/windows/nanoserver:1809`
      - [ ] `mcr.microsoft.com/windows/nanoserver:1809-arm32v7`
      - [ ] `mcr.microsoft.com/windows/nanoserver:1903`
      - [ ] `mcr.microsoft.com/windows/nanoserver:1909`
1. - [ ] Queue build of [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link)
1. - [ ] Queue build of [dotnet-docker-nightly pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=359) (internal MSFT link)
1. - [ ] Confirm images have been ingested by MCR
1. - [ ] Confirm `Last Modified` field has been updated in Docker Hub for [microsoft-dotnet-core](https://hub.docker.com/_/microsoft-dotnet-core)
1. - [ ] Confirm `Last Modified` field has been updated in Docker Hub for [microsoft-dotnet-core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly)
1. - [ ] Confirm build for [dotnet-docker-samples](https://dev.azure.com/dnceng/internal/_build?definitionId=376) (internal MSFT link) was queued. This will be queued automatically by [dotnet-docker-tools-check-base-image-updates](https://dev.azure.com/dnceng/internal/_build?definitionId=536) when it detects that the product images have been updated (detection runs on a schedule). Alternatively, you can manually queue the samples build.
1. - [ ] Confirm sample images have been ingested by MCR
1. - [ ] Confirm `Last Modified` field has been updated in Docker Hub for [microsoft-dotnet-core-samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/)
1. - [ ] Reply to .NET Release team with a status update email
