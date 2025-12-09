# Determining License and Source Pedigree for .NET Container Images

This document is intended to be complimentary to the [Instructions for Finding Linux Legal Metadata](https://aka.ms/mcr/osslinuxmetadata).  This document describes how to interrogate the .NET images to determine licensing and version information for .NET and other components.

> [!WARNING]
> Distroless images require special treatment since they don't contain a shell by default.
> See the [distroless images section](#distroless-images-and-appliance-images) below.

## .NET and Other Components

.NET and other components are carried in .NET images, all of which are from Microsoft or the .NET Foundation. The following list describes the complete set of other software (beyond base images and packages):

* .NET Runtime
* ASP.NET Core Runtime
* .NET SDK
* PowerShell

You can see these components installed in the [runtime](https://github.com/dotnet/dotnet-docker/blob/d90e7bd1d10c8781f0008f5ab1327ca3481e78de/src/runtime/8.0/bookworm-slim/amd64/Dockerfile#L6-L13), [aspnet](https://github.com/dotnet/dotnet-docker/blob/d90e7bd1d10c8781f0008f5ab1327ca3481e78de/src/aspnet/8.0/bookworm-slim/amd64/Dockerfile#L6-L12), and [sdk](https://github.com/dotnet/dotnet-docker/blob/d90e7bd1d10c8781f0008f5ab1327ca3481e78de/src/sdk/8.0/bookworm-slim/amd64/Dockerfile#L26-L48) Dockerfiles.

### .NET Runtime and ASP.NET Core Images

The [runtime](../README.runtime.md) and [ASP.NET Core images](../README.aspnet.md) include a license and third party notice file.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:10.0 /bin/sh -c "find ./usr/share/dotnet | grep -i 'license\|third'"
./usr/share/dotnet/LICENSE.txt
./usr/share/dotnet/ThirdPartyNotices.txt
```

```console
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:10.0 /bin/sh -c "find ./usr/share/dotnet | grep -i 'license\|third'"
./usr/share/dotnet/LICENSE.txt
./usr/share/dotnet/ThirdPartyNotices.txt
```

### .NET SDK Image

The [SDK image](../README.sdk.md) includes the .NET SDK, which includes various .NET components, with associated licenses and third party notice files.

```console
$ docker run --rm mcr.microsoft.com/dotnet/sdk:10.0 /bin/sh -c "find ./usr/share/dotnet ./usr/share/powershell | grep -i 'license\|third'"
./usr/share/dotnet/LICENSE.txt
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/sdk/10.0.100/Sdks/Microsoft.SourceLink.Bitbucket.Git/LICENSE.txt
./usr/share/dotnet/sdk/10.0.100/Sdks/Microsoft.SourceLink.AzureRepos.Git/LICENSE.txt
./usr/share/dotnet/sdk/10.0.100/Sdks/Microsoft.Build.Tasks.Git/LICENSE.txt
./usr/share/dotnet/sdk/10.0.100/Sdks/Microsoft.SourceLink.GitHub/LICENSE.txt
./usr/share/dotnet/sdk/10.0.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/LICENSE.TXT
./usr/share/dotnet/sdk/10.0.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
./usr/share/dotnet/sdk/10.0.100/Sdks/Microsoft.SourceLink.Common/LICENSE.txt
./usr/share/dotnet/sdk/10.0.100/Sdks/Microsoft.SourceLink.GitLab/LICENSE.txt
./usr/share/powershell/.store/powershell.linux.x64/7.6.0-preview.4/powershell.linux.x64/7.6.0-preview.4/tools/net10.0/any/LICENSE.txt
./usr/share/powershell/.store/powershell.linux.x64/7.6.0-preview.4/powershell.linux.x64/7.6.0-preview.4/tools/net10.0/any/Modules/Microsoft.PowerShell.PSResourceGet/LICENSE
./usr/share/powershell/.store/powershell.linux.x64/7.6.0-preview.4/powershell.linux.x64/7.6.0-preview.4/tools/net10.0/any/Modules/Microsoft.PowerShell.ThreadJob/LICENSE
./usr/share/powershell/.store/powershell.linux.x64/7.6.0-preview.4/powershell.linux.x64/7.6.0-preview.4/tools/net10.0/any/Modules/Microsoft.PowerShell.ThreadJob/ThirdPartyNotices.txt
./usr/share/powershell/.store/powershell.linux.x64/7.6.0-preview.4/powershell.linux.x64/7.6.0-preview.4/tools/net10.0/any/Modules/PSReadLine/License.txt
./usr/share/powershell/.store/powershell.linux.x64/7.6.0-preview.4/powershell.linux.x64/7.6.0-preview.4/tools/net10.0/any/ThirdPartyNotices.txt
```

## Distroless Images and Appliance Images

The commands listed above won't work when targeting a [distroless image](./distroless.md) since distroless images don't contain a shell by default.
Instead, this repo contains a [Dockerfile](./scripts/Dockerfile.distroless-wrapper) that copies a distroless image's entire filesystem into a non-distroless base image so that its contents can be easily inspected using a shell.

First, build the Dockerfile, specifying the distroless image tag you wish to inspect.
Cloning the repo or downloading the Dockerfile is not necessary.

```console
docker build -t distroless-wrapper -f ./Dockerfile.distroless-wrapper --build-arg DISTROLESS_IMAGE=mcr.microsoft.com/dotnet/runtime:10.0-noble-chiseled https://github.com/dotnet/dotnet-docker.git#main:documentation/scripts
```

Now that you've got the wrapper image, you can execute the [commands that are documented](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md) targeting the wrapper image instead:

```console
$ docker run --rm distroless-wrapper /bin/sh -c "find ./usr/share/dotnet | grep -i 'license\|third'"
./usr/share/dotnet/LICENSE.txt
./usr/share/dotnet/ThirdPartyNotices.txt
```

### Appliance Images

.NET Monitor, Aspire Dashboard, and YARP images are all [distroless images](./distroless.md) and should be inspected the same way as other distroless .NET images.

Each appliance image includes its own license and third party notices files.
Appliance image binaries and metadata are located under the `/app` directory.
Example using .NET Monitor:

```console
$ docker build -t distroless-wrapper -f ./Dockerfile.distroless-wrapper --build-arg DISTROLESS_IMAGE=mcr.microsoft.com/dotnet/monitor https://github.com/dotnet/dotnet-docker.git#main:documentation/scripts
...

$ docker run --rm distroless-wrapper /bin/sh -c "find ./app | grep -i 'license\|third'"
./app/extensions/S3Storage/LICENSE.TXT
./app/extensions/S3Storage/THIRD-PARTY-NOTICES.TXT
./app/extensions/AzureBlobStorage/LICENSE.TXT
./app/extensions/AzureBlobStorage/THIRD-PARTY-NOTICES.TXT
./app/LICENSE.TXT
./app/THIRD-PARTY-NOTICES.TXT
```
