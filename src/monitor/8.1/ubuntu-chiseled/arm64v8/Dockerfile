ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.1.2 \
    && curl --fail --show-error --location \
        --output dotnet-monitor-egress-azureblobstorage.tar.gz https://builds.dotnet.microsoft.com/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
        --output dotnet-monitor-egress-s3storage.tar.gz https://builds.dotnet.microsoft.com/dotnet/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_azureblobstorage_sha512='f71d208f0b0db9b55fc35dd39506cf1c92416d40b0d5426f998d843f01a8385015ed23de231e39f9db2d500f41a8cb85556bb0d0f8f04f6b75d800dab7c7d52d' \
    && echo "$dotnet_monitor_azureblobstorage_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    && dotnet_monitor_s3storage_sha512='41da37d09dc1c6cc0431dbd385762b4146df213e0c40aeb712235d116d1b7fa213ab08d37a0f7fcfb28fbdf686d5cc43c19ea0ffd4bda4a7f4a8cc82ef46bb71' \
    && echo "$dotnet_monitor_s3storage_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-azureblobstorage.tar.gz --directory /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-s3storage.tar.gz --directory /app \
    && rm \
        dotnet-monitor-egress-azureblobstorage.tar.gz \
        dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.1.2-ubuntu-chiseled-arm64v8

COPY --from=installer ["/app", "/app"]
