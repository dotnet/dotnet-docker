# Published Image Artifact Details

.NET Core container images contain multiple software components, each built from different source and with different licensing. It is important that users of these images can determine the license requirements of these images and have a link back to the source for pedigree purposes.

This document is intended to be informative, primarily defining patterns for discovering license and pedigree information and describing expectations (whether the desired information can be found). It does not list all of the license and pedigree information you might want. This information can change at any time, which also demonstrates why using the documented patterns is important.

The combination of [.NET Core Dockerfiles](https://github.com/dotnet/dotnet-docker) and [.NET Core container images](https://hub.docker.com/_/microsoft-dotnet-core/) are the source and binary artifacts that are used in this document and intended to be used to determine the compliance of .NET Core containers.

This document describes the licensing and source pedigree of the following components:

* Base images (from Docker Hub)
* Packages (from Linux package managers)
* .NET Core and other components (acquired as .tar.gz/.zip files)

## Base Images

.NET Core images are provided for a set of Linux distros, delivered via [MCR](https://azure.microsoft.com/blog/microsoft-syndicates-container-catalog/) (and advertised on Docker Hub). The Linux distros are acquired from Docker Hub as part of building the .NET Core images. The following base images are delivered as a component of .NET Core images.

* [alpine](https://hub.docker.com/_/alpine)
* [debian](https://hub.docker.com/_/debian)
* [ubuntu](https://hub.docker.com/_/ubuntu)
* [buildpack-deps](https://hub.docker.com/_/buildpack-deps)

.NET Core is dependent on those images, and the teams that build them, to satisfactorily describe the licensing and pedigree of these images.

### Retrieving License Information From Base Images

The copyright practices vary across the base images that .NET Core depends on. This is demonstrated via the following introspection technique, for each of the supported distros.

Note: The output of the commands is intentionally cut-off for purposes of brevity. The output you see below is intended to demonstrate the pattern, and you can repeat it to view all current license information for the base image of your choosing.

#### Alpine

Licensing information is not present in Alpine images, as is demonstrated below.  The `-c` grep argument displays the number of copyrights found.

```console
$ docker run --rm alpine:3.11 find . | grep -c copyright
0
```

#### Debian

Licensing information is present in Debian images, as is demonstrated below.

```console
$ docker run --rm debian:buster-slim find . | grep copyright
./usr/share/doc/libpam-runtime/copyright
./usr/share/doc/debian-archive-keyring/copyright
./usr/share/doc/libss2/copyright
./usr/share/doc/libdebconfclient0/copyright
./usr/share/doc/bsdutils/copyright
./usr/share/doc/ncurses-base/copyright
./usr/share/doc/libudev1/copyright
./usr/share/doc/libc6/copyright
./usr/share/doc/debianutils/copyright
./usr/share/doc/liblzma5/copyright
...
```

You can then print any of these copyright files, as demonstrated below:

```console
> docker run --rm debian:buster-slim cat ./usr/share/doc/apt/copyright
Apt is copyright 1997, 1998, 1999 Jason Gunthorpe and others.
Apt is currently developed by APT Development Team <deity@lists.debian.org>.

License: GPLv2+

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.

See /usr/share/common-licenses/GPL-2, or
<http://www.gnu.org/copyleft/gpl.txt> for the terms of the latest version
of the GNU General Public License.
```

#### Ubuntu

Licensing information is present in Ubuntu images, as is demonstrated below.

```console
> docker run --rm ubuntu:bionic find . | grep copyright
./usr/share/doc/libpam-runtime/copyright
./usr/share/doc/sensible-utils/copyright
./usr/share/doc/libss2/copyright
./usr/share/doc/libdebconfclient0/copyright
./usr/share/doc/bsdutils/copyright
./usr/share/doc/ncurses-base/copyright
./usr/share/doc/libudev1/copyright
./usr/share/doc/libc6/copyright
./usr/share/doc/debianutils/copyright
...
```

You can then print any of these copyright files, as demonstrated below:

```console
> docker run --rm ubuntu:bionic cat ./usr/share/doc/apt/copyright
Apt is copyright 1997, 1998, 1999 Jason Gunthorpe and others.
Apt is currently developed by APT Development Team <deity@lists.debian.org>.

License: GPLv2+

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.

See /usr/share/common-licenses/GPL-2, or
<http://www.gnu.org/copyleft/gpl.txt> for the terms of the latest version
of the GNU General Public License.
```

### Retrieving Base Image Pedigree Details

.NET Core relies on the official Docker image repositories to provide detailed information about the images.  This information includes layer information, source code, license information, etc.  This information is stored in the [Official Images "Extended Information" repository](https://github.com/docker-library/repo-info) and is split into two types.

* remote
  * gathered from the Docker Hub/Registry API
  * image digests/blobs, transfer sizes, image metadata, etc.
* local
  * inspected from the image on-disk after it is pulled
  * installed packages, creation date, architecture, environment variables, detected licenses, etc.

An abbreviated copy the ["extended information" for the Debian repo](https://github.com/docker-library/repo-info/tree/master/repos/debian) is included below to give you an idea of the details included.

#### `debian:buster-slim` [remote](https://github.com/docker-library/repo-info/tree/master/repos/debian/remote)

***

## `debian:buster-slim`

```console
$ docker pull debian@sha256:e4c1417236abc57971755ca2bfccd546cbca45b33daf66001a5addae4bf78517
```

* Manifest MIME: `application/vnd.docker.distribution.manifest.list.v2+json`
* Platforms:
  * linux; amd64
  * linux; arm variant v5
  * linux; arm variant v7
  * linux; arm64 variant v8
  * linux; 386
  * linux; ppc64le
  * linux; s390x

### `debian:buster-slim` - linux; amd64

```console
$ docker pull debian@sha256:0c679627b3a61b2e3ee902ec224b0505839bc2ad76d99530e5f0566e47ac8400
```

* Docker Version: 18.06.1-ce
* Manifest MIME: `application/vnd.docker.distribution.manifest.v2+json`
* Total Size: **27.1 MB (27092274 bytes)**  
* (compressed transfer size, not on-disk size)
* Image ID: `sha256:e1af56d072b8d93fce4b566f4bf76311108dbbbe952b12a85418bd32c2fcdda7`
* Default Command: `["bash"]`

```dockerfile
# Sat, 28 Dec 2019 04:21:22 GMT
ADD file:04caaf303199c81ff1a94e2e39d5096f9d02b73294b82758e5bc6e23aff94272 in / 
# Sat, 28 Dec 2019 04:21:23 GMT
CMD ["bash"]
```

* Layers:
  * `sha256:8ec398bc03560e0fa56440e96da307cdf0b1ad153f459b52bca53ae7ddb8236d`  
  * Last Modified: Sat, 28 Dec 2019 04:25:53 GMT  
  * Size: 27.1 MB (27092274 bytes)  
  * MIME: application/vnd.docker.image.rootfs.diff.tar.gzip

...

#### `debian:buster-slim` [local](https://github.com/docker-library/repo-info/tree/master/repos/debian/local)

***

# `debian:buster-slim`

## Docker Metadata

* Image ID: `sha256:e1af56d072b8d93fce4b566f4bf76311108dbbbe952b12a85418bd32c2fcdda7`
* Created: `2019-12-28T04:21:23.037912523Z`
* Virtual Size: ~ 69.21 Mb  
  (total size of all layers on-disk)
* Arch: `linux`/`amd64`
* Command: `["bash"]`
* Environment:
  * `PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin`

## `dpkg` (`.deb`-based packages)

### `dpkg` source package: `acl=2.2.53-4`

Binary Packages:

* `libacl1:amd64=2.2.53-4`

Licenses: (parsed from: `/usr/share/doc/libacl1/copyright`)

* `GPL-2`
* `GPL-2+`
* `LGPL-2+`
* `LGPL-2.1`

Source:

```console
$ apt-get source -qq --print-uris acl=2.2.53-4
'http://deb.debian.org/debian/pool/main/a/acl/acl_2.2.53-4.dsc' acl_2.2.53-4.dsc 2330 SHA256:532eb4029659db74e6625adc2bd277144f33c92cb0603272d61693b069896a85
'http://deb.debian.org/debian/pool/main/a/acl/acl_2.2.53.orig.tar.gz' acl_2.2.53.orig.tar.gz 524300 SHA256:06be9865c6f418d851ff4494e12406568353b891ffe1f596b34693c387af26c7
'http://deb.debian.org/debian/pool/main/a/acl/acl_2.2.53.orig.tar.gz.asc' acl_2.2.53.orig.tar.gz.asc 833 SHA256:06849bece0b56a6a7269173abe101cff223bb9346d74027a3cd5ff80914abf4b
'http://deb.debian.org/debian/pool/main/a/acl/acl_2.2.53-4.debian.tar.gz.asc' acl_2.2.53-4.debian.tar.xz 18572 SHA256:3e6571adea4886a9549bdc2323d5c55ee8f7dafb6a204513111d5943d2776dd8
```

Other potentially useful URLs:

* https://sources.debian.net/src/acl/2.2.53-4/ (for browsing the source)
* https://sources.debian.net/src/acl/2.2.53-4/debian/copyright/ (for direct copyright/license information)
* http://snapshot.debian.org/package/acl/2.2.53-4/ (for access to the source package after it no longer exists in the archive)

### `dpkg` source package: `adduser=3.118`

...

## Packages

.NET Core images install a set of packages (from the associated package manager for the distro).

You can see an example of these packages in this Dockerfile, which is copied below (see `RUN apt-get` instruction).

```Dockerfile
FROM debian:buster-slim

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
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

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
```

The pattern for [Retrieving License Information From Base Images](###retrieving-license-information-from-base-images) can be repeated for packages that are added in .NET Core images (above and beyond the base image). The first package listed in the Dockerfile is `ca-certificates`, which will be used to demonstrate the pattern. The referenced Dockerfile is for an imagine in the [runtime-deps repo](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps), as you will see demonstrated in the example below.

There is no guarantee that the requested package is not present in the base image, but it typically will not be (hence why it is explicitly installed), as is demonstrated below.

```console
$ docker run --rm debian:buster-slim find . | grep copyright | grep -c ca-certificates
0
$ docker run --rm mcr.microsoft.com/dotnet/core/runtime-deps:3.1-buster-slim find . | grep copyright | grep ca-certificates
./usr/share/doc/ca-certificates/copyright
./usr/share/doc/ca-certificates/examples/ca-certificates-local/debian/copyright
```

This pattern can be replicated for all `debian` and `ubuntu` based images. It will not work for `alpine` images as demonstrated below, because they do not carry license information.

```console
$ docker run --rm mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine3.10 find . | grep copyright | grep -c ca-certificates
0
```

You can then print any of these copyright files, as demonstrated below:

```console
> docker run --rm mcr.microsoft.com/dotnet/core/runtime-deps:3.1-buster-slim cat ./usr/share/doc/ca-certificates/copyright
Format: https://www.debian.org/doc/packaging-manuals/copyright-format/1.0/
Source: http://ftp.debian.org/debian/pool/main/c/ca-certificates/

Files: debian/*
       examples/*
       Makefile
       mozilla/*
       sbin/*
Copyright: 2003 Fumitoshi UKAI <ukai@debian.or.jp>
           2009 Philipp Kern <pkern@debian.org>
           2011 Michael Shuler <michael@pbandjelly.org>
           Various Debian Contributors
License: GPL-2+
 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.
 .
 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.
 .
 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301,
 USA.
 .
 On Debian GNU/Linux systems, the complete text of the GNU General Public
 License can be found in '/usr/share/common-licenses/GPL-2'.

...
```

## .NET Core and Other Components

.NET Core and other components are carried in .NET Core images, all of which are from Microsoft or the .NET Foundation. The following list describes the complete set of other software (beyond base images and packages):

* .NET Core Runtime
* ASP.NET Core Runtime
* .NET Core SDK
* PowerShell

You can see these components installed in the [runtime](https://github.com/dotnet/dotnet-docker/blob/d4a9e799d047f3e86cd2730f48b689c371d38480/3.1/runtime/buster-slim/amd64/Dockerfile#L9-L17), [aspnet](https://github.com/dotnet/dotnet-docker/blob/d4a9e799d047f3e86cd2730f48b689c371d38480/3.1/aspnet/buster-slim/amd64/Dockerfile#L4-10), and [sdk](https://github.com/dotnet/dotnet-docker/blob/d4a9e799d047f3e86cd2730f48b689c371d38480/3.1/sdk/buster/amd64/Dockerfile#L26-L49) Dockefiles.

You can discover the licenses for these components using the following pattern.

### .NET Core Runtime Image

The [.NET Core runtime image](https://hub.docker.com/_/microsoft-dotnet-core-runtime/) includes the .NET Core runtime, with an associated license and third party notice file.

```console
$ docker run --rm mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim find . | grep LICENSE
./usr/share/dotnet/LICENSE.txt
```

The license can be printed out, as follows.

```console
$ docker run --rm mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim cat ./usr/share/dotnet/LICENSE.txt
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
$ docker run --rm mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim find . | grep -i third
./usr/share/dotnet/ThirdPartyNotices.txt
```

### .NET Core ASP.NET Image

The [ASP.NET image](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/) includes ASP.NET Core in addition to .NET Core, with associated licenses and third party notice files.

```console
% docker run --rm mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim find . | grep LICENSE
./usr/share/dotnet/LICENSE.txt
% docker run --rm mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim find . | grep -i third
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/3.1.0/THIRD-PARTY-NOTICES.txt
```

### .NET Core SDK Image

The [SDK image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/) includes the .NET Core SDK, which includes various .NET Core components, with associated licenses and third party notice files.

Note: The SDK image is based on [`buildpack-deps`](https://hub.docker.com/_/buildpack-deps), which includes components that we distribute but not use (like Python).

```console
$ docker run --rm mcr.microsoft.com/dotnet/core/sdk:3.1-buster find . | grep LICENSE
./usr/lib/python2.7/LICENSE.txt
./usr/share/doc/git/contrib/persistent-https/LICENSE
./usr/share/powershell/.store/powershell.linux.x64/7.0.0-preview.6/powershell.linux.x64/7.0.0-preview.6/tools/netcoreapp3.1/any/LICENSE.txt
./usr/share/dotnet/sdk/3.1.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/LICENSE.TXT
./usr/share/dotnet/LICENSE.txt
$ docker run --rm mcr.microsoft.com/dotnet/core/sdk:3.1-buster find . | grep -i third | grep dotnet
./usr/share/dotnet/sdk/3.1.100/Sdks/Microsoft.NET.Sdk.WindowsDesktop/THIRD-PARTY-NOTICES.TXT
./usr/share/dotnet/ThirdPartyNotices.txt
./usr/share/dotnet/shared/Microsoft.AspNetCore.App/3.1.0/THIRD-PARTY-NOTICES.txt
$ docker run --rm mcr.microsoft.com/dotnet/core/sdk:3.1-buster find . | grep -i third | grep powershell
./usr/share/powershell/.store/powershell.linux.x64/7.0.0-preview.6/powershell.linux.x64/7.0.0-preview.6/tools/netcoreapp3.1/any/ThirdPartyNotices.txt
```

Python also provides third party notice files, via `buildpack-deps`. You will see those if you use the following (unfiltered) pattern:

```console
$ docker run --rm mcr.microsoft.com/dotnet/core/sdk:3.1-buster find . | grep -i third
./usr/lib/python2.7/dist-packages/mercurial/thirdparty
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/__init__.py
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/zope
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/zope/__init__.py
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/zope/interface
./usr/lib/python2.7/dist-packages/mercurial/thirdparty/zope/interface/exceptions.py
...
```
