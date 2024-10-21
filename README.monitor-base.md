# .NET Monitor Base

## Featured Tags

* `9` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor/base:9`
* `8` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor/base:8`

## About

This image contains the base installation of .NET Monitor, a diagnostic tool for capturing diagnostic artifacts (such as dumps and traces) in an operator-driven or automated manner. This tool is an ASP.NET application that hosts a web API for inspecting .NET processes and collecting diagnostic artifacts.

This image only provides the base functionality of the .NET Monitor tool; it is only meant to be used as a base image upon which .NET Monitor extensions are installed. If you are looking for the full feature set that is provided by the .NET Monitor global tool (including the egress capabilities), see the [dotnet/monitor](./README.monitor.md) image.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

The following Dockerfiles demonstrate how you can use this base image to build a .NET Monitor image with a custom set of extensions.

* [Ubuntu Chiseled - amd64](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/amd64/Dockerfile)
* [Ubuntu Chiseled - arm64v8](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/arm64v8/Dockerfile)

## Related Repositories

.NET:

* [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.md): .NET
* [dotnet/sdk](https://github.com/dotnet/dotnet-docker/blob/main/README.sdk.md): .NET SDK
* [dotnet/aspnet](https://github.com/dotnet/dotnet-docker/blob/main/README.aspnet.md): ASP.NET Core Runtime
* [dotnet/runtime](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime.md): .NET Runtime
* [dotnet/runtime-deps](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime-deps.md): .NET Runtime Dependencies
* [dotnet/monitor](https://github.com/dotnet/dotnet-docker/blob/main/README.monitor.md): .NET Monitor Tool
* [dotnet/aspire-dashboard](https://github.com/dotnet/dotnet-docker/blob/main/README.aspire-dashboard.md): .NET Aspire Dashboard
* [dotnet/nightly/monitor/base](https://github.com/dotnet/dotnet-docker/blob/nightly/README.monitor-base.md): .NET Monitor Base (Preview)
* [dotnet/samples](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md): .NET Samples

.NET Framework:

* [dotnet/framework](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.md): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.samples.md): .NET Framework, ASP.NET and WCF Samples

## Full Tag Listing

### Linux amd64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-rc.2-amd64, 9.0-amd64, 9.0.0-rc.2, 9.0, 9 | [Dockerfile](src/monitor-base/9.0/azurelinux-distroless/amd64/Dockerfile) | Azure Linux 3.0
8.0.5-ubuntu-chiseled-amd64, 8.0-ubuntu-chiseled-amd64, 8-ubuntu-chiseled-amd64, 8.0.5-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.5, 8.0, 8, latest | [Dockerfile](src/monitor-base/8.0/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.5-cbl-mariner-distroless-amd64, 8.0-cbl-mariner-distroless-amd64, 8-cbl-mariner-distroless-amd64, 8.0.5-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](src/monitor-base/8.0/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

### Linux arm64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-rc.2-arm64v8, 9.0-arm64v8, 9.0.0-rc.2, 9.0, 9 | [Dockerfile](src/monitor-base/9.0/azurelinux-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.5-ubuntu-chiseled-arm64v8, 8.0-ubuntu-chiseled-arm64v8, 8-ubuntu-chiseled-arm64v8, 8.0.5-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.5, 8.0, 8, latest | [Dockerfile](src/monitor-base/8.0/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.5-cbl-mariner-distroless-arm64v8, 8.0-cbl-mariner-distroless-arm64v8, 8-cbl-mariner-distroless-arm64v8, 8.0.5-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](src/monitor-base/8.0/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/dotnet/monitor/base/tags/list) for all supported and unsupported tags.*

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
