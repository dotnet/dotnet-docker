# Featured Tags

* `8` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor:8`
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
8.0.2-ubuntu-chiseled-amd64, 8.0-ubuntu-chiseled-amd64, 8-ubuntu-chiseled-amd64, 8.0.2-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.2, 8.0, 8, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.2-cbl-mariner-distroless-amd64, 8.0-cbl-mariner-distroless-amd64, 8-cbl-mariner-distroless-amd64, 8.0.2-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
6.3.6-alpine-amd64, 6.3-alpine-amd64, 6-alpine-amd64, 6.3.6-alpine, 6.3-alpine, 6-alpine, 6.3.6, 6.3, 6 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/alpine/amd64/Dockerfile) | Alpine 3.19
6.3.6-ubuntu-chiseled-amd64, 6.3-ubuntu-chiseled-amd64, 6-ubuntu-chiseled-amd64, 6.3.6-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.3.6-cbl-mariner-amd64, 6.3-cbl-mariner-amd64, 6-cbl-mariner-amd64, 6.3.6-cbl-mariner, 6.3-cbl-mariner, 6-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/cbl-mariner/amd64/Dockerfile) | CBL-Mariner 2.0
6.3.6-cbl-mariner-distroless-amd64, 6.3-cbl-mariner-distroless-amd64, 6-cbl-mariner-distroless-amd64, 6.3.6-cbl-mariner-distroless, 6.3-cbl-mariner-distroless, 6-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

##### .NET Monitor Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.5-amd64, 9.0-preview-amd64, 9.0.0-preview.5, 9.0-preview, 9-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/9.0/azurelinux-distroless/amd64/Dockerfile) | Azure Linux 3.0

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.2-ubuntu-chiseled-arm64v8, 8.0-ubuntu-chiseled-arm64v8, 8-ubuntu-chiseled-arm64v8, 8.0.2-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.2, 8.0, 8, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.2-cbl-mariner-distroless-arm64v8, 8.0-cbl-mariner-distroless-arm64v8, 8-cbl-mariner-distroless-arm64v8, 8.0.2-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.3.6-alpine-arm64v8, 6.3-alpine-arm64v8, 6-alpine-arm64v8, 6.3.6-alpine, 6.3-alpine, 6-alpine, 6.3.6, 6.3, 6 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/alpine/arm64v8/Dockerfile) | Alpine 3.19
6.3.6-ubuntu-chiseled-arm64v8, 6.3-ubuntu-chiseled-arm64v8, 6-ubuntu-chiseled-arm64v8, 6.3.6-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.3.6-cbl-mariner-arm64v8, 6.3-cbl-mariner-arm64v8, 6-cbl-mariner-arm64v8, 6.3.6-cbl-mariner, 6.3-cbl-mariner, 6-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/cbl-mariner/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.3.6-cbl-mariner-distroless-arm64v8, 6.3-cbl-mariner-distroless-arm64v8, 6-cbl-mariner-distroless-arm64v8, 6.3.6-cbl-mariner-distroless, 6.3-cbl-mariner-distroless, 6-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/6.3/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

##### .NET Monitor Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.5-arm64v8, 9.0-preview-arm64v8, 9.0.0-preview.5, 9.0-preview, 9-preview | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/9.0/azurelinux-distroless/arm64v8/Dockerfile) | Azure Linux 3.0

You can retrieve a list of all available tags for dotnet/monitor at https://mcr.microsoft.com/v2/dotnet/monitor/tags/list.
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
