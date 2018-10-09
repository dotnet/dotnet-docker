FROM microsoft/dotnet-nightly:2.2-runtime-deps-alpine3.8

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.2.100-preview3-009423

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='e9a7231759e844932fa114ab2749dcee101011e3426c70e00e38fb3fff28e9632d2106b217dc9670ba6c77b81522f5b7c19258ba8325f777fb5b4087df05a316' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
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
