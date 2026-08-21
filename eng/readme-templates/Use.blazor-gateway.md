{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}You can run this image to launch a Blazor gateway instance in front of one or more statically-published Blazor Web applications.

{{ARGS["top-header"]}}# Layering application content

This image only contains the gateway runtime at `/app`. It does not contain any Blazor application content. To serve a Blazor application, layer or mount the application's published static web assets and its transformed static-web-assets endpoints manifest on top of this image, then reference that manifest from the `ClientApps` configuration section described below.

{{ARGS["top-header"]}}# Configuration

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
  {{FULL_REPO}}:{{if VARIABLES["branch"] = "nightly":11.0-preview^else:latest}}
```

{{ARGS["top-header"]}}# Health checks

This image exposes `/alive` as its liveness endpoint. `/alive` is the endpoint to use for container/orchestrator health probes in Production; it is available in every environment. `/health`, which reports more detailed health information, is only available when `ASPNETCORE_ENVIRONMENT` is set to `Development` and should not be assumed to be present otherwise.

{{ARGS["top-header"]}}# OpenTelemetry support

This image supports OpenTelemetry. It can be configured by passing environment variables to the container:

```bash
docker run --rm -p 8080:8080 -e OTEL_EXPORTER_OTLP_ENDPOINT=https://otlp-endpoint.internal:4317 {{FULL_REPO}}:{{if VARIABLES["branch"] = "nightly":11.0-preview^else:latest}}
```

See the [OTLP Exporter Configuration](https://opentelemetry.io/docs/languages/sdk-configuration/otlp-exporter/) for all supported environment variables.
