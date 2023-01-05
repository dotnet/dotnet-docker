ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.0-alpha.1-alpine3.17-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-alpha.1.23053.7

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='d0976258bf57b44128c0b6180b228676d3390eda26f462c5bc6d857a22a028d9c78d74c63a3578adcea23c0b2dd66f7bf9f5bca3219c2e654667152dab81eabe' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
