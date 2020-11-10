# ASP.NET Core Docker Sample

This sample demonstrates how to build container images for ASP.NET Core web apps. You can use these samples for Linux and Windows containers, for x64, ARM32 and ARM64 architectures.

The sample builds an application in a [.NET SDK container](https://hub.docker.com/_/microsoft-dotnet-sdk/) and then copies the build result into a new image (the one you are building) based on the smaller [.NET Docker Runtime image](https://hub.docker.com/_/microsoft-dotnet-runtime/). You can test the built image locally or deploy it to a [container registry](../push-image-to-acr.md).

The instructions assume that you have cloned this repo, have [Docker](https://www.docker.com/products/docker) installed, and have a command prompt open within the `samples/aspnetapp` directory within the repo.

## Try a pre-built version of the sample

If want to skip ahead, you can try a pre-built version with the following command and access it in your web browser at `http://localhost:8000`.

```console
docker run --rm -it -p 8000:80 mcr.microsoft.com/dotnet/samples:aspnetapp
```

## Build an ASP.NET Core image

You can build and run a .NET-based container image using the following instructions:

```console
docker build --pull -t aspnetapp .
docker run --rm -it -p 8000:80 aspnetapp
```

You should see the following console output as the application starts:

```console
> docker run --rm -it -p 8000:80 aspnetapp
Hosting environment: Production
Content root path: /app
Now listening on: http://[::]:80
Application started. Press Ctrl+C to shut down.
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

> Note: The `-p` argument maps port 8000 on your local machine to port 80 in the container (the form of the port mapping is `host:container`). See the [Docker run reference](https://docs.docker.com/engine/reference/commandline/run/) for more information on command-line parameters. In some cases, you might see an error because the host port you select is already in use. Choose a different port in that case.

You can also view the ASP.NET Core site running in the container on another machine. This is particularly useful if you are wanting to view an application running on an ARM device like a Raspberry Pi on your network. In that scenario, you might view the site at a local IP address such as `http://192.168.1.18:8000`.

In production, you will typically start your container with `docker run -d`. This argument starts the container as a service, without any console interaction. You then interact with it through other Docker commands or APIs exposed by the containerized application.

We recommend that you do not use `--rm` in production. It cleans up container resources, preventing you from collecting logs that may have been captured in a container that has either stopped or crashed.

> Note: See [Establishing docker environment](../establishing-docker-environment.md) for more information on correctly configuring Dockerfiles and `docker build` commands.

## Build an image for Windows Nano Server

The following example demonstrates targeting Windows Nano Server (x64) explicitly (you must have [Windows containers enabled](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers)):

```console
docker build --pull -t aspnetapp:nanoserver -f Dockerfile.nanoserver-x64 .
docker run --rm -it -p 8000:80 aspnetapp:nanoserver
```

You can view in the app in your browser in the same way as demonstrated earlier.

You can use `docker images` to see the images you've built:

```console
> docker images aspnetapp
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
aspnetapp           latest              b2f0ecb7bdf9        About an hour ago   353MB
aspnetapp           nanoserver          d4b7586827f2        About an hour ago   353MB
```

## Build an image for Alpine, Debian or Ubuntu

.NET multi-platform tags result in Debian-based images, for Linux. For example, you will pull a Debian-based image if you use a simple version-based tag, such as `5.0`, as opposed to a distro-specific tag like `5.0-alpine`.

This sample includes Dockerfile examples that explicitly target Alpine, Debian and Ubuntu. The [.NET Docker Sample](../dotnetapp/README.md) demonstrates targeting a larger set of distros.

The following example demonstrates targeting distros explicitly and also shows the size differences between the distros. Tags are added to the image name to differentiate the images.

On Linux:

```console
docker build --pull -t aspnetapp:alpine -f Dockerfile.alpine-x64 .
docker run --rm -it -p 8000:80 aspnetapp:alpine
```

You can view in the app in your browser in the same way as demonstrated earlier.

You can also build for Debian and Ubuntu:

```console
docker build --pull -t aspnetapp:debian -f Dockerfile.debian-x64 .
docker build --pull -t aspnetapp:ubuntu -f Dockerfile.ubuntu-x64 .
```

You can use `docker images` to see the images you've built and to compare file sizes:

```console
% docker images aspnetapp
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
aspnetapp           ubuntu              0f5bc72e4caf        14 seconds ago      209MB
aspnetapp           debian              f70387d4d802        35 seconds ago      212MB
aspnetapp           alpine              6da2c287c42c        10 hours ago        109MB
aspnetapp           latest              8c5d1952e3b7        10 hours ago        212MB
```

You can run these images in the same way as is done above, with Alpine.

## Build an image for ARM32 and ARM64

By default, distro-specific .NET tags target x64, such as `5.0-alpine` or `5.0-focal`. You need to use an architecture-specific tag if you want to target ARM. Note that .NET is only supported on Alpine on ARM64 and x64, and not ARM32.

Note: Docker documentation sometimes refers to ARM32 as `armhf` and ARM64 as `aarch64`.

The following example demonstrates targeting architectures explicitly on Linux, for ARM32 and ARM64.

```console
docker build --pull -t aspnetapp:alpine-arm64 -f Dockerfile.alpine-arm64 .
docker build --pull -t aspnetapp:debian-arm32 -f Dockerfile.debian-arm32 .
docker build --pull -t aspnetapp:debian-arm64 -f Dockerfile.debian-arm64 .
```

You can use `docker images` to see a listing of the images you've built, as you can see in the following example.

```console
% docker images aspnetapp | grep arm
aspnetapp           debian-arm64        8bf21dd704cf        14 seconds ago       223MB
aspnetapp           debian-arm32        29a8bfa90a03        About a minute ago   190MB
aspnetapp           alpine-arm64        8ec6bf841319        2 minutes ago        125MB
```

You can build ARM32 and ARM64 images on ARM or x64 machines. It may be preferred to build on x64 to take advantage of higher performance, and the ability to take advantage of CI/CD services.

You won't be able to run .NET ARM64 images on x64 machines. Docker relies on QEMU to run ARM64 images on X64, but [QEMU isn't supported by .NET](https://github.com/dotnet/coreclr/issues/12972). You must test and run .NET images on actual hardware for the given processor type.

## Optimizing for startup performance

You can improve startup performance by using [Ready to Run (R2R) compilation](https://github.com/dotnet/runtime/blob/master/docs/design/coreclr/botr/readytorun-overview.md) for your application. You can do this by setting the `PublishReadyToRun` property, which will take effect when you publish an application. This is what the `-slim` samples do (they are explained shortly).

You can add the `PublishReadyToRun` property in two ways:

- Set it in your project file, as: `<PublishReadyToRun>true</PublishReadyToRun>`
- Set it on the command line, as:  `/p:PublishReadyToRun=true`

The default `Dockerfile` that comes with the sample doesn't use R2R compilation because the application is too small to warrant it. The bulk of the IL code that is executed in this sample application is within the .NET libraries, which are already R2R-compiled.

## Optimizing for size

You may want to build an ASP.NET Core image that is optimized for size by publishing an application that includes the ASP.NET Core runtime (self-contained) and then is trimmed with the assembly-linker. These are the tools offered in the .NET SDK for producing the smallest images. This approach may be preferred if you are running a single .NET app on a machine. Otherwise, building images on the ASP.NET Core runtime layer is recommended and likely preferred.

The following instructions are for x64 only, but can be straightforwardly updated for use with ARM architectures.

There are a set of '-slim' Dockerfiles included with this sample that are opted into the following [.NET SDK publish operations](https://docs.microsoft.com/dotnet/core/deploying/):

* **Self-contained deployment** -- Publish the runtime with the application.
* **Assembly linking** -- Trim assemblies, including in the .NET framework, to make the application smaller.
* **Ready to Run (R2R) compilation** -- Compile assemblies to R2R format to make startup faster. R2R-compiled assemblies are larger. The benefit of R2R compilation for your application may be outweighed by the size increase, so please do test your application with and without R2R.

You are encouraged to experiment with these options if you want to see which combination of settings works best for you.

The following instructions demonstrate how to build the `slim` Dockerfiles:

```console
docker build --pull -t aspnetapp:debian-slim -f Dockerfile.debian-x64-slim .
docker build --pull -t aspnetapp:alpine-slim -f Dockerfile.alpine-x64-slim .
```

You can then compare sizes between using a shared layer and optimizing for size using the `docker images` command again. The command below uses `grep`. `findstr` on Windows works equally well.

```console
% docker images aspnetapp | grep alpine
aspnetapp           alpine-slim         34135d057c0f        2 hours ago         97.7MB
aspnetapp           alpine              8567c3d23608        2 hours ago         109MB
```

Same thing with Debian:

```console
$ docker images aspnetapp | grep debian
aspnetapp           debian              edfd63050f14        11 seconds ago      212MB
aspnetapp           debian-slim         13c30001b4fb        10 minutes ago      202MB
```

> Note: These image sizes are all uncompressed, on-disk sizes. When you pull an image from a registry, it is compressed, such that the size will be significantly smaller. See [Retrieving Docker Image Sizes](https://gist.github.com/MichaelSimons/fb588539dcefd9b5fdf45ba04c302db6) for more information.

You can do the same thing with Windows Nano Server, as follows:

```console
docker build --pull -t aspnetapp:nanoserver-slim -f Dockerfile.nanoserver-x64-slim .
```

And `docker images` will show you the Nano Server image you've just built.

```console
>docker images aspnetapp
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
aspnetapp           nanoserver-slim     59d9aa2e5826        11 seconds ago      341MB
aspnetapp           nanoserver          1e16a73b42b3        34 seconds ago      353MB
```

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
