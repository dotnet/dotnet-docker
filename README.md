# Latest Version of Common Tags

The following tags are the latest stable versions of the most commonly used images. The complete set of tags is listed further down.

- [`2.0-sdk`](https://github.com/dotnet/dotnet-docker/blob/master/2.0/sdk/stretch/amd64/Dockerfile)
- [`2.0-runtime`](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime/stretch/amd64/Dockerfile)

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

### Container sample: Run a simple application

Type the following command to run a sample console application:

```console
docker run --rm microsoft/dotnet-samples
```

### Container sample: Run a web application

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample microsoft/dotnet-samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. You need to navigate to the application via IP address instead of `localhost` for Windows containers, which is demonstrated in [View the ASP.NET Core app in a running container on Windows](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md#view-the-aspnet-core-app-in-a-running-container-on-windows).

## Complete set of Tags

# Linux amd64 tags

- [`2.0.7-sdk-2.1.200-stretch`, `2.0-sdk-stretch`, `2.0.7-sdk-2.1.200`, `2.0-sdk`, `2-sdk`, `sdk`, `latest` (*2.0/sdk/stretch/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/sdk/stretch/amd64/Dockerfile)
- [`2.0.7-sdk-2.1.200-jessie`, `2.0-sdk-jessie`, `2-sdk-jessie` (*2.0/sdk/jessie/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/sdk/jessie/amd64/Dockerfile)
- [`2.0.7-runtime-stretch`, `2.0-runtime-stretch`, `2.0.7-runtime`, `2.0-runtime`, `2-runtime`, `runtime` (*2.0/runtime/stretch/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime/stretch/amd64/Dockerfile)
- [`2.0.7-runtime-jessie`, `2.0-runtime-jessie`, `2-runtime-jessie` (*2.0/runtime/jessie/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime/jessie/amd64/Dockerfile)
- [`2.0.7-runtime-deps-stretch`, `2.0-runtime-deps-stretch`, `2.0.7-runtime-deps`, `2.0-runtime-deps`, `2-runtime-deps`, `runtime-deps` (*2.0/runtime-deps/stretch/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime-deps/stretch/amd64/Dockerfile)
- [`2.0.7-runtime-deps-jessie`, `2.0-runtime-deps-jessie`, `2-runtime-deps-jessie` (*2.0/runtime-deps/jessie/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime-deps/jessie/amd64/Dockerfile)
- [`1.1.8-sdk-1.1.9-jessie`, `1.1.8-sdk-1.1.9`, `1.1-sdk`, `1-sdk` (*1.1/sdk/jessie/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/sdk/jessie/amd64/Dockerfile)
- [`1.1.8-runtime-jessie`, `1.1.8-runtime`, `1.1-runtime`, `1-runtime` (*1.1/runtime/jessie/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/runtime/jessie/amd64/Dockerfile)
- [`1.0.11-runtime-jessie`, `1.0.11-runtime`, `1.0-runtime` (*1.0/runtime/jessie/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/runtime/jessie/amd64/Dockerfile)
- [`1.0.11-runtime-deps-jessie`, `1.0.11-runtime-deps`, `1.0-runtime-deps`, `1-runtime-deps` (*1.0/runtime-deps/jessie/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/runtime-deps/jessie/amd64/Dockerfile)

**.NET Core 2.1 RC1 tags**

- [`2.1.300-rc1-sdk-stretch`, `2.1-sdk-stretch`, `2.1.300-rc1-sdk`, `2.1-sdk` (*2.1/sdk/stretch/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/stretch/amd64/Dockerfile)
- [`2.1.300-rc1-sdk-alpine3.7`, `2.1-sdk-alpine3.7`, `2.1.300-rc1-sdk-alpine`, `2.1-sdk-alpine` (*2.1/sdk/alpine3.7/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/alpine3.7/amd64/Dockerfile)
- [`2.1.300-rc1-sdk-bionic`, `2.1-sdk-bionic` (*2.1/sdk/bionic/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/bionic/amd64/Dockerfile)
- [`2.1.0-rc1-aspnetcore-runtime-stretch-slim`, `2.1-aspnetcore-runtime-stretch-slim`, `2.1.0-rc1-aspnetcore-runtime`, `2.1-aspnetcore-runtime` (*2.1/aspnetcore-runtime/stretch-slim/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnetcore-runtime/stretch-slim/amd64/Dockerfile)
- [`2.1.0-rc1-aspnetcore-runtime-alpine3.7`, `2.1-aspnetcore-runtime-alpine3.7`, `2.1.0-rc1-aspnetcore-runtime-alpine`, `2.1-aspnetcore-runtime-alpine` (*2.1/aspnetcore-runtime/alpine3.7/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnetcore-runtime/alpine3.7/amd64/Dockerfile)
- [`2.1.0-rc1-aspnetcore-runtime-bionic`, `2.1-aspnetcore-runtime-bionic` (*2.1/aspnetcore-runtime/bionic/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnetcore-runtime/bionic/amd64/Dockerfile)
- [`2.1.0-rc1-runtime-stretch-slim`, `2.1-runtime-stretch-slim`, `2.1.0-rc1-runtime`, `2.1-runtime` (*2.1/runtime/stretch-slim/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/stretch-slim/amd64/Dockerfile)
- [`2.1.0-rc1-runtime-alpine3.7`, `2.1-runtime-alpine3.7`, `2.1.0-rc1-runtime-alpine`, `2.1-runtime-alpine` (*2.1/runtime/alpine3.7/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/alpine3.7/amd64/Dockerfile)
- [`2.1.0-rc1-runtime-bionic`, `2.1-runtime-bionic` (*2.1/runtime/bionic/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/bionic/amd64/Dockerfile)
- [`2.1.0-rc1-runtime-deps-stretch-slim`, `2.1-runtime-deps-stretch-slim`, `2.1.0-rc1-runtime-deps`, `2.1-runtime-deps` (*2.1/runtime-deps/stretch-slim/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime-deps/stretch-slim/amd64/Dockerfile)
- [`2.1.0-rc1-runtime-deps-alpine3.7`, `2.1-runtime-deps-alpine3.7`, `2.1.0-rc1-runtime-deps-alpine`, `2.1-runtime-deps-alpine` (*2.1/runtime-deps/alpine3.7/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime-deps/alpine3.7/amd64/Dockerfile)
- [`2.1.0-rc1-runtime-deps-bionic`, `2.1-runtime-deps-bionic` (*2.1/runtime-deps/bionic/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime-deps/bionic/amd64/Dockerfile)

# Windows Server, version 1803 amd64 tags

- [`2.0.7-sdk-2.1.200-nanoserver-1803`, `2.0-sdk-nanoserver-1803`, `2.0.7-sdk-2.1.200`, `2.0-sdk`, `2-sdk`, `sdk`, `latest` (*2.0/sdk/nanoserver-1803/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/sdk/nanoserver-1803/amd64/Dockerfile)
- [`2.0.7-runtime-nanoserver-1803`, `2.0-runtime-nanoserver-1803`, `2.0.7-runtime`, `2.0-runtime`, `2-runtime`, `runtime` (*2.0/runtime/nanoserver-1803/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime/nanoserver-1803/amd64/Dockerfile)

**.NET Core 2.1 RC1 tags**

- [`2.1.300-rc1-sdk-nanoserver-1803`, `2.1-sdk-nanoserver-1803`, `2.1.300-rc1-sdk`, `2.1-sdk` (*2.1/sdk/nanoserver-1803/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/nanoserver-1803/amd64/Dockerfile)
- [`2.1.0-rc1-aspnetcore-runtime-nanoserver-1803`, `2.1-aspnetcore-runtime-nanoserver-1803`, `2.1.0-rc1-aspnetcore-runtime`, `2.1-aspnetcore-runtime` (*2.1/aspnetcore-runtime/nanoserver-1803/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnetcore-runtime/nanoserver-1803/amd64/Dockerfile)
- [`2.1.0-rc1-runtime-nanoserver-1803`, `2.1-runtime-nanoserver-1803`, `2.1.0-rc1-runtime`, `2.1-runtime` (*2.1/runtime/nanoserver-1803/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/nanoserver-1803/amd64/Dockerfile)

# Windows Server, version 1709 amd64 tags

- [`2.0.7-sdk-2.1.200-nanoserver-1709`, `2.0-sdk-nanoserver-1709`, `2.0.7-sdk-2.1.200`, `2.0-sdk`, `2-sdk`, `sdk`, `latest` (*2.0/sdk/nanoserver-1709/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/sdk/nanoserver-1709/amd64/Dockerfile)
- [`2.0.7-runtime-nanoserver-1709`, `2.0-runtime-nanoserver-1709`, `2.0.7-runtime`, `2.0-runtime`, `2-runtime`, `runtime` (*2.0/runtime/nanoserver-1709/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime/nanoserver-1709/amd64/Dockerfile)

**.NET Core 2.1 RC1 tags**

- [`2.1.300-rc1-sdk-nanoserver-1709`, `2.1-sdk-nanoserver-1709`, `2.1.300-rc1-sdk`, `2.1-sdk` (*2.1/sdk/nanoserver-1709/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/nanoserver-1709/amd64/Dockerfile)
- [`2.1.0-rc1-aspnetcore-runtime-nanoserver-1709`, `2.1-aspnetcore-runtime-nanoserver-1709`, `2.1.0-rc1-aspnetcore-runtime`, `2.1-aspnetcore-runtime` (*2.1/aspnetcore-runtime/nanoserver-1709/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnetcore-runtime/nanoserver-1709/amd64/Dockerfile)
- [`2.1.0-rc1-runtime-nanoserver-1709`, `2.1-runtime-nanoserver-1709`, `2.1.0-rc1-runtime`, `2.1-runtime` (*2.1/runtime/nanoserver-1709/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/nanoserver-1709/amd64/Dockerfile)

# Windows Server 2016 amd64 tags

- [`2.0.7-sdk-2.1.200-nanoserver-sac2016`, `2.0-sdk-nanoserver-sac2016`, `2.0.7-sdk-2.1.200`, `2.0-sdk`, `2-sdk`, `sdk`, `latest` (*2.0/sdk/nanoserver-sac2016/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/sdk/nanoserver-sac2016/amd64/Dockerfile)
- [`2.0.7-runtime-nanoserver-sac2016`, `2.0-runtime-nanoserver-sac2016`, `2.0.7-runtime`, `2.0-runtime`, `2-runtime`, `runtime` (*2.0/runtime/nanoserver-sac2016/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime/nanoserver-sac2016/amd64/Dockerfile)
- [`1.1.8-sdk-1.1.9-nanoserver-sac2016`, `1.1.8-sdk-1.1.9`, `1.1-sdk`, `1-sdk` (*1.1/sdk/nanoserver-sac2016/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/sdk/nanoserver-sac2016/amd64/Dockerfile)
- [`1.1.8-runtime-nanoserver-sac2016`, `1.1.8-runtime`, `1.1-runtime`, `1-runtime` (*1.1/runtime/nanoserver-sac2016/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/runtime/nanoserver-sac2016/amd64/Dockerfile)
- [`1.0.11-runtime-nanoserver-sac2016`, `1.0.11-runtime`, `1.0-runtime` (*1.0/runtime/nanoserver-sac2016/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/runtime/nanoserver-sac2016/amd64/Dockerfile)

**.NET Core 2.1 RC1 tags**

- [`2.1.300-rc1-sdk-nanoserver-sac2016`, `2.1-sdk-nanoserver-sac2016`, `2.1.300-rc1-sdk`, `2.1-sdk` (*2.1/sdk/nanoserver-sac2016/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/nanoserver-sac2016/amd64/Dockerfile)
- [`2.1.0-rc1-aspnetcore-runtime-nanoserver-sac2016`, `2.1-aspnetcore-runtime-nanoserver-sac2016`, `2.1.0-rc1-aspnetcore-runtime`, `2.1-aspnetcore-runtime` (*2.1/aspnetcore-runtime/nanoserver-sac2016/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnetcore-runtime/nanoserver-sac2016/amd64/Dockerfile)
- [`2.1.0-rc1-runtime-nanoserver-sac2016`, `2.1-runtime-nanoserver-sac2016`, `2.1.0-rc1-runtime`, `2.1-runtime` (*2.1/runtime/nanoserver-sac2016/amd64/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/nanoserver-sac2016/amd64/Dockerfile)

# Linux arm32 tags

- [`2.0.7-runtime-stretch-arm32v7`, `2.0-runtime-stretch-arm32v7`, `2.0.7-runtime`, `2.0-runtime`, `2-runtime`, `runtime` (*2.0/runtime/stretch/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime/stretch/arm32v7/Dockerfile)
- [`2.0.7-runtime-deps-stretch-arm32v7`, `2.0-runtime-deps-stretch-arm32v7`, `2.0.7-runtime-deps`, `2.0-runtime-deps`, `2-runtime-deps`, `runtime-deps` (*2.0/runtime-deps/stretch/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime-deps/stretch/arm32v7/Dockerfile)

**.NET Core 2.1 RC1 tags**

- [`2.1.300-rc1-sdk-stretch-arm32v7`, `2.1-sdk-stretch-arm32v7`, `2.1.300-rc1-sdk`, `2.1-sdk` (*2.1/sdk/stretch/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/stretch/arm32v7/Dockerfile)
- [`2.1.300-rc1-sdk-bionic-arm32v7`, `2.1-sdk-bionic-arm32v7` (*2.1/sdk/bionic/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/bionic/arm32v7/Dockerfile)
- [`2.1.0-rc1-aspnetcore-runtime-stretch-slim-arm32v7`, `2.1-aspnetcore-runtime-stretch-slim-arm32v7`, `2.1.0-rc1-aspnetcore-runtime`, `2.1-aspnetcore-runtime` (*2.1/aspnetcore-runtime/stretch-slim/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnetcore-runtime/stretch-slim/arm32v7/Dockerfile)
- [`2.1.0-rc1-aspnetcore-runtime-bionic-arm32v7`, `2.1-aspnetcore-runtime-bionic-arm32v7` (*2.1/aspnetcore-runtime/bionic/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnetcore-runtime/bionic/arm32v7/Dockerfile)
- [`2.1.0-rc1-runtime-stretch-slim-arm32v7`, `2.1-runtime-stretch-slim-arm32v7`, `2.1.0-rc1-runtime`, `2.1-runtime` (*2.1/runtime/stretch-slim/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/stretch-slim/arm32v7/Dockerfile)
- [`2.1.0-rc1-runtime-bionic-arm32v7`, `2.1-runtime-bionic-arm32v7` (*2.1/runtime/bionic/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime/bionic/arm32v7/Dockerfile)
- [`2.1.0-rc1-runtime-deps-stretch-slim-arm32v7`, `2.1-runtime-deps-stretch-slim-arm32v7`, `2.1.0-rc1-runtime-deps`, `2.1-runtime-deps` (*2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile)
- [`2.1.0-rc1-runtime-deps-bionic-arm32v7`, `2.1-runtime-deps-bionic-arm32v7` (*2.1/runtime-deps/bionic/arm32v7/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime-deps/bionic/arm32v7/Dockerfile)

For more information about these images and their history, please see [the relevant Dockerfile](https://github.com/dotnet/dotnet-docker/search?utf8=%E2%9C%93&q=FROM&type=Code). These images are updated via [pull requests to the `dotnet/dotnet-docker` GitHub repo](https://github.com/dotnet/dotnet-docker/pulls).

## What is .NET Core?

[.NET Core](https://docs.microsoft.com/dotnet/core/) is a general purpose development platform maintained by Microsoft and the .NET community on [GitHub](https://github.com/dotnet/core). It is cross-platform, supporting Windows, macOS and Linux, and can be used in device, cloud, and embedded/IoT scenarios.

.NET has several capabilities that make development easier, including automatic memory management, (runtime) generic types, reflection, asynchrony, concurrency, and native interop. Millions of developers take advantage of these capabilities to efficiently build high-quality applications.

You can use C# to write .NET Core apps. C# is simple, powerful, type-safe, and object-oriented while retaining the expressiveness and elegance of C-style languages. Anyone familiar with C and similar languages will find it straightforward to write in C#.

[.NET Core](https://github.com/dotnet/core) is open source (MIT and Apache 2 licenses) and was contributed to the [.NET Foundation](http://dotnetfoundation.org) by Microsoft in 2014. It can be freely adopted by individuals and companies, including for personal, academic or commercial purposes. Multiple companies use .NET Core as part of apps, tools, new platforms and hosting services.

You are invited to [contribute new features](https://github.com/dotnet/core/blob/master/CONTRIBUTING.md), fixes, or updates, large or small; we are always thrilled to receive pull requests, and do our best to process them as fast as we can.

> https://docs.microsoft.com/dotnet/core/

![logo](https://avatars0.githubusercontent.com/u/9141961?v=3&amp;s=100)

## .NET Core Docker Samples

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Building .NET Core Apps with Docker

* [.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

### Develop .NET Core Apps in a Container

* [Develop .NET Core Applications](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-dev-in-container.md) - This sample shows how to develop, build and test .NET Core applications with Docker without the need to install the .NET Core SDK.
* [Develop ASP.NET Core Applications](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnet-docker-dev-in-container.md) - This sample shows how to develop and test ASP.NET Core applications with Docker without the need to install the .NET Core SDK.

### Optimizing Container Size

* [.NET Core Alpine Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.alpine-x64) builds, tests, and runs an application using Alpine.
* [.NET Core self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-selfcontained.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.debian-x64-selfcontained) builds and runs an application as a self-contained application.

### ARM32 / Raspberry Pi

* [.NET Core ARM32 Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-arm32.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.basic-preview) builds and runs an application with Debian on ARM32 (works on Raspberry Pi).
* [ASP.NET Core ARM32 Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.preview) builds and runs an ASP.NET Core application with Debian on ARM32 (works on Raspberry Pi).

## Image variants

The `microsoft/dotnet` images come in different flavors, each designed for a specific use case.

### `microsoft/dotnet:<version>-sdk`

This is the defacto image. If you are unsure about what your needs are, you probably want to use this one. It is designed to be used both as a throw away container (mount your source code and start the container to start your app), as well as the base to build other images off of.

It contains the .NET Core SDK which is comprised of two parts:

1. .NET Core CLI
1. .NET Core
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

### `microsoft/dotnet:<version>-aspnet-core-runtime`

This image contains the ASP.NET Core and .NET Core runtimes and libraries and is optimized for running ASP.NET Core apps in production.

### `microsoft/dotnet:<version>-runtime`

This image contains the .NET Core runtimes and libraries and is optimized for running .NET Core apps in production.

### `microsoft/dotnet:<version>-runtime-deps`

This image contains the native dependencies needed by .NET Core. It does not include .NET Core. It is for  [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

## Issues

If you have any problems with or questions about this image, please contact us through a [GitHub issue](https://github.com/dotnet/dotnet-docker/issues).

## Licenses

* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Windows Nano Server license](https://hub.docker.com/r/microsoft/nanoserver/) (only applies to Windows containers)

## Related Repos

.NET Core Docker Hub repos:

* [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) for ASP.NET Core images.
* [microsoft/dotnet-nightly](https://hub.docker.com/r/microsoft/dotnet-nightly/) for .NET Core preview images.
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/) for .NET Core sample images.

.NET Framework Docker Hub repos:

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) for ASP.NET Web Forms and MVC images.
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) for .NET Framework images.
* [microsoft/dotnet-framework-build](https://hub.docker.com/r/microsoft/dotnet-framework-build/) for building .NET Framework applications with Docker.
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/) for .NET Framework and ASP.NET sample images.
