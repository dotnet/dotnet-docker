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

## Supported Linux distros

The .NET Team publishes images for [multiple distros](../../documentation/supported-platforms.md).

Samples are provided for:

- [Alpine](Dockerfile.alpine)
- [Debian](Dockerfile.debian)
- [Ubuntu](Dockerfile.ubuntu)
- [Ubuntu Chiseled](Dockerfile.chiseled)

The default [Dockerfile](Dockerfile) uses a major.minor version tag, which references a multi-platform image that provides Debian and Windows Nano Server images (depending on the requesting client).

More extensive samples are provided for Alpine:

- [Alpine with trimming, ready-to-run compilation, and self-contained publishing](Dockerfile.alpine-slim)
- [Alpine with ICU (for globalization)](Dockerfile.alpine-icu)

These patterns can be applied to other distros.

## Supported Windows versions

The .NET Team publishes images for [multiple Windows versions](../../documentation/supported-platforms.md.md). You must have [Windows containers enabled](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers) to use these images.

Samples are provided for

- [Nano Server](Dockerfile.nanoserver)
- [Nano Server with trimming, ready-to-run compilation, and self-contained publishing](Dockerfile.nanoserver-slim)
- [Windows Server Core](Dockerfile.windowsservercore)
