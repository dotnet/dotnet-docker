# Develop ASP.NET Core Applications in a Container

Docker containers can provide a local .NET Core development environment without having to install anything on your machine. You can also use containers to augment a locally installed development environment with one that matches production. This scenario is useful if you develop on an operating system that is different than production. If you support multiple operating systems, then this approach can be even more useful.

You can [build container images to create your environment](README.md), as described in a Dockerfile. That's straightforward and the common use case with Docker. This document describes a much more iterative and dynamic use of Docker, defining the container environment primarily via the commandline. .NET Core includes a command called `dotnet watch` that can rerun your application or your tests on each code change. This document describes how to use the Docker CLI and `dotnet watch` to develop applications in a container.

See [Develop .NET Core Applications in a Container](../dotnetapp/aspnet-docker-dev-in-container.md) for .NET Core-specific instructions.

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with [git](https://git-scm.com/downloads), using the following instructions:

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Requirements

The instructions below use .NET Core 2.1 Preview 2 container images. It is possible to make this scenario work with .NET Core 2.0 but requires many extra steps and a bit of magic. You do not need to switch to .NET Core 2.1 on your local machine to try out these instructions. They will work fine with .NET Core 2.0 projects.

It is recommended that you add a [Directory.Build.props](Directory.Build.props) file to your project to use different `obj` and `bin` folders for local and container use, to avoid conflicts between them. You should delete your existing obj and bin folders before making this change. You can also use `dotnet clean` for this purpose.

This approach relies on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument in the following commands) to mount source into the container (without using a Dockerfile). You may need to [Enable shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

## Run your application in a container while you Develop

You can re-run your application in a container with every local code change. This scenario works for both console applications and websites. The syntax differs a bit for Windows and Linux containers.

The instructions assume that you are in the root of the repository. You can use the following commands, given your environment:

**Windows** using **Linux containers**

```console
docker run --rm -p 8000:80 -e RUNNING_IN_CONTAINER=1 -v c:\git\dotnet-docker\samples\aspnetapp:/app/ microsoft/dotnet-nightly:2.1.300-preview2-sdk dotnet watch run"
```

Navigate to the site at `http://localhost:8000` in your browser.

**macOS or Linux** using **Linux containers**

```console
docker run --rm -p 8000:80 -e RUNNING_IN_CONTAINER=1 -v ~/git/dotnet-docker/samples/aspnetapp:/app/ microsoft/dotnet-nightly:2.1.300-preview2-sdk dotnet watch run"
```

Navigate to the site at `http://localhost:8000` in your browser.

**Windows** using **Windows containers**

`dotnet watch run` is not working correctly in containers at this time. The instructions are still documented while we work on enabling this scenario.

```console
docker run --rm -p 8000:80 --name aspnetappsample -e RUNNING_IN_CONTAINER=1 -v c:\git\dotnet-docker\samples\aspnetapp:c:\app\ microsoft/dotnet-nightly:2.1.300-preview2-sdk dotnet watch run"
```

In another command window, type `docker exec aspnetappsample ipconfig`. Navigate to the IP address you see in your browser.

### Updating the site while the container is running

You can demo a relaunch of the site by changing the About controller method in `HomeController.cs`, waiting a few seconds for the site to recompile and then visit `http://localhost:8000/Home/About`

## Test your application in a container while you develop

You can retest your application in a container with every local code change. This works for both console applications and websites. The syntax differs a bit for Windows and Linux containers. You can see this demonstrated in [Develop .NET Core Applications in a Container](../dotnetapp/dotnet-docker-dev-in-container.md).
