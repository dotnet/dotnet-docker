ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.0.1 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/8.0.1-servicing.24081.5/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='5f9f451fce39e67215e654c22ef5d5c1fa4a729a39e1e9d5815cf9024b4d839c28e4fff7a45df7a2fe273158e267162fcc59c1264de148c3c1a309bad087603b' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/8.0.1-servicing.24081.5/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-x64.tar.gz \
    && dotnet_monitor_extension_sha512='b5bca9ae81bd0c08ff10a291f1c4ea3992985e65e31aefb5b2a6d8806aa5a79abd829551c6433435e3138414c558d89d5ef1ece4e1afd7e1418f0c1f5fb658c8' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.0.1-ubuntu-chiseled-amd64

COPY --from=installer ["/app", "/app"]
