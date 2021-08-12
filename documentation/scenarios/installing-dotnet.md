# Installing .NET in a Dockerfile

There can be times where you need .NET installed on a base image that is unavailable amongst the set of official [.NET Docker images](https://hub.docker.com/_/microsoft-dotnet), such as a different Linux distro version or a Windows Server Core image. In that case, you'll need to author your own Dockerfile which installs .NET. The snippets below describe how to do this.

There are two scenarios you should consider depending on how your Docker image is to be consumed: [general platform images](#general-platform-images) and [custom application images](#custom-application-images). But before getting to that, let's first consider whether you actually need to go down this road.

## Determine whether a different base image is needed

Before doing all the work of authoring and maintaining a Dockerfile that installs .NET, it's worthwhile to stop and thoroughly analyze whether you actually do need a different base image than those provided as part of the set of official [.NET Docker images](https://hub.docker.com/_/microsoft-dotnet).

If there's a platform that you require that is available in its own Docker image, ask yourself whether it would be better to use that image and add .NET to it or would it be better to use the .NET image as the base and add the platform to it. An example scenario is using the .NET runtime with PowerShell Core; determine whether you would prefer to start with a [PowerShell Core](https://hub.docker.com/_/microsoft-powershell) image and install .NET runtime onto it or start with a [.NET runtime image](https://hub.docker.com/_/microsoft-dotnet-runtime) and install PowerShell Core onto it.

In some cases, you can workaround dependencies by publishing your .NET application as a [self-contained app](https://docs.microsoft.com/en-us/dotnet/core/deploying) in which case all of your app's dependencies are packaged with the app. This reduces the dependencies that need to be installed separately on the base image. For example, a Windows app may require dependencies that only exist in Windows Server Core but only .NET (pre-5.0) images on Nano Server are available. In that case, the app could be deployed as a self-contained app and operate just fine using the [windows/servercore](https://hub.docker.com/_/microsoft-windows-servercore) image as the base image. Example Dockerfiles that demonstrate publishing a self-contained app are available in the samples for [Linux](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.debian-x64-selfcontained) and [Windows](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.nanoserver-x64-selfcontained).

## Image Purposes

### General Platform Images

If you're building an image for a platform that is intended to be publicly consumed, you should strongly consider following the Docker [guidelines](https://github.com/docker-library/official-images) for official images. Those guidelines will inject a high level of quality in your images and instill trust and confidence by those consuming it. One of the guidelines is on [clarity](https://github.com/docker-library/official-images#clarity) which states that a Dockerfile should be easy to understand/read and avoid the use of scripts that obscure the steps that are taken to produce the image.

For general platform images, it is recommended that you install .NET by [binary archive](#installing-from-a-binary-archive) (Linux/Windows) or [package manager](#installing-from-a-linux-package-manager) (Linux only). Both of those options can be used in a way that conforms to the Docker guidelines for official images.

### Custom Application Images

If you're building an image for a custom application that is not to be intended to be publicly consumed as a platform for other applications, you can get by with a simpler Dockerfile implementation compared to [general platform images](#general-platform-images) if you choose. Because the image is only intended for your own organization's purposes, the need for transparency in the Dockerfile is lessened. Convenience can trump clarity in this case.

For custom application images, it is recommended that you install .NET by [package manager](#installing-from-a-linux-package-manager) (Linux only) or [dotnet-install script](#installing-from-dotnet-install-script) (Linux/Windows).

## Ways to Install .NET

### Installing from a Binary Archive

When authoring your Dockerfiles, you can look to the official [.NET Dockerfiles](https://github.com/dotnet/dotnet-docker) as a template for the install steps. There are several variations depending on the .NET version, OS type, and architecture being used.

In addition to installing .NET, you'll also need to ensure that the [prerequisites](https://github.com/dotnet/core/blob/master/Documentation/prereqs.md) are installed. The [.NET Dockerfiles](https://github.com/dotnet/dotnet-docker) also demonstrate how that can be done.

In the spirit of [clarity](https://github.com/docker-library/official-images#clarity), the Dockerfiles for the official .NET Docker images do not use a general purpose script for installing .NET. Rather, they explicitly provide each step of the installation process and reference the exact URL of the binary archive.

Example (Linux):

```Dockerfile
FROM amd64/ubuntu:focal

RUN apt-get update \
    && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends \
        ca-certificates \
        \
        # .NET dependencies
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu66 \
        libssl1.1 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET
ENV DOTNET_VERSION=5.0.0

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='d4d67df5ff5f6dde0d865a6e87559955bd57429df396cf7d05fe77f09e6220c67dc5e66439b1801ca4d301a62f81f666122bf4b623b31a46b861677dcafc62a4' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
```

Example (Windows):

```Dockerfile
FROM mcr.microsoft.com/windows/servercore:ltsc2022

# Install .NET
ENV DOTNET_VERSION=5.0.0

RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
        $dotnet_sha512 = '968f2aff18aabc8f9ef0e1a6a3c0c3704d7a010ed453f0d56ded804513ccff913032461f1dbe0ce89522e85fbe5593d444f4310e8fdf7de7c7b72d01912086f1'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        Expand-Archive dotnet.zip -DestinationPath dotnet; `
        Remove-Item -Force dotnet.zip
```

This provides full transparency to consumers of the image in regard to where the content is coming from and whether it can be trusted; it's not hiding somewhere buried within a script. It also ensures [repeatability](https://github.com/docker-library/official-images#repeatability), another guideline of official Docker images.

#### Servicing Maintenance

By having the version of .NET you're installing explicitly defined in the Dockerfile, as should be done for clarity reasons, it means the Dockerfile must be regularly maintained to account for servicing releases of .NET. There are two parts of the install steps that will need to updated in order to reference a new release:

* Version environment variable that is referenced in the download URL (e.g. `ENV DOTNET_VERSION=5.0.0`)
* SHA value (e.g. `dotnet_sha512='d4d67df5ff5f6dde0d865a6e87559955bd57429df396cf7d05fe77f09e6220c67dc5e66439b1801ca4d301a62f81f666122bf4b623b31a46b861677dcafc62a4'`)

You can track these values by making use of the information contained in the `releases.json` of the relevant release. For example, the [`releases.json`](https://dotnetcli.azureedge.net/dotnet/release-metadata/5.0/releases.json) for 5.0 contains all the metadata for the 5.0 releases including download links of the binary archives as well as their hash values. The release information is described on the main [release notes](https://github.com/dotnet/core/blob/master/release-notes/README.md) page.

### Installing from a Linux Package Manager

For Linux, you may prefer to use your Linux distro's package manager to install .NET rather than directly from a binary archive. See the [Install .NET on Linux](https://docs.microsoft.com/en-us/dotnet/core/install/linux) guideance on how to install .NET from the various distro package managers.

Using a package manager allows for easier maintenance since you only have to reference the major/minor version of the release and you'll get servicing releases "for free". This is true as long as you are mindful of Docker's caching functionality. You may need to build with the `--no-cache` option to force the build to re-execute the commands that install .NET in order to get an updated servicing release. Alternatively, you can specify the full version (major/minor/build) and increment that with each service release.

Another benefit of installing via a package manager is that all of .NET's dependencies are installed by the package manager for you. You don't have to manually list the prerequisites to be installed like you do when using [binary archives](#installing-from-a-binary-archive) or the [dotnet-install script](#installing-from-dotnet-install-script).

Example:

```Dockerfile
FROM ubuntu:focal
RUN export DEBIAN_FRONTEND=noninteractive \
    apt-get update \
    # Install prerequisites
    && apt-get install -y --no-install-recommends \
       wget \
       ca-certificates \
    \
    # Install Microsoft package feed
    && wget -q https://packages.microsoft.com/config/ubuntu/19.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb \
    \
    # Install .NET
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        dotnet-runtime-5.0 \
    \
    # Cleanup
    && rm -rf /var/lib/apt/lists/*
```

### Installing from dotnet-install script

A set of [installation scripts](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script) are provided to conveniently install .NET on Linux with Bash or Windows with PowerShell. These scripts can be thought of as a happy medium between the two previously mentioned approaches (binary archive link and package manager). They fill a gap on systems where the desired .NET release is not available through a package manager and you don't want to deal with the cost of maintaining a direct link to a binary package. With the installation script, you have flexibility in specifying which version gets installed. You can install a specific version such as 5.0.0, the latest of a release channel such as the latest 5.0 patch, etc.

In addition to installing .NET, you'll also need to ensure that the [prerequisites](https://github.com/dotnet/core/blob/master/Documentation/prereqs.md) are installed. The [.NET Dockerfiles](https://github.com/dotnet/dotnet-docker) also demonstrate how that can be done.

Example (Linux):

```Dockerfile
FROM ubuntu:focal

RUN apt-get update \
    && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends \
        curl \
        ca-certificates \
        \
        # .NET dependencies
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu66 \
        libssl1.1 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -Channel 5.0 -Runtime dotnet -InstallDir /usr/share/dotnet \
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
            -Channel 5.0 `
            -Runtime dotnet; `
        Remove-Item -Force dotnet-install.ps1 `
    && setx /M PATH "%PATH%;C:\Program Files\dotnet"
```
