# Develop .NET Core Applications in a Container

Docker containers can provide a local .NET Core build environment without having to install anything on your machine. This scenario is also useful if you develop on an operating system that is different than the one you build on. Application building in a container, unlike most container scenarios, can work well for both client and server applications.

The goal isn't to produce a Docker image, but build binaries from source on your disk. After running a build in this way, the binaries will be on your disk, not in a Docker container.

There are a few ways to use Docker for containerized builds. For simple scenarios, you can use a combination of docker run and volume mounting, as is done with [Develop .NET Core Applications in a Container](dotnet-docker-dev-in-container.md). For more complex scenarios, you need to first build a custom Dockerfile or build to a stage of an existing Dockerfile, as is done with [Running .NET Core Unit Tests with Docker](dotnet-docker-unit-testing.md).

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with [git](https://git-scm.com/downloads), using the following instructions:

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Requirements

It is recommended that you add a [Directory.Build.props](Directory.Build.props) file to your project to use different `obj` and `bin` folders for local and container use, to avoid conflicts between them. You should delete your existing obj and bin folders before making this change. You can also use `dotnet clean` for this purpose.

This approach relies on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument in the following commands) to mount source into the container (without using a Dockerfile). You may need to [Enable shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

## Build .NET source with Docker run

You can build your application with the .NET Core SDK Docker image. You will find built assets in the `out` directory.

The instructions assume that you are in the root of the repository. You can use the following commands, given your environment:

**Windows** using **Linux containers**

```console
docker run --rm -v c:\git\dotnet-docker\samples\dotnetapp:/app -w /app/dotnetapp microsoft/dotnet:2.0-sdk dotnet publish -c release -o out
```

**Linux or macOS** using **Linux containers**

```console
docker run --rm -v ~/git/dotnet-docker/samples/dotnetapp:/app -w /app/dotnetapp microsoft/dotnet:2.0-sdk dotnet publish -c release -o out
```

**Windows** using **Windows containers**

```console
docker run --rm -v c:\git\dotnet-docker\samples\dotnetapp:c:\app -w c:\app\dotnetapp microsoft/dotnet:2.0-sdk dotnet publish -c release -o out
```

You can now run the application on your local machine, assuming you are at the root of the repository:

```console
cd samples
cd dotnetapp
cd dotnetapp
cd out
dotnet dotnetapp.dll
```

> Note: Application built for a different .NET Core version that you have on your machine or for a specific runtime (`-r` argument) may or may not run on your machine.

## Build .NET source with Docker build and extract binaries with Docker run

More complex applications may require more complex builds than `dotnet publish` on its own provides. This may be accomplished with [multi-stage build](https://docs.docker.com/develop/develop-images/multistage-build/) or other approaches. The following commands demonstrate how to enable this scenario. The instructions assume that you are in the root of the repository.

Build the docker image. The assumption in this example is that an exiting [Dockerfile](Dockerfile) is being used, with a particular stage being targeted.

```console
docker build --pull --target publish -t dotnetapp:publish .
```

Next, extract the built binaries and place them in a new `out` directory on your machine.

You can use the following commands, given your environment:

**Windows** using **Linux containers**

```console
docker run --rm -v c:\git\dotnet-docker\samples\dotnetapp\out:/app/out -w /app dotnetapp:publish cp -r dotnetapp/out .
```

**Linux or macOS** using **Linux containers**

```console
docker run --rm -v ~/git/dotnet-docker/samples/dotnetapp/out:/app/out -w /app dotnetapp:publish cp -r dotnetapp/out .
```

**Windows** using **Windows containers**

```console
mkdir samples\dotnetapp\out
docker run --rm -v c:\git\dotnet-docker\samples\dotnetapp\out:c:\app\out -w \app dotnetapp:publish cmd /C "copy /Y dotnetapp\out out"
```

You can now run the application on your local machine, assuming you are at the root of the repository:

```console
cd samples
cd dotnetapp
cd out
dotnet dotnetapp.dll
```

> Note: Application built for a different .NET Core version that you have on your machine or for a specific runtime (`-r` argument) may or may not run on your machine.
