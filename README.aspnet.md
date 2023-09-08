# Featured Tags

* `7.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/aspnet:7.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/aspnet:6.0`

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
* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.10-bookworm-slim-amd64, 7.0-bookworm-slim-amd64, 7.0.10-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bookworm-slim/amd64/Dockerfile) | Debian 12
7.0.10-bullseye-slim-amd64, 7.0-bullseye-slim-amd64, 7.0.10-bullseye-slim, 7.0-bullseye-slim, 7.0.10, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bullseye-slim/amd64/Dockerfile) | Debian 11
7.0.10-alpine3.18-amd64, 7.0-alpine3.18-amd64, 7.0-alpine-amd64, 7.0.10-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
7.0.10-alpine3.17-amd64, 7.0-alpine3.17-amd64, 7.0.10-alpine3.17, 7.0-alpine3.17 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.17/amd64/Dockerfile) | Alpine 3.17
7.0.10-jammy-amd64, 7.0-jammy-amd64, 7.0.10-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.21-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.21-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.21-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.21-bullseye-slim, 6.0-bullseye-slim, 6.0.21, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.21-alpine3.18-amd64, 6.0-alpine3.18-amd64, 6.0-alpine-amd64, 6.0.21-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
6.0.21-alpine3.17-amd64, 6.0-alpine3.17-amd64, 6.0.21-alpine3.17, 6.0-alpine3.17 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.17/amd64/Dockerfile) | Alpine 3.17
6.0.21-jammy-amd64, 6.0-jammy-amd64, 6.0.21-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.21-focal-amd64, 6.0-focal-amd64, 6.0.21-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04

##### .NET 8 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-preview.7-bookworm-slim-amd64, 8.0-preview-bookworm-slim-amd64, 8.0.0-preview.7-bookworm-slim, 8.0-preview-bookworm-slim, 8.0.0-preview.7, 8.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.0-preview.7-alpine3.18-amd64, 8.0-preview-alpine3.18-amd64, 8.0-preview-alpine-amd64, 8.0.0-preview.7-alpine3.18, 8.0-preview-alpine3.18, 8.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
8.0.0-preview.7-alpine3.18-composite-amd64, 8.0-preview-alpine3.18-composite-amd64, 8.0-preview-alpine-composite-amd64, 8.0.0-preview.7-alpine3.18-composite, 8.0-preview-alpine3.18-composite, 8.0-preview-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18-composite/amd64/Dockerfile) | Alpine 3.18
8.0.0-preview.7-jammy-amd64, 8.0-preview-jammy-amd64, 8.0.0-preview.7-jammy, 8.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.0-preview.7-jammy-chiseled-amd64, 8.0-preview-jammy-chiseled-amd64, 8.0.0-preview.7-jammy-chiseled, 8.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.0-preview.7-jammy-chiseled-composite-amd64, 8.0-preview-jammy-chiseled-composite-amd64, 8.0.0-preview.7-jammy-chiseled-composite, 8.0-preview-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-composite/amd64/Dockerfile) | Ubuntu 22.04

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.10-bookworm-slim-arm64v8, 7.0-bookworm-slim-arm64v8, 7.0.10-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
7.0.10-bullseye-slim-arm64v8, 7.0-bullseye-slim-arm64v8, 7.0.10-bullseye-slim, 7.0-bullseye-slim, 7.0.10, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
7.0.10-alpine3.18-arm64v8, 7.0-alpine3.18-arm64v8, 7.0-alpine-arm64v8, 7.0.10-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
7.0.10-alpine3.17-arm64v8, 7.0-alpine3.17-arm64v8, 7.0.10-alpine3.17, 7.0-alpine3.17 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.17/arm64v8/Dockerfile) | Alpine 3.17
7.0.10-jammy-arm64v8, 7.0-jammy-arm64v8, 7.0.10-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.21-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.21-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.21-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.21-bullseye-slim, 6.0-bullseye-slim, 6.0.21, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.21-alpine3.18-arm64v8, 6.0-alpine3.18-arm64v8, 6.0-alpine-arm64v8, 6.0.21-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
6.0.21-alpine3.17-arm64v8, 6.0-alpine3.17-arm64v8, 6.0.21-alpine3.17, 6.0-alpine3.17 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.17/arm64v8/Dockerfile) | Alpine 3.17
6.0.21-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.21-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.21-focal-arm64v8, 6.0-focal-arm64v8, 6.0.21-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04

