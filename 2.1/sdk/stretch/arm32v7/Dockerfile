FROM arm32v7/buildpack-deps:stretch-scm

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
ENV DOTNET_SDK_VERSION 2.1.503

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='8a97790cdae1aa43dd214e78483fd2201268eb7bc61718832de766bd41741a6a04a33c5a612f4046bb315d2adf5dafaa9d62a6a96ded5af38c002b7c4e682a15' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    # Add NuGet cache (ARM SDK doesn't include it)
    && curl -SL --output /usr/share/dotnet/sdk/$DOTNET_SDK_VERSION/nuGetPackagesArchive.lzma https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/nuGetPackagesArchive.lzma \
    && lzma_sha512='fe8da1ebb79ddfb2dd0532db36a378a352cfd0c64812507684009bcc8ca7a777f7c3ab4f38375bb8fadb4efa298675ad3c7fa434bfdf22370c2608d7832bec9f' \
    && echo "$lzma_sha512 /usr/share/dotnet/sdk/$DOTNET_SDK_VERSION/nuGetPackagesArchive.lzma" | sha512sum -c -

# Install artifacts credential provider
ENV NUGET_CREDENTIAL_PROVIDER_VERSION 0.1.8

RUN curl -SL --output microsoft.nuget.credentialprovider.tar.gz https://github.com/Microsoft/artifacts-credprovider/releases/download/$NUGET_CREDENTIAL_PROVIDER_VERSION/Microsoft.NuGet.CredentialProvider.tar.gz \
    && credprovider_sha512='3c4c10d7aa5f9ca005b455fb380c2bd549432df5f4352eb58d1325de5c14db7f0d59fad6eef1c1f7a076804c32fd255ee84a1ac1d790f5a83adc537b74eee7c0' \
    && echo "$credprovider_sha512  microsoft.nuget.credentialprovider.tar.gz" | sha512sum -c - \
    && mkdir -p $HOME/.nuget \
    && tar -C $HOME/.nuget -xzf microsoft.nuget.credentialprovider.tar.gz plugins/netcore \
    && rm microsoft.nuget.credentialprovider.tar.gz

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
