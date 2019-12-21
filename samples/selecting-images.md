# Selecting .NET Core container images

There are many .NET Core images that are available that you can use. Some are very general and others are intended to satisy more specific needs. Together, they satisfy a wide variety of scenarios.

You can use the referenced images and tags with the docker CLI, for example with `docker pull` or `docker run`, or as part of a FROM statement within a Dockerfile.

## Repos

There are multiple [.NET Core Docker repos](https://hub.docker.com/_/microsoft-dotnet-core) that expose various layers of the .NET Core platform.

* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/) -- Linux-only images that contains the package manager dependencies of .NET Core. Best used for self-contained applications.
* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/) -- Images that contains the .NET Core runtime. Best used for console applications. On Linux, depends on the `runtime-deps` image.
* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/) -- Images that contains the ASP.NET Core runtime. Best used for web aplications and services. Depends on the `runtime` image.
* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/) -- An image that contains the .NET Core SDK (which includes tools and all runtimes). Best used for building and testing applications. Depends on [buildpack-deps](https://hub.docker.com/_/buildpack-deps) for Debian and Ubuntu, on [dotnet/core/aspnet] for Alpine and on [nanoserver](https://hub.docker.com/_/microsoft-windows-nanoserver) for Windows.

The repos above are commonly used on the command line and in Dockerfiles. There are two more repos that may be useful to you:

* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly) -- A duplicate structure of repos for nightly images (which are not supported in production).
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples) -- A set of samples that demonstrate .NET Core being used in console and web scenarios.

## Tags that work everywhere

Each repo exposes a set of tags you can use. There are a set of version number tags, like `3.1`, that you can use on multiple operating systems and are supported on most processor types (x64, ARM64 and ARM32). If you don't see an operating system or processor type in the tag, you know it's a "multi-arch" tag that will work everywhere. 

When you pull these tags, you will get a Debian image for Linux and Windows Nano Server images on Windows (if you are using Windows containers). If you are happy with that behavior, then these are the easiest tags to use and enable you to write Dockerfiles that can be built on multiple machines. However, the images you produce may differ across environments (which may or may not be what you want).

For example, the following command will work in all supported environments:

```console
docker run --rm mcr.microsoft.com/dotnet/core/runtime:3.1 dotnet
```

Similarly, you can build an image with the following `FROM` statement:

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
```

This will work on all operating systems and on all supported chips, but building this Dockerfile on Windows x64 will produce a different image than on Linux ARM64 and they are not interchangeable.

## Targeting a specific operating system

If you want a specific operating system image, you should use a specific operating system tag. We publish images for Alpine, Debian, Ubuntu and Windows Nano Server. By default, operating system-specific tags will pull x64 images.

The following tags demonstrate the pattern used to describe each operating system (using .NET Core 3.1 as the example):

* `3.1-alpine` (Alpine 3.10)
* `3.1-bionic` (Ubuntu 18.04)
* `3.1-buster` (Debian 10)
* `3.1-nanoserver-1910` (Nano Server, version 1910)
* `3.1-nanoserver-1903` (Nano Server, version 1903)
* `3.1-nanoserver-1809` (Nano Server, version 1809)
* `3.1-nanoserver-1803` (Nano Server, version 1803)

For example, the following command will pull an x64 Alpine image:

```console
docker run --rm mcr.microsoft.com/dotnet/core/runtime:3.1-alpine dotnet
```

## Targeting a specific processor type

If you want an image of a specific processor type, you should use a specific processor architecture tag. We publish tags for x64, ARM64 and ARM32.

The following tags demonstrate the pattern used to describe each processor, using the same operating systems listed above.

### x64

* `3.1-alpine`
* `3.1-bionic`
* `3.1-buster`
* `3.1-nanoserver-1910`
* `3.1-nanoserver-1903`
* `3.1-nanoserver-1809`
* `3.1-nanoserver-1803`

### ARM64

* `3.1-alpine-arm64v8`
* `3.1-bionic-arm64v8`
* `3.1-buster-arm64v8`

### ARM32

* `3.1-alpine-arm32v7`
* `3.1-bionic-arm32v7`
* `3.1-buster-arm32v7`
* `3.1-nanoserver-1809-arm32v7`
