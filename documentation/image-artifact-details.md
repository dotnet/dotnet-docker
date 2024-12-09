# Determining License and Source Pedigree for .NET Container Images

This document is intended to be complimentary to the [Instructions for Finding Linux Legal Metadata](https://aka.ms/mcr/osslinuxmetadata).  This document describes how to interrogate the .NET images to determine licensing and version information for .NET and other components.

> [!WARNING]
> Distroless images require special treatment since they don't contain a shell by default.
> Please see [Distroless Images](#distroless-images) section below.

## .NET and Other Components

.NET and other components are carried in .NET images, all of which are from Microsoft or the .NET Foundation. The following list describes the complete set of other software (beyond base images and packages):

* .NET Runtime
* ASP.NET Core Runtime
* .NET SDK
* PowerShell

You can see these components installed in the [runtime](https://github.com/dotnet/dotnet-docker/blob/d90e7bd1d10c8781f0008f5ab1327ca3481e78de/src/runtime/8.0/bookworm-slim/amd64/Dockerfile#L6-L13), [aspnet](https://github.com/dotnet/dotnet-docker/blob/d90e7bd1d10c8781f0008f5ab1327ca3481e78de/src/aspnet/8.0/bookworm-slim/amd64/Dockerfile#L6-L12), and [sdk](https://github.com/dotnet/dotnet-docker/blob/d90e7bd1d10c8781f0008f5ab1327ca3481e78de/src/sdk/8.0/bookworm-slim/amd64/Dockerfile#L26-L48) Dockerfiles.

### .NET Runtime Image

The [.NET runtime image](../README.runtime.md) includes the .NET runtime, with an associated license and third party notice file.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:9.0-bookworm-slim /bin/sh -c "find ./usr/share/dotnet | grep LICENSE"
./usr/share/dotnet/LICENSE.txt
```

The license can be printed out, as follows.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:9.0-bookworm-slim cat ./usr/share/dotnet/LICENSE.txt
The MIT License (MIT)

Copyright (c) .NET Foundation and Contributors

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
...
```

Third party notices can also be found, as demonstrated below.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:9.0-bookworm-slim /bin/sh -c "find ./usr/share/dotnet | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
```

### ASP.NET Core Image

The [ASP.NET Core image](../README.aspnet.md) includes ASP.NET Core in addition to .NET, with associated licenses and third party notice files.

```console
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim /bin/sh -c "find ./usr/share/dotnet | grep LICENSE"
./usr/share/dotnet/LICENSE.txt

$ docker run --rm mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim /bin/sh -c "find ./usr/share/dotnet | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/9.0.0/THIRD-PARTY-NOTICES.txt
```

### .NET SDK Image

The [SDK image](../README.sdk.md) includes the .NET SDK, which includes various .NET components, with associated licenses and third party notice files.

```console
$ docker run --rm mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim /bin/sh -c "find ./usr/share/dotnet ./usr/share/powershell | grep LICENSE"
./usr/share/dotnet/LICENSE.txt
./usr/share/dotnet/sdk/9.0.101/Sdks/Microsoft.NET.Sdk.WindowsDesktop/LICENSE.TXT
./usr/share/powershell/.store/powershell.linux.x64/7.5.0-preview.5/powershell.linux.x64/7.5.0-preview.5/tools/net9.0/any/Modules/Microsoft.PowerShell.PSResourceGet/LICENSE
./usr/share/powershell/.store/powershell.linux.x64/7.5.0-preview.5/powershell.linux.x64/7.5.0-preview.5/tools/net9.0/any/LICENSE.txt

$ docker run --rm mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim /bin/sh -c "find ./usr/share/dotnet | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/9.0.0/THIRD-PARTY-NOTICES.txt
./usr/share/dotnet/sdk/9.0.101/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT

$ docker run --rm mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim /bin/sh -c "find ./usr/share/dotnet ./usr/share/powershell | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/9.0.0/THIRD-PARTY-NOTICES.txt
./usr/share/dotnet/sdk/9.0.101/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
./usr/share/powershell/.store/powershell.linux.x64/7.5.0-preview.5/powershell.linux.x64/7.5.0-preview.5/tools/net9.0/any/ThirdPartyNotices.txt
```

## Distroless Images

The commands listed above won't work when targeting a distroless image since distroless images don't contain a shell by default.
Instead, you can copy the distroless image's filesystem into a another image that does contain a shell and inspect it from there.

This repo contains a [Dockerfile](./scripts/Dockerfile.distroless-wrapper) that takes a distroless container image as input.
The dockerfile copies the distroless image's entire filesystem into a non-distroless base image so that its contents can be inspected with shell scripts.

First, build the Dockerfile, specifying the distroless image tag you wish to inspect:

```console
$image="mcr.microsoft.com/dotnet/aspnet:9.0-azurelinux3.0-distroless"
docker build -t distroless-wrapper -f ./Dockerfile.distroless-wrapper --build-arg DISTROLESS_IMAGE=$image https://github.com/dotnet/dotnet-docker.git#main:documentation/scripts
```

Now that you've got the wrapper image, you can execute the [commands that are documented](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md).
The only difference here is that you'll need to target the wrapper image instead.

For example, instead of executing this command as documented:

```console
docker run --rm $image /bin/sh -c "find ./usr/share/dotnet | grep -i third"
```

You would actually execute this command using the distroless wrapper image:

```console
$ docker run --rm distroless-wrapper /bin/sh -c "find ./usr/share/dotnet | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/9.0.0/THIRD-PARTY-NOTICES.txt
```

## Appliance Images

### .NET Monitor

The [.NET Monitor image](../README.monitor.md) includes .NET Monitor in addition to ASP.NET Core, with associated licenses and third party notice files. The .NET Monitor images are based on distroless ASP.NET Core images. As such, using the instructions from [Distroless Images](#distroless-images) is necessary to gather the license and notice file paths.

First, build the wrapper Dockerfile, specifying the image tag you wish to inspect:

```console
docker build -t distroless-wrapper -f ./Dockerfile.distroless-wrapper --build-arg DISTROLESS_IMAGE=mcr.microsoft.com/dotnet/monitor:8 https://github.com/dotnet/dotnet-docker.git#main:documentation/scripts
```

Finally, execute the following to get license and notice file paths:

```console
$ docker run --rm distroless-wrapper /bin/sh -c "find ./distroless/usr/share/dotnet ./distroless/app | grep LICENSE"
./distroless/usr/share/dotnet/LICENSE.txt
./distroless/app/LICENSE.TXT
./distroless/app/extensions/AzureBlobStorage/LICENSE.TXT
./distroless/app/extensions/S3Storage/LICENSE.TXT
$ docker run --rm distroless-wrapper /bin/sh -c "find ./distroless/usr/share/dotnet ./distroless/app | grep -i third"
./distroless/usr/share/dotnet/ThirdPartyNotices.txt
./distroless/usr/share/dotnet/shared/Microsoft.AspNetCore.App/8.0.0/THIRD-PARTY-NOTICES.txt
./distroless/app/THIRD-PARTY-NOTICES.TXT
./distroless/app/extensions/AzureBlobStorage/THIRD-PARTY-NOTICES.TXT
./distroless/app/extensions/S3Storage/THIRD-PARTY-NOTICES.TXT
```
