# Featured Tags

* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/aspnet:8.0`
* `7.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/aspnet:7.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/aspnet:6.0`

# About

This image contains the ASP.NET Core and .NET runtimes and libraries and is optimized for running ASP.NET Core apps in production.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## New: Ubuntu Chiseled Images

Ubuntu Chiseled .NET images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface by including only the minimal set of packages required to run .NET applications.

Please see the [Ubuntu Chiseled + .NET](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md) documentation page for more info.

## ASP.NET Core Composite Images

Starting from .NET 8, ASP.NET Core Composite images are optimized for performance using [ReadyToRun (R2R) compilation](https://learn.microsoft.com/dotnet/core/deploying/ready-to-run).
For more information, see the [composite images section in the Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md#composite-net-80).

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a web application

You can quickly run a container with a pre-built [.NET Docker image](https://hub.docker.com/_/microsoft-dotnet-samples/), based on the [ASP.NET Core sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md).

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:8080 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. You can also view the ASP.NET Core site running in the container from another machine with a local IP address such as `http://192.168.1.18:8000`.

> Note: ASP.NET Core apps (in official images) listen to [port 8080 by default](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7), starting with .NET 8. The [`-p` argument](https://docs.docker.com/engine/reference/commandline/run/#publish) in these examples maps host port `8000` to container port `8080` (`host:container` mapping). The container will not be accessible without this mapping. ASP.NET Core can be [configured to listen on a different or additional port](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel/endpoints).

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md) to use HTTPS with this image.

# Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

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
8.0.4-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.4-bookworm-slim, 8.0-bookworm-slim, 8.0.4, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.4-alpine3.18-amd64, 8.0-alpine3.18-amd64, 8.0-alpine-amd64, 8.0.4-alpine3.18, 8.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
8.0.4-alpine3.18-composite-amd64, 8.0-alpine3.18-composite-amd64, 8.0-alpine-composite-amd64, 8.0.4-alpine3.18-composite, 8.0-alpine3.18-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18-composite/amd64/Dockerfile) | Alpine 3.18
8.0.4-alpine3.19-amd64, 8.0-alpine3.19-amd64, 8.0.4-alpine3.19, 8.0-alpine3.19, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
8.0.4-alpine3.19-composite-amd64, 8.0-alpine3.19-composite-amd64, 8.0.4-alpine3.19-composite, 8.0-alpine3.19-composite, 8.0-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.19-composite/amd64/Dockerfile) | Alpine 3.19
8.0.4-jammy-amd64, 8.0-jammy-amd64, 8.0.4-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-amd64, 8.0-jammy-chiseled-amd64, 8.0.4-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-extra-amd64, 8.0-jammy-chiseled-extra-amd64, 8.0.4-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-composite-amd64, 8.0-jammy-chiseled-composite-amd64, 8.0.4-jammy-chiseled-composite, 8.0-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-composite/amd64/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-composite-extra-amd64, 8.0-jammy-chiseled-composite-extra-amd64, 8.0.4-jammy-chiseled-composite-extra, 8.0-jammy-chiseled-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-composite-extra/amd64/Dockerfile) | Ubuntu 22.04
8.0.4-cbl-mariner2.0-amd64, 8.0-cbl-mariner2.0-amd64, 8.0.4-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.4-cbl-mariner2.0-distroless-amd64, 8.0-cbl-mariner2.0-distroless-amd64, 8.0.4-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.4-cbl-mariner2.0-distroless-extra-amd64, 8.0-cbl-mariner2.0-distroless-extra-amd64, 8.0.4-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0-distroless-extra/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.4-cbl-mariner2.0-distroless-composite-amd64, 8.0-cbl-mariner2.0-distroless-composite-amd64, 8.0.4-cbl-mariner2.0-distroless-composite, 8.0-cbl-mariner2.0-distroless-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0-distroless-composite/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.4-cbl-mariner2.0-distroless-composite-extra-amd64, 8.0-cbl-mariner2.0-distroless-composite-extra-amd64, 8.0.4-cbl-mariner2.0-distroless-composite-extra, 8.0-cbl-mariner2.0-distroless-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0-distroless-composite-extra/amd64/Dockerfile) | CBL-Mariner 2.0
7.0.18-bookworm-slim-amd64, 7.0-bookworm-slim-amd64, 7.0.18-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bookworm-slim/amd64/Dockerfile) | Debian 12
7.0.18-bullseye-slim-amd64, 7.0-bullseye-slim-amd64, 7.0.18-bullseye-slim, 7.0-bullseye-slim, 7.0.18, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bullseye-slim/amd64/Dockerfile) | Debian 11
7.0.18-alpine3.18-amd64, 7.0-alpine3.18-amd64, 7.0-alpine-amd64, 7.0.18-alpine3.18, 7.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
7.0.18-alpine3.19-amd64, 7.0-alpine3.19-amd64, 7.0.18-alpine3.19, 7.0-alpine3.19, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
7.0.18-jammy-amd64, 7.0-jammy-amd64, 7.0.18-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
7.0.18-jammy-chiseled-amd64, 7.0-jammy-chiseled-amd64, 7.0.18-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
7.0.18-jammy-chiseled-extra-amd64, 7.0-jammy-chiseled-extra-amd64, 7.0.18-jammy-chiseled-extra, 7.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
7.0.18-cbl-mariner2.0-amd64, 7.0-cbl-mariner2.0-amd64, 7.0.18-cbl-mariner2.0, 7.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
7.0.18-cbl-mariner2.0-distroless-amd64, 7.0-cbl-mariner2.0-distroless-amd64, 7.0.18-cbl-mariner2.0-distroless, 7.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.29-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.29-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.29-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.29-bullseye-slim, 6.0-bullseye-slim, 6.0.29, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.29-alpine3.18-amd64, 6.0-alpine3.18-amd64, 6.0-alpine-amd64, 6.0.29-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.18/amd64/Dockerfile) | Alpine 3.18
6.0.29-alpine3.19-amd64, 6.0-alpine3.19-amd64, 6.0.29-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
6.0.29-jammy-amd64, 6.0-jammy-amd64, 6.0.29-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.29-jammy-chiseled-amd64, 6.0-jammy-chiseled-amd64, 6.0.29-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.0.29-jammy-chiseled-extra-amd64, 6.0-jammy-chiseled-extra-amd64, 6.0.29-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
6.0.29-cbl-mariner2.0-amd64, 6.0-cbl-mariner2.0-amd64, 6.0.29-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.29-cbl-mariner2.0-distroless-amd64, 6.0-cbl-mariner2.0-distroless-amd64, 6.0.29-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.29-focal-amd64, 6.0-focal-amd64, 6.0.29-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04

