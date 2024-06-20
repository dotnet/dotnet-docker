**IMPORTANT**

**The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet](https://hub.docker.com/_/microsoft-dotnet-monitor/) for images with official releases of [.NET](https://github.com/dotnet/core).**

# Featured Tags

* `8` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:8`
* `6` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:6`

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

* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor/base](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor-base/): .NET Monitor Base (Preview)
* [dotnet/nightly/aspire-dashboard](https://hub.docker.com/_/microsoft-dotnet-nightly-aspire-dashboard/): .NET Aspire Dashboard (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.2-ubuntu-chiseled-amd64, 8.0-ubuntu-chiseled-amd64, 8.0.2-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8.0.2, 8.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/8.0/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.2-cbl-mariner-distroless-amd64, 8.0-cbl-mariner-distroless-amd64, 8.0.2-cbl-mariner-distroless, 8.0-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/8.0/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
6.3.6-alpine-amd64, 6.3-alpine-amd64, 6-alpine-amd64, 6.3.6-alpine, 6.3-alpine, 6-alpine, 6.3.6, 6.3, 6 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/alpine/amd64/Dockerfile) | Alpine 3.19
6.3.6-ubuntu-chiseled-amd64, 6.3-ubuntu-chiseled-amd64, 6-ubuntu-chiseled-amd64, 6.3.6-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.3.6-cbl-mariner-amd64, 6.3-cbl-mariner-amd64, 6-cbl-mariner-amd64, 6.3.6-cbl-mariner, 6.3-cbl-mariner, 6-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/cbl-mariner/amd64/Dockerfile) | CBL-Mariner 2.0
6.3.6-cbl-mariner-distroless-amd64, 6.3-cbl-mariner-distroless-amd64, 6-cbl-mariner-distroless-amd64, 6.3.6-cbl-mariner-distroless, 6.3-cbl-mariner-distroless, 6-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

##### .NET Monitor Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.6-amd64, 9.0-preview-amd64, 9.0.0-preview.6, 9.0-preview, 9-preview, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/9.0/azurelinux-distroless/amd64/Dockerfile) | Azure Linux 3.0
8.1.0-alpha.1-ubuntu-chiseled-amd64, 8.1-preview-ubuntu-chiseled-amd64, 8-ubuntu-chiseled-amd64, 8.1.0-alpha.1-ubuntu-chiseled, 8.1-preview-ubuntu-chiseled, 8-ubuntu-chiseled, 8.1.0-alpha.1, 8.1-preview, 8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/8.1/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.1.0-alpha.1-cbl-mariner-distroless-amd64, 8.1-preview-cbl-mariner-distroless-amd64, 8-cbl-mariner-distroless-amd64, 8.1.0-alpha.1-cbl-mariner-distroless, 8.1-preview-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/8.1/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.0.2-ubuntu-chiseled-arm64v8, 8.0-ubuntu-chiseled-arm64v8, 8.0.2-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8.0.2, 8.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/8.0/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.2-cbl-mariner-distroless-arm64v8, 8.0-cbl-mariner-distroless-arm64v8, 8.0.2-cbl-mariner-distroless, 8.0-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/8.0/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.3.6-alpine-arm64v8, 6.3-alpine-arm64v8, 6-alpine-arm64v8, 6.3.6-alpine, 6.3-alpine, 6-alpine, 6.3.6, 6.3, 6 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/alpine/arm64v8/Dockerfile) | Alpine 3.19
6.3.6-ubuntu-chiseled-arm64v8, 6.3-ubuntu-chiseled-arm64v8, 6-ubuntu-chiseled-arm64v8, 6.3.6-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.3.6-cbl-mariner-arm64v8, 6.3-cbl-mariner-arm64v8, 6-cbl-mariner-arm64v8, 6.3.6-cbl-mariner, 6.3-cbl-mariner, 6-cbl-mariner | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/cbl-mariner/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.3.6-cbl-mariner-distroless-arm64v8, 6.3-cbl-mariner-distroless-arm64v8, 6-cbl-mariner-distroless-arm64v8, 6.3.6-cbl-mariner-distroless, 6.3-cbl-mariner-distroless, 6-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/6.3/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

##### .NET Monitor Preview Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-preview.6-arm64v8, 9.0-preview-arm64v8, 9.0.0-preview.6, 9.0-preview, 9-preview, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/9.0/azurelinux-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
8.1.0-alpha.1-ubuntu-chiseled-arm64v8, 8.1-preview-ubuntu-chiseled-arm64v8, 8-ubuntu-chiseled-arm64v8, 8.1.0-alpha.1-ubuntu-chiseled, 8.1-preview-ubuntu-chiseled, 8-ubuntu-chiseled, 8.1.0-alpha.1, 8.1-preview, 8 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/8.1/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.1.0-alpha.1-cbl-mariner-distroless-arm64v8, 8.1-preview-cbl-mariner-distroless-arm64v8, 8-cbl-mariner-distroless-arm64v8, 8.1.0-alpha.1-cbl-mariner-distroless, 8.1-preview-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/8.1/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

You can retrieve a list of all available tags for dotnet/nightly/monitor at https://mcr.microsoft.com/v2/dotnet/nightly/monitor/tags/list.
<!--End of generated tags-->
es from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet](https://hub.docker.com/_/microsoft-dotnet-monitor/) for images with official releases of [.NET](https://github.com/dotnet/core).**

# Featured Tags

* `8` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:8`
* `6` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:6`

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

* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor/base](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor-base/): .NET Monitor Base (Preview)
* [dotnet/nightly/aspire-dashboard](https://hub.docker.com/_/microsoft-dotnet-nightly-aspire-dashboard/): .NET Aspire Dashboard (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

View the current tags at the [Microsoft Artifact Registry portal](https://mcr.microsoft.com/product/$dotnet/nightly/monitor/tags) or on [Docker Hub](https://github.com/dotnet/dotnet-docker/blob/nightly).

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