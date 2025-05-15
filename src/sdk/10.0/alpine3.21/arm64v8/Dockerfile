ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:10.0.0-preview.4-alpine3.21-arm64v8 AS installer

# Install .NET SDK
RUN wget -O dotnet.tar.gz https://ci.dot.net/public/Sdk/10.0.100-preview.4.25258.107/dotnet-sdk-10.0.100-preview.4.25258.107-linux-musl-arm64.tar.gz \
    && dotnet_sha512='78f6426365bbfe3e66ebcfcc92134cee8ee1814eaa73c06f801dd29c6cb0158b7d3b9f25539629de5897a607df8129a796c30e9a9ca7867ad99397c668ef88fc' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:10.0.0-preview.4-alpine3.21-arm64v8

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=10.0.100-preview.4.25258.107 \
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip 

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
