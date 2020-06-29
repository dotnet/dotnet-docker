ARG ASPNET_REPO=mcr.microsoft.com/dotnet/core/aspnet
ARG SDK_REPO=mcr.microsoft.com/dotnet/core/sdk

# Installer image
FROM $SDK_REPO:3.1-alpine3.12 AS installer

# Install .NET Monitor
ENV DOTNET_MONITOR_VERSION=5.0.0-preview.2.20326.1
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor5.0/dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg \
    && dotnetmonitor_sha512='dc92038b5e6633b4f0e35ccbd7206e35cc37ca18e3e4811e6e1942e9390f47499ed26cdf85ac817c1c775b70b1fbeb5ed3bf884d077821d62a5d0bfd6516ba94' \
    && echo "$dotnetmonitor_sha512  dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg" | sha512sum -c - \
    && dotnet tool install dotnet-monitor --tool-path /app --add-source / --version $DOTNET_MONITOR_VERSION --no-cache \
    # To reduce image size, remove the copy of the nupkg under the tools.
    && find /app -print | grep -i '.*[.]nupkg$' | xargs rm \
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg


# Monitor image
FROM $ASPNET_REPO:3.1-alpine3.12

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Disable debugger and profiler diagnostics to avoid diagnosing self.
    COMPlus_EnableDiagnostics=0 \
    # Add dotnet-monitor path to front of PATH for easier, prioritized execution
    PATH="/app:${PATH}"

ENTRYPOINT ["dotnet-monitor", "collect"]
