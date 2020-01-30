# Featured Tags

* `3.1` (LTS/Current)
  * `docker pull mcr.microsoft.com/dotnet/core/runtime:3.1`

# About This Image

This image contains the .NET Core runtimes and libraries and is optimized for running .NET Core apps in production.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a simple application

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/), based on the [.NET Core console sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

# Related Repos

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/): .NET Core SDK
* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/): ASP.NET Core Runtime
* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/): .NET Core Runtime Dependencies
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.1-buster-slim, 3.1-buster-slim, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/buster-slim/amd64/Dockerfile) | Debian 10
3.1.1-alpine3.10, 3.1-alpine3.10, 3.1.1-alpine, 3.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/alpine3.10/amd64/Dockerfile) | Alpine 3.10
3.1.1-bionic, 3.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/bionic/amd64/Dockerfile) | Ubuntu 18.04
3.0.2-buster-slim, 3.0-buster-slim, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/buster-slim/amd64/Dockerfile) | Debian 10
3.0.2-alpine3.10, 3.0-alpine3.10, 3.0.2-alpine, 3.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/alpine3.10/amd64/Dockerfile) | Alpine 3.10
3.0.2-bionic, 3.0-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/bionic/amd64/Dockerfile) | Ubuntu 18.04
2.1.15-stretch-slim, 2.1-stretch-slim, 2.1.15, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/stretch-slim/amd64/Dockerfile) | Debian 9
2.1.15-alpine3.10, 2.1-alpine3.10, 2.1.15-alpine, 2.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/alpine3.10/amd64/Dockerfile) | Alpine 3.10
2.1.15-bionic, 2.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/bionic/amd64/Dockerfile) | Ubuntu 18.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.1-buster-slim-arm64v8, 3.1-buster-slim-arm64v8, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/buster-slim/arm64v8/Dockerfile) | Debian 10
3.1.1-alpine3.10-arm64v8, 3.1-alpine3.10-arm64v8, 3.1.1-alpine-arm64v8, 3.1-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/alpine3.10/arm64v8/Dockerfile) | Alpine 3.10
3.1.1-bionic-arm64v8, 3.1-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/bionic/arm64v8/Dockerfile) | Ubuntu 18.04
3.0.2-buster-slim-arm64v8, 3.0-buster-slim-arm64v8, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/buster-slim/arm64v8/Dockerfile) | Debian 10
3.0.2-alpine3.10-arm64v8, 3.0-alpine3.10-arm64v8, 3.0.2-alpine-arm64v8, 3.0-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/alpine3.10/arm64v8/Dockerfile) | Alpine 3.10
3.0.2-bionic-arm64v8, 3.0-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.1-buster-slim-arm32v7, 3.1-buster-slim-arm32v7, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/buster-slim/arm32v7/Dockerfile) | Debian 10
3.1.1-bionic-arm32v7, 3.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
3.0.2-buster-slim-arm32v7, 3.0-buster-slim-arm32v7, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/buster-slim/arm32v7/Dockerfile) | Debian 10
3.0.2-bionic-arm32v7, 3.0-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
2.1.15-stretch-slim-arm32v7, 2.1-stretch-slim-arm32v7, 2.1.15, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/stretch-slim/arm32v7/Dockerfile) | Debian 9
2.1.15-bionic-arm32v7, 2.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

## Windows Server, version 1909 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.1-nanoserver-1909, 3.1-nanoserver-1909, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/nanoserver-1909/amd64/Dockerfile)
3.0.2-nanoserver-1909, 3.0-nanoserver-1909, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/nanoserver-1909/amd64/Dockerfile)
2.1.15-nanoserver-1909, 2.1-nanoserver-1909, 2.1.15, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/nanoserver-1909/amd64/Dockerfile)

## Windows Server, version 1903 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.1-nanoserver-1903, 3.1-nanoserver-1903, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/nanoserver-1903/amd64/Dockerfile)
3.0.2-nanoserver-1903, 3.0-nanoserver-1903, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/nanoserver-1903/amd64/Dockerfile)
2.1.15-nanoserver-1903, 2.1-nanoserver-1903, 2.1.15, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/nanoserver-1903/amd64/Dockerfile)

## Windows Server 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.1-nanoserver-1809, 3.1-nanoserver-1809, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/nanoserver-1809/amd64/Dockerfile)
3.0.2-nanoserver-1809, 3.0-nanoserver-1809, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/nanoserver-1809/amd64/Dockerfile)
2.1.15-nanoserver-1809, 2.1-nanoserver-1809, 2.1.15, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/nanoserver-1809/amd64/Dockerfile)

## Windows Server 2019 arm32 Tags
Tag | Dockerfile
---------| ---------------
3.1.1-nanoserver-1809-arm32v7, 3.1-nanoserver-1809-arm32v7, 3.1.1, 3.1, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.1/runtime/nanoserver-1809/arm32v7/Dockerfile)
3.0.2-nanoserver-1809-arm32v7, 3.0-nanoserver-1809-arm32v7, 3.0.2, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/nanoserver-1809/arm32v7/Dockerfile)

You can retrieve a list of all available tags for dotnet/core/runtime at https://mcr.microsoft.com/v2/dotnet/core/runtime/tags/list.

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
