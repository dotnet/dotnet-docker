# New Linux Distro Version

Distro: &lt;name/version&gt;

## ImageBuilder Tasks

- [ ] Ensure that the ImageBuilder supports the new distro version in the [code](https://github.com/dotnet/docker-tools/blob/main/src/Microsoft.DotNet.ImageBuilder/src/McrTagsMetadataGenerator.cs) to generate the correct README display name from the version specified in the manifest

## Nightly Branch Tasks

1. - [ ] Copy the Dockerfiles of the most recent version of that distro (or author new ones for an entirely new distro) and place them in a version-specific folder under their respective variants (runtime-deps, runtime, aspnet, sdk). Note: not all variants have a corresponding runtime-deps image.
1. - [ ] Modify the Dockerfiles as appropriate for any specific changes related to the new distro version
1. - [ ] Update [manifest.json](https://github.com/dotnet/dotnet-docker/blob/nightly/manifest.json) to reference the new set of Dockerfiles with the appropriate tags
      - [ ] Move any distro-specific floating tags to the newer version (e.g. `3.1-alpine`)
1. - [ ] Update the [test data](https://github.com/dotnet/dotnet-docker/blob/nightly/tests/Microsoft.DotNet.Docker.Tests/TestData.cs) to include the new distro version
1. - [ ] Update the [tags metadata templates](https://github.com/dotnet/dotnet-docker/tree/main/eng/mcr-tags-metadata-templates) to include the new distro version
1. - [ ] Run the command to update the READMEs: `.\eng\readme-templates\Get-GeneratedReadmes.ps1`
1. - [ ] Run the command to update the image size baseline file: `.\tests\performance\Validate-ImageSize.ps1 -UpdateBaselines`
1. - [ ] Inspect generated changes for correctness
1. - [ ] Consider whether sample Dockerfiles should be authored if this is a new distro and them to the [samples](https://github.com/dotnet/dotnet-docker/tree/main/samples)
1. - [ ] Run the command to build and test your changes: `.build-and-test.ps1 -OS <os>`
1. - [ ] Commit generated changes
1. - [ ] Create PR
1. - [ ] Get PR signoff
1. - [ ] Merge PR to nightly branch
1. - [ ] Wait for automatically queued CI build to finish on [dotnet-docker-nightly pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=359) (internal MSFT link)
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/_/microsoft-dotnet)

## Main Branch Tasks

1. - [ ] After the product teams have signed off on the new distro, merge these changes to the main branch as part of the [release process](dotnet-release.md) for the next .NET release
1. - [ ] Create an announcement (example: [Alpine 3.10](https://github.com/dotnet/dotnet-docker/issues/1418)) unless the new distro is added only for pre-release versions in which the announcement would be incorporated in the pre-release notes.
