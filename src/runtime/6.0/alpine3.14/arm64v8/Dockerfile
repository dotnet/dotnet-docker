ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.1-alpine3.14-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.1.21426.17

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='4f0eb4f3704989d2143ab6b3428bdadf652c68619b6f6022f3cf13ec04837caa2c17ac15bc8c9ce3d9fbd7afde77fd10317795d6c2dd97f74ac339e81a0e7b1d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
