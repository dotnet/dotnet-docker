# Release Json Report API

This app demonstrates publishing an app as [native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) in containers.

A similar console app sample supports [single file deployment](../releasesapp/README.md) (non-AOT scenario). This app could also be deployed that way.

## Build image

You can build and run the sample:

```console
docker build --pull -t releasesapi 'https://github.com/dotnet/dotnet-docker.git#:samples/releasesapi'
docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 releasesapi
```

It exposes two endpoints:

- `http://localhost:8000/releases`
- `http://localhost:8000/healthz`

## App

The app is intended as a sort of compliance report for .NET. The report includes supported major releases and those recently out of support. It includes the latest and latest security patch versions for each of those major releases.

This same information is available from the [release JSON](https://github.com/dotnet/core/blob/main/release-notes/releases-index.json) files that the team maintains, but that requires a bit of code to provide the same report.

## Dockerfiles

The sample includes several Dockerfiles with varying functionality. The base images they use are currently experimental (not supported).

These Dockerfiles work on AMD64 and ARM64 when targeting the same architecture.

- [Ubuntu Chiseled](Dockerfile)
- [Alpine Linux](Dockerfile.alpine)
- [Azure Linux Distroless](Dockerfile.azurelinux-distroless)
- [Ubuntu Chiseled with Globalization support](Dockerfile.icu)
- [Alpine Linux with Globalization support](Dockerfile.alpine-icu)
- [Azure Linux Distroless with Globalization support](Dockerfile.azurelinux-distroless-icu)

For cross-compilation support, you will need to install a few extra packages during the build.

### Cross-compilation

The following Dockerfiles demonstrate how to add cross-compilation support for native AOT .NET Dockerfiles.
This means you can build ARM64 images using an AMD64 machine and vice-versa.

- [Build on AMD64 targeting ARM64](Dockerfile.ubuntu-cross-x64-arm64)
- [Build on ARM64 targeting AMD64](Dockerfile.ubuntu-cross-arm64-x64)

For example, to build an Ubuntu ARM64 native AOT .NET image on an AMD64 machine, you can run the following command:

```console
docker build --pull --platform linux/arm64 -t releasesapi -f Dockerfile.ubuntu-cross-x64-arm64 'https://github.com/dotnet/dotnet-docker.git#:samples/releasesapi'
```

Additional [native AOT cross-compilation options](https://github.com/dotnet/runtime/blob/main/src/coreclr/nativeaot/docs/containers.md) are described in the dotnet/runtime repo.

Note that non-AOT .NET images don't need any additional packages for cross-compilation. See [Building images for a specific platform](../build-for-a-platform.md) for more information.
