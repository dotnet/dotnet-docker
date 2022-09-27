**IMPORTANT**

**The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet](https://hub.docker.com/_/microsoft-dotnet-monitor/) for images with official releases of [.NET](https://github.com/dotnet/core).**

# Featured Tags

* `7` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:7`
* `6` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:6`

# About

This image contains the .NET Monitor tool.

Use this image as a sidecar container to collect diagnostic information from other containers running .NET Core 3.1 or later processes.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run the tool

You can run a container with a pre-built [.NET Docker Image](https://hub.docker.com/_/microsoft-dotnet-monitor/), based on the dotnet-monitor global tool.

See the [documentation](https://go.microsoft.com/fwlink/?linkid=2158052) for how to configure the image to be run in a Docker or Kubernetes environment, including how to configure authentication and certificates for https bindings.

# Related Repos

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
6.3.0-alpine-amd64, 6.3-alpine-amd64, 6-alpine-amd64, 6.3.0-alpine, 6.3-alpine, 6-alpine, 6.3.0, 6.3, 6 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/alpine/amd64/Dockerfile) | Alpine 3.16
6.3.0-ubuntu-chiseled-amd64, 6.3-ubuntu-chiseled-amd64, 6-ubuntu-chiseled-amd64, 6.3.0-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.2.2-alpine-amd64, 6.2-alpine-amd64, 6.2.2-alpine, 6.2-alpine, 6.2.2, 6.2 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.2/alpine/amd64/Dockerfile) | Alpine 3.16

##### .NET Monitor Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.0-rc.1-alpine-amd64, 7.0-alpine-amd64, 7-alpine-amd64, 7.0.0-rc.1-alpine, 7.0-alpine, 7-alpine, 7.0.0-rc.1, 7.0, 7, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/7.0/alpine/amd64/Dockerfile) | Alpine 3.16
7.0.0-rc.1-ubuntu-chiseled-amd64, 7.0-ubuntu-chiseled-amd64, 7-ubuntu-chiseled-amd64, 7.0.0-rc.1-ubuntu-chiseled, 7.0-ubuntu-chiseled, 7-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/7.0/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
6.3.0-alpine-arm64v8, 6.3-alpine-arm64v8, 6.3.0-alpine, 6.3-alpine, 6-alpine, 6.3.0, 6.3, 6 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/alpine/arm64v8/Dockerfile) | Alpine 3.16
6.3.0-ubuntu-chiseled-arm64v8, 6.3-ubuntu-chiseled-arm64v8, 6-ubuntu-chiseled-arm64v8, 6.3.0-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.2.2-alpine-arm64v8, 6.2-alpine-arm64v8, 6.2.2-alpine, 6.2-alpine, 6.2.2, 6.2 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.2/alpine/arm64v8/Dockerfile) | Alpine 3.16

##### .NET Monitor Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.0-rc.1-alpine-arm64v8, 7.0-alpine-arm64v8, 7-alpine-arm64v8, 7.0.0-rc.1-alpine, 7.0-alpine, 7-alpine, 7.0.0-rc.1, 7.0, 7, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/7.0/alpine/arm64v8/Dockerfile) | Alpine 3.16
7.0.0-rc.1-ubuntu-chiseled-arm64v8, 7.0-ubuntu-chiseled-arm64v8, 7-ubuntu-chiseled-arm64v8, 7.0.0-rc.1-ubuntu-chiseled, 7.0-ubuntu-chiseled, 7-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/7.0/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04

You can retrieve a list of all available tags for dotnet/nightly/monitor at https://mcr.microsoft.com/v2/dotnet/nightly/monitor/tags/list.
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)*

# Support

## Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/microsoft-support.md)
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
