ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.11-alpine3.13-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install .NET
ENV DOTNET_VERSION=5.0.11

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='86a149621e26a67ec72e8c7c11f8361d20a3a9043e5b66d8048317a43d8de834930fa4b6cf32804adbfb8a3c36875b8f69e0331a4133dc48ab3ac1c6409f2266' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
