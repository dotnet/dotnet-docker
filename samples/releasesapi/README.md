# Release Json Report App

This app demonstrates publishing an app as [native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) in containers. 

> Note: The base images used by this sample are in preview.

A similar console app sample supports [single file deployment](../releasesapp/README.md) (non-AOT scenario). This app could also be deployed that way.

## Build image

You can build and run the sample:

```bash
docker build --pull -t app .
docker run --rm -it -p 8000:8080 app
```

It exposes two endpoints:

- `http://localhost:8000/releases`
- `http://localhost:8000/healthz`

## App

The app is intended as a sort of compliance report for .NET. The report includes supported major releases and those recently out of support. It includes the latest and latest security patch versions for each of those major releases. 

This same information is available from the [release JSON](https://github.com/dotnet/core/blob/main/release-notes/releases-index.json) files that the team maintains, but that requires a bit of code to provide the same report. In fact, this app is that code.

## Build image with the SDK

The easiest way to [build images is with the SDK](https://github.com/dotnet/sdk-container-builds). Native AOT apps [require additional pre-requisitives](https://learn.microsoft.com/dotnet/core/deploying/native-aot/#prerequisites). If you don't have them installed, you can use a `-aot` SDK image, using `docker run`.

## Dockerfiles

The sample includes several Dockerfiles with varying functionality.

These Dockerfiles work on AMD64 and Arm64.

- [Alpine](Dockerfile.alpine)
- [Ubuntu](Dockerfile)

## Cross-compilation

The Ubuntu sample enables cross-compilation. That means that you can use `--platform linux/amd64` on Arm64 and vice-versa. Alpine doesn't support cross-compilation (for native apps).

### Unsupported Dockerfiles

Additional Dockerfiles are provided to help users that to explore other options.

- [Debian](Dockerfile.debian)
- [Mariner (used by Microsoft teams)](Dockerfile.cbl-mariner)

### Cross-compilation (Debian)

The following two Debian samples support cross-compilation and the others above don't.

- [Debian cross-compile on Amd64](Dockerfile.debian-cross-x64-arm64)
- [Debian cross-compile on Arm64](Dockerfile.debian-cross-arm64-x64)

These Dockerfiles need to be built on the host OS and can be used to build for both the host or target architecture. When building for the target architecture, the `--platform` switch must be used.

### Distro/libc version cross-compilation

These Dockerfiles enable building for an old `libc` version (the same one CoreCLR uses; targets Ubuntu 16.04). They also enable architecture cross-compilation (`host-target`). That's why the the Dockerfiles are called `double-cross`.

- [Dockerfile.ubuntu-double-cross-x64-arm64](Dockerfile.ubuntu-double-cross-x64-arm64)
- [Dockerfile.ubuntu-double-cross-x64-x64](Dockerfile.ubuntu-double-cross-x64-x64)

These Dockerfiles need to be built on the host OS (first architecture listed). They can only be used to build for the target architecture (the second architecture listed). The `--platform` switch must be used if the target architecture differs from the host architecture.

The underlying container images -- `dotnet-buildtools/prereqs` -- that these Dockerfiles rely on have a hard assumption of being run on an x64 host. If we want to enable broader portability as a first-class scenario, we'll need to build Arm64 images as well.
