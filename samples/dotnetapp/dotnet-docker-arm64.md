# Use .NET Core and Docker on ARM64

You can use .NET Core and Docker together on [ARM64](https://en.wikipedia.org/wiki/ARM_architecture) devices.

See [Use ASP.NET Core on Linux ARM64 with Docker](../aspnetapp/aspnetcore-docker-arm64.md) for ASP.NET Core apps.

See [.NET Core and Docker for ARM32](dotnet-docker-arm64.md) if you are interested in [ARM32](https://en.wikipedia.org/wiki/ARM_architecture) usage.

> Note: .NET Core can be be used with devices that use [ARMv8](https://en.wikipedia.org/wiki/ARMv8) chips.

## Try a pre-built .NET Core Docker Image

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/), based on this [sample](Dockerfile).

Type the following [Docker](https://www.docker.com/products/docker) command:

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

## Building .NET Core Samples with Docker

You can build the same [.NET Core console samples](README.md) and [ASP.NET Core sample](../aspnetapp/README.md) on ARM64 devices as you can on other architectures. For example, the following instructions will work on an ARM64 device. The instructions assume that you are in the root of this repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
docker run --rm dotnetapp
```

Another option is to build ARM64 Docker images on an X64 machine. You can do by using the same pattern used in the [Dockerfile.debian-arm64-selfcontained](Dockerfile.debian-arm64-selfcontained) dockerfile (demonstrated in a following section). It uses a multi-arch tag for building with the SDK and then an ARM64-specific tag for creating a runtime image. The pattern of building for other architectures only works because the Dockerfile doesn't run code in the runtime image.

## Building Self-contained Applications for ARM64

You can [Build .NET Core Self-Contained Applications with Docker](dotnet-docker-selfcontained.md) for an ARM64 deployment using this [Dockerfile](Dockerfile.debian-arm64-selfcontained).

The instructions assume that you are in the root of this repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp -f Dockerfile.debian-arm64-selfcontained .
docker run --rm dotnetapp
```

## Pushing the image to a Container Registry

Push the image to a container registry after building the image so that you can pull it from another ARM64 device. You can also build an ARM64 image on an X64 machine, push to a registry and then pull from an ARM64 device. Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
