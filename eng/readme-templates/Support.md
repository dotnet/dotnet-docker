{{
    _ ARGS:
      top-header: The string to use as the top-level header. ^

    set isRuntimeDeps to match(SHORT_REPO, "runtime-deps") ^
    set isSamples to match(SHORT_REPO, "samples") ^
    set isMonitor to match(SHORT_REPO, "monitor") || match(SHORT_REPO, "base") ^
    set isAspireDashboard to match(SHORT_REPO, "aspire-dashboard")

}}{{ARGS["top-header"]}} Support

{{if isSamples:These sample images are not intended for production use and may be subject to breaking changes or removal at any time. They are provided as a starting point for developers to experiment with and learn about .NET in a containerized environment.

}}{{ARGS["top-header"]}}# Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

{{ARGS["top-header"]}}# Image Update Policy

* We update supported .NET images within 12 hours of any updates to their base images (e.g. debian:bookworm-slim, windows/nanoserver:ltsc2022, etc.).
* We re-build all .NET images as part of releasing new versions of .NET including new major/minor versions and servicing.
* Distroless images such as Ubuntu Chiseled have no base image, and as such will only be updated with .NET releases and CVE fixes as described below.

{{ARGS["top-header"]}}## CVE Update Policy

.NET container images are regularly monitored for the presence of CVEs. A given image will be rebuilt to pick up fixes for a CVE when:
* We detect the image contains a CVE with a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score of "Critical"
* **AND** the CVE is in a package that is added in our Dockerfile layers (meaning the CVE is in a package we explicitly install or any transitive dependencies of those packages)
* **AND** there is a CVE fix for the package available in the affected base image's package repository.

{{ARGS["top-header"]}}# Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

{{ARGS["top-header"]}} License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md){{if !(isRuntimeDeps || isMonitor || isAspireDashboard):
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server](https://www.microsoft.com/cloud-platform/windows-server-pricing)}}
