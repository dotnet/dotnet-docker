# Installing .NET in a Dockerfile

There can be times where you need .NET installed on a base image that is unavailable amongst the set of official [.NET Docker images](../../README.md), such as a different Linux distro version or a Windows Server Core image. In that case, you'll need to author your own Dockerfile which installs .NET. The snippets below describe how to do this.

There are two scenarios you should consider depending on how your Docker image is to be consumed: [base images](#base-images) and [application images](#application-images). But before getting to that, let's first consider whether you actually need to go down this road.

## Determine whether a different base image is needed

Before doing all the work of authoring and maintaining a Dockerfile that installs .NET, it's worthwhile to stop and thoroughly analyze whether you actually do need a different base image than those provided as part of the set of official [.NET Docker images](../../README.md).

If there's a platform that you require that is available in its own Docker image, ask yourself whether it would be better to use that image and add .NET to it or would it be better to use the .NET image as the base and add the platform to it. An example scenario is using the .NET runtime with PowerShell; determine whether you would prefer to start with a [PowerShell](https://mcr.microsoft.com/product/powershell/about) image and install .NET runtime onto it or start with a [.NET runtime image](../../README.runtime.md) and install PowerShell Core onto it.

In some cases, you can work around the need for the .NET runtime by publishing your application as a [self-contained app](https://learn.microsoft.com/en-us/dotnet/core/deploying/), in which case all of your app's dependencies are packaged with the app. This reduces the dependencies that need to be installed separately on the base image. Example Dockerfiles that demonstrate publishing a self-contained app are available in the [releasesapp sample](../../samples/releasesapp/README.md).

## Image Purposes

### Base Images

If you're building a base image that is intended to be publicly consumed, you should strongly consider following the Docker [guidelines](https://github.com/docker-library/official-images) for official images.
Those guidelines will help you create a high quality and maintainable Dockerfile that is easy for users to understand.

For general base images, it is recommended that you install .NET by [binary archive](#installing-from-a-binary-archive) (Linux/Windows) or [package manager](#installing-from-a-linux-package-manager) (Linux only). Both of those options can be used in a way that conforms to the Docker guidelines for official images.

### Application Images

If you're building an image for a custom application that is not to be intended to be publicly consumed as a platform for other applications, you can get by with a simpler Dockerfile implementation compared to [base images](#base-images) if you choose. Because the image is only intended for your own organization's purposes, the need for transparency in the Dockerfile is lessened. Convenience can trump clarity in this case.

For custom application images, it is recommended that you install .NET by [package manager](#installing-from-a-linux-package-manager) (Linux only) or [dotnet-install script](#installing-from-dotnet-install-script) (Linux/Windows).

## Installing .NET

### Prerequisites

When installing .NET [from a binary archive](#installing-from-a-binary-archive) or the [dotnet-install script](#installing-from-dotnet-install-script), you need to make sure .NET's runtime dependencies are present in order for the installation to function properly.
You can find the full list of .NET dependencies for each Linux distro and .NET version [here](https://github.com/dotnet/core/blob/main/linux.md#dependencies), including example commands for installing the packages.
You can also see the official [.NET Dockerfiles](https://github.com/dotnet/dotnet-docker/tree/main/src) for more package installation examples.

When [installing from a package manager](#installing-from-a-linux-package-manager), the package manager will handle resolving dependencies for you.

### Installing from a Binary Archive

When authoring your Dockerfiles, you can look to the official [.NET Dockerfiles](https://github.com/dotnet/dotnet-docker) as a template for the install steps. There are several variations depending on the .NET version, OS type, and architecture being used.

#### Installing .NET Binaries

In the spirit of [clarity](https://github.com/docker-library/official-images#clarity), the Dockerfiles for the official .NET Docker images do not use a general purpose script for installing .NET. Rather, they explicitly provide each step of the installation process and reference the exact URL of the binary archive.

Example (Linux):

```Dockerfile
FROM amd64/ubuntu:noble

# Reference:
# - https://github.com/dotnet/core/blob/main/release-notes/9.0/os-packages.md#ubuntu-2404-noble-numbat
# - https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/9.0/noble/amd64/Dockerfile
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
        \
        # .NET dependencies
        libc6 \
        libgcc-s1 \
        libicu74 \
        libssl3t64 \
        libstdc++6 \
        tzdata \
    && rm -rf /var/lib/apt/lists/*

# Latest download URLs and SHA512 hashes can be found in releases.json:
# https://github.com/dotnet/core/blob/main/release-notes/9.0/releases.json
ENV DOTNET_URL="https://download.visualstudio.microsoft.com/download/pr/ebc433c4-8f01-43c8-a1e2-bbe1291ba857/e073f3f679d7a4067a56e8f5d12fc0e5/dotnet-runtime-8.0.10-linux-x64.tar.gz"
ENV DOTNET_SHA512="7fb813677720d125c2337fedc6131b230daf1c1d79d5912a1ca6b5e08bf7802b412de3248d645b6483ab23f3fae837ed02a0e520e33020cfef2c888c54f474ac"

# Install .NET
RUN curl -fSL --output dotnet.tar.gz $DOTNET_URL \
    && dotnet_sha512=$DOTNET_SHA512 \
    && echo "$DOTNET_SHA512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
```

Example (Windows):

```Dockerfile
FROM mcr.microsoft.com/windows/servercore:ltsc2022

# Latest download URLs and SHA512 hashes can be found in releases.json:
# https://github.com/dotnet/core/blob/main/release-notes/9.0/releases.json
ENV DOTNET_URL="https://download.visualstudio.microsoft.com/download/pr/697fe02d-5f59-4fd3-ba15-b0ee74bec5d9/4fb434c648aaf10f18682ccbe6d59bc6/dotnet-runtime-8.0.10-win-x64.zip"
ENV DOTNET_SHA512="fefa7e8958a67d1a108457ed55906eb62a53fa61d5fb0187c489b981946d988ff2e31aa1ce7b1fd70ce7b6c1e07c616983161e13cd1009655a9ba5297677a5f7"

# Reference:
# - https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/9.0/nanoserver-ltsc2022/amd64/Dockerfile
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile dotnet.zip $Env:DOTNET_URL; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $Env:DOTNET_SHA512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar -oxzf dotnet.zip -C dotnet; `
        Remove-Item -Force dotnet.zip
```

This provides full transparency to consumers of the image in regard to where the content is coming from and whether it can be trusted; it's not hiding somewhere buried within a script. It also ensures [repeatability](https://github.com/docker-library/official-images#repeatability), another guideline of official Docker images.

#### Servicing Maintenance

By having the version of .NET you're installing explicitly defined in the Dockerfile, as should be done for clarity reasons, it means the Dockerfile must be regularly maintained to account for servicing releases of .NET. There are two parts of the install steps that will need to updated in order to reference a new release:

* .NET download URL (e.g. `ENV DOTNET_URL=https://download.visualstudio.microsoft.com/...`)
* SHA value (e.g. `dotnet_sha512='d4d67df5ff5f6dde0d865a6e87559955bd57429df396cf7d05fe77f09e6220c67dc5e66439b1801ca4d301a62f81f666122bf4b623b31a46b861677dcafc62a4'`)

You can track these values by making use of the information contained in the `releases.json` of the relevant release. For example, the [`releases.json`](https://dotnetcli.azureedge.net/dotnet/release-metadata/6.0/releases.json) for 6.0 contains all the metadata for the 6.0 releases including download links of the binary archives as well as their hash values. The release information is described on the main [release notes](https://github.com/dotnet/core/blob/master/release-notes/README.md) page.

### Installing from a Linux Package Manager

For Linux, you may prefer to use your Linux distro's package manager to install .NET rather than directly from a binary archive.
See the [Install .NET on Linux](https://docs.microsoft.com/en-us/dotnet/core/install/linux) guidance on how to install .NET from the various distro package managers.

Using a package manager allows for easier maintenance since you only have to reference the major/minor version of the release and you'll get servicing releases "for free". This is true as long as you are mindful of Docker's caching functionality. You may need to build with the `--no-cache` option to force the build to re-execute the commands that install .NET in order to get an updated servicing release. Alternatively, you can specify the full version (major/minor/build) and increment that with each service release.

Another benefit of installing via a package manager is that all of .NET's dependencies are installed by the package manager for you. You don't have to manually list the prerequisites to be installed like you do when using [binary archives](#installing-from-a-binary-archive) or the [dotnet-install script](#installing-from-dotnet-install-script).

#### Native Distro Packages vs. `packages.microsoft.com`

.NET is available in the default package manager feeds for some Linux distros as well as from the Microsoft package repository (packages.microsoft.com).
However, you should only use one or the other to install .NET.

Starting with .NET 9, we have stopped publishing .NET packages on `packages.microsoft.com` for distros that publish their own .NET packages.
For more details please see [.NET 9 Package Publishing announcement](https://github.com/dotnet/core/discussions/9556).

.NET is supported by Microsoft when downloaded from a Microsoft source. Best effort support is offered from Microsoft when downloaded from elsewhere. You can open issues at [dotnet/core](https://github.com/dotnet/core/issues/new) if you run into problems.

#### Native Package Installation Example

Note: distro-archive packages are typically limited to the `.1xx` SDK band, while packages.microsoft.com packages are not. Users that want to use the latest feature band builds (on distros without packages.microsoft.com packages available) must install those builds manually via [binary archive](#installing-from-a-binary-archive) or [dotnet-install script](#installing-from-dotnet-install-script).

```Dockerfile
FROM ubuntu:noble
RUN apt-get update \
    && apt-get install -y dotnet-sdk-8.0 \
    && rm -rf /var/lib/apt/lists/*
```

#### Microsoft Package Feeds Installation Example

Instructions for setting up the Microsoft package feed vary between Linux distros.
Please refer to [Install .NET on Linux](https://learn.microsoft.com/en-us/dotnet/core/install/linux#packages) for detailed instructions for each supported distro.

Keep in mind the [Dockerfile best practices](https://docs.docker.com/build/building/best-practices/) when porting the installation commands to your Dockerfile - in particular, installing or using `sudo` in your Dockerfile is not neccesary since the default user in most base container images is `root`.

Here is an example Dockerfile using Debian:

```Dockerfile
FROM debian:bookworm-slim
RUN apt-get update \
    # Install prerequisites
    && apt-get install -y --no-install-recommends \
       wget \
       ca-certificates \
    \
    # Install Microsoft package feed
    && wget -q https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb \
    \
    # Install .NET
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        dotnet-runtime-8.0 \
    \
    # Cleanup
    && rm -rf /var/lib/apt/lists/*
```

### Installing from dotnet-install script

A set of [installation scripts](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script) are provided to conveniently install .NET on Linux with Bash or Windows with PowerShell. These scripts can be thought of as a happy medium between the two previously mentioned approaches (binary archive link and package manager). They fill a gap on systems where the desired .NET release is not available through a package manager and you don't want to deal with the cost of maintaining a direct link to a binary package. With the installation script, you have flexibility in specifying which version gets installed. You can install a specific version such as 6.0.0, the latest of a release channel such as the latest 6.0 patch, etc.

In addition to installing .NET, you'll also need to ensure that the [prerequisites](https://github.com/dotnet/core/blob/main/linux.md#dependencies) are installed. The [.NET Dockerfiles](https://github.com/dotnet/dotnet-docker) also demonstrate how that can be done.

Example (Linux):

```Dockerfile
FROM ubuntu:noble

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
        \
        # .NET dependencies
        libc6 \
        libgcc-s1 \
        libicu74 \
        libssl3t64 \
        libstdc++6 \
        tzdata \
    && rm -rf /var/lib/apt/lists/*

RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -Channel 9.0 -Runtime dotnet -InstallDir /usr/share/dotnet \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
```

Example (Windows):

```Dockerfile
# escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2022
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        Invoke-WebRequest `
            -UseBasicParsing `
            -Uri https://dot.net/v1/dotnet-install.ps1 `
            -OutFile dotnet-install.ps1; `
        ./dotnet-install.ps1 `
            -InstallDir '/Program Files/dotnet' `
            -Channel 9.0 `
            -Runtime dotnet; `
        Remove-Item -Force dotnet-install.ps1 `
    && setx /M PATH "%PATH%;C:\Program Files\dotnet"
```
