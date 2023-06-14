**IMPORTANT**

**The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/) for images with official releases of [.NET](https://github.com/dotnet/core).**

# Featured Tags

* `8.0` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime-deps:8.0-preview`
* `7.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime-deps:7.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime-deps:6.0`

# About

This image contains the native dependencies needed by .NET. It does not include .NET. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

* [.NET self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.debian-x64-slim) builds and runs an application as a self-contained application.

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/monitor](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor/): .NET Monitor Tool (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.7-bullseye-slim-amd64, 7.0-bullseye-slim-amd64, 7.0.7, 7.0.7-bullseye-slim, 7.0, 7.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
7.0.7-alpine3.18-amd64, 7.0-alpine3.18-amd64, 7.0.7-alpine3.18, 7.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
7.0.7-alpine3.17-amd64, 7.0-alpine3.17-amd64, 7.0-alpine-amd64, 7.0.7-alpine3.17, 7.0-alpine3.17, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.17/amd64/Dockerfile) | Alpine 3.17
7.0.7-jammy-amd64, 7.0-jammy-amd64, 7.0.7-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
7.0.7-jammy-chiseled-amd64, 7.0-jammy-chiseled-amd64, 7.0.7-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.0.18-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.18, 6.0.18-bullseye-slim, 6.0, 6.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.18-alpine3.18-amd64, 6.0-alpine3.18-amd64, 6.0.18-alpine3.18, 6.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
6.0.18-alpine3.17-amd64, 6.0-alpine3.17-amd64, 6.0-alpine-amd64, 6.0.18-alpine3.17, 6.0-alpine3.17, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.17/amd64/Dockerfile) | Alpine 3.17
6.0.18-jammy-amd64, 6.0-jammy-amd64, 6.0.18-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.18-jammy-chiseled-amd64, 6.0-jammy-chiseled-amd64, 6.0.18-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.0.18-focal-amd64, 6.0-focal-amd64, 6.0.18-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04

##### .NET 8 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-preview.5-bookworm-slim-amd64, 8.0-preview-bookworm-slim-amd64, 8.0.0-preview.5, 8.0.0-preview.5-bookworm-slim, 8.0-preview, 8.0-preview-bookworm-slim, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.0-preview.5-alpine3.18-amd64, 8.0-preview-alpine3.18-amd64, 8.0-preview-alpine-amd64, 8.0.0-preview.5-alpine3.18, 8.0-preview-alpine3.18, 8.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
8.0.0-preview.5-jammy-amd64, 8.0-preview-jammy-amd64, 8.0.0-preview.5-jammy, 8.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.0-preview.5-jammy-chiseled-amd64, 8.0-preview-jammy-chiseled-amd64, 8.0.0-preview.5-jammy-chiseled, 8.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.7-bullseye-slim-arm64v8, 7.0-bullseye-slim-arm64v8, 7.0.7, 7.0.7-bullseye-slim, 7.0, 7.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
7.0.7-alpine3.18-arm64v8, 7.0-alpine3.18-arm64v8, 7.0.7-alpine3.18, 7.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
7.0.7-alpine3.17-arm64v8, 7.0-alpine3.17-arm64v8, 7.0-alpine-arm64v8, 7.0.7-alpine3.17, 7.0-alpine3.17, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.17/arm64v8/Dockerfile) | Alpine 3.17
7.0.7-jammy-arm64v8, 7.0-jammy-arm64v8, 7.0.7-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.7-jammy-chiseled-arm64v8, 7.0-jammy-chiseled-arm64v8, 7.0.7-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.18-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.18, 6.0.18-bullseye-slim, 6.0, 6.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.18-alpine3.18-arm64v8, 6.0-alpine3.18-arm64v8, 6.0.18-alpine3.18, 6.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
6.0.18-alpine3.17-arm64v8, 6.0-alpine3.17-arm64v8, 6.0-alpine-arm64v8, 6.0.18-alpine3.17, 6.0-alpine3.17, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.17/arm64v8/Dockerfile) | Alpine 3.17
6.0.18-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.18-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.18-jammy-chiseled-arm64v8, 6.0-jammy-chiseled-arm64v8, 6.0.18-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.18-focal-arm64v8, 6.0-focal-arm64v8, 6.0.18-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04

##### .NET 8 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-preview.5-bookworm-slim-arm64v8, 8.0-preview-bookworm-slim-arm64v8, 8.0.0-preview.5, 8.0.0-preview.5-bookworm-slim, 8.0-preview, 8.0-preview-bookworm-slim, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.0-preview.5-alpine3.18-arm64v8, 8.0-preview-alpine3.18-arm64v8, 8.0-preview-alpine-arm64v8, 8.0.0-preview.5-alpine3.18, 8.0-preview-alpine3.18, 8.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
8.0.0-preview.5-jammy-arm64v8, 8.0-preview-jammy-arm64v8, 8.0.0-preview.5-jammy, 8.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.0-preview.5-jammy-chiseled-arm64v8, 8.0-preview-jammy-chiseled-arm64v8, 8.0.0-preview.5-jammy-chiseled, 8.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.7-bullseye-slim-arm32v7, 7.0-bullseye-slim-arm32v7, 7.0.7, 7.0.7-bullseye-slim, 7.0, 7.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
7.0.7-alpine3.18-arm32v7, 7.0-alpine3.18-arm32v7, 7.0.7-alpine3.18, 7.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
7.0.7-alpine3.17-arm32v7, 7.0-alpine3.17-arm32v7, 7.0-alpine-arm32v7, 7.0.7-alpine3.17, 7.0-alpine3.17, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.17/arm32v7/Dockerfile) | Alpine 3.17
7.0.7-jammy-arm32v7, 7.0-jammy-arm32v7, 7.0.7-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.7-jammy-chiseled-arm32v7, 7.0-jammy-chiseled-arm32v7, 7.0.7-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.18-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.18, 6.0.18-bullseye-slim, 6.0, 6.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.18-alpine3.18-arm32v7, 6.0-alpine3.18-arm32v7, 6.0.18-alpine3.18, 6.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
6.0.18-alpine3.17-arm32v7, 6.0-alpine3.17-arm32v7, 6.0-alpine-arm32v7, 6.0.18-alpine3.17, 6.0-alpine3.17, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/alpine3.17/arm32v7/Dockerfile) | Alpine 3.17
6.0.18-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.18-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.18-jammy-chiseled-arm32v7, 6.0-jammy-chiseled-arm32v7, 6.0.18-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.18-focal-arm32v7, 6.0-focal-arm32v7, 6.0.18-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

##### .NET 8 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-preview.5-bookworm-slim-arm32v7, 8.0-preview-bookworm-slim-arm32v7, 8.0.0-preview.5, 8.0.0-preview.5-bookworm-slim, 8.0-preview, 8.0-preview-bookworm-slim, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.0-preview.5-alpine3.18-arm32v7, 8.0-preview-alpine3.18-arm32v7, 8.0-preview-alpine-arm32v7, 8.0.0-preview.5-alpine3.18, 8.0-preview-alpine3.18, 8.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
8.0.0-preview.5-jammy-arm32v7, 8.0-preview-jammy-arm32v7, 8.0.0-preview.5-jammy, 8.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.0-preview.5-jammy-chiseled-arm32v7, 8.0-preview-jammy-chiseled-arm32v7, 8.0.0-preview.5-jammy-chiseled, 8.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/runtime-deps/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04

You can retrieve a list of all available tags for dotnet/nightly/runtime-deps at https://mcr.microsoft.com/v2/dotnet/nightly/runtime-deps/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core-nightly/runtime-deps repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core-nightly/runtime-deps/tags/list.

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
