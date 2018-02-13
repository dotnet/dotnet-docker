# Build .NET Core Self-Contained Applications with Docker

You can build .NET Core self-contained apps with Docker. Self-contained apps are a great option if you do not want to take a dependence on the .NET Runtime, either as a global install or the [.NET Core Runtime Image](https://hub.docker.com/r/microsoft/dotnet/). These instructions are based on the [.NET Core Docker Sample](README.md).

## Context on the IL Linker

The .NET team has built an [IL linker](https://github.com/dotnet/core/blob/master/samples/linker-instructions.md
) to reduce the size of .NET Core applications. It is built on top of the excellent and battle-tested [mono linker](https://github.com/mono/linker). The Xamarin tools also use this linker.

In trivial cases, the linker can reduce the size of applications by 50%. The size wins may be more favorable or more moderate for larger applications. The linker removes code in your application and dependent libraries that are not reached by any code paths. It is effectively an application-specific dead code analysis.

## Add a Reference to the IL Linker

You first need to add a reference to the [linker package](https://dotnet.myget.org/feed/dotnet-core/package/nuget/Illink.Tasks), using the following instructions: The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
cd dotnetapp
dotnet add package ILLink.Tasks -v 0.1.4-preview-981901 -s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json
```

You can also update the Dockerfile to add the package reference, although that's not recommended as a long-term option (invalidates Docker cache with every build).

## Building the Sample for Windows Nanoserver with Docker

You can build and run the [sample](Dockerfile.selfcontained-nanoserver-x64) on Nanoserver using the following commands: The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build -t dotnetapp -f Dockerfile.selfcontained-nanoserver-x64 .
docker run dotnetapp
```

## Building the Sample for Linux X64 with Docker

You can build and run the [sample](Dockerfile.selfcontained-linux-x64) on Linux X64 using the following commands: The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build -t dotnetapp -f Dockerfile.selfcontained-linux-x64 .
docker run dotnetapp
```

## Building the Sample for Linux ARM32 with Docker

You need to build the [sample](Dockerfile.selfcontained-linux-arm32) on Windows. This requirement is due to the .NET Core SDK not being currently supported on ARM32. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build -t dotnetapp -f Dockerfile.selfcontained-linux-arm32 .
```

After building the image, you need to push the image to a container registry so that you can pull it from an ARM32 device.

Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

Full instructions are provided at [Build .NET Core Applications for Raspberry Pi with Docker](dotnet-docker-arm32.md).

You next need to pull and run the image you pushed to the registry.

If you pushed the image to DockerHub, the `docker run` command would look similar to the following.

```console
docker run --rm richlander/dotnetapp
```

If you pushed the image to Azure Container Registry, the `docker run` command would look similar to the following.

```console
docker run --rm richlander.azurecr.io/dotnetapp
```
