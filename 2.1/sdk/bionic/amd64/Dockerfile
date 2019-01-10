FROM buildpack-deps:bionic-scm

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu60 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.503

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='6accdf7a4e09b7d6b93d4df8484191df1290107cf396bfa85b3dd4a75596e50836143dd7cd10d0239244751a8704c2e4586f21d59361ecf527ef2cd4bc15225c' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Install artifacts credential provider
ENV NUGET_CREDENTIAL_PROVIDER_VERSION 0.1.8

RUN curl -SL --output microsoft.nuget.credentialprovider.tar.gz https://github.com/Microsoft/artifacts-credprovider/releases/download/$NUGET_CREDENTIAL_PROVIDER_VERSION/Microsoft.NuGet.CredentialProvider.tar.gz \
    && credprovider_sha512='3c4c10d7aa5f9ca005b455fb380c2bd549432df5f4352eb58d1325de5c14db7f0d59fad6eef1c1f7a076804c32fd255ee84a1ac1d790f5a83adc537b74eee7c0' \
    && echo "$credprovider_sha512  microsoft.nuget.credentialprovider.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/credentialprovider \
    && tar -C /usr/share/credentialprovider -xzf microsoft.nuget.credentialprovider.tar.gz plugins/netcore \
    && rm microsoft.nuget.credentialprovider.tar.gz

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # credential provider location
    NUGET_PLUGIN_PATHS=/usr/share/credentialprovider/plugins/netcore/CredentialProvider.Microsoft/CredentialProvider.Microsoft.dll

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
