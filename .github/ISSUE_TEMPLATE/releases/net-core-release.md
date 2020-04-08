# .NET Core Release

## Release Versions

_The set of .NET Core versions that are being released as a unit._

* runtime/SDK

## 1. Master Branch Tasks

1. - [ ] Merge appropriate commits from nightly branch.  Things to check:
      - [ ] New/EOL distro
      - [ ] Infrastructure changes
      - [ ] PowerShell version
      - [ ] Check for additional changes by diffing the master and nightly branches
      - [ ] &lt;add link to PR/commit&gt;
1. - [ ] Wait for .NET Core archive files (.zip, .tar.gz) to be available at blob storage location
1. - [ ] Run `update-dependencies` tool to update all the necessary files to reflect the specified .NET Core versions (run this command for each version being released):
      - [ ] `dotnet run --project .\eng\update-dependencies\update-dependencies.csproj --sdk-version <sdk> --runtime-version <runtime> --aspnet-version <runtime>`
1. - [ ] Inspect generated changes for correctness
1. - [ ] Commit generated changes
1. - [ ] Create PR
1. - [ ] Get PR signoff
1. - [ ] Merge PR
1. - [ ] Wait for changes to be mirrored to internal [dotnet-docker repo](https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet-docker) (internal MSFT link)
1. - [ ] Build images - Queue build stage of [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link) with variables:

      All releases:

          stages: build

      Servicing release:

          imageBuilder.pathArgs: --path '2.1*' --path '3.1*'

      Preview release:

          imageBuilder.pathArgs: --path '5.0*'
1. - [ ] Wait for NuGet packages to be published during release tic-toc
1. - [ ] Test and publish images - Queue build of [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link) with variables:

      All releases:

          stages: test;publish
          sourceBuildId: <Build ID from the build stage>

      Servicing release:

          imageBuilder.pathArgs: --path '2.1*' --path '3.1*'

      Preview release:

          imageBuilder.pathArgs: --path '5.0*'
1. - [ ] Confirm images have been ingested by MCR
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/_/microsoft-dotnet-core)

## 2. Sample Images (Not needed for Preview-only release)

1. - [ ] Confirm build for [dotnet-docker-samples](https://dev.azure.com/dnceng/internal/_build?definitionId=376) (internal MSFT link) was queued. This will be queued automatically by [dotnet-docker-tools-check-base-image-updates](https://dev.azure.com/dnceng/internal/_build?definitionId=536) when it detects that the product images have been updated (detection runs on a schedule). Alternatively, you can manually queue the samples build.
1. - [ ] Confirm sample images have been ingested by MCR
1. - [ ] Confirm README has been updated in Docker Hub for [microsoft-dotnet-core-samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/)

## 3. Nightly Branch Tasks (Not needed for Preview-only release)

1. - [ ] Merge master branch to nightly
1. - [ ] Run the same `update-dependencies` command(s) in nightly that were run for the master branch
1. - [ ] Inspect generated changes for correctness
1. - [ ] Commit generated changes
1. - [ ] Create PR
1. - [ ] Get PR signoff
1. - [ ] Merge PR
1. - [ ] Wait for automatically queued CI build to finish on [dotnet-docker-nightly pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=359) (internal MSFT link)
1. - [ ] Confirm images have been ingested by MCR
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/_/microsoft-dotnet-core-nightly)
