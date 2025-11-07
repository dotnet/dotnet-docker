ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.1.3 \
    && curl --fail --show-error --location \
        --output dotnet-monitor-egress-azureblobstorage.tar.gz https://builds.dotnet.microsoft.com/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
        --output dotnet-monitor-egress-s3storage.tar.gz https://builds.dotnet.microsoft.com/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_azureblobstorage_sha512='3110f944d89a7ed4b8470096612b78f7e8f3f1f1c0f244afab65672e74e9dfe9bcc43675a121e0997a4935393b4ae7c9df1d649afd95d7fb9599b3741fb55d98' \
    && echo "$dotnet_monitor_azureblobstorage_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    && dotnet_monitor_s3storage_sha512='e2867556d9a81d8bf7b2e6bf47117135f1a6ce4b93b71222c099ec7e299cc3e74e7a4fb8cfdad3734651de32e3d379e828fefae855df3ed60108e33ed31f0fad' \
    && echo "$dotnet_monitor_s3storage_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-azureblobstorage.tar.gz --directory /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-s3storage.tar.gz --directory /app \
    && rm \
        dotnet-monitor-egress-azureblobstorage.tar.gz \
        dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.1.3-azurelinux-distroless-arm64v8

COPY --from=installer ["/app", "/app"]
