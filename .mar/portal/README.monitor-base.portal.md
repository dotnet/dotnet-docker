## About

This image contains the .NET Monitor Base installation.

Use this image as a base image for building a .NET Monitor image with extensions.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

### New: Ubuntu Chiseled images

Ubuntu Chiseled .NET images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface by including only the minimal set of packages required to run .NET applications.

Please see the [Ubuntu Chiseled + .NET](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md) documentation page for more info.

## Featured Tags

* `8` (Release Candidate)
* `docker pull mcr.microsoft.com/dotnet/monitor/base:8`

## Related Repositories

.NET:

* [dotnet/sdk](https://mcr.microsoft.com/product/dotnet/sdk/about): .NET SDK
* [dotnet/aspnet](https://mcr.microsoft.com/product/dotnet/aspnet/about): ASP.NET Core Runtime
* [dotnet/runtime](https://mcr.microsoft.com/product/dotnet/runtime/about): .NET Runtime
* [dotnet/runtime-deps](https://mcr.microsoft.com/product/dotnet/runtime-deps/about): .NET Runtime Dependencies
* [dotnet/monitor](https://mcr.microsoft.com/product/dotnet/monitor/about): .NET Monitor Tool
* [dotnet/samples](https://mcr.microsoft.com/product/dotnet/samples/about): .NET Samples

.NET Framework:

* [dotnet/framework](https://mcr.microsoft.com/catalog?search=dotnet/framework): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://mcr.microsoft.com/product/dotnet/framework/samples/about): .NET Framework, ASP.NET and WCF Samples

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Building a Custom .NET Monitor Image

The following Dockerfiles demonstrate how you can use this base image to build a .NET Monitor image with a custom set of extensions.

* [Ubuntu Chiseled - amd64](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/amd64/Dockerfile)
* [Ubuntu Chiseled - arm64v8](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/arm64v8/Dockerfile)

### Tag Formatting

#### .NET Versions

All .NET container images have both "fixed version" and "floating version" tags.
Floating version tags will always reference the latest version of a specific .NET major version, while fixed version tags will always only reference a specific patch version.
For all tags below, `<.NET Version>` can be substituted for either `<Major.Minor>` or `<Major.Minor.Patch>`, for example: `7.0` or `7.0.12`.

#### Single-platform tags

These "fixed version" tags reference an image with a specific .NET version for a specific operating system and architecture.

- `<.NET Version>-<OS>-<Architecture>`
- `<.NET Version>-<OS>-<variant>-<Architecture>`
- `<.NET Version>-<OS>-<Architecture>`
- `<.NET Version>-<OS>-<variant>-<Architecture>`

#### Multi-platform tags

These tags reference images for [multiple platforms](https://docs.docker.com/build/building/multi-platform/).

- `<.NET Version>`
    - The version-only floating tag refers to the latest Debian version available at the .NET Major Version's release.
- `<.NET Version>-<OS>`
- `<.NET Version>-<OS>-<variant>`

#### Image Variants

By default, Ubuntu and Debian images for .NET 8 will have both `icu` and `tzdata` installed.
These images are intended to satisfy the most common use cases of .NET developers.

Our Alpine and Ubuntu Chiseled images are focused on size.
These images do not include `icu` or `tzdata`, meaning that these images only work with apps that are configured for [globalization-invariant mode](https://learn.microsoft.com/dotnet/core/runtime-config/globalization).
Apps that require globalization support can use the `extra` image variant of the [dotnet/runtime-deps](https://mcr.microsoft.com/product/dotnet/runtime-deps/about) images. 

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
