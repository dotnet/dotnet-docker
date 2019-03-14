ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.1-alpine3.9

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.505

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='3b672c7b7cc0ea05dbaa4b0aed10e3549d156cfd7fba4082eb58f7327934c93a7ea98422badd54f15a263a68717245a8cbcdc8911ce954aaa6683c46458d11cc' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

# Install artifacts credential provider
ENV NUGET_CREDENTIAL_PROVIDER_VERSION 0.1.15

RUN wget -O microsoft.nuget.credentialprovider.tar.gz https://azureartifactassets.blob.core.windows.net/credentialprovider/releases/download/$NUGET_CREDENTIAL_PROVIDER_VERSION/Microsoft.NetCore2.NuGet.CredentialProvider.tar.gz \
    && credprovider_sha512='2257340f30e29449bbccd235ba7fafcecd646ec46da4a7a50373afdce4d025823d2e7251a314b90df6afcd430af4464b468dd6ca2b9dec6ecc83be061e77bd3d' \
    && echo "$credprovider_sha512  microsoft.nuget.credentialprovider.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/credentialprovider \
    && tar -C /usr/share/credentialprovider -xzf microsoft.nuget.credentialprovider.tar.gz \
    && rm microsoft.nuget.credentialprovider.tar.gz

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \ 
    # NuGet Credential provider path
    NUGET_PLUGIN_PATHS=/usr/share/credentialprovider/plugins/netcore/CredentialProvider.Microsoft/CredentialProvider.Microsoft.dll \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
