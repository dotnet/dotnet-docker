# .NET Release

## Release Versions

_The set of .NET versions that are being released as a unit._

* runtime/SDK

## 1. Main Branch Tasks

1. - [ ] Merge appropriate commits from nightly branch.  Things to check:
      - [ ] New/EOL distro
      - [ ] Infrastructure changes
      - [ ] PowerShell version
      - [ ] Check for additional changes by diffing the main and nightly branches
      - [ ] &lt;add link to PR/commit&gt;
1. - [ ] Check whether a change has been made to a Dockerfile that is shared by multiple .NET versions. If a change has been made and the .NET versions that share that file are not being released at the same time, define a separate Dockerfile to isolate the change to the .NET version that's being released. Conversely, after a shared Dockerfile has diverged in such a way, it should be combined again into a shared Dockerfile when the other other .NET version is released. Shared Dockerfiles to check:
      - [ ] 3.1 runtime-deps shared with 5.0 and 6.0
1. - [ ] Wait for .NET archive files (.zip, .tar.gz) to be available at blob storage location
1. - [ ] Run `update-dependencies` tool to update all the necessary files to reflect the specified .NET versions (run this command for each version being released):
      - [ ] `./eng/Set-DotnetVersions.ps1 -ProductVersion <major/minor version> -SdkVersion <sdk> -RuntimeVersion <runtime> -AspnetVersion <aspnet>`
1. - [ ] Inspect generated changes for correctness
1. - [ ] Commit generated changes
1. - [ ] Create PR
1. - [ ] Get PR signoff
1. - [ ] Merge PR
1. - [ ] Wait for changes to be mirrored to internal [dotnet-docker repo](https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet-docker) (internal MSFT link)
1. - [ ] Build images - Queue build stage of [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link) with variables:

      All releases:

          stages: build

      Servicing release (exclude 5.0/6.0 if separate runtime-deps Dockerfiles were defined):

          noCache: true
          imageBuilder.pathArgs: --path 'src/*/2.1/*' --path 'src/*/3.1/*' --path 'src/*/5.0/*' --path 'src/*/6.0/*' --path 'src/monitor/*'

      Preview release (exclude 3.1 if separate runtime-deps Dockerfiles were defined):

          imageBuilder.pathArgs: --path 'src/*/3.1/*' --path 'src/*/5.0/*' --path 'src/*/6.0/*' --path 'src/monitor/*'
1. - [ ] Wait for NuGet packages to be published during release tic-toc
1. - [ ] Test and publish images - Queue build of [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link) with variables:

      All releases:

          stages: test;publish
          sourceBuildId: <Build ID from the build stage>

      Servicing release (exclude 5.0/6.0 if separate runtime-deps Dockerfiles were defined):

          imageBuilder.pathArgs: --path 'src/*/2.1/*' --path 'src/*/3.1/*' --path 'src/*/5.0/*' --path 'src/*/6.0/*' --path 'src/monitor/*'

      Preview release (exclude 3.1 if separate runtime-deps Dockerfiles were defined):

          imageBuilder.pathArgs: --path 'src/*/3.1/*' --path 'src/*/5.0/*' --path 'src/*/6.0/*' --path 'src/monitor/*'
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/_/microsoft-dotnet)

## 2. Sample Images (Not needed for Preview-only release)

1. - [ ] Confirm build for [dotnet-docker-samples](https://dev.azure.com/dnceng/internal/_build?definitionId=376) (internal MSFT link) was queued. This will be queued automatically by [dotnet-docker-tools-check-base-image-updates](https://dev.azure.com/dnceng/internal/_build?definitionId=536) when it detects that the product images have been updated (detection runs on a schedule). Alternatively, you can manually queue the samples build.
1. - [ ] Confirm sample images have been ingested by MCR
1. - [ ] Confirm README has been updated in Docker Hub for [microsoft-dotnet-samples](https://hub.docker.com/_/microsoft-dotnet-samples/)

## 3. Nightly Branch Tasks (Not needed for Preview-only release)

1. - [ ] Merge main branch to nightly
1. - [ ] Run the same `update-dependencies` command(s) in nightly that were run for the main branch
1. - [ ] Inspect generated changes for correctness
1. - [ ] Commit generated changes
1. - [ ] Create PR
1. - [ ] Get PR signoff
1. - [ ] Merge PR
1. - [ ] Wait for automatically queued CI build to finish on [dotnet-docker-nightly pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=359) (internal MSFT link)
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/_/microsoft-dotnet)
