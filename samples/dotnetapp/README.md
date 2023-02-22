# .NET Docker Sample

This sample demonstrates how to build container images for .NET console apps. See [.NET Docker Samples](../README.md) for more samples.

## Try a pre-built version of the sample

You can start by launching a sample from our [container registry](https://mcr.microsoft.com/).

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

## Build a .NET image

You can build and run an image using the following instructions (if you've cloned this repo):

```console
docker build --pull -t dotnetapp .
docker run --rm dotnetapp
```

You can also pass your own message to the app as an argument.

## Targeting a platform or architecture

The .NET team publishes [multi-platform](https://docs.docker.com/build/building/multi-platform/) tags, such as `6.0` and `6.0-alpine`. These tags can be used in multiple contexts (like Linux Arm64 and Windows x64) but can also be configured to produce specific results. This topic is covered in more detail in [Building for a platform](../build-for-a-platform.md).

The most common scenario is building for a specific architecture, using the `--platform` argument. The following pattern can be used for that.

```bash
docker build --platform linux/amd64
```

This command will always produce a `linux/amd64`. This is particularly useful if you are on Apple M1 device but want to build an image that is compatible with x64 cloud resources. The same pattern can be used to target other platforms, like `linux/arm64`.

## Supported Linux distros

The .NET Team publishes images for [multiple distros](../../documentation/supported-platforms.md.md).

Samples are provided for:

- [Alpine](Dockerfile.alpine)
- [Debian](Dockerfile.debian)
- [Ubuntu](Dockerfile.ubuntu)
- [Ubuntu Chiseled](Dockerfile.chiseled)

More extensive samples are provided for Alpine:

- [Alpine + ICU (for globalization)](Dockerfile.alpine-icu)
- [Using trimming, ready-to-run compilation, and self-contained publishing](Dockerfile.alpine-slim)

These patterns can be applied to other distros.

## Supported Windows versions

The .NET Team publishes images for [multiple Windows versions](../../documentation/supported-platforms.md.md). You must have [Windows containers enabled](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers) to use these images.

Samples are provided for

- [Nano Server](Dockerfile.nanoserver)
- [Windows Server Core](Dockerfile.windowsservercore)
- [Using trimming, ready-to-run compilation, and self-contained publishing](Dockerfile.nanoserver-slim)
