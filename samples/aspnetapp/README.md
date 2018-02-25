# ASP.NET Core Docker Sample

This [sample](Dockerfile) demonstrates how to use ASP.NET Core and Docker together. The sample works with both Linux and Windows containers and can also be used without Docker.

The sample builds the application in a container based on the larger [.NET Core SDK Docker image](https://hub.docker.com/r/microsoft/dotnet/). It builds and tests the application and then copies the final build result into a Docker image based on the smaller [ASP.NET Core Docker Runtime image](https://hub.docker.com/r/microsoft/aspnetcore/). It uses Docker [multi-stage build](https://github.com/dotnet/announcements/issues/18) and [multi-arch tags](https://github.com/dotnet/announcements/issues/14).

This sample requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or later of the [Docker client](https://www.docker.com/products/docker).

The [.NET Core Docker Sample](../dotnetapp/README.md) demonstrates more functionality, including unit testing, publishing self-contained applications and using the Alpine base image. The same techniques can be applied to ASP.NET applications.

## Try a pre-built ASP.NET Core Docker Image

You can quickly try a pre-built [sample ASP.NET Core Docker image](https://hub.docker.com/r/microsoft/dotnet-samples/), based on this sample.

Type the following command to run a sample with [Docker](https://www.docker.com/products/docker):

```console
docker run --rm microsoft/dotnet-samples:aspnetapp
```

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with git, using the following instructions:

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Build and run the sample with Docker

You can build and run the sample in Docker using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp .
docker run --rm --name aspnetcore_sample aspnetapp
```

You should see the following console output as the application starts.

```console
C:\git\dotnet-docker\samples\aspnetapp>docker run --rm --name aspnetcore_sample aspnetapp
Hosting environment: Production
Content root path: /app
Now listening on: http://[::]:80
Application started. Press Ctrl+C to shut down.
```

Next, you will want to view the ASP.NET Core application in your browser, as described by the following instructions. The instructions for macOS and Linux directly follow. The instructions for Windows are in the next section.

After the application starts, navigate to `http://localhost:8000` in your web browser.

Note: The `-p` argument maps port 8000 on your local machine to port 80 in the container (the form of the port mapping is `host:container`). See the [Docker run reference](https://docs.docker.com/engine/reference/commandline/run/) for more information on commandline parameters.

Multiple variations of this sample have been provided, as follows. Some of these example Dockerfiles are demonstrated later. Specify an alternate Dockerfile via the `-f` argument.

* [Multi-arch sample](Dockerfile)
* [Linux ARM32 (Raspberry Pi) sample](Dockerfile.linux-arm32)

### View the ASP.NET Core app in a running container on Windows

After the application starts, navigate to the container IP (as opposed to http://localhost) in your web browser with the the following instructions:

1. Open up another command prompt.
1. Run `docker exec aspnetcore_sample ipconfig`.
1. Copy the container IP address and paste into your browser (for example, `172.29.245.43`).

See the following example of how to get the IP address of a running Windows container.

```console
C:\git\dotnet-docker\samples\aspnetapp>docker exec aspnetcore_sample ipconfig

Windows IP Configuration


Ethernet adapter Ethernet:

   Connection-specific DNS Suffix  . : contoso.com
   Link-local IPv6 Address . . . . . : fe80::1967:6598:124:cfa3%4
   IPv4 Address. . . . . . . . . . . : 172.29.245.43
   Subnet Mask . . . . . . . . . . . : 255.255.240.0
   Default Gateway . . . . . . . . . : 172.29.240.1
```

Note: [`docker exec`](https://docs.docker.com/engine/reference/commandline/exec/) supports identifying containers with name or hash. The container name is used in the preceding instructions. `docker exec` runs a new command (as opposed to the [entrypoint](https://docs.docker.com/engine/reference/builder/#entrypoint)) in a running container.

Some people prefer using `docker inspect` for this same purpose, as demonstrated in the following example.

```console
C:\git\dotnet-docker\samples\aspnetapp>docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" aspnetcore_sample
172.25.157.148
```

## Build and run the sample for Linux ARM32 with Docker

You need to build the [sample](Dockerfile.linux-arm32) on an X64 machine. This requirement is due to the .NET Core SDK not being currently supported on ARM32. The instructions assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp -f Dockerfile.arm32 .
```

After building the image, you need to push the image to a container registry so that you can pull it from an ARM32 device. Full instructions are provided at [Build .NET Core Applications for Raspberry Pi with Docker](dotnet-docker-arm32.md).

Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](../dotnetapp/push-image-to-acr.md)
* [Push Docker Images to DockerHub](../dotnetapp/push-image-to-dockerhub.md)

You next need to pull and run the image you pushed to the registry.

If you pushed the image to DockerHub, the `docker run` command would look similar to the following.

```console
docker pull richlander/aspnetapp
docker run --rm -p 8000:80 richlander/aspnetapp
```

If you pushed the image to Azure Container Registry, the `docker run` command would look similar to the following. Remember to `docker login` to Azure Container Registry first, as demonstrated in [Push Docker Images to Azure Container Registry](../dotnetapp/push-image-to-acr.md).

```console
docker pull richlander.azurecr.io/aspnetapp
docker run --rm -p 8000:80 richlander.azurecr.io/aspnetapp
```

After the application starts, visit the site one of two ways:

* From the web browser on the ARM32 device at `http://localhost:8000`
* From the web browser on another device on the same network on the ARM32 device IP on port 8000, similar to: `http://192.168.1.18:8000`

## Build and run the sample locally

You can build and run the sample locally with the [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/core) using the following commands. The commands assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
dotnet run
```

After the application starts, visit `http://localhost:8000` in your web browser.

You can produce an application that is ready to deploy to production locally using the following command.

```console
dotnet publish -c release -o out
```

You can run the application using the following commands.

```console
cd out
dotnet aspnetapp.dll
```

Note: The `-c release` argument builds the application in release mode (the default is debug mode). See the [dotnet run reference](https://docs.microsoft.com/dotnet/core/tools/dotnet-run) for more information on commandline parameters.

## .NET Core Resources

More Samples

* [.NET Core Docker Samples](../README.md)
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
