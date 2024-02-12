ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.0.1 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='eb4bd78fe0d8490ba08e424d3b7e2c223d18d7e58c6df84e0e763e1844f163263b396c49e3025554395245168e6f9298313b7ac4c9e8ea117f256717b497e9a4' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='45f380a7e358a5a5bb3a851dcfea69ad35827e08cba96ad569b8f11c4963a7f4bb65674ab0c8d581e0b5fce3596ce2ff9def3a8267a9ba60267485d5795b28be' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.0.1-cbl-mariner-distroless-amd64

COPY --from=installer ["/app", "/app"]
