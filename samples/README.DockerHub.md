# Linux amd64 tags

- [`dotnetapp-stretch`, `dotnetapp`, `latest` (*samples/dotnetapp/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-stretch`, `aspnetapp` (*samples/aspnetapp/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

# Windows Server, version 1709 amd64 tags

- [`dotnetapp-nanoserver-1709`, `dotnetapp`, `latest` (*samples/dotnetapp/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-nanoserver-1709`, `aspnetapp` (*samples/aspnetapp/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

# Windows Server 2016 amd64 tags

- [`dotnetapp-nanoserver-sac2016`, `dotnetapp`, `latest` (*samples/dotnetapp/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
- [`aspnetapp-nanoserver-sac2016`, `aspnetapp` (*samples/aspnetapp/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)

# Linux arm32 tags

- [`dotnetapp-stretch-arm32v7`, `dotnetapp`, `latest` (*samples/dotnetapp/Dockerfile.debian-arm32*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile.debian-arm32)
- [`aspnetapp-stretch-arm32v7`, `aspnetapp` (*samples/aspnetapp/Dockerfile.debian-arm32*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.debian-arm32)

# .NET Core Docker Samples

This repo contains samples that demonstrate various .NET Core Docker configurations.

You can see the source for these samples at [dotnet/dotnet-docker/samples](https://github.com/dotnet/dotnet-docker/tree/master/samples/README.md) on GitHub. They can be updated by creating a pull request.

## What is .NET Core?

.NET Core is a general-purpose development platform maintained by Microsoft and the .NET community on [GitHub](https://github.com/dotnet/core). It is cross-platform, supporting Windows, macOS and Linux, and can be used in device, cloud, and embedded/IoT scenarios.

.NET has several capabilities that make development easier, including automatic memory management, (runtime) generic types, reflection, asynchrony, concurrency, and native interop. Millions of developers take advantage of these capabilities to efficiently build high-quality applications.

You can use C# to write .NET Core apps. C# is simple, powerful, type-safe, and object-oriented while retaining the expressiveness and elegance of C-style languages. Anyone familiar with C and similar languages will find it straightforward to write in C#.

[.NET Core](https://github.com/dotnet/core) is open-source (MIT and Apache 2 licenses) and was contributed to the [.NET Foundation](http://dotnetfoundation.org) by Microsoft in 2014. It can be freely adopted by individuals and companies, including for personal, academic, or commercial purposes. Multiple companies use .NET Core as part of apps, tools, new platforms, and hosting services.

> https://docs.microsoft.com/dotnet/core/

![logo](https://avatars0.githubusercontent.com/u/9141961?v=3&amp;s=100)

## How to use these Images

### Run a sample .NET Core application within a container

The `dotnetapp` tag is a sample console application that depends on the [.NET Core Runtime image](https://hub.docker.com/r/microsoft/dotnet). You can run it in a container by running the following command.

```console
docker run --rm microsoft/dotnet-samples:dotnetapp
```

### Run a sample ASP.NET Core application within a container

The `aspnetapp` tag is a sample ASP.NET Web application that depends on the [.NET Core Runtime image](https://hub.docker.com/r/microsoft/dotnet). You can run it in a container by running the following command.

```console
docker run --rm -p 8000:80 microsoft/dotnet-samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. See [View the ASP.NET Core app in a running container on Windows](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md#view-the-aspnet-core-app-in-a-running-container-on-windows) for Windows-specific instructions.

## Image variants

The `microsoft/dotnet-sample` images come in multiple flavors.

### `microsoft/dotnet-samples:dotnetapp`

This image demonstrates the minimal use of the [.NET Core Runtime image](https://hub.docker.com/r/microsoft/dotnet).

### `microsoft/dotnet-samples:aspnetapp`

This image demonstrates an ASP.NET Core web application using the [.NET Core Runtime image](https://hub.docker.com/r/microsoft/dotnet).

> Note: The instructions above work for both Linux and Windows containers. The .NET Core docker images use [multi-arch tags](https://github.com/dotnet/announcements/issues/14), which abstract away different operating system choices for most use-cases.

## Related Repos

See the following related repos for other application types:

- [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core images.
- [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) for ASP.NET Core images.
- [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) for ASP.NET Web Forms and MVC images.
- [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) for .NET Framework images (for web applications, see microsoft/aspnet).

## License

View [license information](https://www.microsoft.com/net/dotnet_library_license.htm) for the software contained in this image.

The .NET Core Windows container images use the same license as the [Windows Server 2016 Nano Server base image](https://hub.docker.com/r/microsoft/nanoserver/), as follows:

MICROSOFT SOFTWARE SUPPLEMENTAL LICENSE TERMS

CONTAINER OS IMAGE

Microsoft Corporation (or based on where you live, one of its affiliates) (referenced as “us,” “we,” or “Microsoft”) licenses this Container OS Image supplement to you (“Supplement”). You are licensed to use this Supplement in conjunction with the underlying host operating system software (“Host Software”) solely to assist running the containers feature in the Host Software. The Host Software license terms apply to your use of the Supplement. You may not use it if you do not have a license for the Host Software. You may use this Supplement with each validly licensed copy of the Host Software.

## Supported Docker versions

Supported Docker versions: [the latest release](https://github.com/docker/docker/releases/latest) (down to 1.12.2 on a best-effort basis)

Please see [the Docker installation documentation](https://docs.docker.com/installation/) for details on how to upgrade your Docker daemon.

## User Feedback

### Issues

If you have any problems with or questions about this image, please contact us through a [GitHub issue](https://github.com/dotnet/dotnet-docker/issues).

### Contributing

You are invited to contribute new features, fixes, or updates, large or small; we are always thrilled to receive pull requests, and do our best to process them as fast as we can.

Before you start to code, please read the [.NET Core contribution guidelines](https://github.com/dotnet/coreclr/blob/master/CONTRIBUTING.md).

### Documentation

You can read documentation for .NET Core, including Docker usage in the [.NET Core docs](https://docs.microsoft.com/dotnet/articles/core/). The docs are [open source on GitHub](https://github.com/dotnet/core-docs). Contributions are welcome!
