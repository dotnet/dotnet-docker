# .NET Core Docker Sample

This sample demonstrates how to build container images for .NET Core console apps, for Linux and Windows containers, and for x64, ARM32 and ARM64 architectures. It requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or a later version of the [Docker client](https://www.docker.com/products/docker).

The sample builds the application in a container based on the larger [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). It builds the application and then copies the final build result into a Docker image based on the smaller [.NET Core Docker Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/).

## Try a pre-built .NET Core Docker Image

You can run a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/) with the following command.

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

## Build and run container image

You can build and run the [sample Dockerfile](Dockerfile) in Docker, by cloning the repo and using the following commands. The instructions assume that you are in the root of the repository. 

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
docker run --rm dotnetapp Hello .NET Core from Docker
```

## Relying on tags that work everywhere

The .NET Core team provides a set of version number tags, like the `3.1` tag` that you can use on multiple operating system and are supported on most processor types (x64, ARM64 and ARM32). If you don't see an operating system or processor type in the tag, you know it's a "multi-arch" tag that will work everywhere. These tags, when you pull them, will result in Debian images for Linux and Windows Nano Server on Windows (if you are using Windows containers). If you are happy with that behavior, they are the easiest tags to use and enable you to write Dockerfiles that can be built on multiple machines. However, the images produces may differ across environments (which may or may not be what you want).

For example, the following command will work in all supported environments:

```console
docker run --rm mcr.microsoft.com/dotnet/core/runtime:3.1 dotnet
```

## Targeting a specific  operating system

If you want a specific operating system image all the time, you should use a specific tag to ensure you always get what you want. We publish images for Alpine, Debian, Ubuntu and Windows Nano Server. By default, operating system-specific tags will pull x64 images.

The following tags demonstrate the pattern used to describe each operating system (using .NET Core 3.1 as the example):

* `3.1-alpine` (Debian 3.10)
* `3.1-bionic` (Ubuntu 18.04)
* `3.1-buster` (Debian 10)
* `3.1-nanoserver-1903` (Nano Server, version 1903)
* `3.1-nanoserver-1809` (Nano Server, version 1809)
* `3.1-nanoserver-1803` (Nano Server, version 1803)

For example, the following command will always pull an Alpine image and will only work on x64:

```console
docker run --rm mcr.microsoft.com/dotnet/core/runtime:3.1-alpine dotnet
```

## Targeting a specific processor type

If you want a specific processor type all the time, you should use a specific tag to ensure you always get what you want. We publish tags for x64, ARM64 and ARM32.

The following tags demonstrate the patter used to describe each processor, using the same operating systems listed above.

### x64

* `3.1-alpine` (Debian 3.10)
* `3.1-bionic` (Ubuntu 18.04)
* `3.1-buster` (Debian 10)
* `3.1-nanoserver-1903` (Nano Server, version 1903)
* `3.1-nanoserver-1809` (Nano Server, version 1809)
* `3.1-nanoserver-1803` (Nano Server, version 1803)

### ARM64

* `3.1-alpine-arm64v8` (Debian 3.10)
* `3.1-bionic-arm64v8` (Ubuntu 18.04)
* `3.1-buster-arm64v8` (Debian 10)
* `3.1-nanoserver-1903-arm64v8` (Nano Server, version 1903)
* `3.1-nanoserver-1809-arm64v8` (Nano Server, version 1809)
* `3.1-nanoserver-1803-arm64v8` (Nano Server, version 1803)

### ARM32

* `3.1-alpine-arm32v7` (Debian 3.10)
* `3.1-bionic-arm32v7` (Ubuntu 18.04)
* `3.1-buster-arm32v7` (Debian 10)
* `3.1-nanoserver-1809-arm32v7` (Nano Server, version 1809)

## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
