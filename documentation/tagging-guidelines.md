# Image Tagging Guidelines

This document describes the tagging practices used on the official .NET Core Docker images.

The .NET Core image tags strive to align with the tagging practices utilized by the [Official Images on Docker Hub](https://hub.docker.com/search?q=&type=image&image_filter=official).

## Simple Tags

1. `<Major.Minor.Patch Version>-<OS>-<Architecture>`

    **Examples**

    * `2.1.11-stretch-slim-arm32v7`
    * `2.2.5-alpine3.9`
    * `3.0.0-nanoserver-1809`
    * `3.1.0-preview1-disco-arm64v8`

1. `<Major.Minor Version>-<OS>-<Architecture>`

    **Examples**

    * `2.1-stretch-slim-arm32v7`
    * `2.2-alpine3.9`
    * `3.0-nanoserver-1809`
    * `3.1-disco-arm64v8`

## Shared Tags

1. `<Major.Minor.Patch Version>`

    **Examples**

    * `2.1.11`
    * `2.2.5`
    * `3.0.0`
    * `3.1.0-preview1`

1. `<Major.Minor Version>`

    **Examples**

    * `2.1`
    * `2.2`
    * `3.0`
    * `3.1`

1. `latest` - Will always refer to the most recent non-prerelease `<Major.Minor.Patch Version>` image.

All shared tags [support multiple platforms](https://blog.docker.com/2017/09/docker-official-images-now-multi-platform/) and have the following characteristics:

1. Include entries for all supported architectures.

1. Include Linux entries based on Debian.

1. Include Windows entries for each supported version.

## Tag Parts

* `<Major.Minor.Patch Version>` - The `Major.Minor.Patch` number of the .NET Core version included in the image.

* `<Major.Minor Version>` - The `Major.Minor` number of the .NET Core version included in the image.  The tag is updated to always reference the most recent patch that is currently available for the `Major.Minor` release.

* `<OS>` - The name of the OS release and variant the image is based upon.  The image the tag references is updated whenever a new OS patch is released.  The OS release name does support pinning to specific OS patches.  If OS patch pinning is required then the image digest should be used (e.g. `mcr.microsoft.com/dotnet/core/runtime@sha256:fff4cfe761fde9f3b72377e350eda7cd82caf0c1ec6be281b92d8614860fa449`).

* `<Architecture>` - The architecture the image is based on.  `amd64` is the implied default if no architecture is specified.
