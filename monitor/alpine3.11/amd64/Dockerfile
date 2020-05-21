ARG BASEREPO=mcr.microsoft.com/dotnet/core/aspnet
ARG STAGEREPO=mcr.microsoft.com/dotnet/core/sdk

# Install dotnet-monitor tool into SDK image. For .NET Core 3.1, only the SDK is able to install dotnet tools.
FROM $STAGEREPO:3.1-alpine3.11 AS stage
ENV DOTNET_MONITOR_VERSION=5.0.0-preview.20275.1
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN wget -O dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg https://pkgs.dev.azure.com/dnceng/public/_apis/packaging/feeds/dotnet5-transport%40Local/nuget/packages/dotnet-monitor/versions/$DOTNET_MONITOR_VERSION/content?api-version=5.1-preview.1 \
    && dotnetmonitor_sha512='35c8467574b4df8518b02a66f2f3ff7a22deaacbd1e4099577879fe21e3a695309a75179b2ced6332934ab29edcbf0f570b0c7f75d166b9f26b574865458b2ea' \
    && echo "$dotnetmonitor_sha512  dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg" | sha512sum -c - \
    && dotnet tool install dotnet-monitor -g --add-source / --version $DOTNET_MONITOR_VERSION --no-cache \    
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg

# To reduce image size, remove the copy of the nupkg under the tools.
RUN find /root/.dotnet/tools -print | grep -i '.*[.]nupkg$' | xargs rm

# Create tools image by copying tools from stage container
FROM $BASEREPO:3.1-alpine3.11
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY --from=stage /root/.dotnet/tools /root/.dotnet/tools

ENTRYPOINT ["dotnet-monitor", "collect"]