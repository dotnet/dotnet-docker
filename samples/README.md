# .NET Core Docker Samples

The samples show various ways to use .NET Core and Docker together. You can use the samples as the basis of your own Docker images or just to play.

## Try Samples on Dockerhub

You can run a pre-built [.NET Core sample](https://hub.docker.com/r/microsoft/dotnet-samples/) using [Docker](https://www.docker.com/products/docker). The source of this sample application is [dotnetapp](dotnetapp).

Type the following command to run a sample:

```console
docker run microsoft/dotnet-samples
```

## Samples

You can try the following samples. They include instructions on how to build them, with and without Docker.

### Building .NET Core Apps with Docker

* [.NET Core Docker Sample](dotnetapp) - This [sample](dotnetapp/Dockerfile) builds, tests and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](aspnetapp) - This [sample](aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

### Optimizing Container Size

* [.NET Core Alpine Docker Sample](dotnetapp) - This [sample](dotnetapp/Dockerfile.alpine) builds, tests and runs an application using Alpine.
* [ASP.NET Core Alpine Docker Sample](aspnetapp) - This [sample](aspnetapp/Dockerfile.alpine) builds, tests and runs an application using Alpine (Linux).

### ARM32 / Raspberry Pi

* [.NET Core ARM32 Docker Sample](dotnetapp) - This [sample](dotnetapp/Dockerfile.arm32) builds and runs an application with Debian on ARM32 (works on Raspberry Pi).
* [ASP.NET Core ARM32 Docker Sample](aspnetapp) - This [sample](aspnetapp/Dockerfile.arm32) builds and runs an application with Debian on ARM32 (works on Raspberry Pi).

## Related Repositories

See the following related .NET Core Docker Hub repos:

* [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) for ASP.NET Core images.
* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core images.
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/) for .NET Core sample images.

See the following related .NET Framework Docker Hub repos:

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) for ASP.NET Web Forms and MVC images.
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) for .NET Framework images (for web applications, see microsoft/aspnet).
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/) for .NET Framework sample images.


See the following related GitHub repos:

* [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related announcements.
* [microsoft/dotnet-framework-docker-samples](https://github.com/microsoft/dotnet-framework-docker-samples/) for .NET Framework samples.
