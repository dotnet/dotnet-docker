ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=9.0.6 \
    && curl --fail --show-error --location \
        --output dotnet-monitor-egress-azureblobstorage.tar.gz https://builds.dotnet.microsoft.com/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
        --output dotnet-monitor-egress-s3storage.tar.gz https://builds.dotnet.microsoft.com/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_azureblobstorage_sha512='84b1aba86788c6a2c0e4e4c28036b7ef25c672634b02365f1d166c1f7f1d11c97ef883f9a50cdff78a083972676145bbe8c0a09df59d8fda9981fc183d4ced52' \
    && echo "$dotnet_monitor_azureblobstorage_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    && dotnet_monitor_s3storage_sha512='0ac83d2d95b575c4f891a1190fc92d4dc5d53203fd50a55ab081d9e0652d5ce11a743cd81dea94f8c96acdc2614b2627009b8f0ffb16ae708061cbf8ed27e1ad' \
    && echo "$dotnet_monitor_s3storage_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-azureblobstorage.tar.gz --directory /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-s3storage.tar.gz --directory /app \
    && rm \
        dotnet-monitor-egress-azureblobstorage.tar.gz \
        dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:9.0.6-amd64

COPY --from=installer ["/app", "/app"]
