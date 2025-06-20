ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.1.1 \
    && curl --fail --show-error --location --output dotnet-monitor-egress-azureblobstorage.tar.gz https://ci.dot.net/public/diagnostics/monitor/8.1.1-servicing.25257.8/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='4011e41fdf1a08c2dcbc1ce2b20be10a126e7e2789f8e558bcc87d3a41f34e7c6481128775c51004d02608dcb1915db38119699f31f72ac718ab0f0b1159afb7' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl --fail --show-error --location --output dotnet-monitor-egress-s3storage.tar.gz https://ci.dot.net/public/diagnostics/monitor/8.1.1-servicing.25257.8/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='7da5d082f890b2c36e636f365153ad6229fb1da7c284c6c345fd0168ff6dae6729b5d92c901bf5954b619fc7184de58302be0f094ecdf0492442174e2a3107ab' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-azureblobstorage.tar.gz --directory /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-s3storage.tar.gz --directory /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.1.1-ubuntu-chiseled-arm64v8

COPY --from=installer ["/app", "/app"]
