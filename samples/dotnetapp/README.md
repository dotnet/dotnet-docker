# .NET Core Docker Sample

This [sample](Dockerfile) demonstrates how to use .NET Core and Docker together. The sample can be used with Linux and Windows containers, and for x64, ARM32 and ARM64 architectures. It requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or a later version of the [Docker client](https://www.docker.com/products/docker).

The sample builds the application in a container based on the larger [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). It builds the application and then copies the final build result into a Docker image based on the smaller [.NET Core Docker Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/).

## Try a pre-built .NET Core Docker Image

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/), based on this [sample](Dockerfile).

Type the following [Docker](https://www.docker.com/products/docker) command:

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

## Build and run the sample with Docker

You can build and run the sample in Docker, by cloning the repo and using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
docker run --rm dotnetapp Hello .NET Core from Docker
```

Note: These instructions should work on any operating system and any x64, ARM32 or ARM64 processor. The [Dockerfile](Dockerfile) uses multi-arch tags, which enabled it to support many environments.

## More scenarios

There are many ways in which .NET Core can be used with Docker.

* [Building Linux images](dotnet-docker-linux.md)
* [Building Windows images](dotnet-docker-windows.md)
* [Building ARM32 images](dotnet-docker-arm32.md)
* [Building ARM64 images](dotnet-docker-arm32.md)
* [Building optimized self-contained images](dotnet-docker-selfcontainer.md)
* [Developing in a container](dotnet-docker-dev-in-container.md)
* [Unit testing in a container](dotnet-docker-unit-testing.md)
* [Containerizing your build](dotnet-docker-containerizing-build.md)
* [Building ASP.NET Core images](../aspnetapp/README.md)

## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
