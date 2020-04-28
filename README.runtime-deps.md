## Important: Client Firewall Rules Update to Microsoft Container Registry (MCR)

To provide a consistent FQDNs, the data endpoint will be changing from *.cdn.mscr.io to *.data.mcr.microsoft.com

For more info, see [MCR Client Firewall Rules](https://aka.ms/mcr/firewallrules).
---------------------------------------------------------------------------------

The images from the dotnet/core-nightly repositories include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

# Featured Tags

* `5.0` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/runtime-deps:5.0`

# About This Image

This image contains the native dependencies needed by .NET Core. It does not include .NET Core. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

* [.NET Core self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-selfcontained.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.debian-x64-selfcontained) builds and runs an application as a self-contained application.

# Related Repos

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)
* [dotnet/core-nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-core-nightly-sdk/): .NET Core SDK (Preview)
* [dotnet/core-nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime/): .NET Core Runtime (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.3-buster-slim, 3.1-buster-slim, 3.1.3, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/buster-slim/amd64/Dockerfile) | Debian 10
3.1.3-alpine3.11, 3.1-alpine3.11, 3.1.3-alpine, 3.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/alpine3.11/amd64/Dockerfile) | Alpine 3.11
3.1.3-focal, 3.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.3-bionic, 3.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/bionic/amd64/Dockerfile) | Ubuntu 18.04
2.1.17-stretch-slim, 2.1-stretch-slim, 2.1.17, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile) | Debian 9
2.1.17-alpine3.11, 2.1-alpine3.11, 2.1.17-alpine, 2.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.11/amd64/Dockerfile) | Alpine 3.11
2.1.17-focal, 2.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/focal/amd64/Dockerfile) | Ubuntu 20.04
2.1.17-bionic, 2.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile) | Ubuntu 18.04

##### .NET Core 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.0-preview.4-buster-slim, 5.0-buster-slim, 5.0.0-preview.4, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/5.0/runtime-deps/buster-slim/amd64/Dockerfile) | Debian 10
5.0.0-preview.4-alpine3.11, 5.0-alpine3.11, 5.0.0-preview.4-alpine, 5.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/5.0/runtime-deps/alpine3.11/amd64/Dockerfile) | Alpine 3.11
5.0.0-preview.4-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/5.0/runtime-deps/focal/amd64/Dockerfile) | Ubuntu 20.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.3-buster-slim-arm64v8, 3.1-buster-slim-arm64v8, 3.1.3, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/buster-slim/arm64v8/Dockerfile) | Debian 10
3.1.3-alpine3.11-arm64v8, 3.1-alpine3.11-arm64v8, 3.1.3-alpine-arm64v8, 3.1-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/alpine3.11/arm64v8/Dockerfile) | Alpine 3.11
3.1.3-focal-arm64v8, 3.1-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.3-bionic-arm64v8, 3.1-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

##### .NET Core 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.0-preview.4-buster-slim-arm64v8, 5.0-buster-slim-arm64v8, 5.0.0-preview.4, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/5.0/runtime-deps/buster-slim/arm64v8/Dockerfile) | Debian 10
5.0.0-preview.4-alpine3.11-arm64v8, 5.0-alpine3.11-arm64v8, 5.0.0-preview.4-alpine-arm64v8, 5.0-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/5.0/runtime-deps/alpine3.11/arm64v8/Dockerfile) | Alpine 3.11
5.0.0-preview.4-focal-arm64v8, 5.0-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/5.0/runtime-deps/focal/arm64v8/Dockerfile) | Ubuntu 20.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.3-buster-slim-arm32v7, 3.1-buster-slim-arm32v7, 3.1.3, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/buster-slim/arm32v7/Dockerfile) | Debian 10
3.1.3-bionic-arm32v7, 3.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
2.1.17-stretch-slim-arm32v7, 2.1-stretch-slim-arm32v7, 2.1.17, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile) | Debian 9
2.1.17-bionic-arm32v7, 2.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

##### .NET Core 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.0-preview.4-buster-slim-arm32v7, 5.0-buster-slim-arm32v7, 5.0.0-preview.4, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/5.0/runtime-deps/buster-slim/arm32v7/Dockerfile) | Debian 10

You can retrieve a list of all available tags for dotnet/core-nightly/runtime-deps at https://mcr.microsoft.com/v2/dotnet/core-nightly/runtime-deps/tags/list.

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
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/master/documentation/image-artifact-details.md)
* [Windows Nano Server license](https://hub.docker.com/_/microsoft-windows-nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
