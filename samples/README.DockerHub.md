# Latest Version of Common Tags

The following tags are the latest stable versions of the most commonly used images. The complete set of tags is listed further down.

- [`dotnetapp`](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp`](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

## Container sample: Run a simple application

Type the following command to run a sample console application:

```console
docker run --rm microsoft/dotnet-samples
```

## Container sample: Run a web application

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample microsoft/dotnet-samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-https.md) to use HTTPS with this image.

# Supported Tags

## Linux amd64 tags

- [`dotnetapp-stretch`, `dotnetapp`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-stretch`, `aspnetapp` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

## Linux arm32 tags

- [`dotnetapp-stretch-arm32v7`, `dotnetapp`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-stretch-arm32v7`, `aspnetapp` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

## Windows Server, version 1809 amd64 tags

- [`dotnetapp-nanoserver-1809`, `dotnetapp`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-nanoserver-1809`, `aspnetapp` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

## Windows Server, version 1803 amd64 tags

- [`dotnetapp-nanoserver-1803`, `dotnetapp`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-nanoserver-1803`, `aspnetapp` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

## Windows Server, version 1709 amd64 tags

- [`dotnetapp-nanoserver-1709`, `dotnetapp`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-nanoserver-1709`, `aspnetapp` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

## Windows Server 2016 amd64 tags

- [`dotnetapp-nanoserver-sac2016`, `dotnetapp`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-nanoserver-sac2016`, `aspnetapp` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

## Windows Server, version 1809 amd32 tags

- [`dotnetapp-nanoserver-1809-arm32`, `dotnetapp`, `latest` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-nanoserver-1809-arm32`, `aspnetapp` (*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

# .NET Core Docker Samples

This repo contains samples that demonstrate various .NET Core Docker configurations.

You can see the source for these samples at [dotnet/dotnet-docker/samples](https://github.com/dotnet/dotnet-docker/tree/master/samples/README.md) on GitHub. They can be updated by creating a pull request.

# What is .NET Core?

[.NET Core](https://docs.microsoft.com/dotnet/core/) is a general purpose development platform maintained by Microsoft and the .NET community on [GitHub](https://github.com/dotnet/core). It is cross-platform, supporting Windows, macOS and Linux, and can be used in device, cloud, and embedded/IoT scenarios.

.NET has several capabilities that make development easier, including automatic memory management, (runtime) generic types, reflection, asynchrony, concurrency, and native interop. Millions of developers take advantage of these capabilities to efficiently build high-quality applications.

You can use C# to write .NET Core apps. C# is simple, powerful, type-safe, and object-oriented while retaining the expressiveness and elegance of C-style languages. Anyone familiar with C and similar languages will find it straightforward to write in C#.

[.NET Core](https://github.com/dotnet/core) is open source (MIT and Apache 2 licenses) and was contributed to the [.NET Foundation](http://dotnetfoundation.org) by Microsoft in 2014. It can be freely adopted by individuals and companies, including for personal, academic or commercial purposes. Multiple companies use .NET Core as part of apps, tools, new platforms and hosting services.

You are invited to [contribute new features](https://github.com/dotnet/core/blob/master/CONTRIBUTING.md), fixes, or updates, large or small; we are always thrilled to receive pull requests, and do our best to process them as fast as we can.

> https://docs.microsoft.com/dotnet/core/

![logo](https://avatars0.githubusercontent.com/u/9141961?v=3&amp;s=100)

# Image variants

The `microsoft/dotnet-sample` images come in multiple flavors.

## `microsoft/dotnet-samples:dotnetapp`

This image demonstrates the minimal use of the [.NET Core Runtime image](https://hub.docker.com/r/microsoft/dotnet).

## `microsoft/dotnet-samples:aspnetapp`

This image demonstrates an ASP.NET Core web application using the [ASP.NET Core Runtime image](https://hub.docker.com/r/microsoft/dotnet).

# Issues

If you have any problems with or questions about this image, please contact us through a [GitHub issue](https://github.com/dotnet/dotnet-docker/issues).

# Licenses

- [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
- [Windows Nano Server license](https://hub.docker.com/r/microsoft/nanoserver/) (only applies to Windows containers)

# Related Repos

See the following related repos for other application types:

- [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core images.
- [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) for ASP.NET Web Forms and MVC images.
- [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) for .NET Framework images (for web applications, see microsoft/aspnet).
