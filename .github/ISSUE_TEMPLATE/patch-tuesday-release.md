---
name: Patch Tuesday Release
about: Tracks information for managing a Patch Tuesday update of .NET Core Windows
  images
title: Patch Tuesday Release

---

# Patch Tuesday Release

## Tasks
1. - [ ] Wait for the following Windows images to be have been updated as part of the Windows Patch Tuesday release process:

      - [ ] `mcr.microsoft.com/windows/nanoserver:1809`
      - [ ] `mcr.microsoft.com/windows/nanoserver:1809-arm32v7`
      - [ ] `mcr.microsoft.com/windows/nanoserver:1903`
      - [ ] `mcr.microsoft.com/windows/nanoserver:1909`
1. - [ ] Queue build of [dotnet-docker-framework pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=372) (internal MSFT link)
1. - [ ] Confirm images have been ingested by MCR
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/_/microsoft-dotnet-framework)
