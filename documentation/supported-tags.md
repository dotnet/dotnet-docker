# .NET Container Tags -- Patterns and Policies

This document describes the tagging patterns and policies that are used for the official .NET container images. .NET tags are intended to closely match the tagging patterns used by [Official Images on Docker Hub](https://hub.docker.com/search?q=&type=image&image_filter=official). Please [log an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose) if you encounter problems using .NET images or applying these tagging patterns.

Complete tag lists:

- [runtime-deps](../README.runtime-deps.md#full-tag-listing)
- [runtime](../README.runtime.md#full-tag-listing)
- [aspnet](../README.aspnet.md#full-tag-listing)
- [sdk](../README.sdk.md#full-tag-listing)
- [monitor](../README.monitor.md#full-tag-listing)
- [samples](../README.samples.md#full-tag-listing)
- [Microsoft Artifact Registry](https://mcr.microsoft.com/en-us/catalog?search=dotnet/)

The terms "fixed version" and "floating version" are used throughout. They are defined in [Tag policies](#tag-policies).

## .NET Versions

All .NET container images have both "fixed version" and "floating version" tags.
Floating version tags will always reference the latest version of a specific .NET major version, while fixed version tags will always only reference a specific patch version.
For all of the tags below, `<.NET Version>` can be substituted for either `<Major.Minor>` or `<Major.Minor.Patch>`, for example: `6.0` or `6.0.12`.

## Image Variants

.NET Container Images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](./image-variants.md) contains a summary of the image variants and their use-cases.

## Single-platform tags

These "fixed version" tags reference an image with a specific .NET version for a specific operating system and architecture.

- `<.NET Version>-<OS>-<Architecture>`
- `<.NET Version>-<OS>-<variant>-<Architecture>`
- `<.NET Version>-<OS>-<Architecture>`
- `<.NET Version>-<OS>-<variant>-<Architecture>`

Examples:
- `6.0.12-jammy-amd64`
- `6.0.12-jammy-chiseled-arm64v8`
- `6.0.12-nanoserver-1809`
- `8.0.0-alpine3.18-extra-arm64v8`
- `8.0.0-bullseye-slim-arm32v7`

## Multi-platform tags

These tags reference images for [multiple platforms](https://docs.docker.com/build/building/multi-platform/).

- `<.NET Version>` - This version-only floating tag refers to the latest Debian version available at the .NET Major Version's release.
- `<.NET Version>-<OS>`
- `<.NET Version>-<OS>-<variant>`

Examples:
- `8.0`
- `8.0-jammy`
- `8.0-jammy-chiseled-extra`

**Note:** Since .NET 8, these multi-platform tags **specifically exclude all Windows versions** due to `containerd`'s platform matching algorithm for Windows hosts.
Please see [#4492 (Switch multi-platform tags to Linux only)](https://github.com/dotnet/dotnet-docker/issues/4492) for more context.
If you are using Windows, you will need to explicitly specify an OS Version with a single-platform tag like so:
```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-ltsc2022
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2019
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022
```

### Alpine Floating Tags

These "floating version" tags reference an image with a specific `Major.Minor` (with latest patch) .NET version, for the latest Alpine version, while architecture will be chosen based on the requesting environment.

Examples:

- `8.0-alpine`
- `6.0.12-alpine`

Notes:

- New versions of Alpine will be published with version-specific tags (e.g. `8.0-alpine3.18`).
- Floating tag (e.g. `8.0-alpine`) will be updated with the new Alpine version a month later.
- Tag changes will be [announced](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) so that users know when the tags they want are available.

### Latest

These "floating version" `latest` tag references an image with the latest `Major.Minor.Patch` .NET version, while operating system and architecture will be chosen based on the requesting environment.

Notes:

- The `latest` tag references the latest stable release.
- In the `nightly` image repo, it may reference the latest preview release.

## Tag policies

The following policies are used for the tag patterns we use.

### Fixed version tags

"Fixed version" tags reference an image with a specific `Major.Minor.Patch` .NET version.

Examples:

- `6.0.12`
- `8.0.0-alpine3.18`

Notes:

- These tags are considered _fixed tags_ since they reference a specific .NET patch version.
- They are updated in response to base image updates (like a Debian base image) for the supported life of the image (typically one month).
- The .NET components within the image will not be updated.
- In the rare event that .NET components are updated before the next regular .NET service release, then a new image with a `-1` tag will be created. The same practice will repeat itself if necessary (with `-2` and then `-3` tags).

### Floating version tags

"Floating version" tags references an image with a specific `Major.Minor` .NET version, but float on patch updates.

Examples:

- `6.0`
- `8.0-alpine3.18`

Notes:

- These tags are considered _floating tags_ since they do not reference a specific .NET patch version.
- They are updated in response to base image updates (like a Debian base image) for the supported life of the .NET release.
- The .NET components within the image will be updated, which typically occurs on Patch Tuesday.

### OS tags and base image updates

Version-specific operating system tags reference an image with a specific OS version, but floats on OS patch updates. See [Supported Platforms](supported-platforms.md#operating-systems) for the list of supported operating systems.

Examples:

- `6.0-jammy`
- `8.0-alpine3.18`

Notes:

- These tags are updated in response to base image updates (like an Ubuntu base image) for the supported life of the .NET release.
- Digest pinning is required to request a specific patch of an operating system (e.g. `mcr.microsoft.com/dotnet/runtime@sha256:4d3d5a5131a0621509ab8a75f52955f2d0150972b5c5fb918e2e59d4cb9a9823`).
- For [Debian](https://en.wikipedia.org/wiki/Debian_version_history) and [Ubuntu](https://en.wikipedia.org/wiki/Ubuntu_version_history) images, release codenames are used instead of version numbers.

### Windows tags

For Windows, `amd64` is the only architecture supported and is excluded from the tag name.

## Tag Lifecycle

Each tag will be supported for the lifetime of the .NET and OS version referenced by the tag, unless further restricted according to [platform support policy](supported-platforms.md).

When an OS version reaches End-of-Life (EOL), its tags will no longer be maintained.

When a .NET version reaches EOL, its tags will continue to be maintained (rebuilt for base image updates) until the next .NET servicing date (typically on "Patch Tuesday", the 2nd Tuesday of the month).

Once a tag is no longer maintained, it will be considered unsupported, will no longer be updated. Unsupported tags will continue to exist in the container registry to prevent breaking any references to it.

## Policy Changes

In the event that a change is needed to the tagging patterns used, all tags for the previous pattern will continue to be supported for their original lifetime. They will however be removed from the documentation. [Announcements](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) will be posted when any tagging policy changes are made.
