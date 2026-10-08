{{
    _ ARGS:
      readme-host: Moniker of the site that will host the readme ^

    set isGitHub to ARGS["readme-host"] = "github" ^
    set isNightlyRepo to match(split(REPO, "/")[1], "nightly") || VARIABLES["branch"] = "nightly" ^
    set stableUrl to when(
        isGitHub,
        "https://github.com/dotnet/dotnet-docker/blob/main/README.aspire-dashboard.md",
        "https://mcr.microsoft.com/artifact/mar/aspire/dashboard/about") ^
    set stableName to when(isGitHub, "dotnet/aspire-dashboard", "aspire/dashboard")

}}{{if isGitHub:# Aspire Dashboard

}}{{if isNightlyRepo:> **Important**: The {{REPO}} image is a preview build of the Aspire Dashboard and is not signed. See [{{stableName}}]({{stableUrl}}) for stable releases.

}}## Featured Tags

* `13`
  * `docker pull {{FULL_REPO}}:13`

## About

The [Aspire Dashboard](https://aspire.dev/dashboard/standalone/) is a standalone viewer for logs, traces, and metrics from OpenTelemetry-enabled applications in any language. Source code is available in [microsoft/aspire](https://github.com/microsoft/aspire).

## Usage

Run the dashboard locally:

```console
docker run --rm -d --name aspire-dashboard -p 127.0.0.1:18888:18888 -p 127.0.0.1:4317:18889 -p 127.0.0.1:4318:18890 {{FULL_REPO}}:13
```

Open `http://localhost:18888`. To sign in, use the login URL or token printed in the container logs:

```console
docker logs aspire-dashboard
```

This example keeps browser-token authentication enabled and binds the published ports to the host's loopback interface. Incoming OTLP telemetry is unauthenticated by default in standalone mode; the browser token does not secure telemetry ingestion. Only accept telemetry from trusted applications; configure authentication, HTTPS, and network controls before exposing the dashboard beyond your machine. See [dashboard security guidance](https://aspire.dev/dashboard/security-considerations/).

### Send telemetry

Instrument your application with its language's OpenTelemetry SDK and configure an OTLP exporter. Starting the dashboard does not instrument your application automatically. For applications running on the host, use the endpoint and matching exporter protocol:

* OTLP/gRPC: `http://localhost:4317` (mapped to container port `18889`).
* OTLP/HTTP: `http://localhost:4318` (mapped to container port `18890`).

For applications in other containers, use the dashboard container's name and port (`18889` or `18890`) on a shared Docker network, not their own `localhost`.

### Operational notes

* Telemetry is stored in memory with configurable limits and is lost when the dashboard restarts. The dashboard is intended for development and short-term diagnostics, not durable telemetry storage.
* Standalone mode displays telemetry without an AppHost. Resource listings and captured console logs require a configured [resource service](https://aspire.dev/dashboard/configuration/#resources).

### Documentation and examples

* [Run the standalone dashboard](https://aspire.dev/dashboard/standalone/).
* [Python telemetry tutorial](https://aspire.dev/dashboard/standalone-for-python/).
* [JavaScript and Node.js telemetry tutorial](https://aspire.dev/dashboard/standalone-for-nodejs/).
* [Standalone dashboard sample (C#)](https://github.com/microsoft/aspire-samples/tree/main/samples/standalone-dashboard).
* [Dashboard configuration reference](https://aspire.dev/dashboard/configuration/).

{{if isGitHub:## Full Tag Listing
<!--End of generated tags-->
*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/{{REPO}}/tags/list) for all supported and unsupported tags.*

}}## Support

See the [Aspire support policy](https://aspire.dev/support/) for supported versions and lifecycle information. Container images are also covered by the [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md), [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md), and [Image Update Policy](https://github.com/dotnet/dotnet-docker/blob/main/README.md#image-update-policy).

For container security and vulnerability reporting, see the [Security Policy](https://github.com/dotnet/dotnet-docker/blob/main/SECURITY.md) and [Container Vulnerability Workflow](https://github.com/dotnet/dotnet-docker/blob/main/documentation/vulnerability-reporting.md). Additional security information can be found on [aspire.dev/dashboard/security-considerations](https://aspire.dev/dashboard/security-considerations/#standalone-mode).

### Feedback

* [Dashboard issues and feature requests](https://github.com/microsoft/aspire/issues).
* [Container image issues](https://github.com/dotnet/dotnet-docker/issues/new/choose).
* [Contact Microsoft Support](https://support.microsoft.com/contactus/).

## License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [Aspire MIT license](https://github.com/microsoft/aspire/blob/main/LICENSE.TXT)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
