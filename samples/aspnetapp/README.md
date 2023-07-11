# ASP.NET Core Docker Sample

This sample demonstrates how to build container images for ASP.NET Core web apps. See [.NET Docker Samples](../README.md) for more samples.

## Try a pre-built version of the sample

You can start by launching a sample from our [container registry](https://mcr.microsoft.com/) and access it in your web browser at `http://localhost:8000`.

```console
docker run --rm -it -p 8000:8080 mcr.microsoft.com/dotnet/samples:aspnetapp
```

You can also call an endpoint that the app exposes:

```bash
$ curl http://localhost:8000/Environment
{"runtimeVersion":".NET 8.0.0-preview.6.23329.7","osVersion":"Ubuntu 22.04.2 LTS","osArchitecture":"Arm64","user":"app","processorCount":4,"totalAvailableMemoryBytes":4124442624,"memoryLimit":0,"memoryUsage":31518720,"hostName":"78e2b2cfc0e8"}
```

## Build an ASP.NET Core image

You can build and run an image using the following instructions (if you've cloned this repo):

```console
docker build --pull -t aspnetapp .
docker run --rm -it -p 8000:8080 aspnetapp
```

You should see the following console output as the application starts:

```console
> docker run --rm -it -p 8000:8080 aspnetapp
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://[::]:8080
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

After the application starts, navigate to `http://localhost:8000` in your web browser. You can also view the ASP.NET Core site running in the container from another machine with a local IP address such as `http://192.168.1.18:8000`.

> Note: ASP.NET Core apps (in our official images) listen to [port 8080 by default](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7), starting with .NET 8. The [`-p` argument](https://docs.docker.com/engine/reference/commandline/run/#publish) in these examples maps host port `8000` to container port `8080` (`host:container` mapping). The container will not be accessible without this mapping. ASP.NET Core can be [configured to listen on a different or additional port](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel/endpoints).

You can see the app running via `docker ps`.

```bash
$ docker ps
CONTAINER ID   IMAGE                                        COMMAND         CREATED          STATUS                    PORTS                  NAMES
d79edc6bfcb6   mcr.microsoft.com/dotnet/samples:aspnetapp   "./aspnetapp"   35 seconds ago   Up 34 seconds (healthy)   0.0.0.0:8080->8080/tcp   nice_curran
```

You may notice that the sample includes a [health check](../enable-healthchecks.md), indicated in the "STATUS" column.

## Supported Linux distros

The .NET Team publishes images for [multiple distros](../../documentation/supported-platforms.md).

Samples are provided for:

- [Alpine](Dockerfile.alpine)
- [Debian](Dockerfile.debian)
- [Ubuntu](Dockerfile.ubuntu)
- [Ubuntu Chiseled](Dockerfile.chiseled)

The default [Dockerfile](Dockerfile) uses a major.minor version tag, which references a multi-platform image that provides Debian and Windows Nano Server images (depending on the requesting client).

More extensive samples are provided for Alpine:

- [Alpine with ICU (for globalization)](Dockerfile.alpine-icu)
- [Alpine with trimming, ready-to-run compilation, and self-contained publishing](Dockerfile.alpine-slim)

These patterns can be applied to other distros.

## Supported Windows versions

The .NET Team publishes images for [multiple Windows versions](../../documentation/supported-platforms.md). You must have [Windows containers enabled](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers) to use these images.

Samples are provided for

- [Nano Server](Dockerfile.nanoserver)
- [Nano Server with trimming, ready-to-run compilation, and self-contained publishing](Dockerfile.nanoserver-slim)
- [Windows Server Core](Dockerfile.windowsservercore)
- [Windows Server Core with IIS](Dockerfile.windowsservercore-iis)
