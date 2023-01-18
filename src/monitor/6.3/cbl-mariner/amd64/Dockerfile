ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.13-cbl-mariner2.0-amd64

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
    Logging__Console__FormatterOptions__UseUtcTimestamp=true

WORKDIR /app

# Install .NET Monitor
RUN dotnet_monitor_version=6.3.2 \
    && curl -fSL --output dotnet-monitor.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/$dotnet_monitor_version/dotnet-monitor-$dotnet_monitor_version-linux-x64.tar.gz \
    && dotnet_monitor_sha512='a4704692cf27f7d83b45a7418f001ec9827efac14f9768d515f55d10ee801dae052f607a2c8f02e6f7ed4276c2b8bcd111bd7f6f0fe283208646c30ab9b360e9' \
    && echo "$dotnet_monitor_sha512  dotnet-monitor.tar.gz" | sha512sum -c - \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor.tar.gz -C /app \
    && rm dotnet-monitor.tar.gz

ENTRYPOINT [ "dotnet", "./dotnet-monitor.dll", "collect", "--urls", "https://+:52323", "--metricUrls", "http://+:52325" ]
