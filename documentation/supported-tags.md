# .NET Container Tags -- Patterns and Policies

This document describes the tagging patterns and policies that are used for the official .NET container images. .NET tags are intended to closely match the tagging patterns used by [Official Images on Docker Hub](https://hub.docker.com/search?q=&type=image&image_filter=official). Please [log an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose) if you encounter problems using .NET images or applying these tagging patterns.

Complete tag lists:

- [runtime-deps](../README.runtime-deps.md#full-tag-listing)
- [runtime](../README.runtime.md#full-tag-listing)
- [aspnet](../README.aspnet.md#full-tag-listing)
- [sdk](../README.sdk.md#full-tag-listing)
- [monitor](../README.monitor.md#full-tag-listing)
- [monitor-base](../README.monitor-base.md#full-tag-listing)
- [aspire-dashboard](../README.aspire-dashboard.md#full-tag-listing)
- [samples](../README.samples.md#full-tag-listing)

The terms "fixed version" and "floating version" are used throughout. They are defined in [Tag policies](#tag-policies).

## Single-platform tags

These tags reference an image for a single platform (e.g. "Linux Arm64" or "Windows x64").

### `<Major.Minor.Patch .NET Version>-<OS>-<Architecture>`

These "fixed version" tags reference an image with a specific `Major.Minor.Patch` .NET version for a specific operating system and architecture.

Examples:

- `8.0.11-noble-amd64`
- `8.0.11-noble-arm64v8`
- `8.0.11-nanoserver-ltsc2022`
- `8.0.11-alpine3.21-arm64v8`
- `8.0.11-bookworm-slim-arm32v7`

### `<Major.Minor .NET Version>-<OS>-<Architecture>`

These "floating version" tags reference an image with a specific `Major.Minor` (with latest patch) .NET version for a specific operating system and architecture.

Examples:

- `8.0-noble-arm64v8`
- `8.0-noble-amd64`
- `8.0-nanoserver-ltsc2022`
- `8.0-alpine3.21-arm64v8`
- `8.0-bookworm-slim-arm32v7`

## Multi-platform tags

These tags reference images for [multiple platforms](https://docs.docker.com/build/building/multi-platform/).

They include:

- For .NET 10, multi-platform tags refer to Ubuntu unless otherwise specified (like `8.0-alpine`).
- For .NET versions prior to 10, multi-platform tags refer to Debian unless otherwise specified.
- All [supported architectures](supported-platforms.md#architectures).

> [!WARNING]
> These multi-platform tags **specifically exclude all Windows versions** due
> to `containerd`'s platform matching algorithm for Windows hosts. See
> [containerd/containerd#6508](https://github.com/containerd/containerd/issues/6508)
> and [dotnet/dotnet-docker#4492](https://github.com/dotnet/dotnet-docker/issues/4492)
> for more context.
>
> If you are using Windows, you will need to explicitly specify an OS Version
> with a single-platform tag like so:
>
> ```Dockerfile
> FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-ltsc2022
> FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809
> FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2019
> FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022
> ```

### `<Major.Minor.Patch .NET Version>-<OS version>`

These "fixed version" tags reference an image with a specific `Major.Minor.Patch` .NET version, for a specific operating system, while architecture will be chosen based on the requesting environment.

Examples:

- `8.0.11-noble`
- `8.0.11-alpine3.21`

### `<Major.Minor .NET Version>-<OS version>`

These "floating version" tags reference an image with a specific `Major.Minor` (with latest patch) .NET version, for a specific operating system, while architecture will be chosen based on the requesting environment.

Examples:

- `8.0-alpine3.21`
- `8.0-noble`

### `<Major.Minor .NET Version>-alpine`

These "floating version" tags reference an image with a specific `Major.Minor` (with latest patch) .NET version, for the latest Alpine version, while architecture will be chosen based on the requesting environment.

Examples:

- `8.0-alpine`
- `9.0-alpine`

> [!NOTE]
>
> - New versions of Alpine will be published with version-specific tags (e.g. `8.0-alpine3.21`).
> - Floating tag (e.g. `8.0-alpine`) will be updated with the new Alpine version a month later.
> - Tag changes will be [announced](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) so that users know when the tags they want are available.

### `<Major.Minor.Patch .NET Version>`

These "fixed version" tags reference an image with a specific `Major.Minor.Patch` .NET version, while operating system and architecture will be chosen based on the requesting environment.

Examples:

- `8.0.11`
- `9.0.11`

### `<Major.Minor .NET Version>`

These "floating version" tags reference an image with a specific `Major.Minor` (with latest patch) .NET version, while operating system and architecture will be chosen based on the requesting environment.

Examples:

- `8.0`
- `9.0`

### Image Variants

Starting with 8.0, .NET offers several image variants that provide different features for the size-focused OSes, Alpine, Azure Linux, and Ubuntu Chiseled.
You can use these variants by appending the variant name (e.g. `extra`, `chiseled`) to the OS name.

Examples:

- `8.0-noble-chiseled`
- `8.0.11-noble-chiseled-extra`
- `9.0.0-alpine3.21-extra`

For more information, see the [Image Variants documentation](./image-variants.md).

### `latest`

These "floating version" `latest` tag references an image with the latest `Major.Minor.Patch` .NET version, while operating system and architecture will be chosen based on the requesting environment.

> [!NOTE]
>
> - The `latest` tag references the latest stable release.
> - In the `nightly` image repo, it may reference the latest preview release.

## Tag policies

The following policies are used for the tag patterns we use.

### .NET Versions

Each .NET release goes through multiple [support phases](https://github.com/dotnet/core/blob/main/release-policies.md#support-phases), with varying support levels.

While a new .NET version is in preview, both the fixed and floating version tags have a special preview suffix.
When a .NET version moves from Preview to Release Candidate, the `-preview` suffix is dropped from the floating version tags.
This is because .NET Release Candidates are supported for production use under a "Go-Live" license, so the preview label no longer applies.
In this case, fixed versions will use an `-rc.<RC Number>` suffix since the patch version would otherwise clash with the GA release (e.g. `9.0.0`).

| Support phase | Fixed version tag format | Floating version tag format |
| --- | --- | --- |
| **Preview** | `<Major>.<Minor>.<Patch>-preview.<PreviewVersion>` | `<Major>.<Minor>-preview` |
| **Release Candidate (Go-Live)** | `<Major>.<Minor>.<Patch>-rc.<ReleaseCandidateVersion>` | `<Major>.<Minor>` |
| **Active and Maintenance** | `<Major>.<Minor>.<Patch>` | `<Major>.<Minor>` |

See [.NET's release policies](https://github.com/dotnet/core/blob/main/release-policies.md) for more details.

#### Fixed version tags

"Fixed version" tags reference an image with a specific .NET patch version.

Examples:

- `8.0.11`
- `8.0.11-alpine3.21`
- `9.0.0-preview.7`
- `9.0.0-rc.1`

> [!NOTE]
>
> - _Fixed version tags_ are updated according to the [image update policy](../README.md#image-update-policy) for the supported life of the image (typically one month).
> - _Fixed version tags_ guarantee that the version of .NET in the image will never change.
> - At times, components of .NET images like PowerShell or MinGit may require updates out of band with .NET releases in order to fix critical bugs or vulnerabilities. If this happens, new images will be created with a `-1` suffix appended to the fixed tag so that you can roll back to the previous fixed tag if necessary. The same practice will repeat itself if necessary (with `-2` and then `-3` tags).

#### Floating version tags

"Floating version" tags references an image with a specific `Major.Minor` .NET version, but float on patch updates.

Examples:

- `8.0`
- `9.0`
- `9.0-alpine3.21`
- `9.0-preview`
- `9.0-preview-noble`

> [!NOTE]
>
> - _Floating version tags_ always point to the latest patch version of a given `Major.Minor` .NET release.
> - _Floating version tags_ are updated according to the [image update policy](../README.md#image-update-policy) for the supported life of the .NET release or until the OS they are based on reaches EOL, whichever is sooner. We will post an [Announcement](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) when we remove or stop supporting images for any reason.

### OS tags and base image updates

Version-specific operating system tags reference an image with a specific OS version, but floats on OS patch updates. See [Supported Platforms](supported-platforms.md#operating-systems) for the list of supported operating systems.

Examples:

- `8.0-noble`
- `9.0-alpine3.21`

> [!NOTE]
>
> - These tags are updated in response to base image updates (like an Ubuntu base image) for the supported life of the .NET release or until the OS they are based on reaches EOL, whichever is sooner.
> - Digest pinning is required to request a specific patch of an operating system (e.g. `mcr.microsoft.com/dotnet/runtime@sha256:4d3d5a5131a0621509ab8a75f52955f2d0150972b5c5fb918e2e59d4cb9a9823`).
> - If an image is only available for one operating system, then the operating system will be omitted from the tag.
> - For [Debian](https://en.wikipedia.org/wiki/Debian_version_history) and [Ubuntu](https://en.wikipedia.org/wiki/Ubuntu_version_history) images, release codenames are used instead of version numbers.

### .NET appliance images

.NET produces several appliance images containing useful diagnostic tools and applications:

- [.NET Monitor](../README.monitor.md#full-tag-listing)
- [.NET Monitor Base](../README.monitor-base.md#full-tag-listing)
- [Aspire Dashboard](../README.aspire-dashboard.md#full-tag-listing)

These appliance images provide value based on the .NET apps they include.
Their base operating system is an implementation detail and should not affect the intended use or behavior of these images.
They use [distroless](./distroless.md) operating systems whenever possible, further reducing the importance of the base image OS.
These images may receive base image OS upgrades in the middle of major or minor version releases, provided this doesn't cause any breaking changes to the image's functionality.
As such, these images may have tags that don't specify an OS version, or in some cases won't even refer to an OS at all.

### Windows tags

For Windows, `amd64` is the only architecture supported and is excluded from the tag name.

## Tag Lifecycle

Each tag will be supported for the lifetime of the .NET and OS version referenced by the tag, unless further restricted according to [platform support policy](supported-platforms.md).

When a .NET version or an OS version reaches [end of support](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) (sometimes referred to as end-of-life or EOL), its tags will no longer be supported.
Unsupported tags and images can still be pulled, but they will no longer receive updates for any reason.

See ["Is your image built from a supported .NET tag?"](vulnerability-reporting.md#c-is-your-image-built-from-a-supported-net-tag) for details on how to tell if a specific tag is supported.

## Policy Changes

In the event that a change is needed to the tagging patterns used, all tags for the previous pattern will continue to be supported for their original lifetime. They will however be removed from the documentation. [Announcements](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) will be posted when any tagging policy changes are made.
