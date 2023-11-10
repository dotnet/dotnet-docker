# .NET Image Variants

.NET Container images offer several image variants.

.NET images for Ubuntu, Debian, Windows Server Core, and Nanoserver images are "batteries included" and include ICU libraries.
As of .NET 8, we also guarantee these images include `tzdata`.
These images are intended to satisfy the most common use cases of .NET developers.

Alpine and [Ubuntu Chiseled](#ubuntu-chiseled-net-60) .NET images are focused on size.
These images do not include `icu` or `tzdata`, meaning that these images only work with apps that are configured for [globalization-invariant mode](https://learn.microsoft.com/dotnet/core/runtime-config/globalization).
Apps that require globalization support can use the `extra` image variant of the [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/) images.
Alpine and Chiseled images also come in `extra`, `composite`, and `aot` variants (see below).

### Ubuntu Chiseled (.NET 6.0+)

[Ubuntu Chiseled](https://ubuntu.com/engage/chiselled-ubuntu-images-for-containers) is Ubuntu's take on "distroless" container images.
The Ubuntu Chiseled .NET images include the minimum set of libraries necessary to run .NET applications.
For more information, see the [Ubuntu Chiseled .NET Containers documentation](./ubuntu-chiseled.md).

### `extra` (.NET 8.0)

The `extra` image variant is offered alongside our size-focused base images for self-contained or single file apps that depend on globalization functionality.
Extra images contain everything that the default images do, plus `icu` and `tzdata`.

### `composite` (.NET 8.0)

Compared to the default ASP.NET images, ASP.NET Composite images provide a smaller image size on disk as well as performance improvements for framework-dependent ASP.NET apps by performing some cross-assembly optimizations and between the .NET and ASP.NET runtimes.
However, this means that apps run on the ASP.NET Composite runtime cannot use handpicked custom versions of .NET or ASP.NET assemblies that are built into the image.

### (Preview) `aot` (.NET 8.0)

`aot` images provide an optimized deployment size for [native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) compiled .NET apps.
Native AOT has the lowest size, startup time, and memory footprint of all .NET deployment models.
Please see ["Limiatations of Native AOT deployment"](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot#limitations-of-native-aot-deployment) to see if your app might be compatible.
`aot` image variants are only available for our size-focused `runtime-deps` images: Alpine and Ubuntu Chiseled.
They also require the use of the `aot` SDK image which include extra libraries needed for Native AOT compilation.

> [!NOTE]
> `aot` images are only available as a preview in the [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/) and [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/) repos.
> Native AOT compiled apps will function exactly the same on the existing `runtime-deps` (non-`aot`) images, but with a larger deployment size.
> Please try out these new, smaller images and give us feedback!
