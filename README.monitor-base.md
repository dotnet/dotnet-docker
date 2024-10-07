# .NET Monitor Base

> **Important**: The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).
>
> See [dotnet](https://hub.docker.com/r/microsoft/dotnet-monitor-base/) for images with official releases of [.NET](https://github.com/dotnet/core).

## Featured Tags

* `9` (Release Candidate)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor/base:9`
* `8` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor/base:8`

## About

This image contains the .NET Monitor Base installation.

Use this image as a base image for building a .NET Monitor image with extensions.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

### New: Ubuntu Chiseled Images

Ubuntu Chiseled .NET images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface by including only the minimal set of packages required to run .NET applications.

Please see the [Ubuntu Chiseled + .NET](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md) documentation page for more info.

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Building a Custom .NET Monitor Image

The following Dockerfiles demonstrate how you can use this base image to build a .NET Monitor image with a custom set of extensions.

* [Ubuntu Chiseled - amd64](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/amd64/Dockerfile)
* [Ubuntu Chiseled - arm64v8](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/arm64v8/Dockerfile)

## Related Repositories

.NET:

* [dotnet](https://hub.docker.com/r/microsoft/dotnet/): .NET
* [dotnet/nightly/sdk](https://hub.docker.com/r/microsoft/dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/r/microsoft/dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/r/microsoft/dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/r/microsoft/dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor](https://hub.docker.com/r/microsoft/dotnet-nightly-monitor/): .NET Monitor Tool (Preview)
* [dotnet/nightly/aspire-dashboard](https://hub.docker.com/r/microsoft/dotnet-nightly-aspire-dashboard/): .NET Aspire Dashboard (Preview)
* [dotnet/samples](https://hub.docker.com/r/microsoft/dotnet-samples/): .NET Samples

.NET Framework:

* [dotnet/framework](https://hub.docker.com/r/microsoft/dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

## Full Tag Listing

### Linux amd64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-rc.2-amd64, 9.0-amd64, 9.0.0-rc.2, 9.0, 9, latest | [Dockerfile](src/monitor-base/9.0/azurelinux-distroless/amd64/Dockerfile) | Azure Linux 3.0
8.0.5-ubuntu-chiseled-amd64, 8.0-ubuntu-chiseled-amd64, 8.0.5-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8.0.5, 8.0 | [Dockerfile](src/monitor-base/8.0/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.5-cbl-mariner-distroless-amd64, 8.0-cbl-mariner-distroless-amd64, 8.0.5-cbl-mariner-distroless, 8.0-cbl-mariner-distroless | [Dockerfile](src/monitor-base/8.0/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

#### .NET Monitor Preview Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.1.0-alpha.1-ubuntu-chiseled-amd64, 8.1-preview-ubuntu-chiseled-amd64, 8-ubuntu-chiseled-amd64, 8.1.0-alpha.1-ubuntu-chiseled, 8.1-preview-ubuntu-chiseled, 8-ubuntu-chiseled, 8.1.0-alpha.1, 8.1-preview, 8 | [Dockerfile](src/monitor-base/8.1/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.1.0-alpha.1-cbl-mariner-distroless-amd64, 8.1-preview-cbl-mariner-distroless-amd64, 8-cbl-mariner-distroless-amd64, 8.1.0-alpha.1-cbl-mariner-distroless, 8.1-preview-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](src/monitor-base/8.1/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

### Linux arm64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-rc.2-arm64v8, 9.0-arm64v8, 9.0.0-rc.2, 9.0, 9, latest | [Dockerfile](src/monitor-base/9.0/azurelinux-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.5-ubuntu-chiseled-arm64v8, 8.0-ubuntu-chiseled-arm64v8, 8.0.5-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8.0.5, 8.0 | [Dockerfile](src/monitor-base/8.0/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.5-cbl-mariner-distroless-arm64v8, 8.0-cbl-mariner-distroless-arm64v8, 8.0.5-cbl-mariner-distroless, 8.0-cbl-mariner-distroless | [Dockerfile](src/monitor-base/8.0/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0

#### .NET Monitor Preview Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
8.1.0-alpha.1-ubuntu-chiseled-arm64v8, 8.1-preview-ubuntu-chiseled-arm64v8, 8-ubuntu-chiseled-arm64v8, 8.1.0-alpha.1-ubuntu-chiseled, 8.1-preview-ubuntu-chiseled, 8-ubuntu-chiseled, 8.1.0-alpha.1, 8.1-preview, 8 | [Dockerfile](src/monitor-base/8.1/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.1.0-alpha.1-cbl-mariner-distroless-arm64v8, 8.1-preview-cbl-mariner-distroless-arm64v8, 8-cbl-mariner-distroless-arm64v8, 8.1.0-alpha.1-cbl-mariner-distroless, 8.1-preview-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](src/monitor-base/8.1/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/dotnet/nightly/monitor/base/tags/list) for all supported and unsupported tags.*

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
