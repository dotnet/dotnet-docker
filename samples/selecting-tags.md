# Selecting .NET tags

There are many .NET images that are available that you can use. Some are very general and others are intended to satisfy more specific needs. Together, they satisfy a wide variety of scenarios.

You can use the referenced images and tags with the docker CLI, for example with `docker pull`, `docker run`, or as part of a FROM statement within a Dockerfile.

## .NET Docker repos

There are multiple [.NET Docker repos](../README.md) that expose various layers
of the .NET platform. They can be found under [Featured Repos](/README.md#featured-repos).
Other repos, including preview (nightly) .NET images, can be found under
[Related Repos](/README.md#related-repositories).

## Default Linux tags

The `runtime-deps`, `runtime`, `aspnet`, and `sdk` repos provide version-only
manifest list tags. These tags can sometimes be referred to as "convenience
tags".

* `11.0`
* `11.0.X`
* `latest`

For .NET 11, version-only tags refer to Ubuntu 26.04 (Resolute). The `latest`
tag follows the latest stable .NET release and does not necessarily refer to
.NET 11 while it is in preview.

For comparison, .NET 8 and .NET 9 version-only tags refer to Debian 12
(Bookworm), and .NET 10 version-only tags refer to Ubuntu 24.04 (Noble).

These convenience tags don't support Windows. See the [Multi-Platform
Tags](/documentation/supported-tags.md#multi-platform-tags) documentation for
more info.

## Targeting a specific operating system

If you want a specific operating system image, you should use a specific operating system tag. For .NET 11, we publish images for [Alpine](#alpine-linux), [Azure Linux](#azure-linux), [Ubuntu](#ubuntu), [Windows Nano Server](#nano-server), and [Windows Server Core](#windows-server-core). [Debian](#debian) images are available for .NET 8 and .NET 9.

The following tags demonstrate the pattern used to describe each operating system (using .NET 11.0 as the example):

* `11.0-alpine` (Latest Alpine)
* `11.0-azurelinux3.0` (Azure Linux 3.0)
* `11.0-resolute` (Ubuntu 26.04)
* `11.0-nanoserver-ltsc2025` (Nano Server LTSC 2025)
* `11.0-windowsservercore-ltsc2025` (Windows Server Core LTSC 2025)

For example, the following command will pull an Alpine image for your machine's architecture:

```console
docker pull mcr.microsoft.com/dotnet/runtime:11.0-alpine
```

### Linux

#### [Alpine Linux](https://alpinelinux.org/)

|                 |                                                                    |
|-----------------|--------------------------------------------------------------------|
| Releases        | [Every 6 months](https://alpinelinux.org/releases/)                |
| Security        | [Alpine Linux Security Tracker](https://security.alpinelinux.org/) |
| Support         | [Alpine Linux Community](https://alpinelinux.org/community/)       |
| Package format  | `.apk`                                                             |
| Package manager | `apk`                                                              |

| Supported .NET Features |                            |
|-------------------------|----------------------------|
| [Globalization]         | Invariant mode by default.<br>Globalization supported with `-extra` image variant. |
| [Distroless Images]     | Yes                        |

#### [Azure Linux](https://github.com/microsoft/azurelinux)

|                 |                                                                                            |
|-----------------|--------------------------------------------------------------------------------------------|
| Releases        | Approximately every 2 years                                                                |
| Security        | [Azure Linux Vulnerability Data](https://github.com/microsoft/AzureLinuxVulnerabilityData) |
| Support         | [Azure Linux GitHub repo](https://github.com/microsoft/azurelinux/issues)                  |
| Package format  | `.rpm`                                                                                     |
| Package manager | `tdnf`                                                                                     |

| Supported .NET Features |                                 |
|-------------------------|---------------------------------|
| [Globalization]         | Yes, for non-distroless images.<br>Globalization supported in distroless images with `-extra` image variant. |
| [Distroless Images]     | No                              |

#### [Debian](https://www.debian.org/)

* Stability-focused, extensive package repository.
* Full featured .NET images including many packages.

|                 |                                                                 |
|-----------------|-----------------------------------------------------------------|
| Releases        | [Approximately every 2 years](https://www.debian.org/releases/) |
| Security        | [Debian Security Information](https://www.debian.org/security/) |
| Support         | [Debian User Support](https://www.debian.org/support)           |
| Package format  | `.deb`                                                          |
| Package manager | `apt`/ `dpkg`                                                   |

| Supported .NET Features |     |
|-------------------------|-----|
| [Globalization]         | Yes |
| [Distroless Images]     | No  |

#### [Ubuntu](https://ubuntu.com/)

|                 |                                                                      |
|-----------------|----------------------------------------------------------------------|
| Releases        | [LTS releases every 2 years](https://ubuntu.com/about/release-cycle) |
| Security        | [Ubuntu Security Information](https://ubuntu.com/security/cves)      |
| Support         | [Ubuntu support](https://ubuntu.com/support)<br> [Launchpad](https://bugs.launchpad.net/ubuntu)<br> [Discourse](https://discourse.ubuntu.com/) |
| Package format  | `.deb`                                                               |
| Package manager | `apt`/ `dpkg`                                                        |

| Supported .NET Features |                                                            |
|-------------------------|------------------------------------------------------------|
| [Globalization]           | Yes, for non-Chiseled images.<br>Globalization supported in Chiseled images with `-extra` image variant. |
| [Distroless Images]     | Yes - [Ubuntu Chiseled](/documentation/ubuntu-chiseled.md) |

[Globalization]: ./enable-globalization.md
[Distroless Images]: /documentation/distroless.md

### Windows

#### [Nano Server](https://docs.microsoft.com/virtualization/windowscontainers/manage-containers/container-base-images)

* Small, minimalistic version of Windows.
* Good option for new application development.

#### [Windows Server Core](https://docs.microsoft.com/virtualization/windowscontainers/manage-containers/container-base-images)

* Significantly larger than Nano Server.
* Feature-rich; contains additional components that are missing from Nano Server.
* Supports:
  * IIS
  * .NET Framework
* Good option for "lifting and shifting" Windows Server apps.

## Targeting a specific processor type

If you want an image of a specific processor type, you should use a specific processor architecture tag. We publish tags for x64, ARM64 and ARM32. Linux tags without an architecture suffix select the architecture automatically. Windows images support x64 only.

The following tags demonstrate the pattern used to describe each processor, using the same operating systems listed above.

### x64

* `11.0-alpine-amd64`
* `11.0-resolute-amd64`
* `11.0-nanoserver-ltsc2025`
* `11.0-windowsservercore-ltsc2025`

### ARM64

* `11.0-alpine-arm64v8`
* `11.0-resolute-arm64v8`

### ARM32

* `11.0-alpine-arm32v7`
* `11.0-resolute-arm32v7`

## Matching SDK and Runtime images

For .NET 11, we offer Linux images based on Alpine, Azure Linux, and Ubuntu. People (and organizations) choose each of these distros for different reasons. Ubuntu 26.04 (Resolute) is the default distro selected by the `11.0` tag.

For multi-stage Dockerfiles, there are typically at least two tags referenced, an SDK and a runtime tag. You may want to make a conscious choice to make the distros match for those two tags. If you are targeting Ubuntu Resolute, you can use `11.0` for both stages. If you are targeting another distro for your final runtime image (`aspnet` or `runtime`), then you have a choice, as follows:

* Target a multi-platform tag for the SDK (like `11.0`) to make the SDK stage simple and to enable your Dockerfile to be built in multiple environments (with different processor architectures). This is what most of the samples Dockerfiles in this repo do.
* Match SDK and runtime tags to ensure that you are using the same OS (with the associated shell and commands) and package manager for all stages within a Dockerfile.

## Building for your production environment

Each container image is generated for a specific processor architecture and operating system (Linux or Windows). It is important to construct each Dockerfile so that it will produce the image type you need. Docker [multi-platform](https://www.docker.com/blog/docker-official-images-now-multi-platform/) tags can confuse the situation, since they work on multiple platforms (hence the name) and may produce images that map to your build host and not your production environment.

For multi-stage Dockerfiles, there are typically at least two tags referenced, an SDK and a runtime tag. It is fine to use a multi-platform tag for the SDK. That's the pattern used for .NET samples. You will pull an SDK image that works on your machine. It is important to define a .NET runtime (`runtime-deps`, `runtime`, or `aspnet`) that matches your production environment.

Linux containers are flexible. As long as the processor architecture matches, you can run Alpine, Azure Linux, and Ubuntu images in environments that support Linux containers and meet the images' kernel requirements. [Windows images are more restricted](https://docs.microsoft.com/virtualization/windowscontainers/deploy-containers/version-compatibility). You cannot load containers for newer Windows versions on older hosts. For the best experience, the Windows container version should match the host Windows version.
