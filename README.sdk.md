## Important: Client Firewall Rules Update to Microsoft Container Registry (MCR)

To provide a consistent FQDNs, the data endpoint will be changing from *.cdn.mscr.io to *.data.mcr.microsoft.com

For more info, see [MCR Client Firewall Rules](https://aka.ms/mcr/firewallrules).

---------------------------------------------------------------------------------

The images from the dotnet/core-nightly repositories include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

# Featured Tags

* `3.1` (LTS/Current)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/sdk:3.1`

# About This Image

This image contains the .NET Core SDK which is comprised of three parts:

1. .NET Core CLI
1. .NET Core
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications). This repository is limited to .NET Core 2.1 and 3.1. For .NET 5.0 and higher, see [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/) for those versions.

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
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)
* [dotnet/core-nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime/): .NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime-deps/): .NET Core Runtime Dependencies (Preview)

.NET 5.0+:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/nightly](https://hub.docker.com/_/microsoft-dotnet-nightly/): .NET (Preview)

.NET 5.0+:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/nightly](https://hub.docker.com/_/microsoft-dotnet-nightly/): .NET (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.300-buster, 3.1-buster, 3.1.300, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/buster/amd64/Dockerfile) | Debian 10
3.1.300-alpine3.11, 3.1-alpine3.11, 3.1.300-alpine, 3.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/alpine3.11/amd64/Dockerfile) | Alpine 3.11
3.1.300-focal, 3.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.300-bionic, 3.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/bionic/amd64/Dockerfile) | Ubuntu 18.04
2.1.806-stretch, 2.1-stretch, 2.1.806, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/stretch/amd64/Dockerfile) | Debian 9
2.1.806-alpine3.11, 2.1-alpine3.11, 2.1.806-alpine, 2.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/alpine3.11/amd64/Dockerfile) | Alpine 3.11
2.1.806-focal, 2.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/focal/amd64/Dockerfile) | Ubuntu 20.04
2.1.806-bionic, 2.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/bionic/amd64/Dockerfile) | Ubuntu 18.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.300-buster-arm64v8, 3.1-buster-arm64v8, 3.1.300, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/buster/arm64v8/Dockerfile) | Debian 10
3.1.300-focal-arm64v8, 3.1-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.300-bionic-arm64v8, 3.1-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.300-buster-arm32v7, 3.1-buster-arm32v7, 3.1.300, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/buster/arm32v7/Dockerfile) | Debian 10
3.1.300-bionic-arm32v7, 3.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
2.1.806-stretch-arm32v7, 2.1-stretch-arm32v7, 2.1.806, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/stretch/arm32v7/Dockerfile) | Debian 9
2.1.806-bionic-arm32v7, 2.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

## Windows Server, version 2004 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.300-nanoserver-2004, 3.1-nanoserver-2004, 3.1.300, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/nanoserver-2004/amd64/Dockerfile)
2.1.806-nanoserver-2004, 2.1-nanoserver-2004, 2.1.806, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/nanoserver-2004/amd64/Dockerfile)

## Windows Server, version 1909 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.300-nanoserver-1909, 3.1-nanoserver-1909, 3.1.300, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/nanoserver-1909/amd64/Dockerfile)
2.1.806-nanoserver-1909, 2.1-nanoserver-1909, 2.1.806, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/nanoserver-1909/amd64/Dockerfile)

## Windows Server, version 1903 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.300-nanoserver-1903, 3.1-nanoserver-1903, 3.1.300, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/nanoserver-1903/amd64/Dockerfile)
2.1.806-nanoserver-1903, 2.1-nanoserver-1903, 2.1.806, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/nanoserver-1903/amd64/Dockerfile)

## Windows Server 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.300-nanoserver-1809, 3.1-nanoserver-1809, 3.1.300, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/sdk/nanoserver-1809/amd64/Dockerfile)
2.1.806-nanoserver-1809, 2.1-nanoserver-1809, 2.1.806, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/nanoserver-1809/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/core-nightly/sdk at https://mcr.microsoft.com/v2/dotnet/core-nightly/sdk/tags/list.

# Support

See [Microsoft Support for .NET Core](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Image Update Policy

* We update the supported .NET Core images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:1909, buildpack-deps:bionic-scm, etc.).
* We publish .NET Core images as part of releasing new versions of .NET Core including major/minor and servicing.

# Feedback

* [File a .NET Core Docker issue](https://github.com/dotnet/dotnet-docker/issues)
* [File a .NET Core issue](https://github.com/dotnet/core/issues)
* [File an ASP.NET Core issue](https://github.com/aspnet/home/issues)
* [File an issue for other .NET components](https://github.com/dotnet/core/blob/master/Documentation/core-repos.md)
* [File a Visual Studio Docker Tools issue](https://github.com/microsoft/dockertools/issues)
* [File a Microsoft Container Registry (MCR) issue](https://github.com/microsoft/containerregistry/issues)
* [Ask on Stack Overflow](https://stackoverflow.com/questions/tagged/.net-core)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Windows Nano Server license](https://hub.docker.com/_/microsoft-windows-nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