##### .NET 9 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.3-bookworm-slim-amd64, 9.0-preview-bookworm-slim-amd64, 9.0.0-preview.3-bookworm-slim, 9.0-preview-bookworm-slim, 9.0.0-preview.3, 9.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/bookworm-slim/amd64/Dockerfile) | Debian 12
9.0.0-preview.3-alpine3.19-amd64, 9.0-preview-alpine3.19-amd64, 9.0-preview-alpine-amd64, 9.0.0-preview.3-alpine3.19, 9.0-preview-alpine3.19, 9.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/alpine3.19/amd64/Dockerfile) | Alpine 3.19
9.0.0-preview.3-alpine3.19-composite-amd64, 9.0-preview-alpine3.19-composite-amd64, 9.0-preview-alpine-composite-amd64, 9.0.0-preview.3-alpine3.19-composite, 9.0-preview-alpine3.19-composite, 9.0-preview-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/alpine3.19-composite/amd64/Dockerfile) | Alpine 3.19
9.0.0-preview.3-jammy-amd64, 9.0-preview-jammy-amd64, 9.0.0-preview.3-jammy, 9.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-amd64, 9.0-preview-jammy-chiseled-amd64, 9.0.0-preview.3-jammy-chiseled, 9.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-extra-amd64, 9.0-preview-jammy-chiseled-extra-amd64, 9.0.0-preview.3-jammy-chiseled-extra, 9.0-preview-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-composite-amd64, 9.0-preview-jammy-chiseled-composite-amd64, 9.0.0-preview.3-jammy-chiseled-composite, 9.0-preview-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled-composite/amd64/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-composite-extra-amd64, 9.0-preview-jammy-chiseled-composite-extra-amd64, 9.0.0-preview.3-jammy-chiseled-composite-extra, 9.0-preview-jammy-chiseled-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled-composite-extra/amd64/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-cbl-mariner2.0-amd64, 9.0-preview-cbl-mariner2.0-amd64, 9.0.0-preview.3-cbl-mariner2.0, 9.0-preview-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.3-cbl-mariner2.0-distroless-amd64, 9.0-preview-cbl-mariner2.0-distroless-amd64, 9.0.0-preview.3-cbl-mariner2.0-distroless, 9.0-preview-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.3-cbl-mariner2.0-distroless-extra-amd64, 9.0-preview-cbl-mariner2.0-distroless-extra-amd64, 9.0.0-preview.3-cbl-mariner2.0-distroless-extra, 9.0-preview-cbl-mariner2.0-distroless-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0-distroless-extra/amd64/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.3-cbl-mariner2.0-distroless-composite-amd64, 9.0-preview-cbl-mariner2.0-distroless-composite-amd64, 9.0.0-preview.3-cbl-mariner2.0-distroless-composite, 9.0-preview-cbl-mariner2.0-distroless-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0-distroless-composite/amd64/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.3-cbl-mariner2.0-distroless-composite-extra-amd64, 9.0-preview-cbl-mariner2.0-distroless-composite-extra-amd64, 9.0.0-preview.3-cbl-mariner2.0-distroless-composite-extra, 9.0-preview-cbl-mariner2.0-distroless-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0-distroless-composite-extra/amd64/Dockerfile) | CBL-Mariner 2.0

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.4-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.4-bookworm-slim, 8.0-bookworm-slim, 8.0.4, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.4-alpine3.18-arm64v8, 8.0-alpine3.18-arm64v8, 8.0-alpine-arm64v8, 8.0.4-alpine3.18, 8.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
8.0.4-alpine3.18-composite-arm64v8, 8.0-alpine3.18-composite-arm64v8, 8.0-alpine-composite-arm64v8, 8.0.4-alpine3.18-composite, 8.0-alpine3.18-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18-composite/arm64v8/Dockerfile) | Alpine 3.18
8.0.4-alpine3.19-arm64v8, 8.0-alpine3.19-arm64v8, 8.0.4-alpine3.19, 8.0-alpine3.19, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
8.0.4-alpine3.19-composite-arm64v8, 8.0-alpine3.19-composite-arm64v8, 8.0.4-alpine3.19-composite, 8.0-alpine3.19-composite, 8.0-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.19-composite/arm64v8/Dockerfile) | Alpine 3.19
8.0.4-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.4-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-arm64v8, 8.0-jammy-chiseled-arm64v8, 8.0.4-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-extra-arm64v8, 8.0-jammy-chiseled-extra-arm64v8, 8.0.4-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-composite-arm64v8, 8.0-jammy-chiseled-composite-arm64v8, 8.0.4-jammy-chiseled-composite, 8.0-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-composite/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-composite-extra-arm64v8, 8.0-jammy-chiseled-composite-extra-arm64v8, 8.0.4-jammy-chiseled-composite-extra, 8.0-jammy-chiseled-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-composite-extra/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.4-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner2.0-arm64v8, 8.0.4-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.4-cbl-mariner2.0-distroless-arm64v8, 8.0-cbl-mariner2.0-distroless-arm64v8, 8.0.4-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.4-cbl-mariner2.0-distroless-extra-arm64v8, 8.0-cbl-mariner2.0-distroless-extra-arm64v8, 8.0.4-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0-distroless-extra/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.4-cbl-mariner2.0-distroless-composite-arm64v8, 8.0-cbl-mariner2.0-distroless-composite-arm64v8, 8.0.4-cbl-mariner2.0-distroless-composite, 8.0-cbl-mariner2.0-distroless-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0-distroless-composite/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.4-cbl-mariner2.0-distroless-composite-extra-arm64v8, 8.0-cbl-mariner2.0-distroless-composite-extra-arm64v8, 8.0.4-cbl-mariner2.0-distroless-composite-extra, 8.0-cbl-mariner2.0-distroless-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/cbl-mariner2.0-distroless-composite-extra/arm64v8/Dockerfile) | CBL-Mariner 2.0
7.0.18-bookworm-slim-arm64v8, 7.0-bookworm-slim-arm64v8, 7.0.18-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
7.0.18-bullseye-slim-arm64v8, 7.0-bullseye-slim-arm64v8, 7.0.18-bullseye-slim, 7.0-bullseye-slim, 7.0.18, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
7.0.18-alpine3.18-arm64v8, 7.0-alpine3.18-arm64v8, 7.0-alpine-arm64v8, 7.0.18-alpine3.18, 7.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
7.0.18-alpine3.19-arm64v8, 7.0-alpine3.19-arm64v8, 7.0.18-alpine3.19, 7.0-alpine3.19, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
7.0.18-jammy-arm64v8, 7.0-jammy-arm64v8, 7.0.18-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.18-jammy-chiseled-arm64v8, 7.0-jammy-chiseled-arm64v8, 7.0.18-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.18-jammy-chiseled-extra-arm64v8, 7.0-jammy-chiseled-extra-arm64v8, 7.0.18-jammy-chiseled-extra, 7.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
7.0.18-cbl-mariner2.0-arm64v8, 7.0-cbl-mariner2.0-arm64v8, 7.0.18-cbl-mariner2.0, 7.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
7.0.18-cbl-mariner2.0-distroless-arm64v8, 7.0-cbl-mariner2.0-distroless-arm64v8, 7.0.18-cbl-mariner2.0-distroless, 7.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.29-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.29-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.29-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.29-bullseye-slim, 6.0-bullseye-slim, 6.0.29, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.29-alpine3.18-arm64v8, 6.0-alpine3.18-arm64v8, 6.0-alpine-arm64v8, 6.0.29-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.18/arm64v8/Dockerfile) | Alpine 3.18
6.0.29-alpine3.19-arm64v8, 6.0-alpine3.19-arm64v8, 6.0.29-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
6.0.29-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.29-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.29-jammy-chiseled-arm64v8, 6.0-jammy-chiseled-arm64v8, 6.0.29-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.29-jammy-chiseled-extra-arm64v8, 6.0-jammy-chiseled-extra-arm64v8, 6.0.29-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.29-focal-arm64v8, 6.0-focal-arm64v8, 6.0.29-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04
6.0.29-cbl-mariner2.0-arm64v8, 6.0-cbl-mariner2.0-arm64v8, 6.0.29-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.29-cbl-mariner2.0-distroless-arm64v8, 6.0-cbl-mariner2.0-distroless-arm64v8, 6.0.29-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

