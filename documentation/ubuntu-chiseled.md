# Ubuntu Chiseled + .NET

## What is Ubuntu Chiseled?

.NET's Ubuntu Chiseled images are a type of [distroless container image](./distroless.md) that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface compared to our "full" Ubuntu images that are based on the Ubuntu base images. This is achieved through the following features:

* Minimal set of packages required to run a .NET application
* Non-root user by default
* No package manager
* No shell

## Featured Tags

* `10.0-noble-chiseled`
  * `docker pull mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled`
  * `docker pull mcr.microsoft.com/dotnet/runtime:10.0-noble-chiseled`
  * `docker pull mcr.microsoft.com/dotnet/runtime-deps:10.0-noble-chiseled`
* `8.0-noble-chiseled`
  * `docker pull mcr.microsoft.com/dotnet/aspnet:8.0-noble-chiseled`
  * `docker pull mcr.microsoft.com/dotnet/runtime:8.0-noble-chiseled`
  * `docker pull mcr.microsoft.com/dotnet/runtime-deps:8.0-noble-chiseled`

We’re not offering a chiseled SDK image as there wasn't a strong need for one, and a chiseled SDK image could be hard to use for some scenarios.
You can continue to use the existing full Ubuntu SDK images to build your apps to run on Chiseled.
If you have a compelling use case for a distroless SDK image, please leave a comment on [this issue](https://github.com/dotnet/dotnet-docker/issues/4942) and we’ll be happy to reconsider.

## How do I use Ubuntu Chiseled .NET images?

Please see our sample Dockerfiles for examples on how to use Ubuntu Chiseled .NET images:

* [aspnetapp](../samples/aspnetapp/Dockerfile.chiseled)
* [dotnetapp](../samples/dotnetapp/Dockerfile.chiseled)
* [releasesapi](../samples/releasesapi/Dockerfile) (and [icu version](../samples/releasesapi/Dockerfile.icu))
* [releasesapp](../samples/releasesapp/Dockerfile.chiseled)

If your app's Dockerfile doesn't depend on any shell scripts for setup, Ubuntu Chiseled images could be a drop-in replacement for our full Ubuntu or Debian images.

## How do I install additional packages on Chiseled images?

> [!IMPORTANT]
> Installing additional packages requires the presence of the [Chisel manifest](https://github.com/dotnet/dotnet-docker/issues/6135), which is currently only available in **.NET 10+** images.

[Chisel](https://github.com/canonical/chisel) is built on the idea of package slices.
Slices are basically subsets of packages, with their own content and set of dependencies to other internal and external slices.
Chisel relies on a [database of slices](https://github.com/canonical/chisel-releases) that are indexed per Ubuntu release.
You can only install packages that are available as slices.

First, acquire `chisel` and `chisel-wrapper`.
`chisel-wrapper` (from [rocks-toolbox](https://github.com/canonical/rocks-toolbox/)) is used to generate a dpkg status file documenting which packages are installed, which is used by many vulnerability scanning tools.

```Dockerfile
FROM mcr.microsoft.com/dotnet/nightly/sdk:10.0-noble AS chisel

# Docker build arg documentation: https://docs.docker.com/build/building/variables/
# Find the latest chisel releases: https://github.com/canonical/chisel/releases
ARG CHISEL_VERSION="v1.X.X"
# Find the latest chisel-wrapper releases: https://github.com/canonical/rocks-toolbox/releases
ARG CHISEL_WRAPPER_VERSION="v1.X.X"

# Install chisel's dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        file \
    && rm -rf /var/lib/apt/lists/*

# Install chisel and chisel-wrapper
RUN chisel_url=https://github.com/canonical/chisel/releases/download/${CHISEL_VERSION}/chisel_${CHISEL_VERSION}_linux_amd64.tar.gz \
    && curl -fSLOJ ${chisel_url} \
    && curl -fSL ${chisel_url}.sha384 | sha384sum -c - \
    && tar -xzf chisel_${CHISEL_VERSION}_linux_amd64.tar.gz -C /usr/bin/ chisel \
    && curl -fSL --output /usr/bin/chisel-wrapper https://raw.githubusercontent.com/canonical/rocks-toolbox/${CHISEL_WRAPPER_VERSION}/chisel-wrapper \
    && chmod 755 /usr/bin/chisel-wrapper
```

Then, copy over the filesystem from your desired base image and use `chisel` and `chisel-wrapper` to install additional slices.
See [canonical/chisel-releases](https://github.com/canonical/chisel-releases) for available slices.

```Dockerfile
COPY --from=mcr.microsoft.com/dotnet/nightly/runtime-deps:10.0-noble-chiseled / /rootfs/

RUN chisel-wrapper --generate-dpkg-status /rootfs/var/lib/dpkg/status -- \
        --release ubuntu-24.04 --root /rootfs/ \
            libicu74_libs \
            tzdata-legacy_zoneinfo \
            tzdata_zoneinfo
```

Finally, copy the new root filesystem into a scratch base image for your final (runtime) layer.

```Dockerfile
FROM scratch AS final
COPY --link --from=chisel /rootfs /
WORKDIR /app
COPY --link --from=chisel /app .
# user as defined in .NET base image
USER app
ENTRYPOINT ["./app"]
```

Copying the entire filesystem into the final `FROM scratch` layer results in a perfectly space- and layer-efficient installation of the additional slices.
However, it's important to note that this pattern does not allow any layer sharing with your chosen .NET base image - the first `COPY` instruction in the final stage is essentially creating a completely new base image layer.
Keep this in mind when creating your Dockerfiles - for example, you may find it beneficial to share layers between multiple .NET images running in the same Kubernetes cluster.
