ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.2-alpine3.9

# Install .NET Core
ENV DOTNET_VERSION 2.2.8

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='d0f8e7ac385e7fcaca2a70b1081625be88289e06f031ce12955f0d6df0b6ff2f13e6d93287e30439bb19932b2a06a9d1162577579c9c85da435c4036c609659a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
