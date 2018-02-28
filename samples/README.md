# .NET Core Docker Samples

The samples show various ways to use .NET Core and Docker together. You can use the samples as the basis of your own Docker images or just to play.

The samples exercise various levels of functionality. The [.NET Core Docker sample](dotnetapp/README.md) includes the most functionality, including build, unit testing, and pushing images to a container registry. The [ASP.NET Core Docker sample](aspnetapp/README.md) includes instructions for testing images with [Azure Container Instances](https://azure.microsoft.com/services/container-instances/). The samples include detailed instructions for use with and without Docker.

## Try a pre-built .NET Core Docker Image

You can quickly try a pre-built [sample .NET Core Docker image](https://hub.docker.com/r/microsoft/dotnet-samples/), based on the [.NET Core sample](dotnetapp/README.md).

Type the following command to run a sample with [Docker](https://www.docker.com/products/docker):

```console
docker run --rm microsoft/dotnet-samples
```

## Building .NET Core Apps with Docker

* [.NET Core Docker Sample](dotnetapp/README.md) - This [sample](dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](aspnetapp/README.md) - This [sample](aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

## Push Images to a Container Registry

* [Push Docker Images to Azure Container Registry](dotnetapp/push-image-to-acr.md)
* [Push Docker Images to DockerHub](dotnetapp/push-image-to-dockerhub.md)
* [Deploy ASP.NET Core Applications to Azure Container Instances](aspnetapp/deploy-container-to-aci.md)

## Optimizing Container Size

* [.NET Core Alpine Docker Sample](dotnetapp/README.md) - This [sample](dotnetapp/Dockerfile.alpine-x64) builds, tests, and runs an application using Alpine.
* [.NET Core self-contained Sample](dotnetapp/dotnet-docker-selfcontained.md) - This [sample](dotnetapp/Dockerfile.debian-x64-selfcontained) builds and runs an application as a self-contained application.

## ARM32 / Raspberry Pi

* [.NET Core ARM32 Docker Sample](dotnetapp/dotnet-docker-arm32.md) - This [sample](dotnetapp/Dockerfile.debian-arm32) builds and runs an application with Debian on ARM32 (works on Raspberry Pi).
* [ASP.NET Core ARM32 Docker Sample](aspnetapp/README.md) - This [sample](aspnetapp/Dockerfile.debian-arm32) builds and runs an ASP.NET Core application with Debian on ARM32 (works on Raspberry Pi).

## .NET Core Resources

More Samples

* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)

Docs and More Information:

* [.NET Docs](https://docs.microsoft.com/dotnet/)
* [ASP.NET Docs](https://docs.microsoft.com/aspnet/)
* [dotnet/core](https://github.com/dotnet/core) for starting with .NET Core on GitHub.
* [dotnet/announcements](https://github.com/dotnet/announcements/issues) for .NET announcements.

## Related Repositories

.NET Core Docker Hub repos:

* [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) for ASP.NET Core images.
* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core images.
* [microsoft/dotnet-nightly](https://hub.docker.com/r/microsoft/dotnet-nightly/) for .NET Core preview images.
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/) for .NET Core sample images.

.NET Framework Docker Hub repos:

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) for ASP.NET Web Forms and MVC images.
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) for .NET Framework images.
* [microsoft/dotnet-framework-build](https://hub.docker.com/r/microsoft/dotnet-framework-build/) for building .NET Framework applications with Docker.
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/) for .NET Framework and ASP.NET sample images.
