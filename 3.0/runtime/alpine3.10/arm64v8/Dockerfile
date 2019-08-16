ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.0-alpine3.10-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-preview9-19415-13

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='4109dc375b4ef9e224e61fef17159c397cb7cc6fd0add83bde21cea99e9c98b2c9a3806f85c8bf35304810395b33ab9b56e4fe42450b33e5230dd8b156d3de40' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
