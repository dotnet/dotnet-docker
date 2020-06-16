ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.1-alpine3.11

# Install .NET Core
ENV DOTNET_VERSION 2.1.19

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='438d48589ce292403195603b45eb377f6d3490626c591ee83844f51b10ab2df24d077178ffc58065d1b9b54236dd104147094ca59b6229e3064124bec72e46f9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
