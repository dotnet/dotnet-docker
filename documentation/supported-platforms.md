# Supported Platforms

This document describes the platforms (OS and architectures) supported by the official .NET Docker images.

.NET supports [a broad set of operating systems and versions](https://github.com/dotnet/core/blob/main/os-lifecycle-policy.md). Container images are produced for a select set of operating systems and versions. You can also [build your own .NET images](scenarios/installing-dotnet.md).

## Lifecycle

Policies:

- Images will be published for the latest OS version at the time a new .NET major version is released.
- When new OS versions are released, new images will be added to the latest .NET version and latest LTS .NET version.
- .NET versions that are within 6 months of EOL may not get new image variants.
- We stop publishing updates to images when the associated .NET version reaches [end of support](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) or when the OS base image stops receiving updates, whichever happens first.

Practices:

- Images for new OS versions are typically released within one month of the new OS release, with a goal to release same-day when possible.
- New OS versions are available in [`dotnet/nightly` repositories](https://github.com/dotnet/dotnet-docker/blob/nightly/README.md) first, and are added to the officially supported repos afterwards.
- All new OS releases will be accompanied by an [announcement](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements).

For more general support information, see [.NET Support](https://github.com/dotnet/core/blob/main/support.md) and [OS Support](https://github.com/dotnet/core/blob/main/os-lifecycle-policy.md).

### Linux

We publish images for 3 distros:

- Alpine
- Azure Linux
- Ubuntu

Policies:

- Only one version of Alpine images will be published, with 3 months of grace overlap to enable users to move to the new Alpine version.

Pre-release versions of the above distros will be published in the [nightly repositories](https://github.com/dotnet/dotnet-docker/blob/nightly/README.md), pending the availability of pre-release base images.

### Windows

We push images for two SKUs:

- Nano Server
- Windows Server Core (LTS)

Windows Server support timelines can be found here: [Windows Server release information](https://learn.microsoft.com/windows/release-health/windows-server-release-info). For scenarios where the official .NET images don't meet your needs, you will need to manage your own custom .NET images based on [Windows Server Core](https://mcr.microsoft.com/artifact/mar/windows/servercore/about) or [Windows Server](https://mcr.microsoft.com/artifact/mar/windows/server/about).

## Architectures

.NET images are provided for the following architectures.

- Linux/Windows x86-64 (amd64)
- Linux ARMv7 32-bit (arm32v7)
- Linux ARMv8 64-bit (arm64v8)
