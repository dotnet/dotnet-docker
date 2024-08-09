ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Monitor Base
RUN dotnet_monitor_version=8.1.0-alpha.1.24401.5 \
    && curl -fSL --output dotnet-monitor-base.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/$dotnet_monitor_version/dotnet-monitor-base-$dotnet_monitor_version-linux-x64.tar.gz \
    && dotnet_monitor_base_sha512='5c1b08ca01c011395c90c356e6ab1e8c266cb02568de290f0b9b2429f9ac41b2a6613ce8f2739e7bd7daeae0287090542fc036f1c95728b786eed5e83a6e68ba' \
    && echo "$dotnet_monitor_base_sha512  dotnet-monitor-base.tar.gz" | sha512sum -c - \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor-base.tar.gz -C /app \
    && rm dotnet-monitor-base.tar.gz


# .NET Monitor Base image
FROM $REPO:8.0.7-azurelinux3.0-distroless-amd64

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
