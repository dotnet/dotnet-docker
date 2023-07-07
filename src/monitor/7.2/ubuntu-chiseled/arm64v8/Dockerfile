ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Monitor
RUN dotnet_monitor_version=7.2.1 \
    && curl -fSL --output dotnet-monitor.tar.gz https://dotnetbuilds.azureedge.net/public/diagnostics/monitor/7.2.1-servicing.23356.4/dotnet-monitor-$dotnet_monitor_version-linux-arm64.tar.gz \
    && dotnet_monitor_sha512='d82d40d2125d4ddab7d212e45adf20a095f4862e1c3cfbf047fe49ea394d9b895a51a5d71211dc0f36fc6c071de5d0e93f7c96cc9e8c2ea9473d70cf15cccf7e' \
    && echo "$dotnet_monitor_sha512  dotnet-monitor.tar.gz" | sha512sum -c - \
    && mkdir -p /app \
    && tar -oxzf dotnet-monitor.tar.gz -C /app \
    && rm dotnet-monitor.tar.gz


# .NET Monitor image
FROM $REPO:7.0.8-jammy-chiseled-arm64v8

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
