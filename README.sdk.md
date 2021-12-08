The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).

See [dotnet](https://hub.docker.com/_/microsoft-dotnet/) for images with official releases of [.NET](https://github.com/dotnet/core).

# Featured Tags

 * `7.0` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/nightly/sdk:7.0`
* `6.0` (Current, LTS)
  * `docker pull mcr.microsoft.com/dotnet/nightly/sdk:6.0`

# About This Image

This image contains the .NET SDK which is comprised of three parts:

1. .NET CLI
1. .NET runtime
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Building .NET Apps with Docker

* [.NET Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

## Develop .NET Apps in a Container

The following samples show how to develop, build and test .NET applications with Docker without the need to install the .NET SDK.

* [Build .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/build-in-sdk-container.md)
* [Test .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-tests-in-sdk-container.md)
* [Run .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-in-sdk-container.md)


# Related Repos

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor/): .NET Monitor Tool (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
6.0.100-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.100-bullseye-slim, 6.0-bullseye-slim, 6.0.100, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.100-alpine3.15-amd64, 6.0-alpine3.15-amd64, 6.0-alpine-amd64, 6.0.100-alpine3.15, 6.0-alpine3.15, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/alpine3.15/amd64/Dockerfile) | Alpine 3.15
6.0.100-alpine3.14-amd64, 6.0-alpine3.14-amd64, 6.0.100-alpine3.14, 6.0-alpine3.14 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/alpine3.14/amd64/Dockerfile) | Alpine 3.14
6.0.100-focal-amd64, 6.0-focal-amd64, 6.0.100-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04
5.0.403-bullseye-slim-amd64, 5.0-bullseye-slim-amd64, 5.0.403-bullseye-slim, 5.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/bullseye-slim/amd64/Dockerfile) | Debian 11
5.0.403-buster-slim-amd64, 5.0-buster-slim-amd64, 5.0.403-buster-slim, 5.0-buster-slim, 5.0.403, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/buster-slim/amd64/Dockerfile) | Debian 10
5.0.403-alpine3.15-amd64, 5.0-alpine3.15-amd64, 5.0-alpine-amd64, 5.0.403-alpine3.15, 5.0-alpine3.15, 5.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/alpine3.15/amd64/Dockerfile) | Alpine 3.15
5.0.403-alpine3.14-amd64, 5.0-alpine3.14-amd64, 5.0.403-alpine3.14, 5.0-alpine3.14 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/alpine3.14/amd64/Dockerfile) | Alpine 3.14
5.0.403-focal-amd64, 5.0-focal-amd64, 5.0.403-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.415-bullseye, 3.1-bullseye | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/bullseye/amd64/Dockerfile) | Debian 11
3.1.415-buster, 3.1-buster, 3.1.415, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/buster/amd64/Dockerfile) | Debian 10
3.1.415-alpine3.15, 3.1-alpine3.15, 3.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/alpine3.15/amd64/Dockerfile) | Alpine 3.15
3.1.415-alpine3.14, 3.1-alpine3.14 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/alpine3.14/amd64/Dockerfile) | Alpine 3.14
3.1.415-focal, 3.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.415-bionic, 3.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/bionic/amd64/Dockerfile) | Ubuntu 18.04

##### .NET 7 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.100-alpha.1-bullseye-slim-amd64, 7.0-bullseye-slim-amd64, 7.0.100-alpha.1-bullseye-slim, 7.0-bullseye-slim, 7.0.100-alpha.1, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/bullseye-slim/amd64/Dockerfile) | Debian 11
7.0.100-alpha.1-alpine3.15-amd64, 7.0-alpine3.15-amd64, 7.0-alpine-amd64, 7.0.100-alpha.1-alpine3.15, 7.0-alpine3.15, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/alpine3.15/amd64/Dockerfile) | Alpine 3.15
7.0.100-alpha.1-focal-amd64, 7.0-focal-amd64, 7.0.100-alpha.1-focal, 7.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/focal/amd64/Dockerfile) | Ubuntu 20.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
6.0.100-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.100-bullseye-slim, 6.0-bullseye-slim, 6.0.100, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.100-alpine3.15-arm64v8, 6.0-alpine3.15-arm64v8, 6.0-alpine-arm64v8, 6.0.100-alpine3.15, 6.0-alpine3.15, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/alpine3.15/arm64v8/Dockerfile) | Alpine 3.15
6.0.100-alpine3.14-arm64v8, 6.0-alpine3.14-arm64v8, 6.0.100-alpine3.14, 6.0-alpine3.14 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/alpine3.14/arm64v8/Dockerfile) | Alpine 3.14
6.0.100-focal-arm64v8, 6.0-focal-arm64v8, 6.0.100-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04
5.0.403-bullseye-slim-arm64v8, 5.0-bullseye-slim-arm64v8, 5.0.403-bullseye-slim, 5.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
5.0.403-buster-slim-arm64v8, 5.0-buster-slim-arm64v8, 5.0.403-buster-slim, 5.0-buster-slim, 5.0.403, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/buster-slim/arm64v8/Dockerfile) | Debian 10
5.0.403-focal-arm64v8, 5.0-focal-arm64v8, 5.0.403-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.415-bullseye-arm64v8, 3.1-bullseye-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/bullseye/arm64v8/Dockerfile) | Debian 11
3.1.415-buster-arm64v8, 3.1-buster-arm64v8, 3.1.415, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/buster/arm64v8/Dockerfile) | Debian 10
3.1.415-focal-arm64v8, 3.1-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.415-bionic-arm64v8, 3.1-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

##### .NET 7 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.100-alpha.1-bullseye-slim-arm64v8, 7.0-bullseye-slim-arm64v8, 7.0.100-alpha.1-bullseye-slim, 7.0-bullseye-slim, 7.0.100-alpha.1, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
7.0.100-alpha.1-alpine3.15-arm64v8, 7.0-alpine3.15-arm64v8, 7.0-alpine-arm64v8, 7.0.100-alpha.1-alpine3.15, 7.0-alpine3.15, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/alpine3.15/arm64v8/Dockerfile) | Alpine 3.15
7.0.100-alpha.1-focal-arm64v8, 7.0-focal-arm64v8, 7.0.100-alpha.1-focal, 7.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
6.0.100-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.100-bullseye-slim, 6.0-bullseye-slim, 6.0.100, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.100-alpine3.15-arm32v7, 6.0-alpine3.15-arm32v7, 6.0-alpine-arm32v7, 6.0.100-alpine3.15, 6.0-alpine3.15, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/alpine3.15/arm32v7/Dockerfile) | Alpine 3.15
6.0.100-alpine3.14-arm32v7, 6.0-alpine3.14-arm32v7, 6.0.100-alpine3.14, 6.0-alpine3.14 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/alpine3.14/arm32v7/Dockerfile) | Alpine 3.14
6.0.100-focal-arm32v7, 6.0-focal-arm32v7, 6.0.100-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04
5.0.403-bullseye-slim-arm32v7, 5.0-bullseye-slim-arm32v7, 5.0.403-bullseye-slim, 5.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
5.0.403-buster-slim-arm32v7, 5.0-buster-slim-arm32v7, 5.0.403-buster-slim, 5.0-buster-slim, 5.0.403, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/buster-slim/arm32v7/Dockerfile) | Debian 10
5.0.403-focal-arm32v7, 5.0-focal-arm32v7, 5.0.403-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04
3.1.415-bullseye-arm32v7, 3.1-bullseye-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/bullseye/arm32v7/Dockerfile) | Debian 11
3.1.415-buster-arm32v7, 3.1-buster-arm32v7, 3.1.415, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/buster/arm32v7/Dockerfile) | Debian 10
3.1.415-focal-arm32v7, 3.1-focal-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/focal/arm32v7/Dockerfile) | Ubuntu 20.04
3.1.415-bionic-arm32v7, 3.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

##### .NET 7 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.100-alpha.1-bullseye-slim-arm32v7, 7.0-bullseye-slim-arm32v7, 7.0.100-alpha.1-bullseye-slim, 7.0-bullseye-slim, 7.0.100-alpha.1, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
7.0.100-alpha.1-alpine3.15-arm32v7, 7.0-alpine3.15-arm32v7, 7.0-alpine-arm32v7, 7.0.100-alpha.1-alpine3.15, 7.0-alpine3.15, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/alpine3.15/arm32v7/Dockerfile) | Alpine 3.15
7.0.100-alpha.1-focal-arm32v7, 7.0-focal-arm32v7, 7.0.100-alpha.1-focal, 7.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
6.0.100-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.100, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/nanoserver-ltsc2022/amd64/Dockerfile)
5.0.403-nanoserver-ltsc2022, 5.0-nanoserver-ltsc2022, 5.0.403, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/nanoserver-ltsc2022/amd64/Dockerfile)
3.1.415-nanoserver-ltsc2022, 3.1-nanoserver-ltsc2022, 3.1.415, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/nanoserver-ltsc2022/amd64/Dockerfile)

##### .NET 7 Preview Tags
Tag | Dockerfile
---------| ---------------
7.0.100-alpha.1-nanoserver-ltsc2022, 7.0-nanoserver-ltsc2022, 7.0.100-alpha.1, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/nanoserver-ltsc2022/amd64/Dockerfile)

## Windows Server Core 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
6.0.100-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)
5.0.403-windowsservercore-ltsc2022, 5.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/windowsservercore-ltsc2022/amd64/Dockerfile)

##### .NET 7 Preview Tags
Tag | Dockerfile
---------| ---------------
7.0.100-alpha.1-windowsservercore-ltsc2022, 7.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/windowsservercore-ltsc2022/amd64/Dockerfile)

## Nano Server, version 20H2 amd64 Tags
Tag | Dockerfile
---------| ---------------
6.0.100-nanoserver-20H2, 6.0-nanoserver-20H2, 6.0.100, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/nanoserver-20H2/amd64/Dockerfile)
5.0.403-nanoserver-20H2, 5.0-nanoserver-20H2, 5.0.403, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/nanoserver-20H2/amd64/Dockerfile)
3.1.415-nanoserver-20H2, 3.1-nanoserver-20H2, 3.1.415, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/nanoserver-20H2/amd64/Dockerfile)

## Nano Server, version 2004 amd64 Tags
Tag | Dockerfile
---------| ---------------
5.0.403-nanoserver-2004, 5.0-nanoserver-2004, 5.0.403, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/nanoserver-2004/amd64/Dockerfile)
3.1.415-nanoserver-2004, 3.1-nanoserver-2004, 3.1.415, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/nanoserver-2004/amd64/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
6.0.100-nanoserver-1809, 6.0-nanoserver-1809, 6.0.100, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/nanoserver-1809/amd64/Dockerfile)
5.0.403-nanoserver-1809, 5.0-nanoserver-1809, 5.0.403, 5.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/nanoserver-1809/amd64/Dockerfile)
3.1.415-nanoserver-1809, 3.1-nanoserver-1809, 3.1.415, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/3.1/nanoserver-1809/amd64/Dockerfile)

##### .NET 7 Preview Tags
Tag | Dockerfile
---------| ---------------
7.0.100-alpha.1-nanoserver-1809, 7.0-nanoserver-1809, 7.0.100-alpha.1, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/nanoserver-1809/amd64/Dockerfile)

## Windows Server Core 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
6.0.100-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)
5.0.403-windowsservercore-ltsc2019, 5.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/5.0/windowsservercore-ltsc2019/amd64/Dockerfile)

##### .NET 7 Preview Tags
Tag | Dockerfile
---------| ---------------
7.0.100-alpha.1-windowsservercore-ltsc2019, 7.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/sdk/7.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/nightly/sdk at https://mcr.microsoft.com/v2/dotnet/nightly/sdk/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core-nightly/sdk repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core-nightly/sdk/tags/list.

# Support

See [Microsoft Support for .NET](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Image Update Policy

* We update the supported .NET images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:ltsc2022, buildpack-deps:bionic-scm, etc.).
* We publish .NET images as part of releasing new versions of .NET including major/minor and servicing.

# Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
