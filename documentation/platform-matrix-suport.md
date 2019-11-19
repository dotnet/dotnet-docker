# Platform Matrix and Support

This document describes the platform matrix supported by the official .NET Core Docker images.

## Operating Systems

.NET Core tries to [support a broad set of operating systems and versions](https://github.com/dotnet/core/blob/master/os-lifecycle-policy.md). With containers, it’s too expensive and confusing to support the full matrix of options. In practice, images are produced for a select set of operating systems and versions.

### Linux

Each distribution has a unique approach to releasing, schedule and end-of life (EOL). This prohibits the definition of a one-size-fits-all policy. Instead, a policy is defined for each supported distro.

- Alpine — support tip and retain support for one quarter (3 months) after a new version is released.
- Debian — support one Debian version per .NET Core version, whichever Debian version is the latest when a given .NET Core version ships. This is also the default Linux image used for a given shared/multi-arch tag.
- Ubuntu — support one Ubuntu version per .NET Core version, whichever Ubuntu version is the latest LTS version when a given .NET Core version ships.  In addition, as it gets closer to new Ubuntu LTS versions, support will be added for non-LTS Ubuntu versions as a means of validating the new LTS versions.

### Windows

Nano Server is the only Windows SKU supported by the official .NET Core images.  Nano Server is the best Windows SKU to run .NET Core apps from a performance perspective.  In order for Nano Server to perform well and remain lightweight, it doesn't have support for every scenario.  For these cases, it is expected that consumers will need to manage their own custom .NET Core images based on [Windows Server Core](https://hub.docker.com/_/microsoft-windows-servercore) or [Windows](https://hub.docker.com/_/microsoft-windows).

- Nano Server - support all supported versions with each .NET Core version.  Each version is also included within the shared/multi-arch tags.

## Architectures

.NET Core supports amd64, arm32 and arm64 architectures.  All of the supported architectures are also supported within containers.

- Linux/Windows x86-64 (amd64)
- ARMv7 32-bit (arm32v7)
- ARMv8 64-bit (arm64v8)
