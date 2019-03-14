FROM buildpack-deps:stretch-scm

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu57 \
        liblttng-ust0 \
        libssl1.0.2 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.2.105

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='b7ad26b344995de91848adec56bda5dfe5fef0b83abaa3e4376dc790cf9786e945b625de1ae4cecaf5c5bef86284652886ed87696581553aeda89ee2e2e99517' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Install artifacts credential provider
ENV NUGET_CREDENTIAL_PROVIDER_VERSION 0.1.15

RUN curl -SL --output microsoft.nuget.credentialprovider.tar.gz https://azureartifactassets.blob.core.windows.net/credentialprovider/releases/download/$NUGET_CREDENTIAL_PROVIDER_VERSION/Microsoft.NetCore2.NuGet.CredentialProvider.tar.gz \
    && credprovider_sha512='2257340f30e29449bbccd235ba7fafcecd646ec46da4a7a50373afdce4d025823d2e7251a314b90df6afcd430af4464b468dd6ca2b9dec6ecc83be061e77bd3d' \
    && echo "$credprovider_sha512  microsoft.nuget.credentialprovider.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/credentialprovider \
    && tar -C /usr/share/credentialprovider -xzf microsoft.nuget.credentialprovider.tar.gz \
    && rm microsoft.nuget.credentialprovider.tar.gz

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # NuGet Credential provider path
    NUGET_PLUGIN_PATHS=/usr/share/credentialprovider/plugins/netcore/CredentialProvider.Microsoft/CredentialProvider.Microsoft.dll \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
