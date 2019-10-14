# Containerizing your build

You can use Docker to run your build in an isolated environment using the [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). This approach enables you to build code without installing .NET Core on your machine.

`dotnet publish` (and `build`) produces native executables for applications. If you use a Linux container, you will build a Linux executable that will not run on Windows or macOS. You can use a runtime argument (`-r`) to specify the type of assets that you want to publish. The following examples assume you want assets that match your host operating system, and use runtime arguments to ensure that.

The instructions assume that you have cloned the repository to a specific directory, as demonstrated by the examples.

## Windows using Linux containers

```console
docker run --rm -v c:\git\dotnet-docker\samples\dotnetapp:/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet publish -c release -o out -r win-x64 --self-contained false
```

## Linux using Linux containers

```console
docker run --rm -v ~/git/dotnet-docker/samples/dotnetapp:/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet publish -c release -o out
```

## macOS using Linux containers

```console
docker run --rm -v ~/git/dotnet-docker/samples/dotnetapp:/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet publish -c release -o out -r osx-x64 --self-contained false
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
