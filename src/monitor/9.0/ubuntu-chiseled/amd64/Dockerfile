ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=9.0.0-preview.4.24270.1 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='2fb97bf21caa616c0c91fb47b6cf8560d7116206065a7784335507b25f1f3a826bf7b32b1798a64f5671aa30bd985cf1af0a90ee155872a434cf899fcb3f143b' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='ca14c061fc03d73adccd9ddc4bcaeda58241c8a7fe7db4143f6fe20dd90b053a0e52bf9f75a568d2720fc9f9ec341a53fea3ba9c48240649cce545a411d2111e' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:9.0.0-preview.4-ubuntu-chiseled-amd64

COPY --from=installer ["/app", "/app"]
