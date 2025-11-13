# ASP.NET Core Docker Sample

This sample demonstrates how to build container images for ASP.NET Core web apps. See [.NET Docker Samples](../README.md) for more samples.

## Run the sample image

You can start by launching a sample from our [container registry](https://mcr.microsoft.com/) and accessing it in your web browser at `http://localhost:8000`.

```console
docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 mcr.microsoft.com/dotnet/samples:aspnetapp
```

You should see the following console output as the application starts:

```console
> docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 aspnetapp
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://[::]:8080
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

After the application starts, navigate to `http://localhost:8000` in your web browser.
You can also reach the app's API endpoint from the command line:

```bash
$ curl http://localhost:8000/Environment
{
  "runtimeVersion": ".NET 10.0.0-rc.1.25451.107",
  "osVersion": "Ubuntu 24.04.3 LTS",
  "osArchitecture": "X64",
  "user": "app",
  "processorCount": 32,
  "totalAvailableMemoryBytes": 67396280320,
  "memoryLimit": 0,
  "memoryUsage": 62160896,
  "hostName": "d021ffa2e15f"
}
```

You can see the app running via `docker ps`:

```bash
$ docker ps
CONTAINER ID   IMAGE                                        COMMAND         CREATED          STATUS                    PORTS                  NAMES
CONTAINER ID   IMAGE       COMMAND         CREATED         STATUS         PORTS                                         NAMES
d021ffa2e15f   aspnetapp   "./aspnetapp"   2 minutes ago   Up 2 minutes   0.0.0.0:8000->8080/tcp, [::]:8000->8080/tcp   reverent_aryabhata
```

## Change port

ASP.NET Core apps (in official .NET images) listen to [port 8080 by default](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7). The [`-p` argument](https://docs.docker.com/engine/reference/commandline/run/#publish) in these examples maps host port `8000` to container port `8080` (`host:container` mapping). The web server hosted by the container will not be accessible without this mapping.

ASP.NET Core can be [configured to listen on a different or additional port](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel/endpoints).
For example, setting either of the following evnironment variables will change the container port to `80`:

- `ASPNETCORE_HTTP_PORTS=80`
- `ASPNETCORE_URLS=http://+:80`

> [!NOTE]
> Ports 1 through 1023 are restricted to root users only, and will not work when running as the non-root root user provided in .NET images.

`ASPNETCORE_URLS` overrides `ASPNETCORE_HTTP_PORTS` if set.
The `ASPNETCORE_HTTP_PORTS` envrionment variable is used in the [ASP.NET Core](https://github.com/dotnet/dotnet-docker/blob/d033b1beda6bc9ac933dd88fcc572ec05c28f705/src/runtime-deps/10.0/noble/amd64/Dockerfile#L7)
images to set the default port.

## Enable HTTPS

To host the sample image with HTTPS, follow the instructions for [Running pre-built Container Images with HTTPS](../host-aspnetcore-https.md#hosting-aspnet-core-images-with-docker-over-https).
For a more in-depth guide to developing ASP.NET Core apps with HTTPS, check out [Developing ASP.NET Core Applications with Docker over HTTPS](../run-aspnetcore-https-development.md).

## Build the sample image

You can build the sample image using the following command (cloninig the repo isn't necessary):

```console
docker build --pull -t aspnetapp 'https://github.com/dotnet/dotnet-docker.git#:samples/aspnetapp'
```

If you have cloned the repo, you can build the image using your local copy:

```console
cd samples/aspnetapp
docker build --pull -t aspnetapp .
```

Add the argument `-f <Dockerfile>` to build the sample in a different configuration.
For example, build an [Ubuntu Chiseled](https://devblogs.microsoft.com/dotnet/dotnet-6-is-now-in-ubuntu-2204/#net-in-chiseled-ubuntu-containers) image using [Dockerfile.chiseled](Dockerfile.chiseled):

```console
docker build --pull -t dotnetapp -f Dockerfile.chiseled 'https://github.com/dotnet/dotnet-docker.git#:samples/dotnetapp'
```

You can run your local image the same way as described in [Run the sample image](#run-the-sample-image):

```console
docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 aspnetapp
```

### Multi-platform build

.NET sample Dockerfiles support multi-platform builds via cross-compilation.
First, check out the Docker [multi-platform build prerequisites](https://docs.docker.com/build/building/multi-platform/#prerequisites)

Once you have the prerequisites set up, you can build the ASP.NET app sample for a specific architecture:

```console
# From an amd64 machine:
docker buildx build --platform linux/arm64 -t aspnetapp .

# From an arm64 machine:
docker buildx build --platform linux/amd64 -t aspnetapp .
```

You can also build both platforms at once:

```console
docker buildx build --platform linux/amd64,linux/arm64 -t aspnetapp .
```

This works thanks to .NET's support for cross-compilation.
The build runs on your build machine's architecture and outputs IL for the target architecture.
The app is then copied to the final stage without running any commands on the target image - there's no emulation involved.

> [!NOTE]
> .NET does not support running under QEMU emulation. See [.NET and QEMU](../build-for-a-platform.md#net-and-qemu) for more information.

## Build image with the SDK

The easiest way to [build .NET images is using the SDK](https://learn.microsoft.com/dotnet/core/containers/overview).

```console
cd samples/aspnetapp
dotnet publish -p PublishProfile=DefaultContainer
```

You can control many aspects of the generated container through MSBuild properties.
For example, the following command uses a different base image and publishes the final image to DockerHub:

```console
dotnet publish \
    -p PublishProfile=DefaultContainer \
    -p ContainerBaseImage=mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled \
    -p ContainerRegistry=docker.io \
    -p ContainerRepository=youraccount/aspnetapp
```

These properties can also be [specified in your project file](https://learn.microsoft.com/visualstudio/msbuild/property-element-msbuild).
For a full list of supported properties, see the [.NET SDK publishing reference](https://learn.microsoft.com/dotnet/core/containers/publish-configuration).

## Supported Linux distros

The .NET Team publishes images for [multiple distros](../../documentation/supported-platforms.md).

Sample Dockerfiles are provided for:

- [Alpine](Dockerfile.alpine)
- [Alpine with Composite ready-to-run image](Dockerfile.alpine-composite)
- [Alpine with ICU installed](Dockerfile.alpine-icu)
- [Azure Linux](Dockerfile.azurelinux)
- [Azure Linux Distroless](Dockerfile.azurelinux-distroless)
- [Ubuntu](Dockerfile.ubuntu)
- [Ubuntu Chiseled](Dockerfile.chiseled)
- [Ubuntu Chiseled with Composite ready-to-run image](Dockerfile.chiseled-composite)

## Supported Windows versions

The .NET Team publishes images for [multiple Windows versions](../../documentation/supported-platforms.md). You must have [Windows containers enabled](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers) to use these images.

Samples are provided for

- [Nano Server](Dockerfile.nanoserver)
- [Windows Server Core](Dockerfile.windowsservercore)
- [Windows Server Core with IIS](Dockerfile.windowsservercore-iis) (Note: the IIS sample listens on port `80`, not `8080`)

You can pull a pre-built Windows sample image using the following tag:

- `mcr.microsoft.com/dotnet/samples:aspnetapp-nanoserver-ltsc2022`
