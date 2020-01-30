# .NET Core Docker samples

The following samples and guidance demonstrate how to use .NET Core and Docker for development, testing and production. You can use the samples as the basis of your own Docker images or just to learn.

## Building images

* [Build a .NET Core Docker image](dotnetapp/README.md)
* [Build an ASP.NET Core Docker image](aspnetapp/README.md)
* [Build and test a multi-project solution](complexapp/README.md)

## Development Guidance

* [Establishing a Docker environment](establishing-docker-environment.md)
* [Selecting .NET Core image tags](selecting-tags.md)
* [Run test in a container](run-tests-in-sdk-container.md)
* [Build in an SDK container](build-in-sdk-container.md)
* [Run applications in an SDK container](run-in-sdk-container.md)
* [Run ASP.NET Core Applications in development with Docker and HTTPS](run-aspnetcore-https-development.md)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/master/documentation/image-artifact-details.md)

## Hosting guidance

* [Host ASP.NET Core Images with Docker and HTTPS](host-aspnetcore-https.md)
* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to Docker Hub](push-image-to-dockerhub.md)
* [Deploy ASP.NET Core Applications to Azure Container Instances](deploy-container-to-aci.md)

## Sample snippets

In addition to fully operational sample projects, [code snippets](snippets/) are also provided for demonstrating more specific scenarios.

* [Managing NuGet Credentials in Docker Scenarios](snippets/nuget-credentials.md)
* [Installing .NET Core in a Dockerfile](snippets/installing-dotnet.md)
* [Using the System.Drawing.Common Package in a Docker Container](snippets/using-system-drawing-common.md)

## Try pre-built images

The following commands will run a .NET Core console app in a container:

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

The following command will run an ASP.NET Core console app in a container that you can access in your web browser at `http://localhost:8000`.

```console
docker run --rm -it -p 8000:80 mcr.microsoft.com/dotnet/core/samples:aspnetapp
```

## Docker Repositories

You can find .NET container images at the following Docker repositories:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework
