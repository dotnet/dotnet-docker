This repository contains `Dockerfile` definitions for Docker images that include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/dotnet-docker](https://hub.docker.com/r/microsoft/dotnet/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

# Latest Version of Common Tags

The following tags are the latest stable versions of the most commonly used images. The complete set of tags is listed further down.

- [`2.2-sdk`](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/stretch/amd64/Dockerfile)
- [`2.2-aspnetcore-runtime`](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnetcore-runtime/stretch-slim/amd64/Dockerfile)
- [`2.2-runtime`](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/stretch-slim/amd64/Dockerfile)
- [`2.2-runtime-deps`](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile)

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

## Container sample: Run a simple application

Type the following command to run a sample console application:

```console
docker run --rm microsoft/dotnet-samples
```

## Container sample: Run a web application

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample microsoft/dotnet-samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-https.md) to use HTTPS with this image.

# Tags

## Linux amd64 tags

- [`2.2.102-sdk-stretch`, `2.2-sdk-stretch`, `2.2.102-sdk`, `2.2-sdk`, `sdk`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/stretch/amd64/Dockerfile)
- [`2.2.102-sdk-alpine3.8`, `2.2-sdk-alpine3.8`, `2.2.102-sdk-alpine`, `2.2-sdk-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/alpine3.8/amd64/Dockerfile)
- [`2.2.102-sdk-bionic`, `2.2-sdk-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/bionic/amd64/Dockerfile)
- [`2.2.1-aspnetcore-runtime-stretch-slim`, `2.2-aspnetcore-runtime-stretch-slim`, `2.2.1-aspnetcore-runtime`, `2.2-aspnetcore-runtime`, `aspnetcore-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnetcore-runtime/stretch-slim/amd64/Dockerfile)
- [`2.2.1-aspnetcore-runtime-alpine3.8`, `2.2-aspnetcore-runtime-alpine3.8`, `2.2.1-aspnetcore-runtime-alpine`, `2.2-aspnetcore-runtime-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnetcore-runtime/alpine3.8/amd64/Dockerfile)
- [`2.2.1-aspnetcore-runtime-bionic`, `2.2-aspnetcore-runtime-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnetcore-runtime/bionic/amd64/Dockerfile)
- [`2.2.1-runtime-stretch-slim`, `2.2-runtime-stretch-slim`, `2.2.1-runtime`, `2.2-runtime`, `runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/stretch-slim/amd64/Dockerfile)
- [`2.2.1-runtime-alpine3.8`, `2.2-runtime-alpine3.8`, `2.2.1-runtime-alpine`, `2.2-runtime-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/alpine3.8/amd64/Dockerfile)
- [`2.2.1-runtime-bionic`, `2.2-runtime-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/bionic/amd64/Dockerfile)
- [`2.2.1-runtime-deps-stretch-slim`, `2.2-runtime-deps-stretch-slim`, `2.2.1-runtime-deps`, `2.2-runtime-deps`, `runtime-deps` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile)
- [`2.2.1-runtime-deps-alpine3.8`, `2.2-runtime-deps-alpine3.8`, `2.2.1-runtime-deps-alpine`, `2.2-runtime-deps-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.8/amd64/Dockerfile)
- [`2.2.1-runtime-deps-bionic`, `2.2-runtime-deps-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile)
- [`2.1.503-sdk-stretch`, `2.1-sdk-stretch`, `2.1.503-sdk`, `2.1-sdk` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/stretch/amd64/Dockerfile)
- [`2.1.503-sdk-alpine3.7`, `2.1-sdk-alpine3.7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/alpine3.7/amd64/Dockerfile)
- [`2.1.503-sdk-alpine3.8`, `2.1-sdk-alpine3.8`, `2.1.503-sdk-alpine`, `2.1-sdk-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/alpine3.8/amd64/Dockerfile)
- [`2.1.503-sdk-bionic`, `2.1-sdk-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/bionic/amd64/Dockerfile)
- [`2.1.7-aspnetcore-runtime-stretch-slim`, `2.1-aspnetcore-runtime-stretch-slim`, `2.1.7-aspnetcore-runtime`, `2.1-aspnetcore-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnetcore-runtime/stretch-slim/amd64/Dockerfile)
- [`2.1.7-aspnetcore-runtime-alpine3.7`, `2.1-aspnetcore-runtime-alpine3.7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnetcore-runtime/alpine3.7/amd64/Dockerfile)
- [`2.1.7-aspnetcore-runtime-alpine3.8`, `2.1-aspnetcore-runtime-alpine3.8`, `2.1.7-aspnetcore-runtime-alpine`, `2.1-aspnetcore-runtime-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnetcore-runtime/alpine3.8/amd64/Dockerfile)
- [`2.1.7-aspnetcore-runtime-bionic`, `2.1-aspnetcore-runtime-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnetcore-runtime/bionic/amd64/Dockerfile)
- [`2.1.7-runtime-stretch-slim`, `2.1-runtime-stretch-slim`, `2.1.7-runtime`, `2.1-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/stretch-slim/amd64/Dockerfile)
- [`2.1.7-runtime-alpine3.7`, `2.1-runtime-alpine3.7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/alpine3.7/amd64/Dockerfile)
- [`2.1.7-runtime-alpine3.8`, `2.1-runtime-alpine3.8`, `2.1.7-runtime-alpine`, `2.1-runtime-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/alpine3.8/amd64/Dockerfile)
- [`2.1.7-runtime-bionic`, `2.1-runtime-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/bionic/amd64/Dockerfile)
- [`2.1.7-runtime-deps-stretch-slim`, `2.1-runtime-deps-stretch-slim`, `2.1.7-runtime-deps`, `2.1-runtime-deps` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile)
- [`2.1.7-runtime-deps-alpine3.7`, `2.1-runtime-deps-alpine3.7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.7/amd64/Dockerfile)
- [`2.1.7-runtime-deps-alpine3.8`, `2.1-runtime-deps-alpine3.8`, `2.1.7-runtime-deps-alpine`, `2.1-runtime-deps-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.8/amd64/Dockerfile)
- [`2.1.7-runtime-deps-bionic`, `2.1-runtime-deps-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile)

