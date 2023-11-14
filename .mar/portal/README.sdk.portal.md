## About

This image contains the .NET SDK which is comprised of three parts:

1. .NET CLI
1. .NET runtime
1. ASP.NET Core

Use this image for your development process (developing, building and testing applications).

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Featured Tags

* `8.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:8.0`
* `7.0` (Standard Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:7.0`
* `6.0` (Long-Term Support)
  * `docker pull mcr.microsoft.com/dotnet/sdk:6.0`

## Related Repositories

.NET:

* [dotnet/aspnet](https://mcr.microsoft.com/product/dotnet/aspnet/about): ASP.NET Core Runtime
* [dotnet/runtime](https://mcr.microsoft.com/product/dotnet/runtime/about): .NET Runtime
* [dotnet/runtime-deps](https://mcr.microsoft.com/product/dotnet/runtime-deps/about): .NET Runtime Dependencies
* [dotnet/monitor](https://mcr.microsoft.com/product/dotnet/monitor/about): .NET Monitor Tool
* [dotnet/samples](https://mcr.microsoft.com/product/dotnet/samples/about): .NET Samples
* [dotnet/nightly/sdk](https://mcr.microsoft.com/product/dotnet/nightly/sdk/about): .NET SDK (Preview)

.NET Framework:

* [dotnet/framework](https://mcr.microsoft.com/catalog?search=dotnet/framework): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://mcr.microsoft.com/product/dotnet/framework/samples/about): .NET Framework, ASP.NET and WCF Samples

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Building .NET Apps with Docker

* [.NET Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile) builds, tests, and runs the sample. It includes and builds multiple projects.
* [ASP.NET Core Docker Sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md) - This [sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile) demonstrates using Docker with an ASP.NET Core Web App.

### Develop .NET Apps in a Container

The following samples show how to develop, build and test .NET applications with Docker without the need to install the .NET SDK.

* [Build .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/build-in-sdk-container.md)
* [Test .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-tests-in-sdk-container.md)
* [Run .NET Applications with SDK Container](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-in-sdk-container.md)

## Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

## Support

### Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

### Image Update Policy

* We update the supported .NET images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:ltsc2022, buildpack-deps:bionic-scm, etc.).
* We publish .NET images as part of releasing new versions of .NET including major/minor and servicing.

### Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

## License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
