ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.0-alpine3.9-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-preview7-27813-06

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='53918b587568309dd38d141ebc7f1d3a88497a6a1b1bc41c714eb20de636ea373c72a5e2af004a8cce42d7c2b874bb89f4473362a010e622ac8a3020146fe689' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
