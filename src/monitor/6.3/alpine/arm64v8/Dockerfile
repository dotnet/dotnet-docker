ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.22-alpine3.18-arm64v8

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
    # Logging: JSON format so that analytic platforms can get discrete entry information
    Logging__Console__FormatterName=json \
    # Logging: Use round-trip date/time format without timezone information (always logged in UTC)
    Logging__Console__FormatterOptions__TimestampFormat=yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z' \
    # Logging: Write timestamps using UTC offset (+0:00)
    Logging__Console__FormatterOptions__UseUtcTimestamp=true \
    # Add dotnet-monitor path to front of PATH for easier, prioritized execution
    PATH="/app:${PATH}"

# Install .NET Monitor
RUN dotnet_monitor_version=6.3.3 \
    && wget -O dotnet-monitor.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/6.3.3-servicing.23502.4/dotnet-monitor-$dotnet_monitor_version-linux-musl-arm64.tar.gz \
    && dotnet_monitor_sha512='ed6a7f74492ae2a13b88ad4edb9a8b5826804ab5b8ca10840c1e22d862eb13f72cf58707b20680027aecb3a7b6372369384ac84cadc67567f928a08cce23e173' \
    && echo "$dotnet_monitor_sha512  dotnet-monitor.tar.gz" | sha512sum -c - \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor.tar.gz -C /app \
    && rm dotnet-monitor.tar.gz

ENTRYPOINT [ "dotnet-monitor", "collect", "--urls", "https://+:52323", "--metricUrls", "http://+:52325" ]
