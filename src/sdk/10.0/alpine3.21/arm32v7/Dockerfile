ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:10.0.0-preview.2-alpine3.21-arm32v7 AS installer

# Install .NET SDK
RUN wget -O dotnet.tar.gz https://ci.dot.net/public/Sdk/10.0.100-preview.2.25157.2/dotnet-sdk-10.0.100-preview.2.25157.2-linux-musl-arm.tar.gz \
    && dotnet_sha512='4106599e2c964d18b2605854c47193f553d7fb7172afe1f98ebe095d0532092271b3e1a4c78ef056980ae87e8eb64d0fdd1199863487b982225918bba66bb6eb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:10.0.0-preview.2-alpine3.21-arm32v7

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=10.0.100-preview.2.25157.2 \
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
