The images from this repository are for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/dotnet-docker](https://hub.docker.com/r/microsoft/dotnet/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

# Featured Tags

* `3.0` (Current)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/runtime-deps:3.0`
* `2.1` (LTS)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/runtime-deps:2.1`

# About This Image

This image contains the native dependencies needed by .NET Core. It does not include .NET Core. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

* [.NET Core self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-selfcontained.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.debian-x64-selfcontained) builds and runs an application as a self-contained application.

# Related Repos

.NET Core (Preview):

* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)
* [dotnet/core-nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-core-nightly-sdk/): .NET Core SDK (Preview)
* [dotnet/core-nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime/): .NET Core Runtime (Preview)

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.0-buster-slim, 3.1-buster-slim, 3.1.0, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/amd64/Dockerfile) | Debian 10
3.1.0-alpine3.10, 3.1-alpine3.10, 3.1.0-alpine, 3.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.10/amd64/Dockerfile) | Alpine 3.10
3.1.0-focal, 3.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.0-bionic, 3.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/amd64/Dockerfile) | Ubuntu 18.04
3.0.1-buster-slim, 3.0-buster-slim, 3.0.1, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/amd64/Dockerfile) | Debian 10
3.0.1-alpine3.10, 3.0-alpine3.10, 3.0.1-alpine, 3.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.10/amd64/Dockerfile) | Alpine 3.10
3.0.1-alpine3.9, 3.0-alpine3.9 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.9/amd64/Dockerfile) | Alpine 3.9
3.0.1-disco, 3.0-disco | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/disco/amd64/Dockerfile) | Ubuntu 19.04
3.0.1-bionic, 3.0-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/amd64/Dockerfile) | Ubuntu 18.04
2.2.8-stretch-slim, 2.2-stretch-slim, 2.2.8, 2.2 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile) | Debian 9
2.2.8-alpine3.10, 2.2-alpine3.10, 2.2.8-alpine, 2.2-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.10/amd64/Dockerfile) | Alpine 3.10
2.2.8-alpine3.9, 2.2-alpine3.9 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.9/amd64/Dockerfile) | Alpine 3.9
2.2.8-bionic, 2.2-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile) | Ubuntu 18.04
2.1.14-stretch-slim, 2.1-stretch-slim, 2.1.14, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile) | Debian 9
2.1.14-alpine3.10, 2.1-alpine3.10, 2.1.14-alpine, 2.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.10/amd64/Dockerfile) | Alpine 3.10
2.1.14-alpine3.9, 2.1-alpine3.9 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.9/amd64/Dockerfile) | Alpine 3.9
2.1.14-bionic, 2.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile) | Ubuntu 18.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.0-buster-slim-arm64v8, 3.1-buster-slim-arm64v8, 3.1.0, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm64v8/Dockerfile) | Debian 10
3.1.0-alpine3.10-arm64v8, 3.1-alpine3.10-arm64v8, 3.1.0-alpine-arm64v8, 3.1-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.10/arm64v8/Dockerfile) | Alpine 3.10
3.1.0-focal-arm64v8, 3.1-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.0-bionic-arm64v8, 3.1-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm64v8/Dockerfile) | Ubuntu 18.04
3.0.1-buster-slim-arm64v8, 3.0-buster-slim-arm64v8, 3.0.1, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm64v8/Dockerfile) | Debian 10
3.0.1-alpine3.10-arm64v8, 3.0-alpine3.10-arm64v8, 3.0.1-alpine-arm64v8, 3.0-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.10/arm64v8/Dockerfile) | Alpine 3.10
3.0.1-alpine3.9-arm64v8, 3.0-alpine3.9-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.9/arm64v8/Dockerfile) | Alpine 3.9
3.0.1-disco-arm64v8, 3.0-disco-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/disco/arm64v8/Dockerfile) | Ubuntu 19.04
3.0.1-bionic-arm64v8, 3.0-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.0-buster-slim-arm32v7, 3.1-buster-slim-arm32v7, 3.1.0, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm32v7/Dockerfile) | Debian 10
3.1.0-focal-arm32v7, 3.1-focal-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/runtime-deps/focal/arm32v7/Dockerfile) | Ubuntu 20.04
3.1.0-bionic-arm32v7, 3.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
3.0.1-buster-slim-arm32v7, 3.0-buster-slim-arm32v7, 3.0.1, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/buster-slim/arm32v7/Dockerfile) | Debian 10
3.0.1-disco-arm32v7, 3.0-disco-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/disco/arm32v7/Dockerfile) | Ubuntu 19.04
3.0.1-bionic-arm32v7, 3.0-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
2.2.8-stretch-slim-arm32v7, 2.2-stretch-slim-arm32v7, 2.2.8, 2.2 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile) | Debian 9
2.2.8-bionic-arm32v7, 2.2-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
2.1.14-stretch-slim-arm32v7, 2.1-stretch-slim-arm32v7, 2.1.14, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile) | Debian 9
2.1.14-bionic-arm32v7, 2.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

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
* [Windows Nano Server license](https://hub.docker.com/_/microsoft-windows-nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/en-us/cloud-platform/windows-server-pricing)
