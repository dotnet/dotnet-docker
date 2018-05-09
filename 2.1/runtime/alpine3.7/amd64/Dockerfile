FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine3.7

# Install .NET Core
ENV DOTNET_VERSION 2.1.0-rtm-26508-02

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='b133b15869e94ec1f5854660c62c810883522832c2182dc524418a41717ee8b7551a80d6b46f9ba7f1d4d3b7aa645dd965066df46e212b778148b34b226c9fe5' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps
