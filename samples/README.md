# .NET Core Docker Samples

The samples show various ways to use .NET Core and Docker together. You can use the samples as the basis of your own Docker images or just to play.

## Try a Sample on Dockerhub

You can run a pre-built [.NET Core sample](https://hub.docker.com/r/microsoft/dotnet-samples/) using [Docker](https://www.docker.com/products/docker). The source of this sample application is [dotnetapp](dotnetapp).

Type the following command to run the sample:

```console
docker run microsoft/dotnet-samples
```

## Samples

You can try the following samples. They are available as source and include instructions on how to build them, with and without Docker.

### Building .NET Core Apps with Docker

* [.NET Core End-to-End Docker Sample](dotnetapp) - This [sample](dotnetapp/Dockerfile) builds, tests and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](aspnetapp) - This samples demonstrates using Docker with an ASP.NET Core Web App.

### Optimizing Container Size

* [.NET Core Alpine Docker Sample](dotnetapp) - This [sample](dotnetapp/Dockerfile.alpine) builds, tests and runs an application in Alpine.

### ARM32 / Raspberry Pi

* [.NET Core Docker Production Sample](dotnetapp) - This [sample](dotnetapp/Dockerfile.arm32)includes instructions for running a runtime image with Linux on a Raspberry Pi.

## Related Repositories

See the following related Docker Hub repos:

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) for ASP.NET Web Forms and MVC images.
* [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) for ASP.NET Core images.
* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core images.
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/) for .NET Core sample images.
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) for .NET Framework images (for web applications, see microsoft/aspnet).
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/) for .NET Framework sample images.

See the following related GitHub repos:

* [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related announcements.
* [microsoft/dotnet-framework-docker-samples](https://github.com/microsoft/dotnet-framework-docker-samples/) for .NET Framework samples.
