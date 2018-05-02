# Build and run the sample for Linux ARM32 with Docker

You can use ASP.NET Core and Docker together on [ARM32](https://en.wikipedia.org/wiki/ARM_architecture) devices, with [Docker for Raspberry Pi and ARM32 devices](https://docs.docker.com/install/linux/docker-ce/debian).

> Note: that Docker refers to ARM32 as `armhf` in documentation and other places.

Please see [.NET Core and Docker for ARM64](dotnet-docker-arm64.md) if you are interested in [ARM64](https://en.wikipedia.org/wiki/ARM64) usage.

## Building .NET Core Samples with Docker

You can build almost the same [.NET Core console samples](README.md) and [ASP.NET Core sample](../aspnetapp/README.md) on ARM devices as you can on other architectures. At present, the primary difference is that most .NET Core Docker file samples use .NET Core 2.0 multi-arch tags, and those don't yet offer `linux/arm` manifests. Starting with .NET Core 2.1, multi-arch tags support Linux ARM32 and are usable on ARM32 devices.

For example, the following instructions will work on an ARM32 device. The instructions assume that you are in the root of this repository.

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp -f Dockerfile.latest .
docker run --rm -it -p 8000:80 aspnetapp
```

Another option is to build ARM32 Docker images on an X64 machine. You can do by using the same pattern used in the [Dockerfile.debian-arm32-selfcontained](../dotnetapp/Dockerfile.debian-arm32-selfcontained) dockerfile. It uses a multi-arch tag for building with the SDK and then an ARM32-specific tag for creating a runtime image. Since it doesn't run code in the runtime image, it is possible to build the image on another architecture.

### Viewing the Site

After the application starts, visit the site one of two ways:

* From the web browser on the ARM32 device at `http://localhost:8000`
* From the web browser on another device on the same network on the ARM32 device IP on port 8000, similar to: `http://192.168.1.18:8000`

## Pushing the image to a Container Registry

Push the image to a container registry after building the image so that you can pull it from another ARM32 device. You can also build an ARM32 image on an X64 machine, push to a registry and then pull from an ARM32 device. Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
