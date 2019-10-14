# Use .NET Core and Docker on ARM64

You can build .NET Core container images for [ARM64](https://en.wikipedia.org/wiki/ARM_architecture) devices, with [Docker for ARM64](https://docs.docker.com/install/linux/docker-ce/debian). These instructions use an ARM64 [.NET Core Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/). You can also produce [.NET Core self-contained applications](dotnet-docker-selfcontained.md). The patterns demonstrated in [.NET Core sample](README.md) also apply to ARM64.

> Note: Docker refers to ARM32 as `armhf` in documentation and other places. That's not what you want, for ARM64. See [Use .NET Core and Docker on ARM32](dotnet-docker-arm32.md) if you want to target ARM32 devices.

## Building an image on ARM

You can build the same [.NET Core console samples](README.md) and [ASP.NET Core sample](../aspnetapp/README.md) on ARM devices as you can on other architectures. For example, the following instructions will work on an ARM device. The instructions assume that you are in the root of this repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
docker run --rm dotnetapp
```

## Building an ARM64 image on an x64 machine

A more common option is to build ARM64 Docker images on an X64 machine. You can that do by using a Dockerfile that targets ARM32, such as [Dockerfile.debian-arm64](Dockerfile.debian-arm64). You can then pull the image from your ARM64 device by pushing it to a registry that both machines have access to, from the x64 machine.

## Building Self-contained Applications for ARM64

You can [build a self-contained app](dotnet-docker-selfcontained.md) if you want to optimize the size of the application. There is no sample Dockerfile provided that demonstrates this scenario, however you can start with [Dockerfile.debian-x64-selfcontained](Dockerfile.debian-x64-selfcontained) and then apply the necessary ARM-isms from [Dockerfile.debian-arm32](Dockerfile.debian-arm64).

The two changes required are using the `linux-arm64` RID and using the ARM32 tag for the runtime, which is `3.0-buster-slim-arm64v8`.

## Alpine

You can use .NET Core on Alpine on ARM64 using the [Dockerfile.alpine-arm64](Dockerfile.alpine-arm64).

## Pushing the image to a Container Registry

You can push an image to a container registry after building the image so that you can pull it from an ARM32 device. Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
