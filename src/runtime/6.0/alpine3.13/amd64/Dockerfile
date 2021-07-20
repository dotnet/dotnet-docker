ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.7-alpine3.13-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21369.8

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='231b1af0590ce4c8babe498dff3ba56d4a2ea88ee76890e4b8d4f1f6209754108bfa0a75aeb3e5f0d94cd03e3166a59514ef85eee537e6cc4a4e9309c3848854' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
