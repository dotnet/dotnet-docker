ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:5.0-alpine3.11-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 5.0.0-preview.5.20268.9

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='82a1c9bd496ecebda510415a97dc163601814e2327274d79fd494b4ece07d038e46319688acab7690b42a20d5a6b2fde7b17d8ce7f5e621edd054eddc5c29445' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
