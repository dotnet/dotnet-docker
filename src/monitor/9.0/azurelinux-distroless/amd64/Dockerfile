ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install --assumeyes \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=9.0.3 \
    && curl --fail --show-error --location --output dotnet-monitor-egress-azureblobstorage.tar.gz https://ci.dot.net/public/diagnostics/monitor/9.0.3-servicing.25257.4/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='cc8b1dafcb632fb3a826e9b57fb71519c7918d0debd5e2172f82c430ac8c20df73c63fefb82179768c6b75d346cc3eac026b96e41e1bad19c94b283f771934fc' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum --check - \
    \
    && curl --fail --show-error --location --output dotnet-monitor-egress-s3storage.tar.gz https://ci.dot.net/public/diagnostics/monitor/9.0.3-servicing.25257.4/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='23df20e9cbaa82137d5860f51251de57922e9a4ab02d8ac9fa714f0a5dd98c0a7a8455bfb8bce70fdb0fd56070c394407ea43a4a163a20df47cb245db180e1e2' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum --check - \
    \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-azureblobstorage.tar.gz --directory /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-s3storage.tar.gz --directory /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:9.0.3-amd64

COPY --from=installer ["/app", "/app"]
