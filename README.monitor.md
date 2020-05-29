## Important: Client Firewall Rules Update to Microsoft Container Registry (MCR)

To provide a consistent FQDNs, the data endpoint will be changing from *.cdn.mscr.io to *.data.mcr.microsoft.com

For more info, see [MCR Client Firewall Rules](https://aka.ms/mcr/firewallrules).

# Featured Tags

* `latest` (Preview)
  * `docker pull mcr.microsoft.com/dotnet/nightly/monitor:latest`

# About This Image

This image contains the .NET Monitor tool.

Use this image as a sidecar container to collect diagnostic information from other containers running .NET Core 3.1 or later processes.

# How to Use the Image

TBD

# Related Repos

.NET Core:

* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
* [dotnet/nightly](https://hub.docker.com/_/microsoft-dotnet-nightly/): .NET Core (Preview)

# Full Tag Listing

## Linux amd64 Tags
Tags | Dockerfile | OS Version
-----------| -------------| -------------
latest, 5.0, 5.0.0 | [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/nightly/monitor/alpine3.11/amd64/Dockerfile) | Alpine 3.11

You can retrieve a list of all available tags for dotnet/nightly/monitor at https://mcr.microsoft.com/v2/dotnet/nightly/monitor/tags/list.

# Support

See [Microsoft Support for .NET Core](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Image Update Policy

* We update the supported .NET Core images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:1909, buildpack-deps:bionic-scm, etc.).

# Feedback

* [File a .NET Core Docker issue](https://github.com/dotnet/dotnet-docker/issues)
* [File a .NET Core issue](https://github.com/dotnet/core/issues)
* [File an ASP.NET Core issue](https://github.com/aspnet/home/issues)
* [File an issue for other .NET components](https://github.com/dotnet/core/blob/master/Documentation/core-repos.md)
* [File a Visual Studio Docker Tools issue](https://github.com/microsoft/dockertools/issues)
* [File a Microsoft Container Registry (MCR) issue](https://github.com/microsoft/containerregistry/issues)
* [Ask on Stack Overflow](https://stackoverflow.com/questions/tagged/.net-core)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/nightly/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/nightly/documentation/image-artifact-details.md)