ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:5.0-alpine3.12

# Install .NET Core
ENV DOTNET_VERSION 5.0.0-preview.6.20305.6

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='416eeebfd90d66f68cb6e68842b3b89c080adf4a4773d3af822d1cdc4469c3e3f3ff1171bbcbfec0e4e1ca1f2357fa694fff97770bc72893cf375cf9c564e1bc' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
