ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.8-alpine3.13-amd64

# Install .NET
ENV DOTNET_VERSION=5.0.8

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='3763a6eb74c176dde843eeeebc37050dec8beaaa2e0d2205a9e010c6cdb1ed57bf0b44b6667badea5f9bee763c3fbd13904cdc4d4b156754fe488aef0502ad34' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
