# .NET Container Tags -- Patterns and Policies

This document describes the tagging patterns and policies that are used for the official .NET container images. .NET tags are intended to closely match the tagging patterns used by [Official Images on Docker Hub](https://hub.docker.com/search?q=&type=image&image_filter=official). Please [log an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose) if you encounter problems using .NET images or applying these tagging patterns.

Complete tag list:

- [runtime-deps](../README.runtime-deps.md#full-tag-listing)
- [runtime](../README.runtime.md#full-tag-listing)
- [aspnet](../README.aspnet.md#full-tag-listing)
- [sdk](../README.sdk.md#full-tag-listing)
- [monitor](../README.monitor.md#full-tag-listing)
- [samples](../README.samples.md#full-tag-listing)
- [Microsoft Artifact Registry](https://mcr.microsoft.com/en-us/catalog?search=dotnet/)

## Single-platform Tags

These tags reference an image for a single platform (e.g. "Linux Arm64" or "Windows x64").

### `<Major.Minor.Patch .NET Version>-<OS>-<Architecture>`

Examples:

- `6.0.12-jammy-amd64`
- `6.0.12-jammy-arm64v8`
- `6.0.12-nanoserver-1809`
- `7.0.2-alpine3.17-arm64v8`
- `7.0.2-bullseye-slim-arm32v7`

### `<Major.Minor .NET Version>-<OS>-<Architecture>`

Examples:

- `6.0-jammy-arm64v8`
- `6.0-jammy-amd64`
- `6.0-nanoserver-1809`
- `7.0-alpine3.17-arm64v8`
- `7.0-bullseye-slim-arm32v7`

## Multi-platform images

These tags reference images for [multiple platforms](https://www.docker.com/blog/docker-official-images-now-multi-platform/) (e.g. Arm64 and x64 and Linux and/or Windows).

### `<Major.Minor.Patch .NET Version>-<OS version>`

Examples:

- `6.0.12-jammy`
- `7.0.2-alpine3.17`

### `<Major.Minor .NET Version>-<OS version>`

Examples:

- `6.0-alpine3.17`
- `7.0-jammy`

### `<Major.Minor .NET Version>-<OS name>`

Examples:

- `6.0-alpine`

### `<Major.Minor.Patch .NET Version>`

Examples:

- `6.0.12`
- `7.0.2`

### `<Major.Minor .NET Version>`

Examples:

- `6.0`
- `7.0`

### `latest`

Notes:

- Includes all [supported architectures](supported-platforms.md#architectures).
- Includes Debian for Linux, unless specified (like `6.0-alpine`).
- Includes each supported Nano Server version for OS-agnostic tags (like `7.0` and `latest`)
- The `latest` tag references the latest stable release. In the `nightly` image repo, it may reference the latest preview release.

## Tag and image policies

The following policies are used for the tag patterns we use.

### `<Major.Minor.Patch .NET Version>`

This tag pattern references an image with a specific `Major.Minor.Patch` .NET version.

Examples:

- `6.0.12`
- `7.0.2-alpine3.17`

Notes:

- These tags are considered _fixed tags_ since they reference a specific .NET patch version.
- They are updated in response to base image updates (like a Debian base image) for the supported life of the image (typically one month).
- The .NET components within the image will not be updated.
- In the rare event that .NET components are updated before the next regular .NET service release, then a new image with a `-1` tag will be created. The same practice will repeat itself if necessary (with `-2` and then `-3` tags).

### `<Major.Minor .NET Version>`

This tag pattern references an image with a specific `Major.Minor` .NET version, but floats on patch updates.

Examples:

- `6.0`
- `7.0-alpine3.17`

Notes:

- These tags are considered _floating tags_ since they do not reference a specific patch version.
- They are updated in response to base image updates (like a Debian base image) for the supported life of the .NET release.
- The .NET components within the image will be updated.

### `<OS version>`

This tag pattern references an image with a specific OS version, but floats on OS patch updates. See [Supported Platforms](supported-platforms.md#operating-systems) for the list of supported operating systems.

Examples:

- `6.0-jammy`
- `7.0-alpine3.17`

Notes:

- These tags are updated in response to base image updates (like an Ubuntu base image) for the supported life of the .NET release.
- Digest pinning is required to request a specific patch of an operating system (e.g. `mcr.microsoft.com/dotnet/runtime@sha256:4d3d5a5131a0621509ab8a75f52955f2d0150972b5c5fb918e2e59d4cb9a9823`).
- For [Debian](https://en.wikipedia.org/wiki/Debian_version_history) and [Ubuntu](https://en.wikipedia.org/wiki/Ubuntu_version_history) images, release codenames are used versus version numbers.

### `alpine`

This tag pattern floats to the latest version of Alpine.

Examples:

- `6.0-alpine`
- `7.0-alpine`

Notes:

- When a new version of Alpine is supported, it will initially be published only with version-specific tags (e.g. `6.0-alpine3.17`), with an [announcement](https://github.com/dotnet/dotnet-docker/issues?q=is%3Aopen+is%3Aissue+label%3Aannouncement).
- The new version-specific tag needs to be used initially.
- One month later, the floating tag (e.g. `6.0-alpine`) will be updated with the new Alpine version.
- Another announcement will be posted at this time.

### `<Architecture>`

This tag pattern references an image with a specific architecture. See [Supported Platforms](supported-platforms.md#architectures) for the list of supported architectures.

Examples:

- `6.0-jammy-amd64`
- `7.0-alpine-arm64v8`

Notes:

- For Windows, `amd64` is the only architecture supported and is excluded from the tag name.

## Tag Lifecycle

Each tag will be supported for the lifetime of the .NET and OS version referenced by the tag, unless further restricted according to [platform support policy](supported-platforms.md).

When an OS version reaches End-of-Life (EOL), its tags will no longer be maintained.

When a .NET version reaches EOL, its tags will continue to be maintained (rebuilt for base image updates) until the next .NET servicing date (typically on "Patch Tuesday", the 2nd Tuesday of the month).

Once a tag is no longer maintained, it will be considered unsupported, will no longer be updated. Unsupported tags will continue to exist in the container registry to prevent breaking any references to it.

## Policy Changes

In the event that a change is needed to the tagging patterns used, all tags for the previous pattern will continue to be supported for their original lifetime. They will however be removed from the documentation. [Announcements](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) will be posted when any tagging policy changes are made.
