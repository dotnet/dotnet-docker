# .NET Docker Sample

This sample demonstrates how to build container images for .NET console apps. You can use this samples for Linux and Windows containers, for x64, ARM32 and ARM64 architectures.

The sample builds an application in a [.NET SDK container](https://hub.docker.com/_/microsoft-dotnet-sdk/) and then copies the build result into a new image (the one you are building) based on the smaller [.NET Docker Runtime image](https://hub.docker.com/_/microsoft-dotnet-runtime/). You can test the built image locally or deploy it to a [container registry](../push-image-to-acr.md).

The instructions assume that you have cloned this repo, have [Docker](https://www.docker.com/products/docker) installed, and have a command prompt open within the `samples/dotnetapp` directory within the repo.

## Try a pre-built version of the sample

If want to skip ahead, you can try a pre-built version with the following command:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

## Build a .NET image

You can build and run a .NET-based container image using the following instructions:

```console
docker build --pull -t dotnetapp .
docker run --rm dotnetapp Hello .NET from Docker
```

You can use the `docker images` command to see a listing of your image, as you can see in the following example.

```console
% docker images dotnetapp
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
dotnetapp           latest              baee380605f4        14 seconds ago      189MB
```

The logic to build the image is described in the [Dockerfile](Dockerfile), which follows.

```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

The `sdk:5.0` and `runtime:5.0` tags are both multi-arch tags that will result in an image that is compatible for the given chip and OS. These simple tags (only contain a version number) are great to get started with Docker because they adapt to your environment. We recommend using an OS-specific tag for the runtime for production applications to ensure that you always get the OS you expect. This level of specification isn't needed for the SDK in most cases.

This Dockerfile copies and restores the project file as the first step so that the results of those commands can be cached for subsequent builds since project file edits are less common than source code edits. Editing a `.cs` file, for example, does not invalidate the layer created by copying and restoring project file, which makes subsequent docker builds much faster.

> Note: See [Establishing docker environment](../establishing-docker-environment.md) for more information on correctly configuring Dockerfiles and `docker build` commands.

## Build an image for Alpine, Debian or Ubuntu

.NET multi-platform tags result in Debian-based images, for Linux. For example, you will pull a Debian-based image if you use a simple version-based tag, such as `5.0`, as opposed to a distro-specific tag like `5.0-alpine`.

This sample includes Dockerfile examples that explicitly target Alpine, Debian and Ubuntu. Docker makes it easy to use alternate Dockerfiles by using the `-f` argument.

The following example demonstrates targeting distros explicitly and also shows the size differences between the distros. Tags are added to the image name to differentiate the images.

```console
docker build --pull -t dotnetapp:debian -f Dockerfile.debian-x64 .
docker build --pull -t dotnetapp:ubuntu -f Dockerfile.ubuntu-x64 .
docker build --pull -t dotnetapp:alpine -f Dockerfile.alpine-x64 .
```

You can use `docker images` to see the images you've built and to compare file sizes:

```console
% docker images dotnetapp
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
dotnetapp           alpine              8933fb9821e8        8 seconds ago       87MB
dotnetapp           ubuntu              373df08a06ec        25 seconds ago      187MB
dotnetapp           debian              229dd121a96b        39 seconds ago      190MB
dotnetapp           latest              303eabf97376        56 seconds ago      190MB
```

You can run any of the images you've just built with the following commands:

```console
docker run --rm dotnetapp
docker run --rm dotnetapp:debian
docker run --rm dotnetapp:ubuntu
docker run --rm dotnetapp:alpine
```

If you want to double check the distro of an application, you can do that by configuring a different entrypoint when you run the image, as you see in the following example.

```console
% docker run --rm --entrypoint cat dotnetapp /etc/os-release
PRETTY_NAME="Debian GNU/Linux 10 (buster)"
NAME="Debian GNU/Linux"
VERSION_ID="10"
VERSION="10 (buster)"
VERSION_CODENAME=buster
ID=debian
HOME_URL="https://www.debian.org/"
SUPPORT_URL="https://www.debian.org/support"
BUG_REPORT_URL="https://bugs.debian.org/"

% docker run --rm --entrypoint cat dotnetapp:alpine /etc/os-release
NAME="Alpine Linux"
ID=alpine
VERSION_ID=3.10.3
PRETTY_NAME="Alpine Linux v3.10"
HOME_URL="https://alpinelinux.org/"
BUG_REPORT_URL="https://bugs.alpinelinux.org/"
```

## Build an image for Windows Nano Server

You can target Nano Server directly in the same as you can with Linux. See [Windows container version compatibility](https://docs.microsoft.com/virtualization/windowscontainers/deploy-containers/version-compatibility) to learn about Windows version container requirements.

All supported versions will be demonstrated in the example below. You are encouraged only to use the version that applies to your environment.

This example will work on any supported version of Windows (Windows 10 RS2+).

```console
docker build --pull -t dotnetapp:nanoserver -f Dockerfile.nanoserver-x64 .
docker run --rm dotnetapp:nanoserver Hello .NET from Nano Server
docker images dotnetapp
```

The `Dockerfile.nanoserver-x64` Dockerfile targets a version-specific tag, which will result in a Nano Server version that targets a specific Windows version (and will only work on Windows hosts of the same version or higher). You can update the following the tag to a different version, as needed.

```console
FROM mcr.microsoft.com/dotnet/runtime:5.0-nanoserver-20H2
```

## Build an image for ARM32 and ARM64

By default, distro-specific .NET tags target x64, such as `5.0-alpine` or `5.0-focal`. You need to use an architecture-specific tag if you want to target ARM. Note that for Alpine, .NET is only supported on ARM64 and x64, and not ARM32.

> Note: Docker documentation sometimes refers to ARM32 as `armhf` and ARM64 as `aarch64`.

The following example demonstrates targeting architectures explictly on Linux, for ARM32 and ARM64.

```console
docker build --pull -t dotnetapp:debian-arm32 -f Dockerfile.debian-arm32 .
docker build --pull -t dotnetapp:ubuntu-arm32 -f Dockerfile.ubuntu-arm32 .
docker build --pull -t dotnetapp:alpine-arm32 -f Dockerfile.alpine-arm32 .
docker build --pull -t dotnetapp:debian-arm64 -f Dockerfile.debian-arm64 .
docker build --pull -t dotnetapp:ubuntu-arm64 -f Dockerfile.ubuntu-arm64 .
docker build --pull -t dotnetapp:alpine-arm64 -f Dockerfile.alpine-arm64 .
```

You can use `docker images` to see a listing of the images you've built, as you can see in the following example.

```console
% docker images dotnetapp | grep arm
dotnetapp           ubuntu-arm64        3be8a7da7148        14 seconds ago      193MB
dotnetapp           alpine-arm64        09a1d1bfd477        20 hours ago        99.5MB
dotnetapp           debian-arm64        fa5efe51d9ef        20 hours ago        197MB
dotnetapp           ubuntu-arm32        ea8ac73f8a72        20 hours ago        165MB
dotnetapp           alpine-arm32        f85033da1b6f        20 hours ago        74.2MB
dotnetapp           debian-arm32        4f6ade8318d4        20 hours ago        165MB
```

You can build ARM32 and ARM64 images on x64 machines, but you will not be able to run them. Docker relies on QEMU for this scenario, which isn't supported by .NET. You must test and run .NET imges on actual hardware for the given processor type. A common pattern for this situation is building on x64, [pushing images to a registry](push-image-to-acr.md), and then pulling the image from an ARM device.

## Build an image optimized for startup performance

You can improve startup performance by using [Ready to Run (R2R) compilation](https://github.com/dotnet/runtime/blob/master/docs/design/coreclr/botr/readytorun-overview.md) for your application. You can do this by setting the `PublishReadyToRun` property, which will result in your application being compiled to native code when you publish it. This is what the `-slim` samples do (they are explained shortly).

You can add the `PublishReadyToRun` property in two ways:

- Set it in your project file, as: `<PublishReadyToRun>true</PublishReadyToRun>`
- Set it on the command line, as:  `/p:PublishReadyToRun=true`

The default `Dockerfile` that come with the sample doesn't use R2R compilation because the application is too small to warrant it. The bulk of the IL code that is executed in this sample application is within the .NET libraries, which are already R2R compiled.

## Build an image optimized for size

You may want to build a .NET image that is optimized for size. You can do this by publishing an application as [self-contained](https://docs.microsoft.com/dotnet/core/deploying/) (includes the .NET runtime) and then trimmed with the assembly-linker. This approach may be prefered if you are running a single .NET app on a machine. Otherwise, building images on the .NET runtime layer is recommended for better overall performance characteristics (due to layer sharing).

The following instructions are for x64 only, but can be straightforwardly adapted for use with ARM architectures.

There are a set of `-slim` Dockerfiles included with this sample that are opted into the following .NET SDK publish operations:

* **Self-contained deployment** -- Publish .NET runtime(s) with the application.
* **Assembly linking** -- Trim assemblies, including in the .NET framework, to make the application smaller.
* **Ready to Run (R2R) compilation** -- Compile assemblies to R2R format to make startup faster. R2R Compiled assemblies are larger. The benefit of R2R compilation for your application may be outweighed by the size increase, so please do test your application with and without R2R.

You are encouraged to experiment with these options if you want to see which combination of settings works best for you.

The following instructions demonstrate how to build the `trim` Dockerfiles:

```console
docker build --pull -t dotnetapp:debian-slim -f Dockerfile.debian-x64-slim .
docker build --pull -t dotnetapp:ubuntu-slim -f Dockerfile.ubuntu-x64-slim .
docker build --pull -t dotnetapp:alpine-slim -f Dockerfile.alpine-x64-slim .
```

It is easy to run them all:

```console
docker run --rm dotnetapp:debian-slim
docker run --rm dotnetapp:ubuntu-slim
docker run --rm dotnetapp:alpine-slim
```

You can then compare sizes between using a shared layer and optimizing for size using the `docker images` command again. The command below uses `grep`. `findstr` on Windows works equally well.

```console
% docker images dotnetapp | grep alpine
dotnetapp           alpine-slim      9d23e22d7229        About a minute ago   46.3MB
dotnetapp           alpine           8933fb9821e8        About an hour ago    87MB
% docker images dotnetapp | grep ubuntu
dotnetapp           ubuntu-slim      fe292390c5fb        52 minutes ago      140MB
dotnetapp           ubuntu           373df08a06ec        59 minutes ago      187MB
% docker images dotnetapp | grep debian
dotnetapp           debian-slim      41e834fe89e2        52 minutes ago      147MB
dotnetapp           debian           229dd121a96b        59 minutes ago      190MB
```

> Note: These image sizes are all uncompressed, on-disk sizes. When you pull an image from a registry, it is compressed, such that the size will be significantly smaller. See [Retrieving Docker Image Sizes](https://gist.github.com/MichaelSimons/fb588539dcefd9b5fdf45ba04c302db6) for more information.

You can do the same thing with Windows Nano Server, as follows:

```console
docker build --pull -t dotnetapp:nanoserver-slim -f Dockerfile.nanoserver-x64-slim .
```

You can then use `docker images` command again.

```console
>docker images dotnetapp
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
dotnetapp           nanoserver-slim     7d3666c3b111        4 seconds ago       285MB
dotnetapp           nanoserver          7092d2e6b0a4        9 minutes ago       328MB
```

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/samples/README.md)
