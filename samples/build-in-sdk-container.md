# Build in a .NET Core SDK container

You can use Docker to run your build in an isolated environment using the [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). This is useful to either avoid the need to install .NET Core on the build machine or ensure that your environment is correctly configured (dev, staging, or production).

The instructions assume that you have cloned the repository locally, and that you are in the `samples/dotnetapp` directory (due to the volume mounting syntax), as demonstrated by the examples.

## Requirements

This scenario relies on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument) to make source available within the container (to build it). You may need to enable [shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

`dotnet publish` (and `build`) produces native executables for applications. If you use a Linux container, you will build a Linux executable that will not run on Windows or macOS. You can use a [runtime argument](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog) (`-r`) to specify the type of assets that you want to publish (if they don't match the SDK container). The following examples assume you want assets that match your host operating system, and use runtime arguments to ensure that.

## Pull SDK image

It is recommended to pull the SDK image before running the appropriate command. This ensures that you get the latest patch version of the SDK. Use the following command:

```console
docker pull mcr.microsoft.com/dotnet/core/sdk:3.1
```

## Linux

```console
docker run --rm -v $(pwd):/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet publish -c release -o out
```

You can see the built binaries with the following command:

```console
$ ls out
dotnetapp  dotnetapp.deps.json  dotnetapp.dll  dotnetapp.pdb  dotnetapp.runtimeconfig.json
```

## macOS

```console
docker run --rm -v $(pwd):/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet publish -c release -o out -r osx-x64 --self-contained false
```

You can see the built binaries with the following command:

```console
% ls out
dotnetapp			dotnetapp.pdb
dotnetapp.deps.json		dotnetapp.runtimeconfig.json
dotnetapp.dll
```

## Windows using Linux containers

The following example uses PowerShell.

```console
docker run --rm -v ${pwd}:/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet publish -c release -o out -r win-x64 --self-contained false
```

You can see the built binaries with the following command:

```console
PS C:\git\dotnet-docker\samples\dotnetapp> dir out


    Directory: C:\git\dotnet-docker\samples\dotnetapp\out

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---           1/14/2020  9:19 PM            449 dotnetapp.deps.json
-a---           1/14/2020  9:19 PM           7680 dotnetapp.dll
-a---           1/14/2020  9:19 PM         168448 dotnetapp.exe
-a---           1/14/2020  9:19 PM            736 dotnetapp.pdb
-a---           1/14/2020  9:19 PM            146 dotnetapp.runtimeconfig.json
```

## Windows using Windows containers

The following example uses PowerShell.

```console
docker run --rm -v ${pwd}:c:\app -w c:\app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet publish -c Release -o out
```

You can see the built binaries with the following command:

```console
PS C:\git\dotnet-docker\samples\dotnetapp> dir out


    Directory: C:\git\dotnet-docker\samples\dotnetapp\out

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---           1/14/2020  9:25 PM            419 dotnetapp.deps.json
-a---           1/14/2020  9:25 PM           8192 dotnetapp.dll
-a---           1/14/2020  9:25 PM         168448 dotnetapp.exe
-a---           1/14/2020  9:25 PM            744 dotnetapp.pdb
-a---           1/14/2020  9:25 PM            154 dotnetapp.runtimeconfig.json
```

## Building to a separate location

You may want the build output to be written to a separate location than the source directory. That's easy to do with a second volume mount.

The following example demonstrates doing that on macOS:

```console
docker run --rm -v ~/dotnetapp:/out -v $(pwd):/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet publish -c release -o /out -r osx-x64 --self-contained false
```

You can see the built binaries with the following command:

```console
> ls ~/dotnetapp
dotnetapp			dotnetapp.pdb
dotnetapp.deps.json		dotnetapp.runtimeconfig.json
dotnetapp.dll
```

The following PowerShell example demonstrates doing that on Windows (using Linux containers):

```console
mkdir C:\dotnetapp
docker run --rm -v C:\dotnetapp:/out -v ${pwd}:/app -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet publish -c release -o /out -r win-x64 --self-contained false
```

You can see the built binaries with the following command:

```console
PS C:\git\dotnet-docker\samples\dotnetapp> dir C:\dotnetapp\


    Directory: C:\dotnetapp

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---           1/14/2020  9:27 PM            449 dotnetapp.deps.json
-a---           1/14/2020  9:27 PM           7680 dotnetapp.dll
-a---           1/14/2020  9:27 PM         168448 dotnetapp.exe
-a---           1/14/2020  9:27 PM            736 dotnetapp.pdb
-a---           1/14/2020  9:27 PM            146 dotnetapp.runtimeconfig.json

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
