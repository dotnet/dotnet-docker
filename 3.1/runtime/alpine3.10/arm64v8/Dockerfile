ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.1-alpine3.10-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 3.1.0-preview3.19530.5

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='77005f5a854acb31fd63977d21d1e082212b6a31cd46533cf73c806ea30777f92f637f072a4fb11fd44cb983794c54f1bedc2fd01b68bfac989580a6443fa030' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