**.NET Core 1.0, 1.1 and 3.0 Preview tags**

See the [complete set of tags](https://github.com/dotnet/dotnet-docker/blob/nightly/TAGS.md).

## Linux arm64 tags

**.NET Core 3.0 Preview tags**

See the [complete set of tags](https://github.com/dotnet/dotnet-docker/blob/nightly/TAGS.md).

## Linux arm32 tags

- [`2.2.102-sdk-stretch-arm32v7`, `2.2-sdk-stretch-arm32v7`, `2.2.102-sdk`, `2.2-sdk`, `sdk`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/stretch/arm32v7/Dockerfile)
- [`2.2.102-sdk-bionic-arm32v7`, `2.2-sdk-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/bionic/arm32v7/Dockerfile)
- [`2.2.1-aspnetcore-runtime-stretch-slim-arm32v7`, `2.2-aspnetcore-runtime-stretch-slim-arm32v7`, `2.2.1-aspnetcore-runtime`, `2.2-aspnetcore-runtime`, `aspnetcore-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnetcore-runtime/stretch-slim/arm32v7/Dockerfile)
- [`2.2.1-aspnetcore-runtime-bionic-arm32v7`, `2.2-aspnetcore-runtime-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnetcore-runtime/bionic/arm32v7/Dockerfile)
- [`2.2.1-runtime-stretch-slim-arm32v7`, `2.2-runtime-stretch-slim-arm32v7`, `2.2.1-runtime`, `2.2-runtime`, `runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/stretch-slim/arm32v7/Dockerfile)
- [`2.2.1-runtime-bionic-arm32v7`, `2.2-runtime-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/bionic/arm32v7/Dockerfile)
- [`2.2.1-runtime-deps-stretch-slim-arm32v7`, `2.2-runtime-deps-stretch-slim-arm32v7`, `2.2.1-runtime-deps`, `2.2-runtime-deps`, `runtime-deps` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile)
- [`2.2.1-runtime-deps-bionic-arm32v7`, `2.2-runtime-deps-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile)
- [`2.1.503-sdk-stretch-arm32v7`, `2.1-sdk-stretch-arm32v7`, `2.1.503-sdk`, `2.1-sdk` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/stretch/arm32v7/Dockerfile)
- [`2.1.503-sdk-bionic-arm32v7`, `2.1-sdk-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/bionic/arm32v7/Dockerfile)
- [`2.1.7-aspnetcore-runtime-stretch-slim-arm32v7`, `2.1-aspnetcore-runtime-stretch-slim-arm32v7`, `2.1.7-aspnetcore-runtime`, `2.1-aspnetcore-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnetcore-runtime/stretch-slim/arm32v7/Dockerfile)
- [`2.1.7-aspnetcore-runtime-bionic-arm32v7`, `2.1-aspnetcore-runtime-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnetcore-runtime/bionic/arm32v7/Dockerfile)
- [`2.1.7-runtime-stretch-slim-arm32v7`, `2.1-runtime-stretch-slim-arm32v7`, `2.1.7-runtime`, `2.1-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/stretch-slim/arm32v7/Dockerfile)
- [`2.1.7-runtime-bionic-arm32v7`, `2.1-runtime-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/bionic/arm32v7/Dockerfile)
- [`2.1.7-runtime-deps-stretch-slim-arm32v7`, `2.1-runtime-deps-stretch-slim-arm32v7`, `2.1.7-runtime-deps`, `2.1-runtime-deps` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile)
- [`2.1.7-runtime-deps-bionic-arm32v7`, `2.1-runtime-deps-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

See the [complete set of tags](https://github.com/dotnet/dotnet-docker/blob/nightly/TAGS.md).

## Windows Server, version 1809 amd64 tags

- [`2.2.102-sdk-nanoserver-1809`, `2.2-sdk-nanoserver-1809`, `2.2.102-sdk`, `2.2-sdk`, `sdk`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/nanoserver-1809/amd64/Dockerfile)
- [`2.2.1-aspnetcore-runtime-nanoserver-1809`, `2.2-aspnetcore-runtime-nanoserver-1809`, `2.2.1-aspnetcore-runtime`, `2.2-aspnetcore-runtime`, `aspnetcore-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnetcore-runtime/nanoserver-1809/amd64/Dockerfile)
- [`2.2.1-runtime-nanoserver-1809`, `2.2-runtime-nanoserver-1809`, `2.2.1-runtime`, `2.2-runtime`, `runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/nanoserver-1809/amd64/Dockerfile)
- [`2.1.503-sdk-nanoserver-1809`, `2.1-sdk-nanoserver-1809`, `2.1.503-sdk`, `2.1-sdk` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/nanoserver-1809/amd64/Dockerfile)
- [`2.1.7-aspnetcore-runtime-nanoserver-1809`, `2.1-aspnetcore-runtime-nanoserver-1809`, `2.1.7-aspnetcore-runtime`, `2.1-aspnetcore-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnetcore-runtime/nanoserver-1809/amd64/Dockerfile)
- [`2.1.7-runtime-nanoserver-1809`, `2.1-runtime-nanoserver-1809`, `2.1.7-runtime`, `2.1-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/nanoserver-1809/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

See the [complete set of tags](https://github.com/dotnet/dotnet-docker/blob/nightly/TAGS.md).

## Windows Server 2016, version 1709, and version 1803 amd64 tags

See the [complete set of tags](https://github.com/dotnet/dotnet-docker/blob/nightly/TAGS.md).

## Windows Server, version 1809 arm32 tags

- [`2.2.1-sdk-nanoserver-1809-arm32`, `2.2-sdk-nanoserver-1809-arm32`, `2.2.102-sdk`, `2.2-sdk`, `sdk`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/nanoserver-1809/arm32/Dockerfile)
- [`2.2.1-aspnetcore-runtime-nanoserver-1809-arm32`, `2.2-aspnetcore-runtime-nanoserver-1809-arm32`, `2.2.1-aspnetcore-runtime`, `2.2-aspnetcore-runtime`, `aspnetcore-runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnetcore-runtime/nanoserver-1809/arm32/Dockerfile)
- [`2.2.1-runtime-nanoserver-1809-arm32`, `2.2-runtime-nanoserver-1809-arm32`, `2.2.1-runtime`, `2.2-runtime`, `runtime` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/nanoserver-1809/arm32/Dockerfile)

**.NET Core 3.0 Preview tags**

See the [complete set of tags](https://github.com/dotnet/dotnet-docker/blob/nightly/TAGS.md).

For more information about these images and their history, please see [the relevant Dockerfile](https://github.com/dotnet/dotnet-docker/search?utf8=%E2%9C%93&q=FROM&type=Code). These images are updated via [pull requests to the `dotnet/dotnet-docker` GitHub repo](https://github.com/dotnet/dotnet-docker/pulls).

# What is .NET Core?

[.NET Core](https://docs.microsoft.com/dotnet/core/) is a general purpose development platform maintained by Microsoft and the .NET community on [GitHub](https://github.com/dotnet/core). It is cross-platform, supporting Windows, macOS and Linux, and can be used in device, cloud, and embedded/IoT scenarios.

.NET has several capabilities that make development easier, including automatic memory management, (runtime) generic types, reflection, asynchrony, concurrency, and native interop. Millions of developers take advantage of these capabilities to efficiently build high-quality applications.

You can use C# to write .NET Core apps. C# is simple, powerful, type-safe, and object-oriented while retaining the expressiveness and elegance of C-style languages. Anyone familiar with C and similar languages will find it straightforward to write in C#.

[.NET Core](https://github.com/dotnet/core) is open source (MIT and Apache 2 licenses) and was contributed to the [.NET Foundation](http://dotnetfoundation.org) by Microsoft in 2014. It can be freely adopted by individuals and companies, including for personal, academic or commercial purposes. Multiple companies use .NET Core as part of apps, tools, new platforms and hosting services.

You are invited to [contribute new features](https://github.com/dotnet/core/blob/master/CONTRIBUTING.md), fixes, or updates, large or small; we are always thrilled to receive pull requests, and do our best to process them as fast as we can.

> https://docs.microsoft.com/dotnet/core/

![logo](https://avatars0.githubusercontent.com/u/9141961?v=3&amp;s=100)

# .NET Core Docker Samples

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Building .NET Core Apps with Docker

* [.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

## Develop .NET Core Apps in a Container

* [Develop .NET Core Applications](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-dev-in-container.md) - This sample shows how to develop, build and test .NET Core applications with Docker without the need to install the .NET Core SDK.
* [Develop ASP.NET Core Applications](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnet-docker-dev-in-container.md) - This sample shows how to develop and test ASP.NET Core applications with Docker without the need to install the .NET Core SDK.

## Optimizing Container Size

* [.NET Core Alpine Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.alpine-x64) builds, tests, and runs an application using Alpine.
* [.NET Core self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-selfcontained.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.debian-x64-selfcontained) builds and runs an application as a self-contained application.

## ARM32 / Raspberry Pi

* [.NET Core ARM32 Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-arm32.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile) builds and runs an application with Debian on ARM32 (works on Raspberry Pi).
* [ASP.NET Core ARM32 Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile) builds and runs an ASP.NET Core application with Debian on ARM32 (works on Raspberry Pi).

# Image variants

The `microsoft/dotnet-nightly` images come in different flavors, each designed for a specific use case.

## `microsoft/dotnet-nightly:<version>-sdk`

This is the defacto image. If you are unsure about what your needs are, you probably want to use this one. It is designed to be used both as a throw away container (mount your source code and start the container to start your app), as well as the base to build other images off of.

It contains the .NET Core SDK which is comprised of three parts:

1. .NET Core CLI
1. .NET Core
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

## `microsoft/dotnet-nightly:<version>-aspnetcore-runtime`

This image contains the ASP.NET Core and .NET Core runtimes and libraries and is optimized for running ASP.NET Core apps in production.

## `microsoft/dotnet-nightly:<version>-runtime`

This image contains the .NET Core runtimes and libraries and is optimized for running .NET Core apps in production.

## `microsoft/dotnet-nightly:<version>-runtime-deps`

This image contains the native dependencies needed by .NET Core. It does not include .NET Core. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

# Issues

If you have any problems with or questions about this image, please contact us through a [GitHub issue](https://github.com/dotnet/dotnet-docker/issues).

# Licenses

* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Windows Nano Server license](https://hub.docker.com/r/microsoft/nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2016](https://www.microsoft.com/en-us/cloud-platform/windows-server-pricing)

# Related Repos

.NET Core Docker Hub repos:

* [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) for ASP.NET Core images.
* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core images.
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/) for .NET Core sample images.

.NET Framework Docker Hub repos:

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) for ASP.NET Web Forms and MVC images.
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) for .NET Framework images.
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/) for .NET Framework and ASP.NET sample images.
