# .NET Monitor Release

## Release Version

* &lt;version&gt;

## Main Branch Tasks

1. - [ ] Merge appropriate commits from nightly branch.  Things to check:
      - [ ] New/EOL distro
      - [ ] Infrastructure changes
      - [ ] Check for additional changes by diffing the main and nightly branches
      - [ ] &lt;add link to PR/commit&gt;
1. - [ ] Run `update-dependencies` tool to update all the necessary files to reflect the specified .NET versions (run this command for each version being released):
      - [ ] `dotnet run --project .\eng\update-dependencies\update-dependencies.csproj <major/minor version> --product-version monitor=<version> --channel-name <channel>`
1. - [ ] Inspect generated changes for correctness
1. - [ ] Commit generated changes
1. - [ ] Create PR
1. - [ ] Get PR signoff
1. - [ ] Merge PR
1. - [ ] Wait for changes to be mirrored to internal [dotnet-docker repo](https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet-docker) (internal MSFT link)
1.  - [ ] Build images - Queue build stage of [dotnet-docker pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=373) (internal MSFT link) with variables:

      `imageBuilder.pathArgs: --path 'src/*/3.1/alpine3.13/amd64' --path 'src/*/5.0/alpine3.13/amd64' --path 'src/*/6.0/alpine3.13/amd64' --path 'src/monitor/*'`
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/_/microsoft-dotnet-monitor)
