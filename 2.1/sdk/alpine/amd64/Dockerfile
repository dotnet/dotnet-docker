FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.300-preview1-008162

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-alpine.3.6-x64.tar.gz \
    && dotnet_sha512='f1ceda789db6ede8398d1a87ad4e3ff90b2fefab5e2504a8522a445e7ef4c94b8d1fb5f52df344932360c52d03e7ac1a179c12fdabeb8c979329350b824929e3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps

ENV NUGET_XMLDOC_MODE skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
