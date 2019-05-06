# Featured Tags

* `2.2` (Current)
  * `docker pull mcr.microsoft.com/dotnet/core/aspnet:2.2`
* `2.1` (LTS)
  * `docker pull mcr.microsoft.com/dotnet/core/aspnet:2.1`

# About This Image

This image contains the ASP.NET Core and .NET Core runtimes and libraries and is optimized for running ASP.NET Core apps in production.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

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
* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/): .NET Core Runtime
* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/): .NET Core Runtime Dependencies
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Full Tag Listing

## Linux amd64 tags

- [`2.2.4-stretch-slim`, `2.2-stretch-slim`, `2.2.4`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/aspnet/stretch-slim/amd64/Dockerfile)
- [`2.2.4-alpine3.9`, `2.2-alpine3.9` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/aspnet/alpine3.9/amd64/Dockerfile)
- [`2.2.4-alpine3.8`, `2.2-alpine3.8`, `2.2.4-alpine`, `2.2-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/aspnet/alpine3.8/amd64/Dockerfile)
- [`2.2.4-bionic`, `2.2-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/aspnet/bionic/amd64/Dockerfile)
- [`2.1.10-stretch-slim`, `2.1-stretch-slim`, `2.1.10`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnet/stretch-slim/amd64/Dockerfile)
- [`2.1.10-alpine3.9`, `2.1-alpine3.9` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnet/alpine3.9/amd64/Dockerfile)
- [`2.1.10-alpine3.7`, `2.1-alpine3.7`, `2.1.10-alpine`, `2.1-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnet/alpine3.7/amd64/Dockerfile)
- [`2.1.10-bionic`, `2.1-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnet/bionic/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview5-buster-slim`, `3.0-buster-slim`, `3.0.0-preview5`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/buster-slim/amd64/Dockerfile)
- [`3.0.0-preview5-alpine3.9`, `3.0-alpine3.9`, `3.0.0-preview5-alpine`, `3.0-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/alpine3.9/amd64/Dockerfile)
- [`3.0.0-preview5-disco`, `3.0-disco` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/disco/amd64/Dockerfile)
- [`3.0.0-preview5-bionic`, `3.0-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/bionic/amd64/Dockerfile)

## Linux arm64 tags

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview5-buster-slim-arm64v8`, `3.0-buster-slim-arm64v8`, `3.0.0-preview5`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/buster-slim/arm64v8/Dockerfile)
- [`3.0.0-preview5-disco-arm64v8`, `3.0-disco-arm64v8` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/disco/arm64v8/Dockerfile)
- [`3.0.0-preview5-bionic-arm64v8`, `3.0-bionic-arm64v8` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/bionic/arm64v8/Dockerfile)

## Linux arm32 tags

- [`2.2.4-stretch-slim-arm32v7`, `2.2-stretch-slim-arm32v7`, `2.2.4`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/aspnet/stretch-slim/arm32v7/Dockerfile)
- [`2.2.4-bionic-arm32v7`, `2.2-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/aspnet/bionic/arm32v7/Dockerfile)
- [`2.1.10-stretch-slim-arm32v7`, `2.1-stretch-slim-arm32v7`, `2.1.10`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnet/stretch-slim/arm32v7/Dockerfile)
- [`2.1.10-bionic-arm32v7`, `2.1-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnet/bionic/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview5-buster-slim-arm32v7`, `3.0-buster-slim-arm32v7`, `3.0.0-preview5`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/buster-slim/arm32v7/Dockerfile)
- [`3.0.0-preview5-disco-arm32v7`, `3.0-disco-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/disco/arm32v7/Dockerfile)
- [`3.0.0-preview5-bionic-arm32v7`, `3.0-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/bionic/arm32v7/Dockerfile)

## Windows Server, version 1809 amd64 tags

- [`2.2.4-nanoserver-1809`, `2.2-nanoserver-1809`, `2.2.4`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/aspnet/nanoserver-1809/amd64/Dockerfile)
- [`2.1.10-nanoserver-1809`, `2.1-nanoserver-1809`, `2.1.10`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnet/nanoserver-1809/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview5-nanoserver-1809`, `3.0-nanoserver-1809`, `3.0.0-preview5`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/nanoserver-1809/amd64/Dockerfile)

## Windows Server, version 1803 amd64 tags

- [`2.2.4-nanoserver-1803`, `2.2-nanoserver-1803`, `2.2.4`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/aspnet/nanoserver-1803/amd64/Dockerfile)
- [`2.1.10-nanoserver-1803`, `2.1-nanoserver-1803`, `2.1.10`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.1/aspnet/nanoserver-1803/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview5-nanoserver-1803`, `3.0-nanoserver-1803`, `3.0.0-preview5`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/nanoserver-1803/amd64/Dockerfile)

## Windows Server, version 1809 arm32 tags

- [`2.2.4-nanoserver-1809-arm32v7`, `2.2-nanoserver-1809-arm32v7`, `2.2.4`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/2.2/aspnet/nanoserver-1809/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview5-nanoserver-1809-arm32v7`, `3.0-nanoserver-1809-arm32v7`, `3.0.0-preview5`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/3.0/aspnet/nanoserver-1809/arm32v7/Dockerfile)

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
