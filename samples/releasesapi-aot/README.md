# Release Json Report App

This app demonstrates publishing an app as [native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) in containers.

It demonstrates key aspects:

- Container images to use
- Packages to install
- Project properties to use
- Configuring JSON (de)serialization for trimming and native AOT

## Usage

You can build and run the sample:

```bash
docker build --pull -t app -f Dockerfile.ubuntu .
docker run --rm -it -p 8000:8080 app
```

It exposes two endpoints:

- `http://localhost:8000/releases`
- `http://localhost:8000/healthz`

## Dockerfiles

The Dockerfiles are currently more proof-of-concept than finished product. Because of that, they have differing usage patterns and one that doesn't yet work.

Note: Most of these Dockerfiles can be simplified after [dotnet/sdk #34026](https://github.com/dotnet/sdk/issues/34026) gets fixed in RC1.

### Supported on AMD64 and Arm64

These Dockerfiles are supported on AMD64 and Arm64, but do not enable cross-compilation.

- [Dockerfile.alpine](Dockerfile.alpine)
- [Dockerfile.debian](Dockerfile.debian)
- [Dockerfile.cbl-mariner](Dockerfile.cbl-mariner)
- [Dockerfile.ubuntu](Dockerfile.ubuntu)

### Architecture cross-compilation

These Dockerfiles enable architecture cross-compilation (`host-target`). That means you can build an Arm64 app on x64, or vice-versa.

- [Dockerfile.debian-cross-arm64-x64](Dockerfile.debian-cross-arm64-x64)
- [Dockerfile.debian-cross-x64-arm64](Dockerfile.debian-cross-x64-arm64)
- [Dockerfile.ubuntu-cross-arm64-x64](Dockerfile.ubuntu-cross-arm64-x64)
- [Dockerfile.ubuntu-cross-x64-arm64](Dockerfile.ubuntu-cross-x64-arm64)

These Dockerfiles need to be built on the host OS (first architecture listed) and can be used to build for both the host or target architecture (the second architecture listed). When building for the target architecture, the `--platform` switch must be used.

### Distro/libc version cross-compilation

These Dockerfiles enable building for an old `libc` version (the same one CoreCLR uses; targets Ubuntu 16.04). They also enable architecture cross-compilation (`host-target`). That's why the the Dockerfiles are called `double-cross`.

- [Dockerfile.ubuntu-double-cross-x64-arm64](Dockerfile.ubuntu-double-cross-x64-arm64)
- [Dockerfile.ubuntu-double-cross-x64-x64](Dockerfile.ubuntu-double-cross-x64-x64)

These Dockerfiles need to be built on the host OS (first architecture listed). They can only be used to build for the target architecture (the second architecture listed). The `--platform` switch must be used if the target architecture differs from the host architecture.

The underlying container images -- `dotnet-buildtools/prereqs` -- that these Dockerfiles rely on have a hard assumption of being run on an x64 host. If we want to enable broader portability as a first-class scenario, we'll need to build Arm64 images as well.
