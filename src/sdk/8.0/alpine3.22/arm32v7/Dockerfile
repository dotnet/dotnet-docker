ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:8.0.17-alpine3.22-arm32v7 AS installer

# Install .NET SDK
RUN wget \
        https://builds.dotnet.microsoft.com/dotnet/Sdk/8.0.411/dotnet-sdk-8.0.411-linux-musl-arm.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/checksums/8.0.17-sha.txt \
    && awk -v file="dotnet-sdk-8.0.411-linux-musl-arm.tar.gz" '{gsub(/\r/, "")} $2 == file' 8.0.17-sha.txt | sha512sum -c \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-sdk-8.0.411-linux-musl-arm.tar.gz --directory /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm \
        dotnet-sdk-8.0.411-linux-musl-arm.tar.gz \
        8.0.17-sha.txt


# .NET SDK image
FROM $REPO:8.0.17-alpine3.22-arm32v7

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=8.0.411 \
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Alpine-3.22-arm32

RUN apk add --upgrade --no-cache \
        curl \
        git \
        icu-data-full \
        icu-libs \
        tzdata

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
