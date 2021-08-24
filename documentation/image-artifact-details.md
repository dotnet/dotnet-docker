# Determining License and Source Pedigree for .NET Container Images

This document is intended to be complimentary to the [Instructions for Finding Linux Legal Metadata](https://aka.ms/mcr/osslinuxmetadata).  This document describes how to interrogate the .NET images to determine licensing and version information for .NET and other components.

## .NET and Other Components

.NET and other components are carried in .NET images, all of which are from Microsoft or the .NET Foundation. The following list describes the complete set of other software (beyond base images and packages):

* .NET Runtime
* ASP.NET Core Runtime
* .NET SDK
* PowerShell

You can see these components installed in the [runtime](https://github.com/dotnet/dotnet-docker/blob/d4a9e799d047f3e86cd2730f48b689c371d38480/3.1/runtime/buster-slim/amd64/Dockerfile#L9-L17), [aspnet](https://github.com/dotnet/dotnet-docker/blob/d4a9e799d047f3e86cd2730f48b689c371d38480/3.1/aspnet/buster-slim/amd64/Dockerfile#L4-10), and [sdk](https://github.com/dotnet/dotnet-docker/blob/d4a9e799d047f3e86cd2730f48b689c371d38480/3.1/sdk/buster/amd64/Dockerfile#L26-L49) Dockerfiles.

You can discover the licenses for these components using the following pattern.

### .NET Runtime Image

The [.NET runtime image](https://hub.docker.com/_/microsoft-dotnet-runtime/) includes the .NET runtime, with an associated license and third party notice file.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:5.0-buster-slim find ./usr/share/dotnet | grep LICENSE
./usr/share/dotnet/LICENSE.txt
```

The license can be printed out, as follows.

```console
$ docker run --rm mcr.microsoft.com/dotnet/runtime:5.0-buster-slim cat ./usr/share/dotnet/LICENSE.txt
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
$ docker run --rm mcr.microsoft.com/dotnet/runtime:5.0-buster-slim find ./usr/share/dotnet | grep -i third
./usr/share/dotnet/ThirdPartyNotices.txt
```

### ASP.NET Core Image

The [ASP.NET Core image](https://hub.docker.com/_/microsoft-dotnet-aspnet/) includes ASP.NET Core in addition to .NET, with associated licenses and third party notice files.

```console
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim find ./usr/share/dotnet | grep LICENSE
./usr/share/dotnet/LICENSE.txt
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim find ./usr/share/dotnet | grep -i third
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/5.0.0/THIRD-PARTY-NOTICES.txt
```

### .NET SDK Image

The [SDK image](https://hub.docker.com/_/microsoft-dotnet-sdk/) includes the .NET SDK, which includes various .NET components, with associated licenses and third party notice files.

```console
$ docker run --rm mcr.microsoft.com/dotnet/sdk:5.0-buster-slim find ./usr/share/dotnet ./usr/share/powershell | grep LICENSE
./usr/share/dotnet/LICENSE.txt
./usr/share/dotnet/sdk/5.0.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/LICENSE.TXT
./usr/share/powershell/.store/powershell.linux.x64/7.1.0-rc.2/powershell.linux.x64/7.1.0-rc.2/tools/net5.0/any/LICENSE.txt
$ docker run --rm mcr.microsoft.com/dotnet/sdk:5.0-buster-slim find ./usr/share/dotnet | grep -i third
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/5.0.0/THIRD-PARTY-NOTICES.txt
./usr/share/dotnet/sdk/5.0.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
$ docker run --rm mcr.microsoft.com/dotnet/sdk:5.0-buster-slim find ./usr/share/dotnet ./usr/share/powershell | grep -i third
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/5.0.0/THIRD-PARTY-NOTICES.txt
./usr/share/dotnet/sdk/5.0.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
./usr/share/powershell/.store/powershell.linux.x64/7.1.0-rc.2/powershell.linux.x64/7.1.0-rc.2/tools/net5.0/any/ThirdPartyNotices.txt
```

Note: In .NET Core 3.1, the SDK image is based on [`buildpack-deps`](https://hub.docker.com/_/buildpack-deps), which includes components that we distribute but do not use (like Python). Python also provides third party notice files, via `buildpack-deps`. You will see those if you use the following (unfiltered) pattern:

```console
$ docker run --rm mcr.microsoft.com/dotnet/sdk:3.1 find . | grep -i third
./usr/share/powershell/.store/powershell.linux.x64/7.0.3/powershell.linux.x64/7.0.3/tools/netcoreapp3.1/any/ThirdPartyNotices.txt
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/sdk/3.1.403/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/3.1.9/THIRD-PARTY-NOTICES.txt
./usr/lib/python2.7/dist-packages/mercurial/thirdparty
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/cbor
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/cbor/__init__.py
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/cbor/cbor2
...
```
