# .NET Core Docker Sample

This sample demonstrates how to build container images for .NET Core console apps, for Linux and Windows containers, and for x64, ARM32 and ARM64 architectures. It requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or a later version of the [Docker client](https://www.docker.com/products/docker).

The sample builds the application in a container based on the larger [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). It builds the application and then copies the final build result into a Docker image based on the smaller [.NET Core Docker Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/).

## Try a pre-built .NET Core Docker Image

You can run a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/) with the following command.

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

## Build and run container image

You can build and run the [sample Dockerfile](Dockerfile) in Docker, by cloning the repo and using the following commands. The instructions assume that you are in the root of the repository. They should work on any operating system and with x64, ARM32 or ARM64 processors.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
docker run --rm dotnetapp Hello .NET Core from Docker
```

## Targeting an operating system





## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
