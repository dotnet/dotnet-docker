ARG REPO=mcr.microsoft.com/dotnet/monitor/base

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor extensions
RUN dotnet_monitor_extension_version=8.0.2 \
    && curl -fSL --output dotnet-monitor-egress-azureblobstorage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/8.0.2-servicing.24256.5/dotnet-monitor-egress-azureblobstorage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='fee29475a3e54300dc7bce39bcd510f2b8975c073e511af7371dffdb69ad0113cd315011e1ee646be2064b9af7e98a23b496cc9d92bf22137797132c69a44de8' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-azureblobstorage.tar.gz" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-monitor-egress-s3storage.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/8.0.2-servicing.24256.5/dotnet-monitor-egress-s3storage-$dotnet_monitor_extension_version-linux-arm64.tar.gz \
    && dotnet_monitor_extension_sha512='513dfa2f2c1da2e29f11bb4adcd5e0e403459888c5b70667d4511981638f335deadf7559d7228d0ad2063e220b0e068146d9f6a4e411460a6fb16fd7c7306091' \
    && echo "$dotnet_monitor_extension_sha512  dotnet-monitor-egress-s3storage.tar.gz" | sha512sum -c - \
    \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-egress-azureblobstorage.tar.gz -C /app \
    && rm dotnet-monitor-egress-azureblobstorage.tar.gz \
    && tar -oxzf dotnet-monitor-egress-s3storage.tar.gz -C /app \
    && rm dotnet-monitor-egress-s3storage.tar.gz


# .NET Monitor image
FROM $REPO:8.0.2-ubuntu-chiseled-arm64v8

COPY --from=installer ["/app", "/app"]
