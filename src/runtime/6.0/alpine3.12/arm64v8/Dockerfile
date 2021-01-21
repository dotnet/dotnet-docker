ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-alpine3.12-arm64v8

# Install .NET
ENV DOTNET_VERSION=6.0.0-alpha.1.21069.7

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='8566ab5686d08f9de8589eb03b93cb6cd7ec1ecdf55506cccda3455baf98042bea75d32cedb6fbe13c79462d73c8c977262c3b54930fb37723a1cf64ef5560a2' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
