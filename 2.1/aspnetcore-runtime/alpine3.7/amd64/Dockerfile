ARG REPO=microsoft/dotnet
FROM $REPO:2.1-runtime-deps-alpine3.7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.7-servicing-31150

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='f0b809dd18b240ddc00f69f6706475f8f0107fa2020eb0bfea92bde04d80ec459e2008d88021ba7ad94751d6732b4bd952c241c5e64ee13c3ec35b389fa6cbda' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && apk del .build-deps
