# Determining License and Source Pedigree for .NET Core Container Images

This document is intended to be complimentary to the [Instructions for Finding Linux Legal Metadata](https://aka.ms/mcr/osslinuxmetadata).  This document describes how to interrogate the .NET Core images to determine licensing and version information for .NET Core and other components.

## .NET Core and Other Components

.NET Core and other components are carried in .NET Core images, all of which are from Microsoft or the .NET Foundation. The following list describes the complete set of other software (beyond base images and packages):

* .NET Core Runtime
* ASP.NET Core Runtime
* .NET Core SDK
* PowerShell

You can see these components installed in the [runtime](https://github.com/dotnet/dotnet-docker/blob/d4a9e799d047f3e86cd2730f48b689c371d38480/3.1/runtime/buster-slim/amd64/Dockerfile#L9-L17), [aspnet](https://github.com/dotnet/dotnet-docker/blob/d4a9e799d047f3e86cd2730f48b689c371d38480/3.1/aspnet/buster-slim/amd64/Dockerfile#L4-10), and [sdk](https://github.com/dotnet/dotnet-docker/blob/d4a9e799d047f3e86cd2730f48b689c371d38480/3.1/sdk/buster/amd64/Dockerfile#L26-L49) Dockerfiles.

You can discover the licenses for these components using the following pattern.

### .NET Core Runtime Image

The [.NET Core runtime image](https://hub.docker.com/_/microsoft-dotnet-runtime/) includes the .NET Core runtime, with an associated license and third party notice file.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:3.1-buster-slim find ./usr/share/dotnet | grep LICENSE
./usr/share/dotnet/LICENSE.txt
```

The license can be printed out, as follows.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:3.1-buster-slim cat ./usr/share/dotnet/LICENSE.txt
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
$ docker run --rm mcr.microsoft.com/dotnet/runtime:3.1-buster-slim find ./usr/share/dotnet | grep -i third
./usr/share/dotnet/ThirdPartyNotices.txt
```

### .NET Core ASP.NET Image

The [ASP.NET image](https://hub.docker.com/_/microsoft-dotnet-aspnet/) includes ASP.NET Core in addition to .NET Core, with associated licenses and third party notice files.

```console
% docker run --rm mcr.microsoft.com/dotnet/aspnet:3.1-buster-slim find ./usr/share/dotnet | grep LICENSE
./usr/share/dotnet/LICENSE.txt
% docker run --rm mcr.microsoft.com/dotnet/aspnet:3.1-buster-slim find ./usr/share/dotnet | grep -i third
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/3.1.0/THIRD-PARTY-NOTICES.txt
```

### .NET Core SDK Image

The [SDK image](https://hub.docker.com/_/microsoft-dotnet-sdk/) includes the .NET Core SDK, which includes various .NET Core components, with associated licenses and third party notice files.

Note: The SDK image is based on [`buildpack-deps`](https://hub.docker.com/_/buildpack-deps), which includes components that we distribute but do not use (like Python).

```console
$ docker run --rm mcr.microsoft.com/dotnet/sdk:3.1-buster find ./usr/share/dotnet ./usr/share/powershell | grep LICENSE
./usr/share/dotnet/sdk/3.1.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/LICENSE.TXT
./usr/share/dotnet/LICENSE.txt
./usr/share/powershell/.store/powershell.linux.x64/7.0.0-preview.6/powershell.linux.x64/7.0.0-preview.6/tools/netcoreapp3.1/any/LICENSE.txt
$ docker run --rm mcr.microsoft.com/dotnet/sdk:3.1-buster find ./usr/share/dotnet | grep -i third
./usr/share/dotnet/sdk/3.1.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/3.1.0/THIRD-PARTY-NOTICES.txt
$ docker run --rm mcr.microsoft.com/dotnet/sdk:3.1-buster find ./usr/share/dotnet ./usr/share/powershell | grep -i third
./usr/share/powershell/.store/powershell.linux.x64/7.0.0-preview.6/powershell.linux.x64/7.0.0-preview.6/tools/netcoreapp3.1/any/ThirdPartyNotices.txt
```

Python also provides third party notice files, via `buildpack-deps`. You will see those if you use the following (unfiltered) pattern:

```console
% docker run --rm mcr.microsoft.com/dotnet/sdk:3.1 find . | grep -i third
./usr/share/powershell/.store/powershell.linux.x64/7.0.0-preview.6/powershell.linux.x64/7.0.0-preview.6/tools/netcoreapp3.1/any/ThirdPartyNotices.txt
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/sdk/3.1.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/3.1.0/THIRD-PARTY-NOTICES.txt
./usr/lib/python2.7/dist-packages/mercurial/thirdparty
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/cbor
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/cbor/__init__.py
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/cbor/cbor2
...
```
