**IMPORTANT**

**The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet](https://hub.docker.com/_/microsoft-dotnet-runtime/) for images with official releases of [.NET](https://github.com/dotnet/core).**

# Featured Tags

* `9.0-preview` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime:9.0-preview`
* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime:8.0`
* `7.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime:7.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime:6.0`

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

You can quickly run a container with a pre-built [.NET Docker image](https://hub.docker.com/_/microsoft-dotnet-samples/), based on the [.NET console sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

# Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor/): .NET Monitor Tool (Preview)
* [dotnet/nightly/aspire-dashboard](https://hub.docker.com/_/microsoft-dotnet-nightly-aspire-dashboard/): .NET Aspire Dashboard (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.1-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.1-bookworm-slim, 8.0-bookworm-slim, 8.0.1, 8.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.1-alpine3.18-amd64, 8.0-alpine3.18-amd64, 8.0-alpine-amd64, 8.0.1-alpine3.18, 8.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
8.0.1-alpine3.19-amd64, 8.0-alpine3.19-amd64, 8.0.1-alpine3.19, 8.0-alpine3.19, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
8.0.1-jammy-amd64, 8.0-jammy-amd64, 8.0.1-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.1-jammy-chiseled-amd64, 8.0-jammy-chiseled-amd64, 8.0.1-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.1-jammy-chiseled-extra-amd64, 8.0-jammy-chiseled-extra-amd64, 8.0.1-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
8.0.1-cbl-mariner2.0-amd64, 8.0-cbl-mariner2.0-amd64, 8.0-cbl-mariner-amd64, 8.0.1-cbl-mariner2.0, 8.0-cbl-mariner2.0, 8.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.1-cbl-mariner2.0-distroless-amd64, 8.0-cbl-mariner2.0-distroless-amd64, 8.0-cbl-mariner-distroless-amd64, 8.0.1-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless, 8.0-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0-distroless
7.0.15-bookworm-slim-amd64, 7.0-bookworm-slim-amd64, 7.0.15-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/bookworm-slim/amd64/Dockerfile) | Debian 12
7.0.15-bullseye-slim-amd64, 7.0-bullseye-slim-amd64, 7.0.15-bullseye-slim, 7.0-bullseye-slim, 7.0.15, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/bullseye-slim/amd64/Dockerfile) | Debian 11
7.0.15-alpine3.18-amd64, 7.0-alpine3.18-amd64, 7.0-alpine-amd64, 7.0.15-alpine3.18, 7.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
7.0.15-alpine3.19-amd64, 7.0-alpine3.19-amd64, 7.0.15-alpine3.19, 7.0-alpine3.19, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
7.0.15-jammy-amd64, 7.0-jammy-amd64, 7.0.15-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
7.0.15-jammy-chiseled-amd64, 7.0-jammy-chiseled-amd64, 7.0.15-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
7.0.15-jammy-chiseled-extra-amd64, 7.0-jammy-chiseled-extra-amd64, 7.0.15-jammy-chiseled-extra, 7.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
7.0.15-cbl-mariner2.0-amd64, 7.0-cbl-mariner2.0-amd64, 7.0-cbl-mariner-amd64, 7.0.15-cbl-mariner2.0, 7.0-cbl-mariner2.0, 7.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
7.0.15-cbl-mariner2.0-distroless-amd64, 7.0-cbl-mariner2.0-distroless-amd64, 7.0-cbl-mariner-distroless-amd64, 7.0.15-cbl-mariner2.0-distroless, 7.0-cbl-mariner2.0-distroless, 7.0-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0-distroless
6.0.26-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.26-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.26-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.26-bullseye-slim, 6.0-bullseye-slim, 6.0.26, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.26-alpine3.18-amd64, 6.0-alpine3.18-amd64, 6.0-alpine-amd64, 6.0.26-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
6.0.26-alpine3.19-amd64, 6.0-alpine3.19-amd64, 6.0.26-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
6.0.26-jammy-amd64, 6.0-jammy-amd64, 6.0.26-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.26-jammy-chiseled-amd64, 6.0-jammy-chiseled-amd64, 6.0.26-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.0.26-jammy-chiseled-extra-amd64, 6.0-jammy-chiseled-extra-amd64, 6.0.26-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
6.0.26-focal-amd64, 6.0-focal-amd64, 6.0.26-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04
6.0.26-cbl-mariner2.0-amd64, 6.0-cbl-mariner2.0-amd64, 6.0-cbl-mariner-amd64, 6.0.26-cbl-mariner2.0, 6.0-cbl-mariner2.0, 6.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.26-cbl-mariner2.0-distroless-amd64, 6.0-cbl-mariner2.0-distroless-amd64, 6.0-cbl-mariner-distroless-amd64, 6.0.26-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless, 6.0-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0-distroless

##### .NET 9 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.1-bookworm-slim-amd64, 9.0-preview-bookworm-slim-amd64, 9.0.0-preview.1-bookworm-slim, 9.0-preview-bookworm-slim, 9.0.0-preview.1, 9.0-preview, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/bookworm-slim/amd64/Dockerfile) | Debian 12
9.0.0-preview.1-alpine3.19-amd64, 9.0-preview-alpine3.19-amd64, 9.0-preview-alpine-amd64, 9.0.0-preview.1-alpine3.19, 9.0-preview-alpine3.19, 9.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
9.0.0-preview.1-jammy-amd64, 9.0-preview-jammy-amd64, 9.0.0-preview.1-jammy, 9.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
9.0.0-preview.1-jammy-chiseled-amd64, 9.0-preview-jammy-chiseled-amd64, 9.0.0-preview.1-jammy-chiseled, 9.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
9.0.0-preview.1-jammy-chiseled-extra-amd64, 9.0-preview-jammy-chiseled-extra-amd64, 9.0.0-preview.1-jammy-chiseled-extra, 9.0-preview-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
9.0.0-preview.1-cbl-mariner2.0-amd64, 9.0-preview-cbl-mariner2.0-amd64, 9.0-preview-cbl-mariner-amd64, 9.0.0-preview.1-cbl-mariner2.0, 9.0-preview-cbl-mariner2.0, 9.0-preview-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.1-cbl-mariner2.0-distroless-amd64, 9.0-preview-cbl-mariner2.0-distroless-amd64, 9.0-preview-cbl-mariner-distroless-amd64, 9.0.0-preview.1-cbl-mariner2.0-distroless, 9.0-preview-cbl-mariner2.0-distroless, 9.0-preview-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0-distroless

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.1-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.1-bookworm-slim, 8.0-bookworm-slim, 8.0.1, 8.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.1-alpine3.18-arm64v8, 8.0-alpine3.18-arm64v8, 8.0-alpine-arm64v8, 8.0.1-alpine3.18, 8.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
8.0.1-alpine3.19-arm64v8, 8.0-alpine3.19-arm64v8, 8.0.1-alpine3.19, 8.0-alpine3.19, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
8.0.1-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.1-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.1-jammy-chiseled-arm64v8, 8.0-jammy-chiseled-arm64v8, 8.0.1-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.1-jammy-chiseled-extra-arm64v8, 8.0-jammy-chiseled-extra-arm64v8, 8.0.1-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.1-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner-arm64v8, 8.0.1-cbl-mariner2.0, 8.0-cbl-mariner2.0, 8.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.1-cbl-mariner2.0-distroless-arm64v8, 8.0-cbl-mariner2.0-distroless-arm64v8, 8.0-cbl-mariner-distroless-arm64v8, 8.0.1-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless, 8.0-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0-distroless
7.0.15-bookworm-slim-arm64v8, 7.0-bookworm-slim-arm64v8, 7.0.15-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
7.0.15-bullseye-slim-arm64v8, 7.0-bullseye-slim-arm64v8, 7.0.15-bullseye-slim, 7.0-bullseye-slim, 7.0.15, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
7.0.15-alpine3.18-arm64v8, 7.0-alpine3.18-arm64v8, 7.0-alpine-arm64v8, 7.0.15-alpine3.18, 7.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
7.0.15-alpine3.19-arm64v8, 7.0-alpine3.19-arm64v8, 7.0.15-alpine3.19, 7.0-alpine3.19, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
7.0.15-jammy-arm64v8, 7.0-jammy-arm64v8, 7.0.15-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.15-jammy-chiseled-arm64v8, 7.0-jammy-chiseled-arm64v8, 7.0.15-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.15-jammy-chiseled-extra-arm64v8, 7.0-jammy-chiseled-extra-arm64v8, 7.0.15-jammy-chiseled-extra, 7.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.15-cbl-mariner2.0-arm64v8, 7.0-cbl-mariner2.0-arm64v8, 7.0-cbl-mariner-arm64v8, 7.0.15-cbl-mariner2.0, 7.0-cbl-mariner2.0, 7.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
7.0.15-cbl-mariner2.0-distroless-arm64v8, 7.0-cbl-mariner2.0-distroless-arm64v8, 7.0-cbl-mariner-distroless-arm64v8, 7.0.15-cbl-mariner2.0-distroless, 7.0-cbl-mariner2.0-distroless, 7.0-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0-distroless
6.0.26-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.26-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.26-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.26-bullseye-slim, 6.0-bullseye-slim, 6.0.26, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.26-alpine3.18-arm64v8, 6.0-alpine3.18-arm64v8, 6.0-alpine-arm64v8, 6.0.26-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
6.0.26-alpine3.19-arm64v8, 6.0-alpine3.19-arm64v8, 6.0.26-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
6.0.26-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.26-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.26-jammy-chiseled-arm64v8, 6.0-jammy-chiseled-arm64v8, 6.0.26-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.26-jammy-chiseled-extra-arm64v8, 6.0-jammy-chiseled-extra-arm64v8, 6.0.26-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.26-focal-arm64v8, 6.0-focal-arm64v8, 6.0.26-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04
6.0.26-cbl-mariner2.0-arm64v8, 6.0-cbl-mariner2.0-arm64v8, 6.0-cbl-mariner-arm64v8, 6.0.26-cbl-mariner2.0, 6.0-cbl-mariner2.0, 6.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.26-cbl-mariner2.0-distroless-arm64v8, 6.0-cbl-mariner2.0-distroless-arm64v8, 6.0-cbl-mariner-distroless-arm64v8, 6.0.26-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless, 6.0-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0-distroless

##### .NET 9 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.1-bookworm-slim-arm64v8, 9.0-preview-bookworm-slim-arm64v8, 9.0.0-preview.1-bookworm-slim, 9.0-preview-bookworm-slim, 9.0.0-preview.1, 9.0-preview, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
9.0.0-preview.1-alpine3.19-arm64v8, 9.0-preview-alpine3.19-arm64v8, 9.0-preview-alpine-arm64v8, 9.0.0-preview.1-alpine3.19, 9.0-preview-alpine3.19, 9.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
9.0.0-preview.1-jammy-arm64v8, 9.0-preview-jammy-arm64v8, 9.0.0-preview.1-jammy, 9.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
9.0.0-preview.1-jammy-chiseled-arm64v8, 9.0-preview-jammy-chiseled-arm64v8, 9.0.0-preview.1-jammy-chiseled, 9.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
9.0.0-preview.1-jammy-chiseled-extra-arm64v8, 9.0-preview-jammy-chiseled-extra-arm64v8, 9.0.0-preview.1-jammy-chiseled-extra, 9.0-preview-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
9.0.0-preview.1-cbl-mariner2.0-arm64v8, 9.0-preview-cbl-mariner2.0-arm64v8, 9.0-preview-cbl-mariner-arm64v8, 9.0.0-preview.1-cbl-mariner2.0, 9.0-preview-cbl-mariner2.0, 9.0-preview-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.1-cbl-mariner2.0-distroless-arm64v8, 9.0-preview-cbl-mariner2.0-distroless-arm64v8, 9.0-preview-cbl-mariner-distroless-arm64v8, 9.0.0-preview.1-cbl-mariner2.0-distroless, 9.0-preview-cbl-mariner2.0-distroless, 9.0-preview-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0-distroless

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.1-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.1-bookworm-slim, 8.0-bookworm-slim, 8.0.1, 8.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.1-alpine3.18-arm32v7, 8.0-alpine3.18-arm32v7, 8.0-alpine-arm32v7, 8.0.1-alpine3.18, 8.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
8.0.1-alpine3.19-arm32v7, 8.0-alpine3.19-arm32v7, 8.0.1-alpine3.19, 8.0-alpine3.19, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
8.0.1-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.1-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.1-jammy-chiseled-arm32v7, 8.0-jammy-chiseled-arm32v7, 8.0.1-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.1-jammy-chiseled-extra-arm32v7, 8.0-jammy-chiseled-extra-arm32v7, 8.0.1-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.15-bookworm-slim-arm32v7, 7.0-bookworm-slim-arm32v7, 7.0.15-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
7.0.15-bullseye-slim-arm32v7, 7.0-bullseye-slim-arm32v7, 7.0.15-bullseye-slim, 7.0-bullseye-slim, 7.0.15, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
7.0.15-alpine3.18-arm32v7, 7.0-alpine3.18-arm32v7, 7.0-alpine-arm32v7, 7.0.15-alpine3.18, 7.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
7.0.15-alpine3.19-arm32v7, 7.0-alpine3.19-arm32v7, 7.0.15-alpine3.19, 7.0-alpine3.19, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
7.0.15-jammy-arm32v7, 7.0-jammy-arm32v7, 7.0.15-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.15-jammy-chiseled-arm32v7, 7.0-jammy-chiseled-arm32v7, 7.0.15-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.15-jammy-chiseled-extra-arm32v7, 7.0-jammy-chiseled-extra-arm32v7, 7.0.15-jammy-chiseled-extra, 7.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.26-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.26-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.26-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.26-bullseye-slim, 6.0-bullseye-slim, 6.0.26, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.26-alpine3.18-arm32v7, 6.0-alpine3.18-arm32v7, 6.0-alpine-arm32v7, 6.0.26-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
6.0.26-alpine3.19-arm32v7, 6.0-alpine3.19-arm32v7, 6.0.26-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
6.0.26-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.26-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.26-jammy-chiseled-arm32v7, 6.0-jammy-chiseled-arm32v7, 6.0.26-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.26-jammy-chiseled-extra-arm32v7, 6.0-jammy-chiseled-extra-arm32v7, 6.0.26-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.26-focal-arm32v7, 6.0-focal-arm32v7, 6.0.26-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

##### .NET 9 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.1-bookworm-slim-arm32v7, 9.0-preview-bookworm-slim-arm32v7, 9.0.0-preview.1-bookworm-slim, 9.0-preview-bookworm-slim, 9.0.0-preview.1, 9.0-preview, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
9.0.0-preview.1-alpine3.19-arm32v7, 9.0-preview-alpine3.19-arm32v7, 9.0-preview-alpine-arm32v7, 9.0.0-preview.1-alpine3.19, 9.0-preview-alpine3.19, 9.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
9.0.0-preview.1-jammy-arm32v7, 9.0-preview-jammy-arm32v7, 9.0.0-preview.1-jammy, 9.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
9.0.0-preview.1-jammy-chiseled-arm32v7, 9.0-preview-jammy-chiseled-arm32v7, 9.0.0-preview.1-jammy-chiseled, 9.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
9.0.0-preview.1-jammy-chiseled-extra-arm32v7, 9.0-preview-jammy-chiseled-extra-arm32v7, 9.0.0-preview.1-jammy-chiseled-extra, 9.0-preview-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.1-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/nanoserver-ltsc2022/amd64/Dockerfile)
7.0.15-nanoserver-ltsc2022, 7.0-nanoserver-ltsc2022, 7.0.15, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.26-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.26, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.1-nanoserver-ltsc2022, 9.0-preview-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/nanoserver-ltsc2022/amd64/Dockerfile)

## Windows Server Core 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.1-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)
7.0.15-windowsservercore-ltsc2022, 7.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.26-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.1-windowsservercore-ltsc2022, 9.0-preview-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/windowsservercore-ltsc2022/amd64/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.1-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/nanoserver-1809/amd64/Dockerfile)
7.0.15-nanoserver-1809, 7.0-nanoserver-1809, 7.0.15, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/nanoserver-1809/amd64/Dockerfile)
6.0.26-nanoserver-1809, 6.0-nanoserver-1809, 6.0.26, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/nanoserver-1809/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.1-nanoserver-1809, 9.0-preview-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/nanoserver-1809/amd64/Dockerfile)

## Windows Server Core 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.1-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
7.0.15-windowsservercore-ltsc2019, 7.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/7.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.26-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.1-windowsservercore-ltsc2019, 9.0-preview-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime/9.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/nightly/runtime at https://mcr.microsoft.com/v2/dotnet/nightly/runtime/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core-nightly/runtime repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core-nightly/runtime/tags/list.

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)*

# Support

## Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

## Image Update Policy

* We update the supported .NET images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:ltsc2022, buildpack-deps:bionic-scm, etc.).
* We publish .NET images as part of releasing new versions of .NET including major/minor and servicing.

## Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
