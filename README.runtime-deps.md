# .NET Runtime Dependencies

> **Important**: The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).
>
> See [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime-deps.md) for images with official releases of [.NET](https://github.com/dotnet/core).

## Featured Tags

* `10.0-preview` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime-deps:10.0-preview`
* `9.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime-deps:9.0`
* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/runtime-deps:8.0`

## About

This image contains the native dependencies needed by .NET. It does not include .NET. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

* [.NET self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.debian) builds and runs an application as a self-contained application.

## Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

### Distroless images

.NET [distroless container images](https://github.com/dotnet/dotnet-docker/blob/main/documentation/distroless.md) contain only the minimal set of packages .NET needs, with everything else removed.
Due to their limited set of packages, distroless containers have a minimized security attack surface, smaller deployment sizes, and faster start-up time compared to their non-distroless counterparts.
They contain the following features:

* Minimal set of packages required for .NET applications
* Non-root user by default
* No package manager
* No shell

.NET offers distroless images for [Azure Linux](https://github.com/dotnet/dotnet-docker/blob/main/documentation/azurelinux.md) and [Ubuntu (Chiseled)](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md).

## Related Repositories

.NET:

* [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.md): .NET
* [dotnet/runtime-deps](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime-deps.md): .NET Runtime Dependencies
* [dotnet/nightly/sdk](https://github.com/dotnet/dotnet-docker/blob/nightly/README.sdk.md): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://github.com/dotnet/dotnet-docker/blob/nightly/README.aspnet.md): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://github.com/dotnet/dotnet-docker/blob/nightly/README.runtime.md): .NET Runtime (Preview)
* [dotnet/nightly/monitor](https://github.com/dotnet/dotnet-docker/blob/nightly/README.monitor.md): .NET Monitor Tool (Preview)
* [dotnet/nightly/aspire-dashboard](https://github.com/dotnet/dotnet-docker/blob/nightly/README.aspire-dashboard.md): .NET Aspire Dashboard (Preview)
* [dotnet/nightly/yarp](https://github.com/dotnet/dotnet-docker/blob/nightly/README.yarp.md): YARP (Yet Another Reverse Proxy) (Preview)
* [dotnet/samples](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md): .NET Samples

.NET Framework:

* [dotnet/framework](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.md): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.samples.md): .NET Framework, ASP.NET and WCF Samples

## Full Tag Listing

### Linux amd64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.2-bookworm-slim-amd64, 9.0-bookworm-slim-amd64, 9.0.2-bookworm-slim, 9.0-bookworm-slim, 9.0.2, 9.0 | [Dockerfile](src/runtime-deps/9.0/bookworm-slim/amd64/Dockerfile) | Debian 12
9.0.2-alpine3.21-amd64, 9.0-alpine3.21-amd64, 9.0-alpine-amd64, 9.0.2-alpine3.21, 9.0-alpine3.21, 9.0-alpine | [Dockerfile](src/runtime-deps/9.0/alpine3.21/amd64/Dockerfile) | Alpine 3.21
9.0.2-alpine3.21-aot-amd64, 9.0-alpine3.21-aot-amd64, 9.0-alpine-aot-amd64, 9.0.2-alpine3.21-aot, 9.0-alpine3.21-aot, 9.0-alpine-aot | [Dockerfile](src/runtime-deps/9.0/alpine3.21-aot/amd64/Dockerfile) | Alpine 3.21
9.0.2-alpine3.21-extra-amd64, 9.0-alpine3.21-extra-amd64, 9.0-alpine-extra-amd64, 9.0.2-alpine3.21-extra, 9.0-alpine3.21-extra, 9.0-alpine-extra | [Dockerfile](src/runtime-deps/9.0/alpine3.21-extra/amd64/Dockerfile) | Alpine 3.21
9.0.2-alpine3.20-amd64, 9.0-alpine3.20-amd64, 9.0.2-alpine3.20, 9.0-alpine3.20 | [Dockerfile](src/runtime-deps/9.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
9.0.2-alpine3.20-aot-amd64, 9.0-alpine3.20-aot-amd64, 9.0.2-alpine3.20-aot, 9.0-alpine3.20-aot | [Dockerfile](src/runtime-deps/9.0/alpine3.20-aot/amd64/Dockerfile) | Alpine 3.20
9.0.2-alpine3.20-extra-amd64, 9.0-alpine3.20-extra-amd64, 9.0.2-alpine3.20-extra, 9.0-alpine3.20-extra | [Dockerfile](src/runtime-deps/9.0/alpine3.20-extra/amd64/Dockerfile) | Alpine 3.20
9.0.2-noble-amd64, 9.0-noble-amd64, 9.0.2-noble, 9.0-noble | [Dockerfile](src/runtime-deps/9.0/noble/amd64/Dockerfile) | Ubuntu 24.04
9.0.2-noble-chiseled-amd64, 9.0-noble-chiseled-amd64, 9.0.2-noble-chiseled, 9.0-noble-chiseled | [Dockerfile](src/runtime-deps/9.0/noble-chiseled/amd64/Dockerfile) | Ubuntu 24.04
9.0.2-noble-chiseled-aot-amd64, 9.0-noble-chiseled-aot-amd64, 9.0.2-noble-chiseled-aot, 9.0-noble-chiseled-aot | [Dockerfile](src/runtime-deps/9.0/noble-chiseled-aot/amd64/Dockerfile) | Ubuntu 24.04
9.0.2-noble-chiseled-extra-amd64, 9.0-noble-chiseled-extra-amd64, 9.0.2-noble-chiseled-extra, 9.0-noble-chiseled-extra | [Dockerfile](src/runtime-deps/9.0/noble-chiseled-extra/amd64/Dockerfile) | Ubuntu 24.04
9.0.2-azurelinux3.0-amd64, 9.0-azurelinux3.0-amd64, 9.0.2-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/runtime-deps/9.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
9.0.2-azurelinux3.0-distroless-amd64, 9.0-azurelinux3.0-distroless-amd64, 9.0.2-azurelinux3.0-distroless, 9.0-azurelinux3.0-distroless | [Dockerfile](src/runtime-deps/9.0/azurelinux3.0-distroless/amd64/Dockerfile) | Azure Linux 3.0
9.0.2-azurelinux3.0-distroless-aot-amd64, 9.0-azurelinux3.0-distroless-aot-amd64, 9.0.2-azurelinux3.0-distroless-aot, 9.0-azurelinux3.0-distroless-aot | [Dockerfile](src/runtime-deps/9.0/azurelinux3.0-distroless-aot/amd64/Dockerfile) | Azure Linux 3.0
9.0.2-azurelinux3.0-distroless-extra-amd64, 9.0-azurelinux3.0-distroless-extra-amd64, 9.0.2-azurelinux3.0-distroless-extra, 9.0-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime-deps/9.0/azurelinux3.0-distroless-extra/amd64/Dockerfile) | Azure Linux 3.0
8.0.13-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.13-bookworm-slim, 8.0-bookworm-slim, 8.0.13, 8.0 | [Dockerfile](src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.13-alpine3.21-amd64, 8.0-alpine3.21-amd64, 8.0-alpine-amd64, 8.0.13-alpine3.21, 8.0-alpine3.21, 8.0-alpine | [Dockerfile](src/runtime-deps/8.0/alpine3.21/amd64/Dockerfile) | Alpine 3.21
8.0.13-alpine3.21-aot-amd64, 8.0-alpine3.21-aot-amd64, 8.0-alpine-aot-amd64, 8.0.13-alpine3.21-aot, 8.0-alpine3.21-aot, 8.0-alpine-aot | [Dockerfile](src/runtime-deps/8.0/alpine3.21-aot/amd64/Dockerfile) | Alpine 3.21
8.0.13-alpine3.21-extra-amd64, 8.0-alpine3.21-extra-amd64, 8.0-alpine-extra-amd64, 8.0.13-alpine3.21-extra, 8.0-alpine3.21-extra, 8.0-alpine-extra | [Dockerfile](src/runtime-deps/8.0/alpine3.21-extra/amd64/Dockerfile) | Alpine 3.21
8.0.13-alpine3.20-amd64, 8.0-alpine3.20-amd64, 8.0.13-alpine3.20, 8.0-alpine3.20 | [Dockerfile](src/runtime-deps/8.0/alpine3.20/amd64/Dockerfile) | Alpine 3.20
8.0.13-alpine3.20-aot-amd64, 8.0-alpine3.20-aot-amd64, 8.0.13-alpine3.20-aot, 8.0-alpine3.20-aot | [Dockerfile](src/runtime-deps/8.0/alpine3.20-aot/amd64/Dockerfile) | Alpine 3.20
8.0.13-alpine3.20-extra-amd64, 8.0-alpine3.20-extra-amd64, 8.0.13-alpine3.20-extra, 8.0-alpine3.20-extra | [Dockerfile](src/runtime-deps/8.0/alpine3.20-extra/amd64/Dockerfile) | Alpine 3.20
8.0.13-noble-amd64, 8.0-noble-amd64, 8.0.13-noble, 8.0-noble | [Dockerfile](src/runtime-deps/8.0/noble/amd64/Dockerfile) | Ubuntu 24.04
8.0.13-noble-chiseled-amd64, 8.0-noble-chiseled-amd64, 8.0.13-noble-chiseled, 8.0-noble-chiseled | [Dockerfile](src/runtime-deps/8.0/noble-chiseled/amd64/Dockerfile) | Ubuntu 24.04
8.0.13-noble-chiseled-aot-amd64, 8.0-noble-chiseled-aot-amd64, 8.0.13-noble-chiseled-aot, 8.0-noble-chiseled-aot | [Dockerfile](src/runtime-deps/8.0/noble-chiseled-aot/amd64/Dockerfile) | Ubuntu 24.04
8.0.13-noble-chiseled-extra-amd64, 8.0-noble-chiseled-extra-amd64, 8.0.13-noble-chiseled-extra, 8.0-noble-chiseled-extra | [Dockerfile](src/runtime-deps/8.0/noble-chiseled-extra/amd64/Dockerfile) | Ubuntu 24.04
8.0.13-jammy-amd64, 8.0-jammy-amd64, 8.0.13-jammy, 8.0-jammy | [Dockerfile](src/runtime-deps/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.13-jammy-chiseled-amd64, 8.0-jammy-chiseled-amd64, 8.0.13-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](src/runtime-deps/8.0/jammy-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.13-jammy-chiseled-aot-amd64, 8.0-jammy-chiseled-aot-amd64, 8.0.13-jammy-chiseled-aot, 8.0-jammy-chiseled-aot | [Dockerfile](src/runtime-deps/8.0/jammy-chiseled-aot/amd64/Dockerfile) | Ubuntu 22.04
8.0.13-jammy-chiseled-extra-amd64, 8.0-jammy-chiseled-extra-amd64, 8.0.13-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](src/runtime-deps/8.0/jammy-chiseled-extra/amd64/Dockerfile) | Ubuntu 22.04
8.0.13-azurelinux3.0-amd64, 8.0-azurelinux3.0-amd64, 8.0.13-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/runtime-deps/8.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
8.0.13-azurelinux3.0-distroless-amd64, 8.0-azurelinux3.0-distroless-amd64, 8.0.13-azurelinux3.0-distroless, 8.0-azurelinux3.0-distroless | [Dockerfile](src/runtime-deps/8.0/azurelinux3.0-distroless/amd64/Dockerfile) | Azure Linux 3.0
8.0.13-azurelinux3.0-distroless-aot-amd64, 8.0-azurelinux3.0-distroless-aot-amd64, 8.0.13-azurelinux3.0-distroless-aot, 8.0-azurelinux3.0-distroless-aot | [Dockerfile](src/runtime-deps/8.0/azurelinux3.0-distroless-aot/amd64/Dockerfile) | Azure Linux 3.0
8.0.13-azurelinux3.0-distroless-extra-amd64, 8.0-azurelinux3.0-distroless-extra-amd64, 8.0.13-azurelinux3.0-distroless-extra, 8.0-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime-deps/8.0/azurelinux3.0-distroless-extra/amd64/Dockerfile) | Azure Linux 3.0
8.0.13-cbl-mariner2.0-amd64, 8.0-cbl-mariner2.0-amd64, 8.0.13-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](src/runtime-deps/8.0/cbl-mariner2.0/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.13-cbl-mariner2.0-distroless-amd64, 8.0-cbl-mariner2.0-distroless-amd64, 8.0.13-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](src/runtime-deps/8.0/cbl-mariner2.0-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.13-cbl-mariner2.0-distroless-aot-amd64, 8.0-cbl-mariner2.0-distroless-aot-amd64, 8.0.13-cbl-mariner2.0-distroless-aot, 8.0-cbl-mariner2.0-distroless-aot | [Dockerfile](src/runtime-deps/8.0/cbl-mariner2.0-distroless-aot/amd64/Dockerfile) | CBL-Mariner 2.0
8.0.13-cbl-mariner2.0-distroless-extra-amd64, 8.0-cbl-mariner2.0-distroless-extra-amd64, 8.0.13-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](src/runtime-deps/8.0/cbl-mariner2.0-distroless-extra/amd64/Dockerfile) | CBL-Mariner 2.0

#### .NET 10 Preview Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
10.0.0-preview.1-noble-amd64, 10.0-preview-noble-amd64, 10.0.0-preview.1-noble, 10.0-preview-noble, 10.0.0-preview.1, 10.0-preview, latest | [Dockerfile](src/runtime-deps/10.0/noble/amd64/Dockerfile) | Ubuntu 24.04
10.0.0-preview.1-noble-chiseled-amd64, 10.0-preview-noble-chiseled-amd64, 10.0.0-preview.1-noble-chiseled, 10.0-preview-noble-chiseled | [Dockerfile](src/runtime-deps/10.0/noble-chiseled/amd64/Dockerfile) | Ubuntu 24.04
10.0.0-preview.1-noble-chiseled-extra-amd64, 10.0-preview-noble-chiseled-extra-amd64, 10.0.0-preview.1-noble-chiseled-extra, 10.0-preview-noble-chiseled-extra | [Dockerfile](src/runtime-deps/10.0/noble-chiseled-extra/amd64/Dockerfile) | Ubuntu 24.04
10.0.0-preview.1-alpine3.21-amd64, 10.0-preview-alpine3.21-amd64, 10.0-preview-alpine-amd64, 10.0.0-preview.1-alpine3.21, 10.0-preview-alpine3.21, 10.0-preview-alpine | [Dockerfile](src/runtime-deps/10.0/alpine3.21/amd64/Dockerfile) | Alpine 3.21
10.0.0-preview.1-alpine3.21-extra-amd64, 10.0-preview-alpine3.21-extra-amd64, 10.0-preview-alpine-extra-amd64, 10.0.0-preview.1-alpine3.21-extra, 10.0-preview-alpine3.21-extra, 10.0-preview-alpine-extra | [Dockerfile](src/runtime-deps/10.0/alpine3.21-extra/amd64/Dockerfile) | Alpine 3.21
10.0.0-preview.1-azurelinux3.0-amd64, 10.0-preview-azurelinux3.0-amd64, 10.0.0-preview.1-azurelinux3.0, 10.0-preview-azurelinux3.0 | [Dockerfile](src/runtime-deps/10.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
10.0.0-preview.1-azurelinux3.0-distroless-amd64, 10.0-preview-azurelinux3.0-distroless-amd64, 10.0.0-preview.1-azurelinux3.0-distroless, 10.0-preview-azurelinux3.0-distroless | [Dockerfile](src/runtime-deps/10.0/azurelinux3.0-distroless/amd64/Dockerfile) | Azure Linux 3.0
10.0.0-preview.1-azurelinux3.0-distroless-extra-amd64, 10.0-preview-azurelinux3.0-distroless-extra-amd64, 10.0.0-preview.1-azurelinux3.0-distroless-extra, 10.0-preview-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime-deps/10.0/azurelinux3.0-distroless-extra/amd64/Dockerfile) | Azure Linux 3.0
10.0.0-preview.1-trixie-slim-amd64, 10.0-preview-trixie-slim-amd64, 10.0.0-preview.1-trixie-slim, 10.0-preview-trixie-slim | [Dockerfile](src/runtime-deps/10.0/trixie-slim/amd64/Dockerfile) | Debian 13

### Linux arm64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.2-bookworm-slim-arm64v8, 9.0-bookworm-slim-arm64v8, 9.0.2-bookworm-slim, 9.0-bookworm-slim, 9.0.2, 9.0 | [Dockerfile](src/runtime-deps/9.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
9.0.2-alpine3.21-arm64v8, 9.0-alpine3.21-arm64v8, 9.0-alpine-arm64v8, 9.0.2-alpine3.21, 9.0-alpine3.21, 9.0-alpine | [Dockerfile](src/runtime-deps/9.0/alpine3.21/arm64v8/Dockerfile) | Alpine 3.21
9.0.2-alpine3.21-aot-arm64v8, 9.0-alpine3.21-aot-arm64v8, 9.0-alpine-aot-arm64v8, 9.0.2-alpine3.21-aot, 9.0-alpine3.21-aot, 9.0-alpine-aot | [Dockerfile](src/runtime-deps/9.0/alpine3.21-aot/arm64v8/Dockerfile) | Alpine 3.21
9.0.2-alpine3.21-extra-arm64v8, 9.0-alpine3.21-extra-arm64v8, 9.0-alpine-extra-arm64v8, 9.0.2-alpine3.21-extra, 9.0-alpine3.21-extra, 9.0-alpine-extra | [Dockerfile](src/runtime-deps/9.0/alpine3.21-extra/arm64v8/Dockerfile) | Alpine 3.21
9.0.2-alpine3.20-arm64v8, 9.0-alpine3.20-arm64v8, 9.0.2-alpine3.20, 9.0-alpine3.20 | [Dockerfile](src/runtime-deps/9.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
9.0.2-alpine3.20-aot-arm64v8, 9.0-alpine3.20-aot-arm64v8, 9.0.2-alpine3.20-aot, 9.0-alpine3.20-aot | [Dockerfile](src/runtime-deps/9.0/alpine3.20-aot/arm64v8/Dockerfile) | Alpine 3.20
9.0.2-alpine3.20-extra-arm64v8, 9.0-alpine3.20-extra-arm64v8, 9.0.2-alpine3.20-extra, 9.0-alpine3.20-extra | [Dockerfile](src/runtime-deps/9.0/alpine3.20-extra/arm64v8/Dockerfile) | Alpine 3.20
9.0.2-noble-arm64v8, 9.0-noble-arm64v8, 9.0.2-noble, 9.0-noble | [Dockerfile](src/runtime-deps/9.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.2-noble-chiseled-arm64v8, 9.0-noble-chiseled-arm64v8, 9.0.2-noble-chiseled, 9.0-noble-chiseled | [Dockerfile](src/runtime-deps/9.0/noble-chiseled/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.2-noble-chiseled-aot-arm64v8, 9.0-noble-chiseled-aot-arm64v8, 9.0.2-noble-chiseled-aot, 9.0-noble-chiseled-aot | [Dockerfile](src/runtime-deps/9.0/noble-chiseled-aot/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.2-noble-chiseled-extra-arm64v8, 9.0-noble-chiseled-extra-arm64v8, 9.0.2-noble-chiseled-extra, 9.0-noble-chiseled-extra | [Dockerfile](src/runtime-deps/9.0/noble-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.2-azurelinux3.0-arm64v8, 9.0-azurelinux3.0-arm64v8, 9.0.2-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/runtime-deps/9.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.2-azurelinux3.0-distroless-arm64v8, 9.0-azurelinux3.0-distroless-arm64v8, 9.0.2-azurelinux3.0-distroless, 9.0-azurelinux3.0-distroless | [Dockerfile](src/runtime-deps/9.0/azurelinux3.0-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.2-azurelinux3.0-distroless-aot-arm64v8, 9.0-azurelinux3.0-distroless-aot-arm64v8, 9.0.2-azurelinux3.0-distroless-aot, 9.0-azurelinux3.0-distroless-aot | [Dockerfile](src/runtime-deps/9.0/azurelinux3.0-distroless-aot/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.2-azurelinux3.0-distroless-extra-arm64v8, 9.0-azurelinux3.0-distroless-extra-arm64v8, 9.0.2-azurelinux3.0-distroless-extra, 9.0-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime-deps/9.0/azurelinux3.0-distroless-extra/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.13-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.13-bookworm-slim, 8.0-bookworm-slim, 8.0.13, 8.0 | [Dockerfile](src/runtime-deps/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.13-alpine3.21-arm64v8, 8.0-alpine3.21-arm64v8, 8.0-alpine-arm64v8, 8.0.13-alpine3.21, 8.0-alpine3.21, 8.0-alpine | [Dockerfile](src/runtime-deps/8.0/alpine3.21/arm64v8/Dockerfile) | Alpine 3.21
8.0.13-alpine3.21-aot-arm64v8, 8.0-alpine3.21-aot-arm64v8, 8.0-alpine-aot-arm64v8, 8.0.13-alpine3.21-aot, 8.0-alpine3.21-aot, 8.0-alpine-aot | [Dockerfile](src/runtime-deps/8.0/alpine3.21-aot/arm64v8/Dockerfile) | Alpine 3.21
8.0.13-alpine3.21-extra-arm64v8, 8.0-alpine3.21-extra-arm64v8, 8.0-alpine-extra-arm64v8, 8.0.13-alpine3.21-extra, 8.0-alpine3.21-extra, 8.0-alpine-extra | [Dockerfile](src/runtime-deps/8.0/alpine3.21-extra/arm64v8/Dockerfile) | Alpine 3.21
8.0.13-alpine3.20-arm64v8, 8.0-alpine3.20-arm64v8, 8.0.13-alpine3.20, 8.0-alpine3.20 | [Dockerfile](src/runtime-deps/8.0/alpine3.20/arm64v8/Dockerfile) | Alpine 3.20
8.0.13-alpine3.20-aot-arm64v8, 8.0-alpine3.20-aot-arm64v8, 8.0.13-alpine3.20-aot, 8.0-alpine3.20-aot | [Dockerfile](src/runtime-deps/8.0/alpine3.20-aot/arm64v8/Dockerfile) | Alpine 3.20
8.0.13-alpine3.20-extra-arm64v8, 8.0-alpine3.20-extra-arm64v8, 8.0.13-alpine3.20-extra, 8.0-alpine3.20-extra | [Dockerfile](src/runtime-deps/8.0/alpine3.20-extra/arm64v8/Dockerfile) | Alpine 3.20
8.0.13-noble-arm64v8, 8.0-noble-arm64v8, 8.0.13-noble, 8.0-noble | [Dockerfile](src/runtime-deps/8.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.13-noble-chiseled-arm64v8, 8.0-noble-chiseled-arm64v8, 8.0.13-noble-chiseled, 8.0-noble-chiseled | [Dockerfile](src/runtime-deps/8.0/noble-chiseled/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.13-noble-chiseled-aot-arm64v8, 8.0-noble-chiseled-aot-arm64v8, 8.0.13-noble-chiseled-aot, 8.0-noble-chiseled-aot | [Dockerfile](src/runtime-deps/8.0/noble-chiseled-aot/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.13-noble-chiseled-extra-arm64v8, 8.0-noble-chiseled-extra-arm64v8, 8.0.13-noble-chiseled-extra, 8.0-noble-chiseled-extra | [Dockerfile](src/runtime-deps/8.0/noble-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.13-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.13-jammy, 8.0-jammy | [Dockerfile](src/runtime-deps/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.13-jammy-chiseled-arm64v8, 8.0-jammy-chiseled-arm64v8, 8.0.13-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](src/runtime-deps/8.0/jammy-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.13-jammy-chiseled-aot-arm64v8, 8.0-jammy-chiseled-aot-arm64v8, 8.0.13-jammy-chiseled-aot, 8.0-jammy-chiseled-aot | [Dockerfile](src/runtime-deps/8.0/jammy-chiseled-aot/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.13-jammy-chiseled-extra-arm64v8, 8.0-jammy-chiseled-extra-arm64v8, 8.0.13-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](src/runtime-deps/8.0/jammy-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.13-azurelinux3.0-arm64v8, 8.0-azurelinux3.0-arm64v8, 8.0.13-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/runtime-deps/8.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.13-azurelinux3.0-distroless-arm64v8, 8.0-azurelinux3.0-distroless-arm64v8, 8.0.13-azurelinux3.0-distroless, 8.0-azurelinux3.0-distroless | [Dockerfile](src/runtime-deps/8.0/azurelinux3.0-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.13-azurelinux3.0-distroless-aot-arm64v8, 8.0-azurelinux3.0-distroless-aot-arm64v8, 8.0.13-azurelinux3.0-distroless-aot, 8.0-azurelinux3.0-distroless-aot | [Dockerfile](src/runtime-deps/8.0/azurelinux3.0-distroless-aot/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.13-azurelinux3.0-distroless-extra-arm64v8, 8.0-azurelinux3.0-distroless-extra-arm64v8, 8.0.13-azurelinux3.0-distroless-extra, 8.0-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime-deps/8.0/azurelinux3.0-distroless-extra/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.13-cbl-mariner2.0-arm64v8, 8.0-cbl-mariner2.0-arm64v8, 8.0.13-cbl-mariner2.0, 8.0-cbl-mariner2.0 | [Dockerfile](src/runtime-deps/8.0/cbl-mariner2.0/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.13-cbl-mariner2.0-distroless-arm64v8, 8.0-cbl-mariner2.0-distroless-arm64v8, 8.0.13-cbl-mariner2.0-distroless, 8.0-cbl-mariner2.0-distroless | [Dockerfile](src/runtime-deps/8.0/cbl-mariner2.0-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.13-cbl-mariner2.0-distroless-aot-arm64v8, 8.0-cbl-mariner2.0-distroless-aot-arm64v8, 8.0.13-cbl-mariner2.0-distroless-aot, 8.0-cbl-mariner2.0-distroless-aot | [Dockerfile](src/runtime-deps/8.0/cbl-mariner2.0-distroless-aot/arm64v8/Dockerfile) | CBL-Mariner 2.0
8.0.13-cbl-mariner2.0-distroless-extra-arm64v8, 8.0-cbl-mariner2.0-distroless-extra-arm64v8, 8.0.13-cbl-mariner2.0-distroless-extra, 8.0-cbl-mariner2.0-distroless-extra | [Dockerfile](src/runtime-deps/8.0/cbl-mariner2.0-distroless-extra/arm64v8/Dockerfile) | CBL-Mariner 2.0

#### .NET 10 Preview Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
10.0.0-preview.1-noble-arm64v8, 10.0-preview-noble-arm64v8, 10.0.0-preview.1-noble, 10.0-preview-noble, 10.0.0-preview.1, 10.0-preview, latest | [Dockerfile](src/runtime-deps/10.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
10.0.0-preview.1-noble-chiseled-arm64v8, 10.0-preview-noble-chiseled-arm64v8, 10.0.0-preview.1-noble-chiseled, 10.0-preview-noble-chiseled | [Dockerfile](src/runtime-deps/10.0/noble-chiseled/arm64v8/Dockerfile) | Ubuntu 24.04
10.0.0-preview.1-noble-chiseled-extra-arm64v8, 10.0-preview-noble-chiseled-extra-arm64v8, 10.0.0-preview.1-noble-chiseled-extra, 10.0-preview-noble-chiseled-extra | [Dockerfile](src/runtime-deps/10.0/noble-chiseled-extra/arm64v8/Dockerfile) | Ubuntu 24.04
10.0.0-preview.1-alpine3.21-arm64v8, 10.0-preview-alpine3.21-arm64v8, 10.0-preview-alpine-arm64v8, 10.0.0-preview.1-alpine3.21, 10.0-preview-alpine3.21, 10.0-preview-alpine | [Dockerfile](src/runtime-deps/10.0/alpine3.21/arm64v8/Dockerfile) | Alpine 3.21
10.0.0-preview.1-alpine3.21-extra-arm64v8, 10.0-preview-alpine3.21-extra-arm64v8, 10.0-preview-alpine-extra-arm64v8, 10.0.0-preview.1-alpine3.21-extra, 10.0-preview-alpine3.21-extra, 10.0-preview-alpine-extra | [Dockerfile](src/runtime-deps/10.0/alpine3.21-extra/arm64v8/Dockerfile) | Alpine 3.21
10.0.0-preview.1-azurelinux3.0-arm64v8, 10.0-preview-azurelinux3.0-arm64v8, 10.0.0-preview.1-azurelinux3.0, 10.0-preview-azurelinux3.0 | [Dockerfile](src/runtime-deps/10.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
10.0.0-preview.1-azurelinux3.0-distroless-arm64v8, 10.0-preview-azurelinux3.0-distroless-arm64v8, 10.0.0-preview.1-azurelinux3.0-distroless, 10.0-preview-azurelinux3.0-distroless | [Dockerfile](src/runtime-deps/10.0/azurelinux3.0-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
10.0.0-preview.1-azurelinux3.0-distroless-extra-arm64v8, 10.0-preview-azurelinux3.0-distroless-extra-arm64v8, 10.0.0-preview.1-azurelinux3.0-distroless-extra, 10.0-preview-azurelinux3.0-distroless-extra | [Dockerfile](src/runtime-deps/10.0/azurelinux3.0-distroless-extra/arm64v8/Dockerfile) | Azure Linux 3.0
10.0.0-preview.1-trixie-slim-arm64v8, 10.0-preview-trixie-slim-arm64v8, 10.0.0-preview.1-trixie-slim, 10.0-preview-trixie-slim | [Dockerfile](src/runtime-deps/10.0/trixie-slim/arm64v8/Dockerfile) | Debian 13

### Linux arm32 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.2-bookworm-slim-arm32v7, 9.0-bookworm-slim-arm32v7, 9.0.2-bookworm-slim, 9.0-bookworm-slim, 9.0.2, 9.0 | [Dockerfile](src/runtime-deps/9.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
9.0.2-alpine3.21-arm32v7, 9.0-alpine3.21-arm32v7, 9.0-alpine-arm32v7, 9.0.2-alpine3.21, 9.0-alpine3.21, 9.0-alpine | [Dockerfile](src/runtime-deps/9.0/alpine3.21/arm32v7/Dockerfile) | Alpine 3.21
9.0.2-alpine3.21-aot-arm32v7, 9.0-alpine3.21-aot-arm32v7, 9.0-alpine-aot-arm32v7, 9.0.2-alpine3.21-aot, 9.0-alpine3.21-aot, 9.0-alpine-aot | [Dockerfile](src/runtime-deps/9.0/alpine3.21-aot/arm32v7/Dockerfile) | Alpine 3.21
9.0.2-alpine3.21-extra-arm32v7, 9.0-alpine3.21-extra-arm32v7, 9.0-alpine-extra-arm32v7, 9.0.2-alpine3.21-extra, 9.0-alpine3.21-extra, 9.0-alpine-extra | [Dockerfile](src/runtime-deps/9.0/alpine3.21-extra/arm32v7/Dockerfile) | Alpine 3.21
9.0.2-alpine3.20-arm32v7, 9.0-alpine3.20-arm32v7, 9.0.2-alpine3.20, 9.0-alpine3.20 | [Dockerfile](src/runtime-deps/9.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
9.0.2-alpine3.20-aot-arm32v7, 9.0-alpine3.20-aot-arm32v7, 9.0.2-alpine3.20-aot, 9.0-alpine3.20-aot | [Dockerfile](src/runtime-deps/9.0/alpine3.20-aot/arm32v7/Dockerfile) | Alpine 3.20
9.0.2-alpine3.20-extra-arm32v7, 9.0-alpine3.20-extra-arm32v7, 9.0.2-alpine3.20-extra, 9.0-alpine3.20-extra | [Dockerfile](src/runtime-deps/9.0/alpine3.20-extra/arm32v7/Dockerfile) | Alpine 3.20
9.0.2-noble-arm32v7, 9.0-noble-arm32v7, 9.0.2-noble, 9.0-noble | [Dockerfile](src/runtime-deps/9.0/noble/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.2-noble-chiseled-arm32v7, 9.0-noble-chiseled-arm32v7, 9.0.2-noble-chiseled, 9.0-noble-chiseled | [Dockerfile](src/runtime-deps/9.0/noble-chiseled/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.2-noble-chiseled-aot-arm32v7, 9.0-noble-chiseled-aot-arm32v7, 9.0.2-noble-chiseled-aot, 9.0-noble-chiseled-aot | [Dockerfile](src/runtime-deps/9.0/noble-chiseled-aot/arm32v7/Dockerfile) | Ubuntu 24.04
9.0.2-noble-chiseled-extra-arm32v7, 9.0-noble-chiseled-extra-arm32v7, 9.0.2-noble-chiseled-extra, 9.0-noble-chiseled-extra | [Dockerfile](src/runtime-deps/9.0/noble-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 24.04
8.0.13-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.13-bookworm-slim, 8.0-bookworm-slim, 8.0.13, 8.0 | [Dockerfile](src/runtime-deps/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.13-alpine3.21-arm32v7, 8.0-alpine3.21-arm32v7, 8.0-alpine-arm32v7, 8.0.13-alpine3.21, 8.0-alpine3.21, 8.0-alpine | [Dockerfile](src/runtime-deps/8.0/alpine3.21/arm32v7/Dockerfile) | Alpine 3.21
8.0.13-alpine3.21-aot-arm32v7, 8.0-alpine3.21-aot-arm32v7, 8.0-alpine-aot-arm32v7, 8.0.13-alpine3.21-aot, 8.0-alpine3.21-aot, 8.0-alpine-aot | [Dockerfile](src/runtime-deps/8.0/alpine3.21-aot/arm32v7/Dockerfile) | Alpine 3.21
8.0.13-alpine3.21-extra-arm32v7, 8.0-alpine3.21-extra-arm32v7, 8.0-alpine-extra-arm32v7, 8.0.13-alpine3.21-extra, 8.0-alpine3.21-extra, 8.0-alpine-extra | [Dockerfile](src/runtime-deps/8.0/alpine3.21-extra/arm32v7/Dockerfile) | Alpine 3.21
8.0.13-alpine3.20-arm32v7, 8.0-alpine3.20-arm32v7, 8.0.13-alpine3.20, 8.0-alpine3.20 | [Dockerfile](src/runtime-deps/8.0/alpine3.20/arm32v7/Dockerfile) | Alpine 3.20
8.0.13-alpine3.20-aot-arm32v7, 8.0-alpine3.20-aot-arm32v7, 8.0.13-alpine3.20-aot, 8.0-alpine3.20-aot | [Dockerfile](src/runtime-deps/8.0/alpine3.20-aot/arm32v7/Dockerfile) | Alpine 3.20
8.0.13-alpine3.20-extra-arm32v7, 8.0-alpine3.20-extra-arm32v7, 8.0.13-alpine3.20-extra, 8.0-alpine3.20-extra | [Dockerfile](src/runtime-deps/8.0/alpine3.20-extra/arm32v7/Dockerfile) | Alpine 3.20
8.0.13-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.13-jammy, 8.0-jammy | [Dockerfile](src/runtime-deps/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.13-jammy-chiseled-arm32v7, 8.0-jammy-chiseled-arm32v7, 8.0.13-jammy-chiseled, 8.0-jammy-chiseled | [Dockerfile](src/runtime-deps/8.0/jammy-chiseled/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.13-jammy-chiseled-aot-arm32v7, 8.0-jammy-chiseled-aot-arm32v7, 8.0.13-jammy-chiseled-aot, 8.0-jammy-chiseled-aot | [Dockerfile](src/runtime-deps/8.0/jammy-chiseled-aot/arm32v7/Dockerfile) | Ubuntu 22.04
8.0.13-jammy-chiseled-extra-arm32v7, 8.0-jammy-chiseled-extra-arm32v7, 8.0.13-jammy-chiseled-extra, 8.0-jammy-chiseled-extra | [Dockerfile](src/runtime-deps/8.0/jammy-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 22.04

#### .NET 10 Preview Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
10.0.0-preview.1-noble-arm32v7, 10.0-preview-noble-arm32v7, 10.0.0-preview.1-noble, 10.0-preview-noble, 10.0.0-preview.1, 10.0-preview, latest | [Dockerfile](src/runtime-deps/10.0/noble/arm32v7/Dockerfile) | Ubuntu 24.04
10.0.0-preview.1-noble-chiseled-arm32v7, 10.0-preview-noble-chiseled-arm32v7, 10.0.0-preview.1-noble-chiseled, 10.0-preview-noble-chiseled | [Dockerfile](src/runtime-deps/10.0/noble-chiseled/arm32v7/Dockerfile) | Ubuntu 24.04
10.0.0-preview.1-noble-chiseled-extra-arm32v7, 10.0-preview-noble-chiseled-extra-arm32v7, 10.0.0-preview.1-noble-chiseled-extra, 10.0-preview-noble-chiseled-extra | [Dockerfile](src/runtime-deps/10.0/noble-chiseled-extra/arm32v7/Dockerfile) | Ubuntu 24.04
10.0.0-preview.1-alpine3.21-arm32v7, 10.0-preview-alpine3.21-arm32v7, 10.0-preview-alpine-arm32v7, 10.0.0-preview.1-alpine3.21, 10.0-preview-alpine3.21, 10.0-preview-alpine | [Dockerfile](src/runtime-deps/10.0/alpine3.21/arm32v7/Dockerfile) | Alpine 3.21
10.0.0-preview.1-alpine3.21-extra-arm32v7, 10.0-preview-alpine3.21-extra-arm32v7, 10.0-preview-alpine-extra-arm32v7, 10.0.0-preview.1-alpine3.21-extra, 10.0-preview-alpine3.21-extra, 10.0-preview-alpine-extra | [Dockerfile](src/runtime-deps/10.0/alpine3.21-extra/arm32v7/Dockerfile) | Alpine 3.21
10.0.0-preview.1-trixie-slim-arm32v7, 10.0-preview-trixie-slim-arm32v7, 10.0.0-preview.1-trixie-slim, 10.0-preview-trixie-slim | [Dockerfile](src/runtime-deps/10.0/trixie-slim/arm32v7/Dockerfile) | Debian 13
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/dotnet/nightly/runtime-deps/tags/list) for all supported and unsupported tags.*

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