##### .NET 9 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.3-bookworm-slim-arm64v8, 9.0-preview-bookworm-slim-arm64v8, 9.0.0-preview.3-bookworm-slim, 9.0-preview-bookworm-slim, 9.0.0-preview.3, 9.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
9.0.0-preview.3-alpine3.19-arm64v8, 9.0-preview-alpine3.19-arm64v8, 9.0-preview-alpine-arm64v8, 9.0.0-preview.3-alpine3.19, 9.0-preview-alpine3.19, 9.0-preview-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/alpine3.19/arm64v8/Dockerfile) | Alpine 3.19
9.0.0-preview.3-alpine3.19-composite-arm64v8, 9.0-preview-alpine3.19-composite-arm64v8, 9.0-preview-alpine-composite-arm64v8, 9.0.0-preview.3-alpine3.19-composite, 9.0-preview-alpine3.19-composite, 9.0-preview-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/alpine3.19-composite/arm64v8/Dockerfile) | Alpine 3.19
9.0.0-preview.3-jammy-arm64v8, 9.0-preview-jammy-arm64v8, 9.0.0-preview.3-jammy, 9.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-arm64v8, 9.0-preview-jammy-chiseled-arm64v8, 9.0.0-preview.3-jammy-chiseled, 9.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-extra-arm64v8, 9.0-preview-jammy-chiseled-extra-arm64v8, 9.0.0-preview.3-jammy-chiseled-extra, 9.0-preview-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-composite-arm64v8, 9.0-preview-jammy-chiseled-composite-arm64v8, 9.0.0-preview.3-jammy-chiseled-composite, 9.0-preview-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled-composite/arm64v8/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-composite-extra-arm64v8, 9.0-preview-jammy-chiseled-composite-extra-arm64v8, 9.0.0-preview.3-jammy-chiseled-composite-extra, 9.0-preview-jammy-chiseled-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled-composite-extra/arm64v8/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-cbl-mariner2.0-arm64v8, 9.0-preview-cbl-mariner2.0-arm64v8, 9.0.0-preview.3-cbl-mariner2.0, 9.0-preview-cbl-mariner2.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.3-cbl-mariner2.0-distroless-arm64v8, 9.0-preview-cbl-mariner2.0-distroless-arm64v8, 9.0.0-preview.3-cbl-mariner2.0-distroless, 9.0-preview-cbl-mariner2.0-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.3-cbl-mariner2.0-distroless-extra-arm64v8, 9.0-preview-cbl-mariner2.0-distroless-extra-arm64v8, 9.0.0-preview.3-cbl-mariner2.0-distroless-extra, 9.0-preview-cbl-mariner2.0-distroless-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0-distroless-extra/arm64v8/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.3-cbl-mariner2.0-distroless-composite-arm64v8, 9.0-preview-cbl-mariner2.0-distroless-composite-arm64v8, 9.0.0-preview.3-cbl-mariner2.0-distroless-composite, 9.0-preview-cbl-mariner2.0-distroless-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0-distroless-composite/arm64v8/Dockerfile) | CBL-Mariner 2.0
9.0.0-preview.3-cbl-mariner2.0-distroless-composite-extra-arm64v8, 9.0-preview-cbl-mariner2.0-distroless-composite-extra-arm64v8, 9.0.0-preview.3-cbl-mariner2.0-distroless-composite-extra, 9.0-preview-cbl-mariner2.0-distroless-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/cbl-mariner2.0-distroless-composite-extra/arm64v8/Dockerfile) | CBL-Mariner 2.0

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.4-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.4-bookworm-slim, 8.0-bookworm-slim, 8.0.4, 8.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.4-alpine3.18-arm32v7, 8.0-alpine3.18-arm32v7, 8.0-alpine-arm32v7, 8.0.4-alpine3.18, 8.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
8.0.4-alpine3.18-composite-arm32v7, 8.0-alpine3.18-composite-arm32v7, 8.0-alpine-composite-arm32v7, 8.0.4-alpine3.18-composite, 8.0-alpine3.18-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.18-composite/arm32v7/Dockerfile) | Alpine 3.18
8.0.4-alpine3.19-arm32v7, 8.0-alpine3.19-arm32v7, 8.0.4-alpine3.19, 8.0-alpine3.19, 8.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
8.0.4-alpine3.19-composite-arm32v7, 8.0-alpine3.19-composite-arm32v7, 8.0.4-alpine3.19-composite, 8.0-alpine3.19-composite, 8.0-alpine-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.19-composite/arm32v7/Dockerfile) | Alpine 3.19
8.0.4-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.4-jammy, 8.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-arm32v7, 8.0-jammy-chiseled-arm32v7, 8.0.4-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-extra-arm32v7, 8.0-jammy-chiseled-extra-arm32v7, 8.0.4-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-composite-arm32v7, 8.0-jammy-chiseled-composite-arm32v7, 8.0.4-jammy-chiseled-composite, 8.0-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-composite/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.4-jammy-chiseled-composite-extra-arm32v7, 8.0-jammy-chiseled-composite-extra-arm32v7, 8.0.4-jammy-chiseled-composite-extra, 8.0-jammy-chiseled-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/jammy-chiseled-composite-extra/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.18-bookworm-slim-arm32v7, 7.0-bookworm-slim-arm32v7, 7.0.18-bookworm-slim, 7.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
7.0.18-bullseye-slim-arm32v7, 7.0-bullseye-slim-arm32v7, 7.0.18-bullseye-slim, 7.0-bullseye-slim, 7.0.18, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
7.0.18-alpine3.18-arm32v7, 7.0-alpine3.18-arm32v7, 7.0-alpine-arm32v7, 7.0.18-alpine3.18, 7.0-alpine3.18 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
7.0.18-alpine3.19-arm32v7, 7.0-alpine3.19-arm32v7, 7.0.18-alpine3.19, 7.0-alpine3.19, 7.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
7.0.18-jammy-arm32v7, 7.0-jammy-arm32v7, 7.0.18-jammy, 7.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.18-jammy-chiseled-arm32v7, 7.0-jammy-chiseled-arm32v7, 7.0.18-jammy-chiseled, 7.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
7.0.18-jammy-chiseled-extra-arm32v7, 7.0-jammy-chiseled-extra-arm32v7, 7.0.18-jammy-chiseled-extra, 7.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.29-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.29-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.29-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.29-bullseye-slim, 6.0-bullseye-slim, 6.0.29, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.29-alpine3.18-arm32v7, 6.0-alpine3.18-arm32v7, 6.0-alpine-arm32v7, 6.0.29-alpine3.18, 6.0-alpine3.18, 6.0-alpine | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.18/arm32v7/Dockerfile) | Alpine 3.18
6.0.29-alpine3.19-arm32v7, 6.0-alpine3.19-arm32v7, 6.0.29-alpine3.19, 6.0-alpine3.19 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/alpine3.19/arm32v7/Dockerfile) | Alpine 3.19
6.0.29-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.29-jammy, 6.0-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.29-jammy-chiseled-arm32v7, 6.0-jammy-chiseled-arm32v7, 6.0.29-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.29-jammy-chiseled-extra-arm32v7, 6.0-jammy-chiseled-extra-arm32v7, 6.0.29-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.29-focal-arm32v7, 6.0-focal-arm32v7, 6.0.29-focal, 6.0-focal | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

