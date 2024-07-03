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

You can see these components installed in the [runtime](https://github.com/dotnet/dotnet-docker/blob/4915d568024f581ee95608e3b93984f15434a5f3/src/runtime/8.0/bookworm-slim/amd64/Dockerfile#L6-L13), [aspnet](https://github.com/dotnet/dotnet-docker/blob/4915d568024f581ee95608e3b93984f15434a5f3/src/aspnet/8.0/bookworm-slim/amd64/Dockerfile#L6-L12), and [sdk](https://github.com/dotnet/dotnet-docker/blob/4915d568024f581ee95608e3b93984f15434a5f3/src/sdk/8.0/bookworm-slim/amd64/Dockerfile#L26-L48) Dockerfiles.

### .NET Runtime Image

The [.NET runtime image](https://hub.docker.com/_/microsoft-dotnet-runtime/) includes the .NET runtime, with an associated license and third party notice file.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:6.0-bullseye-slim /bin/sh -c "find ./usr/share/dotnet | grep LICENSE"
./usr/share/dotnet/LICENSE.txt
```

The license can be printed out, as follows.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:6.0-bullseye-slim cat ./usr/share/dotnet/LICENSE.txt
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
$ docker run --rm mcr.microsoft.com/dotnet/runtime:6.0-bullseye-slim /bin/sh -c "find ./usr/share/dotnet | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
```

### ASP.NET Core Image

The [ASP.NET Core image](https://hub.docker.com/_/microsoft-dotnet-aspnet/) includes ASP.NET Core in addition to .NET, with associated licenses and third party notice files.

```console
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim /bin/sh -c "find ./usr/share/dotnet | grep LICENSE"
./usr/share/dotnet/LICENSE.txt
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim /bin/sh -c "find ./usr/share/dotnet | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/6.0.0/THIRD-PARTY-NOTICES.txt
```

### .NET SDK Image

The [SDK image](https://hub.docker.com/_/microsoft-dotnet-sdk/) includes the .NET SDK, which includes various .NET components, with associated licenses and third party notice files.

```console
$ docker run --rm mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim /bin/sh -c "find ./usr/share/dotnet ./usr/share/powershell | grep LICENSE"
./usr/share/dotnet/LICENSE.txt
./usr/share/dotnet/sdk/6.0.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/LICENSE.TXT
./usr/share/powershell/.store/powershell.linux.x64/7.2.0-preview.10/powershell.linux.x64/7.2.0-preview.10/tools/net6.0/any/LICENSE.txt
$ docker run --rm mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim /bin/sh -c "find ./usr/share/dotnet | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/6.0.0/THIRD-PARTY-NOTICES.txt
./usr/share/dotnet/sdk/6.0.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
$ docker run --rm mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim /bin/sh -c "find ./usr/share/dotnet ./usr/share/powershell | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/6.0.0/THIRD-PARTY-NOTICES.txt
./usr/share/dotnet/sdk/6.0.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
./usr/share/powershell/.store/powershell.linux.x64/7.2.0-preview.10/powershell.linux.x64/7.2.0-preview.10/tools/net6.0/any/ThirdPartyNotices.txt
```

## Distroless Images

The commands listed above won't work when targeting a distroless image since distroless images don't contain a shell by default.
Instead, you can copy the distroless image's filesystem into a another image that does contain a shell and inspect it from there.

First, create a Dockerfile that will be used as the wrapper around the distroless container:

```Dockerfile
ARG DISTROLESS_IMAGE
FROM $DISTROLESS_IMAGE AS distroless

FROM mcr.microsoft.com/cbl-mariner/base/core:2.0

COPY --from=distroless / /distroless
```

Note the last instruction copies the entire contents of the distroless container's filesystem to the `/distroless` directory in the wrapper.
This will be the target location used when executing commands.

Next, build the Dockerfile, specifying the distroless image tag you wish to inspect:

```
docker build -t distroless-wrapper --build-arg DISTROLESS_IMAGE=mcr.microsoft.com/dotnet/aspnet:6.0-cbl-mariner2.0-distroless .
```

Now that you've got the wrapper image, you can execute the [commands that are documented](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md). The only difference here is that you'll need to target the wrapper image and adjust the target path.

For example, instead of executing this command as documented:

```
docker run --rm mcr.microsoft.com/dotnet/aspnet:6.0-cbl-mariner2.0-distroless /bin/sh -c "find ./usr/share/dotnet | grep -i third"
./usr/share/dotnet/ThirdPartyNotices.txt
```

You would actually execute this command to use the distroless wrapper image (note the difference in the image tag and path parameter):

```
docker run --rm distroless-wrapper /bin/sh -c "find ./distroless/usr/share/dotnet | grep -i third"
./distroless/usr/share/dotnet/ThirdPartyNotices.txt
```
