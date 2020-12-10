ARG ASPNET_REPO=mcr.microsoft.com/dotnet/aspnet
ARG SDK_REPO=mcr.microsoft.com/dotnet/sdk

# Installer image
FROM $SDK_REPO:3.1-alpine3.12 AS installer

# Install .NET Monitor
ENV DOTNET_MONITOR_VERSION=5.0.0-preview.3.20609.1
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://pkgs.dev.azure.com/dnceng/public/_apis/packaging/feeds/dotnet-tools/nuget/packages/dotnet-monitor/versions/$DOTNET_MONITOR_VERSION/content \
    && dotnetmonitor_sha512='da5d0172416b5f35e1c5408081d7b8a06dab7a958d907c91c6df480c2cdd4dd1501838885bd9d1b9b635a3bf499e4422311ffa009b3c360dee1f4197437a0893' \
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
