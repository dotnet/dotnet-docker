ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.7-alpine3.14-arm32v7

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21373.17

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='86b6d7def685f35b49c279d52943b8f8e9c08dcbce60eeef70484049d5537a257bd5a7080078c931985acc429dae6827ce5c1ddda497b0102f8c42d40448ab5e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