##### .NET 8 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-preview.7-bookworm-slim-arm64v8, 8.0-preview-bookworm-slim-arm64v8, 8.0.0-preview.7-bookworm-slim, 8.0-preview-bookworm-slim, 8.0.0-preview.7, 8.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.0-preview.7-alpine3.18-arm64v8, 8.0-preview-alpine3.18-arm64v8, 8.0-preview-alpine-arm64v8, 8.0.0-preview.7-alpine3.18, 8.0-preview-alpine3.18, 8.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
8.0.0-preview.7-alpine3.18-composite-arm64v8, 8.0-preview-alpine3.18-composite-arm64v8, 8.0-preview-alpine-composite-arm64v8, 8.0.0-preview.7-alpine3.18-composite, 8.0-preview-alpine3.18-composite, 8.0-preview-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18-composite/arm64v8/Dockerfile) | Alpine 3.18
8.0.0-preview.7-jammy-arm64v8, 8.0-preview-jammy-arm64v8, 8.0.0-preview.7-jammy, 8.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.0-preview.7-jammy-chiseled-arm64v8, 8.0-preview-jammy-chiseled-arm64v8, 8.0.0-preview.7-jammy-chiseled, 8.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.0-preview.7-jammy-chiseled-composite-arm64v8, 8.0-preview-jammy-chiseled-composite-arm64v8, 8.0.0-preview.7-jammy-chiseled-composite, 8.0-preview-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-composite/arm64v8/Dockerfile) | Ubuntu 22.04

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
7.0.10-bookworm-slim-arm32v7, 7.0-bookworm-slim-arm32v7, 7.0.10-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
7.0.10-bullseye-slim-arm32v7, 7.0-bullseye-slim-arm32v7, 7.0.10-bullseye-slim, 7.0-bullseye-slim, 7.0.10, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
7.0.10-alpine3.18-arm32v7, 7.0-alpine3.18-arm32v7, 7.0-alpine-arm32v7, 7.0.10-alpine3.18, 7.0-alpine3.18, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
7.0.10-alpine3.17-arm32v7, 7.0-alpine3.17-arm32v7, 7.0.10-alpine3.17, 7.0-alpine3.17 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.17/arm32v7/Dockerfile) | Alpine 3.17
7.0.10-jammy-arm32v7, 7.0-jammy-arm32v7, 7.0.10-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.21-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.21-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.21-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.21-bullseye-slim, 6.0-bullseye-slim, 6.0.21, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.21-alpine3.18-arm32v7, 6.0-alpine3.18-arm32v7, 6.0-alpine-arm32v7, 6.0.21-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
6.0.21-alpine3.17-arm32v7, 6.0-alpine3.17-arm32v7, 6.0.21-alpine3.17, 6.0-alpine3.17 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.17/arm32v7/Dockerfile) | Alpine 3.17
6.0.21-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.21-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.21-focal-arm32v7, 6.0-focal-arm32v7, 6.0.21-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

##### .NET 8 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.0-preview.7-bookworm-slim-arm32v7, 8.0-preview-bookworm-slim-arm32v7, 8.0.0-preview.7-bookworm-slim, 8.0-preview-bookworm-slim, 8.0.0-preview.7, 8.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.0-preview.7-alpine3.18-arm32v7, 8.0-preview-alpine3.18-arm32v7, 8.0-preview-alpine-arm32v7, 8.0.0-preview.7-alpine3.18, 8.0-preview-alpine3.18, 8.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
8.0.0-preview.7-alpine3.18-composite-arm32v7, 8.0-preview-alpine3.18-composite-arm32v7, 8.0-preview-alpine-composite-arm32v7, 8.0.0-preview.7-alpine3.18-composite, 8.0-preview-alpine3.18-composite, 8.0-preview-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18-composite/arm32v7/Dockerfile) | Alpine 3.18
8.0.0-preview.7-jammy-arm32v7, 8.0-preview-jammy-arm32v7, 8.0.0-preview.7-jammy, 8.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.0-preview.7-jammy-chiseled-arm32v7, 8.0-preview-jammy-chiseled-arm32v7, 8.0.0-preview.7-jammy-chiseled, 8.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.0-preview.7-jammy-chiseled-composite-arm32v7, 8.0-preview-jammy-chiseled-composite-arm32v7, 8.0.0-preview.7-jammy-chiseled-composite, 8.0-preview-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-composite/arm32v7/Dockerfile) | Ubuntu 22.04

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
7.0.10-nanoserver-ltsc2022, 7.0-nanoserver-ltsc2022, 7.0.10, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.21-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.21, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

##### .NET 8 Preview Tags
Tag | Dockerfile
---------| ---------------
8.0.0-preview.7-nanoserver-ltsc2022, 8.0-preview-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/nanoserver-ltsc2022/amd64/Dockerfile)

## Windows Server Core 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
7.0.10-windowsservercore-ltsc2022, 7.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.21-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

##### .NET 8 Preview Tags
Tag | Dockerfile
---------| ---------------
8.0.0-preview.7-windowsservercore-ltsc2022, 8.0-preview-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
7.0.10-nanoserver-1809, 7.0-nanoserver-1809, 7.0.10, 7.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/nanoserver-1809/amd64/Dockerfile)
6.0.21-nanoserver-1809, 6.0-nanoserver-1809, 6.0.21, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/nanoserver-1809/amd64/Dockerfile)

##### .NET 8 Preview Tags
Tag | Dockerfile
---------| ---------------
8.0.0-preview.7-nanoserver-1809, 8.0-preview-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/nanoserver-1809/amd64/Dockerfile)

## Windows Server Core 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
7.0.10-windowsservercore-ltsc2019, 7.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.21-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)

##### .NET 8 Preview Tags
Tag | Dockerfile
---------| ---------------
8.0.0-preview.7-windowsservercore-ltsc2019, 8.0-preview-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/aspnet at https://mcr.microsoft.com/v2/dotnet/aspnet/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core/aspnet repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core/aspnet/tags/list.

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
