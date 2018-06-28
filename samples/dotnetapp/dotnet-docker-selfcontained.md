# Build .NET Core Self-Contained Applications with Docker

You can build .NET Core self-contained apps with Docker. Self-contained apps are a great option if you do not want to take a dependence on the .NET Runtime, either as a global install or the [.NET Core Runtime Image](https://hub.docker.com/r/microsoft/dotnet/). These instructions are based on the [.NET Core Docker Sample](README.md).

Multiple variations of this sample have been provided, as follows. Some of these example Dockerfiles are demonstrated later. Specify an alternate Dockerfile via the `-f` argument.

* [Alpine x64 self-contained sample, with build and unit testing](Dockerfile.alpine-x64-selfcontained)
* [Nano Server x64 self-contained sample, with build and unit testing](Dockerfile.nanoserver-x64-selfcontained)
* [Debian x64 self-contained sample, with build and unit testing](Dockerfile.debian-x64-selfcontained)
* [Debian ARM32 self-contained sample, with build and unit testing](Dockerfile.debian-arm32-selfcontained)

## Context on the IL Linker

The .NET team has built an [IL linker](https://github.com/dotnet/core/blob/master/samples/linker-instructions.md
) to reduce the size of .NET Core applications. It is built on top of the excellent and battle-tested [mono linker](https://github.com/mono/linker). The Xamarin tools also use this linker.

In trivial cases, the linker can reduce the size of applications by 50%. The size wins may be more favorable or more moderate for larger applications. The linker removes code in your application and dependent libraries that are not reached by any code paths. It is effectively an application-specific dead code analysis.

## Add a Reference to the IL Linker

You first need to add a reference to the [linker package](https://dotnet.myget.org/feed/dotnet-core/package/nuget/Illink.Tasks) to take advantage of IL linking, using the following instructions.

```console
dotnet add package ILLink.Tasks -v 0.1.5-preview-1461378 -s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json
```

The various "self-contained" Dockerfiles add the linker package to the sample project, as demonstrated in the following examples.

## Building the Sample for Windows Nano Server with Docker

You can build and run the [sample](Dockerfile.nanoserver-x64-selfcontained) in a [Nano Server container](https://hub.docker.com/r/microsoft/nanoserver/) using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp:nanoserver-selfcontained -f Dockerfile.nanoserver-x64-selfcontained .
```

You can test the image with the following instructions.

```console
docker run --rm dotnetapp:nanoserver-selfcontained
```

## Building the Sample for Linux X64 with Docker

You can build and run the [sample](Dockerfile.selfcontained-linux-x64) in a [Debian container](https://hub.docker.com/r/library/debian/) using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp:alpine-x64-selfcontained -f Dockerfile.alpine-x64-selfcontained .
```

You can test the image with the following instructions.

```console
docker run --rm dotnetapp:alpine-x64-selfcontained
```

## Build and run the sample for Linux ARM32 with Docker

You can build and run the sample for ARM32 and Raspberry Pi with [Build .NET Core Applications for Raspberry Pi with Docker](dotnet-docker-arm32.md) instructions. To create a self-contained application with those instructions, you need to use the [Dockerfile.debian-arm32-selfcontained](Dockerfile.debian-arm32-selfcontained) Dockerfile.

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
