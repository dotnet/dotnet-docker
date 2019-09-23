# Build .NET Core Self-Contained Applications with Docker

You can build .NET Core self-contained apps with Docker. Self-contained apps are a great option if you do not want to take a dependence on the .NET Runtime, either as a global install or the [.NET Core Runtime Image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/). These instructions are based on the [.NET Core Docker Sample](README.md).

Multiple variations of this sample have been provided, as follows. Some of these example Dockerfiles are demonstrated later. Specify an alternate Dockerfile via the `-f` argument.

* [Alpine x64 self-contained sample, with build and unit testing](Dockerfile.alpine-x64-selfcontained)
* [Nano Server x64 self-contained sample, with build and unit testing](Dockerfile.nanoserver-x64-selfcontained)
* [Debian x64 self-contained sample, with build and unit testing](Dockerfile.debian-x64-selfcontained)
* [Debian ARM32 self-contained sample, with build and unit testing](Dockerfile.debian-arm32-selfcontained)

## Assembly trimming

The self-contained Dockerfiles make use of the .NET Core trimming tool that can reduce the size of applications by analyzing IL and trimming unused assemblies.  For a "Hello World" application, the linker reduces the size from ~68MB to ~28MB. The size wins may be more favorable or more moderate for larger applications.  To learn more about assembly trimming see the [.NET Core 3.0 Preview 6 blog](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-6/).

## Building the Sample for Windows Nano Server with Docker

You can build and run the [sample](Dockerfile.nanoserver-x64-selfcontained) in a [Nano Server container](https://hub.docker.com/_/microsoft-windows-nanoserver/) using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp:nanoserver-selfcontained -f Dockerfile.nanoserver-x64-selfcontained .
```

You can test the image with the following instructions.

```console
docker run --rm dotnetapp:nanoserver-selfcontained
```

## Building the Sample for Linux X64 with Docker

You can build and run the [sample](Dockerfile.selfcontained-linux-x64) in a [Debian container](https://hub.docker.com/r/library/debian/) using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp:alpine-x64-selfcontained -f Dockerfile.alpine-x64-selfcontained .
```

You can test the image with the following instructions.

```console
docker run --rm dotnetapp:alpine-x64-selfcontained
```

## Build and run the sample for Linux ARM32 with Docker

You can build and run the sample for ARM32 and Raspberry Pi with the [Use .NET Core and Docker on ARM32 and Raspberry Pi](dotnet-docker-arm32.md) instructions. To create a self-contained application with those instructions, you need to use the [Dockerfile.debian-arm32-selfcontained](Dockerfile.debian-arm32-selfcontained) Dockerfile.

## Build and run the sample for Linux ARM64 with Docker

You can build and run the sample for ARM64 with the [Use .NET Core and Docker on ARM64](dotnet-docker-arm64.md) instructions.

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
