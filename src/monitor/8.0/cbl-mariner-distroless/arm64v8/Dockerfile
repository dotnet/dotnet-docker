ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.0.0-preview.5.23307.11 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='7c7fe7a0ff0be0f831ecf0a7960797e27c031316cf911a96aaaba7b957a88aa4f639f0b8087665ea2c1f898b9bc6c13be7aa948bcaf6a1f39735da826aecd701' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='1d8e3c2aa7242f5d90e86d7ad0d40cf1c8b1f2be0dbc990d48f43c65b2bee7c873f751af6532720a82880593cc7ef3685cbd8653a7a664ef1c5cf40d6aa012d2' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.0.0-preview.5-cbl-mariner-distroless-arm64v8

COPY --from=installer ["/app", "/app"]