##### .NET 9 Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.3-bookworm-slim-arm32v7, 9.0-preview-bookworm-slim-arm32v7, 9.0.0-preview.3-bookworm-slim, 9.0-preview-bookworm-slim, 9.0.0-preview.3, 9.0-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
9.0.0-preview.3-jammy-arm32v7, 9.0-preview-jammy-arm32v7, 9.0.0-preview.3-jammy, 9.0-preview-jammy | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-arm32v7, 9.0-preview-jammy-chiseled-arm32v7, 9.0.0-preview.3-jammy-chiseled, 9.0-preview-jammy-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-extra-arm32v7, 9.0-preview-jammy-chiseled-extra-arm32v7, 9.0.0-preview.3-jammy-chiseled-extra, 9.0-preview-jammy-chiseled-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-composite-arm32v7, 9.0-preview-jammy-chiseled-composite-arm32v7, 9.0.0-preview.3-jammy-chiseled-composite, 9.0-preview-jammy-chiseled-composite | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled-composite/arm32v7/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-jammy-chiseled-composite-extra-arm32v7, 9.0-preview-jammy-chiseled-composite-extra-arm32v7, 9.0.0-preview.3-jammy-chiseled-composite-extra, 9.0-preview-jammy-chiseled-composite-extra | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/jammy-chiseled-composite-extra/arm32v7/Dockerfile) | Ubuntu 22.04

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.4-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/nanoserver-ltsc2022/amd64/Dockerfile)
7.0.18-nanoserver-ltsc2022, 7.0-nanoserver-ltsc2022, 7.0.18, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.29-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.29, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.3-nanoserver-ltsc2022, 9.0-preview-nanoserver-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/nanoserver-ltsc2022/amd64/Dockerfile)

