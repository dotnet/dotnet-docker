# Featured Tags

* `5.0` (Current)
  * `docker pull mcr.microsoft.com/dotnet/runtime-deps:5.0`
* `3.1` (LTS)
  * `docker pull mcr.microsoft.com/dotnet/runtime-deps:3.1`

# About This Image

This image contains the native dependencies needed by .NET. It does not include .NET. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

* [.NET self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/dotnet-docker-selfcontained.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.debian-x64-selfcontained) builds and runs an application as a self-contained application.

# Related Repos

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.9-buster-slim-amd64, 5.0-buster-slim-amd64, 5.0.9, 5.0.9-buster-slim, 5.0, 5.0-buster-slim, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/buster-slim/amd64/Dockerfile) | Debian 10
5.0.9-alpine3.13-amd64, 5.0-alpine3.13-amd64, 5.0-alpine-amd64, 5.0.9-alpine3.13, 5.0-alpine3.13, 5.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/alpine3.13/amd64/Dockerfile) | Alpine 3.13
5.0.9-focal-amd64, 5.0-focal-amd64, 5.0.9-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.18-buster-slim, 3.1-buster-slim, 3.1.18, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/buster-slim/amd64/Dockerfile) | Debian 10
3.1.18-alpine3.13, 3.1-alpine3.13, 3.1-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/alpine3.13/amd64/Dockerfile) | Alpine 3.13
3.1.18-focal, 3.1-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/focal/amd64/Dockerfile) | Ubuntu 20.04
3.1.18-bionic, 3.1-bionic | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/bionic/amd64/Dockerfile) | Ubuntu 18.04

##### .NET 6.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
6.0.0-preview.7-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.0-preview.7, 6.0.0-preview.7-bullseye-slim, 6.0, 6.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.0-preview.7-alpine3.13-amd64, 6.0-alpine3.13-amd64, 6.0-alpine-amd64, 6.0.0-preview.7-alpine3.13, 6.0-alpine3.13, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/alpine3.13/amd64/Dockerfile) | Alpine 3.13
6.0.0-preview.7-focal-amd64, 6.0-focal-amd64, 6.0.0-preview.7-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/focal/amd64/Dockerfile) | Ubuntu 20.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.9-buster-slim-arm64v8, 5.0-buster-slim-arm64v8, 5.0.9, 5.0.9-buster-slim, 5.0, 5.0-buster-slim, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/buster-slim/arm64v8/Dockerfile) | Debian 10
5.0.9-alpine3.13-arm64v8, 5.0-alpine3.13-arm64v8, 5.0-alpine-arm64v8, 5.0.9-alpine3.13, 5.0-alpine3.13, 5.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/alpine3.13/arm64v8/Dockerfile) | Alpine 3.13
5.0.9-focal-arm64v8, 5.0-focal-arm64v8, 5.0.9-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.18-buster-slim-arm64v8, 3.1-buster-slim-arm64v8, 3.1.18, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/buster-slim/arm64v8/Dockerfile) | Debian 10
3.1.18-alpine3.13-arm64v8, 3.1-alpine3.13-arm64v8, 3.1-alpine-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/alpine3.13/arm64v8/Dockerfile) | Alpine 3.13
3.1.18-focal-arm64v8, 3.1-focal-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/focal/arm64v8/Dockerfile) | Ubuntu 20.04
3.1.18-bionic-arm64v8, 3.1-bionic-arm64v8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/bionic/arm64v8/Dockerfile) | Ubuntu 18.04

##### .NET 6.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
6.0.0-preview.7-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.0-preview.7, 6.0.0-preview.7-bullseye-slim, 6.0, 6.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.0-preview.7-alpine3.13-arm64v8, 6.0-alpine3.13-arm64v8, 6.0-alpine-arm64v8, 6.0.0-preview.7-alpine3.13, 6.0-alpine3.13, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/alpine3.13/arm64v8/Dockerfile) | Alpine 3.13
6.0.0-preview.7-focal-arm64v8, 6.0-focal-arm64v8, 6.0.0-preview.7-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/focal/arm64v8/Dockerfile) | Ubuntu 20.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0.9-buster-slim-arm32v7, 5.0-buster-slim-arm32v7, 5.0.9, 5.0.9-buster-slim, 5.0, 5.0-buster-slim, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/buster-slim/arm32v7/Dockerfile) | Debian 10
5.0.9-alpine3.13-arm32v7, 5.0-alpine3.13-arm32v7, 5.0.9-alpine3.13, 5.0-alpine3.13, 5.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/5.0/alpine3.13/arm32v7/Dockerfile) | Alpine 3.13
5.0.9-focal-arm32v7, 5.0-focal-arm32v7, 5.0.9-focal, 5.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/focal/arm32v7/Dockerfile) | Ubuntu 20.04
3.1.18-buster-slim-arm32v7, 3.1-buster-slim-arm32v7, 3.1.18, 3.1 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/buster-slim/arm32v7/Dockerfile) | Debian 10
3.1.18-focal-arm32v7, 3.1-focal-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/focal/arm32v7/Dockerfile) | Ubuntu 20.04
3.1.18-bionic-arm32v7, 3.1-bionic-arm32v7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/bionic/arm32v7/Dockerfile) | Ubuntu 18.04

##### .NET 6.0 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
6.0.0-preview.7-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.0-preview.7, 6.0.0-preview.7-bullseye-slim, 6.0, 6.0-bullseye-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.0-preview.7-alpine3.13-arm32v7, 6.0-alpine3.13-arm32v7, 6.0-alpine-arm32v7, 6.0.0-preview.7-alpine3.13, 6.0-alpine3.13, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/5.0/alpine3.13/arm32v7/Dockerfile) | Alpine 3.13
6.0.0-preview.7-focal-arm32v7, 6.0-focal-arm32v7, 6.0.0-preview.7-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/3.1/focal/arm32v7/Dockerfile) | Ubuntu 20.04

You can retrieve a list of all available tags for dotnet/runtime-deps at https://mcr.microsoft.com/v2/dotnet/runtime-deps/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core/runtime-deps repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core/runtime-deps/tags/list.

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
