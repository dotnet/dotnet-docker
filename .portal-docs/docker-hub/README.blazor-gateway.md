> **Important**: The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).
>
> See [dotnet](https://hub.docker.com/r/microsoft/dotnet/) for images with official releases of [.NET](https://github.com/dotnet/core).

# Featured Tags

* `11.0-preview`
  * `docker pull mcr.microsoft.com/dotnet/nightly/blazor-gateway:11.0-preview`

# About

This image contains the Blazor gateway, a prebuilt reverse-proxy front end for hosting statically-published Blazor Web applications.

The gateway image only contains the gateway runtime. It does not contain any application content. Consumers must layer or mount their published Blazor application's static web assets and endpoints manifest into the image.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Introduction to .NET and Docker](https://learn.microsoft.com/dotnet/core/docker/introduction) to learn more.

You can run this image to launch a Blazor gateway instance in front of one or more statically-published Blazor Web applications.

## Layering application content

This image only contains the gateway runtime at `/app`. It does not contain any Blazor application content. To serve a Blazor application, layer or mount the application's published static web assets and its transformed static-web-assets endpoints manifest on top of this image, then reference that manifest from the `ClientApps` configuration section described below.

## Configuration

The gateway listens by default on port 8080. It is configured entirely through the standard ASP.NET Core configuration system (environment variables, mounted `appsettings.json` files, and so on). The following configuration section families are supported:

* `ClientApps` &mdash; the set of Blazor applications the gateway serves, including each application's endpoints manifest location and path prefix.
* `ReverseProxy` &mdash; standard [YARP](https://aka.ms/YarpDocumentation) reverse-proxy routes and clusters, used to proxy requests to backend APIs alongside the Blazor application(s).
* Service discovery &mdash; the gateway uses [.NET service discovery](https://learn.microsoft.com/dotnet/core/extensions/service-discovery) to resolve destination addresses for reverse-proxy clusters.
* `Gateway` &mdash; gateway-level settings.

For example, the following command mounts a published Blazor application's `wwwroot` directory and endpoints manifest, then maps the application at the root path:

```bash
docker run --rm -p 8080:8080 \
  -v $(pwd)/myapp/wwwroot:/app/wwwroot:ro \
  -v $(pwd)/myapp/MyApp.staticwebassets.endpoints.json:/app/myapp.endpoints.json:ro \
  -e ClientApps__myapp__EndpointsManifest=/app/myapp.endpoints.json \
  -e ClientApps__myapp__PathPrefix= \
  mcr.microsoft.com/dotnet/nightly/blazor-gateway:11.0-preview
```

## Health checks

This image exposes `/alive` as its liveness endpoint. `/alive` is the endpoint to use for container/orchestrator health probes in Production; it is available in every environment. `/health`, which reports more detailed health information, is only available when `ASPNETCORE_ENVIRONMENT` is set to `Development` and should not be assumed to be present otherwise.

## OpenTelemetry support

This image supports OpenTelemetry. It can be configured by passing environment variables to the container:

```bash
docker run --rm -p 8080:8080 -e OTEL_EXPORTER_OTLP_ENDPOINT=https://otlp-endpoint.internal:4317 mcr.microsoft.com/dotnet/nightly/blazor-gateway:11.0-preview
```

See the [OTLP Exporter Configuration](https://opentelemetry.io/docs/languages/sdk-configuration/otlp-exporter/) for all supported environment variables.

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/r/microsoft/dotnet/): .NET
* [dotnet/nightly/sdk](https://hub.docker.com/r/microsoft/dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/r/microsoft/dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/r/microsoft/dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/r/microsoft/dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor](https://hub.docker.com/r/microsoft/dotnet-nightly-monitor/): .NET Monitor Tool (Preview)
* [dotnet/nightly/aspire-dashboard](https://hub.docker.com/r/microsoft/dotnet-nightly-aspire-dashboard/): Aspire Dashboard (Preview)
* [dotnet/nightly/yarp](https://hub.docker.com/r/microsoft/dotnet-nightly-yarp/): YARP (Yet Another Reverse Proxy) (Preview)
* [dotnet/samples](https://hub.docker.com/r/microsoft/dotnet-samples/): .NET Samples

.NET Framework:

* [dotnet/framework](https://hub.docker.com/r/microsoft/dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

View the current tags at the [Microsoft Artifact Registry portal](https://mcr.microsoft.com/artifact/mar/dotnet/nightly/blazor-gateway/tags) or on [GitHub](https://github.com/dotnet/dotnet-docker/blob/nightly/README.blazor-gateway.md#full-tag-listing).

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
