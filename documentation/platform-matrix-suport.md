# Platform Matrix and Support

This document describes the platform matrix supported by the official .NET Core Docker images.

## Operating Systems

.NET Core tries to [support a broad set of operating systems and versions](https://github.com/dotnet/core/blob/master/os-lifecycle-policy.md). With containers, it’s too expensive and confusing to support the full matrix of options. In practice, images are produced for a select set of operating systems and versions.

### Linux

Each distribution (distro) has a unique approach to releasing, schedule and end-of life (EOL). This prohibits the definition of a one-size-fits-all policy. Instead, a policy is defined for each supported distro.

- Alpine — support tip and retain support for one quarter (3 months) after a new version is released.
- Debian — support the latest *stable* version at the time a `major.minor` version of .NET Core is released.  Add support for each new *stable* version as it is released.  Debian images are included in the shared/multi-arch tags.
- Ubuntu — support the latest *LTS* version at the time a `major.minor` version of .NET Core is released.  Add support for each new *LTS* version as it is released.

Pre-release versions of the supported distros will be made available within the [nightly repositories](https://hub.docker.com/_/microsoft-dotnet-core-nightly) based on the availability of pre-release OS base images.

### Windows

Nano Server is the only Windows SKU supported by the official .NET Core images.  Nano Server is the best Windows SKU to run .NET Core apps from a performance perspective.  In order for Nano Server to perform well and remain lightweight, it doesn't have support for every scenario.  For these cases, it is expected that consumers will need to manage their own custom .NET Core images based on [Windows Server Core](https://hub.docker.com/_/microsoft-windows-servercore) or [Windows](https://hub.docker.com/_/microsoft-windows).

- Nano Server - support all supported versions with each .NET Core version.  Each version is also included within the shared/multi-arch tags.

## Architectures

.NET Core supports amd64, arm32 and arm64 architectures.  All of the supported architectures are also supported within containers.

- Linux/Windows x86-64 (amd64)
- ARMv7 32-bit (arm32v7)
- ARMv8 64-bit (arm64v8)
