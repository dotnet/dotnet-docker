## About

**IMPORTANT**

**The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet](https://mcr.microsoft.com/product/dotnet/monitor/about) for images with official releases of [.NET](https://github.com/dotnet/core).**

This image contains the .NET Monitor tool.

Use this image as a sidecar container to collect diagnostic information from other containers running .NET Core 3.1 or later processes.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## Featured Tags

* `7` (RC)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:7`
* `6` (LTS)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:6`

## Related Repos

.NET:

* [dotnet/samples](https://mcr.microsoft.com/product/dotnet/samples/about): .NET Samples
* [dotnet/nightly/sdk](https://mcr.microsoft.com/product/dotnet/nightly/sdk/about): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://mcr.microsoft.com/product/dotnet/nightly/aspnet/about): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://mcr.microsoft.com/product/dotnet/nightly/runtime/about): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://mcr.microsoft.com/product/dotnet/nightly/runtime-deps/about): .NET Runtime Dependencies (Preview)

.NET Framework:

* [dotnet/framework](https://mcr.microsoft.com/catalog?search=dotnet/framework): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://mcr.microsoft.com/product/dotnet/framework/samples/about): .NET Framework, ASP.NET and WCF Samples

## Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

### Container sample: Run the tool

You can run a container with a pre-built [.NET Docker Image](https://mcr.microsoft.com/product/dotnet/monitor/about), based on the dotnet-monitor global tool.

See the [documentation](https://go.microsoft.com/fwlink/?linkid=2158052) for how to configure the image to be run in a Docker or Kubernetes environment, including how to configure authentication and certificates for https bindings.

## Support

### Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/microsoft-support.md)
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
