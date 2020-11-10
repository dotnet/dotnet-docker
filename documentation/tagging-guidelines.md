# Image Tagging Guidelines

This document describes the tagging practices used on the official .NET Docker images.

The .NET image tags strive to align with the tagging practices utilized by the [Official Images on Docker Hub](https://hub.docker.com/search?q=&type=image&image_filter=official).

## Simple Tags

1. `<Major.Minor.Patch Version>-<OS>-<Architecture>`

    **Examples**

    * `2.1.23-stretch-slim-arm32v7`
    * `2.1.23-alpine3.12`
    * `3.1.9-nanoserver-1809`
    * `5.0.0-focal-arm64v8`
    * `5.0.0-focal-amd64`

> NOTE: In versions 2.1 and 3.1 of .NET Core, tags for the AMD64 architecture do not include the "-amd64" suffix (e.g. `2.1.23-alpine3.12`). In .NET 5.0 and higher, it is explicitly included for Linux distros (e.g. `5.0.0-focal-amd64`).

1. `<Major.Minor Version>-<OS>-<Architecture>`

    **Examples**

    * `2.1-stretch-slim-arm32v7`
    * `3.1-alpine3.12`
    * `3.1-nanoserver-1809`
    * `5.0-focal-amd64`
    * `5.0-focal-arm64v8`

> NOTE: In versions 2.1 and 3.1 of .NET Core, tags for the AMD64 architecture do not include the "-amd64" suffix (e.g. `3.1-alpine3.12`). In .NET 5.0 and higher, it is explicitly included for Linux distros (e.g. `5.0-focal-amd64`).

## Shared Tags

1. `<Major.Minor.Patch Version>`

    **Examples**

    * `2.1.23`
    * `3.1.9`
    * `5.0.0`

1. `<Major.Minor Version>`

    **Examples**

    * `2.1`
    * `3.1`
    * `5.0`

1. `latest`

    * dotnet - `latest` will reference the most recent non-prerelease `<Major.Minor.Patch Version>` image.
    * dotnet/nightly - `latest` will reference the most recent `<Major.Minor.Patch Version>` image.  This implies `latest` will at times reference prerelease versions.  In the event when there are multiple active prerelease versions (e.g. 3.1 preview 3 and 5.0 preview 1), `latest` will reference the lower prerelease version (e.g. 3.1 preview 3) until the point when the lower version (e.g. 3.1) is released.  Once this happens, `latest` will reference the higher version (e.g. 5.0 preview 1).

All shared tags [support multiple platforms](https://blog.docker.com/2017/09/docker-official-images-now-multi-platform/) and have the following characteristics:

1. Include entries for all supported architectures.

1. Include Linux entries based on Debian.

1. Include Windows Nano Server entries for each supported version.

## Tag Parts

* `<Major.Minor.Patch Version>` - The `Major.Minor.Patch` number of the .NET version included in the image.

* `<Major.Minor Version>` - The `Major.Minor` number of the .NET version included in the image.  The tag is updated to always reference the most recent patch that is currently available for the `Major.Minor` release.

* `<OS>` - The name of the OS release and variant the image is based upon.  The image the tag references is updated whenever a new OS patch is released.  The OS release name does support pinning to specific OS patches.  If OS patch pinning is required then the image digest should be used (e.g. `mcr.microsoft.com/dotnet/runtime@sha256:4d3d5a5131a0621509ab8a75f52955f2d0150972b5c5fb918e2e59d4cb9a9823`).

* `<Architecture>` - The architecture the image is based on.  For .NET Core 2.1 and 3.1, `amd64` is the implied default if no architecture is specified. In .NET 5.0 and higher, `amd64` is explicitly included for Linux distros (e.g. `5.0-focal-amd64`).
