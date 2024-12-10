# .NET Docker Sample

This sample demonstrates how to build container images for .NET console apps. See [.NET Docker Samples](../README.md) for more samples.

## Run the sample image

You can start by launching a sample from our [container registry](https://mcr.microsoft.com/product/dotnet/samples/about).

```console
docker run --rm mcr.microsoft.com/dotnet/samples:dotnetapp-chiseled
```

## Build the image

You can build and run an image using the following instructions (cloninig the repo isn't necessary):

```console
docker build --pull -t dotnetapp 'https://github.com/dotnet/dotnet-docker.git#:samples/dotnetapp'
docker run --rm dotnetapp
```

Add the argument `-f <Dockerfile>` to build the sample in a different configuration.
For example, build an [Ubuntu Chiseled](../../documentation/ubuntu-chiseled.md) image using [Dockerfile.chiseled](Dockerfile.chiseled):

```console
docker build --pull -t dotnetapp -f Dockerfile.chiseled 'https://github.com/dotnet/dotnet-docker.git#:samples/dotnetapp'
```

## Supported Linux distros

The .NET Team publishes images for [multiple distros](../../documentation/supported-platforms.md).

Samples are provided for:

- [Alpine](Dockerfile.alpine)
- [Alpine with ICU installed](Dockerfile.alpine-icu)
- [Debian](Dockerfile.debian)
- [Ubuntu](Dockerfile.ubuntu)
- [Ubuntu Chiseled](Dockerfile.chiseled)

## Supported Windows versions

The .NET Team publishes images for [multiple Windows versions](../../documentation/supported-platforms.md). You must have [Windows containers enabled](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers) to use these images.

Samples are provided for

- [Nano Server](Dockerfile.nanoserver)
- [Windows Server Core](Dockerfile.windowsservercore)

Windows variants of the sample can be pulled via one the following image names:

- `mcr.microsoft.com/dotnet/samples:dotnetapp-nanoserver-1809`
- `mcr.microsoft.com/dotnet/samples:dotnetapp-nanoserver-ltsc2022`
