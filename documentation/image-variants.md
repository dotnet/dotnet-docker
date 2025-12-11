# .NET Image Variants

.NET container images offer several image variants.

.NET images for Ubuntu, Debian, Azure Linux, Windows Server Core, and Nano Server images are full-featured and include ICU and time zone libraries which provide full Unicode and globalization support.
These images are intended to satisfy the most common use cases of .NET developers.

Alpine and [Ubuntu Chiseled](./ubuntu-chiseled.md) .NET images are focused on size.
By default, these images do not include `icu` or `tzdata`, meaning that these images only work with apps that are configured for [globalization-invariant mode](https://learn.microsoft.com/dotnet/core/runtime-config/globalization).
Apps that require globalization support can use the `extra` image variant of the [dotnet/runtime-deps](../README.runtime-deps.md) images. Because this is only available with `runtime-deps` images, it requires a [self-contained deployment](https://learn.microsoft.com/dotnet/core/deploying/#publish-self-contained) of the application.
Alpine, Azure Linux, and Ubuntu Chiseled images also come in `extra`, `composite`, and `aot` variants (see below).

## Distroless

[Ubuntu Chiseled](https://ubuntu.com/engage/chiselled-ubuntu-images-for-containers) and [Azure Linux](./azurelinux.md) distroless .NET images contain only the minimum set of libraries necessary to run .NET applications with everything else removed.
For more information, see the [distroless .NET images documentation](./distroless.md).

## `extra` (.NET 8+)

For apps that depend on globalization functionality, the `extra` image variant is offered for [Ubuntu Chiseled](./ubuntu-chiseled.md) and [Azure Linux](./azurelinux.md) distroless `runtime-deps`, `runtime`, and `aspnet` images as well as `runtime-deps` images for Alpine Linux.
These `extra` images contain everything that the default images do, plus `icu` and `tzdata`.

## `composite` (.NET 8+)

ASP.NET Core Composite images provide a smaller size on disk while keeping the performance of the default [ReadyToRun (R2R) setting](https://learn.microsoft.com/dotnet/core/deploying/ready-to-run).
The caveat is that the composite images have tighter version coupling. This means the final app run on them cannot use handpicked custom versions of the framework and/or ASP.NET assemblies that are built into the composite binary.
For a full technical description on how the composites work, we have a [feature doc here](https://github.com/dotnet/runtime/blob/main/docs/design/features/readytorun-composite-format-design.md).

## `aot` (.NET 10+, SDK-only)

SDK images with the `-aot` suffix include [additional libraries](https://learn.microsoft.com/dotnet/core/deploying/native-aot#prerequisites) required for native AOT compilation that aren't present in the default `mcr.microsoft.com/dotnet/sdk` images.

[Native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) compilation produces an app that's self-contained and that has been ahead-of-time (AOT) compiled to native code.
Native AOT apps have faster startup time and smaller memory footprints.
Native AOT apps can be deployed on the standard `mcr.microsoft.com/dotnet/runtime-deps` images.
See ["Limitations of Native AOT deployment"](https://learn.microsoft.com/dotnet/core/deploying/native-aot#limitations-of-native-aot-deployment) to see if your app might be compatible.
