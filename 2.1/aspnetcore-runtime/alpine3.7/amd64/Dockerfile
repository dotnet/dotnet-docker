FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine3.7

# Workaround https://github.com/aspnet/libuv-package/issues/23
# Install libuv globally and link it so coreclr can laod it
RUN apk add --no-cache libuv \
    && ln -s /usr/lib/libuv.so.1 /usr/lib/libuv.so

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.0

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='001cb500bfbc1bc5843943979101f402f0bc575bce4f87cfa301b4ec7ded801a3376520d80de87990cbf21f1617647fb40dbf21051c6627549bd365232bfa5f7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && apk del .build-deps
