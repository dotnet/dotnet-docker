ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.1-alpine3.10-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 3.1.0-preview2.19525.6

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='6fdd6bc8d383024bd07ceccf618e3a30e7be87f133b1b14fdccb1e31ef62ccddf1e64a321d1a3e7b81816be5fb958b40df9ed172941425981db7bb20817d2ecd' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
