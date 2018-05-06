FROM microsoft/dotnet:2.1-runtime-deps-alpine3.7

# Workaround https://github.com/aspnet/libuv-package/issues/23
# Install libuv globally and link it so coreclr can laod it
RUN apk add --no-cache libuv \
    && ln -s /usr/lib/libuv.so.1 /usr/lib/libuv.so

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.0-rc1-30678

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-alpine.3.6-x64.tar.gz \
    && aspnetcore_sha512='0b53ef74883c1753607c2e8687f98facaa26b15110de91fe0245190ecf12e7f79eaede2b48830593165033f3cc17cf70a7806e53ad093a9223bb5c1c68c9edcc' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && apk del .build-deps
