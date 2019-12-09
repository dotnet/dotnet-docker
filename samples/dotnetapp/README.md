# .NET Core Docker Sample

This [sample](Dockerfile) demonstrates how to use .NET Core and Docker together. It builds multiple projects and executes unit tests in a container. The sample works with both Linux and Windows containers and can also be used without Docker.

The sample builds the application in a container based on the larger [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). It builds and [tests](dotnet-docker-unit-testing.md) the application and then copies the final build result into a Docker image based on the smaller [.NET Core Docker Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/). It uses Docker [multi-stage build](https://github.com/dotnet/announcements/issues/18) and [multi-arch tags](https://github.com/dotnet/announcements/issues/14).

This sample requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or later of the [Docker client](https://www.docker.com/products/docker).

## Try a pre-built .NET Core Docker Image

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/), based on this [sample](Dockerfile).

Type the following [Docker](https://www.docker.com/products/docker) command:

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with [git](https://git-scm.com/downloads), using the following instructions.

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Build and run the sample with Docker

You can build and run the sample in Docker using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
docker run --rm dotnetapp Hello .NET Core from Docker
```

The commands above run unit tests as part `docker build`. You can also [run .NET unit tests as part of `docker run`](dotnet-docker-unit-testing.md). The following instructions provide you with the simplest way of doing that.

```console
docker build --target testrunner -t dotnetapp:test .
docker run --rm -it dotnetapp:test
```

You can mount a volume while running the image in order to save the test results to your local disk. The instructions to do that are provided in [Running Unit Tests with Docker](dotnet-docker-unit-testing.md)

Multiple variations of this sample have been provided, as follows. Some of these example Dockerfiles are demonstrated later. Specify an alternate Dockerfile via the `-f` argument.

* [Multi-arch sample with build and unit testing](Dockerfile)
* [Alpine x64 sample, with build and unit testing](Dockerfile.alpine-x64)
* [Alpine x64 sample, with Globalization enabled](Dockerfile.alpine-x64-globalization)
* [Alpine x64 self-contained sample, with build and unit testing](Dockerfile.alpine-x64-selfcontained)
* [Nano Server x64 self-contained sample, with build and unit testing](Dockerfile.nanoserver-x64-selfcontained)
* [Debian x64 self-contained sample, with build and unit testing](Dockerfile.debian-x64-selfcontained)
* [Debian ARM32 self-contained sample, with build and unit testing](Dockerfile.debian-arm32-selfcontained)

## Build and run the sample for Alpine with Docker

You can build and run the sample with [Alpine](https://hub.docker.com/_/alpine/) using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp:alpine -f Dockerfile.alpine-x64 .
docker run --rm -it dotnetapp:alpine Hello .NET Core from Alpine
```

[Globalization is disabled](https://github.com/dotnet/announcements/issues/20) by default with Alpine images in order to produce smaller images. You can re-enable globalization if your application relies on it. [Dockerfile.alpine-x64-globalization](Dockerfile.alpine-x64-globalization) enables globalization for Alpine images, but produces larger images.

## Build and run the sample for Ubuntu 18.04 with Docker

You can also build for [Ubuntu 18.04](https://hub.docker.com/_/ubuntu/), with a `bionic` tag. The `bionic` tags are documented at [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/) and [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/). You would switch to use the `3.1-bionic` tag for both the build and runtime phases.

## Build and run the sample for Linux ARM32 with Docker

You can build and run the sample for ARM32 and Raspberry Pi with the [Use .NET Core and Docker on ARM32 and Raspberry Pi](dotnet-docker-arm32.md) instructions.

## Build and run the sample for Linux ARM64 with Docker

You can build and run the sample for ARM64 with the [Use .NET Core and Docker on ARM64](dotnet-docker-arm64.md) instructions.

## Build .NET Core Self-Contained Applications with Docker

You can build [Build .NET Core Self-Contained Applications with Docker](dotnet-docker-selfcontained.md).

## Develop .NET Core Applications in a container

You can develop applications without a .NET Core installation on your machine with the [Develop .NET Core applications in a container](dotnet-docker-dev-in-container.md) instructions. These instructions are also useful if your development and production environments do not match.

## Run Docker Image on Another Device

You can push the image to a container registry so that you can pull and run it on another device. Straightforward instructions are provided for pushing to both Azure Container Registry and DockerHub.

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

## Build and run the sample locally

You can build and run the sample locally with the [.NET Core 3.1 SDK](https://www.microsoft.com/net/download/core) using the following instructions. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
dotnet run Hello .NET Core
```

You can produce an application that is ready to deploy to production using the following command.

```console
dotnet publish -c Release -o out
```

You can run the published application using the following command:

```console
cd out
dotnet dotnetapp.dll
```

Note: The `-c Release` argument builds the application in release mode (the default is debug mode). See the [dotnet publish reference](https://docs.microsoft.com/dotnet/core/tools/dotnet-publish) for more information on commandline parameters.

## .NET Resources

More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)

Docs and More Information:

* [.NET Docs](https://docs.microsoft.com/dotnet/)
* [ASP.NET Docs](https://docs.microsoft.com/aspnet/)
* [dotnet/core](https://github.com/dotnet/core) for starting with .NET Core on GitHub.
* [dotnet/announcements](https://github.com/dotnet/announcements/issues) for .NET announcements.

## Related Docker Hub Repositories

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples
