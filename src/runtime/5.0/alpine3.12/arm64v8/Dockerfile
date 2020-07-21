ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:5.0-alpine3.12-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 5.0.0-preview.7.20364.11

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='813b804b3a88aaee78b887e565b05c34e9c9aa3d2dd99f9265caa10639a3f53c999df0f6e3260ddd8c4784cb9fb8b1f1aa4bd1754978570ffba778108dffd10f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
