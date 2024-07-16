# Azure Linux Distroless

Azure Linux Distroless container images contain only the minimal set of packages .NET needs, with everything else removed.

Due to their limited set of packages, distroless containers have a minimized security attack surface, smaller deployment sizes, and faster start-up time compared to non-distroless images.

Azure Linux distroless .NET images are available for all supported .NET versions in the following image repos:

- [`mcr.microsoft.com/dotnet/runtime`](../README.runtime.md)
- [`mcr.microsoft.com/dotnet/aspnet`](../README.aspnet.md)
- [`mcr.microsoft.com/dotnet/runtime-deps`](../README.runtime-deps.md) (for self-contained apps)

You can use the following image tags (SDK is not available for distroless):

- `6.0-cbl-mariner2.0-distroless`
- `8.0-cbl-mariner2.0-distroless`

## Vulnerability Scanning

Azure Linux Distroless images maintain a detailed list of packages installed in the `/var/lib/rpmmanifest/container-manifest-1` and `/var/lib/rpmmanifest/container-manifest-2` files, which is supported by major image scanners like Qualys, Trivy, and Syft.

## Globalization

Since Azure Linux Distroless images are focused on providing a small deployment size, they do not include `icu` or `tzdata` libraries by default.
However, we offer an `extra` image variant that includes `icu` and `tzdata`.
You can use this in place of the default chiseled image by appending the `-extra` suffix to the image tag like so:

- `mcr.microsoft.com/dotnet/runtime-deps:8.0-cbl-mariner-distroless-extra`
- `mcr.microsoft.com/dotnet/runtime:8.0-cbl-mariner-distroless-extra`
- `mcr.microsoft.com/dotnet/aspnet:8.0-cbl-mariner-distroless-extra`

## Installing Additional Packages

If your app requires additional packages besides `icu` and `tzdata`, you can follow the same pattern that .NET uses to install the .NET runtime dependencies.

### Azure Linux 3.0 (Preview):

```Dockerfile
FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-azurelinux3.0-distroless AS base

FROM azurelinuxpreview.azurecr.io/public/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y fdupes \
    && tdnf clean all

COPY --from=base / /staging1
COPY --from=base / /staging2

RUN tdnf install -y --releasever=3.0 --installroot /staging2 tzdata \
    && tdnf clean all --releasever=3.0 --installroot /staging2

# Prepare the staging2 directory to be copied to the final stage by removing unnecessary files
# that will only cause extra image bloat.
RUN \
    # Remove duplicates from staging2 that exist in staging1
    fdupes /staging1 /staging2 -rdpN \
    \
    # Delete duplicate symlinks
    # Function to find and format symlinks w/o including root dir (format: /path/to/symlink /path/to/target)
    && getsymlinks() { find $1 -type l -printf '%p %l\n' | sed -n "s/^\\$1\\(.*\\)/\\1/p"; } \
    # Combine set of symlinks between staging1 and staging2
    && (getsymlinks "/staging1"; getsymlinks "/staging2") \
        # Sort them
        | sort \
        # Find the duplicates
        | uniq -d \
        # Extract just the path to the symlink
        | cut -d' ' -f1 \
        # Prepend the staging2 directory to the paths
        | sed -e 's/^/\/staging2/' \
        # Delete the files
        | xargs rm \
    \
    # General cleanup
    && rm -rf /staging2/etc/tdnf \
    && rm -rf /staging2/run/* \
    && rm -rf /staging2/var/cache/tdnf \
    && rm -rf /staging2/var/lib/rpm \
    && rm -rf /staging2/usr/share/doc \
    && rm -rf /staging2/usr/share/man \
    && find /staging2 -type d -empty -delete

FROM base
COPY --from=installer /staging2/ /
```

### Azure Linux 2.0:

```Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:6.0-cbl-mariner2.0-distroless AS base

FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y fdupes \
    && tdnf clean all

COPY --from=base / /staging1
COPY --from=base / /staging2

RUN tdnf install -y --releasever=2.0 --installroot /staging2 <package-name> \
    && tdnf clean all --releasever=2.0 --installroot /staging2

# Prepare the staging2 directory to be copied to the final stage by removing unnecessary files
# that will only cause extra image bloat.
RUN \
    # Remove duplicates from staging2 that exist in staging1
    fdupes /staging1 /staging2 -rdpN \
    \
    # Delete duplicate symlinks
    # Function to find and format symlinks w/o including root dir (format: /path/to/symlink /path/to/target)
    && getsymlinks() { find $1 -type l -printf '%p %l\n' | sed -n "s/^\\$1\\(.*\\)/\\1/p"; } \
    # Combine set of symlinks between staging1 and staging2
    && (getsymlinks "/staging1"; getsymlinks "/staging2") \
        # Sort them
        | sort \
        # Find the duplicates
        | uniq -d \
        # Extract just the path to the symlink
        | cut -d' ' -f1 \
        # Prepend the staging2 directory to the paths
        | sed -e 's/^/\/staging2/' \
        # Delete the files
        | xargs rm \
    \
    # General cleanup
    && rm -rf /staging2/etc/tdnf \
    && rm -rf /staging2/run/* \
    && rm -rf /staging2/var/cache/tdnf \
    && rm -rf /staging2/var/lib/rpm \
    && rm -rf /staging2/usr/share/doc \
    && rm -rf /staging2/usr/share/man \
    && find /staging2/var/log -type f -size +0 -delete \
    && find /staging2 -type d -empty -delete

FROM base
COPY --from=installer /staging2/ /
```
