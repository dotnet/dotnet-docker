# .NET SDK

> **Important**: The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).
>
> See [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.sdk.md) for images with official releases of [.NET](https://github.com/dotnet/core).

## Featured Tags

* `9.0` (Release Candidate)
  * `docker pull mcr.microsoft.com/dotnet/nightly/sdk:9.0`
* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/sdk:8.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/sdk:6.0`

## About

This image contains the .NET SDK which is comprised of three parts:

1. .NET CLI
1. .NET runtime
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Building .NET Apps with Docker

* [.NET Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

### Develop .NET Apps in a Container

The following samples show how to develop, build and test .NET applications with Docker without the need to install the .NET SDK.

* [Build .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/build-in-sdk-container.md)
* [Test .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-tests-in-sdk-container.md)
* [Run .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-in-sdk-container.md)

## Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

## Related Repositories

.NET:

* [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.md): .NET
* [dotnet/sdk](https://github.com/dotnet/dotnet-docker/blob/main/README.sdk.md): .NET SDK
* [dotnet/nightly/aspnet](https://github.com/dotnet/dotnet-docker/blob/nightly/README.aspnet.md): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://github.com/dotnet/dotnet-docker/blob/nightly/README.runtime.md): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://github.com/dotnet/dotnet-docker/blob/nightly/README.runtime-deps.md): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor](https://github.com/dotnet/dotnet-docker/blob/nightly/README.monitor.md): .NET Monitor Tool (Preview)
* [dotnet/nightly/aspire-dashboard](https://github.com/dotnet/dotnet-docker/blob/nightly/README.aspire-dashboard.md): .NET Aspire Dashboard (Preview)
* [dotnet/samples](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md): .NET Samples

.NET Framework:

* [dotnet/framework](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.md): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.samples.md): .NET Framework, ASP.NET and WCF Samples

## Full Tag Listing

### Linux amd64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.100-bookworm-slim-amd64, 9.0-bookworm-slim-amd64, 9.0.100-bookworm-slim, 9.0-bookworm-slim, 9.0.100, 9.0, latest | [Dockerfile](src/sdk/9.0/bookworm-slim/amd64/Dockerfile) | Debian 12
9.0.100-alpine3.20-amd64, 9.0-alpine3.20-amd64, 9.0-alpine-amd64, 9.0.100-alpine3.20, 9.0-alpine3.20, 9.0-alpine | [Dockerfile](src/sdk/9.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
9.0.100-alpine3.20-aot-amd64, 9.0-alpine3.20-aot-amd64, 9.0-alpine-aot-amd64, 9.0.100-alpine3.20-aot, 9.0-alpine3.20-aot, 9.0-alpine-aot | [Dockerfile](src/sdk/9.0/alpine3.20-aot/amd64/Dockerfile) | Alpine 3.20
9.0.100-noble-amd64, 9.0-noble-amd64, 9.0.100-noble, 9.0-noble | [Dockerfile](src/sdk/9.0/noble/amd64/Dockerfile) | Ubuntu 24.04
9.0.100-noble-aot-amd64, 9.0-noble-aot-amd64, 9.0.100-noble-aot, 9.0-noble-aot | [Dockerfile](src/sdk/9.0/noble-aot/amd64/Dockerfile) | Ubuntu 24.04
9.0.100-azurelinux3.0-amd64, 9.0-azurelinux3.0-amd64, 9.0.100-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/sdk/9.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
9.0.100-azurelinux3.0-aot-amd64, 9.0-azurelinux3.0-aot-amd64, 9.0.100-azurelinux3.0-aot, 9.0-azurelinux3.0-aot | [Dockerfile](src/sdk/9.0/azurelinux3.0-aot/amd64/Dockerfile) | Azure Linux 3.0
8.0.403-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.403-bookworm-slim, 8.0-bookworm-slim, 8.0.403, 8.0 | [Dockerfile](src/sdk/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.403-alpine3.20-amd64, 8.0-alpine3.20-amd64, 8.0-alpine-amd64, 8.0.403-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](src/sdk/8.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
8.0.403-alpine3.20-aot-amd64, 8.0-alpine3.20-aot-amd64, 8.0-alpine-aot-amd64, 8.0.403-alpine3.20-aot, 8.0-alpine3.20-aot, 8.0-alpine-aot | [Dockerfile](src/sdk/8.0/alpine3.20-aot/amd64/Dockerfile) | Alpine 3.20
8.0.403-noble-amd64, 8.0-noble-amd64, 8.0.403-noble, 8.0-noble | [Dockerfile](src/sdk/8.0/noble/amd64/Dockerfile) | Ubuntu 24.04
8.0.403-noble-aot-amd64, 8.0-noble-aot-amd64, 8.0.403-noble-aot, 8.0-noble-aot | [Dockerfile](src/sdk/8.0/noble-aot/amd64/Dockerfile) | Ubuntu 24.04
8.0.403-jammy-amd64, 8.0-jammy-amd64, 8.0.403-jammy, 8.0-jammy | [Dockerfile](src/sdk/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.403-jammy-aot-amd64, 8.0-jammy-aot-amd64, 8.0.403-jammy-aot, 8.0-jammy-aot | [Dockerfile](src/sdk/8.0/jammy-aot/amd64/Dockerfile) | Ubuntu 22.04
8.0.403-azurelinux3.0-amd64, 8.0-azurelinux3.0-amd64, 8.0.403-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/sdk/8.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
8.0.403-azurelinux3.0-aot-amd64, 8.0-azurelinux3.0-aot-amd64, 8.0.403-azurelinux3.0-aot, 8.0-azurelinux3.0-aot | [Dockerfile](src/sdk/8.0/azurelinux3.0-aot/amd64/Dockerfile) | Azure Linux 3.0
8.0.403-cbl-mariner2.0-amd64, 8.0-cbl-mariner2.0-amd64, 8.0.403-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](src/sdk/8.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.403-cbl-mariner2.0-aot-amd64, 8.0-cbl-mariner2.0-aot-amd64, 8.0.403-cbl-mariner2.0-aot, 8.0-cbl-mariner2.0-aot | [Dockerfile](src/sdk/8.0/cbl-mariner2.0-aot/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.427-1-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.427-1-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](src/sdk/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.427-1-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.427-1-bullseye-slim, 6.0-bullseye-slim, 6.0.427-1, 6.0 | [Dockerfile](src/sdk/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.427-1-alpine3.20-amd64, 6.0-alpine3.20-amd64, 6.0-alpine-amd64, 6.0.427-1-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](src/sdk/6.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
6.0.427-1-jammy-amd64, 6.0-jammy-amd64, 6.0.427-1-jammy, 6.0-jammy | [Dockerfile](src/sdk/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.427-1-cbl-mariner2.0-amd64, 6.0-cbl-mariner2.0-amd64, 6.0.427-1-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](src/sdk/6.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.427-1-focal-amd64, 6.0-focal-amd64, 6.0.427-1-focal, 6.0-focal | [Dockerfile](src/sdk/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04

### Linux arm64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.100-bookworm-slim-arm64v8, 9.0-bookworm-slim-arm64v8, 9.0.100-bookworm-slim, 9.0-bookworm-slim, 9.0.100, 9.0, latest | [Dockerfile](src/sdk/9.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
9.0.100-alpine3.20-arm64v8, 9.0-alpine3.20-arm64v8, 9.0-alpine-arm64v8, 9.0.100-alpine3.20, 9.0-alpine3.20, 9.0-alpine | [Dockerfile](src/sdk/9.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
9.0.100-alpine3.20-aot-arm64v8, 9.0-alpine3.20-aot-arm64v8, 9.0-alpine-aot-arm64v8, 9.0.100-alpine3.20-aot, 9.0-alpine3.20-aot, 9.0-alpine-aot | [Dockerfile](src/sdk/9.0/alpine3.20-aot/arm64v8/Dockerfile) | Alpine 3.20
9.0.100-noble-arm64v8, 9.0-noble-arm64v8, 9.0.100-noble, 9.0-noble | [Dockerfile](src/sdk/9.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.100-noble-aot-arm64v8, 9.0-noble-aot-arm64v8, 9.0.100-noble-aot, 9.0-noble-aot | [Dockerfile](src/sdk/9.0/noble-aot/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.100-azurelinux3.0-arm64v8, 9.0-azurelinux3.0-arm64v8, 9.0.100-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/sdk/9.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.100-azurelinux3.0-aot-arm64v8, 9.0-azurelinux3.0-aot-arm64v8, 9.0.100-azurelinux3.0-aot, 9.0-azurelinux3.0-aot | [Dockerfile](src/sdk/9.0/azurelinux3.0-aot/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.403-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.403-bookworm-slim, 8.0-bookworm-slim, 8.0.403, 8.0 | [Dockerfile](src/sdk/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.403-alpine3.20-arm64v8, 8.0-alpine3.20-arm64v8, 8.0-alpine-arm64v8, 8.0.403-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](src/sdk/8.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
8.0.403-alpine3.20-aot-arm64v8, 8.0-alpine3.20-aot-arm64v8, 8.0-alpine-aot-arm64v8, 8.0.403-alpine3.20-aot, 8.0-alpine3.20-aot, 8.0-alpine-aot | [Dockerfile](src/sdk/8.0/alpine3.20-aot/arm64v8/Dockerfile) | Alpine 3.20
8.0.403-noble-arm64v8, 8.0-noble-arm64v8, 8.0.403-noble, 8.0-noble | [Dockerfile](src/sdk/8.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.403-noble-aot-arm64v8, 8.0-noble-aot-arm64v8, 8.0.403-noble-aot, 8.0-noble-aot | [Dockerfile](src/sdk/8.0/noble-aot/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.403-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.403-jammy, 8.0-jammy | [Dockerfile](src/sdk/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.403-jammy-aot-arm64v8, 8.0-jammy-aot-arm64v8, 8.0.403-jammy-aot, 8.0-jammy-aot | [Dockerfile](src/sdk/8.0/jammy-aot/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.403-azurelinux3.0-arm64v8, 8.0-azurelinux3.0-arm64v8, 8.0.403-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/sdk/8.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.403-azurelinux3.0-aot-arm64v8, 8.0-azurelinux3.0-aot-arm64v8, 8.0.403-azurelinux3.0-aot, 8.0-azurelinux3.0-aot | [Dockerfile](src/sdk/8.0/azurelinux3.0-aot/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.403-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner2.0-arm64v8, 8.0.403-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](src/sdk/8.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.403-cbl-mariner2.0-aot-arm64v8, 8.0-cbl-mariner2.0-aot-arm64v8, 8.0.403-cbl-mariner2.0-aot, 8.0-cbl-mariner2.0-aot | [Dockerfile](src/sdk/8.0/cbl-mariner2.0-aot/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.427-1-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.427-1-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](src/sdk/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.427-1-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.427-1-bullseye-slim, 6.0-bullseye-slim, 6.0.427-1, 6.0 | [Dockerfile](src/sdk/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.427-1-alpine3.20-arm64v8, 6.0-alpine3.20-arm64v8, 6.0-alpine-arm64v8, 6.0.427-1-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](src/sdk/6.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
6.0.427-1-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.427-1-jammy, 6.0-jammy | [Dockerfile](src/sdk/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.427-1-cbl-mariner2.0-arm64v8, 6.0-cbl-mariner2.0-arm64v8, 6.0.427-1-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](src/sdk/6.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.427-1-focal-arm64v8, 6.0-focal-arm64v8, 6.0.427-1-focal, 6.0-focal | [Dockerfile](src/sdk/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04

### Linux arm32 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.100-bookworm-slim-arm32v7, 9.0-bookworm-slim-arm32v7, 9.0.100-bookworm-slim, 9.0-bookworm-slim, 9.0.100, 9.0, latest | [Dockerfile](src/sdk/9.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
9.0.100-alpine3.20-arm32v7, 9.0-alpine3.20-arm32v7, 9.0-alpine-arm32v7, 9.0.100-alpine3.20, 9.0-alpine3.20, 9.0-alpine | [Dockerfile](src/sdk/9.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
9.0.100-noble-arm32v7, 9.0-noble-arm32v7, 9.0.100-noble, 9.0-noble | [Dockerfile](src/sdk/9.0/noble/arm32v7/Dockerfile) | Ubuntu 24.04
8.0.403-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.403-bookworm-slim, 8.0-bookworm-slim, 8.0.403, 8.0 | [Dockerfile](src/sdk/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.403-alpine3.20-arm32v7, 8.0-alpine3.20-arm32v7, 8.0-alpine-arm32v7, 8.0.403-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](src/sdk/8.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
8.0.403-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.403-jammy, 8.0-jammy | [Dockerfile](src/sdk/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.427-1-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.427-1-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](src/sdk/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.427-1-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.427-1-bullseye-slim, 6.0-bullseye-slim, 6.0.427-1, 6.0 | [Dockerfile](src/sdk/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.427-1-alpine3.20-arm32v7, 6.0-alpine3.20-arm32v7, 6.0-alpine-arm32v7, 6.0.427-1-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](src/sdk/6.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
6.0.427-1-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.427-1-jammy, 6.0-jammy | [Dockerfile](src/sdk/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.427-1-focal-arm32v7, 6.0-focal-arm32v7, 6.0.427-1-focal, 6.0-focal | [Dockerfile](src/sdk/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

### Nano Server 2025 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.100-nanoserver-ltsc2025, 9.0-nanoserver-ltsc2025 | [Dockerfile](src/sdk/9.0/nanoserver-ltsc2025/amd64/Dockerfile)
8.0.403-nanoserver-ltsc2025, 8.0-nanoserver-ltsc2025 | [Dockerfile](src/sdk/8.0/nanoserver-ltsc2025/amd64/Dockerfile)

### Windows Server Core 2025 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.100-windowsservercore-ltsc2025, 9.0-windowsservercore-ltsc2025 | [Dockerfile](src/sdk/9.0/windowsservercore-ltsc2025/amd64/Dockerfile)
8.0.403-windowsservercore-ltsc2025, 8.0-windowsservercore-ltsc2025 | [Dockerfile](src/sdk/8.0/windowsservercore-ltsc2025/amd64/Dockerfile)

### Nano Server 2022 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.100-nanoserver-ltsc2022, 9.0-nanoserver-ltsc2022 | [Dockerfile](src/sdk/9.0/nanoserver-ltsc2022/amd64/Dockerfile)
8.0.403-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](src/sdk/8.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.427-1-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.427-1, 6.0 | [Dockerfile](src/sdk/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

### Windows Server Core 2022 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.100-windowsservercore-ltsc2022, 9.0-windowsservercore-ltsc2022 | [Dockerfile](src/sdk/9.0/windowsservercore-ltsc2022/amd64/Dockerfile)
8.0.403-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](src/sdk/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.427-1-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](src/sdk/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

### Nano Server, version 1809 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.100-nanoserver-1809, 9.0-nanoserver-1809 | [Dockerfile](src/sdk/9.0/nanoserver-1809/amd64/Dockerfile)
8.0.403-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](src/sdk/8.0/nanoserver-1809/amd64/Dockerfile)
6.0.427-1-nanoserver-1809, 6.0-nanoserver-1809, 6.0.427-1, 6.0 | [Dockerfile](src/sdk/6.0/nanoserver-1809/amd64/Dockerfile)

### Windows Server Core 2019 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.100-windowsservercore-ltsc2019, 9.0-windowsservercore-ltsc2019 | [Dockerfile](src/sdk/9.0/windowsservercore-ltsc2019/amd64/Dockerfile)
8.0.403-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](src/sdk/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.427-1-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](src/sdk/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/dotnet/nightly/sdk/tags/list) for all supported and unsupported tags.*

## Support

### Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

### Image Update Policy

* **Base Image Updates:** Images are re-built within 12 hours of any updates to their base images (e.g. debian:bookworm-slim, windows/nanoserver:ltsc2022, etc.).
* **.NET Releases:** Images are re-built as part of releasing new .NET versions. This includes new major versions, minor versions, and servicing releases.
* **Critical CVEs:** Images are re-built to pick up critical CVE fixes as described by the CVE Update Policy below.
* **Monthly Re-builds:** Images are re-built monthly, typically on the second Tuesday of the month, in order to pick up lower-severity CVE fixes.
* **Out-Of-Band Updates:** Images can sometimes be re-built when out-of-band updates are necessary to address critical issues. If this happens, new fixed version tags will be updated according to the [Fixed version tags documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md#fixed-version-tags).

#### CVE Update Policy

.NET container images are regularly monitored for the presence of CVEs. A given image will be rebuilt to pick up fixes for a CVE when:

* We detect the image contains a CVE with a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score of "Critical"
* **AND** the CVE is in a package that is added in our Dockerfile layers (meaning the CVE is in a package we explicitly install or any transitive dependencies of those packages)
* **AND** there is a CVE fix for the package available in the affected base image's package repository.

Please refer to the [Security Policy](https://github.com/dotnet/dotnet-docker/blob/main/SECURITY.md) and [Container Vulnerability Workflow](https://github.com/dotnet/dotnet-docker/blob/main/documentation/vulnerability-reporting.md) for more detail about what to do when a CVE is encountered in a .NET image.

### Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

## License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server](https://www.microsoft.com/cloud-platform/windows-server-pricing)
