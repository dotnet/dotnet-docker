# Container best practices for .NET

This document describes best practices for building and using containers with .NET.

## Setting the Docker context

The [build context](https://docs.docker.com/build/building/context/) is a key aspect to using `docker build`. In the most basic case, all assets are in a single directory and the context can match. In other cases, assets are spread out more. Getting the context right and the location of the Dockerfile in relation is perhaps the most important part of building container images.

### Basic pattern

The [dotnetapp](dotnetapp/README.md) samples has all assets in one directory. It uses the following pattern.

```bash
docker build --pull -t dotnetapp .
```

The context is the current directory.

Multiple Dockerfile variations are provided. They can be specified with the `-f` argument, as follows.

```bash
docker build --pull -t dotnetapp -f Dockerfile.alpine .
```

### Multiple projects

The [complexapp](complexapp/README.md) samples is more complex, being composed of multiple projects

```bash
rich@mazama:~/dotnet-docker/samples/complexapp$ ls -l
total 44
drwxr-xr-x 2 rich rich  4096 Feb 21 15:11 complexapp
-rw-r--r-- 1 rich rich  5046 Feb 21 15:11 complexapp.sln
-rw-r--r-- 1 rich rich  1002 Feb 21 15:11 Dockerfile
drwxr-xr-x 2 rich rich  4096 Feb 21 15:11 libbar
drwxr-xr-x 2 rich rich  4096 Feb 21 15:11 libfoo
-rw-r--r-- 1 rich rich 12511 Feb 21 15:11 README.md
drwxr-xr-x 2 rich rich  4096 Feb 21 15:11 tests
```

In this case, the [`Dockerfile`](complexapp/Dockerfile) is placed at the highest scope for the app and assumes the directory structure within. The typical pattern is to put the `Dockerfile` at the same place where you'd put a solution file. That's the case with this example.

That's apparent from the first part of the `Dockerfile`.

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY complexapp/*.csproj complexapp/
COPY libfoo/*.csproj libfoo/
COPY libbar/*.csproj libbar/
RUN dotnet restore complexapp/complexapp.csproj
```

Given this structure, the Dockerfile can be built with the same pattern.

```bash
docker build --pull -t complexapp .
```

### Whole repo scope

Another pattern is matching the build context to the whole repo and then using `-f` to locate the Dockerfile. That pattern often looks like the following.

```bash
docker build --pull -t app -f src/app/Dockerfile .
```

This pattern makes sense if the whole repo is required for the build.

## Annotated Dockerfile -- Basic scenarios

The following [Dockerfile](dotnetapp/Dockerfile) can be thought of as the "basic model" for building .NET apps. It satisfies a lot of needs and the other more advanced Dockerfiles offer relatively small variations on the same model. The annotations describe the beav

```dockerfile
# Establishes the `build` stage using an SDK image.
# This multi-platform tag will provide a compatible image
# on Linux or Windows, for Arm32, Arm64, and x64 architectures.
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# Copies msbuild assets into the image.
# Other assets like `nuget.config` or `Directory.build.props` (if they exist) would need to be copied.
# Copying a mininmal set of assets and the restoring packages is good for caching.
# If a .cs file is changed, Docker will realize that and jump past the restore line.
COPY *.csproj .
# Restores packages.
# It is best to restore and publish in terms of a specific runtime.
# This pattern only matters if the app depends on packages with `runtime` folders.
# In many scenarios, it is best to let the SDK pick the currect `runtime` value, rather than using `-r` directly.
RUN dotnet restore --use-current-runtime

# Copy the rest of the app assets, like `.cs` files
# In cases where `docker build` is regularly re-run, 
# the build will often start from this `COPY` directive,
# since all other work is safely cached.
COPY . .
# Publishes a framework-dependent app.
# Builds the code in the `/source` folder and places the final output in the `/app` folder.
# There is no need to first run `dotnet build`.
# `--no-restore` offers a small performance win since `dotnet publish` doesn't need 
# to validate that nothing has changed.
RUN dotnet publish --use-current-runtime --self-contained false --no-restore -o /app

# Establishes the final stage.
# Also uses a multi-platform tag.
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
# Copies the code in the `/app` folder from the previous stage into the `/app` folder.
COPY --from=build /app .
# Sets the entrypoint for the app.
# This launch pattern was chosen since it works the same in both Linux and Windows containers.
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

This Dockerfile above works well when the machines you build and host on are similar (both x64, for example). If your build and production machine are different (for example, Arm64 and x64), then you need to adopt a different pattern.

## Annotated Dockerfile -- Advanced platform targeting

The following [Dockerfile](aspnetapp/Dockerfile.alpine) offers more control for targeting, with the `--platform` switch. It is very similar to the prior sample, with a couple variations to change the behavior. In particular, we want the .NET SDK to always run natively (best performance and avoids running in QEMU, which .NET doesn't support).

```dockerfile
# Establishes the `build` stage using an SDK image.
# This multi-platform tag will provide a compatible Alpine image
# for Arm32, Arm64, and x64 architectures.
# Docker exposes the ability to specify the platform to request for multi-platform images.
# By default, the build and target platforms match.
# If a `--platform` value is specified via the CLI, then it sets the platform to request.
# It can be overridden, as we are doing here with the `$BUILDPLATFORM` value (which Docker sets by default)
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
# Docker also sets a `$TARGETPLATFORM`, and `$TARGETOS` and `$TARGETARCH` for the component parts.
# To use `TARGETARCH`, it must first be set, with no value`
ARG TARGETARCH
WORKDIR /source

# copy csproj and restore as distinct layers
COPY aspnetapp/*.csproj .
# In this case, we do not want to use `--use-current-runtime` since that will match `$BUILDPLATFORM`.
# We only need to set the architecture, so we can set that via the `-a` argument using the `$TARGETARCH` value.
RUN dotnet restore -a $TARGETARCH

# copy everything else and build app
COPY aspnetapp/. .
# We are setting `-a` in the same way as above.
RUN dotnet publish -a $TARGETARCH --self-contained false --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
# This healthcheck directive queries the `healthz` endpoint exposed by ASP.NET Core middlware.
HEALTHCHECK CMD wget -qO- -t1 http://localhost:80/healthz || exit 1
WORKDIR /app
COPY --from=build /app .
# This launch pattern launches an executable (since this Dockerfile is Linux-specific).
ENTRYPOINT ["./aspnetapp"]
```

This pattern requires the .NET 7.0.300 SDK. The various combinations of `-a` and `$TARGETARCH` were previously not supported.

## Producing production-ready assets

The sample Dockerfiles do not set the configuration via the `dotnet` CLI, but rely on the `PublishRelease` setting in the [project files](dotnetapp/dotnetapp.csproj). This setting is new as of .NET 7.0.200. It is supported for both .NET 6 and .NET 7 projects.

In past .NET versions, the following pattern would be used to produce release assets:

```bash
dotnet publish -c Release
```

That is no longer needed. With .NET 7.0.200, the following setting can be used in a project file. With .NET 8, `PublishRelease=true` is the default, which means that the project file setting is not required.

```xml
<PropertyGroup>
  <PublishRelease>true</PublishRelease>
</PropertyGroup>
```



## Docker in production

There are multiple choices for production, like Docker, Docker Compose, and Kubernetes. The following guidance applies to running Docker.

Start your container with `docker run -d`. This argument starts the container as a service, without any console interaction. You then interact with it through other Docker commands or APIs exposed by the containerized application.

Do not use `--rm` in production. It cleans up container resources, preventing collecting logs that may have been captured in a container that has either stopped or crashed.
