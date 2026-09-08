ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.1.4 \
    && curl --fail --show-error --location \
        --output dotnet-monitor-egress-azureblobstorage.tar.gz https://builds.dotnet.microsoft.com/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
        --output dotnet-monitor-egress-s3storage.tar.gz https://builds.dotnet.microsoft.com/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_azureblobstorage_sha512='da731216abe7587fa0b73fe5c0d804bbc433729b35b74fd02785958196c355493ebee8a71c55647e97afb9a774747149214ee2bdbb38275e59024755194780fe' \
    && echo "$dotnet_monitor_azureblobstorage_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    && dotnet_monitor_s3storage_sha512='dc6a434ea9280805ce8cfbb414a2970bb091417c9f66fccde86d74b20425f2db76ab84bc7b5486bd715907b8e0d4f2bb23362041f097cea04f45fbfd4aca9f80' \
    && echo "$dotnet_monitor_s3storage_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-azureblobstorage.tar.gz --directory /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-s3storage.tar.gz --directory /app \
    && rm \
        dotnet-monitor-egress-azureblobstorage.tar.gz \
        dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.1.4-azurelinux-distroless-arm64v8

COPY --from=installer ["/app", "/app"]
