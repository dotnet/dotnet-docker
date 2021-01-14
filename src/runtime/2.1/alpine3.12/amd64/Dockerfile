ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:2.1-alpine3.12

# Install .NET Core
ENV DOTNET_VERSION=2.1.24

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='2f8e6c70fc9e2d21408336a3200f1ede0f97a143605870059aa9c8338d0991aec46b8961372ebb9514c676cb2279ada4380688512988c55fd13e22ce25052494' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
