ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=9.0.0 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/9.0.0-rtm.24528.3/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='e6cc55b61cb3f793b37e85baef778cbc50d577c5a0ac4c7c650135a618dc656228d3efbae67cc2c1684dc6590b5be4faac9e4f36b4bd55450f673dc346d1843b' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/9.0.0-rtm.24528.3/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='ecfb290ce5868052cce197084fa9770c4e7ab96db0d737091c2d0822bebad38bb9393e724f0c8a515ed954782f183b8c5013948891f17469fe03f7fc05d7fb43' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:9.0.0-arm64v8

COPY --from=installer ["/app", "/app"]
