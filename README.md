# Featured Repos

* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples

# About

[.NET](https://docs.microsoft.com/dotnet/core/) is a general purpose development platform maintained by Microsoft and the .NET community on [GitHub](https://github.com/dotnet/core). It is cross-platform, supports Windows, macOS, and Linux, and can be used in device, cloud, and embedded/IoT scenarios.

.NET has several capabilities that make development productive, including automatic memory management, (runtime) generic types, reflection, [asynchronous constructs](https://learn.microsoft.com/dotnet/csharp/async), concurrency, and native interop. Millions of developers take advantage of these capabilities to efficiently build high-quality applications.

You can use C# or F# to write .NET apps.

- [C#](https://docs.microsoft.com/dotnet/csharp/) is powerful, type-safe, and object-oriented while retaining the expressiveness and elegance of C-style languages. Anyone familiar with C and similar languages will find it straightforward to write in C#.
- [F#](https://docs.microsoft.com/dotnet/fsharp/) is a cross-platform, open-source, functional programming language for .NET. It also includes object-oriented and imperative programming.

[.NET](https://github.com/dotnet/core) is open source (MIT and Apache 2 licenses) and was contributed to the [.NET Foundation](http://dotnetfoundation.org) by Microsoft in 2014. It can be freely adopted by individuals and companies, including for personal, academic or commercial purposes. Multiple companies use .NET as part of apps, tools, new platforms and hosting services.

You are invited to [contribute new features](https://github.com/dotnet/core/blob/master/CONTRIBUTING.md), fixes, or updates, large or small; we are always thrilled to receive pull requests, and do our best to process them as fast as we can.

> https://docs.microsoft.com/dotnet/core/

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.

# Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

## Container sample: Run a simple application

Enter the following command to run a console app in a container with a pre-built [.NET Docker image](https://hub.docker.com/_/microsoft-dotnet-samples/):

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

## Container sample: Run a web app

Enter the following command to run a console app in a container with a pre-built [.NET Docker image](https://hub.docker.com/_/microsoft-dotnet-samples/):

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

When the app starts, navigate to `http://localhost:8000` in a browser.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md) to use HTTPS with this image.

# Related Repositories

.NET:

* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor/): .NET Monitor Tool (Preview)

.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

# Support

## Lifecycle

* [Microsoft Support for .NET](https://github.com/dotnet/core/blob/main/microsoft-support.md)
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
