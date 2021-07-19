ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-alpine3.14-arm64v8

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21368.2

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='7675c0876d5cb892af2f62bc76a77f8cd5cbff05200a7602ed40c867f08a5643a19dd8d2c9423d59d9aaae8dde61ec3cb886284aa61929e4e38208d520ad0c7e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
