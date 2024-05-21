ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=9.0.0-preview.4.24270.1 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='c7f69defc7498d41365de94eb14d438f024d05517b0c117c0deace04c5833541cdd1e393aa0a4a62d7cc5348f83713acb8101d85fc1395095f880e3653842df5' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='c206c7aed4abc2999322c1bcea1f754e4b7c1468e6fe3b256ce8e466426ebe3af5cc70f57895b7542ca1a6c1e8f3e70847c15a7c4b32d26c3c0289607f7c55ff' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:9.0.0-preview.4-ubuntu-chiseled-arm64v8

COPY --from=installer ["/app", "/app"]
