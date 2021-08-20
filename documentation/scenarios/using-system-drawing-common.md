# Using the System.Drawing.Common Package in a Docker Container

If your application makes use of the System.Drawing.Common NuGet package, you'll need to ensure some additional dependencies are installed in your Docker container for both Linux and Windows. See #618 (Linux) and #1098 (Windows) for more info on this issue.

## Linux

For Linux, it's just a matter of installing a few extra packages in your Dockerfile:

Debian/Ubuntu:

```Dockerfile
# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*
```

Alpine:

```Dockerfile
# install System.Drawing native dependencies
RUN apk add \
        --no-cache \
        --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ \
        libgdiplus
```

## Windows

For Windows, it's going to be more work because the required dependencies are not available in Windows Nano Server, the OS type which the official .NET images for Windows are based on. With .NET 5.0 and higher, official images for Windows Server Core are available so it's recommended to use those images if you can. Otherwise, you'll need to use the [Windows Server Core](https://hub.docker.com/_/microsoft-windows-servercore) or [Windows](https://hub.docker.com/_/microsoft-windows) images that are installed with the necessary dependencies. Note that the [Server Core](https://hub.docker.com/_/microsoft-windows-servercore) and [Windows](https://hub.docker.com/_/microsoft-windows) images are substantially larger than [Nano Server](https://hub.docker.com/_/microsoft-windows-nanoserver) images.

You have two deployment options available: you can choose to use a [framework-dependent](https://docs.microsoft.com/dotnet/core/deploying/#framework-dependent-deployments-fdd) or [self-contained](https://docs.microsoft.com/dotnet/core/deploying/#self-contained-deployments-scd) deployment. A framework-dependent deployment requires that the .NET runtime be installed on the base image whereas with a self-contained deployment, the app and all of its dependencies (including .NET) are deployed together.

### Self-contained deployment

A self-contained deployment is particularly handy in this scenario where there's a dependency on the System.Drawing.Common package. This is because there's no need to use a base image that has .NET installed. By deploying your app and all of its .NET dependencies, you can select the particular Windows base image that meets your need. In this example, since Windows Server Core has the required files and Nano Server does not, you can simply select a Windows Server Core image to use as your base image.

```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:3.1-nanoserver-ltsc2022 AS build

WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out -r win-x64 --self-contained true


# reference the Server Core image from the windows repo
FROM mcr.microsoft.com/windows/servercore:ltsc2022 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnetapp"]
```

### Framework-dependent deployment

For a framework-dependent deployment, you're relying on .NET being installed in the base image you'll be referencing. Because there are no official .NET Core 3.1 images based on [Windows Server Core](https://hub.docker.com/_/microsoft-windows-servercore) or [Windows](https://hub.docker.com/_/microsoft-windows), you'll need to author a Dockerfile that installs .NET Core yourself.

Follow the instructions for [Installing .NET in a Dockerfile](installing-dotnet.md) in order to create a custom image based on [Windows Server Core](https://hub.docker.com/_/microsoft-windows-servercore) or [Windows](https://hub.docker.com/_/microsoft-windows) that has .NET Core installed. For this example, let's say that you've tagged your image as `servercore/runtime:3.1`. You can then construct your application's Dockerfile to reference that tag as the base image:

```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:3.1-nanoserver-ltsc2022 AS build

WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out


# reference custom Server Core runtime image tag
FROM servercore/runtime:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```
