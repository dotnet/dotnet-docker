ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.0.0-preview.4.23227.7 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='46de001f297cb8eba441ccb8439abca17cb40e18c41ce456c2951664a31d8c48e00aaacc6851dc806f0e8aa609e63b081aa718b61568162725e561054dd95696' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='4d87dec197c186e81a8ddf894ed3136a431d7bc2c8038bfb943a3b096b2b7d86ba2a955eec211dcbe01cb014cfe636ba3bc0c1f36e376cb65dbb719c0c84fa99' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.0.0-preview.4-ubuntu-chiseled-arm64v8

COPY --from=installer ["/app", "/app"]
