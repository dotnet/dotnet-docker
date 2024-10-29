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
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/9.0.0-rtm.24528.3/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='f9fc2ce9f608b4f35c8adbf79de11464f8eb9ac3b2d68c7bd7af273fa3987226e808bcfcc49b7d14904f233543e15dc26a4c35f50c35c9e47c74bdfd741ee24b' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/9.0.0-rtm.24528.3/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='ddb1999216e01a40371ecb6f1aee2a5586d58c0e66083843874d197e603f54c71200f9cc81c2879e97b84e99917192355a05089be796d6b46ac222890b6e1b70' \
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
