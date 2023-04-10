# Featured Tags

* `7.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/runtime:7.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/runtime:6.0`

# About

This image contains the .NET runtimes and libraries and is optimized for running .NET apps in production.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a simple application

You can quickly run a container with a pre-built [.NET Docker image](https://hub.docker.com/_/microsoft-dotnet-samples/), based on the [.NET console sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.5-bullseye-slim-amd64, 7.0-bullseye-slim-amd64, 7.0.5-bullseye-slim, 7.0-bullseye-slim, 7.0.5, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/bullseye-slim/amd64/Dockerfile) | Debian 11
7.0.5-alpine3.17-amd64, 7.0-alpine3.17-amd64, 7.0-alpine-amd64, 7.0.5-alpine3.17, 7.0-alpine3.17, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/alpine3.17/amd64/Dockerfile) | Alpine 3.17
7.0.5-jammy-amd64, 7.0-jammy-amd64, 7.0.5-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.16-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.16-bullseye-slim, 6.0-bullseye-slim, 6.0.16, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.16-alpine3.17-amd64, 6.0-alpine3.17-amd64, 6.0-alpine-amd64, 6.0.16-alpine3.17, 6.0-alpine3.17, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/alpine3.17/amd64/Dockerfile) | Alpine 3.17
6.0.16-jammy-amd64, 6.0-jammy-amd64, 6.0.16-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.16-focal-amd64, 6.0-focal-amd64, 6.0.16-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04

##### .NET 8 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-preview.3-bookworm-slim-amd64, 8.0-preview-bookworm-slim-amd64, 8.0.0-preview.3-bookworm-slim, 8.0-preview-bookworm-slim, 8.0.0-preview.3, 8.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.0-preview.3-alpine3.17-amd64, 8.0-preview-alpine3.17-amd64, 8.0-preview-alpine-amd64, 8.0.0-preview.3-alpine3.17, 8.0-preview-alpine3.17, 8.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.17/amd64/Dockerfile) | Alpine 3.17
8.0.0-preview.3-jammy-amd64, 8.0-preview-jammy-amd64, 8.0.0-preview.3-jammy, 8.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.0-preview.3-jammy-chiseled-amd64, 8.0-preview-jammy-chiseled-amd64, 8.0.0-preview.3-jammy-chiseled, 8.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.5-bullseye-slim-arm64v8, 7.0-bullseye-slim-arm64v8, 7.0.5-bullseye-slim, 7.0-bullseye-slim, 7.0.5, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
7.0.5-alpine3.17-arm64v8, 7.0-alpine3.17-arm64v8, 7.0-alpine-arm64v8, 7.0.5-alpine3.17, 7.0-alpine3.17, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/alpine3.17/arm64v8/Dockerfile) | Alpine 3.17
7.0.5-jammy-arm64v8, 7.0-jammy-arm64v8, 7.0.5-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.16-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.16-bullseye-slim, 6.0-bullseye-slim, 6.0.16, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.16-alpine3.17-arm64v8, 6.0-alpine3.17-arm64v8, 6.0-alpine-arm64v8, 6.0.16-alpine3.17, 6.0-alpine3.17, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/alpine3.17/arm64v8/Dockerfile) | Alpine 3.17
6.0.16-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.16-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.16-focal-arm64v8, 6.0-focal-arm64v8, 6.0.16-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04

