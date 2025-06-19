ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install --assumeyes \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor Base
RUN dotnet_monitor_version=8.1.1 \
    && curl --fail --show-error --location --output dotnet-monitor-base.tar.gz https://ci.dot.net/public/diagnostics/monitor/8.1.1-servicing.25257.8/dotnet-monitor-base-$dotnet_monitor_version-linux-arm64.tar.gz \
    && dotnet_monitor_base_sha512='b41db3c12fa66c20afb6dbd57b2468f0106010091d01393071b3263095ec5cd58cdec58a87c67554dacbfde20c9ad3a34d9cdc22a4ce0a126ebba6e19bb03ef6' \
    && echo "$dotnet_monitor_base_sha512  dotnet-monitor-base.tar.gz" | sha512sum --check - \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file dotnet-monitor-base.tar.gz --directory /app \
    && rm dotnet-monitor-base.tar.gz


# .NET Monitor Base image
FROM $REPO:8.0.17-azurelinux3.0-distroless-arm64v8

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
