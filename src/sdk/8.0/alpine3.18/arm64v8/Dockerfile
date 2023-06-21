ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:8.0.0-preview.6-alpine3.18-arm64v8

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=8.0.100-preview.6.23319.41 \
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Alpine-3.18-arm64

RUN apk add --no-cache \
        curl \
        icu-data-full \
        icu-libs \
        git

# Install .NET SDK
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='49e72d35609f0813065f27c56049518c97a555ce7cd01ebb8c84dbf1b7160feeceddf41f3cd557363a27677ea0255fced7fd382c35104136544456fdcb562cd0' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help
