# Supported Tags

This document describes the supported tags for the official .NET Docker images.

The .NET team strives to align image tagging practices with the tagging practices utilized by the [Official Images on Docker Hub](https://hub.docker.com/search?q=&type=image&image_filter=official).

## Simple Tags

_Simple Tags_ reference an image for a single platform (e.g. `Windows x64` or `Linux ARMv8 64-bit`).

1. `<Major.Minor.Patch .NET Version>-<OS>-<Architecture>`

    **Examples**

    * `5.0.0-focal-amd64`
    * `5.0.0-focal-arm64v8`
    * `3.1.18-nanoserver-1809`
    * `3.1.18-alpine3.13`
    * `3.1.18-buster-slim-arm32v7`

1. `<Major.Minor .NET Version>-<OS>-<Architecture>`

    **Examples**

    * `5.0-focal-arm64v8`
    * `5.0-focal-amd64`
    * `3.1-nanoserver-1809`
    * `3.1-alpine3.12`
    * `3.1-buster-slim-arm32v7`

## Shared Tags

_Shared Tags_ reference images for [multiple platforms](https://blog.docker.com/2017/09/docker-official-images-now-multi-platform/). The .NET shared tags have the following characteristics:

* Include entries for all [supported architectures](supported-platforms.md#architectures).
* When no OS is specified:
    1. Include Linux entries for Debian.
    1. Include Windows entries for each supported Nano Server version.

1. `<Major.Minor.Patch .NET Version>-<OS>`

    **Examples**

    * `5.0.2-focal`
    * `5.0.2-alpine3.12`

1. `<Major.Minor .NET Version>-<OS>`

    **Examples**

    * `5.0-focal`
    * `5.0-alpine3.12`

1. `<Major.Minor.Patch .NET Version>`

    **Examples**

    * `5.0.0`
    * `3.1.9`

1. `<Major.Minor .NET Version>`

    **Examples**

    * `5.0`
    * `3.1`

1. `latest`

    * [dotnet/](https://hub.docker.com/_/microsoft-dotnet) - `latest` will reference the `<Major.Minor.Patch Version>` image for the most recent GA release.
    * [dotnet/nightly/](https://github.com/dotnet/dotnet-docker/blob/nightly/README.md) - `latest` will reference the `<Major.Minor.Patch Version>` image for the most recent release. This implies `latest` will at times reference prerelease versions. In the event when there are multiple active prerelease versions (e.g. 3.1 preview 3 and 5.0 preview 1), `latest` will reference the lower prerelease version (e.g. 3.1 preview 3) until the point when the lower version (e.g. 3.1) is released. Once this happens, `latest` will reference the higher version (e.g. 5.0 preview 1).

## Tag Parts

1. `<Major.Minor.Patch .NET Version>` - The `Major.Minor.Patch` number of the .NET version included in the image.

    * Tags which use this version format are considered _fixed tags_. The .NET related contents of the referenced images are guaranteed to not change.
    * In the event servicing of the .NET contents of the image is required outside of a regular .NET service release, a `-n` suffix will be added to the .NET version number where n is an incremental count (e.g. [5.0.1-1](https://github.com/dotnet/dotnet-docker/pull/2516)).

1. `<Major.Minor .NET Version>` - The `Major.Minor` number of the .NET version included in the image.

    * Tags which use this version format are considered _floating tags_. These tags are continuously updated to always reference the most recent .NET patch available for the specified `Major.Minor` release.

1. `<OS>` - The name of the OS the image is based upon. See [Supported Platforms](supported-platforms.md#operating-systems) for the list of supported operating systems.

    * The referenced image is automatically updated whenever a new OS patch is released. The OS release name doesn't support pinning to specific OS patches. If OS patch pinning is required then the image digest should be used (e.g. `mcr.microsoft.com/dotnet/runtime@sha256:4d3d5a5131a0621509ab8a75f52955f2d0150972b5c5fb918e2e59d4cb9a9823`).
    * For [Debian](https://en.wikipedia.org/wiki/Debian_version_history) and [Ubuntu](https://en.wikipedia.org/wiki/Ubuntu_version_history) images, release codenames are used versus version numbers.

1. `<Architecture>` - The architecture the image is based on. See [Supported Platforms](supported-platforms.md#architectures) for the list of supported architectures.

    * For Windows, `amd64` is the only architecture supported and is excluded from the tag name.
    * For .NET Core 3.1, `amd64` is the implied default if no architecture is specified.

## Tag Listing

Each [Docker Hub repository](https://hub.docker.com/_/microsoft-dotnet) contains a detailed listing of all supported tags. The listing is broken apart by OS platform (e.g. `Linux amd64 Tags` or `Nano Server 2022 amd64 Tags`). Each row represents a single image and contains all of the tags that reference it. For example the following entry represents the 5.0 runtime Buster image which is referenced by seven tags:

Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.2-buster-slim-amd64, 5.0-buster-slim-amd64, 5.0.2-buster-slim, 5.0-buster-slim, 5.0.2, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/5.0/buster-slim/amd64/Dockerfile) | Debian 10

## Tag Lifecycle

Each tag will be supported for the lifetime of the .NET and OS version referenced by the tag. Once either either of these reaches EOL, the tag will be considered unsupported, will no longer be updated and will be removed from the [Tag Listing](#tag-listing). Unsupported tags will be preserved to prevent breaking any references to it. See [Microsoft support for .NET](https://github.com/dotnet/core/blob/master/microsoft-support.md) for additional details.

### Examples

* `5.0` - Will be supported for the lifetime of the .NET 5.0 release.
* `5.0.2` - Will be supported for the lifetime of the 5.0.2 servicing release.
* `5.0-windowservercore-ltsc2019` - Will be supported for the lifetime of the .NET 5.0 and Windows Server Core LTSC 2019 releases, whichever is shorter.
* `5.0-focal` - Will be supported for the lifetime of the .NET 5.0 and Ubuntu Focal release, whichever is shorter.

## Policy Changes

In the event that a change is needed to the tagging patterns used, all tags for the previous pattern will continue to be supported for their original lifetime. They will however be removed from the documentation. [Announcements](https://github.com/dotnet/dotnet-docker/labels/announcement) will be posted when any tagging policy changes are made.
