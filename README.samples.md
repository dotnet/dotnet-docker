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

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-https.md) to use HTTPS with this image.

# Related Repos

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/): .NET Core SDK
* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/): ASP.NET Core Runtime
* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/): .NET Core Runtime
* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/): .NET Core Runtime Dependencies
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

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
dotnetapp-buster-slim-arm32v7, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.linux-arm32) | Debian 10
aspnetapp-buster-slim-arm32v7, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.linux-arm32) | Debian 10

## Linux arm64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
dotnetapp-buster-slim-arm64v8, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile) | Debian 10
aspnetapp-buster-slim-arm64v8, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile) | Debian 10

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

## Windows Server 2019 arm32 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-1809-arm32v7, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.nanoserver-arm32)
aspnetapp-nanoserver-1809-arm32v7, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.nanoserver-arm32)

## Windows Server, version 1803 amd64 Tags
Tag | Dockerfile
---------| ---------------
dotnetapp-nanoserver-1803, dotnetapp, latest | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
aspnetapp-nanoserver-1803, aspnetapp | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

You can retrieve a list of all available tags for dotnet/core/samples at https://mcr.microsoft.com/v2/dotnet/core/samples/tags/list.

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
