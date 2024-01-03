# .NET container samples

The following samples and guidance demonstrate how to use .NET and Docker for development, testing and production. You can use the samples for learning about containers or as the basis of your own container images.

Kubernetes samples are provided in the [kubernetes](kubernetes/README.md) directory.

> Note: Samples ports and users are changing, with .NET 8 <br>
> [Breaking change: dotnet/samples port and user changing](https://github.com/dotnet/dotnet-docker/discussions/4764)

## Building images

* [Build a .NET container image](dotnetapp/README.md)
* [Build an ASP.NET Core container image](aspnetapp/README.md)
* [Build a single file app](releasesapp/README.md)
* [Build a native AOT app](releasesapi/README.md)
* [Building a globalization and time zone aware (or unaware) image](globalapp/README.md)
* [Build for a platform](build-for-a-platform.md)

## Development guidance

* [Selecting .NET image tags](../documentation/supported-tags.md)
* [Enable (or disable) globalization](enable-globalization.md)
* [Build and test a multi-project solution](complexapp/README.md)
* [Run test in a container](run-tests-in-sdk-container.md)
* [Build in an SDK container](build-in-sdk-container.md)
* [Run applications in an SDK container](run-in-sdk-container.md)
* [Run ASP.NET Core Applications in development with container and HTTPS](run-aspnetcore-https-development.md)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)

## Hosting guidance

* [Host ASP.NET Core Images with container and HTTPS](host-aspnetcore-https.md)
* [Push container images to Azure Container Registry](push-image-to-acr.md)
* [Push container images to Docker Hub](push-image-to-dockerhub.md)
* [Deploy ASP.NET Core Applications to Azure Container Instances](deploy-container-to-aci.md)

## Other documentation

* [Introduction to .NET and Docker](https://learn.microsoft.com/dotnet/core/docker/)
* [Announcing built-in container support for the .NET SDK](https://devblogs.microsoft.com/dotnet/announcing-builtin-container-support-for-the-dotnet-sdk/)
* [Staying safe in containers](https://devblogs.microsoft.com/dotnet/staying-safe-with-dotnet-containers/)
* [Improving multi-platform container support](https://devblogs.microsoft.com/dotnet/improving-multiplatform-container-support/)
* [Container blog posts](https://devblogs.microsoft.com/dotnet/category/containers/)

## Try pre-built images

The following commands will run a .NET console app in a container:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

The following command will run an ASP.NET Core console app in a container that you can access in your web browser at `http://localhost:8000`.

```console
docker run --rm -it -p 8000:8080 mcr.microsoft.com/dotnet/samples:aspnetapp
```

## Docker Repositories

You can find .NET container images at the following Docker repositories:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework
