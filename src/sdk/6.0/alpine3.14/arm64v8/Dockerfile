ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.0-alpine3.14-arm64v8

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.100-rtm.21518.63 \
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Unset Logging__Console__FormatterName from aspnet base image
    Logging__Console__FormatterName= \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Alpine-3.14-arm64

RUN apk add --no-cache \
        curl \
        icu-libs \
        git

# Install .NET SDK
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='4b3de42d69ac16cf797297344c1472c539af634f6a9be9b89cd04d28c78a7e0e5ff6fdf989b79f679938559a1f7a6aa0adeaa85a59bfc2dd29ab49cbecdec782' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help
