# Featured Tags

* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/runtime:8.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/runtime:6.0`

# About

This image contains the .NET runtimes and libraries and is optimized for running .NET apps in production.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## New: Ubuntu Chiseled Images

Ubuntu Chiseled .NET images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface by including only the minimal set of packages required to run .NET applications.

Please see the [Ubuntu Chiseled + .NET](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md) documentation page for more info.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a simple application

You can quickly run a container with a pre-built [.NET Docker image](https://hub.docker.com/r/microsoft/dotnet-samples/), based on the [.NET console sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

# Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/r/microsoft/dotnet/): .NET
* [dotnet/sdk](https://hub.docker.com/r/microsoft/dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/r/microsoft/dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime-deps](https://hub.docker.com/r/microsoft/dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor](https://hub.docker.com/r/microsoft/dotnet-monitor/): .NET Monitor Tool
* [dotnet/aspire-dashboard](https://hub.docker.com/r/microsoft/dotnet-aspire-dashboard/): .NET Aspire Dashboard
* [dotnet/samples](https://hub.docker.com/r/microsoft/dotnet-samples/): .NET Samples
* [dotnet/nightly/runtime](https://hub.docker.com/r/microsoft/dotnet-nightly-runtime/): .NET Runtime (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/r/microsoft/dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.7-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.7-bookworm-slim, 8.0-bookworm-slim, 8.0.7, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.7-alpine3.20-amd64, 8.0-alpine3.20-amd64, 8.0-alpine-amd64, 8.0.7-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
8.0.7-alpine3.19-amd64, 8.0-alpine3.19-amd64, 8.0.7-alpine3.19, 8.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
8.0.7-noble-amd64, 8.0-noble-amd64, 8.0.7-noble, 8.0-noble | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/noble/amd64/Dockerfile) | Ubuntu 24.04
8.0.7-noble-chiseled-amd64, 8.0-noble-chiseled-amd64, 8.0.7-noble-chiseled, 8.0-noble-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/noble-chiseled/amd64/Dockerfile) | Ubuntu 24.04
8.0.7-noble-chiseled-extra-amd64, 8.0-noble-chiseled-extra-amd64, 8.0.7-noble-chiseled-extra, 8.0-noble-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/noble-chiseled-extra/amd64/Dockerfile) | Ubuntu 24.04
8.0.7-jammy-amd64, 8.0-jammy-amd64, 8.0.7-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.7-jammy-chiseled-amd64, 8.0-jammy-chiseled-amd64, 8.0.7-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.7-jammy-chiseled-extra-amd64, 8.0-jammy-chiseled-extra-amd64, 8.0.7-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
8.0.7-cbl-mariner2.0-amd64, 8.0-cbl-mariner2.0-amd64, 8.0.7-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.7-cbl-mariner2.0-distroless-amd64, 8.0-cbl-mariner2.0-distroless-amd64, 8.0.7-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.7-cbl-mariner2.0-distroless-extra-amd64, 8.0-cbl-mariner2.0-distroless-extra-amd64, 8.0.7-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/cbl-mariner2.0-distroless-extra/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.32-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.32-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.32-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.32-bullseye-slim, 6.0-bullseye-slim, 6.0.32, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.32-alpine3.20-amd64, 6.0-alpine3.20-amd64, 6.0-alpine-amd64, 6.0.32-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
6.0.32-alpine3.19-amd64, 6.0-alpine3.19-amd64, 6.0.32-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
6.0.32-jammy-amd64, 6.0-jammy-amd64, 6.0.32-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.32-jammy-chiseled-amd64, 6.0-jammy-chiseled-amd64, 6.0.32-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.0.32-jammy-chiseled-extra-amd64, 6.0-jammy-chiseled-extra-amd64, 6.0.32-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
6.0.32-focal-amd64, 6.0-focal-amd64, 6.0.32-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04
6.0.32-cbl-mariner2.0-amd64, 6.0-cbl-mariner2.0-amd64, 6.0.32-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.32-cbl-mariner2.0-distroless-amd64, 6.0-cbl-mariner2.0-distroless-amd64, 6.0.32-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

##### .NET 9 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.6-bookworm-slim-amd64, 9.0-preview-bookworm-slim-amd64, 9.0.0-preview.6-bookworm-slim, 9.0-preview-bookworm-slim, 9.0.0-preview.6, 9.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/bookworm-slim/amd64/Dockerfile) | Debian 12
9.0.0-preview.6-alpine3.20-amd64, 9.0-preview-alpine3.20-amd64, 9.0-preview-alpine-amd64, 9.0.0-preview.6-alpine3.20, 9.0-preview-alpine3.20, 9.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
9.0.0-preview.6-noble-amd64, 9.0-preview-noble-amd64, 9.0.0-preview.6-noble, 9.0-preview-noble | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/noble/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-preview.6-noble-chiseled-amd64, 9.0-preview-noble-chiseled-amd64, 9.0.0-preview.6-noble-chiseled, 9.0-preview-noble-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/noble-chiseled/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-preview.6-noble-chiseled-extra-amd64, 9.0-preview-noble-chiseled-extra-amd64, 9.0.0-preview.6-noble-chiseled-extra, 9.0-preview-noble-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/noble-chiseled-extra/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-preview.6-azurelinux3.0-amd64, 9.0-preview-azurelinux3.0-amd64, 9.0.0-preview.6-azurelinux3.0, 9.0-preview-azurelinux3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
9.0.0-preview.6-azurelinux3.0-distroless-amd64, 9.0-preview-azurelinux3.0-distroless-amd64, 9.0.0-preview.6-azurelinux3.0-distroless, 9.0-preview-azurelinux3.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/azurelinux3.0-distroless/amd64/Dockerfile) | Azure Linux 3.0
9.0.0-preview.6-azurelinux3.0-distroless-extra-amd64, 9.0-preview-azurelinux3.0-distroless-extra-amd64, 9.0.0-preview.6-azurelinux3.0-distroless-extra, 9.0-preview-azurelinux3.0-distroless-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/azurelinux3.0-distroless-extra/amd64/Dockerfile) | Azure Linux 3.0

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.7-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.7-bookworm-slim, 8.0-bookworm-slim, 8.0.7, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.7-alpine3.20-arm64v8, 8.0-alpine3.20-arm64v8, 8.0-alpine-arm64v8, 8.0.7-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
8.0.7-alpine3.19-arm64v8, 8.0-alpine3.19-arm64v8, 8.0.7-alpine3.19, 8.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
8.0.7-noble-arm64v8, 8.0-noble-arm64v8, 8.0.7-noble, 8.0-noble | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.7-noble-chiseled-arm64v8, 8.0-noble-chiseled-arm64v8, 8.0.7-noble-chiseled, 8.0-noble-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/noble-chiseled/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.7-noble-chiseled-extra-arm64v8, 8.0-noble-chiseled-extra-arm64v8, 8.0.7-noble-chiseled-extra, 8.0-noble-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/noble-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.7-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.7-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.7-jammy-chiseled-arm64v8, 8.0-jammy-chiseled-arm64v8, 8.0.7-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.7-jammy-chiseled-extra-arm64v8, 8.0-jammy-chiseled-extra-arm64v8, 8.0.7-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.7-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner2.0-arm64v8, 8.0.7-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.7-cbl-mariner2.0-distroless-arm64v8, 8.0-cbl-mariner2.0-distroless-arm64v8, 8.0.7-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.7-cbl-mariner2.0-distroless-extra-arm64v8, 8.0-cbl-mariner2.0-distroless-extra-arm64v8, 8.0.7-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/cbl-mariner2.0-distroless-extra/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.32-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.32-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.32-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.32-bullseye-slim, 6.0-bullseye-slim, 6.0.32, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.32-alpine3.20-arm64v8, 6.0-alpine3.20-arm64v8, 6.0-alpine-arm64v8, 6.0.32-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
6.0.32-alpine3.19-arm64v8, 6.0-alpine3.19-arm64v8, 6.0.32-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
6.0.32-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.32-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.32-jammy-chiseled-arm64v8, 6.0-jammy-chiseled-arm64v8, 6.0.32-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.32-jammy-chiseled-extra-arm64v8, 6.0-jammy-chiseled-extra-arm64v8, 6.0.32-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.32-focal-arm64v8, 6.0-focal-arm64v8, 6.0.32-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04
6.0.32-cbl-mariner2.0-arm64v8, 6.0-cbl-mariner2.0-arm64v8, 6.0.32-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.32-cbl-mariner2.0-distroless-arm64v8, 6.0-cbl-mariner2.0-distroless-arm64v8, 6.0.32-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

##### .NET 9 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.6-bookworm-slim-arm64v8, 9.0-preview-bookworm-slim-arm64v8, 9.0.0-preview.6-bookworm-slim, 9.0-preview-bookworm-slim, 9.0.0-preview.6, 9.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
9.0.0-preview.6-alpine3.20-arm64v8, 9.0-preview-alpine3.20-arm64v8, 9.0-preview-alpine-arm64v8, 9.0.0-preview.6-alpine3.20, 9.0-preview-alpine3.20, 9.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
9.0.0-preview.6-noble-arm64v8, 9.0-preview-noble-arm64v8, 9.0.0-preview.6-noble, 9.0-preview-noble | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-preview.6-noble-chiseled-arm64v8, 9.0-preview-noble-chiseled-arm64v8, 9.0.0-preview.6-noble-chiseled, 9.0-preview-noble-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/noble-chiseled/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-preview.6-noble-chiseled-extra-arm64v8, 9.0-preview-noble-chiseled-extra-arm64v8, 9.0.0-preview.6-noble-chiseled-extra, 9.0-preview-noble-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/noble-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-preview.6-azurelinux3.0-arm64v8, 9.0-preview-azurelinux3.0-arm64v8, 9.0.0-preview.6-azurelinux3.0, 9.0-preview-azurelinux3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.0-preview.6-azurelinux3.0-distroless-arm64v8, 9.0-preview-azurelinux3.0-distroless-arm64v8, 9.0.0-preview.6-azurelinux3.0-distroless, 9.0-preview-azurelinux3.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/azurelinux3.0-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.0-preview.6-azurelinux3.0-distroless-extra-arm64v8, 9.0-preview-azurelinux3.0-distroless-extra-arm64v8, 9.0.0-preview.6-azurelinux3.0-distroless-extra, 9.0-preview-azurelinux3.0-distroless-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/azurelinux3.0-distroless-extra/arm64v8/Dockerfile) | Azure Linux 3.0

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.7-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.7-bookworm-slim, 8.0-bookworm-slim, 8.0.7, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.7-alpine3.20-arm32v7, 8.0-alpine3.20-arm32v7, 8.0-alpine-arm32v7, 8.0.7-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
8.0.7-alpine3.19-arm32v7, 8.0-alpine3.19-arm32v7, 8.0.7-alpine3.19, 8.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
8.0.7-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.7-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.7-jammy-chiseled-arm32v7, 8.0-jammy-chiseled-arm32v7, 8.0.7-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.7-jammy-chiseled-extra-arm32v7, 8.0-jammy-chiseled-extra-arm32v7, 8.0.7-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.32-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.32-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.32-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.32-bullseye-slim, 6.0-bullseye-slim, 6.0.32, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.32-alpine3.20-arm32v7, 6.0-alpine3.20-arm32v7, 6.0-alpine-arm32v7, 6.0.32-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
6.0.32-alpine3.19-arm32v7, 6.0-alpine3.19-arm32v7, 6.0.32-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
6.0.32-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.32-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.32-jammy-chiseled-arm32v7, 6.0-jammy-chiseled-arm32v7, 6.0.32-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.32-jammy-chiseled-extra-arm32v7, 6.0-jammy-chiseled-extra-arm32v7, 6.0.32-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.32-focal-arm32v7, 6.0-focal-arm32v7, 6.0.32-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

##### .NET 9 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.6-bookworm-slim-arm32v7, 9.0-preview-bookworm-slim-arm32v7, 9.0.0-preview.6-bookworm-slim, 9.0-preview-bookworm-slim, 9.0.0-preview.6, 9.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
9.0.0-preview.6-alpine3.20-arm32v7, 9.0-preview-alpine3.20-arm32v7, 9.0-preview-alpine-arm32v7, 9.0.0-preview.6-alpine3.20, 9.0-preview-alpine3.20, 9.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
9.0.0-preview.6-noble-arm32v7, 9.0-preview-noble-arm32v7, 9.0.0-preview.6-noble, 9.0-preview-noble | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/noble/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.0-preview.6-noble-chiseled-arm32v7, 9.0-preview-noble-chiseled-arm32v7, 9.0.0-preview.6-noble-chiseled, 9.0-preview-noble-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/noble-chiseled/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.0-preview.6-noble-chiseled-extra-arm32v7, 9.0-preview-noble-chiseled-extra-arm32v7, 9.0.0-preview.6-noble-chiseled-extra, 9.0-preview-noble-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/noble-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 24.04

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.7-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.32-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.32, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.6-nanoserver-ltsc2022, 9.0-preview-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/nanoserver-ltsc2022/amd64/Dockerfile)

## Windows Server Core 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.7-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.32-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.6-windowsservercore-ltsc2022, 9.0-preview-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/windowsservercore-ltsc2022/amd64/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.7-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/nanoserver-1809/amd64/Dockerfile)
6.0.32-nanoserver-1809, 6.0-nanoserver-1809, 6.0.32, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/nanoserver-1809/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.6-nanoserver-1809, 9.0-preview-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/nanoserver-1809/amd64/Dockerfile)

## Windows Server Core 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.7-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.32-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.6-windowsservercore-ltsc2019, 9.0-preview-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/runtime at https://mcr.microsoft.com/v2/dotnet/runtime/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core/runtime repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core/runtime/tags/list.

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)*

# Support

## Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

## Image Update Policy

* We update supported .NET images within 12 hours of any updates to their base images (e.g. debian:bookworm-slim, windows/nanoserver:ltsc2022, etc.).
* We re-build all .NET images as part of releasing new versions of .NET including new major/minor versions and servicing.
* Distroless images such as Ubuntu Chiseled have no base image, and as such will only be updated with .NET releases and CVE fixes as described below.

### CVE Update Policy

.NET container images are regularly monitored for the presence of CVEs. A given image will be rebuilt to pick up fixes for a CVE when:
* We detect the image contains a CVE with a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score of "Critical"
* **AND** the CVE is in a package that is added in our Dockerfile layers (meaning the CVE is in a package we explicitly install or any transitive dependencies of those packages)
* **AND** there is a CVE fix for the package available in the affected base image's package repository.

Please refer to the [Security Policy](https://github.com/dotnet/dotnet-docker/blob/main/SECURITY.md) and [Container Vulnerability Workflow](https://github.com/dotnet/dotnet-docker/blob/main/documentation/vulnerability-reporting.md) for more detail about what to do when a CVE is encountered in a .NET image.

## Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server](https://www.microsoft.com/cloud-platform/windows-server-pricing)
