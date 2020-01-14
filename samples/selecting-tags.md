# Selecting .NET Core tags

There are many .NET Core images that are available that you can use. Some are very general and others are intended to satisfy more specific needs. Together, they satisfy a wide variety of scenarios.

You can use the referenced images and tags with the docker CLI, for example with `docker pull`, `docker run`, or as part of a FROM statement within a Dockerfile.

## .NET Core Docker repos

There are multiple [.NET Core Docker repos](https://hub.docker.com/_/microsoft-dotnet-core) that expose various layers of the .NET Core platform.

* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/) -- Linux-only images that contains the native dependencies of .NET Core. Best used for self-contained applications.
* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/) -- Images that contains the .NET Core runtime. Best used for console applications. On Linux, depends on the `runtime-deps` image.
* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/) -- Images that contains the ASP.NET Core runtime. Best used for web applications and services. Depends on the `runtime` image.
* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/) -- An image that contains the .NET Core SDK (which includes tools and all runtimes). Best used for building and testing applications. Depends on [buildpack-deps](https://hub.docker.com/_/buildpack-deps) for Debian and Ubuntu, on [dotnet/core/aspnet] for Alpine and on [windows/nanoserver](https://hub.docker.com/_/microsoft-windows-nanoserver) for Windows.

The repos above are commonly used on the command line and in Dockerfiles. There are two more repos that may be useful to you:

* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly) -- A duplicate structure of repos which contain the latest pre-released versions of .NET Core. (which are not supported in production).
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples) -- A set of samples that demonstrate .NET Core being used in console and web scenarios.

## Tags that work everywhere

Each repo exposes a set of tags you can use. There are a set of version number tags, like `3.1`, that you can use on multiple operating systems and are supported on most processor types (x64, ARM64 and ARM32). If you don't see an operating system or processor type in the tag, you know it's a [multi-platform](https://www.docker.com/blog/docker-official-images-now-multi-platform/) tag that will work everywhere.

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

If you want a specific operating system image, you should use a specific operating system tag. We publish images for Alpine, Debian, Ubuntu and Windows Nano Server.

The following tags demonstrate the pattern used to describe each operating system (using .NET Core 3.1 as the example):

* `3.1-alpine` (Alpine 3.10)
* `3.1-bionic` (Ubuntu 18.04)
* `3.1-buster` (Debian 10)
* `3.1-buster-slim` (Debian 10)
* `3.1-nanoserver-1909` (Nano Server, version 1909)
* `3.1-nanoserver-1903` (Nano Server, version 1903)
* `3.1-nanoserver-1809` (Nano Server, version 1809)

> Note: Both `buster` and `buster-slim` tags are used, for .NET Core Debian images. The `buster` base image is used for SDK images and `buster-slim` is used by runtime-related images.

For example, the following command will pull an x64 Alpine image:

```console
docker pull mcr.microsoft.com/dotnet/core/runtime:3.1-alpine
```

## Targeting a specific processor type

If you want an image of a specific processor type, you should use a specific processor architecture tag. We publish tags for x64, ARM64 and ARM32. Tags without an architecture suffix are for x64 images.

The following tags demonstrate the pattern used to describe each processor, using the same operating systems listed above.

### x64

* `3.1-alpine`
* `3.1-bionic`
* `3.1-buster`
* `3.1-buster-slim`
* `3.1-nanoserver-1909`
* `3.1-nanoserver-1903`
* `3.1-nanoserver-1809`

### ARM64

* `3.1-alpine-arm64v8`
* `3.1-bionic-arm64v8`
* `3.1-buster-arm64v8`
* `3.1-buster-slim-arm64v8`

### ARM32

* `3.1-alpine-arm32v7`
* `3.1-bionic-arm32v7`
* `3.1-buster-arm32v7`
* `3.1-buster-slim-arm32v7`
* `3.1-nanoserver-1809-arm32v7`

## Matching SDK and Runtime images

As already stated, we offer images for Alpine, Debian and Ubuntu, for Linux. People (and organizations) choose each of these distros for different reasons. Many people likely choose Debian, for example, because it is the default distro (for example, the `3.1` tag in each of the .NET Core Docker repos will pull a Debian image).

For multi-stage Dockerfiles, there are typically at least two tags referenced, an SDK and a runtime tag. You may want to make a conscious choice to make the distros match for those two tags. If you are only targeting Debian, this is easy, because you can just use the simple multi-platform tags we expose (like `3.1`), and you'll always get Debian (when building for Linux containers). If you are targeting Alpine or Ubuntu for your final runtime image (`aspnet` or `runtime`), then you have a choice, as follows:

* Target a multi-platform tag for the SDK (like `3.1`) to make the SDK stage simple and to enable your Dockerfile to be built in multiple environments (with different processor architectures). This is what most of the samples Dockerfiles in this repo do.
* Match SDK and runtime tags to ensure that you are using the same OS (with the associated shell and commands) and package manager for all stages within a Dockerfile.

## Building for your production environment

Each container image is generated for a specific processor architecture and operating system (Linux or Windows). It is important to construct each Dockerfile so that it will produce the image type you need. Docker [multi-platform](https://www.docker.com/blog/docker-official-images-now-multi-platform/) tags can confuse the situation, since they work on multiple platforms (hence the name) and may produce images that map to your build host and not your production environment.

For multi-stage Dockerfiles, there are typically at least two tags referenced, an SDK and a runtime tag. It is fine to use a multi-platform tag for the SDK. That's the pattern used for .NET Core samples. You will pull an SDK image that works on your machine. It is important to define a .NET Core runtime (`runtime-deps`, `runtime`, or `aspnet`) that matches your production environment.

Linux containers are flexible. As long as the processor architecture matches, you can run Alpine, Debian and Ubuntu (the distros we produce images for) in any environment that supports Linux containers. [Windows images are more restricted](https://docs.microsoft.com/virtualization/windowscontainers/deploy-containers/version-compatibility). You cannot load containers for newer Windows versions on older hosts. For the best experience, the Windows container version should match the host Windows version.

There are multiple patterns used in the samples:

* Multi-platform tags for both SDK and runtime (can be built and run in any single environment) -- see [dotnetapp/Dockerfile](dotnetapp/Dockerfile) and [aspnetapp/Dockerfile](dotnetapp/Dockerfile)
* Multi-platform tag for the SDK and architecture-specific Linux runtime tag (can be built on any environment that supports Linux containers and run in any processor-specific environment that supports Linux containers) -- see [dotnetapp/Dockerfile.alpine-x64](dotnetapp/Dockerfile) and [aspnetapp/Dockerfile.alpine-arm64](aspnetapp/Dockerfile.alpine-arm64)
* Multi-platform tag for the SDK and a Windows-version-specific runtime tag (can be built on any environment that supports Windows containers and run in any processor-specific environment that supports the specific Windows versions) -- see [dotnetapp/Dockerfile.nanoserver-x64](dotnetapp/Dockerfile.nanoserver-x64) and [aspnetapp/Dockerfile.nanoserver-arm32](aspnetapp/Dockerfile.nanoserver-arm32)
