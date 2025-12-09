# Supported Platforms

This document describes the platforms (OS and architectures) supported by the official .NET Docker images.

## Operating Systems

.NET supports [a broad set of operating systems and versions](https://github.com/dotnet/core/blob/main/os-lifecycle-policy.md). When producing container images, it’s impractical to support the full matrix of OS, arch, and .NET version combinations. In practice, images are produced for a select set of operating systems and versions. If official .NET container images aren't provided for your preferred OS, [let us know by opening a discussion](https://github.com/dotnet/dotnet-docker/discussions). Alternatively, you can [author your own .NET images](scenarios/installing-dotnet.md).

- Images for new OS versions are typically released within one month of the new OS release, with a goal to release same-day when possible.
- New OS versions are available in [`dotnet/nightly` repositories](https://github.com/dotnet/dotnet-docker/blob/nightly/README.md) first, and are added to the officially supported repos afterwards.
- All new OS releases will be accompanied by an [announcement](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements).
- Unless otherwise specified (see Alpine Linux below), we stop publishing updates to images when the .NET version in the image reaches [end of support](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) or when the OS base image stops receiving updates, whichever happens first.

These policies are specific to .NET container images. For more information on overall .NET OS support, see [.NET OS Support Tracking](https://github.com/dotnet/core/issues/9638).

### Linux

Since Linux distributions ("distros") each have unique release and support policies, a one-size-fits-all policy doesn't make sense.
Instead, we have separate policies for each distro.
We publish .NET container images for the following versions of each Linux distro:

- Alpine Linux
  - .NET images will be published for the latest Alpine Linux version.
  - When new Alpine versions are released, new images will be added for all in-support .NET versions.
  - .NET images for the previous version of Alpine will published for 3 months after the new version is released.
- Azure Linux and Ubuntu LTS
  - .NET images will be published images for the latest OS version at the time a new major/minor version of .NET is released.
  - When new OS versions are released, new images will be added to the latest .NET version and latest LTS .NET version (if they differ).
- Debian
  - Existing .NET Debian images will continue to receive base image updates, but new .NET images will not be added for future Debian versions.

Pre-release versions of the above distros will be published in the [nightly repositories](https://github.com/dotnet/dotnet-docker/blob/nightly/README.md), pending the availability of pre-release base images.

#### FedRAMP Compliance

For [.NET appliance images](./supported-tags.md#net-appliance-images) based on Azure Linux, base image OS upgrades will be delayed until the new version of Azure Linux has FedRAMP approval.

### Windows

The official .NET images support Nano Server as well as LTS versions of Windows Server Core. Nano Server is the best Windows SKU to run .NET apps from a performance perspective. In order for Nano Server to perform well and remain lightweight, it doesn't have support for every scenario. In case your scenario isn't supported by Nano Server, you may need to use one of the .NET images based on Windows Server Core.

Windows Server support timelines can be found here: [Windows Server release information](https://learn.microsoft.com/windows/release-health/windows-server-release-info). .NET images will be published for the following Windows OS versions:

- Nano Server - all versions in the [Mainstream Support](https://learn.microsoft.com/lifecycle/policies/fixed#mainstream-support) phase.
- Windows Server Core - all versions in the [Mainstream Support](https://learn.microsoft.com/lifecycle/policies/fixed#mainstream-support) phase.

For scenarios where the official .NET images don't meet your needs, you will need to manage your own custom .NET images based on [Windows Server Core](https://mcr.microsoft.com/artifact/mar/windows/servercore/about) or [Windows Server](https://mcr.microsoft.com/artifact/mar/windows/server/about).

## Architectures

.NET images are provided for the following architectures.

- Linux/Windows x86-64 (amd64)
- Linux ARMv7 32-bit (arm32v7)
- Linux ARMv8 64-bit (arm64v8)
