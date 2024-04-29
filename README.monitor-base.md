# Featured Tags

* `8` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor/base:8`

# About

This image contains the .NET Monitor Base installation.

Use this image as a base image for building a .NET Monitor image with extensions.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## New: Ubuntu Chiseled Images

Ubuntu Chiseled .NET images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface by including only the minimal set of packages required to run .NET applications.

Please see the [Ubuntu Chiseled + .NET](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md) documentation page for more info.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Building a Custom .NET Monitor Image

The following Dockerfiles demonstrate how you can use this base image to build a .NET Monitor image with a custom set of extensions.

* [Ubuntu Chiseled - amd64](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/amd64/Dockerfile)
* [Ubuntu Chiseled - arm64v8](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/arm64v8/Dockerfile)

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.1-ubuntu-chiseled-amd64, 8.0-ubuntu-chiseled-amd64, 8-ubuntu-chiseled-amd64, 8.0.1-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.1, 8.0, 8, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor-base/8.0/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.1-cbl-mariner-distroless-amd64, 8.0-cbl-mariner-distroless-amd64, 8-cbl-mariner-distroless-amd64, 8.0.1-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor-base/8.0/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

##### .NET Monitor Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.3-ubuntu-chiseled-amd64, 9.0-preview-ubuntu-chiseled-amd64, 9.0.0-preview.3-ubuntu-chiseled, 9.0-preview-ubuntu-chiseled, 9.0.0-preview.3, 9.0-preview, 9-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor-base/9.0/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-cbl-mariner-distroless-amd64, 9.0-preview-cbl-mariner-distroless-amd64, 9.0.0-preview.3-cbl-mariner-distroless, 9.0-preview-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor-base/9.0/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.1-ubuntu-chiseled-arm64v8, 8.0-ubuntu-chiseled-arm64v8, 8-ubuntu-chiseled-arm64v8, 8.0.1-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.1, 8.0, 8, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor-base/8.0/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.1-cbl-mariner-distroless-arm64v8, 8.0-cbl-mariner-distroless-arm64v8, 8-cbl-mariner-distroless-arm64v8, 8.0.1-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor-base/8.0/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

##### .NET Monitor Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.3-ubuntu-chiseled-arm64v8, 9.0-preview-ubuntu-chiseled-arm64v8, 9.0.0-preview.3-ubuntu-chiseled, 9.0-preview-ubuntu-chiseled, 9.0.0-preview.3, 9.0-preview, 9-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor-base/9.0/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
9.0.0-preview.3-cbl-mariner-distroless-arm64v8, 9.0-preview-cbl-mariner-distroless-arm64v8, 9.0.0-preview.3-cbl-mariner-distroless, 9.0-preview-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor-base/9.0/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

You can retrieve a list of all available tags for dotnet/monitor/base at https://mcr.microsoft.com/v2/dotnet/monitor/base/tags/list.
<!--End of generated tags-->

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
