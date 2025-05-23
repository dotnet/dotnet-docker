# New .NET Version Release Lifecycle

This document is a collection of tasks that need to be completed over the lifecycle of a .NET release.
Each top-level checkbox represents an issue that should be filed.
File all release and pre-release issues at once when a new .NET version is in Alpha.
File an issue for removing an EOL .NET version *after* its last release, so that it can be removed before the next release.
Add due dates to each of the issues so that we don't lose track of them.

## Breaking changes

- For each breaking change made throughout development, file a new issue using the ".NET breaking change" template in the [dotnet/docs repo](https://github.com/dotnet/docs/issues/new/choose).
- List any breaking change issues created below:

## Alpha

- [ ] Create a new Milestone in this repo for the new .NET version
  - [ ] Update the `https://aka.ms/netcontainers-roadmap` short link to the new milestone. You can do this from [aka.ms](https://aka.ms/). It is linked to from [dotnet/core/roadmap.md](https://github.com/dotnet/core/blob/main/roadmap.md).
- [ ] Add new .NET version images to `nightly` branch
  - [ ] Create new images
    - [ ] Add entries for the new .NET versions in `manifest.versions.json`
    - [ ] Add new images for the new .NET version in each repo in `manifest.json`
      - [ ] Move `latest` tags to the new .NET version
      - [ ] [Floating tags should have a `-preview` suffix](https://github.com/dotnet/dotnet-docker/issues/4117). For example: `9.0-preview`.
      - For fixed tags, the `-alpha.1` suffixes that is part of the .NET version sufficient and no additional suffix is necessary
    - Adjust existing templates or add new templates in `eng/dockerfile-templates/` if necessary
    - If there are no major changes between .NET versions, the new images can share `runtime-deps` layers with the previous .NET version
    - Include only the latest/LTS version of each operating system according to our [supported operating systems documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md#operating-systems).
      - If adding a new OS version, follow the guidelines in [new-distro-release.md](./new-distro-release.md)
    - If PowerShell is not yet supported or functional on the new .NET version, file an issue at [PowerShell/PowerShell](https://github.com/PowerShell/PowerShell/issues) or link to an existing issue, exclude PowerShell from the new SDK images, and [file an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose) to re-enable PowerShell in .NET Docker images
  - [ ] Update tests
    - [ ] Add new version info to [ImageVersion.cs](https://github.com/dotnet/dotnet-docker/blob/nightly/tests/Microsoft.DotNet.Docker.Tests/ImageVersion.cs)
    - [ ] Update the [test data](https://github.com/dotnet/dotnet-docker/blob/nightly/tests/Microsoft.DotNet.Docker.Tests/TestData.cs) to include the new distro version
    - If various tests are not yet functional on the new .NET version, file issues on product team repos as necessary, disable the test with a link to the issue, and file an issue in the dotnet/dotnet-docker repo to re-enable the test
  - [ ] Update readmes
    - [ ] Update the [MCR tags metadata templates](https://github.com/dotnet/dotnet-docker/tree/main/eng/mcr-tags-metadata-templates) to include the new distro version
      - The new tags should be in a preview sub-table, like so: <https://github.com/dotnet/dotnet-docker/blob/8fc28db4f706d81a1fd075f2c6b2ca514ae75c84/eng/mcr-tags-metadata-templates/aspnet-tags.yml#L1-L3>

## Preview 1

- [ ] Add new .NET version to featured tags in `nightly` branch ([Example PR](https://github.com/dotnet/dotnet-docker/pull/5157))
- [ ] Merge new .NET version images from `nightly` branch to `main`

## RC1

- [ ] Switch to non-preview tagging pattern for new .NET version (<https://github.com/dotnet/dotnet-docker/issues/4117>)
  - [ ] Remove preview suffix from tags in `manifest.json` and/or `manifest.versions.json`
  - [ ] Move new version tags out of preview sub-tables in [MCR tags metadata templates](https://github.com/dotnet/dotnet-docker/tree/main/eng/mcr-tags-metadata-templates)
  - [ ] In [readme templates](https://github.com/dotnet/dotnet-docker/blob/main/eng/readme-templates/FeaturedTags.md):
    - [ ] Switch Featured Tags from "(Preview)" to "(Release Candidate)" in `nightly` branch
    - [ ] Add new .NET Version to Featured Tags as "(Release Candidate)" in `main` branch
- [ ] Move .NET Docker projects to the new .NET version's TFM
  - [Example issue](https://github.com/dotnet/docker-tools/issues/1181)
- [ ] Update samples to new .NET version
  - Consider creating new samples to feature new .NET or .NET Docker features
  - [ ] Update documentation as necessary to reference the new .NET version
  - [ ] Update IIS sample URL to preview version of `dotnet-hosting-win.exe` (`/dotnet/<version>/preview/`): <https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.windowsservercore-iis#L19>.
- [ ] Review supported OS versions for new .NET version before release
  - Include only the latest/LTS version of each operating system according to our [supported operating systems documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md#operating-systems).

## GA

In this repo:

- [ ] Move `latest` tags in `manifest.json` to the new version
- [ ] Update documentation as necessary to reference the new .NET version
- [ ] Update IIS sample URL to final version of `dotnet-hosting-win.exe` (remove "preview" from the URL): <https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.windowsservercore-iis#L19>.
- [ ] Remove suffix "(Release Candidate)" from Featured Tags and replace with "(Long-Term Support)" or "(Standard Support)"

In other repos:

- [ ] Add the new .NET version to the .NET Framework SDK image
  - [ ] <https://github.com/microsoft/dotnet-framework-docker/blob/6a3c9d048f75c1e69c0e1059564cd56a90bf9e3c/eng/dockerfile-templates/sdk/Dockerfile#L56-L59>
  - [ ] <https://github.com/microsoft/dotnet-framework-docker/blob/58a18cf958883c6a67ae888854bd9bb09f085e83/eng/dockerfile-templates/sdk/Dockerfile.ltsc2016#L80-L81>

Create announcement:

- [ ] Post a new announcement on the [GitHub Discussions page](https://github.com/dotnet/dotnet-docker/discussions/new?category=announcements) page and pin the announcement
  - Include new .NET Docker features and breaking changes and defer to other official .NET documentation/announcements for other features
  - [Example announcement](https://github.com/dotnet/dotnet-docker/discussions/4995)

## EOL

- [ ] Remove EOL .NET version from `main` and `nightly` branches (one issue) on the EOL date (or the day after, if necessary)
  - [ ] Remove all entries for the EOL .NET version in:
    - [ ] `manifest.json`
    - [ ] `manifest.versions.json`
    - [ ] [ImageVersion.cs](/tests/Microsoft.DotNet.Docker.Tests/ImageVersion.cs)
    - [ ] [TestData.cs](/tests/Microsoft.DotNet.Docker.Tests/TestData.cs)
    - [ ] [mcr-tags-metadata-templates](/eng/mcr-tags-metadata-templates)
    - [ ] [Featured tags](/eng/readme-templates/FeaturedTags.md)
  - [ ] Delete the Dockerfiles
  - [ ] Search for and simplify conditions including the EOL .NET Version in the Dockerfile templates and tests
  - [ ] Update the path variable appropriately for the [dotnet-docker-nightly-pr-no-cache](https://dev.azure.com/dnceng-public/public/_build?definitionId=184) pipeline so that it continues to target active Dockerfiles.
  - [ ] Replace all references to the EOL .NET version in documentation with a new .NET version
  - [ ] Remove the EOL .NET version from the .NET Framework SDK images
    - [ ] <https://github.com/microsoft/dotnet-framework-docker/blob/6a3c9d048f75c1e69c0e1059564cd56a90bf9e3c/eng/dockerfile-templates/sdk/Dockerfile#L56-L59>
    - [ ] <https://github.com/microsoft/dotnet-framework-docker/blob/58a18cf958883c6a67ae888854bd9bb09f085e83/eng/dockerfile-templates/sdk/Dockerfile.ltsc2016#L80-L81>
  - [ ] Post a new announcement on the [GitHub Discussions page](https://github.com/dotnet/dotnet-docker/discussions/new?category=announcements)
