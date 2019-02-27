The images from this repository are for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/dotnet-docker](https://hub.docker.com/r/microsoft/dotnet/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

# Featured Tags

* `2.2` (Current)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/runtime-deps:2.2`
* `2.1` (LTS)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/runtime-deps:2.1`

# About This Image

This image contains the native dependencies needed by .NET Core. It does not include .NET Core. It is for [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

* [.NET Core self-contained Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-selfcontained.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.debian-x64-selfcontained) builds and runs an application as a self-contained application.

# Related Repos

.NET Core (Preview):

* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)
* [dotnet/core-nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-core-nightly-sdk/): .NET Core SDK (Preview)
* [dotnet/core-nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime/): .NET Core Runtime (Preview)

.NET Core:

* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/): .NET Core
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet/): .NET Core Samples

.NET Framework:

* [dotnet/framework/aspnet](https://hub.docker.com/_/microsoft-dotnet-framework-aspnet): ASP.NET Web Forms and MVC
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/): .NET Framework
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/): .NET Framework and ASP.NET Samples

# Full Tag Listing

## Linux amd64 tags

- [`2.2.2-stretch-slim`, `2.2-stretch-slim`, `2.2.2`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile)
- [`2.2.2-alpine3.8`, `2.2-alpine3.8`, `2.2.2-alpine`, `2.2-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/runtime-deps/alpine3.8/amd64/Dockerfile)
- [`2.2.2-bionic`, `2.2-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile)
- [`2.1.8-stretch-slim`, `2.1-stretch-slim`, `2.1.8`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/amd64/Dockerfile)
- [`2.1.8-alpine3.7`, `2.1-alpine3.7`, `2.1.8-alpine`, `2.1-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/alpine3.7/amd64/Dockerfile)
- [`2.1.8-bionic`, `2.1-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/amd64/Dockerfile)
- [`1.1.11-stretch`, `1.1-stretch` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.1/runtime-deps/stretch/amd64/Dockerfile)
- [`1.0.14-jessie`, `1.0-jessie`, `1.0.14`, `1.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.0/runtime-deps/jessie/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim`, `3.0-stretch-slim`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/stretch-slim/amd64/Dockerfile)
- [`3.0.0-preview-alpine3.9`, `3.0-alpine3.9`, `3.0.0-preview-alpine`, `3.0-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/alpine3.9/amd64/Dockerfile)
- [`3.0.0-preview-bionic`, `3.0-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/amd64/Dockerfile)

## Linux arm64 tags

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim-arm64v8`, `3.0-stretch-slim-arm64v8`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/stretch-slim/arm64v8/Dockerfile)
- [`3.0.0-preview-bionic-arm64v8`, `3.0-bionic-arm64v8` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm64v8/Dockerfile)

## Linux arm32 tags

- [`2.2.2-stretch-slim-arm32v7`, `2.2-stretch-slim-arm32v7`, `2.2.2`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile)
- [`2.2.2-bionic-arm32v7`, `2.2-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile)
- [`2.1.8-stretch-slim-arm32v7`, `2.1-stretch-slim-arm32v7`, `2.1.8`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile)
- [`2.1.8-bionic-arm32v7`, `2.1-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/runtime-deps/bionic/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim-arm32v7`, `3.0-stretch-slim-arm32v7`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/stretch-slim/arm32v7/Dockerfile)
- [`3.0.0-preview-bionic-arm32v7`, `3.0-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/runtime-deps/bionic/arm32v7/Dockerfile)

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
