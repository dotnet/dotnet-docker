# Featured Tags

* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/core/samples:dotnetapp`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/core/samples:aspnetapp`

# About This Image

These images contain sample .NET Core and ASP.NET Core applications.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a simple application

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/), based on the [.NET Core console sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

## Container sample: Run a web application

You can quickly run a container with a pre-built [.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/), based on the [ASP.NET Core sample](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md).

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/core/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/master/samples/host-aspnetcore-https.md) to use HTTPS with this image.

# Related Repos

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/): .NET Core SDK
* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/): ASP.NET Core Runtime
* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/): .NET Core Runtime
* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/): .NET Core Runtime Dependencies
* [dotnet/nightly](https://hub.docker.com/_/microsoft-dotnet-nightly/): .NET Core (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-buster-slim, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile) | Debian 10
aspnetapp-buster-slim, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile) | Debian 10

## Linux arm32 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-buster-slim-arm32v7, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.debian-arm32) | Debian 10
aspnetapp-buster-slim-arm32v7, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.debian-arm32) | Debian 10

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-buster-slim-arm64v8, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile) | Debian 10
aspnetapp-buster-slim-arm64v8, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile) | Debian 10

## Windows Server, version 2004 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-2004, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
aspnetapp-nanoserver-2004, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

## Windows Server, version 1909 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-1909, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
aspnetapp-nanoserver-1909, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

## Windows Server, version 1903 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-1903, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
aspnetapp-nanoserver-1903, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

## Windows Server 2019 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-1809, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
aspnetapp-nanoserver-1809, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

You can retrieve a list of all available tags for dotnet/core/samples at https://mcr.microsoft.com/v2/dotnet/core/samples/tags/list.

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
