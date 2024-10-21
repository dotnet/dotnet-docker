# .NET Monitor Tool

## Featured Tags

* `9` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor:9`
* `8` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor:8`
* `6` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor:6`

## About

This image contains .NET Monitor, a diagnostic tool for capturing diagnostic artifacts (such as dumps and traces) in an operator-driven or automated manner. This tool is an ASP.NET application that hosts a web API for inspecting .NET processes and collecting diagnostic artifacts. This image also contains .NET Monitor extensions for egressing artifacts to Azure Blob Storage and Amazon S3.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

You can run this image as a sidecar container to collect diagnostic information and artifacts from other containers running .NET Core 3.1 or .NET 5 and later.

See [Running in Kubernetes](https://github.com/dotnet/dotnet-monitor/blob/main/documentation/kubernetes.md) or [Running in Docker Compose](https://github.com/dotnet/dotnet-monitor/blob/main/documentation/docker-compose.md) for examples of how to run this image in orchestration environments.

See [documentation](https://go.microsoft.com/fwlink/?linkid=2158052) for how to configure the image and documentation for the web API.

## Related Repositories

.NET:

* [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.md): .NET
* [dotnet/sdk](https://github.com/dotnet/dotnet-docker/blob/main/README.sdk.md): .NET SDK
* [dotnet/aspnet](https://github.com/dotnet/dotnet-docker/blob/main/README.aspnet.md): ASP.NET Core Runtime
* [dotnet/runtime](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime.md): .NET Runtime
* [dotnet/runtime-deps](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime-deps.md): .NET Runtime Dependencies
* [dotnet/monitor/base](https://github.com/dotnet/dotnet-docker/blob/main/README.monitor-base.md): .NET Monitor Base
* [dotnet/aspire-dashboard](https://github.com/dotnet/dotnet-docker/blob/main/README.aspire-dashboard.md): .NET Aspire Dashboard
* [dotnet/nightly/monitor](https://github.com/dotnet/dotnet-docker/blob/nightly/README.monitor.md): .NET Monitor Tool (Preview)
* [dotnet/samples](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md): .NET Samples

.NET Framework:

* [dotnet/framework](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.md): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.samples.md): .NET Framework, ASP.NET and WCF Samples

## Full Tag Listing

### Linux amd64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-rc.2, 9.0, 9 | [Dockerfile](src/monitor/9.0/azurelinux-distroless/amd64/Dockerfile) | Azure Linux 3.0
8.0.5-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.5, 8.0, 8, latest | [Dockerfile](src/monitor/8.0/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
8.0.5-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](src/monitor/8.0/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0
6.3.9-alpine, 6.3-alpine, 6-alpine, 6.3.9, 6.3, 6 | [Dockerfile](src/monitor/6.3/alpine/amd64/Dockerfile) | Alpine 3.20
6.3.9-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](src/monitor/6.3/ubuntu-chiseled/amd64/Dockerfile) | Ubuntu 22.04
6.3.9-cbl-mariner, 6.3-cbl-mariner, 6-cbl-mariner | [Dockerfile](src/monitor/6.3/cbl-mariner/amd64/Dockerfile) | CBL-Mariner 2.0
6.3.9-cbl-mariner-distroless, 6.3-cbl-mariner-distroless, 6-cbl-mariner-distroless | [Dockerfile](src/monitor/6.3/cbl-mariner-distroless/amd64/Dockerfile) | CBL-Mariner 2.0

### Linux arm64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
9.0.0-rc.2, 9.0, 9 | [Dockerfile](src/monitor/9.0/azurelinux-distroless/arm64v8/Dockerfile) | Azure Linux 3.0
8.0.5-ubuntu-chiseled, 8.0-ubuntu-chiseled, 8-ubuntu-chiseled, 8.0.5, 8.0, 8, latest | [Dockerfile](src/monitor/8.0/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
8.0.5-cbl-mariner-distroless, 8.0-cbl-mariner-distroless, 8-cbl-mariner-distroless | [Dockerfile](src/monitor/8.0/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.3.9-alpine, 6.3-alpine, 6-alpine, 6.3.9, 6.3, 6 | [Dockerfile](src/monitor/6.3/alpine/arm64v8/Dockerfile) | Alpine 3.20
6.3.9-ubuntu-chiseled, 6.3-ubuntu-chiseled, 6-ubuntu-chiseled | [Dockerfile](src/monitor/6.3/ubuntu-chiseled/arm64v8/Dockerfile) | Ubuntu 22.04
6.3.9-cbl-mariner, 6.3-cbl-mariner, 6-cbl-mariner | [Dockerfile](src/monitor/6.3/cbl-mariner/arm64v8/Dockerfile) | CBL-Mariner 2.0
6.3.9-cbl-mariner-distroless, 6.3-cbl-mariner-distroless, 6-cbl-mariner-distroless | [Dockerfile](src/monitor/6.3/cbl-mariner-distroless/arm64v8/Dockerfile) | CBL-Mariner 2.0
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/dotnet/monitor/tags/list) for all supported and unsupported tags.*

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
