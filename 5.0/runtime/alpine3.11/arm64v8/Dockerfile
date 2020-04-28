ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:5.0-alpine3.11-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 5.0.0-preview.4.20223.9

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='8425f19b5dba474f821d9b29c9e946430da2a7af7062aabb4ef32f2b7bdfd6372ce92f3c11614ed22ec06ea07bd6d0285e123c70dc2d845aeeef207a85c2d994' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
