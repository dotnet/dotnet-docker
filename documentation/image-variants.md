# .NET Image Variants

.NET container images offer several image variants.

.NET images for Ubuntu, Debian, Windows Server Core, and Nano Server images are full-featured and include ICU libraries which provide Unicode and globalization support.
As of .NET 8, we also guarantee these images include time zone information (e.g. `tzdata` in Linux).
These images are intended to satisfy the most common use cases of .NET developers.

Alpine and [Ubuntu Chiseled](#ubuntu-chiseled-net-60) .NET images are focused on size.
By default, these images do not include `icu` or `tzdata`, meaning that these images only work with apps that are configured for [globalization-invariant mode](https://learn.microsoft.com/dotnet/core/runtime-config/globalization).
Apps that require globalization support can use the `extra` image variant of the [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/) images. Because this is only available with `runtime-deps` images, it requires a [self-contained deployment](https://learn.microsoft.com/dotnet/core/deploying/#publish-self-contained) of the application.
Alpine and Chiseled images also come in `extra`, `composite`, and `aot` variants (see below).

### `chiseled`

[Ubuntu Chiseled](https://ubuntu.com/engage/chiselled-ubuntu-images-for-containers) is Ubuntu's take on "distroless" container images.
The Ubuntu Chiseled .NET images include the minimum set of libraries necessary to run .NET applications.
For more information, see the [Ubuntu Chiseled .NET Containers documentation](./ubuntu-chiseled.md).

### `extra` (.NET 8.0+)

For apps that depend on globalization functionality, the `extra` image variant is offered for [Ubuntu Chiseled](./ubuntu-chiseled.md) `runtime-deps`, `runtime`, and `aspnet` images as well as `runtime-deps` images for Alpine Linux and CBL Mariner distroless.
These `extra` images contain everything that the default images do, plus `icu` and `tzdata`.

### `composite` (.NET 8.0+)

ASP.NET Core Composite images provide a smaller size on disk while keeping the performance of the default [ReadyToRun (R2R) setting](https://learn.microsoft.com/dotnet/core/deploying/ready-to-run).
The caveat is that the composite images have tighter version coupling. This means the final app run on them cannot use handpicked custom versions of the framework and/or ASP.NET assemblies that are built into the composite binary.
For a full technical description on how the composites work, we have a [feature doc here](https://github.com/dotnet/runtime/blob/main/docs/design/features/readytorun-composite-format-design.md).

### (Preview) `aot` (.NET 8.0+)

The `aot` images provide an optimized deployment size for [Native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) compiled .NET apps.
Native AOT has the lowest size, startup time, and memory footprint of all .NET deployment models.
Please see ["Limitations of Native AOT deployment"](https://learn.microsoft.com/dotnet/core/deploying/native-aot#limitations-of-native-aot-deployment) to see if your app might be compatible.
`aot` image variants are only available for our size-focused `runtime-deps` images: Alpine and Ubuntu Chiseled.
They also require the use of the `aot` SDK image which include extra libraries needed for Native AOT compilation.

> [!NOTE]
> `aot` images are only available as a preview in the [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/) and [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/) repos.
> Native AOT compiled apps will function exactly the same on the existing `runtime-deps` (non-`aot`) images, but with a larger deployment size.
> Please try out these new, smaller images and give us feedback!
