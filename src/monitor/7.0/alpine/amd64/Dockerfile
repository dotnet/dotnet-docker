ARG ASPNET_REPO=mcr.microsoft.com/dotnet/aspnet
ARG SDK_REPO=mcr.microsoft.com/dotnet/sdk

# Installer image
FROM $SDK_REPO:7.0.100-preview.1-alpine3.15-amd64 AS installer

# Install .NET Monitor
ENV DOTNET_MONITOR_VERSION=7.0.0-preview.1.22104.6
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$DOTNET_MONITOR_VERSION/dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg \
    && dotnetmonitor_sha512='65419fa73a6bc9f07bbd08bbe73cf42a64b801f9d9fb1fc26527b7e415f035b28b6d1dce53772993fbbfffd36b8443297dde179c4bb5ddbc72b337f9d957aaf1' \
    && echo "$dotnetmonitor_sha512  dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg" | sha512sum -c - \
    && dotnet tool install dotnet-monitor --tool-path /app --add-source / --version $DOTNET_MONITOR_VERSION --framework net7.0 --no-cache \
    # To reduce image size, remove all non-net7.0 TFMs
    # To do this safely, we need to first find everything that dotnet tool installed named "dotnet-monitor".
    # The /dotnet-monitor/ folder exists under .store which is not a stable constant to rely on. The result is multi stage search:
    # 1. Find any files in /app
    # 2. Match anything that is under a *folder* called 'dotnet-monitor'
    # 3. Match anything from step 2 that isn't in a '/tools/net7.0' folder (/tools is the folder we use in a nuget file that is stable)
    # 4. Delete everything from step 3
    && find /app -type f -print | grep -i '.*/dotnet-monitor/.*' | grep -v -i '.*/tools/net7[.]0' | xargs rm -rf \
    # To reduce image size further, remove the non-linux assemblies
    && find /app -type f -print | grep -i '.*/dotnet-monitor/.*' | grep -i -E '/(runtimes|shims)/(win|osx)' | xargs rm -rf \
    # Remove all the empty directories left by the previous step
    && find /app -type d -empty -delete \
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg


# Monitor image
FROM $ASPNET_REPO:7.0.0-preview.1-alpine3.15-amd64

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
