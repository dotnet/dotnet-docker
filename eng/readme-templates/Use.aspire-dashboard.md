{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}Run the dashboard locally:

```console
docker run --rm -d --name aspire-dashboard -p 127.0.0.1:18888:18888 -p 127.0.0.1:4317:18889 -p 127.0.0.1:4318:18890 {{FULL_REPO}}:13
```

Open `http://localhost:18888`. To sign in, use the login URL or token printed in the container logs:

```console
docker logs aspire-dashboard
```

This example keeps browser-token authentication enabled and binds the published ports to the host's loopback interface. Incoming OTLP telemetry is unauthenticated by default in standalone mode; the browser token does not secure telemetry ingestion. Only accept telemetry from trusted applications; configure HTTPS, authentication, and network controls before exposing the dashboard beyond your machine. See [dashboard security guidance](https://aspire.dev/dashboard/security-considerations/).

{{ARGS["top-header"]}}# Send telemetry

Instrument your application with its language's OpenTelemetry SDK and configure an OTLP exporter with the matching protocol and endpoint. Starting the dashboard does not instrument your application automatically. For applications running on the host, use:

* OTLP/gRPC: `http://localhost:4317` (mapped to container port `18889`).
* OTLP/HTTP: `http://localhost:4318` (mapped to container port `18890`).

For applications in other containers, use the dashboard container's name and port (`18889` or `18890`) on a shared Docker network, not their own `localhost`.

{{ARGS["top-header"]}}# Operational notes

* Telemetry is stored in memory with configurable limits and is lost when the dashboard restarts. The dashboard is intended for development and short-term diagnostics, not durable telemetry storage.
* Standalone mode displays telemetry without an AppHost. Resource listings and captured console logs require a configured [resource service](https://aspire.dev/dashboard/configuration/#resources).

{{ARGS["top-header"]}}# Documentation and examples

* [Run the standalone dashboard](https://aspire.dev/dashboard/standalone/).
* [Python telemetry tutorial](https://aspire.dev/dashboard/standalone-for-python/).
* [JavaScript and Node.js telemetry tutorial](https://aspire.dev/dashboard/standalone-for-nodejs/).
* [Standalone dashboard sample (C#)](https://github.com/microsoft/aspire-samples/tree/main/samples/standalone-dashboard).
* [Dashboard configuration reference](https://aspire.dev/dashboard/configuration/).
