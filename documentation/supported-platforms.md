# Supported Platform

This document describes the platforms (OS and architectures) supported by the official .NET Docker images.

## Operating Systems

.NET tries to [support a broad set of operating systems and versions](https://github.com/dotnet/core/blob/master/os-lifecycle-policy.md). With containers, it’s too expensive and confusing to support the full matrix of options. In practice, images are produced for a select set of operating systems and versions. If official .NET Docker images aren't provided for your choice OS, you'll need to [author your own Dockerfile which installs .NET](scenarios/installing-dotnet.md).

### Linux

Each distribution (distro) has a unique approach to releasing, schedule, and end-of life (EOL). This prohibits the definition of a one-size-fits-all policy. Instead, a policy is defined for each supported distro.

- Alpine — support latest and retain support for the previous version one quarter (3 months) after a new version is released.
- Debian — support the latest *stable* version at the time a `major.minor` version of .NET is released. As new *stable* versions are released, support is added to the latest .NET version and latest LTS (if they differ).
- Ubuntu — support the latest *LTS* version at the time a `major.minor` version of .NET is released. As new *LTS* versions are released, support is added to the latest .NET version and latest LTS (if they differ).

Pre-release versions of the supported distros will be made available within the [nightly repositories](https://github.com/dotnet/dotnet-docker/blob/nightly/README.md) based on the availability of pre-release OS base images.

### Windows

The official .NET images support Nano Server as well as LTS versions of Windows Server Core for .NET 5.0 and higher. Nano Server is the best Windows SKU to run .NET apps from a performance perspective. In order for Nano Server to perform well and remain lightweight, it doesn't have support for every scenario. In case your scenario isn't supported by Nano Server, you may need to use one of the .NET images based on Windows Server Core. For scenarios where the official .NET images don't meet your needs, you will need to manage your own custom .NET images based on [Windows Server Core](https://hub.docker.com/_/microsoft-windows-servercore) or [Windows](https://hub.docker.com/_/microsoft-windows).

- Nano Server - support all supported versions with each .NET version.
- Windows Server Core - support all LTS versions starting with .NET 5.0.

## Architectures

.NET images are provided for the following architectures.

- Linux/Windows x86-64 (amd64)
- Linux ARMv7 32-bit (arm32v7)
- Linux ARMv8 64-bit (arm64v8)
