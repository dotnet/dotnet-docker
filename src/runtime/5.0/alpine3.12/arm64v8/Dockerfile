ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12-arm64v8

# Install .NET
ENV DOTNET_VERSION 5.0.0-rc.2.20471.9

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='f92eafdba571b40f0b832aa133bcfaa5c316e16753f61e1f68d3006c16fd8f47c8ca651477ead3b42ea1e44ff1398c8bcfea8974f8b25aff1680a794f3a2ac53' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
