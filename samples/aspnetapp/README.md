# ASP.NET Core Docker Sample

This sample demonstrates how to build container images for ASP.NET Core web apps. You can use this samples for Linux and Windows containers, for x64, ARM32 and ARM64 architectures.

The sample builds an application in a [.NET Core SDK container](https://hub.docker.com/_/microsoft-dotnet-core-sdk/) and then copies the build result into a new image (the one you are building) based on the smaller [.NET Core Docker Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/). You can test the built image locally or deploy it to a [container registry](../push-image-to-acr.md).

The instructions assume that you have cloned this repo, have [Docker](https://www.docker.com/products/docker) installed, and have a command prompt open within the `samples/aspnetapp` directory within the repo.

If want to skip to the final result, you can try a pre-built version with the following command and access in your web browser at `http://localhost:8000`.

```console
docker run --rm -it -p 8000:80 mcr.microsoft.com/dotnet/core/samples:aspnetapp
```

Note: This pattern works on Windows, macOS and Linux. Earlier Windows versions need to use a different set of commands that are described later in this document.

## Build and run the sample with Docker

You can build and run a .NET Core-based container image using the following instructions:

```console
docker build --pull -t aspnetapp .
docker run --rm -it -p 8000:80 aspnetapp
```

You should see the following console output as the application starts.

```console
> docker run --rm -it -p 8000:80 aspnetapp
Hosting environment: Production
Content root path: /app
Now listening on: http://[::]:80
Application started. Press Ctrl+C to shut down.
```

After the application starts, navigate to `http://localhost:8000` in your web browser. 

Note: Earlier Windows versions need to use a different set of commands that are described at the end of the document.

Note: The `-p` argument maps port 8000 on your local machine to port 80 in the container (the form of the port mapping is `host:container`). See the [Docker run reference](https://docs.docker.com/engine/reference/commandline/run/) for more information on commandline parameters. In some cases, you might see an error because the host port you select is already in use. Choose a different port in that case.

## Build an image for Windows Nano Server

The following example demonstrates targeting Windows Nano Server (x64) explicity:

```console
docker build --pull -t aspnetapp:nanoserver -f Dockerfile.nanoserver-x64 .
docker run --rm -it -p 8000:80 aspnetapp:nanoserver
```

You can view in the app in your browser in the same way as demonstrated earlier.



## Build an image for Alpine, Debian or Ubuntu

By default, .NET Core uses Debian base images for Linux containers. You will get a Debian-based image if you use a tag with only a version number, such as `3.1`, as opposed to a distro-specific tag like `3.1-alpine`.

This sample includes Dockerfile examples that explicitly target Alpine, Debian and Ubuntu. Docker makes it easy to use alternate Dockfiles by using the `-f` argument. The [.NET Core Docker Sample](../dotnetapp/README.md) demonstrates targeting a larger set of distros.

The following example demonstrates targeting distros explictly and also shows the size differences between the distros. Tags are added to the image name to differentiate the images.

On Linux:

```console
docker build --pull -t aspnetapp:alpine -f Dockerfile.alpine-x64 .
docker run --rm -it -p 8000:80 aspnetapp:alpine
```

You can view in the app in your browser in the same way as demonstrated earlier.

You can use `docker images` to see the images you've built and to compare file sizes:

```console
% docker images aspnetapp
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
aspnetapp           alpine              8567c3d23608        34 seconds ago      109MB
aspnetapp           latest              eaf9b1b09d69        9 minutes ago       212MB
```

## Build an image for ARM32 and ARM64

By default, distro-specific .NET Core tags target x64, such as `3.1-alpine` or `3.1-nanoserver`. You need to use an architecture-specific tag if you want to target ARM. Note that .NET Core in only supported on Alpine on ARM64 and x64, and not ARM32.

Note: Docker documentation refers to ARM32 as `armhf` and ARM64 as `aarch64`.

The following example demonstrates targeting architectures explictly on Linux, for ARM32 and ARM64.

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

You can build ARM32 and ARM64 images on x64 machines, but you will not be able to run them. Docker relies on QEMU for this scenario, which isn't supported by .NET Core. You must test and run .NET Core imges on actual hardware for the given processor type.

You can do the same thing on Windows, as follows:

```console
docker build --pull -t aspnetapp:nanoserver-arm32 -f Dockerfile.nanoserver-arm32 .
docker images aspnetapp | findstr arm
```

## Build an image optimized for startup performance

You can improve startup performance by using [Ready to Run compilation](https://github.com/dotnet/runtime/blob/master/docs/design/coreclr/botr/readytorun-overview.md) for your application. You can do this by setting the `PublishReadyToRun` property, which will take affect when you publish an application. This is what the `-trim` samples do (they are explained shortly). 

You can add this property in two ways:

- Set the property to your profile file, as: `<PublishReadyToRun>true</PublishReadyToRun>'
- Set the property on the command line, as:  `/p:PublishReadToRun=true`

The default `Dockerfile` that come with the sample doesn't use Ready to Run compilation because the application is too small to warrant it. The majority of the startup benefit available comes from .NET Core libraries -- which are Ready to Run compiled -- since they make up the bulk of the IL code that is executed in an application.

## Build an image optimized for size

You may want to build an ASP.NET Core image that is optimized for size, by publishing an application that includes the ASP.NET Core runtime (self-contained) and then is trimmed with the assembly-linker. These are the tools offered in the .NET Core SDK for producing the smallest images. This approach may be prefered if you are running a single .NET Core app on a machine. Otherwise, building images on the ASP.NET Core runtime layer is recommended and likely preferred. 

The following instructions are for x64 only, but can be straightforwardly updated for use with ARM architectures.

There are a set of '-trim' Dockerfiles included with this sample that are opted into the following .NET Core SDK publish operations:

* **Self-contained deployment** -- Publish the runtime with the application.
* **Assembly linking** -- Trim assemblies, including in the .NET Core framework, to make the application smaller.
* **Ready to Run compilation** -- Compile assemblies to Ready to Run format to make startup faster.

The first two operations reduce size, which can decrease image pull times. The last operation improves startup performance, but increases size. You can experiment with these options if you want to see which combination of settings works best for you.

The following instructions demonstrate how to build the `slim` Dockerfiles:

```console
docker build --pull -t aspnetapp:debian-trim -f Dockerfile.debian-x64-trim .
docker build --pull -t aspnetapp:alpine-trim -f Dockerfile.alpine-x64-trim .
```

You can then compare sizes between using a shared layer and optimizing for size using the `docker images` command again. The command below uses `grep`. `findstr` on Windows works equally well.

```console
% docker images aspnetapp | grep alpine
aspnetapp           alpine-trim         34135d057c0f        2 hours ago         97.7MB
aspnetapp           alpine              8567c3d23608        2 hours ago         109MB
```

Same thing with Debian:

```console
% docker images aspnetapp | grep debian
aspnetapp           debian-trim         43c186e2fe71        About an hour ago   206MB

% docker images aspnetapp | grep latest
aspnetapp           latest              eaf9b1b09d69        41 minutes ago      212MB
```

Note: These sizes are all uncompressed, on-disk sizes. When you pull an image from a registry, it is compressed, such that the size will be significantly smaller.

The same operations are supported for Nano Server, as follows:

```console
docker build --pull -t aspnetapp:nanoserver-trim -f Dockerfile.nanoserver-x64-trim .
docker images aspnetapp | findstr nanoserver
```

## View ASP.NET Core apps via IP address

On older versions of Windows, the only way to navigate to a container-hosted website is via an IP address, not `localhost`. The following instructions describe a pattern for working around this issue.

Run the application using the `--name` argument:

```console
docker build --pull -t aspnetapp .
docker run --rm -it -p 8000:80 --name aspnetcore_sample aspnetapp
```

In another command prompt, you need to run the following command:

```console
docker exec aspnetcore_sample ipconfig
```

You should see something like:

```console
C:\git\dotnet-docker\samples\aspnetapp>docker exec aspnetcore_sample ipconfig

Windows IP Configuration


Ethernet adapter Ethernet:

   Connection-specific DNS Suffix  . : contoso.com
   Link-local IPv6 Address . . . . . : fe80::1967:6598:124:cfa3%4
   IPv4 Address. . . . . . . . . . . : 172.29.245.43
   Subnet Mask . . . . . . . . . . . : 255.255.240.0
   Default Gateway . . . . . . . . . : 172.29.240.1
```

Navigate to the IP address you see, which would be `172.29.245.43` in the example above. 

Note: [`docker exec`](https://docs.docker.com/engine/reference/commandline/exec/) supports identifying containers with name or hash. The container name is used in the preceding instructions. `docker exec` runs a new command (as opposed to the [entrypoint](https://docs.docker.com/engine/reference/builder/#entrypoint)) in a running container.

Alternative, `docker inspect` can  be used for the same purpose, as demonstrated in the following example.

```console
C:\git\dotnet-docker\samples\aspnetapp>docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" aspnetcore_sample
172.29.245.43
```

## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
