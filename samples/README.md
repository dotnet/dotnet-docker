# .NET Core Docker Samples

The samples show various ways to use .NET Core and Docker together. You can use the samples as the basis of your own Docker images or just to learn.

## Try a pre-built .NET Core Docker Image

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/).

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

## Samples

The samples demonstrate how to use Docker for development, testing and production.

* [Build a .NET Core Docker image](dotnetapp/README.md)
* [Build an ASP.NET Core Docker image](aspnetapp/README.md)
* [Build in a container](build-in-container.md)
* [Unit test in a container](unit-testing-in-container.md)
* [Develop .NET Core applications in a container](dotnetapp/dotnet-docker-dev-in-container.md)
* [Host ASP.NET Core Images with Docker and HTTPS](aspnetapp/aspnetcore-docker-https.md)
* [Develop ASP.NET Core Applications with Docker and HTTPS](aspnetapp/aspnetcore-docker-https-development.md)
* [Push Docker Images to Azure Container Registry](dotnetapp/push-image-to-acr.md)
* [Push Docker Images to DockerHub](dotnetapp/push-image-to-dockerhub.md)
* [Deploy ASP.NET Core Applications to Azure Container Instances](aspnetapp/deploy-container-to-aci.md)

## Docker Repositories

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

## .NET Core Resources

* [.NET Docs](https://docs.microsoft.com/dotnet/)
* [ASP.NET Docs](https://docs.microsoft.com/aspnet/)
