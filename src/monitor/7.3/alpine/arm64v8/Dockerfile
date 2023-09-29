ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:7.0.11-alpine3.18-arm64v8

WORKDIR /app

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Disable debugger and profiler diagnostics to avoid diagnosing self.
    COMPlus_EnableDiagnostics=0 \
    # Default Filter
    DefaultProcess__Filters__0__Key=ProcessId \
    DefaultProcess__Filters__0__Value=1 \
    # Remove Unix Domain Socket before starting diagnostic port server
    DiagnosticPort__DeleteEndpointOnStartup=true \
    # Server GC mode
    DOTNET_gcServer=1 \
    # Logging: JSON format so that analytic platforms can get discrete entry information
    Logging__Console__FormatterName=json \
    # Logging: Use round-trip date/time format without timezone information (always logged in UTC)
    Logging__Console__FormatterOptions__TimestampFormat=yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z' \
    # Logging: Write timestamps using UTC offset (+0:00)
    Logging__Console__FormatterOptions__UseUtcTimestamp=true \
    # Add dotnet-monitor path to front of PATH for easier, prioritized execution
    PATH="/app:${PATH}"

# Install .NET Monitor
RUN dotnet_monitor_version=7.3.1 \
    && wget -O dotnet-monitor.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/7.3.1-servicing.23478.8/dotnet-monitor-$dotnet_monitor_version-linux-musl-arm64.tar.gz \
    && dotnet_monitor_sha512='e77a9552e55eb94bbce749b746df5bc1534c89d2b1e77797249757af9635eba9aad1ada41256e13fa2a6b33c28432c522043961b5bb99c73b7e3eb4140c369ca' \
    && echo "$dotnet_monitor_sha512  dotnet-monitor.tar.gz" | sha512sum -c - \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor.tar.gz -C /app \
    && rm dotnet-monitor.tar.gz

ENTRYPOINT [ "dotnet-monitor" ]
CMD [ "collect", "--urls", "https://+:52323", "--metricUrls", "http://+:52325" ]