## Windows Server Core 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.4-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)
7.0.18-windowsservercore-ltsc2022, 7.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.29-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.3-windowsservercore-ltsc2022, 9.0-preview-windowsservercore-ltsc2022 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/windowsservercore-ltsc2022/amd64/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.4-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/nanoserver-1809/amd64/Dockerfile)
7.0.18-nanoserver-1809, 7.0-nanoserver-1809, 7.0.18, 7.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/nanoserver-1809/amd64/Dockerfile)
6.0.29-nanoserver-1809, 6.0-nanoserver-1809, 6.0.29, 6.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/nanoserver-1809/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.3-nanoserver-1809, 9.0-preview-nanoserver-1809 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/nanoserver-1809/amd64/Dockerfile)

## Windows Server Core 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
8.0.4-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
7.0.18-windowsservercore-ltsc2019, 7.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/7.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.29-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)

##### .NET 9 Preview Tags
Tag | Dockerfile
---------| ---------------
9.0.0-preview.3-windowsservercore-ltsc2019, 9.0-preview-windowsservercore-ltsc2019 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/9.0/windowsservercore-ltsc2019/amd64/Dockerfile)

You can retrieve a list of all available tags for dotnet/aspnet at https://mcr.microsoft.com/v2/dotnet/aspnet/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core/aspnet repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core/aspnet/tags/list.

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)*

# Support

## Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

## Image Update Policy

* We update supported .NET images within 12 hours of any updates to their base images (e.g. debian:bookworm-slim, windows/nanoserver:ltsc2022, etc.).
* We re-build all .NET images as part of releasing new versions of .NET including new major/minor versions and servicing.
* Distroless images such as Ubuntu Chiseled have no base image, and as such will only be updated with .NET releases and CVE fixes as described below.

### CVE Update Policy

.NET container images are regularly monitored for the presence of CVEs. A given image will be rebuilt to pick up fixes for a CVE when:
* We detect the image contains a CVE with a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score of "Critical"
* **AND** the CVE is in a package that is added in our Dockerfile layers (meaning the CVE is in a package we explicitly install or any transitive dependencies of those packages)
* **AND** there is a CVE fix for the package available in the affected base image's package repository.

## Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server](https://www.microsoft.com/cloud-platform/windows-server-pricing)
