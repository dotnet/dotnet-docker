FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine3.7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.6-servicing-31078

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='82faabdb70c7a1c29685cf9792b5f041c519f10fc799740b14b573e6f24ae8da8293367936eeba099d33ed0386532c1d7673421b1455090a4e4d36dc333e19e2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && apk del .build-deps
