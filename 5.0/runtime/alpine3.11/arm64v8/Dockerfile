ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:5.0-alpine3.11-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 5.0.0-preview.4.20251.6

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='9c3a7b23dd4a0967c66807c899e3ebdc703d863f970ad98cb1ff8950f46a20304db5ba05218bd0b965b28cce5b07b2e790c3a8b89b2c787fdf93713ed8949151' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
