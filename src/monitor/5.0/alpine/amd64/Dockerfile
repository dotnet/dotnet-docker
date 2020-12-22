ARG ASPNET_REPO=mcr.microsoft.com/dotnet/aspnet
ARG SDK_REPO=mcr.microsoft.com/dotnet/sdk

# Installer image
FROM $SDK_REPO:3.1-alpine3.12 AS installer

# Install .NET Monitor
ENV DOTNET_MONITOR_VERSION=5.0.0-preview.3.20621.2
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://pkgs.dev.azure.com/dnceng/public/_apis/packaging/feeds/dotnet-tools/nuget/packages/dotnet-monitor/versions/$DOTNET_MONITOR_VERSION/content \
    && dotnetmonitor_sha512='886471b8ddfb8a19ea4cf0baac6cb16c8edfd368d12b60b3cd9e96bd322daa95a4a6be0a735f7878118c33a506c5149cec78b473a9f5f4ee8294a17c03d8ce4c' \
    && echo "$dotnetmonitor_sha512  dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg" | sha512sum -c - \
    && dotnet tool install dotnet-monitor --tool-path /app --add-source / --version $DOTNET_MONITOR_VERSION --no-cache \
    # To reduce image size, remove the copy of the nupkg under the tools.
    && find /app -print | grep -i '.*[.]nupkg$' | xargs rm \
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg


# Monitor image
FROM $ASPNET_REPO:3.1-alpine3.12

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Disable debugger and profiler diagnostics to avoid diagnosing self.
    COMPlus_EnableDiagnostics=0 \
    # Add dotnet-monitor path to front of PATH for easier, prioritized execution
    PATH="/app:${PATH}"

RUN chmod 755 dotnet-monitor

ENTRYPOINT ["dotnet-monitor", "collect"]
