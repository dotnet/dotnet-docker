ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.1.0-alpha.1.24162.6 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='5d68933c2d9403e50330b45fac1d850069c6231f96e5b237c4be0c5ef49dddd50dc9720ccdde5f41ede1afbbfcd3883eb9676cfa2ffe3a664a885a61738d37a7' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/$dotnet_monitor_extension_version/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='5d203eaa0ecff1f527d3e07828f5a336c48ff66f6777314e3671e1602001971b04efdd709dc37353bbae69ffd090a1fbb6ebd666368cc4bea535bcfb9f77fd0c' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.1.0-alpha.1-ubuntu-chiseled-amd64

COPY --from=installer ["/app", "/app"]
