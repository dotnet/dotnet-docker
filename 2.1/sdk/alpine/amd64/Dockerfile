FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.300-preview1-008123

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-alpine.3.6-x64.tar.gz \
    && dotnet_sha512='9a8a9bf060bbd8e6950e40e456f5a9556746b88407511bd60371b4c5a40e67abf675f42cd4ff69211e83fe59d9bad5b9efec92b5e49f7f17c608200c7fb05956' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps

# Trigger the population of the local package cache
ENV NUGET_XMLDOC_MODE skip
RUN mkdir warmup \
    && cd warmup \
    && dotnet new \
    && cd .. \
    && rm -rf warmup \
    && rm -rf /tmp/NuGetScratch

RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8
