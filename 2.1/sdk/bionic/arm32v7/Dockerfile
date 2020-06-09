FROM arm32v7/buildpack-deps:bionic-scm

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
ENV DOTNET_SDK_VERSION 2.1.807

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='0b3e80abf6895d46cee8ad5c81aa7968cc6876f5a98c79ce6ad5f28fb43208aff8e5f2bca618c36577bf9f91d1f7a4f962accbe2ab472f713ba8e503519802b0' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    # Add NuGet cache (ARM SDK doesn't include it)
    && curl -SL --output /usr/share/dotnet/sdk/$DOTNET_SDK_VERSION/nuGetPackagesArchive.lzma https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/nuGetPackagesArchive.lzma \
    && lzma_sha512='f6038cd8524d0b9389704687f603fd1d68fab3b2dff1650829d2ce253a754b681c83aa93388124c463e71d5f9413325c7a4b599d453b1bbddb8f25bdbc25f5be' \
    && echo "$lzma_sha512 /usr/share/dotnet/sdk/$DOTNET_SDK_VERSION/nuGetPackagesArchive.lzma" | sha512sum -c -

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
