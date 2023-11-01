{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme ^
    set repo to when(IS_PRODUCT_FAMILY,
        "product-family",
        when(PARENT_REPO = "monitor", cat("monitor-", SHORT_REPO), SHORT_REPO))
}}{{ARGS["top-header"]}}# Tag Formatting

{{ARGS["top-header"]}}## .NET Versions

All .NET container images have both "fixed version" and "floating version" tags.
Floating version tags will always reference the latest version of a specific .NET major version, while fixed version tags will always only reference a specific patch version.
For all tags below, `<.NET Version>` can be substituted for either `<Major.Minor>` or `<Major.Minor.Patch>`, for example: `7.0` or `7.0.12`.

{{ARGS["top-header"]}}## Single-platform tags

These "fixed version" tags reference an image with a specific .NET version for a specific operating system and architecture.

- `<.NET Version>-<OS>-<Architecture>`
- `<.NET Version>-<OS>-<variant>-<Architecture>`
- `<.NET Version>-<OS>-<Architecture>`
- `<.NET Version>-<OS>-<variant>-<Architecture>`

{{ARGS["top-header"]}}## Multi-platform tags

These tags reference images for [multiple platforms](https://docs.docker.com/build/building/multi-platform/).

- `<.NET Version>`
    - The version-only floating tag refers to the latest Debian version available at the .NET Major Version's release.
- `<.NET Version>-<OS>`
- `<.NET Version>-<OS>-<variant>`

{{ARGS["top-header"]}}## Image Variants

By default, Ubuntu and Debian images for .NET 8 will have both `icu` and `tzdata` installed.
These images are intended to satisfy the most common use cases of .NET developers.

Our Alpine and Ubuntu Chiseled images are focused on size.
These images do not and will not include `icu` or `tzdata`, meaning that these images only work iwth apps that are configured for [globalization-invariant mode](https://learn.microsoft.com/dotnet/core/runtime-config/globalization).
Apps that require globalization support can use the `extra` image variant of the [dotnet/runtime-deps]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/runtime-deps" ])}}) images.

Example tags:
- `8.0-bookworm-slim`
- `6.0-jammy`
- `7.0-alpine3.18-arm64v8`{{if or(repo = "runtime-deps", repo = "product-family"):

{{ARGS["top-header"]}}### `extra`

The `extra` image variant is offered alongside our size-focused base images for self-contained or single file apps that depend on globalization functionality.
Extra images contain everything that the default images do, plus `icu` and `tzdata`.

Example tags:
- `8.0-jammy-chiseled-extra`
- `8.0.0-alpine3.18-extra`}}{{if or(repo = "aspnet", repo = "product-family"):

{{ARGS["top-header"]}}### `composite`

Compared to the default ASP.NET images, ASP.NET Composite images provide a smaller image size on disk as well as performance improvements for framework-dependent ASP.NET apps by performing some cross-assembly optimizations and between the .NET and ASP.NET runtimes.
However, this means that apps run on the ASP.NET Composite runtime cannot use handpicked custom versions of .NET or ASP.NET assemblies that are built into the image.

Example tags:

- `8.0.0-jammy-chiseled-composite`
- `8.0-alpine3.18-composite` }}{{if or(repo = "runtime-deps", repo = "sdk", repo = "product-family"):

{{ARGS["top-header"]}}### (Preview) `aot`

`aot` images provide an optimized deployment size for [native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) compiled .NET apps.
Native AOT has the lowest size, startup time, and memory footprint of all .NET deployment models.
Please see ["Limiatations of Native AOT deployment"](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot#limitations-of-native-aot-deployment) to see if your app might be compatible.
`aot` image variants are only available for our size-focused `runtime-deps` images: Alpine and Ubuntu Chiseled.
They also require the use of the `aot` SDK image which include extra libraries needed for Native AOT compilation.

Example tags:{{if repo = "sdk":
- `8.0.100-jammy-aot`
- `8.0-alpine3.18-aot`^else:
- `8.0-jammy-chiseled-aot`
- `8.0.0-alpine3.18-aot`}}

**Note:** `aot` images are only available as a preview in the [dotnet/nightly/sdk]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/nightly/sdk" ])}}) and [dotnet/nightly/runtime-deps]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/nightly/runtime-deps" ])}}) repos.
Native AOT compiled apps will function exactly the same on the existing `runtime-deps` (non-`aot`) images, but with a larger deployment size.
Please try these new, smaller images out and give us feedback!}}
