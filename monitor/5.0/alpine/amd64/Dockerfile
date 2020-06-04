ARG ASPNET_REPO=mcr.microsoft.com/dotnet/core/aspnet
ARG SDK_REPO=mcr.microsoft.com/dotnet/core/sdk

# Installer image
FROM $SDK_REPO:3.1-alpine3.12 AS installer

# Install .NET Monitor
ENV DOTNET_MONITOR_VERSION=5.0.0-preview.20303.1
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://pkgs.dev.azure.com/dnceng/public/_apis/packaging/feeds/dotnet5-transport%40Local/nuget/packages/dotnet-monitor/versions/$DOTNET_MONITOR_VERSION/content?api-version=5.1-preview.1 \
    && dotnetmonitor_sha512='a21a0d23c65ad0d180683730606fa918f1ba25316496cf5352fc7a34ad8e9577c86e9b804b7676bd8f1da07d0d1f0b8185c7e83bb4e8e70c0968353d0fd031b5' \
    && echo "$dotnetmonitor_sha512  dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg" | sha512sum -c - \
    && dotnet tool install dotnet-monitor --tool-path /app --add-source / --version $DOTNET_MONITOR_VERSION --no-cache \
    # To reduce image size, remove the copy of the nupkg under the tools.
    && find /app -print | grep -i '.*[.]nupkg$' | xargs rm \
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg


# Monitor image
FROM $ASPNET_REPO:3.1-alpine3.12

WORKDIR /app
COPY --from=installer /app .
ENV PATH="${PATH}:/app"

ENTRYPOINT ["dotnet-monitor", "collect"]
