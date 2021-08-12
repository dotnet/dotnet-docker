# Featured Tags

* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp`

# About This Image

These images contain sample .NET and ASP.NET Core applications.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

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
docker run -it --rm -p 8000:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md) to use HTTPS with this image.

# Related Repos

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
dotnetapp-buster-slim-amd64, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile) | Debian 10
aspnetapp-buster-slim-amd64, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile) | Debian 10

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-buster-slim-arm32v7, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.debian-arm32) | Debian 10
aspnetapp-buster-slim-arm32v7, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.debian-arm32) | Debian 10

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-buster-slim-arm64v8, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile) | Debian 10
aspnetapp-buster-slim-arm64v8, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile) | Debian 10

## Nano Server 2022 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-ltsc2022, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
aspnetapp-nanoserver-ltsc2022, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)

## Nano Server, version 20H2 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-20H2, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
aspnetapp-nanoserver-20H2, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)

## Nano Server, version 2004 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-2004, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
aspnetapp-nanoserver-2004, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)

## Nano Server, version 1809 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-1809, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
aspnetapp-nanoserver-1809, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)

You can retrieve a list of all available tags for dotnet/samples at https://mcr.microsoft.com/v2/dotnet/samples/tags/list.
<!--End of generated tags-->

For tags contained in the old dotnet/core/samples repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core/samples/tags/list.

# Support

See [Microsoft Support for .NET](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Image Update Policy

* We update the supported .NET images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:ltsc2022, buildpack-deps:bionic-scm, etc.).
* We publish .NET images as part of releasing new versions of .NET including major/minor and servicing.

# Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
