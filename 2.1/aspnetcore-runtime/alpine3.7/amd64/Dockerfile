FROM microsoft/dotnet:2.1-runtime-deps-alpine3.7

# Workaround https://github.com/aspnet/libuv-package/issues/23
# Install libuv globally and link it so coreclr can load it
RUN apk add --no-cache libuv \
    && ln -s /usr/lib/libuv.so.1 /usr/lib/libuv.so

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.1-rtm-30843

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='d4431455bde9217652e39c1ffdbbc6067bc24c14590c42a178c51fc0cde153f8f1740dc3bbc3f2afd5b7c2e35c855d1974ad28b9ca311c9dd607cd76fea7ba42' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && apk del .build-deps
