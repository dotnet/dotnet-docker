# Containerizing your build

You can use Docker to run your build in an isolated environment using the [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). This is useful to either avoid the need to install .NET Core on the build machine or ensure that your environment is correctly configured (dev, staging, or production).

The instructions assume that you have cloned the repository locally, as demonstrated by the examples.

## Requirements

This scenario relies on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument) to make source available within the container (to build it). You may need to [Enable shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

`dotnet publish` (and `build`) produces native executables for applications. If you use a Linux container, you will build a Linux executable that will not run on Windows or macOS. You can use a runtime argument (`-r`) to specify the type of assets that you want to publish. The following examples assume you want assets that match your host operating system, and use runtime arguments to ensure that.


## Linux

```console
docker run --rm -v ~/git/dotnet-docker/samples/dotnetapp:/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet publish -c release -o out
```

## macOS

```console
docker run --rm -v ~/git/dotnet-docker/samples/dotnetapp:/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet publish -c release -o out -r osx-x64 --self-contained false
```

## Windows using Linux containers

```console
docker run --rm -v c:\git\dotnet-docker\samples\dotnetapp:/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet publish -c release -o out -r win-x64 --self-contained false
```

## Windows using Windows containers

```console
docker run --rm -v c:\git\dotnet-docker\samples\dotnetapp:c:\app -w c:\app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet publish -c Release -o out
```

## Building to a separate location

You may want the build output to be written to a separate location than the source directory. That's easy to do with a second volume mount. The following example demonstrates doing that on macOS.

```console
docker run --rm -v ~/dotnetapp:/out -v ~/git/dotnet-docker/samples/dotnetapp:/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet publish -c release -o /out -r osx-x64 --self-contained false
```

## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
