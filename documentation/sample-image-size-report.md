# .NET image size comparison

.NET offers a variety of deployment options for applications which pair well
with the container images that we publish. It's possible to produce very small
container images. This document summarizes the available options to help you
make the best base image choice for your apps and environment.

The table below shows how base image choice and publish type affects typical
image sizes and for a simple .NET minimal web API. These images were produced
from the ["releasesapi" sample](../samples/releasesapi).

| Base Image                                 | Publish Type                  | Distroless | Globalization | Compressed Size |
| ------------------------------------------ | ----------------------------- | ---------- | ------------- | --------------: |
| [`aspnet:10.0`]                            | [Framework-dependent]         | ✖️ No      | ✅ Yes         |        92.48 MB |
| [`aspnet:10.0-noble-chiseled`]             | [Framework-dependent]         | ✅ Yes      | ✖️ No         |        52.81 MB |
| [`aspnet:10.0-noble-chiseled-extra`]       | [Framework-dependent]         | ✅ Yes      | ✅ Yes         |        67.68 MB |
| [`runtime-deps:10.0`]                      | [Self-contained] + [Trimming] | ✖️ No      | ✖️ No         |        61.53 MB |
| [`runtime-deps:10.0`]                      | [Self-contained] + [Trimming] | ✖️ No      | ✅ Yes         |        61.63 MB |
| [`runtime-deps:10.0-noble-chiseled`]       | [Self-contained] + [Trimming] | ✅ Yes      | ✖️ No         |        21.86 MB |
| [`runtime-deps:10.0-noble-chiseled-extra`] | [Self-contained] + [Trimming] | ✅ Yes      | ✅ Yes         |        36.82 MB |
| [`runtime-deps:10.0`]                      | [Native AOT]                  | ✖️ No      | ✖️ No         |        51.27 MB |
| [`runtime-deps:10.0`]                      | [Native AOT]                  | ✖️ No      | ✅ Yes         |        51.36 MB |
| [`runtime-deps:10.0-noble-chiseled`]       | [Native AOT]                  | ✅ Yes      | ✖️ No         |        11.60 MB |
| [`runtime-deps:10.0-noble-chiseled-extra`] | [Native AOT]                  | ✅ Yes      | ✅ Yes         |        26.56 MB |
| [`aspnet:10.0-alpine`]                     | [Framework-dependent]         | ✖️ No      | ✖️ No         |        51.93 MB |
| [`runtime-deps:10.0-alpine`]               | [Self-contained] + [Trimming] | ✖️ No      | ✖️ No         |        20.95 MB |
| [`runtime-deps:10.0-alpine-extra`]         | [Self-contained] + [Trimming] | ✖️ No      | ✅ Yes         |        35.52 MB |
| [`runtime-deps:10.0-alpine`]               | [Native AOT]                  | ✖️ No      | ✖️ No         |        10.69 MB |
| [`runtime-deps:10.0-alpine-extra`]         | [Native AOT]                  | ✖️ No      | ✅ Yes         |        25.25 MB |

> [!NOTE]
> Please note that these image sizes are a snapshot of deployment sizes from
> November 2025. Image sizes will fluctuate over time due to base image and
> package updates.

Watch the [announcements page](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements)
for the latest information on new features and changes in .NET container images.

## About Publishing Options

### .NET Image Variants

- **[Ubuntu Chiseled] images** images are a type of [distroless container image](./distroless.md)
  that contain only a minimal set of packages with everything else removed - no
  shell, no package manager, and no globalization support by default. This
  results in a dramatically smaller deployment size and attack surface.
- **`extra` images** add `icu` and `tzdata` to Alpine and Chiseled/distroless
  images for full globalization support in .NET apps.

See [.NET Image Variants](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md)
for more details.

### Publishing Types

- **[Self-contained]** deployments bundle the .NET Runtime with your app so that
  it can run without the .NET Runtime installed.
- **[Trimming]** for self-contained apps removes unused code from the .NET
  Runtime and libraries to reduce application size.
- **[Native AOT]** deployment produces an app that is compiled ahead-of-time
  (AOT) for the smallest deployment size and startup time.

See [".NET Application Publishing Overview"](https://learn.microsoft.com/dotnet/core/deploying)
for more details on all of the supported options for publishing .NET apps.

[Ubuntu Chiseled]:                                 https://github.com/dotnet/dotnet-docker/blob/main/documentation/ubuntu-chiseled.md
[Self-contained]:                                  https://learn.microsoft.com/dotnet/core/deploying/#publish-self-contained
[Trimming]:                                        https://learn.microsoft.com/dotnet/core/deploying/trimming/trim-self-contained
[Native AOT]:                                      https://learn.microsoft.com/dotnet/core/deploying/native-aot/
[Framework-dependent]:                             https://learn.microsoft.com/dotnet/core/deploying/#publish-framework-dependent
[`aspnet:10.0`]:                            https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/10.0/noble
[`aspnet:10.0-noble-chiseled`]:             https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/10.0/noble-chiseled
[`aspnet:10.0-noble-chiseled-extra`]:       https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/10.0/noble-chiseled-extra
[`runtime-deps:10.0`]:                      https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/10.0/noble
[`runtime-deps:10.0-noble-chiseled`]:       https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/10.0/noble-chiseled
[`runtime-deps:10.0-noble-chiseled-extra`]: https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/10.0/noble-chiseled-extra
[`aspnet:10.0-alpine`]:                     https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/10.0/alpine3.22
[`runtime-deps:10.0-alpine`]:               https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/10.0/alpine3.22
[`runtime-deps:10.0-alpine-extra`]:         https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/10.0/alpine3.22-extra
