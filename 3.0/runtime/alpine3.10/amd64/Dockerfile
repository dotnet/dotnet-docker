ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.0-alpine3.10

# Install .NET Core
ENV DOTNET_VERSION 3.0.3

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='d62a1c9da4e38c1b2fe3f31180486008eff9b7b74d0ef8b1016c27c573bcf61a7b11a4874b9a1b67af3a16abef10ba0b993e8730e45cf51342b6281056a71fa3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
