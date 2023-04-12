# .NET Release

## Release Versions

_The set of .NET versions that are being released as a unit._

* .NET &lt;major/minor version&gt;
  * Runtime: &lt;full version&gt;
  * ASP.NET Core: &lt;full version&gt;
  * SDK: &lt;full version&gt;
  * Monitor: &lt;full version&gt;

## 1. Main Branch Tasks

1. - [ ] Merge appropriate commits from nightly branch on local repo. Identify what to merge by checking [commit history](https://github.com/dotnet/dotnet-docker/commits/nightly) since the last release. Things to check:
      - [ ] New/EOL distro
      - [ ] Infrastructure changes
      - [ ] PowerShell version
      - [ ] Check for additional changes by diffing the main and nightly branches
      - [ ] &lt;add link to PR/commit&gt;
1. - [ ] Check whether a change has been made to a Dockerfile that is shared by multiple .NET versions. If a change has been made and the .NET versions that share that file are not being released at the same time, define a separate Dockerfile to isolate the change to the .NET version that's being released. Conversely, after a shared Dockerfile has diverged in such a way, it should be combined again into a shared Dockerfile when the other other .NET version is released. Shared Dockerfiles to check:
      - [ ] 6.0 runtime-deps shared with 7.0
1. - [ ] Run `update-dependencies` tool to update all the necessary files to reflect the specified .NET versions (run this command for each version being released):
      - [ ] `./eng/Set-DotnetVersions.ps1 -ProductVersion <major/minor version> -SdkVersion <sdk> -RuntimeVersion <runtime> -AspnetVersion <aspnet>`
      - [ ] .NET Monitor has its own major/minor versioning scheme, so it is updated separately:
         - [ ] `./eng/Set-DotnetVersions.ps1 -ProductVersion <major/minor version> -MonitorVersion <version>`
1. - [ ] Wait for .NET archive files (.zip, .tar.gz) to be available at blob storage location
1. - [ ] Inspect generated changes for correctness
1. - [ ] Create PR
1. - [ ] Get PR approval
1. - [ ] Merge PR
1. - [ ] Wait for changes to be mirrored to internal [dotnet-docker repo](https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet-docker) (internal MSFT link)
1. - [ ] Run [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link) with variables:

      Servicing release:

          noCache: true

      If NuGet packages aren't published yet, you can split the pipeline up into two runs. First, run only the build stage with the variables:

          stages: build
      
      Then once NuGet packages are live, run the pipeline again with variables:

          stages: test;publish
          sourceBuildId: <Build ID from the build stage>
1. - [ ] Confirm successful pipeline run(s)
1. - [ ] Update [CBL Mariner docs on Engineering Hub](https://eng.ms/docs/products/mariner-linux/gettingstarted/containers/dotnet) with new Mariner tags by clicking the `Improve this Doc` button and editing the file in the repo.

## 2. Sample Images (Not needed for Preview-only release)

1. - [ ] Confirm run for [dotnet-docker-samples pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=376) (internal MSFT link) was queued. This will be queued automatically by [dotnet-docker-tools-check-base-image-updates](https://dev.azure.com/dnceng/internal/_build?definitionId=536) when it detects that the product images have been updated (detection runs on a schedule). Alternatively, you can manually queue the samples build.
1. - [ ] Confirm successful pipeline run

## 3. Nightly Branch Tasks (Not needed for Preview-only release)

1. - [ ] Merge main branch to nightly on local repo
1. - [ ] Run the same `Set-DotnetVersions.ps1` command(s) in nightly that were run for the main branch
1. - [ ] Inspect generated changes for correctness
1. - [ ] Create PR
1. - [ ] Get PR approval
1. - [ ] Merge PR
1. - [ ] Confirm CI build is automatically run for [dotnet-docker-nightly pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=359) (internal MSFT link)
1. - [ ] Confirm successful pipeline run
1. - [ ] If the release contains a preview, update the `channel` variable in the [dotnet-docker-update-dependencies pipeline](https://dev.azure.com/dnceng/internal/_apps/hub/ms.vss-build-web.ci-designer-hub?pipelineId=470) to point to the next release. You can see available channels and builds in [BarViz](https://maestro-prod.westus2.cloudapp.azure.com/). If there are no builds available for the next preview yet, drop the preview designation (e.g. `8.0.1xx`). When branding happens and dotnet/installer branches, then this variable will need to be updated again.
