ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-alpine3.12-arm64v8

# Install .NET
ENV DOTNET_VERSION 6.0.0-alpha.1.21052.6

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='c833d0ce59f19bcbc1c4afa20a04ac0fcd8b2f9c5d5b99319f4d7d8c62a236e397d569e40c610c3797e527a1b996574c4922cd487fc1d0b3d6269b08ece8ae1e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
