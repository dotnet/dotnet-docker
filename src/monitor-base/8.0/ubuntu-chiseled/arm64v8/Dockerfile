ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor Base
RUN dotnet_monitor_version=8.0.2 \
    && curl -fSL --output dotnet-monitor-base.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/8.0.2-servicing.24328.6/dotnet-monitor-base-$dotnet_monitor_version-linux-arm64.tar.gz \
    && dotnet_monitor_base_sha512='2f0c743c6e2069d52b7c3f283fa4aeb4612bcde27b282f4dd549f85bd9082ce2037086a7e0cf5774ea59e1a0800e68e645a910369207fe23c6fcfe73e6b62ccb' \
    && echo "$dotnet_monitor_base_sha512  dotnet-monitor-base.tar.gz" | sha512sum -c - \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-base.tar.gz -C /app \
    && rm dotnet-monitor-base.tar.gz


# .NET Monitor Base image
FROM $REPO:8.0.6-jammy-chiseled-arm64v8

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from aspnet base image
    ASPNETCORE_HTTP_PORTS= \
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

ENTRYPOINT [ "dotnet-monitor" ]
CMD [ "collect", "--urls", "https://+:52323", "--metricUrls", "http://+:52325" ]
