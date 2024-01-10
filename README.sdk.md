# Featured Tags

* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:8.0`
* `7.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:7.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:6.0`

# About

This image contains the .NET SDK which is comprised of three parts:

1. .NET CLI
1. .NET runtime
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Building .NET Apps with Docker

* [.NET Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

## Develop .NET Apps in a Container

The following samples show how to develop, build and test .NET applications with Docker without the need to install the .NET SDK.

* [Build .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/build-in-sdk-container.md)
* [Test .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-tests-in-sdk-container.md)
* [Run .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-in-sdk-container.md)

# Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.101-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.101-bookworm-slim, 8.0-bookworm-slim, 8.0.101, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.101-alpine3.18-amd64, 8.0-alpine3.18-amd64, 8.0-alpine-amd64, 8.0.101-alpine3.18, 8.0-alpine3.18, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
8.0.101-alpine3.19-amd64, 8.0-alpine3.19-amd64, 8.0.101-alpine3.19, 8.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
8.0.101-jammy-amd64, 8.0-jammy-amd64, 8.0.101-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.101-cbl-mariner2.0-amd64, 8.0-cbl-mariner2.0-amd64, 8.0-cbl-mariner-amd64, 8.0.101-cbl-mariner2.0, 8.0-cbl-mariner2.0, 8.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
7.0.405-bookworm-slim-amd64, 7.0-bookworm-slim-amd64, 7.0.405-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bookworm-slim/amd64/Dockerfile) | Debian 12
7.0.405-bullseye-slim-amd64, 7.0-bullseye-slim-amd64, 7.0.405-bullseye-slim, 7.0-bullseye-slim, 7.0.405, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bullseye-slim/amd64/Dockerfile) | Debian 11
7.0.405-alpine3.18-amd64, 7.0-alpine3.18-amd64, 7.0-alpine-amd64, 7.0.405-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
7.0.405-alpine3.19-amd64, 7.0-alpine3.19-amd64, 7.0.405-alpine3.19, 7.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
7.0.405-jammy-amd64, 7.0-jammy-amd64, 7.0.405-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
7.0.405-cbl-mariner2.0-amd64, 7.0-cbl-mariner2.0-amd64, 7.0-cbl-mariner-amd64, 7.0.405-cbl-mariner2.0, 7.0-cbl-mariner2.0, 7.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.418-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.418-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.418-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.418-bullseye-slim, 6.0-bullseye-slim, 6.0.418, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.418-alpine3.18-amd64, 6.0-alpine3.18-amd64, 6.0-alpine-amd64, 6.0.418-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
6.0.418-alpine3.19-amd64, 6.0-alpine3.19-amd64, 6.0.418-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
6.0.418-jammy-amd64, 6.0-jammy-amd64, 6.0.418-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.418-cbl-mariner2.0-amd64, 6.0-cbl-mariner2.0-amd64, 6.0-cbl-mariner-amd64, 6.0.418-cbl-mariner2.0, 6.0-cbl-mariner2.0, 6.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.418-focal-amd64, 6.0-focal-amd64, 6.0.418-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.101-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.101-bookworm-slim, 8.0-bookworm-slim, 8.0.101, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.101-alpine3.18-arm64v8, 8.0-alpine3.18-arm64v8, 8.0-alpine-arm64v8, 8.0.101-alpine3.18, 8.0-alpine3.18, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
8.0.101-alpine3.19-arm64v8, 8.0-alpine3.19-arm64v8, 8.0.101-alpine3.19, 8.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
8.0.101-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.101-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.101-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner-arm64v8, 8.0.101-cbl-mariner2.0, 8.0-cbl-mariner2.0, 8.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
7.0.405-bookworm-slim-arm64v8, 7.0-bookworm-slim-arm64v8, 7.0.405-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
7.0.405-bullseye-slim-arm64v8, 7.0-bullseye-slim-arm64v8, 7.0.405-bullseye-slim, 7.0-bullseye-slim, 7.0.405, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
7.0.405-alpine3.18-arm64v8, 7.0-alpine3.18-arm64v8, 7.0-alpine-arm64v8, 7.0.405-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
7.0.405-alpine3.19-arm64v8, 7.0-alpine3.19-arm64v8, 7.0.405-alpine3.19, 7.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
7.0.405-jammy-arm64v8, 7.0-jammy-arm64v8, 7.0.405-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.405-cbl-mariner2.0-arm64v8, 7.0-cbl-mariner2.0-arm64v8, 7.0-cbl-mariner-arm64v8, 7.0.405-cbl-mariner2.0, 7.0-cbl-mariner2.0, 7.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.418-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.418-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.418-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.418-bullseye-slim, 6.0-bullseye-slim, 6.0.418, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.418-alpine3.18-arm64v8, 6.0-alpine3.18-arm64v8, 6.0-alpine-arm64v8, 6.0.418-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
6.0.418-alpine3.19-arm64v8, 6.0-alpine3.19-arm64v8, 6.0.418-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
6.0.418-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.418-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.418-cbl-mariner2.0-arm64v8, 6.0-cbl-mariner2.0-arm64v8, 6.0-cbl-mariner-arm64v8, 6.0.418-cbl-mariner2.0, 6.0-cbl-mariner2.0, 6.0-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.418-focal-arm64v8, 6.0-focal-arm64v8, 6.0.418-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.101-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.101-bookworm-slim, 8.0-bookworm-slim, 8.0.101, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.101-alpine3.18-arm32v7, 8.0-alpine3.18-arm32v7, 8.0-alpine-arm32v7, 8.0.101-alpine3.18, 8.0-alpine3.18, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
8.0.101-alpine3.19-arm32v7, 8.0-alpine3.19-arm32v7, 8.0.101-alpine3.19, 8.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
8.0.101-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.101-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.405-bookworm-slim-arm32v7, 7.0-bookworm-slim-arm32v7, 7.0.405-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
7.0.405-bullseye-slim-arm32v7, 7.0-bullseye-slim-arm32v7, 7.0.405-bullseye-slim, 7.0-bullseye-slim, 7.0.405, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
7.0.405-alpine3.18-arm32v7, 7.0-alpine3.18-arm32v7, 7.0-alpine-arm32v7, 7.0.405-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
7.0.405-alpine3.19-arm32v7, 7.0-alpine3.19-arm32v7, 7.0.405-alpine3.19, 7.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
7.0.405-jammy-arm32v7, 7.0-jammy-arm32v7, 7.0.405-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.418-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.418-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.418-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.418-bullseye-slim, 6.0-bullseye-slim, 6.0.418, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.418-alpine3.18-arm32v7, 6.0-alpine3.18-arm32v7, 6.0-alpine-arm32v7, 6.0.418-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
6.0.418-alpine3.19-arm32v7, 6.0-alpine3.19-arm32v7, 6.0.418-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
6.0.418-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.418-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.418-focal-arm32v7, 6.0-focal-arm32v7, 6.0.418-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.101-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/nanoserver-ltsc2022/amd64/Dockerfile)
7.0.405-nanoserver-ltsc2022, 7.0-nanoserver-ltsc2022, 7.0.405, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.418-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.418, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

## Windows Server Core 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.101-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)
7.0.405-windowsservercore-ltsc2022, 7.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.418-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.101-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/nanoserver-1809/amd64/Dockerfile)
7.0.405-nanoserver-1809, 7.0-nanoserver-1809, 7.0.405, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/nanoserver-1809/amd64/Dockerfile)
6.0.418-nanoserver-1809, 6.0-nanoserver-1809, 6.0.418, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/nanoserver-1809/amd64/Dockerfile)

## Windows Server Core 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.101-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
7.0.405-windowsservercore-ltsc2019, 7.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.418-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/sdk at https://mcr.microsoft.com/v2/dotnet/sdk/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core/sdk repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core/sdk/tags/list.

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
