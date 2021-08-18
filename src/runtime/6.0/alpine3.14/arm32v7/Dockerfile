ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.1-alpine3.14-arm32v7

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.1.21417.1

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='8911e0dcf370932dedff1741c59993a721b1d0d73bdb014339b9afcc9fb7083008c8997ccbfa01a4e8eb7762944910974cb3428cb9c01662564fa391dca6a2e4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
