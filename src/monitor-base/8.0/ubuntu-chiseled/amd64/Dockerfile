ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor Base
RUN dotnet_monitor_version=8.0.2 \
    && curl -fSL --output dotnet-monitor-base.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/8.0.2-servicing.24328.6/dotnet-monitor-base-$dotnet_monitor_version-linux-x64.tar.gz \
    && dotnet_monitor_base_sha512='cc57bc8d2335e2dd85d61fb5c2d6e30f0fb0d8974d08997c29ca196fdc9c2c6b36f9bab45aca5410ddd66cfe99236d155b751fdd20f35b08bc3a5f2423d07aae' \
    && echo "$dotnet_monitor_base_sha512  dotnet-monitor-base.tar.gz" | sha512sum -c - \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-base.tar.gz -C /app \
    && rm dotnet-monitor-base.tar.gz


# .NET Monitor Base image
FROM $REPO:8.0.6-jammy-chiseled-amd64

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
