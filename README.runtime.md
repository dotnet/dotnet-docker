The images from this repository include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/dotnet-docker](https://hub.docker.com/r/microsoft/dotnet/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

# Featured Tags

* `2.2` (Current)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/runtime:2.2`
* `2.1` (LTS)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/runtime:2.1`

# About This Image

This image contains the .NET Core runtimes and libraries and is optimized for running .NET Core apps in production.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a simple application

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/r/microsoft/dotnet-samples/), based on the [.NET Core console sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm microsoft/dotnet-samples
```

# Related Repos

.NET Core (Preview):

* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)
* [dotnet/core-nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-core-nightly-sdk/): .NET Core SDK (Preview)
* [dotnet/core-nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime-deps/): .NET Core Runtime Dependencies (Preview)

.NET Core:

* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/): .NET Core
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet/): .NET Core Samples

.NET Framework:

* [dotnet/framework/aspnet](https://hub.docker.com/_/microsoft-dotnet-framework-aspnet): ASP.NET Web Forms and MVC
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/): .NET Framework
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/): .NET Framework and ASP.NET Samples

# Full Tag Listing

## Linux amd64 tags

- [`2.2.2-stretch-slim`, `2.2-stretch-slim`, `2.2.2`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/stretch-slim/amd64/Dockerfile)
- [`2.2.2-alpine3.8`, `2.2-alpine3.8`, `2.2.2-alpine`, `2.2-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/alpine3.8/amd64/Dockerfile)
- [`2.2.2-bionic`, `2.2-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/bionic/amd64/Dockerfile)
- [`2.1.8-stretch-slim`, `2.1-stretch-slim`, `2.1.8`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/stretch-slim/amd64/Dockerfile)
- [`2.1.8-alpine3.7`, `2.1-alpine3.7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/alpine3.7/amd64/Dockerfile)
- [`2.1.8-alpine3.8`, `2.1-alpine3.8`, `2.1.8-alpine`, `2.1-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/alpine3.8/amd64/Dockerfile)
- [`2.1.8-bionic`, `2.1-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/bionic/amd64/Dockerfile)
- [`1.1.11-stretch`, `1.1-stretch` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.1/runtime/stretch/amd64/Dockerfile)
- [`1.1.11-jessie`, `1.1-jessie`, `1.1.11`, `1.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.1/runtime/jessie/amd64/Dockerfile)
- [`1.0.14-jessie`, `1.0-jessie`, `1.0.14`, `1.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.0/runtime/jessie/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim`, `3.0-stretch-slim`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/stretch-slim/amd64/Dockerfile)
- [`3.0.0-preview-alpine3.9`, `3.0-alpine3.9`, `3.0.0-preview-alpine`, `3.0-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/alpine3.9/amd64/Dockerfile)
- [`3.0.0-preview-bionic`, `3.0-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/bionic/amd64/Dockerfile)

## Linux arm64 tags

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim-arm64v8`, `3.0-stretch-slim-arm64v8`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/stretch-slim/arm64v8/Dockerfile)
- [`3.0.0-preview-bionic-arm64v8`, `3.0-bionic-arm64v8` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/bionic/arm64v8/Dockerfile)

## Linux arm32 tags

- [`2.2.2-stretch-slim-arm32v7`, `2.2-stretch-slim-arm32v7`, `2.2.2`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/stretch-slim/arm32v7/Dockerfile)
- [`2.2.2-bionic-arm32v7`, `2.2-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/bionic/arm32v7/Dockerfile)
- [`2.1.8-stretch-slim-arm32v7`, `2.1-stretch-slim-arm32v7`, `2.1.8`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/stretch-slim/arm32v7/Dockerfile)
- [`2.1.8-bionic-arm32v7`, `2.1-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/bionic/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim-arm32v7`, `3.0-stretch-slim-arm32v7`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/stretch-slim/arm32v7/Dockerfile)
- [`3.0.0-preview-bionic-arm32v7`, `3.0-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/bionic/arm32v7/Dockerfile)

## Windows Server, version 1809 amd64 tags

- [`2.2.2-nanoserver-1809`, `2.2-nanoserver-1809`, `2.2.2`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/nanoserver-1809/amd64/Dockerfile)
- [`2.1.8-nanoserver-1809`, `2.1-nanoserver-1809`, `2.1.8`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/nanoserver-1809/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-nanoserver-1809`, `3.0-nanoserver-1809`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/nanoserver-1809/amd64/Dockerfile)

## Windows Server, version 1803 amd64 tags

- [`2.2.2-nanoserver-1803`, `2.2-nanoserver-1803`, `2.2.2`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/nanoserver-1803/amd64/Dockerfile)
- [`2.1.8-nanoserver-1803`, `2.1-nanoserver-1803`, `2.1.8`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/nanoserver-1803/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-nanoserver-1803`, `3.0-nanoserver-1803`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/nanoserver-1803/amd64/Dockerfile)

## Windows Server, version 1709 amd64 tags

- [`2.2.2-nanoserver-1709`, `2.2-nanoserver-1709`, `2.2.2`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/nanoserver-1709/amd64/Dockerfile)
- [`2.1.8-nanoserver-1709`, `2.1-nanoserver-1709`, `2.1.8`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/nanoserver-1709/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-nanoserver-1709`, `3.0-nanoserver-1709`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/nanoserver-1709/amd64/Dockerfile)

## Windows Server 2016 amd64 tags

- [`2.2.2-nanoserver-sac2016`, `2.2-nanoserver-sac2016`, `2.2.2`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/nanoserver-sac2016/amd64/Dockerfile)
- [`2.1.8-nanoserver-sac2016`, `2.1-nanoserver-sac2016`, `2.1.8`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime/nanoserver-sac2016/amd64/Dockerfile)
- [`1.1.11-nanoserver-sac2016`, `1.1-nanoserver-sac2016`, `1.1.11`, `1.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.1/runtime/nanoserver-sac2016/amd64/Dockerfile)
- [`1.0.14-nanoserver-sac2016`, `1.0-nanoserver-sac2016`, `1.0.14`, `1.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.0/runtime/nanoserver-sac2016/amd64/Dockerfile)

## Windows Server, version 1809 arm32 tags

- [`2.2.2-nanoserver-1809-arm32`, `2.2-nanoserver-1809-arm32`, `2.2.2`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime/nanoserver-1809/arm32/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-nanoserver-1809-arm32`, `3.0-nanoserver-1809-arm32`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime/nanoserver-1809/arm32/Dockerfile)

For more information about these images and their history, please see [the relevant Dockerfile](https://github.com/dotnet/dotnet-docker/search?utf8=%E2%9C%93&q=FROM&type=Code). These images are updated via [pull requests to the `dotnet/dotnet-docker` GitHub repo](https://github.com/dotnet/dotnet-docker/pulls).

# Support

See [Microsoft Support for .NET Core](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Feedback

* [File a .NET Core Docker issue](https://github.com/dotnet/dotnet-docker/issues)
* [File a .NET Core issue](https://github.com/dotnet/core/issues)
* [File an ASP.NET Core issue](https://github.com/aspnet/home/issues)
* [File an issue for other components](Documentation/core-repos.md)
* [Ask on Stack Overflow](https://stackoverflow.com/questions/tagged/.net-core)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Windows Nano Server license](https://hub.docker.com/r/microsoft/nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/en-us/cloud-platform/windows-server-pricing)
