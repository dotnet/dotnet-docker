# Featured Tags

* `2.2` (Current)
  * `docker pull mcr.microsoft.com/dotnet/core/sdk:2.2`
* `2.1` (LTS)
  * `docker pull mcr.microsoft.com/dotnet/core/sdk:2.1`

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

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/): ASP.NET Core Runtime
* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/): .NET Core Runtime
* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/): .NET Core Runtime Dependencies
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 tags

- [`2.2.203-stretch`, `2.2-stretch`, `2.2.203`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/sdk/stretch/amd64/Dockerfile)
- [`2.2.203-alpine3.9`, `2.2-alpine3.9` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/sdk/alpine3.9/amd64/Dockerfile)
- [`2.2.203-alpine3.8`, `2.2-alpine3.8`, `2.2.203-alpine`, `2.2-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/sdk/alpine3.8/amd64/Dockerfile)
- [`2.2.203-bionic`, `2.2-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/sdk/bionic/amd64/Dockerfile)
- [`2.1.603-stretch`, `2.1-stretch`, `2.1.603`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/stretch/amd64/Dockerfile)
- [`2.1.603-alpine3.9`, `2.1-alpine3.9` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/alpine3.9/amd64/Dockerfile)
- [`2.1.603-alpine3.7`, `2.1-alpine3.7`, `2.1.603-alpine`, `2.1-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/alpine3.7/amd64/Dockerfile)
- [`2.1.603-bionic`, `2.1-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/bionic/amd64/Dockerfile)
- [`1.1.13-stretch`, `1.1-stretch` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/sdk/stretch/amd64/Dockerfile)
- [`1.1.13-jessie`, `1.1-jessie`, `1.1.13`, `1.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/sdk/jessie/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview3-stretch`, `3.0-stretch`, `3.0.100-preview3`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/stretch/amd64/Dockerfile)
- [`3.0.100-preview3-alpine3.9`, `3.0-alpine3.9`, `3.0.100-preview3-alpine`, `3.0-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/alpine3.9/amd64/Dockerfile)
- [`3.0.100-preview3-bionic`, `3.0-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/bionic/amd64/Dockerfile)

## Linux arm64 tags

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview3-stretch-arm64v8`, `3.0-stretch-arm64v8`, `3.0.100-preview3`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/stretch/arm64v8/Dockerfile)
- [`3.0.100-preview3-bionic-arm64v8`, `3.0-bionic-arm64v8` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/bionic/arm64v8/Dockerfile)

## Linux arm32 tags

- [`2.2.203-stretch-arm32v7`, `2.2-stretch-arm32v7`, `2.2.203`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/sdk/stretch/arm32v7/Dockerfile)
- [`2.2.203-bionic-arm32v7`, `2.2-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/sdk/bionic/arm32v7/Dockerfile)
- [`2.1.603-stretch-arm32v7`, `2.1-stretch-arm32v7`, `2.1.603`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/stretch/arm32v7/Dockerfile)
- [`2.1.603-bionic-arm32v7`, `2.1-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/bionic/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview3-stretch-arm32v7`, `3.0-stretch-arm32v7`, `3.0.100-preview3`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/stretch/arm32v7/Dockerfile)
- [`3.0.100-preview3-bionic-arm32v7`, `3.0-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/bionic/arm32v7/Dockerfile)

## Windows Server, version 1809 amd64 tags

- [`2.2.203-nanoserver-1809`, `2.2-nanoserver-1809`, `2.2.203`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/sdk/nanoserver-1809/amd64/Dockerfile)
- [`2.1.603-nanoserver-1809`, `2.1-nanoserver-1809`, `2.1.603`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/nanoserver-1809/amd64/Dockerfile)
- [`1.1.13-nanoserver-1809`, `1.1-nanoserver-1809`, `1.1.13`, `1.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/sdk/nanoserver-1809/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview3-nanoserver-1809`, `3.0-nanoserver-1809`, `3.0.100-preview3`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/nanoserver-1809/amd64/Dockerfile)

## Windows Server, version 1803 amd64 tags

- [`2.2.203-nanoserver-1803`, `2.2-nanoserver-1803`, `2.2.203`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/sdk/nanoserver-1803/amd64/Dockerfile)
- [`2.1.603-nanoserver-1803`, `2.1-nanoserver-1803`, `2.1.603`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/sdk/nanoserver-1803/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview3-nanoserver-1803`, `3.0-nanoserver-1803`, `3.0.100-preview3`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/nanoserver-1803/amd64/Dockerfile)

## Windows Server, version 1809 arm32 tags

- [`2.2.203-nanoserver-1809-arm32v7`, `2.2-nanoserver-1809-arm32v7`, `2.2.203`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/sdk/nanoserver-1809/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.100-preview3-nanoserver-1809-arm32v7`, `3.0-nanoserver-1809-arm32v7`, `3.0.100-preview3`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/sdk/nanoserver-1809/arm32v7/Dockerfile)

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
* [Windows Nano Server license](https://hub.docker.com/_/microsoft-windows-nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/en-us/cloud-platform/windows-server-pricing)
