ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:5.0-alpine3.12

# Install .NET Core
ENV DOTNET_VERSION 5.0.0-preview.7.20318.2

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='2c3761b24031adc86231c1a87a77e1ee8b66ceddae2634550705c32294d63cf877072462499e8fd4214d0d8c81c7ceff5fffc81ffe87e96f60ecb5b40efeb0ad' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
