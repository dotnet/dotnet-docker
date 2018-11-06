FROM microsoft/dotnet-nightly:3.0-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-preview-27106-02

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='90ec30438a28b3b9f805f9238bcef14551dcc2485d01a37a54e7d04fd567aa232e09a1ee683f877d2799950a9a62ee97f1fa862d48cf1094e994fbb3571b7db8' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
