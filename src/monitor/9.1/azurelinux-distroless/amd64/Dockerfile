ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=9.1.0-preview.3.25418.1 \
    && curl --fail --show-error --location \
        --output dotnet-monitor-egress-azureblobstorage.tar.gz https://ci.dot.net/public/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
        --output dotnet-monitor-egress-s3storage.tar.gz https://ci.dot.net/public/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_azureblobstorage_sha512='9edd5d9a2638aac1934cf3cf9c8d01373469ebf91b5031278920b2c2bcea51971f8aa147d9264860577a243d145460b324df8420b011d346e2eb35dc0b579cf7' \
    && echo "$dotnet_monitor_azureblobstorage_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    && dotnet_monitor_s3storage_sha512='47972b5ee7997bb8ea1027aeab5c416248319a3bf87a21245a4814132bd41ed1df5dfd583d5eb2b04751110bede41e0f9ba9e2573ab790c412ff6264d0e1eb7f' \
    && echo "$dotnet_monitor_s3storage_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-azureblobstorage.tar.gz --directory /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-s3storage.tar.gz --directory /app \
    && rm \
        dotnet-monitor-egress-azureblobstorage.tar.gz \
        dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:9.1.0-preview.3-amd64

COPY --from=installer ["/app", "/app"]
