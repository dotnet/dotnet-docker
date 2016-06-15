![](https://avatars0.githubusercontent.com/u/9141961?v=3&amp;s=100)

.NET Core Docker Image
====================

This repository contains `Dockerfile` definitions for [dotnet/cli](https://github.com/dotnet/cli) Docker images.

This project is part of .NET Core command-line (CLI) tools. You can find samples, documentation, and getting started instructions for .NET Core CLI tools on our [getting started](http://go.microsoft.com/fwlink/?LinkID=798306&clcid=0x409) page.

[![Downloads from Docker Hub](https://img.shields.io/docker/pulls/microsoft/dotnet.svg)](https://registry.hub.docker.com/u/microsoft/dotnet)
[![Stars on Docker Hub](https://img.shields.io/docker/stars/microsoft/dotnet.svg)](https://registry.hub.docker.com/u/microsoft/dotnet)


## Supported tags

### Development images
-       [`0.0.1-alpha`, (*0.0.1-alpha/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/0.0.1-alpha/Dockerfile)
-       [`0.0.1-alpha-onbuild`, (*0.0.1-alpha/onbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/0.0.1-alpha/onbuild/Dockerfile)
-       [`1.0.0-preview1` (*1.0.0-preview1/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview1/Dockerfile)
-       [`1.0.0-preview1-onbuild` (*1.0.0-preview1/onbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview1/onbuild/Dockerfile)
-       [`1.0.0-preview2`, `latest` (*1.0.0-preview2/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/Dockerfile)
-       [`1.0.0-preview2-onbuild`, `onbuild` (*1.0.0-preview2/onbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/onbuild/Dockerfile)

### Runtime images
-       [`1.0.0-rc2-core-deps` (*1.0.0-rc2/core-deps/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-rc2/core-deps/Dockerfile)
-       [`1.0.0-rc2-core` (*1.0.0-rc2/core/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-rc2/core/Dockerfile)
-       [`1.0.0-core-deps`, `core-deps` (*1.0.0/core-deps/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0/core-deps/Dockerfile)
-       [`1.0.0-core`, `core` (*1.0.0/core/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0/core/Dockerfile)

## Image variants

The `microsoft/dotnet` image come in different flavors, each designed for a specific use case.

### `microsoft/dotnet:<version>`

This image contains the .NET Core SDK which is comprised of two parts: 

1. .NET Core
2. .NET Core command line tools

This image is recommended if you are trying .NET Core for the first time, as it allows both developing and running 
applications. Use this image for your development process (developing, building and testing applications). 

### `microsoft/dotnet:<version>-onbuild`

The most straightforward way to use this image is to use a .NET container as both the build and runtime environment. In your `Dockerfile`, writing something along the lines of the following will compile and run your project:

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

### `microsoft/dotnet:<version>-core-deps`

This image contains the operating system with all of the native dependencies needed by .NET Core. Use this image to:

1. Run a [self-contained](http://dotnet.github.io/docs/core-concepts/app-types.html) application.
2. Build a custom copy of .NET Core by compiling [coreclr](https://github.com/dotnet/coreclr) and [corefx](https://github.com/dotnet/corefx).

### `microsoft/dotnet:<version>-core`

This image contains only .NET Core (runtime and libraries) and it is optimized for running [portable .NET Core applications](http://dotnet.github.io/docs/core-concepts/app-types.html). If you wish to run self-contained applications, please use the `core-deps` image described above. 
