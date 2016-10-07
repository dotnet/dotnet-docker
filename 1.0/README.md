![](https://avatars0.githubusercontent.com/u/9141961?v=3&amp;s=100)

.NET Core Docker Image
====================

This repository contains `Dockerfile` definitions for [dotnet/cli](https://github.com/dotnet/cli) Docker images.

This project is part of .NET Core command-line (CLI) tools. You can find samples, documentation, and getting started instructions for .NET Core CLI tools on our [getting started](http://go.microsoft.com/fwlink/?LinkID=798306&clcid=0x409) page.

[![Downloads from Docker Hub](https://img.shields.io/docker/pulls/microsoft/dotnet.svg)](https://hub.docker.com/r/microsoft/dotnet)
[![Stars on Docker Hub](https://img.shields.io/docker/stars/microsoft/dotnet.svg)](https://hub.docker.com/r/microsoft/dotnet)


## Supported tags

### Development images
-       [`1.0.0-preview2-sdk`, `latest` (*1.0.0-preview2/debian/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/debian/Dockerfile)
-       [`1.0.0-preview2-nanoserver-sdk`, `nanoserver` (*1.0.0-preview2/nanoserver/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/nanoserver/Dockerfile)
-       [`1.0.0-preview2-onbuild`, `onbuild` (*1.0.0-preview2/debian/onbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/debian/onbuild/Dockerfile)
-       [`1.0.0-preview2-nanoserver-onbuild`, `nanoserver-onbuild` (*1.0.0-preview2/nanoserver/onbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/nanoserver/onbuild/Dockerfile)

### Runtime images
-       [`1.0.1-core`, `1.0-core`, `1-core`, `core` (*1.0/debian/core/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/core/Dockerfile)
-       [`1.0.1-nanoserver-core`, `1.0-nanoserver-core`, `1-nanoserver-core`, `nanoserver-core` (*1.0/nanoserver/core/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/nanoserver/core/Dockerfile)
-       [`1.0.1-core-deps`, `1.0-core-deps`, `1-core-deps`, `core-deps` (*1.0/debian/core-deps/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/core-deps/Dockerfile)

## Image variants

The `microsoft/dotnet` images come in different flavors, each designed for a specific use case.

See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/en-us/dotnet/articles/core/docker/building-net-docker-images) to get an understanding of the different Docker images that are offered and when is the right use case for them.

### `microsoft/dotnet:<version>-sdk`

This image contains the .NET Core SDK which is comprised of two parts:

1. .NET Core
2. .NET Core command line tools

This image is recommended if you are trying .NET Core for the first time, as it allows both developing and running
applications. Use this image for your development process (developing, building and testing applications).

### `microsoft/dotnet:<version>-onbuild`

The most straightforward way to use this image is to use a Docker container as both the build and runtime environment for your application. Creating a simple `Dockerfile` with the following content in the same directory as your project files will compile and run your project:

```dockerfile
FROM microsoft/dotnet:onbuild
```

This image includes multiple `ONBUILD` triggers which should cover most applications. The build will `COPY . /dotnetapp` and `RUN dotnet restore`.

This image also includes the `ENTRYPOINT dotnet run` instruction which will run your application when the Docker image is run.

You can then build and run the Docker image:

```console
$ docker build -t my-dotnet-app .
$ docker run -it --rm --name my-running-app my-dotnet-app
```

### `microsoft/dotnet:<version>-core`

This image contains only .NET Core (runtime and libraries) and it is optimized for running [portable .NET Core applications](https://docs.microsoft.com/en-us/dotnet/articles/core/app-types). If you wish to run self-contained applications, please use the `core-deps` image described below. 

### `microsoft/dotnet:<version>-core-deps`

This image contains the operating system with all of the native dependencies needed by .NET Core. Use this image to:

1. Run a [self-contained](https://docs.microsoft.com/en-us/dotnet/articles/core/app-types) application.
2. Build a custom copy of .NET Core by compiling [coreclr](https://github.com/dotnet/coreclr) and [corefx](https://github.com/dotnet/corefx).

## Windows Containers

  Windows Containers images use the `microsoft/nanoserver` base OS image from Windows Server 2016.  For more information on Windows Containers and a getting started guide, please see: [Windows Containers Documentation](http://aka.ms/windowscontainers).

-       `1.0.0-preview2-nanoserver-sdk`
-       `1.0.0-preview2-nanoserver-onbuild`
-       `1.0-nanoserver-core`
