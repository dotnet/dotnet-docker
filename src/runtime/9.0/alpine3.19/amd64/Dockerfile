ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.0-alpha.1-alpine3.19-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-alpha.1.24061.26

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='ea3cc82e0c9d1c95a51e68d872c60f3b9bf6117a47cb0f94ee151f04ce6b8e189203b56423f33dfe1eb259d2d12ae2e3df5d385510dd432a09588b88c5085d59' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
