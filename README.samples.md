# Featured Tags

* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp`

# About

These images contain sample .NET and ASP.NET Core applications.

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

## New: Ubuntu Chiseled Images

Ubuntu Chiseled .NET images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface by including only the minimal set of packages required to run .NET applications.

Please see the [Ubuntu Chiseled + .NET](https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md) documentation page for more info.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a simple application

You can quickly run a container with a pre-built [.NET Docker image](https://hub.docker.com/_/microsoft-dotnet-samples/), based on the [.NET console sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

## Container sample: Run a web application

You can quickly run a container with a pre-built [.NET Docker image](https://hub.docker.com/_/microsoft-dotnet-samples/), based on the [ASP.NET Core sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md).

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:8080 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. You can also view the ASP.NET Core site running in the container from another machine with a local IP address such as `http://192.168.1.18:8000`.

> Note: ASP.NET Core apps (in official images) listen to [port 8080 by default](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7), starting with .NET 8. The [`-p` argument](https://docs.docker.com/engine/reference/commandline/run/#publish) in these examples maps host port `8000` to container port `8080` (`host:container` mapping). The container will not be accessible without this mapping. ASP.NET Core can be [configured to listen on a different or additional port](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel/endpoints).

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md) to use HTTPS with this image.

# Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.

# Related Repositories

.NET:

* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-alpine-amd64, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.alpine) | Alpine
aspnetapp-alpine-amd64, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.alpine) | Alpine

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-alpine-arm32v7, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.alpine) | Alpine
aspnetapp-alpine-arm32v7, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.alpine) | Alpine

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-alpine-arm64v8, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.alpine) | Alpine
aspnetapp-alpine-arm64v8, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.alpine) | Alpine

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-ltsc2022, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.nanoserver)
aspnetapp-nanoserver-ltsc2022, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.nanoserver)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-1809, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.nanoserver)
aspnetapp-nanoserver-1809, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.nanoserver)

You can retrieve a list of all available tags for dotnet/samples at https://mcr.microsoft.com/v2/dotnet/samples/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core/samples repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core/samples/tags/list.

*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)*

# Support

These sample images are not intended for production use and may be subject to breaking changes or removal at any time. They are provided as a starting point for developers to experiment with and learn about .NET in a containerized environment.

## Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/support.md)
* [Supported Container Platforms Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
* [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)

## Image Update Policy

* We update the supported .NET images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:ltsc2022, buildpack-deps:bionic-scm, etc.).
* We publish .NET images as part of releasing new versions of .NET including major/minor and servicing.

## Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
