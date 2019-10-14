## Building .NET Core Docker Images for Linux

You can build .NET Core container images for multiple Linux distributions. The .NET team produces .NET Core container images for Alpine, Debian, and Ubuntu. These instructions use an x64 Linux [.NET Core Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/). You can also target [ARM processors](dotnet-docker-arm64.md) or [.NET Core self-contained applications](dotnet-docker-selfcontained.md). 

The more general examples demonstrated in [.NET Core sample](README.md) might be a better starting point if you are new to using .NET Core with containers.

## Build an Alpine image

The following instructions show how to build an image that is based on x64 [Alpine](https://hub.docker.com/_/alpine/):

```console
docker build --pull -t dotnetapp -f Dockerfile.alpine-x64 .
docker run --rm dotnetapp Hello .NET Core from Alpine
```

### ARM64 

If you want to target ARM64, you can use the [Dockerfile.alpine-arm64](Dockerfile.alpine-arm64) file instead:

```console
docker build --pull -t dotnetapp -f Dockerfile.alpine-arm64 .
```

Note: [Globalization is disabled](https://github.com/dotnet/announcements/issues/20) by default with Alpine images in order to produce smaller images. You can re-enable globalization if your application relies on it (instructions are in the Alpine Dockerfiles), however the resulting image will be larger.

## Build a Debian image

The following instructions show how to build an image that is based on x64 [Debian](https://hub.docker.com/_/debian/):

```console
docker build --pull -t dotnetapp -f Dockerfile.debian-x64 .
docker run --rm dotnetapp Hello .NET Core from Debian
```

### ARM64

If you want to target ARM64, you can use the [Dockerfile.debian-arm64](Dockerfile.debian-arm64) file instead:

```console
docker build --pull -t dotnetapp -f Dockerfile.debian-arm64 .
```

### ARM32

If you want to target ARM32, you can use the [Dockerfile.debian-arm32](Dockerfile.debian-arm32) file,with the following command instead:

```console
docker build --pull -t dotnetapp -f Dockerfile.debian-arm32 .
```

## Build an Ubuntu image

The following instructions show how to build an image that is based on x64 [Ubuntu](https://hub.docker.com/_/ubuntu/):

```console
docker build --pull -t dotnetapp -f Dockerfile.ubuntu-x64 .
docker run --rm dotnetapp Hello .NET Core from Ubuntu
```

### ARM64

If you want to target ARM64, you can use the [Dockerfile.ubuntu-arm64](Dockerfile.ubuntu-arm64) file instead:

```console
docker build --pull -t dotnetapp -f Dockerfile.ubuntu-arm64 .
```

### ARM32

If you want to target ARM32, you can instead use the [Dockerfile.ubuntu-arm32](Dockerfile.ubuntu-arm32) file,with the following command:

```console
docker build --pull -t dotnetapp -f Dockerfile.ubuntu-arm32 .
```

## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)