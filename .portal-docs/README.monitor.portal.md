## About

This image contains the .NET Monitor tool.

Use this image as a sidecar container to collect diagnostic information from other containers running .NET Core 3.1 or later processes.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

### New: Ubuntu Chiseled Images

Ubuntu Chiseled .NET images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface by including only the minimal set of packages required to run .NET applications.

Please see the [Ubuntu Chiseled + .NET](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md) documentation page for more info.

## Featured Tags

* `8` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor:8`
* `6` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/monitor:6`

## Related Repositories

.NET:

* [dotnet/sdk](https://mcr.microsoft.com/product/dotnet/sdk/about): .NET SDK
* [dotnet/aspnet](https://mcr.microsoft.com/product/dotnet/aspnet/about): ASP.NET Core Runtime
* [dotnet/runtime](https://mcr.microsoft.com/product/dotnet/runtime/about): .NET Runtime
* [dotnet/runtime-deps](https://mcr.microsoft.com/product/dotnet/runtime-deps/about): .NET Runtime Dependencies
* [dotnet/monitor/base](https://mcr.microsoft.com/product/dotnet/monitor/base/about): .NET Monitor Base
* [dotnet/aspire-dashboard](https://mcr.microsoft.com/product/dotnet/aspire-dashboard/about): .NET Aspire Dashboard
* [dotnet/samples](https://mcr.microsoft.com/product/dotnet/samples/about): .NET Samples
* [dotnet/nightly/monitor](https://mcr.microsoft.com/product/dotnet/nightly/monitor/about): .NET Monitor Tool (Preview)

.NET Framework:

* [dotnet/framework](https://mcr.microsoft.com/catalog?search=dotnet/framework): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://mcr.microsoft.com/product/dotnet/framework/samples/about): .NET Framework, ASP.NET and WCF Samples

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Container sample: Run the tool

You can run a container with a pre-built [.NET Docker Image](https://mcr.microsoft.com/product/dotnet/monitor/about), based on the dotnet-monitor global tool.

See the [documentation](https://go.microsoft.com/fwlink/?linkid=2158052) for how to configure the image to be run in a Docker or Kubernetes environment, including how to configure authentication and certificates for https bindings.

## Support

### Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

### Image Update Policy

* We update supported .NET images within 12 hours of any updates to their base images (e.g. debian:bookworm-slim, windows/nanoserver:ltsc2022, etc.).
* We re-build all .NET images as part of releasing new versions of .NET including new major/minor versions and servicing.
* Distroless images such as Ubuntu Chiseled have no base image, and as such will only be updated with .NET releases and CVE fixes as described below.

#### CVE Update Policy

.NET container images are regularly monitored for the presence of CVEs. A given image will be rebuilt to pick up fixes for a CVE when:
* We detect the image contains a CVE with a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score of "Critical"
* **AND** the CVE is in a package that is added in our Dockerfile layers (meaning the CVE is in a package we explicitly install or any transitive dependencies of those packages)
* **AND** there is a CVE fix for the package available in the affected base image's package repository.

### Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

## License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
