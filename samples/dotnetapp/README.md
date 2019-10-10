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

## Scenarios

There are many ways in which .NET Core can be used with Docker. The following Dockerfiles demonstrate a variety of scenarios, which might match your needs or give you a sense of further tweaks that are needed.

* [Any OS or architecture](Dockerfile)
* [Alpine ARM64](Dockerfile.alpine-arm64)
* [Alpine x64](Dockerfile.alpine-x64)
* [Alpine x64 self-contained](Dockerfile.alpine-x64-selfcontained)
* [Debian ARM32](Dockerfile.debian-arm32)
* [Debian ARM64](Dockerfile.debian-arm64)
* [Debian x64](Dockerfile.debian-x64)
* [Debian x64 self-contained](Dockerfile.debian-x64-selfcontained)
* [Ubuntu ARM32](Dockerfile.Ubuntu-arm32)
* [Ubuntu ARM64](Dockerfile.Ubuntu-arm64)
* [Ubuntu x64](Dockerfile.Ubuntu-x64)
* [Ubuntu x64 self-contained](Dockerfile.debian-x64-selfcontained)
* [Nano Server ARM32](Dockerfile.nanoserver-arm32)
* [Nano Server x64](Dockerfile.nanoserver-x64)
* [Nano Server x64 self-contained](Dockerfile.nanoserver-x64-selfcontained)

Note: The Docker client checks for and uses a `Dockerfile` file, by default. If one is not found, it is an error. You can also specify an alternate file with th `-f` argument, for example `-f Dockerfile.alpine-x64`. You need to use the `-f` argument to specify the alternately named Dockerfiles listed above.

## Build a .NET Core Alpine image

The following instructions show how to build an image that is based on x64 [Alpine](https://hub.docker.com/_/alpine/):

```console
docker build --pull -t dotnetapp -f Dockerfile.alpine-x64 .
docker run --rm -it dotnetapp Hello .NET Core from Alpine
```

If you want to target ARM64, you can use the [Dockerfile.alpine-arm64](Dockerfile.alpine-arm64) file instead:

```console
docker build --pull -t dotnetapp -f Dockerfile.alpine-arm64 .
```

[Globalization is disabled](https://github.com/dotnet/announcements/issues/20) by default with Alpine images in order to produce smaller images. You can re-enable globalization if your application relies on it (instructions are in the Dockerfile), however the resulting image will be larger.

## Build a .NET Core Debian image

The following instructions show how to build an image that is based on x64 [Debian](https://hub.docker.com/_/debian/):

```console
docker build --pull -t dotnetapp -f Dockerfile.debian-x64 .
docker run --rm -it dotnetapp Hello .NET Core from Debian
```

If you want to target ARM64, you can use the [Dockerfile.debian-arm64](Dockerfile.debin-arm64) file instead:

```console
docker build --pull -t dotnetapp -f Dockerfile.debian-arm64 .
```

If you want to target ARM32, you can use the [Dockerfile.debian-arm32](Dockerfile.debin-arm32) file,with the following command instead:

```console
docker build --pull -t dotnetapp -f Dockerfile.debian-arm32 .
```

## Build a .NET Core Ubuntu image

The following instructions show how to build an image that is based on x64 [Ubuntu](https://hub.docker.com/_/ubuntu/):

```console
docker build --pull -t dotnetapp -f Dockerfile.ubuntu-x64 .
docker run --rm -it dotnetapp Hello .NET Core from Ubuntu
```

If you want to target ARM64, you can use the [Dockerfile.ubuntu-arm64](Dockerfile.ubuntu-arm64) file instead:

```console
docker build --pull -t dotnetapp -f Dockerfile.ubuntu-arm64 .
```

If you want to target ARM32, you can instead use the [Dockerfile.ubuntu-arm32](Dockerfile.ubuntu-arm32) file,with the following command:

```console
docker build --pull -t dotnetapp -f Dockerfile.ubuntu-arm32 .
```

## Build a self-contained image

The instructions above result in building [framework dependent apps](https://docs.microsoft.com/dotnet/core/deploying/) that are based on a .NET runtime image. You can also build [self-contained apps](https://docs.microsoft.com/en-us/dotnet/core/deploying/) and base them directly on an operating system image. The following instructions show how to do that with x64, however, the Dockerfiles can be updated to work for ARM processors as well.


The following instructions show how to build self-contained app that based on an x64 [Alpine](https://hub.docker.com/_/alpine/)-based image:

```console
docker build --pull -t dotnetapp -f Dockerfile.alpine-x64-selcontained .
docker run --rm -it dotnetapp Hello .NET Core from Alpine
```

The following instructions show how to build self-contained app that based on an x64 [Debian](https://hub.docker.com/_/debian/)-based image:

```console
docker build --pull -t dotnetapp -f Dockerfile.ubuntu-x64-selcontained .
docker run --rm -it dotnetapp Hello .NET Core from Debian
```

The following instructions show how to build self-contained app that based on an x64 [Ubuntu](https://hub.docker.com/_/ubuntu/)-based image:

```console
docker build --pull -t dotnetapp -f Dockerfile.ubuntu-x64-selcontained .
docker run --rm -it dotnetapp Hello .NET Core from Ubuntu
```

## .NET Resources

More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
