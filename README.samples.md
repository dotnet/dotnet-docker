# .NET Samples

## Featured Tags

* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp`
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp-chiseled`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp`
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp-chiseled`

## About

These images contain sample .NET and ASP.NET Core applications.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Container sample: Run a simple application

You can quickly run a container with a pre-built [.NET Docker image](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md), based on the [.NET console sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

### Container sample: Run a web application

You can quickly run a container with a pre-built [.NET Docker image](https://github.com/dotnet/dotnet-docker/blob/main/README.samples.md), based on the [ASP.NET Core sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md).

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:8080 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. You can also view the ASP.NET Core site running in the container from another machine with a local IP address such as `http://192.168.1.18:8000`.

> Note: ASP.NET Core apps (in official images) listen to [port 8080 by default](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7), starting with .NET 8. The [`-p` argument](https://docs.docker.com/engine/reference/commandline/run/#publish) in these examples maps host port `8000` to container port `8080` (`host:container` mapping). The container will not be accessible without this mapping. ASP.NET Core can be [configured to listen on a different or additional port](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel/endpoints).

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md) to use HTTPS with this image.

## Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

### Distroless images

.NET "distroless" container images contain only the minimal set of packages .NET needs, with everything else removed.
Due to their limited set of packages, distroless containers have a minimized security attack surface, smaller deployment sizes, and faster start-up time compared to their non-distroless counterparts.
They contain the following features:

* Minimal set of packages required for .NET applications
* Non-root user by default
* No package manager
* No shell

.NET offers distroless images for [Azure Linux](https://github.com/dotnet/dotnet-docker/blob/main/documentation/azurelinux.md) and [Ubuntu (Chiseled)](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md).

## Related Repositories

.NET:

* [dotnet](https://github.com/dotnet/dotnet-docker/blob/main/README.md): .NET
* [dotnet/sdk](https://github.com/dotnet/dotnet-docker/blob/main/README.sdk.md): .NET SDK
* [dotnet/aspnet](https://github.com/dotnet/dotnet-docker/blob/main/README.aspnet.md): ASP.NET Core Runtime
* [dotnet/runtime](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime.md): .NET Runtime
* [dotnet/runtime-deps](https://github.com/dotnet/dotnet-docker/blob/main/README.runtime-deps.md): .NET Runtime Dependencies
* [dotnet/monitor](https://github.com/dotnet/dotnet-docker/blob/main/README.monitor.md): .NET Monitor Tool
* [dotnet/aspire-dashboard](https://github.com/dotnet/dotnet-docker/blob/main/README.aspire-dashboard.md): .NET Aspire Dashboard

.NET Framework:

* [dotnet/framework](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.md): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/README.samples.md): .NET Framework, ASP.NET and WCF Samples

## Full Tag Listing

### Linux amd64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-9.0-alpine-amd64, dotnetapp-9.0 | [Dockerfile](samples/dotnetapp/Dockerfile.alpine) | Alpine
dotnetapp-chiseled-9.0-amd64, dotnetapp-chiseled-9.0 | [Dockerfile](samples/dotnetapp/Dockerfile.chiseled) | Ubuntu
aspnetapp-9.0-alpine-amd64, aspnetapp-9.0 | [Dockerfile](samples/aspnetapp/Dockerfile.alpine) | Alpine
aspnetapp-chiseled-9.0-amd64, aspnetapp-chiseled-9.0 | [Dockerfile](samples/aspnetapp/Dockerfile.chiseled) | Ubuntu
dotnetapp-8.0-alpine-amd64, dotnetapp-alpine-amd64, dotnetapp-8.0, dotnetapp, latest | [Dockerfile](samples/8.0/dotnetapp/Dockerfile.alpine) | Alpine
dotnetapp-chiseled-8.0-amd64, dotnetapp-chiseled-amd64, dotnetapp-chiseled-8.0, dotnetapp-chiseled | [Dockerfile](samples/8.0/dotnetapp/Dockerfile.chiseled) | Ubuntu
aspnetapp-8.0-alpine-amd64, aspnetapp-alpine-amd64, aspnetapp-8.0, aspnetapp | [Dockerfile](samples/8.0/aspnetapp/Dockerfile.alpine) | Alpine
aspnetapp-chiseled-8.0-amd64, aspnetapp-chiseled-amd64, aspnetapp-chiseled-8.0, aspnetapp-chiseled | [Dockerfile](samples/8.0/aspnetapp/Dockerfile.chiseled) | Ubuntu

### Linux arm64 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-9.0-alpine-arm64v8, dotnetapp-9.0 | [Dockerfile](samples/dotnetapp/Dockerfile.alpine) | Alpine
dotnetapp-chiseled-9.0-arm64v8, dotnetapp-chiseled-9.0 | [Dockerfile](samples/dotnetapp/Dockerfile.chiseled) | Ubuntu
aspnetapp-9.0-alpine-arm64v8, aspnetapp-9.0 | [Dockerfile](samples/aspnetapp/Dockerfile.alpine) | Alpine
aspnetapp-chiseled-9.0-arm64v8, aspnetapp-chiseled-9.0 | [Dockerfile](samples/aspnetapp/Dockerfile.chiseled) | Ubuntu
dotnetapp-8.0-alpine-arm64v8, dotnetapp-alpine-arm64v8, dotnetapp-8.0, dotnetapp, latest | [Dockerfile](samples/8.0/dotnetapp/Dockerfile.alpine) | Alpine
dotnetapp-chiseled-8.0-arm64v8, dotnetapp-chiseled-arm64v8, dotnetapp-chiseled-8.0, dotnetapp-chiseled | [Dockerfile](samples/8.0/dotnetapp/Dockerfile.chiseled) | Ubuntu
aspnetapp-8.0-alpine-arm64v8, aspnetapp-alpine-arm64v8, aspnetapp-8.0, aspnetapp | [Dockerfile](samples/8.0/aspnetapp/Dockerfile.alpine) | Alpine
aspnetapp-chiseled-8.0-arm64v8, aspnetapp-chiseled-arm64v8, aspnetapp-chiseled-8.0, aspnetapp-chiseled | [Dockerfile](samples/8.0/aspnetapp/Dockerfile.chiseled) | Ubuntu

### Linux arm32 Tags

Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-9.0-alpine-arm32v7, dotnetapp-9.0 | [Dockerfile](samples/dotnetapp/Dockerfile.alpine) | Alpine
dotnetapp-chiseled-9.0-arm32v7, dotnetapp-chiseled-9.0 | [Dockerfile](samples/dotnetapp/Dockerfile.chiseled) | Ubuntu
aspnetapp-9.0-alpine-arm32v7, aspnetapp-9.0 | [Dockerfile](samples/aspnetapp/Dockerfile.alpine) | Alpine
aspnetapp-chiseled-9.0-arm32v7, aspnetapp-chiseled-9.0 | [Dockerfile](samples/aspnetapp/Dockerfile.chiseled) | Ubuntu
dotnetapp-8.0-alpine-arm32v7, dotnetapp-alpine-arm32v7, dotnetapp-8.0, dotnetapp, latest | [Dockerfile](samples/8.0/dotnetapp/Dockerfile.alpine) | Alpine
dotnetapp-chiseled-8.0-arm32v7, dotnetapp-chiseled-arm32v7, dotnetapp-chiseled-8.0, dotnetapp-chiseled | [Dockerfile](samples/8.0/dotnetapp/Dockerfile.chiseled) | Ubuntu
aspnetapp-8.0-alpine-arm32v7, aspnetapp-alpine-arm32v7, aspnetapp-8.0, aspnetapp | [Dockerfile](samples/8.0/aspnetapp/Dockerfile.alpine) | Alpine
aspnetapp-chiseled-8.0-arm32v7, aspnetapp-chiseled-arm32v7, aspnetapp-chiseled-8.0, aspnetapp-chiseled | [Dockerfile](samples/8.0/aspnetapp/Dockerfile.chiseled) | Ubuntu

### Nano Server 2022 amd64 Tags

Tag | Dockerfile
---------| ---------------
dotnetapp-9.0-nanoserver-ltsc2022, dotnetapp-9.0 | [Dockerfile](samples/dotnetapp/Dockerfile.nanoserver)
aspnetapp-9.0-nanoserver-ltsc2022, aspnetapp-9.0 | [Dockerfile](samples/aspnetapp/Dockerfile.nanoserver)
dotnetapp-8.0-nanoserver-ltsc2022, dotnetapp-nanoserver-ltsc2022, dotnetapp-8.0, dotnetapp, latest | [Dockerfile](samples/8.0/dotnetapp/Dockerfile.nanoserver)
aspnetapp-8.0-nanoserver-ltsc2022, aspnetapp-nanoserver-ltsc2022, aspnetapp-8.0, aspnetapp | [Dockerfile](samples/8.0/aspnetapp/Dockerfile.nanoserver)

### Nano Server, version 1809 amd64 Tags

Tag | Dockerfile
---------| ---------------
dotnetapp-9.0-nanoserver-1809, dotnetapp-9.0 | [Dockerfile](samples/dotnetapp/Dockerfile.nanoserver)
aspnetapp-9.0-nanoserver-1809, aspnetapp-9.0 | [Dockerfile](samples/aspnetapp/Dockerfile.nanoserver)
dotnetapp-8.0-nanoserver-1809, dotnetapp-nanoserver-1809, dotnetapp-8.0, dotnetapp, latest | [Dockerfile](samples/8.0/dotnetapp/Dockerfile.nanoserver)
aspnetapp-8.0-nanoserver-1809, aspnetapp-nanoserver-1809, aspnetapp-8.0, aspnetapp | [Dockerfile](samples/8.0/aspnetapp/Dockerfile.nanoserver)
<!--End of generated tags-->

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/dotnet/samples/tags/list) for all supported and unsupported tags.*

## Support

These sample images are not intended for production use and may be subject to breaking changes or removal at any time. They are provided as a starting point for developers to experiment with and learn about .NET in a containerized environment.

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
