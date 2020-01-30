---
name: ".NET Core Release"
about: Tracks information for managing a .NET Core release
title: ".NET Core Release"

---

# .NET Core Release

## Release Versions
_The set of .NET Core versions that are being released as a unit._

* runtime/SDK

## 1. Master Branch Tasks
1. - [ ] Merge appropriate commits from nightly branch (new/EOL distros, infra changes, etc):
      - [ ] commit link
1. - [ ] Wait for .NET Core archive files (.zip, .tar.gz) to be available at blob storage location
1. - [ ] Run `update-dependencies` tool to update all the necessary files to reflect the specified .NET Core versions:
      - [ ] `dotnet run --project .\eng\update-dependencies\update-dependencies.csproj --sdk-version <sdk> --runtime-version <runtime> --aspnet-version <runtime>`
1. - [ ] Inspect generated changes for correctness
1. - [ ] Commit generated changes
1. - [ ] Create PR
1. - [ ] Get PR signoff
1. - [ ] Merge PR
1. - [ ] Wait for changes to be mirrored to internal [dotnet-docker repo](https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet-docker) (internal MSFT link)
1. - [ ] Queue build stage of [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link) with variables:

      Servicing release:

          imageBuilder.pathArgs: --path '2.1*' --path '3.0*' --path '3.1*'
          stages: build
1. - [ ] Wait for NuGet packages to be published during release tic-toc
1. - [ ] Queue build of [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link) with variables:

      Servicing release:

          imageBuilder.pathArgs: --path '2.1*' --path '3.0*' --path '3.1*'
          stages: test;publish
1. - [ ] Confirm images have been ingested by MCR
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/_/microsoft-dotnet-core)

## 2. Nightly Branch Tasks
1. - [ ] Run the same command(s) as step 2 for the master branch
1. - [ ] Inspect generated changes for correctness
1. - [ ] Commit generated changes
1. - [ ] Create PR
1. - [ ] Get PR signoff
1. - [ ] Merge PR
1. - [ ] Wait for automatically queued CI build to finish on [dotnet-docker-nightly pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=359) (internal MSFT link)
1. - [ ] Confirm images have been ingested by MCR
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/_/microsoft-dotnet-core-nightly)
