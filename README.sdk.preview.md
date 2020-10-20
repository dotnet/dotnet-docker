As part of the .NET 5.0 release the Docker images are published to different repositories.  The 2.1 and 3.1 images are published to [core branded repositories](https://hub.docker.com/_/microsoft-dotnet-core/) while the 5.0 and higher versions will be published to [non-core branded repositories](https://hub.docker.com/_/microsoft-dotnet/).  See the [related issue](https://github.com/dotnet/dotnet-docker/issues/1765) for more details.

# Featured Tags

* `5.0` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/sdk:5.0`

# About This Image

This image contains the .NET Core SDK which is comprised of three parts:

1. .NET Core CLI
1. .NET Core
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Building .NET Core Apps with Docker

* [.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

## Develop .NET Core Apps in a Container

* [Develop .NET Core Applications](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-dev-in-container.md) - This sample shows how to develop, build and test .NET Core applications with Docker without the need to install the .NET Core SDK.
* [Develop ASP.NET Core Applications](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnet-docker-dev-in-container.md) - This sample shows how to develop and test ASP.NET Core applications with Docker without the need to install the .NET Core SDK.

# Related Repos

.NET Core 2.1/3.1:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/): .NET Core SDK
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET 5.0+:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/nightly](https://hub.docker.com/_/microsoft-dotnet-nightly/): .NET (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
##### .NET 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.100-rc.2-buster-slim-amd64, 5.0-buster-slim-amd64, 5.0.100-rc.2-buster-slim, 5.0-buster-slim, 5.0.100-rc.2, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/buster-slim/amd64/Dockerfile) | Debian 10
5.0.100-rc.2-alpine3.12-amd64, 5.0-alpine3.12-amd64, 5.0-alpine-amd64, 5.0.100-rc.2-alpine3.12, 5.0-alpine3.12, 5.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/alpine3.12/amd64/Dockerfile) | Alpine 3.12
5.0.100-rc.2-focal-amd64, 5.0-focal-amd64, 5.0.100-rc.2-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/focal/amd64/Dockerfile) | Ubuntu 20.04

## Linux arm64 Tags
##### .NET 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.100-rc.2-buster-slim-arm64v8, 5.0-buster-slim-arm64v8, 5.0.100-rc.2-buster-slim, 5.0-buster-slim, 5.0.100-rc.2, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/buster-slim/arm64v8/Dockerfile) | Debian 10
5.0.100-rc.2-focal-arm64v8, 5.0-focal-arm64v8, 5.0.100-rc.2-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04

## Linux arm32 Tags
##### .NET 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.100-rc.2-buster-slim-arm32v7, 5.0-buster-slim-arm32v7, 5.0.100-rc.2-buster-slim, 5.0-buster-slim, 5.0.100-rc.2, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/buster-slim/arm32v7/Dockerfile) | Debian 10
5.0.100-rc.2-focal-arm32v7, 5.0-focal-arm32v7, 5.0.100-rc.2-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

## Windows Server, version 2009 amd64 Tags
##### .NET 5.0 Preview Tags
Tag | Dockerfile
---------| ---------------
5.0.100-rc.2-nanoserver-2009, 5.0-nanoserver-2009, 5.0.100-rc.2, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/nanoserver-2009/amd64/Dockerfile)

## Windows Server, version 2004 amd64 Tags
##### .NET 5.0 Preview Tags
Tag | Dockerfile
---------| ---------------
5.0.100-rc.2-nanoserver-2004, 5.0-nanoserver-2004, 5.0.100-rc.2, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/nanoserver-2004/amd64/Dockerfile)

## Windows Server, version 1909 amd64 Tags
##### .NET 5.0 Preview Tags
Tag | Dockerfile
---------| ---------------
5.0.100-rc.2-nanoserver-1909, 5.0-nanoserver-1909, 5.0.100-rc.2, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/nanoserver-1909/amd64/Dockerfile)

## Windows Server 2019 amd64 Tags
##### .NET 5.0 Preview Tags
Tag | Dockerfile
---------| ---------------
5.0.100-rc.2-nanoserver-1809, 5.0-nanoserver-1809, 5.0.100-rc.2, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/nanoserver-1809/amd64/Dockerfile)
5.0.0-rc.2-windowsservercore-ltsc2019, 5.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/src/sdk/5.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/sdk at https://mcr.microsoft.com/v2/dotnet/sdk/tags/list.

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
