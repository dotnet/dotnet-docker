.NET CLI Preview Docker Image
====================

This repository contains `Dockerfile` definitions for [dotnet/cli] Docker images.

This project is part of .NET Core command-line (CLI) tools. You can find samples, documentation, and getting started instructions for .NET Core CLI tools at the [dotnet/cli] repo.

# Supported tags and respective `Dockerfile` links

-	[`0.0.1-alpha`, latest (*0.0.1-alpha/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/0.0.1-alpha/Dockerfile)
-	[`0.0.1-alpha-onbuild`, `latest-onbuild` (*0.0.1-alpha/onbuild/Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/0.0.1-alpha/onbuild/Dockerfile)

# How to use this image

## Start an instance of your app

The most straightforward way to use this image is to use a .NET container as both the build and runtime environment. In your `Dockerfile`, writing something along the lines of the following will compile and run your project:

```dockerfile
FROM microsoft/dotnet:0.0.1-alpha-onbuild
```

This image includes multiple `ONBUILD` triggers which should cover most applications. The build will `COPY . /dotnetapp` and `RUN dotnet restore`.

This image also includes the `ENTRYPOINT dotnet run` instruction which will run your application when the Docker image is run.

You can then build and run the Docker image:

```console
$ docker build -t myDotNetApp .
$ docker run -it --rm --name myRunningApp myDotNetApp
```

## Compile your app inside the Docker container

There may be occasions where it is not appropriate to run your app inside a container. To compile, but not run your app inside the Docker instance, you can write something like:

```console
$ docker run --rm -v "$PWD":/myapp -w /myapp microsoft/dotnet:0.0.1-alpha dotnet compile
```

This will add your current directory as a volume to the container, set the working directory to the volume, and run the command `dotnet compile` which will tell dotnet to compile the project in the working directory.

# Image variants

The `microsoft/dotnet` image come in many flavors, each designed for a specific use case.

## `microsoft/dotnet:<version>`

This is the defacto image. If you are unsure about what your needs are, you probably want to use this one. It is designed to be used both as a throw away container (mount your source code and start the container to start your app), as well as the base to build other images off of. This tag is based off of [`buildpack-deps`](https://registry.hub.docker.com/_/buildpack-deps/). `buildpack-deps` is designed for the average user of docker who has many images on their system. It, by design, has a large number of extremely common Debian packages. This reduces the number of packages that images that derive from it need to install, thus reducing the overall size of all images on your system.

## `microsoft/dotnet:<version>-onbuild`

This image makes building derivative images easier. For most use cases, creating a `Dockerfile` in the base of your project directory with the line `FROM microsoft/dotnet:onbuild` will be enough to create a stand-alone image for your project.

While the `onbuild` variant is really useful for "getting off the ground running" (zero to Dockerized in a short period of time), it's not recommended for long-term usage within a project due to the lack of control over *when* the `ONBUILD` triggers fire (see also [`docker/docker#5714`](https://github.com/docker/docker/issues/5714), [`docker/docker#8240`](https://github.com/docker/docker/issues/8240), [`docker/docker#11917`](https://github.com/docker/docker/issues/11917)).

Once you've got a handle on how your project functions within Docker, you'll probably want to adjust your `Dockerfile` to inherit from a non-`onbuild` variant and copy the commands from the `onbuild` variant `Dockerfile` (moving the `ONBUILD` lines to the end and removing the `ONBUILD` keywords) into your own file so that you have tighter control over them and more transparency for yourself and others looking at your `Dockerfile` as to what it does. This also makes it easier to add additional requirements as time goes on (such as installing more packages before performing the previously-`ONBUILD` steps).

[dotnet/cli]: https://github.com/dotnet/cli
