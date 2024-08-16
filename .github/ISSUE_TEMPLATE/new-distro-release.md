---
name: ".NET Docker Release - New Linux Distro"
about: "Checklist for releasing .NET container images for a new or updated Linux distro"
title: ".NET Container Images Release - New Linux Distro - <distro name/version>"
labels: docker
assignees: lbussell
---

## ImageBuilder Tasks

- [ ] Ensure that the ImageBuilder supports the new distro version in the [code](https://github.com/dotnet/docker-tools/blob/main/src/Microsoft.DotNet.ImageBuilder/src/McrTagsMetadataGenerator.cs) to generate the correct README display name from the version specified in the manifest

## Nightly Branch Tasks

1. - [ ] Copy the Dockerfiles of the most recent version of that distro (or author new ones for an entirely new distro) and place them in a version-specific folder under their respective variants (runtime-deps, runtime, aspnet, sdk). Note: not all variants have a corresponding runtime-deps image.
1. - [ ] Modify the Dockerfiles as appropriate for any specific changes related to the new distro version
1. - [ ] Update [manifest.json](https://github.com/dotnet/dotnet-docker/blob/nightly/manifest.json) to reference the new set of Dockerfiles with the appropriate tags
      - [ ] For Alpine Linux, make sure to only update the floating tags according to our [policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). Only move the floating tags in the nightly branch if they should be updated in main for the next servicing release.
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

1. - [ ] After the product teams have signed off on the new distro, merge these changes to the main branch as part of the [release process](https://github.com/dotnet/release/blob/main/.github/ISSUE_TEMPLATE/dotnet-docker-servicing-release.md) for the next .NET release
      - [ ] For Alpine, [file an issue](https://github.com/dotnet/dotnet-docker/issues/new?body=In+the+MMMM+YYYY+servicing+release%2C+%5BAlpine+3.XX+container+images+were+published%5D%28link+to+announcement%29.+In+the+MMMM+YYYY+servicing+release%2C+all+Alpine+floating+tags+were+updated+to+target+Alpine+3.XX+instead+of+Alpine+3.XX-1+according+to+our+%5Btagging+policy%5D%28https%3A%2F%2Fgithub.com%2Fdotnet%2Fdotnet-docker%2Fblob%2Fmain%2Fdocumentation%2Fsupported-tags.md%29.%0D%0A%0D%0APer+the+%5B.NET+Docker+platform+support+policy%5D%28https%3A%2F%2Fgithub.com%2Fdotnet%2Fdotnet-docker%2Fblob%2Fmain%2Fdocumentation%2Fsupported-platforms.md%23linux%29%2C+Alpine+3.XX+images+will+no+longer+be+maintained+starting+on+YYYY-MM-DD.+This+issue+tracks+removing+those+Dockerfiles.%0D%0A%0D%0ARelated%3A+link+to+PR+adding+Alpine+3.XX&title=Remove+Alpine+3.XX+Dockerfiles) to update the floating `alpine` tag according to our [policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md).
1. - [ ] Create an announcement (example: [Alpine 3.10](https://github.com/dotnet/dotnet-docker/issues/1418)) unless the new distro is added only for pre-release versions in which the announcement would be incorporated in the pre-release notes.
