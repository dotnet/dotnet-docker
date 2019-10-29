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


Other option using self-contained app


For Windows, it's going to be more work because the required dependencies are not available in Windows Nano Server, the OS type which the official .NET Core images for Windows are based on. Instead, you'll need to use the [Windows Server Core images](https://hub.docker.com/_/microsoft-windows-servercore) that are installed with the necessary dependencies. Because there are no official .NET Core images based on Server Core, you'll need to author a Dockerfile that installs .NET Core yourself. Note that Server Core images are substantially larger than Nano Server images.

The quickest way to produce an image with .NET Core installed is to locate the relevant Dockerfiles in the [dotnet-docker](https://github.com/dotnet/dotnet-docker) repo that match the .NET Core and Windows version you want to work with. Copy those Dockerfiles and replace `mcr.microsoft.com/windows/nanoserver` with `mcr.microsoft.com/windows/servercore`. You can then build images from those Dockerfiles and tag them with your own tags (this example tags them with `servercore/sdk:4.8` and `servercore/runtime:4.8`) that you can then reference from your application's Dockerfile.

These commands illustrate copying one of the .NET Core runtime Dockerfiles and modifying it to be based on Server Core instead of Nano Server:

```PowerShell
Invoke-WebRequest -OutFile Dockerfile.runtime -Uri https://raw.githubusercontent.com/dotnet/dotnet-docker/master/3.0/runtime/nanoserver-1903/amd64/Dockerfile
((Get-Content Dockerfile.runtime) -Replace 'mcr.microsoft.com/windows/nanoserver','mcr.microsoft.com/windows/servercore') | Set-Content Dockerfile.runtime
docker build -f Dockerfile.runtime -t servercore/runtime:3.0 .
```

Example application Dockerfile that references the custom runtime image:

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:3.0-nanoserver-1903 AS build

WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out


# reference custom Server Core runtime image tag
FROM servercore/runtime:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
COPY *.png .
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```
