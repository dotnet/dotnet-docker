# Release Json Report API

This app demonstrates publishing an app as [native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) in containers.

> [!NOTE]
> The base images used by this sample are in preview.

A similar console app sample supports [single file deployment](../releasesapp/README.md) (non-AOT scenario). This app could also be deployed that way.

## Build image

You can build and run the sample:

```bash
docker build --pull -t app .
docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 app
```

It exposes two endpoints:

- `http://localhost:8000/releases`
- `http://localhost:8000/healthz`

## App

The app is intended as a sort of compliance report for .NET. The report includes supported major releases and those recently out of support. It includes the latest and latest security patch versions for each of those major releases.

This same information is available from the [release JSON](https://github.com/dotnet/core/blob/main/release-notes/releases-index.json) files that the team maintains, but that requires a bit of code to provide the same report.

## Dockerfiles

The sample includes several Dockerfiles with varying functionality. The base images they use are currently experimental (not supported).

These Dockerfiles work on AMD64 and Arm64, targeting the matching architecture.

- [Alpine](Dockerfile.alpine)
- [Ubuntu](Dockerfile)

### Cross-compilation

The following samples support cross-compile, which means you can use `--platform linux/amd64` on Arm64 and vice-versa.

- [Debian cross-compile on Amd64](Dockerfile.debian-cross-x64-arm64)
- [Debian cross-compile on Arm64](Dockerfile.debian-cross-arm64-x64)
- [Ubuntu](Dockerfile)

The Debian Dockerfiles need to be built on the specified architecture and can be used to build for both the host or target architecture. When building for the target architecture, the `--platform` switch must be used.

Additional [cross-compilation options](https://github.com/dotnet/runtime/blob/main/src/coreclr/nativeaot/docs/containers.md) are described in the dotnet/runtime repo.
