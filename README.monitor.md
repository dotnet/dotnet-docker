The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

As part of the .NET 5.0 release the Docker images are published to different repositories.  The 2.1 and 3.1 images are published to [core branded repositories](https://hub.docker.com/_/microsoft-dotnet-core/) while the 5.0 and higher versions will be published to [non-core branded repositories](https://hub.docker.com/_/microsoft-dotnet/).  See the [related issue](https://github.com/dotnet/dotnet-docker/issues/1765) for more details.

# Featured Tags

* `5.0` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:5.0`

# About This Image

This image contains the .NET Monitor tool.

Use this image as a sidecar container to collect diagnostic information from other containers running .NET Core 3.1 or later processes.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

TBD

# Related Repos

.NET Core 2.1/3.1:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET 5.0+:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/nightly](https://hub.docker.com/_/microsoft-dotnet-nightly/): .NET (Preview)
* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
5.0-alpine, 5.0.0-preview.3, 5.0, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/src/monitor/5.0/alpine/amd64/Dockerfile) | Alpine 3.12

You can retrieve a list of all available tags for dotnet/nightly/monitor at https://mcr.microsoft.com/v2/dotnet/nightly/monitor/tags/list.

# Support

See [Microsoft Support for .NET Core](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Image Update Policy

* We update the supported .NET Core images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:1909, buildpack-deps:bionic-scm, etc.).
* We publish .NET Core images as part of releasing new versions of .NET Core including major/minor and servicing.

# Feedback

* [File a .NET Core Docker issue](https://github.com/dotnet/dotnet-docker/issues)
* [File a .NET Core issue](https://github.com/dotnet/core/issues)
* [File an ASP.NET Core issue](https://github.com/aspnet/home/issues)
* [File an issue for other .NET components](https://github.com/dotnet/core/blob/master/Documentation/core-repos.md)
* [File a Visual Studio Docker Tools issue](https://github.com/microsoft/dockertools/issues)
* [File a Microsoft Container Registry (MCR) issue](https://github.com/microsoft/containerregistry/issues)
* [Ask on Stack Overflow](https://stackoverflow.com/questions/tagged/.net-core)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/master/documentation/image-artifact-details.md)
* [Windows Nano Server license](https://hub.docker.com/_/microsoft-windows-nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
