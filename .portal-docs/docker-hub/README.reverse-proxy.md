> **Important**: The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).
>
> See [dotnet](https://hub.docker.com/r/microsoft/dotnet-reverse-proxy/) for images with official releases of [.NET](https://github.com/dotnet/core).

# Featured Tags

* `2.3`
  * `docker pull mcr.microsoft.com/dotnet/nightly/reverse-proxy:2.3`

# About

This image contains an implementation of YARP, a reverse proxy framework in .NET.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

You can run this image to launch a YARP instance.

See [Basic YARP Sample](https://github.com/microsoft/reverse-proxy/tree/main/samples/BasicYarpSample) for a sample config to use with this container.

YARP expect the config file to be in `/etc/reverse-proxy.config`, and listen by default on port 5000.

It can be run with this command:

```console
docker run -v $(pwd)/my-config.config:/etc/reverse-proxy.config -p 5000:5000 mcr.microsoft.com/dotnet/reverse-proxy:latest
```

See [documentation](https://microsoft.github.io/reverse-proxy/articles/index.html) for how to configure the image and documentation for the reverse proxy configuration.

The support policy can be found [here](https://github.com/microsoft/reverse-proxy/blob/main/docs/roadmap.md).

# Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

## Distroless images

.NET [distroless container images](https://github.com/dotnet/dotnet-docker/blob/main/documentation/distroless.md) contain only the minimal set of packages .NET needs, with everything else removed.
Due to their limited set of packages, distroless containers have a minimized security attack surface, smaller deployment sizes, and faster start-up time compared to their non-distroless counterparts.
They contain the following features:

* Minimal set of packages required for .NET applications
* Non-root user by default
* No package manager
* No shell

.NET offers distroless images for [Azure Linux](https://github.com/dotnet/dotnet-docker/blob/main/documentation/azurelinux.md) and [Ubuntu (Chiseled)](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md).

# Related Repositories

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

# Full Tag Listing

View the current tags at the [Microsoft Artifact Registry portal](https://mcr.microsoft.com/product/dotnet/nightly/reverse-proxy/tags) or on [GitHub](https://github.com/dotnet/dotnet-docker/blob/nightly/README.reverse-proxy.md#full-tag-listing).

# Support

## Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

## Image Update Policy

* **Base Image Updates:** Images are re-built within 12 hours of any updates to their base images (e.g. debian:bookworm-slim, windows/nanoserver:ltsc2022, etc.).
* **.NET Releases:** Images are re-built as part of releasing new .NET versions. This includes new major versions, minor versions, and servicing releases.
* **Critical CVEs:** Images are re-built to pick up critical CVE fixes as described by the CVE Update Policy below.
* **Monthly Re-builds:** Images are re-built monthly, typically on the second Tuesday of the month, in order to pick up lower-severity CVE fixes.
* **Out-Of-Band Updates:** Images can sometimes be re-built when out-of-band updates are necessary to address critical issues. If this happens, new fixed version tags will be updated according to the [Fixed version tags documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md#fixed-version-tags).

### CVE Update Policy

.NET container images are regularly monitored for the presence of CVEs. A given image will be rebuilt to pick up fixes for a CVE when:

* We detect the image contains a CVE with a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score of "Critical"
* **AND** the CVE is in a package that is added in our Dockerfile layers (meaning the CVE is in a package we explicitly install or any transitive dependencies of those packages)
* **AND** there is a CVE fix for the package available in the affected base image's package repository.

Please refer to the [Security Policy](https://github.com/dotnet/dotnet-docker/blob/main/SECURITY.md) and [Container Vulnerability Workflow](https://github.com/dotnet/dotnet-docker/blob/main/documentation/vulnerability-reporting.md) for more detail about what to do when a CVE is encountered in a .NET image.

## Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server](https://www.microsoft.com/cloud-platform/windows-server-pricing)
