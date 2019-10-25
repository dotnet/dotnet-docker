# Installing .NET Core in a Dockerfile

There can be times where you need .NET Core installed on a base image than is unavailable amongst the set of official .NET Core Docker images, such as a boutique Linux distro for example. In that case, you'll need to author your own Dockerfile which installs .NET Core. The snippets below describe how to do this.

There are two scenarios you should consider depending on how your Docker image is to be consumed: [general platform images](#general-platform-images) and [custom application images](#custom-application-images).

## General Platform Images

If you're building an image for a platform that is intended to be publicly consumed, you should strongly consider following the Docker [guidelines](https://github.com/docker-library/official-images) for official images. Those guidelines will inject a high level of quality in your images and instill trust and confidence by those consuming it. One of the guidelines is on [clarity](https://github.com/docker-library/official-images#clarity). In the spirit of clarity, the Dockerfiles for the official .NET Docker images do not use a general purpose script for installing .NET Core. Rather, they explicity provide each step of the installation process and reference the exact URL of the binary archive.

Example:

```Dockerfile
# Install .NET Core
ENV DOTNET_VERSION 3.0.0

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='0cabf85877eb3ee0415e6f8de9390c95ec90fa8f5a0fdb104f1163924fd52d89932a51c2e07b5c13a6b9802d5b6962676042a586ec8aff4f2a641d33c6c84dec' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
```

This provides full transparency to consumers of the image in regard to where the content is coming from and whether it can be trusted; it's not hiding somewhere buried within a script. It also ensures repeatability, another [guideline](https://github.com/docker-library/official-images#repeatability) of official Docker images.

For general platform images, it is recommended that you install .NET Core by [binary archive](#installing-from-a-binary-archive) (Linux/Windows) or [package manager](#installing-from-a-linux-package-manager) (Linux only).

## Custom Application Images

If you're building an image for a custom application that is not to be intended to be publicly consumed as a platform for other applications, you can get by with a simpler Dockerfile implementation compared to [general platform images](#general-platform-images) if you choose. Because the image is only intended for your own organization's purposes, the need for transparency in the Dockerfile is lessened. Convenience can trump clarity in this case.

For custom application images, it is recommended that you install .NET Core by [package manager](#installing-from-a-linux-package-manager)(Linux only) or [dotnet-install script](#installing-from-dotnet-install-script) (Linux/Windows).

## Installing from a Binary Archive

When authoring your Dockerfiles, you can look to the official [.NET Core Dockerfiles](https://github.com/dotnet/dotnet-docker) as a template for the install steps. There are several variations depending on the .NET Core version, OS type, and architecture being used.

### Servicing Maintenance

By having the version of .NET Core you're installing explicitly defined in the Dockerfile, as should be done for clarity reasons, it means the Dockerfile must be regularly maintained to account for servicing releases of .NET Core. There are two parts of the install steps that will need to updated in order to reference a new release:

* Version environment variable that is referenced in the download URL (e.g. `ENV DOTNET_VERSION 3.0.0`)
* SHA value (e.g. `dotnet_sha512='0cabf85877eb3ee0415e6f8de9390c95ec90fa8f5a0fdb104f1163924fd52d89932a51c2e07b5c13a6b9802d5b6962676042a586ec8aff4f2a641d33c6c84dec'`)

You can track these values by making use of the information contained in the `releases.json` of the relevant release. For example, the `[releases.json](https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/3.0/releases.json)` for 3.0 contains all the metadata for the 3.0 releases including download links of the binary archives as well as their hash values. The release information is described on the main [release notes](https://github.com/dotnet/core/blob/master/release-notes/README.md) page.

## Installing from a Linux Package Manager

For Linux, you may prefer to use your Linux distro's package manager to install .NET Core rather than directly from a binary archive. Each .NET Core release provides [release notes](https://github.com/dotnet/core/tree/master/release-notes) that contains details on how to install from the various distro package managers. Using a package manager allows for easier maintenance since you only have to reference the major/minor version of the release and you'll get servicing releases "for free". (This is true as long as you are mindful of Docker's caching functionality. You may need to build with the `--no-cache` option to force the build to re-execute the commands that install .NET Core in order to get an updated servicing release.)

Example:

```Dockerfile
FROM ubuntu:disco
RUN apt-get update \
    # Install pre-requisites
    && apt-get install -y --no-install-recommends \
       wget \
       ca-certificates \
    \
    # Install Microsoft package feed
    && wget -q https://packages.microsoft.com/config/ubuntu/19.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb \
    \
    # Install .NET Core
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        dotnet-runtime-3.0 \
    \
    # Cleanup
    && rm -rf /var/lib/apt/lists/*
```

## Installing from dotnet-install script

A set of [installation scripts](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script) are provided to conveniently install .NET Core on Linux with Bash or Windows with PowerShell. These scripts can be thought of as a happy medium between the two previously mentioned approaches (binary archive and package manager). They fill a gap on systems where the desired .NET Core release is not available through a package manager. And they can also simplify the maintenance of Dockerfiles by not needing to specify explicit version paths to binary archives.

Example:

```Dockerfile
FROM ubuntu:disco

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
        ca-certificates \
        \
        # .NET Core dependencies
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu63 \
        libssl1.1 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -Channel 3.0 -Runtime dotnet -InstallDir /usr/share/dotnet \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
```
