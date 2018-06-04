# Use .NET Core and Docker on ARM32 and Raspberry Pi

You can use .NET Core and Docker together on [ARM32](https://en.wikipedia.org/wiki/ARM_architecture) devices, with [Docker for Raspberry Pi and ARM32 devices](https://docs.docker.com/install/linux/docker-ce/debian).

> Note: that Docker refers to ARM32 as `armhf` in documentation and other places.

See [Use ASP.NET Core on Linux ARM32 with Docker](../aspnetapp/aspnetcore-docker-arm32.md) for ASP.NET Core apps.

See [.NET Core and Docker for ARM64](dotnet-docker-arm64.md) if you are interested in [ARM64](https://en.wikipedia.org/wiki/ARM64) usage.

> Note: .NET Core can be be used with devices that use [ARMv7](https://en.wikipedia.org/wiki/ARMv7) and [ARMv8](https://en.wikipedia.org/wiki/ARMv8) chips, for example [Raspberry Pi2](https://www.raspberrypi.org/products/raspberry-pi-2-model-b/) and [Raspberry Pi3](https://www.raspberrypi.org/products/raspberry-pi-3-model-b-plus/), respectively. .NET Core does not support [ARMv6 / ARM11](https://en.wikipedia.org/wiki/ARM11) devices, for example [Raspberry Pi Zero](https://www.raspberrypi.org/products/raspberry-pi-zero/).

## Try a pre-built .NET Core Docker Image

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/r/microsoft/dotnet-samples/), based on this [sample](Dockerfile.basic-preview).

Type the following [Docker](https://www.docker.com/products/docker) command:

```console
docker run --rm microsoft/dotnet-samples
```

## Building .NET Core Samples with Docker

You can build the same [.NET Core console samples](README.md) and [ASP.NET Core sample](../aspnetapp/README.md) on ARM devices as you can on other architectures. For example, the following instructions will work on an ARM32 device. The instructions assume that you are in the root of this repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
docker run --rm dotnetapp
```

Another option is to build ARM32 Docker images on an X64 machine. You can do by using the same pattern used in the [Dockerfile.debian-arm32-selfcontained](Dockerfile.debian-arm32-selfcontained) dockerfile (demonstrated in a following section). It uses a multi-arch tag for building with the SDK and then an ARM32-specific tag for creating a runtime image. The pattern of building for other architectures only works because the Dockerfile doesn't run code in the runtime image.

## Building Self-contained Applications for ARM32

You can [Build .NET Core Self-Contained Applications with Docker](dotnet-docker-selfcontained.md) for an ARM32 deployment using this [Dockerfile](Dockerfile.debian-arm32-selfcontained).

The instructions assume that you are in the root of this repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp -f Dockerfile.debian-arm32-selfcontained .
docker run --rm dotnetapp
```

## Pushing the image to a Container Registry

Push the image to a container registry after building the image so that you can pull it from another ARM32 device. You can also build an ARM32 image on an X64 machine, push to a registry and then pull from an ARM32 device. Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
