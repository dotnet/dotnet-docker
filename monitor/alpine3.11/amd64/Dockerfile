ARG ASPNET_REPO=mcr.microsoft.com/dotnet/core/aspnet
ARG SDK_REPO=mcr.microsoft.com/dotnet/core/sdk

# Installer image
FROM $SDK_REPO:3.1-alpine3.11 AS installer

# Install .NET Monitor
ENV DOTNET_MONITOR_VERSION=5.0.0-preview.20279.1
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://pkgs.dev.azure.com/dnceng/public/_apis/packaging/feeds/dotnet5-transport%40Local/nuget/packages/dotnet-monitor/versions/$DOTNET_MONITOR_VERSION/content?api-version=5.1-preview.1 \
    && dotnetmonitor_sha512='fa36952ba359306f0617b216a5f6d0d9ab042b7b99678a171ff8b0fa7b15d264f51a7594af7d88a0dab9df75c5c86c7d4d31a3f2c71f1c2dfc9a902aeb818c5a' \
    && echo "$dotnetmonitor_sha512  dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg" | sha512sum -c - \
    && dotnet tool install dotnet-monitor --tool-path /app --add-source / --version $DOTNET_MONITOR_VERSION --no-cache \
    # To reduce image size, remove the copy of the nupkg under the tools.
    && find /app -print | grep -i '.*[.]nupkg$' | xargs rm \
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg


# Monitor image
FROM $ASPNET_REPO:3.1-alpine3.11

WORKDIR /app
COPY --from=installer /app .
ENV PATH="${PATH}:/app"

ENTRYPOINT ["dotnet-monitor", "collect"]
