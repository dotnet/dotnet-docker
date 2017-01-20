# Supported tags and respective `Dockerfile` links

-       [`1.0.3-runtime`, `1.0-runtime` (*1.0/debian/runtime/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/runtime/Dockerfile)
-       [`1.0.3-runtime-nanoserver`, `1.0-runtime-nanoserver` (*1.0/nanoserver/runtime/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/nanoserver/runtime/Dockerfile)
-       [`1.0.3-runtime-deps`, `1.0-runtime-deps` (*1.0/debian/runtime-deps/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/runtime-deps/Dockerfile)
-       [`1.0.3-sdk-projectjson`, `1.0-sdk-projectjson` (*1.0/debian/sdk/projectjson/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/sdk/projectjson/Dockerfile)
-       [`1.0.3-sdk-projectjson-nanoserver`, `1.0-sdk-projectjson-nanoserver` (*1.0/nanoserver/sdk/projectjson/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/nanoserver/sdk/projectjson/Dockerfile)
-       [`1.0.3-sdk-msbuild-rc3`, `1.0-sdk-msbuild` (*1.0/debian/sdk/msbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/sdk/msbuild/Dockerfile)
-       [`1.0.3-sdk-msbuild-rc3-nanoserver`, `1.0-sdk-msbuild-nanoserver` (*1.0/nanoserver/sdk/msbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.0/nanoserver/sdk/msbuild/Dockerfile)
-       [`1.1.0-runtime`, `1.1-runtime`, `1-runtime`, `runtime` (*1.1/debian/runtime/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/debian/runtime/Dockerfile)
-       [`1.1.0-runtime-nanoserver`, `1.1-runtime-nanoserver`, `1-runtime-nanoserver`, `runtime-nanoserver` (*1.1/nanoserver/runtime/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/nanoserver/runtime/Dockerfile)
-       [`1.1.0-runtime-deps`, `1.1-runtime-deps`, `1-runtime-deps`, `runtime-deps` (*1.1/debian/runtime-deps/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/debian/runtime-deps/Dockerfile)
-       [`1.1.0-sdk-projectjson`, `1.1-sdk-projectjson`, `1-sdk-projectjson`, `sdk`, `latest` (*1.1/debian/sdk/projectjson/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/debian/sdk/projectjson/Dockerfile)
-       [`1.1.0-sdk-projectjson-nanoserver`, `1.1-sdk-projectjson-nanoserver`, `1-sdk-projectjson-nanoserver`, `sdk-nanoserver`, `nanoserver` (*1.1/nanoserver/sdk/projectjson/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/nanoserver/sdk/projectjson/Dockerfile)
-       [`1.1.0-sdk-msbuild-rc3`, `1.1-sdk-msbuild`, `1-sdk-msbuild` (*1.1/debian/sdk/msbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/debian/sdk/msbuild/Dockerfile)
-       [`1.1.0-sdk-msbuild-rc3-nanoserver`, `1.1-sdk-msbuild-nanoserver`, `1-sdk-msbuild-nanoserver` (*1.1/nanoserver/sdk/msbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/1.1/nanoserver/sdk/msbuild/Dockerfile)

For more information about these images and their history, please see [the relevant Dockerfile (`dotnet/dotnet-docker`)](https://github.com/dotnet/dotnet-docker/search?utf8=%E2%9C%93&q=FROM&type=Code). These images are updated via [pull requests to the `dotnet/dotnet-docker` GitHub repo](https://github.com/dotnet/dotnet-docker/pulls?utf8=%E2%9C%93&q=).

[![Downloads from Docker Hub](https://img.shields.io/docker/pulls/microsoft/dotnet.svg)](https://hub.docker.com/r/microsoft/dotnet)
[![Stars on Docker Hub](https://img.shields.io/docker/stars/microsoft/dotnet.svg)](https://hub.docker.com/r/microsoft/dotnet)

# What is .NET Core?

.NET Core is a general purpose development platform maintained by Microsoft and the .NET community on [GitHub](https://github.com/dotnet/core). It is cross-platform, supporting Windows, macOS and Linux, and can be used in device, cloud, and embedded/IoT scenarios.

.NET has several capabilities that make development easier, including automatic memory management, (runtime) generic types, reflection, asynchrony, concurrency, and native interop. Millions of developers take advantage of these capabilities to efficiently build high-quality applications.

You can use C# to write .NET Core apps. C# is simple, powerful, type-safe, and object-oriented while retaining the expressiveness and elegance of C-style languages. Anyone familiar with C and similar languages will find it straightforward to write in C#.

[.NET Core](https://github.com/dotnet/core) is open source (MIT and Apache 2 licenses) and was contributed to the [.NET Foundation](http://dotnetfoundation.org) by Microsoft in 2014. It can be freely adopted by individuals and companies, including for personal, academic or commercial purposes. Multiple companies use .NET Core as part of apps, tools, new platforms and hosting services.

> https://docs.microsoft.com/dotnet/articles/core/

![logo](https://avatars0.githubusercontent.com/u/9141961?v=3&amp;s=100)

# How to use these Images

## Run a simple application within a container

You can run a [sample application](https://hub.docker.com/r/microsoft/dotnet-samples/) (Linux image) that depends on these images in a container by running the following command.

```console
docker run microsoft/dotnet-samples
```

## Run a .NET Core application with the .NET Core Runtime image

For production scenarios, you will want to deploy and run a pre-built application with a .NET Core Runtime image. This results in smaller Docker images compared to the SDK image. The SDK is not needed for production scenarios. You can try the instructions below or use the [dotnetapp-prod sample](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-prod) if you want to try a pre-made version that's ready go.

You need to create a `Dockerfile`. Start by taking a dependency on a .NET Core runtime image by adding a `FROM` line to your `Dockerfile`:

```dockerfile
FROM microsoft/dotnet:runtime
```

For [Windows Containers][win-containers], you should instead include the following line in your `Dockerfile`:

```dockerfile
FROM microsoft/dotnet:nanoserver-runtime
```

Add the following additional lines to your Dockerfile.

```dockerfile
WORKDIR /dotnetapp
COPY out .
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

Build your application with the `dotnet` tools using the following commands:

```console
dotnet restore
dotnet publish -c Release -o out
```

Build and run the Docker image:

```console
docker build -t dotnetapp .
docker run -it --rm dotnetapp
```

The `Dockerfile` and the Docker commands assumes that your application is called `dotnetapp`. You can change the `Dockerfile` and the commands, as needed.

## Build and run an application with a .NET Core SDK Image

You can use the .NET Core SDK Docker image as a build and runtime environment. It's a useful image for iterative development and the easiest way to get started using .NET Core with Docker. It isn't recommended for production since it's a bigger image than necessary, although it can work for that, too. You can try the instructions below or use the [dotnetapp-dev sample](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-dev) if you want to try a pre-made version that's ready go.

In your Dockerfile, include the following line to reference the .NET Core SDK:

```dockerfile
FROM microsoft/dotnet
```

For [Windows Containers](http://aka.ms/windowscontainers), you should instead include the Nanoserver version of the .NET Core SDK image:

```dockerfile
FROM microsoft/dotnet:nanoserver
```

Add the following additional lines to your Dockerfile, which will both build and run your application in the container. This Dockerfile has been optimized (note the two `COPY` commands) to take advantage of Docker layering, resulting in faster image building for iterative development.

```dockerfile
WORKDIR /dotnetapp

# copy project.json and restore as distinct layers
COPY project.json .
RUN dotnet restore

# copy and build everything else
COPY . .
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/dotnetapp.dll"]
```

You can then build and run the Docker image:

```console
docker build -t dotnetapp .
docker run -it --rm dotnetapp
```

The `Dockerfile` and the Docker commands assumes that your application is called `dotnetapp`. You can change the `Dockerfile` and the commands, as needed.

## Interactively build and run a simple .NET Core application

You can interactively try out .NET Core by taking advantage of the convenience of a container. Try the following set of commands to create and run a .NET Core application in a minute (depending on your internet speed).

```console
docker run -it --rm microsoft/dotnet
[now in the container]
mkdir app
cd app
dotnet new
ls
dotnet restore
dotnet run
dotnet bin/Debug/netcoreapp1.0/app.dll
dotnet publish -c Release -o out
dotnet out/app.dll
exit
 ```

The experience is very similar using [Windows Containers][win-containers]. The commands should be the same, with the exception of the `docker run` (specifically the image name), `ls` and the directory separators. Try the following `docker run` command, to replace the `docker run` command above:

```console
docker run -it --rm microsoft/dotnet:nanoserver
```

The steps above are intended to show the basic functions of .NET Core tools. Try running `dotnet run` twice. You'll see that the second invocation skips compilation. The subsequent command after `dotnet run` demonstrates that you can run an application directly out of the bin folder, without the additional build logic that `dotnet run` adds. The last two commands demonstrate the publishing scenario, which prepares an app to be deployed on the same or other machine, with a requirement on only the .NET Core Runtime, not the larger SDK. Naturally, you don't have to exit immediately, but can continue to try out the product as long as you want.

You can extend your interactive exploration of .NET Core by git cloning the [dotnet/dotnet-docker-samples](https://github.com/dotnet/dotnet-docker-samples) repo. Try the following commands (only works on Linux containers), assuming you are running interactively in the container:

```console
git clone https://github.com/dotnet/dotnet-docker-samples
cd dotnet-docker-samples
cd dotnetapp-dev
dotnet restore
dotnet run
````

## Interactively build and run an ASP.NET Core application

 You can interactively try out ASP.NET Core by taking advantage of the convenience of a container. Try the following set of commands to create and run an ASP.NET Core application in a minute (depending on your internet speed).

 ```console
docker run -p 8000:80 -e "ASPNETCORE_URLS=http://+:80" -it --rm microsoft/dotnet
[now in the container]
mkdir app
cd app
dotnet new -t web
dotnet restore
dotnet run
exit
 ```

After running `dotnet run` in the container, browse to `http://localhost:8000` in your host machine.

The experience is very similar using [Windows Containers][win-containers]. The commands should be the same, with the exception of the `docker run` (specifically the image name). Replace the `docker run` command above with the following two commands:

 ```console
docker run -e "ASPNETCORE_URLS=http://+:80" -it --rm microsoft/dotnet:nanoserver
ipconfig
 ```
Copy the IP address from the output of `ipconfig`. After running `dotnet run` in the container, browse to that IP address in your browser on your host machine.

You should see a default ASP.NET Core site and logging activity in the container.

Please use the images at [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/). They are recommended and optimized for ASP.NET core development and production and are built on the images in this repo.

## Image variants

The `microsoft/dotnet` images come in different flavors, each designed for a specific use case.

### `microsoft/dotnet:<version>-sdk`

This is the defacto image. If you are unsure about what your needs are, you probably want to use this one. It is designed to be used both as a throw away container (mount your source code and start the container to start your app), as well as the base to build other images off of.

It contains the .NET Core SDK which is comprised of two parts:

1. .NET Core
2. .NET Core command line tools

Use this image for your development process (developing, building and testing applications).

There currently are two flavors of the sdk images, `projectjson` and `msbuild`.  These two flavors exist while the [transition occurs from project.json to msbuild](https://blogs.msdn.microsoft.com/dotnet/2016/05/23/changes-to-project-json/).  Once the tooling stabilizes, the project json variant will be deprecated and there will only be an msbuild variant.

### `microsoft/dotnet:<version>-runtime`

This image contains the .NET Core (runtime and libraries) and is optimized for running .NET Core apps in production.

### `microsoft/dotnet:<version>-runtime-deps`

This image contains the operating system with all of the native dependencies needed by .NET Core. This is for  [self-contained](https://docs.microsoft.com/dotnet/articles/core/deploying/index) applications.

### `microsoft/dotnet:<version>-nanoserver`

There are multiple images for Windows Nanoserver, for .NET Core and Runtime distributions.

For more information on Windows Containers and a getting started guide, please see: [Windows Containers Documentation](http://aka.ms/windowscontainers).

## More Examples using these Images

You can learn more about using .NET Core with Docker with [.NET Docker samples](https://github.com/dotnet/dotnet-docker-samples):

- [Development](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-dev) sample using the `sdk` .NET Core SDK image.
- [Production](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-prod) sample using the `runtime` .NET Core image.
- [Self-contained](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-selfcontained) sample using the `runtime-deps` base OS image (with native dependencies added).
- [Preview](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-preview) sample using a Preview `sdk` .NET Core SDK image.

Windows Container Dockerfile variants are provided at the same locations, above, and rely on slightly different .NET Core Docker images.

You can directly run a .NET Core Docker image from the [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/) repo.

See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/articles/core/docker/building-net-docker-images) to learn more about the various Docker images and when to use each for them.

## Related Repos

See the following related repos for other application types:

- [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) for ASP.NET Core images.
- [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) for ASP.NET Web Forms and MVC images.
- [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) for .NET Framework images (for web applications, see microsoft/aspnet).

# License

View [license information](https://www.microsoft.com/net/dotnet_library_license.htm) for the software contained in this image.

.NET Core source code is separately licensed as [MIT LICENSE](https://github.com/dotnet/core/blob/master/LICENSE).

The .NET Core Windows container images use the same license as the [Windows Server 2016 Nano Server base image](https://hub.docker.com/r/microsoft/nanoserver/), as follows:

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

You can read documentation for .NET Core, including Docker usage in the [.NET Core docs](https://docs.microsoft.com/en-us/dotnet/articles/core/). The docs are [open source on GitHub](https://github.com/dotnet/core-docs). Contributions are welcome!

[win-containers]: http://aka.ms/windowscontainers
