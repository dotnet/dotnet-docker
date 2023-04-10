{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}{{ARGS["top-header"]}}# Building a Custom .NET Monitor Image

The following Dockerfiles demonstrate how you can use this base image to build a .NET Monitor image with a custom set of extensions.

* [CBL Mariner Distroless - amd64](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/cbl-mariner-distroless/amd64/Dockerfile)
* [CBL Mariner Distroless - arm64v8](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/cbl-mariner-distroless/arm64v8/Dockerfile)
* [Ubuntu Chiseled - amd64](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/amd64/Dockerfile)
* [Ubuntu Chiseled - arm64v8](https://github.com/dotnet/dotnet-docker/blob/main/src/monitor/8.0/ubuntu-chiseled/arm64v8/Dockerfile)
