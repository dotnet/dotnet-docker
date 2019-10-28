# Using the System.Drawing.Common Package in a Docker Container

If your application makes use of the System.Drawing.Common NuGet package, you'll need to ensure some additional dependencies are installed in your Docker container for both Linux and Windows. See #618 (Linux) and #1098 (Windows) for more info on this issue.

For Linux, it's just a matter of installing a few extra packages in your Dockerfile:

```Dockerfile
# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*
```

For Windows, it's going to be more work because the required dependencies are not available in Windows Nano Server, the OS type which the official .NET Core images for Windows are based on. Instead, you'll need to use the [Windows Server Core images](https://hub.docker.com/_/microsoft-windows-servercore) that are installed with the necessary dependencies. Because there are no official .NET Core images based on Server Core, you'll need to author a Dockerfile that installs .NET Core yourself.

The quickest way to produce an image with .NET Core installed is to locate the relevant Dockerfiles in the [dotnet-docker](https://github.com/dotnet/dotnet-docker) repo that match the .NET Core and Windows version you want to work with. Copy those Dockerfiles and replace `mcr.microsoft.com/windows/nanoserver` with `mcr.microsoft.com/windows/servercore`. You can then build images from those Dockerfiles and tag them with your own tags (this example tags them with `servercore/sdk:4.8` and `servercore/runtime:4.8`) that you can then reference from your application's Dockerfile.

Example application Dockerfile:

```Dockerfile
# reference custom Server Core SDK image tag
FROM servercore/sdk:4.8 AS build

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
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```
