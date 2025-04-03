ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:9.0.4-alpine3.20-arm32v7 AS installer

# Install .NET SDK
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Sdk/9.0.203/dotnet-sdk-9.0.203-linux-musl-arm.tar.gz \
    && dotnet_sha512='4d43cb65e398dbeed77613eea92ae0c2ccb4720b7b3db47afc144e310b3e76cc6da42e2a5415d9f1b98617240bc7331961b5a83fe8550dc7df5e06010a21267e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:9.0.4-alpine3.20-arm32v7

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=9.0.203 \
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Alpine-3.20-arm32

RUN apk add --upgrade --no-cache \
        curl \
        git \
        icu-data-full \
        icu-libs \
        libatomic \
        tzdata

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
