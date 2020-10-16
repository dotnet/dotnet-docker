# Featured Tags

* `3.1` (LTS/Current)
  * `docker pull mcr.microsoft.com/dotnet/core/runtime-deps:3.1`

# About This Image

This image contains the native dependencies needed by .NET Core. It does not include .NET Core. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

* [.NET Core self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-selfcontained.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.debian-x64-selfcontained) builds and runs an application as a self-contained application.

# Related Repos

.NET Core 2.1/3.1:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/): .NET Core SDK
* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/): ASP.NET Core Runtime
* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/): .NET Core Runtime
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET 5.0+:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/nightly](https://hub.docker.com/_/microsoft-dotnet-nightly/): .NET (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.9-buster-slim, 3.1-buster-slim, 3.1.9, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/buster-slim/amd64/Dockerfile) | Debian 10
3.1.9-alpine3.12, 3.1-alpine3.12, 3.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/alpine3.12/amd64/Dockerfile) | Alpine 3.12
3.1.9-focal, 3.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.9-bionic, 3.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/bionic/amd64/Dockerfile) | Ubuntu 18.04
2.1.23-stretch-slim, 2.1-stretch-slim, 2.1.23, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/2.1/stretch-slim/amd64/Dockerfile) | Debian 9
2.1.23-alpine3.12, 2.1-alpine3.12, 2.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/2.1/alpine3.12/amd64/Dockerfile) | Alpine 3.12
2.1.23-focal, 2.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/2.1/focal/amd64/Dockerfile) | Ubuntu 20.04
2.1.23-bionic, 2.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/2.1/bionic/amd64/Dockerfile) | Ubuntu 18.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.9-buster-slim-arm64v8, 3.1-buster-slim-arm64v8, 3.1.9, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/buster-slim/arm64v8/Dockerfile) | Debian 10
3.1.9-alpine3.12-arm64v8, 3.1-alpine3.12-arm64v8, 3.1-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/alpine3.12/arm64v8/Dockerfile) | Alpine 3.12
3.1.9-focal-arm64v8, 3.1-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.9-bionic-arm64v8, 3.1-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.9-buster-slim-arm32v7, 3.1-buster-slim-arm32v7, 3.1.9, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/buster-slim/arm32v7/Dockerfile) | Debian 10
3.1.9-focal-arm32v7, 3.1-focal-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/focal/arm32v7/Dockerfile) | Ubuntu 20.04
3.1.9-bionic-arm32v7, 3.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
2.1.23-stretch-slim-arm32v7, 2.1-stretch-slim-arm32v7, 2.1.23, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/2.1/stretch-slim/arm32v7/Dockerfile) | Debian 9
2.1.23-focal-arm32v7, 2.1-focal-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/2.1/focal/arm32v7/Dockerfile) | Ubuntu 20.04
2.1.23-bionic-arm32v7, 2.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/2.1/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

You can retrieve a list of all available tags for dotnet/core/runtime-deps at https://mcr.microsoft.com/v2/dotnet/core/runtime-deps/tags/list.

# Support

See [Microsoft Support for .NET Core](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Image Update Policy

* We update the supported .NET Core images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:1909, buildpack-deps:bionic-scm, etc.).
* We publish .NET Core images as part of releasing new versions of .NET Core including major/minor and servicing.

# Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/master/documentation/image-artifact-details.md)
* [Windows Nano Server license](https://hub.docker.com/_/microsoft-windows-nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
