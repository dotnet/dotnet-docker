The images from this repository include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/dotnet-docker](https://hub.docker.com/r/microsoft/dotnet/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

The images from this repository contain the native dependencies needed by .NET Core. It does not include .NET Core. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

# Latest Version of Common Tags

The following tag is the latest stable version of the most commonly used image. The complete set of tags is listed further down.

- [`mcr.microsoft.com/dotnet/core-nightly/runtime-deps:2.2`](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime-deps/stretch-slim/amd64/Dockerfile)

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# Supported Tags

## Linux amd64 tags

- [`2.2.1-stretch-slim`, `2.2-stretch-slim`, `2.2.1`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile)
- [`2.2.1-alpine3.8`, `2.2-alpine3.8`, `2.2.1-alpine`, `2.2-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.8/amd64/Dockerfile)
- [`2.2.1-bionic`, `2.2-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile)
- [`2.1.7-stretch-slim`, `2.1-stretch-slim`, `2.1.7`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile)
- [`2.1.7-alpine3.7`, `2.1-alpine3.7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.7/amd64/Dockerfile)
- [`2.1.7-alpine3.8`, `2.1-alpine3.8`, `2.1.7-alpine`, `2.1-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.8/amd64/Dockerfile)
- [`2.1.7-bionic`, `2.1-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile)
- [`1.1.10-stretch`, `1.1-stretch` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.1/runtime-deps/stretch/amd64/Dockerfile)
- [`1.0.13-jessie`, `1.0-jessie`, `1.0.13`, `1.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.0/runtime-deps/jessie/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim`, `3.0-stretch-slim`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/stretch-slim/amd64/Dockerfile)
- [`3.0.0-preview-alpine3.8`, `3.0-alpine3.8`, `3.0.0-preview-alpine`, `3.0-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.8/amd64/Dockerfile)
- [`3.0.0-preview-bionic`, `3.0-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/amd64/Dockerfile)

## Linux arm64 tags

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim-arm64v8`, `3.0-stretch-slim-arm64v8`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/stretch-slim/arm64v8/Dockerfile)
- [`3.0.0-preview-bionic-arm64v8`, `3.0-bionic-arm64v8` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm64v8/Dockerfile)

## Linux arm32 tags

- [`2.2.1-stretch-slim-arm32v7`, `2.2-stretch-slim-arm32v7`, `2.2.1`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile)
- [`2.2.1-bionic-arm32v7`, `2.2-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile)
- [`2.1.7-stretch-slim-arm32v7`, `2.1-stretch-slim-arm32v7`, `2.1.7`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile)
- [`2.1.7-bionic-arm32v7`, `2.1-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim-arm32v7`, `3.0-stretch-slim-arm32v7`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/stretch-slim/arm32v7/Dockerfile)
- [`3.0.0-preview-bionic-arm32v7`, `3.0-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm32v7/Dockerfile)

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

# Issues

If you have any problems with or questions about this image, please contact us through a [GitHub issue](https://github.com/dotnet/dotnet-docker/issues).

# Licenses

* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Windows Nano Server license](https://hub.docker.com/r/microsoft/nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/en-us/cloud-platform/windows-server-pricing)

# Related Repos

.NET Core Docker Hub repos:

* [dotnet/dotnet-docker](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core preview images.
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/) for .NET Core sample images.
* [mcr.microsoft.com/dotnet/core-nightly](https://hub.docker.com/r/microsoft/dotnet/core-nightly/) for .NET Core preview images.
* [mcr.microsoft.com/dotnet/core-nightly/sdk](https://hub.docker.com/r/microsoft/dotnet/core-nightly/sdk/) for .NET Core SDK preview images.
* [mcr.microsoft.com/dotnet/core-nightly/aspnet](https://hub.docker.com/r/microsoft/dotnet/core-nightly/aspnet/) for ASP.NET Core preview images.
* [mcr.microsoft.com/dotnet/core-nightly/runtime](https://hub.docker.com/r/microsoft/dotnet/core-nightly/runtime/) for .NET Core runtime preview images.

.NET Framework Docker Hub repos:

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) for ASP.NET Web Forms and MVC images.
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) for .NET Framework images.
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/) for .NET Framework and ASP.NET sample images.
