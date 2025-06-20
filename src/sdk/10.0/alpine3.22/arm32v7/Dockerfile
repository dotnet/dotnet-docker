ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm32v7 AS installer

# Install .NET SDK
RUN wget --output-document dotnet.tar.gz https://ci.dot.net/public/Sdk/10.0.100-preview.6.25274.104/dotnet-sdk-10.0.100-preview.6.25274.104-linux-musl-arm.tar.gz \
    && dotnet_sha512='93d3470dd1197442b9a66fa0f56a2f7e0ffd494711d29c3bcbd2d673e4a02f344506b303b424fd28101d1fb3090d25915ee991fc7f64f4fd5dbcc36cd135f8b7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm32v7

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=10.0.100-preview.6.25274.104 \
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
