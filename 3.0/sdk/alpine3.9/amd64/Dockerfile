ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.0-alpine3.9

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 3.0.100-preview-010184

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='a51cd98ac12b8cc3fe58334602768ab998f6ba654acaaba2a13eddb362cf174433ba6c72931a6239555ce4a8ffa4cbcc8f091c5d06154acde26685f0d6bdb10a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

# Install artifacts credential provider
ENV NUGET_CREDENTIAL_PROVIDER_VERSION 0.1.8

RUN wget -O microsoft.nuget.credentialprovider.tar.gz https://github.com/Microsoft/artifacts-credprovider/releases/download/$NUGET_CREDENTIAL_PROVIDER_VERSION/Microsoft.NuGet.CredentialProvider.tar.gz \
    && credprovider_sha512='3c4c10d7aa5f9ca005b455fb380c2bd549432df5f4352eb58d1325de5c14db7f0d59fad6eef1c1f7a076804c32fd255ee84a1ac1d790f5a83adc537b74eee7c0' \
    && echo "$credprovider_sha512  microsoft.nuget.credentialprovider.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/credentialprovider \
    && tar -C /usr/share/credentialprovider -xzf microsoft.nuget.credentialprovider.tar.gz plugins/netcore \
    && rm microsoft.nuget.credentialprovider.tar.gz

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \ 
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # Nuget Credential provider path
    NUGET_PLUGIN_PATHS=/usr/share/credentialprovider/plugins/netcore/CredentialProvider.Microsoft/CredentialProvider.Microsoft.dll

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
