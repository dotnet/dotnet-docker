ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.2-alpine3.21-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=9.0.2

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='df116ef9b7f6b717b7c7f057e826c9e1f1ed0d743fa6b26e9229fc36e500ab834d19ae1ab55ebc28b1c9b8cb4a7f41c62edd08c2dd2cdcb6e912defea2810ffb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
