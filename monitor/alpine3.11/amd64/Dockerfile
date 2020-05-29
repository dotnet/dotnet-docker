ARG BASEREPO=mcr.microsoft.com/dotnet/core/aspnet
ARG INSTALLERREPO=mcr.microsoft.com/dotnet/core/sdk

# Install dotnet-monitor tool into SDK image. For .NET Core 3.1, only the SDK is able to install dotnet tools.
FROM $INSTALLERREPO:3.1-alpine3.11 AS installer
ENV DOTNET_MONITOR_VERSION=5.0.0-preview.20278.1
# 'dotnet tool install' requires package versions to be in the file name.
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://pkgs.dev.azure.com/dnceng/public/_apis/packaging/feeds/dotnet5-transport%40Local/nuget/packages/dotnet-monitor/versions/$DOTNET_MONITOR_VERSION/content?api-version=5.1-preview.1 \
    && dotnetmonitor_sha512='26d5b43b5bf9ea8dbdd810e6892481a23774490f23bc3e9ea08b9b50c5e5b66111210b90794a846a0ca696d1ed6835976cf453c71928129851aa51149103568e' \
    && echo "$dotnetmonitor_sha512  dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg" | sha512sum -c - \
    && dotnet tool install dotnet-monitor --tool-path /app --add-source / --version $DOTNET_MONITOR_VERSION --no-cache \    
    # To reduce image size, remove the copy of the nupkg under the tools.
    && find /app -print | grep -i '.*[.]nupkg$' | xargs rm \
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg

# Create tools image by copying tools from installer container.
FROM $BASEREPO:3.1-alpine3.11
COPY --from=installer /app /app

ENTRYPOINT ["/app/dotnet-monitor", "collect"]
