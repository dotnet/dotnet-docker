# Use .NET Core and Docker on ARM32 and Raspberry Pi

You can build .NET Core container images for [ARM32](https://en.wikipedia.org/wiki/ARM_architecture) devices, with [Docker for ARM32](https://docs.docker.com/install/linux/docker-ce/debian). These instructions use an ARM32 [.NET Core Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/). You can also produce [.NET Core self-contained applications](dotnet-docker-selfcontained.md).

> Note: that Docker refers to ARM32 as `armhf` in documentation and other places.

The more general examples demonstrated in [.NET Core sample](README.md) might be a better starting point if you are new to using .NET Core with containers.

## Building an ARM32 image

The following instructions show how to build an image that is based on ARM32 

You can build the same [.NET Core console samples](README.md) and [ASP.NET Core sample](../aspnetapp/README.md) on ARM devices as you can on other architectures. For example, the following instructions will work on an ARM device. The instructions assume that you are in the root of this repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp -f Dockerfile.debian-arm32 .
docker run --rm dotnetapp
```

## Building an ARM32 image on an x64 machine

A more common option is to build ARM32 Docker images on an X64 machine. You can do that by using a Dockerfile that targets ARM32, such as [Dockerfile.debian-arm32](Dockerfile.debian-arm32). You can then push the image to a shared registry (that both machines have access to) and then pull it from your ARM32 device.

The following command shows how to build the image. You won't be able to run it on x64 machine, but will need to `docker push` it.

```console
docker build --pull -t dotnetapp -f Dockerfile.debian-arm32 .
```

You can also use this Dockerfile on an ARM32 device (and then won't need to push to a registry).

## Alpine

At this time, we do not support Alpine on ARM32. Please file an issue if this Alpine is important to you on ARM32.

## Pushing the image to a Container Registry

You can push an image to a container registry after building the image so that you can pull it from an ARM32 device. Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
