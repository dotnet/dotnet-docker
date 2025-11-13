---
name: ".NET Docker Release - New Linux Distro"
about: "Checklist for releasing .NET container images for a new or updated Linux distro"
title: ".NET Container Images Release - New Linux Distro - <distro name/version>"
labels: docker
assignees: lbussell
---

## ImageBuilder Tasks

- [ ] Ensure that the ImageBuilder supports the new distro version in the [code](https://github.com/dotnet/docker-tools/blob/main/src/ImageBuilder/McrTagsMetadataGenerator.cs) to generate the correct README display name from the version specified in the manifest

## Nightly Branch Tasks

1. - [ ] Copy the Dockerfiles of the most recent version of that distro (or author new ones for an entirely new distro) and place them in a version-specific folder under their respective variants (runtime-deps, runtime, aspnet, sdk). Note: not all variants have a corresponding runtime-deps image.
1. - [ ] Modify the Dockerfiles as appropriate for any specific changes related to the new distro version
1. - [ ] Update [manifest.json](https://github.com/dotnet/dotnet-docker/blob/nightly/manifest.json) to reference the new set of Dockerfiles with the appropriate tags
      - [ ] For Alpine Linux, make sure to only update the floating tags according to our [policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). Only move the floating tags in the nightly branch if they should be updated in main for the next servicing release.
1. - [ ] Update the [test data](https://github.com/dotnet/dotnet-docker/blob/nightly/tests/Microsoft.DotNet.Docker.Tests/TestData.cs) to include the new distro version
1. - [ ] Update the [tags metadata templates](https://github.com/dotnet/dotnet-docker/tree/main/eng/mcr-tags-metadata-templates) to include the new distro version
1. - [ ] Run the command to update the READMEs: `.\eng\readme-templates\Get-GeneratedReadmes.ps1`
1. - [ ] Inspect generated changes for correctness
1. - [ ] Consider whether sample Dockerfiles should be authored if this is a new distro and them to the [samples](https://github.com/dotnet/dotnet-docker/tree/main/samples)
1. - [ ] Run the command to build and test your changes: `.build-and-test.ps1 -OS <os>`
1. - [ ] Commit generated changes
1. - [ ] Create PR
1. - [ ] Get PR signoff
1. - [ ] Merge PR to nightly branch
1. - [ ] Wait for automatically queued CI build to finish on [dotnet-docker-nightly pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=359) (internal MSFT link)
1. - [ ] Confirm READMEs have been updated in [Docker Hub](https://hub.docker.com/r/microsoft/dotnet) and the [MAR Portal](https://mcr.microsoft.com/catalog?search=dotnet)

## Determining when to move a new distro to the main branch

According to our [policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md#operating-systems), distros can be moved to the main branch at any time.
However, in the interest of acting predictably and reducing risk, we should operate by the following guidelines:

- The new distro **MUST** be actively tested/validated by other .NET teams. At a minimum, the Runtime must be tested.
- The new distro **SHOULD** be in the `nightly` branch for at least 1 week prior to releasing it in the main branch.
- The new distro **SHOULD** be released to the `main` branch on their release date if possible.
  - New distros **may** be released with scheduled .NET servicing if more time is needed for validation.
  - New distros **may** release for different architectures/.NET versions at different times if there are blocking issues.
- Alpine images **SHOULD** be released along with scheduled .NET servicing, due to their frequency.

### Case studies

- **Alpine 3.19** - added to `main` after less than 1 week in `nightly`.
  - [Released on 2023-12-07](https://www.alpinelinux.org/posts/Alpine-3.19.0-released.html) (Thurs), two days after Patch Tuesday
  - [Added to `nightly` on 2024-01-03](https://redirect.github.com/dotnet/dotnet-docker/pull/5065), significantly delayed due to holidays.
  - Added to `main` on [2024-01-09](https://redirect.github.com/dotnet/dotnet-docker/discussions/5091), due to the significant delay between Alpine release and next .NET release. Releasing with 2024-02 Patch Tuesday was not desirable.
- **Ubuntu 24.04 "Noble"** - ARM32 release delayed, skipped for .NET 8
  - 2024-03-05 - [Added to `nightly`](https://redirect.github.com/dotnet/dotnet-docker/pull/5241)
  - 2024-04-25 - [24.04 Released](https://ubuntu.com/blog/canonical-releases-ubuntu-24-04-noble-numbat)
  - 2024-04-23 - Discovered issue blocking ARM32 release - [HTTPS requests fail on Ubuntu 24.04 Noble ARM32 due to bundled certs "NotTimeValid" error (dotnet/runtime#101444)](https://redirect.github.com/dotnet/runtime/issues/101444)
  - 2024-05-14 - [Noble images added to `main`](https://redirect.github.com/dotnet/dotnet-docker/discussions/5466) (May Patch Tuesday)
  - 2024-06-11 - [.NET 9 ARM32 Noble images added to `main`](https://redirect.github.com/dotnet/dotnet-docker/discussions/5557) (June Patch Tuesday) - allowed time for the issue to be fixed in .NET Runtime

## Main Branch Tasks

When it's time to move a new distro to the main branch, then do the following:

1. - [ ] Merge the new distro changes to the main branch as part of the [release process](https://github.com/dotnet/release/blob/main/.github/ISSUE_TEMPLATE/dotnet-docker-servicing-release.md) for the next .NET release
      - [ ] For Alpine, [file an issue](https://github.com/dotnet/dotnet-docker/issues/new?body=In+the+MMMM+YYYY+servicing+release%2C+%5BAlpine+3.XX+container+images+were+published%5D%28link+to+announcement%29.+In+the+MMMM+YYYY+servicing+release%2C+all+Alpine+floating+tags+were+updated+to+target+Alpine+3.XX+instead+of+Alpine+3.XX-1+according+to+our+%5Btagging+policy%5D%28https%3A%2F%2Fgithub.com%2Fdotnet%2Fdotnet-docker%2Fblob%2Fmain%2Fdocumentation%2Fsupported-tags.md%29.%0D%0A%0D%0APer+the+%5B.NET+Docker+platform+support+policy%5D%28https%3A%2F%2Fgithub.com%2Fdotnet%2Fdotnet-docker%2Fblob%2Fmain%2Fdocumentation%2Fsupported-platforms.md%23linux%29%2C+Alpine+3.XX+images+will+no+longer+be+maintained+starting+on+YYYY-MM-DD.+This+issue+tracks+removing+those+Dockerfiles.%0D%0A%0D%0ARelated%3A+link+to+PR+adding+Alpine+3.XX&title=Remove+Alpine+3.XX+Dockerfiles) to update the floating `alpine` tag according to our [policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md).
1. - [ ] Create announcement
      - [ ] If the new distro is added for pre-release versions only, include the announcement in the new preview release announcement.
      - [ ] If the new distro applies to in-support .NET versions, post a separate announcement. Example: [Alpine 3.20](https://github.com/dotnet/dotnet-docker/discussions/5556)
