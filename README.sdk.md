# .NET SDK

## Featured Tags

* `10.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:10.0`
* `9.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:9.0`
* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:8.0`

## About

This image contains the .NET SDK which is comprised of three parts:

1. .NET CLI
1. .NET runtime
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Introduction to .NET and Docker](https://learn.microsoft.com/dotnet/core/docker/introduction) to learn more.

### Building .NET Apps with Docker

* [.NET Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

### Develop .NET Apps in a Container

The following samples show how to develop, build and test .NET applications with Docker without the need to install the .NET SDK.

* [Build .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/build-in-sdk-container.md)
* [Test .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-tests-in-sdk-container.md)
* [Run .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-in-sdk-container.md)

## Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

## Related Repositories

.NET:

* [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.md): .NET
* [dotnet/aspnet](https://github.com/dotnet/dotnet-docker/blob/main/README.aspnet.md): ASP.NET Core Runtime
* [dotnet/runtime](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime.md): .NET Runtime
* [dotnet/runtime-deps](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime-deps.md): .NET Runtime Dependencies
* [dotnet/monitor](https://github.com/dotnet/dotnet-docker/blob/main/README.monitor.md): .NET Monitor Tool
* [dotnet/aspire-dashboard](https://github.com/dotnet/dotnet-docker/blob/main/README.aspire-dashboard.md): Aspire Dashboard
* [dotnet/nightly/sdk](https://github.com/dotnet/dotnet-docker/blob/nightly/README.sdk.md): .NET SDK (Preview)
* [dotnet/samples](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md): .NET Samples

.NET Framework:

* [dotnet/framework](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.md): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.samples.md): .NET Framework, ASP.NET and WCF Samples

## Full Tag Listing

### Linux amd64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
10.0.101-noble-amd64, 10.0-noble-amd64, 10.0.101-noble, 10.0-noble, 10.0.101, 10.0, latest | [Dockerfile](src/sdk/10.0/noble/amd64/Dockerfile) | Ubuntu 24.04
10.0.101-noble-aot-amd64, 10.0-noble-aot-amd64, 10.0.101-noble-aot, 10.0-noble-aot, 10.0.101-aot, 10.0-aot | [Dockerfile](src/sdk/10.0/noble-aot/amd64/Dockerfile) | Ubuntu 24.04
10.0.101-alpine3.22-amd64, 10.0-alpine3.22-amd64, 10.0-alpine-amd64, 10.0.101-alpine3.22, 10.0-alpine3.22, 10.0-alpine | [Dockerfile](src/sdk/10.0/alpine3.22/amd64/Dockerfile) | Alpine 3.22
10.0.101-alpine3.22-aot-amd64, 10.0-alpine3.22-aot-amd64, 10.0-alpine-aot-amd64, 10.0.101-alpine3.22-aot, 10.0-alpine3.22-aot, 10.0-alpine-aot | [Dockerfile](src/sdk/10.0/alpine3.22-aot/amd64/Dockerfile) | Alpine 3.22
10.0.101-alpine3.23-amd64, 10.0-alpine3.23-amd64, 10.0.101-alpine3.23, 10.0-alpine3.23 | [Dockerfile](src/sdk/10.0/alpine3.23/amd64/Dockerfile) | Alpine 3.23
10.0.101-alpine3.23-aot-amd64, 10.0-alpine3.23-aot-amd64, 10.0.101-alpine3.23-aot, 10.0-alpine3.23-aot | [Dockerfile](src/sdk/10.0/alpine3.23-aot/amd64/Dockerfile) | Alpine 3.23
10.0.101-azurelinux3.0-amd64, 10.0-azurelinux3.0-amd64, 10.0.101-azurelinux3.0, 10.0-azurelinux3.0 | [Dockerfile](src/sdk/10.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
10.0.101-azurelinux3.0-aot-amd64, 10.0-azurelinux3.0-aot-amd64, 10.0.101-azurelinux3.0-aot, 10.0-azurelinux3.0-aot | [Dockerfile](src/sdk/10.0/azurelinux3.0-aot/amd64/Dockerfile) | Azure Linux 3.0
9.0.308-bookworm-slim-amd64, 9.0-bookworm-slim-amd64, 9.0.308-bookworm-slim, 9.0-bookworm-slim, 9.0.308, 9.0 | [Dockerfile](src/sdk/9.0/bookworm-slim/amd64/Dockerfile) | Debian 12
9.0.308-alpine3.22-amd64, 9.0-alpine3.22-amd64, 9.0-alpine-amd64, 9.0.308-alpine3.22, 9.0-alpine3.22, 9.0-alpine | [Dockerfile](src/sdk/9.0/alpine3.22/amd64/Dockerfile) | Alpine 3.22
9.0.308-alpine3.23-amd64, 9.0-alpine3.23-amd64, 9.0.308-alpine3.23, 9.0-alpine3.23 | [Dockerfile](src/sdk/9.0/alpine3.23/amd64/Dockerfile) | Alpine 3.23
9.0.308-noble-amd64, 9.0-noble-amd64, 9.0.308-noble, 9.0-noble | [Dockerfile](src/sdk/9.0/noble/amd64/Dockerfile) | Ubuntu 24.04
9.0.308-azurelinux3.0-amd64, 9.0-azurelinux3.0-amd64, 9.0.308-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/sdk/9.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0
8.0.416-bookworm-slim-amd64, 8.0-bookworm-slim-amd64, 8.0.416-bookworm-slim, 8.0-bookworm-slim, 8.0.416, 8.0 | [Dockerfile](src/sdk/8.0/bookworm-slim/amd64/Dockerfile) | Debian 12
8.0.416-alpine3.22-amd64, 8.0-alpine3.22-amd64, 8.0-alpine-amd64, 8.0.416-alpine3.22, 8.0-alpine3.22, 8.0-alpine | [Dockerfile](src/sdk/8.0/alpine3.22/amd64/Dockerfile) | Alpine 3.22
8.0.416-alpine3.23-amd64, 8.0-alpine3.23-amd64, 8.0.416-alpine3.23, 8.0-alpine3.23 | [Dockerfile](src/sdk/8.0/alpine3.23/amd64/Dockerfile) | Alpine 3.23
8.0.416-noble-amd64, 8.0-noble-amd64, 8.0.416-noble, 8.0-noble | [Dockerfile](src/sdk/8.0/noble/amd64/Dockerfile) | Ubuntu 24.04
8.0.416-jammy-amd64, 8.0-jammy-amd64, 8.0.416-jammy, 8.0-jammy | [Dockerfile](src/sdk/8.0/jammy/amd64/Dockerfile) | Ubuntu 22.04
8.0.416-azurelinux3.0-amd64, 8.0-azurelinux3.0-amd64, 8.0.416-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/sdk/8.0/azurelinux3.0/amd64/Dockerfile) | Azure Linux 3.0

### Linux arm64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
10.0.101-noble-arm64v8, 10.0-noble-arm64v8, 10.0.101-noble, 10.0-noble, 10.0.101, 10.0, latest | [Dockerfile](src/sdk/10.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
10.0.101-noble-aot-arm64v8, 10.0-noble-aot-arm64v8, 10.0.101-noble-aot, 10.0-noble-aot, 10.0.101-aot, 10.0-aot | [Dockerfile](src/sdk/10.0/noble-aot/arm64v8/Dockerfile) | Ubuntu 24.04
10.0.101-alpine3.22-arm64v8, 10.0-alpine3.22-arm64v8, 10.0-alpine-arm64v8, 10.0.101-alpine3.22, 10.0-alpine3.22, 10.0-alpine | [Dockerfile](src/sdk/10.0/alpine3.22/arm64v8/Dockerfile) | Alpine 3.22
10.0.101-alpine3.22-aot-arm64v8, 10.0-alpine3.22-aot-arm64v8, 10.0-alpine-aot-arm64v8, 10.0.101-alpine3.22-aot, 10.0-alpine3.22-aot, 10.0-alpine-aot | [Dockerfile](src/sdk/10.0/alpine3.22-aot/arm64v8/Dockerfile) | Alpine 3.22
10.0.101-alpine3.23-arm64v8, 10.0-alpine3.23-arm64v8, 10.0.101-alpine3.23, 10.0-alpine3.23 | [Dockerfile](src/sdk/10.0/alpine3.23/arm64v8/Dockerfile) | Alpine 3.23
10.0.101-alpine3.23-aot-arm64v8, 10.0-alpine3.23-aot-arm64v8, 10.0.101-alpine3.23-aot, 10.0-alpine3.23-aot | [Dockerfile](src/sdk/10.0/alpine3.23-aot/arm64v8/Dockerfile) | Alpine 3.23
10.0.101-azurelinux3.0-arm64v8, 10.0-azurelinux3.0-arm64v8, 10.0.101-azurelinux3.0, 10.0-azurelinux3.0 | [Dockerfile](src/sdk/10.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
10.0.101-azurelinux3.0-aot-arm64v8, 10.0-azurelinux3.0-aot-arm64v8, 10.0.101-azurelinux3.0-aot, 10.0-azurelinux3.0-aot | [Dockerfile](src/sdk/10.0/azurelinux3.0-aot/arm64v8/Dockerfile) | Azure Linux 3.0
9.0.308-bookworm-slim-arm64v8, 9.0-bookworm-slim-arm64v8, 9.0.308-bookworm-slim, 9.0-bookworm-slim, 9.0.308, 9.0 | [Dockerfile](src/sdk/9.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
9.0.308-alpine3.22-arm64v8, 9.0-alpine3.22-arm64v8, 9.0-alpine-arm64v8, 9.0.308-alpine3.22, 9.0-alpine3.22, 9.0-alpine | [Dockerfile](src/sdk/9.0/alpine3.22/arm64v8/Dockerfile) | Alpine 3.22
9.0.308-alpine3.23-arm64v8, 9.0-alpine3.23-arm64v8, 9.0.308-alpine3.23, 9.0-alpine3.23 | [Dockerfile](src/sdk/9.0/alpine3.23/arm64v8/Dockerfile) | Alpine 3.23
9.0.308-noble-arm64v8, 9.0-noble-arm64v8, 9.0.308-noble, 9.0-noble | [Dockerfile](src/sdk/9.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
9.0.308-azurelinux3.0-arm64v8, 9.0-azurelinux3.0-arm64v8, 9.0.308-azurelinux3.0, 9.0-azurelinux3.0 | [Dockerfile](src/sdk/9.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.416-bookworm-slim-arm64v8, 8.0-bookworm-slim-arm64v8, 8.0.416-bookworm-slim, 8.0-bookworm-slim, 8.0.416, 8.0 | [Dockerfile](src/sdk/8.0/bookworm-slim/arm64v8/Dockerfile) | Debian 12
8.0.416-alpine3.22-arm64v8, 8.0-alpine3.22-arm64v8, 8.0-alpine-arm64v8, 8.0.416-alpine3.22, 8.0-alpine3.22, 8.0-alpine | [Dockerfile](src/sdk/8.0/alpine3.22/arm64v8/Dockerfile) | Alpine 3.22
8.0.416-alpine3.23-arm64v8, 8.0-alpine3.23-arm64v8, 8.0.416-alpine3.23, 8.0-alpine3.23 | [Dockerfile](src/sdk/8.0/alpine3.23/arm64v8/Dockerfile) | Alpine 3.23
8.0.416-noble-arm64v8, 8.0-noble-arm64v8, 8.0.416-noble, 8.0-noble | [Dockerfile](src/sdk/8.0/noble/arm64v8/Dockerfile) | Ubuntu 24.04
8.0.416-jammy-arm64v8, 8.0-jammy-arm64v8, 8.0.416-jammy, 8.0-jammy | [Dockerfile](src/sdk/8.0/jammy/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.416-azurelinux3.0-arm64v8, 8.0-azurelinux3.0-arm64v8, 8.0.416-azurelinux3.0, 8.0-azurelinux3.0 | [Dockerfile](src/sdk/8.0/azurelinux3.0/arm64v8/Dockerfile) | Azure Linux 3.0

### Linux arm32 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
10.0.101-noble-arm32v7, 10.0-noble-arm32v7, 10.0.101-noble, 10.0-noble, 10.0.101, 10.0, latest | [Dockerfile](src/sdk/10.0/noble/arm32v7/Dockerfile) | Ubuntu 24.04
10.0.101-alpine3.22-arm32v7, 10.0-alpine3.22-arm32v7, 10.0-alpine-arm32v7, 10.0.101-alpine3.22, 10.0-alpine3.22, 10.0-alpine | [Dockerfile](src/sdk/10.0/alpine3.22/arm32v7/Dockerfile) | Alpine 3.22
10.0.101-alpine3.23-arm32v7, 10.0-alpine3.23-arm32v7, 10.0.101-alpine3.23, 10.0-alpine3.23 | [Dockerfile](src/sdk/10.0/alpine3.23/arm32v7/Dockerfile) | Alpine 3.23
9.0.308-bookworm-slim-arm32v7, 9.0-bookworm-slim-arm32v7, 9.0.308-bookworm-slim, 9.0-bookworm-slim, 9.0.308, 9.0 | [Dockerfile](src/sdk/9.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
9.0.308-alpine3.22-arm32v7, 9.0-alpine3.22-arm32v7, 9.0-alpine-arm32v7, 9.0.308-alpine3.22, 9.0-alpine3.22, 9.0-alpine | [Dockerfile](src/sdk/9.0/alpine3.22/arm32v7/Dockerfile) | Alpine 3.22
9.0.308-alpine3.23-arm32v7, 9.0-alpine3.23-arm32v7, 9.0.308-alpine3.23, 9.0-alpine3.23 | [Dockerfile](src/sdk/9.0/alpine3.23/arm32v7/Dockerfile) | Alpine 3.23
9.0.308-noble-arm32v7, 9.0-noble-arm32v7, 9.0.308-noble, 9.0-noble | [Dockerfile](src/sdk/9.0/noble/arm32v7/Dockerfile) | Ubuntu 24.04
8.0.416-bookworm-slim-arm32v7, 8.0-bookworm-slim-arm32v7, 8.0.416-bookworm-slim, 8.0-bookworm-slim, 8.0.416, 8.0 | [Dockerfile](src/sdk/8.0/bookworm-slim/arm32v7/Dockerfile) | Debian 12
8.0.416-alpine3.22-arm32v7, 8.0-alpine3.22-arm32v7, 8.0-alpine-arm32v7, 8.0.416-alpine3.22, 8.0-alpine3.22, 8.0-alpine | [Dockerfile](src/sdk/8.0/alpine3.22/arm32v7/Dockerfile) | Alpine 3.22
8.0.416-alpine3.23-arm32v7, 8.0-alpine3.23-arm32v7, 8.0.416-alpine3.23, 8.0-alpine3.23 | [Dockerfile](src/sdk/8.0/alpine3.23/arm32v7/Dockerfile) | Alpine 3.23
8.0.416-jammy-arm32v7, 8.0-jammy-arm32v7, 8.0.416-jammy, 8.0-jammy | [Dockerfile](src/sdk/8.0/jammy/arm32v7/Dockerfile) | Ubuntu 22.04

### Nano Server 2025 amd64 Tags

Tag | Dockerfile
---------| ---------------
10.0.101-nanoserver-ltsc2025, 10.0-nanoserver-ltsc2025 | [Dockerfile](src/sdk/10.0/nanoserver-ltsc2025/amd64/Dockerfile)
9.0.308-nanoserver-ltsc2025, 9.0-nanoserver-ltsc2025 | [Dockerfile](src/sdk/9.0/nanoserver-ltsc2025/amd64/Dockerfile)
8.0.416-nanoserver-ltsc2025, 8.0-nanoserver-ltsc2025 | [Dockerfile](src/sdk/8.0/nanoserver-ltsc2025/amd64/Dockerfile)

### Windows Server Core 2025 amd64 Tags

Tag | Dockerfile
---------| ---------------
10.0.101-windowsservercore-ltsc2025, 10.0-windowsservercore-ltsc2025 | [Dockerfile](src/sdk/10.0/windowsservercore-ltsc2025/amd64/Dockerfile)
9.0.308-windowsservercore-ltsc2025, 9.0-windowsservercore-ltsc2025 | [Dockerfile](src/sdk/9.0/windowsservercore-ltsc2025/amd64/Dockerfile)
8.0.416-windowsservercore-ltsc2025, 8.0-windowsservercore-ltsc2025 | [Dockerfile](src/sdk/8.0/windowsservercore-ltsc2025/amd64/Dockerfile)

### Nano Server 2022 amd64 Tags

Tag | Dockerfile
---------| ---------------
10.0.101-nanoserver-ltsc2022, 10.0-nanoserver-ltsc2022 | [Dockerfile](src/sdk/10.0/nanoserver-ltsc2022/amd64/Dockerfile)
9.0.308-nanoserver-ltsc2022, 9.0-nanoserver-ltsc2022 | [Dockerfile](src/sdk/9.0/nanoserver-ltsc2022/amd64/Dockerfile)
8.0.416-nanoserver-ltsc2022, 8.0-nanoserver-ltsc2022 | [Dockerfile](src/sdk/8.0/nanoserver-ltsc2022/amd64/Dockerfile)

### Windows Server Core 2022 amd64 Tags

Tag | Dockerfile
---------| ---------------
10.0.101-windowsservercore-ltsc2022, 10.0-windowsservercore-ltsc2022 | [Dockerfile](src/sdk/10.0/windowsservercore-ltsc2022/amd64/Dockerfile)
9.0.308-windowsservercore-ltsc2022, 9.0-windowsservercore-ltsc2022 | [Dockerfile](src/sdk/9.0/windowsservercore-ltsc2022/amd64/Dockerfile)
8.0.416-windowsservercore-ltsc2022, 8.0-windowsservercore-ltsc2022 | [Dockerfile](src/sdk/8.0/windowsservercore-ltsc2022/amd64/Dockerfile)

### Nano Server, version 1809 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.308-nanoserver-1809, 9.0-nanoserver-1809 | [Dockerfile](src/sdk/9.0/nanoserver-1809/amd64/Dockerfile)
8.0.416-nanoserver-1809, 8.0-nanoserver-1809 | [Dockerfile](src/sdk/8.0/nanoserver-1809/amd64/Dockerfile)

### Windows Server Core 2019 amd64 Tags

Tag | Dockerfile
---------| ---------------
9.0.308-windowsservercore-ltsc2019, 9.0-windowsservercore-ltsc2019 | [Dockerfile](src/sdk/9.0/windowsservercore-ltsc2019/amd64/Dockerfile)
8.0.416-windowsservercore-ltsc2019, 8.0-windowsservercore-ltsc2019 | [Dockerfile](src/sdk/8.0/windowsservercore-ltsc2019/amd64/Dockerfile)
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/dotnet/sdk/tags/list) for all supported and unsupported tags.*

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
