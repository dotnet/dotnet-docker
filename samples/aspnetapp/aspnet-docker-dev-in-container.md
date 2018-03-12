# Develop ASP.NET Core Applications in a Container

Docker containers can provide a local .NET Core development environment without having to install anything on your machine. You can also use containers to augment a locally installed development environment with one that matches production. This scenario is useful if you develop on an operating system that is different than production. If you support multiple operating systems, then this approach can be even more useful.

You can [build container images to create your environment](README.md), as described in a Dockerfile. That's straightforward and the common use case with Docker. This document describes a much more iterative and dynamic use of Docker, defining the container environment primarily via the commandline. .NET Core includes a command called `dotnet watch` that can rerun your application or your tests on each code change. This document describes how to use the Docker CLI and `dotnet watch` to develop applications in a container.

This approach relies on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument in the following commands) to mount the app into the container (without using a Dockerfile). You may need to [Enable shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

See [Develop .NET Core Applications in a Container](../dotnetapp/aspnet-docker-dev-in-container.md) for .NET Core-specific instructions.

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with [git](https://git-scm.com/downloads), using the following instructions:

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Project Requirements

If you are using .NET Core 2.0 or earlier, you need a project reference to the  [Microsoft.DotNet.Watcher.Tools](https://www.nuget.org/packages/Microsoft.DotNet.Watcher.Tools/) CLI Tool so that you can use [`dotnet watch`](https://docs.microsoft.com/aspnet/core/tutorials/dotnet-watch).

```xml
<ItemGroup>
    <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />
</ItemGroup>
```

If you are using .NET Core 2.1 Preview 2 or later, then this tool is not necessary (and should be removed as a project reference). `dotnet watch` is delivered with the .NET Core SDK starting with .NET Core 2.1 Preview 2.

## Run your application in a container while you Develop

You can re-run your application in a container with every local code change. This scenario works for both console applications and websites. The syntax differs a bit for Windows and Linux containers.

The instructions assume that you are in the root of the repository. You can use the following commands, given your environment:

**Windows** using **Windows containers**

`dotnet watch run` is not working correctly in containers at this time. The instructions are still documented while we work on enabling this scenario.

```console
docker run --rm -p 8000:80 --name aspnetappsample -e ASPNETCORE_URLS=http://+:80 -e DOTNET_USE_POLLING_FILE_WATCHER=1 -v c:\git\dotnet-docker\samples\aspnetapp:c:\app\ microsoft/dotnet:2.0-sdk cmd /c "cd app/aspnetapp && dotnet restore && dotnet watch run"
```

In another command window, type `docker exec aspnetappsample ipconfig`. The type IP address you see in your browser.

**Windows** using **Linux containers**

```console
docker run --rm -p 8000:80 -e ASPNETCORE_URLS=http://+:80 -e DOTNET_USE_POLLING_FILE_WATCHER=1 -v c:\git\dotnet-docker\samples\aspnetapp:/app/ microsoft/dotnet:2.0-sdk bash -c "cd app/aspnetapp && dotnet restore && dotnet watch run"
```

Navigate to the site at `http://localhost:8000` in your browser.

**macOS or Linux** using **Linux containers**

```console
docker run --rm -p 8000:80 -e ASPNETCORE_URLS=http://+:80 -e DOTNET_USE_POLLING_FILE_WATCHER=1 -v ~/git/dotnet-docker/samples/aspnetapp:/app/ microsoft/dotnet:2.0-sdk bash -c "cd app/aspnetapp && dotnet restore && dotnet watch run"
```

Navigate to the site at `http://localhost:8000` in your browser.

### Updating the site while the container is running

You can demo a relaunch of the site by changing the About controller method in `HomeController.cs`, waiting a few seconds for the site to recompile and then visit `http://localhost:8000/Home/About`

## Test your application in a container while you develop

You can retest your application in a container with every local code change. This works for both console applications and websites. The syntax differs a bit for Windows and Linux containers. You can see this demonstrated in [Develop .NET Core Applications in a Container](dotnet-docker-dev-in-container.md).
