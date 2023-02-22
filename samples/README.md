# .NET container samples

The following samples and guidance demonstrate how to use .NET and Docker for development, testing and production. You can equally use the samples for learning about containers or as the basis of your own container images.

## Building images

* [Build a .NET container image](dotnetapp/README.md)
* [Build an ASP.NET Core container image](aspnetapp/README.md)
* [Building a globalization and timezone aware (or unaware) image](globalapp/README.md)
* [Container best practices](container-best-practices.md)
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
* [Push container Images to Azure Container Registry](push-image-to-acr.md)
* [Push container Images to Docker Hub](push-image-to-dockerhub.md)
* [Deploy ASP.NET Core Applications to Azure Container Instances](deploy-container-to-aci.md)

## Other documentation

* [.NET Container documentation](../documentation/README.md).
* [Introduction to .NET and Docker](https://learn.microsoft.com/dotnet/core/docker/)]

## Try pre-built images

The following commands will run a .NET console app in a container:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

The following command will run an ASP.NET Core console app in a container that you can access in your web browser at `http://localhost:8000`.

```console
docker run --rm -it -p 8000:80 mcr.microsoft.com/dotnet/samples:aspnetapp
```

## Docker Repositories

You can find .NET container images at the following Docker repositories:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework
