ARG ASPNET_REPO=mcr.microsoft.com/dotnet/aspnet
ARG SDK_REPO=mcr.microsoft.com/dotnet/sdk

# Installer image
FROM $SDK_REPO:6.0.101-alpine3.15-amd64 AS installer

# Install .NET Monitor
ENV DOTNET_MONITOR_VERSION=7.0.0-alpha.1.22070.6
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$DOTNET_MONITOR_VERSION/dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg \
    && dotnetmonitor_sha512='97f1e327ce7138f98aab37ec8656dc3554fadfe60dbd75c810b2e30eef81ea418ee64780d07ee649b63da2f554565e9b68e8a8ffcb2b4c3a1536eb9bad978fc7' \
    && echo "$dotnetmonitor_sha512  dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg" | sha512sum -c - \
    && dotnet tool install dotnet-monitor --tool-path /app --add-source / --version $DOTNET_MONITOR_VERSION --framework net6.0 --no-cache \
    # To reduce image size, remove the copy of the nupkg under the tools.
    && find /app -print | grep -i '.*[.]nupkg$' | xargs rm \
    # To reduce image size, remove all non-net6.0 TFMs
    && find /app -print | grep -i '.*/netcoreapp3[.]1$' | xargs rm -rf \
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg


# Monitor image
FROM $ASPNET_REPO:6.0.1-alpine3.15-amd64

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Disable debugger and profiler diagnostics to avoid diagnosing self.
    COMPlus_EnableDiagnostics=0 \
    # Default Filter
    DefaultProcess__Filters__0__Key=ProcessId \
    DefaultProcess__Filters__0__Value=1 \
    # Logging: JSON format so that analytic platforms can get discrete entry information
    Logging__Console__FormatterName=json \
    # Logging: Use round-trip date/time format without timezone information (always logged in UTC)
    Logging__Console__FormatterOptions__TimestampFormat=yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z' \
    # Logging: Write timestamps using UTC offset (+0:00)
    Logging__Console__FormatterOptions__UseUtcTimestamp=true \
    # Add dotnet-monitor path to front of PATH for easier, prioritized execution
    PATH="/app:${PATH}"

RUN chmod 755 dotnet-monitor

ENTRYPOINT [ "dotnet-monitor", "collect", "--urls", "https://+:52323", "--metricUrls", "http://+:52325" ]
