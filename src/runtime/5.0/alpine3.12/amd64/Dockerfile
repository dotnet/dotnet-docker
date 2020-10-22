ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12-amd64

# Install .NET
ENV DOTNET_VERSION 5.0.0-rtm.20517.6

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-5.0.0-linux-musl-x64.tar.gz \
    && dotnet_sha512='febcb7a48e908679920f79b2ce0479e30f606bf1c824e81d7f2ea69d6bb39e6f4c59e03b263ce1142d04e3110bd6ba98910762ca8cc6618b0650284da5c06851' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
