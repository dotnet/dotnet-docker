ARG ASPNET_REPO=mcr.microsoft.com/dotnet/aspnet
ARG SDK_REPO=mcr.microsoft.com/dotnet/sdk

# Installer image
FROM $SDK_REPO:6.0.100-rc.2-alpine3.14-amd64 AS installer

# Install .NET Monitor
ENV DOTNET_MONITOR_VERSION=6.0.0-rc.1.21518.6
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://pkgs.dev.azure.com/dnceng/public/_apis/packaging/feeds/dotnet-tools/nuget/packages/dotnet-monitor/versions/$DOTNET_MONITOR_VERSION/content \
    && dotnetmonitor_sha512='d405a2756e353c56b3b0f3e32103ca75d8cabd9d91666f2e4ef057af7872cfa29a1efbc881f799f49774bb3b84deae0e18c746e3a13bbd61589fbc19c066b2ac' \
    && echo "$dotnetmonitor_sha512  dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg" | sha512sum -c - \
    && dotnet tool install dotnet-monitor --tool-path /app --add-source / --version $DOTNET_MONITOR_VERSION --framework net6.0 --no-cache \
    # To reduce image size, remove the copy of the nupkg under the tools.
    && find /app -print | grep -i '.*[.]nupkg$' | xargs rm \
    # To reduce image size, remove all non-net6.0 TFMs
    && find /app -print | grep -i '.*/netcoreapp3[.]1$' | xargs rm -rf \
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg


# Monitor image
FROM $ASPNET_REPO:6.0.0-rc.2-alpine3.14-amd64

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

ENTRYPOINT [ "dotnet-monitor", "collect", "-urls", "https://+:52323", "--metricUrls", "http://+:52325" ]
