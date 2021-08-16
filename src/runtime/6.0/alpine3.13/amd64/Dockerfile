ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.1-alpine3.13-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.1.21414.4

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='12236a61e90927cd2ef49ebdd4da57fc1b023726dbd0578f3d5dcc9621a78893538549e1eb321178ee38e5b7fb32bfcbd95dd46859fdd21de9b415305b556125' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
