**IMPORTANT**

**The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/) for images with official releases of [.NET](https://github.com/dotnet/core).**

# Featured Tags

* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/aspnet:8.0`
* `7.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/aspnet:7.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/aspnet:6.0`

# About

This image contains the ASP.NET Core and .NET runtimes and libraries and is optimized for running ASP.NET Core apps in production.

## Composite container images

Starting from .NET 8, a composite version of the ASP.NET images, `denoted with the -composite` tag part, is being offered alongside the regular image. The main characteristics of these images are their smaller size on disk while keeping the performance of the default [ReadyToRun (R2R) setting](https://learn.microsoft.com/dotnet/core/deploying/ready-to-run). The caveat is that the composite images have tighter version coupling. This means the final app run on them cannot use handpicked custom versions of the framework and/or ASP.NET assemblies that are built into the composite binary.

For a full technical description on how the composites work, we have a [feature doc here](https://github.com/dotnet/runtime/blob/main/docs/design/features/readytorun-composite-format-design.md).

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a web application

You can quickly run a container with a pre-built [.NET Docker image](https://hub.docker.com/_/microsoft-dotnet-samples/), based on the [ASP.NET Core sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md).

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md) to use HTTPS with this image.

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
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
8.0.0-rc.2-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.0-rc.2-bookworm-slim, 8.0-bookworm-slim, 8.0.0-rc.2, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.0-rc.2-alpine3.18-amd64, 8.0-alpine3.18-amd64, 8.0-alpine-amd64, 8.0.0-rc.2-alpine3.18, 8.0-alpine3.18, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
8.0.0-rc.2-alpine3.18-composite-amd64, 8.0-alpine3.18-composite-amd64, 8.0-alpine-composite-amd64, 8.0.0-rc.2-alpine3.18-composite, 8.0-alpine3.18-composite, 8.0-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/alpine3.18-composite/amd64/Dockerfile) | Alpine 3.18
8.0.0-rc.2-jammy-amd64, 8.0-jammy-amd64, 8.0.0-rc.2-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.0-rc.2-jammy-chiseled-amd64, 8.0-jammy-chiseled-amd64, 8.0.0-rc.2-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.0-rc.2-jammy-chiseled-composite-amd64, 8.0-jammy-chiseled-composite-amd64, 8.0.0-rc.2-jammy-chiseled-composite, 8.0-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/jammy-chiseled-composite/amd64/Dockerfile) | Ubuntu 22.04
7.0.12-bookworm-slim-amd64, 7.0-bookworm-slim-amd64, 7.0.12-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/bookworm-slim/amd64/Dockerfile) | Debian 12
7.0.12-bullseye-slim-amd64, 7.0-bullseye-slim-amd64, 7.0.12-bullseye-slim, 7.0-bullseye-slim, 7.0.12, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/bullseye-slim/amd64/Dockerfile) | Debian 11
7.0.12-alpine3.18-amd64, 7.0-alpine3.18-amd64, 7.0-alpine-amd64, 7.0.12-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
7.0.12-jammy-amd64, 7.0-jammy-amd64, 7.0.12-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
7.0.12-jammy-chiseled-amd64, 7.0-jammy-chiseled-amd64, 7.0.12-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.0.23-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.23-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.23-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.23-bullseye-slim, 6.0-bullseye-slim, 6.0.23, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.23-alpine3.18-amd64, 6.0-alpine3.18-amd64, 6.0-alpine-amd64, 6.0.23-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
6.0.23-jammy-amd64, 6.0-jammy-amd64, 6.0.23-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.23-jammy-chiseled-amd64, 6.0-jammy-chiseled-amd64, 6.0.23-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.0.23-focal-amd64, 6.0-focal-amd64, 6.0.23-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-rc.2-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.0-rc.2-bookworm-slim, 8.0-bookworm-slim, 8.0.0-rc.2, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.0-rc.2-alpine3.18-arm64v8, 8.0-alpine3.18-arm64v8, 8.0-alpine-arm64v8, 8.0.0-rc.2-alpine3.18, 8.0-alpine3.18, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
8.0.0-rc.2-alpine3.18-composite-arm64v8, 8.0-alpine3.18-composite-arm64v8, 8.0-alpine-composite-arm64v8, 8.0.0-rc.2-alpine3.18-composite, 8.0-alpine3.18-composite, 8.0-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/alpine3.18-composite/arm64v8/Dockerfile) | Alpine 3.18
8.0.0-rc.2-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.0-rc.2-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.0-rc.2-jammy-chiseled-arm64v8, 8.0-jammy-chiseled-arm64v8, 8.0.0-rc.2-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.0-rc.2-jammy-chiseled-composite-arm64v8, 8.0-jammy-chiseled-composite-arm64v8, 8.0.0-rc.2-jammy-chiseled-composite, 8.0-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/jammy-chiseled-composite/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.12-bookworm-slim-arm64v8, 7.0-bookworm-slim-arm64v8, 7.0.12-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
7.0.12-bullseye-slim-arm64v8, 7.0-bullseye-slim-arm64v8, 7.0.12-bullseye-slim, 7.0-bullseye-slim, 7.0.12, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
7.0.12-alpine3.18-arm64v8, 7.0-alpine3.18-arm64v8, 7.0-alpine-arm64v8, 7.0.12-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
7.0.12-jammy-arm64v8, 7.0-jammy-arm64v8, 7.0.12-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.12-jammy-chiseled-arm64v8, 7.0-jammy-chiseled-arm64v8, 7.0.12-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.23-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.23-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.23-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.23-bullseye-slim, 6.0-bullseye-slim, 6.0.23, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.23-alpine3.18-arm64v8, 6.0-alpine3.18-arm64v8, 6.0-alpine-arm64v8, 6.0.23-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
6.0.23-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.23-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.23-jammy-chiseled-arm64v8, 6.0-jammy-chiseled-arm64v8, 6.0.23-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.23-focal-arm64v8, 6.0-focal-arm64v8, 6.0.23-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-rc.2-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.0-rc.2-bookworm-slim, 8.0-bookworm-slim, 8.0.0-rc.2, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.0-rc.2-alpine3.18-arm32v7, 8.0-alpine3.18-arm32v7, 8.0-alpine-arm32v7, 8.0.0-rc.2-alpine3.18, 8.0-alpine3.18, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
8.0.0-rc.2-alpine3.18-composite-arm32v7, 8.0-alpine3.18-composite-arm32v7, 8.0-alpine-composite-arm32v7, 8.0.0-rc.2-alpine3.18-composite, 8.0-alpine3.18-composite, 8.0-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/alpine3.18-composite/arm32v7/Dockerfile) | Alpine 3.18
8.0.0-rc.2-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.0-rc.2-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.0-rc.2-jammy-chiseled-arm32v7, 8.0-jammy-chiseled-arm32v7, 8.0.0-rc.2-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.0-rc.2-jammy-chiseled-composite-arm32v7, 8.0-jammy-chiseled-composite-arm32v7, 8.0.0-rc.2-jammy-chiseled-composite, 8.0-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/jammy-chiseled-composite/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.12-bookworm-slim-arm32v7, 7.0-bookworm-slim-arm32v7, 7.0.12-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
7.0.12-bullseye-slim-arm32v7, 7.0-bullseye-slim-arm32v7, 7.0.12-bullseye-slim, 7.0-bullseye-slim, 7.0.12, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
7.0.12-alpine3.18-arm32v7, 7.0-alpine3.18-arm32v7, 7.0-alpine-arm32v7, 7.0.12-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
7.0.12-jammy-arm32v7, 7.0-jammy-arm32v7, 7.0.12-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.12-jammy-chiseled-arm32v7, 7.0-jammy-chiseled-arm32v7, 7.0.12-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.23-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.23-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.23-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.23-bullseye-slim, 6.0-bullseye-slim, 6.0.23, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.23-alpine3.18-arm32v7, 6.0-alpine3.18-arm32v7, 6.0-alpine-arm32v7, 6.0.23-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
6.0.23-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.23-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.23-jammy-chiseled-arm32v7, 6.0-jammy-chiseled-arm32v7, 6.0.23-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.23-focal-arm32v7, 6.0-focal-arm32v7, 6.0.23-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.0-rc.2-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/nanoserver-ltsc2022/amd64/Dockerfile)
7.0.12-nanoserver-ltsc2022, 7.0-nanoserver-ltsc2022, 7.0.12, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.23-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.23, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

## Windows Server Core 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.0-rc.2-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)
7.0.12-windowsservercore-ltsc2022, 7.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.23-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.0-rc.2-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/nanoserver-1809/amd64/Dockerfile)
7.0.12-nanoserver-1809, 7.0-nanoserver-1809, 7.0.12, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/nanoserver-1809/amd64/Dockerfile)
6.0.23-nanoserver-1809, 6.0-nanoserver-1809, 6.0.23, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/nanoserver-1809/amd64/Dockerfile)

## Windows Server Core 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.0-rc.2-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
7.0.12-windowsservercore-ltsc2019, 7.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/7.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.23-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/aspnet/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/nightly/aspnet at https://mcr.microsoft.com/v2/dotnet/nightly/aspnet/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core-nightly/aspnet repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core-nightly/aspnet/tags/list.

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
