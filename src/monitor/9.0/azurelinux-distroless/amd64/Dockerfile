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
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/9.0.0-rtm.24513.1/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='ced7f5f6c047c4f1ee9fea16f0c0663f1ce807be087eab505354ba4468c1b466ceb3376d4f3efe4cac49e85c48925771252419fcf0c378301e9668d395db16e5' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/9.0.0-rtm.24513.1/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='c471f9368d4fef4eaa7c4c2c0adb03c939b882c330dbc862019a4c31296341479a9c1df36e250f771944b5890aa4baf3f0f2aa0b2ad2c200a14d4df3e6596a44' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:9.0.0-amd64

COPY --from=installer ["/app", "/app"]
