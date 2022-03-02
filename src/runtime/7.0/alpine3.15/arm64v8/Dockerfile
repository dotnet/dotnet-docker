ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.2-alpine3.15-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.2.22128.3

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='4b0f63643e34047e125f4ca9ad17006d34a17f434188e46aa5d6d8760784e8fa83a6bc26d50ba8fc72a887315e276b48515a271d415e71a03c18103e5883b681' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