##### .NET 8 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-preview.3-bookworm-slim-arm64v8, 8.0-preview-bookworm-slim-arm64v8, 8.0.0-preview.3-bookworm-slim, 8.0-preview-bookworm-slim, 8.0.0-preview.3, 8.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.0-preview.3-alpine3.17-arm64v8, 8.0-preview-alpine3.17-arm64v8, 8.0-preview-alpine-arm64v8, 8.0.0-preview.3-alpine3.17, 8.0-preview-alpine3.17, 8.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.17/arm64v8/Dockerfile) | Alpine 3.17
8.0.0-preview.3-jammy-arm64v8, 8.0-preview-jammy-arm64v8, 8.0.0-preview.3-jammy, 8.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.0-preview.3-jammy-chiseled-arm64v8, 8.0-preview-jammy-chiseled-arm64v8, 8.0.0-preview.3-jammy-chiseled, 8.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.5-bullseye-slim-arm32v7, 7.0-bullseye-slim-arm32v7, 7.0.5-bullseye-slim, 7.0-bullseye-slim, 7.0.5, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
7.0.5-alpine3.17-arm32v7, 7.0-alpine3.17-arm32v7, 7.0-alpine-arm32v7, 7.0.5-alpine3.17, 7.0-alpine3.17, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/alpine3.17/arm32v7/Dockerfile) | Alpine 3.17
7.0.5-jammy-arm32v7, 7.0-jammy-arm32v7, 7.0.5-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.16-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.16-bullseye-slim, 6.0-bullseye-slim, 6.0.16, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.16-alpine3.17-arm32v7, 6.0-alpine3.17-arm32v7, 6.0-alpine-arm32v7, 6.0.16-alpine3.17, 6.0-alpine3.17, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/alpine3.17/arm32v7/Dockerfile) | Alpine 3.17
6.0.16-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.16-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.16-focal-arm32v7, 6.0-focal-arm32v7, 6.0.16-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

##### .NET 8 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-preview.3-bookworm-slim-arm32v7, 8.0-preview-bookworm-slim-arm32v7, 8.0.0-preview.3-bookworm-slim, 8.0-preview-bookworm-slim, 8.0.0-preview.3, 8.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.0-preview.3-alpine3.17-arm32v7, 8.0-preview-alpine3.17-arm32v7, 8.0-preview-alpine-arm32v7, 8.0.0-preview.3-alpine3.17, 8.0-preview-alpine3.17, 8.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.17/arm32v7/Dockerfile) | Alpine 3.17
8.0.0-preview.3-jammy-arm32v7, 8.0-preview-jammy-arm32v7, 8.0.0-preview.3-jammy, 8.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.0-preview.3-jammy-chiseled-arm32v7, 8.0-preview-jammy-chiseled-arm32v7, 8.0.0-preview.3-jammy-chiseled, 8.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
7.0.5-nanoserver-ltsc2022, 7.0-nanoserver-ltsc2022, 7.0.5, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.16-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.16, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

##### .NET 8 Preview Tags
Tag | Dockerfile
---------| ---------------
8.0.0-preview.3-nanoserver-ltsc2022, 8.0-preview-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/nanoserver-ltsc2022/amd64/Dockerfile)

## Windows Server Core 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
7.0.5-windowsservercore-ltsc2022, 7.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.16-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

##### .NET 8 Preview Tags
Tag | Dockerfile
---------| ---------------
8.0.0-preview.3-windowsservercore-ltsc2022, 8.0-preview-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
7.0.5-nanoserver-1809, 7.0-nanoserver-1809, 7.0.5, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/nanoserver-1809/amd64/Dockerfile)
6.0.16-nanoserver-1809, 6.0-nanoserver-1809, 6.0.16, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/nanoserver-1809/amd64/Dockerfile)

##### .NET 8 Preview Tags
Tag | Dockerfile
---------| ---------------
8.0.0-preview.3-nanoserver-1809, 8.0-preview-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/nanoserver-1809/amd64/Dockerfile)

## Windows Server Core 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
7.0.5-windowsservercore-ltsc2019, 7.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/7.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.16-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)

##### .NET 8 Preview Tags
Tag | Dockerfile
---------| ---------------
8.0.0-preview.3-windowsservercore-ltsc2019, 8.0-preview-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/runtime at https://mcr.microsoft.com/v2/dotnet/runtime/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core/runtime repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core/runtime/tags/list.

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)*

# Support

## Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/microsoft-support.md)
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
