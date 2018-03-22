# Build .NET Core Applications in a Container

You can use containers to establish a .NET Core build environment with only Docker and optionally a source control tool. The environment can be made to match your local machine, production or both. If you support multiple operating systems, then this approach might become a key part of your development process.

The goal is to build binaries from your disk source, not to produce a Docker image. After a successful build, the binaries will be on your disk, not in a Docker container.

You can use the [.NET Core SDK image](https://hub.docker.com/r/microsoft/dotnet/) for your build environment. For more complex environments, you can build your own image and re-use the same pattern demonstrated in the following instructions. [`docker cp`](https://docs.docker.com/engine/reference/commandline/cp/) can be used to copy built binaries from an image. That case is not documented here.

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with [git](https://git-scm.com/downloads), using the following instructions:

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Requirements

It is recommended that you add a [Directory.Build.props](Directory.Build.props) file to your project to use different `obj` and `bin` folders for local and container use, to avoid conflicts between them. Delete your existing obj and bin folders before making this change. You can also use `dotnet clean` for this purpose.

These instructions rely on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument in the following commands) to mount local directories into a container (without using a Dockerfile). You may need to [Enable shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

## Build source with the .NET Core SDK, using `docker run`

Build your application with the .NET Core SDK Docker image. Built assets will be in the `out` directory on your local disk.

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

> Note: Applications built for a different .NET Core version on your machine, or for a specific runtime (`-r` argument) might not run on your machine

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
