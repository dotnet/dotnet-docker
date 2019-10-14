# Build .NET Core Self-Contained Applications with Docker

You can build .NET Core [self-contained apps](https://docs.microsoft.com/dotnet/core/deploying/) with Docker. Self-contained apps are a great option if you do not want to take a dependence on an [.NET Core Runtime image layer](https://hub.docker.com/_/microsoft-dotnet-core-runtime/).

The Dockerfiles used in this document make use of the assembly linker and crossgen assembly compiler in the .NET Core SDK. The linker reduces the size of your application, and crossgen compiles your application to native code to improve startup. Using crossgen increases the size of your application, so we recommend not using it is size is reduction is your highest priority. Crossgen is currently not supported on Alpine.

## Build an Alpine image

The following instructions show how to build self-contained app that based on an x64 [Alpine](https://hub.docker.com/_/alpine/)-based image:

```console
docker build --pull -t dotnetapp -f Dockerfile.alpine-x64-selfcontained .
docker run --rm -it dotnetapp Hello .NET Core from Alpine
```

## Build a Debian image

The following instructions show how to build self-contained app that based on an x64 [Debian](https://hub.docker.com/_/debian/)-based image:

```console
docker build --pull -t dotnetapp -f Dockerfile.debian-x64-selfcontained .
docker run --rm -it dotnetapp Hello .NET Core from Debian
```

## Build an Ubuntu image

The following instructions show how to build self-contained app that based on an x64 [Ubuntu](https://hub.docker.com/_/ubuntu/)-based image:

```console
docker build --pull -t dotnetapp -f Dockerfile.ubuntu-x64-selfcontained .
docker run --rm -it dotnetapp Hello .NET Core from Ubuntu
```

## Building a Windows Nano Server Image

The following instructions show how to build self-contained app that based on an x64 [Nano Server](https://hub.docker.com/_/microsoft-windows-nanoserver/)-based image:

```console
docker build --pull -t dotnetapp -f Dockerfile.nanoserver-x64-selfcontained .
docker run --rm dotnetapp
```

Note: The Nano Server dockerfile targets the latest version of Nano Server. It can be modified to target an earlier Nano Server version.

## Targeting ARM Processors

You can update the example Dockerfiles to work for [ARM processors](dotnet-docker-arm64.md). There are two parts that need to be changed:

 * Runtime (RID) targeting
 * Base image to pull

Let's look at the key lines from  [Dockerfile.debian-x64-selfcontained](Dockerfile.debian-x64-selfcontained) that need to be changed to support ARM64 instead of x64.

These two lines:

```Dockerfile
RUN dotnet publish -c release -o /app -r linux-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true
```

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.0-buster-slim
```

Need to be changed to:

```Dockerfile
RUN dotnet publish -c release -o /app -r linux-arm64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true
```

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.0-buster-slim-arm64v8
```

If you instead want to target ARM32, then you need these are the correct two lines:

```Dockerfile
RUN dotnet publish -c release -o /app -r linux-arm --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true
```

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.0-buster-slim-arm32v7
```

The same pattern can be applied to Ubuntu and Alpine, although .NET Core is only supported on 64-bit ARM processors for Alpine.

## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
