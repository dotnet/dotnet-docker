FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.300-preview2-008380

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-alpine.3.6-x64.tar.gz \
    && dotnet_sha512='ee8a95cccb9151675ce5b777001c834d1bd0fc21ce7fa0c098cca516d4be461f0bdf26366226a6a8b64c3e1857da4fd82d0e3b826d761d2ed102d88701cb3b68' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER true

ENV NUGET_XMLDOC_MODE skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
