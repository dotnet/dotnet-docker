ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.2-alpine3.9

# Install .NET Core
ENV DOTNET_VERSION 2.2.7

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='e5e5437b57041395bf0af0b7472a615fb6aaef72a052a9b16d891cf0e9ea8a9cb09c28e0bf1d0f4808d5b61648324f3ab77dddcc7e426ba1fb91235a039eaaf1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
