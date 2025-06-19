ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.1.1 \
    && curl --fail --show-error --location --output dotnet-monitor-egress-azureblobstorage.tar.gz https://ci.dot.net/public/diagnostics/monitor/8.1.1-servicing.25257.8/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='072f71d2127a3302cd91677373d6e4b96af56a0b031b1215199e07657e6fc1965eed015fc8b8485d41735bd417d20d4d5007269ef5da022bcdda46551aa6388c' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum --check - \
    \
    && curl --fail --show-error --location --output dotnet-monitor-egress-s3storage.tar.gz https://ci.dot.net/public/diagnostics/monitor/8.1.1-servicing.25257.8/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='a23c9f60273cf24d1d2f5b0b676894741bf7e12977fcf62dc1ba81ca2469b24112b306d7da5d208c69974f19eb6a682d85bbc85db39e932cd09b3a295d87e168' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum --check - \
    \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-azureblobstorage.tar.gz --directory /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-egress-s3storage.tar.gz --directory /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.1.1-ubuntu-chiseled-amd64

COPY --from=installer ["/app", "/app"]
