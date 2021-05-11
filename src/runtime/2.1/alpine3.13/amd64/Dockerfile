ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:2.1-alpine3.13

# Install .NET Core
ENV DOTNET_VERSION=2.1.28

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='a8e6f3e20b6ecc9bb8501ae61caadeecfcf7cb61343b1f4008138f2dcf15f23bad4c228d61f334b1dec5afb635d6bfb5aae01e82bc95a70af5509b51d2d40efe' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
