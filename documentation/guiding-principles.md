# Guiding Principles

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
        1. Support running within a side car image (possibly utilizing --net option)

1. Components installed within the images are required to have the same or longer support lifecycle as [.NET Core](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).  For example, if a component is included with an LTSC release of .NET, then that component version will need to be supported for the life of the LTSC release.  Components are expected to be patched as appropriate.

1. Breaking changes are not allowed within a release.  This includes changes such as adding/removing components, adding/removing ENVs, and major/minor version changes to included components.

1. There should be parity in the matrix of supported image variants.  Examples include:
    1. If support for a new distro is added for a particular .NET Core version, then new `runtime-deps`, `runtime`, `aspnet`, and `sdk` images should all be added for the new distro.
    1. If a new component is added, it should be available across all supported OS types and architectures.

1. The matrix of supported container operating systems and versions will evolve on a continuous bases. The [Supported Platforms](supported-platforms.md) describes this in detail.

## Image Tagging

The .NET Core image tags strive to align with the tagging practices utilized by the [Official Images on Docker Hub](https://hub.docker.com/search?q=&type=image&image_filter=official).  The [Tagging Guidelines](tagging-guidelines.md) describe this in detail.

## Engineering

1. Images will be included as part of the .NET Core release process.  This means the Docker images will be released at the same time as the core product.

1. Images will be rebuilt within hours of base image changes. For example, suppose a particular version of Alpine is patched.  The .NET Core images based on this version of Alpine will be rebuilt based on this new base image within hours of its release.

1. Images will never be deleted from the [official Docker repositories](https://hub.docker.com/_/microsoft-dotnet-core/). This does not apply to the [nightly repositories](https://hub.docker.com/_/microsoft-dotnet-core-nightly).

1. The [Dockerfiles](https://github.com/dotnet/dotnet-docker/search?q=filename%3ADockerfile) used to produce all of the images will be publicly available. Customers will be able to take the Dockerfiles and build them to produce their own equivalent images.  No special build steps or permissions should be needed to build the Dockerfiles.

1. If a change is ever made to the tagging patterns, all of the old tags will be serviced appropriately through the lifetime of the contained .NET version.  All old tags will no longer be documented within the tag details section of the readme.

1. No experimental Docker features will be utilized within the infrastructure used to produce the images.  Utilizing experimental features can negatively affect the reliability of image production and introduces a risk to the integrity of the resulting artifacts.
