ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.7-alpine3.13-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21373.17

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='52168ff809ecf4f4ccd011f33c739bc62e36e33dfe9fb700f6142e60e7d67216fe209874c07d03f11fc432e21b5eda95667e25634d0259c431d724d0ff86526b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
