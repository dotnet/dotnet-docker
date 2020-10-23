ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12-arm64v8

# Install .NET
ENV DOTNET_VERSION 5.0.0-rtm.20519.4

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-5.0.0-linux-musl-arm64.tar.gz \
    && dotnet_sha512='b2858df7e3bc9d45f2014e0d1cab4490b511694881c713dc5af8e472bca6b218d6a9fc94776727310a8e14a38a29d66475f67d3d02783132125c7e9d285d1379' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
