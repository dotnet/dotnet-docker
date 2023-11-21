# Featured Tags

* `8` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor:8`
* `7` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor:7`
* `6` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor:6`

# About

This image contains the .NET Monitor tool.

Use this image as a sidecar container to collect diagnostic information from other containers running .NET Core 3.1 or later processes.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## New: Ubuntu Chiseled Images

Ubuntu Chiseled .NET images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface by including only the minimal set of packages required to run .NET applications.

Please see the [Ubuntu Chiseled + .NET](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md) documentation page for more info.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run the tool

You can run a container with a pre-built [.NET Docker Image](https://hub.docker.com/_/microsoft-dotnet-monitor/), based on the dotnet-monitor global tool.

See the [documentation](https://go.microsoft.com/fwlink/?linkid=2158052) for how to configure the image to be run in a Docker or Kubernetes environment, including how to configure authentication and certificates for https bindings.

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor/base](https://hub.docker.com/_/microsoft-dotnet-monitor-base/): .NET Monitor Base
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/monitor](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor/): .NET Monitor Tool (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-ubuntu-chiseled-amd64, 8.0-ubuntu-chiseled-amd64, 8-ubuntu-chiseled-amd64, 8.0.0-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.0, 8.0, 8, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.0-cbl-mariner-distroless-amd64, 8.0-cbl-mariner-distroless-amd64, 8-cbl-mariner-distroless-amd64, 8.0.0-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0-distroless
7.3.2-alpine-amd64, 7.3-alpine-amd64, 7-alpine-amd64, 7.3.2-alpine, 7.3-alpine, 7-alpine, 7.3.2, 7.3, 7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/7.3/alpine/amd64/Dockerfile) | Alpine 3.18
7.3.2-ubuntu-chiseled-amd64, 7.3-ubuntu-chiseled-amd64, 7-ubuntu-chiseled-amd64, 7.3.2-ubuntu-chiseled, 7.3-ubuntu-chiseled, 7-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/7.3/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
7.3.2-cbl-mariner-amd64, 7.3-cbl-mariner-amd64, 7-cbl-mariner-amd64, 7.3.2-cbl-mariner, 7.3-cbl-mariner, 7-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/7.3/cbl-mariner/amd64/Dockerfile) | CBL-Mariner 2.0
7.3.2-cbl-mariner-distroless-amd64, 7.3-cbl-mariner-distroless-amd64, 7-cbl-mariner-distroless-amd64, 7.3.2-cbl-mariner-distroless, 7.3-cbl-mariner-distroless, 7-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/7.3/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0-distroless
6.3.4-alpine-amd64, 6.3-alpine-amd64, 6-alpine-amd64, 6.3.4-alpine, 6.3-alpine, 6-alpine, 6.3.4, 6.3, 6 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/alpine/amd64/Dockerfile) | Alpine 3.18
6.3.4-ubuntu-chiseled-amd64, 6.3-ubuntu-chiseled-amd64, 6-ubuntu-chiseled-amd64, 6.3.4-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.3.4-cbl-mariner-amd64, 6.3-cbl-mariner-amd64, 6-cbl-mariner-amd64, 6.3.4-cbl-mariner, 6.3-cbl-mariner, 6-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/cbl-mariner/amd64/Dockerfile) | CBL-Mariner 2.0
6.3.4-cbl-mariner-distroless-amd64, 6.3-cbl-mariner-distroless-amd64, 6-cbl-mariner-distroless-amd64, 6.3.4-cbl-mariner-distroless, 6.3-cbl-mariner-distroless, 6-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0-distroless

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-ubuntu-chiseled-arm64v8, 8.0-ubuntu-chiseled-arm64v8, 8-ubuntu-chiseled-arm64v8, 8.0.0-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.0, 8.0, 8, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.0-cbl-mariner-distroless-arm64v8, 8.0-cbl-mariner-distroless-arm64v8, 8-cbl-mariner-distroless-arm64v8, 8.0.0-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0-distroless
7.3.2-alpine-arm64v8, 7.3-alpine-arm64v8, 7-alpine-arm64v8, 7.3.2-alpine, 7.3-alpine, 7-alpine, 7.3.2, 7.3, 7 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/7.3/alpine/arm64v8/Dockerfile) | Alpine 3.18
7.3.2-ubuntu-chiseled-arm64v8, 7.3-ubuntu-chiseled-arm64v8, 7-ubuntu-chiseled-arm64v8, 7.3.2-ubuntu-chiseled, 7.3-ubuntu-chiseled, 7-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/7.3/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
7.3.2-cbl-mariner-arm64v8, 7.3-cbl-mariner-arm64v8, 7-cbl-mariner-arm64v8, 7.3.2-cbl-mariner, 7.3-cbl-mariner, 7-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/7.3/cbl-mariner/arm64v8/Dockerfile) | CBL-Mariner 2.0
7.3.2-cbl-mariner-distroless-arm64v8, 7.3-cbl-mariner-distroless-arm64v8, 7-cbl-mariner-distroless-arm64v8, 7.3.2-cbl-mariner-distroless, 7.3-cbl-mariner-distroless, 7-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/7.3/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0-distroless
6.3.4-alpine-arm64v8, 6.3-alpine-arm64v8, 6-alpine-arm64v8, 6.3.4-alpine, 6.3-alpine, 6-alpine, 6.3.4, 6.3, 6 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/alpine/arm64v8/Dockerfile) | Alpine 3.18
6.3.4-ubuntu-chiseled-arm64v8, 6.3-ubuntu-chiseled-arm64v8, 6-ubuntu-chiseled-arm64v8, 6.3.4-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.3.4-cbl-mariner-arm64v8, 6.3-cbl-mariner-arm64v8, 6-cbl-mariner-arm64v8, 6.3.4-cbl-mariner, 6.3-cbl-mariner, 6-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/cbl-mariner/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.3.4-cbl-mariner-distroless-arm64v8, 6.3-cbl-mariner-distroless-arm64v8, 6-cbl-mariner-distroless-arm64v8, 6.3.4-cbl-mariner-distroless, 6.3-cbl-mariner-distroless, 6-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0-distroless

You can retrieve a list of all available tags for dotnet/monitor at https://mcr.microsoft.com/v2/dotnet/monitor/tags/list.
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)*

# Support

## Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

## Image Update Policy

* We update the supported .NET images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:ltsc2022, buildpack-deps:bionic-scm, etc.).
* We publish .NET images as part of releasing new versions of .NET including major/minor and servicing.

## Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
