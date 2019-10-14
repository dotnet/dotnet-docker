# Develop .NET Core applications in a container

You can use containers to establish a .NET Core development environment with only Docker and an editor. The environment can be made to match your local machine, production or both. If you support multiple operating systems, then this approach might become an important part of your development process. You can also [develop ASP.NET Core apps in a container](../aspnetapp/aspnet-docker-dev-in-container.md).

This pattern enables you to rerun your application in a container with every local code change. This scenario works for both console applications and websites. The syntax differs for Windows and Linux containers.

## Requirements

This approach relies on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument in the following commands) to mount source into the container (without using a Dockerfile). You may need to [Enable shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

To avoid conflicts between container usage and your local environment, you need to use a different set of `obj` and `bin` folders for your container environment. The easiest way to do that is to copy this [Directory.Build.props](Directory.Build.props) to your project, like with the following:

```console
curl -o Directory.Build.props https://raw.githubusercontent.com/dotnet/dotnet-docker/master/samples/dotnetapp/Directory.Build.props
```

The instructions assume that you have cloned the repository to a specific directory, as demonstrated by the examples.

## Windows using Linux containers

```console
docker run --rm -it -v c:\git\dotnet-docker\samples\dotnetapp:/app/ -w /app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch run
```

## Linux or macOS using Linux containers

```console
docker run --rm -it -v ~/git/dotnet-docker/samples/dotnetapp:/app/ -w /app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch run
```

## Windows using Windows containers

```console
docker run --rm -it -v c:\git\dotnet-docker\samples\dotnetapp:c:\app\ -w \app mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch run
```

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
