# Featured Tags

* `2.2` (Current)
  * `docker pull mcr.microsoft.com/dotnet/core/runtime:2.2`
* `2.1` (LTS)
  * `docker pull mcr.microsoft.com/dotnet/core/runtime:2.1`

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
2.2.6-stretch-slim, 2.2-stretch-slim, 2.2.6, 2.2, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/stretch-slim/amd64/Dockerfile) | Debian 9
2.2.6-alpine3.9, 2.2-alpine3.9 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/alpine3.9/amd64/Dockerfile) | Alpine 3.9
2.2.6-alpine3.8, 2.2-alpine3.8, 2.2.6-alpine, 2.2-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/alpine3.8/amd64/Dockerfile) | Alpine 3.8
2.2.6-bionic, 2.2-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/bionic/amd64/Dockerfile) | Ubuntu 18.04
2.1.12-stretch-slim, 2.1-stretch-slim, 2.1.12, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/stretch-slim/amd64/Dockerfile) | Debian 9
2.1.12-alpine3.9, 2.1-alpine3.9 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/alpine3.9/amd64/Dockerfile) | Alpine 3.9
2.1.12-alpine3.7, 2.1-alpine3.7, 2.1.12-alpine, 2.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/alpine3.7/amd64/Dockerfile) | Alpine 3.7
2.1.12-bionic, 2.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/bionic/amd64/Dockerfile) | Ubuntu 18.04

##### .NET Core 3.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.0.0-preview8-buster-slim, 3.0-buster-slim, 3.0.0-preview8, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/buster-slim/amd64/Dockerfile) | Debian 10
3.0.0-preview8-alpine3.9, 3.0-alpine3.9, 3.0.0-preview8-alpine, 3.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/alpine3.9/amd64/Dockerfile) | Alpine 3.9
3.0.0-preview8-disco, 3.0-disco | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/disco/amd64/Dockerfile) | Ubuntu 19.04
3.0.0-preview8-bionic, 3.0-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/bionic/amd64/Dockerfile) | Ubuntu 18.04

## Linux arm64 Tags
##### .NET Core 3.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.0.0-preview8-buster-slim-arm64v8, 3.0-buster-slim-arm64v8, 3.0.0-preview8, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/buster-slim/arm64v8/Dockerfile) | Debian 10
3.0.0-preview8-alpine3.9-arm64v8, 3.0-alpine3.9-arm64v8, 3.0.0-preview8-alpine-arm64v8, 3.0-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/alpine3.9/arm64v8/Dockerfile) | Alpine 3.9
3.0.0-preview8-disco-arm64v8, 3.0-disco-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/disco/arm64v8/Dockerfile) | Ubuntu 19.04
3.0.0-preview8-bionic-arm64v8, 3.0-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
2.2.6-stretch-slim-arm32v7, 2.2-stretch-slim-arm32v7, 2.2.6, 2.2, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/stretch-slim/arm32v7/Dockerfile) | Debian 9
2.2.6-bionic-arm32v7, 2.2-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
2.1.12-stretch-slim-arm32v7, 2.1-stretch-slim-arm32v7, 2.1.12, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/stretch-slim/arm32v7/Dockerfile) | Debian 9
2.1.12-bionic-arm32v7, 2.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

##### .NET Core 3.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.0.0-preview8-buster-slim-arm32v7, 3.0-buster-slim-arm32v7, 3.0.0-preview8, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/buster-slim/arm32v7/Dockerfile) | Debian 10
3.0.0-preview8-disco-arm32v7, 3.0-disco-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/disco/arm32v7/Dockerfile) | Ubuntu 19.04
3.0.0-preview8-bionic-arm32v7, 3.0-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

## Windows Server, version 1903 amd64 Tags
Tag | Dockerfile
---------| ---------------
2.2.6-nanoserver-1903, 2.2-nanoserver-1903, 2.2.6, 2.2, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/nanoserver-1903/amd64/Dockerfile)
2.1.12-nanoserver-1903, 2.1-nanoserver-1903, 2.1.12, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/nanoserver-1903/amd64/Dockerfile)

##### .NET Core 3.0 Preview tags
Tag | Dockerfile
---------| ---------------
3.0.0-preview8-nanoserver-1903, 3.0-nanoserver-1903, 3.0.0-preview8, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/nanoserver-1903/amd64/Dockerfile)

## Windows Server 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
2.2.6-nanoserver-1809, 2.2-nanoserver-1809, 2.2.6, 2.2, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/nanoserver-1809/amd64/Dockerfile)
2.1.12-nanoserver-1809, 2.1-nanoserver-1809, 2.1.12, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/nanoserver-1809/amd64/Dockerfile)

##### .NET Core 3.0 Preview tags
Tag | Dockerfile
---------| ---------------
3.0.0-preview8-nanoserver-1809, 3.0-nanoserver-1809, 3.0.0-preview8, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/nanoserver-1809/amd64/Dockerfile)

## Windows Server 2019 arm32 Tags
Tag | Dockerfile
---------| ---------------
2.2.6-nanoserver-1809-arm32v7, 2.2-nanoserver-1809-arm32v7, 2.2.6, 2.2, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/nanoserver-1809/arm32v7/Dockerfile)

##### .NET Core 3.0 Preview tags
Tag | Dockerfile
---------| ---------------
3.0.0-preview8-nanoserver-1809-arm32v7, 3.0-nanoserver-1809-arm32v7, 3.0.0-preview8, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/nanoserver-1809/arm32v7/Dockerfile)

## Windows Server, version 1803 amd64 Tags
Tag | Dockerfile
---------| ---------------
2.2.6-nanoserver-1803, 2.2-nanoserver-1803, 2.2.6, 2.2, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.2/runtime/nanoserver-1803/amd64/Dockerfile)
2.1.12-nanoserver-1803, 2.1-nanoserver-1803, 2.1.12, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/nanoserver-1803/amd64/Dockerfile)

##### .NET Core 3.0 Preview tags
Tag | Dockerfile
---------| ---------------
3.0.0-preview8-nanoserver-1803, 3.0-nanoserver-1803, 3.0.0-preview8, 3.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/3.0/runtime/nanoserver-1803/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/core/runtime at https://mcr.microsoft.com/v2/dotnet/core/runtime/tags/list.

# Support

See [Microsoft Support for .NET Core](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Feedback

* [File a .NET Core Docker issue](https://github.com/dotnet/dotnet-docker/issues)
* [File a .NET Core issue](https://github.com/dotnet/core/issues)
* [File an ASP.NET Core issue](https://github.com/aspnet/home/issues)
* [File an issue for other components](Documentation/core-repos.md)
* [Ask on Stack Overflow](https://stackoverflow.com/questions/tagged/.net-core)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Windows Nano Server license](https://hub.docker.com/_/microsoft-windows-nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/en-us/cloud-platform/windows-server-pricing)
