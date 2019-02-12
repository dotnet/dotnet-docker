The images from this repository include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/dotnet-docker](https://hub.docker.com/r/microsoft/dotnet/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

# Featured Tags

* `2.2` (Current)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/sdk:2.2`
* `2.1` (LTS)
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/sdk:2.1`

# About This Image

This image contains the .NET Core SDK which is comprised of three parts:

1. .NET Core CLI
1. .NET Core
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Building .NET Core Apps with Docker

* [.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

## Develop .NET Core Apps in a Container

* [Develop .NET Core Applications](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-dev-in-container.md) - This sample shows how to develop, build and test .NET Core applications with Docker without the need to install the .NET Core SDK.
* [Develop ASP.NET Core Applications](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnet-docker-dev-in-container.md) - This sample shows how to develop and test ASP.NET Core applications with Docker without the need to install the .NET Core SDK.

# Related Repos

.NET Core (Preview):

* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)
* [dotnet/core-nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime/): .NET Core Runtime (Preview)
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

- [`2.2.104-stretch`, `2.2-stretch`, `2.2.104`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/stretch/amd64/Dockerfile)
- [`2.2.104-alpine3.8`, `2.2-alpine3.8`, `2.2.104-alpine`, `2.2-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/alpine3.8/amd64/Dockerfile)
- [`2.2.104-bionic`, `2.2-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/bionic/amd64/Dockerfile)
- [`2.1.504-stretch`, `2.1-stretch`, `2.1.504`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/stretch/amd64/Dockerfile)
- [`2.1.504-alpine3.7`, `2.1-alpine3.7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/alpine3.7/amd64/Dockerfile)
- [`2.1.504-alpine3.8`, `2.1-alpine3.8`, `2.1.504-alpine`, `2.1-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/alpine3.8/amd64/Dockerfile)
- [`2.1.504-bionic`, `2.1-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/bionic/amd64/Dockerfile)
- [`1.1.12-stretch`, `1.1-stretch` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.1/sdk/stretch/amd64/Dockerfile)
- [`1.1.12-jessie`, `1.1-jessie`, `1.1.12`, `1.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.1/sdk/jessie/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview-stretch`, `3.0-stretch`, `3.0.100-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/stretch/amd64/Dockerfile)
- [`3.0.100-preview-alpine3.9`, `3.0-alpine3.9`, `3.0.100-preview-alpine`, `3.0-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/alpine3.9/amd64/Dockerfile)
- [`3.0.100-preview-bionic`, `3.0-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/bionic/amd64/Dockerfile)

## Linux arm64 tags

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview-stretch-arm64v8`, `3.0-stretch-arm64v8`, `3.0.100-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/stretch/arm64v8/Dockerfile)
- [`3.0.100-preview-bionic-arm64v8`, `3.0-bionic-arm64v8` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/bionic/arm64v8/Dockerfile)

## Linux arm32 tags

- [`2.2.104-stretch-arm32v7`, `2.2-stretch-arm32v7`, `2.2.104`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/stretch/arm32v7/Dockerfile)
- [`2.2.104-bionic-arm32v7`, `2.2-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/bionic/arm32v7/Dockerfile)
- [`2.1.504-stretch-arm32v7`, `2.1-stretch-arm32v7`, `2.1.504`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/stretch/arm32v7/Dockerfile)
- [`2.1.504-bionic-arm32v7`, `2.1-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/bionic/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview-stretch-arm32v7`, `3.0-stretch-arm32v7`, `3.0.100-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/stretch/arm32v7/Dockerfile)
- [`3.0.100-preview-bionic-arm32v7`, `3.0-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/bionic/arm32v7/Dockerfile)

## Windows Server, version 1809 amd64 tags

- [`2.2.104-nanoserver-1809`, `2.2-nanoserver-1809`, `2.2.104`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/nanoserver-1809/amd64/Dockerfile)
- [`2.1.504-nanoserver-1809`, `2.1-nanoserver-1809`, `2.1.504`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/nanoserver-1809/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview-nanoserver-1809`, `3.0-nanoserver-1809`, `3.0.100-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/nanoserver-1809/amd64/Dockerfile)

## Windows Server, version 1803 amd64 tags

- [`2.2.104-nanoserver-1803`, `2.2-nanoserver-1803`, `2.2.104`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/nanoserver-1803/amd64/Dockerfile)
- [`2.1.504-nanoserver-1803`, `2.1-nanoserver-1803`, `2.1.504`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/nanoserver-1803/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview-nanoserver-1803`, `3.0-nanoserver-1803`, `3.0.100-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/nanoserver-1803/amd64/Dockerfile)

## Windows Server, version 1709 amd64 tags

- [`2.2.104-nanoserver-1709`, `2.2-nanoserver-1709`, `2.2.104`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/nanoserver-1709/amd64/Dockerfile)
- [`2.1.504-nanoserver-1709`, `2.1-nanoserver-1709`, `2.1.504`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/nanoserver-1709/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview-nanoserver-1709`, `3.0-nanoserver-1709`, `3.0.100-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/nanoserver-1709/amd64/Dockerfile)

## Windows Server 2016 amd64 tags

- [`2.2.104-nanoserver-sac2016`, `2.2-nanoserver-sac2016`, `2.2.104`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/nanoserver-sac2016/amd64/Dockerfile)
- [`2.1.504-nanoserver-sac2016`, `2.1-nanoserver-sac2016`, `2.1.504`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/sdk/nanoserver-sac2016/amd64/Dockerfile)
- [`1.1.12-nanoserver-sac2016`, `1.1-nanoserver-sac2016`, `1.1.12`, `1.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/1.1/sdk/nanoserver-sac2016/amd64/Dockerfile)

## Windows Server, version 1809 arm32 tags

- [`2.2.104-nanoserver-1809-arm32`, `2.2-nanoserver-1809-arm32`, `2.2.104`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/sdk/nanoserver-1809/arm32/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview-nanoserver-1809-arm32`, `3.0-nanoserver-1809-arm32`, `3.0.100-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/sdk/nanoserver-1809/arm32/Dockerfile)

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
