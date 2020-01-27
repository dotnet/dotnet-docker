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

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/): .NET Core SDK
* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/): ASP.NET Core Runtime
* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/): .NET Core Runtime
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.1-buster-slim, 3.1-buster-slim, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/amd64/Dockerfile) | Debian 10
3.1.1-alpine3.11, 3.1-alpine3.11, 3.1.1-alpine, 3.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.11/amd64/Dockerfile) | Alpine 3.11
3.1.1-alpine3.10, 3.1-alpine3.10 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.10/amd64/Dockerfile) | Alpine 3.10
3.1.1-focal, 3.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.1-bionic, 3.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/amd64/Dockerfile) | Ubuntu 18.04
3.0.2-buster-slim, 3.0-buster-slim, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/amd64/Dockerfile) | Debian 10
3.0.2-alpine3.11, 3.0-alpine3.11, 3.0.2-alpine, 3.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.11/amd64/Dockerfile) | Alpine 3.11
3.0.2-alpine3.10, 3.0-alpine3.10 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.10/amd64/Dockerfile) | Alpine 3.10
3.0.2-bionic, 3.0-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/amd64/Dockerfile) | Ubuntu 18.04
2.1.15-stretch-slim, 2.1-stretch-slim, 2.1.15, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile) | Debian 9
2.1.15-alpine3.11, 2.1-alpine3.11, 2.1.15-alpine, 2.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.11/amd64/Dockerfile) | Alpine 3.11
2.1.15-alpine3.10, 2.1-alpine3.10 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.10/amd64/Dockerfile) | Alpine 3.10
2.1.15-focal, 2.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/focal/amd64/Dockerfile) | Ubuntu 20.04
2.1.15-bionic, 2.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile) | Ubuntu 18.04

##### .NET Core 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.0-alpha-buster-slim, 5.0-buster-slim, 5.0.0-alpha, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/amd64/Dockerfile) | Debian 10
5.0.0-alpha-alpine3.11, 5.0-alpine3.11, 5.0.0-alpha-alpine, 5.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.11/amd64/Dockerfile) | Alpine 3.11
5.0.0-alpha-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/amd64/Dockerfile) | Ubuntu 20.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.1-buster-slim-arm64v8, 3.1-buster-slim-arm64v8, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm64v8/Dockerfile) | Debian 10
3.1.1-alpine3.11-arm64v8, 3.1-alpine3.11-arm64v8, 3.1.1-alpine-arm64v8, 3.1-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.11/arm64v8/Dockerfile) | Alpine 3.11
3.1.1-alpine3.10-arm64v8, 3.1-alpine3.10-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.10/arm64v8/Dockerfile) | Alpine 3.10
3.1.1-focal-arm64v8, 3.1-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.1-bionic-arm64v8, 3.1-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm64v8/Dockerfile) | Ubuntu 18.04
3.0.2-buster-slim-arm64v8, 3.0-buster-slim-arm64v8, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm64v8/Dockerfile) | Debian 10
3.0.2-alpine3.11-arm64v8, 3.0-alpine3.11-arm64v8, 3.0.2-alpine-arm64v8, 3.0-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.11/arm64v8/Dockerfile) | Alpine 3.11
3.0.2-alpine3.10-arm64v8, 3.0-alpine3.10-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.10/arm64v8/Dockerfile) | Alpine 3.10
3.0.2-bionic-arm64v8, 3.0-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

##### .NET Core 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.0-alpha-buster-slim-arm64v8, 5.0-buster-slim-arm64v8, 5.0.0-alpha, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm64v8/Dockerfile) | Debian 10
5.0.0-alpha-alpine3.11-arm64v8, 5.0-alpine3.11-arm64v8, 5.0.0-alpha-alpine-arm64v8, 5.0-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.11/arm64v8/Dockerfile) | Alpine 3.11
5.0.0-alpha-focal-arm64v8, 5.0-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/arm64v8/Dockerfile) | Ubuntu 20.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.1-buster-slim-arm32v7, 3.1-buster-slim-arm32v7, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm32v7/Dockerfile) | Debian 10
3.1.1-focal-arm32v7, 3.1-focal-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/arm32v7/Dockerfile) | Ubuntu 20.04
3.1.1-bionic-arm32v7, 3.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
3.0.2-buster-slim-arm32v7, 3.0-buster-slim-arm32v7, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm32v7/Dockerfile) | Debian 10
3.0.2-bionic-arm32v7, 3.0-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
2.1.15-stretch-slim-arm32v7, 2.1-stretch-slim-arm32v7, 2.1.15, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile) | Debian 9
2.1.15-focal-arm32v7, 2.1-focal-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/focal/arm32v7/Dockerfile) | Ubuntu 20.04
2.1.15-bionic-arm32v7, 2.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

##### .NET Core 5.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.0-alpha-buster-slim-arm32v7, 5.0-buster-slim-arm32v7, 5.0.0-alpha, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm32v7/Dockerfile) | Debian 10
5.0.0-alpha-focal-arm32v7, 5.0-focal-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/arm32v7/Dockerfile) | Ubuntu 20.04

You can retrieve a list of all available tags for dotnet/core-nightly/runtime-deps at https://mcr.microsoft.com/v2/dotnet/core-nightly/runtime-deps/tags/list.

# Support

See [Microsoft Support for .NET Core](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Feedback

* [File a .NET Core Docker issue](https://github.com/dotnet/dotnet-docker/issues)
* [File a .NET Core issue](https://github.com/dotnet/core/issues)
* [File an ASP.NET Core issue](https://github.com/aspnet/home/issues)
* [File an issue for other components](https://github.com/dotnet/core/blob/master/Documentation/core-repos.md)
* [Ask on Stack Overflow](https://stackoverflow.com/questions/tagged/.net-core)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/master/documentation/image-artifact-details.md)
* [Windows Nano Server license](https://hub.docker.com/_/microsoft-windows-nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/en-us/cloud-platform/windows-server-pricing)
