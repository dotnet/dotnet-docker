ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.0.8 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://ci.dot.net/public/diagnostics/monitor/8.0.8-servicing.25104.1/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='d42e7bbf900f5432e76dae983f28f3bfbd0cb1eab0433197379edaee1fe013ad8d2f78df7502476f589a234521e8b1c8fd873de048a0d92eaa87f4f8b4cfc357' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://ci.dot.net/public/diagnostics/monitor/8.0.8-servicing.25104.1/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='0fcd310d795938c96d2c8b30fc4b705207b9e00f6463914d9d55e8640796861d719f71359b2b09d92fc1fe103307ed6fb4a3fe261e22b17df39ccee6607d42dc' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.0.8-ubuntu-chiseled-arm64v8

COPY --from=installer ["/app", "/app"]
