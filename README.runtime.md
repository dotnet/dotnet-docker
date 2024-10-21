# .NET Runtime

## Featured Tags

* `9.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/runtime:9.0`
* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/runtime:8.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/runtime:6.0`

## About

This image contains the .NET runtimes and libraries and is optimized for running .NET apps in production.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Container sample: Run a simple application

You can quickly run a container with a pre-built [.NET Docker image](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md), based on the [.NET console sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

## Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

### Distroless images

.NET "distroless" container images contain only the minimal set of packages .NET needs, with everything else removed.
Due to their limited set of packages, distroless containers have a minimized security attack surface, smaller deployment sizes, and faster start-up time compared to their non-distroless counterparts.
They contain the following features:

* Minimal set of packages required for .NET applications
* Non-root user by default
* No package manager
* No shell

.NET offers distroless images for [Azure Linux](https://github.com/dotnet/dotnet-docker/blob/main/documentation/azurelinux.md) and [Ubuntu (Chiseled)](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md).

## Related Repositories

.NET:

* [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.md): .NET
* [dotnet/sdk](https://github.com/dotnet/dotnet-docker/blob/main/README.sdk.md): .NET SDK
* [dotnet/aspnet](https://github.com/dotnet/dotnet-docker/blob/main/README.aspnet.md): ASP.NET Core Runtime
* [dotnet/runtime-deps](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime-deps.md): .NET Runtime Dependencies
* [dotnet/monitor](https://github.com/dotnet/dotnet-docker/blob/main/README.monitor.md): .NET Monitor Tool
* [dotnet/aspire-dashboard](https://github.com/dotnet/dotnet-docker/blob/main/README.aspire-dashboard.md): .NET Aspire Dashboard
* [dotnet/nightly/runtime](https://github.com/dotnet/dotnet-docker/blob/nightly/README.runtime.md): .NET Runtime (Preview)
* [dotnet/samples](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md): .NET Samples

.NET Framework:

* [dotnet/framework](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.md): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.samples.md): .NET Framework, ASP.NET and WCF Samples

## Full Tag Listing

### Linux amd64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-rc.2-bookworm-slim-amd64, 9.0-bookworm-slim-amd64, 9.0.0-rc.2-bookworm-slim, 9.0-bookworm-slim, 9.0.0-rc.2, 9.0, latest | [Dockerfile](src/runtime/9.0/bookworm-slim/amd64/Dockerfile) | Debian 12
9.0.0-rc.2-alpine3.20-amd64, 9.0-alpine3.20-amd64, 9.0-alpine-amd64, 9.0.0-rc.2-alpine3.20, 9.0-alpine3.20, 9.0-alpine | [Dockerfile](src/runtime/9.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
9.0.0-rc.2-noble-amd64, 9.0-noble-amd64, 9.0.0-rc.2-noble, 9.0-noble | [Dockerfile](src/runtime/9.0/noble/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-rc.2-noble-chiseled-amd64, 9.0-noble-chiseled-amd64, 9.0.0-rc.2-noble-chiseled, 9.0-noble-chiseled | [Dockerfile](src/runtime/9.0/noble-chiseled/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-rc.2-noble-chiseled-extra-amd64, 9.0-noble-chiseled-extra-amd64, 9.0.0-rc.2-noble-chiseled-extra, 9.0-noble-chiseled-extra | [Dockerfile](src/runtime/9.0/noble-chiseled-extra/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-rc.2-azurelinux3.0-amd64, 9.0-azurelinux3.0-amd64, 9.0.0-rc.2-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/runtime/9.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
9.0.0-rc.2-azurelinux3.0-distroless-amd64, 9.0-azurelinux3.0-distroless-amd64, 9.0.0-rc.2-azurelinux3.0-distroless, 9.0-azurelinux3.0-distroless | [Dockerfile](src/runtime/9.0/azurelinux3.0-distroless/amd64/Dockerfile) | Azure Linux 3.0
9.0.0-rc.2-azurelinux3.0-distroless-extra-amd64, 9.0-azurelinux3.0-distroless-extra-amd64, 9.0.0-rc.2-azurelinux3.0-distroless-extra, 9.0-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime/9.0/azurelinux3.0-distroless-extra/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.10-bookworm-slim, 8.0-bookworm-slim, 8.0.10, 8.0 | [Dockerfile](src/runtime/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.10-alpine3.20-amd64, 8.0-alpine3.20-amd64, 8.0-alpine-amd64, 8.0.10-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](src/runtime/8.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
8.0.10-noble-amd64, 8.0-noble-amd64, 8.0.10-noble, 8.0-noble | [Dockerfile](src/runtime/8.0/noble/amd64/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-amd64, 8.0-noble-chiseled-amd64, 8.0.10-noble-chiseled, 8.0-noble-chiseled | [Dockerfile](src/runtime/8.0/noble-chiseled/amd64/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-extra-amd64, 8.0-noble-chiseled-extra-amd64, 8.0.10-noble-chiseled-extra, 8.0-noble-chiseled-extra | [Dockerfile](src/runtime/8.0/noble-chiseled-extra/amd64/Dockerfile) | Ubuntu 24.04
8.0.10-jammy-amd64, 8.0-jammy-amd64, 8.0.10-jammy, 8.0-jammy | [Dockerfile](src/runtime/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-amd64, 8.0-jammy-chiseled-amd64, 8.0.10-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](src/runtime/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-extra-amd64, 8.0-jammy-chiseled-extra-amd64, 8.0.10-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](src/runtime/8.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
8.0.10-azurelinux3.0-amd64, 8.0-azurelinux3.0-amd64, 8.0.10-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/runtime/8.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-amd64, 8.0-azurelinux3.0-distroless-amd64, 8.0.10-azurelinux3.0-distroless, 8.0-azurelinux3.0-distroless | [Dockerfile](src/runtime/8.0/azurelinux3.0-distroless/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-extra-amd64, 8.0-azurelinux3.0-distroless-extra-amd64, 8.0.10-azurelinux3.0-distroless-extra, 8.0-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime/8.0/azurelinux3.0-distroless-extra/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-cbl-mariner2.0-amd64, 8.0-cbl-mariner2.0-amd64, 8.0.10-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](src/runtime/8.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-amd64, 8.0-cbl-mariner2.0-distroless-amd64, 8.0.10-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](src/runtime/8.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-extra-amd64, 8.0-cbl-mariner2.0-distroless-extra-amd64, 8.0.10-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](src/runtime/8.0/cbl-mariner2.0-distroless-extra/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.35-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.35-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](src/runtime/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.35-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.35-bullseye-slim, 6.0-bullseye-slim, 6.0.35, 6.0 | [Dockerfile](src/runtime/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.35-alpine3.20-amd64, 6.0-alpine3.20-amd64, 6.0-alpine-amd64, 6.0.35-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](src/runtime/6.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
6.0.35-jammy-amd64, 6.0-jammy-amd64, 6.0.35-jammy, 6.0-jammy | [Dockerfile](src/runtime/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-amd64, 6.0-jammy-chiseled-amd64, 6.0.35-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](src/runtime/6.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-extra-amd64, 6.0-jammy-chiseled-extra-amd64, 6.0.35-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](src/runtime/6.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
6.0.35-focal-amd64, 6.0-focal-amd64, 6.0.35-focal, 6.0-focal | [Dockerfile](src/runtime/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04
6.0.35-cbl-mariner2.0-amd64, 6.0-cbl-mariner2.0-amd64, 6.0.35-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](src/runtime/6.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.35-cbl-mariner2.0-distroless-amd64, 6.0-cbl-mariner2.0-distroless-amd64, 6.0.35-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless | [Dockerfile](src/runtime/6.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

### Linux arm64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-rc.2-bookworm-slim-arm64v8, 9.0-bookworm-slim-arm64v8, 9.0.0-rc.2-bookworm-slim, 9.0-bookworm-slim, 9.0.0-rc.2, 9.0, latest | [Dockerfile](src/runtime/9.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
9.0.0-rc.2-alpine3.20-arm64v8, 9.0-alpine3.20-arm64v8, 9.0-alpine-arm64v8, 9.0.0-rc.2-alpine3.20, 9.0-alpine3.20, 9.0-alpine | [Dockerfile](src/runtime/9.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
9.0.0-rc.2-noble-arm64v8, 9.0-noble-arm64v8, 9.0.0-rc.2-noble, 9.0-noble | [Dockerfile](src/runtime/9.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-rc.2-noble-chiseled-arm64v8, 9.0-noble-chiseled-arm64v8, 9.0.0-rc.2-noble-chiseled, 9.0-noble-chiseled | [Dockerfile](src/runtime/9.0/noble-chiseled/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-rc.2-noble-chiseled-extra-arm64v8, 9.0-noble-chiseled-extra-arm64v8, 9.0.0-rc.2-noble-chiseled-extra, 9.0-noble-chiseled-extra | [Dockerfile](src/runtime/9.0/noble-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-rc.2-azurelinux3.0-arm64v8, 9.0-azurelinux3.0-arm64v8, 9.0.0-rc.2-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/runtime/9.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.0-rc.2-azurelinux3.0-distroless-arm64v8, 9.0-azurelinux3.0-distroless-arm64v8, 9.0.0-rc.2-azurelinux3.0-distroless, 9.0-azurelinux3.0-distroless | [Dockerfile](src/runtime/9.0/azurelinux3.0-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.0-rc.2-azurelinux3.0-distroless-extra-arm64v8, 9.0-azurelinux3.0-distroless-extra-arm64v8, 9.0.0-rc.2-azurelinux3.0-distroless-extra, 9.0-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime/9.0/azurelinux3.0-distroless-extra/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.10-bookworm-slim, 8.0-bookworm-slim, 8.0.10, 8.0 | [Dockerfile](src/runtime/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.10-alpine3.20-arm64v8, 8.0-alpine3.20-arm64v8, 8.0-alpine-arm64v8, 8.0.10-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](src/runtime/8.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
8.0.10-noble-arm64v8, 8.0-noble-arm64v8, 8.0.10-noble, 8.0-noble | [Dockerfile](src/runtime/8.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-arm64v8, 8.0-noble-chiseled-arm64v8, 8.0.10-noble-chiseled, 8.0-noble-chiseled | [Dockerfile](src/runtime/8.0/noble-chiseled/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-extra-arm64v8, 8.0-noble-chiseled-extra-arm64v8, 8.0.10-noble-chiseled-extra, 8.0-noble-chiseled-extra | [Dockerfile](src/runtime/8.0/noble-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.10-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.10-jammy, 8.0-jammy | [Dockerfile](src/runtime/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-arm64v8, 8.0-jammy-chiseled-arm64v8, 8.0.10-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](src/runtime/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-extra-arm64v8, 8.0-jammy-chiseled-extra-arm64v8, 8.0.10-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](src/runtime/8.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.10-azurelinux3.0-arm64v8, 8.0-azurelinux3.0-arm64v8, 8.0.10-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/runtime/8.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-arm64v8, 8.0-azurelinux3.0-distroless-arm64v8, 8.0.10-azurelinux3.0-distroless, 8.0-azurelinux3.0-distroless | [Dockerfile](src/runtime/8.0/azurelinux3.0-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-extra-arm64v8, 8.0-azurelinux3.0-distroless-extra-arm64v8, 8.0.10-azurelinux3.0-distroless-extra, 8.0-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime/8.0/azurelinux3.0-distroless-extra/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner2.0-arm64v8, 8.0.10-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](src/runtime/8.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-arm64v8, 8.0-cbl-mariner2.0-distroless-arm64v8, 8.0.10-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](src/runtime/8.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-extra-arm64v8, 8.0-cbl-mariner2.0-distroless-extra-arm64v8, 8.0.10-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](src/runtime/8.0/cbl-mariner2.0-distroless-extra/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.35-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.35-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](src/runtime/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.35-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.35-bullseye-slim, 6.0-bullseye-slim, 6.0.35, 6.0 | [Dockerfile](src/runtime/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.35-alpine3.20-arm64v8, 6.0-alpine3.20-arm64v8, 6.0-alpine-arm64v8, 6.0.35-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](src/runtime/6.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
6.0.35-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.35-jammy, 6.0-jammy | [Dockerfile](src/runtime/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-arm64v8, 6.0-jammy-chiseled-arm64v8, 6.0.35-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](src/runtime/6.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-extra-arm64v8, 6.0-jammy-chiseled-extra-arm64v8, 6.0.35-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](src/runtime/6.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.35-focal-arm64v8, 6.0-focal-arm64v8, 6.0.35-focal, 6.0-focal | [Dockerfile](src/runtime/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04
6.0.35-cbl-mariner2.0-arm64v8, 6.0-cbl-mariner2.0-arm64v8, 6.0.35-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](src/runtime/6.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.35-cbl-mariner2.0-distroless-arm64v8, 6.0-cbl-mariner2.0-distroless-arm64v8, 6.0.35-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless | [Dockerfile](src/runtime/6.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

### Linux arm32 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-rc.2-bookworm-slim-arm32v7, 9.0-bookworm-slim-arm32v7, 9.0.0-rc.2-bookworm-slim, 9.0-bookworm-slim, 9.0.0-rc.2, 9.0, latest | [Dockerfile](src/runtime/9.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
9.0.0-rc.2-alpine3.20-arm32v7, 9.0-alpine3.20-arm32v7, 9.0-alpine-arm32v7, 9.0.0-rc.2-alpine3.20, 9.0-alpine3.20, 9.0-alpine | [Dockerfile](src/runtime/9.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
9.0.0-rc.2-noble-arm32v7, 9.0-noble-arm32v7, 9.0.0-rc.2-noble, 9.0-noble | [Dockerfile](src/runtime/9.0/noble/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.0-rc.2-noble-chiseled-arm32v7, 9.0-noble-chiseled-arm32v7, 9.0.0-rc.2-noble-chiseled, 9.0-noble-chiseled | [Dockerfile](src/runtime/9.0/noble-chiseled/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.0-rc.2-noble-chiseled-extra-arm32v7, 9.0-noble-chiseled-extra-arm32v7, 9.0.0-rc.2-noble-chiseled-extra, 9.0-noble-chiseled-extra | [Dockerfile](src/runtime/9.0/noble-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 24.04
8.0.10-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.10-bookworm-slim, 8.0-bookworm-slim, 8.0.10, 8.0 | [Dockerfile](src/runtime/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.10-alpine3.20-arm32v7, 8.0-alpine3.20-arm32v7, 8.0-alpine-arm32v7, 8.0.10-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](src/runtime/8.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
8.0.10-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.10-jammy, 8.0-jammy | [Dockerfile](src/runtime/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-arm32v7, 8.0-jammy-chiseled-arm32v7, 8.0.10-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](src/runtime/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-extra-arm32v7, 8.0-jammy-chiseled-extra-arm32v7, 8.0.10-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](src/runtime/8.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.35-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.35-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](src/runtime/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.35-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.35-bullseye-slim, 6.0-bullseye-slim, 6.0.35, 6.0 | [Dockerfile](src/runtime/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.35-alpine3.20-arm32v7, 6.0-alpine3.20-arm32v7, 6.0-alpine-arm32v7, 6.0.35-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](src/runtime/6.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
6.0.35-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.35-jammy, 6.0-jammy | [Dockerfile](src/runtime/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-arm32v7, 6.0-jammy-chiseled-arm32v7, 6.0.35-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](src/runtime/6.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-extra-arm32v7, 6.0-jammy-chiseled-extra-arm32v7, 6.0.35-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](src/runtime/6.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.35-focal-arm32v7, 6.0-focal-arm32v7, 6.0.35-focal, 6.0-focal | [Dockerfile](src/runtime/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

### Nano Server 2022 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-rc.2-nanoserver-ltsc2022, 9.0-nanoserver-ltsc2022 | [Dockerfile](src/runtime/9.0/nanoserver-ltsc2022/amd64/Dockerfile)
8.0.10-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](src/runtime/8.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.35-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.35, 6.0 | [Dockerfile](src/runtime/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

### Windows Server Core 2022 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-rc.2-windowsservercore-ltsc2022, 9.0-windowsservercore-ltsc2022 | [Dockerfile](src/runtime/9.0/windowsservercore-ltsc2022/amd64/Dockerfile)
8.0.10-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](src/runtime/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.35-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](src/runtime/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

### Nano Server, version 1809 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-rc.2-nanoserver-1809, 9.0-nanoserver-1809 | [Dockerfile](src/runtime/9.0/nanoserver-1809/amd64/Dockerfile)
8.0.10-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](src/runtime/8.0/nanoserver-1809/amd64/Dockerfile)
6.0.35-nanoserver-1809, 6.0-nanoserver-1809, 6.0.35, 6.0 | [Dockerfile](src/runtime/6.0/nanoserver-1809/amd64/Dockerfile)

### Windows Server Core 2019 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-rc.2-windowsservercore-ltsc2019, 9.0-windowsservercore-ltsc2019 | [Dockerfile](src/runtime/9.0/windowsservercore-ltsc2019/amd64/Dockerfile)
8.0.10-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](src/runtime/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.35-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](src/runtime/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/dotnet/runtime/tags/list) for all supported and unsupported tags.*

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
