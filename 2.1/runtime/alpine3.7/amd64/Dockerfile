FROM microsoft/dotnet:2.1-runtime-deps-alpine3.7

# Install .NET Core
ENV DOTNET_VERSION 2.1.1-servicing-26605-02

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='262fd085ad147c296617649f843701c2b7c420fedc4167b88bf6fd589bd41eb4ba171bbf296f0f4587c2d50814995a573dfc90beed91adea6cb6211386a2c1aa' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps
