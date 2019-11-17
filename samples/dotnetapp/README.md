# .NET Core Docker Sample

This sample demonstrates how to build container images for .NET Core console apps, which are supported for Linux and Windows containers, and for x64, ARM32 and ARM64 architectures.

The sample builds an application in a [.NET Core SDK container](https://hub.docker.com/_/microsoft-dotnet-core-sdk/) and then copies the build result into a new image (the one you are building) based on the smaller [.NET Core Docker Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/). You can test the built image locally or deploy it to a container registry.

The instructions assume that you have cloned this repo, have [Docker](https://www.docker.com/products/docker) installed, and have a command prompt open within the `samples/dotnetapp` directory within the repo.

If want to skip to the final result, you can try a pre-built version with the following command:

```console
docker run --rm mcr.microsoft.com/dotnet/core/samples
```

## Build and run a container image

You can to build a .NET Core-based container image using the following instructions:

```console
docker build --pull -t dotnetapp .
docker run --rm dotnetapp Hello .NET Core from Docker
```

You can use the `docker images` command to see a listing of your image, as you can see in the following example.

```console
rich@thundera dotnetapp % docker images dotnetapp
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
dotnetapp           latest              baee380605f4        14 seconds ago      189MB
```

The Docker build command used above uses the standard pattern to build images, so doesn't tell you much about what is actually happening. The logic is provided in the [Dockerfile](Dockerfile) file, which follows.

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY *.cs .
RUN dotnet publish -c release -o /app

# final stage, which will result in generated image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./dotnetapp"]
```

This Dockerfile builds and publishes the .NET Core application and then copies the result to a rumtime-based image. It copies and restores the project file as the first step so that the results of those commands can be cached for subsequent builds since project file edits are less common than source code edits.

## Build an image optimized for startup performance

You can opt any application into Ready to Run compilation by adding a `PublishReadToRun` property. This is what the `-trim` samples do (they are explained shortly). The default `Dockerfile` that come with the sample doesn't do that because the application is too small to warrant it. .NET Core provides the majority of the startup benefit available since most of the code actually run in an application within the core framework, which itself is Ready to Run compiled.

You can add this property in two ways:

- Add the property to your profile file, as: `<PublishReadyToRun>true</PublishReadyToRun>'
- Add the property on the command line, as:  `/p:PublishReadToRun=true`

## Build an image for Alpine, Debian or Ubuntu

By default, .NET Core uses Debian base images for Linux containers. You will get a Debian-based image if you use a tag with only a version number, such as `3.1`, as opposed to a distro-specific tag like `3.1-alpine`.

This sample includes Dockerfile examples that explicitly target Alpine, Debian and Ubuntu. Docker makes it easy to use alternate Dockfiles by using the `-f` argument.

The following example demonstrates targeting distros explictly and also shows the size differences between the distros. Tags are added to the image name to differentiate the images.

```console
docker build --pull -t dotnetapp:debian -f Dockerfile.debian-x64 .
docker build --pull -t dotnetapp:ubuntu -f Dockerfile.ubuntu-x64 .
docker build --pull -t dotnetapp:alpine -f Dockerfile.alpine-x64 .
```

You can use `docker images` to see the images you've built:

```console
rich@thundera dotnetapp % docker images dotnetapp
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
dotnetapp           alpine              8933fb9821e8        8 seconds ago       87MB
dotnetapp           ubuntu              373df08a06ec        25 seconds ago      187MB
dotnetapp           debian              229dd121a96b        39 seconds ago      190MB
dotnetapp           latest              303eabf97376        56 seconds ago      190MB
```

You can run these images:

```console
docker run dotnetapp
docker run dotnetapp:debian
docker run dotnetapp:ubuntu
docker run dotnetapp:alpine
```

If you want to double check the distro of an application, you can do that by configuring a different entrypoint when you run the image, as you see in the following example.

```console
rich@thundera ~ % docker run --entrypoint cat dotnetapp /etc/os-release    
PRETTY_NAME="Debian GNU/Linux 10 (buster)"
NAME="Debian GNU/Linux"
VERSION_ID="10"
VERSION="10 (buster)"
VERSION_CODENAME=buster
ID=debian
HOME_URL="https://www.debian.org/"
SUPPORT_URL="https://www.debian.org/support"
BUG_REPORT_URL="https://bugs.debian.org/"
rich@thundera ~ % docker run --entrypoint cat dotnetapp:alpine /etc/os-release
NAME="Alpine Linux"
ID=alpine
VERSION_ID=3.10.3
PRETTY_NAME="Alpine Linux v3.10"
HOME_URL="https://alpinelinux.org/"
BUG_REPORT_URL="https://bugs.alpinelinux.org/"
```

## Build an image for Windows Nano Server

You can also target Nano Server directly in the same as you can with Linux. The only difference is that Nano Server has a stronger coupling between the host Windows version and the guest container version. All supported versions will be demonstrated in the example below. You are enouraged only to use the version that applies to your environment.

This example will work on any supported version of Windows (Windows 10 RS2+).

```console
docker build --pull -t dotnetapp -f Dockerfile.nanoserver-x64 .
docker run --rm dotnetapp Hello .NET Core from Nano Server
docker images dotnetapp | findstr nanoserver
```

The `Dockerfile.nanoserver-x64` Dockerfile targets a multi-arch tag, which will result in a Nano Server version that matches the Windows host version. This works well if that's what you want, but can be a problem if your development environment and target environment don't match. 

You can update the following line in the Dockerfile with the following version-specific tags to explicitly target a given Nano Server version.

```console
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-nanoserver
```

Nano Server version-specific tags:

* 3.1-nanoserver-1909
* 3.1-nanoserver-1903
* 3.1-nanoserver-1809

## Build an image optimized for size

You may want to build a .NET Core image that is optimized for size, by publishing an application that includes the .NET Core runtime (self-contained) and then is trimmed with the assembly-linker. These are the tools offered in the .NET Core SDK for producing the smallest images. This approach may be prefered if you are running a single .NET Core app on a machine. Otherwise, building images on the .NET Core runtime layer is recommended and likely preferred. 

The following instructions are for x64 only, but can be straightforwardly updated for use with ARM architectures.

There are a set of '-trim' Dockerfiles included with this sample that are opted into the following .NET Core SDK publish operations:

* **Self-contained deployment** -- Publish the runtime with the application.
* **Assembly linking** -- Trim assemblies, including in the .NET Core framework, to make the application smaller.
* **Ready to Run compilation** -- Compile assemblies to Ready to Run format to make startup faster.

The first two operations reduce size, which can decrease image pull times. The last operation improves startup performance, but increases size. You can experiment with these options if you want to see which combination of settings works best for you.

The following instructions demonstrate how to build the `slim` Dockerfiles:

```console
docker build --pull -t dotnetapp:debian-trim -f Dockerfile.debian-x64-trim .
docker build --pull -t dotnetapp:ubuntu-trim -f Dockerfile.ubuntu-x64-trim .
docker build --pull -t dotnetapp:alpine-trim -f Dockerfile.alpine-x64-trim .
```

You can then compare sizes between using a shared layer and optimizing for size using the `docker images` command again. The command below uses `grep`. `findstr` on Windows works equally well.

```console
rich@thundera dotnetapp % docker images dotnetapp | grep alpine
dotnetapp           alpine-trim      9d23e22d7229        About a minute ago   46.3MB
dotnetapp           alpine              8933fb9821e8        About an hour ago    87MB
rich@thundera dotnetapp % docker images dotnetapp | grep ubuntu
dotnetapp           ubuntu-trim      fe292390c5fb        52 minutes ago      140MB
dotnetapp           ubuntu              373df08a06ec        59 minutes ago      187MB
rich@thundera dotnetapp % docker images dotnetapp | grep debian
dotnetapp           debian-trim      41e834fe89e2        52 minutes ago      147MB
dotnetapp           debian              229dd121a96b        59 minutes ago      190MB
```

Note: These sizes are all uncompressed, on-disk sizes. When you pull an image from a registry, it is compressed, such that the size will be significantly smaller.

The same operations are supported for Nano Server, as follows:

```console
docker build --pull -t dotnetapp:nanoserver-trim -f Dockerfile.nanoserver-x64-trim .
docker images dotnetapp | findstr nanoserver
```

## Build an image for ARM32 and ARM64

By default, distro-specific .NET Core tags target x64, such as `3.1-alpine` or `3.1-nanoserver`. You need to use an architecture-specific tag if you want to target ARM. Note that Alpine is only supported on ARM64 and x64, not ARM32.

Note: Docker refers to ARM32 as `armhf` and ARM64 as `aarch64` in documentation and other places.

The following example demonstrates targeting architectures explictly on Liux, for ARM32 and ARM64.

```console
docker build --pull -t dotnetapp:debian-arm32 -f Dockerfile.debian-arm32 .
docker build --pull -t dotnetapp:ubuntu-arm32 -f Dockerfile.ubuntu-arm32 .
docker build --pull -t dotnetapp:debian-arm64 -f Dockerfile.debian-arm64 .
docker build --pull -t dotnetapp:ubuntu-arm64 -f Dockerfile.ubuntu-arm64 .
docker build --pull -t dotnetapp:alpine-arm64 -f Dockerfile.alpine-arm64 .
```

You can use `docker images` to see a listing of the images you've built, as you can see in the following example.

```console
rich@thundera ~ % docker images dotnetapp | grep arm
dotnetapp           ubuntu-arm64        3be8a7da7148        14 seconds ago      193MB
dotnetapp           alpine-arm64        09a1d1bfd477        20 hours ago        99.5MB
dotnetapp           debian-arm64        fa5efe51d9ef        20 hours ago        197MB
dotnetapp           ubuntu-arm32        ea8ac73f8a72        20 hours ago        165MB
dotnetapp           debian-arm32        4f6ade8318d4        20 hours ago        165MB
```

You can build ARM32 and ARM64 images on x64 machines, but you will not be able to run them. Docker relies on QEMU for this scenario, which isn't supported by .NET Core. You must test and run .NET Core imges on actual hardware for the given processor type.

You can do the same thing on Windows, as follows:

```console
docker build --pull -t dotnetapp:nanoserver-arm32 -f Dockerfile.nanoserver-arm32 .
docker images dotnetapp | findstr arm
```

## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
