# Ubuntu Chiseled + .NET

## What is Ubuntu Chiseled?

.NET's Ubuntu Chiseled images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface compared to our "full" Ubuntu images that are based on the Ubuntu base images. This is achieved through the following features:

- Minimal set of packages required to run a .NET application
- Non-root user by default
- No package manager
- No shell

## Featured Tags

* `8.0-noble-chiseled`
  * `docker pull mcr.microsoft.com/dotnet/aspnet:8.0-noble-chiseled`
  * `docker pull mcr.microsoft.com/dotnet/runtime:8.0-noble-chiseled`
  * `docker pull mcr.microsoft.com/dotnet/runtime-deps:8.0-noble-chiseled`

We’re not offering a chiseled SDK image as there wasn't a strong need for one, and a chiseled SDK image could be hard to use for some scenarios.
You can continue to use the existing full Ubuntu SDK images to build your apps to run on Chiseled.
If you have a compelling use case for a distroless SDK image, please leave a comment on [this issue](https://github.com/dotnet/dotnet-docker/issues/4942) and we’ll be happy to reconsider.

## How do I use Ubuntu Chiseled .NET images?

Please see our sample Dockerfiles for examples on how to use Ubuntu Chiseled .NET images:
- [aspnetapp](../samples/aspnetapp/Dockerfile.chiseled)
- [dotnetapp](../samples/dotnetapp/Dockerfile.chiseled)
- [releasesapi](../samples/releasesapi/Dockerfile.ubuntu-chiseled) (and [icu version](../samples/releasesapi/Dockerfile.ubuntu-chiseled-icu))
- [releasesapp](../samples/releasesapp/Dockerfile.chiseled)

If your app's Dockerfile doesn't install any additional Linux packages or depend on any shell scripts for setup, Ubuntu Chiseled images could be a drop-in replacement for our full Ubuntu or Debian images.
