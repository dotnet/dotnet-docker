ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor
RUN dotnet_monitor_version=6.3.5 \
    && curl -fSL --output dotnet-monitor.tar.gz https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor/$dotnet_monitor_version/dotnet-monitor-$dotnet_monitor_version-linux-x64.tar.gz \
    && dotnet_monitor_sha512='4834f0f2a0d0037b3c3cbb4cc25875a3da175b54866ba7c731c1e967c3dfdde40def8ac434b71bbf8e591ed69df6bc89c9772cf688fa444ea1daf655bfd2a5e3' \
    && echo "$dotnet_monitor_sha512  dotnet-monitor.tar.gz" | sha512sum -c - \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor.tar.gz -C /app \
    && rm dotnet-monitor.tar.gz


# .NET Monitor image
FROM $REPO:6.0.29-cbl-mariner2.0-distroless-amd64

COPY --from=installer ["/app", "/app"]

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

ENTRYPOINT [ "dotnet-monitor", "collect", "--urls", "https://+:52323", "--metricUrls", "http://+:52325" ]
