# Build .NET Core Self-Contained Applications with Docker

You can build .NET Core self-contained apps with Docker. Self-contained apps are a great option if you do not want to take a dependence on the .NET Runtime, either as a global install or the [.NET Core Runtime Image](https://hub.docker.com/r/microsoft/dotnet/). These instructions are based on the [.NET Core Docker Sample](README.md).

Multiple variations of this sample have been provided, as follows. Some of these example Dockerfiles are demonstrated later. Specify an alternate Dockerfile via the `-f` argument.

* [Nano Server self-contained x64 sample with build and unit testing](Dockerfile.nanoserver-x64-selfcontained)
* [Debian self-contained x64 sample with build and unit testing](Dockerfile.debian-x64-selfcontained)
* [Debian self-contained ARM32 sample with build and unit testing](Dockerfile.debian-arm32-selfcontained)

## Context on the IL Linker

The .NET team has built an [IL linker](https://github.com/dotnet/core/blob/master/samples/linker-instructions.md
) to reduce the size of .NET Core applications. It is built on top of the excellent and battle-tested [mono linker](https://github.com/mono/linker). The Xamarin tools also use this linker.

In trivial cases, the linker can reduce the size of applications by 50%. The size wins may be more favorable or more moderate for larger applications. The linker removes code in your application and dependent libraries that are not reached by any code paths. It is effectively an application-specific dead code analysis.

## Add a Reference to the IL Linker

You first need to add a reference to the [linker package](https://dotnet.myget.org/feed/dotnet-core/package/nuget/Illink.Tasks) to take advantage of IL linking, using the following instructions.

```console
dotnet add package ILLink.Tasks -v 0.1.4-preview-981901 -s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json
```

The various "self-contained" Dockerfiles do this as part of their implementation, as demonstrated in the following examples.

## Building the Sample for Windows Nano Server with Docker

You can build and run the [sample](Dockerfile.nanoserver-x64-selfcontained) in a [Nano Server container](https://hub.docker.com/r/microsoft/nanoserver/) using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp:nanoserver-selfcontained -f Dockerfile.nanoserver-x64-selfcontained .
docker run --rm dotnetapp:nanoserver-selfcontained
```

## Building the Sample for Linux X64 with Docker

You can build and run the [sample](Dockerfile.selfcontained-linux-x64) in a [Debian container](https://hub.docker.com/r/library/debian/) using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp:debian-x64-selfcontained -f Dockerfile.selfcontained-debian-x64 .
docker run --rm dotnetapp:debian-x64-selfcontained
```

## Building the Sample for Linux ARM32 with Docker

Full instructions are provided at [Build .NET Core Applications for Raspberry Pi with Docker](dotnet-docker-arm32.md). Summarized instructions follow.

You need to build the [sample](Dockerfile.debian-arm32-selfcontained)on an X64 machine. This requirement is due to the .NET Core SDK not being currently supported on ARM32. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp:debian-arm32-selfcontained -f Dockerfile.debian-arm32-selfcontained .
```

After building the image, you need to push the image to a container registry so that you can pull it from an ARM32 device.

Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

You next need to pull and run the image you pushed to the registry.

If you pushed the image to DockerHub, the `docker run` command would look similar to the following.

```console
docker run --rm richlander/dotnetapp:debian-arm32-selfcontained
```

If you pushed the image to Azure Container Registry, the `docker run` command would look similar to the following. You need to `docker login` to Azure Container Registry before you can pull images.

```console
docker run --rm richlander.azurecr.io/dotnetapp:debian-arm32-selfcontained
```
