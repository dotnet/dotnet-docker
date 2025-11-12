## About

> **Important**: The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).
>
> See [dotnet](https://mcr.microsoft.com/catalog?search=dotnet) for images with official releases of [.NET](https://github.com/dotnet/core).

This image contains an implementation of YARP, a reverse proxy framework in .NET.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Featured Tags

* `2.3-preview`
  * `docker pull mcr.microsoft.com/dotnet/nightly/yarp:2.3-preview`

## Related Repositories

.NET:

* [dotnet](https://mcr.microsoft.com/catalog?search=dotnet): .NET
* [dotnet/nightly/sdk](https://mcr.microsoft.com/product/dotnet/nightly/sdk/about): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://mcr.microsoft.com/product/dotnet/nightly/aspnet/about): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://mcr.microsoft.com/product/dotnet/nightly/runtime/about): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://mcr.microsoft.com/product/dotnet/nightly/runtime-deps/about): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor](https://mcr.microsoft.com/product/dotnet/nightly/monitor/about): .NET Monitor Tool (Preview)
* [dotnet/nightly/aspire-dashboard](https://mcr.microsoft.com/product/dotnet/nightly/aspire-dashboard/about): Aspire Dashboard (Preview)
* [dotnet/samples](https://mcr.microsoft.com/product/dotnet/samples/about): .NET Samples

.NET Framework:

* [dotnet/framework](https://mcr.microsoft.com/catalog?search=dotnet/framework): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://mcr.microsoft.com/product/dotnet/framework/samples/about): .NET Framework, ASP.NET and WCF Samples

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Introduction to .NET and Docker](https://learn.microsoft.com/dotnet/core/docker/introduction) to learn more.

You can run this image to launch a YARP instance.

### Configuration

YARP expects the config file to be in `/etc/yarp.config`, and listens by default on port 5000.

Example of configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ReverseProxy": {
    "Routes": {
      "route1": {
        "ClusterId": "cluster1",
        "Match": {
          "Path": "/aspnetapp/{**catch-all}"
        },
        "Transforms": [
            { "PathRemovePrefix": "/aspnetapp" }
        ]
      }
    },
    "Clusters": {
      "cluster1": {
        "Destinations": {
          "destination1": {
            "Address": "http://aspnetapp1:8080"
          }
        }
      }
    }
  }
}
```

It can then be used with the following command (where `my-config.config` is a file containing this configuration):

```bash
docker run --rm --name myaspnetapp -d -t  mcr.microsoft.com/dotnet/samples:aspnetapp 
docker run --rm -v $(pwd)/my-config.config:/etc/yarp.config -p 5000:5000 --link myaspnetapp:aspnetapp1 mcr.microsoft.com/dotnet/yarp:latest
```

This example will proxy every requests from `http://localhost:5000/aspnetapp` to the `mcr.microsoft.com/dotnet/samples:aspnetapp` container deployed.

The [YARP GitHub repository](https://github.com/dotnet/yarp/tree/main/samples/) contains more configuration samples.

For more details, see the [documentation](https://aka.ms/YarpDocumentation) for how to configure the image and documentation for the reverse proxy configuration.

### OpenTelemetry support

This image supports OpenTelemetry. It can be configured by passing environment variables to the container:

```bash
docker run --rm -v $(pwd)/my-config.config:/etc/yarp.config -p 5000:5000 -e OTEL_EXPORTER_OTLP_ENDPOINT=https://otlp-endpoint.internal:4317 mcr.microsoft.com/dotnet/yarp:latest
```

See the [OTLP Exporter Configuration](https://opentelemetry.io/docs/languages/sdk-configuration/otlp-exporter/) for all supported environment variables.

You can skip HTTPS validation for the OTLP endpoint only by passing the environment variable `YARP_UNSAFE_OLTP_CERT_ACCEPT_ANY_SERVER_CERTIFICATE`.

## Support

### Lifecycle

* [Microsoft Support for YARP](https://github.com/dotnet/yarp/blob/main/docs/roadmap.md)
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
