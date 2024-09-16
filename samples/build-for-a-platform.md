# Building images for a specific platform

Docker exposes [multiple ways to interact with platforms](https://docs.docker.com/build/building/multi-platform/). Sometimes this will result in the images you want and sometimes not depending on how you structure your Dockerfiles and your use of the `docker` CLI. The most common scenario for needing to pay attention to platform targeting is if you have an `arm64` development machine (like Apple M1/M*) and are pushing images to an `x64` cloud. This equally applies to `docker run` and `docker build`.

In Docker terminology, `platform` refers to operating system + architecture. For example the combination of Linux and `x64` is a platform, described by the `linux/amd64` platform string. The `linux` part is relevant, however, the most common use for platform targeting is controlling the architecture, choosing (primarily) between `amd64` and `arm64`.

This document covers the different ways you can target a specific platform or multiple platforms when building .NET container images.

Notes:

- `amd64` is used for historical reasons and is synonymous with `x64`, however, `x64` is not an accepted alias.
- .NET tags are described in [.NET Container Tags -- Patterns and Policies](../documentation//supported-tags.md).
- This document applies to Linux containers only. Windows .NET containers only support `x64`. Additionally, most of the examples on this page require BuildKit, which is not currently supported for Windows containers.

## Single-platform Dockerfiles

The default scenario is using multi-platform tags, which will work in multiple environments.

For example, using `FROM` statements like the following:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
```

A Dockerfile following this pattern can be built with the following command:

```bash
docker build -t app .
```

It can be built on any supported operating system and architecture. For example, if built on a Mac with Apple Silicon, Docker will produce a `linux/arm64` image.

You can inspect an image with `docker inspect` to determine OS and architecture target.

```bash
$ docker inspect app -f "{{.Os}}/{{.Architecture}}"
linux/amd64
```

You can also do this with base images (that you have pulled):

```bash
$ docker inspect mcr.microsoft.com/dotnet/runtime:9.0 -f "{{.Os}}/{{.Architecture}}"
linux/amd64
```

Windows Containers include extra version information:

```pwsh
> docker inspect mcr.microsoft.com/dotnet/runtime:9.0-nanoserver-ltsc2022 -f "{{.Os}}/{{.Architecture}}"
windows/amd64
> docker inspect mcr.microsoft.com/dotnet/runtime:9.0-nanoserver-ltsc2022 -f "{{.OsVersion}}"
10.0.20348.2700
```

This model works very well given a homogenous compute environment. For example, it works well if dev, CI, and prod machines are all `x64`. However, it doesn't as well in heterogenous environments, like if dev machines are `arm64` and prod machines are `x64`. This is because Docker defaults to the native architecture, but that means that resulting images might not match.

## Multi-platform Dockerfiles with `--platform`

.NET supports multi-platform image builds via cross-compilation. This is the recommended approach for .NET Dockerfiles, and is the pattern all of our samples use. To get started, check out the Docker [multi-platform build prerequisites](https://docs.docker.com/build/building/multi-platform/#prerequisites).

[BuildKit](https://docs.docker.com/build/buildkit/), the default builder for Docker, [exposes multiple environment variables](https://docs.docker.com/reference/dockerfile/#automatic-platform-args-in-the-global-scope) that can be used to customize multi-platform container builds. All .NET sample Dockerfiles use these variables to support multi-platform builds using the following pattern:

```Dockerfile
# Build stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
```

You can see [one of the sample Dockerfiles](aspnetapp/Dockerfile) for a complete example. Such a Dockerfile can be built using the following commands:

```bash
# Build targeting the current machine's platform
docker build -t app .

# From an amd64 machine, build an arm64 image:
docker buildx build --platform linux/arm64 -t app .

# From an arm64 machine, build an amd64 image:
docker buildx build --platform linux/amd64 -t app .
```

You can also build multiple platforms at once:

```bash
docker buildx build --platform linux/amd64,linux/arm64 -t app .
```

Using this pattern, the SDK will always run natively on the build machine by targeting the `$BUILDPLATFORM`. Then, Docker builds the final stage targeting whatever platform you passed in via the `--platform` argument. This works thanks to .NET's native support for cross-compilation. The build runs on your build machine's architecture and outputs IL for the target architecture. The app is then copied to the final stage without running any commands on the target image - there's no emulation involved.

If you are cross-compiling images, be cautious of using `RUN` instructions on the final image layer. Any instructions in the final layer will be run under emulation when targeting a platform that isn't the same as the build platform. The best practice is to use the final layer for composing the image's filesystem by copying files from other intermediate layers. This model also has the best performance since all computation is run natively. Do not run any `dotnet` commands or executables in the final layer if you are cross-building your Dockerfile, since [.NET doesn't support running under QEMU emulation](#net-and-qemu).

## Locking Dockerfiles to one platform

Another approach is to always build for one platform by using matching architecture-specific tags for each stage. This model has the benefit that Dockerfiles are simple and always produce the same results. It has the downside that it can result in a Dockerfile per platform (in a heterogenous compute environment), requiring users to know which to build.

```Dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine-amd64 AS build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine-amd64
```

Building a Dockerfile with single-platform tags does not require passing the `--platform` argument to the build command.

This pattern results in, for example, `amd64` images always being used. Those images will work on any platform but will require the use of emulation if an `amd64` image is used on `arm64` and vice versa. .NET doesn't support QEMU emulation, as covered later. As a result, this pattern is only appropriate for homogenous environments (all `amd64` or all `arm64`).

## .NET and QEMU

Docker Desktop uses [QEMU](https://www.qemu.org/) for emulation, for example running `x64` code on an `arm64` machine. [.NET doesn't support being run in QEMU](https://github.com/dotnet/core/blob/main/release-notes/8.0/supported-os.md#qemu). That means that the SDK needs to always be run natively, to enable [multi-stage build](https://docs.docker.com/build/building/multi-stage/). Multi-stage build is used by all of our samples.

As a result, we need a reliable pattern that can produce multiple variants of images on one machine, but that doesn't use emulation. That's what this document describes.

> [QEMU context](https://gitlab.com/qemu-project/qemu/-/issues/249)
