FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine3.7

# Install .NET Core
ENV DOTNET_VERSION 2.1.6-servicing-27017-02

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='56d9356e40a5c4793109921b0847dd919767a96600ab69285b9e9cfccfbfe3d16457e4aed04f2448e5b0406af45b94baa8b859475e58f0a72fe569b022917c5d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps
