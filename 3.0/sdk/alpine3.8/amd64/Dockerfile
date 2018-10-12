FROM microsoft/dotnet-nightly:3.0-runtime-deps-alpine3.8

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 3.0.100-alpha1-009642

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='21561bcbe5d23b668dc41f40ba60e10af25b20628163428a36332b5bc004cd1d281c41aaa2c685cacedb2a6c4a06a02445d443eb809b803ddced193f0bf66446' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \ 
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # Set dotnet sdk home path
    DOTNET_CLI_HOME=/var/cache \
    # Append dotnet global tools path to PATH
    PATH=${PATH}:/var/cache/.dotnet/tools

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
