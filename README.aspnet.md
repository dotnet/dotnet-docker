## Important: Client Firewall Rules Update to Microsoft Container Registry (MCR)

To provide a consistent FQDNs, the data endpoint will be changing from *.cdn.mscr.io to *.data.mcr.microsoft.com

For more info, see [MCR Client Firewall Rules](https://aka.ms/mcr/firewallrules).
---------------------------------------------------------------------------------

The images from the dotnet/core-nightly repositories include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/dotnet-docker](https://hub.docker.com/r/microsoft/dotnet/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

# Featured Tags

* `3.1` (LTS/Current)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/aspnet:3.1`

# About This Image

This image contains the ASP.NET Core and .NET Core runtimes and libraries and is optimized for running ASP.NET Core apps in production. This repository is limited to .NET Core 2.1 and 3.1. For .NET 5.0 and higher, see [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/) for those versions.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a web application

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/), based on the [ASP.NET Core sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md).

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/core/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/master/samples/host-aspnetcore-https.md) to use HTTPS with this image.

# Related Repos

.NET Core 2.1/3.1:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)
* [dotnet/core-nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-core-nightly-sdk/): .NET Core SDK (Preview)
* [dotnet/core-nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime/): .NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime-deps/): .NET Core Runtime Dependencies (Preview)

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
3.1.4-buster-slim, 3.1-buster-slim, 3.1.4, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/buster-slim/amd64/Dockerfile) | Debian 10
3.1.4-alpine3.11, 3.1-alpine3.11, 3.1.4-alpine, 3.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/alpine3.11/amd64/Dockerfile) | Alpine 3.11
3.1.4-focal, 3.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.4-bionic, 3.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/bionic/amd64/Dockerfile) | Ubuntu 18.04
2.1.18-stretch-slim, 2.1-stretch-slim, 2.1.18, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/stretch-slim/amd64/Dockerfile) | Debian 9
2.1.18-alpine3.11, 2.1-alpine3.11, 2.1.18-alpine, 2.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/alpine3.11/amd64/Dockerfile) | Alpine 3.11
2.1.18-focal, 2.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/focal/amd64/Dockerfile) | Ubuntu 20.04
2.1.18-bionic, 2.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/bionic/amd64/Dockerfile) | Ubuntu 18.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.4-buster-slim-arm64v8, 3.1-buster-slim-arm64v8, 3.1.4, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/buster-slim/arm64v8/Dockerfile) | Debian 10
3.1.4-alpine3.11-arm64v8, 3.1-alpine3.11-arm64v8, 3.1.4-alpine-arm64v8, 3.1-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/alpine3.11/arm64v8/Dockerfile) | Alpine 3.11
3.1.4-focal-arm64v8, 3.1-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.4-bionic-arm64v8, 3.1-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
3.1.4-buster-slim-arm32v7, 3.1-buster-slim-arm32v7, 3.1.4, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/buster-slim/arm32v7/Dockerfile) | Debian 10
3.1.4-bionic-arm32v7, 3.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/bionic/arm32v7/Dockerfile) | Ubuntu 18.04
2.1.18-stretch-slim-arm32v7, 2.1-stretch-slim-arm32v7, 2.1.18, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/stretch-slim/arm32v7/Dockerfile) | Debian 9
2.1.18-bionic-arm32v7, 2.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

## Windows Server, version 1909 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.4-nanoserver-1909, 3.1-nanoserver-1909, 3.1.4, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/nanoserver-1909/amd64/Dockerfile)
2.1.18-nanoserver-1909, 2.1-nanoserver-1909, 2.1.18, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/nanoserver-1909/amd64/Dockerfile)

## Windows Server, version 1903 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.4-nanoserver-1903, 3.1-nanoserver-1903, 3.1.4, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/nanoserver-1903/amd64/Dockerfile)
2.1.18-nanoserver-1903, 2.1-nanoserver-1903, 2.1.18, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/nanoserver-1903/amd64/Dockerfile)

## Windows Server 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
3.1.4-nanoserver-1809, 3.1-nanoserver-1809, 3.1.4, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/3.1/aspnet/nanoserver-1809/amd64/Dockerfile)
2.1.18-nanoserver-1809, 2.1-nanoserver-1809, 2.1.18, 2.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/nanoserver-1809/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/core-nightly/aspnet at https://mcr.microsoft.com/v2/dotnet/core-nightly/aspnet/tags/list.

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
