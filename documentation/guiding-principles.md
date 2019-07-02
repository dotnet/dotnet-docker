# .NET Core Docker Guiding Principles

These are the guiding principles for the content, tagging and production of the .NET Core Docker images.  They establish a framework for the behavior customers can expect and the decision-making process when making changes to the images.  They are not hard rules which are stringently enforced as there will be occasions where it is sensible to make special exceptions.

## Image Content

1. Images are intended to satisfy the common usage scenarios.  They are not intended to satisfy every possible usage scenario.  As a result of this, decisions will be made (e.g. components excluded, configurations made, etc.) in order to keep the image size manageable.

    It is expected there will be scenarios in which customers will need to create derived images that add the required components/settings. A few examples include:

    1. Alpine images have the .NET Core Invariant Mode enabled ([details](https://github.com/dotnet/dotnet-docker/issues/371)).
    1. System.Drawing.Common native dependencies are not included.

    It is expected that component authors package/author their bits which make them more easily consumable without including them within the .NET Core images.  Examples include:

    1. xcopy installs
    1. Package as a .NET Core global tool
    1. Ship within a Docker image
        1. Support standalone execution and multi-stage builds
        1. Support running within a side car image (e.g. utilizing --net option)

1. Components installed within the images are required to have the same or longer support lifecycle as [.NET Core](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).  For example, if a component is included with an LTSC release of .NET, then that component version will need to be supported for the life of the LTSC release.  Components are expected to be patched as appropriate.

1. Breaking changes are not allowed within a release.  This includes changes such as adding/removing components, adding/removing ENVs, and major/minor version changes to included components.

1. There should be parity in the matrix of supported image variants.  Examples include:
    1. If support for a new distro is added for a particular .NET Core version, then new `runtime-deps`, `runtime`, `aspnet`, and `sdk` images should all be added for the new distro.
    1. If a new component is added, it should be available within all of the supported architectures - `amd64`, `arm32`, `arm64`, etc.

1. The set of supported Linux distros and versions will evolve with the ecosystem.

    **// Question: Should something be stated on how we uptake new OS versions?**

1. Nano Server is the only Windows SKU supported.  Nano Server is the best Windows SKU to run .NET Core apps from a performance perspective.  There are .NET Core scenarios which are not supported by Nano Server.  For these cases, customers will need to manage their own custom images based on [Windows Server Core](https://hub.docker.com/_/microsoft-windows-servercore) or [ Windows](https://hub.docker.com/_/microsoft-windows).

## Image Tagging

The .NET Core image tags strive to align with the tagging practices utilized by the [Official Images on Docker Hub](https://hub.docker.com/search?q=&type=image&image_filter=official).

### Simple Tags

1. `<Major.Minor.Patch Version>-<OS>-<Architecture>` - e.g. `2.2.5-alpine3.9`, `2.1.11-stretch-slim-arm32v7`, `3.0.0-nanoserver-1809`, `5.0.0-preview1-disco-arm64`

1. `<Major.Minor Version>-<OS>-<Architecture>` - e.g. `2.2-alpine3.9`, `2.1-stretch-slim-arm32v7`, `3.0-nanoserver-1809`

### Shared Tags

1. `<Major.Minor.Patch Version>` - e.g `2.1.11`, `3.0.0`, `5.0.0-preview1`
1. `<Major.Minor Version>` - e.g `2.1`, `3.0`
1. `latest` - Will always refer to the most recent non-prerelease `<Major.Minor.Patch Version>` image.

    All shared tags [support multiple platforms](https://blog.docker.com/2017/09/docker-official-images-now-multi-platform/) and have the following characteristics:

    1. Include entries for all supported architectures.

    1. Include Linux entries based on Debian.

    1. Include Windows entries for each supported version.

### Tag Parts

* `<Major.Minor.Patch Version>` - The `Major.Minor.Patch` number of the .NET Core version included in the image.

* `<Major.Minor Version>` - The `Major.Minor` number of the .NET Core version included in the image.  The tag is updated to always reference the most recent patch that is currently available for the `Major.Minor` release.

* `<OS>` - The name of the OS release and variant the image is based.  The image the tag references is updated whenever a new OS patch is released.  The OS release name does support pinning to a specific OS patch.  If OS patch pinning is required then the image digest should be used (e.g. `mcr.microsoft.com/dotnet/core/runtime@sha256:fff4cfe761fde9f3b72377e350eda7cd82caf0c1ec6be281b92d8614860fa449`).

* `<Architecture>` - The architecture the image is based on.  `amd64` is the implied default if no architecture is specified.

## Engineering

1. Images will be included as part of the .NET Core release process.  This means the Docker images will be release at the same time as the core product.

1. Images will get rebuilt within hours of base image changes. For example suppose a particular version of Alpine is patched.  The .NET Core images based on this version of Alpine will get rebuilt based on this new base image within hours of its release.

1. Images will never be deleted from the [official Docker repositories](https://hub.docker.com/_/microsoft-dotnet-core/). This does not apply to the [nightly repositories](https://hub.docker.com/_/microsoft-dotnet-core-nightly).

1. The [Dockerfiles](https://github.com/dotnet/dotnet-docker/search?q=filename%3ADockerfile) used to produce all of the images will be publicly available. Customers will be able to take the Dockerfiles and build them to produce their own equivalent images.  No special build steps or permissions should be needed to build the Dockerfiles.

1. If a change is ever made to the tagging patterns, all of the old tags will be serviced appropriately through the lifetime of the contained .NET version.  All old tags will no longer be documented within the tag details section of the readme.
