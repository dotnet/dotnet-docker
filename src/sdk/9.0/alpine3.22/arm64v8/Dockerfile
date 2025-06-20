ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:9.0.6-alpine3.22-arm64v8 AS installer

# Install .NET SDK
RUN wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Sdk/9.0.301/dotnet-sdk-9.0.301-linux-musl-arm64.tar.gz \
    && dotnet_sha512='b782638f6f0d14ab910de395274b169664c652b534e476b39d219c55a46d6314ee51d4a2ca4709bec045ac1eb277f389b290eac5c9a6b75e501be42c071563f7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:9.0.6-alpine3.22-arm64v8

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=9.0.301 \
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Alpine-3.22-arm64

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
