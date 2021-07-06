ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.6-alpine3.13-arm32v7

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.6.21352.12

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='0cb731082c257294b97a2a0536ed9fc4f156cdfc58e15f60ffb575d43c082ac68ffadd63137b0d9b29a459f782c9ef86b972c8c42c15c567d570e069be66d91e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
