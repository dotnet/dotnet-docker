The images from this repository include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/dotnet-docker](https://hub.docker.com/r/microsoft/dotnet/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

# Featured Tags

* [`2.2` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/stretch-slim/amd64/Dockerfile) - Current
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/aspnet:2.2`
* [`2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/stretch-slim/amd64/Dockerfile) - LTS
  * `docker pull mcr.microsoft.com/dotnet/core-nightly/aspnet:2.1`

# About This Image

This image contains the ASP.NET Core and .NET Core runtimes and libraries and is optimized for running ASP.NET Core apps in production.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a web application

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample microsoft/dotnet-samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-https.md) to use HTTPS with this image.

# Related Repos

.NET Core (Preview):

* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)
* [dotnet/core-nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-core-nightly-sdk/): .NET Core SDK (Preview)
* [dotnet/core-nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime/): .NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime-deps/): .NET Core Runtime Dependencies (Preview)

.NET Core:

* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/): .NET Core
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet/): .NET Core Samples

# Full Tag Listing

## Linux amd64 tags

- [`2.2.1-stretch-slim`, `2.2-stretch-slim`, `2.2.1`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/stretch-slim/amd64/Dockerfile)
- [`2.2.1-alpine3.8`, `2.2-alpine3.8`, `2.2.1-alpine`, `2.2-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/alpine3.8/amd64/Dockerfile)
- [`2.2.1-bionic`, `2.2-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/bionic/amd64/Dockerfile)
- [`2.1.7-stretch-slim`, `2.1-stretch-slim`, `2.1.7`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/stretch-slim/amd64/Dockerfile)
- [`2.1.7-alpine3.7`, `2.1-alpine3.7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/alpine3.7/amd64/Dockerfile)
- [`2.1.7-alpine3.8`, `2.1-alpine3.8`, `2.1.7-alpine`, `2.1-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/alpine3.8/amd64/Dockerfile)
- [`2.1.7-bionic`, `2.1-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/bionic/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim`, `3.0-stretch-slim`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/stretch-slim/amd64/Dockerfile)
- [`3.0.0-preview-alpine3.8`, `3.0-alpine3.8`, `3.0.0-preview-alpine`, `3.0-alpine` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/alpine3.8/amd64/Dockerfile)
- [`3.0.0-preview-bionic`, `3.0-bionic` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/bionic/amd64/Dockerfile)

## Linux arm64 tags

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim-arm64v8`, `3.0-stretch-slim-arm64v8`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/stretch-slim/arm64v8/Dockerfile)
- [`3.0.0-preview-bionic-arm64v8`, `3.0-bionic-arm64v8` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/bionic/arm64v8/Dockerfile)

## Linux arm32 tags

- [`2.2.1-stretch-slim-arm32v7`, `2.2-stretch-slim-arm32v7`, `2.2.1`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/stretch-slim/arm32v7/Dockerfile)
- [`2.2.1-bionic-arm32v7`, `2.2-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/bionic/arm32v7/Dockerfile)
- [`2.1.7-stretch-slim-arm32v7`, `2.1-stretch-slim-arm32v7`, `2.1.7`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/stretch-slim/arm32v7/Dockerfile)
- [`2.1.7-bionic-arm32v7`, `2.1-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/bionic/arm32v7/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-stretch-slim-arm32v7`, `3.0-stretch-slim-arm32v7`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/stretch-slim/arm32v7/Dockerfile)
- [`3.0.0-preview-bionic-arm32v7`, `3.0-bionic-arm32v7` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/bionic/arm32v7/Dockerfile)

## Windows Server, version 1809 amd64 tags

- [`2.2.1-nanoserver-1809`, `2.2-nanoserver-1809`, `2.2.1`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/nanoserver-1809/amd64/Dockerfile)
- [`2.1.7-nanoserver-1809`, `2.1-nanoserver-1809`, `2.1.7`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/nanoserver-1809/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-nanoserver-1809`, `3.0-nanoserver-1809`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/nanoserver-1809/amd64/Dockerfile)

## Windows Server, version 1803 amd64 tags

- [`2.2.1-nanoserver-1803`, `2.2-nanoserver-1803`, `2.2.1`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/nanoserver-1803/amd64/Dockerfile)
- [`2.1.7-nanoserver-1803`, `2.1-nanoserver-1803`, `2.1.7`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/nanoserver-1803/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-nanoserver-1803`, `3.0-nanoserver-1803`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/nanoserver-1803/amd64/Dockerfile)

## Windows Server, version 1709 amd64 tags

- [`2.2.1-nanoserver-1709`, `2.2-nanoserver-1709`, `2.2.1`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/nanoserver-1709/amd64/Dockerfile)
- [`2.1.7-nanoserver-1709`, `2.1-nanoserver-1709`, `2.1.7`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/nanoserver-1709/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-nanoserver-1709`, `3.0-nanoserver-1709`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/nanoserver-1709/amd64/Dockerfile)

## Windows Server 2016 amd64 tags

- [`2.2.1-nanoserver-sac2016`, `2.2-nanoserver-sac2016`, `2.2.1`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/nanoserver-sac2016/amd64/Dockerfile)
- [`2.1.7-nanoserver-sac2016`, `2.1-nanoserver-sac2016`, `2.1.7`, `2.1` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.1/aspnet/nanoserver-sac2016/amd64/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-nanoserver-sac2016`, `3.0-nanoserver-sac2016`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/nanoserver-sac2016/amd64/Dockerfile)

## Windows Server, version 1809 arm32 tags

- [`2.2.1-nanoserver-1809-arm32`, `2.2-nanoserver-1809-arm32`, `2.2.1`, `2.2`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/2.2/aspnet/nanoserver-1809/arm32/Dockerfile)

**.NET Core 3.0 Preview tags**

- [`3.0.0-preview-nanoserver-1809-arm32`, `3.0-nanoserver-1809-arm32`, `3.0.0-preview`, `3.0` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/nightly/3.0/aspnet/nanoserver-1809/arm32/Dockerfile)

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
* [Windows Nano Server license](https://hub.docker.com/r/microsoft/nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/en-us/cloud-platform/windows-server-pricing)
