# Build in a .NET SDK container

You can use Docker to run your build in an isolated environment using the [.NET SDK Docker image](../README.sdk.md).
This is useful to avoid the need to install .NET on the build machine and helps ensure that your build environment is correctly configured (dev, staging, or production).

## Build using a Dockerfile (Requires buildx)

Docker [buildx](https://docs.docker.com/reference/cli/docker/buildx/) has built-in support for [exporting files from the docker build command](https://docs.docker.com/build/building/export/).
Using a Dockerfile to build .NET apps is advantageous because it allows you to specify all of the apps required dependencies and build instructions in one place.

The `dotnetapp` sample contains a sample [Dockerfile](./dotnetapp/Dockerfile.sdk-build) that supports this functionality.
This sample uses a multi-stage Dockerfile with a `FROM scratch` stage.
The Dockerfile copies the build outputs into that stage using the `COPY` instruction.
Then, when you provide the `--output/-o <directory>` argument to the Docker build command, Docker will copy the entire filesystem of the final stage of the image to the specified directory.
Since the sample Dockerfile's final stage is a `FROM scratch` stage, the result is that the build outputs are placed in the specified directory on the host machine's disk.

### Build single-platform binary

From the `samples/dotnetapp` directory:

```pwsh
docker build --pull -f Dockerfile.sdk-build --output ./out .
```

You can also give it a try without cloning this repository:

```pwsh
docker build --pull -f Dockerfile.sdk-build --output ./out 'https://github.com/dotnet/dotnet-docker.git#:samples/dotnetapp'
```

### Build binaries for multiple platforms at once

Taking advantage of Docker buildx, you can cross-build binaries for multiple platforms at once, all without using emulation.
For more info about how this works, see our documentation on [building images for a specific platform](./build-for-a-platform.md).

```pwsh
docker buildx build --pull --platform linux/amd64,linux/arm64 -f ./samples/dotnetapp/Dockerfile.sdk-build --output out ./samples/dotnetapp/
```

Docker buildx will create a separate sub-directory for each target platform:

```pwsh
PS> tree /F out
C:\...\dotnetapp\out
├───linux_amd64
│       dotnetapp
│       dotnetapp.deps.json
│       dotnetapp.dll
│       dotnetapp.pdb
│       dotnetapp.runtimeconfig.json
└───linux_arm64
        dotnetapp
        dotnetapp.deps.json
        dotnetapp.dll
        dotnetapp.pdb
        dotnetapp.runtimeconfig.json
```

## Build by running Docker container directly

If you can't use Docker buildx or don't want to use a Dockerfile, you can build your app by running the SDK image directly and volume mounting your app's source code into the container.
These instructions assume that you have cloned the repository locally, and that you are in the `samples/dotnetapp` directory (due to the volume mounting syntax), as demonstrated by the examples.

### Requirements

This scenario relies on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument) to make source available within the container (to build it). You may need to enable [shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

`dotnet publish` (and `build`) produces native executables for applications. If you use a Linux container, you will build a Linux executable that will not run on Windows or macOS. You can use a [runtime argument](https://docs.microsoft.com/dotnet/core/rid-catalog) (`-r`) to specify the type of assets that you want to publish (if they don't match the SDK container). The following examples assume you want assets that match your host operating system, and use runtime arguments to ensure that.

### Pull SDK image

It is recommended to pull the SDK image before running the appropriate command. This ensures that you get the latest patch version of the SDK. Use the following command:

```console
docker pull mcr.microsoft.com/dotnet/sdk:9.0
```

### Linux

```console
docker run --rm -v $(pwd):/app -w /app mcr.microsoft.com/dotnet/sdk:9.0 dotnet publish -c Release -o out
```

You can see the built binaries with the following command:

```console
$ ls out
dotnetapp  dotnetapp.deps.json  dotnetapp.dll  dotnetapp.pdb  dotnetapp.runtimeconfig.json
```

### macOS

```console
docker run --rm -v $(pwd):/app -w /app mcr.microsoft.com/dotnet/sdk:9.0 dotnet publish -c Release -o out -r osx-x64 --self-contained false
```

You can see the built binaries with the following command:

```console
% ls out
dotnetapp                       dotnetapp.pdb
dotnetapp.deps.json             dotnetapp.runtimeconfig.json
dotnetapp.dll
```

### Windows using Linux containers

The following example uses PowerShell.

```console
docker run --rm -v ${pwd}:/app -w /app mcr.microsoft.com/dotnet/sdk:9.0 dotnet publish -c Release -o out -r win-x64 --self-contained false
```

You can see the built binaries with the following command:

```console
PS C:\git\dotnet-docker\samples\dotnetapp> dir out


    Directory: C:\git\dotnet-docker\samples\dotnetapp\out

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a----         11/2/2020  10:46 AM            449 dotnetapp.deps.json
-a----         11/2/2020  10:46 AM           7680 dotnetapp.dll
-a----         11/2/2020  10:46 AM         141312 dotnetapp.exe
-a----         11/2/2020  10:46 AM           9444 dotnetapp.pdb
-a----         11/2/2020  10:46 AM            152 dotnetapp.runtimeconfig.json
```

### Windows using Windows containers

The following example uses PowerShell.

```console
docker run --rm -v ${pwd}:c:\app -w c:\app mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-ltsc2022 dotnet publish -c Release -o out
```

> [!WARNING]
> From .NET 8 onwards, [.NET multi-platform images are Linux-only](https://learn.microsoft.com/dotnet/core/compatibility/containers/8.0/multi-platform-tags).
> This means Windows containers must all be referenced by a full tag name including the specific Windows version.

You can see the built binaries with the following command:

```console
PS C:\git\dotnet-docker\samples\dotnetapp> dir out


    Directory: C:\git\dotnet-docker\samples\dotnetapp\out

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a----         11/2/2020  10:49 AM            419 dotnetapp.deps.json
-a----         11/2/2020  10:49 AM           8192 dotnetapp.dll
-a----         11/2/2020  10:49 AM         141312 dotnetapp.exe
-a----         11/2/2020  10:49 AM           9440 dotnetapp.pdb
-a----         11/2/2020  10:49 AM            160 dotnetapp.runtimeconfig.json
```

### Building to a separate location

You may want the build output to be written to a separate location than the source directory. That's easy to do with a second volume mount.

The following example demonstrates doing that on macOS:

```console
docker run --rm -v ~/dotnetapp:/out -v $(pwd):/app -w /app mcr.microsoft.com/dotnet/sdk:9.0 dotnet publish -c Release -o /out -r osx-x64 --self-contained false
```

You can see the built binaries with the following command:

```console
> ls ~/dotnetapp
dotnetapp                       dotnetapp.pdb
dotnetapp.deps.json             dotnetapp.runtimeconfig.json
dotnetapp.dll
```

The following PowerShell example demonstrates doing that on Windows (using Linux containers):

```console
mkdir C:\dotnetapp
docker run --rm -v C:\dotnetapp:c:\app\out -v ${pwd}:c:\app -w /app mcr.microsoft.com/dotnet/sdk:9.0 dotnet publish -c Release -o out -r win-x64 --self-contained false
```

You can see the built binaries with the following command:

```console
PS C:\git\dotnet-docker\samples\dotnetapp> dir C:\dotnetapp\


    Directory: C:\dotnetapp

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a----         11/2/2020  10:52 AM            472 dotnetapp.deps.json
-a----         11/2/2020  10:52 AM           7680 dotnetapp.dll
-a----         11/2/2020  10:52 AM         141312 dotnetapp.exe
-a----         11/2/2020  10:52 AM           9452 dotnetapp.pdb
-a----         11/2/2020  10:52 AM            160 dotnetapp.runtimeconfig.json
```

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/samples/README.md)
