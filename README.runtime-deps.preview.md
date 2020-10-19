As part of the .NET 5.0 release the Docker images are published to different repositories.  The 2.1 and 3.1 images are published to [core branded repositories](https://hub.docker.com/_/microsoft-dotnet-core/) while the 5.0 and higher versions will be published to [non-core branded repositories](https://hub.docker.com/_/microsoft-dotnet/).  See the [related issue](https://github.com/dotnet/dotnet-docker/issues/1765) for more details.

# Featured Tags

* `5.0` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/runtime-deps:5.0`

# About This Image

This image contains the native dependencies needed by .NET Core. It does not include .NET Core. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

* [.NET Core self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-selfcontained.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.debian-x64-selfcontained) builds and runs an application as a self-contained application.

# Related Repos

.NET Core 2.1/3.1:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/): .NET Core Runtime Dependencies
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET 5.0+:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/nightly](https://hub.docker.com/_/microsoft-dotnet-nightly/): .NET (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
##### .NET 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.0-rc.2-buster-slim-amd64, 5.0-buster-slim-amd64, 5.0.0-rc.2, 5.0.0-rc.2-buster-slim, 5.0, 5.0-buster-slim, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/buster-slim/amd64/Dockerfile) | Debian 10
5.0.0-rc.2-alpine3.12-amd64, 5.0-alpine3.12-amd64, 5.0-alpine-amd64, 5.0.0-rc.2-alpine3.12, 5.0-alpine3.12, 5.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/alpine3.12/amd64/Dockerfile) | Alpine 3.12
5.0.0-rc.2-focal-amd64, 5.0-focal-amd64, 5.0.0-rc.2-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/focal/amd64/Dockerfile) | Ubuntu 20.04

## Linux arm64 Tags
##### .NET 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.0-rc.2-buster-slim-arm64v8, 5.0-buster-slim-arm64v8, 5.0.0-rc.2, 5.0.0-rc.2-buster-slim, 5.0, 5.0-buster-slim, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/buster-slim/arm64v8/Dockerfile) | Debian 10
5.0.0-rc.2-alpine3.12-arm64v8, 5.0-alpine3.12-arm64v8, 5.0-alpine-arm64v8, 5.0.0-rc.2-alpine3.12, 5.0-alpine3.12, 5.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/alpine3.12/arm64v8/Dockerfile) | Alpine 3.12
5.0.0-rc.2-focal-arm64v8, 5.0-focal-arm64v8, 5.0.0-rc.2-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/focal/arm64v8/Dockerfile) | Ubuntu 20.04

## Linux arm32 Tags
##### .NET 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.0-rc.2-buster-slim-arm32v7, 5.0-buster-slim-arm32v7, 5.0.0-rc.2, 5.0.0-rc.2-buster-slim, 5.0, 5.0-buster-slim, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/buster-slim/arm32v7/Dockerfile) | Debian 10
5.0.0-rc.2-focal-arm32v7, 5.0-focal-arm32v7, 5.0.0-rc.2-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/runtime-deps/3.1/focal/arm32v7/Dockerfile) | Ubuntu 20.04

You can retrieve a list of all available tags for dotnet/runtime-deps at https://mcr.microsoft.com/v2/dotnet/runtime-deps/tags/list.

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
