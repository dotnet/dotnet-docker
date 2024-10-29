# ASP.NET Core Runtime

> **Important**: The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).
>
> See [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.aspnet.md) for images with official releases of [.NET](https://github.com/dotnet/core).

## Featured Tags

* `9.0` (Release Candidate)
  * `docker pull mcr.microsoft.com/dotnet/nightly/aspnet:9.0`
* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/aspnet:8.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/aspnet:6.0`

## About

This image contains the ASP.NET Core and .NET runtimes and libraries and is optimized for running ASP.NET Core apps in production.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

### New: Ubuntu Chiseled Images

Ubuntu Chiseled .NET images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface by including only the minimal set of packages required to run .NET applications.

Please see the [Ubuntu Chiseled + .NET](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md) documentation page for more info.

### ASP.NET Core Composite Images

Starting from .NET 8, ASP.NET Core Composite images are optimized for performance using [ReadyToRun (R2R) compilation](https://learn.microsoft.com/dotnet/core/deploying/ready-to-run).
For more information, see the [composite images section in the Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md#composite-net-80).

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Container sample: Run a web application

You can quickly run a container with a pre-built [.NET Docker image](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md), based on the [ASP.NET Core sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md).

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:8080 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. You can also view the ASP.NET Core site running in the container from another machine with a local IP address such as `http://192.168.1.18:8000`.

> Note: ASP.NET Core apps (in official images) listen to [port 8080 by default](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7), starting with .NET 8. The [`-p` argument](https://docs.docker.com/engine/reference/commandline/run/#publish) in these examples maps host port `8000` to container port `8080` (`host:container` mapping). The container will not be accessible without this mapping. ASP.NET Core can be [configured to listen on a different or additional port](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel/endpoints).

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md) to use HTTPS with this image.

## Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

## Related Repositories

.NET:

* [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.md): .NET
* [dotnet/aspnet](https://github.com/dotnet/dotnet-docker/blob/main/README.aspnet.md): ASP.NET Core Runtime
* [dotnet/nightly/sdk](https://github.com/dotnet/dotnet-docker/blob/nightly/README.sdk.md): .NET SDK (Preview)
* [dotnet/nightly/runtime](https://github.com/dotnet/dotnet-docker/blob/nightly/README.runtime.md): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://github.com/dotnet/dotnet-docker/blob/nightly/README.runtime-deps.md): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor](https://github.com/dotnet/dotnet-docker/blob/nightly/README.monitor.md): .NET Monitor Tool (Preview)
* [dotnet/nightly/aspire-dashboard](https://github.com/dotnet/dotnet-docker/blob/nightly/README.aspire-dashboard.md): .NET Aspire Dashboard (Preview)
* [dotnet/samples](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md): .NET Samples

.NET Framework:

* [dotnet/framework](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.md): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.samples.md): .NET Framework, ASP.NET and WCF Samples

## Full Tag Listing

### Linux amd64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-bookworm-slim-amd64, 9.0-bookworm-slim-amd64, 9.0.0-bookworm-slim, 9.0-bookworm-slim, 9.0.0, 9.0, latest | [Dockerfile](src/aspnet/9.0/bookworm-slim/amd64/Dockerfile) | Debian 12
9.0.0-alpine3.20-amd64, 9.0-alpine3.20-amd64, 9.0-alpine-amd64, 9.0.0-alpine3.20, 9.0-alpine3.20, 9.0-alpine | [Dockerfile](src/aspnet/9.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
9.0.0-alpine3.20-composite-amd64, 9.0-alpine3.20-composite-amd64, 9.0-alpine-composite-amd64, 9.0.0-alpine3.20-composite, 9.0-alpine3.20-composite, 9.0-alpine-composite | [Dockerfile](src/aspnet/9.0/alpine3.20-composite/amd64/Dockerfile) | Alpine 3.20
9.0.0-noble-amd64, 9.0-noble-amd64, 9.0.0-noble, 9.0-noble | [Dockerfile](src/aspnet/9.0/noble/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-amd64, 9.0-noble-chiseled-amd64, 9.0.0-noble-chiseled, 9.0-noble-chiseled | [Dockerfile](src/aspnet/9.0/noble-chiseled/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-extra-amd64, 9.0-noble-chiseled-extra-amd64, 9.0.0-noble-chiseled-extra, 9.0-noble-chiseled-extra | [Dockerfile](src/aspnet/9.0/noble-chiseled-extra/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-composite-amd64, 9.0-noble-chiseled-composite-amd64, 9.0.0-noble-chiseled-composite, 9.0-noble-chiseled-composite | [Dockerfile](src/aspnet/9.0/noble-chiseled-composite/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-composite-extra-amd64, 9.0-noble-chiseled-composite-extra-amd64, 9.0.0-noble-chiseled-composite-extra, 9.0-noble-chiseled-composite-extra | [Dockerfile](src/aspnet/9.0/noble-chiseled-composite-extra/amd64/Dockerfile) | Ubuntu 24.04
9.0.0-azurelinux3.0-amd64, 9.0-azurelinux3.0-amd64, 9.0.0-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/aspnet/9.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
9.0.0-azurelinux3.0-distroless-amd64, 9.0-azurelinux3.0-distroless-amd64, 9.0.0-azurelinux3.0-distroless, 9.0-azurelinux3.0-distroless | [Dockerfile](src/aspnet/9.0/azurelinux3.0-distroless/amd64/Dockerfile) | Azure Linux 3.0
9.0.0-azurelinux3.0-distroless-extra-amd64, 9.0-azurelinux3.0-distroless-extra-amd64, 9.0.0-azurelinux3.0-distroless-extra, 9.0-azurelinux3.0-distroless-extra | [Dockerfile](src/aspnet/9.0/azurelinux3.0-distroless-extra/amd64/Dockerfile) | Azure Linux 3.0
9.0.0-azurelinux3.0-distroless-composite-amd64, 9.0-azurelinux3.0-distroless-composite-amd64, 9.0.0-azurelinux3.0-distroless-composite, 9.0-azurelinux3.0-distroless-composite | [Dockerfile](src/aspnet/9.0/azurelinux3.0-distroless-composite/amd64/Dockerfile) | Azure Linux 3.0
9.0.0-azurelinux3.0-distroless-composite-extra-amd64, 9.0-azurelinux3.0-distroless-composite-extra-amd64, 9.0.0-azurelinux3.0-distroless-composite-extra, 9.0-azurelinux3.0-distroless-composite-extra | [Dockerfile](src/aspnet/9.0/azurelinux3.0-distroless-composite-extra/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.10-bookworm-slim, 8.0-bookworm-slim, 8.0.10, 8.0 | [Dockerfile](src/aspnet/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.10-alpine3.20-amd64, 8.0-alpine3.20-amd64, 8.0-alpine-amd64, 8.0.10-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](src/aspnet/8.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
8.0.10-alpine3.20-composite-amd64, 8.0-alpine3.20-composite-amd64, 8.0-alpine-composite-amd64, 8.0.10-alpine3.20-composite, 8.0-alpine3.20-composite, 8.0-alpine-composite | [Dockerfile](src/aspnet/8.0/alpine3.20-composite/amd64/Dockerfile) | Alpine 3.20
8.0.10-noble-amd64, 8.0-noble-amd64, 8.0.10-noble, 8.0-noble | [Dockerfile](src/aspnet/8.0/noble/amd64/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-amd64, 8.0-noble-chiseled-amd64, 8.0.10-noble-chiseled, 8.0-noble-chiseled | [Dockerfile](src/aspnet/8.0/noble-chiseled/amd64/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-extra-amd64, 8.0-noble-chiseled-extra-amd64, 8.0.10-noble-chiseled-extra, 8.0-noble-chiseled-extra | [Dockerfile](src/aspnet/8.0/noble-chiseled-extra/amd64/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-composite-amd64, 8.0-noble-chiseled-composite-amd64, 8.0.10-noble-chiseled-composite, 8.0-noble-chiseled-composite | [Dockerfile](src/aspnet/8.0/noble-chiseled-composite/amd64/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-composite-extra-amd64, 8.0-noble-chiseled-composite-extra-amd64, 8.0.10-noble-chiseled-composite-extra, 8.0-noble-chiseled-composite-extra | [Dockerfile](src/aspnet/8.0/noble-chiseled-composite-extra/amd64/Dockerfile) | Ubuntu 24.04
8.0.10-jammy-amd64, 8.0-jammy-amd64, 8.0.10-jammy, 8.0-jammy | [Dockerfile](src/aspnet/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-amd64, 8.0-jammy-chiseled-amd64, 8.0.10-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](src/aspnet/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-extra-amd64, 8.0-jammy-chiseled-extra-amd64, 8.0.10-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](src/aspnet/8.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-composite-amd64, 8.0-jammy-chiseled-composite-amd64, 8.0.10-jammy-chiseled-composite, 8.0-jammy-chiseled-composite | [Dockerfile](src/aspnet/8.0/jammy-chiseled-composite/amd64/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-composite-extra-amd64, 8.0-jammy-chiseled-composite-extra-amd64, 8.0.10-jammy-chiseled-composite-extra, 8.0-jammy-chiseled-composite-extra | [Dockerfile](src/aspnet/8.0/jammy-chiseled-composite-extra/amd64/Dockerfile) | Ubuntu 22.04
8.0.10-azurelinux3.0-amd64, 8.0-azurelinux3.0-amd64, 8.0.10-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/aspnet/8.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-amd64, 8.0-azurelinux3.0-distroless-amd64, 8.0.10-azurelinux3.0-distroless, 8.0-azurelinux3.0-distroless | [Dockerfile](src/aspnet/8.0/azurelinux3.0-distroless/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-extra-amd64, 8.0-azurelinux3.0-distroless-extra-amd64, 8.0.10-azurelinux3.0-distroless-extra, 8.0-azurelinux3.0-distroless-extra | [Dockerfile](src/aspnet/8.0/azurelinux3.0-distroless-extra/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-composite-amd64, 8.0-azurelinux3.0-distroless-composite-amd64, 8.0.10-azurelinux3.0-distroless-composite, 8.0-azurelinux3.0-distroless-composite | [Dockerfile](src/aspnet/8.0/azurelinux3.0-distroless-composite/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-composite-extra-amd64, 8.0-azurelinux3.0-distroless-composite-extra-amd64, 8.0.10-azurelinux3.0-distroless-composite-extra, 8.0-azurelinux3.0-distroless-composite-extra | [Dockerfile](src/aspnet/8.0/azurelinux3.0-distroless-composite-extra/amd64/Dockerfile) | Azure Linux 3.0
8.0.10-cbl-mariner2.0-amd64, 8.0-cbl-mariner2.0-amd64, 8.0.10-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-amd64, 8.0-cbl-mariner2.0-distroless-amd64, 8.0.10-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-extra-amd64, 8.0-cbl-mariner2.0-distroless-extra-amd64, 8.0.10-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0-distroless-extra/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-composite-amd64, 8.0-cbl-mariner2.0-distroless-composite-amd64, 8.0.10-cbl-mariner2.0-distroless-composite, 8.0-cbl-mariner2.0-distroless-composite | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0-distroless-composite/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-composite-extra-amd64, 8.0-cbl-mariner2.0-distroless-composite-extra-amd64, 8.0.10-cbl-mariner2.0-distroless-composite-extra, 8.0-cbl-mariner2.0-distroless-composite-extra | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0-distroless-composite-extra/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.35-bookworm-slim-amd64, 6.0-bookworm-slim-amd64, 6.0.35-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](src/aspnet/6.0/bookworm-slim/amd64/Dockerfile) | Debian 12
6.0.35-bullseye-slim-amd64, 6.0-bullseye-slim-amd64, 6.0.35-bullseye-slim, 6.0-bullseye-slim, 6.0.35, 6.0 | [Dockerfile](src/aspnet/6.0/bullseye-slim/amd64/Dockerfile) | Debian 11
6.0.35-alpine3.20-amd64, 6.0-alpine3.20-amd64, 6.0-alpine-amd64, 6.0.35-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](src/aspnet/6.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
6.0.35-jammy-amd64, 6.0-jammy-amd64, 6.0.35-jammy, 6.0-jammy | [Dockerfile](src/aspnet/6.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-amd64, 6.0-jammy-chiseled-amd64, 6.0.35-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](src/aspnet/6.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-extra-amd64, 6.0-jammy-chiseled-extra-amd64, 6.0.35-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](src/aspnet/6.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
6.0.35-focal-amd64, 6.0-focal-amd64, 6.0.35-focal, 6.0-focal | [Dockerfile](src/aspnet/6.0/focal/amd64/Dockerfile) | Ubuntu 20.04
6.0.35-cbl-mariner2.0-amd64, 6.0-cbl-mariner2.0-amd64, 6.0.35-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](src/aspnet/6.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
6.0.35-cbl-mariner2.0-distroless-amd64, 6.0-cbl-mariner2.0-distroless-amd64, 6.0.35-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless | [Dockerfile](src/aspnet/6.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

### Linux arm64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-bookworm-slim-arm64v8, 9.0-bookworm-slim-arm64v8, 9.0.0-bookworm-slim, 9.0-bookworm-slim, 9.0.0, 9.0, latest | [Dockerfile](src/aspnet/9.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
9.0.0-alpine3.20-arm64v8, 9.0-alpine3.20-arm64v8, 9.0-alpine-arm64v8, 9.0.0-alpine3.20, 9.0-alpine3.20, 9.0-alpine | [Dockerfile](src/aspnet/9.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
9.0.0-alpine3.20-composite-arm64v8, 9.0-alpine3.20-composite-arm64v8, 9.0-alpine-composite-arm64v8, 9.0.0-alpine3.20-composite, 9.0-alpine3.20-composite, 9.0-alpine-composite | [Dockerfile](src/aspnet/9.0/alpine3.20-composite/arm64v8/Dockerfile) | Alpine 3.20
9.0.0-noble-arm64v8, 9.0-noble-arm64v8, 9.0.0-noble, 9.0-noble | [Dockerfile](src/aspnet/9.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-arm64v8, 9.0-noble-chiseled-arm64v8, 9.0.0-noble-chiseled, 9.0-noble-chiseled | [Dockerfile](src/aspnet/9.0/noble-chiseled/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-extra-arm64v8, 9.0-noble-chiseled-extra-arm64v8, 9.0.0-noble-chiseled-extra, 9.0-noble-chiseled-extra | [Dockerfile](src/aspnet/9.0/noble-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-composite-arm64v8, 9.0-noble-chiseled-composite-arm64v8, 9.0.0-noble-chiseled-composite, 9.0-noble-chiseled-composite | [Dockerfile](src/aspnet/9.0/noble-chiseled-composite/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-composite-extra-arm64v8, 9.0-noble-chiseled-composite-extra-arm64v8, 9.0.0-noble-chiseled-composite-extra, 9.0-noble-chiseled-composite-extra | [Dockerfile](src/aspnet/9.0/noble-chiseled-composite-extra/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.0-azurelinux3.0-arm64v8, 9.0-azurelinux3.0-arm64v8, 9.0.0-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/aspnet/9.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.0-azurelinux3.0-distroless-arm64v8, 9.0-azurelinux3.0-distroless-arm64v8, 9.0.0-azurelinux3.0-distroless, 9.0-azurelinux3.0-distroless | [Dockerfile](src/aspnet/9.0/azurelinux3.0-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.0-azurelinux3.0-distroless-extra-arm64v8, 9.0-azurelinux3.0-distroless-extra-arm64v8, 9.0.0-azurelinux3.0-distroless-extra, 9.0-azurelinux3.0-distroless-extra | [Dockerfile](src/aspnet/9.0/azurelinux3.0-distroless-extra/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.0-azurelinux3.0-distroless-composite-arm64v8, 9.0-azurelinux3.0-distroless-composite-arm64v8, 9.0.0-azurelinux3.0-distroless-composite, 9.0-azurelinux3.0-distroless-composite | [Dockerfile](src/aspnet/9.0/azurelinux3.0-distroless-composite/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.0-azurelinux3.0-distroless-composite-extra-arm64v8, 9.0-azurelinux3.0-distroless-composite-extra-arm64v8, 9.0.0-azurelinux3.0-distroless-composite-extra, 9.0-azurelinux3.0-distroless-composite-extra | [Dockerfile](src/aspnet/9.0/azurelinux3.0-distroless-composite-extra/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.10-bookworm-slim, 8.0-bookworm-slim, 8.0.10, 8.0 | [Dockerfile](src/aspnet/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.10-alpine3.20-arm64v8, 8.0-alpine3.20-arm64v8, 8.0-alpine-arm64v8, 8.0.10-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](src/aspnet/8.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
8.0.10-alpine3.20-composite-arm64v8, 8.0-alpine3.20-composite-arm64v8, 8.0-alpine-composite-arm64v8, 8.0.10-alpine3.20-composite, 8.0-alpine3.20-composite, 8.0-alpine-composite | [Dockerfile](src/aspnet/8.0/alpine3.20-composite/arm64v8/Dockerfile) | Alpine 3.20
8.0.10-noble-arm64v8, 8.0-noble-arm64v8, 8.0.10-noble, 8.0-noble | [Dockerfile](src/aspnet/8.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-arm64v8, 8.0-noble-chiseled-arm64v8, 8.0.10-noble-chiseled, 8.0-noble-chiseled | [Dockerfile](src/aspnet/8.0/noble-chiseled/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-extra-arm64v8, 8.0-noble-chiseled-extra-arm64v8, 8.0.10-noble-chiseled-extra, 8.0-noble-chiseled-extra | [Dockerfile](src/aspnet/8.0/noble-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-composite-arm64v8, 8.0-noble-chiseled-composite-arm64v8, 8.0.10-noble-chiseled-composite, 8.0-noble-chiseled-composite | [Dockerfile](src/aspnet/8.0/noble-chiseled-composite/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.10-noble-chiseled-composite-extra-arm64v8, 8.0-noble-chiseled-composite-extra-arm64v8, 8.0.10-noble-chiseled-composite-extra, 8.0-noble-chiseled-composite-extra | [Dockerfile](src/aspnet/8.0/noble-chiseled-composite-extra/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.10-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.10-jammy, 8.0-jammy | [Dockerfile](src/aspnet/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-arm64v8, 8.0-jammy-chiseled-arm64v8, 8.0.10-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](src/aspnet/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-extra-arm64v8, 8.0-jammy-chiseled-extra-arm64v8, 8.0.10-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](src/aspnet/8.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-composite-arm64v8, 8.0-jammy-chiseled-composite-arm64v8, 8.0.10-jammy-chiseled-composite, 8.0-jammy-chiseled-composite | [Dockerfile](src/aspnet/8.0/jammy-chiseled-composite/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-composite-extra-arm64v8, 8.0-jammy-chiseled-composite-extra-arm64v8, 8.0.10-jammy-chiseled-composite-extra, 8.0-jammy-chiseled-composite-extra | [Dockerfile](src/aspnet/8.0/jammy-chiseled-composite-extra/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.10-azurelinux3.0-arm64v8, 8.0-azurelinux3.0-arm64v8, 8.0.10-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/aspnet/8.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-arm64v8, 8.0-azurelinux3.0-distroless-arm64v8, 8.0.10-azurelinux3.0-distroless, 8.0-azurelinux3.0-distroless | [Dockerfile](src/aspnet/8.0/azurelinux3.0-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-extra-arm64v8, 8.0-azurelinux3.0-distroless-extra-arm64v8, 8.0.10-azurelinux3.0-distroless-extra, 8.0-azurelinux3.0-distroless-extra | [Dockerfile](src/aspnet/8.0/azurelinux3.0-distroless-extra/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-composite-arm64v8, 8.0-azurelinux3.0-distroless-composite-arm64v8, 8.0.10-azurelinux3.0-distroless-composite, 8.0-azurelinux3.0-distroless-composite | [Dockerfile](src/aspnet/8.0/azurelinux3.0-distroless-composite/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-azurelinux3.0-distroless-composite-extra-arm64v8, 8.0-azurelinux3.0-distroless-composite-extra-arm64v8, 8.0.10-azurelinux3.0-distroless-composite-extra, 8.0-azurelinux3.0-distroless-composite-extra | [Dockerfile](src/aspnet/8.0/azurelinux3.0-distroless-composite-extra/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.10-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner2.0-arm64v8, 8.0.10-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-arm64v8, 8.0-cbl-mariner2.0-distroless-arm64v8, 8.0.10-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-extra-arm64v8, 8.0-cbl-mariner2.0-distroless-extra-arm64v8, 8.0.10-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0-distroless-extra/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-composite-arm64v8, 8.0-cbl-mariner2.0-distroless-composite-arm64v8, 8.0.10-cbl-mariner2.0-distroless-composite, 8.0-cbl-mariner2.0-distroless-composite | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0-distroless-composite/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.10-cbl-mariner2.0-distroless-composite-extra-arm64v8, 8.0-cbl-mariner2.0-distroless-composite-extra-arm64v8, 8.0.10-cbl-mariner2.0-distroless-composite-extra, 8.0-cbl-mariner2.0-distroless-composite-extra | [Dockerfile](src/aspnet/8.0/cbl-mariner2.0-distroless-composite-extra/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.35-bookworm-slim-arm64v8, 6.0-bookworm-slim-arm64v8, 6.0.35-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](src/aspnet/6.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
6.0.35-bullseye-slim-arm64v8, 6.0-bullseye-slim-arm64v8, 6.0.35-bullseye-slim, 6.0-bullseye-slim, 6.0.35, 6.0 | [Dockerfile](src/aspnet/6.0/bullseye-slim/arm64v8/Dockerfile) | Debian 11
6.0.35-alpine3.20-arm64v8, 6.0-alpine3.20-arm64v8, 6.0-alpine-arm64v8, 6.0.35-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](src/aspnet/6.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
6.0.35-jammy-arm64v8, 6.0-jammy-arm64v8, 6.0.35-jammy, 6.0-jammy | [Dockerfile](src/aspnet/6.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-arm64v8, 6.0-jammy-chiseled-arm64v8, 6.0.35-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](src/aspnet/6.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-extra-arm64v8, 6.0-jammy-chiseled-extra-arm64v8, 6.0.35-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](src/aspnet/6.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
6.0.35-focal-arm64v8, 6.0-focal-arm64v8, 6.0.35-focal, 6.0-focal | [Dockerfile](src/aspnet/6.0/focal/arm64v8/Dockerfile) | Ubuntu 20.04
6.0.35-cbl-mariner2.0-arm64v8, 6.0-cbl-mariner2.0-arm64v8, 6.0.35-cbl-mariner2.0, 6.0-cbl-mariner2.0 | [Dockerfile](src/aspnet/6.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.0.35-cbl-mariner2.0-distroless-arm64v8, 6.0-cbl-mariner2.0-distroless-arm64v8, 6.0.35-cbl-mariner2.0-distroless, 6.0-cbl-mariner2.0-distroless | [Dockerfile](src/aspnet/6.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

### Linux arm32 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-bookworm-slim-arm32v7, 9.0-bookworm-slim-arm32v7, 9.0.0-bookworm-slim, 9.0-bookworm-slim, 9.0.0, 9.0, latest | [Dockerfile](src/aspnet/9.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
9.0.0-alpine3.20-arm32v7, 9.0-alpine3.20-arm32v7, 9.0-alpine-arm32v7, 9.0.0-alpine3.20, 9.0-alpine3.20, 9.0-alpine | [Dockerfile](src/aspnet/9.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
9.0.0-alpine3.20-composite-arm32v7, 9.0-alpine3.20-composite-arm32v7, 9.0-alpine-composite-arm32v7, 9.0.0-alpine3.20-composite, 9.0-alpine3.20-composite, 9.0-alpine-composite | [Dockerfile](src/aspnet/9.0/alpine3.20-composite/arm32v7/Dockerfile) | Alpine 3.20
9.0.0-noble-arm32v7, 9.0-noble-arm32v7, 9.0.0-noble, 9.0-noble | [Dockerfile](src/aspnet/9.0/noble/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-arm32v7, 9.0-noble-chiseled-arm32v7, 9.0.0-noble-chiseled, 9.0-noble-chiseled | [Dockerfile](src/aspnet/9.0/noble-chiseled/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-extra-arm32v7, 9.0-noble-chiseled-extra-arm32v7, 9.0.0-noble-chiseled-extra, 9.0-noble-chiseled-extra | [Dockerfile](src/aspnet/9.0/noble-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-composite-arm32v7, 9.0-noble-chiseled-composite-arm32v7, 9.0.0-noble-chiseled-composite, 9.0-noble-chiseled-composite | [Dockerfile](src/aspnet/9.0/noble-chiseled-composite/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.0-noble-chiseled-composite-extra-arm32v7, 9.0-noble-chiseled-composite-extra-arm32v7, 9.0.0-noble-chiseled-composite-extra, 9.0-noble-chiseled-composite-extra | [Dockerfile](src/aspnet/9.0/noble-chiseled-composite-extra/arm32v7/Dockerfile) | Ubuntu 24.04
8.0.10-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.10-bookworm-slim, 8.0-bookworm-slim, 8.0.10, 8.0 | [Dockerfile](src/aspnet/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.10-alpine3.20-arm32v7, 8.0-alpine3.20-arm32v7, 8.0-alpine-arm32v7, 8.0.10-alpine3.20, 8.0-alpine3.20, 8.0-alpine | [Dockerfile](src/aspnet/8.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
8.0.10-alpine3.20-composite-arm32v7, 8.0-alpine3.20-composite-arm32v7, 8.0-alpine-composite-arm32v7, 8.0.10-alpine3.20-composite, 8.0-alpine3.20-composite, 8.0-alpine-composite | [Dockerfile](src/aspnet/8.0/alpine3.20-composite/arm32v7/Dockerfile) | Alpine 3.20
8.0.10-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.10-jammy, 8.0-jammy | [Dockerfile](src/aspnet/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-arm32v7, 8.0-jammy-chiseled-arm32v7, 8.0.10-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](src/aspnet/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-extra-arm32v7, 8.0-jammy-chiseled-extra-arm32v7, 8.0.10-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](src/aspnet/8.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-composite-arm32v7, 8.0-jammy-chiseled-composite-arm32v7, 8.0.10-jammy-chiseled-composite, 8.0-jammy-chiseled-composite | [Dockerfile](src/aspnet/8.0/jammy-chiseled-composite/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.10-jammy-chiseled-composite-extra-arm32v7, 8.0-jammy-chiseled-composite-extra-arm32v7, 8.0.10-jammy-chiseled-composite-extra, 8.0-jammy-chiseled-composite-extra | [Dockerfile](src/aspnet/8.0/jammy-chiseled-composite-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.35-bookworm-slim-arm32v7, 6.0-bookworm-slim-arm32v7, 6.0.35-bookworm-slim, 6.0-bookworm-slim | [Dockerfile](src/aspnet/6.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
6.0.35-bullseye-slim-arm32v7, 6.0-bullseye-slim-arm32v7, 6.0.35-bullseye-slim, 6.0-bullseye-slim, 6.0.35, 6.0 | [Dockerfile](src/aspnet/6.0/bullseye-slim/arm32v7/Dockerfile) | Debian 11
6.0.35-alpine3.20-arm32v7, 6.0-alpine3.20-arm32v7, 6.0-alpine-arm32v7, 6.0.35-alpine3.20, 6.0-alpine3.20, 6.0-alpine | [Dockerfile](src/aspnet/6.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
6.0.35-jammy-arm32v7, 6.0-jammy-arm32v7, 6.0.35-jammy, 6.0-jammy | [Dockerfile](src/aspnet/6.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-arm32v7, 6.0-jammy-chiseled-arm32v7, 6.0.35-jammy-chiseled, 6.0-jammy-chiseled | [Dockerfile](src/aspnet/6.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.35-jammy-chiseled-extra-arm32v7, 6.0-jammy-chiseled-extra-arm32v7, 6.0.35-jammy-chiseled-extra, 6.0-jammy-chiseled-extra | [Dockerfile](src/aspnet/6.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04
6.0.35-focal-arm32v7, 6.0-focal-arm32v7, 6.0.35-focal, 6.0-focal | [Dockerfile](src/aspnet/6.0/focal/arm32v7/Dockerfile) | Ubuntu 20.04

### Nano Server 2025 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-nanoserver-ltsc2025, 9.0-nanoserver-ltsc2025 | [Dockerfile](src/aspnet/9.0/nanoserver-ltsc2025/amd64/Dockerfile)
8.0.10-nanoserver-ltsc2025, 8.0-nanoserver-ltsc2025 | [Dockerfile](src/aspnet/8.0/nanoserver-ltsc2025/amd64/Dockerfile)

### Windows Server Core 2025 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-windowsservercore-ltsc2025, 9.0-windowsservercore-ltsc2025 | [Dockerfile](src/aspnet/9.0/windowsservercore-ltsc2025/amd64/Dockerfile)
8.0.10-windowsservercore-ltsc2025, 8.0-windowsservercore-ltsc2025 | [Dockerfile](src/aspnet/8.0/windowsservercore-ltsc2025/amd64/Dockerfile)

### Nano Server 2022 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-nanoserver-ltsc2022, 9.0-nanoserver-ltsc2022 | [Dockerfile](src/aspnet/9.0/nanoserver-ltsc2022/amd64/Dockerfile)
8.0.10-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](src/aspnet/8.0/nanoserver-ltsc2022/amd64/Dockerfile)
6.0.35-nanoserver-ltsc2022, 6.0-nanoserver-ltsc2022, 6.0.35, 6.0 | [Dockerfile](src/aspnet/6.0/nanoserver-ltsc2022/amd64/Dockerfile)

### Windows Server Core 2022 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-windowsservercore-ltsc2022, 9.0-windowsservercore-ltsc2022 | [Dockerfile](src/aspnet/9.0/windowsservercore-ltsc2022/amd64/Dockerfile)
8.0.10-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](src/aspnet/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)
6.0.35-windowsservercore-ltsc2022, 6.0-windowsservercore-ltsc2022 | [Dockerfile](src/aspnet/6.0/windowsservercore-ltsc2022/amd64/Dockerfile)

### Nano Server, version 1809 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-nanoserver-1809, 9.0-nanoserver-1809 | [Dockerfile](src/aspnet/9.0/nanoserver-1809/amd64/Dockerfile)
8.0.10-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](src/aspnet/8.0/nanoserver-1809/amd64/Dockerfile)
6.0.35-nanoserver-1809, 6.0-nanoserver-1809, 6.0.35, 6.0 | [Dockerfile](src/aspnet/6.0/nanoserver-1809/amd64/Dockerfile)

### Windows Server Core 2019 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.0-windowsservercore-ltsc2019, 9.0-windowsservercore-ltsc2019 | [Dockerfile](src/aspnet/9.0/windowsservercore-ltsc2019/amd64/Dockerfile)
8.0.10-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](src/aspnet/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
6.0.35-windowsservercore-ltsc2019, 6.0-windowsservercore-ltsc2019 | [Dockerfile](src/aspnet/6.0/windowsservercore-ltsc2019/amd64/Dockerfile)
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/dotnet/nightly/aspnet/tags/list) for all supported and unsupported tags.*

## Support

### Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

### Image Update Policy

* **Base Image Updates:** Images are re-built within 12 hours of any updates to their base images (e.g. debian:bookworm-slim, windows/nanoserver:ltsc2022, etc.).
* **.NET Releases:** Images are re-built as part of releasing new .NET versions. This includes new major versions, minor versions, and servicing releases.
* **Critical CVEs:** Images are re-built to pick up critical CVE fixes as described by the CVE Update Policy below.
* **Monthly Re-builds:** Images are re-built monthly, typically on the second Tuesday of the month, in order to pick up lower-severity CVE fixes.
* **Out-Of-Band Updates:** Images can sometimes be re-built when out-of-band updates are necessary to address critical issues. If this happens, new fixed version tags will be updated according to the [Fixed version tags documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md#fixed-version-tags).

#### CVE Update Policy

.NET container images are regularly monitored for the presence of CVEs. A given image will be rebuilt to pick up fixes for a CVE when:

* We detect the image contains a CVE with a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score of "Critical"
* **AND** the CVE is in a package that is added in our Dockerfile layers (meaning the CVE is in a package we explicitly install or any transitive dependencies of those packages)
* **AND** there is a CVE fix for the package available in the affected base image's package repository.

Please refer to the [Security Policy](https://github.com/dotnet/dotnet-docker/blob/main/SECURITY.md) and [Container Vulnerability Workflow](https://github.com/dotnet/dotnet-docker/blob/main/documentation/vulnerability-reporting.md) for more detail about what to do when a CVE is encountered in a .NET image.

### Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

## License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server](https://www.microsoft.com/cloud-platform/windows-server-pricing)
