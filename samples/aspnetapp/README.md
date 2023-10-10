# ASP.NET Core Docker Sample

This sample demonstrates how to build container images for ASP.NET Core web apps. See [.NET Docker Samples](../README.md) for more samples.

> Note: .NET 8 container images use port `8080`, by default. Previous .NET versions used port `80`. The instructions for the sample assume the use of port `8080`.

## Run the sample image

You can start by launching a sample from our [container registry](https://mcr.microsoft.com/) and access it in your web browser at `http://localhost:8000`.

```console
docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 mcr.microsoft.com/dotnet/samples:aspnetapp
```

You can also call an endpoint that the app exposes:

```bash
$ curl http://localhost:8000/Environment
{"runtimeVersion":".NET 8.0.0-preview.6.23329.7","osVersion":"Ubuntu 22.04.2 LTS","osArchitecture":"Arm64","user":"app","processorCount":4,"totalAvailableMemoryBytes":4124442624,"memoryLimit":0,"memoryUsage":31518720,"hostName":"78e2b2cfc0e8"}
```

This container image is built with [Ubuntu Chiseled](https://devblogs.microsoft.com/dotnet/dotnet-6-is-now-in-ubuntu-2204/#net-in-chiseled-ubuntu-containers), with [Dockerfile](Dockerfile.chiseled-composite).

## Change port

You can change the port ASP.NET Core uses with one of the following environment variables. However, port `8080` (set by default) is recommended.

The following examples change the port to port `80`.

Supported with .NET 8+:

```bash
ASPNETCORE_HTTP_PORTS=80
```

Supported with .NET Core 1.0+

```bash
ASPNETCORE_URLS=http://+:80 
```

Note: `ASPNETCORE_URLS` overwrites `ASPNETCORE_HTTP_PORTS` if set.

These environment variables are used in [.NET 8](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7C5-L7C31) and [.NET 6](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/6.0/bookworm-slim/amd64/Dockerfile#L5) Dockerfiles, respectively.

## Build image

You can built an image using one of the provided Dockerfiles.

```console
docker build --pull -t aspnetapp .
docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 aspnetapp
```

You should see the following console output as the application starts:

```console
> docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 aspnetapp
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://[::]:8080
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

After the application starts, navigate to `http://localhost:8000` in your web browser. You can also view the ASP.NET Core site running in the container from another machine with a local IP address such as `http://192.168.1.18:8000`.

> Note: ASP.NET Core apps (in official images) listen to [port 8080 by default](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7), starting with .NET 8. The [`-p` argument](https://docs.docker.com/engine/reference/commandline/run/#publish) in these examples maps host port `8000` to container port `8080` (`host:container` mapping). The container will not be accessible without this mapping. ASP.NET Core can be [configured to listen on a different or additional port](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel/endpoints).

You can see the app running via `docker ps`.

```bash
$ docker ps
CONTAINER ID   IMAGE                                        COMMAND         CREATED          STATUS                    PORTS                  NAMES
d79edc6bfcb6   mcr.microsoft.com/dotnet/samples:aspnetapp   "./aspnetapp"   35 seconds ago   Up 34 seconds (healthy)   0.0.0.0:8080->8080/tcp   nice_curran
```

You may notice that the sample includes a [health check](../enable-healthchecks.md), indicated in the "STATUS" column.

## Build image with the SDK

The easiest way to [build images is with the SDK](https://github.com/dotnet/sdk-container-builds). 

```console
dotnet publish /p:PublishProfile=DefaultContainer
```

That command can be further customized to use a different base image and publish to a container registry. You must first use `docker login` to login to the registry.

```console
dotnet publish /p:PublishProfile=DefaultContainer /p:ContainerBaseImage=mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled /p:ContainerRegistry=docker.io /p:ContainerRepository=youraccount/aspnetapp
```

## Supported Linux distros

The .NET Team publishes images for [multiple distros](../../documentation/supported-platforms.md).

Samples are provided for:

- [Alpine](Dockerfile.alpine)
- [Alpine with Composite ready-to-run image](Dockerfile.alpine-composite)
- [Alpine with ICU installed](Dockerfile.alpine-icu)
- [Debian](Dockerfile.debian)
- [Ubuntu](Dockerfile.ubuntu)
- [Ubuntu Chiseled](Dockerfile.chiseled)
- [Ubuntu Chiseled with Composite ready-to-run image](Dockerfile.chiseled-composite)

## Supported Windows versions

The .NET Team publishes images for [multiple Windows versions](../../documentation/supported-platforms.md). You must have [Windows containers enabled](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers) to use these images.

Samples are provided for

- [Nano Server](Dockerfile.nanoserver)
- [Windows Server Core](Dockerfile.windowsservercore)
- [Windows Server Core with IIS](Dockerfile.windowsservercore-iis)

Windows variants of the sample can be pulled via one the following registry addresses:

- `mcr.microsoft.com/dotnet/samples:aspnetapp-nanoserver-1809`
- `mcr.microsoft.com/dotnet/samples:aspnetapp-nanoserver-ltsc2022`
