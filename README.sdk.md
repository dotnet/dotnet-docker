# Featured Tags

* `8.0` (Release Candidate)
  * `docker pull mcr.microsoft.com/dotnet/sdk:8.0`
* `7.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:7.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:6.0`

# About

This image contains the .NET SDK which is comprised of three parts:

1. .NET CLI
1. .NET runtime
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Building .NET Apps with Docker

* [.NET Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

## Develop .NET Apps in a Container

The following samples show how to develop, build and test .NET applications with Docker without the need to install the .NET SDK.

* [Build .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/build-in-sdk-container.md)
* [Test .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-tests-in-sdk-container.md)
* [Run .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-in-sdk-container.md)

## Tag Formatting

### .NET Versions

All .NET container images have both "fixed version" and "floating version" tags.
Floating version tags will always reference the latest version of a specific .NET major version, while fixed version tags will always only reference a specific patch version.
For all tags below, `<.NET Version>` can be substituted for either `<Major.Minor>` or `<Major.Minor.Patch>`, for example: `7.0` or `7.0.12`.

### Single-platform tags

These "fixed version" tags reference an image with a specific .NET version for a specific operating system and architecture.

- `<.NET Version>-<OS>-<Architecture>`
- `<.NET Version>-<OS>-<variant>-<Architecture>`
- `<.NET Version>-<OS>-<Architecture>`
- `<.NET Version>-<OS>-<variant>-<Architecture>`

### Multi-platform tags

These tags reference images for [multiple platforms](https://docs.docker.com/build/building/multi-platform/).

- `<.NET Version>`
    - The version-only floating tag refers to the latest Debian version available at the .NET Major Version's release.
- `<.NET Version>-<OS>`
- `<.NET Version>-<OS>-<variant>`

## Image Variants

By default, Ubuntu and Debian images for .NET 8 will have both `icu` and `tzdata` installed.

Our Alpine and Ubuntu Chiseled images are focused on size.
These images do not and will not include `icu` or `tzdata`, meaning that these images only work iwth apps that are configured for [globalization-invariant mode](https://learn.microsoft.com/dotnet/core/runtime-config/globalization).
Apps that require globalization support can use the `extra` image variant.

### (Preview) `aot`

`aot` images provide an optimized deployment size for [native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) compiled .NET apps.
Native AOT has the lowest size, startup time, and memory footprint of all .NET deployment models.
Please see ["Limiatations of Native AOT deployment"](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot#limitations-of-native-aot-deployment) to see if your app might be compatible.

Example tags:
- `8.0.100-jammy-aot`
- `8.0-alpine3.18-aot`

**Note:** `aot` images are only available as a preview in the [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/) and [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/) repos.
Native AOT compiled apps will function exactly the same on the existing `runtime-deps` (non-`aot`) images, but with a larger deployment size.
Please try these new, smaller images out and give us feedback!

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.100-rc.2-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.100-rc.2-bookworm-slim, 8.0-bookworm-slim, 8.0.100-rc.2, 8.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.100-rc.2-alpine3.18-amd64, 8.0-alpine3.18-amd64, 8.0-alpine-amd64, 8.0.100-rc.2-alpine3.18, 8.0-alpine3.18, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
8.0.100-rc.2-jammy-amd64, 8.0-jammy-amd64, 8.0.100-rc.2-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
7.0.403-bookworm-slim-amd64, 7.0-bookworm-slim-amd64, 7.0.403-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bookworm-slim/amd64/Dockerfile) | Debian 12
7.0.403-bullseye-slim-amd64, 7.0-bullseye-slim-amd64, 7.0.403-bullseye-slim, 7.0-bullseye-slim, 7.0.403, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bullseye-slim/amd64/Dockerfile) | Debian 11
7.0.403-alpine3.18-amd64, 7.0-alpine3.18-amd64, 7.0-alpine-amd64, 7.0.403-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
7.0.403-jammy-amd64, 7.0-jammy-amd64, 7.0.403-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.416-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.416-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.416-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.416-bullseye-slim, 6.0-bullseye-slim, 6.0.416, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.416-alpine3.18-amd64, 6.0-alpine3.18-amd64, 6.0-alpine-amd64, 6.0.416-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
6.0.416-jammy-amd64, 6.0-jammy-amd64, 6.0.416-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.416-focal-amd64, 6.0-focal-amd64, 6.0.416-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.100-rc.2-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.100-rc.2-bookworm-slim, 8.0-bookworm-slim, 8.0.100-rc.2, 8.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.100-rc.2-alpine3.18-arm64v8, 8.0-alpine3.18-arm64v8, 8.0-alpine-arm64v8, 8.0.100-rc.2-alpine3.18, 8.0-alpine3.18, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
8.0.100-rc.2-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.100-rc.2-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.403-bookworm-slim-arm64v8, 7.0-bookworm-slim-arm64v8, 7.0.403-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
7.0.403-bullseye-slim-arm64v8, 7.0-bullseye-slim-arm64v8, 7.0.403-bullseye-slim, 7.0-bullseye-slim, 7.0.403, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
7.0.403-alpine3.18-arm64v8, 7.0-alpine3.18-arm64v8, 7.0-alpine-arm64v8, 7.0.403-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
7.0.403-jammy-arm64v8, 7.0-jammy-arm64v8, 7.0.403-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.416-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.416-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.416-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.416-bullseye-slim, 6.0-bullseye-slim, 6.0.416, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.416-alpine3.18-arm64v8, 6.0-alpine3.18-arm64v8, 6.0-alpine-arm64v8, 6.0.416-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
6.0.416-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.416-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.416-focal-arm64v8, 6.0-focal-arm64v8, 6.0.416-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.100-rc.2-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.100-rc.2-bookworm-slim, 8.0-bookworm-slim, 8.0.100-rc.2, 8.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.100-rc.2-alpine3.18-arm32v7, 8.0-alpine3.18-arm32v7, 8.0-alpine-arm32v7, 8.0.100-rc.2-alpine3.18, 8.0-alpine3.18, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
8.0.100-rc.2-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.100-rc.2-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.403-bookworm-slim-arm32v7, 7.0-bookworm-slim-arm32v7, 7.0.403-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
7.0.403-bullseye-slim-arm32v7, 7.0-bullseye-slim-arm32v7, 7.0.403-bullseye-slim, 7.0-bullseye-slim, 7.0.403, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
7.0.403-alpine3.18-arm32v7, 7.0-alpine3.18-arm32v7, 7.0-alpine-arm32v7, 7.0.403-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
7.0.403-jammy-arm32v7, 7.0-jammy-arm32v7, 7.0.403-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.416-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.416-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.416-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.416-bullseye-slim, 6.0-bullseye-slim, 6.0.416, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.416-alpine3.18-arm32v7, 6.0-alpine3.18-arm32v7, 6.0-alpine-arm32v7, 6.0.416-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
6.0.416-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.416-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.416-focal-arm32v7, 6.0-focal-arm32v7, 6.0.416-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.100-rc.2-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/nanoserver-ltsc2022/amd64/Dockerfile)
7.0.403-nanoserver-ltsc2022, 7.0-nanoserver-ltsc2022, 7.0.403, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.416-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.416, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

## Windows Server Core 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.100-rc.2-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)
7.0.403-windowsservercore-ltsc2022, 7.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.416-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.100-rc.2-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/nanoserver-1809/amd64/Dockerfile)
7.0.403-nanoserver-1809, 7.0-nanoserver-1809, 7.0.403, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/nanoserver-1809/amd64/Dockerfile)
6.0.416-nanoserver-1809, 6.0-nanoserver-1809, 6.0.416, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/nanoserver-1809/amd64/Dockerfile)

## Windows Server Core 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.100-rc.2-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
7.0.403-windowsservercore-ltsc2019, 7.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/7.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.416-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/sdk at https://mcr.microsoft.com/v2/dotnet/sdk/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core/sdk repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core/sdk/tags/list.

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
