## Building .NET Core Docker Images for Windows

You can build .NET Core Docker images for Windows. The instructions provided use a x64 Windows [.NET Core Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/). You can also target [ARM processors](dotnet-docker-arm64.md) or [.NET Core self-contained applications](dotnet-docker-selfcontained.md).

The more general examples demonstrated in [.NET Core sample](README.md) might be a better starting point if you are new to using .NET Core with containers.

## Build a Nano Server image

The following instructions show how to build an image that is based on x64 [Nano Server](https://hub.docker.com/_/microsoft-windows-nanoserver):

```console
docker build --pull -t dotnetapp -f Dockerfile.nanoserver-x64 .
docker run --rm dotnetapp Hello .NET Core from Nano Server]
```

## Build an ARM32 image

If you want to target ARM32, you can instead use the [Dockerfile.nanoserver-arm32](Dockerfile.nanoserver-arm32) file,with the following command:

```console
docker build --pull -t dotnetapp -f Dockerfile.nanoserver-arm32 .
```

## Target an older Nano Server version

You may need to target an earlier version of Nano Server, due to [Windows container compatiblity requirements](https://docs.microsoft.com/virtualization/windowscontainers/deploy-containers/version-compatibility). You can update the Dockerfile to pull a different Nano Server base image.

This is the key line frm [Dockerfile.nanoserver-x64](Dockerfile.nanoserver-x64) that selects the Nano Server version:

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/runtime:3.0-nanoserver
```

You will notice that there is no version specified in that tag. You can instead add a version that is supported on the [.NET Core Runtime Repo](https://hub.docker.com/_/microsoft-dotnet-core-runtime/), like with the following:


```Dockerfile
FROM mcr.microsoft.com/dotnet/core/runtime:3.0-nanoserver-1803
```

This Dockerfile segment would pull a Windows Server, version 1803 x64 base image.

## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
