# .NET Core Docker Samples

This repo contains samples that demonstrate various .NET Core Docker configurations, which you can use as the basis of your own Docker images. The samples can also be used with and without Docker.

You can pick the sample that best fits the scenario you are interested in, on Windows, macOS or Linux.

## Getting Started

You can run a pre-built [.NET Core sample](https://hub.docker.com/r/microsoft/dotnet-samples/) using [Docker](https://www.docker.com/products/docker). The source of this sample application is the [dotnetapp-prod](dotnetapp-prod) sample.

Type the following command to run the sample:

```console
docker run microsoft/dotnet-samples
```

## Samples

The following samples show different ways to use .NET Core images. They depend on the [.NET Core 2.0 Docker images](https://hub.docker.com/r/microsoft/dotnet/) on Docker Hub, provided by the .NET Team at Microsoft. They use Docker [multi-stage build](https://github.com/dotnet/announcements/issues/18) and [multi-arch tags](https://github.com/dotnet/announcements/issues/14) where appropriate.

### Building .NET Core Apps with Docker

* [.NET Core "Starter" Docker Sample](dotnetapp-prod) - This sample builds and runs a single project in Docker. It's a simple sample to start with.
* [.NET Core End-to-End Docker Sample](dotnetapp-dev) - This sample builds, tests and runs the sample using Docker. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](aspnetapp) - This samples demonstrates a Dockerized ASP.NET Core Web App.

### Optimizing Container Size

* [.NET Core self-contained application Docker Production Sample](dotnetapp-selfcontained) - This sample is also good for production scenarios since it relies on an operating system image (without .NET Core). [Self-contained .NET Core apps](https://docs.microsoft.com/dotnet/articles/core/deploying/) include .NET Core as part of the app and not as a centrally installed component in a base image.
* [.NET Core Docker Alpine Production Sample](dotnetapp-prod-alpine-preview) - This sample illustrates how to use the new lightweight Alpine based .NET Core Runtime image that is currently in preview.

> Related: See [.NET Core Alpine Docker Image announcement](https://github.com/dotnet/dotnet-docker-nightly/issues/500)

### ARM32 / Raspberry Pi

* [.NET Core Docker Production Sample](dotnetapp-prod) - This sample includes instructions for running a runtime image with Linux on a Raspberry Pi.
* [.NET Core self-contained application Docker Production Sample](dotnetapp-selfcontained) - This sample includes instructions for running a self-contained image with Linux on a Raspberry Pi.
* Related: See [.NET Core on Raspberry Pi](https://github.com/dotnet/core/blob/master/samples/RaspberryPiInstructions.md)

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
