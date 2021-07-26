ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.7-alpine3.13-arm64v8

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21373.17

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='22f975c6b76137df15d6faa24f8bb0537f78fe9ec78bd95dcb6670658904f8b970e908dd11a5af146dc4657367bdd36b27f7146e7b9b7085bcf33a3dfc87efd5' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
