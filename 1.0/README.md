# Supported tags and respective `Dockerfile` links

-       [`1.0.0-preview2-sdk`, `latest` (*1.0.0-preview2/debian/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/debian/Dockerfile)
-       [`1.0.0-preview2-nanoserver-sdk`, `nanoserver` (*1.0.0-preview2/nanoserver/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/nanoserver/Dockerfile)
/debian/onbuild/Dockerfile)
-       [`1.0.0-preview2.1-sdk` (*1.0.0-preview2.1/debian/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2.1/debian/Dockerfile)
-       [`1.0.0-preview2.1-nanoserver-sdk` (*1.0.0-preview2.1/nanoserver/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2.1/nanoserver/Dockerfile)
-       [`1.0.1-core`, `1.0-core`, `1-core`, `core` (*1.0/debian/core/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/core/Dockerfile)
-       [`1.0.1-nanoserver-core`, `1.0-nanoserver-core`, `1-nanoserver-core`, `nanoserver-core` (*1.0/nanoserver/core/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/nanoserver/core/Dockerfile)
-       [`1.0.1-core-deps`, `1.0-core-deps`, `1-core-deps`, `core-deps` (*1.0/debian/core-deps/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/core-deps/Dockerfile)
-       [`1.1.0-preview1-core` (*1.1.0-preview1/debian/core/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1.0-preview1/debian/core/Dockerfile)
-       [`1.1.0-preview1-nanoserver-core` (*1.1.0-preview1/nanoserver/core/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1.0-preview1/nanoserver/core/Dockerfile)
-       [`1.1.0-preview1-core-deps` (*1.1.0-preview1/debian/core-deps/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1.0-preview1/debian/core-deps/Dockerfile)

For more information about these images and their history, please see [the relevent Dockerfile (`dotnet/dotnet-docker`)](https://github.com/dotnet/dotnet-docker/search?utf8=%E2%9C%93&q=FROM&type=Code). These images are updated via [pull requests to the `dotnet/dotnet-docker` GitHub repo](https://github.com/dotnet/dotnet-docker/pulls?utf8=%E2%9C%93&q=).

[![Downloads from Docker Hub](https://img.shields.io/docker/pulls/microsoft/dotnet.svg)](https://hub.docker.com/r/microsoft/dotnet)
[![Stars on Docker Hub](https://img.shields.io/docker/stars/microsoft/dotnet.svg)](https://hub.docker.com/r/microsoft/dotnet)

# What is .NET Core?

.NET Core is a general purpose development platform maintained by Microsoft and the .NET community on [GitHub](https://github.com/dotnet/core). It is cross-platform, supporting Windows, macOS and Linux, and can be used in device, cloud, and embedded/IoT scenarios. It has several key features that are attractive to many developers, including automatic memory management and modern programming languages, that make it easier to efficiently build high-quality applications. It offers a high-level programming environment while providing low-level access to native memory and APIs.

You can use C# to write .NET Core apps. C# is simple, powerful, type-safe, and object-oriented while retaining the expressiveness and elegance of C-style languages. Anyone familiar with C and similar languages will find it straightforward to write in C#.

[.NET Core](https://github.com/dotnet/core) is open source (MIT license) and was contributed to the [.NET Foundation](http://dotnetfoundation.org) by Microsoft in 2014. It can be freely adopted by individuals and companies, including for personal, academic or commercial purposes. Multiple companies use .NET Core as part of apps, tools, new platforms and hosting services.

> https://docs.microsoft.com/en-us/dotnet/articles/core/

![logo](https://avatars0.githubusercontent.com/u/9141961?v=3&amp;s=100)

# How to use this Image

## Create a Docker Container for your .NET Core app

The most straightforward way to use this image is to use a .NET Core container as both the build and runtime environment. In your Dockerfile, writing the following to compile and run your project:

```dockerfile
FROM microsoft/dotnet:1.0.0-preview2-onbuild
```

This image includes multiple ONBUILD triggers (effectively projected into your Dockerfile) which should cover simple applications (not intended for production and/or larger apps). The build will copy, restore, build and `dotnet run` your app.

If you are using Windows containers, you should including the following in your Dockerfile:

```dockerfile
FROM microsoft/dotnet:nanoserver-onbuild
```

You can then build and run the Docker image:

```console
$ docker build -t my-dotnet-app .
$ docker run -it --rm --name my-running-app my-dotnet-app
```

# More Applicaton Scenarios

You can learn more about using .NET Core with Docker with [.NET Docker samples](https://github.com/dotnet/dotnet-docker-samples):

- [Devlopment](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-dev)
- [Production](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-prod)
- [Self-contained](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-selfcontained)
- [Preview](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-preview)

## Image variants

The `microsoft/dotnet` images come in different flavors, each designed for a specific use case.

See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/articles/core/docker/building-net-docker-images) to get an understanding of the different Docker images that are offered and when is the right use case for them.

### `microsoft/dotnet:<version>-sdk`

This image contains the .NET Core SDK which is comprised of two parts:

1. .NET Core
2. .NET Core command line tools

This image is recommended if you are trying .NET Core for the first time, as it allows both developing and running
applications. Use this image for your development process (developing, building and testing applications).

### `microsoft/dotnet:<version>-onbuild`

The most straightforward way to use this image is to use a Docker container as both the build and runtime environment for your application. Creating a simple `Dockerfile` with the following content in the same directory as your project files will compile and run your project:

```dockerfile
FROM microsoft/dotnet:onbuild
```

This image includes multiple `ONBUILD` triggers which should cover most applications. The build will `COPY . /dotnetapp` and `RUN dotnet restore`.

This image also includes the `ENTRYPOINT dotnet run` instruction which will run your application when the Docker image is run.

You can then build and run the Docker image:

```console
$ docker build -t my-dotnet-app .
$ docker run -it --rm --name my-running-app my-dotnet-app
```

### `microsoft/dotnet:<version>-core`

This image contains only .NET Core (runtime and libraries) and it is optimized for running [framework-dependent .NET Core applications](https://docs.microsoft.com/dotnet/articles/core/deploying/index). If you wish to run self-contained applications, please use the `core-deps` image described below. 

### `microsoft/dotnet:<version>-core-deps`

This image contains the operating system with all of the native dependencies needed by .NET Core. Use this image to:

1. Run a [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) application.
2. Build a custom copy of .NET Core by compiling [coreclr](https://github.com/dotnet/coreclr) and [corefx](https://github.com/dotnet/corefx).

### Windows Containers

Windows Containers images use the `microsoft/nanoserver` base OS image from Windows Server 2016.  For more information on Windows Containers and a getting started guide, please see: [Windows Containers Documentation](http://aka.ms/windowscontainers).

# License

View [license information](https://github.com/dotnet/core/blob/master/LICENSE) for the software contained in this image.

Windows Container images are licensed per the Windows license:

MICROSOFT SOFTWARE SUPPLEMENTAL LICENSE TERMS

CONTAINER OS IMAGE

Microsoft Corporation (or based on where you live, one of its affiliates) (referenced as “us,” “we,” or “Microsoft”) licenses this Container OS Image supplement to you (“Supplement”). You are licensed to use this Supplement in conjunction with the underlying host operating system software (“Host Software”) solely to assist running the containers feature in the Host Software. The Host Software license terms apply to your use of the Supplement. You may not use it if you do not have a license for the Host Software. You may use this Supplement with each validly licensed copy of the Host Software.

# Supported Docker versions

This image is officially supported on Docker version 1.12.2.

Please see [the Docker installation documentation](https://docs.docker.com/installation/) for details on how to upgrade your Docker daemon.

# User Feedback

## Issues

If you have any problems with or questions about this image, please contact us through a [GitHub issue](https://github.com/dotnet/dotnet-docker/issues).

## Contributing

You are invited to contribute new features, fixes, or updates, large or small; we are always thrilled to receive pull requests, and do our best to process them as fast as we can.

Before you start to code, please read the [.NET Core contribution guidelines](https://github.com/dotnet/coreclr/blob/master/CONTRIBUTING.md).

## Documentation

You can read documentation for .NET Core, including Docker usage in the [.NET Core docs](https://docs.microsoft.com/en-us/dotnet/articles/core/). The docs are also [open source on GitHub](https://github.com/dotnet/core-docs). Contributions are welcome!
